using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenSteamworks.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlVector<T> where T : unmanaged {
    public CUtlMemory<T> m_Memory;
    public int m_Size;
    public CUtlVector(int length, T defaultObject) {
        this.m_Size = length;
        this.m_Memory = new CUtlMemory<T>(0, this.m_Size);
        unsafe {
            for (int i = 0; i < length; i++)
            {
                this.Base()[i] = defaultObject;
            }
        }
        
    }

    public T* Base() {
        return (T*)m_Memory.m_pMemory;
    }
    public T Element(int i) {
        unsafe {
            return Base()[i];
        }
    }

    public void Free() {
        this.m_Memory.Free();
    }

    public List<T> ToManagedAndFree() {
        var str = this.ToManaged();
        this.Free();
        return str;
    }
    public List<T> ToManaged() {
        List<T> list = new();
        for (int i = 0; i < this.m_Size; i++)
        {
            list.Add(this.Element(i));
        }
        return list;
    }
}