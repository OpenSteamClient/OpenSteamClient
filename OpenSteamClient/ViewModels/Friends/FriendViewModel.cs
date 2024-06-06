using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamClient.Translation;
using OpenSteamClient.UIImpl;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.ConCommands;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

namespace OpenSteamClient.ViewModels.Friends;

public partial class FriendEntityViewModel : AvaloniaCommon.ViewModelBase
{
    private class MappedContextMenuItems {
        public CGameID ForGame;
        public List<MenuItemViewModel> Items = new();
    }

    public Bitmap? SmallAvatar {
        get {
            var data = Entity.GetSmallAvatar(out uint width, out uint height);
            if (data == null) {
                return null;
            }

            return AvatarToAvaloniaBitmap(data, width, height);
        }
    }
    
    public Bitmap? MediumAvatar {
        get {
            var data = Entity.GetMediumAvatar(out uint width, out uint height);
            if (data == null) {
                return null;
            }

            return AvatarToAvaloniaBitmap(data, width, height);
        }
    }

    public Bitmap? LargeAvatar {
        get {
            var data = Entity.GetLargeAvatar(out uint width, out uint height);
            if (data == null) {
                return null;
            }

            return AvatarToAvaloniaBitmap(data, width, height);
        }
    }

    private Bitmap AvatarToAvaloniaBitmap(byte[] data, uint width, uint height) {
        unsafe {
            fixed (byte* ptr = data) {
                return new Bitmap(PixelFormat.Rgba8888, AlphaFormat.Premul, (nint)ptr, PixelSize.FromSize(new Size(width, height), 1.0), Vector.One * 96, (int)(width * 4));
            }
        }
    }


    public string DisplayName {
        get {
            if (!string.IsNullOrEmpty(Entity.Nickname)) {
                return Entity.Nickname;
            }

            return Entity.Name;
        }
    }

    public string PersonaStatus {
        get {
            var tm = AvaloniaApp.Container.Get<TranslationManager>();

            if (Entity.InGame.IsValid()) {
                try
                {
                    var appsManager = AvaloniaApp.Container.Get<AppsManager>();
                    return appsManager.GetApp(Entity.InGame).Name;
                }
                catch (System.Exception)
                {
                    return "In Game - Unknown";
                }
            }

            return Entity.PersonaState switch
            {
                EPersonaState.Online => tm.GetTranslationForKey("#FriendsUI_PersonaState_Online"),
                EPersonaState.Snooze or 
                EPersonaState.Away => tm.GetTranslationForKey("#FriendsUI_PersonaState_Away"),
                EPersonaState.Offline => string.Format(tm.GetTranslationForKey("#FriendsUI_LastOnline"), LastOnlineFormat(Entity.LastLogoffTime)),
                EPersonaState.Invisible => tm.GetTranslationForKey("#FriendsUI_PersonaState_Invisible"),
                _ => "Unknown",
            };
        }
    }

    private IntervalFunc? updateLastOnlineTimer;
    private void UpdateLastOnlineTimer() {
        var offset = DateTime.UtcNow - Entity.LastLogoffTime;
        TimeSpan updateSpan = TimeSpan.FromMinutes(30);
        if (offset.TotalMinutes < 60) {
            updateSpan = TimeSpan.FromSeconds(1);
        }

        updateLastOnlineTimer?.Stop();
        updateLastOnlineTimer = new(UpdateLastOnline, updateSpan);
    }

    private void UpdateLastOnline() {
        OnPropertyChanged(nameof(PersonaStatus));
    }

