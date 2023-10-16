using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenSteamworks.Native.JIT;

namespace OpenSteamworks.ConCommands;

public unsafe class ConsoleNative {

    public ConsoleNative(Native.ClientNative clientNative) {
        IConCommandBaseAccessor accessor;
        delegate* unmanaged[Cdecl]<IConCommandBaseAccessor*, ConCommandBase*, byte> ptr = &RegisterConCommandBase;
        accessor.accessorFunc = &ptr;

        clientNative.IClientEngine.ConCommandInit(&accessor);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe byte RegisterConCommandBase(IConCommandBaseAccessor *acc, ConCommandBase *pVar)
    {
        //NOTE: I don't know if the steam client contains ConVars as well as ConCommands
        Console.WriteLine("[ConCommands] ConCommand added: " + Marshal.PtrToStringAuto(pVar->m_pszName) + " : " + Marshal.PtrToStringAuto(pVar->m_pszHelpString) + ", flags: " + (int)pVar->m_nFlags + "(" + pVar->m_nFlags + ")");
        Console.WriteLine("s_pConCommandBases: " + (IntPtr)pVar->s_pConCommandBases + ", s_pAccessor: " + (IntPtr)pVar->s_pAccessor);
        // ConCommandBase_Funcs funcs = JITEngine.GenerateUniqueClass<ConCommandBase_Funcs>((IntPtr)pVar);
        // Console.WriteLine("[ConCommands] IsCommand: " + funcs.IsCommand());
        // Console.WriteLine("[ConCommands] Unk2: " + funcs.Unk2());
        // //Console.WriteLine("[ConCommands] Unk3: " + funcs.Unk3());
        //Console.WriteLine("[ConCommands] Flags: " + );

        // Console.WriteLine("[ConCommands] IsCommand2: " + funcs.IsCommand2() + " pVar->m_bRegistered " +  pVar->m_bRegistered);
        pVar->m_bRegistered = true;
        
        // Console.WriteLine("[ConCommands] IsCommand2: " + funcs.IsCommand2() + " pVar->m_bRegistered " +  pVar->m_bRegistered);
        // Console.WriteLine("[ConCommands] GetName: " + funcs.GetName());
        // Console.WriteLine("[ConCommands] GetDescription: " + funcs.GetHelpText());
        // Console.WriteLine("[ConCommands] ReturnsABool: " + funcs.ReturnsABool());
        // //Console.WriteLine("[ConCommands] IsCommand6: " + printPtr(funcs.IsCommand6()));
        // Console.WriteLine("[ConCommands] ReturnsABool2: " + funcs.ReturnsABool2());
        // Console.WriteLine("[ConCommands] IsCommand8: " + funcs.IsCommand8());
        
        // Console.WriteLine("[ConCommands] IsCommand9: " + funcs.IsCommand9());
        // Console.WriteLine("[ConCommands] IsCommand10: " + funcs.IsCommand10());
        // Console.WriteLine("[ConCommands] IsCommand11: " + funcs.IsCommand11());
        // Console.WriteLine("[ConCommands] IsCommand12: " + funcs.IsCommand12());
        // Console.WriteLine("[ConCommands] IsCommand13: " + funcs.IsCommand13());

        //Console.WriteLine("[ConCommands] ConCommand re-read: " + Marshal.PtrToStringAuto(pVar->m_pszName) + " : " + Marshal.PtrToStringAuto(pVar->m_pszHelpString) + ", flags: " + pVar->m_nFlags);
        return 1;
    }
    private unsafe static string printPtr(void* ptr) {
        return string.Format("0x{0:x}", (IntPtr)ptr);
    }
}