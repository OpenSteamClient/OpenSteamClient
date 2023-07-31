using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using ClientUI.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels;

public partial class MessageBoxViewModel : ViewModelBase
{
    [ObservableProperty]
    private string title = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor("HasHeader")]
    private string header = "";
    public bool HasHeader => !string.IsNullOrEmpty(this.Header);

    [ObservableProperty]
    private string content = "";

    [ObservableProperty]
    private string iconPath;

    [ObservableProperty]
    private Bitmap icon;

    [NotifyPropertyChangedFor("IsOkShowed")]
    [NotifyPropertyChangedFor("IsYesShowed")]
    [NotifyPropertyChangedFor("IsNoShowed")]
    [NotifyPropertyChangedFor("IsCancelShowed")]

    [ObservableProperty]
    private MessageBoxButton enabledButtons;
    public MessageBoxButton? ButtonClicked { get; private set; }
    public bool IsOkShowed => EnabledButtons.HasFlag(MessageBoxButton.Ok);
    public bool IsYesShowed => EnabledButtons.HasFlag(MessageBoxButton.Yes);
    public bool IsNoShowed => EnabledButtons.HasFlag(MessageBoxButton.No);
    public bool IsCancelShowed => EnabledButtons.HasFlag(MessageBoxButton.Cancel);
    public double Width { get; set; } = double.NaN;
    public double Height { get; set; } = double.NaN;
    public Action Copy { get; set; }
    public Action EnterPressed { get; set; }
    public Action EscPressed { get; set; }
    private MessageBox messageBox;
    public MessageBoxViewModel(MessageBoxIcon icon, MessageBoxButton enabledButtons, MessageBox messageBox) {
        this.messageBox = messageBox;
        this.Copy = messageBox.Copy;
        this.enabledButtons = enabledButtons;
        this.EnterPressed = PressDefaultIfPossible;
        this.EscPressed = messageBox.Close;
        this.IconPath = $"avares://ClientUI/Assets/{icon.ToString().ToLowerInvariant()}.ico";
        this.Icon = new Bitmap(AssetLoader.Open(new Uri(this.iconPath)));
    }
    public void PressDefaultIfPossible() {
        // If there's only one button enabled, then click it
        switch (EnabledButtons)
        {
            case MessageBoxButton.No:
                NoClicked();
                break;
            case MessageBoxButton.Yes:
                YesClicked();
                break;
            case MessageBoxButton.Cancel:
                CancelClicked();
            break;
            case MessageBoxButton.Ok:
                OkClicked();
            break;
        }
        // If there's multiple, try to cancel, never take a yes action automatically
        if (EnabledButtons.HasFlag(MessageBoxButton.Cancel)) {
            CancelClicked();
        }
    }
    public void OkClicked() {
        ButtonClicked = MessageBoxButton.Ok;
        messageBox.QueueClose();
    }
    public void YesClicked() {
        ButtonClicked = MessageBoxButton.Yes;
        messageBox.QueueClose();
    }
    public void NoClicked() {
        ButtonClicked = MessageBoxButton.No;
        messageBox.QueueClose();
    }
    public void CancelClicked() {
        ButtonClicked = MessageBoxButton.Cancel;
        messageBox.QueueClose();
    }

}