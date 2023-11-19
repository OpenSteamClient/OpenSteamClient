using System.Text.Json.Serialization;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Config;

public class LoginUsers : ConfigFile<LoginUsers>
{
    public override LoginUsers GetThis() => this;
    public int Autologin { get; set; } = -1;
    public int MostRecent { get; set; } = -1;
    public List<LoginUser> Users { get; set; } = new();
    public LoginUsers() {}

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
        return Users.ElementAtOrDefault(this.Autologin);
    }

    public LoginUser? GetMostRecent() {
        return Users.ElementAtOrDefault(this.MostRecent);
    }
}

public enum LoginMethod
{
    UsernamePassword,
    JWT,
    Cached
}

public class LoginUser {
    [JsonIgnore]
    /// <summary>
    /// Used when logging in to tell the client how to log in.
    /// </summary>
    public LoginMethod? LoginMethod { get; set; } = null;
    [JsonIgnore]
    /// <summary>
    /// Used when <see cref="LoginMethod"/> is set to <see cref="LoginMethod.JWT"/>.
    /// </summary>
    public string? LoginToken { get; set; } = null;
    [JsonIgnore]
    /// <summary>
    /// Used when <see cref="LoginMethod"/> is set to <see cref="LoginMethod.UsernamePassword"/>.
    /// </summary>
    public string? Password { get; set; } = null;
    [JsonIgnore]
    /// <summary>
    /// Used when <see cref="LoginMethod"/> is set to <see cref="LoginMethod.UsernamePassword"/>.
    /// You should set this to a valid Steam Guard Code (from the mobile app or email) if you know the account is steam guard protected.
    /// </summary>
    public string? SteamGuardCode { get; set; } = null;
    /// <summary>
    /// Whether this login should be/is remembered (token/cached credentials stored).
    /// </summary>
    public bool Remembered { get; set; } = false;
    public CSteamID SteamID { get; set; } = 0;
    public string AccountName { get; set; } = "";
    public bool AllowAutoLogin { get; set; } = true;
    public LoginUser() {}

    public LoginUser(string username, string password, bool rememberPassword) {
        this.LoginMethod = Config.LoginMethod.UsernamePassword;
        this.AccountName = username;
        this.Password = password;
        this.Remembered = rememberPassword;
    }

    public LoginUser(string username, string JWT) {
        this.LoginMethod = Config.LoginMethod.JWT;
        this.AccountName = username;
        this.LoginToken = JWT;
    }
}