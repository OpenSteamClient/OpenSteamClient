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
        internal StoredMessage(byte[] fullMsg) {
            this.fullMsg = fullMsg;
            using (var stream = new MemoryStream(fullMsg)) {
                // The steamclient is a strange beast. A 64-bit library compiled for little endian.
                using (var reader = new EndianAwareBinaryReader(stream, Encoding.UTF8, EndianAwareBinaryReader.Endianness.Little))
                {
                    this.eMsg = (EMsg)(~PROTOBUF_MASK & reader.ReadUInt32());

                    // Read the header
                    var header_size = reader.ReadUInt32();
                    Console.WriteLine("header_size: " + header_size);
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
    private IClientUser iClientUser;
    private bool disposed = false;

    internal Connection(IClientSharedConnection iSharedConnection, IClientUser iClientUser) {
        this.nativeConnection = iSharedConnection.AllocateSharedConnection();
        this.iSharedConnection = iSharedConnection;
        this.iClientUser = iClientUser;
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
            if (!iClientUser.BConnected()) {
                iClientUser.EConnect();
                while (!iClientUser.BConnected())
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            var resultMsg = new ProtoMsg<TResult>();
            if (msg.AllowRewrite) {
                // :( unfortunately loginmanager is part of OpenSteamworks.Client so we can't use an in-progress login to get this steamid. TODO: subject to change
                msg.header.Steamid = iClientUser.GetSteamID();
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
            if (!iClientUser.BConnected()) {
                iClientUser.EConnect();
                while (!iClientUser.BConnected())
                {
                    System.Threading.Thread.Sleep(50);
                }
            }

            if (msg.AllowRewrite) {
                // :( unfortunately loginmanager is part of OpenSteamworks.Client so we can't use an in-progress login to get this steamid. TODO: subject to change
                msg.header.Steamid = iClientUser.GetSteamID();
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
    private Dictionary<EMsg, Action<StoredMessage>> eMsgHandlers = new();
    private Dictionary<string, Action<StoredMessage>> serviceMethodHandlers = new();
    public void StartPollThread() {
        shouldPoll = true;
        pollThread = Task.Run(() =>
        {
            //TODO: Resizing a CUtlBuffer should work. It doesn't, and it will crash if forced to resize (never worked in C++ version either, why?).
            CUtlBuffer buffer = new CUtlBuffer(100000);

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
                        Console.WriteLine("Got message: " + callOut + ", size: " + buffer.m_Put + ", waited " + secondsWaited + "ms");
                        var sm = new StoredMessage(buffer.ToManaged());
                        if (eMsgHandlers.ContainsKey(sm.eMsg)) {
                            eMsgHandlers[sm.eMsg].Invoke(sm);
                            eMsgHandlers.Remove(sm.eMsg);
                        } else if (serviceMethodHandlers.ContainsKey(sm.header.TargetJobName)) {
                            serviceMethodHandlers[sm.header.TargetJobName].Invoke(sm);
                            serviceMethodHandlers.Remove(sm.header.TargetJobName);
                        } else {
                            storedMessages.Add(sm);
                        }
                        
                        secondsWaited = 0;

                        //TODO: allow seeking to the beginning of the buffer to avoid this
                        buffer.Free();
                        buffer = new CUtlBuffer(100000);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        secondsWaited += 0.100;
                    }
                }
            }

            buffer.Free();
        });
    }

    public void StopPollThread() {
        shouldPoll = false;
    }

    public void SetEMsgHandler(EMsg emsg, Action<StoredMessage> callback) {
        eMsgHandlers.Add(emsg, callback);
    }

    public void SetServiceMethodHandler(string method, Action<StoredMessage> callback) {
        serviceMethodHandlers.Add(method, callback);
    }

    public async Task<StoredMessage> WaitForEMsg(EMsg emsg) {
        var tcs = new TaskCompletionSource<StoredMessage>();

        SetEMsgHandler(emsg, msg => {
            tcs.TrySetResult(msg);
        });

        return await tcs.Task;
    }

    public async Task<StoredMessage> WaitForServiceMethod(string method) {
        var tcs = new TaskCompletionSource<StoredMessage>();

        SetServiceMethodHandler(method, msg => {
            tcs.TrySetResult(msg);
        });

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