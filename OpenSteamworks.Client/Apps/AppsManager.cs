
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using ValveKeyValue;
using static OpenSteamworks.Callbacks.CallbackManager;

namespace OpenSteamworks.Client.Apps;

public class AppPlaytimeChangedEventArgs : EventArgs {
    public AppPlaytimeChangedEventArgs(uint appid, AppPlaytime_t playtime) { AppID = appid; PlaytimeAllTime = TimeSpan.FromMinutes(playtime.AllTime); PlaytimeLastTwoWeeks = TimeSpan.FromMinutes(playtime.LastTwoWeeks); }
    public uint AppID { get; }
    public TimeSpan PlaytimeAllTime { get; }
    public TimeSpan PlaytimeLastTwoWeeks { get; }
}

public class AppLastPlayedChangedEventArgs : EventArgs {
    public AppLastPlayedChangedEventArgs(uint appid, UInt32 lastPlayed) { AppID = appid; LastPlayed = DateTimeOffset.FromUnixTimeSeconds(lastPlayed).DateTime; }
    public uint AppID { get; }
    public DateTime LastPlayed { get; }
}

public class AppsManager : ILogonLifetime
{
    public ReadOnlyCollectionEx<AppId_t> OwnedAppIDs {
        get {
            return new ReadOnlyCollectionEx<AppId_t>(ownedAppIDs);
        }
    }

    private object ownedAppsLock = new();
    private HashSet<AppId_t> ownedAppIDs { get; init; } = new();
    private SteamClient steamClient;
    private ClientMessaging clientMessaging;
    private Logger logger;
    private InstallManager installManager;
    private LoginManager loginManager;
    public readonly ClientApps ClientApps;

    public EventHandler<AppPlaytimeChangedEventArgs>? AppPlaytimeChanged;
    public EventHandler<AppLastPlayedChangedEventArgs>? AppLastPlayedChanged;

    public AppsManager(SteamClient steamClient, ClientApps clientApps, ClientMessaging clientMessaging, InstallManager installManager, LoginManager loginManager) {
        this.logger = Logger.GetLogger("AppsManager", installManager.GetLogPath("AppsManager"));
        this.ClientApps = clientApps;
        this.loginManager = loginManager;
        this.steamClient = steamClient;
        this.clientMessaging = clientMessaging;
        this.installManager = installManager;
        steamClient.CallbackManager.RegisterHandler<AppMinutesPlayedDataNotice_t>(OnAppMinutesPlayedDataNotice);
        steamClient.CallbackManager.RegisterHandler<AppLastPlayedTimeChanged_t>(OnAppLastPlayedTimeChanged);
        steamClient.CallbackManager.RegisterHandler<AppLicensesChanged_t>(OnAppLicensesChanged);
    }

    public void OnAppMinutesPlayedDataNotice(CallbackHandler<AppMinutesPlayedDataNotice_t> handler, AppMinutesPlayedDataNotice_t notice) {
        UInt32 allTime = 0;
        UInt32 lastTwoWeeks = 0;
        if (steamClient.NativeClient.IClientUser.BGetAppMinutesPlayed(notice.m_nAppID, ref allTime, ref lastTwoWeeks))
        {
            this.AppPlaytimeChanged?.Invoke(this, new AppPlaytimeChangedEventArgs(notice.m_nAppID, new AppPlaytime_t(allTime, lastTwoWeeks)));
        }
    }

    public void OnAppLastPlayedTimeChanged(CallbackHandler<AppLastPlayedTimeChanged_t> handler, AppLastPlayedTimeChanged_t lastPlayedTimeChanged) {
        AppLastPlayedChanged?.Invoke(this, new AppLastPlayedChangedEventArgs(lastPlayedTimeChanged.m_nAppID, lastPlayedTimeChanged.m_lastPlayed));
    }

    public void OnAppLicensesChanged(CallbackHandler<AppLicensesChanged_t> handler, AppLicensesChanged_t licensesChanged) {
        if (hasLogOnFinished) {
            lock (ownedAppsLock)
            {
                if (licensesChanged.bReloadAll) {
                    ownedAppIDs.Clear();
                }

                AppId_t[] actualAppsUpdated = new AppId_t[licensesChanged.m_unAppsUpdated];
                Array.Copy(licensesChanged.m_rgAppsUpdated, actualAppsUpdated, licensesChanged.m_unAppsUpdated);
                ownedAppIDs.UnionWith(actualAppsUpdated);
            }
        }
    }

