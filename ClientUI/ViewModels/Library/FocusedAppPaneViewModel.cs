using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ClientUI.ViewModels.Library;
using ClientUI.Views;
using ClientUI.Views.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using SkiaSharp;

namespace ClientUI.ViewModels.Library;

public partial class FocusedAppPaneViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private IBrush hero;

    [ObservableProperty]
    private IBrush logo;

    [NotifyPropertyChangedFor(nameof(HasLogoAsText))]
    [ObservableProperty]
    private string logoAsText;

    [ObservableProperty]
    private string playButtonLocalizationToken;

    [ObservableProperty]
    private ICommand playButtonAction;

    [ObservableProperty]
    private double logoHeight;

    [ObservableProperty]
    private double logoWidth;

    [ObservableProperty]
    private double logoLeft;

    [ObservableProperty]
    private double logoTop;

    [ObservableProperty]
    private double logoRight;

    [ObservableProperty]
    private double logoBottom;

    [ObservableProperty]
    private VerticalAlignment logoVerticalAlignment;

    [ObservableProperty]
    private HorizontalAlignment logoHorizontalAlignment;

    public double HeroHeight => 450;
    public double HeroWidth => 1400;
    public double LogoContainerHeight => HeroHeight - 32;
    public double LogoContainerWidth => HeroWidth - 32;
    public bool HasLogoAsText => !string.IsNullOrEmpty(LogoAsText);

    private readonly AppBase app;

    public FocusedAppPaneViewModel(CGameID gameid)
    {
        LogoAsText = "";
        app = AvaloniaApp.Container.Get<AppsManager>().GetApp(gameid);
        AvaloniaApp.Container.Get<CallbackManager>().RegisterHandler<AppEventStateChange_t>(OnAppEventStateChange);
        this.Name = app.Name;
        SetLibraryAssets();

        app.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
        PlayButtonLocalizationToken = "Initial state";
        PlayButtonAction = new RelayCommand(InvalidAction);
        UpdatePlayButton(app.State);
    }

    private void OnAppEventStateChange(CallbackManager.CallbackHandler<AppEventStateChange_t> handler, AppEventStateChange_t change)
    {
        UpdatePlayButton(change.m_eNewState);
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(playButtonAction))]
    [MemberNotNull(nameof(PlayButtonAction))]
    [MemberNotNull(nameof(playButtonLocalizationToken))]
    [MemberNotNull(nameof(PlayButtonLocalizationToken))]
#pragma warning restore MVVMTK0034
    private void UpdatePlayButton(EAppState state)
    {
        if (state.HasFlag(EAppState.AppRunning))
        {
            PlayButtonLocalizationToken = "#App_StopApp";
            PlayButtonAction = new RelayCommand(KillApp);
        }
        else if (state.HasFlag(EAppState.Terminating))
        {
            PlayButtonLocalizationToken = "#App_StoppingApp";
            PlayButtonAction = new RelayCommand(InvalidAction);
        }
        else if (state.HasFlag(EAppState.UpdateRunning))
        {
            PlayButtonLocalizationToken = "#App_PauseAppUpdate";
            PlayButtonAction = new RelayCommand(PauseUpdate);
        }
        else if (state.HasFlag(EAppState.UpdateRequired) || state.HasFlag(EAppState.UpdatePaused) || state.HasFlag(EAppState.UpdateQueued))
        {
            PlayButtonLocalizationToken = "#App_UpdateApp";
            PlayButtonAction = new RelayCommand(Update);
        }
        else if (app.LaunchOptions.Any() && state == EAppState.FullyInstalled)
        {
            if (app.Type == EAppType.Game)
            {
                PlayButtonLocalizationToken = "#App_PlayApp";
                PlayButtonAction = new RelayCommand(Launch);
            }
            else
            {
                PlayButtonLocalizationToken = "#App_LaunchApp";
                PlayButtonAction = new RelayCommand(Launch);
            }
        } else if (state == EAppState.Uninstalled) {
            PlayButtonLocalizationToken = "#App_InstallApp";
            PlayButtonAction = new RelayCommand(RequestInstall);
        } else if ((!app.LaunchOptions.Any() || !app.IsOwnedAndPlayable) && state == EAppState.FullyInstalled) {
            PlayButtonLocalizationToken = "#App_UninstallApp";
            PlayButtonAction = new RelayCommand(Uninstall);
        }
        else
        {
            PlayButtonLocalizationToken = "Unknown state: " + state.ToString();
            PlayButtonAction = new RelayCommand(InvalidAction);
        }
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(hero))]
    [MemberNotNull(nameof(Hero))]
    [MemberNotNull(nameof(logo))]
    [MemberNotNull(nameof(Logo))]
