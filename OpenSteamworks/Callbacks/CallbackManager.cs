using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks;

public class SteamAPICallResult<T> where T: unmanaged {
    public bool failed;
    public ESteamAPICallFailure failureReason;
    public T? data = null;

    public SteamAPICallResult(bool failed, ESteamAPICallFailure failureReason, T? data) {
        this.failed = failed;
        this.failureReason = failureReason;
        this.data = data;
    }
}

public class CallbackManager
{
    public class CallbackHandler {
        public int callbackId;
        public Delegate func;
        public bool oneShot;
        public CallbackHandler(int callbackId, Delegate func, bool oneShot) {
            this.callbackId = callbackId;
            this.func = func;
            this.oneShot = oneShot;
        }
    }

    private SteamClient client;
    private bool logIncomingCallbacks;
    private bool logCallbackContents;
    private bool poll = true;
    private bool pausePoll = false;
    private bool didPoll = true;
    private readonly Thread pollThread;
    private readonly object handlersLock = new();
    private readonly HashSet<CallbackHandler> handlers = new();

    public CallbackManager(SteamClient client, bool logIncomingCallbacks, bool logCallbackContents) {
        this.client = client;
        this.logIncomingCallbacks = logIncomingCallbacks;
        this.logCallbackContents = logCallbackContents;

        // Create the thread here, but don't start it immediately
        pollThread = new(this.NativePollThread)
        {
            Name = "CallbackPollThread"
        };
    }

    public void StartThread() {
        if (!pollThread.IsAlive) {
            pollThread.Start();
        }
    }

    public void PauseThread() {
        pausePoll = true;
        while (didPoll)
        {
            System.Threading.Thread.Sleep(50);
        }
    }

    public void ContinueThread() {
        pausePoll = false;
        while (!didPoll)
        {
            System.Threading.Thread.Sleep(50);
        }
    }

    public Task<SteamAPICallResult<T>> WaitForSteamAPICallResult<T>(SteamAPICall_t handle, int expectedCallbackID, CancellationToken token = default) where T: unmanaged {
        return Task.Run(() =>
        {
            // if (this.client.NativeClient.IClientUtils.GetAPICallFailureReason(handle) == ESteamAPICallFailure.k_ESteamAPICallFailureInvalidHandle) {
            //     throw new ArgumentException(nameof(handle));
            // }

            unsafe {
                int callbackSize = sizeof(T);
                void* data = NativeMemory.AllocZeroed((nuint)callbackSize);
                bool failed = false;
                bool gotResult = false;
                Console.WriteLine("test: " + this.client.NativeClient.IClientUtils.GetAPICallFailureReason(this.client.NativeClient.IClientUtils.AllocPendingAPICallHandle()));
                while (!gotResult)
                {
                    Console.WriteLine("failureReason: " + this.client.NativeClient.IClientUtils.GetAPICallFailureReason(handle));
                    gotResult = this.client.NativeClient.IClientUtils.GetAPICallResult(handle, data, callbackSize, expectedCallbackID, ref failed);
                    System.Threading.Thread.Sleep(50);
                    if (token.IsCancellationRequested) {
                        throw new OperationCanceledException();
                    }
                }

                return new SteamAPICallResult<T>(failed, this.client.NativeClient.IClientUtils.GetAPICallFailureReason(handle), (T?)Marshal.PtrToStructure((IntPtr)data, typeof(T)));
            }
        });
    }
    
    public void NativePollThread() {
        bool hasCallback = false;

        unsafe {
            CallbackMsg_t msg = new();
            do
            {                
                if (pausePoll) {
                    didPoll = false;
                    do
                    {
                        System.Threading.Thread.Sleep(50);
                    } while (pausePoll);
                    didPoll = true;
                }
                
                hasCallback = this.client.NativeClient.native_Steam_BGetCallback(client.NativeClient.pipe, (nint)(&msg));
                if (hasCallback) {
                    var hasType = CallbackConstants.IDToType.TryGetValue(msg.m_iCallback, out Type? type);

                    LogCallback(msg);

                    if (hasType && type != null) {
                        var obj = Marshal.PtrToStructure((IntPtr)msg.m_pubParam, type);
                        if (obj != null) {
                            LogCallbackData(obj, type);

                            // Send to listeners if any exist
                            if (GetHandlersForId(msg.m_iCallback, out List<Delegate>? handlers)) {
                                foreach (var handler in handlers)
                                {
                                    handler?.DynamicInvoke(obj);
                                }
                            }
                        } else {
                            //TODO: logger
                            Console.WriteLine("PtrToStructure returned null. Message skipped.");
                        }
                    } else {
                        // Handle untyped (id only) handlers
                        if (GetHandlersForId(msg.m_iCallback, out List<Delegate>? handlers)) {
                            foreach (var handler in handlers)
                            {
                                handler?.DynamicInvoke();
                            }
                        }
                    }

                    this.client.NativeClient.native_Steam_FreeLastCallback(client.NativeClient.pipe);
                } else {
                    System.Threading.Thread.Sleep(50);
                }
                
            } while (poll);
        }
    }

