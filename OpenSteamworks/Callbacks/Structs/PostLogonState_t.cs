using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct PostLogonState_t 
{
	public UInt16 unk1;         // 2 bytes
	public UInt16 unk2;         // 2 bytes
	public UInt16 unk3;         // 2 bytes
	public bool logonComplete;         // 2 bytes
	public UInt16 unk5;		   // 2 bytes
};