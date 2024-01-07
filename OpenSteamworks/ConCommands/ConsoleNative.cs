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
        Logging.ConCommandsLogger.Debug("ConCommand added: " + Marshal.PtrToStringAuto(pVar->m_pszName) + " : " + Marshal.PtrToStringAuto(pVar->m_pszHelpString) + ", flags: " + (int)pVar->m_nFlags + "(" + pVar->m_nFlags + ")");
        Logging.ConCommandsLogger.Debug("s_pConCommandBases: " + (IntPtr)pVar->s_pConCommandBases + ", s_pAccessor: " + (IntPtr)pVar->s_pAccessor);
        // ConCommandBase_Funcs funcs = JITEngine.GenerateUniqueClass<ConCommandBase_Funcs>((IntPtr)pVar);
        // Logging.ConCommandsLogger.Debug("IsCommand: " + funcs.IsCommand());
        // Logging.ConCommandsLogger.Debug("Unk2: " + funcs.Unk2());
        // //Logging.ConCommandsLogger.Debug("Unk3: " + funcs.Unk3());
        //Logging.ConCommandsLogger.Debug("Flags: " + );

        // Logging.ConCommandsLogger.Debug("IsCommand2: " + funcs.IsCommand2() + " pVar->m_bRegistered " +  pVar->m_bRegistered);
        pVar->m_bRegistered = true;
        
        // Logging.ConCommandsLogger.Debug("IsCommand2: " + funcs.IsCommand2() + " pVar->m_bRegistered " +  pVar->m_bRegistered);
        // Logging.ConCommandsLogger.Debug("GetName: " + funcs.GetName());
        // Logging.ConCommandsLogger.Debug("GetDescription: " + funcs.GetHelpText());
        // Logging.ConCommandsLogger.Debug("ReturnsABool: " + funcs.ReturnsABool());
        // //Logging.ConCommandsLogger.Debug("IsCommand6: " + printPtr(funcs.IsCommand6()));
        // Logging.ConCommandsLogger.Debug("ReturnsABool2: " + funcs.ReturnsABool2());
        // Logging.ConCommandsLogger.Debug("IsCommand8: " + funcs.IsCommand8());
        
        // Logging.ConCommandsLogger.Debug("IsCommand9: " + funcs.IsCommand9());
        // Logging.ConCommandsLogger.Debug("IsCommand10: " + funcs.IsCommand10());
        // Logging.ConCommandsLogger.Debug("IsCommand11: " + funcs.IsCommand11());
        // Logging.ConCommandsLogger.Debug("IsCommand12: " + funcs.IsCommand12());
        // Logging.ConCommandsLogger.Debug("IsCommand13: " + funcs.IsCommand13());

        //Logging.ConCommandsLogger.Debug("ConCommand re-read: " + Marshal.PtrToStringAuto(pVar->m_pszName) + " : " + Marshal.PtrToStringAuto(pVar->m_pszHelpString) + ", flags: " + pVar->m_nFlags);
        return 1;
    }
    private unsafe static string printPtr(void* ptr) {
        return string.Format("0x{0:x}", (IntPtr)ptr);
    }
}