#include <thread>
#include <algorithm>
#include <map>
#include <QObject>
#include "../../interop/includesteamworks.h"
#include "../../globals.h"
#include "../../threading/thread.h"

#pragma once

struct DownloadSpeedInfo {
    uint64 downloadSpeed = 0;
    uint64 topDownloadSpeed = 0;
    uint64 totalDownloaded = 0;
};

class DownloadInfoThread : public Thread
{
    Q_OBJECT
private:
    bool shouldStop = false;

public:
    std::string ThreadName();
    void ThreadMain();
    void StopThread();
    DownloadInfoThread() {}
    ~DownloadInfoThread() {}
signals:
    void DownloadSpeedUpdate(DownloadSpeedInfo);
    void DownloadingAppChange(AppId_t);
};