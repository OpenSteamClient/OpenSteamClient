using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct WebAuthRequestCallback_t
{
	public bool m_bSuccessful;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
	public string m_rgchToken;
};