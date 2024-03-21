using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.IPCClient.Interfaces;
using OpenSteamworks.Messaging;
using OpenSteamworks.Utils;

namespace OpenSteamworks.IPCClient;

public class FunctionSerializer : IDisposable {
    private readonly MemoryStream stream;
    private readonly EndianAwareBinaryWriter writer;
    private uint fencepost;
    private bool argsLocked = false;
    private readonly IPCBaseInterface iface;

    internal FunctionSerializer(IPCBaseInterface iface, byte interfaceid, uint functionid, uint fencepost) {
        this.iface = iface;
        this.fencepost = fencepost;
        stream = new MemoryStream();
        writer = new EndianAwareBinaryWriter(stream);

        stream.WriteByte(interfaceid);
        uint userToUse = 0;
        if (iface.client.ConnectionType == IPCClient.IPCConnectionType.Client && !InterfaceMap.ClientInterfacesNoUser.Contains(interfaceid)) {
            userToUse = iface.steamuser;
        }

        writer.Write(userToUse);
        writer.Flush();

        writer.Write(functionid);
        writer.Flush();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ThrowIfArgsLocked() {
        if (argsLocked) {
            throw new Exception("Arguments have been locked. Cannot add new arguments.");
        }
    }

    public void AddArg(int arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(uint arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }
    
    public void AddArg(long arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(ulong arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(bool arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(char arg) {
        ThrowIfArgsLocked();
        // This is terrible
        writer.Write(Encoding.UTF8.GetBytes(new string(new char[] { arg })));
    }

    public void AddArg(byte arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(sbyte arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(short arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(ushort arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(nint arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(nuint arg) {
        ThrowIfArgsLocked();
        writer.Write(arg);
    }

    public void AddArg(string arg) {
        ThrowIfArgsLocked();
        var strlen = arg.Length+1;
        if (strlen < byte.MaxValue+1) {
            writer.Write((byte)strlen);
        } else {
            writer.Write(byte.MaxValue);
        }
        writer.Write(Encoding.UTF8.GetBytes(arg + "\0"));
    }


    public unsafe void WriteArray<T>(T[] arr) where T: unmanaged {
        //TODO: this is sketchy. Find a way to do this with managed code that won't involve a 1000-case switch statement.
        fixed (T* arrPtr = arr) {
            int size = sizeof(T) * arr.Length;
            writer.Write(arr.Length);
            stream.Write(new ReadOnlySpan<byte>(arrPtr, size));
            writer.Flush();
        }
    }

    public unsafe void WriteStruct<T>(T structt) where T: unmanaged {
        int size = Marshal.SizeOf<T>();
        fixed (byte* ptr = new byte[size]) {
            Marshal.StructureToPtr(structt, (nint)ptr, false);
            stream.Write(new ReadOnlySpan<byte>(ptr, size));
        }
    }

    public unsafe void CopyCUtlBuffer(CUtlBuffer* bufPtr) {
        ThrowIfArgsLocked();

    }

    public unsafe void AddDataFromPointer(void* ptr, uint length) {
        writer.Write(length);
        writer.Flush();
        stream.Write(new ReadOnlySpan<byte>(ptr, (int)length));
    }

    public void FinalizeArgs() {
        ThrowIfArgsLocked();
        writer.WriteUInt32(fencepost);
    }

    public byte[] Serialize() {
        return stream.ToArray();
    }

    public FunctionDeserializer SendAndWaitForResponse() {
        var respBytes = this.iface.client.SendAndWaitForResponse(IPCClient.IPCCommandCode.Interface, this.Serialize());
        var deserializer = new FunctionDeserializer(respBytes);
        return deserializer;
    }

    public void Dispose()
    {
        ((IDisposable)writer).Dispose();
    }
}