#include <QObject>
#include <QThread>
#include <string>
#include <thread>

#pragma once

// A thread. This can be used for long running tasks that send signals periodically
class Thread : public QThread
{
    Q_OBJECT
private:
    void StopThreadInternal();
public:
    // This function returns the name of the thread to use. Doesn't need to be unique.
    virtual std::string ThreadName() = 0;

    // The main function that will run in another thread. Override this
    virtual void ThreadMain() = 0;

    // This function will be called before Thread::StopThreadInternal
    // Do not call this function from outside the thread, use the stop/killThread slots instead.
    virtual void StopThread() = 0;

    void run() override;

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