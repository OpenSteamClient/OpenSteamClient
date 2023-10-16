using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct HTML_VerticalScroll_t
{
	public HHTMLBrowser unBrowserHandle;
	public UInt32 unScrollMax;
	public UInt32 unScrollCurrent;
	public float flPageScale;
	public bool bVisible;
	public UInt32 unPageSize;
};
