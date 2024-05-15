using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System;

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
    /// Not a real part of the protocol.
    /// | size | msg (includes command as first byte) |
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
    // private bool loopDidRun = false;
    // private bool pauseLoop = false;
    // private bool waitingForResponse = false;
    // private byte[]? lastResponse = null;
    public uint HSteamUser { get; private set; } = 0;
    public uint HSteamPipe { get; private set; } = 1;
    public enum IPCConnectionType {
        Client,
        Service
    }

    private readonly IPCConnectionType connectionType;
    private readonly Thread pollThread;

    public IPCClient(string ipaddress, IPCConnectionType connectionType) {
        this.connectionType = connectionType;
        Console.WriteLine("Connecting to " + ipaddress);
        socket = new(SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true
        };
        
        var ports = ipaddress.Split(":");
        string ip = ports[0];
        int port = int.Parse(ports[1], CultureInfo.InvariantCulture.NumberFormat);
        socket.Connect(ip, port);
        Console.WriteLine("Connected to " + ipaddress);

        pollThread = new(PollThread);
        pollThread.Start();

        Console.WriteLine("Connect to pipe");
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
                // | HostPID | HostThreadID | Seed |
                var hostpid = reader.ReadUInt32();
                var hosttid = reader.ReadUInt32();
                var seed = reader.ReadUInt32();
                Console.WriteLine($"Got host pid: {hostpid}, host tid: {hosttid}, seed: {seed}");
            }
        }

        if (connectionType == IPCConnectionType.Client) {
            Console.WriteLine("Connect to user");

            // | GlobalUserType |
            var resp = SendAndWaitForResponse(IPCCommandCode.ConnectToGlobalUser, new byte[] {2});
            using (var response = new MemoryStream(resp)) {
                var reader = new EndianAwareBinaryReader(response);

                HSteamUser = reader.ReadUInt32();
                if (HSteamUser+1 < 2) {
                    HSteamUser = 0;
                    throw new Exception($"ConnectToGlobalUser returned invalid HSteamUserEngine={HSteamUser+1}");
                }

                Console.WriteLine("Got HSteamUser " + HSteamUser);
            }
            
            Console.WriteLine();
        }
    }

    public TRet CallIPCFunction<TRet>(uint steamuser, byte interfaceid, uint functionid, uint fencepost, object[] args) {
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8);

            stream.WriteByte(interfaceid);

            uint userToUse = 0;
            if (connectionType == IPCConnectionType.Client && !InterfaceMap.ClientInterfacesNoUser.Contains(interfaceid)) {
                userToUse = steamuser;
            }

            Console.WriteLine("USER: " + steamuser);
            writer.Write(steamuser);
            writer.Flush();

            Console.WriteLine("Function ID: " + functionid);
            writer.Write(functionid);
            writer.Flush();

            List<int> changeableArgs = new();
            foreach (var arg in args)
            {
                var argtype = arg.GetType();
                if (argtype == typeof(int)) {
                    writer.Write((int)arg);
                } else if (argtype == typeof(uint)) {
                    writer.Write((uint)arg);
                } else if (argtype == typeof(long)) {
                    writer.Write((long)arg);
                } else if (argtype == typeof(ulong)) {
                    writer.Write((ulong)arg);
                } else if (argtype == typeof(bool)) {
                    writer.Write((bool)arg);
                } else if (argtype == typeof(char)) {
                    // This is terrible
                    writer.Write(Encoding.UTF8.GetBytes(new string(new char[] { (char)arg })));
                } else if (argtype == typeof(byte)) {
                    writer.Write((byte)arg);
                } else if (argtype == typeof(sbyte)) {
                    writer.Write((sbyte)arg);
                } else if (argtype == typeof(short)) {
                    writer.Write((short)arg);
                } else if (argtype == typeof(ushort)) {
                    writer.Write((ushort)arg);
                } else if (argtype == typeof(nint)) {
                    writer.Write((nint)arg);
                } else if (argtype == typeof(nuint)) {
                    writer.Write((nuint)arg);
                } else if (argtype == typeof(string)) {
                    var strlen = ((string)arg).Length+1;
                    if (strlen < byte.MaxValue+1) {
                        writer.Write((byte)strlen);
                    } else {
                        writer.Write(byte.MaxValue);
                    }
                    writer.Write(Encoding.UTF8.GetBytes((string)arg + "\0"));
                } else if (argtype == typeof(StringBuilder)) {
                    // No-op
                    changeableArgs.Add(Array.IndexOf(args, arg));
                } else {
                    throw new InvalidOperationException("Type " + argtype.Name + " is unsupported in IPC function calls");
                }

                writer.Flush();
            }

            // There seems to be another supported way to do fenceposts that relates to the end result of some math operation being 254. The original method relies on a randomly generated one instead (which would kind of suck to automate...)
            Console.WriteLine("Fencepost: " + fencepost);
            writer.Write(fencepost);
            writer.Flush();  

            var responseBytes = SendAndWaitForResponse(IPCCommandCode.Interface, stream.ToArray());
            Console.WriteLine("RESPONSE:" + string.Join(" ", responseBytes));

            TRet returnValue;
            using (var responseStream = new MemoryStream(responseBytes)) {
                returnValue = ReadFromResponse<TRet>(responseStream);

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

            return returnValue;
        }
    }

    private static string ReadStringOfLengthFromResponse(MemoryStream responseStream, int length) {
        var reader = new EndianAwareBinaryReader(responseStream, Encoding.UTF8);
        return Encoding.Default.GetString(reader.ReadBytes(length));
    }
    
    private static TRet ReadFromResponse<TRet>(MemoryStream responseStream) {
        var reader = new EndianAwareBinaryReader(responseStream, Encoding.UTF8);
        var rettype = typeof(TRet);
        if (rettype == typeof(int)) {
            return (TRet)(object)reader.ReadInt32();
        } else if (rettype == typeof(uint)) {
            return (TRet)(object)reader.ReadUInt32();
        } else if (rettype == typeof(long)) {
            return (TRet)(object)reader.ReadInt64();
        } else if (rettype == typeof(ulong)) {
            return (TRet)(object)reader.ReadUInt64();
        } else if (rettype == typeof(bool)) {
            return (TRet)(object)reader.ReadBoolean();
        } else if (rettype == typeof(char)) {
            // This is terrible
            return (TRet)(object)reader.ReadChar();
        } else if (rettype == typeof(byte)) {
            return (TRet)(object)reader.ReadByte();
        } else if (rettype == typeof(sbyte)) {
            return (TRet)(object)reader.ReadSByte();
        } else if (rettype == typeof(short)) {
            return (TRet)(object)reader.ReadInt16();
        } else if (rettype == typeof(ushort)) {
            return (TRet)(object)reader.ReadUInt16();
        } else if (rettype == typeof(nint)) {
            return (TRet)(object)(nint)reader.ReadInt64();
        } else if (rettype == typeof(nuint)) {
            return (TRet)(object)(nuint)reader.ReadUInt64();
        } else if (rettype == typeof(string)) {
            // Skip a byte
            reader.ReadByte();
            return (TRet)(object)reader.ReadNullTerminatedString();
        } else {
            throw new InvalidOperationException("Type " + rettype.Name + " is unsupported in IPC function returns");
        }
    }
    
    public unsafe void SendAndIgnoreResponse(IPCCommandCode command, byte[] data) {
        var serialized = Serialize(command, data);
        var sentLen = socket.Send(serialized);
        Console.WriteLine($"sent {sentLen} bytes");
    }

    public unsafe byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data) {
        pauseHandling = true;
        var serialized = Serialize(command, data);
        var sentLen = socket.Send(serialized);
        Console.WriteLine($"sent {sentLen} bytes, waiting for response");
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
    private Action? releaseGlobalUserHandler;
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
            case IPCCommandCode.ReleaseGlobalUser:
                releaseGlobalUserHandler = () =>
                {
                    tcs.TrySetResult(Array.Empty<byte>());
                    releaseGlobalUserHandler = null;
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
                
                Console.WriteLine("Got whole " + string.Join(" ", buffer[0..len]));  
                var msgs = SplitMessages(buffer[0..len]);
                Console.WriteLine("Data contains " + msgs.Count + " messages");
                foreach (var msg in msgs)
                {
                    Console.WriteLine("Msg: " + string.Join(" ", msg));  
                    HandleMessage(msg);
                }
            }
        }
    }

    private void HandleMessage(byte[] msg) {
        while (pauseHandling) { };
        if (msg.Length == 0) {
            return;
        }
        
        using (var stream = new MemoryStream(msg))
        {
            var reader = new EndianAwareBinaryReader(stream);
            switch (reader.ReadByte())
            {
                case 2:
                    Console.WriteLine("Got callback data!");
                    var user = reader.ReadUInt32();
                    var id = reader.ReadUInt32();
                    var len = reader.ReadUInt32();

                    if (user == 0 && id == 0 && len == 0) {
                        
                    } else {
                        Console.WriteLine("CB: user: " + user + ", id: " + id + ", len: " + len);
                    }
                    
                    serializeCallbacksResultHandler?.Invoke(msg[1..]);
                    break;
                case 4:
                    Console.WriteLine("Released global user");
                    releaseGlobalUserHandler?.Invoke();
                    break;
                case 11:
                    Console.WriteLine("Got function response data!");
                    commandResultHandler?.Invoke(msg[1..]);
                    break;
                case 3:
                    if (msg.Length == 5) {
                        Console.WriteLine("Connect to user data possibly received");

                        connectToGlobalUserResultHandler?.Invoke(msg[1..]);
                        break;
                    }
                    goto default;
                case 7: //Unrequested message, don't send this anywhere
                    if (msg.Length == 1) {
                        Console.WriteLine("Callbacks available in queue");
                        haveCallback = true;
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
                    Console.WriteLine("WARNING: Got unsupported message type " + msg[0]);
                    break;
            }
        }
    }

    public void Shutdown() {
        SendAndWaitForResponse(IPCCommandCode.ReleaseGlobalUser, new byte[] { 1, 0, 0, 0 });
        SendAndIgnoreResponse(IPCCommandCode.TerminatePipe, new byte[] { 1, 0, 0, 0 });

        pollShouldRun = false;
        this.socket.Disconnect(true);
    }

    private bool haveCallback = false;
    private void QueueCallbackSerialize() {
        // lock (pollLockObj)
        // {
        //     //var serialized = Serialize(IPCCommandCode.SerializeCallbacks, new byte[] { 1 });
        //     //var sentLen = socket.Send(serialized);
        // }
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
