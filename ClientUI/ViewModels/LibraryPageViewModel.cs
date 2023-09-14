using System.Collections.ObjectModel;

namespace ClientUI.ViewModels;

public partial class LibraryPageViewModel : ViewModelBase
{
    public ObservableCollection<SAppViewModel> Apps { get; } = new();
    public LibraryPageViewModel() {

    }
}
