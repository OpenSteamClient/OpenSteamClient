using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ClientUI.Enums;
using ClientUI.ViewModels;

namespace ClientUI;

public partial class MessageBox : Window
{
    public MessageBox(MessageBoxViewModel vm)
    {
        vm.SetMessageBox(this);
        this.DataContext = vm;
        InitializeComponent();
    }

    public void Copy()
    {
        MessageBoxViewModel? viewModel = (this.DataContext as MessageBoxViewModel);
        if (viewModel == null) {
            Console.WriteLine("DataContext failed to cast to MessageBoxViewModel! Copy failed!");
            return;
        }

        string fullTextToCopy = "";
        fullTextToCopy += viewModel.Title + "\n";
        fullTextToCopy += viewModel.Header + "\n";
        fullTextToCopy += viewModel.Content + "\n";
        fullTextToCopy += viewModel.EnabledButtons.ToString();

        if (this.Clipboard == null) {
            Console.WriteLine("this.Clipboard is null! Copy failed!");
            return;
        }

        this.Clipboard.SetTextAsync(fullTextToCopy);
    }

    public void QueueClose()
    {
        Dispatcher.UIThread.InvokeAsync(base.Close);
    }

    public static MessageBoxButton? Show(string title, string message)
    {
        return Show(title, "", message);
    }

    public static MessageBoxButton? Error(string title, string message)
    {
        return Error(title, "", message);
    }

    public static MessageBoxButton? Error(Exception e)
    {
        return Error(string.IsNullOrEmpty(e.Message) ? "Unknown error" : e.Message, e.Message, e.ToString());
    }
    public static MessageBoxButton? QuestionYesNo(string title, string header, string message)
    {
        return Show(title, header, message, MessageBoxIcon.QUESTION, MessageBoxButton.No | MessageBoxButton.Yes);
    }
    public static MessageBoxButton? QuestionYesNoCancel(string title, string header, string message)
    {
        return Show(title, header, message, MessageBoxIcon.QUESTION, MessageBoxButton.No | MessageBoxButton.Yes | MessageBoxButton.Cancel);
    }
    public static MessageBoxButton? Info(string title, string header, string message)
    {
        return Show(title, header, message, MessageBoxIcon.INFORMATION);
    }
    public static MessageBoxButton? Warning(string title, string header, string message)
    {
        return Show(title, header, message, MessageBoxIcon.WARNING);
    }
    public static MessageBoxButton? Error(string title, string header, string message)
    {
        return Show(title, header, message, MessageBoxIcon.ERROR);
    }

    public static MessageBoxButton? Show(string title, string header, string message)
    {
        return Show(title, header, message, MessageBoxIcon.INFORMATION);
    }

    public static MessageBoxButton? Show(string title, string header, string message, MessageBoxIcon icon = MessageBoxIcon.INFORMATION, MessageBoxButton buttons = MessageBoxButton.Ok) {
        var messageBoxViewModel = new MessageBoxViewModel(icon, buttons);
        messageBoxViewModel.Title = title;
        messageBoxViewModel.Header = header;
        messageBoxViewModel.Content = message;

        var messageBox = new MessageBox(messageBoxViewModel);

        using (var source = new CancellationTokenSource())
        {
            var tcs = new TaskCompletionSource<MessageBoxButton?>();

            messageBox.Closing += (object? sender, WindowClosingEventArgs args) =>
            {
                tcs.TrySetResult(messageBoxViewModel.ButtonClicked);
                args.Cancel = false;
            };

            messageBox.Show();

            tcs.Task.ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
            Dispatcher.UIThread.MainLoop(source.Token);
            return tcs.Task.Result;
        }
    }
    
}