#include "threadcontroller.h"

#include "../interop/callbackthread.h"
#include <algorithm>
#include <QApplication>
#include <QThread>
#include <QObject>
#include "../globals.h"
#include "../interop/appmanager.h"

ThreadController::ThreadController() {
    callbackThread = new CallbackThread();
    loginThread = new LoginThread();
    computerInUseThread = new ComputerInUseThread();
    downloadInfoThread = new DownloadInfoThread();

    ThreadController::initThread(callbackThread);
    ThreadController::initThread(computerInUseThread);
}

ThreadController::~ThreadController() {

}

void ThreadController::initThread(Thread *thread) 
{
    std::cout << "[ThreadController] Initialized thread " << thread->ThreadName() << std::endl;
    threads.push_back(thread);
}

void ThreadController::startThreads() {
    for (auto thread : threads) {
        std::cout << "[ThreadController] Starting " << thread->ThreadName() << std::endl;
        QMetaObject::invokeMethod(thread, &Thread::startThread, Qt::ConnectionType::QueuedConnection);
        std::cout << "[ThreadController] Started " << thread->ThreadName() << std::endl;
    }
}

void ThreadController::StartThread(Thread *thread) {
    QMetaObject::invokeMethod(thread, &Thread::startThread, Qt::ConnectionType::QueuedConnection);
}

void ThreadController::removeThread(Thread* thread) {
    // checks if the pointer exists in the list
    if (std::find(threads.begin(), threads.end(), thread) != threads.end())
    {
        threads.remove(thread);     
    }
}

void ThreadController::StopThreadsBlocking() {
    // We need to make a copy since removing elements affects the for loop
    std::vector<Thread *> threadsCopy;
    for (auto &&i : threads)
    {
        threadsCopy.push_back(i);
    }

    for (auto &&thread : threadsCopy)
    {
        StopThread(thread);
    }

    // Blocks until the threads have stopped
    while (threads.size() > 0) {
        std::this_thread::sleep_for(std::chrono::milliseconds(50));
    }
}

void ThreadController::StopThread(Thread* thread) {
    std::cout << "[ThreadController] Stopping " << thread->ThreadName() << std::endl;
    auto conn = std::make_shared<QMetaObject::Connection>();
    *conn = connect(thread, &Thread::threadExited, [this, thread, conn](int returnCode)
            { 
                std::cout << "[ThreadController] Stopped " << thread->ThreadName() << std::endl;
                this->removeThread(thread); 
                disconnect(*conn); 
            });
    QMetaObject::invokeMethod(thread, &Thread::stopThread);
}

void ThreadController::StartJob(Job *job) {
    std::cout << "[ThreadController/Jobs] Starting " << job->ThreadName() << std::endl;
    QMetaObject::invokeMethod(job, &Thread::startThread, Qt::ConnectionType::QueuedConnection);
    std::cout << "[ThreadController/Jobs] Started " << job->ThreadName() << std::endl;
}