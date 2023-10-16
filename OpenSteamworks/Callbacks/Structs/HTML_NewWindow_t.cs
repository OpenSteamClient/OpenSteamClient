using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct HTML_NewWindow_t
{
	public HHTMLBrowser unBrowserHandle;
	public string pchURL;
	public UInt32 unX;
	public UInt32 unY;
	public UInt32 unWide;
	public UInt32 unTall;
	public HHTMLBrowser unNewWindow_BrowserHandle_IGNORE;
};
