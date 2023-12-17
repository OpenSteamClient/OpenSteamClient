using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct AppLifetimeNotice_t
{
    // Hmm, this causes problems for 64-bit pids...
	public UInt32 pidOfGame;

    // What do these mean?
    public UInt16 unk3;
    public UInt16 unk4;

    // These are basically always 0, some legacy thing?
    public UInt16 unk5;
    public UInt16 unk6;
    
	public bool m_bExiting;
};