namespace OpenSteamworks.IPCClient.Interfaces;

public abstract class IPCBaseInterface {
    internal readonly IPCClient client;
    internal readonly uint steamuser;
    protected IPCBaseInterface(IPCClient client, uint steamuser) {
        this.client = client;
        this.steamuser = steamuser;
    }

    protected FunctionSerializer CreateIPCFunctionCall(byte interfaceid, uint functionid, uint fencepost) {
        return new FunctionSerializer(this, interfaceid, functionid, fencepost);
    }
}