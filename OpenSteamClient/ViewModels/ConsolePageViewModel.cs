using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamClient.Extensions;
using OpenSteamClient.Views;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.ConCommands;

namespace OpenSteamClient.ViewModels;

public partial class ConsolePageViewModel : AvaloniaCommon.ViewModelBase
{
    private class LogColorizer : DocumentColorizingTransformer
    {
        // This dictionary doesn't need to be concurrent, but let's keep it concurrent just in case
        public ConcurrentDictionary<int, Logger.Level> LinesByLevel { get; init; } = new();

        protected override void ColorizeLine(DocumentLine line)
        {
            if (!line.IsDeleted && LinesByLevel.TryGetValue(line.LineNumber, out Logger.Level value)) {
                ChangeLinePart(line.Offset, line.EndOffset, (vis) => SetColor(vis, value));
            }
        }

        void SetColor(VisualLineElement element, Logger.Level level)
        {
            IImmutableSolidColorBrush brush;
            switch (level)
            {
                case Logger.Level.DEBUG:
                    brush = Brushes.Gray;
                    break;

                case Logger.Level.WARNING:
                    brush = Brushes.Yellow;
                    break;

                case Logger.Level.ERROR:
                    brush = Brushes.DarkRed;
                    break;
                
                case Logger.Level.FATAL:
                    brush = Brushes.Red;
                    break;

                default:
                    return;
            }
            // This is where you do anything with the line
            element.TextRunProperties.SetForegroundBrush(brush);
        }
    }

    [ObservableProperty]
    private string currentCommandText = "";

    [ObservableProperty]
    private TextDocument outputText = new();

    [ObservableProperty]
    private TextEditorOptions options = new()
    {
        AllowScrollBelowDocument = false
    };

    private readonly LogColorizer colorizer = new();
    public ObservableCollection<string> AutocompleteNames { get; init; } = new();
    private readonly ConsolePage page;

    public ConsolePageViewModel(ConsolePage page)
    {
        this.page = page;

        foreach (var item in ConCommandHandler.ConCommands)
        {
            AutocompleteNames.Add(item.Key);
        }

        page.LogLines.TextArea.TextView.LineTransformers.Add(colorizer);
        Logger.DataReceived += OnDataReceived;
    }

    private void OnDataReceived(object? sender, Logger.DataReceivedEventArgs e)
    {
        AvaloniaApp.Current?.RunOnUIThread(DispatcherPriority.Background, () =>
        {
            if (e.FullLine) {
                int lc = OutputText.LineCount;
                colorizer.LinesByLevel[lc] = e.Level;
            }

            // This is the most performant and least memory intensive way of adding text.
            OutputText.Insert(OutputText.TextLength, e.Text);
            
            var current = page.LogLines.TextArea.Caret.Line;
            var length = OutputText.LineCount;
            
            // Jump to the end of the text if we're near enough, to keep the logs tracking
            if (current >= length - 20) {
                page.LogLines.TextArea.Caret.BringCaretToView();
            }
        });
    }

    public void SendCommand() {
        var cmd = CurrentCommandText;
        CurrentCommandText = "";
        ConCommandHandler.ExecuteConsoleCommand(cmd);
    }
}
