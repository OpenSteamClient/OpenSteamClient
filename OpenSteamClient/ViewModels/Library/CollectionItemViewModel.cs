using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OpenSteamClient.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Structs;

namespace OpenSteamClient.ViewModels.Library;

public partial class CollectionItemViewModel : Node
{
    protected override string ActualName => $"{collection.Name} ({Children.Count})";
    public string ID => collection.ID;

    private readonly Collection collection;
    public CollectionItemViewModel(Collection collection)
    {
        this.collection = collection;
        this.Icon = Brushes.Transparent;
        this.StatusIcon = Brushes.Transparent;
        this.HasIcon = false;
        this.IsApp = false;
    }
}