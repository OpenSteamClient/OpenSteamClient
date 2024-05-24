using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamClient.UIImpl;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.ConCommands;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamClient.ViewModels.Friends;

public partial class FriendEntityViewModel : AvaloniaCommon.ViewModelBase
{
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
                EPersonaState.Online => "Online",
                EPersonaState.Snooze or 
                EPersonaState.Away => "Away",
                EPersonaState.Offline => "Last online " + LastOnlineFormat(Entity.LastLogoffTime) + " ago",
                EPersonaState.Invisible => "Invisible",
                _ => "Unknown",
            };
        }
    }

    private static string LastOnlineFormat(DateTime logoffTime) {
        var offset = DateTime.UtcNow - logoffTime;
        
        if (offset.Days > (31 * 12)) {
            string yearOrYears = offset.Days < (31 * 24) ? "year" : "years";
            string monthOrMonths = offset.Days < 31 * 2 ? "month" : "months";
            return $"{offset.Days / (31 * 12)} {yearOrYears} and {offset.Days % (31 * 12)} {monthOrMonths}";
        } else if (offset.Days > 31) {
            string monthOrMonths = offset.Days < 31 * 2 ? "month" : "months";
            string dayOrDays = offset.Days == 1 ? "day" : "days";
            return $"{offset.Days / 31} {monthOrMonths} and {offset.Days % 31} {dayOrDays}";
        } else if (offset.Days > 7) {
            string weekOrWeeks = offset.Days < 14 ? "week" : "weeks";
            string dayOrDays = offset.Days % 7 == 1 ? "day" : "days";
            if (offset.Days % 7 < 1) {
                return $"{offset.Days / 7} {weekOrWeeks}";
            }

            return $"{offset.Days / 7} {weekOrWeeks} and {offset.Days % 7} {dayOrDays}";
        } else if (offset.Days > 0) {
            string dayOrDays = offset.Days == 1 ? "day" : "days";
            string hourOrHours = offset.Hours == 1 ? "hour" : "hours";
            return $"{offset.Days} {dayOrDays} and {offset.Hours} {hourOrHours}";
        } else if (offset.Hours > 0) {
            string hourOrHours = offset.Hours == 1 ? "hour" : "hours";
            string minuteOrMinutes = offset.Minutes == 1 ? "minute" : "minutes";
            return $"{offset.Hours} {hourOrHours} and {offset.Minutes} {minuteOrMinutes}";
        } else if (offset.Minutes > 0) {
            string minuteOrMinutes = offset.Minutes == 1 ? "minute" : "minutes";
            string secondOrSeconds = offset.Seconds == 1 ? "second" : "seconds";
            return $"{offset.Minutes} {minuteOrMinutes} and {offset.Seconds} {secondOrSeconds}";
        } else {
            return logoffTime.ToShortDateString();
        }
    }

    // This is a bit confusing, but yes it belongs here.
    //TODO: Determine invitability
    public bool SelfCanInvite => mgr.CurrentUser.InGame.IsValid();
    public string SelfCanInviteDbg => ((ulong)mgr.CurrentUser.InGame).ToString();
    

    public CSteamID ID => Entity.SteamID;
    public FriendsManager.Entity Entity { get; init; }
    private readonly FriendsManager mgr;
    private readonly FriendsUI friendsUI;

    public FriendEntityViewModel(FriendsUI friendsUI, FriendsManager mgr, FriendsManager.Entity ent)
    {
        this.friendsUI = friendsUI;
        this.mgr = mgr;
        this.Entity = ent;
    }

    public void InviteToGame() {
        var currentGame = AvaloniaApp.Container.Get<FriendsManager>().CurrentUser.InGame;
        if (currentGame.IsValid() && currentGame.IsSteamApp()) {
            AvaloniaApp.Container.Get<IClientFriends>().InviteUserToGame(currentGame.AppID, Entity.SteamID, string.Empty);
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
        }
    
        if (change.HasFlag(EPersonaChange.Avatar)) {
            OnPropertyChanged(nameof(SmallAvatar));
            OnPropertyChanged(nameof(MediumAvatar));
            OnPropertyChanged(nameof(LargeAvatar));
        }
    }
}
