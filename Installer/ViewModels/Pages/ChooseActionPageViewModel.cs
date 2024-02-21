using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Installer.Enums;

namespace Installer.ViewModels.Pages;

public partial class ChooseActionPageViewModel : AvaloniaCommon.ViewModelBase {
    public bool InstallAvailable => mainWindowViewModel.AvailableActions.HasFlag(InstallAction.Install);
    public bool RepairAvailable => mainWindowViewModel.AvailableActions.HasFlag(InstallAction.Repair);
    public bool UninstallAvailable => mainWindowViewModel.AvailableActions.HasFlag(InstallAction.Uninstall);

    public bool InstallChecked {
        get => mainWindowViewModel.SelectedAction == InstallAction.Install;
        set {
            if (value) {
                mainWindowViewModel.SelectedAction = InstallAction.Install;
            } else {
                mainWindowViewModel.SelectedAction = InstallAction.None;
            }

            this.OnPropertyChanged(nameof(InstallChecked));
        }
    }

    public bool RepairChecked {
        get => mainWindowViewModel.SelectedAction == InstallAction.Repair;
        set {
            if (value) {
                mainWindowViewModel.SelectedAction = InstallAction.Repair;
            } else {
                mainWindowViewModel.SelectedAction = InstallAction.None;
            }

            this.OnPropertyChanged(nameof(RepairChecked));
        }
    }

    public bool UninstallChecked {
        get => mainWindowViewModel.SelectedAction == InstallAction.Uninstall;
        set {
            if (value) {
                mainWindowViewModel.SelectedAction = InstallAction.Uninstall;
            } else {
                mainWindowViewModel.SelectedAction = InstallAction.None;
            }

            this.OnPropertyChanged(nameof(UninstallChecked));
        }
    }

    private readonly MainWindowViewModel mainWindowViewModel;
    public ChooseActionPageViewModel(MainWindowViewModel mainWindowViewModel) {
        this.mainWindowViewModel = mainWindowViewModel;
    }
}