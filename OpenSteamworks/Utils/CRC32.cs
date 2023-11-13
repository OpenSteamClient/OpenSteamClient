using System;

namespace OpenSteamworks.Utils;

public class CRC32 {
    public const UInt32 DefaultPolynomial = 0xedb88320u;
    public const UInt32 DefaultSeed = 0xffffffffu;

    private uint hash = DefaultSeed;
    private static UInt32[] defaultTable = new UInt32[256];

    static CRC32() {
        // Initialize the default table
        for (var i = 0; i < 256; i++)
        {
            var entry = (UInt32)i;
            for (var j = 0; j < 8; j++)
                if ((entry & 1) == 1)
                    entry = (entry >> 1) ^ DefaultPolynomial;
                else
                    entry >>= 1;
            defaultTable[i] = entry;
        }
    }

    public CRC32() {

    }

    public void AddBytes(byte[] bytes) {
        for (var i = 0; i < bytes.Length; i++)
            hash = (hash >> 8) ^ defaultTable[bytes[i] ^ hash & 0xff];
    }

    public uint GetHash() {
        return hash;
    }
}