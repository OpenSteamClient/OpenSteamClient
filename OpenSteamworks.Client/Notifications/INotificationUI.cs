using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Notifications;

public interface INotificationUI {
    public void ShowPlayingGame(CSteamID steamIDUser, CGameID gameID);
    public void ShowChatMessage(CSteamID steamIDUser, string chatMessage);
    public void ShowGoneOnline(CSteamID steamIDUser);
    public void ShowFriendRequest(CSteamID steamIDUser);
    
}