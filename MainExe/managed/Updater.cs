using System.Runtime.InteropServices;

namespace managed;

public unsafe sealed class Updater : IDisposable {
    public NullTerminatedUnsafeString CurrentBeta = new();

    public Updater() {
        CurrentBeta.CurrentString = "opensteam_release";
    }

    public void Dispose()
    {
        CurrentBeta.Dispose();
    }
}