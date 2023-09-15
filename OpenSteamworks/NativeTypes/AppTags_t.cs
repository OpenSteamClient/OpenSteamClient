using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct AppTags_t {
    public CUtlString name;
}