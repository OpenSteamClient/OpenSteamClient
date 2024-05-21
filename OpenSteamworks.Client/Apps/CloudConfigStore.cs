using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.Protobuf.WebUI;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using Profiler;

namespace OpenSteamworks.Client.Apps;

public class NamespaceData {
    public readonly object DataLock = new();

    public string this[string key] {
        get {
            lock (DataLock)
            {
                return Entries.ToImmutableList().Find(e => e.Key == key && !e.IsDeleted)?.Value ?? throw new IndexOutOfRangeException("Key not found");
            }
        }
        set {
            lock (DataLock)
            {
                if (string.IsNullOrEmpty(key)) {
                    throw new ArgumentException("Attempting to add an empty key. This will brick WebUI!");
                }
                
                CCloudConfigStore_Entry? entry = Entries.ToImmutableList().Find(e => e.Key == key);
                if (entry == null)
                {
                    entry = new CCloudConfigStore_Entry
                    {
                        Key = key,
                        IsDeleted = false,
                        Timestamp = clientUtils.GetServerRealTime(),
                        Value = value,
                        Version = 1
                    };

                    Entries.Add(entry);
                }

                entry.IsDeleted = false;
                entry.Value = value;
                entry.Timestamp = clientUtils.GetServerRealTime();
                // I'm not really sure what versions are supposed to mean. I'm assuming it just gets incremented by one, which it seems like it does (it also maybe does jumps of 10 every now and then).
                entry.Version++;

                lock (ChangesLock)
                {
                    Changes.Add(entry);
                    ccs.MarkForUpload();
                }
            }
        }
    }

    internal readonly object ChangesLock = new();
    internal readonly List<CCloudConfigStore_Entry> Changes = new();

