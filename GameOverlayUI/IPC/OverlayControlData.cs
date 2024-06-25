namespace GameOverlayUI.IPC;

public struct OverlayControlData {
    public EOverlayState State = EOverlayState.NotRunning;
    public uint MemoryResized = 0;


    public static unsafe void WaitForLoopStart(OverlayControlData* data) {
        while (data->State != EOverlayState.ClientRequestLoopStart)
        {
            // This is terible, but it will have to do unless we start using linux mutexes and that's complex (and even more not-cross platform).
            Thread.Sleep(1);
        }
    }

    public static unsafe void RequestInputData(OverlayControlData* data, out uint newAllocation) {
        data->State = EOverlayState.ServerRequestInputData;
        while (data->State != EOverlayState.ResponseAvailable)
        {
            //Thread.Sleep(1);
        }

        newAllocation = data->MemoryResized;
        data->MemoryResized = 0;
    }

    public static unsafe void RequestDisplayAllocation(OverlayControlData* data, out uint newAllocation) {
        data->State = EOverlayState.ServerRequestDisplayAllocation;
        while (data->State != EOverlayState.ResponseAvailable)
        {
            //Thread.Sleep(1);
        }

        newAllocation = data->MemoryResized;
        data->MemoryResized = 0;
    }

    public OverlayControlData() { }
}