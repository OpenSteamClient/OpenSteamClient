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

public class CallResult<T> where T: unmanaged {
    public bool failed;
    public ESteamAPICallFailure failureReason;
    public T data;

    public CallResult(bool failed, ESteamAPICallFailure failureReason, T data) {
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

    /// <summary>
    /// Waits for an api call with the specified type to complete. Might block forever if given an invalid handle or if the call creating function succeeds too quickly before the handler gets registered.
    /// </summary>
    /// <typeparam name="T">Type of the call</typeparam>
    /// <param name="handle">Handle to the call</param>
    /// <param name="resumeThread">Whether to continue callbackthread after the handler is registered internally</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Task<CallResult<T>> WaitForAPICallResult<T>(SteamAPICall_t handle, bool resumeThread = true, CancellationToken cancellationToken = default) where T: unmanaged {
        unsafe {
            this.PauseThread();
            int callbackID = GetCallbackID(typeof(T));
            int callbackSize = sizeof(T);
            bool hasFreed = false;
            void* data = NativeMemory.AllocZeroed((nuint)callbackSize);
            bool failed = false;
            ESteamAPICallFailure failureReason;
            var tcs = new TaskCompletionSource<CallResult<T>>();

            var handler = this.RegisterHandler<SteamAPICallCompleted_t>((CallbackHandler handler, SteamAPICallCompleted_t compl) =>
            {
                if (compl.m_hAsyncCall == handle) {
                    if (!this.client.NativeClient.IClientUtils.GetAPICallResult(handle, data, callbackSize, callbackID, ref failed)) {
                        throw new Exception("GetAPICallResult returned false with our handle after receiving SteamAPICallCompleted_t. Bugged?");
                    }
                    failureReason = this.client.NativeClient.IClientUtils.GetAPICallFailureReason(handle);
                    if (failed) {
                        tcs.TrySetResult(new CallResult<T>(failed, failureReason, default));
                    } else {
                        T? val = (T?)Marshal.PtrToStructure((IntPtr)data, typeof(T));
                        if (!val.HasValue) {
                            failed = true;
                            tcs.TrySetResult(new CallResult<T>(failed, failureReason, default));
                        } else {
                            tcs.TrySetResult(new CallResult<T>(failed, failureReason, val.Value));
                        }
                    }

                    this.DeregisterHandler(handler);
                    if (!hasFreed) {
                        hasFreed = true;
                        NativeMemory.Free(data);
                    }
                }
            }, false);

            cancellationToken.Register(() =>
            {
                this.DeregisterHandler(handler);
                if (!hasFreed) {
                    hasFreed = true;
                    NativeMemory.Free(data);
                }
                tcs.TrySetCanceled();
            });

            if (resumeThread) {
                this.ContinueThread();
            }

            return tcs.Task;
        }
    }
    
