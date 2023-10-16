using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct HTML_LinkAtPosition_t
{
	public HHTMLBrowser unBrowserHandle;
	public UInt32 x;
	public UInt32 y;
	public string pchURL;
	public bool bInput;
	public bool bLiveLink;
};
