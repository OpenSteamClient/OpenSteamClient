using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct DeauthorizeDeviceResult_t
{
    public EResult m_eResult;
    public CSteamID m_OwnerAccountID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	public string m_szErrorDetail;
};
