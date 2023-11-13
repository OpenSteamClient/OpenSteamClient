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

namespace ClientUI.Controls;

public static class AvaloniaCursors {
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

    
    public static Cursor GetCursorForEMouseCursor(EMouseCursor steamCursor) {
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
        private SKImageInfo targetBitmapInfo;
        private readonly SKPaint simplePaint;

        public HTMLBufferImg(SKColorType format, SKAlphaType alphaFormat, int width, int height) {
            this.width = width;
            this.height = height;
            Console.WriteLine("allocating bitmap of size " + width + "x" + height);
            this.targetBitmapInfo = new SKImageInfo(width, height, format, alphaFormat);
            this.targetBitmap = AllocBitmap();
            this.simplePaint = new SKPaint
            {
                IsAntialias = false,
                HintingLevel = SKPaintHinting.NoHinting,
                IsAutohinted = false,
                IsDither = false,
                SubpixelText = false,
                BlendMode = SKBlendMode.SrcIn
            };
        }

        private SKBitmap AllocBitmap() {
            if (targetBitmapInfo.Width == 0 || targetBitmapInfo.Height == 0) {
                throw new InvalidOperationException("Cannot allocate a bitmap of size 0");
            }

            return new(targetBitmapInfo, SKBitmapAllocFlags.ZeroPixels);
        }

        public Rect Bounds => new(0, 0, width, height);

        public bool Equals(ICustomDrawOperation? other)
        {
            return false;
        }

        public bool HitTest(Point p)
        {
            return Bounds.Intersects(new Rect(p.X, p.Y, 1, 1));
        }

        private Rect renderMaxRect;
        public void PreRender(Rect renderMaxRect) {
            this.renderMaxRect = renderMaxRect;
        }

        public void Render(ImmediateDrawingContext context)
        {
            var leasef = context.PlatformImpl.GetFeature<ISkiaSharpApiLeaseFeature>();
            if (leasef == null) {
                throw new PlatformNotSupportedException("Your platform does not support SkiaSharpApiLeaseFeature.");
            }
            
            using (var lease = leasef.Lease())
            {
                lock (targetBitmapLock) {
                    lease.SkCanvas.DrawBitmap(targetBitmap, renderMaxRect.ToSKRect());
                }
            }
        }

        public void Resize(int newWidth, int newHeight) {
            lock (targetBitmapLock) {
                ResizeInternal(newWidth, newHeight);
            }
        }

        private void ResizeInternal(int newWidth, int newHeight) {
            // Disallow resizing to 0x0
            if (newWidth == 0 || newHeight == 0) {
                Console.WriteLine("Trying to resize to 0! Resize ignored");
                return;
            }

            this.width = newWidth;
            this.height = newHeight;
            targetBitmapInfo = new SKImageInfo(width, height, this.targetBitmapInfo.ColorType, this.targetBitmapInfo.AlphaType);
            targetBitmap.Dispose();
            targetBitmap = AllocBitmap();
        }

        public void UpdateData(int newWidth, int newHeight, IntPtr dataPtr) {
            Console.WriteLine("UpdateData called " + newWidth + "x" + newHeight + " : " + dataPtr);
            lock (targetBitmapLock) {
                if (this.targetBitmapInfo.Width != newWidth || this.targetBitmapInfo.Height != newHeight) {
                    ResizeInternal(newWidth, newHeight);
                }

                if (!targetBitmap.InstallPixels(targetBitmapInfo, dataPtr)) {
                    throw new Exception("InstallPixels failed");
                }
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

    private static int initCount = 0;
    private readonly HTMLBufferImg htmlImgBuffer;
    private readonly ISteamHTMLSurface005 ssurface;
    private readonly IClientHTMLSurface surface;
    private readonly SteamClient client;
    private static readonly Encoding utfEncoder = new UTF32Encoding(false, true, false);
    public HHTMLBrowser BrowserHandle { get; private set; } = 0;
    // The scroll multiplier affects how fast scrolling works. Piping the scroll wheel directly into steam makes scrolling slow, so simply multiply it by this value.
    private const int SCROLL_MULTIPLIER = 20;
    
    public HTMLSurface(SteamClient client) : base()
    {
        InitializeComponent();
        // Allocate an initial buffer at 720p, since we don't know our size yet
        this.htmlImgBuffer = new HTMLBufferImg(SKColorType.Bgra8888, SKAlphaType.Unpremul, 720, 1080);
        this.Focusable = true;
        this.client = client;
        this.ssurface = client.NativeClient.ISteamHTMLSurface;
        this.surface = client.NativeClient.IClientHTMLSurface;
        client.CallbackManager.RegisterHandler<HTML_NeedsPaint_t>(this.OnHTML_NeedsPaint);
        client.CallbackManager.RegisterHandler<HTML_SetCursor_t>(this.OnHTML_SetCursor);
        // Reactive bloat. No events here, need to do this instead...
        this.GetObservable(BoundsProperty).Subscribe(new AnonymousObserver<Rect>(BoundsChange));
    }

    private void OnHTML_SetCursor(CallbackHandler handler, HTML_SetCursor_t setCursor) {
        if (setCursor.unBrowserHandle == BrowserHandle) {
            SetCursorNonUICode(AvaloniaCursors.GetCursorForEMouseCursor(setCursor.eMouseCursor));
        }
    }

    private void SetCursorNonUICode(Cursor cursor) {
        Dispatcher.UIThread.InvokeAsync(() => {
            this.Cursor = cursor;
        }, DispatcherPriority.Input);
    }

    private void ForceRedrawNonUICode() {
        Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Render);
    }

    private void BoundsChange(Rect newBounds) {
        if (this.BrowserHandle != 0) {
            this.surface.SetSize(this.BrowserHandle, (uint)newBounds.Width, (uint)newBounds.Height);
        }
    }
    
    private void OnHTML_NeedsPaint(CallbackHandler handler, HTML_NeedsPaint_t paintEvent) {
        if (paintEvent.unBrowserHandle == this.BrowserHandle) {
            htmlImgBuffer.UpdateData((int)paintEvent.unWide, (int)paintEvent.unTall, paintEvent.pBGRA);
            this.ForceRedrawNonUICode();
        } else {
            Console.WriteLine("Ignoring paint event for browser we don't own: " + paintEvent.unBrowserHandle + ", ours: " + this.BrowserHandle);
        }
    }

    public async Task<HHTMLBrowser> CreateBrowser(string userAgent, string? customCSS) {
        if (initCount == 0) {
            Console.WriteLine("Initializing IClientHTMLSurface");
            this.surface.Init();
        }
        initCount++;

        this.client.CallbackManager.PauseThread();
        var callHandle = this.surface.CreateBrowser(userAgent, customCSS);
        if (callHandle == 0) {
            throw new InvalidOperationException("CreateBrowser failed due to no call handle being returned.");
        }

        var result = await this.client.CallbackManager.WaitForAPICallResult<HTML_BrowserReady_t>(callHandle, true, new CancellationTokenSource(2000).Token);
        if (result.failed) {
            throw new InvalidOperationException("CreateBrowser failed due to CallResult failure: " + result.failureReason);
        }

        this.BrowserHandle = result.data.unBrowserHandle;

        Console.WriteLine("Created new browser with handle " + this.BrowserHandle);
        this.surface.AllowStartRequest(this.BrowserHandle, true);
        this.htmlImgBuffer.Resize((int)this.Bounds.Width, (int)this.Bounds.Height);
        this.surface.SetSize(this.BrowserHandle, (uint)this.Bounds.Width, (uint)this.Bounds.Height);

        return result.data.unBrowserHandle;
    }

    public void RemoveBrowser() {
        if (this.BrowserHandle == 0) {
            return;
        }

        // Free surfaces
        this.surface.RemoveBrowser(this.BrowserHandle);

        // Shutdown the interface if no other surfaces are left
        initCount--;
        if (initCount == 0) {
            Console.WriteLine("Freeing IClientHTMLSurface, no surfaces left");
            this.surface.Shutdown();
        }
    }
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);
        this.htmlImgBuffer.PreRender(new Rect(0, 0, Bounds.Width, Bounds.Height));
        context.Custom(this.htmlImgBuffer);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e) {
        base.OnPointerWheelChanged(e);
        if (this.BrowserHandle != 0) {
            this.surface.MouseWheel(this.BrowserHandle, (int)e.Delta.Y * SCROLL_MULTIPLIER, (int)e.Delta.X * SCROLL_MULTIPLIER);
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e) {
        base.OnPointerMoved(e);
        if (this.BrowserHandle != 0) {
            var rect = e.GetCurrentPoint(this).Position;
            this.surface.MouseMove(this.BrowserHandle, (int)rect.X, (int)rect.Y);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e) {
        base.OnPointerMoved(e);
        if (this.BrowserHandle != 0) {
            var prop = e.GetCurrentPoint(this).Properties;
            var btn = PointerPropertiesToHTMLMouseButton(prop);
            this.surface.MouseDown(this.BrowserHandle, btn);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e) {
        base.OnPointerMoved(e);
        if (this.BrowserHandle != 0) {
            var prop = e.GetCurrentPoint(this).Properties;
            var btn = PointerPropertiesToHTMLMouseButton(prop);
            this.surface.MouseUp(this.BrowserHandle, btn);
        }
    }

    protected override void OnGotFocus(GotFocusEventArgs e) {
        base.OnGotFocus(e);
        if (this.BrowserHandle != 0) {
            this.surface.SetKeyFocus(this.BrowserHandle, true);
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e) {
        base.OnLostFocus(e);
        if (this.BrowserHandle != 0) {
            //this.surface.SetKeyFocus(this.BrowserHandle, false);
        }
    }

    private static EHTMLMouseButton PointerPropertiesToHTMLMouseButton(PointerPointProperties prop) {
        switch (prop.PointerUpdateKind)
        {
            case PointerUpdateKind.LeftButtonPressed:
            case PointerUpdateKind.LeftButtonReleased:
                return EHTMLMouseButton.k_EHTMLMouseButton_Left;

            case PointerUpdateKind.MiddleButtonPressed:
            case PointerUpdateKind.MiddleButtonReleased:
                return EHTMLMouseButton.k_EHTMLMouseButton_Middle;

            case PointerUpdateKind.RightButtonPressed:
            case PointerUpdateKind.RightButtonReleased:
                return EHTMLMouseButton.k_EHTMLMouseButton_Right;
            
            default:
                throw new Exception("PointerPressed received but no mouse buttons were down.");
        }
    }

    private static EHTMLKeyModifiers KeyModifiersToEHTMLKeyModifiers(KeyModifiers modifiers) {
        EHTMLKeyModifiers htmlKeyModifiers = EHTMLKeyModifiers.k_EHTMLKeyModifier_None;
        if (modifiers.HasFlag(KeyModifiers.Control)) {
            htmlKeyModifiers |= EHTMLKeyModifiers.k_EHTMLKeyModifier_CtrlDown;
        }
        
        if (modifiers.HasFlag(KeyModifiers.Alt)) {
            htmlKeyModifiers |= EHTMLKeyModifiers.k_EHTMLKeyModifier_AltDown;
        }

        if (modifiers.HasFlag(KeyModifiers.Shift)) {
            htmlKeyModifiers |= EHTMLKeyModifiers.k_EHTMLKeyModifier_ShiftDown;
        }

        return htmlKeyModifiers;
    }

    private static int GetNativeKeyCodeForKey(Key key) {
        if (OperatingSystem.IsLinux()) {
            return (int)X11KeyTransform.X11KeyFromKey(key);
        } else if (OperatingSystem.IsWindows()) {
            return 0;
        }

        throw new PlatformNotSupportedException("This OS is not supported.");
    }

    protected override void OnKeyDown(KeyEventArgs e) {
        // Keys shouldn't do anything else as long as this control has focus
        e.Handled = true;

        base.OnKeyDown(e);
        Console.WriteLine("OnKeyDown a:'" + e.Key + "' s:'" + e.KeySymbol + "' n:'" + GetNativeKeyCodeForKey(e.Key) + "' " + " an:'" + e.NativeKeyCode + "'");
        if (this.BrowserHandle != 0) {
            // If the key has an avalonia-provided symbol AND the keypress doesn't have any modifiers it's eligible for being typed
            if (false && e.KeySymbol != null && e.KeyModifiers == 0) {
                // strip out all control characters so we don't type them
                string controlCharsRemoved = new(e.KeySymbol.Where(c => !char.IsControl(c)).ToArray());

                // If the character didn't consist entirely of control characters, type it
                if (controlCharsRemoved != string.Empty) {
                    Console.WriteLine("is char");
                    this.surface.KeyChar(this.BrowserHandle, BitConverter.ToInt32(utfEncoder.GetBytes(e.KeySymbol)), KeyModifiersToEHTMLKeyModifiers(e.KeyModifiers));
                }
            }
            
            Console.WriteLine("is actual key");
            this.ssurface.KeyDown(this.BrowserHandle, e.NativeKeyCode, KeyModifiersToEHTMLKeyModifiers(e.KeyModifiers));
        }
    }
    
    protected override void OnKeyUp(KeyEventArgs e) {
        base.OnKeyUp(e);
        Console.WriteLine("OnKeyUp a:'" + e.Key + "' s:'" + e.KeySymbol + "' n:'" + GetNativeKeyCodeForKey(e.Key) + "' " + " an:'" + e.NativeKeyCode + "'");
        if (this.BrowserHandle != 0) {
            if (e.KeySymbol == null) {
                this.ssurface.KeyUp(this.BrowserHandle, e.NativeKeyCode, KeyModifiersToEHTMLKeyModifiers(e.KeyModifiers));
            }
        }
    }
}
