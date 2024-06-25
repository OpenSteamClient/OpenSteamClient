#if AVALONIA_ILAUNCHER_API

using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform.Storage;

namespace GameOverlayUI.Impl;

public class OverlayLauncherFactory : ILauncherFactory
{
    public ILauncher CreateLauncher(TopLevel topLevel)
    {
        return new OverlayLauncher();
    }
}

#endif