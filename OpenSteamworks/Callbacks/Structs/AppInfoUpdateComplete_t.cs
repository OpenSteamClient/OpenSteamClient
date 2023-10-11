using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
/// <summary>
/// The fields of this callback are unknown and unused by steamui.so, so we cannot get type info through Ghidra. Instead let's just define CallbackID here, and not define this in the global callback list to avoid issues.
/// </summary>
public unsafe struct AppInfoUpdateComplete_t
{
    public const int CallbackID = 1280003;
};