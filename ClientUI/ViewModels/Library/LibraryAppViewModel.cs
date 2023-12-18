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

public partial class LibraryAppViewModel : ViewModelBase, INode {
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private IBrush icon;
    
    public ObservableCollection<INode> Children { get; } = new();

    private AppBase app;
    public LibraryAppViewModel(AppId_t appid) {
        app = AvaloniaApp.Container.Get<AppsManager>().GetApp(new CGameID(appid));
        this.Name = app.Name;
        SetLibraryAssets();

        app.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(icon))]
    [MemberNotNull(nameof(Icon))]
#pragma warning restore MVVMTK0034
    private void SetLibraryAssets() {
        if (app.LocalIconPath != null)
        {
            this.Icon = new ImageBrush()
            {
                Source = new Bitmap(app.LocalIconPath),
            };
        } else {
            this.Icon = Brushes.DarkGray;
        }
    }

    public void OnLibraryAssetsUpdated(object? sender, EventArgs e) {
        SetLibraryAssets();
    }
}