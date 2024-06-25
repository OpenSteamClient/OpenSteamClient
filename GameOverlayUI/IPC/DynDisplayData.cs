using System.Diagnostics;
using Avalonia.Media.Imaging;

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

    public static void SetPixels(DynDisplayData* ptr, Bitmap frame)
    {
        //TODO: This code needs additional sanity checks (such as resolution check)
        //TODO: Resize support (currently game window resizing won't work, need to re-mmap)

        #if LOGSPAM
            Console.WriteLine("c: " + CalculateDataLength(ptr) + ", a: " + data.Length);
            Console.WriteLine("W: " + ptr->Width + ", H: " + ptr->Height);
        #endif
        
        const int BPP = 4;
        frame.CopyPixels(new Avalonia.PixelRect(0, 0, frame.PixelSize.Width, frame.PixelSize.Height), (nint)(&ptr->DynamicData), (int)CalculateDataLength(ptr), frame.PixelSize.Width * BPP);
    }

    public DynDisplayData()
    {
    }
}