using Avalonia.Controls;
using ClientUI.ViewModels;

namespace ClientUI.Views;

public partial class ProgressWindow : Window
{
    public ProgressWindow()
    {
        InitializeComponent();
        this.DataContext = new ProgressWindowViewModel();
    }
}