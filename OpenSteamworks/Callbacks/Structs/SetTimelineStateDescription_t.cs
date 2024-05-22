using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct SetTimelineStateDescription_t 
{
    public CGameID gameid;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string description;
    public float deltaTime;
} 