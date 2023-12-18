using System.Collections.ObjectModel;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ClientUI.ViewModels.Library;

public interface INode {
    public string Name { get; set; }
    public IBrush Icon { get; set; }
    public ObservableCollection<INode> Children { get; }
}