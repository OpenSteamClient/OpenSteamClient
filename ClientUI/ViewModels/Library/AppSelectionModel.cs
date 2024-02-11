using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls.Selection;

namespace ClientUI.ViewModels.Library;

public class AppSelectionModel : ISelectionModel
{
    private List<LibraryAppViewModel> selectedApps = new();
    public IEnumerable? Source { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool SingleSelect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int SelectedIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IReadOnlyList<int> SelectedIndexes => throw new NotImplementedException();

    public object? SelectedItem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IReadOnlyList<object?> SelectedItems => throw new NotImplementedException();

    public int AnchorIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int Count => throw new NotImplementedException();

    public event EventHandler<SelectionModelIndexesChangedEventArgs>? IndexesChanged;
    public event EventHandler<SelectionModelSelectionChangedEventArgs>? SelectionChanged;
    public event EventHandler? LostSelection;
    public event EventHandler? SourceReset;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void BeginBatchUpdate()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public void Deselect(int index)
    {
        throw new NotImplementedException();
    }

    public void DeselectRange(int start, int end)
    {
        throw new NotImplementedException();
    }

    public void EndBatchUpdate()
    {
        throw new NotImplementedException();
    }

    public bool IsSelected(int index)
    {
        throw new NotImplementedException();
    }

    public void Select(int index)
    {
        throw new NotImplementedException();
    }

    public void SelectAll()
    {
        throw new NotImplementedException();
    }

    public void SelectRange(int start, int end)
    {
        throw new NotImplementedException();
    }
}