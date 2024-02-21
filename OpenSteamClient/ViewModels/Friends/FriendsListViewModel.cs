using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenSteamClient.ViewModels.Friends;

public partial class FriendsListViewModel : AvaloniaCommon.ViewModelBase
{
    public ObservableCollection<FriendViewModel> Friends = new();

    public FriendsListViewModel()
    {

    }
}
