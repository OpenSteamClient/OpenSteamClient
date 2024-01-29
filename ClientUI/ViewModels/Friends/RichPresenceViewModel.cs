using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels.Friends;

public partial class RichPresenceViewModel : ViewModelBase
{
    [ObservableProperty]
    private string key;

    [ObservableProperty]
    private string value;

    public RichPresenceViewModel(string key, string value)
    {
        this.Key = key;
        this.Value = value;
    }
}
