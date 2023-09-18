using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Messaging;
using OpenSteamworks.Protobuf.WebUI;

namespace OpenSteamworks.ClientInterfaces;

public enum EUserConfigStoreNamespace {
    k_EUserConfigStoreNamespaceInvalid = 0,
    k_EUserConfigStoreNamespaceLibrary = 1
}
/// <summary>
/// Shit. We're really deep in uncharted waters with this one. This protobuf is used exclusively by the webui.
/// No data to decompile in steamclient or steamui. 
/// </summary>
public class CloudConfigStore {
    private ClientMessaging messaging;
    private Connection connection;
    public CloudConfigStore(ClientMessaging messaging) {
        this.messaging = messaging;
        this.connection = messaging.AllocateConnection();
    }

    /// <summary>
    /// Handles CloudConfigStoreClient.NotifyChange#1
    /// </summary>
    public void OnCloudConfigStoreClient_NotifyChange(CCloudConfigStore_Change_Notification notification) {
        
    }

    /// <summary>
    /// Download a namespace.
    /// </summary>
    public async Task<CCloudConfigStore_NamespaceData> DownloadNamespace(EUserConfigStoreNamespace @namespace) {
        ProtoMsg<CCloudConfigStore_Download_Request> msg = new("CloudConfigStore.Download#1");
        msg.body.Versions.Add(new CCloudConfigStore_NamespaceVersion() {
            Enamespace = (uint)@namespace,
        });

        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Download_Response, CCloudConfigStore_Download_Request>(msg);
        Console.WriteLine("got resp");
        Console.WriteLine(resp.ToString());
        if (resp.body.Data.Count == 0) {
            throw new ArgumentException("Namespace " + @namespace + " doesn't exist.");
        }
        return resp.body.Data.First();
    }   

    /// <summary>
    /// Uploads namespace data.
    /// </summary>
    public async Task<IEnumerable<CCloudConfigStore_NamespaceVersion>> Upload(CCloudConfigStore_NamespaceData data) {
        ProtoMsg<CCloudConfigStore_Upload_Request> msg = new("CloudConfigStore.Upload#1");
        msg.body.Data.Add(data);
        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Upload_Response, CCloudConfigStore_Upload_Request>(msg);
        return resp.body.Versions;
    }
}