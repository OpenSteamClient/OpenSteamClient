using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OpenSteamworks.Native.JIT;

namespace OpenSteamworks.ConCommands;

public unsafe abstract class ManagedConCommandBase {
    public int Source { get; init; }
    public string Name { get; init; }
    public string? HelpText { get; init; } = null;
    protected ConCommandBase_Funcs? NativeCommand { get; init; } = null;

    public ManagedConCommandBase(int source, string name, string? helpText = null) {
        this.Source = source;
        this.Name = name;
        this.HelpText = helpText;
    }

    public ManagedConCommandBase(int source, ConCommandBase_Funcs nativeCommand) {
        this.Source = source;
        this.NativeCommand = nativeCommand;
        this.Name = Marshal.PtrToStringUTF8(nativeCommand.m_pszName) ?? string.Empty;
        this.HelpText = Marshal.PtrToStringUTF8(nativeCommand.m_pszHelpString);
    }

    public static ManagedConCommandBase CreateCmdOrVar(int source, nint nativeCommand) {
        ConCommandBase_Funcs generated = JITEngine.GenerateClass<ConCommandBase_Funcs>(nativeCommand);
        if (generated.IsCommand()) {
            return new ManagedConCommand(source, generated);
        } else {
            return new ManagedConVar(source, generated);
        }
    }

    public abstract void Invoke(string[] cmdline);
}

public sealed unsafe class ManagedConCommand : ManagedConCommandBase {
    private readonly Action<string[]>? managedAction = null;

    public ManagedConCommand(Action<string[]> managedAction, int source, string name, string? helpText = null) : base(source, name, helpText) {
        this.managedAction = managedAction;
    }

    public ManagedConCommand(int source, ConCommandBase_Funcs nativeCommand) : base(source, nativeCommand) {
        if (!nativeCommand.IsCommand()) {
            throw new Exception("Tried to create ConCommand from convar!");
        }
    }

    public override void Invoke(string[] cmdline) {
        if (this.NativeCommand != null) {
            InvokeNative(cmdline);
            return;
        }

        managedAction?.Invoke(cmdline);
    }

    private void InvokeNative(string[] args) {
        if (NativeCommand == null) {
            return;
        }

        Console.WriteLine("IsCommand: " + NativeCommand.IsCommand());

        if (NativeCommand.IsFlagSet(8)) {
            Logging.ConCommandsLogger.Error($"Non-user command %s received at console; ignoring");
            return;
        }

        if (!NativeCommand.IsCommand()) {
            return;
        }

        // byte bVar3 = 0;
        // byte[] cmdBuf = new byte[512];
        // fixed (byte* ptr = cmdBuf) {
        //     // Clears first 4 bytes
        //     cmdBuf[0] = 0;
        //     cmdBuf[1] = 0;
        //     cmdBuf[2] = 0;
        //     cmdBuf[3] = 0;

        //     uint* clearPtr = (uint*)(ptr + 4);

        //     // Clears 508 bytes (uint is 4 long)
        //     for (int i = 127; i != 0; i--)
        //     {
        //         *clearPtr = 0;
        //         clearPtr = clearPtr + (uint)bVar3 * -2 + 1;
        //     }

        //     // 512 zeroed

        //     for (int i = 1; i < args.Length; i++)
        //     {
        //         var arg = args[i];
        //         Encoding.UTF8.GetBytes(arg + "\0").CopyTo(cmdBuf, 0);

        //         if (i != args.Length) {
        //             Encoding.UTF8.GetBytes(" \0").CopyTo(cmdBuf, 0);
        //         }
        //     }

        //     Logging.ConCommandsLogger.Info("Executing command");
        //     basefuncs.Dispatch(1, (byte*)ptr);
        // }

        commandArgs nativeArgs = new();
        List<nint> nativeArgsList = new();
        List<GCHandle> handlesToFree = new();
        foreach (var item in args)
        {
            nativeArgs.argCount++;
            nativeArgsList.Add(InteropHelp.EncodeUTF8String(item, out GCHandle gcHandle));
            handlesToFree.Add(gcHandle);
        }

        nint[] nativeArgsArr = nativeArgsList.ToArray();
        fixed (nint* head = nativeArgsArr) {
            nativeArgs.argsListHead = (byte**)head;
            Logging.ConCommandsLogger.Info("Executing command");
            NativeCommand.Dispatch(1, (byte*)&nativeArgs);
        }

        foreach (var item in handlesToFree)
        {
            var handle = item;
            InteropHelp.FreeString(ref handle);
        }
    }
}

