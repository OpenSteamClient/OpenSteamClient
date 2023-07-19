using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OpenSteamworks;

namespace OpenSteamworks.Callbacks;

public class CallbackManager
{
    private struct MissedMessage {
        public Type type;
        public object obj;
        public CallbackMsg_t cbMsg;
        public DateTime timeQueued;
    }

    private SteamClient client;
    private bool logIncomingCallbacks;
    private bool logCallbackContents;
    private bool poll = true;
    private Thread pollThread;
    private List<MissedMessage> missedMessages = new List<MissedMessage>();
    private List<Tuple<int, Action<object>, bool>> handlers = new List<Tuple<int, Action<object>, bool>>();

    public CallbackManager(SteamClient client, bool logIncomingCallbacks, bool logCallbackContents) {
        this.client = client;
        this.logIncomingCallbacks = logIncomingCallbacks;
        this.logCallbackContents = logCallbackContents;

        // Start the thread here, we do miss some messages as not all listeners are setup yet
        // Should we have an internal queue for these messages?
        pollThread = new Thread(this.NativePollThread);
        pollThread.Start();
    }
    public void NativePollThread() {
        bool hasCallback = false;

        unsafe {
            CallbackMsg_t msg = new CallbackMsg_t();
            do
            {
                ProcessMissedMessages();
                hasCallback = this.client.NativeClient.native_Steam_BGetCallback(client.NativeClient.pipe, (nint)(&msg));
                if (hasCallback) {
                    var hasType = CallbackConstants.IDToType.TryGetValue(msg.m_iCallback, out Type? type);

                    LogCallback(msg);

                    if (hasType && type != null) {
                        var obj = Marshal.PtrToStructure((IntPtr)msg.m_pubParam, type);
                        if (obj != null) {
                            LogCallbackData(obj, type);

                            // Send to listeners or store if no listeners exist
                            if (GetHandlersForId(msg.m_iCallback, out List<Action<object>>? handlers)) {
                                foreach (var handler in handlers)
                                {
                                    handler?.Invoke(obj);
                                }
                            } else {
                                missedMessages.Add(new MissedMessage() {
                                    type = type,
                                    obj = obj,
                                    cbMsg = msg,
                                    timeQueued = DateTime.Now
                                });
                            }
                        } else {
                            //TODO: logger
                            Console.WriteLine("PtrToStructure returned null. Message skipped.");
                        }
                    }

                    this.client.NativeClient.native_Steam_FreeLastCallback(client.NativeClient.pipe);
                }
                System.Threading.Thread.Sleep(10);
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
                Console.WriteLine($"Received callback [ID: {msg.m_iCallback}, name: {callbackName}, param length: {msg.m_cubParam}, data ptr: {string.Format("0x{0:X}", (IntPtr)msg.m_pubParam)}]");
            }
        }
    }

    private void LogCallbackData(object obj, Type type) {
        if (logCallbackContents) {
            FieldInfo[] fields = type.GetFields();
            Console.WriteLine($"Begin Message {type.Name}");
            foreach (var field in fields)
            {
                Console.WriteLine("    " + field.Name + ": " + field.GetValue(obj));
            }

            Console.WriteLine($"End of Message {type.Name}");
        }
    }

    private void ProcessMissedMessages() {
        if (missedMessages.Count > 0) {
            List<MissedMessage> messagesToRemove = new List<MissedMessage>();
            foreach (var missedMsg in missedMessages)
            {
                var type = missedMsg.type;
                var obj = missedMsg.obj;
                if (GetHandlersForId(missedMsg.cbMsg.m_iCallback, out List<Action<object>>? handlers)) {
                    foreach (var handler in handlers)
                    {
                        handler?.Invoke(obj);
                    }
                    messagesToRemove.Add(missedMsg);
                } 

                // Remove messages that haven't been got in 60 seconds
                if (missedMsg.timeQueued.AddSeconds(60) < DateTime.Now) {
                    Console.WriteLine("Removing " + missedMsg.type.Name + " as it's expired");
                    messagesToRemove.Add(missedMsg);
                }
            }
            foreach (var msgToRemove in messagesToRemove)
            {
                missedMessages.Remove(msgToRemove);
            }
        }
    }
    private bool GetHandlersForId(int id, [NotNullWhen(true)] out List<Action<object>>? handlersOut) {
        handlersOut = new List<Action<object>>();
        List<Tuple<int, Action<object>, bool>> handlersToRemove = new List<Tuple<int, Action<object>, bool>>();

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
    public void RequestStop() {
        poll = false;
    }
}