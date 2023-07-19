using Common.Utils;

namespace ClientUI.ViewModels;

public class ProgressWindowViewModel : ViewModelBase
{
    public bool Throbber => _progress.Throbber;
    public int InitialProgress => _progress.InitialProgress;
    public int Progress => _progress.Progress;
    public int MaxProgress => _progress.MaxProgress;
    public string Operation => _progress.Operation;
    public string SubOperation => _progress.SubOperation;
    private ExtendedProgress<int> _progress;
    public ProgressWindowViewModel(ExtendedProgress<int> prog) {
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