public sealed unsafe class ManagedConVar : ManagedConCommandBase
{
    private string managedValue = string.Empty;
    private readonly string defaultValue = string.Empty;

    public ManagedConVar(string defaultValue, int source, string name, string? helpText) : base(source, name, helpText) {
        this.defaultValue = defaultValue;
        this.managedValue = defaultValue;
    }

    public ManagedConVar(int source, ConCommandBase_Funcs nativeCommand) : base(source, nativeCommand) {
        if (nativeCommand.IsCommand()) {
            throw new Exception("Tried to create ConVar from command!");
        }
    }

    public string Value {
        get => GetValue();
        set => SetValue(value);
    }

    public string DefaultValue {
        get => GetDefaultValue();
    }

    private string GetValue() {
        if (base.NativeCommand != null) {
            return GetValueNative();
        }

        return managedValue;
    }

    private string GetDefaultValue() {
        if (base.NativeCommand != null) {
            return GetValueNative(true);
        }

        return defaultValue;
    }

    private void SetValue(string val) {
        if (base.NativeCommand != null) {
            NativeCommand.SetConvarValue(val);
            return;
        }

        managedValue = val;
    }

    private string GetValueNative(bool getDefault = false) {
        if (NativeCommand == null) {
            return string.Empty;
        }

        ConCommandBase* commandBase = (ConCommandBase*)JITEngine.GetPointerOfGeneratedClass(NativeCommand);
        // Print out the current (or now changed) value
        string val = string.Empty;
        string actualVal = string.Empty;
        if (commandBase->m_nFlags.HasFlag(ConCommandFlags.FCVAR_NEVER_AS_STRING)) {
            return "FCVAR_NEVER_AS_STRING";
        }

        var span = new ReadOnlySpan<byte>(commandBase->pCommandCallback, 200);
        File.WriteAllBytes("/tmp/concommand_sub.bin", span.ToArray());
        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 1))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 2)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 3))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 4))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 5)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 6))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\"");

        //DEFAULT (STR)
        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 7))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" (def)");

        if (getDefault) {
            actualVal = val;
        }

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 8)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 8");

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 9)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 9");

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 10)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 10");

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 11)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 11");

        // VALUE (STR)
        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 12))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" (val)");

        if (!getDefault) {
            actualVal = val;
        }

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 13)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 13");

        var thirteen = *(nint*)((nint)(commandBase->pCommandCallback) + (8 * 13));
        Console.WriteLine("thirteen=" + thirteen); 
        if (thirteen > 1000000) {
            val = Marshal.PtrToStringUTF8(thirteen) ?? string.Empty;
            Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 13");

            val = Marshal.PtrToStringUTF8(thirteen) ?? string.Empty;
            Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 13");
        }

        val = Marshal.PtrToStringUTF8((nint)(commandBase->pCommandCallback) + (8 * 14)) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 14");

        val = Marshal.PtrToStringUTF8(*(nint*)((nint)(commandBase->pCommandCallback) + (8 * 14))) ?? string.Empty;
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{val}\" 14");

        return actualVal;
    }

    public override void Invoke(string[] cmdline)
    {
        if (cmdline.Length > 0) {
            SetValue(cmdline[0]);
        }
        
        Logging.ConCommandsLogger.Info($"\"{Name}\" = \"{Value}\"");
    }
}