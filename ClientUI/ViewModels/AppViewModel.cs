using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels;

public partial class AppViewModel : ViewModelBase {
    [ObservableProperty]
    private string name = "";
    public AppViewModel() {

    }
}