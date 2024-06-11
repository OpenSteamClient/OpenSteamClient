using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using GameOverlayUI.Platform;
using SkiaSharp;

namespace GameOverlayUI.IPC;

public unsafe class SharedMemory : IDisposable {
    
    [DllImport("c")]
    private static extern int getuid();

    [DllImport("c")]
    private static extern void* mmap(void* addr, uint length, int prot, int flags, int fd, uint offset);

    [DllImport("c")]
    private static extern int munmap(void* addr, uint length);

    [DllImport("c", SetLastError = true)]
    private static extern int ftruncate(int fd, uint length);

    [DllImport("c")]
    private static extern int shm_open([MarshalAs(UnmanagedType.LPUTF8Str)] string name, int oflag, int mode);

    [DllImport("c")]
    private static extern int shm_unlink([MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    private readonly long inputDataLength;
    public DynInputData* InputData { get; private set; }

    private readonly long displayDataLength;
    public DynDisplayData* DisplayData { get; private set; }

    
    public uint GamePID { get; init; }
    public string IPCNameInput => $"/u{getuid()}_OpenSteamClientIPC-GameOverlay_{GamePID}_Input";
    public string IPCNameDisplay => $"/u{getuid()}_OpenSteamClientIPC-GameOverlay_{GamePID}_Display";

    public SharedMemory(uint gamePID) {
        if (!OperatingSystem.IsLinux()) {
            throw new NotSupportedException();
        }

        this.GamePID = gamePID;

        InputData = (DynInputData*)OpenSharedMemory(IPCNameInput, 1024, out inputDataLength);
        DisplayData = (DynDisplayData*)OpenSharedMemory(IPCNameDisplay, 0x100000, out displayDataLength);
        //TODO: Remove this
        DisplayData->Width = 1920;
        DisplayData->Height = 1080;
    }

    private static void* OpenSharedMemory(string ipcName, uint initialSize, out long length) {
        string ipcFilepath = $"/dev/shm{ipcName}";
        var result = shm_open(ipcName, 66, 511);
        Console.WriteLine("open result: " + result);
        length = new FileInfo(ipcFilepath).Length;
        Console.WriteLine("Bytes Allocated: " + length);
        if (length < initialSize) {
            Console.WriteLine("ftruncate: " + ftruncate(result, initialSize));
            length = new FileInfo(ipcFilepath).Length;
            Console.WriteLine("New Size: " + length);
        }

        void* ptr = mmap((void*)0x0, initialSize, 3, 1, result, 0);
        Console.WriteLine("mmap result: " + (nint)ptr);

        return ptr;
    }

    public void RunInputFrame() {
        if (DynInputData.TryDequeueAll(InputData, out List<InputData> inputData)) {
            Console.WriteLine("Got input data, count: " + inputData.Count);
        }
    }

    public void SetPixels(WriteableBitmap frame) {
        if (frame.Format != PixelFormat.Bgra8888) {
            throw new Exception("Invalid format for image");
        }

        const int BPP = 4;

        byte[] data = new byte[frame.PixelSize.Width * frame.PixelSize.Height * BPP];
        fixed (byte* ptr = data) {
            frame.CopyPixels(new Avalonia.PixelRect(0, 0, frame.PixelSize.Width, frame.PixelSize.Height), (nint)ptr, data.Length, frame.PixelSize.Width * BPP);
        }
        
        DynDisplayData.SetPixels(DisplayData, data);
    }

    public void Dispose()
    {
        if (InputData == null) {
            throw new ObjectDisposedException(nameof(SharedMemory));
        }


        ReleaseSharedMemory(IPCNameDisplay, DisplayData, (uint)displayDataLength);
        DisplayData = null;

        ReleaseSharedMemory(IPCNameInput, InputData, (uint)inputDataLength);
        InputData = null;
    }

    private static void ReleaseSharedMemory(string ipcName, void* ptr, uint length) {
        var result = munmap(ptr, length);
        Console.WriteLine("munmap result: " + result);

        result = shm_unlink(ipcName);
        Console.WriteLine("unlink result: " + result);
    }
}