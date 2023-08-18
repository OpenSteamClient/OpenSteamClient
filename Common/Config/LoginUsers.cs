using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace Common.Config;

public class LoginUsers : ConfigFile<LoginUsers>
{
    public override LoginUsers GetThis() => this;
    public List<LoginUser> Users { get; set; } = new();
    public LoginUsers() {}
}

public struct LoginUser {
    public required CSteamID SteamID { get; set; }
    public required string AccountName { get; set; }
    public required bool AllowAutoLogin { get; set; }
    public required bool MostRecent { get; set; }
}