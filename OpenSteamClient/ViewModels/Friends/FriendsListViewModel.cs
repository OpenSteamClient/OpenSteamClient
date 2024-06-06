using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using AvaloniaCommon.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamClient.UIImpl;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;

namespace OpenSteamClient.ViewModels.Friends;

public partial class FriendsListViewModel : AvaloniaCommon.ViewModelBase
{
    public ObservableCollectionEx<FriendEntityViewModel> Friends { get; init; }
    public FriendEntityViewModel CurrentUser { get; init; }

    private readonly FriendsManager friendsManager;
    public FriendsListViewModel(FriendsUI friendsUI, FriendsManager friendsManager)
    {
        this.friendsManager = friendsManager;
        this.CurrentUser = new FriendEntityViewModel(friendsUI, friendsManager, friendsManager.CurrentUser);
        this.Friends = new(this.friendsManager.GetFriendEntities().Select(e => new FriendEntityViewModel(friendsUI, friendsManager, e)));
    }

    public void SetOnline()
        => SetPersonaState(EPersonaState.Online);

    public void SetAway()
        => SetPersonaState(EPersonaState.Away);

    public void SetInvisible()
        => SetPersonaState(EPersonaState.Invisible);

    public void SetOffline()
        => SetPersonaState(EPersonaState.Offline);

    private void SetPersonaState(EPersonaState state) {
        AvaloniaApp.Container.Get<IClientFriends>().SetPersonaState(state);
    }
}
