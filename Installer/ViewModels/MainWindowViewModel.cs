using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Installer.Enums;
using Installer.Extensions;
using Installer.Translation;
using Installer.ViewModels.Pages;
using Installer.Views;

namespace Installer.ViewModels;

public partial class MainWindowViewModel : AvaloniaCommon.ViewModelBase {
    [ObservableProperty]
    private UserControl currentPage;

    [ObservableProperty]
    private Bitmap openSteamLogo;
    
    public bool CanClose => OnCancel != null;
    public bool PreviousEnabled => CanClose && OnPrevious != null;
    public bool NextEnabled => CanClose && OnNext != null;

    [NotifyPropertyChangedFor(nameof(CanClose))]
    [NotifyPropertyChangedFor(nameof(PreviousEnabled))]
    [NotifyPropertyChangedFor(nameof(NextEnabled))]
    [ObservableProperty]
    private ICommand? onCancel;

    [NotifyPropertyChangedFor(nameof(PreviousEnabled))]
    [ObservableProperty]
    private ICommand? onPrevious;

    [NotifyPropertyChangedFor(nameof(NextEnabled))]
    [ObservableProperty]
    private ICommand? onNext;

    [ObservableProperty]
    private LanguageViewModel selectedLanguage;

    public LinearGradientBrush PanelBackground { get; init; }
    public ObservableCollection<LanguageViewModel> Languages { get; init; } = new();

    private readonly MainWindow window;
    private readonly List<UserControl> Pages = new();

    private bool canGoBack = true;
    public bool CanGoBack {
        get => canGoBack;
        set {
            canGoBack = value;
            UpdateActions();
        }
    }

    public InstallAction AvailableActions { get; set; }
    private InstallAction selectedAction = InstallAction.None;
    public InstallAction SelectedAction {
        get => selectedAction;
        set {
            selectedAction = value;
            UpdateActions();
        }
    }

    private bool canGoNext = false;
    public bool CanGoNext {
        get => canGoNext;
        set {
            canGoNext = value;
            UpdateActions();
        }
    }
    
    private bool HasPrevious => CanGoBack && Pages.IndexOf(CurrentPage) > 0;
    private bool HasNext => CanGoNext && Pages.IndexOf(CurrentPage) + 1 < Pages.Count;
    private readonly UserControl chooseActionPage;
    private readonly UserControl placeholderPage;
    private readonly PlaceholderPageViewModel placeholderPageViewModel;

    public MainWindowViewModel(MainWindow window) {
        this.PanelBackground = new LinearGradientBrush();
        PanelBackground.StartPoint = new Avalonia.RelativePoint(0, 0, Avalonia.RelativeUnit.Relative);
        PanelBackground.EndPoint = new Avalonia.RelativePoint(1, 1, Avalonia.RelativeUnit.Relative);
        PanelBackground.GradientStops.Add(new GradientStop() {
            Offset = 0,
            Color = Color.Parse("#ff0000"),
        });

        PanelBackground.GradientStops.Add(new GradientStop() {
            Offset = 1,
            Color = Color.Parse("#7b0000"),
        });

        foreach (var item in TranslationManager.ValidUILanguages)
        {
            Languages.Add(new LanguageViewModel(item));
        }

        SelectedLanguage = Languages.First(l => AvaloniaApp.TranslationManager.CurrentTranslation.Language == l.ELang);

        this.window = window;

        //TODO: check install status here and determine the available actions
        AvailableActions = InstallAction.Install | InstallAction.Repair | InstallAction.Uninstall;

        {
            var page = new WelcomePage();
            page.DataContext = new WelcomePageViewModel(this);
            Pages.Add(page);
        }

        {
            chooseActionPage = new ChooseActionPage();
            chooseActionPage.DataContext = new ChooseActionPageViewModel(this);
            Pages.Add(chooseActionPage);
        }

        {
            placeholderPage = new PlaceholderPage();
            placeholderPageViewModel = new PlaceholderPageViewModel();
            placeholderPage.DataContext = placeholderPageViewModel;
            Pages.Add(placeholderPage);
        }

        {
            var page = new InstallingPage();
            page.DataContext = new InstallingPageViewModel();
            Pages.Add(page);
        }
        
        this.CurrentPage = Pages.First();

        this.OpenSteamLogo = new Bitmap(AssetLoader.Open(new Uri("avares://Installer/Assets/opensteam-logo.ico")));
        this.PropertyChanged += InternalOnPropertyChanged;
        this.OnCancel = new RelayCommand(Cancel);
        this.OnNext = new RelayCommand(Next);

        window.Closing += OnClosing;

        UpdateActions();
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = !CanClose;
    }

