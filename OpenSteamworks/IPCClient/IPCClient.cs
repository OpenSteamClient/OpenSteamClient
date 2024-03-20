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
using System.Diagnostics;

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

    public enum IPCResponseCode : byte {
        SerializeCallbacks = 2,
        Interface = 11,
        ConnectToGlobalUser = 3,
        ReleaseGlobalUser = 4,
        CallbacksAvailable = 7,
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

    private TcpClient tcpClient;
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
        
        var ports = ipaddress.Split(":");
        string ip = ports[0];
        int port = int.Parse(ports[1], CultureInfo.InvariantCulture.NumberFormat);
        tcpClient = new TcpClient(ip, port);
        tcpClient.NoDelay = true;

        Logging.NativeClientLogger.Info("Connected to " + ipaddress);

        if (!skipInitialization) {
            ConnectToSteamPipe(out uint hostPid);
            
            pollThread = new(PollThread);
            pollThread.Start();
            
            if (connectionType == IPCConnectionType.Client) {
                ConnectToGlobalUser();
            }
        } else {
            pollThread = new(PollThread);
            pollThread.Start();
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
        var sentLen = Send(serialized);
        Logging.NativeClientLogger.Debug($"sent {sentLen} bytes");
    }

    public unsafe byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data) {
        var serialized = Serialize(command, data);
        var sentLen = Send(serialized);
        Logging.NativeClientLogger.Debug($"sent {sentLen} bytes, waiting for response");
        var resp = WaitForResponseTo(command);
        if (command == IPCCommandCode.Interface) {
            IPCCallCount++;
        }

        return resp;
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
    private Action<byte[]>? connectToGlobaluserResultHandler;

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
                connectToGlobaluserResultHandler = (data) =>
                {
                    tcs.TrySetResult(data);
                    connectToGlobaluserResultHandler = null;
                };
                break;
            case IPCCommandCode.ConnectPipe:
                if (pollThread != null) {
                    throw new InvalidOperationException("Tried to connect pipe but poll thread already exists! Please construct a new IPCClient if you want to create a new pipe/reconnect");
                }

                return WaitForMessageOfLength(12);
            case IPCCommandCode.TerminatePipe:
            default:
                throw new InvalidOperationException("Unhandled IPCCommandCode");
        }

        // This is probably bad
        tcs.Task.Wait();
        return tcs.Task.Result;
    }

    private bool pollShouldRun = true;
    private SpinLock socketLock = new();

    private uint expectedLength = 0;
    private readonly List<byte> incompleteMsg = [];

    private void PollThread() {
        var stream = tcpClient.GetStream();
        while (pollShouldRun)
        {
            if (!tcpClient.Connected) {
                return;
            }

            
            while (stream.DataAvailable)
            {
                if (!tcpClient.Connected) {
                    break;
                }

                //TODO: This probably isn't ideal
                var available = tcpClient.Available;
                byte[] buf = new byte[available];
                stream.Read(buf, 0, available);
                HandleData(buf);
            }
        }
    }

    private void HandleData(byte[] partial)
    {
        if (HandlePartial(partial, out byte[]? msg))
        {
            using var stream = new MemoryStream(msg);
            using var reader = new EndianAwareBinaryReader(stream, OpenSteamworks.Utils.Enum.Endianness.Little);

            //TODO
        }
    }

    /// <summary>
    /// Handles a partial message.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="msg"></param>
    /// <returns>False if the data was partial, true if a full message was completed</returns>
    private bool HandlePartial(byte[] data, [NotNullWhen(true)] out byte[]? msg)
    {
        using var stream = new MemoryStream(data);
        using var reader = new EndianAwareBinaryReader(stream, OpenSteamworks.Utils.Enum.Endianness.Little);

        // If we don't have a partial message, read the length now
        if (expectedLength == 0)
        {
            expectedLength = reader.ReadUInt32();
        }

        // Add to the incomplete message buffer
        incompleteMsg.AddRange(data);

        // Exit now if we have a partial message
        if (incompleteMsg.Count - sizeof(uint) != expectedLength)
        {
            msg = null;
            return false;
        }

        expectedLength = 0;
        msg = incompleteMsg.ToArray();
        incompleteMsg.Clear();
        return true;
    }

    private void HandleMessage(byte[] msg) {
        using (var stream = new MemoryStream(msg))
        {
            var reader = new EndianAwareBinaryReader(stream);

            switch ((IPCResponseCode)reader.ReadByte())
            {
                case IPCResponseCode.SerializeCallbacks:
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
                case IPCResponseCode.Interface:
                    Logging.NativeClientLogger.Debug("Got function response data!");
                    commandResultHandler?.Invoke(msg[1..]);
                    break;
                case IPCResponseCode.ConnectToGlobalUser:
                    if (msg.Length == 5) {
                        connectToGlobaluserResultHandler?.Invoke(msg[1..]);
                        break;                        
                    }
                    goto default;

                case IPCResponseCode.CallbacksAvailable: //Unrequested message, don't send this anywhere
                    if (msg.Length == 1) {
                        Logging.NativeClientLogger.Debug("Callbacks available in queue");
                        QueueCallbackSerialize();
                        break;
                    }
                    goto default;
                default:
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
        var serialized = Serialize(IPCCommandCode.SerializeCallbacks, new byte[] { 1 });
        Send(serialized);
    }

    private int Send(byte[] buffer) {
        bool gotLock = false;
        try
        {
            socketLock.Enter(ref gotLock);
            return socket.Send(buffer);
        }
        finally
        {
            if (gotLock) socketLock.Exit();
        }
    }

    private byte[] WaitForMessageOfLength(int length, bool callFromPollThread = false) {
        if (!callFromPollThread && this.pollThread != null) {
            throw new InvalidOperationException("Do not call WaitForMessageOfLength if pollthread has been created! They will conflict and result in undefined behaviour!");
        }

        bool gotLock = false;
        try
        {
            socketLock.Enter(ref gotLock);
            byte[] msg = new byte[length];
            var recvLen = socket.Receive(msg, 0, length, SocketFlags.None);
            Console.WriteLine($"{recvLen} == {length}");
            Trace.Assert(recvLen == length);
            return msg;
        }
        finally
        {
            if (gotLock) socketLock.Exit();
        }
    }
}