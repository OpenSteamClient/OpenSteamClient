using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using ClientUI.Controls;
using Avalonia.Interactivity;
using System.Diagnostics.CodeAnalysis;
using OpenSteamworks;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

namespace ClientUI.Controls;

public partial class BaseWebPage : BasePage
{
    /// <summary>
    /// Defines the <see cref="URL"/> property.
    /// </summary>
    public static readonly AttachedProperty<string> URLProperty =
        AvaloniaProperty.RegisterAttached<BaseWebPage, string>(nameof(URL), typeof(BaseWebPage), "", true);

    /// <summary>
    /// Defines the <see cref="CustomCSS"/> property.
    /// </summary>
    public static readonly AttachedProperty<string?> CustomCSSProperty =
        AvaloniaProperty.RegisterAttached<BaseWebPage, string?>(nameof(CustomCSS), typeof(BaseWebPage), "" , true);

    /// <summary>
    /// Defines the <see cref="UserAgent"/> property.
    /// </summary>
    public static readonly AttachedProperty<string> UserAgentProperty =
        AvaloniaProperty.RegisterAttached<BaseWebPage, string>(nameof(UserAgent), typeof(BaseWebPage), "Valve Steam Client" , true);

    /// <summary>
    /// Defines the <see cref="SetSteamCookies"/> property.
    /// </summary>
    public static readonly AttachedProperty<bool> SetSteamCookiesProperty =
        AvaloniaProperty.RegisterAttached<BaseWebPage, bool>(nameof(SetSteamCookies), typeof(BaseWebPage), false, true);

    /// <summary>
    /// Gets or sets the url of the page to display.
    /// </summary>
    public string URL
    {
        get => this.GetValue(URLProperty);
        set => this.SetValue(URLProperty, value);
    }

    /// <summary>
    /// Gets or sets the custom css.
    /// </summary>
    public string? CustomCSS
    {
        get => this.GetValue(CustomCSSProperty);
        set => this.SetValue(CustomCSSProperty, value);
    }

    /// <summary>
    /// Gets or sets the user agent.
    /// </summary>
    public string UserAgent
    {
        get => this.GetValue(UserAgentProperty);
        set => this.SetValue(UserAgentProperty, value);
    }

    /// <summary>
    /// Gets or sets if the webview should set steam cookies for auth.
    /// </summary>
    public bool SetSteamCookies
    {
        get => this.GetValue(SetSteamCookiesProperty);
        set => this.SetValue(SetSteamCookiesProperty, value);
    }

    private HTMLSurface? webviewControl;

    //I'd love to use bindings here, but due to us being inherited from control we can't.
    private readonly ContentControl webviewContainer;
    private readonly Button prevButton;
    private readonly Button nextButton;
    private readonly Button refreshButton;
    private readonly Button openDevToolsButton;
    private readonly TextBox currentURLTextBox;
    private bool hasLoaded = false;

    public BaseWebPage() : base()
    {
        AvaloniaXamlLoader.Load(this);
        this.TranslatableInit();

        var webviewContainer = this.FindControl<ContentControl>("WebviewContainer");
        if (webviewContainer == null) {
            throw new NullReferenceException("webviewContainer not found");
        }
        this.webviewContainer = webviewContainer;

        var prevButton = this.FindControl<Button>("PrevButton");
        if (prevButton == null) {
            throw new NullReferenceException("prevButton not found");
        }
        this.prevButton = prevButton;

        var nextButton = this.FindControl<Button>("NextButton");
        if (nextButton == null) {
            throw new NullReferenceException("nextButton not found");
        }
        this.nextButton = nextButton;

        var refreshButton = this.FindControl<Button>("RefreshButton");
        if (refreshButton == null) {
            throw new NullReferenceException("refreshButton not found");
        }
        this.refreshButton = refreshButton;

        var openDevToolsButton = this.FindControl<Button>("DevToolsButton");
        if (openDevToolsButton == null) {
            throw new NullReferenceException("openDevToolsButton not found");
        }
        this.openDevToolsButton = openDevToolsButton;

        var currentURLTextBox = this.FindControl<TextBox>("CurrentURLTextBox");
        if (currentURLTextBox == null) {
            throw new NullReferenceException("currentURLTextBox not found");
        }
        this.currentURLTextBox = currentURLTextBox;

        this.AttachedToVisualTree += BaseWebPage_AttachedToVisualTree;
        this.DetachedFromVisualTree += BaseWebPage_DetachedFromVisualTree;
    }

    private void OnHTML_CanGoBackAndForward_t(CallbackManager.CallbackHandler<HTML_CanGoBackAndForward_t> handler, HTML_CanGoBackAndForward_t data) {
        if (this.webviewControl != null && this.webviewControl.BrowserHandle == data.unBrowserHandle) {
            Dispatcher.UIThread.Invoke(() =>
            {
                prevButton.IsEnabled = data.bCanGoBack;
                nextButton.IsEnabled = data.bCanGoForward;
            });
        }
    }

    private void OnHTML_URLChanged_t(CallbackManager.CallbackHandler<HTML_URLChanged_t> handler, HTML_URLChanged_t data) {
        if (this.webviewControl != null && this.webviewControl.BrowserHandle == data.unBrowserHandle) {
            Dispatcher.UIThread.Invoke(() =>
            {
                currentURLTextBox.Text = data.pchURL;
            });
        }
    }

    private async void BaseWebPage_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e) {
        if (hasLoaded) {
            this.webviewControl?.SetBackgroundMode(false);
            this.webviewControl?.RequestRepaint();
            return;
        }

        hasLoaded = true;
        
        this.webviewControl = new HTMLSurface();
        webviewContainer.Content = this.webviewControl;
        
        this.prevButton.Command = new RelayCommand(this.webviewControl.Previous);
        this.nextButton.Command = new RelayCommand(this.webviewControl.Next);
        this.refreshButton.Command = new RelayCommand(this.webviewControl.Refresh);
        this.openDevToolsButton.Command = new RelayCommand(this.webviewControl.OpenDevTools);

        await this.webviewControl.CreateBrowserAsync(this.UserAgent, this.CustomCSS);
        
        var callbackManager = AvaloniaApp.Container.Get<CallbackManager>();
        callbackManager.RegisterHandler<HTML_CanGoBackAndForward_t>(OnHTML_CanGoBackAndForward_t);
        callbackManager.RegisterHandler<HTML_URLChanged_t>(OnHTML_URLChanged_t);
        
        this.webviewControl.LoadURL(this.URL);

        if (SetSteamCookies) {
            await this.webviewControl.SetSteamCookies();
        }
    }

    private void BaseWebPage_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e) {
        if (!hasLoaded) {
            return;
        }

        this.webviewControl?.SetBackgroundMode(true);
    }

    public override void Free() {
        this.webviewControl?.RemoveBrowser();
        base.Free();
    }
}