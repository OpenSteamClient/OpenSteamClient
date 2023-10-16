var fs = require('fs');
var path = require('path');

var structs = [
    {
    "callback_id": 4501,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" }
    ],
    "struct": "HTML_BrowserReady_t"
    },
    {
    "callback_id": 4502,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pBGRA", "fieldtype":"const char *" },
        { "fieldname":"unWide", "fieldtype":"uint32" },
        { "fieldname":"unTall", "fieldtype":"uint32" },
        { "fieldname":"unUpdateX", "fieldtype":"uint32" },
        { "fieldname":"unUpdateY", "fieldtype":"uint32" },
        { "fieldname":"unUpdateWide", "fieldtype":"uint32" },
        { "fieldname":"unUpdateTall", "fieldtype":"uint32" },
        { "fieldname":"unScrollX", "fieldtype":"uint32" },
        { "fieldname":"unScrollY", "fieldtype":"uint32" },
        { "fieldname":"flPageScale", "fieldtype":"float" },
        { "fieldname":"unPageSerial", "fieldtype":"uint32" }
    ],
    "struct": "HTML_NeedsPaint_t"
    },
    {
    "callback_id": 4503,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchURL", "fieldtype":"const char *" },
        { "fieldname":"pchTarget", "fieldtype":"const char *" },
        { "fieldname":"pchPostData", "fieldtype":"const char *" },
        { "fieldname":"bIsRedirect", "fieldtype":"bool" }
    ],
    "struct": "HTML_StartRequest_t"
    },
    {
    "callback_id": 4504,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" }
    ],
    "struct": "HTML_CloseBrowser_t"
    },
    {
    "callback_id": 4505,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchURL", "fieldtype":"const char *" },
        { "fieldname":"pchPostData", "fieldtype":"const char *" },
        { "fieldname":"bIsRedirect", "fieldtype":"bool" },
        { "fieldname":"pchPageTitle", "fieldtype":"const char *" },
        { "fieldname":"bNewNavigation", "fieldtype":"bool" }
    ],
    "struct": "HTML_URLChanged_t"
    },
    {
    "callback_id": 4506,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchURL", "fieldtype":"const char *" },
        { "fieldname":"pchPageTitle", "fieldtype":"const char *" }
    ],
    "struct": "HTML_FinishedRequest_t"
    },
    {
    "callback_id": 4507,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchURL", "fieldtype":"const char *" }
    ],
    "struct": "HTML_OpenLinkInNewTab_t"
    },
    {
    "callback_id": 4508,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchTitle", "fieldtype":"const char *" }
    ],
    "struct": "HTML_ChangedTitle_t"
    },
    {
    "callback_id": 4509,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"unResults", "fieldtype":"uint32" },
        { "fieldname":"unCurrentMatch", "fieldtype":"uint32" }
    ],
    "struct": "HTML_SearchResults_t"
    },
    {
    "callback_id": 4510,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"bCanGoBack", "fieldtype":"bool" },
        { "fieldname":"bCanGoForward", "fieldtype":"bool" }
    ],
    "struct": "HTML_CanGoBackAndForward_t"
    },
    {
    "callback_id": 4511,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"unScrollMax", "fieldtype":"uint32" },
        { "fieldname":"unScrollCurrent", "fieldtype":"uint32" },
        { "fieldname":"flPageScale", "fieldtype":"float" },
        { "fieldname":"bVisible", "fieldtype":"bool" },
        { "fieldname":"unPageSize", "fieldtype":"uint32" }
    ],
    "struct": "HTML_HorizontalScroll_t"
    },
    {
    "callback_id": 4512,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"unScrollMax", "fieldtype":"uint32" },
        { "fieldname":"unScrollCurrent", "fieldtype":"uint32" },
        { "fieldname":"flPageScale", "fieldtype":"float" },
        { "fieldname":"bVisible", "fieldtype":"bool" },
        { "fieldname":"unPageSize", "fieldtype":"uint32" }
    ],
    "struct": "HTML_VerticalScroll_t"
    },
    {
    "callback_id": 4513,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"x", "fieldtype":"uint32" },
        { "fieldname":"y", "fieldtype":"uint32" },
        { "fieldname":"pchURL", "fieldtype":"const char *" },
        { "fieldname":"bInput", "fieldtype":"bool" },
        { "fieldname":"bLiveLink", "fieldtype":"bool" }
    ],
    "struct": "HTML_LinkAtPosition_t"
    },
    {
    "callback_id": 4514,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchMessage", "fieldtype":"const char *" }
    ],
    "struct": "HTML_JSAlert_t"
    },
    {
    "callback_id": 4515,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchMessage", "fieldtype":"const char *" }
    ],
    "struct": "HTML_JSConfirm_t"
    },
    {
    "callback_id": 4516,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchTitle", "fieldtype":"const char *" },
        { "fieldname":"pchInitialFile", "fieldtype":"const char *" }
    ],
    "struct": "HTML_FileOpenDialog_t"
    },
    {
    "callback_id": 4521,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchURL", "fieldtype":"const char *" },
        { "fieldname":"unX", "fieldtype":"uint32" },
        { "fieldname":"unY", "fieldtype":"uint32" },
        { "fieldname":"unWide", "fieldtype":"uint32" },
        { "fieldname":"unTall", "fieldtype":"uint32" },
        { "fieldname":"unNewWindow_BrowserHandle_IGNORE", "fieldtype":"HHTMLBrowser" }
    ],
    "struct": "HTML_NewWindow_t"
    },
    {
    "callback_id": 4522,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"eMouseCursor", "fieldtype":"uint32" }
    ],
    "struct": "HTML_SetCursor_t"
    },
    {
    "callback_id": 4523,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchMsg", "fieldtype":"const char *" }
    ],
    "struct": "HTML_StatusText_t"
    },
    {
    "callback_id": 4524,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchMsg", "fieldtype":"const char *" }
    ],
    "struct": "HTML_ShowToolTip_t"
    },
    {
    "callback_id": 4525,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"pchMsg", "fieldtype":"const char *" }
    ],
    "struct": "HTML_UpdateToolTip_t"
    },
    {
    "callback_id": 4526,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" }
    ],
    "struct": "HTML_HideToolTip_t"
    },
    {
    "callback_id": 4527,
    "fields": [
        { "fieldname":"unBrowserHandle", "fieldtype":"HHTMLBrowser" },
        { "fieldname":"unOldBrowserHandle", "fieldtype":"HHTMLBrowser" }
    ],
    "struct": "HTML_BrowserRestarted_t"
    },
]

console.log("Callback name map:");
structs.forEach((struct) => {
    // {101, "23SteamServersConnected_t"},
    console.log("{"+struct.callback_id + ', "' + struct.struct + '"},');
});

console.log("Callback data map:");
structs.forEach((struct) => {
    console.log("{typeof("+struct.struct+"), " + struct.callback_id + "},");
});
