using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppStateInfo_s {
    //TODO: reverse
    // This enum contains a lot of useful stuff, like app ownership data, install data, current betas, oslists and possibly more, but we don't know it's layout (yet)
}