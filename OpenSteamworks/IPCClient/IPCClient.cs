using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenSteamworks.Extensions;
using OpenSteamworks.Messaging;
using OpenSteamworks.Structs;
using System.Globalization;
using OpenSteamworks.Utils;

namespace OpenSteamworks.IPCClient;

public class IPCClient {
    public enum IPCCommandCode : byte {
        Interface = 1,
        SerializeCallbacks = 2,
        ConnectToGlobalUser = 3,
        ReleaseGlobalUser = 4,
        TerminatePipe = 5,
        Ping = 6,
        ConnectPipe = 9,
    }

    /// <summary>
    /// | size | command | data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ClientMsgHeader {
        public UInt32 DataLength;
        public IPCCommandCode Command;
        public unsafe byte[] Serialize() {
            if (sizeof(ClientMsgHeader) != 5) {
                throw new Exception("Unexpected size " + sizeof(ClientMsgHeader));
            }

            byte[] hdr = new byte[sizeof(ClientMsgHeader)];
            using (var stream = new MemoryStream(hdr))
            {
                var writer = new EndianAwareBinaryWriter(stream);
                writer.WriteUInt32(1 + DataLength);
                writer.Write((byte)Command);
            }

            return hdr;
        }
    }

    private Socket socket;
    public enum IPCConnectionType {
        Client,
        Service
    }

    public uint IPCCallCount { get; private set; } = 0;
    public Queue<CallbackMsg_t> CallbackQueue { get; init; } = new();
    public IPCConnectionType ConnectionType { get; private set; }
    private readonly Thread pollThread;

    public IPCClient(string ipaddress, IPCConnectionType connectionType, bool skipInitialization = false) {
        this.ConnectionType = connectionType;
        Logging.NativeClientLogger.Info("Connecting to " + ipaddress);
        socket = new(SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true
        };
        
        var ports = ipaddress.Split(":");
        string ip = ports[0];
        int port = int.Parse(ports[1], CultureInfo.InvariantCulture.NumberFormat);
        socket.Connect(ip, port);
        Logging.NativeClientLogger.Info("Connected to " + ipaddress);

        pollThread = new(PollThread);
        pollThread.Start();

        if (!skipInitialization) {
            ConnectToSteamPipe(out uint hostPid);

            if (connectionType == IPCConnectionType.Client) {
                ConnectToGlobalUser();
            }
        }
    }

    public void ReleaseUser(HSteamUser hUser) {
        uint user = (uint)(int)hUser;
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream);
            writer.WriteUInt32(user);
            SendAndWaitForResponse(IPCCommandCode.ReleaseGlobalUser, stream.ToArray());
        }
    }

    public void ResetIPCCallCount() {
        IPCCallCount = 0;
    }

    public void ConnectToSteamPipe(out uint hostPid) {
        hostPid = 0;
        Logging.NativeClientLogger.Info("Connect to pipe");
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream);
            // | Success | ProcID | ThreadID |
            writer.WriteUInt32(1);
            writer.WriteUInt32((uint)Environment.ProcessId);
            writer.WriteUInt32((uint)Environment.CurrentManagedThreadId);
            var resp2 = SendAndWaitForResponse(IPCCommandCode.ConnectPipe, stream.ToArray());
            using (var response = new MemoryStream(resp2)) {
                var reader = new EndianAwareBinaryReader(response);
                // | HostPID | HostThreadID | Seed |
                hostPid = reader.ReadUInt32();
                var hosttid = reader.ReadUInt32();
                var seed = reader.ReadUInt32();
                Logging.NativeClientLogger.Info($"Got host pid: {hostPid}, host tid: {hosttid}, seed: {seed}");
            }
        }
    }

    public HSteamUser ConnectToGlobalUser() {
        Logging.NativeClientLogger.Info("Connect to user");

        // | GlobalUserType |
        uint HSteamUser;
        var resp = SendAndWaitForResponse(IPCCommandCode.ConnectToGlobalUser, new byte[] {2});
        using (var response = new MemoryStream(resp)) {
            var reader = new EndianAwareBinaryReader(response);

            HSteamUser = reader.ReadUInt32();
            if (HSteamUser+1 < 2) {
                throw new Exception($"ConnectToGlobalUser returned invalid HSteamUserEngine={HSteamUser+1}");
            }

            Logging.NativeClientLogger.Info("Got HSteamUser " + HSteamUser);
        }

        return (int)HSteamUser;
    }

    public void SendAndIgnoreResponse(IPCCommandCode command, byte[] data) {
        var serialized = Serialize(command, data);
        var sentLen = socket.Send(serialized);
        IPCCallCount++;
        Logging.NativeClientLogger.Debug($"sent {sentLen} bytes");
    }

    public unsafe byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data) {
        pauseHandling = true;
        var serialized = Serialize(command, data);
        var sentLen = socket.Send(serialized);
        IPCCallCount++;
        Logging.NativeClientLogger.Debug($"sent {sentLen} bytes, waiting for response");
        return WaitForResponseTo(command);
    }

    private unsafe byte[] Serialize(IPCCommandCode command, byte[] data) {

        var hdr = new ClientMsgHeader() { DataLength = (uint)data.Length, Command = command }.Serialize();
        byte[] bytes;
        using (var stream = new MemoryStream()) {
            stream.Write(hdr);
            stream.Write(data);
            bytes = stream.ToArray();
        }

        return bytes;
    }

    private Action<byte[]>? commandResultHandler;
    private Action<byte[]>? serializeCallbacksResultHandler;
    private Action<byte[]>? connectToGlobalUserResultHandler;
    private Action<byte[]>? connectPipeResultHandler;
    private byte[] WaitForResponseTo(IPCCommandCode sentCommand) {
        TaskCompletionSource<byte[]> tcs = new();
        switch (sentCommand)
        {
            case IPCCommandCode.Interface:
                commandResultHandler = (data) =>
                {
                    tcs.TrySetResult(data);
                    commandResultHandler = null;
                };
                break;
            case IPCCommandCode.SerializeCallbacks:
                serializeCallbacksResultHandler = (data) =>
                {
                    tcs.TrySetResult(data);
                    serializeCallbacksResultHandler = null;
                };
                break;
            case IPCCommandCode.ConnectToGlobalUser:
                connectToGlobalUserResultHandler = (data) =>
                {
                    tcs.TrySetResult(data);
                    connectToGlobalUserResultHandler = null;
                };
                break;
            case IPCCommandCode.Ping:
                break;
            case IPCCommandCode.ConnectPipe:
                connectPipeResultHandler = (data) =>
                {
                    tcs.TrySetResult(data);
                    connectPipeResultHandler = null;
                };
                break;
            case IPCCommandCode.TerminatePipe:
            default:
                throw new InvalidOperationException("Unhandled IPCCommandCode");
        }

        pauseHandling = false;

        // This is probably bad
        tcs.Task.Wait();
        return tcs.Task.Result;
    }

    private bool pauseHandling = true;
    private bool pollShouldRun = true;
    private readonly object pollLockObj = new();

    private void PollThread() {
        byte[] buffer = new byte[8192];
        while (pollShouldRun)
        {
            lock (pollLockObj)
            {
                var len = socket.Receive(buffer);
                if (!socket.Connected) {
                    return;
                }

                IPCCallCount++;
                Logging.NativeClientLogger.Debug("Got whole " + string.Join(" ", buffer[0..len]));  
                var msgs = SplitMessages(buffer[0..len]);
                Logging.NativeClientLogger.Debug("Data contains " + msgs.Count + " messages");
                foreach (var msg in msgs)
                {
                    Logging.NativeClientLogger.Debug("Msg: " + string.Join(" ", msg));  
                    HandleMessage(msg);
                }
            }
        }
    }

    private void HandleMessage(byte[] msg) {
        while (pauseHandling) { };
        using (var stream = new MemoryStream(msg))
        {
            var reader = new EndianAwareBinaryReader(stream);
            switch (reader.ReadByte())
            {
                case 2:
                    if (stream.Length < sizeof(UInt32)*3) {
                        goto default;
                    }

                    Logging.NativeClientLogger.Debug("Got callback data!");
                    var user = reader.ReadUInt32();
                    var id = reader.ReadUInt32();
                    var len = reader.ReadUInt32();

                    if (user == 0 && id == 0 && len == 0) {
                        
                    } else {
                        Logging.NativeClientLogger.Debug("CB: user: " + user + ", id: " + id + ", len: " + len);
                        CallbackQueue.Enqueue(new CallbackMsg_t() {steamUser = (int)user, callbackID = (int)id, callbackData = reader.ReadBytes((int)len) });
                    }
                    
                    serializeCallbacksResultHandler?.Invoke(msg[1..]);
                    break;
                case 11:
                    Logging.NativeClientLogger.Debug("Got function response data!");
                    commandResultHandler?.Invoke(msg[1..]);
                    break;
                case 3:
                    if (msg.Length == 5) {
                        Logging.NativeClientLogger.Debug("Connect to user data possibly received");

                        connectToGlobalUserResultHandler?.Invoke(msg[1..]);
                        break;
                    }
                    goto default;
                case 7: //Unrequested message, don't send this anywhere
                    if (msg.Length == 1) {
                        Logging.NativeClientLogger.Debug("Callbacks available in queue");
                        QueueCallbackSerialize();
                        break;
                    }
                    goto default;
                default:
                    if (msg.Length == 12 && connectPipeResultHandler != null) {
                        // Send the whole message to the connectpipe result handler, since it is probably waiting for a response
                        connectPipeResultHandler.Invoke(msg);
                        break;
                    }
                    Logging.NativeClientLogger.Warning("Got unsupported message type " + msg[0] + " with length " + msg.Length);
                    break;
            }
        }
    }

    public void Shutdown() {
        pollShouldRun = false;
        this.socket.Disconnect(true);
        this.CallbackQueue.Clear();
    }

    private void QueueCallbackSerialize() {
        lock (pollLockObj)
        {
            var serialized = Serialize(IPCCommandCode.SerializeCallbacks, new byte[] { 1 });
            var sentLen = socket.Send(serialized);
        }
    }

    private static List<byte[]> SplitMessages(byte[] data) {
        List<byte[]> messages = new();
        if (data.Length > 4) {
            using (var stream = new MemoryStream(data))
            {
                var reader = new EndianAwareBinaryReader(stream);

                uint totalsize = 0;
                while (true)
                {
                    if (stream.Position+4 >= stream.Length) {
                        break;
                    }

                    var readsize = reader.ReadUInt32();
                    
                    // No/invalid length in any of the headers, thus we only have a single headerless message
                    if (readsize >= data.Length || readsize == 0) {
                        return new List<byte[]>() {data};
                    }

                    totalsize += 4;
                    totalsize += readsize;

                    var readdata = reader.ReadBytes((int)readsize);
                    messages.Add(readdata); 
                }

                if (totalsize != stream.Position) {
                    throw new Exception("Sizes don't match in multipart message! totalsize " + totalsize + " data.Length " + data.Length);
                }
            }
        } else {
            return [data];
        }

        return messages;
    }
}