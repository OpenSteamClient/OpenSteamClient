// HAS_RENDERIMMEDIATE (for custom avalonia builds (on by default))
#define HAS_RENDERIMMEDIATE

using System;
using Avalonia.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using OpenSteamworks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Generated;
using static OpenSteamworks.Callbacks.CallbackManager;
using Avalonia.Threading;
using Avalonia.Input;
using OpenSteamworks.Enums;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using Avalonia.Interactivity;
using ClientUI.PlatformSpecific;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using OpenSteamworks.Client.Startup;
using Avalonia.Media.Imaging;
using System.IO;
using OpenSteamworks.Utils;

namespace ClientUI.Controls;

public static class AvaloniaCursors
{
    public readonly static Cursor Default = Cursor.Default;
    public readonly static Cursor None = new(StandardCursorType.None);
    public readonly static Cursor Arrow = new(StandardCursorType.Arrow);
    public readonly static Cursor IBeam = new(StandardCursorType.Ibeam);
    public readonly static Cursor Wait = new(StandardCursorType.Wait);
    public readonly static Cursor Waitarrow = new(StandardCursorType.AppStarting);
    public readonly static Cursor Crosshair = new(StandardCursorType.Cross);
    public readonly static Cursor UpArrow = new(StandardCursorType.UpArrow);
    public readonly static Cursor SizeNW = new(StandardCursorType.TopLeftCorner);
    public readonly static Cursor SizeSE = new(StandardCursorType.BottomRightCorner);
    public readonly static Cursor SizeNE = new(StandardCursorType.TopRightCorner);
    public readonly static Cursor SizeSW = new(StandardCursorType.BottomLeftCorner);
    public readonly static Cursor SizeW = new(StandardCursorType.LeftSide);
    public readonly static Cursor SizeE = new(StandardCursorType.RightSide);
    public readonly static Cursor SizeN = new(StandardCursorType.TopSide);
    public readonly static Cursor SizeS = new(StandardCursorType.BottomSide);
    public readonly static Cursor SizeWE = new(StandardCursorType.SizeWestEast);
    public readonly static Cursor SizeNS = new(StandardCursorType.SizeNorthSouth);
    public readonly static Cursor SizeAll = new(StandardCursorType.SizeAll);
    public readonly static Cursor No = new(StandardCursorType.No);
    public readonly static Cursor Hand = new(StandardCursorType.Hand);
    // This is where we'd have a lot of "panning" cursors, but I have no idea what they are. Maybe the cursor you get when you middle click to navigate?
    public readonly static Cursor Help = new(StandardCursorType.Help);


    public static Cursor GetCursorForEMouseCursor(EMouseCursor steamCursor)
    {
        return steamCursor switch
        {
            EMouseCursor.dc_none => None,
            EMouseCursor.dc_arrow => Arrow,
            EMouseCursor.dc_ibeam => IBeam,
            EMouseCursor.dc_hourglass => Wait,
            EMouseCursor.dc_waitarrow => Waitarrow,
            EMouseCursor.dc_crosshair => Crosshair,
            EMouseCursor.dc_sizenw => SizeNW,
            EMouseCursor.dc_sizese => SizeSE,
            EMouseCursor.dc_sizene => SizeNE,
            EMouseCursor.dc_sizesw => SizeSW,
            EMouseCursor.dc_sizew => SizeW,
            EMouseCursor.dc_sizee => SizeE,
            EMouseCursor.dc_sizen => SizeN,
            EMouseCursor.dc_sizes => SizeS,
            EMouseCursor.dc_sizewe => SizeWE,
            EMouseCursor.dc_sizens => SizeNS,
            EMouseCursor.dc_sizeall => SizeAll,
            EMouseCursor.dc_no => No,
            EMouseCursor.dc_hand => Hand,
            EMouseCursor.dc_help => Help,
            _ => Default,
        };
    }
}

