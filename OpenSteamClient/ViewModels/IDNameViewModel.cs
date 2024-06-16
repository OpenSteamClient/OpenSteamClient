using AvaloniaCommon;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenSteamClient.ViewModels;

/// <summary>
/// Simple ID/Name container
/// </summary>
public partial class IDNameViewModel : ViewModelBase {
    [ObservableProperty]
    private string iD;

    [ObservableProperty]
    private string name;

    public IDNameViewModel(string id, string name) {
        this.ID = id;
        this.Name = name;
    }
}