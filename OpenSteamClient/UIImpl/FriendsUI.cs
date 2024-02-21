using System.Collections.ObjectModel;
using System.Linq;
using OpenSteamClient.ViewModels.Friends;
using OpenSteamClient.Views;
using OpenSteamClient.Views.Friends;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamClient.UIImpl;

[ImplementsInterfaceAttribute<IFriendsUI>]
public partial class FriendsUI : IFriendsUI
{
    private readonly IClientFriends friends;
    private readonly FriendsListViewModel friendsListViewModel;

    public FriendsUI(IClientFriends friends)
    {
        this.friends = friends;
        this.friendsListViewModel = new();
    }

    public void ShowFriendsList()
    {
        AvaloniaApp.Current?.TryShowDialogSingle(() => new FriendsList() { DataContext = friendsListViewModel });
    }

    public void ShowChatUI(CSteamID steamid)
    {
    }

    public void UpdateFriendState(CSteamID friendID, EPersonaChange change)
    {
        var match = friendsListViewModel.Friends.Where(f => f.ID == friendID).FirstOrDefault();
        if (match != null) {
            match.UpdateState(change);
        } else {
            friendsListViewModel.Friends.Add(new FriendViewModel(friends, friendID));
        }
    }
}
