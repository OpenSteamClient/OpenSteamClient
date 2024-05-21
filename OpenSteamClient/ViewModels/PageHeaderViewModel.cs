using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using OpenSteamClient.Controls;
using OpenSteamClient.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;

namespace OpenSteamClient.ViewModels;

public partial class PageHeaderViewModel : AvaloniaCommon.ViewModelBase
{
    [ObservableProperty]
    private string pageName;

    [ObservableProperty]
    private string pageLocToken;

    [ObservableProperty]
    private bool isVisible = true;

    public Type PageType { get; init; }
    public Type ViewModelType { get; init; }
    public Func<BasePage> PageCtor { get; init; }
    public Func<object, object> ViewModelCtor { get; init; }
    public ICommand SwitchPageAction { get; init; }
    public ObservableCollection<MenuItem> ContextMenuItems { get; } = new() { };
    public bool HasContextMenu
    {
        get
        {
            return ContextMenuItems.Any();
        }
    }

    public bool IsWebPage { get; init; }

    [ObservableProperty]
    private IBrush buttonBackground;

    [ObservableProperty]
    private IBrush buttonForeground;

    [ObservableProperty]
    private bool canUse;

    public PageHeaderViewModel(MainWindowViewModel mainWindowViewModel, string name, string locToken, Type pageType, Type viewModelType, bool defaultVisible = true)
    {
        this.IsVisible = defaultVisible;
        this.PageName = name;
        this.PageLocToken = locToken;
        this.PageType = pageType;
        this.IsWebPage = pageType.IsAssignableTo(typeof(BaseWebPage));
        this.ViewModelType = viewModelType;
        this.PageCtor = () =>
        {
            var ctrl = (BasePage?)Activator.CreateInstance(pageType);
            if (ctrl == null)
            {
                throw new NullReferenceException("Activator.CreateInstance failed");
            }

            return ctrl;
        };

        this.ViewModelCtor = (page) => AvaloniaApp.Container.ConstructOnly(viewModelType, page);
        this.SwitchPageAction = new RelayCommand(() => mainWindowViewModel.SwitchToPage(pageType));
        this.ContextMenuItems.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
        {
            this.OnPropertyChanged(nameof(HasContextMenu));
        };

        this.ButtonBackground = AvaloniaApp.Theme!.ButtonBackground;
        this.ButtonForeground = AvaloniaApp.Theme!.ButtonForeground;

        if (this.IsWebPage)
        {
            // Unload page action
            this.ContextMenuItems.Add(AvaloniaApp.Container.Get<TranslationManager>().CreateTranslated(new MenuItem()
            {
                Command = new RelayCommand(() =>
                {
                    mainWindowViewModel.UnloadPage(PageType);
                    this.ButtonBackground = AvaloniaApp.Theme!.ButtonBackground;
                    this.ButtonForeground = AvaloniaApp.Theme!.ButtonForeground;
                })
            }, "#PageHeader_UnloadPage", "Unload page"));

            this.CanUse = AvaloniaApp.Container.Get<SteamHTML>().CanRun();
        }
        else
        {
            this.CanUse = true;
        }
    }
}
