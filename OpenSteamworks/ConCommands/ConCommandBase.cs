using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Native.JIT;
using OpenSteamworks.Utils;

namespace OpenSteamworks.ConCommands;

using CVarDLLIdentifier_t = System.Int32;

[Flags]
public enum ConCommandFlags : uint
{
    FCVAR_NEVER_AS_STRING = 1 << 0,
    UNK1 = 1 << 1,
    UNK2 = 1 << 2,
    UNK3 = 1 << 3,
    UNK4 = 1 << 4,
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct ConCommandBase
{
    public void* vtable;
    public ConCommandBase* m_pNext;
    public UInt64 m_bRegistered;
    public IntPtr m_pszName;
    public IntPtr m_pszHelpString;
    public ConCommandFlags m_nFlags;
    public uint unk;
    public void* pCommandCallback;
    public UInt64 unk1;
    public void* completionCallback;
    public UInt64 hasCompletionCallback;
    public UInt64 usingNewCommandCallback;
    public UInt32 usingCommandCallbackInterface;
    public uint padding;
    // This padding doesn't exist for concommands
    public byte extrapadding;
    public void* unknownPointer;
};

//NOTE: Since we don't support inheritance in JITEngine, make sure all inheriting classes contain the same functions!
public unsafe interface ConCommandBase_Funcs
{
    public ConCommandBase* Next { get; set; }
    public UInt64 m_bRegistered { get; set; }
    public IntPtr m_pszName { get; set; }
    public IntPtr m_pszHelpString { get; set; }
    public ConCommandFlags m_nFlags { get; set; }
    public uint unk { get; set; }
    public void* pCommandCallback { get; set; }

    public void Destructor1();
    public void Destructor2();
    public bool IsCommand();
    public bool IsFlagSet(int flag);
    //public bool IsCommand2();
    //public bool IsCommand3();
    public void AddFlags(int flags);
    public void RemoveFlags(int flags);
    public bool IsCommand4();
    public string GetName();
    public bool IsCommand6();
    // public bool IsFlagSet(int flag);
    // public void AddFlags(int flags);
    // public void RemoveFlags(int flags);
    // public int GetFlags();
    // public string GetName();
    // public string? GetHelpText();
    public bool IsCommand7();
    public ConCommandBase* GetNext();
    public int SetConvarValue(string partial);
    public int AutoCompleteSuggest(string partial, CUtlVector<CUtlString>* commands);
    public bool ReturnsABool();

    //public CVarDLLIdentifier_t GetDLLIdentifier();
    // public void Create(string name, string? helpString = null, int flags = 0);
    // public void Init();
    // public void Init1();
    // public void Init2();
    // public void Init3();
    //public void Init4();
    //public bool CanAutoComplete();
    //public int AutoCompleteSuggest(string partial, CUtlStringList* commands);
    public void Dispatch(uint unk, byte* args);
}

public unsafe struct ConVar
{
    public ConVar* m_pParent;

    // Static data
    public IntPtr m_pszDefaultValue;

    // Value
    // Dynamically allocated
    public IntPtr m_pszString;
    public int m_StringLength;

    // Values
    public float m_fValue;
    public int m_nValue;

    // Min/Max values
    public bool m_bHasMin;
    public float m_fMinVal;
    public bool m_bHasMax;
    public float m_fMaxVal;

    // Call this function when ConVar changes
    public void* m_fnChangeCallback;
}

public unsafe struct commandArgs {
    public uint unk = 0;
    public uint unk1 = 0;
    public uint unk2 = 0;
    public uint unk3 = 0;
    public byte** argsListHead = null;
    public uint unk4 = 0;
    public uint unk5 = 0;
    public uint argCount = 0;

    public commandArgs()
    {

    }
}

// public unsafe interface ConCommand_Funcs
// {
//     public void Destructor();
//     public void Destructor1();
//     public bool IsCommand();
//     public bool IsFlagSet(int flag);
//     public void AddFlags(int flags);
//     public void RemoveFlags(int flags);
//     public int GetFlags();
//     public string GetName();
//     public string? GetHelpText();
//     public bool IsRegistered();
//     public ConCommandBase* GetNext();
//     public bool ReturnsABool();
//     public CVarDLLIdentifier_t GetDLLIdentifier();
//     public void Create(string name, string? helpString = null, int flags = 0);
//     public void Init();
//     public bool IsCommand2();
//     public int AutoCompleteSuggest(string partial, CUtlStringList* commands);
//     public bool CanAutoComplete();
//     public void Dispatch(CCommand* command);
// }

// public unsafe interface ConVar_Funcs
// {
//     public void Destructor();
//     public void Destructor1();
//     public bool IsCommand();
//     public bool IsFlagSet(int flag);
//     public void AddFlags(int flags);
//     public void RemoveFlags(int flags);
//     public int GetFlags();
//     public string GetName();
//     public string? GetHelpText();
//     public bool IsRegistered();
//     public ConCommandBase* GetNext();
//     public bool ReturnsABool();
//     public CVarDLLIdentifier_t GetDLLIdentifier();
//     public void Create(string name, string? helpString = null, int flags = 0);
//     public void Init();
// }