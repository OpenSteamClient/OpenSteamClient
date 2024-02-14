using System.Runtime.InteropServices;

namespace managed;

public unsafe sealed class UnsafeByteArray : IDisposable {
    public bool HasAllocation { get; private set; } = false;
    private int currentLength = 0;

    /// <summary>
    /// Gets the pointer to the current data. This pointer will never point to a non-existent block of memory. It can be null if the memory hasn't been set.
    /// </summary>
    public byte* CurrentPtr { get; private set; } = null;
    public byte[] GetCurrentBytes() {
        if (CurrentPtr == null) {
            throw new InvalidOperationException("Cannot get current bytes without an allocation");
        }

        return new ReadOnlySpan<byte>(CurrentPtr, currentLength).ToArray();
    }

    public void SetCurrentBytes(byte[] newBytes) {
        PrepareBuffer(newBytes.Length);
        Marshal.Copy(newBytes, 0, (nint)CurrentPtr, currentLength);
    }

    private void PrepareBuffer(int newLength) {
        if (!HasAllocation) {
            HasAllocation = true;
            CurrentPtr = (byte*)NativeMemory.AllocZeroed((nuint)newLength);
            return;
        }

        if (newLength == currentLength) {
            ZeroMemory();
            return;
        }

        // Else, do a simple realloc
        currentLength = newLength;
        CurrentPtr = (byte*)NativeMemory.Realloc(CurrentPtr, (nuint)newLength);
        ZeroMemory();
    }

    private void ZeroMemory() {
        // This might be slow.
        for (int i = 0; i < currentLength; i++)
        {
            CurrentPtr[i] = 0;
        }
    }

    public void Dispose()
    {
        if (HasAllocation) {
            NativeMemory.Free(CurrentPtr);
            CurrentPtr = null;
            currentLength = 0;
            HasAllocation = false;
        }
    }
}