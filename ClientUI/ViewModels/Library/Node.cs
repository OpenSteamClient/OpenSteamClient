using System;
using System.Collections.ObjectModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels.Library;

public partial class Node : ViewModelBase, IComparable<Node>
{
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