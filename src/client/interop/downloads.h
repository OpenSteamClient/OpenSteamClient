#pragma once

#include <queue>
#include <vector>
#include "app.h"
#include <QObject>
#include <mutex>

enum DownloadType
{
    k_DownloadTypeApp,
    k_DownloadTypeDepot,
    k_DownloadTypeInvalid
};

struct DownloadItem
{
    DownloadType type = k_DownloadTypeInvalid;
    
    // Can also be a DepotId_t
    AppId_t id = 0;

    App *app = nullptr;
    RTime32 autostartTime = 0;

    int queueIndex = -1;
    bool queued = false;
    bool scheduled = false;
    bool unscheduled = false;
};

// This class is rather suboptimal.
//TODO: depot support
class DownloadManager : public QObject
{
    Q_OBJECT
private:
    // All items
    std::vector<DownloadItem *> items;
    std::map<AppId_t, DownloadItem*> appItems;
    std::map<DepotId_t, DownloadItem*> depotItems;

    std::mutex changeMutex;
    AppManager *appManager;

public: 
    // The client internally implements these as linked lists. Should we do it similarly?

    // Current download. Nullptr if none.
    DownloadItem *currentDownload;

    // The queue is sorted however the user wants
    // After a download completes, the next app in the queue will automatically start downloading
    std::map<int, DownloadItem*> queue;

    // Scheduled apps start downloading when the time matches
    std::map<RTime32, DownloadItem*> scheduled;

    // Sorted alphabetically always, can't rearrange
    std::map<std::string, DownloadItem*> unscheduled;

    // How many apps have updates
    int updateCount = 0;

public:
    DownloadManager(AppManager *appManager);
    ~DownloadManager();
    void ReloadDownloadInfo();

private:
    // You can also pass in a DepotId_t
    DownloadItem *ConstructDownloadItem(AppId_t appid, bool bIsDepot = false);

    void ApplyChanges(std::map<int, DownloadItem *> newQueue, std::map<RTime32, DownloadItem *> newScheduled, std::map<std::string, DownloadItem *> newUnscheduled);

signals:
    void ItemMoved(DownloadItem* item, int newPos);
    void ItemQueued(DownloadItem *item);
    void ItemScheduled(DownloadItem *item);
    void ItemUnscheduled(DownloadItem *item);
    
public slots:
    // 
    void MoveItemToPositionInQueue(DownloadItem *item, int newPos);
    // Move an item from unscheduled to queue. Pass newPos if you want it to go to a specific position in queue.
    void QueueItem(DownloadItem *item, int newPos = -1);
    void UnqueueItem(DownloadItem *item);

private slots:
    void CallbackManager_DownloadScheduleChanged(DownloadScheduleChanged_t info);

};