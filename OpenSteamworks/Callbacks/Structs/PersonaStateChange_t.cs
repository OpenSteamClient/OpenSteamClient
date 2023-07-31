using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct PersonaStateChange_t
{
	public CSteamID steamid;
	public EPersonaChange changeFlags;
};