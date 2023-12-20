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

public partial class LibraryAppViewModel : ViewModelBase, INode {
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private IBrush icon;

    [ObservableProperty]
    private bool hasIcon;

    [ObservableProperty]
    private bool isApp;
    
    public ObservableCollection<INode> Children { get; } = new();
    public AppBase App { get; init; }
    public CGameID GameID { get; init; }

    public LibraryAppViewModel(LibraryPageViewModel page, AppId_t appid) {
        this.HasIcon = true;
        this.IsApp = true;
        
        App = AvaloniaApp.Container.Get<AppsManager>().GetApp(new CGameID(appid));
        this.Name = App.Name;
        this.GameID = App.GameID;

        SetLibraryAssets();
        App.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
    }



#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(icon))]
    [MemberNotNull(nameof(Icon))]
#pragma warning restore MVVMTK0034
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

    public void OnLibraryAssetsUpdated(object? sender, EventArgs e) {
        SetLibraryAssets();
    }
}