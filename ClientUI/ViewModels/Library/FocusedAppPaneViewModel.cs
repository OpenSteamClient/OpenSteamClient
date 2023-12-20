using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ClientUI.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels.Library;

public partial class FocusedAppPaneViewModel : ViewModelBase {
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private IBrush hero;

    [ObservableProperty]
    private IBrush logo;

    private readonly AppBase app;
    public FocusedAppPaneViewModel(CGameID gameid) {
        app = AvaloniaApp.Container.Get<AppsManager>().GetApp(gameid);
        this.Name = app.Name;
        SetLibraryAssets();

        app.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(hero))]
    [MemberNotNull(nameof(Hero))]
    [MemberNotNull(nameof(logo))]
    [MemberNotNull(nameof(Logo))]
#pragma warning restore MVVMTK0034
    private void SetLibraryAssets() {
        if (app.LocalHeroPath != null)
        {
            this.Hero = new ImageBrush()
            {
                Source = new Bitmap(app.LocalHeroPath),
            };
        } else {
            this.Hero = Brushes.DarkGray;
        }

        if (app.LocalLogoPath != null)
        {
            this.Logo = new ImageBrush()
            {
                Source = new Bitmap(app.LocalLogoPath),
            };
        } else {
            this.Logo = Brushes.Transparent;
        }
    }

    public void OnLibraryAssetsUpdated(object? sender, EventArgs e) {
        SetLibraryAssets();
    }
}