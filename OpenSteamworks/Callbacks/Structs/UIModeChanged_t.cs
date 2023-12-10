using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct UIModeChanged_t
{
    public EUIMode m_uiModeOld;
    public EUIMode m_uiModeNew;
};