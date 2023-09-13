using System;
using ClientUI.Translation;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool canLogonOffline = false;
    private TranslationManager tm;
    private SteamClient client;
    private LoginManager loginManager;
    public MainWindowViewModel(SteamClient client, TranslationManager tm, LoginManager loginManager) {
        CanLogonOffline = client.NativeClient.IClientUser.CanLogonOffline();
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
    }
    public void DBG_Crash() {
        throw new Exception("test");
    }
    public void DBG_LaunchFactorio() {
        var gameid = new CGameID(427520);
        EAppUpdateError launchresult = client.NativeClient.IClientAppManager.LaunchApp(gameid, 3, 0, "");
        MessageBox.Show("result", launchresult.ToString());
    }
    public void DBG_OpenInterfaceList() {
        App.Current?.OpenInterfaceList();
    }
    public void DBG_ChangeLanguage() {
        // Very simple logic, just switches between english and finnish. 
        Translation.TranslationManager tm = App.Container.GetComponent<Translation.TranslationManager>();

        ELanguage lang = tm.CurrentTranslation.Language;
        Console.WriteLine(string.Format(tm.GetTranslationForKey("#SettingsWindow_YourCurrentLanguage"), tm.GetTranslationForKey("#LanguageNameTranslated"), tm.CurrentTranslation.LanguageFriendlyName));
        if (lang == ELanguage.English) {
            tm.SetLanguage(ELanguage.Finnish);
        } else {
            tm.SetLanguage(ELanguage.English);
        }
    }
    public void Quit() {
        App.Current?.ExitEventually();
    }

    public void OpenSettings() {

    }

    public void GoOffline() {
        client.NativeClient.IClientUser.LogOnOffline();
    }

    public void SignOut() {
        this.loginManager.Logout(true);
    }
    public void ChangeAccount() {
        this.loginManager.Logout();
    }
}
