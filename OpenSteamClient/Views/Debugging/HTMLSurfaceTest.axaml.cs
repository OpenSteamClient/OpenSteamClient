using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using OpenSteamClient.PlatformSpecific;
using OpenSteamworks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Startup;
using static OpenSteamworks.Callbacks.CallbackManager;

namespace OpenSteamClient.Views;

public partial class HTMLSurfaceTest : Window
{
    private Controls.HTMLSurface surfaceControl;
    private ISteamClient client;
    public HTMLSurfaceTest() : base()
    {
        InitializeComponent();

        this.client = AvaloniaApp.Container.Get<ISteamClient>();
        surfaceControl = new();
        this.FindControl<DockPanel>("webviewContainer")!.Children.Add(surfaceControl);

        this.Closing += (object? sender, WindowClosingEventArgs e) =>
        {
            surfaceControl.RemoveBrowser();
        };

        this.client.CallbackManager.RegisterHandler<HTML_ChangedTitle_t>(OnHTML_ChangedTitle_t);
    }

    public async Task Init(string userAgent, string url)
    {
        var handle = await this.surfaceControl.CreateBrowserAsync(userAgent, "");
        this.client.IClientHTMLSurface.LoadURL(handle, url, null);
    }

    private void OnHTML_ChangedTitle_t(CallbackHandler<HTML_ChangedTitle_t> handler, HTML_ChangedTitle_t data)
    {
        if (surfaceControl.BrowserHandle == data.unBrowserHandle)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                this.Title = data.pchTitle;
            });
        }
    }
}
