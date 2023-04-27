#include <QObject>
#include <QThread>
#include <string>
#include <thread>

#pragma once

// A wrapper for a QThread/QObject combo
class Thread : public QObject
{
    Q_OBJECT
private:
    void StopThreadInternal();
public:
    // The QThread this object will be moved to
    QThread *backingQThread;

    // This function returns the name of the thread to use. Doesn't need to be unique.
    virtual std::string ThreadName() = 0;

    // The main function that will run in another thread. Override this
    virtual void ThreadMain() = 0;

    // This function will be called before Thread::StopThreadInternal
    // Do not call this function from outside the thread, use the stop/killThread slots instead.
    virtual void StopThread() = 0;

    Thread();
    ~Thread();
public slots:
    void startThread();
    void stopThread();
    void killThread();
private slots:
    void stoppedBackingThread();
signals:
    // Called when the thread exits by itself or if it's killed or requested to stop
    void threadExited(int returnCode);
};