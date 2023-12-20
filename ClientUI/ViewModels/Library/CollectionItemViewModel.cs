using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ClientUI.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels;

public partial class CollectionItemViewModel : ViewModelBase, INode {
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string id = "";

    [ObservableProperty]
    private IBrush icon;

    [ObservableProperty]
    private bool hasIcon;

    [ObservableProperty]
    private bool isApp;
    
    public CGameID GameID => CGameID.Zero;


    private readonly Collection collection;
    private readonly OpenSteamworks.Client.Apps.Library.Library library;

    public ObservableCollection<INode> Children { get; } = new();
    public CollectionItemViewModel(OpenSteamworks.Client.Apps.Library.Library library, Collection collection) {
        this.library = library;
        this.collection = collection;
        this.Id = collection.ID;
        this.Name = $"{collection.Name} ({library.GetAppsInCollection(collection).Count})";
        this.Icon = Brushes.Transparent;
        this.HasIcon = false;
        this.IsApp = false;
    }
}