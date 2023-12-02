using System;

namespace OpenSteamworks.Utils;

/// <summary>
/// A simple byte array buffer which doubles in size when requested or until a given call's data fits.
/// </summary>
public class IncrementingBuffer : IncrementingBase<byte[]> {
    private byte[] data;
    public override byte[] Data { get => data; set => data = value; }
    public override int Length => data.Length;

    public IncrementingBuffer(int startingLength = 512) {
        data = new byte[startingLength];
    }

    public override byte[] Allocate(int size)
    {
        return new byte[size];
    }
}