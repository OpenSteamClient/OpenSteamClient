using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.Protobuf.WebUI;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Managers;

public class NamespaceData {
    private object dataLock = new object();
    public string this[string key] {
        get {
            if (keys[key].IsDeleted) {
                throw new Exception("This key is deleted");
            }
            return keys[key].Value;
        }
        set {
            lock (dataLock)
            {
                if (keys.ContainsKey(key)) {
                    keys[key].IsDeleted = false;
                    keys[key].Value = value;
                    keys[key].Timestamp = clientUtils.GetServerRealTime();
                    // I'm not really sure what versions are supposed to mean. I'm assuming it just gets incremented by one, which it seems like it does.
                    keys[key].Version++;
                } else {
                    if (string.IsNullOrEmpty(key)) {
                        throw new ArgumentException("Attempting to add an empty key. This will brick WebUI!");
                    }
                    keys[key] = new CCloudConfigStore_Entry
                    {
                        Key = key,
                        IsDeleted = false,
                        Timestamp = clientUtils.GetServerRealTime(),
                        Value = value,
                        Version = 1
                    };
                }
            }
        }
    }

    public EUserConfigStoreNamespace Namespace {
        get {
            return (EUserConfigStoreNamespace)this.protobufData.Enamespace;
        }
    }

    public ulong Horizon {
        get {
            return this.protobufData.Horizon;
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("Dumping " + this.Namespace.ToString());
        foreach (var item in this.keys)
        {
            sb.AppendLine(item.Key + ": '" + item.Value.Value + "'" + (item.Value.IsDeleted ? " (deleted)" : "") + ", timestamp: " + item.Value.Timestamp);
        }

        return sb.ToString();
    }
    
    public Dictionary<string, string> GetKeysStartingWith(string matcher) {
        Dictionary<string, string> matchedKeys = new();
        foreach (var item in keys)
        {
            if (item.Value.IsDeleted) {
                continue;
            }

            if (item.Key.StartsWith(matcher)) {
                matchedKeys.Add(item.Key, item.Value.Value);
            }
        }

        return matchedKeys;
    }

    private Dictionary<string, CCloudConfigStore_Entry> keys = new();
    private CCloudConfigStore_NamespaceData protobufData;
    internal CSteamID belongsToUser;
    private IClientUtils clientUtils;
    internal NamespaceData(CCloudConfigStore_NamespaceData protobufData, CSteamID steamid, IClientUtils clientUtils) {
        this.protobufData = protobufData;
        this.belongsToUser = steamid;
        this.clientUtils = clientUtils;

        foreach (var item in this.protobufData.Entries)
        {
            this.keys.Add(item.Key, item);
        }
    }

    // Gets the NamespaceData object as it's protobuf counterpart
    public CCloudConfigStore_NamespaceData GetAsProtobuf() {
        CCloudConfigStore_NamespaceData data = protobufData.Clone();
        foreach (var item in keys)
        {
            // Can't use find without creating a List...
            int existingIndex = data.Entries.ToList().FindIndex(e => e.Key == item.Key);
            bool keyExists = existingIndex != -1;

            if (keyExists) {
                // If the key existed before, simply replace the value
                data.Entries[existingIndex].Value = item.Value.Value;
            } else {
                // If it didn't exist before, simply add it to the list. The this[] operator takes care of everything else
                data.Entries.Add(item.Value);
            }
            
        }

        return data;
    }

    /// <summary>
    /// Synchronizes the namespace data to a newly received object from the server
    /// If a conflict occurs, all local data will be overridden
    /// TODO: subject to change
    /// </summary>
    /// <param name="remoteData">New data from the server</param>
    internal void Synchronize(CCloudConfigStore_NamespaceData remoteData) {
        // First, add our keys to the new data
        var remoteEntriesList = remoteData.Entries.ToList();
        foreach (var localEntry in GetAsProtobuf().Entries)
        {
            var indexOfKey = remoteEntriesList.FindIndex(e => e.Key == localEntry.Key);
            if (indexOfKey == -1) {
                // Add our entries if they don't collide with any new ones
                remoteData.Entries.Add(localEntry);
            } else {
                // What to do in this situation?
                Console.WriteLine("Warning: Namespace key '" + localEntry.Key + "' exists in remote, local: '" + localEntry.Value + "', remote: '" + remoteEntriesList[indexOfKey].Value + "'");
            }
        }

        foreach (var remoteEntry in remoteData.Entries)
        {
            if (this.keys.ContainsKey(remoteEntry.Key)) {
                if (this.keys[remoteEntry.Key].Value != remoteEntry.Value) {
                    Console.WriteLine("Warning: Namespace data different in remote entry, local: '" + this.keys[remoteEntry.Key].Value + "', remote: '" + remoteEntry.Value + "', overriding local");
                }
                this.keys[remoteEntry.Key] = remoteEntry;
            } else {
                this.keys.Add(remoteEntry.Key, remoteEntry);
            }
        }

        // Override local
        lock (dataLock)
        { 
            this.protobufData = remoteData;
            this.keys.Clear();
            foreach (var item in remoteData.Entries)
            {
                this.keys.Add(item.Key, item);
            }
        }
    }
} 

/// <summary>
/// CloudConfigStore seems to be a versatile system for storing all kinds of config data.
/// It is currently only used for storing library related data.
/// Take backups before mucking about with this, it WILL wipe all your collections, showcases(Shelves), and might break other stuff as well.
/// </summary>
public class CloudConfigStore : Component {
    private LoginManager loginManager;
    private Connection connection;
    private ConfigManager configManager;
    private List<NamespaceData> loadedNamespaces = new();
    private IClientUtils clientUtils;
    public CloudConfigStore(IContainer container, ClientMessaging messaging, LoginManager loginManager, ConfigManager configManager, IClientUtils clientUtils) : base(container) {
        this.clientUtils = clientUtils;
        this.loginManager = loginManager;
        this.configManager = configManager;
        this.loginManager.LoggingOff += OnLoggingOff;
        this.connection = messaging.AllocateConnection();
        this.connection.AddServiceMethodHandler("CloudConfigStoreClient.NotifyChange#1", (Connection.StoredMessage msg) => this.OnCloudConfigStoreClient_NotifyChange(ProtoMsg<CCloudConfigStore_Change_Notification>.FromBinary(msg.fullMsg)));
    }

