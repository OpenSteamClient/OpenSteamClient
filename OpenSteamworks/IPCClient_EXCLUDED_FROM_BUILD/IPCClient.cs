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
    private readonly IPCConnectionType connectionType;
    private readonly Thread pollThread;

    public IPCClient(string ipaddress, IPCConnectionType connectionType, bool skipInitialization = false) {
        this.connectionType = connectionType;
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

    private static string ReadStringOfLengthFromResponse(MemoryStream responseStream, int length) {
        var reader = new EndianAwareBinaryReader(responseStream, Encoding.UTF8);
        return Encoding.Default.GetString(reader.ReadBytes(length));
    }
    
    private static TRet ReadTypeFromStream<TRet>(MemoryStream stream) {
        return (TRet)ReadTypeFromStream(stream, typeof(TRet));
    }

    private static object ReadTypeFromStream(MemoryStream stream, Type type) {
        var reader = new EndianAwareBinaryReader(stream, Encoding.UTF8);
        if (type == typeof(int)) {
            return reader.ReadInt32();
        } else if (type == typeof(uint)) {
            return reader.ReadUInt32();
        } else if (type == typeof(long)) {
            return reader.ReadInt64();
        } else if (type == typeof(ulong)) {
            return reader.ReadUInt64();
        } else if (type == typeof(bool)) {
            return reader.ReadBoolean();
        } else if (type == typeof(char)) {
            // This is terrible
            return reader.ReadChar();
        } else if (type == typeof(byte)) {
            return reader.ReadByte();
        } else if (type == typeof(sbyte)) {
            return reader.ReadSByte();
        } else if (type == typeof(short)) {
            return reader.ReadInt16();
        } else if (type == typeof(ushort)) {
            return reader.ReadUInt16();
        } else if (type == typeof(nint)) {
            return (nint)reader.ReadInt64();
        } else if (type == typeof(nuint)) {
            return (nuint)reader.ReadUInt64();
        } else if (type == typeof(string)) {
            // Skip a byte
            reader.ReadByte();
            return reader.ReadNullTerminatedString();
        } else if (type.IsValueType) {
            return reader.ReadStruct(type);
        } else {
            throw new InvalidOperationException("Type " + type.Name + " is unsupported in IPC deserialization");
        }
    }

    private static void WriteObjectToStream(object obj, MemoryStream stream) {
        var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8);
        var objtype = obj.GetType();
        if (objtype == typeof(int)) {
            writer.Write((int)obj);
        } else if (objtype == typeof(uint)) {
            writer.Write((uint)obj);
        } else if (objtype == typeof(long)) {
            writer.Write((long)obj);
        } else if (objtype == typeof(ulong)) {
            writer.Write((ulong)obj);
        } else if (objtype == typeof(bool)) {
            writer.Write((bool)obj);
        } else if (objtype == typeof(char)) {
            // This is terrible
            writer.Write(Encoding.UTF8.GetBytes(new string(new char[] { (char)obj })));
        } else if (objtype == typeof(byte)) {
            writer.Write((byte)obj);
        } else if (objtype == typeof(sbyte)) {
            writer.Write((sbyte)obj);
        } else if (objtype == typeof(short)) {
            writer.Write((short)obj);
        } else if (objtype == typeof(ushort)) {
            writer.Write((ushort)obj);
        } else if (objtype == typeof(nint)) {
            writer.Write((nint)obj);
        } else if (objtype == typeof(nuint)) {
            writer.Write((nuint)obj);
        } else if (objtype == typeof(string)) {
            var strlen = ((string)obj).Length+1;
            if (strlen < byte.MaxValue+1) {
                writer.Write((byte)strlen);
            } else {
                writer.Write(byte.MaxValue);
            }
            writer.Write(Encoding.UTF8.GetBytes((string)obj + "\0"));
        } else if (objtype.IsArray) {
            throw new InvalidOperationException("How to serialize arrays?");
        } else if (objtype == typeof(StringBuilder)) {
            // No-op
        } else {
            throw new InvalidOperationException("Type " + objtype.Name + " is unsupported in IPC serialization");
        }

        writer.Flush();
    }

    // I hate this. Code duplication all over the place.
    public void CallIPCFunctionNoReturn(uint hUser, byte interfaceid, uint functionid, uint fencepost, object[] args) {
        if (interfaceid == 0 || fencepost == 0 || functionid == 0) {
            throw new Exception("No IPC info, cannot call function");
        }

        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8);

            stream.WriteByte(interfaceid);

            uint userToUse = 0;
            if (connectionType == IPCConnectionType.Client && !InterfaceMap.ClientInterfacesNoUser.Contains(interfaceid)) {
                userToUse = hUser;
            }

            Logging.NativeClientLogger.Info("USER: " + hUser);
            writer.Write(hUser);
            writer.Flush();

            Logging.NativeClientLogger.Info("Function ID: " + functionid);
            writer.Write(functionid);
            writer.Flush();
            List<int> changeableArgs = new();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var argtype = arg.GetType();
                if (argtype == typeof(StringBuilder)) {
                    changeableArgs.Add(Array.IndexOf(args, arg));
                }

                WriteObjectToStream(arg, stream);
            }

            // There seems to be another supported way to do fenceposts that relates to the end result of some math operation being 254. The original method relies on a randomly generated one instead (which would kind of suck to automate...)
            Console.WriteLine("Fencepost: " + fencepost);
            writer.Write(fencepost);
            writer.Flush();  

            var responseBytes = SendAndWaitForResponse(IPCCommandCode.Interface, stream.ToArray());
            Console.WriteLine("RESPONSE:" + string.Join(" ", responseBytes));

            using (var responseStream = new MemoryStream(responseBytes)) {
                // Handle changeable arguments
                foreach (var argi in changeableArgs)
                {
                    var argtype = args[argi].GetType();
                    var maxLen = (int)args[argi + 1];
                    if (argtype == typeof(StringBuilder)) {
                        (args[argi] as StringBuilder)!.Append(ReadStringOfLengthFromResponse(responseStream, maxLen));
                    }
                }
            }
        }
    }
    
    public FunctionSerializer CreateIPCFunctionCall(uint steamuser, byte interfaceid, uint functionid, uint fencepost) {
        return new FunctionSerializer(this.connectionType, steamuser, interfaceid, functionid, fencepost);
    }

    public FunctionDeserializer CallIPCFunctionEx(FunctionSerializer serializer) {
        var responseBytes = SendAndWaitForResponse(IPCCommandCode.Interface, serializer.Serialize());
        Console.WriteLine("RESPONSE:" + string.Join(" ", responseBytes));
        return new FunctionDeserializer(responseBytes);
    }

    public TRet CallIPCFunction<TRet>(uint hUser, byte interfaceid, uint functionid, uint fencepost, object[] args) {
        if (interfaceid == 0 || fencepost == 0 || functionid == 0) {
            throw new Exception("No IPC info, cannot call function");
        }
        
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8);

            stream.WriteByte(interfaceid);

            uint userToUse = 0;
            if (connectionType == IPCConnectionType.Client && !InterfaceMap.ClientInterfacesNoUser.Contains(interfaceid)) {
                userToUse = hUser;
            }

            Logging.NativeClientLogger.Info("USER: " + hUser);
            writer.Write(hUser);
            writer.Flush();

            Logging.NativeClientLogger.Info("Function ID: " + functionid);
            writer.Write(functionid);
            writer.Flush();
            List<int> changeableArgs = new();
            for (int i = 0; i < args.Length; i++)
            {
                Logging.NativeClientLogger.Info("Getting type of arg");
                var argtype = args[i].GetType();
                if (argtype == typeof(StringBuilder)) {
                    changeableArgs.Add(i);
                }

                WriteObjectToStream(args[i], stream);
            }

            // There seems to be another supported way to do fenceposts that relates to the end result of some math operation being 254. The original method relies on a randomly generated one instead (which would kind of suck to automate...)
            Console.WriteLine("Fencepost: " + fencepost);
            writer.Write(fencepost);
            writer.Flush();  

            var responseBytes = SendAndWaitForResponse(IPCCommandCode.Interface, stream.ToArray());
            Console.WriteLine("RESPONSE:" + string.Join(" ", responseBytes));

            TRet returnValue;
            using (var responseStream = new MemoryStream(responseBytes)) {
                returnValue = ReadTypeFromStream<TRet>(responseStream);

                // Handle changeable arguments
                foreach (var argi in changeableArgs)
                {
                    var argtype = args[argi].GetType();
                    var maxLen = (int)args[argi + 1];
                    if (argtype == typeof(StringBuilder)) {
                        (args[argi] as StringBuilder)!.Append(ReadStringOfLengthFromResponse(responseStream, maxLen));
                    } else if (argtype.IsArray) {
                        var reader = new EndianAwareBinaryReader(responseStream);
                        var elementtype = argtype.GetElementType()!;
                        var elementsize = Marshal.SizeOf(elementtype);
                        var elementsCount = reader.ReadUInt32();
                        if (elementsCount > maxLen) {
                            Logging.GeneralLogger.Warning("elementsCount is greater than max length! Some data will be truncated");
                        }

                        for (int i = 0; i < maxLen; i++)
                        {
                            var elem = ReadTypeFromStream(responseStream, elementtype);
                            (args[argi] as dynamic[])![i] = elem;
                        }
                    } 
                }
            }

            return returnValue;
        }
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
            return new List<byte[]>() {data};
        }

        return messages;
    }
}