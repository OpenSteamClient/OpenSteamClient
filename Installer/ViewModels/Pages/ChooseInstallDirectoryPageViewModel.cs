using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Platform.Storage;
using Avalonia.Reactive;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Installer.Extensions;
using Installer.Views;

namespace Installer.ViewModels.Pages;

public partial class ChooseInstallDirectoryPageViewModel : AvaloniaCommon.ViewModelBase {
    [ObservableProperty]
    private string currentPath;

    [NotifyPropertyChangedFor(nameof(HasError))]
    [ObservableProperty]
    private string currentError;

    public bool HasError => !string.IsNullOrEmpty(CurrentError);

    private readonly MainWindowViewModel mainWindowViewModel;
    private readonly ChooseInstallDirectoryPage page;
    public ChooseInstallDirectoryPageViewModel(ChooseInstallDirectoryPage page, MainWindowViewModel mainWindowViewModel) {
        this.mainWindowViewModel = mainWindowViewModel;
        this.page = page;
        CurrentPath = "C:\\Program Files\\OpenSteamClient";
        CurrentError = "";
        Dispatcher.UIThread.Invoke(() => page.AttachedToVisualTree += OnAttachedToVisualTree);
        AvaloniaApp.TranslationManager.TranslationChanged += (object? sender, EventArgs args) => ValidateNext();
        ValidateNext();
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs args) {
        var textbox = page.FindControlNested<TextBox>("InstallDirBox");
        ArgumentNullException.ThrowIfNull(textbox);
        textbox.GetObservable(TextBox.TextProperty).Subscribe(new AnonymousObserver<string?>((str) => ValidateNext()));
        page.AttachedToVisualTree -= OnAttachedToVisualTree;
    }

    public async void SelectDirectory() {
        var folders = await AvaloniaApp.Current!.MainWindow!.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = AvaloniaApp.TranslationManager.GetTranslationForKey("#PickInstallDirectory_FolderPickerTitle")
        });

        if (folders.Count == 0) {
            return;
        }

        if (Path.GetFileName(folders[0].Path.LocalPath) != "OpenSteamClient") {
            CurrentPath = Path.Combine(folders[0].Path.LocalPath, "OpenSteamClient");
        } else {
            CurrentPath = folders[0].Path.LocalPath;
        }
    }

    public bool TestIfValidDirectory() {
        if (!Path.IsPathRooted(CurrentPath)) {
            CurrentError = AvaloniaApp.TranslationManager.GetTranslationForKey("#InstallError_PathNotAbsoluteError");
            return false;
        }

        if (Directory.Exists(CurrentPath)) {
            if (Directory.EnumerateFileSystemEntries(CurrentPath).Any()) {
                CurrentError = AvaloniaApp.TranslationManager.GetTranslationForKey("#InstallError_ContainsFilesError");
                return false;
            }  
        } else {
            // If the directory is named OpenSteamClient, we can create it later
            if (Path.GetFileName(CurrentPath) != "OpenSteamClient") {
                CurrentError = AvaloniaApp.TranslationManager.GetTranslationForKey("#InstallError_DirectoryNotFoundError");
                return false;
            }
        }

        CurrentError = "";
        return true;
    }

    public void ValidateNext()
    {
        Console.WriteLine("Validating");
        //TODO: this might cause performance issues with the rapid speed we're calling it in TextProperty updated
        mainWindowViewModel.CanGoNext = TestIfValidDirectory();
    }
}