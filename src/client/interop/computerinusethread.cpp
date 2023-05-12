#include "computerinusethread.h"
#include "../ext/steamclient.h"
#include <opensteamworks/IClientUser.h>

std::string ComputerInUseThread::ThreadName() {
    return "ComputerInUse";
}

void ComputerInUseThread::ThreadMain() {
    do
    {
        Global_SteamClientMgr->ClientUser->SetComputerInUse();
        std::this_thread::sleep_for(std::chrono::seconds(5));
    } while (!shouldStop);
}
void ComputerInUseThread::StopThread() {
    this->shouldStop = true;
}