/// <summary>
/// An HTMLSurface for showing a webpage via IClientHTMLSurface. Needs htmlhost.
/// Cannot be instantiated from axaml, must be created in codebehind.
/// Only handles paints, input events and cursor changes, register other handlers yourself
/// </summary>
public partial class HTMLSurface : UserControl
{
    private class HTMLBufferImg : ICustomDrawOperation
    {
        private int width;
        private int height;
        private bool disposedValue;
        private readonly object targetBitmapLock = new();
        private SKBitmap targetBitmap;
        private nint lastPtr = 0;
        internal bool isCurrentlyRenderable = false;
        private static readonly SKPaint simplePaint;

        static HTMLBufferImg()
        {
            simplePaint = new SKPaint
            {
                IsAntialias = true,
                HintingLevel = SKPaintHinting.Full,
                IsAutohinted = true,
                IsDither = true,
                SubpixelText = true,
                BlendMode = SKBlendMode.Src,
            };
        }

        public HTMLBufferImg(SKColorType format, SKAlphaType alphaFormat, int width, int height)
        {
            this.width = width;
            this.height = height;
            Console.WriteLine("allocating initial bitmap of size " + width + "x" + height);
            this.targetBitmap = AllocBitmap(new(width, height, format, alphaFormat));

        }

        private SKBitmap AllocBitmap(SKImageInfo info)
        {
            if (info.Width == 0 || info.Height == 0)
            {
                throw new InvalidOperationException("Cannot allocate a bitmap of size 0");
            }

            return new(info, SKBitmapAllocFlags.ZeroPixels);
        }

        public Rect Bounds => new(0, 0, width, height);

        public bool Equals(ICustomDrawOperation? other)
        {
            return other != null && other.GetType() == typeof(HTMLBufferImg) && ((HTMLBufferImg)other).lastPtr == this.lastPtr;
        }

        public bool HitTest(Point p)
        {
            return Bounds.Intersects(new Rect(p.X, p.Y, 1, 1));
        }

        public void Render(ImmediateDrawingContext context)
        {
            if (!isCurrentlyRenderable)
            {
                return;
            }

            var leasef = context.PlatformImpl.GetFeature<ISkiaSharpApiLeaseFeature>();
            if (leasef == null)
            {
                throw new PlatformNotSupportedException("Your platform does not support SkiaSharpApiLeaseFeature.");
            }

            using (var lease = leasef.Lease())
            {
                lease.SkCanvas.DrawBitmap(targetBitmap, 0f, 0f, simplePaint);
            }
        }

        private void ResizeInternal(int newWidth, int newHeight)
        {
            unsafe
            {
                checked
                {
                    // Disallow resizing to 0x0
                    if (newWidth == 0 || newHeight == 0)
                    {
                        Console.WriteLine("Trying to resize to 0! Resize ignored");
                        return;
                    }

                    this.width = newWidth;
                    this.height = newHeight;
                    var newInfo = new SKImageInfo(this.width, this.height, targetBitmap.Info.ColorType, targetBitmap.Info.AlphaType);
                    targetBitmap.Dispose();
                    targetBitmap = AllocBitmap(newInfo);
                    // if (currentPtr != 0) {
                    //     NativeMemory.Free((void*)currentPtr);
                    // }

                    //currentPtrSize = (nuint)(this.targetBitmapInfo.Width * this.targetBitmapInfo.Height * this.targetBitmapInfo.BytesPerPixel);
                    //currentPtr = (IntPtr)NativeMemory.AllocZeroed(currentPtrSize);
                }
            }
        }

        Stopwatch lockTime = new();
        Stopwatch resizeTime = new();
        Stopwatch memcpyTime = new();
        Stopwatch installPixelsTime = new();

