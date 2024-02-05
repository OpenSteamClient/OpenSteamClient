using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Generated;
using static OpenSteamworks.IPCClient.Interfaces.InterfaceConsts;

namespace OpenSteamworks.IPCClient.Interfaces;

public class IPC_IClientSharedConnection : IPCBaseInterface, IClientSharedConnection
{
    public IPC_IClientSharedConnection(IPCClient client, uint steamuser) : base(client, steamuser) { } 

    public uint AllocateSharedConnection()
    {
        using (FunctionSerializer method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, IClientSharedConnection_AllocateSharedConnection1_FunctionID, IClientSharedConnection_AllocateSharedConnection1_Fencepost)) {
            method.FinalizeArgs();
            using (FunctionDeserializer ret = client.CallIPCFunctionEx(method)) {
                return ret.ReadUInt();
            }
        }
    }

    public unsafe bool BPopReceivedMessage(uint hConn, CUtlBuffer* bufOut, out uint hCall)
    {
        using (FunctionSerializer method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, 11111111, 1111111)) {
            method.AddArg(hConn);
            method.FinalizeArgs();
            using (FunctionDeserializer ret = client.CallIPCFunctionEx(method)) {
                var retval = ret.ReadBoolean();
                ret.ReadCUtlBuffer(bufOut);
                hCall = ret.ReadUInt();
                return retval;
            }
        }
    }

    public void InitiateConnection(uint connection)
    {
        using (FunctionSerializer method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, 11111111, 1111111)) {
            method.AddArg(connection);
            method.FinalizeArgs();
            client.CallIPCFunctionEx(method);
        }
    }

    public void RegisterEMsgHandler(uint hConn, uint eMsg)
    {
        using (FunctionSerializer method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, 11111111, 1111111)) {
            method.AddArg(hConn);
            method.AddArg(eMsg);
            method.FinalizeArgs();
            client.CallIPCFunctionEx(method);
        }
    }

    public void RegisterServiceMethodHandler(uint hConn, string method)
    {
        using (FunctionSerializer _method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, 11111111, 1111111)) {
            _method.AddArg(hConn);
            _method.AddArg(method);
            _method.FinalizeArgs();
            client.CallIPCFunctionEx(_method);
        }
    }

    public void ReleaseSharedConnection(uint connection)
    {
        using (FunctionSerializer _method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, 11111111, 1111111)) {
            _method.AddArg(connection);
            _method.FinalizeArgs();
            client.CallIPCFunctionEx(_method);
        }
    }

    public unsafe int SendMessage(uint connection, void* msg, size_t size)
    {
        using var _method = client.CreateIPCFunctionCall(steamuser, IClientSharedConnection_InterfaceID, 11111111, 1111111);
        _method.AddArg(connection);
        _method.AddDataFromPointer(msg, (uint)(UIntPtr)size);
        _method.FinalizeArgs();
        using var ret = client.CallIPCFunctionEx(_method);
        return ret.ReadInt();
    }

    public unsafe int SendMessageAndAwaitResponse(uint connection, void* msg, size_t size)
    {
        throw new System.NotImplementedException();
    }
}