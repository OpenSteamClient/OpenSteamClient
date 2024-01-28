using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using ClientUI.ViewModels.Library;
using ClientUI.Views.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Client.Managers;

namespace ClientUI.ViewModels;

public partial class LibraryPageViewModel : ViewModelBase
{
    public ObservableCollection<CollectionItemViewModel> Nodes { get; init; }
    public ObservableCollection<Node> SelectedNodes { get; } = new();


    [ObservableProperty]
    private Control? sideContent;

    public LibraryPageViewModel(AppsManager appsManager, LibraryManager libraryManager)
    {
        var library = libraryManager.GetLibrary();

        //TODO: this is a temp hack to sort collections properly. We (once again) need to make a proper sortable array.
        List<CollectionItemViewModel> nodes = new();
        foreach (var collection in library.Collections)
        {
            var collectionviewmodel = this.GetOrCreateCategory(ref nodes, library, collection);
            var appids = library.GetAppsInCollection(collection.ID);
            var apps = appids.Select(appid => new LibraryAppViewModel(this, appid)).ToList();
            apps.Sort();

            foreach (var app in apps)
            {
                collectionviewmodel.Children.Add(app);
            }
        }
        
        nodes.Sort();
        Nodes = new(nodes);

        this.SelectedNodes.CollectionChanged += SelectionChanged;
    }

    private void SelectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (!SelectedNodes.Any())
        {
            return;
        }

        if (!SelectedNodes[0].IsApp)
        {
            return;
        }

        SideContent = new FocusedAppPane()
        {
            DataContext = AvaloniaApp.Container.ConstructOnly<FocusedAppPaneViewModel>(SelectedNodes[0].GameID),
        };
    }

    private CollectionItemViewModel GetOrCreateCategory(ref List<CollectionItemViewModel> nodes, OpenSteamworks.Client.Apps.Library.Library library, Collection collection)
    {
        foreach (var item in nodes)
        {
            if (item is CollectionItemViewModel cvm)
            {
                if (cvm.ID == collection.ID)
                {
                    return cvm;
                }
            }
        }

        CollectionItemViewModel vm = new(library, collection);
        nodes.Add(vm);
        
        return vm;
    }
}
