using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct CompatManagerToolRegistered_t
{
	public UInt32 unk1;
    public AppId_t unk2;
};