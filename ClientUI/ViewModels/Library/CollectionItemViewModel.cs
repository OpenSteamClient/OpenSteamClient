using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClientUI.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels;

public partial class CollectionItemViewModel : ViewModelBase, INode {
    [ObservableProperty]
    private string name = "wtf";

    [ObservableProperty]
    private string id = "";

    public ObservableCollection<INode> Children { get; } = new();
    public CollectionItemViewModel(string id, string name) {
        this.Id = id;
        this.Name = name;
    }
}