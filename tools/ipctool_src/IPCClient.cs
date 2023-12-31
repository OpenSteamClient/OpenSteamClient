using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

public class IPCClient {
    public enum IPCCommandCode : byte {
        Interface = 1,
        SerializeCallbacks = 2,
        ConnectToGlobalUser = 3,
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
    
    public struct ServerHello
    {
        public UInt32 processId;
        public UInt32 threadId;
        public UInt32 seed;
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
    public IPCClient(string ipaddress, IPCConnectionType connectionType) {
        this.connectionType = connectionType;
        Console.WriteLine("Connecting to " + ipaddress);
        socket = new(SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true
        };
        
        var ports = ipaddress.Split(":");
        string ip = ports[0];
        int port = int.Parse(ports[1]);
        socket.Connect(ip, port);
        Console.WriteLine("Connected to " + ipaddress);

        Console.WriteLine("Connect to pipe");
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream);
            // | Success | ProcID | ThreadID |
            writer.WriteUInt32(1);
            writer.WriteUInt32((uint)Environment.ProcessId);
            writer.WriteUInt32((uint)Environment.CurrentManagedThreadId);
            var resp2 = SendAndWaitForResponse(IPCCommandCode.ConnectPipe, stream.ToArray(), 12);
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
            var resp = SendAndWaitForResponse(IPCCommandCode.ConnectToGlobalUser, new byte[] {2}, 5);
            using (var response = new MemoryStream(resp)) {
                var reader = new EndianAwareBinaryReader(response);
                if (response.Length != 5) {
                    throw new Exception($"ConnectToGlobalUser unexpected result size {response.Length}");
                }

                // | LeadByte | HSteamUser |
                var leadByte = reader.ReadByte();
                if (leadByte != 3) {
                    throw new Exception($"ConnectToGlobalUser unexpected lead response byte {leadByte}");
                }

                HSteamUser = reader.ReadUInt32();
                if (HSteamUser+1 < 2) {
                    HSteamUser = 0;
                    throw new Exception($"ConnectToGlobalUser returned invalid HSteamUserEngine={HSteamUser+1}");
                }

                Console.WriteLine("Got HSteamUser " + HSteamUser);
            }
            
            Console.WriteLine();
        }

        // Console.WriteLine("CB");
        // pauseLoop = true;
        // using (var stream = new MemoryStream()) {
        //     var writer = new StreamWriter(stream);
        //     // writer.Write(HSteamPipe);
        //     // writer.Write(HSteamUser);
        //     // writer.Write((UInt32)Environment.ProcessId);
        //     // writer.Write((UInt32)Environment.CurrentManagedThreadId);
        //     var resp2 = SendAndWaitForResponse(IPCCommandCode.SerializeCallbacks, stream.ToArray());
        //     using (var response = new MemoryStream(resp2)) {
        //         var reader = new EndianAwareBinaryReader(response);
        //         var unk1 = reader.ReadUInt32();
        //         //var unk2 = reader.ReadUInt32();
        //         Console.WriteLine($"unk1: {unk1}");
        //     }
        // }
        
        // Console.WriteLine();
    }

    /// <summary>
    /// Call a service IPC Function. Does not need the fencepost, since it can be calculated.
    /// </summary>
    public TRet CallIPCFunctionClient<TRet>(byte interfaceid, uint functionid, uint fencepost, params object[] args) {
        return CallIPCFunctionInternal<TRet>(interfaceid, functionid, fencepost, args);
    }

    /// <summary>
    /// Call a service IPC Function. Does not need the fencepost, since it can be calculated.
    /// </summary>
    public TRet CallIPCFunctionService<TRet>(byte interfaceid, uint functionid, params object[] args) {
        return CallIPCFunctionInternal<TRet>(interfaceid, functionid, (uint)functionid + 254, args);
    }

