using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppPlaytime_t {
    public UInt32 AllTime = 0;
    public UInt32 LastTwoWeeks = 0;

    public AppPlaytime_t(UInt32 allTime, UInt32 lastTwoWeeks) {
        this.AllTime = allTime;
        this.LastTwoWeeks = lastTwoWeeks;
    }

    public override string ToString()
    {
        return "AllTime: " + AllTime +  ", LastTwoWeeks: " + LastTwoWeeks;
    }
}