using System;
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

namespace OpenSteamworks.Callbacks;

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
    private readonly Thread pollThread;
    private readonly List<CallbackHandler> handlers = new();

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
    public void NativePollThread() {
        bool hasCallback = false;

        unsafe {
            CallbackMsg_t msg = new();
            do
            {
                hasCallback = this.client.NativeClient.native_Steam_BGetCallback(client.NativeClient.pipe, (nint)(&msg));
                if (hasCallback) {
                    var hasType = CallbackConstants.IDToType.TryGetValue(msg.m_iCallback, out Type? type);

                    LogCallback(msg);

                    if (hasType && type != null) {
                        var obj = Marshal.PtrToStructure((IntPtr)msg.m_pubParam, type);
                        if (obj != null) {
                            LogCallbackData(obj, type);

                            // Send to listeners if any exist
                            Console.WriteLine("Handler count: " + this.handlers.Count);
                            if (GetHandlersForId(msg.m_iCallback, out List<Delegate>? handlers)) {
                                foreach (var handler in handlers)
                                {
                                    handler?.DynamicInvoke(obj);
                                }
                            } else {
                                Console.WriteLine("No handler for " + msg.m_iCallback);
                            }
                        } else {
                            //TODO: logger
                            Console.WriteLine("PtrToStructure returned null. Message skipped.");
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
                Console.WriteLine($"Received callback [ID: {msg.m_iCallback}, name: {callbackName}, param length: {msg.m_cubParam}, data ptr: {string.Format("0x{0:x}", (IntPtr)msg.m_pubParam)}]");
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
            Attribute? attribute = func.GetCustomAttribute(typeof(CallbackListenerAttribute<>));
            if (attribute == null) {
                continue;
            }
            Type expectedType = attribute.GetType().GetGenericArguments()[0];
            Type argType = func.GetParameters()[0].ParameterType;
            if (expectedType != argType) {
                throw new ArgumentException("Function " + func.Name + " has invalid arguments for CallbackListenerAttribute. Expected " + expectedType.Name + ", had " + argType.Name);
            }
            var actType = typeof(Action<>).MakeGenericType(expectedType);
            this.RegisterHandler(func.CreateDelegate(actType, obj), expectedType, false);
        }
    }
    private CallbackHandler RegisterHandlerForId(int id, Delegate func, bool oneShot = false) {
        CallbackHandler handler = new CallbackHandler(id, func, oneShot);
        
        handlers.Add(handler);
        return handler;
    }

    public void DeregisterHandler(CallbackHandler handler) {
        handlers.Remove(handler);
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