using System;
using OpenSteamClient.Translation;
using OpenSteamClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using OpenSteamworks.ClientInterfaces;
using System.Linq;
using AvaloniaCommon;
using OpenSteamworks.Utils;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using System.ComponentModel;
using OpenSteamworks.Client.Apps.Compat;
using System.Collections.Generic;
using OpenSteamworks.Client.Config;

namespace OpenSteamClient.ViewModels;

public partial class SettingsWindowViewModel : AvaloniaCommon.ViewModelBase
{
    private readonly TranslationManager tm;
    private readonly ISteamClient client;
    private readonly LoginManager loginManager;
    private readonly SettingsWindow settingsWindow;
    private readonly ClientApps clientApps;
    private readonly CompatManager compatManager;
    private readonly ConfigManager configManager;

    public SettingsWindowViewModel(ConfigManager configManager, ClientApps clientApps, CallbackManager callbackManager, SettingsWindow settingsWindow, CompatManager compatManager, ISteamClient client, TranslationManager tm, LoginManager loginManager)
    {
        callbackManager.RegisterHandler<LibraryFoldersChanged_t>(OnLibraryFoldersChanged);
        this.configManager = configManager;
        this.compatManager = compatManager;
        this.clientApps = clientApps;
        this.settingsWindow = settingsWindow;
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
        this.PropertyChanged += SelfOnPropertyChanged;
        RefreshLibraryFolders();
        RefreshCompatTools();
        RefreshLanguages();
    }

    private void SelfOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedLibraryFolder)) {
            RefreshGamesList();
        } else if (e.PropertyName == nameof(SelectedCompatTool)) {
            SelectedCompatToolChanged();
        } else if (e.PropertyName == nameof(SelectedLanguage)) {
            SelectedLanguageChanged();
        }
    }

    // Library folders window
    private void OnLibraryFoldersChanged(CallbackManager.CallbackHandler<LibraryFoldersChanged_t> handler, LibraryFoldersChanged_t folder)
    {
        AvaloniaApp.Current?.RunOnUIThread(DispatcherPriority.Background, () => RefreshLibraryFolders());
    }


    public ObservableCollectionEx<LibraryFolderViewModel> LibraryFolders { get; } = new();
    public ObservableCollectionEx<string> AppsInCurrentLibraryFolder { get; } = new();

    [ObservableProperty]
    private LibraryFolderViewModel? selectedLibraryFolder;

    [ObservableProperty]
    private int selectedLibraryFolderIdx;

    public void LibraryFolders_OnAddClicked() {
        AvaloniaApp.Current?.RunOnUIThread(DispatcherPriority.Send, async () =>
        {
            var files = await settingsWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = tm.GetTranslationForKey("#LibraryFolders_PathToNewLibraryFolder"),
                AllowMultiple = false
            });

            if (files.Count >= 1)
            {
                var newFolder = clientApps.AddLibraryFolder(files[0].Path.AbsolutePath);
                Console.WriteLine("Got new folder " + newFolder);

                if (newFolder > 0) {
                    RefreshLibraryFolders();
                } else {
                    MessageBox.Show(tm.GetTranslationForKey("#LibraryFolders_FailedToAddNewFolderTitle"), tm.GetTranslationForKey("#LibraryFolders_FailedToAddNewFolder"));
                }
            }
        });
    }

    public void LibraryFolders_OnRemoveClicked() {
        if (SelectedLibraryFolder == null) {
            return;
        }

        if (!clientApps.RemoveLibraryFolder(SelectedLibraryFolder.ID, out uint inUseByApp)) {
            MessageBox.Show(tm.GetTranslationForKey("#LibraryFolders_FailedToRemoveFolderTitle"), string.Empty, string.Format(tm.GetTranslationForKey("#LibraryFolders_FailedToRemoveFolder"), clientApps.GetAppName(inUseByApp)), AvaloniaCommon.Enums.MessageBoxIcon.ERROR);
        }
    }

    private void RefreshLibraryFolders() {
        LibraryFolders.BlockUpdates = true;
        
        LibraryFolders.Clear();
        LibraryFolders.AddRange(LibraryFolderViewModel.GetLibraryFolders(clientApps));

        LibraryFolders.BlockUpdates = false;
        LibraryFolders.FireReset();
        

        SelectedLibraryFolder = LibraryFolders.FirstOrDefault();
        SelectedLibraryFolderIdx = 0;

        RefreshGamesList();
    }

    private void RefreshGamesList() {
        AppsInCurrentLibraryFolder.BlockUpdates = true;
        AppsInCurrentLibraryFolder.Clear();
        if (SelectedLibraryFolder != null) {
            AppsInCurrentLibraryFolder.AddRange(clientApps.GetAppsInFolder(SelectedLibraryFolder.ID).Select(appid => clientApps.GetAppName(appid)));
        }
        
        AppsInCurrentLibraryFolder.BlockUpdates = false;
        AppsInCurrentLibraryFolder.FireReset();
    }

    // Compat tools

    public ObservableCollectionEx<IDNameViewModel> CompatTools { get; } = new();

    [ObservableProperty]
    private IDNameViewModel? selectedCompatTool;

    private void SelectedCompatToolChanged() {
        if (SelectedCompatTool == null) {
            return;
        }
        
        compatManager.SpecifyCompatTool(0, SelectedCompatTool.ID, string.Empty, 250);
    }

    private void RefreshCompatTools() {
        CompatTools.Clear();
        var windowsCompatTools = compatManager.CompatToolPlatforms.Where(kv => kv.Value == ERemoteStoragePlatform.PlatformWindows).Select(kv => kv.Key);
        CompatTools.AddRange(windowsCompatTools.Select(id => new IDNameViewModel(id, compatManager.GetFriendlyNameForCompatTool(id))));

        SelectedCompatTool = CompatTools.Find(t => t.ID == compatManager.GetCompatToolForApp(0));
    }

    // Friends

    public bool AutologinToFriendsNetwork {
        get => configManager.Get<UserSettings>().LoginToFriendsNetworkAutomatically;
        set => configManager.Get<UserSettings>().LoginToFriendsNetworkAutomatically = value;
    }

    // Localization
    public ObservableCollectionEx<IDNameViewModel> Languages { get; } = new();

    [ObservableProperty]
    private IDNameViewModel? selectedLanguage;

    private void SelectedLanguageChanged() {
        if (SelectedLanguage == null) {
            return;
        }

        tm.SetLanguage(ELanguageConversion.ELanguageFromAPIName(SelectedLanguage.ID));
    }

    private void RefreshLanguages() {
        Languages.Clear();
        foreach (var _item in Enum.GetValues(typeof(ELanguage)))
        {
            var item = (ELanguage)_item;
            bool hasUITranslation = tm.HasUITranslation(item, out string? translationName);
            if (!hasUITranslation) {
                translationName = item.ToString();
            } else {
                translationName += " (UI)";
            }

            string key = ELanguageConversion.APINameFromELanguage(item);
            Languages.Add(new IDNameViewModel(key, translationName));
        }

        SelectedLanguage = Languages.Find(l => l.ID == ELanguageConversion.APINameFromELanguage(tm.CurrentTranslation.Language));
    }
}
