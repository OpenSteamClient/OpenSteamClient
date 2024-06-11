using OpenSteamworks.Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System;

namespace OpenSteamClient.ViewModels;

public partial class ProgressWindowViewModel : AvaloniaCommon.ViewModelBase
{
    [ObservableProperty]
    private string title = "Progress (Generic)";
    public bool Throbber => _exprogress.Throbber;
    public int InitialProgress => _exprogress.InitialProgress;
    public int Progress => _exprogress.Progress;
    public int MaxProgress => _exprogress.MaxProgress;
    public string Operation
    {
        get
        {
            if (Translations.ContainsKey(_exprogress.Operation))
            {
                return Translations[_exprogress.Operation];
            }
            return _exprogress.Operation;
        }
    }
    public string SubOperation
    {
        get
        {
            if (Translations.ContainsKey(_exprogress.SubOperation))
            {
                return Translations[_exprogress.SubOperation];
            }
            return _exprogress.SubOperation;
        }
    }
    public Dictionary<string, string> Translations = new();
    private readonly IExtendedProgress<int> _exprogress;

    public ProgressWindowViewModel(IExtendedProgress<int> prog, string title = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            this.Title = title;
        }

        _exprogress = prog;

        prog.ProgressChanged += (object? sender, int newProgress) =>
        {
            this.OnPropertyChanged(nameof(Throbber));
            this.OnPropertyChanged(nameof(Progress));
            this.OnPropertyChanged(nameof(MaxProgress));
            this.OnPropertyChanged(nameof(Operation));
            this.OnPropertyChanged(nameof(SubOperation));
        };
    }

    public ProgressWindowViewModel(Progress<string> operation, Progress<string> subOperation, int initialProgress = 0, Progress<int>? progress = null, Progress<int>? maxProgress = null, string title = ""): 
        this(new ExtendedProgressWrap<int>(operation, subOperation, initialProgress, progress, maxProgress), title) { }
}