    private static string LastOnlineFormat(DateTime logoffTime) {
        var offset = DateTime.UtcNow - logoffTime;
        var tm = AvaloniaApp.Container.Get<TranslationManager>();
        string loc_year = tm.GetTranslationForKey("#TimeDate_Year");
        string loc_years = tm.GetTranslationForKey("#TimeDate_Years");

        string loc_month = tm.GetTranslationForKey("#TimeDate_Month");
        string loc_months = tm.GetTranslationForKey("#TimeDate_Months");

        string loc_week = tm.GetTranslationForKey("#TimeDate_Week");
        string loc_weeks = tm.GetTranslationForKey("#TimeDate_Weeks");

        string loc_day = tm.GetTranslationForKey("#TimeDate_Day");
        string loc_days = tm.GetTranslationForKey("#TimeDate_Days");

        string loc_hour = tm.GetTranslationForKey("#TimeDate_Hour");
        string loc_hours = tm.GetTranslationForKey("#TimeDate_Hours");

        string loc_minute = tm.GetTranslationForKey("#TimeDate_Minute");
        string loc_minutes = tm.GetTranslationForKey("#TimeDate_Minutes");

        string loc_second = tm.GetTranslationForKey("#TimeDate_Second");
        string loc_seconds = tm.GetTranslationForKey("#TimeDate_Seconds");

        string loc_and = tm.GetTranslationForKey("#TimeDate_And");

        if (offset.Days > (31 * 12)) {
            string yearOrYears = offset.Days < (31 * 24) ? loc_year : loc_years;
            string monthOrMonths = offset.Days < 31 * 2 ? loc_month : loc_months;
            return $"{offset.Days / (31 * 12)} {yearOrYears} {loc_and} {offset.Days % (31 * 12)} {monthOrMonths}";
        } else if (offset.Days > 31) {
            string monthOrMonths = offset.Days < 31 * 2 ? loc_month : loc_months;
            string dayOrDays = offset.Days == 1 ? loc_day : loc_days;
            return $"{offset.Days / 31} {monthOrMonths} {loc_and} {offset.Days % 31} {dayOrDays}";
        } else if (offset.Days > 7) {
            string weekOrWeeks = offset.Days < 14 ? loc_week : loc_weeks;
            string dayOrDays = offset.Days % 7 == 1 ? loc_day : loc_days;
            if (offset.Days % 7 < 1) {
                return $"{offset.Days / 7} {weekOrWeeks}";
            }

            return $"{offset.Days / 7} {weekOrWeeks} {loc_and} {offset.Days % 7} {dayOrDays}";
        } else if (offset.Days > 0) {
            string dayOrDays = offset.Days == 1 ? loc_day : loc_days;
            string hourOrHours = offset.Hours == 1 ? loc_hour : loc_hours;
            return $"{offset.Days} {dayOrDays} {loc_and} {offset.Hours} {hourOrHours}";
        } else if (offset.Hours > 0) {
            string hourOrHours = offset.Hours == 1 ? loc_hour : loc_hours;
            string minuteOrMinutes = offset.Minutes == 1 ? loc_minute : loc_minutes;
            return $"{offset.Hours} {hourOrHours} {loc_and} {offset.Minutes} {minuteOrMinutes}";
        } else if (offset.Minutes > 0) {
            string minuteOrMinutes = offset.Minutes == 1 ? loc_minute : loc_minutes;
            string secondOrSeconds = offset.Seconds == 1 ? loc_second : loc_seconds;
            return $"{offset.Minutes} {minuteOrMinutes} {loc_and} {offset.Seconds} {secondOrSeconds}";
        } else {
            return logoffTime.ToShortDateString();
        }
    }

