using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenSteamworks.Downloads;

/// <summary>
/// A list that can have items inserted in any order. The list will collapse empty gaps automatically when converting to list for enumeration or ConvertToList is called. The operation does not affect the backing store, which always stays ordered.
/// </summary>
/// <typeparam name="T"></typeparam>
public class AbstractOrderedList<T> : IEnumerable<T>, IList<T> where T: notnull {
    private readonly Dictionary<int, T> backingStore = new();

    public int Count => backingStore.Count;
    public bool IsReadOnly => false;
    public T this[int index] { 
        get => backingStore[index]; 
        set => backingStore[index] = value; 
    }

    public AbstractOrderedList() {
        
    }

    public AbstractOrderedList(IEnumerable<T> other) {
        int index = 0;
        foreach (var item in other) {
            backingStore[index] = item;
            index++;
        }
    }

    public List<T> ConvertToList() {
        List<T> list = new();
        foreach (var item in backingStore.OrderBy(k => k.Key))
        {
            list.Add(item.Value);
        }

        return list;
    }

    /// <summary>
    /// Inserts an item at a specific index. Will override previous items.
    /// </summary>
    public void Insert(int index, T item) {
        backingStore[index] = item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ConvertToList().GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ConvertToList().GetEnumerator();
    }

    public int IndexOf(T item)
    {
        KeyValuePair<int, T> firstOrDefault = backingStore.FirstOrDefault(i => (object?)i.Value == (object?)item);
        if ((object?)firstOrDefault.Value == (object?)default(T)) {
            return -1;
        }

        return firstOrDefault.Key;
    }

    public void RemoveAt(int index)
    {
        backingStore.Remove(index);
    }

    public void Add(T item)
    {
        backingStore.Add(backingStore.Count, item);
    }

    public void Clear()
    {
        backingStore.Clear();
    }

    public bool Contains(T item)
    {
        return IndexOf(item) != -1;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ConvertToList().CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        var idx = IndexOf(item);
        if (idx == -1) {
            return false;
        }

        return backingStore.Remove(idx);
    }
}