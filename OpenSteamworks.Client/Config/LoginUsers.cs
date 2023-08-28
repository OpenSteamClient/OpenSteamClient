using System.Text.Json.Serialization;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Config;

public class LoginUsers : ConfigFile<LoginUsers>
{
    public override LoginUsers GetThis() => this;
    public List<LoginUser> Users { get; set; } = new();
    public LoginUsers() {}
}

public enum LoginMethod
{
    UsernamePassword,
    JWT,
    Cached
}

public struct LoginUser {
    [JsonIgnore]
    /// <summary>
    /// Used when logging in to tell the client how to log in.
    /// </summary>
    public LoginMethod? LoginMethod { get; set; }
    [JsonIgnore]
    /// <summary>
    /// Used when <see cref="LoginMethod"/> is set to <see cref="LoginMethod.JWT"/>.
    /// </summary>
    public string? LoginToken { get; set; }
    [JsonIgnore]
    /// <summary>
    /// Used when <see cref="LoginMethod"/> is set to <see cref="LoginMethod.UsernamePassword"/>.
    /// </summary>
    public string? Password { get; set; }
    [JsonIgnore]
    /// <summary>
    /// Used when <see cref="LoginMethod"/> is set to <see cref="LoginMethod.UsernamePassword"/>.
    /// You should set this to a valid Steam Guard Code (from the mobile app or email) if you know the account is steam guard protected.
    /// </summary>
    public string? SteamGuardCode { get; set; }
    /// <summary>
    /// Whether this login should be/is remembered.
    /// </summary>
    public bool Remembered { get; set; } = false;
    public CSteamID? SteamID { get; set; }
    public string AccountName { get; set; } = "";
    public bool AllowAutoLogin { get; set; } = false;
    public bool MostRecent { get; set; } = false;
    public LoginUser() {}

    public LoginUser(string username, string password, bool rememberPassword) {
        this.LoginMethod = Config.LoginMethod.UsernamePassword;
        this.AccountName = username;
        this.Password = password;
        this.Remembered = rememberPassword;
    }
}