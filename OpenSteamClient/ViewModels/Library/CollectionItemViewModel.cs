using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OpenSteamClient.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Structs;

namespace OpenSteamClient.ViewModels;

public partial class CollectionItemViewModel : Node
{
    public string ID => collection.ID;

    private readonly Collection collection;
    public CollectionItemViewModel(OpenSteamworks.Client.Apps.Library.Library library, Collection collection)
    {
        this.collection = collection;
        this.Name = CalculateName(library, collection);
        this.Icon = Brushes.Transparent;
        this.StatusIcon = Brushes.Transparent;
        this.HasIcon = false;
        this.IsApp = false;
    }

    public static string CalculateName(OpenSteamworks.Client.Apps.Library.Library library, Collection collection) {
        return $"{collection.Name} ({library.GetAppsInCollection(collection.ID).Count})";
    }
}