using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using OpenSteamClient.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Structs;

namespace OpenSteamClient.ViewModels.Library;

public partial class LibraryAppViewModel : Node
{
    public AppBase App { get; init; }
    protected override string ActualName => App.Name;

    public LibraryAppViewModel(CGameID gameid)
    {
        this.HasIcon = true;
        this.IsApp = true;

        App = AvaloniaApp.Container.Get<AppsManager>().GetApp(gameid);
        this.GameID = App.GameID;

        SetLibraryAssets();
        SetStatusIcon();
        App.LibraryAssetsUpdated += OnLibraryAssetsUpdated;
        App.NameChanged += OnNameChanged;
    }

    private void OnNameChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Name));
    }

    private void SetLibraryAssets()
    {
        // Constructing an ImageBrush needs to happen on the main thread (strange design but sure, whatever)
        AvaloniaApp.Current?.RunOnUIThread(DispatcherPriority.Send, () =>
        {
            if (App.LocalIconPath != null)
            {
                this.Icon = new ImageBrush()
                {
                    Source = new Bitmap(App.LocalIconPath),
                };
            }
            else
            {
                this.Icon = Brushes.DarkGray;
            }
        });
    }

    private void SetStatusIcon()
    {
        StatusIcon = Brushes.Transparent;
        if (App is SteamApp SApp)
        {
            
        }
    }

    public void OnLibraryAssetsUpdated(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Invoke(() => SetLibraryAssets());
    }
}