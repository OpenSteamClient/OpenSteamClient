using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    private Bitmap? icon;
    
    public ObservableCollection<INode> Children { get; } = new();

    public LibraryAppViewModel(AppId_t appid) {
        var app = AvaloniaApp.Container.Get<AppsManager>().GetApp(new CGameID(appid));
        this.Name = app.Name;
        if (app.CachedIconPath != null) {
            this.Icon = new Bitmap(app.CachedIconPath);
        }
    }
}