using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PersonaStateChange_t
{
	public CSteamID steamid;
	public EPersonaChange changeFlags;
};