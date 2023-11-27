using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ShortcutRemoved_t
{
	public AppId_t m_nAppID;
    public bool m_bRemote;
};