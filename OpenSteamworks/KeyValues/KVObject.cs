using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OpenSteamworks.KeyValues;

public class KVObject : IEquatable<KVObject>, ICloneable {
    public string Name { get; set; }
    public dynamic Value { get; internal set; }
    public bool HasChildren => Value is List<KVObject>;
    public List<KVObject> Children {
        get {
            ThrowIfNotList();
            return (Value as List<KVObject>)!;
        }
    }

    public void SetChild(KVObject kv) {
        ThrowIfNotList();

        var existingObj = GetChild(kv.Name);
        if (existingObj != null) {
            existingObj.Value = kv.Value;
            return;
        }

        Children.Add(kv);
    }

    public bool RemoveChild(string key) {
        ThrowIfNotList();
        return this.Children.RemoveAll((obj) => obj.Name == key) > 0;
    }

    public KVObject? GetChild(string key) {
        ThrowIfNotList();
        
        foreach (var item in Children)
        {
            if (item.Name == key) {
                return item;
            }
        }

        return null;
    }

    public KVChildrenDictionary GetChildrenAsTrackingDictionary() {
        return new KVChildrenDictionary(this);
    }
    
    private void ThrowIfNotList() {
        if (Value is not List<KVObject>) {
            throw new InvalidOperationException("This operation is invalid on KVObjects where Value is not List<KVObject>");
        }
    }

    public KVObject(string name, List<KVObject> children) {
        this.Name = name;
        this.Value = children;
    }

    public KVObject(string name, KVObject value) {
        this.Name = name;
        this.Value = new List<KVObject>() { value };
    }

    public KVObject(string name, string value) {
        this.Name = name;
        this.Value = value;
    }

    public KVObject(string name, bool value) {
        this.Name = name;
        this.Value = Convert.ToInt32(value);
    }

    public KVObject(string name, int value) {
        this.Name = name;
        this.Value = value;
    }

    public KVObject(string name, uint value) {
        this.Name = name;
        this.Value = value.ToString();
    }

    public KVObject(string name, float value) {
        this.Name = name;
        this.Value = value;
    }

    public KVObject(string name, ulong value) {
        this.Name = name;
        this.Value = value;
    }

    public KVObject(string name, long value) {
        this.Name = name;
        this.Value = value;
    }

    private T GetValueAs<T>() where T: IParsable<T> {
        if (Value is string str) {
            return T.Parse(str, null);
        } else if (Value is T t) {
            return t;
        }

        throw new InvalidOperationException("Could not get value of type " + typeof(T).Name + "; type of Value is " + Value.GetType().Name);
    }

    private void SetValue<T>(T val) where T: IFormattable {
        if (Value is string) {
            var asStr = val.ToString();
            if (asStr == null) {
                throw new NullReferenceException("IFormattable object returned null.");
            }

            Value = asStr;
        } else if (Value is T) {
            Value = val;
        }

        throw new InvalidOperationException("Attempting to change type of value from " + Value?.GetType().Name + " to " + typeof(T).Name);
    }

    public bool GetValueAsBool() => Convert.ToBoolean(GetValueAs<int>());
    public int GetValueAsInt() => GetValueAs<int>();
    public uint GetValueAsUInt() => GetValueAs<uint>();
    public float GetValueAsFloat() => GetValueAs<float>();
    public ulong GetValueAsULong() => GetValueAs<ulong>();
    public long GetValueAsLong() => GetValueAs<long>();
    
    public void SetValue(bool val) => SetValue<int>(Convert.ToInt32(val));
    public void SetValue(int val) => SetValue<int>(val);
    public void SetValue(uint val) => SetValue<uint>(val);
    public void SetValue(float val) => SetValue<float>(val);
    public void SetValue(ulong val) => SetValue<ulong>(val);
    public void SetValue(long val) => SetValue<long>(val);
    public void SetValue(string val) {
        if (Value is string) {
            Value = val;
        }

        throw new InvalidOperationException("Attempting to change type of value from " + Value?.GetType().Name + " to string");
    }

    public string GetValueAsString() {
        if (Value is string str) {
            return str;
        } else if (Value is IFormattable c) {
            string? asStr = c.ToString();
            if (asStr == null) {
                throw new NullReferenceException("IFormattable object returned null.");
            }

            return asStr;
        }

        throw new InvalidOperationException("Could not get value of type string");
    }

    public bool HasChild(string key) {
        return GetChild(key) != null;
    }

    public bool TryGetChild(string key, [NotNullWhen(true)] out KVObject? obj)
    {
        var child = GetChild(key);
        if (child == null) {
            obj = null;
            return false;
        }

        obj = child;
        return true;
    }

    public override bool Equals(object? obj)
    {
        return Equals((KVObject?)obj, true);
    }

    public bool Equals(KVObject? other, bool valueTypeEqual = false)
    {
        if (object.ReferenceEquals(this, other)) {
            return true;
        }

        if (other is null) {
            Logging.GeneralLogger.Debug("KVObject comparison fails: other is not null");
            return false;
        }

        if (other.HasChildren != this.HasChildren) {
            Logging.GeneralLogger.Debug("KVObject comparison fails: other.HasChildren == this.HasChildren");
            return false;
        }

        if (this.HasChildren && other.HasChildren) {
            if (this.Children.Count != other.Children.Count) {
                Logging.GeneralLogger.Debug($"KVObject comparison fails: this.Children.Count ({this.Children.Count}) == other.Children.Count ({other.Children.Count})");
                return false;
            }

            foreach (var item in this.Children)
            {
                if (!item.Equals(other.GetChild(item.Name), valueTypeEqual)) {
                    Logging.GeneralLogger.Debug("KVObject comparison fails: item.Equals(other.GetChild(item.Name))");
                    return false;
                }
            }
        } else {
            if (valueTypeEqual && this.Value.GetType() != other.Value.GetType()) {
                Logging.GeneralLogger.Debug("KVObject comparison fails: this.Value.GetType() == other.Value.GetType()");
                return false;
            }

            if (this.GetValueAsString() != other.GetValueAsString()) {
                Logging.GeneralLogger.Debug("KVObject comparison fails: this.GetValueAsString() == other.GetValueAsString()");
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Deep clone this KVObject
    /// </summary>
    public KVObject Clone()
    {
        if (HasChildren) {
            var cloned = new KVObject(Name, new List<KVObject>());
            foreach (var item in Children)
            {
                cloned.Children.Add(item.Clone());
            }

            return cloned;
        } else {
            return new KVObject(Name, Value);
        }
    }

    public override int GetHashCode()
    {
        unchecked {
            int hash = Name.GetHashCode();
            if (HasChildren) {
                foreach (var item in Children)
                {
                    hash *= item.GetHashCode();
                }
            } else {
                hash *= Value.GetHashCode();
            }

            return hash;
        }
    }

    object ICloneable.Clone()
    {
        return this.Clone();
    }

    public bool Equals(KVObject? other)
    {
        return Equals(other, true);
    }
}