    public EUserConfigStoreNamespace Namespace {
        get {
            lock (DataLock)
            {
                return (EUserConfigStoreNamespace)this.ProtobufData.Enamespace;
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        lock (DataLock)
        {
            sb.AppendLine("Dumping " + this.Namespace.ToString());
            foreach (var item in Entries)
            {
                sb.AppendLine(item.Key + ": '" + item.Value + "'" + (item.IsDeleted ? " (deleted)" : "") + ", timestamp: " + item.Timestamp);
            }
        }

        return sb.ToString();
    }
    
    public List<CCloudConfigStore_Entry> GetEntriesStartingWithKeyName(string startsWith) {
        return Where(e => e.Key.StartsWith(startsWith));
    }

    /// <summary>
    /// Get all entries that satisfy the condition.
    /// Ignores deleted entries
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public List<CCloudConfigStore_Entry> Where(Func<CCloudConfigStore_Entry, bool> predicate) {
        lock (DataLock)
        {
            return Entries.Where(e => !e.IsDeleted && predicate(e)).ToList();
        }
    }    

    private RepeatedField<CCloudConfigStore_Entry> Entries => ProtobufData.Entries;
    public CCloudConfigStore_NamespaceData ProtobufData { get; private set; }
    private readonly IClientUtils clientUtils;
    private readonly CloudConfigStore ccs;

    internal NamespaceData(CCloudConfigStore_NamespaceData protobufData, CloudConfigStore ccs, IClientUtils clientUtils) {
        this.ccs = ccs;
        this.ProtobufData = protobufData;
        this.clientUtils = clientUtils;
    }

    /// <summary>
    /// Copies the namespace data from a newly received object and overriding the local data
    /// </summary>
    /// <param name="remoteData">New data from the server</param>
    internal void ServerUpdate(CCloudConfigStore_NamespaceData remoteData) {
        lock (DataLock) {
            foreach (var remoteItem in remoteData.Entries)
            {
                var localItem = this.Entries.ToImmutableList().Find(e => e.Key == remoteItem.Key);
                if (localItem == null) {
                    // Simplest operation, simply add the new object to our list
                    this.Entries.Add(remoteItem);
                    continue;
                }

                // If the object is already in our list, simply clone the fields over (also handles removal fine, since we check for IsDeleted)
                localItem.MergeFrom(remoteItem);
            }
        }
    }
} 

/// <summary>
/// CloudConfigStore seems to be a versatile system for storing all kinds of config data.
/// It is currently only used for storing library related data.
/// Take backups before mucking about with this, it might brick ValveSteam if given wrong/unparseable data.
/// </summary>
public class CloudConfigStore : ILogonLifetime {
    private readonly LoginManager loginManager;
    private readonly Connection connection;
    private readonly List<NamespaceData> loadedNamespaces = new();
    private readonly IClientUtils clientUtils;
    private readonly InstallManager installManager;
    private readonly Logger logger;

    public event EventHandler<EUserConfigStoreNamespace>? NamespaceUpdated;
    public CloudConfigStore(ClientMessaging messaging, LoginManager loginManager, IClientUtils clientUtils, InstallManager installManager) {
        this.logger = Logger.GetLogger("CloudConfigStore", installManager.GetLogPath("CloudConfigStore"));
        this.installManager = installManager;
        this.clientUtils = clientUtils;
        this.loginManager = loginManager;
        this.connection = messaging.AllocateConnection();
        this.connection.AddServiceMethodHandler("CloudConfigStoreClient.NotifyChange#1", (Connection.StoredMessage msg) => this.OnCloudConfigStoreClient_NotifyChange(ProtoMsg<CCloudConfigStore_Change_Notification>.FromBinary(msg.fullMsg)));
    }

    internal string GetNamespaceFilename(NamespaceData ns) {
        return GetNamespaceFilename(loginManager.CurrentUser!.SteamID, ns.Namespace);
    }

    internal static string GetNamespaceFilename(CSteamID belongsToUser, EUserConfigStoreNamespace @namespace) {
        return $"U{belongsToUser.AccountID}-ns-{(uint)@namespace}";
    }

    /// <summary>
    /// Gets the folder where local namespace data is stored.
    /// </summary>
    /// <returns></returns>
    public string GetStorageFolder() {
        var directory = Path.Combine(this.installManager.CacheDir, "cloudconfigstore");
        Directory.CreateDirectory(directory);
        return directory;
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress) {
        await HandleConfigStoreShutdown();
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        // Preload the "library" config store
        try
        {
            await GetNamespaceData(EUserConfigStoreNamespace.Library);
        }
        catch (System.Exception ex)
        {
            logger.Error("CCS preload failed:");
            logger.Error(ex);
        }
    }

    private async Task HandleConfigStoreShutdown() {
        logger.Info("Flushing namespaces to disk...");
        await CacheNamespaces();
        try
        {
            if (loginManager.IsOnline()) {
                logger.Info("Uploading changes to server");
                UtilityFunctions.Debounce_MarkExecuted(uploadDebounce);
                await UploadChanges();
                logger.Info("Uploaded namespaces to server");
            } else {
                logger.Warning("Not uploading namespaces, we're in offline mode.");
            }
        }
        catch (System.Exception ex)
        {
            logger.Error("Failed to upload namespaces: " + ex.ToString());
        }

        this.loadedNamespaces.Clear();
    }

    private async void OnCloudConfigStoreClient_NotifyChange(ProtoMsg<CCloudConfigStore_Change_Notification> notification) {
        foreach (var newVersion in notification.body.Versions)
        {
            foreach (var loadedNamespace in loadedNamespaces)
            {
                loadedNamespace.ServerUpdate(await DownloadNamespace((EUserConfigStoreNamespace)newVersion.Enamespace, newVersion.Version - 1));
            }

            NamespaceUpdated?.Invoke(this, (EUserConfigStoreNamespace)newVersion.Enamespace);
        }
    }

    /// <summary>
    /// Gets a namespace. Attempts to use newest data, but may fall back to local cache without an internet connection.
    /// Will save it into the local cache.
    /// If the namespace is currently loaded, it will be used instead of trying the internet and cache.
    /// </summary>
    public async Task<NamespaceData> GetNamespaceData(EUserConfigStoreNamespace @namespace) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("CloudConfigStore.GetNamespaceData");

        if (loginManager.CurrentUser == null || loginManager.CurrentUser.SteamID == 0) {
            logger.Error("Cannot retrieve namespace data without a login.");
            throw new InvalidOperationException("Cannot retrieve namespace data without a login.");
        }

        foreach (var item in loadedNamespaces)
        {
            if (item.Namespace == @namespace) {
                return item;
            }
        }

        CCloudConfigStore_NamespaceData resp;
        string filename = GetNamespaceFilename(loginManager.CurrentUser.SteamID, @namespace);
        string loggerName = "Namespace-N" + ((int)@namespace) + "-U-" + loginManager.CurrentUser.SteamID.AccountID;
        NamespaceData? nsdata = null;
        if (File.Exists(filename)) {
            try {
                byte[] bytes = await File.ReadAllBytesAsync(filename, default);
                resp = CCloudConfigStore_NamespaceData.Parser.ParseFrom(bytes);
                nsdata = new NamespaceData(resp, this, this.clientUtils);
                if (loginManager.IsOffline()) {
                    loadedNamespaces.Add(nsdata);
                    return nsdata;
                }
            } catch (Exception e) {
                if (loginManager.IsOffline()) {
                    logger.Error("Failed to load namespace data from cache and we're in offline mode. ");
                    logger.Error(e);
                    throw new Exception("Failed to load namespace data from cache and we're in offline mode.", e);
                }

                logger.Error("Failed to load cached namespace, loading full.");
                logger.Error(e);
            }
        } 

        if (loginManager.IsOffline()) {
            logger.Error("No namespace data cache and we're in offline mode.");
            throw new Exception("No namespace data cache and we're in offline mode.");
        }

        ulong prevVer = 0;
        if (nsdata != null) {
            prevVer = nsdata.ProtobufData.Version;
            loadedNamespaces.Add(nsdata);
        }

        resp = await DownloadNamespace(@namespace, prevVer);
        if (nsdata != null) {
            nsdata.ServerUpdate(resp);
            await CacheNamespace(nsdata);
        } else {
            nsdata = new NamespaceData(resp, this, this.clientUtils);
            loadedNamespaces.Add(nsdata);
            await CacheNamespace(nsdata);
        }

        return nsdata;
    }   

    /// <summary>
    /// Downloads namespace data from the server. Does not use the cache and will never use local data.
    /// </summary>
    internal async Task<CCloudConfigStore_NamespaceData> DownloadNamespace(EUserConfigStoreNamespace @namespace, ulong lastVersion) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("CloudConfigStore.DownloadNamespace");
        logger.Info("Downloading namespace " + @namespace);
        ProtoMsg<CCloudConfigStore_Download_Request> msg = new("CloudConfigStore.Download#1");
        msg.body.Versions.Add(new CCloudConfigStore_NamespaceVersion() {
            Enamespace = (uint)@namespace,
            Version = lastVersion
        });

        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Download_Response, CCloudConfigStore_Download_Request>(msg);
        if (resp.body.Data.Count == 0) {
            logger.Error("Namespace " + @namespace + " doesn't exist.");
            throw new ArgumentException("Namespace " + @namespace + " doesn't exist.");
        }
        
        logger.Info("Downloaded namespace " + @namespace + " successfully");
        return resp.body.Data.First();
    }   

