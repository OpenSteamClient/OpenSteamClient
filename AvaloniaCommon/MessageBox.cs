namespace AvaloniaCommon;

using Avalonia.Controls;
using Avalonia.Threading;
using AvaloniaCommon.Enums;
using AvaloniaCommon.ViewModels;

public static class MessageBox
{
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

    public static MessageBoxButton? Show(string title, string header, string message, MessageBoxIcon icon = MessageBoxIcon.INFORMATION, MessageBoxButton buttons = MessageBoxButton.Ok)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            return Dispatcher.UIThread.Invoke(() => ShowInternal(title, header, message, icon, buttons));
        }
        else
        {
            return ShowInternal(title, header, message, icon, buttons);
        }
    }

    internal static MessageBoxButton? ShowInternal(string title, string header, string message, MessageBoxIcon icon, MessageBoxButton buttons)
    {
        var messageBoxViewModel = new MessageBoxViewModel(icon, buttons)
        {
            Title = title,
            Header = header,
            Content = message
        };

        var messageBox = new Views.MessageBox();
        messageBoxViewModel.SetMessageBox(messageBox);
        messageBox.DataContext = messageBoxViewModel;

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
