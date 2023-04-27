#include <thread>
#include <algorithm>
#include <map>
#include <QObject>
#include "includesteamworks.h"
#include "../globals.h"
#include "../threading/thread.h"

#pragma once

class ComputerInUseThread : public Thread
{
    Q_OBJECT
private:
    bool shouldStop = false;

public:
    std::string ThreadName();
    void ThreadMain();
    void StopThread();
    ComputerInUseThread() {}
    ~ComputerInUseThread() {}
};