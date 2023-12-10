using OpenSteamworks.Generated;

namespace OpenSteamworks.ClientInterfaces;

public class ClientCompat
{
    private SteamClient client;
    public ClientCompat(SteamClient client)
    {
        this.client = client;
    }
}