using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct SharedConnectionMessageReady_t
{
	public UInt32 m_hResult;
};