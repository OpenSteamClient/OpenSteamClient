using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct CallbackMsg_t {
    public HSteamUser m_hSteamUser;
	public int m_iCallback;
	public void* m_pubParam;
	public int m_cubParam;
}