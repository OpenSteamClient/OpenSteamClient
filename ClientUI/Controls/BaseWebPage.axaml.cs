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

    private HTMLSurface? webviewControl;
    private readonly ContentControl webviewContainer;
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

        this.AttachedToVisualTree += BaseWebPage_AttachedToVisualTree;
        this.DetachedFromVisualTree += BaseWebPage_DetachedFromVisualTree;
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
        await this.webviewControl.CreateBrowserAsync(this.UserAgent, this.CustomCSS);
        this.webviewControl.LoadURL(this.URL);
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