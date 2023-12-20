using System.Collections.ObjectModel;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels.Library;

public interface INode {
    public string Name { get; set; }
    public bool HasIcon { get; }
    public IBrush Icon { get; set; }
    public ObservableCollection<INode> Children { get; }
    public bool IsApp { get; }
    public CGameID GameID { get; }
}