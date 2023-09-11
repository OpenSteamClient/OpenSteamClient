using OpenSteamworks.Client.CommonEventArgs;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Messaging;

namespace OpenSteamworks.Client.Login;

public class ChallengeUrlGeneratedEventArgs : EventArgs {
    public ChallengeUrlGeneratedEventArgs(string url) { URL = url; }
    public string URL { get; }
}

public class TokenGeneratedEventArgs : EventArgs {
    public TokenGeneratedEventArgs(string token, string accountName) { Token = token; AccountName = accountName; }
    public string Token { get; }
    public string AccountName { get; }
}

internal class LoginPoll {
    public delegate void ErrorEventHandler(object sender, EResultEventArgs e);
    public delegate void ChallengeUrlGeneratedHandler(object sender, ChallengeUrlGeneratedEventArgs e);
    public delegate void RefreshTokenGeneratedHandler(object sender, TokenGeneratedEventArgs e);
    public delegate void AccessTokenGeneratedHandler(object sender, TokenGeneratedEventArgs e);
    public event ErrorEventHandler? Error;
    public event ChallengeUrlGeneratedHandler? ChallengeUrlGenerated;
    public event RefreshTokenGeneratedHandler? RefreshTokenGenerated;
    public event AccessTokenGeneratedHandler? AccessTokenGenerated;

    private readonly ProtoMsg<CAuthentication_PollAuthSessionStatus_Request> pollMsg;
    private ClientMessaging clientMessaging;
    public bool IsPolling { get; private set; }
    public Task? PollThread { get; private set; }
    public float Interval { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientID">The ClientID to use</param>
    /// <param name="requestId">The RequestID to use</param>
    /// <param name="interval">Interval in seconds (5.1s)</param>
    internal LoginPoll(ClientMessaging clientMessaging, ulong clientID, Google.Protobuf.ByteString requestId, float interval) {
        this.clientMessaging = clientMessaging;
        pollMsg = new("Authentication.PollAuthSessionStatus#1", true);
        pollMsg.body.ClientId = clientID;
        pollMsg.body.RequestId = requestId;
        this.Interval = interval;
    }

    private async void PollMain() {
        using (var conn = this.clientMessaging.AllocateConnection())
        {
            while (IsPolling)
            {            
                ProtoMsg<CAuthentication_PollAuthSessionStatus_Response> pollResp = await conn.ProtobufSendMessageAndAwaitResponse<CAuthentication_PollAuthSessionStatus_Response, CAuthentication_PollAuthSessionStatus_Request>(pollMsg);
                
                if (pollResp.body.HasNewClientId) {
                    pollMsg.body.ClientId = pollResp.body.NewClientId;
                }

                if (pollResp.body.HasNewChallengeUrl) {
                    ChallengeUrlGenerated?.Invoke(this, new ChallengeUrlGeneratedEventArgs(pollResp.body.NewChallengeUrl));
                }

                if (pollResp.body.HasRefreshToken) {
                    IsPolling = false;
                    RefreshTokenGenerated?.Invoke(this, new TokenGeneratedEventArgs(pollResp.body.RefreshToken, pollResp.body.AccountName));
                }

                if (pollResp.header.Eresult != (int)EResult.k_EResultOK) {
                    IsPolling = false;
                    Error?.Invoke(this, new EResultEventArgs((EResult)pollResp.header.Eresult));
                }

                // The Interval we get is in seconds (in format 5.1s).
                System.Threading.Thread.Sleep((int)(Interval * 1000));
            }   
        }
    }

    public void StopPolling() {
        IsPolling = false;
    }
    public void StartPolling() {
        if (this.PollThread == null || this.PollThread.IsCompleted) {
            this.PollThread = new Task(this.PollMain);
        }

        IsPolling = true;
        this.PollThread.Start();
    }
}