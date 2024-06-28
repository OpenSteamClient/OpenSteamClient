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
using System.Linq;
using System.Net;

namespace OpenSteamworks.IPCClient;

//TODO: Needs Windows support
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

    private readonly TcpClient tcpClient;
    public enum IPCConnectionType {
        Client,
        Service
    }

    public uint IPCCallCount { get; private set; } = 0;
    public IPCConnectionType ConnectionType { get; private set; }
    public bool CallbacksAvailable { get; private set; }

    private readonly Thread callbackReadThread;
    private bool callbackReadThreadShouldRun = true;
    private bool pipeIsConnected = false;
    private readonly object serializeLock = new();

    public IPCClient(string ipcService, IPCConnectionType connectionType) {
        this.callbackReadThread = new(CallbackReadThread);
        this.ConnectionType = connectionType;
        IPEndPoint ep = GetEndPointFromName(ipcService);
        Logging.IPCLogger.Info("Connecting to " + ep.ToString());
        
        tcpClient = new TcpClient(ep.Address.ToString(), ep.Port);
        tcpClient.NoDelay = true;

        Logging.IPCLogger.Info("Connected to " + ep.ToString());

        ConnectToSteamPipe(out uint hostPid);
        
        if (connectionType == IPCConnectionType.Client) {
            ConnectToGlobalUser();
        }

        this.callbackReadThread.Start();
    }

    private void CallbackReadThread() {
        var stream = tcpClient.GetStream();
        while (callbackReadThreadShouldRun)
        {
            if (!tcpClient.Connected) {
                Shutdown();
                break;
            }

           lock (serializeLock)
           {
                while (stream.DataAvailable)
                {
                    if (!tcpClient.Connected) {
                        break;
                    }
    
                    var available = tcpClient.Available;
                    byte[] buf = new byte[available];
                    stream.Read(buf, 0, available);
                    HandleData(buf);
                }
           }
        }

        callbackReadThreadShouldRun = true;
    }

    private uint expectedLength = 0;
    private readonly List<byte> incompleteMsg = new();

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
        if (incompleteMsg.Count - sizeof(uint) < expectedLength)
        {
            msg = null;
            return false;
        }

        expectedLength = 0;
        msg = incompleteMsg.ToArray();
        incompleteMsg.Clear();
        return true;
    }
    
    private void HandleData(byte[] partial)
    {
        Logging.IPCLogger.Debug("Got data from server " + string.Join(" ", partial));
        if (HandlePartial(partial, out byte[]? msg))
        {
            Logging.IPCLogger.Debug("Full msg: " + string.Join(" ", msg));
            using var stream = new MemoryStream(msg);
            using var reader = new EndianAwareBinaryReader(stream, OpenSteamworks.Utils.Enum.Endianness.Little);

            var len = reader.ReadUInt32();
            var cmd = reader.ReadByte();
            if (!System.Enum.IsDefined(typeof(IPCResponseCode), cmd))
            {
                Logging.IPCLogger.Debug("Unsupported response code " + cmd);
                return;
                //throw new InvalidOperationException("Unsupported response code " + cmd);
            }

            try
            {
                HandleMessage((IPCResponseCode)cmd);
            }
            catch (System.Exception e)
            {
                Logging.IPCLogger.Debug("Got error while handling message:");
                Logging.IPCLogger.Debug(e);
                if (!pipeIsConnected) {
                    Logging.IPCLogger.Debug("Error is fatal");

                    // This has to be done like this, otherwise it will hang forever
                    Task.Run(() => this.Shutdown());
                }
            }
        }
    }
    
    private void HandleMessage(IPCResponseCode code)
    {
        switch (code)
        {
            case IPCResponseCode.CallbacksAvailable: // S: 1 0 0 0 7
                var cb = SendAndWaitForResponse(IPCCommandCode.SerializeCallbacks, []); // C: 1 0 0 0 2
                // S: 17 0 0 0 2 1 0 0 0 209 161 16 0 4 0 0 0 1 0 0 0
                // S: 13 0 0 0 2 0 0 0 0 0 0 0 0 0 0 0 0
                // S: 1 0 0 0 7
                Logging.IPCLogger.Debug("Got CB " + ReadCB(cb));
                while (tcpClient.Client.Poll(TimeSpan.FromMilliseconds(56), SelectMode.SelectRead))
                {
                    Logging.IPCLogger.Debug("poll success, avail: " + tcpClient.Available);
                    if (tcpClient.Available < 13) {
                        break;
                    }

                    var cb2 = WaitForMessageOfLength(13); // S: 13 0 0 0 2 0 0 0 0 0 0 0 0 0 0 0 0
                    if (cb2[4] == (byte)IPCCommandCode.SerializeCallbacks) {
                        Logging.IPCLogger.Debug("More?");
                        var ca2 = WaitForMessageOfLength(5);
                        if (ca2[4] == (byte)IPCResponseCode.CallbacksAvailable) {
                            Logging.IPCLogger.Debug("More callbacks available");

                            cb2 = WaitForMessageOfLength(13);
                            Logging.IPCLogger.Debug("Got CB2 " + ReadCB(cb2));
                        } else {
                            Logging.IPCLogger.Debug("No");
                            break;
                        }
                    } else {
                        
                    }
                }

                break;
            default:
                Logging.IPCLogger.Debug("Got unsupported command " + code);
                break;
        }
    }

    private int ReadCB(byte[] cb) {
        using var reader = new EndianAwareBinaryReader(new MemoryStream(cb), Utils.Enum.Endianness.Little);
        var firstByte = reader.ReadByte();
        if (firstByte != 2) {
            Logging.IPCLogger.Debug("CB unknown first byte " + firstByte);
        }

        var steamUser = reader.ReadInt32();
        var callbackID = reader.ReadInt32();
        var len = reader.ReadUInt32();
        var callbackData = reader.ReadBytes((int)len);
        return callbackID;
    }

    private IPEndPoint GetEndPointFromName(string ipcName) {
        string? overrideIP = Environment.GetEnvironmentVariable($"{ipcName}_{Environment.ProcessId}");
        if (!string.IsNullOrEmpty(overrideIP)) {
            if (IPEndPoint.TryParse(overrideIP, out IPEndPoint? endPoint)) {
                return endPoint;
            }

        } else {
            overrideIP = Environment.GetEnvironmentVariable(ipcName);
            if (!string.IsNullOrEmpty(overrideIP)) {
                if (IPEndPoint.TryParse(overrideIP, out IPEndPoint? endPoint)) {
                    return endPoint;
                }
            }
        }

        if (ipcName == "Steam3Master") {
            return new IPEndPoint(IPAddress.Loopback, 57343);
        } else if (ipcName == "SteamClientService") {
            return new IPEndPoint(IPAddress.Loopback, 57344);
        }

        throw new ArgumentException("Unknown IPC service " + ipcName, nameof(ipcName));
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
        Logging.IPCLogger.Info("Connect to pipe");
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream);
            // | Success | ProcID | ThreadID |
            writer.WriteUInt32(1);
            writer.WriteUInt32((uint)Environment.ProcessId);
            
            if (OperatingSystem.IsWindows()) {
                [DllImport("kernel32.dll")]
                static extern int GetCurrentThreadId();
                writer.WriteUInt32((uint)GetCurrentThreadId());
            } else if (OperatingSystem.IsLinux()) {
                [DllImport("libc")]
                static extern int gettid();
                writer.WriteUInt32((uint)gettid());
            } else {
                throw new PlatformNotSupportedException();
            }

            var resp2 = SendAndWaitForResponse(IPCCommandCode.ConnectPipe, stream.ToArray());
            using (var response = new MemoryStream(resp2)) {
                var reader = new EndianAwareBinaryReader(response);
                // Length (12) | HostPID | HostThreadID | Seed |
                Trace.Assert(reader.ReadUInt32() == 12);
                hostPid = reader.ReadUInt32();
                var hosttid = reader.ReadUInt32();
                var seed = reader.ReadUInt32();
                Logging.IPCLogger.Info($"Got host pid: {hostPid}, host tid: {hosttid}, seed: {seed}");
                ThrowIfInvalidHostProcess((int)hostPid, (int)hosttid);
                pipeIsConnected = true;
            }
        }
    }

    private void ThrowIfInvalidHostProcess(int pid, int tid) {
        var proc = Process.GetProcesses().First(p => p.Id == pid);
        if (proc == null) {
            throw new Exception("Process " + pid + " does not exist!");
        }

        for (int i = 0; i < proc.Threads.Count; i++)
        {
            var thrd = proc.Threads[i];
            if (thrd.Id == tid) {
                return;
            }
        }

        throw new Exception($"Thread {tid} in process {pid} ({proc.ProcessName}) does not exist!");
    }

    public HSteamUser ConnectToGlobalUser() {
        Logging.IPCLogger.Info("Connect to user");

        // | GlobalUserType |
        uint hUser;
        var resp = SendAndWaitForResponse(IPCCommandCode.ConnectToGlobalUser, new byte[] {2});
        using (var response = new MemoryStream(resp)) {
            var reader = new EndianAwareBinaryReader(response);

            reader.ReadByte();
            hUser = reader.ReadUInt32();
            if (hUser+1 < 2) {
                throw new Exception($"ConnectToGlobalUser returned invalid HSteamUserEngine={hUser+1}");
            }

            Logging.IPCLogger.Info("Got HSteamUser " + hUser);
        }

        return (int)hUser;
    }

    public byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data) {
        lock (serializeLock)
        {
            var serialized = Serialize(command, data);
            Send(serialized);
            Logging.IPCLogger.Debug($"sent {data.Length} bytes as {command}, waiting for response");
            var resp = WaitForResponseTo(command);

            if (command == IPCCommandCode.Interface) {
                IPCCallCount++;
            }

            return resp;
        }
    }

    private byte[] Serialize(IPCCommandCode command, byte[] data) {

        var hdr = new ClientMsgHeader() { DataLength = (uint)data.Length, Command = command }.Serialize();
        byte[] bytes;
        using (var stream = new MemoryStream()) {
            stream.Write(hdr);
            stream.Write(data);
            bytes = stream.ToArray();
        }

        return bytes;
    }

    private byte[] WaitForResponseTo(IPCCommandCode sentCommand) {
        switch (sentCommand)
        {
            case IPCCommandCode.Interface:
            case IPCCommandCode.SerializeCallbacks:
            case IPCCommandCode.ConnectToGlobalUser:
                return WaitForMessage();
            case IPCCommandCode.ConnectPipe:
                return WaitForMessageOfLength(16);
            case IPCCommandCode.TerminatePipe:
            default:
                throw new InvalidOperationException("Unhandled IPCCommandCode (or received error)");
        }

        throw new UnreachableException();
    }

    private SpinLock socketLock = new();

    public void Shutdown() {
        this.tcpClient.Close();
    }

    private void Send(byte[] buffer) {
        bool gotLock = false;
        try
        {
            socketLock.Enter(ref gotLock);
            var stream = tcpClient.GetStream();
            stream.Write(buffer);
            stream.Flush();
        }
        finally
        {
            if (gotLock) socketLock.Exit();
        }
    }

    private byte[] WaitForMessageOfLength(int length) {
        var stream = tcpClient.GetStream();
        bool gotLock = false;
        try
        {
            socketLock.Enter(ref gotLock);
            while (tcpClient.Available < length) { }
            
            byte[] msg = new byte[length];
            stream.ReadExactly(msg, 0, length);
            return msg;
        }
        finally
        {
            if (gotLock) socketLock.Exit();
        }
    }

    /// <summary>
    /// Waits for a message of any length.
    /// </summary>
    private byte[] _WaitForMessage() {
        if (!tcpClient.Connected) {
            throw new InvalidOperationException("TCPClient lost connection");
        }
        
        var stream = tcpClient.GetStream();

        while (tcpClient.Available < sizeof(uint)) { }

        int length = 0;
        {
            byte[] lengthBuf = new byte[sizeof(uint)];
            Trace.Assert(stream.Read(lengthBuf, 0, sizeof(uint)) == sizeof(uint));
            using var reader = new BinaryReader(new MemoryStream(lengthBuf));
            length = reader.ReadInt32();
        }

        while (tcpClient.Available < length) { }

        byte[] buf = new byte[length];
        stream.Read(buf, 0, length);
        return buf;
    }

    public byte[] WaitForMessage() {
        var msg = _WaitForMessage();

        if (msg.Length == 0) {
            return msg;
        }

        //TODO: There's probably a better way to check this, but just do this for now
        if (msg[0] == 7 && msg.Length == 1) {
            CallbacksAvailable = true;
            msg = _WaitForMessage();
        }

        Logging.IPCLogger.Debug("Got msg " + string.Join(" ", msg));

        return msg;
    }

    public bool BGetCallback(out CallbackMsg_t callback) {
        callback = new();
        
        // lock (commandLock)
        // {
        //     var stream = tcpClient.GetStream();
        //     if (!CallbacksAvailable && stream.DataAvailable) {
        //         byte b = (byte)stream.ReadByte();
        //         if (b == (byte)IPCResponseCode.CallbacksAvailable) {
        //             CallbacksAvailable = true;
        //         }
        //     }

        //     if (CallbacksAvailable) {
        //         CallbacksAvailable = false;
        //         var resp = SendAndWaitForResponse(IPCCommandCode.SerializeCallbacks, []);
        //         Logging.IPCLogger.Debug("Resp: " + resp.Length);
        //         using var reader = new EndianAwareBinaryReader(new MemoryStream(resp), Utils.Enum.Endianness.Little);
        //         var firstByte = reader.ReadByte();
        //         if (firstByte != 2) {
        //             Logging.IPCLogger.Debug("CB unknown first byte " + firstByte);
        //         }

        //         callback.steamUser = reader.ReadInt32();
        //         callback.callbackID = reader.ReadInt32();
        //         var len = reader.ReadUInt32();
        //         callback.callbackData = reader.ReadBytes((int)len);

        //         if (stream.DataAvailable) {
        //             byte b = (byte)stream.ReadByte();
        //             if (b == (byte)IPCResponseCode.CallbacksAvailable) {
        //                 Logging.IPCLogger.Debug("Another callback");
        //                 CallbacksAvailable = true;
        //             }

        //             // byte b2 = (byte)stream.ReadByte();
        //             // if (b2 == (byte)IPCResponseCode.CallbacksAvailable) {
        //             //     Logging.IPCLogger.Debug("More than 1 callback");
        //             // }
        //         } else {
        //             Logging.IPCLogger.Debug("No more data");
        //         }

        //         return true;
        //     }
        // }

        return false;
    }
}