//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================
using System;
using System.Runtime.Versioning;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Generated;

//NOTE: the client always seems to get a 65536 browser id for it's first browser
public unsafe interface IClientHTMLSurface
{
    public unknown_ret Unknown_0_DONTUSE();
    // The second destructor only exists on Linux (and maybe macos?)
#if !_WINDOWS
    public unknown_ret Unknown_1_DONTUSE();
#endif
    public bool Init();
    public bool Shutdown();
    public SteamAPICall<HTML_BrowserReady_t> CreateBrowser(string userAgent, string? customCSS);
    public void RemoveBrowser(HHTMLBrowser handle);
    public void AllowStartRequest(HHTMLBrowser handle, bool allow);
    public void LoadURL(HHTMLBrowser handle, string url, string? postRequest);
    public void SetSize(HHTMLBrowser handle, UInt32 width, UInt32 height);
    public void StopLoad(HHTMLBrowser handle);
    public void Reload(HHTMLBrowser handle);
    public void GoBack(HHTMLBrowser handle);
    public void GoForward(HHTMLBrowser handle);
    public void AddHeader(HHTMLBrowser handle, string key, string value);
    public void ExecuteJavascript(HHTMLBrowser handle, string jsToEval);
    public void MouseUp(HHTMLBrowser handle, EHTMLMouseButton button);
    public void MouseDown(HHTMLBrowser handle, EHTMLMouseButton button);
    public void MouseDoubleClick(HHTMLBrowser handle, EHTMLMouseButton button);
    public void MouseMove(HHTMLBrowser handle, int x, int y);
    // Might also have uint32 modifiers
    public void MouseWheel(HHTMLBrowser handle, int xDelta, int yDelta);
    /// <summary>
    /// Uses Xorg's KeySums on Linux, CGKeyCode on MacOS, VKey:s on Windows.
    /// Cannot type into text fields.
    /// </summary>
    public void KeyDown(HHTMLBrowser handle, int keyCode, EHTMLKeyModifiers modifiers, bool isSystemKey);
    public void KeyUp(HHTMLBrowser handle, int keyCode, EHTMLKeyModifiers modifiers);
    /// <summary>
    /// You can't feed control keys. Use KeyDown for it.
    /// Types characters into the current focused element
    /// </summary>
    public void KeyChar(HHTMLBrowser handle, int unicodeCharPoint, EHTMLKeyModifiers modifiers);
    public void SetHorizontalScroll(HHTMLBrowser handle, UInt32 absolutePos);
    public void SetVerticalScroll(HHTMLBrowser handle, UInt32 absolutePos);
    public void SetKeyFocus(HHTMLBrowser handle, bool hasFocus);
    public void ViewSource(HHTMLBrowser handle);
    public void CopyToClipboard(HHTMLBrowser handle);
    public void PasteFromClipboard(HHTMLBrowser handle);
    public void Find(HHTMLBrowser handle, string searchStr, bool goToNext, bool reverse);
    public void StopFind(HHTMLBrowser handle);
    public SteamAPICall_t GetLinkAtPosition(HHTMLBrowser handle, int x, int y);
    public void JSDialogResponse(HHTMLBrowser handle, bool response);
    public void FileLoadDialogResponse(HHTMLBrowser handle, string selectedPath);
    // Doesn't take a HHTMLBrowser. Wtf?
    public void SetCookie(string hostname, string key, string value, string path, RTime32 expiry, bool secure, bool httpOnly);
    public void SetPageScaleFactor(HHTMLBrowser handle, float zoom, int x, int y);
    public void SetBackgroundMode(HHTMLBrowser handle, bool backgroundMode);
    public void SetDPIScalingFactor(HHTMLBrowser handle, float scaleFactor);
    public unknown_ret OpenDeveloperTools(HHTMLBrowser handle);
    public unknown_ret Validate(void* cvalidator, string unk);
}