    // This is a bit confusing, but yes it belongs here.
    //TODO: Determine invitability
    public bool SelfCanInvite => mgr.CurrentUser.InGame.IsValid();
    public string SelfCanInviteDbg => ((ulong)mgr.CurrentUser.InGame).ToString();
    public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; init; } = new();
    private readonly MappedContextMenuItems gameContextMenuItemsSelf = new();
    private readonly MappedContextMenuItems gameContextMenuItemsCurrentUser = new();

    public CSteamID ID => Entity.SteamID;
    public FriendsManager.Entity Entity { get; init; }
    private readonly FriendsManager mgr;
    private readonly FriendsUI friendsUI;
    private readonly MenuItemViewModel addToFavourites;

    public void HandlePointerPressed(object sender, RoutedEventArgs e) {
        Console.WriteLine("OnPointerPressed");
    }
    
    public FriendEntityViewModel(FriendsUI friendsUI, FriendsManager mgr, FriendsManager.Entity ent)
    {
        this.friendsUI = friendsUI;
        this.mgr = mgr;
        this.Entity = ent;

        AvaloniaApp.Container.Get<TranslationManager>().TranslationChanged += OnTranslationChanged;

        // Init context menu items
        ContextMenuItems.Add(new(string.Empty, "#FriendsUI_Call", Call));
        ContextMenuItems.Add(new(string.Empty, "#FriendsUI_ViewProfile", ViewProfile));

        MenuItemViewModel tradingSubMenu = new(string.Empty, "#FriendsUI_Trading_SubMenu");
        tradingSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_ViewInventory", ViewInventory));
        tradingSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_SendTradeOffer", SendTradeOffer));
        ContextMenuItems.Add(tradingSubMenu);

        MenuItemViewModel manageSubMenu = new(string.Empty, "#FriendsUI_Manage_SubMenu");
        manageSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_ChangeNickName", ChangeNickname));

        addToFavourites = new(string.Empty, "#FriendsUI_AddToFavourites", AddToFavourites);
        manageSubMenu.SubItems.Add(addToFavourites);

        manageSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_Categorize", Categorize));
        manageSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_Notifications", NotificationsSettings));
        manageSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_RemoveFromFriends", RemoveFromFriends));
        manageSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_RecentNames"));
        manageSubMenu.SubItems.Add(new(string.Empty, "#FriendsUI_Block", Block));
        ContextMenuItems.Add(manageSubMenu);

        UpdateContextMenu();

        UpdateLastOnlineTimer();
    }

    private void OnTranslationChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(PersonaStatus));
    }

    public void UpdateContextMenu() {
        List<MenuItemViewModel> toRemove = new();
        List<MenuItemViewModel> toAdd = new();

        if (Entity.InGame != gameContextMenuItemsSelf.ForGame) {
            toRemove.AddRange(gameContextMenuItemsSelf.Items);
        }

        if (mgr.CurrentUser.InGame != gameContextMenuItemsCurrentUser.ForGame) {
            toRemove.AddRange(gameContextMenuItemsCurrentUser.Items);
        }

        gameContextMenuItemsSelf.ForGame = Entity.InGame;
        gameContextMenuItemsCurrentUser.ForGame = mgr.CurrentUser.InGame;

        List<CGameID> games = new();

        // Add our current game only if we're not in the same game
        if (Entity.InGame != mgr.CurrentUser.InGame) {
            if (mgr.CurrentUser.InGame.IsValid()) {
                games.Add(mgr.CurrentUser.InGame);
            }
        }
        
        if (Entity.InGame.IsValid()) {
            games.Add(Entity.InGame);
        }

        foreach (var game in games)
        {
            //TODO: More sophisticated logic here
            bool canInvite = game == mgr.CurrentUser.InGame;
            bool canJoin = game == Entity.InGame;

            // "#FriendsUI_InviteToGame": "Invite to game",
            // "#FriendsUI_InviteToWatch": "Invite to watch",
            // "#FriendsUI_JoinGame": "Join game",
            // "#FriendsUI_WatchGame": "Watch game",
            // "#FriendsUI_StorePage": "Store page",
            // "#FriendsUI_CommunityHub": "Community hub",

            try
            {
                var app = AvaloniaApp.Container.Get<AppsManager>().GetApp(game);
                toAdd.Add(new(app.Name, string.Empty));
            }
            catch (System.Exception e)
            {
                friendsUI.Logger.Error($"Error getting app {game} friend {Entity.SteamID} is playing: ");
                friendsUI.Logger.Error(e);
                toAdd.Add(new("Unknown Game", string.Empty));
            }

            toAdd.Add(new(string.Empty, "#FriendsUI_InviteToGame", () => InviteToGame(game)));
            toAdd.Add(new(string.Empty, "#FriendsUI_InviteToWatch", () => InviteToWatch(game)));
            toAdd.Add(new(string.Empty, "#FriendsUI_JoinGame", () => JoinGame(game)));
            toAdd.Add(new(string.Empty, "#FriendsUI_WatchGame", () => WatchGame(game)));
            toAdd.Add(new(string.Empty, "#FriendsUI_StorePage", () => OpenStorePage(game)));
            toAdd.Add(new(string.Empty, "#FriendsUI_CommunityHub", () => OpenCommunityHub(game)));
        }

        foreach (var item in toRemove)
        {
            ContextMenuItems.Remove(item);
        }

        foreach (var item in toAdd)
        {
            ContextMenuItems.Add(item);
        }
    }

    private void OpenCommunityHub(CGameID gameid)
    {
        //TODO: Web browser
        friendsUI.Logger.Error("Community page not implemented!");
    }

    private void OpenStorePage(CGameID gameid)
    {
        //TODO: Web browser
        friendsUI.Logger.Error("Store page not implemented!");
    }

    private void WatchGame(CGameID gameid)
    {
        //TODO
        friendsUI.Logger.Error("Game streaming not implemented!");
    }

    private void JoinGame(CGameID gameid)
    {
        if (!Entity.JoinGame(gameid)) {
            friendsUI.Logger.Error("Lobby join failed");
        }
    }

    private void InviteToWatch(CGameID gameid)
    {
        //TODO
        friendsUI.Logger.Error("Game streaming not implemented!");
    }

    private void Call()
    {
        //TODO: Voice calling
        friendsUI.Logger.Error("Voice call not implemented!");

        //NOTE: This does not do what you'd expect. Test it out with a friend (preferably with them being on Windows)
        // Just tell them to lower their system volume first...
        // And no, VoiceCallNew is not any better. In fact, they do the exact same thing
        // var friends = AvaloniaApp.Container.Get<IClientFriends>();
        // friends.VoiceCall(mgr.CurrentUser.SteamID, Entity.SteamID);
    }

    private void ViewProfile()
    {
        //TODO: Web browser
        friendsUI.Logger.Error("Profile page not implemented!");
    }

    private void ViewInventory()
    {
        //TODO: Web browser
        friendsUI.Logger.Error("View inventory not implemented!");
    }

    private void SendTradeOffer()
    {
        //TODO: Web browser
        friendsUI.Logger.Error("Send trade offer not implemented!");
    }

    private void ChangeNickname()
    {
        //TODO: Change nickname dialog
        friendsUI.Logger.Error("Change nickname not implemented!");
    }

    public void AddToFavourites()
    {
        //TODO: Favourites system
        friendsUI.Logger.Error("Add to favourites offer not implemented!");
    }

    private void Categorize()
    {
        //TODO: Categories
        friendsUI.Logger.Error("Categorize not implemented!");
    }

    private void NotificationsSettings()
    {
        //TODO: Notification settings
        friendsUI.Logger.Error("Notification settings not implemented!");
    }

    private void RemoveFromFriends()
    {
        //TODO: Confirmation dialog
        AvaloniaApp.Container.Get<IClientFriends>().RemoveFriend(Entity.SteamID);
    }

    private void Block() {
        //TODO: Confirmation dialog
        AvaloniaApp.Container.Get<IClientFriends>().SetIgnoreFriend(Entity.SteamID, true);
    }

    private void InviteToGame(CGameID gameid) {
        if (gameid.IsValid() && gameid.IsSteamApp()) {
            AvaloniaApp.Container.Get<IClientFriends>().InviteUserToGame(gameid.AppID, Entity.SteamID, string.Empty);
        }
    }

    public void UpdateSelfCanInvite(CGameID inGame)
    {
        Console.WriteLine("SelfCanInvite changed to " + SelfCanInvite + ", now in game: " + inGame);
        OnPropertyChanged(nameof(SelfCanInvite));
    }

    public void UpdateState(EPersonaChange change)
    {
        if (change.HasFlag(EPersonaChange.Name) || change.HasFlag(EPersonaChange.NameFirstSet) || change.HasFlag(EPersonaChange.Nickname)) {
            OnPropertyChanged(nameof(DisplayName));
        }

        if (change.HasFlag(EPersonaChange.Status) || change.HasFlag(EPersonaChange.ComeOnline) || change.HasFlag(EPersonaChange.GoneOffline) || change.HasFlag(EPersonaChange.GamePlayed)) {
            OnPropertyChanged(nameof(PersonaStatus));
            UpdateLastOnlineTimer();
        }

        if (change.HasFlag(EPersonaChange.GamePlayed) || change.HasFlag(EPersonaChange.GameServer) || change.HasFlag(EPersonaChange.RichPresence)) {
            UpdateContextMenu();
        }
    
        if (change.HasFlag(EPersonaChange.Avatar)) {
            OnPropertyChanged(nameof(SmallAvatar));
            OnPropertyChanged(nameof(MediumAvatar));
            OnPropertyChanged(nameof(LargeAvatar));
        }
    }
}