        public void UpdateData(int newWidth, int newHeight, IntPtr dataPtr)
        {
            Console.WriteLine("UpdateData called " + newWidth + "x" + newHeight + " : " + dataPtr);
            lockTime.Reset();
            resizeTime.Reset();
            memcpyTime.Reset();
            installPixelsTime.Reset();

            if (newWidth == 0 || newWidth == 1 || newHeight == 0 || newHeight == 1)
            {
                Console.WriteLine("Ignoring UpdateData where newWidth or newHeight was invalid");
                return;
            }

            if (dataPtr == 0)
            {
                Console.WriteLine("Ignoring UpdateData where dataPtr is null");
                return;
            }

            lockTime.Start();
            lock (targetBitmapLock)
            {
                lockTime.Stop();

                if (this.targetBitmap.Width != newWidth || this.targetBitmap.Height != newHeight)
                {
                    resizeTime.Start();
                    ResizeInternal(newWidth, newHeight);
                    resizeTime.Stop();
                }


                // unsafe {
                //     memcpyTime.Start();
                //     NativeMemory.Copy((void*)dataPtr, (void*)currentPtr, currentPtrSize);
                //     memcpyTime.Stop();
                // }

                // installPixelsTime.Start();
                //NOTE: installpixels does not copy. It simply sets an internal pointer to the one specified here. This is bad in our use case, where the pointer is only valid during this function call. (Which means we need to memcpy or lock, which is also bad)
                targetBitmap.InstallPixels(targetBitmap.Info, dataPtr);
                installPixelsTime.Stop();

                if (!File.Exists("/tmp/frame.png"))
                {
                    using (var file = File.OpenWrite("/tmp/frame.png"))
                    {
                        targetBitmap.Encode(file, SKEncodedImageFormat.Png, 100);
                    }
                }

                //bitmap = new Bitmap(PixelFormat.Bgra8888, AlphaFormat.Unpremul, dataPtr, PixelSize.FromSizeWithDpi(new Size(this.width, this.height), 96), new Vector(96, 96), 4 * ((width * this.targetBitmapInfo.BytesPerPixel + 3) / 4));
                lastPtr = dataPtr;
                installPixelsTime.Stop();
                isCurrentlyRenderable = true;
                Console.WriteLine("UpdateData took " + lockTime.Elapsed.TotalMilliseconds + "ms + " + resizeTime.Elapsed.TotalMilliseconds + "ms + " + memcpyTime.Elapsed.TotalMilliseconds + "ms + " + installPixelsTime.Elapsed.TotalMilliseconds + "ms");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.targetBitmap.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    private readonly HTMLBufferImg htmlImgBuffer;
    private readonly IClientHTMLSurface surface;
    private readonly ISteamClient client;
    private readonly SteamHTML htmlHost;
    private static readonly Encoding utfEncoder = new UTF32Encoding(false, true, false);
    public HHTMLBrowser BrowserHandle { get; private set; } = 0;
    // The scroll multiplier affects how fast scrolling works. Piping the scroll wheel directly into steam makes scrolling slow, so simply multiply it by this value.
    //TODO: this is terrible and doesn't allow for smooth scrolling. This seems to be the same underlying issue that ValveSteam had on Linux for a long time, until their recent switch to a full web browser window. 
    // However, on the release of the deck, if you tricked ValveSteam it was running on deck, it scrolled the library smoothly (albeit with only software rendering)
    // Maybe we can use something else to simulate smooth scrolling?
    private const int SCROLL_MULTIPLIER = 35;

    public HTMLSurface() : base()
    {
        this.client = AvaloniaApp.Container.Get<ISteamClient>();
        this.htmlHost = AvaloniaApp.Container.Get<SteamHTML>();

        InitializeComponent();

        // Allocate an initial buffer at 720p, since we don't know our size yet
        this.htmlImgBuffer = new HTMLBufferImg(SKColorType.Bgra8888, SKAlphaType.Unpremul, 720, 1080);
        this.Focusable = true;

        this.surface = client.IClientHTMLSurface;
        client.CallbackManager.RegisterHandler<HTML_NeedsPaint_t>(this.OnHTML_NeedsPaint);
        client.CallbackManager.RegisterHandler<HTML_SetCursor_t>(this.OnHTML_SetCursor);
        client.CallbackManager.RegisterHandler<HTML_ShowToolTip_t>(this.OnHTML_ShowToolTip_t);
        client.CallbackManager.RegisterHandler<HTML_HideToolTip_t>(this.OnHTML_HideToolTip_t);
        // Reactive bloat. No events here, need to do this instead...
        this.GetObservable(BoundsProperty).Subscribe(new AnonymousObserver<Rect>(BoundsChange));
    }

    private void OnHTML_SetCursor(CallbackHandler<HTML_SetCursor_t> handler, HTML_SetCursor_t setCursor)
    {
        if (setCursor.unBrowserHandle == BrowserHandle)
        {
            SetCursorNonUICode(AvaloniaCursors.GetCursorForEMouseCursor(setCursor.eMouseCursor));
        }
    }

    public void Refresh()
    {
        this.surface.Reload(this.BrowserHandle);
    }

    public void Previous()
    {
        this.surface.GoBack(this.BrowserHandle);
    }

    public void Next()
    {
        this.surface.GoForward(this.BrowserHandle);
    }

    public void OpenDevTools()
    {
        this.surface.OpenDeveloperTools(this.BrowserHandle);
    }

    private void SetCursorNonUICode(Cursor cursor)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            this.Cursor = cursor;
        }, DispatcherPriority.Input);
    }

