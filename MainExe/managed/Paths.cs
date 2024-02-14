using System.Runtime.InteropServices;

namespace managed;

public unsafe sealed class Paths : IDisposable {
    public NullTerminatedUnsafeString LoggingPath = new();

    public Paths() {
        LoggingPath.CurrentString = "opensteam_release";
    }

    public void Dispose()
    {
        LoggingPath.Dispose();
    }
}