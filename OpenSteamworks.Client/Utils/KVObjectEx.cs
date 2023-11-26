using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Client.Extensions;
using ValveKeyValue;

namespace OpenSteamworks.Client.Utils;

/// <summary>
/// Base class for extended KVObjects
/// </summary>
public abstract class KVObjectEx {
    private readonly KVObject kv;
    public KVObjectEx(KVObject kv) {
        this.kv = kv;
    }

    public KVValue this[string key] => kv[key];

    protected int DefaultIfUnset(string key, int def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (int)kv;
    }

    protected uint DefaultIfUnset(string key, uint def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (uint)kv;
    }

    protected long DefaultIfUnset(string key, long def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (long)kv;
    }

    protected ulong DefaultIfUnset(string key, ulong def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (ulong)kv;
    }

    protected float DefaultIfUnset(string key, float def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (float)kv;
    }

    protected string DefaultIfUnset(string key, string def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (string)kv;
    }

    protected bool DefaultIfUnset(string key, bool def) {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return (int)kv == 1;
    }

    protected T? DefaultIfUnset<T>(string key, Func<KVObject, T> ctor, T? def) where T: KVObjectEx {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return ctor(kv[key].GetAsKVObject());
    }

    protected IEnumerable<T> EmptyListIfUnset<T>(string key, Func<KVObject, T> ctor) where T: KVObjectEx {
        List<T> list = new();
        if (!TryGetKey(key, out KVValue? kv)) {
            return list;
        }

        foreach (var item in kv.GetChildrenAsKVObjects())
        {
            list.Add(ctor(item));
        }

        return list;
    }
    
    private bool TryGetKey(string key, [NotNullWhen(true)] out KVValue? kv) {
        string[]? keys;
        if (key.Contains('/')) {
            keys = key.Split('/');
        } else {
            keys = new string[1] { key };
        }

        bool placeholder = true;
        KVValue? last = null;
        foreach (var item in keys)
        {
            if (placeholder) {
                last = this.kv[item];
                placeholder = false;
                if (last == null) {
                    kv = null;
                    return false;
                }

                continue;
            }

            if (last![item] == null) {
                kv = null;
                return false;
            }

            last = last[item];
        }

        kv = last!;
        return true;
    }
}