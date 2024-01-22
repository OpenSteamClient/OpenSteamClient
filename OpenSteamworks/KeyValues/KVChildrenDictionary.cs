using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using OpenSteamworks.Utils;

namespace OpenSteamworks.KeyValues;

public class KVChildrenDictionary : IDictionary<string, object>
{
    private class KVChildrenEnumerator : IEnumerator, IEnumerator<KeyValuePair<string, object>>
    {
        private readonly KVChildrenDictionary dict;

        private int index = 0;
        public KeyValuePair<string, object> Current => dict.ElementAt(index);
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (index+1 > dict.Count) {
                return false;
            }

            index++;
            return true;
        }

        public void Reset()
        {
            index = 0;
        }

        void IDisposable.Dispose()
        {
            // No-op
        }

        public KVChildrenEnumerator(KVChildrenDictionary dict) {
            this.dict = dict;
        }
    }

    private readonly KVObject rootObject;
    public dynamic this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ICollection<string> Keys => new List<string>(this.rootObject.Children.Select(o => o.Name));

    public ICollection<dynamic> Values => new List<dynamic>(this.rootObject.Children.Select(o => o.Value));

    public int Count => this.rootObject.Children.Count;
    public bool IsReadOnly => false;

    public KVChildrenDictionary(KVObject rootObject) {
        UtilityFunctions.Assert(rootObject.HasChildren);
        this.rootObject = rootObject;
    }

    public void Add(string key, dynamic value)
    {
        this.rootObject.Children.Add(new KVObject(key, value));
    }

    public void Add(KeyValuePair<string, dynamic> item)
    {
        this.rootObject.Children.Add(new KVObject(item.Key, item.Value));
    }

    public void Clear()
    {
        this.rootObject.Children.Clear();
    }

    public bool Contains(KeyValuePair<string, dynamic> item)
    {
        var child = this.rootObject.GetChild(item.Key);
        if (child == null) {
            return false;
        }

        return child.Value == item.Value;
    }

    public bool ContainsKey(string key)
    {
        return this.rootObject.GetChild(key) != null;
    }

    public IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator()
    {
        return new KVChildrenEnumerator(this);
    }

    public bool Remove(string key)
    {
        return this.rootObject.RemoveChild(key);
    }

    public bool Remove(KeyValuePair<string, dynamic> item)
    {
        return this.rootObject.RemoveChild(item.Key);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out dynamic value)
    {
        var obj = this.rootObject.GetChild(key);
        if (obj == null) {
            value = null;
            return false;
        }

        value = obj;
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new KVChildrenEnumerator(this);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        throw new InvalidOperationException("Copying KVChildrenDictionary is unsupported");
    }
}