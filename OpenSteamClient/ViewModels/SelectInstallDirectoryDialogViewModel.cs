using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using OpenSteamClient.Extensions;
using OpenSteamClient.Translation;
using OpenSteamClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.ClientInterfaces;

namespace OpenSteamClient.ViewModels;

public partial class SelectInstallDirectoryDialogViewModel : AvaloniaCommon.ViewModelBase {
    public ObservableCollection<LibraryFolderViewModel> LibraryFolders { get; init; }

    [ObservableProperty]
    private LibraryFolderViewModel? selectedLibraryFolder;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? textBlockText;

    private readonly SteamApp app;
    private readonly SelectInstallDirectoryDialog dialog;
    private readonly ClientApps clientApps;

    public SelectInstallDirectoryDialogViewModel(ClientApps clientApps, TranslationManager tm, SelectInstallDirectoryDialog dialog, SteamApp app) {
        this.clientApps = clientApps;
        this.app = app;
        this.dialog = dialog;
        Title = string.Format(tm.GetTranslationForKey("#SelectInstallDirectoryDialog_Title"), app.Name);
        TextBlockText = string.Format(tm.GetTranslationForKey("#SelectInstallDirectoryDialog_SelectLibraryFolder"), app.Name);

        LibraryFolders = new(LibraryFolderViewModel.GetLibraryFolders(clientApps));
    }

    public void OnCancelClicked() {
        dialog.Close();
    }
    
    public void OnInstallClicked() {
        Console.WriteLine("Installing " + app.Name + " to " + SelectedLibraryFolder!.Path);
        clientApps.InstallApp(app.AppID, SelectedLibraryFolder!.ID);
        dialog.Close();
    }
}