    private async Task CacheNamespaces() {
        foreach (var item in this.loadedNamespaces)
        {
            await CacheNamespace(item);
        }
    }

    public async Task CacheNamespace(NamespaceData data) {
        await File.WriteAllBytesAsync(Path.Combine(this.GetStorageFolder(), GetNamespaceFilename(data)), data.ProtobufData.ToByteArray());
    }

    private readonly object uploadDebounce = new();
    private readonly object changesLock = new();
    private readonly List<CCloudConfigStore_NamespaceData> changes = new();

    /// <summary>
    /// Debounces and eventually uploads changed data to the server.
    /// </summary>
    public void MarkForUpload() {
        lock (changesLock)
        {
            foreach (var item in this.loadedNamespaces)
            {
                lock (item.Changes)
                {
                    if (item.Changes.Count == 0) {
                        continue;
                    }

                    CCloudConfigStore_NamespaceData nsdata = new()
                    {
                        Enamespace = item.ProtobufData.Enamespace,
                        Version = item.ProtobufData.Version
                    };

                    nsdata.Entries.AddRange(item.Changes);
                    foreach (var change in item.Changes)
                    {
                        // Since the objects are stored by reference, let's increment the versions here temporarily (they'll get incremented by the server on response)
                        change.Version++;
                    }

                    item.Changes.Clear();
                    changes.Add(nsdata);
                }

                item.ProtobufData.Version++;
            }
        }
        
        if (changes.Count == 0) {
            Console.WriteLine("MarkForUpload: No namespaces to sync");
            return;
        }

        UtilityFunctions.Debounce(() => UploadChanges().GetAwaiter().GetResult(), uploadDebounce, TimeSpan.FromSeconds(2));
    }


    private async Task UploadChanges() {
        ProtoMsg<CCloudConfigStore_Upload_Request> req = new("CloudConfigStore.Upload#1");

        lock (changesLock)
        {
            if (changes.Count == 0) {
                Console.WriteLine("Attempted to upload namespace data with 0 entries.");
                return;
            }

            req.body.Data.Add(changes);
            changes.Clear();
        }

        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Upload_Response, CCloudConfigStore_Upload_Request>(req);
        foreach (var item in resp.body.Versions)
        {
            var ns = this.loadedNamespaces.Find(n => (uint)n.Namespace == item.Enamespace);
            if (ns == null) {
                Console.WriteLine("Upload result got unknown namespace: " + item.Enamespace);
                continue;
            }

            ns.ProtobufData.Version = item.Version;
        }
    }
}
