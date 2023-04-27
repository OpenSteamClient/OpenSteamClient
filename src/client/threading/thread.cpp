#include "thread.h"
#include <iostream>

Thread::Thread()
{
    backingQThread = new QThread();
    QThread::connect(backingQThread, &QThread::finished, this, &QObject::deleteLater);
    QThread::connect(backingQThread, &QThread::finished, this, &Thread::stoppedBackingThread);
    // connect(this, &Worker::resultReady, this, &Controller::handleResults);
}


Thread::~Thread()
{
    // It's probably safe to delete at this point
    delete backingQThread;
}

void Thread::stoppedBackingThread() {
    emit threadExited(0);
}
void Thread::StopThreadInternal() {
    backingQThread->exit();
    emit threadExited(0);
}

void Thread::startThread() {
    std::cout << "StartThread called" << std::endl;
    this->moveToThread(backingQThread);
    backingQThread->setObjectName(this->ThreadName());
    backingQThread->start();
    QMetaObject::invokeMethod(this, &Thread::ThreadMain, Qt::ConnectionType::QueuedConnection);
}

void Thread::ThreadMain() {
    std::cout << "ThreadMain called in Thread class, not your own subclass! Function not overriden!" << std::endl;
}

void Thread::stopThread() {
    StopThread();
    StopThreadInternal();
}
void Thread::killThread() {
    // kill logic here
    backingQThread->terminate();
    emit threadExited(-1);
}