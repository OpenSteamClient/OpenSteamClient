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

namespace OpenSteamClient.ViewModels.Friends;

public partial class FriendsListViewModel : AvaloniaCommon.ViewModelBase
{
    public ObservableCollectionEx<FriendEntityViewModel> Friends { get; init; }


    private readonly FriendsManager friendsManager;
    public FriendsListViewModel(FriendsUI friendsUI, FriendsManager friendsManager)
    {
        this.friendsManager = friendsManager;
        this.Friends = new(this.friendsManager.GetFriendEntities().Select(e => new FriendEntityViewModel(friendsUI, friendsManager, e)));
    }
}
