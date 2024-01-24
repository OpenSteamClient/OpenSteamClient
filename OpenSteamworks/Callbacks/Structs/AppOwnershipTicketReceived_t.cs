using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct AppOwnershipTicketReceived_t 
{
	public AppId_t m_nAppID;
}; 