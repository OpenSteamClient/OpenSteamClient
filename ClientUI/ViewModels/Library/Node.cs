using System;
using System.Collections.ObjectModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels.Library;

public partial class Node : ViewModelBase, IComparable<Node> {
    [ObservableProperty]
    private string name = "";

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

    public ObservableCollection<Node> Children { get; protected set; } = new();
    public CGameID GameID { get; protected set; }

    public int CompareTo(Node? other)
    {
        return string.Compare(this.Name, other?.Name);
    }
}