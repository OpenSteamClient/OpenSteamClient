#include "../threading/job.h"
#include "../ext/steamclient.h"

#pragma once

enum PollerType
{
    k_EPollerTypeQR,
    K_EPollerTypeCredentials
};

class JobLoginPolling : public Job {
    Q_OBJECT
private:
    int pollingInterval = 0;

public:
    uint64_t client_id = 0;
    std::string request_id = "";
    PollerType type;

    std::string JobName();
    void JobMain();
    JobLoginPolling();
    ~JobLoginPolling();
public slots:
    void StartPolling(int intervalSec, uint64_t clientId, std::string requestId);
    void StopPolling(); 
signals:
    void OnTokenAvailable(std::string username, std::string token);
    void OnError(std::string, EResult eResult);
    void NewChallengeUrl(std::string);
};