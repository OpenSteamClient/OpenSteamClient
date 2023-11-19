using System.Collections.ObjectModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Managers;

namespace ClientUI.ViewModels;

public partial class LibraryPageViewModel : ViewModelBase
{
    public ObservableCollection<LibraryItemViewModel> Apps { get; } = new();
    public LibraryPageViewModel(AppsManager appsManager) {
        // foreach (var app in appsManager.GetLibraryApps())
        // {
            
        // }
    }
}
