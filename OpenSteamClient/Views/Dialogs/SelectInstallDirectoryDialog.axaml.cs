using Avalonia.Controls;
using OpenSteamClient.Extensions;

namespace OpenSteamClient.Views;

public partial class SelectInstallDirectoryDialog : Window
{
    public SelectInstallDirectoryDialog()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}