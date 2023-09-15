using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using ClientUI.Translation;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool showGoOffline = false;

    [ObservableProperty]
    private bool showGoOnline = false;

    [ObservableProperty]
    private Control currentPage;
    public bool CanLogonOffline => client.NativeClient.IClientUser.CanLogonOffline();
    public bool IsOfflineMode => client.NativeClient.IClientUtils.GetOfflineMode();
    private Action? openSettingsWindow;
    private TranslationManager tm;
    private SteamClient client;
    private LoginManager loginManager;
    public MainWindowViewModel(SteamClient client, TranslationManager tm, LoginManager loginManager, Action openSettingsWindowAction) {
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
        this.openSettingsWindow = openSettingsWindowAction;
        this.CurrentPage = new LibraryPage() {
            DataContext = AvaloniaApp.Container.ConstructOnly<LibraryPageViewModel>()
        };
    }
    public void DBG_Crash() {
        throw new Exception("test");
    }
    public void DBG_LaunchFactorio() {
        var gameid = new CGameID(427520);
        EAppUpdateError launchresult = client.NativeClient.IClientAppManager.LaunchApp(gameid, 3, 0, "");
        MessageBox.Show("result", launchresult.ToString());
    }
    public void DBG_OpenInterfaceList() => AvaloniaApp.Current?.OpenInterfaceList();
    public void DBG_ChangeLanguage() {
        // Very simple logic, just switches between english and finnish. 
        Translation.TranslationManager tm = AvaloniaApp.Container.GetComponent<Translation.TranslationManager>();

        ELanguage lang = tm.CurrentTranslation.Language;
        Console.WriteLine(string.Format(tm.GetTranslationForKey("#SettingsWindow_YourCurrentLanguage"), tm.GetTranslationForKey("#LanguageNameTranslated"), tm.CurrentTranslation.LanguageFriendlyName));
        if (lang == ELanguage.English) {
            tm.SetLanguage(ELanguage.Finnish);
        } else {
            tm.SetLanguage(ELanguage.English);
        }
    }
    public void DBG_TestMaps() {
        unsafe {
            var map = new CUtlMap<uint, uint>(1, 80000, &LessFunc);
            Console.WriteLine("LastPlayedMap: " + client.NativeClient.IClientUser.BGetAppsLastPlayedMap(&map));
            var asManaged = map.ToManaged();
            foreach (var item in asManaged)
            {
                Console.WriteLine(item.Key+":"+item.Value);
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static byte LessFunc(uint* first, uint* second) {
        Console.WriteLine("Steam called LessFunc");
        return Convert.ToByte(&first < &second);
    }

    public void Quit() {
        AvaloniaApp.Current?.ExitEventually();
    }

    public void OpenSettings() {
        this.openSettingsWindow?.Invoke();
    }

    public void GoOffline() {
        client.NativeClient.IClientUtils.SetOfflineMode(true);
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
    }
    public void GoOnline() {
        client.NativeClient.IClientUtils.SetOfflineMode(false);
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
    }

    public void SignOut() {
        this.loginManager.Logout(true);
    }
    public void ChangeAccount() {
        this.loginManager.Logout();
    }
}
