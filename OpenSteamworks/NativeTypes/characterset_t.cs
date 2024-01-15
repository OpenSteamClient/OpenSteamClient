using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct characterset_t
{
	public fixed byte set[256];
	public bool InCharacterset(byte c) {
		fixed (byte* setp = set) {
            for (int i = 0; i < 256; i++)
			{
				if (setp[i] == c) {
                    return true;
                }
			}
        }

        return false;
    }
	public unsafe static characterset_t* AllocateCharset(byte[] chars) {
        characterset_t set = new();
        for (int i = 0; i < chars.Length; i++)
		{
            set.set[i] = chars[i];
        }
        var handle = GCHandle.Alloc(set, GCHandleType.Pinned);
        return (characterset_t*)handle.AddrOfPinnedObject();
    }
};