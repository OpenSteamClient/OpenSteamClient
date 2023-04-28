#include <QThread>
#include <QObject>
#include <list>
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
    std::list<Thread*> threads;
    void StopThread(Thread *thread);

public:
    CallbackThread *callbackThread;
    LoginThread *loginThread;
    ComputerInUseThread *computerInUseThread;
    DownloadInfoThread *downloadInfoThread;

    void initThread(Thread *thread);
    void removeThread(Thread *thread);
    void StartThread(Thread *thread);
    void StopThreadsBlocking();
    ThreadController();
    ~ThreadController();
public slots:
    void StartJob(Job *job);
    void startThreads();
signals:
};
