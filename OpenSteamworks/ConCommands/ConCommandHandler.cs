using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Native.JIT;
using OpenSteamworks.Utils;

namespace OpenSteamworks.ConCommands;

public static class ConCommandHandler {
    public static ReadOnlyDictionary<string, ManagedConCommandBase> ConCommands {
        get => new(conCommands);
    }

    private static readonly Dictionary<string, ManagedConCommandBase> conCommands = new();

    public static unsafe bool RegisterNativeConCommandBase(nint cmd) {
        ManagedConCommandBase conCommand = ManagedConCommandBase.CreateCmdOrVar(GetSource("NativeClient"), cmd);
        if (conCommands.ContainsKey(conCommand.Name)) {
            return false;
        }

        conCommands.Add(conCommand.Name, conCommand);
        return true;
    }

    public static ManagedConCommand CreateConCommand(Action<string[]> action, int source, string name, string? helpText = null) {
        if (conCommands.ContainsKey(name)) {
            throw new InvalidOperationException("A concommand with name " + name + " has already been registered.");
        }

        var cmd = new ManagedConCommand(action, source, name, helpText);
        conCommands.Add(name, cmd);

        return cmd;
    }

    public static ManagedConVar CreateConVar(string defaultValue, int source, string name, string? helpText = null) {
        if (conCommands.ContainsKey(name)) {
            throw new InvalidOperationException("A concommand with name " + name + " has already been registered.");
        }

        var cmd = new ManagedConVar(defaultValue, source, name, helpText);
        conCommands.Add(name, cmd);

        return cmd;
    }

    public unsafe static void ExecuteConsoleCommand(string cmdline) {
        string[] args = ParseArgs(cmdline);
        string cmdname = args[0];

        // Remove the first arg if we have one, otherwise use an empty array for the args
        if (args.Length > 1) {
            args = args[1..];
        } else {
            args = [];
        }

        if (!conCommands.TryGetValue(cmdname, out ManagedConCommandBase? cmd)) {
            // Don't throw here, instead warn the user
            Logging.ConCommandsLogger.Warning("Tried to execute '" + cmdline + "' but could not find command named " + cmdname);
            return;
        }

        cmd.Invoke(args);
    }

    public static string[] ParseArgs(string cmdline) {
        List<string> args = new();

        List<char> currentArg = new();
        bool readingQuotedString = false;
        using var reader = new StringReader(cmdline);
        int ci = 0;
        while ((ci = reader.Read()) != -1)
        {
            var c = (char)ci;

            if (c == '\\') {
                var next = reader.Peek();
                if (next != -1) {

                    // Add allowed escape chars here
                    if (next == '\"') {
                        var nc = (char)next;
                        currentArg.Add(nc);
                        continue;
                    }
                }
            }

            if (c == '\"') {
                if (readingQuotedString) {
                    readingQuotedString = false;
                } else {
                    readingQuotedString = true;
                }

                continue;
            }

            if (c == ' ' && !readingQuotedString) {
                args.Add(new string(currentArg.ToArray()));
                currentArg.Clear();
                continue;
            }

            currentArg.Add(c);
        }

        if (currentArg.Count > 0) {
            args.Add(new string(currentArg.ToArray()));
            currentArg.Clear();
        }

        foreach (var item in args)
        {
            Console.WriteLine(item);
        }

        return args.ToArray();
    }

    private readonly static List<string> sources = new();
    public static int GetSource(string name) {
        if (sources.Contains(name)) {
            return sources.IndexOf(name);
        }
 
        sources.Add(name);
        return sources.Count - 1;
    }

    public static string GetSourceName(int sourceID) {
        if (sourceID >= sources.Count) {
            return string.Empty;
        }

        return sources[sourceID];
    }
}