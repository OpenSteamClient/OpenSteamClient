using System.Collections;
using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Client.Extensions;
using ValveKeyValue;

namespace OpenSteamworks.Client.Utils;

/// <summary>
/// Base class for extended KVObjects
/// </summary>
public abstract class KVObjectEx {
    // Simple dictionary wrapper that copies itself everytime a value is written. This is bad, but we'll deal with this until we write our own keyvalue parser
    public class ProxyDictionary<TValue> : IDictionary<string, TValue> where TValue: KVObjectEx {
        private readonly Dictionary<string, TValue> internalDictionary = new();
        private readonly KVObjectEx wrappedObject;
        private readonly string dictionaryKey;

        public TValue this[string key] { get => ((IDictionary<string, TValue>)internalDictionary)[key]; set => Add(key, value); }
        public ICollection<string> Keys => ((IDictionary<string, TValue>)internalDictionary).Keys;
        public ICollection<TValue> Values => ((IDictionary<string, TValue>)internalDictionary).Values;
        public int Count => ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).Count;
        public bool IsReadOnly => ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).IsReadOnly;

        internal ProxyDictionary(Dictionary<string, TValue> dictionaryToWrap, KVObjectEx wrappedObject, string dictionaryKey) {
            this.internalDictionary = dictionaryToWrap;
            this.wrappedObject = wrappedObject;
            this.dictionaryKey = dictionaryKey;
        }

        public void Add(string key, TValue value)
        {
            ((IDictionary<string, TValue>)internalDictionary).Add(key, value);
            wrappedObject.SetValue(dictionaryKey, internalDictionary);
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).Add(item);
            wrappedObject.SetValue(dictionaryKey, internalDictionary);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).Clear();
            wrappedObject.SetValue(dictionaryKey, internalDictionary);
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, TValue>)internalDictionary).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, TValue>>)internalDictionary).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, TValue>)internalDictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return ((ICollection<KeyValuePair<string, TValue>>)internalDictionary).Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
        {
            return ((IDictionary<string, TValue>)internalDictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)internalDictionary).GetEnumerator();
        }
    }
    
    private readonly KVObject kv;
    public KVObject UnderlyingObject => kv;
    
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

    protected T? DefaultIfUnset<T>(string key, Func<KVObject, T> ctor, T? def = null) where T: KVObjectEx {
        if (!TryGetKey(key, out KVValue? kv)) {
            return def;
        }

        return ctor(kv[key].GetAsKVObject());
    }

    protected void SetValue(string key, KVValue val) {
        kv[key] = val;
    }

    protected void SetValue<TValue>(string key, Dictionary<string, TValue> val) where TValue: KVObjectEx {
        // This is truly one of the functions
        var kvobj = new KVObject(key, val.Select(p => new KVObject(p.Key, p.Value.UnderlyingObject.Value)));
        kv.Add(kvobj);
    }

    protected void SetValue(string key, Dictionary<string, uint> val) {
        // This is truly one of the functions
        kv[key] = (KVValue)val.Select(p => new KVObject(p.Key, p.Value.ToString()));
    }

    protected void SetValue(string key, int val) {
        kv[key] = val.ToString();
    }

    protected void SetValue(string key, uint val) {
        kv[key] = val.ToString();
    }

    protected void SetValue(string key, long val) {
        kv[key] = val.ToString();
    }

    protected void SetValue(string key, ulong val) {
        kv[key] = val.ToString();
    }

    protected void SetValue(string key, float val) {
        kv[key] = val.ToString();
    }

    protected void SetValue(string key, string val) {
        kv[key] = val.ToString();
    }

    protected void SetValue(string key, bool val) {
        kv[key] = Convert.ToInt32(val);
    }

    protected void SetValue(string key, KVObjectEx val) {
        // This probably isn't valid
        kv[key] = val.UnderlyingObject.Value;
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

    protected ProxyDictionary<TValue> EmptyDictionaryIfUnset<TValue>(string key, Func<KVObject, TValue> ctor) where TValue: KVObjectEx {
        ProxyDictionary<TValue> dict = new(new(), this, key);
        if (!TryGetKey(key, out KVValue? kv)) {
            return dict;
        }

        foreach (var item in kv.GetChildrenAsKVObjects())
        {
            dict.Add(item.Name, ctor(item));
        }

        return dict;
    }

    protected IDictionary<string, string> EmptyStringDictionaryIfUnset(string key) {
        Dictionary<string, string> dict = new();
        if (!TryGetKey(key, out KVValue? kv)) {
            return dict;
        }

        foreach (var item in kv.GetChildrenAsKVObjects())
        {
            dict.Add(item.Name, (string)item.Value);
        }

        return dict;
    }

    protected IDictionary<string, bool> EmptyBoolDictionaryIfUnset(string key) {
        Dictionary<string, bool> dict = new();
        if (!TryGetKey(key, out KVValue? kv)) {
            return dict;
        }

        foreach (var item in kv.GetChildrenAsKVObjects())
        {
            dict.Add(item.Name, (bool)item.Value);
        }

        return dict;
    }
    
    protected bool TryGetKey(string key, [NotNullWhen(true)] out KVValue? kv) {
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