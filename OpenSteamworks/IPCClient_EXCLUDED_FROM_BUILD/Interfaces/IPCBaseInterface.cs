namespace OpenSteamworks.IPCClient.Interfaces;

public abstract class IPCBaseInterface {
    protected readonly IPCClient client;
    protected readonly uint steamuser;
    protected IPCBaseInterface(IPCClient client, uint steamuser) {
        this.client = client;
        this.steamuser = steamuser;
    }
}