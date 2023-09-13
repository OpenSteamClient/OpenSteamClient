using System;
using System.Threading.Tasks;
using OpenSteamworks.Generated;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Messaging;

public class Connection : IDisposable {
    private uint nativeConnection;
    private IClientSharedConnection iSharedConnection;
    private IClientUser iClientUser;
    private bool disposed = false;

    internal Connection(IClientSharedConnection iSharedConnection, IClientUser iClientUser) {
        this.nativeConnection = iSharedConnection.AllocateSharedConnection();
        this.iSharedConnection = iSharedConnection;
        this.iClientUser = iClientUser;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sends a oneshot protobuf message. Awaits for results.
    /// </summary>
    public async Task<ProtoMsg<TResult>> ProtobufSendMessageAndAwaitResponse<TResult, TMessage>(ProtoMsg<TMessage> msg) 
    where TResult: Google.Protobuf.IMessage<TResult>, new()
    where TMessage: Google.Protobuf.IMessage<TMessage>, new() {
        return await Task.Run(() =>
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

            unsafe {
                byte[] serialized = msg.Serialize();

                // Register a service method handler if we have a jobname
                if (!string.IsNullOrEmpty(msg.JobName)) {
                    this.iSharedConnection.RegisterServiceMethodHandler(this.nativeConnection, msg.JobName);
                    this.iSharedConnection.RegisterEMsgHandler(this.nativeConnection, (uint)EMsg.KEmsgServiceMethodResponse);
                }

                fixed (void* pointerToFirst = serialized)
                {
                    nuint size = (nuint)serialized.Length * sizeof(byte);
                    this.iSharedConnection.SendMessageAndAwaitResponse(this.nativeConnection, pointerToFirst, size);
                }

                //TODO: Resizing a CUtlBuffer should work. It doesn't, and it will crash if forced to resize (never worked in C++ version either, why?).
                CUtlBuffer buffer = new CUtlBuffer(800000);
                uint callOut = 0;

                // This is terrible. But it works. Kind of.
                double secondsWaited = 0;
                while (!this.iSharedConnection.BPopReceivedMessage(this.nativeConnection, &buffer, ref callOut))
                {
                    System.Threading.Thread.Sleep(20);
                    secondsWaited += 0.020;
                    if (secondsWaited > 10) {
                        throw new TimeoutException("More than 10 seconds has gone by and we haven't received a response. Assuming request failed.");
                    }
                }

                resultMsg.FillFromBinary(buffer.ToManagedAndFree());
            }

            return resultMsg;
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!this.disposed)
        {
            if(disposing)
            {
                // Dispose other managed resources.
            }
            disposed = true;

            this.iSharedConnection.ReleaseSharedConnection(this.nativeConnection);
        }
    }

    ~Connection()
    {
        Dispose(disposing: false);
    }
}