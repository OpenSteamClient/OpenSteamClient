using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.ConCommands;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels.Friends;

public partial class FriendViewModel : ViewModelBase
{
    [ObservableProperty]
    private CSteamID iD;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string nickName;

    [ObservableProperty]
    private string profilePictureHash;

    [ObservableProperty]
    private EFriendRelationship relationship;

    [ObservableProperty]
    private EFriendFlags friendFlags;

    [ObservableProperty]
    private EPersonaState personaState;

    [ObservableProperty]
    private int steamLevel;

    [ObservableProperty]
    private CGameID inGame;

    [ObservableProperty]
    private CSteamID currentLobby;

    public readonly ObservableCollection<RichPresenceViewModel> RichPresence = new();

    private readonly IClientFriends friends;

    public FriendViewModel(IClientFriends friends, CSteamID steamid)
    {
        this.friends = friends;
        CurrentLobby = CSteamID.Zero;
        ID = steamid;
        Name = "";
        NickName = "";
        ProfilePictureHash = "";
        Relationship = EFriendRelationship.None;
        FriendFlags = EFriendFlags.None;
        PersonaState = EPersonaState.Offline;
        SteamLevel = 0;
    }

    public void UpdateState(EPersonaChange change)
    {
        if (change.HasFlag(EPersonaChange.Name) || change.HasFlag(EPersonaChange.NameFirstSet)) {
            Name = friends.GetFriendPersonaName(this.ID);
        }

        if (change.HasFlag(EPersonaChange.Status) || change.HasFlag(EPersonaChange.ComeOnline) || change.HasFlag(EPersonaChange.GoneOffline)) {
            PersonaState = friends.GetFriendPersonaState(this.ID);
            if (PersonaState == EPersonaState.Offline) {
                UpdateState(EPersonaChange.GamePlayed);
            }
        }

        if (change.HasFlag(EPersonaChange.Avatar)) {
            StringBuilder str = new(256);
            if (friends.GetFriendAvatarHash(str, str.Capacity, this.ID)) {
                ProfilePictureHash = str.ToString();
            }
        }

        if (change.HasFlag(EPersonaChange.RelationshipChanged)) {
            Relationship = friends.GetFriendRelationship(this.ID);
        }

        if (change.HasFlag(EPersonaChange.Nickname)) {
            NickName = friends.GetPlayerNickname(this.ID);
        }

        if (change.HasFlag(EPersonaChange.SteamLevel)) {
            SteamLevel = friends.GetFriendSteamLevel(this.ID);
        }

        if (change.HasFlag(EPersonaChange.GameServer) || change.HasFlag(EPersonaChange.RichPresence) || change.HasFlag(EPersonaChange.GamePlayed)) {
            friends.GetFriendGamePlayed(this.ID, out FriendGameInfo_t info);
            this.InGame = info.m_gameID;
            this.CurrentLobby = info.m_steamIDLobby;
            if (InGame.IsValid()) {
                // Only get rich presence for steamapps
                if (InGame.IsSteamApp()) {
                    var count = friends.GetFriendRichPresenceKeyCount(InGame.AppID, this.ID);
                    for (int i = 0; i < count; i++)
                    {
                        string key = friends.GetFriendRichPresenceKeyByIndex(InGame.AppID, this.ID, i);
                        string value = friends.GetFriendRichPresence(InGame.AppID, this.ID, key);
                        var existingKey = RichPresence.Where(k => k.Key == key).FirstOrDefault();
                        if (existingKey != null) {
                            existingKey.Value = value;
                        } else {
                            RichPresence.Add(new RichPresenceViewModel(key, value));
                        }
                    }
                }
            } else {
                // Clear all rich presence when user exits game
                RichPresence.Clear();
            }
        }
    }
}
