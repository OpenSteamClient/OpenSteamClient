using System;
using System.Threading.Tasks;
using OpenSteamworks.Generated;

namespace OpenSteamworks.Messaging;

public class Connection : IDisposable {
    private uint nativeConnection;
    private IClientSharedConnection iSharedConnection;
    private bool disposed = false;

    internal Connection(IClientSharedConnection iSharedConnection) {
        this.nativeConnection = iSharedConnection.AllocateSharedConnection();
        this.iSharedConnection = iSharedConnection;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sends a oneshot message. Does not have a way to retrieve results.
    /// </summary>
    public void SendMessage() {
        
    }

    /// <summary>
    /// Sends a oneshot message. Awaits for results.
    /// </summary>
    public async Task<TResult> SendMessageAndAwaitResponse<TResult, TMessage>(TMessage msg) 
    where TResult: class, IMessage, new()
    where TMessage: class, IMessage {
        var resultMsg = new TResult();
        
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