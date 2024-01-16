using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using OpenSteamworks.Utils;

namespace OpenSteamworks.ConCommands;

public static class ConCommandHandler {
    public static ReadOnlyDictionary<string, ManagedConCommand> ConCommands {
        get => new(conCommands);
    }

    private static Dictionary<string, ManagedConCommand> conCommands = new();

    public static unsafe bool RegisterNativeConCommand(ConCommand* cmd) {
        ManagedConCommand conCommand = new(cmd, ManagedConCommand.ESource.NativeClient);
        if (conCommands.ContainsKey(conCommand.Name)) {
            return false;
        }

        conCommands.Add(conCommand.Name, conCommand);
        return true;
    }

    public static unsafe bool RegisterNativeConVar(ConVar* cmd) {
        return true;
    }

    public static void RegisterManagedConCommand(ManagedConCommand conCommand) {
        if (conCommands.ContainsKey(conCommand.Name)) {
            throw new InvalidOperationException("A concommand with name " + conCommand.Name + " has already been registered.");
        }

        conCommands.Add(conCommand.Name, conCommand);
    }

    public static void ExecuteConsoleCommand(string cmdline) {
        var command = new CCommand(cmdline);
        string commandName = command.GetCommandName();
        if (conCommands.TryGetValue(commandName, out ManagedConCommand? conCommand)) {
            conCommand.Invoke(command);
            return;
        }

        // Don't throw here, instead warn the user
        Logging.ConCommandsLogger.Warning("Tried to execute '" + cmdline + "' but could not find command named " + commandName);
    }
}

public unsafe class ManagedConCommand {
    public enum ESource {
        NativeClient,
        ClientUI,
        OSWFramework
    }

    public ESource Source { get; init; }
    public string Name { get; init; }
    public string? HelpText { get; init; } = null;
    private readonly Action<CCommand>? managedAction = null;
    private readonly ConCommand* nativeCommand = null;

    public ManagedConCommand(Action<CCommand> managedAction, ESource source, string name, string? helpText = null) {
        this.managedAction = managedAction;
        this.Source = source;
        this.Name = name;
        this.HelpText = helpText;
    }

    public ManagedConCommand(ConCommand* nativeCommand, ESource source) {
        this.nativeCommand = nativeCommand;
        this.Source = source;
        this.Name = Marshal.PtrToStringUTF8(nativeCommand->m_pszName)!;
        this.HelpText = Marshal.PtrToStringUTF8(nativeCommand->m_pszHelpString);
    }

    public unsafe void Invoke(CCommand cmdline) {
        if (nativeCommand != null) {
            nativeCommand->pCommandCallback(&cmdline, &cmdline, nativeCommand);
        }

        if (managedAction != null) {
            managedAction.Invoke(cmdline);
        }
    }
}