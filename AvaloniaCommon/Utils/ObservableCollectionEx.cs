using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace AvaloniaCommon.Utils;

//TODO: This needs to be looked at.
// For some reason it sometimes crashes avalonia, even though we do everything properly
[Serializable]
[DebuggerDisplay("Count = {Count}")]
public class ObservableCollectionEx<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList, INotifyCollectionChanged, INotifyPropertyChanged
{
    private readonly List<T> underlyingCollection;
    private List<T>? filteredCollection;
    private Predicate<T>? filter;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsFiltered => filteredCollection != null;

    // This name is a bit ambiguous, maybe change this?
    private List<T> actualCollection {
        get {
            if (filteredCollection == null) {
                return underlyingCollection;
            }

            return filteredCollection;
        }
    }
    
    public ObservableCollectionEx(IEnumerable<T> collection) {
        underlyingCollection = new(collection);
    }

    public ObservableCollectionEx() {
        underlyingCollection = new();
    }
    
    /// <summary>
    /// Filters the current collection with the current data (which may be filtered as well), not affecting the underlying data source.
    /// </summary>
    public void FilterCurrent(Predicate<T> del) {
        this.filteredCollection = actualCollection.Where(x => del(x)).ToList();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <summary>
    /// Filters the current collection with the original data (which will never be filtered), not affecting the underlying data source.
    /// </summary>
    public void FilterOriginal(Predicate<T> del) {
        this.filteredCollection = underlyingCollection.Where(x => del(x)).ToList();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void SetFilter(Predicate<T> del) {
        this.filter = del;
        FilterOriginal(del);
    }

    /// <summary>
    /// Save and use the selected predicate for filtering now, and every time the collection is updated. Uses FilterOriginal.
    /// This may be inefficient, since it fires Reset events for every action.
    /// </summary>
    /// <param name="match"></param>
    public void FilterOriginalOnChange(Predicate<T> match) {
        this.filter = match;
        FilterOnChange();
    }

    private bool FilterOnChange() {
        if (this.filter != null) {
            this.filteredCollection = underlyingCollection.Where(x => filter(x)).ToList();
            return true;
        }

        return false;
    }

    private int prevCount = -1;
    private void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
        // if (FilterOnChange()) {
        //     Console.WriteLine("Firing reset");
        //     CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        // } else {
        //     CollectionChanged?.Invoke(this, e);
        // }

        // Always reset to avoid bugs.
        Console.WriteLine("Firing reset");
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        
        if (prevCount != Count) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }

        prevCount = Count;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }

    /// <summary>
    /// Removes filtering.
    /// </summary>
    public void ClearFilter() {
        this.filter = null;
        if (filteredCollection != null) {
            filteredCollection = null;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public int RemoveAll(Predicate<T> match) {
        Dictionary<int, T> items = new();
        foreach (var item in underlyingCollection)
        {
            if (match(item)) {
                items.Add(underlyingCollection.IndexOf(item), item);
            }
        }

        var ret = underlyingCollection.RemoveAll(match);

        //TODO: This should calculate the indexes correctly, but for now let's do this (performance)
        foreach (var item in items)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>() { item.Value }, item.Key - 1));
        }

        return ret;
    }

    public T? Find(Predicate<T> match) 
        => actualCollection.Find(match);

    public List<T> FindAll(Predicate<T> match) 
        => actualCollection.FindAll(match);

    public void UnionWith(IEnumerable<T> items) {
        AddRange(items.Where(i => !underlyingCollection.Contains(i)));
    }

    public bool AddUnique(T item) {
        if (underlyingCollection.Contains(item)) {
            return false;
        }

        Add(item);
        return true;
    }

    public void AddRange(IEnumerable<T> items) {
        var idx = underlyingCollection.Count;
        underlyingCollection.AddRange(items);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToList(), idx));
    }

    public void Sort() {
        underlyingCollection.Sort();
        filteredCollection?.Sort();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }


    // Below is the interfaces that need to be implemented for this to be a collection. Don't worry about these

    /// <summary>
    /// The count of the elements, filtered
    /// </summary>
    public int Count => ((ICollection<T>)actualCollection).Count;

    /// <summary>
    /// The count of the elements, unfiltered
    /// </summary>
    public int FullCount => ((ICollection<T>)underlyingCollection).Count;

    public bool IsReadOnly => ((ICollection<T>)underlyingCollection).IsReadOnly;

    public bool IsSynchronized => ((ICollection)underlyingCollection).IsSynchronized;

    public object SyncRoot => ((ICollection)underlyingCollection).SyncRoot;

    public bool IsFixedSize => ((IList)underlyingCollection).IsFixedSize;

    // I don't know if this is best practice.
    object? IList.this[int index] { get => ((IList)actualCollection)[index]; set => underlyingCollection.Insert(index, (T)value!); }
    
    public T this[int index] { get => ((IList<T>)actualCollection)[index]; set => underlyingCollection.Insert(index, value); }

    public void Add(T item)
    {
        ((ICollection<T>)underlyingCollection).Add(item);
        var index = underlyingCollection.IndexOf(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>() { item }, index));
    }

    public void Clear()
    {
        ((ICollection<T>)underlyingCollection).Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(T item)
    {
        return ((ICollection<T>)actualCollection).Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ((ICollection<T>)actualCollection).CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        var index = underlyingCollection.IndexOf(item);
        var ret = underlyingCollection.Remove(item);
        if (ret) {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>() { item }, index - 1));
        }

        return ret;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)actualCollection).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return ((System.Collections.IEnumerable)actualCollection).GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return ((IList<T>)actualCollection).IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        ((IList<T>)underlyingCollection).Insert(index, item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>() { item }, index));
    }

    public void RemoveAt(int index)
    {
        var item = underlyingCollection[index];
        ((IList<T>)underlyingCollection).RemoveAt(index);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>() { item }, index - 1));
    }

    public void CopyTo(Array array, int index)
    {
        ((ICollection)actualCollection).CopyTo(array, index);
    }

    public int Add(object? value)
    {
        var ret = ((IList)underlyingCollection).Add(value);
        var index = ((IList)underlyingCollection).IndexOf(value);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<object?>() { value }, index));
        return ret;
    }

    public bool Contains(object? value)
    {
        return ((IList)actualCollection).Contains(value);
    }

    public int IndexOf(object? value)
    {
        return ((IList)actualCollection).IndexOf(value);
    }

    public void Insert(int index, object? value)
    {
        ((IList)underlyingCollection).Insert(index, value);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<object?>() { value }, index));
    }

    public void Remove(object? value)
    {
        var index = ((IList)underlyingCollection).IndexOf(value);
        if (index > -1) {
            ((IList)underlyingCollection).Remove(value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<object?>() { value }, index - 1));
        }
    }
}