using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamClient.Translation;
using OpenSteamClient.Views;
using OpenSteamworks.Client.Apps;

namespace OpenSteamClient.ViewModels;

public partial class PickLaunchOptionDialogViewModel : AvaloniaCommon.ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanPressOK))]
    private LaunchOptionViewModel? selectedOption;
    public event EventHandler<int>? OptionSelected;

    public ObservableCollection<LaunchOptionViewModel> LaunchOptions { get; init; }
    public bool CanPressOK => SelectedOption != null;

    private readonly PickLaunchOptionDialog dialog;
    private readonly TranslationManager tm;
    private readonly AppBase app;
    public PickLaunchOptionDialogViewModel(PickLaunchOptionDialog dialog, AppBase app)
    {
        this.app = app;
        this.tm = AvaloniaApp.Container.Get<TranslationManager>();
        this.dialog = dialog;
        this.dialog.Title = string.Format(tm.GetTranslationForKey("#LaunchOptionDialog_Title"), app.Name);
        this.dialog.DescText.Text = string.Format(tm.GetTranslationForKey("#LaunchOptionDialog_ChooseHowToLaunch"), app.Name);
        this.dialog.Closed += OnClosed;
        LaunchOptions = new(app.LaunchOptions.Select(MapOption));
    }

    private LaunchOptionViewModel MapOption(AppBase.ILaunchOption opt) {
        string name = opt.Name;
        if (string.IsNullOrEmpty(opt.Name)) {
            string translationKey = "#LaunchOptionDialog_GenericOption";
            if (app.Type == OpenSteamworks.Enums.EAppType.Game) {
                translationKey += "Game";
            } else {
                translationKey += "App";
            }

            name = string.Format(tm.GetTranslationForKey(translationKey), app.Name);
        }
    
        return new(opt.ID, name, opt.Description);
    }

    private void OnClosed(object? sender, EventArgs e)
        => Close();

    public void Close()
        => dialog.Close();

    public void OK() {
        Close();
        
        if (this.SelectedOption != null) {
            OptionSelected?.Invoke(this, this.SelectedOption.ID);
        }
    }
}