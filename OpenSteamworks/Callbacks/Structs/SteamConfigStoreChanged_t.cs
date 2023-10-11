using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct SteamConfigStoreChanged_t
{
	public EConfigStore ConfigStore;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string PathToChange;
};