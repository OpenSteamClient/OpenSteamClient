using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Messaging;
using OpenSteamworks.Protobuf.WebUI;

namespace OpenSteamworks.Client.Managers;

public enum EUserConfigStoreNamespace {
    k_EUserConfigStoreNamespaceInvalid = 0,
    k_EUserConfigStoreNamespaceLibrary = 1
}
/// <summary>
/// Shit. We're really deep in uncharted waters with this one. This protobuf is used exclusively by the webui.
/// No data to decompile in steamclient or steamui. 
/// </summary>
public class CloudConfigStoreManager {
    private ClientMessaging messaging;
    private Connection connection;
    public CloudConfigStoreManager(ClientMessaging messaging) {
        this.messaging = messaging;
        this.connection = messaging.AllocateConnection();
    }

    /// <summary>
    /// Handles CloudConfigStoreClient.NotifyChange#1
    /// </summary>
    public void OnCloudConfigStoreClient_NotifyChange(CCloudConfigStore_Change_Notification notification) {
        
    }

    /// <summary>
    /// Handles CloudConfigStore.Download#1
    /// </summary>
    public async Task<IEnumerable<CCloudConfigStore_NamespaceData>> Download(List<CCloudConfigStore_NamespaceVersion> versions) {
        ProtoMsg<CCloudConfigStore_Download_Request> msg = new("CloudConfigStore.Download#1");
        msg.body.Versions.Add(versions);
        Console.WriteLine("sending");
        Console.WriteLine(msg.ToString());
        connection.StartPollThread();
        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Download_Response, CCloudConfigStore_Download_Request>(msg);
        Console.WriteLine("got resp");
        Console.WriteLine(resp.ToString());
        return resp.body.Data;
    }   

    /// <summary>
    /// Handles CloudConfigStore.Upload#1
    /// </summary>
    public async Task<IEnumerable<CCloudConfigStore_NamespaceVersion>> Upload(List<CCloudConfigStore_NamespaceData> data) {
        ProtoMsg<CCloudConfigStore_Upload_Request> msg = new("CloudConfigStore.Upload#1");
        msg.body.Data.AddRange(data);
        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Upload_Response, CCloudConfigStore_Upload_Request>(msg);
        return resp.body.Versions;
    }

    public string FormatRequestURL(string url) {
        return string.Format("U{0}-{1}", "304570273", "cloud-storage-namespace-1");
    }
}