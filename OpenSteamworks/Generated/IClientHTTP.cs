//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientHTTP
{
    // WARNING: Arguments are unknown!
    public unknown_ret CreateHTTPRequest();  // argc: 2, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestContextValue();  // argc: 3, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestNetworkActivityTimeout();  // argc: 2, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestHeaderValue();  // argc: 3, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestGetOrPostParameter();  // argc: 3, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret SendHTTPRequest();  // argc: 2, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret SendHTTPRequestAndStreamResponse();  // argc: 2, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret DeferHTTPRequest();  // argc: 1, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret PrioritizeHTTPRequest();  // argc: 1, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret CancelHTTPRequest();  // argc: 1, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseHeaderSize();  // argc: 3, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseHeaderValue();  // argc: 4, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseBodySize();  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseBodyData();  // argc: 3, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPStreamingResponseBodyData();  // argc: 4, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseHTTPRequest();  // argc: 1, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPDownloadProgressPct();  // argc: 2, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestRawPostBody();  // argc: 4, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret CreateCookieContainer();  // argc: 1, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseCookieContainer();  // argc: 1, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret SetCookie();  // argc: 4, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestCookieContainer();  // argc: 2, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestUserAgentInfo();  // argc: 2, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestRequiresVerifiedCertificate();  // argc: 2, index: 24
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestAbsoluteTimeoutMS();  // argc: 2, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPRequestWasTimedOut();  // argc: 2, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestRedirectsEnabled();  // argc: 2, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret SaveHTTPResponseBodyToDisk();  // argc: 3, index: 28
}