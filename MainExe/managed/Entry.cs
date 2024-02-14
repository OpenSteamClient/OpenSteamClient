using System;
using System.Diagnostics.CodeAnalysis;

namespace managed;

public static class Entry
{
    [NotNull]
    public static Updater? Updater { get; private set; }

    [NotNull]
    public static Paths? Paths { get; private set; }

    public delegate int MainDelegate();
    public static int Main() {
        Console.WriteLine("EntryMain called");
        Paths = new Paths();
        Updater = new Updater();
        return 0;
    }
    
    public unsafe delegate void* SteamBootstrapper_GetInstallDirDelegate();
    public static unsafe void* SteamBootstrapper_GetInstallDir() {
        Console.WriteLine("SteamBootstrapper_GetInstallDir called");
        return Updater.CurrentBeta.CurrentPtr;
    }

    public unsafe delegate void PermitDownloadClientUpdatesDelegate(bool permit);
    public static unsafe void PermitDownloadClientUpdates(bool permit) {
        Console.WriteLine("PermitDownloadClientUpdates called with (permit: " + permit + ")");
    }

    
}
