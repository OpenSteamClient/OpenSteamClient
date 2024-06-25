using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using SkiaSharp;

namespace GameOverlayUI.IPC;

public unsafe class SharedMemoryManager : IDisposable {
    public class SharedMemorySegment<TData> : IDisposable where TData : unmanaged
    {
        public string IPCFilePath => $"/dev/shm{IPCName}";
        public string IPCName { get; }
        public long Length { get; private set; }
        public TData* Data { get; private set; }
        public int FD { get; private set; }

        public SharedMemorySegment(string ipcName, long initialSize) {
            IPCName = ipcName;
            Resize(initialSize, true);
        }

        public void Resize(long newLength)
            => Resize(newLength, false);

        private void Resize(long newLength, bool isInit) {
            if (newLength == Length && !isInit) {
                #if LOGSPAM
                Console.WriteLine("No need to resize");
                #endif
                
                return;
            }

            if (Data != null) {
                munmap(Data, (uint)Length);
            }

            FD = shm_open(IPCName, 66, 511);
            Console.WriteLine("open result: " + FD);

            Console.WriteLine("ftruncate: " + ftruncate(FD, (uint)newLength));
            Length = new FileInfo(IPCFilePath).Length;
            Console.WriteLine("New Size: " + Length);

            Data = (TData*)mmap((void*)0x0, (uint)newLength, 3, 1, FD, 0);
            Console.WriteLine("mmap result: " + (nint)Data);

            var closeResult = close(FD);
            Console.WriteLine("close result: " + closeResult);

            Length = newLength;
        }

        public void Dispose()
        {
            if (Data == null) {
                throw new ObjectDisposedException(nameof(Data));
            }

            ReleaseSharedMemory();
            Data = null;
            Length = 0;
        }

        private void ReleaseSharedMemory() {
            var result = munmap(Data, (uint)Length);
            Console.WriteLine("munmap result: " + result);

            if (!isClient) {
                result = shm_unlink(IPCName);
                Console.WriteLine("unlink result: " + result);
            }
        }
    }

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

    [DllImport("c")]
    private static extern int close(int fd);

    public SharedMemorySegment<DynInputData> InputData { get; private set; }
    public SharedMemorySegment<DynDisplayData> DisplayData { get; private set; }
    public SharedMemorySegment<OverlayControlData> ControlData { get; private set; }

    
    public uint GamePID { get; init; }
    public string IPCNameInput => $"/u{getuid()}_OpenSteamClientIPC-GameOverlay_{GamePID}_Input";
    public string IPCNameDisplay => $"/u{getuid()}_OpenSteamClientIPC-GameOverlay_{GamePID}_Display";
    public string IPCNameControl => $"/u{getuid()}_OpenSteamClientIPC-GameOverlay_{GamePID}_Control";

    private static bool isClient = false;
    public SharedMemoryManager(uint gamePID, bool client = false) {
        isClient = client;

        if (!OperatingSystem.IsLinux()) {
            throw new NotSupportedException();
        }

        this.GamePID = gamePID;

        InputData = new(IPCNameInput, 1024);
        DisplayData = new(IPCNameDisplay, 1920 * 1080 * 4);
        ControlData = new(IPCNameControl, sizeof(OverlayControlData));

        if (!client) {
            // The server setups the initial screen allocation
            DisplayData.Data->Width = 1920;
            DisplayData.Data->Height = 1080;
            DisplayData.Resize(DynDisplayData.CalculateDataLength(DisplayData.Data) + sizeof(DynDisplayData));
        }
    }

    public List<InputData> GetInputData() {
        if (DynInputData.TryDequeueAll(InputData.Data, out List<InputData> inputData)) {
            Console.WriteLine("Got input data, count: " + inputData.Count);
        }

        return inputData;
    }

    public void SetPixels(Bitmap frame) {
        if (frame.Format != PixelFormat.Bgra8888) {
            throw new Exception("Invalid format for image");
        }
        
        DynDisplayData.SetPixels(DisplayData.Data, frame);
    }

    public void Dispose()
    {
        InputData.Dispose();
        DisplayData.Dispose();
        ControlData.Dispose();
    }
}