    private TRet CallIPCFunctionInternal<TRet>(byte interfaceid, uint functionid, uint fencepost, object[] args) {
        using (var stream = new MemoryStream()) {
            var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8);

            stream.WriteByte(interfaceid);

            uint userToUse = 0;
            if (connectionType == IPCConnectionType.Client && !InterfaceMap.ClientInterfacesNoUser.Contains(interfaceid)) {
                userToUse = HSteamUser;
            }

            Console.WriteLine("USER: " + HSteamUser);
            writer.Write(HSteamUser);
            writer.Flush();

            Console.WriteLine("Function ID: " + functionid);
            writer.Write(functionid);
            writer.Flush();

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
            if (responseBytes[0] != 11) {
                throw new Exception("IPC call failed, expected byte 11 but got " + responseBytes[0]);
            }

            Console.WriteLine("Return:" + string.Join(" ", responseBytes[1..]));
            using (var responseStream = new MemoryStream(responseBytes, 1, responseBytes.Length-1)) {
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
        }
    }

    private unsafe byte[] WaitForAnyResponse() {
        Console.WriteLine("Awaiting any response");
        // waitingForResponse = true;
        // pauseLoop = false;
        // while (lastResponse == null)
        // {
            
        // }

        byte[] responseBytes = new byte[8192];
        var len = socket.Receive(responseBytes);
        var trimmed = responseBytes[0..len];
        byte[]? final = null;
        if (trimmed.Length >= 4) {
            using (var stream = new MemoryStream(trimmed))
            {
                var reader = new EndianAwareBinaryReader(stream);
                var readsize = reader.ReadUInt32();
                if (trimmed.Length-4 == readsize) {
                    final = trimmed[4..];
                }
            }
        }

        if (final == null) {
            final = trimmed;
        }
       
        Console.WriteLine("Got: " + string.Join(" ", final));

        // var copy = lastResponse!;
        // lastResponse = null;
        // waitingForResponse = false;
        return final;
    }

    private unsafe byte[] WaitForResponseOfSize(int responseLength) {
        Console.WriteLine($"Awaiting response of size {responseLength}");
        bool hadHeader = false;
        byte[] lastResponse;
        while (true)
        {
            lastResponse = WaitForAnyResponse();
            if (lastResponse != null && lastResponse.Length == responseLength) {
                break;
            }

            // This sucks. Sometimes the messages have their size prefixed and sometimes not. This is hacky, but it seems to be randomly determined what messages get the header...
            if (lastResponse != null && lastResponse.Length-4 == responseLength && lastResponse.Length > 4) {
                hadHeader = true;
                break;
            }

            if (lastResponse != null && lastResponse.Length != responseLength) {
                // Skip this response
                continue;
            }
        }

        Console.WriteLine("Got the response");

        byte[] copy;
        if (hadHeader) {
            copy = lastResponse![4..];
        } else {
            copy = lastResponse!;
        }

        // waitingForResponse = false;
        return copy;
    }

    // private void PauseLoop() {
    //     pauseLoop = true;
    //     while (loopDidRun) {

    //     }
    // }

    public unsafe byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data) {
        // PauseLoop();
        var serialized = Serialize(command, data);
        var sentLen = socket.Send(serialized);
        Console.WriteLine($"sent {sentLen} bytes, waiting for response");
        return WaitForAnyResponse();
    }

    public unsafe byte[] SendAndWaitForResponse(IPCCommandCode command, byte[] data, int responseLength) {
        // PauseLoop();
        var serialized = Serialize(command, data);
        var sentLen = socket.Send(serialized);
        Console.WriteLine($"sent {sentLen} bytes, waiting for response");
        return WaitForResponseOfSize(responseLength);
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

    // private void ReadLoop() {
    //     byte[] responseBytes = new byte[8192];
    //     while (true)
    //     {
    //         if (pauseLoop) {
    //             loopDidRun = false;
    //         }
            
    //         while (pauseLoop)
    //         {
                
    //         }

    //         loopDidRun = true;
    //         Console.WriteLine("Polling");
    //         int bytesReceived = socket.Receive(responseBytes);

    //         // Receiving 0 bytes means EOF has been reached
    //         if (bytesReceived == 0) break;

    //         Console.WriteLine("Got " + bytesReceived);
    //         Console.WriteLine(string.Join(" ", responseBytes[0..bytesReceived]));

    //         if (waitingForResponse) {
    //             lastResponse = responseBytes[0..bytesReceived];
    //             while (lastResponse != null)
    //             {
                    
    //             }
    //         }
    //     }
    // }
}