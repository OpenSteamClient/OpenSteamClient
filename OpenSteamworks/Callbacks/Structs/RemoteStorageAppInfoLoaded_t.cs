using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct RemoteStorageAppInfoLoaded_t 
{
	public AppId_t m_nAppID;
    public EResult m_eResult;
}; 