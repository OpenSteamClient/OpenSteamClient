#include "thread.h"
#include <iostream>

Thread::Thread()
{
    QThread::connect(this, &QThread::finished, this, &Thread::stoppedBackingThread);
    // connect(this, &Worker::resultReady, this, &Controller::handleResults);
}

Thread::~Thread()
{

}

void Thread::stoppedBackingThread() {
    emit threadExited(0);
}

void Thread::StopThreadInternal() {
    emit threadExited(0);
    this->deleteLater();
}

void Thread::run() {
    ThreadMain();
}

void Thread::startThread() {
    std::cout << "[Thread] StartThread called" << std::endl;

    this->setObjectName(this->ThreadName());
    this->start();
}

std::string Thread::ThreadName() {
    std::cout << "[Thread] Programmer error! ThreadName called in Thread class, not your own subclass! Function not overridden!" << std::endl;
    return "UnnamedThread";
}

void Thread::ThreadMain() {
    std::cout << "[Thread] Programmer error! ThreadMain called in Thread class, not your own subclass! Function not overridden!" << std::endl;
}

void Thread::StopThread() {
    std::cout << "[Thread] Programmer error! StopThread called in Thread class, not your own subclass! Function not overridden!" << std::endl;
}

void Thread::stopThread() {
    StopThread();
    StopThreadInternal();
}
void Thread::killThread() {
    // kill logic here

    // Note: this signal is not guaranteed to have a valid sender object
    emit threadExited(-1);

    this->terminate();
}