using System.Diagnostics;

namespace GameOverlayUI.IPC;

public unsafe struct DynDisplayData {
    public uint Width;
    public uint Height;

    /// <summary>
    /// Start of dynamic data
    /// </summary>
    public byte DynamicData;

    public static long CalculateDataLength(DynDisplayData* ptr) {
        return ptr->Width * ptr->Height * 4;
    }

    public static void SetPixels(DynDisplayData* ptr, byte[] data)
    {
        //TODO: This code needs additional sanity checks (such as resolution check)
        //TODO: Resize support (currently game window resizing won't work, need to re-mmap)

        #if LOGSPAM
            Console.WriteLine("c: " + CalculateDataLength(ptr) + ", a: " + data.Length);
            Console.WriteLine("W: " + ptr->Width + ", H: " + ptr->Height);
        #endif
        
        Trace.Assert(CalculateDataLength(ptr) == data.Length);
        Span<byte> target = new(&ptr->DynamicData, (int)CalculateDataLength(ptr));
        data.CopyTo(target);
    }

    public DynDisplayData()
    {
    }
}