using System.Text;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Structs;
using SkiaSharp;

namespace OpenSteamworks.Client.Friends;

public class FriendsManager : ILogonLifetime
{
    public class Entity {
        public CSteamID SteamID { get; init; }
        public bool IsCurrentUser { get; init; }
        public string Name => IsCurrentUser ? mgr.friends.GetPersonaName() : mgr.friends.GetFriendPersonaName(SteamID);
        public string Nickname => IsCurrentUser ? string.Empty : mgr.friends.GetPlayerNickname(SteamID);
        public EPersonaState PersonaState => IsCurrentUser ? mgr.friends.GetPersonaState() : mgr.friends.GetFriendPersonaState(SteamID);
        public EPersonaStateFlag PersonaStateFlag => IsCurrentUser ? 0 : mgr.friends.GetFriendPersonaStateFlags(SteamID);
        public EFriendRelationship Relationship => IsCurrentUser ? EFriendRelationship.Friend : mgr.friends.GetFriendRelationship(SteamID);
        public string AvatarHash {
            get {
                StringBuilder builder = new(1024);
                if (!mgr.friends.GetFriendAvatarHash(builder, builder.Capacity, SteamID)) {
                    return string.Empty;
                }

                return builder.ToString();
            }
        }
        
        public bool HasAvatar {
            get {
                var ret = mgr.friends.GetLargeFriendAvatar(SteamID);

                // The function can return -1 if the avatar is being retrieved.
                // It will return 0 if the avatar doesn't exist.
                return ret > 0 && !(ret < 0);
            }
        }
        
        public KVObject RichPresence {
            get {
                var obj = new KVObject("RP", new List<KVObject>());
                if (InGame.IsSteamApp()) {
                    var appid = InGame.AppID;
                    int keyCount = mgr.friends.GetFriendRichPresenceKeyCount(appid, SteamID);
                    for (int i = 0; i < keyCount; i++)
                    {
                        var key = mgr.friends.GetFriendRichPresenceKeyByIndex(appid, SteamID, i);
                        var val = mgr.friends.GetFriendRichPresence(appid, SteamID, key);
                        obj[key] = new KVObject(key, val);
                    }
                }
                
                return obj;
            }
        }

        public FriendGameInfo_t FriendGameInfo {
            get {
                mgr.friends.GetFriendGamePlayed(SteamID, out FriendGameInfo_t gamePlayed);
                return gamePlayed;
            }
        }
        
        public CGameID InGame => FriendGameInfo.m_gameID;
        public HImage SmallAvatar => mgr.friends.GetSmallFriendAvatar(SteamID);
        public HImage MediumAvatar => mgr.friends.GetMediumFriendAvatar(SteamID);
        public HImage LargeAvatar {
            get {
                var ret = mgr.friends.GetLargeFriendAvatar(SteamID);

                // Never return -1 from this func.
                return ret < 0 ? 0 : ret;
            }
        }

        public CSteamID LobbyID => FriendGameInfo.m_steamIDLobby;
        public uint GameIP => FriendGameInfo.m_unGameIP;
        public ushort GamePort => FriendGameInfo.m_usGamePort;
        public ushort GameQueryPort => FriendGameInfo.m_usQueryPort;
        public DateTime LastLogonTime => (DateTime)mgr.friends.GetFriendLastLogoffTime(SteamID);
        public DateTime LastLogoffTime => (DateTime)mgr.friends.GetFriendLastLogoffTime(SteamID);

        private readonly FriendsManager mgr;
        public Entity(FriendsManager mgr, CSteamID steamid, bool currentUser = false) {
            this.mgr = mgr;
            this.SteamID = steamid;
            this.IsCurrentUser = currentUser;
        }

        public unsafe byte[]? GetSmallAvatar(out uint width, out uint height)
            => mgr.GetImageBytes(SmallAvatar, out width, out height);

