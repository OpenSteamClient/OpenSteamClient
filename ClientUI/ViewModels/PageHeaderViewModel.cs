using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using ClientUI.Controls;
using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;

namespace ClientUI.ViewModels;

public partial class PageHeaderViewModel : ViewModelBase
{
    [ObservableProperty]
    private string pageName;

    public Type PageType { get; init; }
    public Type ViewModelType { get; init; }
    public Func<BasePage> PageCtor { get; init; }
    public Func<object> ViewModelCtor { get; init; }
    public Action SwitchPageAction { get; init; }
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

    public PageHeaderViewModel(MainWindowViewModel mainWindowViewModel, string name, Type pageType, Type viewModelType)
    {
        this.PageName = name;
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

        this.ViewModelCtor = () => AvaloniaApp.Container.ConstructOnly(viewModelType);
        this.SwitchPageAction = () => mainWindowViewModel.SwitchToPage(pageType);
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
