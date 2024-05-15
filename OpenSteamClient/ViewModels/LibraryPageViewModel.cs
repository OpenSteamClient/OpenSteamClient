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

namespace OpenSteamClient.ViewModels;

public partial class LibraryPageViewModel : AvaloniaCommon.ViewModelBase
{
    public ObservableCollection<CollectionItemViewModel> Nodes { get; init; } = new();
    public ObservableCollection<Node> SelectedNodes { get; } = new();


    [ObservableProperty]
    private Control? sideContent;

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
            var collectionviewmodel = this.GetOrCreateCategory(library, collection);
            var appids = library.GetAppsInCollection(collection.ID);

            //TODO: Write our own observable collection to implement RemoveAll, Find, Sort, etc (this is seriously stupid, leaving lots of performance on the table here)
            //TODO: Also having a SortedInsert would be useful
            //collectionviewmodel.Children.RemoveAll()

            int removeCount = 0;
            foreach (var item in collectionviewmodel.Children.ToList())
            {
                if (!appids.Contains(item.GameID)) {
                    // Remove item if it no longer exists
                    collectionviewmodel.Children.Remove(item);
                    removeCount++;
                }
            }

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

                // This add isn't sorted, so we need to do some trickery
                //collectionviewmodel.Children.Add(new LibraryAppViewModel(this, app));

                // So do this instead for a sorted insert ;) (this is terrible)
                SortedAddInefficient(collectionviewmodel.Children, new LibraryAppViewModel(this, app));
                addCount++;
            }

            Console.WriteLine("Added " + addCount + " apps");
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
                    cvm.Name = CollectionItemViewModel.CalculateName(library, collection);
                    return cvm;
                }
            }
        }

        CollectionItemViewModel vm = new(library, collection);
        SortedAddInefficient(Nodes, vm);
        
        return vm;
    }

    private static void SortedAddInefficient<T>(ObservableCollection<T> coll, T val) {
        // Add the copy to the list, sort and find out what index it goes to
        var copy = new List<T>(coll);
        copy.Add(val);
        copy.Sort();
        coll.Insert(copy.IndexOf(val), val);
    }
}
