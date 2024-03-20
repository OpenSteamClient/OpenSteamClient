using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Extensions;
using OpenSteamworks.Messaging;
using OpenSteamworks.Utils;

namespace OpenSteamworks.IPCClient;

public class FunctionDeserializer : IDisposable {
    private readonly MemoryStream stream;
    private readonly EndianAwareBinaryReader reader;

    internal FunctionDeserializer(byte[] data) {
        stream = new MemoryStream(data);
        reader = new EndianAwareBinaryReader(stream);
    }

    public int ReadInt() {
        return reader.ReadInt32();
    }

    public uint ReadUInt() {
        return reader.ReadUInt32();
    }

    public long ReadLong() {
        return reader.ReadInt64();
    }

    public ulong ReadULong() {
        return reader.ReadUInt64();
    }

    public bool ReadBoolean() {
        return reader.ReadBoolean();
    }

    public char ReadChar() {
        return (char)reader.ReadByte();
    }

    public byte ReadByte() {
        return reader.ReadByte();
    }

    public sbyte ReadSByte() {
        return reader.ReadSByte();
    }

    public short ReadShort() {
        return reader.ReadInt16();
    }

    public ushort ReadUShort() {
        return reader.ReadUInt16();
    }

    public nint ReadNInt() {
        checked
        {
            return (nint)reader.ReadInt64();
        }
    }

    public nuint ReadNUInt() {
        checked
        {
            return (nuint)reader.ReadUInt64();
        }
    }

    public string ReadString() {
        // Skip a byte
        reader.ReadByte();
        return reader.ReadNullTerminatedUTF8String();
    }

    public void ReadStringBuilder(StringBuilder arg) {
        int maxLen = arg.Capacity;
        arg.Clear();
        arg.Append(Encoding.Default.GetString(reader.ReadBytes(maxLen)));
    }

    public unsafe void ReadArray<T>(T[] arr) where T: unmanaged {
        //TODO: this is sketchy. Find a way to do this with managed code that won't involve a 1000-case switch statement.
        fixed (T* arrPtr = arr) {
            int size = sizeof(T) * arr.Length;
            Marshal.Copy(reader.ReadBytes(size), 0, (nint)arrPtr, size);
        }
    }

    public unsafe void ReadCUtlBuffer(CUtlBuffer* ptrToTarget) {
        checked
        {
            int length = (int)ReadUInt();
            fixed (byte* bytes = reader.ReadBytes(length)) {
                ptrToTarget->Put(bytes, length);
            }
        }
    }

    public unsafe void ReadProtobufHack<TMsg>(IntPtr nativeptr) where TMsg: Google.Protobuf.IMessage<TMsg>, new() {
        TMsg proto = ReadProtobuf<TMsg>();
        if (!ProtobufHack.CopyToNative<TMsg>(proto, nativeptr)) {
            throw new Exception("Copying protobuf to native pointer failed");
        }
    }

    public TMsg ReadProtobuf<TMsg>() where TMsg: Google.Protobuf.IMessage<TMsg>, new() {
        checked
        {
            var parser = new Google.Protobuf.MessageParser<TMsg>(() => new TMsg());
            int length = (int)ReadUInt();
            return parser.ParseFrom(reader.ReadBytes(length));
        }
    }

    public void Dispose()
    {
        ((IDisposable)reader).Dispose();
    }
}