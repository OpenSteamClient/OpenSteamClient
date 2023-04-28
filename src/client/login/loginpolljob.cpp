#include "loginpolljob.h"
#include "../globals.h"
#include "../threading/threadcontroller.h"
#include "../messaging/protomsg.hpp"
#include <steammessages_auth.steamclient.pb.h>

void JobLoginPolling::StopPolling() {
    pollingInterval = 0;
}

void JobLoginPolling::JobMain() {
    // Wait until we have an interval, a sign to start polling
    while (pollingInterval == 0) {
        std::this_thread::sleep_for(std::chrono::milliseconds(50));
    }

    // Run this until we get a satisfactory result or an error
    while (pollingInterval != 0) {
        ProtoMsg<CAuthentication_PollAuthSessionStatus_Request> *pollMsg = new ProtoMsg<CAuthentication_PollAuthSessionStatus_Request>(true);
        pollMsg->SetJobName("Authentication.PollAuthSessionStatus#1");
        
        pollMsg->body.set_client_id(client_id);
        pollMsg->body.set_request_id(request_id);

        ProtoMsg<CAuthentication_PollAuthSessionStatus_Response> pollResp = pollMsg->SendMessageAndAwaitResponse<CAuthentication_PollAuthSessionStatus_Response>();
        if (pollResp.success) {
            // New QR codes get delivered this way
            if (pollResp.body.has_new_challenge_url()) {
                emit NewChallengeUrl(pollResp.body.new_challenge_url());
            }

            if (pollResp.body.has_new_client_id()) {
                client_id = pollResp.body.new_client_id();
            }

            if (pollResp.body.has_had_remote_interaction()) {
                if (pollResp.body.has_refresh_token()) {
                    emit OnTokenAvailable(pollResp.body.account_name(), pollResp.body.refresh_token());
                    pollingInterval = 0;
                    std::cout << "[LoginPollJob] Token received, shutting down polling" << std::endl;
                    return;
                }
              
            }
            
        } else {
            std::string errStr = "";
            
            switch (pollResp.header.eresult())
            {
            case k_EResultFileNotFound:
                errStr = "User declined the request";
                break;
            case k_EResultExpired:
                errStr = "Challenge expired";
                break;

            default:
                errStr = "Polling failed";
                break;
            }
            emit OnError(errStr, (EResult)pollResp.header.eresult());
            pollingInterval = 0;
            return;
        }
        std::this_thread::sleep_for(std::chrono::seconds(pollingInterval));
    }
}

std::string JobLoginPolling::JobName() {
    return "LoginPolling";
}

JobLoginPolling::JobLoginPolling() {

}

JobLoginPolling::~JobLoginPolling() {

}

void JobLoginPolling::StartPolling(int intervalSec, uint64_t clientId, std::string requestId) {
    Global_ThreadController->StartJob(this);
    pollingInterval = intervalSec;
    client_id = clientId;
    request_id = requestId;
}