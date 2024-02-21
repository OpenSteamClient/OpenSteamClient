using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenSteamClient.ViewModels.Friends;

public partial class RichPresenceViewModel : AvaloniaCommon.ViewModelBase
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
