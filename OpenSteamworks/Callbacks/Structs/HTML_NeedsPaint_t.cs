using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct HTML_NeedsPaint_t
{
	public HHTMLBrowser unBrowserHandle;
	public IntPtr pBGRA;
	public UInt32 unWide;
	public UInt32 unTall;
	public UInt32 unUpdateX;
	public UInt32 unUpdateY;
	public UInt32 unUpdateWide;
	public UInt32 unUpdateTall;
	public UInt32 unScrollX;
	public UInt32 unScrollY;
	public float flPageScale;
	public UInt32 unPageSerial;
};
