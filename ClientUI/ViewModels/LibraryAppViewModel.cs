using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClientUI.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;

namespace ClientUI.ViewModels;

public partial class LibraryAppViewModel : ViewModelBase, INode {
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string icon = "";
    public ObservableCollection<INode> Children { get; } = new();

    public LibraryAppViewModel(AppId_t appid) {
        var steamapp = AvaloniaApp.Container.Get<AppsManager>().GetSteamAppSync(appid);
        this.Name = steamapp.Common.Name;
    }
}