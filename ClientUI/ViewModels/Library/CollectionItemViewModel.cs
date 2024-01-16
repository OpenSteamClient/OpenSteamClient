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

public partial class CollectionItemViewModel : Node
{
    public string ID => collection.ID;

    private readonly Collection collection;
    private readonly OpenSteamworks.Client.Apps.Library.Library library;
    public CollectionItemViewModel(OpenSteamworks.Client.Apps.Library.Library library, Collection collection)
    {
        this.library = library;
        this.collection = collection;
        this.Name = $"{collection.Name} ({library.GetAppsInCollection(collection.ID).Count})";
        this.Icon = Brushes.Transparent;
        this.StatusIcon = Brushes.Transparent;
        this.HasIcon = false;
        this.IsApp = false;
    }
}