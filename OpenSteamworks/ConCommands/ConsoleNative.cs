using System;
using System.IO;
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
        throw new Exception("temporary breakpoint for concommand debugging");
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe byte RegisterConCommandBase(IConCommandBaseAccessor *acc, ConCommandBase *pVar)
    {
        if (!File.Exists("/tmp/concommand.bin") && Marshal.PtrToStringAuto(pVar->m_pszName) == "package_info_print") {
            Span<byte> bytes = new(pVar, 100);
            System.IO.File.WriteAllBytes("/tmp/concommand.bin", bytes.ToArray());
        }
        
        ConCommandBase_Funcs basefuncs = JITEngine.GenerateClass<ConCommandBase_Funcs>((IntPtr)pVar);
        Logging.ConCommandsLogger.Info("");
        Logging.ConCommandsLogger.Info("PName: " + Marshal.PtrToStringAuto(pVar->m_pszName));
        Logging.ConCommandsLogger.Info("m_bRegistered: " + pVar->m_bRegistered);
        Logging.ConCommandsLogger.Info("IsCommand: " + basefuncs.IsCommand());
        Logging.ConCommandsLogger.Info("m_nFlags: " + pVar->m_nFlags);
        Logging.ConCommandsLogger.Info("IsFlagSet 0: " + basefuncs.IsFlagSet(0));
        Logging.ConCommandsLogger.Info("IsFlagSet 1: " + basefuncs.IsFlagSet(1));
        Logging.ConCommandsLogger.Info("IsFlagSet 2: " + basefuncs.IsFlagSet(2));
        Logging.ConCommandsLogger.Info("IsFlagSet 3: " + basefuncs.IsFlagSet(3));
        Logging.ConCommandsLogger.Info("IsFlagSet 4: " + basefuncs.IsFlagSet(4));
        Logging.ConCommandsLogger.Info("hasCompletionCallback: " + pVar->hasCompletionCallback);
        Logging.ConCommandsLogger.Info("usingNewCommandCallback: " + pVar->usingNewCommandCallback);
        Logging.ConCommandsLogger.Info("usingCommandCallbackInterface: " + pVar->usingCommandCallbackInterface);
        Logging.ConCommandsLogger.Info("pCommandCallback: " + (nint)pVar->pCommandCallback);
        Logging.ConCommandsLogger.Info("completionCallback: " + (nint)pVar->completionCallback);
        Logging.ConCommandsLogger.Info("unknownPointer: " + (nint)pVar->unknownPointer);

        // if (basefuncs.IsCommand()) {
        //     Logging.ConCommandsLogger.Info("CanAutoComplete: " + basefuncs.CanAutoComplete());
        // }

        if (basefuncs.IsCommand() && Marshal.PtrToStringAuto(pVar->m_pszName) == "app_status") {
            var ccommand = new CCommand("app_status 730");
            Logging.ConCommandsLogger.Info("m_nArgc " + ccommand.m_nArgc);
            basefuncs.Dispatch(&ccommand, &ccommand);
        }

        if (basefuncs.IsCommand() && Marshal.PtrToStringAuto(pVar->m_pszName) == "apps_installed") {
            var ccommand = new CCommand("apps_installed");
            Logging.ConCommandsLogger.Info("m_nArgc " + ccommand.m_nArgc);
            //Logging.ConCommandsLogger.Info("m_pArgSBuffer" + ccommand.m_pArgSBuffer);
            basefuncs.Dispatch(&ccommand, &ccommand);
        }

        return 1;
    }

    private unsafe static string printPtr(void* ptr) {
        return string.Format("0x{0:x}", (IntPtr)ptr);
    }
}