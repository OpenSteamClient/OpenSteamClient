using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSteamworks.Generated;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Protobuf;

namespace OpenSteamworks.Messaging;

public class Connection : IDisposable {
    public class StoredMessage {
        public const uint PROTOBUF_MASK = 0x80000000;
        public EMsg eMsg;
        public CMsgProtoBufHeader header;
        public byte[] fullMsg;
        public DateTime removalTime;
        internal StoredMessage(byte[] fullMsg) {
            this.fullMsg = fullMsg;
            this.removalTime = DateTime.UtcNow.AddMinutes(1);
            using (var stream = new MemoryStream(fullMsg)) {
                // The steamclient is a strange beast. A 64-bit library compiled for little endian.
                using (var reader = new EndianAwareBinaryReader(stream, Encoding.UTF8, EndianAwareBinaryReader.Endianness.Little))
                {
                    this.eMsg = (EMsg)(~PROTOBUF_MASK & reader.ReadUInt32());

                    // Read the header
                    var header_size = reader.ReadUInt32();
                    SteamClient.MessagingLogger.Debug("header_size: " + header_size);
                    byte[] header_binary = reader.ReadBytes((int)header_size);

                    // Parse the header
                    this.header = CMsgProtoBufHeader.Parser.ParseFrom(header_binary);

                    this.fullMsg = fullMsg;
                }
            }
        }
    }
    private uint nativeConnection;
    private IClientSharedConnection iSharedConnection;
    private IClientUser clientUser;
    private bool disposed = false;

