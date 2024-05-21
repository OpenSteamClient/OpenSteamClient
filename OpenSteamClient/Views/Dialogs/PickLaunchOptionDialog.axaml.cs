using Avalonia.Controls;
using OpenSteamClient.Extensions;

namespace OpenSteamClient.Views;

public partial class PickLaunchOptionDialog : Window
{
    public PickLaunchOptionDialog()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}