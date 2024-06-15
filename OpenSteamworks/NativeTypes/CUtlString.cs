using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;



[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlString {
    public byte *m_pchString = (byte*)IntPtr.Zero;
    public CUtlString() {}
    public string? ToManaged() {
        return Marshal.PtrToStringAuto((IntPtr)this.m_pchString);
    }

    public CUtlString(string str) {
        var bytes = Encoding.UTF8.GetBytes(str + "\0");
        this.m_pchString = (byte*)NativeMemory.Alloc((nuint)bytes.Length);
        bytes.CopyTo(new Span<byte>(this.m_pchString, bytes.Length));
    }

    public override string ToString() {
        string? str = this.ToManaged();
        if (str == null) {
            return string.Empty;
        }
        
        return str;
    }

    public string? ToManagedAndFree() {
        var str = this.ToManaged();
        this.Free();
        return str;
    }
    
    public void Free() {
        NativeMemory.Free(this.m_pchString);
    }
}