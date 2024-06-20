using System.Runtime.InteropServices;

namespace GameOverlayUI.IPC;

public unsafe struct DynInputData {
    public uint NumQueued;
    public byte DynamicStart;

    public static bool TryDequeueAll(DynInputData* ptr, out List<InputData> inputData)
    {
        inputData = new();
        if (ptr->NumQueued < 1) {
            return false;
        }

        Span<byte> span = new(&ptr->DynamicStart, (int)CalculateDataLength(ptr));
        for (int i = 0; i < ptr->NumQueued; i++)
        {
            var data = Marshal.PtrToStructure<InputData>((nint)(&ptr->DynamicStart + (i * Marshal.SizeOf<InputData>())));
            inputData.Add(data);
        }

        ptr->NumQueued = 0;
        return true;
    }

    public static void EnqueueAll(DynInputData* ptr, List<InputData> inputData)
    {
        long max = CalculateDataLength(ptr);
        long queued = CalculateDataLength((uint)inputData.Count);
        if (queued > max) {
            Console.WriteLine("Enqueuing greater number of input events than buffer can fit");
        }

        for (int i = 0; i < inputData.Count; i++)
        {
            nint bufPtr = (nint)(&ptr->DynamicStart) + (i * Marshal.SizeOf<InputData>());
            Marshal.StructureToPtr(inputData[i], bufPtr, false);
        }

        ptr->NumQueued = (uint)inputData.Count;
    }

    public static long CalculateDataLength(DynInputData* ptr) {
        return CalculateDataLength(ptr->NumQueued);
    }

    public static long CalculateDataLength(uint numInputs) {
        return numInputs * Marshal.SizeOf<InputData>();
    }
}