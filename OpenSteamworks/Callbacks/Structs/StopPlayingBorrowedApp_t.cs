using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct StopPlayingBorrowedApp_t
{
    public UInt32 m_unAppID;
    public CSteamID m_OwnerAccountID;
    public Int32 m_nSecondLeft;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
    public string m_szOwnerName;
};