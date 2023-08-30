using OpenSteamworks.Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ClientUI.ViewModels;

public partial class ProgressWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string title = "Progress (Generic)";
    public bool Throbber => _progress.Throbber;
    public int InitialProgress => _progress.InitialProgress;
    public int Progress => _progress.Progress;
    public int MaxProgress => _progress.MaxProgress;
    public string Operation {
        get {
            if (Translations.ContainsKey(_progress.Operation)) {
                return Translations[_progress.Operation];
            }
            return _progress.Operation;
        }
    }
    public string SubOperation {
        get {
            if (Translations.ContainsKey(_progress.SubOperation)) {
                return Translations[_progress.SubOperation];
            }
            return _progress.SubOperation;
        }
    }
    public Dictionary<string, string> Translations = new();
    private ExtendedProgress<int> _progress;
    public ProgressWindowViewModel(ExtendedProgress<int> prog, string title = "") {
        if (!string.IsNullOrEmpty(title)) {
            this.Title = title;
        }
        
        _progress = prog;
        
        _progress.ProgressChanged += (object? sender, int newProgress) => {
            this.OnPropertyChanged("Throbber");
            this.OnPropertyChanged("Progress");
            this.OnPropertyChanged("MaxProgress");
            this.OnPropertyChanged("Operation");
            this.OnPropertyChanged("SubOperation");
        };
        
    }
}
