using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct ExecuteSteamURL_t {
    public string m_pchSteamURL;
}