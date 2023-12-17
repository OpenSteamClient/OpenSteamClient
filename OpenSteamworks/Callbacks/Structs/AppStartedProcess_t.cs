using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct AppStartedProcess_t
{
	public AppId_t m_nAppID;
    public UInt32 processID;
    public UInt32 unk;
};