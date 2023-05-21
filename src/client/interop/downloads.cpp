#include "downloads.h"
#include "../ext/steamclient.h"
#include <opensteamworks/IClientAppManager.h>
#include "appmanager.h"
#include "../globals.h"
#include "../threading/threadcontroller.h"

extern void LogAppUpdateInfo(AppUpdateInfo_s);

DownloadManager::DownloadManager(AppManager *appManager)
{
    this->appManager = appManager;
    ReloadDownloadInfo();
    connect(Global_ThreadController->callbackThread, &CallbackThread::DownloadScheduleChanged, this, &DownloadManager::CallbackManager_DownloadScheduleChanged);
}

DownloadManager::~DownloadManager()
{

}

void DownloadManager::CallbackManager_DownloadScheduleChanged(DownloadScheduleChanged_t info) {
    DEBUG_MSG << "DownloadScheduleChanged" << std::endl;

    DEBUG_MSG << "downloadsenabled " << info.m_bDownloadEnabled << std::endl;
    DEBUG_MSG << "unk1 " << info.unk1 << std::endl;
    DEBUG_MSG << "unk2 " << info.unk2 << std::endl;
    DEBUG_MSG << "totalAppsScheduled " << info.m_nTotalAppsScheduled << std::endl;

    {
        std::lock_guard guard(changeMutex);
        this->queue.clear();

        for (size_t i = 0; i < info.m_nTotalAppsScheduled; i++)
        {
            AppId_t appid = info.m_rgunAppSchedule[i];
            int index = Global_SteamClientMgr->ClientAppManager->GetAppDownloadQueueIndex(appid);

            DownloadItem *item = nullptr;
            if (!appItems.contains(appid) || appItems.at(appid) == nullptr)
            {
                item = ConstructDownloadItem(appid);
            } 
            else 
            {
                item = appItems.at(appid);
            }
            

            if (item != nullptr) {
                this->queue.insert({index, item});
                continue;
            }
            
            std::cerr << "[DownloadManager] WARNING: Failed to find DownloadItem for appid " << appid << ", queue will be missing an item!" << std::endl;
        }
    }
}

DownloadItem *DownloadManager::ConstructDownloadItem(AppId_t appid, bool bIsDepot) 
{
    if (!this->appManager->apps.contains(appid) && !bIsDepot) {
        std::cerr << "[DownloadManager] No app found for id " << appid << std::endl;
        return nullptr;
    }

    App *app = this->appManager->apps.at(appid);

    auto di = new DownloadItem();
    
    if (bIsDepot) {
        di->app = nullptr;
        di->type = k_DownloadTypeDepot;
        di->autostartTime = 0;
    } else {
        di->app = app;
        di->type = k_DownloadTypeApp;
        di->autostartTime = Global_SteamClientMgr->ClientAppManager->GetAppAutoUpdateDelayedUntilTime(appid);
        LogAppUpdateInfo(app->updateInfo);
    }

    di->id = appid;

    items.push_back({di});
    switch (di->type)
    {
    case k_DownloadTypeApp:
        appItems.insert({appid, di});
        break;
    case k_DownloadTypeDepot:
        depotItems.insert({appid, di});
        break;

    default:
        break;
    }

    return di;
}

void DownloadManager::ReloadDownloadInfo() {
    std::lock_guard guard(changeMutex);

    for (auto &&i : items)
    {
        delete i;
    }

    this->items.clear();

    this->queue.clear();
    this->unscheduled.clear();
    this->scheduled.clear();

    DEBUG_MSG << "[DownloadManager] Populating download queue" << std::endl;

    this->updateCount = 0;

    for (auto &&i : this->appManager->apps)
    {
        if (!i.second->state->UpdateRequired) {
            continue;
        }

        this->updateCount++;

        i.second->UpdateUpdateInfo();

        int index = Global_SteamClientMgr->ClientAppManager->GetAppDownloadQueueIndex(i.first);

        DEBUG_MSG << "[DownloadManager] index for " << i.second->name << " is " << index << std::endl;
        DEBUG_MSG << "[DownloadManager] delayed until " << Global_SteamClientMgr->ClientAppManager->GetAppAutoUpdateDelayedUntilTime(i.first) << std::endl;

        ConstructDownloadItem(i.first);
    }

    for (auto &&i : items)
    {
        if (i->type != k_DownloadTypeApp) {
            return;
        }

        if (i->autostartTime != 0) {
            i->scheduled = true;
            this->scheduled.insert({i->autostartTime, i});
        } else {
            i->unscheduled = true;
            this->unscheduled.insert({i->app->name, i});
        }

        int index = Global_SteamClientMgr->ClientAppManager->GetAppDownloadQueueIndex(i->app->appid);

        if (index != -1) {
            i->queued = true;
            i->queueIndex = index;
            this->queue.insert({index, i});
        }
    }
}

// Calculates changes of the apps, fires events and deletes finished items.
void DownloadManager::ApplyChanges(std::map<int, DownloadItem*> newQueue, std::map<RTime32, DownloadItem*> newScheduled, std::map<std::string, DownloadItem*> newUnscheduled) 
{
    std::lock_guard guard(changeMutex);
}

void DownloadManager::MoveItemToPositionInQueue(DownloadItem *item, int newPos) {
    Global_SteamClientMgr->ClientAppManager->SetAppDownloadQueueIndex(item->id, newPos);
}

void DownloadManager::QueueItem(DownloadItem *item, int newPos) {
    if (newPos < 0) {
        Global_SteamClientMgr->ClientAppManager->ChangeAppDownloadQueuePlacement(item->id, k_EAppDownloadQueuePlacementPriorityAutoUpdate);
    } else {
        Global_SteamClientMgr->ClientAppManager->SetAppDownloadQueueIndex(item->id, newPos);
    }
    
}

void DownloadManager::UnqueueItem(DownloadItem *item) {
    Global_SteamClientMgr->ClientAppManager->ChangeAppDownloadQueuePlacement(item->id, k_EAppDownloadQueuePlacementPriorityNone);
}