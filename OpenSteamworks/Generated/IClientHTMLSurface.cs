//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

global using HHTMLBrowser = System.UInt32;

using System;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Generated;

//NOTE: the client always seems to get a 65536 browser id for it's first browser
public unsafe interface IClientHTMLSurface
{
    public unknown_ret Unknown_0_DONTUSE();
    public unknown_ret Unknown_1_DONTUSE();
    public bool Init();
    public bool Shutdown();
    public SteamAPICall_t CreateBrowser(string userAgent, string? customCSS);
    public unknown_ret RemoveBrowser(HHTMLBrowser handle);
    public unknown_ret AllowStartRequest(HHTMLBrowser handle, bool allow);
    public unknown_ret LoadURL(HHTMLBrowser handle, string url, string postRequest = "");
    public unknown_ret SetSize(HHTMLBrowser handle, UInt32 unk2, UInt32 unk3);
    public unknown_ret StopLoad(HHTMLBrowser handle);
    public unknown_ret Reload(HHTMLBrowser handle);
    public unknown_ret GoBack(HHTMLBrowser handle);
    public unknown_ret GoForward(HHTMLBrowser handle);
    public unknown_ret AddHeader(HHTMLBrowser handle, string unk2, string unk3);
    public unknown_ret ExecuteJavascript(HHTMLBrowser handle, string unk2);
    public unknown_ret MouseUp(HHTMLBrowser handle, EHTMLMouseButton button);
    public unknown_ret MouseDown(HHTMLBrowser handle, EHTMLMouseButton button);
    public unknown_ret MouseDoubleClick(HHTMLBrowser handle, EHTMLMouseButton button);
    public unknown_ret MouseMove(HHTMLBrowser handle, int unk1, int unk2);
    public unknown_ret MouseWheel(HHTMLBrowser handle, int unk1);
    public unknown_ret KeyDown(HHTMLBrowser handle, UInt32 unk1, EHTMLKeyModifiers modifiers, bool unk2);
    public unknown_ret KeyUp(HHTMLBrowser handle, UInt32 unk1, EHTMLKeyModifiers modifiers);
    public unknown_ret KeyChar(HHTMLBrowser handle, UInt32 unk1, EHTMLKeyModifiers modifiers);
    public unknown_ret SetHorizontalScroll(HHTMLBrowser handle, UInt32 unk2);
    public unknown_ret SetVerticalScroll(HHTMLBrowser handle, UInt32 unk2);
    public unknown_ret SetKeyFocus(HHTMLBrowser handle, bool unk2);
    public unknown_ret ViewSource(HHTMLBrowser handle);
    public unknown_ret CopyToClipboard(HHTMLBrowser handle);
    public unknown_ret PasteFromClipboard(HHTMLBrowser handle);
    public unknown_ret Find(HHTMLBrowser handle, string unk2, bool unk3, bool unk4);
    public unknown_ret StopFind(HHTMLBrowser handle);
    public unknown_ret GetLinkAtPosition(HHTMLBrowser handle, int unk2, int unk3);
    public unknown_ret JSDialogResponse(HHTMLBrowser handle, bool unk2);
    public unknown_ret FileLoadDialogResponse(HHTMLBrowser handle, string unk2);
    public unknown_ret SetCookie(string unk1, string unk2, string unk3, string unk4, UInt32 unk5, bool unk6, bool unk7);
    public unknown_ret SetPageScaleFactor(HHTMLBrowser handle, float unk2, int unk3, int unk4);
    public unknown_ret SetBackgroundMode(HHTMLBrowser handle, bool unk2);
    public unknown_ret SetDPIScalingFactor(HHTMLBrowser handle, float unk2);
    public unknown_ret OpenDeveloperTools(HHTMLBrowser handle);
    public unknown_ret Validate(void* cvalidator, string unk);
}