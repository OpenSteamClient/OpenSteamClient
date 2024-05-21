using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;
using OpenSteamworks.Client.Login;

namespace OpenSteamworks.Client.Config;

public class LoginUsers: IConfigFile {
    static string IConfigFile.ConfigName => "LoginUsers_001";
    static bool IConfigFile.PerUser => false;
    static bool IConfigFile.AlwaysSave => false;

    [ConfigNeverVisible]
    public int Autologin { get; set; } = -1;

    [ConfigNeverVisible]
    public int MostRecent { get; set; } = -1;

    [ConfigNeverVisible]
    public List<LoginUser> Users { get; set; } = new();

    public void SetUserAsMostRecent(LoginUser user) {
        if (!Users.Contains(user)) {
            return;
        }

        this.MostRecent = Users.IndexOf(user);
    }

    public void SetUserAsAutologin(LoginUser user) {
        if (!Users.Contains(user)) {
            return;
        }

        this.Autologin = Users.IndexOf(user);
    }

    public bool AddUser(LoginUser user) {
        if (Users.Any(u => u.AccountName == user.AccountName)) {
            return false;
        } else {
            Users.Add(user);
        }

        return true;
    }

    public bool RemoveUser(LoginUser user) {
        int i = Users.FindIndex(u => u.AccountName == user.AccountName);
        if (i == -1) {
            return false;
        }

        Users.RemoveAt(i);
        if (this.Autologin == i) {
            this.Autologin = -1;
        }

        if (this.MostRecent == i) {
            this.MostRecent = -1;
        }
        
        return true;
    }

    public LoginUser? GetAutologin() {
        var autologinUser = Users.ElementAtOrDefault(this.Autologin);
        if (autologinUser?.AllowAutoLogin == false) {
            return null;
        }

        return autologinUser;
    }

    public LoginUser? GetMostRecent() {
        return Users.ElementAtOrDefault(this.MostRecent);
    }
}