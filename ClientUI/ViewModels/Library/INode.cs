using System.Collections.ObjectModel;

namespace ClientUI.ViewModels.Library;

public interface INode {
    public string Name { get; set; }
    public ObservableCollection<INode> Children { get; }
}