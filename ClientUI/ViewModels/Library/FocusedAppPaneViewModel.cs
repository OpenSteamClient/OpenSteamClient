using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ClientUI.ViewModels.Library;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

namespace ClientUI.ViewModels.Library;

public partial class FocusedAppPaneViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private IBrush hero;

    [ObservableProperty]
    private IBrush logo;

    [ObservableProperty]
    private string playButtonLocalizationToken;

    [ObservableProperty]
    private ICommand playButtonAction;

    private readonly AppBase app;
    public FocusedAppPaneViewModel(CGameID gameid)
    {
        app = AvaloniaApp.Container.Get<AppsManager>().GetApp(gameid);
        AvaloniaApp.Container.Get<CallbackManager>().RegisterHandler<AppEventStateChange_t>(OnAppEventStateChange);
        this.Name = app.Name;
        SetLibraryAssets();

        app.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
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
        } else if (state == EAppState.Uninstalled || (!app.LaunchOptions.Any() && state == EAppState.FullyInstalled)) {
            PlayButtonLocalizationToken = "#App_InstallApp";
            PlayButtonAction = new RelayCommand(RequestInstall);
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
        if (app.LocalHeroPath != null)
        {
            this.Hero = new ImageBrush()
            {
                Source = new Bitmap(app.LocalHeroPath),
            };
        }
        else
        {
            this.Hero = Brushes.DarkGray;
        }

        if (app.LocalLogoPath != null)
        {
            this.Logo = new ImageBrush()
            {
                Source = new Bitmap(app.LocalLogoPath),
            };
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