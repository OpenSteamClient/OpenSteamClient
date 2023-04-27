#include <QThread>
#include <QObject>
#include <vector>
#include <string>
#include "thread.h"
#include "job.h"
#include "../globals.h"
#include "../interop/callbackthread.h"
#include "../login/loginthread.h"
#include "../interop/computerinusethread.h"
#include "../gui/downloads/downloadinfothread.h"

#pragma once

class ThreadController : public QObject
{
    Q_OBJECT
private:
    std::vector<Thread*> threads;
public:
    CallbackThread *callbackThread;
    LoginThread *loginThread;
    ComputerInUseThread *computerInUseThread;
    DownloadInfoThread *downloadInfoThread;

    void initThread(Thread *thread);
    void StartThread(Thread *thread);
    ThreadController();
    ~ThreadController();
public slots:
    void StartJob(Job *job);
    void startThreads();
    void stopThreads();
signals:
};