    internal string GetNamespaceFilename(NamespaceData ns) {
        return GetNamespaceFilename(ns.belongsToUser, ns.Namespace);
    }

    internal string GetNamespaceFilename(CSteamID belongsToUser, EUserConfigStoreNamespace @namespace) {
        return $"U-{belongsToUser.GetAccountId()}-cloudconfigstore-namespace-{(uint)@namespace}";
    }

    /// <summary>
    /// Gets the folder where local namespace data is stored.
    /// </summary>
    /// <returns></returns>
    public string GetStorageFolder() {
        var directory = Path.Combine(this.configManager.CacheDir, "cloudconfigstore");
        Directory.CreateDirectory(directory);
        return directory;
    }

    private void OnLoggingOff(object? sender, EventArgs e) {
        HandleConfigStoreShutdown().Wait();
    }

    private async Task HandleConfigStoreShutdown() {
        Console.WriteLine("Flushing namespaces to disk...");
        await CacheNamespaces();
        try
        {
            if (loginManager.IsOnline()) {
                Console.WriteLine("Uploading namespaces to server...");
                UploadNamespaces().Wait();
            } else {
                Console.WriteLine("Warning: Not uploading namespaces, we're in offline mode.");
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Failed to upload namespaces: " + ex.ToString());
        }

        this.loadedNamespaces.Clear();
    }

    private async void OnCloudConfigStoreClient_NotifyChange(ProtoMsg<CCloudConfigStore_Change_Notification> notification) {
        Console.WriteLine("CloudConfigStore change detected. This is untested, and loss of data may occur. Backing up affected namespaces...");
        foreach (var newVersion in notification.body.Versions)
        {
            foreach (var loadedNamespace in loadedNamespaces)
            {
                await BackupNamespace(loadedNamespace);
                loadedNamespace.Synchronize(await DownloadNamespace((EUserConfigStoreNamespace)newVersion.Enamespace));
            }
        }
    }

    /// <summary>
    /// Gets a namespace. Attempts to use newest data, but may fall back to local cache without an internet connection.
    /// Will save it into the local cache.
    /// If the namespace is currently loaded, it will be used instead of trying the internet and cache.
    /// </summary>
    public async Task<NamespaceData> GetNamespaceData(EUserConfigStoreNamespace @namespace) {
        if (loginManager.CurrentUser == null || !loginManager.CurrentUser.SteamID.HasValue) {
            throw new InvalidOperationException("Cannot retrieve namespace data without a login.");
        }

        foreach (var item in loadedNamespaces)
        {
            if (item.Namespace == @namespace) {
                return item;
            }
        }

        CCloudConfigStore_NamespaceData resp;
        NamespaceData nsData;
        string filename = GetNamespaceFilename(loginManager.CurrentUser.SteamID.Value, @namespace);
        if (loginManager.IsOffline()) {
            if (File.Exists(filename)) {
                try {
                    byte[] bytes = await File.ReadAllBytesAsync(filename, default);
                    resp = CCloudConfigStore_NamespaceData.Parser.ParseFrom(bytes);
                    nsData = new NamespaceData(resp, loginManager.CurrentUser.SteamID.Value, this.clientUtils);
                    loadedNamespaces.Add(nsData);
                    return nsData;
                } catch (Exception e) {
                    throw new Exception("Failed to load namespace data from cache and we're in offline mode.", e);
                }
            } else {
                throw new Exception("No namespace data cache and we're in offline mode.");
            }
        }

        resp = await DownloadNamespace(@namespace);
        nsData = new NamespaceData(resp, loginManager.CurrentUser.SteamID.Value, this.clientUtils);
        loadedNamespaces.Add(nsData);
        await CacheNamespace(nsData);
        return nsData;
    }   

    /// <summary>
    /// Downloads namespace data from the server. Does not use the cache and will never use local data.
    /// </summary>
    internal async Task<CCloudConfigStore_NamespaceData> DownloadNamespace(EUserConfigStoreNamespace @namespace) {
        ProtoMsg<CCloudConfigStore_Download_Request> msg = new("CloudConfigStore.Download#1");
        msg.body.Versions.Add(new CCloudConfigStore_NamespaceVersion() {
            Enamespace = (uint)@namespace,
        });

        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Download_Response, CCloudConfigStore_Download_Request>(msg);
        if (resp.body.Data.Count == 0) {
            throw new ArgumentException("Namespace " + @namespace + " doesn't exist.");
        }

        return resp.body.Data.First();
    }   

    private async Task CacheNamespaces() {
        foreach (var item in this.loadedNamespaces)
        {
            await CacheNamespace(item);
        }
    }

    private async Task UploadNamespaces() {
        foreach (var item in this.loadedNamespaces)
        {
            await UploadNamespace(item);
        }
    }

    public async Task CacheNamespace(NamespaceData data) {
        await File.WriteAllBytesAsync(Path.Combine(this.GetStorageFolder(), GetNamespaceFilename(data)), data.GetAsProtobuf().ToByteArray());
    }

    /// <summary>
    /// Saves a namespace data object to the server and locally.
    /// </summary>
    public async Task SaveNamespace(NamespaceData data) {
        await this.Upload(data.GetAsProtobuf());
        await this.CacheNamespace(data);
    }

    public async Task UploadNamespace(NamespaceData data) {
        await this.Upload(data.GetAsProtobuf());
    }

    public async Task BackupNamespace(NamespaceData data) {
        await File.WriteAllBytesAsync(Path.Combine(this.GetStorageFolder(), $"backup-{DateTimeOffset.Now.ToUnixTimeSeconds()}-{GetNamespaceFilename(data)}"), data.GetAsProtobuf().ToByteArray());
    }

    /// <summary>
    /// Uploads namespace data. Overrides anything on remote. Be careful, the API accepts basically anything, and will happily delete all configstore data if uploading an empty object.
    /// </summary>
    private async Task<IEnumerable<CCloudConfigStore_NamespaceVersion>> Upload(CCloudConfigStore_NamespaceData data) {
        if (data.Entries.Count == 0) {
            throw new Exception("Attempted to upload namespace data with 0 entries.");
        }
        foreach (var entry in data.Entries)
        {
            Console.WriteLine("entry(" + entry.Key + ")");
            if (!entry.HasKey || string.IsNullOrEmpty(entry.Key)) {
                Console.WriteLine("Deleting entry with empty key");
                // This key was probably created accidentally. Having this will brick the webui though, so let's remove it just in case (but it won't immediately remove it, so you'll need to wait some time)
                // In the meanwhile, you can launch games from the store or big picture mode
                entry.IsDeleted = true;
            }
            if (entry.IsDeleted) {
                Console.WriteLine("Deleted key found: " + entry.Key);
            }
        }

        ProtoMsg<CCloudConfigStore_Upload_Request> msg = new("CloudConfigStore.Upload#1");
        msg.body.Data.Add(data);
        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Upload_Response, CCloudConfigStore_Upload_Request>(msg);
        Console.WriteLine("resp: " + resp.ToString());
        return resp.body.Versions;
    }

    public override async Task RunStartup() {
        await EmptyAwaitable();
    }
    public override async Task RunShutdown()
    {
        await EmptyAwaitable();
    }
}
