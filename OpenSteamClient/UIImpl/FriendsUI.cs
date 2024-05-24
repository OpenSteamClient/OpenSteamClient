using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenSteamClient.ViewModels.Friends;
using OpenSteamClient.Views;
using OpenSteamClient.Views.Friends;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamClient.UIImpl;

[DIRegisterInterfaceAttribute<IFriendsUI>]
public partial class FriendsUI : IFriendsUI
{
    private readonly FriendsListViewModel friendsListViewModel;
    private readonly FriendsManager friendsManager;
    public Logger Logger { get; init; }

    public FriendsUI(FriendsManager friendsManager, InstallManager im)
    {
        this.Logger = Logger.GetLogger("FriendsUI", im.GetLogPath("FriendsUI"));
        this.friendsManager = friendsManager;
        friendsManager.EntityChanged += OnEntityChanged;
        this.friendsListViewModel = new(this, friendsManager);
    }

    private void OnEntityChanged(object? sender, Tuple<FriendsManager.Entity, EPersonaChange> e)
    {
        if (e.Item1.SteamID == friendsManager.CurrentUser.SteamID) {
            foreach (var item in friendsListViewModel.Friends)
            {
                item.UpdateSelfCanInvite(friendsManager.CurrentUser.InGame);
            }   
        }

        var match = friendsListViewModel.Friends.Where(f => f.ID == e.Item1.SteamID).FirstOrDefault();
        if (match != null) {
            match.UpdateState(e.Item2);
        } else {
            if (!friendsManager.IsFriendsWith(e.Item1.SteamID)) {
                return;
            }

            this.Logger.Info("Failed to find friend " + e.Item1.SteamID + "; creating");
            friendsListViewModel.Friends.Add(new FriendEntityViewModel(this, friendsManager, e.Item1));
        }
    }

    public void ShowFriendsList()
    {
        AvaloniaApp.Current?.TryShowDialogSingle(() => new FriendsList() { DataContext = friendsListViewModel });
    }

    public void ShowChatUI(CSteamID steamid)
    {
        this.Logger.Error("Chat UI unimplemented");
    }
}