        public unsafe byte[]? GetMediumAvatar(out uint width, out uint height)
            => mgr.GetImageBytes(MediumAvatar, out width, out height);

        public unsafe byte[]? GetLargeAvatar(out uint width, out uint height)
            => mgr.GetImageBytes(LargeAvatar, out width, out height);

        public bool JoinGame(CGameID gameid)
        {
            if (!gameid.IsSteamApp()) {
                return false;
            }

            var rp = RichPresence;
            if (rp.TryGetChild("connect", out KVObject? connectDataRP)) {
                if (mgr.friends.NotifyRichPresenceJoinRequested(gameid.AppID, SteamID, connectDataRP.GetValueAsString()))
                {
                    return true;
                }
            }

            return mgr.friends.NotifyLobbyJoinRequested(gameid.AppID, SteamID, LobbyID);
        }
    }


    public Entity CurrentUser => GetEntity(user.GetSteamID());
    public Entity GetEntity(CSteamID steamid) {
        return new(this, steamid, false);
    }

    private readonly IClientUser user;
    private readonly IClientFriends friends;
    private readonly IClientUtils utils;
    private readonly UserSettings userSettings;
    private readonly Container container;
    private IFriendsUI? FriendsUI => container.GetNullable<IFriendsUI>();

    public event EventHandler<Tuple<Entity, EPersonaChange>>? EntityChanged;

    public IEnumerable<CSteamID> Friends {
        get {
            List<CSteamID> friendIDs = new();
            var countFriends = friends.GetFriendCount();
            for (int i = 0; i < countFriends; i++)
            {
                friendIDs.Add(friends.GetFriendByIndex(i));
            }

            friendIDs.Remove(user.GetSteamID());
            return friendIDs;
        }
    }

    public unsafe byte[]? GetImageBytes(HImage handle, out uint width, out uint height) {
        width = 0;
        height = 0;
        if (handle == 0) {
            return null;
        }

        if (!utils.GetImageSize(handle, out width, out height)) {
            return null;
        }

        byte[] buf = new byte[width * height * 4];
        fixed (byte* ptr = buf) {
            if (!utils.GetImageRGBA(handle, ptr, buf.Length)) {
                return null;
            }
        }

        return buf;
    }

    public FriendsManager(Container container, ISteamClient client, UserSettings userSettings) {
        this.container = container;
        this.user = client.IClientUser;
        this.friends = client.IClientFriends;
        this.utils = client.IClientUtils;
        this.userSettings = userSettings;
        client.CallbackManager.RegisterHandler<OpenFriendsDialog_t>(OnOpenFriendsDialog);
        client.CallbackManager.RegisterHandler<OpenChatDialog_t>(OnOpenChatDialog);
        client.CallbackManager.RegisterHandler<PersonaStateChange_t>(OnPersonaStateChange);
    }

    public IEnumerable<Entity> GetFriendEntities()
        => Friends.Select(GetEntity);


    public bool IsFriendsWith(CSteamID steamid) => friends.HasFriend(steamid, EFriendFlags.Immediate);

    private void OnPersonaStateChange(CallbackManager.CallbackHandler<PersonaStateChange_t> handler, PersonaStateChange_t t)
    {
        EntityChanged?.Invoke(this, new(GetEntity(t.steamid), t.changeFlags));
    }

    private void OnOpenChatDialog(CallbackManager.CallbackHandler<OpenChatDialog_t> handler, OpenChatDialog_t t)
    {
        FriendsUI?.ShowChatUI(t.ChatID);
    }

    private void OnOpenFriendsDialog(CallbackManager.CallbackHandler<OpenFriendsDialog_t> handler, OpenFriendsDialog_t t)
    {
        FriendsUI?.ShowFriendsList();
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        if (userSettings.LoginToFriendsNetworkAutomatically) {
            this.user.SetSelfAsChatDestination(true);
        }

        await Task.CompletedTask;
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress)
    {
        await Task.CompletedTask;
    }
}