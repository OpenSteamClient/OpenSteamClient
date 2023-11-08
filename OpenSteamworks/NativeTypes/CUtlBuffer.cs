using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlBuffer {
    public CUtlMemory<UInt8> m_Memory;
	public int m_Get;
	public int m_Put;

	public int m_nMaxPut;
	public UInt16 m_nTab;

	public UInt8 m_Error;
	public UInt8 m_Flags;

	public delegate* unmanaged[Thiscall]<CUtlBuffer*, int, byte> m_GetOverflowFunc;
	public delegate* unmanaged[Thiscall]<CUtlBuffer*, int, byte> m_PutOverflowFunc;

    public CUtlBuffer(int length, int growSize = 0) {
        this.m_Memory = new CUtlMemory<UInt8>(growSize, length);
        this.m_Error = 0;
        this.m_Get = 0;
        this.m_Put = 0;
        this.m_nTab = 0;
        this.m_Flags = 0;

        this.m_nMaxPut = -1;

        unsafe {
            this.m_GetOverflowFunc = &GetOverflow;
            this.m_PutOverflowFunc = &PutOverflow;
        }
    }

    public void Free() {
        this.m_Memory.Free();
    }

    public byte[] ToManaged()
    {
        byte[] allBytes = this.m_Memory.ToManaged();
        byte[] usedBytes = new byte[this.m_Put];
        Array.Copy(allBytes, usedBytes, this.m_Put);
        return usedBytes;
    }

    public void SeekToBeginning() {
        this.m_Put = 0;
    }

    public byte[] ToManagedAndFree() {
        var str = this.ToManaged();
        this.Free();
        return str;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    public static byte PutOverflow(CUtlBuffer* buf, int nSize) {
        SteamClient.CUtlLogger.Debug("PutOverflow called");
        int nGrowDelta = (buf->m_Put + nSize) - buf->m_Memory.m_nAllocationCount;

        if (nGrowDelta > 0)
        {
            buf->m_Memory.Grow( nGrowDelta );
        }
            
        return 1;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    public static byte GetOverflow(CUtlBuffer* buf, int nSize) {
        SteamClient.CUtlLogger.Debug("GetOverflow called");
        return 0;
    }
}