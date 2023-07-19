namespace ClientUI;

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using MsBox.Avalonia;

public static class MessageBox
{
    public static void Show(string title, string header, string message)
    {
        GetMessageBoxStandardWrap(new MsBox.Avalonia.Dto.MessageBoxStandardParams { ContentTitle = title, ContentMessage = message, ContentHeader = header });
    }

    public static void Show(string title, string message)
    {
        GetMessageBoxStandardWrap(new MsBox.Avalonia.Dto.MessageBoxStandardParams { ContentTitle = title, ContentMessage = message });
    }

    /// <summary>
    /// Like Error(Exception), but rethrows the error
    /// </summary>
    public static void FatalError(Exception e)
    {
        Error(e);
        throw e;
    }
    public static void Error(Exception e)
    {
        Error(string.IsNullOrEmpty(e.Message) ? "Unknown error" : e.Message, e.Message, (e.StackTrace != null) ? e.StackTrace : "no stacktrace");
    }

    public static void Error(string title, string header, string message)
    {
        Show(title, header, message, MsBox.Avalonia.Enums.Icon.Error);
    }

    public static void Show(string title, string header, string message, MsBox.Avalonia.Enums.Icon icon) {
        GetMessageBoxStandardWrap(new MsBox.Avalonia.Dto.MessageBoxStandardParams { ContentTitle = title, ContentMessage = message, ContentHeader = header, Icon = icon });
    }

    public static void Show(string title, string message, MsBox.Avalonia.Enums.Icon icon) {
        GetMessageBoxStandardWrap(new MsBox.Avalonia.Dto.MessageBoxStandardParams { ContentTitle = title, ContentMessage = message, Icon = icon });
    }
    private static void GetMessageBoxStandardWrap(MsBox.Avalonia.Dto.MessageBoxStandardParams @params) {
        using (var source = new CancellationTokenSource())
        {
            MessageBoxManager.GetMessageBoxStandard(@params).ShowAsync().ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
            Dispatcher.UIThread.MainLoop(source.Token);
        }
    }
}