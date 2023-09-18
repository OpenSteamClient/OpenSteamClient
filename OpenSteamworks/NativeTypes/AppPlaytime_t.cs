using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppPlaytime_t {
    public UInt32 allTime = 0;
    public UInt32 lastTwoWeeks = 0;
    public AppPlaytime_t() {

    }
    public override string ToString()
    {
        return "allTime: " + allTime +  ", lastTwoWeeks: " + lastTwoWeeks;
    }
}