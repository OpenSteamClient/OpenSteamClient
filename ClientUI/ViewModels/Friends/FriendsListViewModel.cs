using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels.Friends;

public partial class FriendsListViewModel : ViewModelBase
{
    public ObservableCollection<FriendViewModel> Friends = new();

    public FriendsListViewModel()
    {

    }
}
