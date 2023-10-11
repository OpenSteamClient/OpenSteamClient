using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
/// <summary>
/// I have no idea what any of the fields in here mean. I've done a rough guess though, and according to ghidra the lengths of the fields seem to be correct.
/// </summary>
public struct PostLogonState_t 
{
	public byte unk1;         // 1 byte
	public byte maybeABoolUnk2;         // 1 byte
	public byte maybeABoolUnk3;         // 1 byte
	public byte unk4;         // 1 byte
	public byte unk5;         // 1 byte
	public byte unk6;         // 1 byte
    public bool connectedToCMs; // 1 byte
    public bool ifFalseDoSomething; // 1 byte
	public bool isLoading;         // 1 byte
	public byte unk7;
}; // 10 bytes