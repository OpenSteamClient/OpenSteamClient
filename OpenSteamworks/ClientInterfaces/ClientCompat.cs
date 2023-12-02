using OpenSteamworks.Generated;

namespace OpenSteamworks.ClientInterfaces;

public class ClientCompat : ClientInterface
{
    private SteamClient client;
    public ClientCompat(SteamClient client) : base(client)
    {
        this.client = client;
    }
    
    internal override void RunShutdownTasks()
    {
        base.RunShutdownTasks();
    }

}