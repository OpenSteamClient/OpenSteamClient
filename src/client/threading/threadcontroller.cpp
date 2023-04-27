#include "threadcontroller.h"

#include "../interop/callbackthread.h"
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
    ThreadController::initThread(loginThread);
    ThreadController::initThread(computerInUseThread);
}

ThreadController::~ThreadController() {

}
void ThreadController::initThread(Thread *thread) 
{
    std::cout << "Initialized thread " << thread->ThreadName() << std::endl;
    threads.push_back(thread);
}

void ThreadController::startThreads() {
    for (auto thread : threads) {
        std::cout << "Starting " << thread->ThreadName() << std::endl;
        QMetaObject::invokeMethod(thread, &Thread::startThread, Qt::ConnectionType::QueuedConnection);
        std::cout << "Started " << thread->ThreadName() << std::endl;
    }
}

void ThreadController::StartThread(Thread *thread) {
    QMetaObject::invokeMethod(thread, &Thread::startThread, Qt::ConnectionType::QueuedConnection);
}

void ThreadController::stopThreads() {
    for (auto thread : threads) {
        std::cout << "Stopping " << thread->ThreadName() << std::endl;
        QMetaObject::invokeMethod(thread, &Thread::stopThread, Qt::ConnectionType::BlockingQueuedConnection);
    }
}
void ThreadController::StartJob(Job *job) {
    std::cout << "Starting " << job->ThreadName() << std::endl;
    QMetaObject::invokeMethod(job, &Thread::startThread, Qt::ConnectionType::QueuedConnection);
    std::cout << "Started " << job->ThreadName() << std::endl;
}