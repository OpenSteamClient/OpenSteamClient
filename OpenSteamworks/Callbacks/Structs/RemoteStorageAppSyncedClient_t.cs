using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct RemoteStorageAppSyncedClient_t 
{
    public AppId_t appid;
    public EResult result;
    public int numDownloads;
} 