    internal Connection(IClientSharedConnection iSharedConnection, IClientUser clientUser) {
        this.nativeConnection = iSharedConnection.AllocateSharedConnection();
        this.iSharedConnection = iSharedConnection;
        this.clientUser = clientUser;
        this.StartPollThread();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sends a oneshot protobuf message. Awaits for results.
    /// </summary>
    public async Task<ProtoMsg<TResult>> ProtobufSendMessageAndAwaitResponse<TResult, TMessage>(ProtoMsg<TMessage> msg, EMsg expectedResponseMsg = EMsg.Invalid) 
    where TResult: Google.Protobuf.IMessage<TResult>, new()
    where TMessage: Google.Protobuf.IMessage<TMessage>, new() {
        return await Task.Run(async () =>
        {
            if (!clientUser.BConnected()) {
                clientUser.EConnect();
                while (!clientUser.BConnected())
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            var resultMsg = new ProtoMsg<TResult>();
            if (msg.AllowRewrite) {
                // :( unfortunately loginmanager is part of OpenSteamworks.Client so we can't use an in-progress login to get this steamid. TODO: subject to change
                msg.header.Steamid = clientUser.GetSteamID();
            }

            // Register a service method handler if we have a jobname
            if (!string.IsNullOrEmpty(msg.JobName)) {
                this.RegisterServiceMethodHandler(msg.JobName);
                this.RegisterEMsgHandler(EMsg.ServiceMethodResponse);
            }

            unsafe {
                byte[] serialized = msg.Serialize();

                fixed (void* pointerToFirst = serialized)
                {
                    nuint size = (nuint)serialized.Length * sizeof(byte);
                    this.iSharedConnection.SendMessageAndAwaitResponse(this.nativeConnection, pointerToFirst, size);
                }
            }

            if (!string.IsNullOrEmpty(msg.JobName)) {
                // Waiting for a job 
                var recvd = await WaitForServiceMethod(msg.JobName);
                resultMsg.FillFromBinary(recvd.fullMsg);
            } else {
                // Waiting for an emsg
                if (expectedResponseMsg == EMsg.Invalid) {
                    throw new ArgumentException("Sending non-service method but didn't indicate response EMsg.");
                }

                var recvd = await WaitForEMsg(expectedResponseMsg);
                resultMsg.FillFromBinary(recvd.fullMsg);
            }

            return resultMsg;
        });
    }

    /// <summary>
    /// Sends a oneshot protobuf message.
    /// </summary>
    public async void ProtobufSendMessage<TMessage>(ProtoMsg<TMessage> msg) 
    where TMessage: Google.Protobuf.IMessage<TMessage>, new() {
        await Task.Run(() =>
        {
            if (!clientUser.BConnected()) {
                clientUser.EConnect();
                while (!clientUser.BConnected())
                {
                    System.Threading.Thread.Sleep(50);
                }
            }

            if (msg.AllowRewrite) {
                // :( unfortunately loginmanager is part of OpenSteamworks.Client so we can't use an in-progress login to get this steamid. TODO: subject to change
                msg.header.Steamid = clientUser.GetSteamID();
            }

            // Register a service method handler if we have a jobname
            if (!string.IsNullOrEmpty(msg.JobName)) {
                this.iSharedConnection.RegisterServiceMethodHandler(this.nativeConnection, msg.JobName);
                this.iSharedConnection.RegisterEMsgHandler(this.nativeConnection, (uint)EMsg.ServiceMethodResponse);
            }

            unsafe {
                byte[] serialized = msg.Serialize();

                fixed (void* pointerToFirst = serialized)
                {
                    nuint size = (nuint)serialized.Length * sizeof(byte);
                    this.iSharedConnection.SendMessage(this.nativeConnection, pointerToFirst, size);
                }
            }
        });
    }

    public void RegisterServiceMethodHandler(string serviceMethod) {
        this.iSharedConnection.RegisterServiceMethodHandler(this.nativeConnection, serviceMethod);
    }

    public void RegisterEMsgHandler(EMsg emsg) {
        this.iSharedConnection.RegisterEMsgHandler(this.nativeConnection, (uint)emsg);
    }

    private bool shouldPoll = false;
    private Task? pollThread;
    private List<StoredMessage> storedMessages = new();
    private Dictionary<EMsg, Delegate> eMsgHandlers = new();
    private Dictionary<string, Delegate> serviceMethodHandlers = new();
    public void StartPollThread() {
        if (shouldPoll) {
            throw new InvalidOperationException("Already polling");
        }
        
        shouldPoll = true;
        pollThread = Task.Run(() =>
        {
            //TODO: Resizing a CUtlBuffer should work. It doesn't, and it will crash if forced to resize (never worked in C++ version either, why?).
            CUtlBuffer buffer = new(100000);

            uint callOut = 0;
            double secondsWaited = 0;
            bool hasMessage = false;

            while (shouldPoll)
            {
                unsafe
                {
                    hasMessage = this.iSharedConnection.BPopReceivedMessage(this.nativeConnection, &buffer, ref callOut);
                    if (hasMessage)
                    {
                        SteamClient.MessagingLogger.Debug("Got message: " + callOut + ", size: " + buffer.m_Put + ", waited " + secondsWaited + "ms");
                        var sm = new StoredMessage(buffer.ToManaged());
                        if (eMsgHandlers.ContainsKey(sm.eMsg)) {
                            eMsgHandlers[sm.eMsg].DynamicInvoke(sm);
                        } else if (serviceMethodHandlers.ContainsKey(sm.header.TargetJobName)) {
                            serviceMethodHandlers[sm.header.TargetJobName].DynamicInvoke(sm);
                        } else {
                            storedMessages.Add(sm);
                        }
                        
                        secondsWaited = 0;

                        buffer.SeekToBeginning();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        secondsWaited += 0.100;
                    }
                }

                // Remove messages that haven't been retrieved in one minute
                this.storedMessages.RemoveAll(item => DateTime.UtcNow > item.removalTime);
            }

            buffer.Free();
        });
    }

    public void StopPollThread() {
        shouldPoll = false;
    }

    /// <summary>
    /// Adds a handler to be called when a specific EMsg is received. If existing messages are stored, the callback will be retroactively called for all the fitting stored messages.
    /// </summary>
    /// <param name="emsg">EMsg to handle</param>
    /// <param name="callback">Callback to call when the EMsg is received</param>
    /// <param name="oneShot">Whether the handler should automatically be removed once it is called</param>
    public void AddEMsgHandler(EMsg emsg, Action<StoredMessage> callback, bool oneShot = false) {
        Action<StoredMessage> realCallback = callback;
        if (oneShot) {
            realCallback = (StoredMessage msg) =>
            {
                RemoveEMsgHandler(emsg, realCallback);
                callback.Invoke(msg);
            };
        }

        if (!eMsgHandlers.ContainsKey(emsg)) {
            eMsgHandlers.Add(emsg, realCallback);
        } else {
            eMsgHandlers[emsg] = Delegate.Combine(eMsgHandlers[emsg], realCallback);
        }

        foreach (var item in storedMessages)
        {
            if (item.eMsg == emsg) {
                storedMessages.Remove(item);
                realCallback(item);
                if (oneShot) {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Adds a handler to be called when a specific service method is received. If existing messages are stored, the callback will be retroactively called for all the fitting stored messages.
    /// </summary>
    /// <param name="method">Service method to handle</param>
    /// <param name="callback">Callback to call when the service method is received</param>
    /// <param name="oneShot">Whether the handler should automatically be removed once it is called</param>
    public void AddServiceMethodHandler(string method, Action<StoredMessage> callback, bool oneShot = false) {
        Action<StoredMessage> realCallback = callback;
        if (oneShot) {
            realCallback = (StoredMessage msg) =>
            {
                RemoveServiceMethodHandler(method, realCallback);
                callback.Invoke(msg);
            };
        }

        if (!serviceMethodHandlers.ContainsKey(method)) {
            serviceMethodHandlers.Add(method, realCallback);
        } else {
            serviceMethodHandlers[method] = Delegate.Combine(serviceMethodHandlers[method], realCallback);
        }

        foreach (var item in storedMessages)
        {
            if (item.header.TargetJobName == method) {
                storedMessages.Remove(item);
                realCallback(item);
                if (oneShot) {
                    break;
                }
            }
        }
    }

    public void RemoveEMsgHandler(EMsg emsg, Action<StoredMessage> callback) {
        Delegate.Remove(eMsgHandlers[emsg], callback);
        eMsgHandlers.Remove(emsg);
    }

    public void RemoveServiceMethodHandler(string method, Action<StoredMessage> callback) {
        Delegate.Remove(serviceMethodHandlers[method], callback);
        serviceMethodHandlers.Remove(method);
    }

    /// <summary>
    /// Waits until a message with a specific EMsg is received. Will use stored messages if they exist
    /// </summary>
    /// <param name="emsg">EMsg to wait for</param>
    /// <returns>The StoredMessage object for further parsing into a ProtoMsg</returns>
    public async Task<StoredMessage> WaitForEMsg(EMsg emsg) {
        var tcs = new TaskCompletionSource<StoredMessage>();

        AddEMsgHandler(emsg, msg => {
            tcs.TrySetResult(msg);
        }, true);

        return await tcs.Task;
    }

    /// <summary>
    /// Waits until a message with a specific TargetJobName(service method name) is received. Will use stored messages if they exist
    /// </summary>
    /// <param name="method">Service method to  wait for</param>
    /// <returns>The StoredMessage object for further parsing into a ProtoMsg</returns>
    public async Task<StoredMessage> WaitForServiceMethod(string method) {        
        var tcs = new TaskCompletionSource<StoredMessage>();

        AddServiceMethodHandler(method, msg => {
            tcs.TrySetResult(msg);
        }, true);

        return await tcs.Task;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!this.disposed)
        {
            if(disposing)
            {
                // Dispose other managed resources.
                this.storedMessages.Clear();
            }
            disposed = true;

            this.StopPollThread();
            this.iSharedConnection.ReleaseSharedConnection(this.nativeConnection);
        }
    }

    ~Connection()
    {
        Dispose(disposing: false);
    }
}