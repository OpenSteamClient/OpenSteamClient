using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using AvaloniaCommon;
using OpenSteamClient.Controls;
using OpenSteamClient.Translation;
using OpenSteamClient.ViewModels.Downloads;
using OpenSteamClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;

using OpenSteamworks.Protobuf;
using OpenSteamworks.Protobuf.WebUI;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using Profiler;

namespace OpenSteamClient.ViewModels;

public partial class MainWindowViewModel : AvaloniaCommon.ViewModelBase
{
    [ObservableProperty]
    private bool showGoOffline = false;

    [ObservableProperty]
    private bool showGoOnline = false;

    [ObservableProperty]
    private BasePage currentPage;

    private PageHeaderViewModel? CurrentPageHeader;
    private readonly Dictionary<Type, BasePage> LoadedPages = new();
    public ObservableCollection<PageHeaderViewModel> PageList { get; } = new() { };

    public bool CanLogonOffline => client.IClientUser.CanLogonOffline() == 1;
    public bool IsOfflineMode => client.IClientUtils.GetOfflineMode();
    private readonly Action openSettingsWindow;
    private readonly TranslationManager tm;
    private readonly ISteamClient client;
    private readonly LoginManager loginManager;
    private readonly AppsManager appsManager;
    private readonly MainWindow mainWindow;

    public MainWindowViewModel(MainWindow mainWindow, ISteamClient client, AppsManager appsManager, TranslationManager tm, LoginManager loginManager, Action openSettingsWindowAction)
    {
        this.mainWindow = mainWindow;
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
        this.openSettingsWindow = openSettingsWindowAction;
        this.appsManager = appsManager;

        this.client.CallbackManager.RegisterHandler(1210004, OnCGameNetworkingUI_AppSummary);
        this.client.CallbackManager.RegisterHandler(1210001, OnClientNetworking_ConnectionStateChanged);

        PageList.Add(new(this, "Store", typeof(StorePage), typeof(ViewModelBase)));
        PageList.Add(new(this, "Library", typeof(LibraryPage), typeof(LibraryPageViewModel)));
        //TODO: this isn't final, we might move downloads to the bottom still
        PageList.Add(new(this, "Downloads", typeof(DownloadsPage), typeof(DownloadsPageViewModel)));
        PageList.Add(new(this, "Community", typeof(CommunityPage), typeof(ViewModelBase)));
        PageList.Add(new(this, "Console", typeof(ConsolePage), typeof(ConsolePageViewModel)));

        SwitchToPage(typeof(LibraryPage));
        this.client.IClientAppManager.GetUpdateInfo(1281930, out AppUpdateInfo_s updateInfo);
        Console.WriteLine(updateInfo.ToString());
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(currentPage))]
    [MemberNotNull(nameof(CurrentPage))]
