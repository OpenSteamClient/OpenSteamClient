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

public partial class SettingsWindowViewModel : ViewModelBase
{
    private TranslationManager tm;
    private SteamClient client;
    private LoginManager loginManager;
    public SettingsWindowViewModel(SteamClient client, TranslationManager tm, LoginManager loginManager) {
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
    }
}
