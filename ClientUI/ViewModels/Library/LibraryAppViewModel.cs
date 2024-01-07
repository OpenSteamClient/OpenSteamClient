using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ClientUI.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels.Library;

public partial class LibraryAppViewModel : Node {    
    public AppBase App { get; init; }

    public LibraryAppViewModel(LibraryPageViewModel page, AppId_t appid) {
        this.HasIcon = true;
        this.IsApp = true;
        
        App = AvaloniaApp.Container.Get<AppsManager>().GetApp(new CGameID(appid));
        this.Name = App.Name;
        this.GameID = App.GameID;

        SetLibraryAssets();
        SetStatusIcon();
        App.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
    }

    private void SetLibraryAssets() {
        if (App.LocalIconPath != null)
        {
            this.Icon = new ImageBrush()
            {
                Source = new Bitmap(App.LocalIconPath),
            };
        } else {
            this.Icon = Brushes.DarkGray;
        }
    }

    private void SetStatusIcon() {
        StatusIcon = Brushes.Transparent;
        if (App is SteamApp SApp) {
            
        }
    }

    public void OnLibraryAssetsUpdated(object? sender, EventArgs e) {
        SetLibraryAssets();
    }
}