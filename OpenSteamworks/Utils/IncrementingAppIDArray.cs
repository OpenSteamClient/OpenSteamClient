using System;

namespace OpenSteamworks.Utils;

/// <summary>
/// A simple AppId_t array which doubles in size when requested or until a given call's data fits.
/// </summary>
public class IncrementingAppIDArray : IncrementingBase<AppId_t[]> {
    private AppId_t[] data;
    public override AppId_t[] Data { get => data; set => data = value; }
    public override int Length => data.Length;

    public IncrementingAppIDArray(int startingLength = 128) {
        data = new AppId_t[startingLength];
    }

    public override AppId_t[] Allocate(int size)
    {
        return new AppId_t[size];
    }
}