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
using OpenSteamworks.Callbacks.Structs;

namespace OpenSteamworks.Callbacks;

public class CallbackManager
{
    private SteamClient client;
    private bool logIncomingCallbacks;
    private bool logCallbackContents;
    private bool poll = true;
    private readonly Thread pollThread;
    private readonly List<Tuple<int, Action<object>, bool>> handlers = new();

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
                            if (GetHandlersForId(msg.m_iCallback, out List<Action<object>>? handlers)) {
                                foreach (var handler in handlers)
                                {
                                    handler?.Invoke(obj);
                                }
                            }
                        } else {
                            //TODO: logger
                            Console.WriteLine("PtrToStructure returned null. Message skipped.");
                        }
                    }

                    this.client.NativeClient.native_Steam_FreeLastCallback(client.NativeClient.pipe);
                }
                System.Threading.Thread.Sleep(50);
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
    private bool GetHandlersForId(int id, [NotNullWhen(true)] out List<Action<object>>? handlersOut) {
        handlersOut = new List<Action<object>>();
        List<Tuple<int, Action<object>, bool>> handlersToRemove = new();

        foreach (var handler in handlers)
        {
            if (handler.Item1 == id) {
                handlersOut.Add(handler.Item2);
                if (handler.Item3 == true) {
                    handlersToRemove.Add(handler);
                }
            }
        }

        foreach (var handler in handlersToRemove)
        {
            handlers.Remove(handler);
        }

        if (handlersOut.Count > 0) {
            return true;
        }

        return false;
    }
    public void RegisterHandler<T>(Action<object> handler, bool oneShot = false) where T : struct {
        if (!CallbackConstants.TypeToID.TryGetValue(typeof(T), out int id)) {
            throw new ArgumentException("T was not defined in TypeToID");
        }

        RegisterHandlerForId(id, handler, oneShot);
    }
    private void RegisterHandlerForId(int id, Action<object> handler, bool oneShot = false) {
        handlers.Add(new Tuple<int, Action<object>, bool>(id, handler, oneShot)); 
    }
    public void DeregisterHandler(Action<object> handler) {

    }
    public void RequestStopAndWaitForExit() {
        poll = false;
        do
        {
            System.Threading.Thread.Sleep(10);
        } while (this.pollThread.IsAlive);
    }
}