using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ShortcutChanged_t
{
	public AppId_t m_nAppID;
    public bool m_bRemote;
};