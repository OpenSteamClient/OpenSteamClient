using System.Text.Json.Serialization;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Login;

public class LoginUser {
    public enum ELoginMethod
    {
        UsernamePassword,
        JWT,
        Cached
    }

    [JsonIgnore]
    /// <summary>
    /// Used when logging in to tell the client how to log in.
    /// </summary>
    public ELoginMethod? LoginMethod { get; set; } = null;
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
        this.LoginMethod = ELoginMethod.UsernamePassword;
        this.AccountName = username;
        this.Password = password;
        this.Remembered = rememberPassword;
    }

    public LoginUser(string username, string JWT) {
        this.LoginMethod = ELoginMethod.JWT;
        this.AccountName = username;
        this.LoginToken = JWT;
    }
}