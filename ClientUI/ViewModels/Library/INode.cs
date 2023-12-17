using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;

namespace ClientUI.ViewModels.Library;

public interface INode {
    public string Name { get; set; }
    public Bitmap? Icon { get; set; }
    public ObservableCollection<INode> Children { get; }
}