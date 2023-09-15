using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels;

public partial class LibraryItemViewModel : ViewModelBase {
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private bool isCategory = false;

    [ObservableProperty]
    private string icon = "";
    public LibraryItemViewModel() {

    }
}