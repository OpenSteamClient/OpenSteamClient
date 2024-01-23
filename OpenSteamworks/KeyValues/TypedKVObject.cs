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
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsInt();
        }

        return def;
    }

    protected uint DefaultIfUnset(string key, uint def) {
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsUInt();
        }

        return def;
    }

    protected long DefaultIfUnset(string key, long def) {
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsLong();
        }

        return def;
    }

    protected ulong DefaultIfUnset(string key, ulong def) {
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsULong();
        }

        return def;
    }

    protected float DefaultIfUnset(string key, float def) {
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsFloat();
        }

        return def;
    }

    protected string DefaultIfUnset(string key, string def) {
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsString();
        }

        return def;
    }

    protected bool DefaultIfUnset(string key, bool def) {
        if (TryGetKey(key, out KVObject? kv)) {
            return kv.GetValueAsBool();
        }

        return def;
    }

    protected T? DefaultIfUnset<T>(string key, Func<KVObject, T> ctor, T? def = null) where T: TypedKVObject {
        if (TryGetKey(key, out KVObject? kv)) {
            return ctor(kv);
        }

        return def;
    }

    protected void SetValue<T>(string key, T val) => kv.SetChild(new KVObject(key, (dynamic)val!));

    protected void SetValueTypedObject<T>(string key, T val) where T: TypedKVObject {
        SetValue(key, val.UnderlyingObject.Value);
    }

    protected void SetValue<T>(string key, Dictionary<string, T> val) where T: TypedKVObject {
        var child = kv.GetChild(key);
        if (child == null) {
            child = new KVObject(key, new List<KVObject>());
        }

        foreach (var item in val)
        {
            child.SetChild(new KVObject(item.Key, item.Value.UnderlyingObject.Value));
        }

        kv.SetChild(child);
    }

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