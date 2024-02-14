using System.Runtime.InteropServices;
using System.Text;

namespace managed;

public unsafe sealed class NullTerminatedUnsafeString : IDisposable {
    private readonly UnsafeByteArray underlyingBytes = new();
    public string CurrentString {
        get {
            if (!underlyingBytes.HasAllocation) {
                return string.Empty;
            }

            return Marshal.PtrToStringUTF8((nint)underlyingBytes.CurrentPtr)!;
        }

        set {
            underlyingBytes.SetCurrentBytes(Encoding.UTF8.GetBytes(value + "\0"));
        }
    }

    public byte* CurrentPtr => underlyingBytes.CurrentPtr;
    public bool HasAllocation => underlyingBytes.HasAllocation;

    public NullTerminatedUnsafeString() { }

    public NullTerminatedUnsafeString(string str) {
        this.CurrentString = str;
    }

    public void Dispose()
    {
        underlyingBytes.Dispose();
    }
}