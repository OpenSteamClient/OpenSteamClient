using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using OpenSteamClient.ViewModels.Library;
using OpenSteamClient.Views.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Client.Managers;
using AvaloniaCommon.Utils;

namespace OpenSteamClient.ViewModels;

public partial class LibraryPageViewModel : AvaloniaCommon.ViewModelBase
{
    public ObservableCollectionEx<CollectionItemViewModel> Nodes { get; init; } = new();
    public ObservableCollectionEx<Node> SelectedNodes { get; } = new();


    [ObservableProperty]
    private Control? sideContent;

    private string searchText = string.Empty;
    public string SearchText {
        get => searchText;
        set {
            searchText = value;
            UpdateGamesList();
        }
    }

    private void UpdateGamesList()
    {
        foreach (var coll in Nodes)
        {
            if (searchText == string.Empty) {
                coll.Children.ClearFilter();
            } else {
                coll.Children.FilterOriginal(f => f.GetSortableName().Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
                coll.Children.Sort();
            }
        }
    }

    private readonly OpenSteamworks.Client.Apps.Library.Library library;

    public LibraryPageViewModel(AppsManager appsManager, LibraryManager libraryManager)
    {
        library = libraryManager.GetLibrary();
        library.LibraryUpdated += OnLibraryUpdated;
        OnLibraryUpdated(this, EventArgs.Empty);

        this.SelectedNodes.CollectionChanged += SelectionChanged;
    }

    private void OnLibraryUpdated(object? sender, EventArgs e)
    {
        foreach (var collection in library.Collections)
        {
            var appids = library.GetAppsInCollection(collection.ID);
            if (appids.Count == 0) {
                // Don't show empty collections
                continue;
            }

            var collectionviewmodel = this.GetOrCreateCategory(library, collection);
            int removeCount = collectionviewmodel.Children.RemoveAll(i => !appids.Contains(i.GameID));
            Console.WriteLine("Removed " + removeCount + " apps");

            int addCount = 0;
            foreach (var app in appids)
            {
                var existingApp = collectionviewmodel.Children.Where(c => c.GameID == app).FirstOrDefault();
                if (existingApp != null)
                {
                    // Already in collection, no need to readd
                    continue;
                }

                collectionviewmodel.Children.AddUnique(new LibraryAppViewModel(this, app));
                addCount++;
            }

            collectionviewmodel.Children.Sort();
            Console.WriteLine("Added " + addCount + " apps");
        }

        // Delete collections that no longer exist
        foreach (var item in Nodes.ToList())
        {
            if (item is CollectionItemViewModel cvm)
            {
                if (library.Collections.Find(c => c.ID == cvm.ID) == null)
                {
                    Nodes.Remove(item);
                }
            }
        }
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

        var pane = new FocusedAppPane();
        pane.DataContext = AvaloniaApp.Container.ConstructOnly<FocusedAppPaneViewModel>(pane, SelectedNodes[0].GameID);
        SideContent = pane;
    }

    private CollectionItemViewModel GetOrCreateCategory(OpenSteamworks.Client.Apps.Library.Library library, Collection collection)
    {
        foreach (var item in Nodes)
        {
            if (item is CollectionItemViewModel cvm)
            {
                if (cvm.ID == collection.ID)
                {
                    return cvm;
                }
            }
        }

        CollectionItemViewModel vm = new(collection);
        Nodes.Add(vm);
        Nodes.Sort();
        
        return vm;
    }
}
