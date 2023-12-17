using System;

namespace OpenSteamworks.Utils;

/// <summary>
/// A simple uint array which doubles in size when requested or until a given call's data fits.
/// </summary>
public class IncrementingUIntArray : IncrementingBase<uint[]> {
    private uint[] data;
    public override uint[] Data { get => data; set => data = value; }
    public override int Length => data.Length;

    public IncrementingUIntArray(int startingLength = 128) {
        data = new uint[startingLength];
    }

    public override uint[] Allocate(int size)
    {
        return new uint[size];
    }
}