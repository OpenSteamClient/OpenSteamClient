using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace OpenSteamworks.Utils;

public class ReadOnlyCollectionEx<T> : ICollection<T>
{
    private readonly ICollection<T> decoratedCollection;

    public ReadOnlyCollectionEx(ICollection<T> decorated_collection)
    {
        decoratedCollection = decorated_collection;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return decoratedCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)decoratedCollection).GetEnumerator();
    }

    public void Add(T item)
    {
        throw new NotSupportedException();
    }

    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Contains(T item)
    {
        return decoratedCollection.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        decoratedCollection.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        throw new NotSupportedException();
    }

    public int Count
    {
        get { return decoratedCollection.Count; }
    }

    public bool IsReadOnly
    {
        get { return true; }
    }
}