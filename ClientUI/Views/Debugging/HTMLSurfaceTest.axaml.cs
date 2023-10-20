using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClientUI.PlatformSpecific;
using OpenSteamworks;

namespace ClientUI.Views;

public partial class HTMLSurfaceTest : Window
{
    private Controls.HTMLSurface surfaceControl;
    private SteamClient client;
    public HTMLSurfaceTest(SteamClient client) : base()
    {
        InitializeComponent();
        this.client = client;
        surfaceControl = new(client);
        this.FindControl<DockPanel>("webviewContainer")!.Children.Add(surfaceControl);

        this.Closing += (object? sender, WindowClosingEventArgs e) =>
        {
            surfaceControl.RemoveBrowser();
        };
    }

    public async Task Init(string userAgent, string url) {
        var handle = await this.surfaceControl.CreateBrowser(userAgent, "");
        this.client.NativeClient.IClientHTMLSurface.LoadURL(handle, url, null);
    }
}
