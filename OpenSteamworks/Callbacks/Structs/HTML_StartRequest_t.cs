using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct HTML_StartRequest_t
{
	public HHTMLBrowser unBrowserHandle;
	public string pchURL;
	public string pchTarget;
	public string pchPostData;
	public bool bIsRedirect;
};