    public void NativePollThread() {
        bool hasCallback = false;
        CallbackMsg_t msg = new();
        Stopwatch fullPollTime = new();
        Stopwatch runFrameTime = new();
        Stopwatch bGetCallbackTime = new();
        Stopwatch idToTypeTime = new();
        Stopwatch logCallbackTime = new();
        Stopwatch ptrToStructureTime = new();
        Stopwatch logCallbackDataTime = new();
        Stopwatch getTypedHandlersTime = new();
        Stopwatch executeTypedHandlersTime = new();
        Stopwatch getIDHandlersTime = new();
        Stopwatch executeIDHandlersTime = new();
        unsafe {
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

                fullPollTime.Reset();
                fullPollTime.Start();
                this.client.NativeClient.IClientEngine.RunFrame();
                hasCallback = this.client.NativeClient.native_Steam_BGetCallback(client.NativeClient.pipe, (nint)(&msg));
                if (hasCallback) {
                    var hasType = CallbackConstants.IDToType.TryGetValue(msg.m_iCallback, out Type? type);

                    LogCallback(msg);

                    if (hasType && type != null) {
                        try
                        {
                            var obj = Marshal.PtrToStructure((IntPtr)msg.m_pubParam, type);
                            if (obj != null) {
                                LogCallbackData(obj, type);

                                // Send to listeners if any exist
                                if (GetHandlersForId(msg.m_iCallback, out List<CallbackHandler>? handlers)) {
                                    foreach (var handler in handlers)
                                    {
                                        handler.func?.DynamicInvoke(handler, obj);
                                    }
                                }
                            } else {
                                //TODO: logger
                                Console.WriteLine("PtrToStructure returned null. Message skipped.");
                            }
                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine("PtrToStructure threw an error. Message skipped.");
                            Console.WriteLine(e);
                        }
                    } else {
                        // Handle untyped (id only) handlers
                        if (GetHandlersForId(msg.m_iCallback, out List<CallbackHandler>? handlers)) {
                            foreach (var handler in handlers)
                            {
                                handler.func?.DynamicInvoke(handler);
                            }
                        }
                    }

                    this.client.NativeClient.native_Steam_FreeLastCallback(client.NativeClient.pipe);
                    fullPollTime.Stop();
                    Console.WriteLine("Callback handling took " + fullPollTime.Elapsed.TotalMilliseconds + "ms");
                } else {
                    // Sleep only if we have no extra messages
                    //System.Threading.Thread.Sleep(1);
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
            // if (type.Name.StartsWith("HTML_")) {
            //     return;
            // }

            var fields = type.GetFields();
            Console.WriteLine($"Begin Message {type.Name}");
            try {
                foreach (var field in fields)
                {
                    dynamic? value = field.GetValue(obj);
                    string? substituteValue = null;
                    if (value is not null) {
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
    private bool GetHandlersForId(int id, [NotNullWhen(true)] out List<CallbackHandler>? handlersOut) {
        handlersOut = new List<CallbackHandler>();
        List<CallbackHandler> handlersToRemove = new();

        foreach (var handler in handlers.Where(handler => handler.callbackId == id))
        {
            handlersOut.Add(handler);
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

    public static int GetCallbackID(Type callbackType) {
        if (!CallbackConstants.TypeToID.TryGetValue(callbackType, out int id)) {
            throw new ArgumentException(callbackType + " was not defined in TypeToID");
        }

        return id;
    }

    public CallbackHandler RegisterHandler<T>(Action<CallbackHandler, T> handler, bool oneShot = false) where T : struct {
        return this.RegisterHandlerInternal(GetCallbackID(typeof(T)), handler, oneShot);
    }

    public CallbackHandler RegisterHandler(Action<CallbackHandler, object> handler, Type type, bool oneShot = false) {
        return RegisterHandlerInternal(GetCallbackID(type), handler, oneShot);
    }

    public void RegisterCallbackListenerAttributesFor(object obj) {
        foreach (var func in obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            Attribute? typedAttribute = func.GetCustomAttribute(typeof(CallbackListenerAttribute<>));
            if (typedAttribute != null) {
                Type expectedType = typedAttribute.GetType().GetGenericArguments()[0];
                var argTypes = func.GetParameters().Select(p => p.ParameterType).ToArray();
                if (argTypes.Length != 2 || argTypes[0] != typeof(CallbackHandler) || argTypes[1] != expectedType) {
                    throw new ArgumentException("Function " + func.Name + " has invalid arguments for CallbackListenerAttribute. Expected CallbackHandler, " + expectedType.Name + " but had " + string.Join(", ", argTypes.Select(a => a.Name)));
                }
                var actType = typeof(Action<,>).MakeGenericType(typeof(CallbackHandler), expectedType);
                this.RegisterHandlerInternal(GetCallbackID(expectedType), func.CreateDelegate(actType, obj), false);
            }

            CallbackIDListenerAttribute? idAttribute = (CallbackIDListenerAttribute?)func.GetCustomAttribute(typeof(CallbackIDListenerAttribute));
            if (idAttribute != null) {
                if (func.GetParameters().Length != 1) {
                    throw new ArgumentException("Function " + func.Name + " has wrong arguments for CallbackIDListenerAttribute. Expected 1 argument of type CallbackHandler.");
                }
                this.RegisterHandlerInternal(idAttribute.CallbackID, func.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(CallbackHandler)), obj), false);
            }
        }
    }

    public CallbackHandler RegisterHandlerForId(int id, Action<CallbackHandler> func, bool oneShot = false) {
        return RegisterHandlerInternal(id, func, oneShot);
    }

    private CallbackHandler RegisterHandlerInternal(int id, Delegate func, bool oneShot) {
        CallbackHandler handler = new(id, func, oneShot);
        lock (handlersLock)
        {
            handlers.Add(handler);
        }
        return handler;
    }

    public async Task WaitForCallback(int callbackId) {
        var tcs = new TaskCompletionSource();

        RegisterHandlerForId(callbackId, (CallbackHandler handler) => {
            Console.WriteLine("WaitForCallback handler fired");
            tcs.TrySetResult();
        }, true);

        await tcs.Task;
    }

    public async Task<object> WaitForCallback(Type callbackType) {
        var tcs = new TaskCompletionSource<object>();

        RegisterHandler((CallbackHandler handler, object callbackResult) => {
            Console.WriteLine("WaitForCallback handler fired");
            tcs.TrySetResult(callbackResult);
        }, callbackType, true);

        return await tcs.Task;
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