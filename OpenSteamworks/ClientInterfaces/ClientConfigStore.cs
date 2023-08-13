using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;

namespace OpenSteamworks.ClientInterfaces;

public class ClientConfigStore : ClientInterface {
    private IClientConfigStore nativeClientConfigStore;
    public ClientConfigStore(SteamClient client) : base(client) {
        this.nativeClientConfigStore = client.NativeClient.IClientConfigStore;
    }
    public bool IsSet( EConfigStore configStore, string key ) {
        return this.nativeClientConfigStore.IsSet(configStore, key);
    }
    public bool? GetBool( EConfigStore configStore, string key) {
        if (!IsSet(configStore, key)) {
            return null;
        }
        // It's fine to have a default here as that condition should never be fulfilled
        return this.nativeClientConfigStore.GetBool(configStore, key, false);
    }
    public int? GetInt( EConfigStore configStore, string key) {
        if (!IsSet(configStore, key)) {
            return null;
        }
        // It's fine to have a default here as that condition should never be fulfilled
        return this.nativeClientConfigStore.GetInt(configStore, key, 0);
    }
    public ulong? GetUlong( EConfigStore configStore, string key) {
        if (!IsSet(configStore, key)) {
            return null;
        }
        // It's fine to have a default here as that condition should never be fulfilled
        return this.nativeClientConfigStore.GetUint64(configStore, key, 0);
    }
    public float? GetFloat(EConfigStore configStore, string key) {
        if (!IsSet(configStore, key)) {
            return null;
        }
        // It's fine to have a default here as that condition should never be fulfilled
        return this.nativeClientConfigStore.GetFloat(configStore, key, 0);
    }
    public string? GetString(EConfigStore configStore, string key) {
        if (!IsSet(configStore, key)) {
            return null;
        }
        // It's fine to have a default here as that condition should never be fulfilled
        return this.nativeClientConfigStore.GetString(configStore, key, "");
    }
    /// <summary>
    /// NOTE: Capped at 4096 bytes
    /// </summary>
    public byte[]? GetBinary(EConfigStore configStore, string key) {
        if (!IsSet(configStore, key)) {
            return null;
        }
        unsafe {
            byte[] bytes = new byte[4096];
            fixed (byte* firstByte = bytes ) {
                var gotLength = this.nativeClientConfigStore.GetBinary(configStore, key, (nint)firstByte, 4096);
            }
            return bytes;
        }
    }
    public void SetBool(EConfigStore configStore, string key, bool value) {
        if (!this.nativeClientConfigStore.SetBool(configStore, key, value)) {
            throw new Exception("Failed to set key " + key + " in store " + configStore + "to " + value);
        }
    }
    public void SetInt(EConfigStore configStore, string key, int value) {
        if (!this.nativeClientConfigStore.SetInt(configStore, key, value)) {
            throw new Exception("Failed to set key " + key + " in store " + configStore + "to " + value);
        }
    }
    public void SetUlong(EConfigStore configStore, string key, ulong value) {
        if (!this.nativeClientConfigStore.SetUint64(configStore, key, value)) {
            throw new Exception("Failed to set key " + key + " in store " + configStore + "to " + value);
        }
    }
    public void SetFloat(EConfigStore configStore, string key, float value) {
        if (!this.nativeClientConfigStore.SetFloat(configStore, key, value)) {
            throw new Exception("Failed to set key " + key + " in store " + configStore + "to " + value);
        }
    }
    public void SetString(EConfigStore configStore, string key, string value)  {
        if (!this.nativeClientConfigStore.SetString(configStore, key, value)) {
            throw new Exception("Failed to set key " + key + " in store " + configStore + "to " + value);
        }
    }
    /// <summary>
    /// NOTE: Capped at 4096 bytes
    /// </summary>
    public void SetBinary(EConfigStore configStore, string key, byte[] value) {
        unsafe {
            fixed (byte* firstByte = value ) {
                if (!this.nativeClientConfigStore.SetBinary(configStore, key, (IntPtr)firstByte, (uint)value.Length)) {
                    throw new Exception("Failed to set key " + key + " in store " + configStore + " to a binary value");
                }
            }
        }
    }
    public void RemoveKey(EConfigStore configStore, string key) {
        if (!this.nativeClientConfigStore.RemoveKey(configStore, key)) {
            throw new Exception("Failed to remove key " + key + " in store " + configStore);
        }
    }

    public void FlushToDisk(bool bIsShuttingDown = false) {
        this.nativeClientConfigStore.FlushToDisk(bIsShuttingDown);
    }

    internal override void RunShutdownTasks()
    {
        base.RunShutdownTasks();
        this.FlushToDisk(true);
    }
}