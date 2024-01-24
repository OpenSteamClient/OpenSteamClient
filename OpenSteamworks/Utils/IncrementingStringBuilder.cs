using System;
using System.Text;

namespace OpenSteamworks.Utils;

/// <summary>
/// A simple StringBuilder which doubles in size when requested or until a given call's data fits.
/// </summary>
public class IncrementingStringBuilder : IncrementingBase<StringBuilder> {
    private StringBuilder data;
    public override StringBuilder Data { get => data; set => data = value; }
    public override int Length => data.Capacity;

    public IncrementingStringBuilder(int startingLength = 128) {
        data = new StringBuilder(startingLength);
    }

    public override StringBuilder Allocate(int size)
    {
        return new StringBuilder(size);
    }

    public override string ToString()
    {
        return data.ToString();
    }
}