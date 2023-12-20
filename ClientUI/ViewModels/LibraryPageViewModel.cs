using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using ClientUI.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Client.Managers;

namespace ClientUI.ViewModels;

public partial class LibraryPageViewModel : ViewModelBase
{
    public ObservableCollection<INode> Nodes { get; } = new();
    public ObservableCollection<INode> SelectedNodes { get; } = new();


    [ObservableProperty]
    private Control? sideContent;
    
    public LibraryPageViewModel(AppsManager appsManager, LibraryManager libraryManager) {
        var library = libraryManager.GetLibrary();
        foreach (var collection in library.Collections)
        {
            var collectionviewmodel = this.GetOrCreateCategory(library, collection);
            foreach (var app in library.GetAppsInCollection(collection))
            {
                collectionviewmodel.Children.Add(new LibraryAppViewModel(this, app));
            }
        }

        this.SelectedNodes.CollectionChanged += SelectionChanged;
    }

    private void SelectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        if (!SelectedNodes.Any()) {
            return;
        }

        if (!SelectedNodes[0].IsApp) {
            return; 
        }

        SideContent = new TextBlock() {
            Text = "Content for app " + SelectedNodes[0].GameID,
        };
    }

    public void OnAppClicked(LibraryAppViewModel appvm) {
        SideContent = new TextBlock() {
            Text = "Content for app " + appvm.App.GameID,
        };
    }

    
    private CollectionItemViewModel GetOrCreateCategory(OpenSteamworks.Client.Apps.Library.Library library, Collection collection) {
        foreach (var item in this.Nodes)
        {
            if (item is CollectionItemViewModel cvm) {
                if (cvm.Id == collection.ID) {
                    return cvm;
                }
            }
        }

        CollectionItemViewModel vm = new(library, collection);
        this.Nodes.Add(vm);
        return vm;
    }
}
