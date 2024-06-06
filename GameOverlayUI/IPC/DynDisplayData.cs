
using GameOverlayUI.Platform;

namespace GameOverlayUI.IPC;

public unsafe struct DynDisplayData {
    /// <summary>
    /// Display mutex for reading and writing display pixel data.
    /// </summary>
    public OverlayMutex Mutex;
    public uint Width;
    public uint Height;

    /// <summary>
    /// Start of dynamic data
    /// </summary>
    public byte DynamicData;

    public long CalculateDataLength() {
        return this.Width * this.Height * 4;
    }

    public static void SetPixels(DynDisplayData* ptr, byte[] data)
    {
        // Always lock here so the overlay client doesn't render stale frames

        var err = LinuxFutex.OverlayMutexLock(&ptr->Mutex, 0);
        if (err != 0) {
            throw new Exception("Getting lock failed, errno: " + err);
        }

        try
        {
            //TODO: This code needs additional sanity checks (such as resolution check)
            //TODO: Resize support (currently game window resizing won't work, need to re-mmap)
            Span<byte> target = new(&ptr->DynamicData, (int)ptr->CalculateDataLength());
            data.CopyTo(target);
        }
        finally
        {
            LinuxFutex.OverlayMutexUnlock(&ptr->Mutex);
        }
    }

    public DynDisplayData()
    {
    }
}