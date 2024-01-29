using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CompatManagerToolRegistered_t
{
	public AppId_t toolAppID;
    //[MarshalAs(UnmanagedType.LPUTF8Str)]
    public IntPtr ptrToSomeStruct;
    public UInt32 unk1;
};