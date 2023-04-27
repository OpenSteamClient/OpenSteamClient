#include <QObject>
#include "thread.h"

#pragma once

// A job is a thread that runs temporarily, until a result/error is received. 
// These are meant to be instantiated inline, and return values retrieved with Qt's signals and slots.
class Job : public Thread {
    Q_OBJECT
public:
    virtual std::string JobName() = 0;
    virtual void JobMain() = 0;
    
    std::string ThreadName()
    {
        return "Job"+JobName();
    }
    void ThreadMain() {
        JobMain();
    }
    void StopThread() {

    }
    Job();
    ~Job();
};