    private void LogCallback(CallbackMsg_t msg) {
        if (logIncomingCallbacks)
        {
            string callbackName = "Unknown";
            if (CallbackConstants.CallbackNames.TryGetValue(msg.m_iCallback, out string? callbackNameOut))
            {
                callbackName = callbackNameOut;
            }

            unsafe {
                lock (handlersLock)
                {
                    Console.WriteLine($"Received callback [ID: {msg.m_iCallback}, name: {callbackName}, param length: {msg.m_cubParam}, data ptr: {string.Format("0x{0:x}", (IntPtr)msg.m_pubParam)}, has handlers: {handlers.Any(e => e.callbackId == msg.m_iCallback)}]");
                }
            }
        }
    }

    private void LogCallbackData(object obj, Type type) {
        if (logCallbackContents) {
            FieldInfo[] fields = type.GetFields();
            Console.WriteLine($"Begin Message {type.Name}");
            try {
                foreach (var field in fields)
                {
                    dynamic? value = field.GetValue(obj);
                    string? substituteValue = null;
                    if ((object?)value != null) {
                        var valueType = value.GetType();
                        if (valueType.IsArray) {
                            Type elementType = valueType.GetElementType()!;
                            if (elementType.IsPrimitive || elementType == typeof(decimal) || elementType == typeof(string)) {
                                List<string> strings = new();
                                foreach (var item in value)
                                {
                                    strings.Add(item.ToString());
                                }

                                substituteValue = $"[{string.Join(",", strings)}]";
                            }
                        }
                    }
                    
                    if (substituteValue != null) {
                        Console.WriteLine("    " + field.Name + ": " + substituteValue);
                    } else {
                        Console.WriteLine("    " + field.Name + ": " + value);
                    }
                    
                }
            } catch (Exception) {
                Console.WriteLine("Encountered an error.");
            }

            Console.WriteLine($"End of Message {type.Name}");
        }
    }
    private bool GetHandlersForId(int id, [NotNullWhen(true)] out List<Delegate>? handlersOut) {
        handlersOut = new List<Delegate>();
        List<CallbackHandler> handlersToRemove = new();

        foreach (var handler in handlers.Where(handler => handler.callbackId == id))
        {
            handlersOut.Add(handler.func);
            if (handler.oneShot == true) {
                handlersToRemove.Add(handler);
            }
        }

        foreach (var handler in handlersToRemove)
        {
            DeregisterHandler(handler);
        }

        if (handlersOut.Count > 0) {
            return true;
        }

        return false;
    }

    public CallbackHandler RegisterHandler<T>(Action<T> handler, bool oneShot = false) where T : struct {
        return this.RegisterHandler(handler, typeof(T), oneShot);
    }

    public CallbackHandler RegisterHandler(Delegate handler, Type type, bool oneShot = false) {
        if (!CallbackConstants.TypeToID.TryGetValue(type, out int id)) {
            throw new ArgumentException("T was not defined in TypeToID");
        }

        return RegisterHandlerForId(id, handler, oneShot);
    }

    public void RegisterHandlersFor(object obj) {
        foreach (var func in obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            Attribute? typedAttribute = func.GetCustomAttribute(typeof(CallbackListenerAttribute<>));
            if (typedAttribute != null) {
                Type expectedType = typedAttribute.GetType().GetGenericArguments()[0];
                Type argType = func.GetParameters()[0].ParameterType;
                if (expectedType != argType) {
                    throw new ArgumentException("Function " + func.Name + " has invalid arguments for CallbackListenerAttribute. Expected " + expectedType.Name + ", had " + argType.Name);
                }
                var actType = typeof(Action<>).MakeGenericType(expectedType);
                this.RegisterHandler(func.CreateDelegate(actType, obj), expectedType, false);
            }

            CallbackIDListenerAttribute? idAttribute = (CallbackIDListenerAttribute?)func.GetCustomAttribute(typeof(CallbackIDListenerAttribute));
            if (idAttribute != null) {
                if (func.GetParameters().Length > 0) {
                    throw new ArgumentException("Function " + func.Name + " has arguments for CallbackIDListenerAttribute. Expected 0 arguments. ");
                }
                this.RegisterHandlerForId(idAttribute.CallbackID, func.CreateDelegate(typeof(Action), obj), false);
            }
        }
    }

    public CallbackHandler RegisterHandlerForId(int id, Delegate func, bool oneShot = false) {
        CallbackHandler handler = new(id, func, oneShot);
        lock (handlersLock)
        {
            handlers.Add(handler);
        }
        return handler;
    }

    public async Task WaitForCallback(int callbackId) {
        var tcs = new TaskCompletionSource();

        RegisterHandlerForId(callbackId, () => {
            Console.WriteLine("WaitForCallback handler fired");
            tcs.TrySetResult();
        }, true);

        await tcs.Task;
    }

    public void DeregisterHandler(CallbackHandler handler) {
        lock (handlersLock)
        {
            handlers.Remove(handler);
        }
    }

    public void RequestStopAndWaitForExit() {
        Console.WriteLine("Stopping CallbackThread");
        poll = false;
        do
        {
            System.Threading.Thread.Sleep(10);
        } while (this.pollThread.IsAlive);
        
        Console.WriteLine("Stopped CallbackThread");
    }
}