    private void ForceRedrawNonUICode()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            // Strange quirk, calling paint will not force a repaint unless the control is marked as dirty
#if HAS_RENDERIMMEDIATE
            VisualRoot?.Renderer.RenderImmediate(this);
#else
            VisualRoot?.Renderer.AddDirty(this);
            VisualRoot?.Renderer.Paint(this.Bounds);
#endif
        }, DispatcherPriority.MaxValue);
    }

    private void BoundsChange(Rect newBounds)
    {
        if (this.BrowserHandle != 0)
        {
            Console.WriteLine($"Bounds changed! ({(uint)newBounds.Width}x{(uint)newBounds.Height})");
            if (!this.htmlImgBuffer.isCurrentlyRenderable)
            {
                this.surface.SetSize(this.BrowserHandle, (uint)newBounds.Width, (uint)newBounds.Height);
            }
            else
            {
                Console.WriteLine("Currently rendering, not resizing!");
            }
        }
    }

    Stopwatch paintUpdateDataTime = new();
    Stopwatch forceRedrawTime = new();
    //TODO: performance could be improved by using unUpdateXXXX properties to only update the part of the canvas that needs updating
    private void OnHTML_NeedsPaint(CallbackHandler<HTML_NeedsPaint_t> handler, HTML_NeedsPaint_t paintEvent)
    {
        if (paintEvent.unBrowserHandle == this.BrowserHandle)
        {
            forceRedrawTime.Reset();
            paintUpdateDataTime.Reset();
            paintUpdateDataTime.Start();
            htmlImgBuffer.UpdateData((int)paintEvent.unWide, (int)paintEvent.unTall, paintEvent.pBGRA);
            paintUpdateDataTime.Stop();
            forceRedrawTime.Start();
            this.ForceRedrawNonUICode();
            forceRedrawTime.Stop();
            Console.WriteLine("Paint took " + forceRedrawTime.Elapsed.TotalMilliseconds + "ms + " + paintUpdateDataTime.Elapsed.TotalMilliseconds + "ms ");
            htmlImgBuffer.isCurrentlyRenderable = false;
        }
    }

    private void OnHTML_ShowToolTip_t(CallbackHandler<HTML_ShowToolTip_t> handler, HTML_ShowToolTip_t ev)
    {
        if (ev.unBrowserHandle == this.BrowserHandle)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                this[ToolTip.TipProperty] = ev.pchMsg;
                this[ToolTip.IsOpenProperty] = true;
            });
        }
    }

    private void OnHTML_HideToolTip_t(CallbackHandler<HTML_HideToolTip_t> handler, HTML_HideToolTip_t ev)
    {
        if (ev.unBrowserHandle == this.BrowserHandle)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                this[ToolTip.IsOpenProperty] = false;
            });
        }
    }

    public void SetBackgroundMode(bool background)
    {
        this.surface.SetBackgroundMode(this.BrowserHandle, background);
    }

    internal void RequestRepaint()
    {
        //TODO: this is a hack. Can we find a better way?
        this.surface.SetSize(this.BrowserHandle, (uint)this.Bounds.Width, (uint)this.Bounds.Height);
    }

    /// <summary>
    /// Sets this webview with the current user's web auth token for accessing steam websites.
    /// </summary>
    /// <returns></returns>
    public async Task SetSteamCookies()
    {
        string[] domains = new string[] { "https://store.steampowered.com", "https://help.steampowered.com", "https://steamcommunity.com" };
        StringBuilder language = new(128);
        this.client.IClientUser.GetLanguage(language, language.Capacity);

        string vractiveStr = "0";
        if (this.client.IClientUtils.IsSteamRunningInVR())
        {
            vractiveStr = "1";
        }

        foreach (var item in domains)
        {
            checked
            {
                this.surface.SetCookie(item, "steamLoginSecure", await GetWebToken(), "/", 0, true, true);
                this.surface.SetCookie(item, "Steam_Language", language.ToString(), "/", 0, false, false);
                this.surface.SetCookie(item, "vractive", vractiveStr, "/", 0, false, false);
            }
        }
    }

    public async Task<string> GetWebToken()
    {
        // No need to use incrementing stringbuilder here, since the webauth tokens are always this size
        StringBuilder sb = new(1024);
        string token;
        if (!this.client.IClientUser.GetCurrentWebAuthToken(sb, (uint)sb.Capacity))
        {
            await this.client.CallbackManager.PauseThreadAsync();

            var callHandle = this.client.IClientUser.RequestWebAuthToken();
            if (callHandle == 0)
            {
                throw new InvalidOperationException("SetWebAuthToken failed due to no call handle being returned.");
            }

            var result = await this.client.CallbackManager.WaitForAPICallResultAsync<WebAuthRequestCallback_t>(callHandle, true, new CancellationTokenSource(10000).Token);
            if (result.failed)
            {
                throw new InvalidOperationException("SetWebAuthToken failed due to CallResult failure: " + result.failureReason);
            }

            token = result.data.m_rgchToken;
        }
        else
        {
            token = sb.ToString();
        }

        return token;
    }

    public async Task<HHTMLBrowser> CreateBrowserAsync(string userAgent, string? customCSS)
    {
        await GetWebToken();
        this.htmlHost.Start();
        await this.client.CallbackManager.PauseThreadAsync();
        var callHandle = this.surface.CreateBrowser(userAgent, customCSS);
        if (callHandle == 0)
        {
            this.htmlHost.Stop();
            throw new InvalidOperationException("CreateBrowser failed due to no call handle being returned.");
        }

        var result = await this.client.CallbackManager.WaitForAPICallResultAsync<HTML_BrowserReady_t>(callHandle, true, new CancellationTokenSource(2000).Token);
        if (result.failed)
        {
            this.htmlHost.Stop();
            throw new InvalidOperationException("CreateBrowser failed due to CallResult failure: " + result.failureReason);
        }

        this.BrowserHandle = result.data.unBrowserHandle;

        Console.WriteLine("Created new browser with handle " + this.BrowserHandle);
        this.surface.AllowStartRequest(this.BrowserHandle, true);
        this.surface.SetSize(this.BrowserHandle, (uint)this.Bounds.Width, (uint)this.Bounds.Height);

        return result.data.unBrowserHandle;
    }

    public void RemoveBrowser()
    {
        if (this.BrowserHandle == 0)
        {
            return;
        }

        // Free surfaces
        this.surface.RemoveBrowser(this.BrowserHandle);

        // Shutdown the interface if no other surfaces are left
        this.htmlHost.Stop();
    }

    public void LoadURL(string url)
    {
        this.surface.LoadURL(this.BrowserHandle, url, null);
    }

    public override void Render(DrawingContext context)
    {
        context.DrawRectangle(Brushes.Black, null, this.Bounds, 0, 0, default);
        context.Custom(this.htmlImgBuffer);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (this.BrowserHandle != 0)
        {
            this.surface.MouseWheel(this.BrowserHandle, (int)e.Delta.Y * SCROLL_MULTIPLIER, (int)e.Delta.X * SCROLL_MULTIPLIER);
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (this.BrowserHandle != 0)
        {
            var rect = e.GetCurrentPoint(this).Position;
            this.surface.MouseMove(this.BrowserHandle, (int)rect.X, (int)rect.Y);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerMoved(e);
        if (this.BrowserHandle != 0)
        {
            var prop = e.GetCurrentPoint(this).Properties;
            var btn = PointerPropertiesToHTMLMouseButton(prop);
            this.surface.MouseDown(this.BrowserHandle, btn);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerMoved(e);
        if (this.BrowserHandle != 0)
        {
            var prop = e.GetCurrentPoint(this).Properties;
            var btn = PointerPropertiesToHTMLMouseButton(prop);
            this.surface.MouseUp(this.BrowserHandle, btn);
        }
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        if (this.BrowserHandle != 0)
        {
            this.surface.SetKeyFocus(this.BrowserHandle, true);
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        if (this.BrowserHandle != 0)
        {
            this.surface.SetKeyFocus(this.BrowserHandle, false);
        }
    }

    private static EHTMLMouseButton PointerPropertiesToHTMLMouseButton(PointerPointProperties prop)
    {
        switch (prop.PointerUpdateKind)
        {
            case PointerUpdateKind.LeftButtonPressed:
            case PointerUpdateKind.LeftButtonReleased:
                return EHTMLMouseButton.Left;

            case PointerUpdateKind.MiddleButtonPressed:
            case PointerUpdateKind.MiddleButtonReleased:
                return EHTMLMouseButton.Middle;

            case PointerUpdateKind.RightButtonPressed:
            case PointerUpdateKind.RightButtonReleased:
                return EHTMLMouseButton.Right;

            default:
                throw new Exception("PointerPressed received but no mouse buttons were down.");
        }
    }

    private static EHTMLKeyModifiers KeyModifiersToEHTMLKeyModifiers(KeyModifiers modifiers)
    {
        EHTMLKeyModifiers htmlKeyModifiers = EHTMLKeyModifiers.None;
        if (modifiers.HasFlag(KeyModifiers.Control))
        {
            htmlKeyModifiers |= EHTMLKeyModifiers.CtrlDown;
        }

        if (modifiers.HasFlag(KeyModifiers.Alt))
        {
            htmlKeyModifiers |= EHTMLKeyModifiers.AltDown;
        }

        if (modifiers.HasFlag(KeyModifiers.Shift))
        {
            htmlKeyModifiers |= EHTMLKeyModifiers.ShiftDown;
        }

        return htmlKeyModifiers;
    }

    private static int GetNativeKeyCodeForKeyEvent(KeyEventArgs e)
    {
        if (OperatingSystem.IsLinux())
        {
            return (int)X11KeyTransform.X11KeyFromKey(e.Key);
        }
        else if (OperatingSystem.IsWindows())
        {
            return 0;
            // return (int)e.NativeKeyCode;
        }

        throw new PlatformNotSupportedException("This OS is not supported.");
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // Keys shouldn't do anything else as long as this control has focus
        e.Handled = true;

        base.OnKeyDown(e);
        Console.WriteLine("OnKeyDown a:'" + e.Key + "' s:'" + e.KeySymbol + "' n:'" + GetNativeKeyCodeForKeyEvent(e) + "' " + " an:'" + e.NativeKeyCode + "'");
        if (this.BrowserHandle != 0)
        {
            // If the key has an avalonia-provided symbol AND the keypress doesn't have any modifiers it's eligible for being typed
            if (e.KeySymbol != null)
            {
                // strip out all control characters so we don't type them
                string controlCharsRemoved = new(e.KeySymbol.Where(c => !char.IsControl(c)).ToArray());

                // If the character didn't consist entirely of control characters, type it
                if (controlCharsRemoved != string.Empty)
                {
                    Console.WriteLine("is char");
                    this.surface.KeyChar(this.BrowserHandle, BitConverter.ToInt32(utfEncoder.GetBytes(e.KeySymbol)), KeyModifiersToEHTMLKeyModifiers(e.KeyModifiers));
                }
            }

            Console.WriteLine("is actual key");
            this.surface.KeyDown(this.BrowserHandle, GetNativeKeyCodeForKeyEvent(e), KeyModifiersToEHTMLKeyModifiers(e.KeyModifiers), false);
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        Console.WriteLine("OnKeyUp a:'" + e.Key + "' s:'" + e.KeySymbol + "' n:'" + GetNativeKeyCodeForKeyEvent(e) + "' " + " an:'" + e.NativeKeyCode + "'");
        if (this.BrowserHandle != 0)
        {
            this.surface.KeyUp(this.BrowserHandle, e.NativeKeyCode, KeyModifiersToEHTMLKeyModifiers(e.KeyModifiers));
        }
    }
}
