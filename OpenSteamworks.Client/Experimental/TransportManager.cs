using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Google.Protobuf;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Messaging;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Protobuf.WebUI;
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
    private ulong currentClientMsgID = 0;

    private readonly object waitingHandlersLock = new();
    private readonly List<KeyValuePair<ulong, TaskCompletionSource<byte[]>>> waitingHandlers = new();
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
                    var header = Protobuf.WebUI.CMsgProtoBufHeader.Parser.ParseFrom(header_binary);
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
                return;
            }
        }
    }

    public const uint PROTOBUF_MASK = 0x80000000;
    private readonly object waitingClientHandlersLock = new();
    private readonly List<KeyValuePair<ulong, TaskCompletionSource<JsonNode>>> waitingClientHandlers = new();
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
                    string msg = Encoding.UTF8.GetString(stream.ToArray());
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.SetLength(0);
    
                    Logger.GeneralLogger.Trace("ClientData: '" + msg + "'");

                    JsonNode jsonMsg;
                    ulong seq;
                    try
                    {
                        jsonMsg = JsonNode.Parse(msg)!;
                        seq = (ulong)jsonMsg["sequenceid"]!;
                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine("Error parsing message: " + e);
                        return;
                    }

                    lock (waitingClientHandlersLock)
                    {
                        var matches = waitingClientHandlers.Where(k => k.Key == seq).ToList();
    
                        foreach (var item in matches)
                        {
                            waitingClientHandlers.Remove(item);
                            item.Value.SetResult(jsonMsg);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Logger.GeneralLogger.Trace("Loop encountered error: " + e.ToString());
                return;
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

        await SendTransportMessage("TransportAuth.Authenticate#1", new Protobuf.WebUI.CTransportAuth_Authenticate_Request() { AuthKey = transportInfo.AuthKey });

        // I'm unsure what to do with this. This could also just be a way to access the IPC interfaces, like the TCP-based SteamPipe protocol.
        clientWS = new ClientWebSocket();
        clientWS.Options.SetRequestHeader("Origin", "https://steamloopback.host");
        await clientWS.ConnectAsync(new Uri($"ws://127.0.0.1:27060/clientsocket/"), CancellationToken.None);

        clientThread = new Thread(ClientSocketReadThread);
        clientThread.Start();

        var resp = await SendClientMessage("GetClientInfo");
        if ((int?)resp["success"] != 1) {
            Logger.GeneralLogger.Error("GetClientInfo failed");
            return;
        }

        lock (supportedClientMessagesLock)
        {
            supportedClientMessages = new(resp["supported_messages"]!.AsArray().Select(s => ((string?)s) ?? string.Empty));
        }
        Logger.GeneralLogger.Trace("GetClientInfo, supported messages: " + string.Join(", ", supportedClientMessages));
    }

    public async Task<byte[]> SendTransportMessage<T>(string msgName, T body, bool notification = false) where T: IMessage<T>, new() {
        if (transportWS == null) {
            throw new InvalidOperationException("No websocket connection exists.");
        }

        var msgid = Interlocked.Increment(ref currentMsgID);
        var waitTask = WaitForTransportMessageResponse(msgid);
        var req = new ProtoMsg<T>(msgName, true)
        {
            EMsg = EMsg.ServiceMethod,
            body = body
        };
        
        req.header.JobidSource = msgid;
        req.header.WebuiAuthKey = transportInfo?.AuthKey;

        await transportWS.SendAsync(req.Serialize(), WebSocketMessageType.Binary, true, CancellationToken.None);

        if (!notification) {
            return await waitTask;
        } else {
            return [];
        }
    }

    private readonly object supportedClientMessagesLock = new();
    private List<string> supportedClientMessages = new() { "GetClientInfo" };
    public async Task<JsonNode> SendClientMessage(string msgName, JsonObject? body = null, bool notification = false) {
        if (clientWS == null) {
            throw new InvalidOperationException("No websocket connection exists.");
        }

        lock (supportedClientMessagesLock)
        {
            if (!supportedClientMessages.Contains(msgName)) {
                Logger.GeneralLogger.Warning("Sending unsupported message type '" + msgName + "'");
            }
        }

        var msgid = Interlocked.Increment(ref currentClientMsgID);
        var waitTask = WaitForClientMessageResponse(msgid);
        
        body ??= new();
        body["message"] = msgName;
        body["sequenceid"] = msgid.ToString();
        body["universe"] = ((int)this.steamClient.IClientUtils.GetConnectedUniverse()).ToString();
        body["accountid"] = this.steamClient.IClientUser.GetSteamID().AccountID.ToString();

        await clientWS.SendAsync(Encoding.UTF8.GetBytes(body.ToJsonString(null)), WebSocketMessageType.Text, true, CancellationToken.None);

        if (!notification) {
            return await waitTask;
        } else {
            return new JsonObject();
        }
    }

    private Task<byte[]> WaitForTransportMessageResponse(ulong msgid) {
        TaskCompletionSource<byte[]> tcs = new();
        lock (waitingHandlersLock)
        {
            waitingHandlers.Add(new(msgid, tcs));
        }

        return tcs.Task;
    }

    private Task<JsonNode> WaitForClientMessageResponse(ulong msgid) {
        TaskCompletionSource<JsonNode> tcs = new();
        lock (waitingClientHandlersLock)
        {
            waitingClientHandlers.Add(new(msgid, tcs));
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