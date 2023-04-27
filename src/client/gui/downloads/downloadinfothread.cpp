#include "downloadinfothread.h"
#include "../../ext/steamclient.h"

std::string DownloadInfoThread::ThreadName() {
    return "DownloadInfoPoller";
}

#define UPDATE_INTERVAL 1

void DownloadInfoThread::ThreadMain()
{
    std::cout << "DownloadInfoThread started" << std::endl;
    uint64 totalDownloadedLast = 0;
    DownloadSpeedInfo speedInfo;
    AppId_t prevDownloadingApp = 0;
    do
    {
        DownloadStats_s stats;
        Global_SteamClientMgr->ClientAppManager->GetDownloadStats(&stats);

        speedInfo.downloadSpeed = (stats.totalDownloaded - totalDownloadedLast) * UPDATE_INTERVAL;
        totalDownloadedLast = stats.totalDownloaded;
        speedInfo.totalDownloaded = stats.totalDownloaded;

        if (speedInfo.downloadSpeed > speedInfo.topDownloadSpeed) {
            speedInfo.topDownloadSpeed = speedInfo.downloadSpeed;
        }

        DEBUG_MSG << "DOWNLOAD STAT DEBUG: step: " << stats.currentStep << " total: " << stats.totalDownloaded << " estimatedSpeed: " << stats.estimatedDownloadSpeed << " speed: " << std::to_string(speedInfo.downloadSpeed) << " topspeed: " << std::to_string(speedInfo.topDownloadSpeed) << std::endl;
        emit DownloadSpeedUpdate(speedInfo);

        if (prevDownloadingApp != Global_SteamClientMgr->ClientAppManager->GetDownloadingAppID()) {
            prevDownloadingApp = Global_SteamClientMgr->ClientAppManager->GetDownloadingAppID();
            emit DownloadingAppChange(prevDownloadingApp);
        }
        
        std::this_thread::sleep_for(std::chrono::milliseconds(1000 / UPDATE_INTERVAL));
    } while (!shouldStop);
}
void DownloadInfoThread::StopThread() {
    this->shouldStop = true;
}