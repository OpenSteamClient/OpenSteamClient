using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.ConCommands;

[Flags]
public enum ConCommandFlags : int
{
    FCVAR_NEVER_AS_STRING = 1 << 0,
    UNK1 = 1 << 1,
    UNK2 = 1 << 2,
    UNK3 = 1 << 3,
    UNK4 = 1 << 4,
};

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ConCommandBase {
    public void *vtable;
    public ConCommandBase *m_pNext; // 0x04
    public bool m_bRegistered; // 0x08
    public IntPtr m_pszName; // 0x0C
    public IntPtr m_pszHelpString; // 0x10
    public ConCommandFlags m_nFlags; // 0x14

    public ConCommandBase *s_pConCommandBases; // 0x18
    public IConCommandBaseAccessor *s_pAccessor; // 0x1C
};

public unsafe interface ConCommandBase_Funcs {
    public void Destructor();
    public void AnotherDestructor();
    public bool IsCommand();
    public uint Unk2();
    public uint Unk3();
    public int GetFlags();
    public uint IsCommand2();
    public string GetName();
    public string GetHelpText();
    public bool ReturnsABool();
    public void* IsCommand6();
    public bool ReturnsABool2();
    // Doesn't return a valid pointer
    public uint IsCommand8();
    public uint IsCommand9();
    public uint IsCommand10();
    public uint IsCommand11();
    public uint IsCommand12();
    public uint IsCommand13();
    public uint IsCommand14();
    public uint IsCommand15();
}