using System.Net.WebSockets;
using Google.Protobuf;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Messaging;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Experimental;

public class TransportManager : ILogonLifetime
{
    private readonly ISteamClient steamClient;

    public TransportManager(ISteamClient steamClient) {
        this.steamClient = steamClient;
    }

    private Thread? transportThread;
    private ClientWebSocket? transportWS;
    private Thread? clientThread;
    private ClientWebSocket? clientWS;
    private volatile bool readThreadRunning = true;
    private ulong currentMsgID = 0;

    private async void TransportReadThread() {
        Memory<byte> buf = new byte[4096];
        using MemoryStream stream = new();

        while (readThreadRunning)
        {
            try
            {
                if (transportWS == null) {
                    return;
                }
    
                if (transportWS.State != WebSocketState.Open) {
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
    
                var result = await transportWS.ReceiveAsync(buf, CancellationToken.None);
    
                if (result.MessageType == WebSocketMessageType.Close) {
                    return;
                }
    
                stream.Write(buf[0..result.Count].Span);
                if (result.EndOfMessage) {
                    var msg = stream.ToArray();
                    stream.Seek(0, SeekOrigin.Begin);
    
                    Logger.GeneralLogger.Trace("ClientData: " + string.Join(" ", msg));
                    using var reader = new EndianAwareBinaryReader(stream);
                    var masked = reader.ReadUInt32();
                    Logger.GeneralLogger.Trace("masked: " + masked);
                    var EMsg = (EMsg)(~PROTOBUF_MASK & masked);
                    Logger.GeneralLogger.Trace("unmasked: " + EMsg);
    
                    // Read the header
                    var header_size = reader.ReadUInt32();
                    Logger.GeneralLogger.Trace("header_size: " + header_size);
                    byte[] header_binary = reader.ReadBytes((int)header_size);
                    
                    // Parse the header
                    var header = CMsgProtoBufHeader.Parser.ParseFrom(header_binary);
                    Logger.GeneralLogger.Write(header.ToString());
    
                    lock (waitingHandlersLock)
                    {
                        var matches = waitingHandlers.Where(k => k.Key == header.JobidTarget).ToList();
    
                        foreach (var item in matches)
                        {
                            waitingHandlers.Remove(item);
                            item.Value.SetResult(msg);
                        }
                    }
    
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.SetLength(0);
                }
            }
            catch (System.Exception e)
            {
                Logger.GeneralLogger.Trace("Loop encountered error: " + e.ToString());
                throw;
            }
        }
    }

    public const uint PROTOBUF_MASK = 0x80000000;

    private readonly object waitingHandlersLock = new();
    private readonly List<KeyValuePair<ulong, TaskCompletionSource<byte[]>>> waitingHandlers = new();
    private async void ClientSocketReadThread() {
        Memory<byte> buf = new byte[4096];
        using MemoryStream stream = new();

        while (readThreadRunning)
        {
            try
            {
                if (clientWS == null) {
                    return;
                }
    
                if (clientWS.State != WebSocketState.Open) {
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
    
                var result = await clientWS.ReceiveAsync(buf, CancellationToken.None);
    
                if (result.MessageType == WebSocketMessageType.Close) {
                    return;
                }
    
                stream.Write(buf[0..result.Count].Span);
                if (result.EndOfMessage) {
                    var msg = stream.ToArray();
                    stream.Seek(0, SeekOrigin.Begin);
    
                    Logger.GeneralLogger.Trace("ClientData: " + string.Join(" ", msg));
                    using var reader = new EndianAwareBinaryReader(stream);
                    var masked = reader.ReadUInt32();
                    Logger.GeneralLogger.Trace("masked: " + masked);
                    var EMsg = (EMsg)(~PROTOBUF_MASK & masked);
                    Logger.GeneralLogger.Trace("unmasked: " + EMsg);
    
                    // Read the header
                    var header_size = reader.ReadUInt32();
                    Logger.GeneralLogger.Trace("header_size: " + header_size);
                    byte[] header_binary = reader.ReadBytes((int)header_size);
    
                    // Parse the header
                    var header = CMsgProtoBufHeader.Parser.ParseFrom(header_binary);
                    Logger.GeneralLogger.Trace(header.ToString());
    
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.SetLength(0);
                }
            }
            catch (System.Exception e)
            {
                Logger.GeneralLogger.Trace("Loop encountered error: " + e.ToString());
                throw;
            }
        }
    }

    private CMsgWebUITransportInfo? transportInfo;
    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        steamClient.IClientUtils.SetWebUITransportWebhelperPID((uint)Environment.ProcessId);

        using (var hack = ProtobufHack.Create<CMsgWebUITransportInfo>()) {

            Logger.GeneralLogger.Trace("info: " + steamClient.IClientUtils.GetWebUITransportInfo(hack.ptr));
            transportInfo = hack.GetManaged();
        }

        Logger.GeneralLogger.Trace(transportInfo.ToString());

        transportWS = new ClientWebSocket();
        transportWS.Options.SetRequestHeader("Origin", "https://steamloopback.host");
        transportWS.Options.SetRequestHeader("Authorization", transportInfo.AuthKey);
        await transportWS.ConnectAsync(new Uri($"ws://127.0.0.1:{transportInfo.Port}/transportsocket/"), CancellationToken.None);
        
        transportThread = new Thread(TransportReadThread);
        transportThread.Start();

        await SendTransportMessage("TransportAuth.Authenticate#1", new CTransportAuth_Authenticate_Request() { AuthKey = transportInfo.AuthKey });

        // I'm unsure what to do with this. This could also just be a way to access the IPC interfaces, like the TCP-based SteamPipe protocol.
        clientWS = new ClientWebSocket();
        clientWS.Options.SetRequestHeader("Origin", "https://steamloopback.host");
        await clientWS.ConnectAsync(new Uri($"ws://127.0.0.1:27060/clientsocket/"), CancellationToken.None);

        clientThread = new Thread(ClientSocketReadThread);
        clientThread.Start();
    }

    public async Task SendTransportMessage<T>(string msgName, T body, bool notification = false) where T: IMessage<T>, new() {
        if (transportWS == null) {
            throw new InvalidOperationException("No websocket connection exists.");
        }

        var msgid = Interlocked.Increment(ref currentMsgID);
        var waitTask = WaitForTransportMessageResponse(msgid);
        var req = new ProtoMsg<T>(msgName, true);
        req.EMsg = EMsg.ServiceMethod;
        req.body = body;
        req.header.JobidSource = msgid;
        req.header.WebuiAuthKey = transportInfo?.AuthKey;

        await transportWS.SendAsync(req.Serialize(), WebSocketMessageType.Binary, true, CancellationToken.None);
        await waitTask;
    }

    private Task<byte[]> WaitForTransportMessageResponse(ulong msgid) {
        TaskCompletionSource<byte[]> tcs = new();
        lock (waitingHandlersLock)
        {
            waitingHandlers.Add(new(msgid, tcs));
        }

        return tcs.Task;
    }

    public async Task OnLoggingOff(IProgress<string> progress)
    {
        readThreadRunning = false;
        transportThread = null;
        currentMsgID = 1;

        if (transportWS == null) {
            return;
        }

        await transportWS.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
    }
}