#pragma warning restore MVVMTK0034
    private void SetLibraryAssets()
    {

        if (app.LocalHeroPath == null && app.LocalLogoPath == null) {
            LogoAsText = app.Name;
        }

        if (app.LocalHeroPath != null)
        {
            this.Hero = new ImageBrush()
            {
                Source = new Bitmap(app.LocalHeroPath)
            };
        } else {
            this.Hero = Brushes.Transparent;
        }

        if (app.LocalLogoPath != null)
        {
            this.Logo = new ImageBrush()
            {
                Source = new Bitmap(app.LocalLogoPath),
            };

            if (app.LibraryAssetAlignment != null) {
                this.LogoHeight = (this.HeroHeight / 100) * app.LibraryAssetAlignment.LogoHeightPercentage;
                this.LogoWidth = (this.HeroWidth / 100) * app.LibraryAssetAlignment.LogoWidthPercentage;
                if (app.LibraryAssetAlignment.LogoPinnedPosition == "CenterCenter") {
                    LogoHorizontalAlignment = HorizontalAlignment.Center;
                    LogoVerticalAlignment = VerticalAlignment.Center;
                } else if (app.LibraryAssetAlignment.LogoPinnedPosition == "UpperCenter") {
                    LogoHorizontalAlignment = HorizontalAlignment.Center;
                    LogoVerticalAlignment = VerticalAlignment.Top;
                } else if (app.LibraryAssetAlignment.LogoPinnedPosition == "BottomCenter") {
                    LogoHorizontalAlignment = HorizontalAlignment.Center;
                    LogoVerticalAlignment = VerticalAlignment.Bottom;
                } else if (app.LibraryAssetAlignment.LogoPinnedPosition == "BottomLeft") {
                    LogoHorizontalAlignment = HorizontalAlignment.Left;
                    LogoVerticalAlignment = VerticalAlignment.Bottom;
                } else {
                    throw new InvalidOperationException("Unhandled alignment: " + app.LibraryAssetAlignment.LogoPinnedPosition);
                }
            }
        }
        else
        {
            this.Logo = Brushes.Transparent;
        }
    }

    public void OnLibraryAssetsUpdated(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Invoke(SetLibraryAssets);
    }

    private void InvalidAction()
    {
        throw new InvalidOperationException("Nothing to do");
    }

    private void PauseUpdate()
    {
        this.app.PauseUpdate();
    }

    private void Update()
    {
        this.app.Update();
    }

    private void KillApp()
    {
        this.app.Kill();
    }

    private void Uninstall() {
        //this.app.Uninstall();
    }

    private void RequestInstall() {
        UtilityFunctions.Assert(app is SteamApp);
        SelectInstallDirectoryDialog dialog = new();
        dialog.DataContext = AvaloniaApp.Container.ConstructOnly<SelectInstallDirectoryDialogViewModel>(dialog, (app as SteamApp)!);

        AvaloniaApp.Current?.TryShowDialog(dialog);
    }


    private void Launch()
    {
        if (this.app.DefaultLaunchOptionID != null)
        {
            this.app.Launch("", this.app.DefaultLaunchOptionID.Value);
        }
    }
}