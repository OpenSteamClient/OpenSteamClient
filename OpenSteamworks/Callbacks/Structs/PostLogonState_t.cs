using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
/// <summary>
/// I have no idea what any of the fields in here mean. I've done a rough guess though, and according to ghidra the lengths of the fields seem to be correct.
/// </summary>
public struct PostLogonState_t 
{
	static PostLogonState_t() {
		unsafe
		{
			UtilityFunctions.Assert(sizeof(PostLogonState_t) == 10);
		}
    }

	public byte unk1;         // 1 byte
	public byte unk2;         // 1 byte
	public byte unk3;         // 1 byte
	public byte unk4;         // 1 byte
	public byte unk5;         // 1 byte
	public byte unk6;         // 1 byte
    public byte connectedToCMs; // 1 byte
    public byte unk8; // 1 byte
	public byte hasAppInfo;         // 1 byte
	public byte unk10;
}; // 10 bytes