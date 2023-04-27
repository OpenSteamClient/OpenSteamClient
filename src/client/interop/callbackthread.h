#include <thread>
#include <algorithm>
#include <map>
#include <QObject>
#include "includesteamworks.h"
#include "../globals.h"
#include "../threading/thread.h"

#pragma once

class CallbackThread : public Thread
{
    Q_OBJECT
private:
    bool shouldStop = false;
    std::multimap<int, void*> handlers;

public:
    std::string ThreadName();
    void ThreadMain();
    void StopThread();
    CallbackThread() {}
    ~CallbackThread() {}
signals:
    void AppLicensesChanged(AppLicensesChanged_t changeInfo);

    // Called when a login fails
    // Also called when the connection fails for any other reason
    void SteamServerConnectFailure(SteamServerConnectFailure_t failureInfo);

    // Called when the connection has been re-estabilished or estabilished for the first time,
    // For example on first logon
    void SteamServersConnected(SteamServersConnected_t successInfo);

    void PostLogonState(PostLogonState_t info);

    void CheckAppBetaPasswordResponse(CheckAppBetaPasswordResponse_t info);
};