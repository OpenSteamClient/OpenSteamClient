using Avalonia.Controls;
using ClientUI.Extensions;

namespace ClientUI.Views;

public partial class SelectInstallDirectoryDialog : Window
{
    public SelectInstallDirectoryDialog()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}