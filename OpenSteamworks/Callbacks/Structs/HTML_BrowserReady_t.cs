using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct HTML_BrowserReady_t
{
    public const int CallbackID = 4501;
    public HHTMLBrowser unBrowserHandle;
};