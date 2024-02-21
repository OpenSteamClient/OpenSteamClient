using Avalonia.Controls;
using OpenSteamClient.ViewModels;
using OpenSteamworks.Client.Utils;

namespace OpenSteamClient.Views;

public partial class ProgressWindow : Window
{
    public ProgressWindow(ProgressWindowViewModel vm)
    {
        this.DataContext = vm;
        InitializeComponent();
    }
}