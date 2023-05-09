#pragma once

#include <queue>
#include <vector>
#include "../../interop/app.h"

struct DownloadItem
{
    App *app;
    
};

class DownloadManager
{
private:
    /* data */
    DownloadItem currentDownload;
    std::queue<DownloadItem> queue;
    std::vector<DownloadItem> unscheduled;

public:
    DownloadManager(/* args */);
    ~DownloadManager();
};

DownloadManager::DownloadManager(/* args */)
{
}

DownloadManager::~DownloadManager()
{
}
