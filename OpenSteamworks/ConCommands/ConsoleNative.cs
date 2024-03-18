using System;
using System.Collections.Generic;
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
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe byte RegisterConCommandBase(IConCommandBaseAccessor *acc, ConCommandBase *pVar)
    {
        //TODO: whole concommand system breaks, prints a bunch of chinese characters and has an infinite loop at IsCommand (at least on Windows)
        //TODO: we really should figure out how to access the concommand system with functions instead relying on the class struct fields
        if (!OperatingSystem.IsLinux()) {
            return 1;
        }

        if (!File.Exists("/tmp/concommand.bin") && Marshal.PtrToStringAuto(pVar->m_pszName) == "package_info_print") {
            Span<byte> bytes = new(pVar, 100);
            System.IO.File.WriteAllBytes("/tmp/concommand.bin", bytes.ToArray());
        }
        
        ConCommandBase_Funcs basefuncs = JITEngine.GenerateClass<ConCommandBase_Funcs>((IntPtr)pVar);
        // Logging.ConCommandsLogger.Info("");
        // Logging.ConCommandsLogger.Info("PName: " + Marshal.PtrToStringAuto(pVar->m_pszName));
        // Logging.ConCommandsLogger.Info("m_bRegistered: " + pVar->m_bRegistered);
        // Logging.ConCommandsLogger.Info("IsCommand: " + basefuncs.IsCommand());
        // Logging.ConCommandsLogger.Info("m_nFlags: " + pVar->m_nFlags);
        // Logging.ConCommandsLogger.Info("IsFlagSet 0: " + basefuncs.IsFlagSet(0));
        // Logging.ConCommandsLogger.Info("IsFlagSet 1: " + basefuncs.IsFlagSet(1));
        // Logging.ConCommandsLogger.Info("IsFlagSet 2: " + basefuncs.IsFlagSet(2));
        // Logging.ConCommandsLogger.Info("IsFlagSet 3: " + basefuncs.IsFlagSet(3));
        // Logging.ConCommandsLogger.Info("IsFlagSet 4: " + basefuncs.IsFlagSet(4));
        // Logging.ConCommandsLogger.Info("hasCompletionCallback: " + pVar->hasCompletionCallback);
        // Logging.ConCommandsLogger.Info("usingNewCommandCallback: " + pVar->usingNewCommandCallback);
        // Logging.ConCommandsLogger.Info("usingCommandCallbackInterface: " + pVar->usingCommandCallbackInterface);
        // Logging.ConCommandsLogger.Info("pCommandCallback: " + (nint)pVar->pCommandCallback);
        // Logging.ConCommandsLogger.Info("completionCallback: " + (nint)pVar->completionCallback);
        // Logging.ConCommandsLogger.Info("unknownPointer: " + (nint)pVar->unknownPointer);

        // if (basefuncs.IsCommand()) {
        //     Logging.ConCommandsLogger.Info("CanAutoComplete: " + basefuncs.CanAutoComplete());
        // }

        if (basefuncs.IsCommand()) {
            // if (Marshal.PtrToStringAuto(pVar->m_pszName) == "apps_installed") {
            //     var concommand = (ConCommand*)pVar;
            //     var ccommand = new CCommand("apps_installed");
            //     Logging.ConCommandsLogger.Info("m_nArgc " + ccommand.m_nArgc);
            //     concommand->pCommandCallback(&ccommand, &ccommand, concommand);
            //     //Logging.ConCommandsLogger.Info("m_pArgSBuffer" + ccommand.m_pArgSBuffer);
            //     //basefuncs.Dispatch(&ccommand, &ccommand);
            // }

            // if (Marshal.PtrToStringAuto(pVar->m_pszName) == "app_status") {
            //     var concommand = (ConCommand*)pVar;
            //     var ccommand = new CCommand("app_status 730");
            //     Logging.ConCommandsLogger.Info("m_nArgc " + ccommand.m_nArgc);
            //     concommand->pCommandCallback(&ccommand, &ccommand, concommand);
            //     //basefuncs.Dispatch2(&ccommand, &ccommand);
            // }

            return Convert.ToByte(ConCommandHandler.RegisterNativeConCommand((ConCommand*)pVar));
            // if (Marshal.PtrToStringAuto(concommand->m_pszName) == "app_status") {
            //     var ccommand = new CCommand("app_status 730");
            //     Logging.ConCommandsLogger.Info("m_nArgc " + ccommand.m_nArgc);
            //     concommand->pCommandCallback(&ccommand, &ccommand, concommand);
            //     //basefuncs.Dispatch2(&ccommand, &ccommand);
            // }
        } else {
            return Convert.ToByte(ConCommandHandler.RegisterNativeConVar((ConVar*)pVar));
        }
    }
}