#include "computerinusethread.h"
#include "../ext/steamclient.h"

std::string ComputerInUseThread::ThreadName() {
    return "ComputerInUse";
}

void ComputerInUseThread::ThreadMain() {
    std::cout << "ComputerInUseThread started" << std::endl;
    do
    {
        Global_SteamClientMgr->ClientUser->SetComputerInUse();
        std::this_thread::sleep_for(std::chrono::seconds(5));
    } while (!shouldStop);
}
void ComputerInUseThread::StopThread() {
    this->shouldStop = true;
}