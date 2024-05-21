using System;
using System.Runtime.InteropServices;
using OpenSteamworks;



[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlMemory<T> where T : unmanaged {
    public UInt32 m_unSizeOfElements = 0;
	public void* m_pMemory = null;
	public int m_nAllocationCount = 0;
	public int m_nGrowSize = 0;

    public CUtlMemory(int growSize = 0, int nInitSize = 0) {
        this.m_nAllocationCount = nInitSize;
        this.m_unSizeOfElements = (uint)sizeof(T);
        nuint size = (nuint)(this.m_unSizeOfElements * this.m_nAllocationCount);
        Logging.CUtlLogger.Debug("Allocating CUtlMemory of size " + size);
        this.m_pMemory = NativeMemory.AllocZeroed(size);
        this.m_nGrowSize = growSize;
    }

    public CUtlMemory(IntPtr pBuffer, int nSize) {
        this.m_unSizeOfElements = (uint)sizeof(T);
        this.m_pMemory = (void*)pBuffer;
        this.m_nAllocationCount = nSize;
    }

    public void Free() {
        Logging.CUtlLogger.Debug("Freeing CUtlMemory");
        NativeMemory.Free(this.m_pMemory);
    }

    public T* Base() {
        return (T*)m_pMemory;
    }

    public byte[] ToManagedAndFree() {
        var str = this.ToManaged();
        this.Free();
        return str;
    }

    private int UtlMemory_CalcNewAllocationCount( int nAllocationCount, int nGrowSize, int nNewSize, int nBytesItem )
    {
        if ( nGrowSize != 0 )
        { 
            nAllocationCount = ((1 + ((nNewSize - 1) / nGrowSize)) * nGrowSize);
        }
        else 
        {
            if ( nAllocationCount == 0 )
            {
                if ( nBytesItem > 0 )
                {
                    // Compute an allocation which is at least as big as a cache line...
                    nAllocationCount = (31 + nBytesItem) / nBytesItem;
                }
                else
                {
                    // Should be impossible, but if hit try to grow an amount that may be large
                    // enough for most cases and thus avoid both divide by zero above as well as
                    // likely memory corruption afterwards.
                    Logging.CUtlLogger.Debug("nBytesItem is " + nBytesItem + "in UtlMemory_CalcNewAllocationCount");
                    nAllocationCount = 256;
                }
            }

            // Cap growth to avoid high-end doubling insanity (1 GB -> 2 GB -> overflow)
            int nMaxGrowStep = int.Max( 1, 256*1024*1024 / ( nBytesItem > 0 ? nBytesItem : 1 ) );
            while (nAllocationCount < nNewSize)
            {
                // Grow by doubling, but at most 256 MB at a time.
                nAllocationCount += int.Min( nAllocationCount, nMaxGrowStep );
            }
        }

        return nAllocationCount;
    }

    public void Grow(int num) {
        // Make sure we have at least numallocated + num allocations.
        // Use the grow rules specified for this memory (in m_nGrowSize)
        int nAllocationRequested = m_nAllocationCount + num;

        var was = m_nAllocationCount * m_unSizeOfElements;
        this.m_nAllocationCount = UtlMemory_CalcNewAllocationCount(m_nAllocationCount, m_nGrowSize, nAllocationRequested, (int)m_unSizeOfElements);
        Logging.CUtlLogger.Info("Growing memory to " + m_nAllocationCount * m_unSizeOfElements + ", was: " + was);
        this.m_pMemory = NativeMemory.Realloc(this.m_pMemory, (nuint)(m_nAllocationCount * m_unSizeOfElements));
    }

    public byte[] ToManaged() {
        byte[] bytes = new byte[this.m_unSizeOfElements * this.m_nAllocationCount];

        // This is a hack but I haven't found a better solution either
        unsafe {
            fixed (byte* firstByte = bytes ) {
                NativeMemory.Copy(this.m_pMemory, firstByte, (nuint)(this.m_unSizeOfElements * this.m_nAllocationCount));
            }
        }

        return bytes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Iterator_t {
        public int index;
        public static Iterator_t InvalidIterator() {
            return new Iterator_t() { index = -1 };
        }
    }

    public readonly int NumAllocated()
    {
        return m_nAllocationCount;
    }

    public T this[int i] {
        get => Base()[i];
        set => Base()[i] = value;
    }
}