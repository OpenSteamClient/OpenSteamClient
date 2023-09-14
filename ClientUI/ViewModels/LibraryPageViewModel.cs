using System.Collections.ObjectModel;

namespace ClientUI.ViewModels;

public partial class LibraryPageViewModel : ViewModelBase
{
    public ObservableCollection<AppViewModel> Apps { get; } = new();
    public LibraryPageViewModel() {

    }
}
