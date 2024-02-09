using System.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Installer.ViewModels.Pages;

public partial class WelcomePageViewModel : ViewModelBase {
    [ObservableProperty]
    private bool sSAAgreed = false;

    [ObservableProperty]
    private bool oSCDevelopersAgreed = false;

    private readonly MainWindowViewModel mainWindowViewModel;
    public WelcomePageViewModel(MainWindowViewModel mainWindowViewModel) {
        this.mainWindowViewModel = mainWindowViewModel;
        this.PropertyChanged += InternalOnPropertyChanged;
    }

    private void InternalOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SSAAgreed) || e.PropertyName == nameof(OSCDevelopersAgreed)) {
            mainWindowViewModel.CanGoNext = SSAAgreed && OSCDevelopersAgreed;
        }
    }

    public void ShowSSA() {
        ProcessStartInfo processInfo = new ProcessStartInfo
        {
            FileName = AvaloniaApp.TranslationManager.GetTranslationForKey("#LocalizedSteamSSALink"),
            UseShellExecute = true
        };
        
        Process.Start(processInfo);
    }
}