    private void InternalOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CanClose)) {
            if (CanClose) {
                WindowToolbar.EnableCloseButton(window);
            } else {
                WindowToolbar.DisableCloseButton(window);
            }
        }

        if (e.PropertyName == nameof(SelectedLanguage)) {
            AvaloniaApp.TranslationManager.SetLanguage(SelectedLanguage.ELang);
        }
    }

    private void Cancel() {
        AvaloniaApp.Current?.MainWindow?.Close();
    }

    private void Previous() {
        if (HasPrevious) {
            CurrentPage = Pages.ElementAt(Pages.IndexOf(CurrentPage) - 1);
        }

        UpdateActions();
    }

    private void Next() {
        if (HasNext) {
            var nextPage = Pages.ElementAt(Pages.IndexOf(CurrentPage) + 1);
            if (nextPage is PlaceholderPage) {
                switch (SelectedAction)
                {
                    case InstallAction.None:
                        placeholderPageViewModel.InternalControl = new TextBlock() {
                            Text = "No install action selected! (You should not be able to see this screen)"
                        };
                        break;
                    case InstallAction.Install:
                        var page = new ChooseInstallDirectoryPage();
                        page.DataContext = new ChooseInstallDirectoryPageViewModel(page, this);
                        placeholderPageViewModel.InternalControl = page;
                        break;
                    case InstallAction.Repair:
                        placeholderPageViewModel.InternalControl = new TextBlock() {
                            Text = "Repair page"
                        };
                        break;
                    case InstallAction.Uninstall:
                        placeholderPageViewModel.InternalControl = new TextBlock() {
                            Text = "Uninstall page"
                        };
                        break;
                }
            }

            if (nextPage is InstallingPage) {
                OnCancel = null;
            }

            CurrentPage = nextPage;
        }

        UpdateActions();
    }

    private void UpdateActions() {
        if (HasPrevious) {
            OnPrevious = new RelayCommand(Previous);
        } else {
            OnPrevious = null;
        }

        if (CurrentPage is ChooseActionPage) {
            if (SelectedAction == InstallAction.None) {
                OnNext = null;
                return;
            }
        }

        if (!CanGoNext) {
            OnNext = null;
            return;
        }

        Console.WriteLine("CurrentPage is " + (CurrentPage == null ? "null" : CurrentPage.GetType().Name));
        Console.WriteLine("InternalControl is " + (placeholderPageViewModel.InternalControl == null ? "null" : placeholderPageViewModel.InternalControl!.GetType().Name));

        if (HasNext) {
            OnNext = new RelayCommand(Next);

            var nextBtn = window.FindControlNested<Button>("NextButton");
            string translation = AvaloniaApp.TranslationManager.GetTranslationForKey("#Dialog_Next");
            if (CurrentPage is PlaceholderPage) {
                if (placeholderPageViewModel.InternalControl is ChooseInstallDirectoryPage) {
                    translation = AvaloniaApp.TranslationManager.GetTranslationForKey("#Dialog_Install");
                }
            }

            if (nextBtn != null) {
                nextBtn.Content = translation;
            }
        } else {
            var nextBtn = window.FindControlNested<Button>("NextButton");
            if (nextBtn != null) {
                nextBtn.Content = AvaloniaApp.TranslationManager.GetTranslationForKey("#Dialog_Finish");
            }

            OnNext = new RelayCommand(Cancel);
        }
    }
}