using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlString {
    public byte *m_pchString = (byte*)IntPtr.Zero;
    public CUtlString() {}
    public string? ToManaged() {
        return Marshal.PtrToStringAuto((IntPtr)this.m_pchString);
    }

    public override string ToString() {
        string? str = this.ToManaged();
        if (str == null) {
            return "";
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