using OpenSteamworks.Structs;

namespace OpenSteamworks.IPCClient.Interfaces;

public class ClientShortcuts : IPCBaseInterface
{
    public ClientShortcuts(IPCClient client, uint steamuser) : base(client, steamuser) { }

    public CGameID GetGameIDForAppID(AppId_t appid) {
        using var serializer = CreateIPCFunctionCall(InterfaceConsts.IClientShortcuts_InterfaceID, InterfaceConsts.IClientShortcuts_GetGameIDForAppID_A1_R1_FunctionID, InterfaceConsts.IClientShortcuts_GetGameIDForAppID_A1_R1_Fencepost);
        serializer.AddArg(appid);
        serializer.FinalizeArgs();
        using var deserializer = serializer.SendAndWaitForResponse();
        return new CGameID(deserializer.ReadULong());
    }
}