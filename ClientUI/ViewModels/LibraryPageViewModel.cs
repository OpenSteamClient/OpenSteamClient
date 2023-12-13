using System.Collections.ObjectModel;
using ClientUI.ViewModels.Library;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Apps.Library;
using OpenSteamworks.Client.Managers;

namespace ClientUI.ViewModels;

public partial class LibraryPageViewModel : ViewModelBase
{
    public ObservableCollection<INode> Nodes { get; } = new();
    public LibraryPageViewModel(AppsManager appsManager, LibraryManager libraryManager) {
        var library = libraryManager.GetLibrary();
        foreach (var collection in library.Collections)
        {
            var collectionviewmodel = this.GetOrCreateCategory(collection);
            foreach (var app in library.GetAppsInCollection(collection))
            {
                collectionviewmodel.Children.Add(new LibraryAppViewModel(app));
            }
        }
    }

    private CollectionItemViewModel GetOrCreateCategory(Collection collection) {
        foreach (var item in this.Nodes)
        {
            if (item is CollectionItemViewModel cvm) {
                if (cvm.Id == collection.ID) {
                    return cvm;
                }
            }
        }

        CollectionItemViewModel vm = new(collection.ID, collection.Name);
        this.Nodes.Add(vm);
        return vm;
    }
}
