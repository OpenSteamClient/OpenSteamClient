using OpenSteamworks.Generated;

namespace OpenSteamworks.ClientInterfaces;

public class ClientCompat
{
    private ISteamClient client;
    public ClientCompat(ISteamClient client)
    {
        this.client = client;
    }
}