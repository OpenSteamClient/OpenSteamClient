using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Friends;

public interface IFriendsUI {
    public void ShowFriendsList();
    public void ShowChatUI(CSteamID steamid);
}