using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Media;
using OpenSteamworks.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Structs;

namespace OpenSteamClient.ViewModels.Library;

public abstract partial class Node : AvaloniaCommon.ViewModelBase, IComparable<Node>
{
    // This is real stupid. 
    // Avalonia breaks if CollectionViewModel (parent) has an override for a bound property, causing binding to fail for all LibraryAppViewModel (children)
    // So instead, do this hack.
    // TODO: Stop using TreeView.
    public string Name => ActualName;
    protected abstract string ActualName { get; }

    [ObservableProperty]
    private IBrush icon = Brushes.Transparent;

    [ObservableProperty]
    private IBrush statusIcon = Brushes.Transparent;

    [ObservableProperty]
    private bool hasIcon;

    [ObservableProperty]
    private bool isApp;

    [ObservableProperty]
    private bool isExpanded;

    public ObservableCollectionEx<Node> Children { get; protected set; } = new();
    public CGameID GameID { get; protected set; }

    public Node() {
        Children.CollectionChanged += OnChildrenChanged;
    }

    protected void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        base.OnPropertyChanged(nameof(Name));
    }

    public int CompareTo(Node? other)
    {
        return string.Compare(this.GetSortableName(), other?.GetSortableName());
    }

    public string GetSortableName() {
        if (!this.IsApp) {
            return this.Name;
        }

        if (this.Name.StartsWith("A ")) {
            return this.Name.Replace("A ", "");
        } else if (this.Name.StartsWith("The ")) {
            return this.Name.Replace("The ", "");
        }

        return this.Name;
    }
}