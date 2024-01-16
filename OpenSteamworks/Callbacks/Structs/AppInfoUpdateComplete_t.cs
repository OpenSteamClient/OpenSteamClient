using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct AppInfoUpdateComplete_t
{
    public const int CallbackID = 1280003;
};