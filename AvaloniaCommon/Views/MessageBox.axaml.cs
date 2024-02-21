using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using AvaloniaCommon.Enums;
using AvaloniaCommon.ViewModels;

namespace AvaloniaCommon.Views;

public partial class MessageBox : Window
{
    public MessageBox()
    {
        InitializeComponent();
    }

    public void Copy()
    {
        MessageBoxViewModel? viewModel = (this.DataContext as MessageBoxViewModel);
        if (viewModel == null)
        {
            Console.WriteLine("DataContext failed to cast to MessageBoxViewModel! Copy failed!");
            return;
        }

        string fullTextToCopy = "";
        fullTextToCopy += viewModel.Title + "\n";
        fullTextToCopy += viewModel.Header + "\n";
        fullTextToCopy += viewModel.Content + "\n";
        fullTextToCopy += viewModel.EnabledButtons.ToString();

        if (this.Clipboard == null)
        {
            Console.WriteLine("this.Clipboard is null! Copy failed!");
            return;
        }

        this.Clipboard.SetTextAsync(fullTextToCopy);
    }

    public void QueueClose()
    {
        Dispatcher.UIThread.InvokeAsync(base.Close);
    }
}