    private bool hasLogOnFinished = false;
    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        var ownedApps = await this.GetAppsForSteamID(e.User.SteamID);

        //TODO: what should we do here? Implementing some sort of App class sounds like a good idea in theory, but would make AppsManager largely obsolete.
        //TODO: how to organize getting app's names, soundtrack infos, etc easily?
        lock (ownedAppsLock)
        {
            ownedAppIDs.UnionWith(ownedApps);
        }
        hasLogOnFinished = true;
    }

    public SteamApp GetSteamAppSync(AppId_t appid) {
        return AppBase.CreateSteamApp(this, appid);
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress) {
        hasLogOnFinished = false;
        ownedAppIDs.Clear();
    }

    /// <summary>
    /// May return '' or 'public' depending on the phase of the moon, angle of the sun and some other unknown factors (public seems to be the correct behaviour, does '' stand for failure?)
    /// </summary>
    public string GetBetaForApp(AppId_t appid) {
        IncrementingStringBuilder betaName = new();
        betaName.RunUntilFits(() => steamClient.NativeClient.IClientAppManager.GetActiveBeta(appid, betaName.Data, betaName.Length));
        return betaName.ToString();
    }

    /// <summary>
    /// Gets all owned apps for a SteamID. 
    /// Will not work in offline mode. Yet. TODO: we need a robust caching system.
    /// </summary>
    /// <param name="steamid">SteamID of the user to request apps for</param>
    public async Task<HashSet<AppId_t>> GetAppsForSteamID(CSteamID steamid, bool includeSteamPackageGames = true, bool includeFreeGames = true) {
        logger.Debug("Attempting to get owned apps for " + steamid);
        ProtoMsg<Protobuf.CPlayer_GetOwnedGames_Request> request = new("Player.GetOwnedGames#1");
        request.body.Steamid = steamid;
        request.body.IncludeAppinfo = false;
        request.body.IncludeExtendedAppinfo = false;
        request.body.IncludeFreeSub = includeSteamPackageGames;
        request.body.IncludePlayedFreeGames = includeFreeGames;

        ProtoMsg<Protobuf.CPlayer_GetOwnedGames_Response> response;
        HashSet<AppId_t> ownedApps = new();
        using (var conn = this.clientMessaging.AllocateConnection())
        {
            response = await conn.ProtobufSendMessageAndAwaitResponse<Protobuf.CPlayer_GetOwnedGames_Response, Protobuf.CPlayer_GetOwnedGames_Request>(request);
        }

        foreach (var protoApp in response.body.Games)
        {
            // Why the fuck is the AppID field an int here?????
            ownedApps.Add((uint)protoApp.Appid);
        }

        logger.Debug(steamid + " owns " + ownedApps.Count + " games");
        return ownedApps;
    }

    public async Task RunInstallScriptAsync(AppId_t appid, bool uninstall = false) {
        await Task.Run(() => RunInstallScriptSync(appid, uninstall));
    }

    public void RunInstallScriptSync(AppId_t appid, bool uninstall = false) {
        //TODO: we still aren't 100% sure about the second arg.
        steamClient.NativeClient.IClientUser.RunInstallScript(appid, "english", false);
        while (steamClient.NativeClient.IClientUser.IsInstallScriptRunning() != 0)
        {
            Thread.Sleep(30);
        }
    }

    public async Task<EResult> LaunchApp(AppBase app, int launchOption, string userLaunchOptions) {
        return EResult.OK;
        // //TODO: make this actually async, not spaghetti, use compatmanager, use app class, add validation, test on windows, create a better keyvalue system with arrays, maybe other issues
        // var logger = this.logger.CreateSubLogger("LaunchApp");

        // string workingDir = "";
        // string gameExe = "";
        // if (app.IsSteamApp) {
        //     SteamApp steamApp = (SteamApp)app;
           

        //     //TODO: What function should we use here?
        //     if (this.steamClient.NativeClient.IClientRemoteStorage.IsCloudEnabledForAccount() && this.steamClient.NativeClient.IClientRemoteStorage.IsCloudEnabledForApp(app.AppID)) {
        //         this.steamClient.NativeClient.IClientRemoteStorage.RunAutoCloudOnAppLaunch(app.AppID);
        //     }

        //     logger.Info("Getting launch info");
        //     StringBuilder gameInstallDir = new(1024);
        //     StringBuilder launchOptionExe = new(1024);
        //     StringBuilder launchOptionCommandLine = new(1024);
        //     this.steamClient.NativeClient.IClientAppManager.GetAppInstallDir(app.AppID, gameInstallDir, 1024);
            
        //     //TODO: do these keys exist 100% of the time for all apps?
        //     // For EA games, they usually only have an executable "link2ea://launchgame/" which isn't valid on filesystem, and also are missing the 'arguments' key. WTF?
        //     //TODO: how to handle link2ea protocol links (especially with proton) 
        //     this.steamClient.NativeClient.IClientApps.GetAppData(app.AppID, $"config/launch/{launchOption}/executable", launchOptionExe, 1024);
        //     this.steamClient.NativeClient.IClientApps.GetAppData(app.AppID, $"config/launch/{launchOption}/arguments", launchOptionCommandLine, 1024);
        //     workingDir = gameInstallDir.ToString();
        //     gameExe = launchOptionExe.ToString();
        // } else if (app.IsShortcut) {

        // } else if (app.IsMod) {

        // } else {
        //     throw new UnreachableException("Something is seriously wrong.");
        // }

        // string commandLine = "";

        // // First fill compat tools
        // if (this.steamClient.NativeClient.IClientCompat.BIsCompatLayerEnabled() && this.steamClient.NativeClient.IClientCompat.BIsCompatibilityToolEnabled(appid))
        // {
        //     //TODO: how to handle "selected by valve testing" tools (like for csgo)
        //     string compattool = this.steamClient.NativeClient.IClientCompat.GetCompatToolName(app.AppID);
        //     if (!string.IsNullOrEmpty(compattool)) {
        //         KVSerializer serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        //         bool useSessions = false;
        //         List<AppId_t> compatToolAppIDs = new();
        //         //TODO: this is terrible, but the API's don't seem to provide other ways to do this (HOW DO NON-STEAM COMPAT TOOLS WORK??????)
        //         bool hasDepTool = true;
        //         StringBuilder compatToolAppidStr = new(1024);
        //         StringBuilder compatToolInstallDir = new(1024);

        //         //TODO: loop over compat_tools, map into dictionaries
        //         this.steamClient.NativeClient.IClientApps.GetAppData(891390, $"extended/compat_tools/{compattool}/appid", compatToolAppidStr, 1024);

        //         AppId_t depTool = uint.Parse(compatToolAppidStr.ToString());
        //         compatToolAppIDs.Add(depTool);
        //         while (hasDepTool) {
        //             this.steamClient.NativeClient.IClientAppManager.GetAppInstallDir(depTool, compatToolInstallDir, 1024);
        //             var bytes = File.ReadAllBytes(Path.Combine(compatToolInstallDir.ToString(), "toolmanifest.vdf"));
        //             KVObject manifest;
        //             using (var stream = new MemoryStream(bytes))
        //             {
        //                 manifest = serializer.Deserialize(stream);
        //             }

        //             hasDepTool = (string?)manifest["require_tool_appid"] != null;
        //             if (hasDepTool) {
        //                 depTool = uint.Parse((string)manifest["require_tool_appid"]);
        //                 compatToolAppIDs.Add(depTool);
        //             }
        //         }

                
        //         foreach (var compatToolAppID in compatToolAppIDs)
        //         {
        //             this.steamClient.NativeClient.IClientAppManager.GetAppInstallDir(compatToolAppID, compatToolInstallDir, 1024);
        //             var bytes = File.ReadAllBytes(Path.Combine(compatToolInstallDir.ToString(), "toolmanifest.vdf"));
        //             //TODO: how to decide correct verb to use?
        //             var verb = "waitforexitandrun";
        //             KVObject manifest;
        //             using (var stream = new MemoryStream(bytes))
        //             {
        //                 manifest = serializer.Deserialize(stream);
        //             }

        //             if ((string)manifest["version"] != "2") {
        //                 throw new Exception("Unsupported manifest version '" + (string)manifest["version"] + "'");
        //             }

        //             if ((string)manifest["use_sessions"] == "1") {
        //                 useSessions = true;
        //             }

        //             var commandline = (string)manifest["commandline"];
        //             commandline = commandline.Replace("%verb%", verb);
        //             var compatToolCommandLine = $"'{compatToolInstallDir}'{commandline}";
        //             commandLine = compatToolCommandLine + " " + commandLine;
        //         }

        //         if (useSessions) {
        //             this.steamClient.NativeClient.IClientCompat.StartSession(app.AppID);
        //         }
        //     }
        // }

        // // Then prefix with reaper, launchwrapper
        // if (OperatingSystem.IsLinux()) {
        //     string steamToolsPath = Path.Combine(this.installManager.InstallDir, "linux64");

        //     // This is the base command line. It seems to always be used for all games, regardless of if you have compat tools enabled or not.
        //     commandLine = $"{steamToolsPath}/reaper SteamLaunch AppId={app.AppID} -- {steamToolsPath}/steam-launch-wrapper -- " + commandLine;
        // }

        // //TODO: does windows use x86launcher.exe or x64launcher.exe?

        // commandLine += $"'{workingDir}/{gameExe}' {launchOptionCommandLine}";

        // if (userLaunchOptions.Contains("%command%")) {
        //     string prefix = userLaunchOptions.Split("%command%")[0];
        //     string suffix = userLaunchOptions.Split("%command%")[1];
        //     // Yep. It's this simple. this allows stuff like %command%-mono for tModLoader
        //     commandLine = prefix + commandLine + suffix;
        // }

        // //TODO: synchronize controller config (how?)
        // //TODO: handle "site licenses" (extra wtf???)

        // if (app.IsMod) {
        //     //TODO: where to get sourcemod path?
        //     commandLine += " -game sourcemodpathhere";
        // }

        // logger.Info("Built launch command line: " + commandLine);
        // logger.Info("Creating process");
        // using (var vars = new TemporaryEnvVars())
        // {
        //     // Do we really need all these vars?
        //     // Valid vars: STEAM_GAME_LAUNCH_SHELL STEAM_RUNTIME_LIBRARY_PATH STEAM_COMPAT_FLAGS SYSTEM_PATH SYSTEM_LD_LIBRARY_PATH SUPPRESS_STEAM_OVERLAY STEAM_CLIENT_CONFIG_FILE SteamRealm SteamLauncherUI
        //     vars.SetEnvironmentVariable("SteamAppUser", this.loginManager.CurrentUser!.AccountName);
        //     vars.SetEnvironmentVariable("SteamAppId", app.AppID.ToString());
        //     vars.SetEnvironmentVariable("SteamGameId", ((ulong)app.GameID).ToString());
        //     vars.SetEnvironmentVariable("SteamClientLaunch", "1");
        //     vars.SetEnvironmentVariable("SteamClientService", "127.0.0.1:57344");
        //     vars.SetEnvironmentVariable("AppId", app.AppID.ToString());
        //     vars.SetEnvironmentVariable("SteamLauncherUI", "clientui");
        //     //vars.SetEnvironmentVariable("PROTON_LOG", "1");
        //     vars.SetEnvironmentVariable("STEAM_COMPAT_CLIENT_INSTALL_PATH", installManager.InstallDir);
        //     this.steamClient.NativeClient.IClientUtils.SetLastGameLaunchMethod(0);  
        //     StringBuilder libraryFolder = new(1024);
        //     this.steamClient.NativeClient.IClientAppManager.GetLibraryFolderPath(this.steamClient.NativeClient.IClientAppManager.GetAppLibraryFolder(app.AppID), libraryFolder, libraryFolder.Capacity);
        //     logger.Info("Appid " + app.AppID + " is installed into library folder " + this.steamClient.NativeClient.IClientAppManager.GetAppLibraryFolder(app.AppID) + " with path " + libraryFolder);
        //     vars.SetEnvironmentVariable("STEAM_COMPAT_DATA_PATH", Path.Combine(libraryFolder.ToString(), "steamapps", "compatdata", app.AppID.ToString()));
            
        //     logger.Info("Vars for launch: ");
        //     foreach (var item in UtilityFunctions.GetEnvironmentVariables())
        //     {
        //         logger.Info(item.Key + "=" + item.Value);
        //     }

        //     // For some reason, the CGameID SpawnProcess takes is a pointer.
        //     CGameID gameidref = app.GameID;
        //     this.steamClient.NativeClient.IClientUser.SpawnProcess("", commandLine, workingDir, ref gameidref, app.Name);
        // }

        // return EResult.OK;
    }
    
    public Logger GetLoggerForApp(AppBase app) {
        string name;
        if (app.IsSteamApp) {
            name = "SteamApp";
        } else if (app.IsMod) {
            name = "Mod";
        } else if (app.IsShortcut) {
            name = "Shortcut";
        } else {
            name = "App";
        }

        name += app.GameID;
        return Logger.GetLogger(name, installManager.GetLogPath(name));
    }
}