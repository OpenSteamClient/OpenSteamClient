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

    private readonly TcpClient tcpClient;
    public enum IPCConnectionType {
        Client,
        Service
    }

    public uint IPCCallCount { get; private set; } = 0;
    public IPCConnectionType ConnectionType { get; private set; }
    public bool CallbacksAvailable { get; private set; }

    public IPCClient(string ipaddress, IPCConnectionType connectionType, bool skipInitialization = false) {
        this.ConnectionType = connectionType;
        Logging.IPCLogger.Info("Connecting to " + ipaddress);
        
        var ports = ipaddress.Split(":");
        string ip = ports[0];
        int port = int.Parse(ports[1], CultureInfo.InvariantCulture.NumberFormat);
        tcpClient = new TcpClient(ip, port);
        tcpClient.NoDelay = true;

        Logging.IPCLogger.Info("Connected to " + ipaddress);

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
        Logging.IPCLogger.Info("Connect to pipe");
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream);
            // | Success | ProcID | ThreadID |
            writer.WriteUInt32(1);
            writer.WriteUInt32((uint)Environment.ProcessId);
            writer.WriteUInt32((uint)Environment.CurrentManagedThreadId);
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
        uint HSteamUser;
        var resp = SendAndWaitForResponse(IPCCommandCode.ConnectToGlobalUser, new byte[] {2});
        using (var response = new MemoryStream(resp)) {
            var reader = new EndianAwareBinaryReader(response);

            HSteamUser = reader.ReadUInt32();
            if (HSteamUser+1 < 2) {
                throw new Exception($"ConnectToGlobalUser returned invalid HSteamUserEngine={HSteamUser+1}");
            }

            Logging.IPCLogger.Info("Got HSteamUser " + HSteamUser);
        }

        return (int)HSteamUser;
    }

    public void SendAndIgnoreResponse(IPCCommandCode command, byte[] data) {
        var serialized = Serialize(command, data);
        Send(serialized);
        Logging.IPCLogger.Debug($"sent {data.Length} bytes");
    }

    public byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data) {
        var serialized = Serialize(command, data);
        Send(serialized);
        Logging.IPCLogger.Debug($"sent {data.Length} bytes as {command}, waiting for response");
        var resp = WaitForResponseTo(command);
        if (command == IPCCommandCode.Interface) {
            IPCCallCount++;
        }

        return resp;
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
        TaskCompletionSource<byte[]> tcs = new();
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

        //TODO: There's probably a better way to check this, but just do this for now
        if (msg[0] == 7 && msg.Length == 1) {
            CallbacksAvailable = true;
            msg = _WaitForMessage();
        }

        return msg;
    }
}