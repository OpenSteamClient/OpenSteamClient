using Avalonia.Controls;
using ClientUI.ViewModels;
using Common.Utils;

namespace ClientUI.Views;

public partial class ProgressWindow : Window
{
    public ProgressWindow(ProgressWindowViewModel vm)
    {
        this.DataContext = vm;
        InitializeComponent();
    }
}