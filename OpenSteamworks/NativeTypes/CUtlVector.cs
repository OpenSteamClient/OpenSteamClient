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

    public CUtlVector() {
        this.m_Size = 0;
        this.m_Memory = new CUtlMemory<T>(0, this.m_Size);
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

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlStringList {
    public CUtlMemory<CUtlString> m_Memory;
    public int m_Size;
    public CUtlStringList(int length) {
        this.m_Size = length;
        this.m_Memory = new CUtlMemory<CUtlString>(0, this.m_Size);
        unsafe {
            for (int i = 0; i < length; i++)
            {
                this.Base()[i] = new CUtlString();
            }
        }
    }

    public CUtlStringList() {
        this.m_Size = 0;
        this.m_Memory = new CUtlMemory<CUtlString>(0, this.m_Size);
    }

    public CUtlString* Base() {
        return (CUtlString*)m_Memory.m_pMemory;
    }
    public CUtlString Element(int i) {
        unsafe {
            return Base()[i];
        }
    }

    public void Free() {
        this.m_Memory.Free();
    }

    public List<string?> ToManagedAndFree() {
        var str = this.ToManaged();
        for (int i = 0; i < this.m_Size; i++)
        {
            this.Element(i).Free();
        }
        this.Free();
        return str;
    }
    
    public List<string?> ToManaged() {
        List<string?> list = new();
        for (int i = 0; i < this.m_Size; i++)
        {
            list.Add(this.Element(i).ToManaged());
        }
        return list;
    }
}