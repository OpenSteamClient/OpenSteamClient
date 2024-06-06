
using GameOverlayUI.Platform;

namespace GameOverlayUI.IPC;

public unsafe struct DynInputData {
    /// <summary>
    /// Input mutex for reading and writing keyboard and mouse events.
    /// </summary>
    public OverlayMutex Mutex;

    public uint NumQueued;
    public byte DynamicStart;

    public static bool TryDequeueAll(DynInputData* ptr, out List<InputData> inputData)
    {
        inputData = new();
        if (ptr->NumQueued < 1) {
            return false;
        }

        var err = LinuxFutex.OverlayMutexLock(&ptr->Mutex, 5);
        if (err != 0) {
            throw new Exception("Getting lock failed, errno: " + err);
        }

        try
        {
            Span<byte> span = new(&ptr->DynamicStart, (int)(ptr->NumQueued * sizeof(InputData)));
            for (int i = 0; i < ptr->NumQueued; i++)
            {
                InputData* data = &((InputData*)&ptr->DynamicStart)[i];
                inputData.Add(*data);
                *data = new();
            }

            ptr->NumQueued = 0;
        }
        finally
        {
            LinuxFutex.OverlayMutexUnlock(&ptr->Mutex);
        }

        return true;
    }
}