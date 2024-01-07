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
    public unknown_ret CreateHTTPRequest();  // argc: 2, index: 1, ipc args: [bytes4, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestContextValue();  // argc: 3, index: 2, ipc args: [bytes4, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestNetworkActivityTimeout();  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestHeaderValue();  // argc: 3, index: 4, ipc args: [bytes4, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestGetOrPostParameter();  // argc: 3, index: 5, ipc args: [bytes4, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SendHTTPRequest();  // argc: 2, index: 6, ipc args: [bytes4], ipc returns: [bytes1, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SendHTTPRequestAndStreamResponse();  // argc: 2, index: 7, ipc args: [bytes4], ipc returns: [bytes1, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DeferHTTPRequest();  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret PrioritizeHTTPRequest();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CancelHTTPRequest();  // argc: 1, index: 10, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseHeaderSize();  // argc: 3, index: 11, ipc args: [bytes4, string], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseHeaderValue();  // argc: 4, index: 12, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseBodySize();  // argc: 2, index: 13, ipc args: [bytes4], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPResponseBodyData();  // argc: 3, index: 14, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPStreamingResponseBodyData();  // argc: 4, index: 15, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseHTTPRequest();  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPDownloadProgressPct();  // argc: 2, index: 17, ipc args: [bytes4], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestRawPostBody();  // argc: 4, index: 18, ipc args: [bytes4, string, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateCookieContainer();  // argc: 1, index: 19, ipc args: [bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseCookieContainer();  // argc: 1, index: 20, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCookie();  // argc: 4, index: 21, ipc args: [bytes4, string, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestCookieContainer();  // argc: 2, index: 22, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestUserAgentInfo();  // argc: 2, index: 23, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestRequiresVerifiedCertificate();  // argc: 2, index: 24, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestAbsoluteTimeoutMS();  // argc: 2, index: 25, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHTTPRequestWasTimedOut();  // argc: 2, index: 26, ipc args: [bytes4], ipc returns: [bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetHTTPRequestRedirectsEnabled();  // argc: 2, index: 27, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SaveHTTPResponseBodyToDisk();  // argc: 3, index: 28, ipc args: [bytes4, string], ipc returns: [bytes1, bytes8]
}