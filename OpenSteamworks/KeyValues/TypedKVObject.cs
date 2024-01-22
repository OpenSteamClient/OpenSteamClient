using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using OpenSteamworks.Utils;

namespace OpenSteamworks.KeyValues;

/// <summary>
/// Base class for typed KVObjects
/// </summary>
public abstract class TypedKVObject { 
    protected KVObject kv { get; init; }
    public KVObject UnderlyingObject => kv;
    
    public TypedKVObject(KVObject kv) {
        this.kv = kv;
    }
    
    protected int DefaultIfUnset(string key, int def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsInt();
    }

    protected uint DefaultIfUnset(string key, uint def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsUInt();
    }

    protected long DefaultIfUnset(string key, long def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsLong();
    }

    protected ulong DefaultIfUnset(string key, ulong def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsULong();
    }

    protected float DefaultIfUnset(string key, float def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsFloat();
    }

    protected string DefaultIfUnset(string key, string def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsString();
    }

    protected bool DefaultIfUnset(string key, bool def) {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        return kv.GetValueAsBool();
    }

    protected T? DefaultIfUnset<T>(string key, Func<KVObject, T> ctor, T? def = null) where T: TypedKVObject {
        if (!TryGetKey(key, out KVObject? kv)) {
            return def;
        }

        var child = kv.GetChild(key);
        if (child == null) {
            return null;
        }

        return ctor(child);
    }

    protected void SetValue(string key, dynamic val) => kv.SetChild(new KVObject(key, val));

    protected void SetValue<T>(string key, T val) where T: TypedKVObject {
        SetValue(key, val.UnderlyingObject);
    }

    protected void SetValue<T>(string key, Dictionary<string, T> val) where T: TypedKVObject {
        var child = kv.GetChild(key);
        if (child == null) {
            child = new KVObject(key, new List<KVObject>());
            kv.SetChild(child);
        }

        foreach (var item in val)
        {
            child.SetChild(new KVObject(item.Key, item.Value.UnderlyingObject));
        }
    }

    protected void SetValue(string key, int val) {
        SetValue(key, val.ToString());
    }

    protected void SetValue(string key, uint val) {
        SetValue(key, val.ToString());
    }

    protected void SetValue(string key, long val) {
        SetValue(key, val.ToString());
    }

    protected void SetValue(string key, ulong val) {
        SetValue(key, val.ToString());
    }

    protected void SetValue(string key, float val) {
        SetValue(key, val.ToString());
    }

    protected void SetValue(string key, string val) => kv.SetChild(new KVObject(key, val));
    protected void SetValue(string key, bool val) => kv.SetChild(new KVObject(key, val));

    protected IEnumerable<T> EmptyListIfUnset<T>(string key, Func<KVObject, T> ctor) where T: TypedKVObject {
        List<T> list = new();
        if (!TryGetKey(key, out KVObject? kv)) {
            return list;
        }

        foreach (var item in kv.Children)
        {
            list.Add(ctor(item));
        }

        return list;
    }

    protected Dictionary<string, TValue> EmptyDictionaryIfUnset<TValue>(string key, Func<KVObject, TValue> ctor) where TValue: TypedKVObject {
        Dictionary<string, TValue> dict = new();
        if (!TryGetKey(key, out KVObject? kv)) {
            return dict;
        }

        foreach (var item in kv.Children)
        {
            dict.Add(item.Name, ctor(item));
        }

        return dict;
    }

    protected IDictionary<string, string> EmptyStringDictionaryIfUnset(string key) {
        Dictionary<string, string> dict = new();
        if (!TryGetKey(key, out KVObject? kv)) {
            return dict;
        }

        foreach (var item in kv.Children)
        {
            dict.Add(item.Name, (string)item.Value);
        }

        return dict;
    }

    protected IDictionary<string, bool> EmptyBoolDictionaryIfUnset(string key) {
        Dictionary<string, bool> dict = new();
        if (!TryGetKey(key, out KVObject? kv)) {
            return dict;
        }

        foreach (var item in kv.Children)
        {
            dict.Add(item.Name, (bool)item.Value);
        }

        return dict;
    }
    
    protected bool TryGetKey(string key, [NotNullWhen(true)] out KVObject? kv) {
        if (!this.kv.Children.Any()) {
            kv = null;
            return false;
        }

        string[]? keys;
        if (key.Contains('/')) {
            keys = key.Split('/');
        } else {
            keys = new string[1] { key };
        }

        bool placeholder = true;
        KVObject? last = null;
        foreach (var item in keys)
        {
            if (placeholder) {
                last = this.kv.GetChild(item);
                placeholder = false;
                if (last == null) {
                    kv = null;
                    return false;
                }

                continue;
            }

            var next = last?.GetChild(item);
            if (next == null) {
                kv = null;
                return false;
            }

            last = next;
        }

        UtilityFunctions.AssertNotNull(last);
        kv = last;
        return true;
    }
}