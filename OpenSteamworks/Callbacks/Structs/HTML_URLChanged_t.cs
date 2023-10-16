using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct HTML_URLChanged_t
{
	public HHTMLBrowser unBrowserHandle;
	public string pchURL;
	public string pchPostData;
	public bool bIsRedirect;
	public string pchPageTitle;
	public bool bNewNavigation;
};