#pragma warning restore MVVMTK0034
    internal void SwitchToPage(Type pageType)
    {
        PageHeaderViewModel model = this.PageList.Where(item => item.PageType == pageType).First();
        var (type, page) = LoadedPages.Where(item => item.Key == model.PageType).FirstOrDefault();
        if (page == null)
        {
            page = model.PageCtor();
            page.DataContext = model.ViewModelCtor();
            LoadedPages.Add(model.PageType, page);
        }

        // Set selected button
        model.ButtonBackground = Brushes.Green;
        if (CurrentPageHeader != null)
        {
            CurrentPageHeader.ButtonBackground = AvaloniaApp.Theme!.ButtonBackground;
            if (CurrentPageHeader.IsWebPage)
            {
                CurrentPageHeader.ButtonBackground = AvaloniaApp.Theme!.AccentButtonBackground;
            }
        }

        CurrentPage = page;
        CurrentPageHeader = model;
    }

    internal void UnloadPage(Type pageType)
    {
        if (!LoadedPages.ContainsKey(pageType))
        {
            return;
        }

        PageHeaderViewModel model = this.PageList.Where(item => item.PageType == pageType).First();
        var loadedPage = LoadedPages[pageType];
        loadedPage.Free();
        loadedPage.DataContext = null;
        LoadedPages.Remove(pageType);
    }

    public void DBG_Crash()
    {
        throw new Exception("test");
    }

    private unsafe void OnCGameNetworkingUI_AppSummary(CallbackManager.CallbackHandler handler, byte[] data)
    {
        try
        {
            byte[] dataoffset = data[8..];
            var parsed = CGameNetworkingUI_AppSummary.Parser.ParseFrom(dataoffset);
            Console.WriteLine("appid: " + parsed.Appid);
            Console.WriteLine("connections: " + parsed.ActiveConnections);
            Console.WriteLine("loss: " + parsed.MainCxn.PacketLoss);
            Console.WriteLine("ping: " + parsed.MainCxn.PingMs);
        }
        catch (System.Exception e)
        {
            Logger.GetLogger("MainWindowViewModel").Error(e);
        }
    }

    private unsafe void OnClientNetworking_ConnectionStateChanged(CallbackManager.CallbackHandler handler, byte[] data)
    {
        try
        {
            byte[] dataoffset = data[4..];

            var state = CGameNetworkingUI_ConnectionState.Parser.ParseFrom(dataoffset);
            Logger.GetLogger("MainWindowViewModel").Info("AddressRemote: " + state.AddressRemote);
            Logger.GetLogger("MainWindowViewModel").Info("state: " + state.ConnectionState);
            Logger.GetLogger("MainWindowViewModel").Info("appid: " + state.Appid);
            Logger.GetLogger("MainWindowViewModel").Info("relay: " + state.SdrpopidLocal);
            Logger.GetLogger("MainWindowViewModel").Info("datacenter: " + state.SdrpopidRemote);
            Logger.GetLogger("MainWindowViewModel").Info("statustoken: " + state.StatusLocToken);
            Logger.GetLogger("MainWindowViewModel").Info("server identity: " + state.IdentityRemote);
            Logger.GetLogger("MainWindowViewModel").Info("local identity: " + state.IdentityLocal);
            Logger.GetLogger("MainWindowViewModel").Info("ping: " + state.PingDefaultInternetRoute);
            Logger.GetLogger("MainWindowViewModel").Info("connected for: " + state.E2EQualityLocal.Lifetime.ConnectedSeconds);
        }
        catch (System.Exception e)
        {
            Logger.GetLogger("MainWindowViewModel").Error(e);
        }
    }

    public void DBG_OpenInterfaceList() => AvaloniaApp.Current?.OpenInterfaceList();
    public void DBG_ChangeLanguage()
    {
        // Very simple logic, just switches between english and finnish. 
        var tm = AvaloniaApp.Container.Get<TranslationManager>();

        ELanguage lang = tm.CurrentTranslation.Language;
        Console.WriteLine(string.Format(tm.GetTranslationForKey("#SettingsWindow_YourCurrentLanguage"), tm.GetTranslationForKey("#LanguageNameTranslated"), tm.CurrentTranslation.LanguageFriendlyName));
        if (lang == ELanguage.English)
        {
            tm.SetLanguage(ELanguage.Finnish);
        }
        else
        {
            tm.SetLanguage(ELanguage.English);
        }
    }
    
    public async void DBG_TestHTMLSurface()
    {
        HTMLSurfaceTest testWnd = new();
        testWnd.Show();
        await testWnd.Init("Valve Steam Client", "https://google.com/");
    }

    public void DBG_DumpProfiler() {
        CProfiler.CurrentProfiler?.PrintStats();
    }

    public void Quit()
    {
        AvaloniaApp.Current?.ExitEventually();
    }

    public void OpenSettings()
    {
        this.openSettingsWindow?.Invoke();
    }

    public void GoOffline()
    {
        client.IClientUtils.SetOfflineMode(true);
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
    }
    public void GoOnline()
    {
        client.IClientUtils.SetOfflineMode(false);
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
    }

    public async void SignOut()
    {
        ExtendedProgress<int> progress = new(0, 100, "Logging off");
        AvaloniaApp.Current?.ForceProgressWindow(new ProgressWindowViewModel(progress, "Logging off"));
        await this.loginManager.LogoutAsync(progress, true);
    }

    public async void ChangeAccount()
    {
        await this.loginManager.LogoutAsync();
    }
    
    public void OpenFriendsList()
    {
        if (AvaloniaApp.Container.TryGet(out IClientFriends? friends)) {
            friends.OpenFriendsDialog();
        }
    }

    public void SetPersonaOnline() {
        if (AvaloniaApp.Container.TryGet(out IClientUser? user)) {
            user.SetSelfAsChatDestination(true);
        }

        if (AvaloniaApp.Container.TryGet(out IClientFriends? friends)) {
            friends.SetPersonaState(EPersonaState.Online);
        }
    }

    public void SetPersonaAway() {
        if (AvaloniaApp.Container.TryGet(out IClientFriends? friends)) {
            friends.SetPersonaState(EPersonaState.Away);
        }
    }

    public void SetPersonaInvisible() {
        if (AvaloniaApp.Container.TryGet(out IClientFriends? friends)) {
            friends.SetPersonaState(EPersonaState.Invisible);
        }
    }

    public void SetPersonaOffline() {
        if (AvaloniaApp.Container.TryGet(out IClientUser? user)) {
            user.SetSelfAsChatDestination(false);
        }

        if (AvaloniaApp.Container.TryGet(out IClientFriends? friends)) {
            friends.SetPersonaState(EPersonaState.Offline);
        }
    }
}
