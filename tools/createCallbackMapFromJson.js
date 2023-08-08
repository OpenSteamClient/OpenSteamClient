var callbacks = [
    {
        "id": 101,
        "name": "23SteamServersConnected_t",
        "size": 1,
        "posted_at": ["0x32542a"]
    },
    {
        "id": 102,
        "name": "27SteamServerConnectFailure_t",
        "size": 8,
        "posted_at": ["0x32062c","0x320cfb","0x322e79","0x32327e","0x932bb8"]
    },
    {
        "id": 103,
        "name": "26SteamServersDisconnected_t",
        "size": 4,
        "posted_at": ["0x320478"]
    },
    {
        "id": 113,
        "name": "22ClientGameServerDeny_t",
        "size": 16,
        "posted_at": ["0x32b1a6"]
    },
    {
        "id": 115,
        "name": "18GSPolicyResponse_t",
        "size": 1,
        "posted_at": ["0x32b26c"]
    },
    {
        "id": 125,
        "name": "17LicensesUpdated_t",
        "size": 1,
        "posted_at": ["0x1e0a11","0x93e502"]
    },
    {
        "id": 143,
        "name": "28ValidateAuthTicketResponse_t",
        "size": 20,
        "posted_at": ["0x258e93"]
    },
    {
        "id": 164,
        "name": "Unknown",
        "size": 256,
        "posted_at": ["0x8fd395","0x90384b"]
    },
    {
        "id": 167,
        "name": "Unknown",
        "size": 32,
        "posted_at": ["0x924cbb","0x924cf9"]
    },
    {
        "id": 201,
        "name": "17GSClientApprove_t",
        "size": 16,
        "posted_at": ["0x4ab958"]
    },
    {
        "id": 202,
        "name": "14GSClientDeny_t",
        "size": 140,
        "posted_at": ["0x4abe4e","0x4ac850","0x4b0b3d","0x4b1863"]
    },
    {
        "id": 203,
        "name": "14GSClientKick_t",
        "size": 12,
        "posted_at": ["0x4abc3a","0x4af304"]
    },
    {
        "id": 204,
        "name": "20GSClientSteam2Deny_t",
        "size": 8,
        "posted_at": ["0x4b0f90","0x4b135d","0x4b156e"]
    },
    {
        "id": 205,
        "name": "22GSClientSteam2Accept_t",
        "size": 12,
        "posted_at": ["0x4b0f4d","0x4b130e","0x4b1523"]
    },
    {
        "id": 206,
        "name": "27GSClientAchievementStatus_t",
        "size": 140,
        "posted_at": ["0x4b4f7f"]
    },
    {
        "id": 207,
        "name": "17GSGameplayStats_t",
        "size": 16,
        "posted_at": ["0x4b374c"]
    },
    {
        "id": 208,
        "name": "21GSClientGroupStatus_t",
        "size": 18,
        "posted_at": ["0x4b341e"]
    },
    {
        "id": 211,
        "name": "37ComputeNewPlayerCompatibilityResult_t",
        "size": 24,
        "posted_at": ["0x4ab7b6"]
    },
    {
        "id": 304,
        "name": "20PersonaStateChange_t",
        "size": 12,
        "posted_at": ["0x9617a5","0x974530","0x98279a","0x95d25f","0x984362"]
    },
    {
        "id": 331,
        "name": "22GameOverlayActivated_t",
        "size": 8,
        "posted_at": ["0x954f86"]
    },
    {
        "id": 333,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x95438e"]
    },
    {
        "id": 334,
        "name": "19AvatarImageLoaded_t",
        "size": 20,
        "posted_at": ["0x97bba6","0x97be3d","0x97bf00"]
    },
    {
        "id": 336,
        "name": "26FriendRichPresenceUpdate_t",
        "size": 12,
        "posted_at": ["0x266ee9","0x95eba4","0x964182","0x266f27","0x9641df"]
    },
    {
        "id": 337,
        "name": "Unknown",
        "size": 264,
        "posted_at": ["0x954418"]
    },
    {
        "id": 338,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x4c2dc2","0x4c2df6"]
    },
    {
        "id": 339,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x4c2f06","0x4c2ff6"]
    },
    {
        "id": 340,
        "name": "Unknown",
        "size": 18,
        "posted_at": ["0x4c2fa4"]
    },
    {
        "id": 348,
        "name": "27UnreadChatMessagesChanged_t",
        "size": 1,
        "posted_at": ["0x9547a1"]
    },
    {
        "id": 349,
        "name": "Unknown",
        "size": 1024,
        "posted_at": ["0x963160"]
    },
    {
        "id": 350,
        "name": "29EquippedProfileItemsChanged_t",
        "size": 8,
        "posted_at": ["0x964afa"]
    },
    {
        "id": 351,
        "name": "22EquippedProfileItems_t",
        "size": 20,
        "posted_at": ["0x981c10"]
    },
    {
        "id": 501,
        "name": "25FavoritesListChangedOld_t",
        "size": 1,
        "posted_at": ["0x90479f","0x905467","0x93fa69"]
    },
    {
        "id": 502,
        "name": "22FavoritesListChanged_t",
        "size": 28,
        "posted_at": ["0x904800","0x9054cd","0x93faab"]
    },
    {
        "id": 503,
        "name": "13LobbyInvite_t",
        "size": 24,
        "posted_at": ["0x264b9f"]
    },
    {
        "id": 504,
        "name": "12LobbyEnter_t",
        "size": 20,
        "posted_at": ["0x996a64","0x996b9b","0x996c9b","0x999df1"]
    },
    {
        "id": 505,
        "name": "17LobbyDataUpdate_t",
        "size": 20,
        "posted_at": ["0x991404","0x996fba","0x997237","0x996c02","0x997592","0x999e6c"]
    },
    {
        "id": 506,
        "name": "17LobbyChatUpdate_t",
        "size": 28,
        "posted_at": ["0x9794bf","0x9919e1","0x991ccd","0x998434"]
    },
    {
        "id": 507,
        "name": "14LobbyChatMsg_t",
        "size": 24,
        "posted_at": ["0x9900c1","0x97a2cf"]
    },
    {
        "id": 509,
        "name": "Unknown",
        "size": 24,
        "posted_at": ["0x98c0da"]
    },
    {
        "id": 510,
        "name": "16LobbyMatchList_t",
        "size": 4,
        "posted_at": ["0x99919e","0x9996d0","0x99977e"]
    },
    {
        "id": 512,
        "name": "13LobbyKicked_t",
        "size": 20,
        "posted_at": ["0x979654"]
    },
    {
        "id": 513,
        "name": "14LobbyCreated_t",
        "size": 12,
        "posted_at": ["0x999ce2","0x999f0c","0x99a047"]
    },
    {
        "id": 701,
        "name": "11IPCountry_t",
        "size": 1,
        "posted_at": ["0x276fc5"]
    },
    {
        "id": 702,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8946a9"]
    },
    {
        "id": 736,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x277fa2"]
    },
    {
        "id": 738,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x891461"]
    },
    {
        "id": 739,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x88eed9"]
    },
    {
        "id": 1014,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x1ebbe5"]
    },
    {
        "id": 1030,
        "name": "18TimedTrialStatus_t",
        "size": 16,
        "posted_at": ["0x2cf2a6","0x2cf597","0x2cf8e8"]
    },
    {
        "id": 1101,
        "name": "19UserStatsReceived_t",
        "size": 20,
        "posted_at": ["0x9f8fe5","0xa02765"]
    },
    {
        "id": 1102,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x9eb512"]
    },
    {
        "id": 1103,
        "name": "23UserAchievementStored_t",
        "size": 148,
        "posted_at": ["0x9efac7","0x9efbea","0x9f0e85","0x9f0f4f","0x9f243b","0x9f60e1"]
    },
    {
        "id": 1108,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x9f3549"]
    },
    {
        "id": 1109,
        "name": "28UserAchievementIconFetched_t",
        "size": 144,
        "posted_at": ["0x9efb1d","0x9f268e"]
    },
    {
        "id": 1201,
        "name": "22SocketStatusCallback_t",
        "size": 20,
        "posted_at": ["0x2acb07"]
    },
    {
        "id": 1202,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x7e7f83"]
    },
    {
        "id": 1203,
        "name": "Unknown",
        "size": 9,
        "posted_at": ["0x7e3c0c"]
    },
    {
        "id": 1295,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x2af912"]
    },
    {
        "id": 1297,
        "name": "Unknown",
        "size": 148,
        "posted_at": ["0x7e3f85"]
    },
    {
        "id": 1309,
        "name": "32RemoteStoragePublishFileResult_t",
        "size": 16,
        "posted_at": ["0x9bf4c1"]
    },
    {
        "id": 1316,
        "name": "40RemoteStorageUpdatePublishedFileResult_t",
        "size": 16,
        "posted_at": ["0x9aa531"]
    },
    {
        "id": 1321,
        "name": "38RemoteStoragePublishedFileSubscribed_t",
        "size": 12,
        "posted_at": ["0x9bc518","0x9bc4f4"]
    },
    {
        "id": 1322,
        "name": "40RemoteStoragePublishedFileUnsubscribed_t",
        "size": 12,
        "posted_at": ["0x99f7fc","0x99f7de"]
    },
    {
        "id": 1323,
        "name": "35RemoteStoragePublishedFileDeleted_t",
        "size": 12,
        "posted_at": ["0x99f890","0x9a0209","0x99f872","0x9a01c0"]
    },
    {
        "id": 1329,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x99ed72","0x99ee23"]
    },
    {
        "id": 1330,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x2e59a5","0x2e5b4e"]
    },
    {
        "id": 1701,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x23538d"]
    },
    {
        "id": 1702,
        "name": "Unknown",
        "size": 39,
        "posted_at": ["0x235707"]
    },
    {
        "id": 1800,
        "name": "17GSStatsReceived_t",
        "size": 12,
        "posted_at": ["0xa02655"]
    },
    {
        "id": 2001,
        "name": "24GameStatsSessionIssued_t",
        "size": 16,
        "posted_at": ["0x239a9b"]
    },
    {
        "id": 2002,
        "name": "24GameStatsSessionClosed_t",
        "size": 12,
        "posted_at": ["0x23acff","0x23b1e3","0x23b4dd"]
    },
    {
        "id": 2102,
        "name": "28HTTPRequestHeadersReceived_t",
        "size": 12,
        "posted_at": ["0x23c52f","0x23c50b"]
    },
    {
        "id": 2103,
        "name": "25HTTPRequestDataReceived_t",
        "size": 20,
        "posted_at": ["0x23ebff","0x23ebda"]
    },
    {
        "id": 2301,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x9cdd64","0x9ced63"]
    },
    {
        "id": 2302,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x9c6aa9"]
    },
    {
        "id": 2501,
        "name": "38SteamUnifiedMessagesSendMethodResult_t",
        "size": 24,
        "posted_at": ["0x2f3458","0x8fa4a3"]
    },
    {
        "id": 2601,
        "name": "27BigPictureStreamingResult_t",
        "size": 8,
        "posted_at": ["0x9016e8","0x90458e"]
    },
    {
        "id": 2602,
        "name": "22StopStreamingRequest_t",
        "size": 1,
        "posted_at": ["0x9055cf","0x907162","0x910690"]
    },
    {
        "id": 2603,
        "name": "25BigPictureStreamingDone_t",
        "size": 1,
        "posted_at": ["0x90f19c"]
    },
    {
        "id": 2604,
        "name": "28BigPictureStreamRestarting_t",
        "size": 1,
        "posted_at": ["0x90177c","0x903d22"]
    },
    {
        "id": 2801,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x35fb38","0x374acd"]
    },
    {
        "id": 2802,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x36b6c9"]
    },
    {
        "id": 3405,
        "name": "15ItemInstalled_t",
        "size": 12,
        "posted_at": ["0x2e5dd1"]
    },
    {
        "id": 3418,
        "name": "32UserSubscribedItemsListChanged_t",
        "size": 4,
        "posted_at": ["0x8ba737"]
    },
    {
        "id": 3901,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x1fa7df"]
    },
    {
        "id": 3902,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x1e99c9","0x22d417"]
    },
    {
        "id": 4001,
        "name": "PlaybackStatusHasChanged_t",
        "size": 1,
        "posted_at": ["0x7a1ebc","0x7c49e3"]
    },
    {
        "id": 4002,
        "name": "VolumeHasChanged_t",
        "size": 4,
        "posted_at": ["0x7c5a07","0x7c5c9c","0x7c5dec"]
    },
    {
        "id": 4105,
        "name": "MusicPlayerWantsPlay_t",
        "size": 1,
        "posted_at": ["0x7a0eb5"]
    },
    {
        "id": 4106,
        "name": "MusicPlayerWantsPause_t",
        "size": 1,
        "posted_at": ["0x7a0fd5"]
    },
    {
        "id": 4107,
        "name": "MusicPlayerWantsPlayPrevious_t",
        "size": 1,
        "posted_at": ["0x7a0aac"]
    },
    {
        "id": 4108,
        "name": "MusicPlayerWantsPlayNext_t",
        "size": 1,
        "posted_at": ["0x7a0b2c"]
    },
    {
        "id": 4604,
        "name": "BroadcastUploadStart_t",
        "size": 1,
        "posted_at": ["0x30592d"]
    },
    {
        "id": 4605,
        "name": "BroadcastUploadStop_t",
        "size": 4,
        "posted_at": ["0x2fbea9"]
    },
    {
        "id": 4611,
        "name": "GetVideoURLResult_t",
        "size": 264,
        "posted_at": ["0x303868","0x303aac"]
    },
    {
        "id": 4624,
        "name": "GetOPFSettingsResult_t",
        "size": 8,
        "posted_at": ["0x303ca2","0x303e7b"]
    },
    {
        "id": 4700,
        "name": "27SteamInventoryResultReady_t",
        "size": 8,
        "posted_at": ["0x24042b","0x24eabe","0x240960"]
    },
    {
        "id": 4701,
        "name": "26SteamInventoryFullUpdate_t",
        "size": 4,
        "posted_at": ["0x240a9d"]
    },
    {
        "id": 4702,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x24f3ed"]
    },
    {
        "id": 5001,
        "name": "30SteamParentalSettingsChanged_t",
        "size": 1,
        "posted_at": ["0x910aac","0x913563"]
    },
    {
        "id": 5201,
        "name": "31SearchForGameProgressCallback_t",
        "size": 36,
        "posted_at": ["0x99b62f","0x99b6b5"]
    },
    {
        "id": 5202,
        "name": "29SearchForGameResultCallback_t",
        "size": 32,
        "posted_at": ["0x99b591","0x99b79c"]
    },
    {
        "id": 5211,
        "name": "39RequestPlayersForGameProgressCallback_t",
        "size": 12,
        "posted_at": ["0x99aa30","0x99ac69"]
    },
    {
        "id": 5212,
        "name": "Unknown",
        "size": 56,
        "posted_at": ["0x99ab89"]
    },
    {
        "id": 5213,
        "name": "42RequestPlayersForGameFinalResultCallback_t",
        "size": 20,
        "posted_at": ["0x99ad5b"]
    },
    {
        "id": 5305,
        "name": "33AvailableBeaconLocationsUpdated_t",
        "size": 1,
        "posted_at": ["0x2c1db7"]
    },
    {
        "id": 5306,
        "name": "22ActiveBeaconsUpdated_t",
        "size": 1,
        "posted_at": ["0x2c3082"]
    },
    {
        "id": 5701,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x826a5f"]
    },
    {
        "id": 5702,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x824580"]
    },
    {
        "id": 5901,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x93113f","0x9311e2"]
    },
    {
        "id": 5902,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x930afe"]
    },
    {
        "id": 5903,
        "name": "Unknown",
        "size": 24,
        "posted_at": ["0x9308ca"]
    },
    {
        "id": 5904,
        "name": "Unknown",
        "size": 548,
        "posted_at": ["0x930dd7","0x90b2ac"]
    },
    {
        "id": 5905,
        "name": "Unknown",
        "size": 28,
        "posted_at": ["0x911b9e"]
    },
    {
        "id": 5906,
        "name": "Unknown",
        "size": 540,
        "posted_at": ["0x931351"]
    },
    {
        "id": 1010001,
        "name": "30GameOverlayActivateRequested_t",
        "size": 8476,
        "posted_at": ["0x954e79","0x956a9b","0x956ba4","0x956ca4","0x956db9","0x956ec2"]
    },
    {
        "id": 1010002,
        "name": "Unknown",
        "size": 284,
        "posted_at": ["0x2768fe","0x276a6d","0x985b54"]
    },
    {
        "id": 1010003,
        "name": "13FriendAdded_t",
        "size": 12,
        "posted_at": ["0x25a123"]
    },
    {
        "id": 1010004,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x97b2bb","0x97b7ef"]
    },
    {
        "id": 1010005,
        "name": "15FriendChatMsg_t",
        "size": 28,
        "posted_at": ["0x259ef0","0x968712"]
    },
    {
        "id": 1010007,
        "name": "16ChatRoomInvite_t",
        "size": 156,
        "posted_at": ["0x264d52"]
    },
    {
        "id": 1010008,
        "name": "15ChatRoomEnter_t",
        "size": 176,
        "posted_at": ["0x27f5eb","0x983e9b"]
    },
    {
        "id": 1010009,
        "name": "23ChatMemberStateChange_t",
        "size": 28,
        "posted_at": ["0x97919a"]
    },
    {
        "id": 1010010,
        "name": "13ChatRoomMsg_t",
        "size": 24,
        "posted_at": ["0x26ea8e","0x97a1fa"]
    },
    {
        "id": 1010011,
        "name": "18ChatRoomDlgClose_t",
        "size": 8,
        "posted_at": ["0x974bd7"]
    },
    {
        "id": 1010012,
        "name": "17ChatRoomClosing_t",
        "size": 8,
        "posted_at": ["0x272e18"]
    },
    {
        "id": 1010013,
        "name": "17ChatRoomKicking_t",
        "size": 16,
        "posted_at": ["0x9795d9"]
    },
    {
        "id": 1010014,
        "name": "17ChatRoomBanning_t",
        "size": 16,
        "posted_at": ["0x979251"]
    },
    {
        "id": 1010015,
        "name": "16ChatRoomCreate_t",
        "size": 20,
        "posted_at": ["0x26e40a","0x967a12"]
    },
    {
        "id": 1010016,
        "name": "16OpenChatDialog_t",
        "size": 12,
        "posted_at": ["0x954b1d","0x957e52","0x97987d"]
    },
    {
        "id": 1010017,
        "name": "22ChatRoomActionResult_t",
        "size": 28,
        "posted_at": ["0x26e634"]
    },
    {
        "id": 1010018,
        "name": "23ChatRoomDlgSerialized_t",
        "size": 8,
        "posted_at": ["0x95d49a"]
    },
    {
        "id": 1010019,
        "name": "17ClanInfoChanged_t",
        "size": 12,
        "posted_at": ["0x26691f","0x25ce6c","0x25cfc5","0x276aee","0x97bac6","0x97bb29"]
    },
    {
        "id": 1010020,
        "name": "23ChatMemberInfoChanged_t",
        "size": 20,
        "posted_at": ["0x960bf4"]
    },
    {
        "id": 1010021,
        "name": "21ChatRoomInfoChanged_t",
        "size": 20,
        "posted_at": ["0x272d7a"]
    },
    {
        "id": 1010023,
        "name": "22ChatRoomSpeakChanged_t",
        "size": 20,
        "posted_at": ["0x961400","0x979376"]
    },
    {
        "id": 1010024,
        "name": "20NotifyIncomingCall_t",
        "size": 24,
        "posted_at": ["0x954c0e","0x95601e","0x95728e"]
    },
    {
        "id": 1010025,
        "name": "14NotifyHangup_t",
        "size": 8,
        "posted_at": ["0x954c7e","0x955dde"]
    },
    {
        "id": 1010026,
        "name": "21NotifyRequestResume_t",
        "size": 4,
        "posted_at": ["0x954ce6","0x955d66"]
    },
    {
        "id": 1010027,
        "name": "33NotifyChatRoomVoiceStateChanged_t",
        "size": 16,
        "posted_at": ["0x954d66","0x956096"]
    },
    {
        "id": 1010028,
        "name": "21ChatRoomDlgUIChange_t",
        "size": 12,
        "posted_at": ["0x95489a"]
    },
    {
        "id": 1010029,
        "name": "20VoiceCallInitiated_t",
        "size": 20,
        "posted_at": ["0x955bb8","0x9587ac","0x96109c"]
    },
    {
        "id": 1010030,
        "name": "15FriendIgnored_t",
        "size": 24,
        "posted_at": ["0x26db26"]
    },
    {
        "id": 1010031,
        "name": "25VoiceInputDeviceChanged_t",
        "size": 1,
        "posted_at": ["0x954a29"]
    },
    {
        "id": 1010032,
        "name": "26VoiceEnabledStateChanged_t",
        "size": 1,
        "posted_at": ["0x96223f"]
    },
    {
        "id": 1010033,
        "name": "26FriendsWhoPlayGameUpdate_t",
        "size": 8,
        "posted_at": ["0x96bda0","0x96bf26","0x96df03"]
    },
    {
        "id": 1010035,
        "name": "20RichInviteReceived_t",
        "size": 268,
        "posted_at": ["0x9557ef"]
    },
    {
        "id": 1010036,
        "name": "19FriendsMenuChange_t",
        "size": 4,
        "posted_at": ["0x95473e"]
    },
    {
        "id": 1010039,
        "name": "19TradeStartSession_t",
        "size": 8,
        "posted_at": ["0x955224"]
    },
    {
        "id": 1010040,
        "name": "21TradeInviteCanceled_t",
        "size": 16,
        "posted_at": ["0x955120"]
    },
    {
        "id": 1010041,
        "name": "16GameUsingVoice_t",
        "size": 1,
        "posted_at": ["0x96150e"]
    },
    {
        "id": 1010042,
        "name": "21FriendsGroupCreated_t",
        "size": 8,
        "posted_at": ["0x265288"]
    },
    {
        "id": 1010043,
        "name": "21FriendsGroupDeleted_t",
        "size": 8,
        "posted_at": ["0x258225"]
    },
    {
        "id": 1010044,
        "name": "21FriendsGroupRenamed_t",
        "size": 4,
        "posted_at": ["0x265600"]
    },
    {
        "id": 1010045,
        "name": "25FriendsGroupMemberAdded_t",
        "size": 4,
        "posted_at": ["0x2584ad"]
    },
    {
        "id": 1010046,
        "name": "27FriendsGroupMemberRemoved_t",
        "size": 4,
        "posted_at": ["0x25875d"]
    },
    {
        "id": 1010048,
        "name": "23PlayerNicknameChanged_t",
        "size": 4,
        "posted_at": ["0x265978"]
    },
    {
        "id": 1010049,
        "name": "21EmoticonListChanged_t",
        "size": 1,
        "posted_at": ["0x977d9e"]
    },
    {
        "id": 1010050,
        "name": "25OpenChatRoomGroupDialog_t",
        "size": 16,
        "posted_at": ["0x95490e","0x957f28"]
    },
    {
        "id": 1010051,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x889a9b"]
    },
    {
        "id": 1010052,
        "name": "25ShowChatRoomGroupInvite_t",
        "size": 32,
        "posted_at": ["0x954989"]
    },
    {
        "id": 1010053,
        "name": "25OpenInviteToTradeDialog_t",
        "size": 8,
        "posted_at": ["0x954b8a"]
    },
    {
        "id": 1010054,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x889b4b"]
    },
    {
        "id": 1010055,
        "name": "31VoiceMutingHotKeyStateChanged_t",
        "size": 1,
        "posted_at": ["0x96229b"]
    },
    {
        "id": 1010056,
        "name": "19OpenFriendsDialog_t",
        "size": 2,
        "posted_at": ["0x954a9e","0x957d9c"]
    },
    {
        "id": 1010057,
        "name": "25UserFriendRequestRollup_t",
        "size": 4,
        "posted_at": ["0x97b76c"]
    },
    {
        "id": 1010058,
        "name": "36OverlayBrowserProtocolRegistration_t",
        "size": 40,
        "posted_at": ["0x958690","0x970f8a"]
    },
    {
        "id": 1010059,
        "name": "23CommunityItemDownload_t",
        "size": 536,
        "posted_at": ["0x980ae3","0x980cc8"]
    },
    {
        "id": 1020001,
        "name": "10SystemIM_t",
        "size": 4108,
        "posted_at": ["0x271fa2"]
    },
    {
        "id": 1020004,
        "name": "17BeginLogonRetry_t",
        "size": 1,
        "posted_at": ["0x313cbb"]
    },
    {
        "id": 1020009,
        "name": "20UninstallClientApp_t",
        "size": 4,
        "posted_at": ["0x257c8e"]
    },
    {
        "id": 1020011,
        "name": "25ClientAppNewsItemUpdate_t",
        "size": 12,
        "posted_at": ["0x272734"]
    },
    {
        "id": 1020012,
        "name": "27ClientSteamNewsItemUpdate_t",
        "size": 32,
        "posted_at": ["0x272940"]
    },
    {
        "id": 1020013,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x272839"]
    },
    {
        "id": 1020014,
        "name": "23LegacyCDKeyRegistered_t",
        "size": 520,
        "posted_at": ["0x32a910"]
    },
    {
        "id": 1020015,
        "name": "27AccountInformationUpdated_t",
        "size": 1,
        "posted_at": ["0x276ddd","0x2770c5"]
    },
    {
        "id": 1020017,
        "name": "16GuestPassAcked_t",
        "size": 20,
        "posted_at": ["0x26dd05","0x9013c8","0x908166"]
    },
    {
        "id": 1020018,
        "name": "19GuestPassRedeemed_t",
        "size": 12,
        "posted_at": ["0x257b37","0x902980","0x907c6e"]
    },
    {
        "id": 1020019,
        "name": "19UpdateGuestPasses_t",
        "size": 12,
        "posted_at": ["0x2718e4"]
    },
    {
        "id": 1020020,
        "name": "25LogOnCredentialsChanged_t",
        "size": 1,
        "posted_at": ["0x94e1f2"]
    },
    {
        "id": 1020024,
        "name": "16DRMDataRequest_t",
        "size": 12,
        "posted_at": ["0x29a472"]
    },
    {
        "id": 1020025,
        "name": "17DRMDataResponse_t",
        "size": 8,
        "posted_at": ["0x233630"]
    },
    {
        "id": 1020028,
        "name": "28AppOwnershipTicketReceived_t",
        "size": 4,
        "posted_at": ["0x2592cc"]
    },
    {
        "id": 1020030,
        "name": "19AppLifetimeNotice_t",
        "size": 16,
        "posted_at": ["0x93cc1f","0x947eb3","0x9483ea"]
    },
    {
        "id": 1020037,
        "name": "30ClientMarketingMessageUpdate_t",
        "size": 1,
        "posted_at": ["0x2723a7"]
    },
    {
        "id": 1020038,
        "name": "23ValidateEmailResponse_t",
        "size": 4,
        "posted_at": ["0x26f497"]
    },
    {
        "id": 1020040,
        "name": "24VerifyPasswordResponse_t",
        "size": 4,
        "posted_at": ["0x26e015"]
    },
    {
        "id": 1020043,
        "name": "29MicroTxnAuthRequestCallback_t",
        "size": 16,
        "posted_at": ["0x27fe28"]
    },
    {
        "id": 1020044,
        "name": "29MicroTxnAuthDismissCallback_t",
        "size": 8,
        "posted_at": ["0x93efa1"]
    },
    {
        "id": 1020046,
        "name": "28AppMinutesPlayedDataNotice_t",
        "size": 4,
        "posted_at": ["0x9339ef","0x93c9fb"]
    },
    {
        "id": 1020049,
        "name": "30EnableMachineLockingResponse_t",
        "size": 4,
        "posted_at": ["0x27e386"]
    },
    {
        "id": 1020055,
        "name": "25LoginInformationChanged_t",
        "size": 75,
        "posted_at": ["0x94e546","0x94eb56","0x94edaf"]
    },
    {
        "id": 1020058,
        "name": "24UpdateItemAnnouncement_t",
        "size": 8,
        "posted_at": ["0x257e7b"]
    },
    {
        "id": 1020060,
        "name": "27UpdateCommentNotification_t",
        "size": 12,
        "posted_at": ["0x257f6c"]
    },
    {
        "id": 1020061,
        "name": "29FriendPublishedStatusUpdate_t",
        "size": 524,
        "posted_at": ["0x25959e"]
    },
    {
        "id": 1020062,
        "name": "34UpdateOfflineMessageNotification_t",
        "size": 4,
        "posted_at": ["0x258062"]
    },
    {
        "id": 1020063,
        "name": "29FriendMessageHistoryChatLog_t",
        "size": 20,
        "posted_at": ["0x25a940","0x95dcd7"]
    },
    {
        "id": 1020065,
        "name": "30VanityURLChangedNotification_t",
        "size": 256,
        "posted_at": ["0x25940d","0x325b49"]
    },
    {
        "id": 1020066,
        "name": "30GetSteamGuardDetailsResponse_t",
        "size": 12,
        "posted_at": ["0x27b6fe"]
    },
    {
        "id": 1020068,
        "name": "StartStreamingUI_t",
        "size": 1,
        "posted_at": ["0x9107b1"]
    },
    {
        "id": 1020069,
        "name": "StopStreamingUI_t",
        "size": 1,
        "posted_at": ["0x910944","0x929a1e"]
    },
    {
        "id": 1020070,
        "name": "AppLastPlayedTimeChanged_t",
        "size": 8,
        "posted_at": ["0x933881","0x9e2ed4"]
    },
    {
        "id": 1020071,
        "name": "24UpdateUserNotification_t",
        "size": 8,
        "posted_at": ["0x25bd18"]
    },
    {
        "id": 1020073,
        "name": "28OfflineLogonTicketReceived_t",
        "size": 1,
        "posted_at": ["0x9370b8"]
    },
    {
        "id": 1020076,
        "name": "35NotifyAsyncNotificationsRequested_t",
        "size": 4,
        "posted_at": ["0x901804","0x903ebf"]
    },
    {
        "id": 1020077,
        "name": "PlayingSessionChanged_t",
        "size": 4,
        "posted_at": ["0x9043aa","0x90c612"]
    },
    {
        "id": 1020078,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x8a8b72","0x8a8fa8"]
    },
    {
        "id": 1020079,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x8a8d0f"]
    },
    {
        "id": 1020081,
        "name": "29GetTwoFactorDetailsResponse_t",
        "size": 8,
        "posted_at": ["0x27b515"]
    },
    {
        "id": 1020082,
        "name": "PrepareForSuspendProgress_t",
        "size": 4,
        "posted_at": ["0x27349d","0x273edf","0x274095","0x27418c"]
    },
    {
        "id": 1020083,
        "name": "ResumeSuspendedGamesProgress_t",
        "size": 8,
        "posted_at": ["0x27761f","0x277b94","0x27805c","0x278350","0x278614","0x278708"]
    },
    {
        "id": 1020084,
        "name": "SteamGuardFileCreated_t",
        "size": 1,
        "posted_at": ["0x27ec1d"]
    },
    {
        "id": 1020086,
        "name": "25SiteLicenseSeatCheckout_t",
        "size": 8,
        "posted_at": ["0x86b9c5","0x903410","0x923133"]
    },
    {
        "id": 1020087,
        "name": "16PostLogonState_t",
        "size": 10,
        "posted_at": ["0x2744f9","0x27457c","0x2747a0","0x2749d0","0x274b52","0x274de5","0x940c6c","0x940e0b"]
    },
    {
        "id": 1020088,
        "name": "24RequestAccountLinkInfo_t",
        "size": 16,
        "posted_at": ["0x931950"]
    },
    {
        "id": 1020089,
        "name": "22SSAAgreeStateChanged_t",
        "size": 8,
        "posted_at": ["0x90362f","0x907947","0x931bd7"]
    },
    {
        "id": 1020090,
        "name": "27SiteLicenseAvailableSeats_t",
        "size": 8,
        "posted_at": ["0x903100","0x92332d"]
    },
    {
        "id": 1020091,
        "name": "NewSteamAnnouncementStateChange_t",
        "size": 24,
        "posted_at": ["0x911421"]
    },
    {
        "id": 1020092,
        "name": "StartBigPictureForStreaming_t",
        "size": 1,
        "posted_at": ["0x910784"]
    },
    {
        "id": 1020093,
        "name": "StopBigPictureForStreaming_t",
        "size": 1,
        "posted_at": ["0x910968","0x929a42"]
    },
    {
        "id": 1020094,
        "name": "AppLicensesChanged_t",
        "size": 264,
        "posted_at": ["0x91f50a","0x91f806"]
    },
    {
        "id": 1020095,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8a2c64","0x8a3125"]
    },
    {
        "id": 1020098,
        "name": "ServiceRepairRequest_t",
        "size": 1,
        "posted_at": ["0x931e5e"]
    },
    {
        "id": 1020099,
        "name": "CommunityPreferencesUpdated_t",
        "size": 1,
        "posted_at": ["0x93a744"]
    },
    {
        "id": 1020102,
        "name": "SteamServersConnectedPreAuthentication_t",
        "size": 1,
        "posted_at": ["0x32880d"]
    },
    {
        "id": 1020103,
        "name": "OverlayState_t",
        "size": 12,
        "posted_at": ["0x8fcdc9","0x90376f"]
    },
    {
        "id": 1020104,
        "name": "OverlaySettingsChanged_t",
        "size": 1,
        "posted_at": ["0x8fce13","0x902db9"]
    },
    {
        "id": 1040003,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x8b1031","0x8b11a1"]
    },
    {
        "id": 1040011,
        "name": "25SteamConfigStoreChanged_t",
        "size": 260,
        "posted_at": ["0x345957","0x3494c5","0x349542"]
    },
    {
        "id": 1040013,
        "name": "Unknown",
        "size": 4248,
        "posted_at": ["0x893cb3"]
    },
    {
        "id": 1040015,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x8958b1"]
    },
    {
        "id": 1040017,
        "name": "ShutdownLauncher_t",
        "size": 2,
        "posted_at": ["0x87807d","0x88d4f6","0x88ddf9"]
    },
    {
        "id": 1040018,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x891d07","0x892e89"]
    },
    {
        "id": 1040022,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8a4929","0x8a4bc9","0x8a4ef2","0x8a5342","0x8a5bb2","0x8a6002"]
    },
    {
        "id": 1040023,
        "name": "Unknown",
        "size": 24584,
        "posted_at": ["0x892387"]
    },
    {
        "id": 1040024,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x892572"]
    },
    {
        "id": 1040025,
        "name": "Unknown",
        "size": 264,
        "posted_at": ["0x8927d3"]
    },
    {
        "id": 1040026,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x88d0e6","0x88e0c9","0x890209"]
    },
    {
        "id": 1040027,
        "name": "SetPersonaStateInternal_t",
        "size": 4,
        "posted_at": ["0x9839a9","0x984149"]
    },
    {
        "id": 1040028,
        "name": "Unknown",
        "size": 1032,
        "posted_at": ["0x88df52"]
    },
    {
        "id": 1040031,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x88d3fe","0x88d81e","0x88d941","0x88db19","0x88e1e9"]
    },
    {
        "id": 1040032,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x88d30e","0x88da29"]
    },
    {
        "id": 1040033,
        "name": "Unknown",
        "size": 4288,
        "posted_at": ["0x88e362"]
    },
    {
        "id": 1040034,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x88dd01"]
    },
    {
        "id": 1040035,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x88dc11"]
    },
    {
        "id": 1040037,
        "name": "Unknown",
        "size": 36,
        "posted_at": ["0x8929ea"]
    },
    {
        "id": 1040039,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x891639"]
    },
    {
        "id": 1040040,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x892cc9"]
    },
    {
        "id": 1040041,
        "name": "32ReceivedClientLogUploadRequest_t",
        "size": 8,
        "posted_at": ["0x270cc6"]
    },
    {
        "id": 1040042,
        "name": "33CompletedClientLogUploadRequest_t",
        "size": 8,
        "posted_at": ["0x275e9c"]
    },
    {
        "id": 1040043,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x88d6ae","0x897809"]
    },
    {
        "id": 1040044,
        "name": "Unknown",
        "size": 784,
        "posted_at": ["0x8955c9"]
    },
    {
        "id": 1040045,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x88d216","0x88e5bc"]
    },
    {
        "id": 1040046,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x892154","0x892214","0x893016","0x8930d6"]
    },
    {
        "id": 1050001,
        "name": "26ScreenshotUploadProgress_t",
        "size": 24,
        "posted_at": ["0x9c7a06","0x9c7ac1","0x9db832"]
    },
    {
        "id": 1050002,
        "name": "19ScreenshotWritten_t",
        "size": 24,
        "posted_at": ["0x9cddc1","0x9d6e52"]
    },
    {
        "id": 1050003,
        "name": "20ScreenshotUploaded_t",
        "size": 600,
        "posted_at": ["0x9db758"]
    },
    {
        "id": 1050005,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x9d37ab"]
    },
    {
        "id": 1050006,
        "name": "21ScreenshotTriggered_t",
        "size": 8,
        "posted_at": ["0x9c82cd","0x9c89c1","0x38e31d"]
    },
    {
        "id": 1070001,
        "name": "25ParentalLockChangeBegin_t",
        "size": 1,
        "posted_at": ["0x9109d2"]
    },
    {
        "id": 1070002,
        "name": "21ParentalLockChanged_t",
        "size": 2,
        "posted_at": ["0x910a28"]
    },
    {
        "id": 1070003,
        "name": "35ParentalLockChangeAttemptComplete_t",
        "size": 4,
        "posted_at": ["0x910a6e","0x912c2e"]
    },
    {
        "id": 1070004,
        "name": "25ParentalWebTokenChanged_t",
        "size": 4,
        "posted_at": ["0x910bfc","0x91306f"]
    },
    {
        "id": 1070005,
        "name": "21ParentalSetComplete_t",
        "size": 8,
        "posted_at": ["0x913525"]
    },
    {
        "id": 1080001,
        "name": "AuthorizeDeviceResult_t",
        "size": 88,
        "posted_at": ["0x230644"]
    },
    {
        "id": 1080002,
        "name": "DeauthorizeDeviceResult_t",
        "size": 80,
        "posted_at": ["0x2308cd"]
    },
    {
        "id": 1080003,
        "name": "Unknown",
        "size": 84,
        "posted_at": ["0x232a85"]
    },
    {
        "id": 1080004,
        "name": "Unknown",
        "size": 76,
        "posted_at": ["0x9426d6","0x942858","0x942a29"]
    },
    {
        "id": 1080007,
        "name": "DeviceAuthChanged_t",
        "size": 1,
        "posted_at": ["0x2306bc","0x23094a","0x2311d7","0x231f1c","0x23263c"]
    },
    {
        "id": 1090007,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8ab399"]
    },
    {
        "id": 1100000,
        "name": "LocalLibraryRemoveAlbumFromCrawling_t",
        "size": 4,
        "posted_at": ["0x7a4515"]
    },
    {
        "id": 1100001,
        "name": "LocalLibraryUpdateAlbumFromCrawling_t",
        "size": 4,
        "posted_at": ["0x7a440b"]
    },
    {
        "id": 1100002,
        "name": "LocalLibraryCrawlingFinished_t",
        "size": 1,
        "posted_at": ["0x7a4eec"]
    },
    {
        "id": 1100003,
        "name": "LocalLibraryCrawlingInterrupted_t",
        "size": 1,
        "posted_at": ["0x7a4724"]
    },
    {
        "id": 1100007,
        "name": "QueueEntriesHaveChanged_t",
        "size": 1,
        "posted_at": ["0x7c593b"]
    },
    {
        "id": 1100008,
        "name": "CurrentQueueEntryHasChanged_t",
        "size": 1,
        "posted_at": ["0x7a24bf","0x7c5231","0x7c52ec","0x7c62aa","0x7c78f0","0x7c7f1d","0x7c7f98","0x7c8750"]
    },
    {
        "id": 1100009,
        "name": "QueueHasEnded_t",
        "size": 1,
        "posted_at": ["0x7c87d5"]
    },
    {
        "id": 1100010,
        "name": "CurrentQueueEntrySecondsElapsedChanged_t",
        "size": 4,
        "posted_at": ["0x7c4aa7"]
    },
    {
        "id": 1100013,
        "name": "LocalLibraryDatabaseHasBeenReset_t",
        "size": 1,
        "posted_at": ["0x7a409e"]
    },
    {
        "id": 1100014,
        "name": "LocalLibraryCrawlingWillStart_t",
        "size": 1,
        "posted_at": ["0x7a4592"]
    },
    {
        "id": 1100015,
        "name": "LocalLibraryUpdateFromCrawling_t",
        "size": 4,
        "posted_at": ["0x7a4615"]
    },
    {
        "id": 1100018,
        "name": "LocalLibraryRemoveArtistFromCrawling_t",
        "size": 1024,
        "posted_at": ["0x7a6267"]
    },
    {
        "id": 1100020,
        "name": "LocalLibraryCrawlingMessage_t",
        "size": 4,
        "posted_at": ["0x7a47b5"]
    },
    {
        "id": 1100021,
        "name": "RemotePlayerRegistered_t",
        "size": 4,
        "posted_at": ["0x7a00f3","0x7a1342"]
    },
    {
        "id": 1100024,
        "name": "RemotePlayerDisplayNameChanged_t",
        "size": 4,
        "posted_at": ["0x7a17d8"]
    },
    {
        "id": 1100027,
        "name": "ArtistListDataEntry_t",
        "size": 2056,
        "posted_at": ["0x7b8597"]
    },
    {
        "id": 1100028,
        "name": "AlbumListDataEntry_t",
        "size": 4120,
        "posted_at": ["0x7b8a7d"]
    },
    {
        "id": 1100029,
        "name": "TrackListDataEntryForAlbumID_t",
        "size": 3092,
        "posted_at": ["0x7b90c5"]
    },
    {
        "id": 1100031,
        "name": "PlaylistListDataEntry_t",
        "size": 2080,
        "posted_at": ["0x7b9a1d"]
    },
    {
        "id": 1100032,
        "name": "TrackListDataEntryForPlaylistID_t",
        "size": 3096,
        "posted_at": ["0x7bac2d"]
    },
    {
        "id": 1100033,
        "name": "PlaylistNameHasChanged_t",
        "size": 1028,
        "posted_at": ["0x7ba5aa"]
    },
    {
        "id": 1100034,
        "name": "PlaylistAdded_t",
        "size": 8,
        "posted_at": ["0x7ba130"]
    },
    {
        "id": 1100035,
        "name": "PlaylistDeleted_t",
        "size": 4,
        "posted_at": ["0x7ba328"]
    },
    {
        "id": 1100036,
        "name": "PlaylistMoved_t",
        "size": 8,
        "posted_at": ["0x7b9e8d"]
    },
    {
        "id": 1100037,
        "name": "PlaylistEntriesHaveMoved_t",
        "size": 12,
        "posted_at": ["0x7bb1ec"]
    },
    {
        "id": 1100038,
        "name": "PlaylistEntriesDeleted_t",
        "size": 8,
        "posted_at": ["0x7bc7b1"]
    },
    {
        "id": 1100039,
        "name": "PlaylistAllEntriesDeleted_t",
        "size": 4,
        "posted_at": ["0x7bc8da"]
    },
    {
        "id": 1100040,
        "name": "PlaylistEntriesHaveChanged_t",
        "size": 4,
        "posted_at": ["0x7a5cb0","0x7bcdd2","0x7bd3c6","0x7bdb0d","0x7be043","0x7be30f"]
    },
    {
        "id": 1100042,
        "name": "PlaylistDurationHasChanged_t",
        "size": 4,
        "posted_at": ["0x7bc421"]
    },
    {
        "id": 1110002,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x8115b2"]
    },
    {
        "id": 1110004,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x816ef7"]
    },
    {
        "id": 1110005,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x7ff61d","0x804c85"]
    },
    {
        "id": 1110006,
        "name": "Unknown",
        "size": 80,
        "posted_at": ["0x7fcc2d","0x800e1f","0x800eff","0x800fb2","0x801280","0x8018ba","0x801afd","0x801bf0","0x801e20","0x801ee5"]
    },
    {
        "id": 1110007,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x816645","0x83448d","0x834ab1"]
    },
    {
        "id": 1110009,
        "name": "Unknown",
        "size": 272,
        "posted_at": ["0x801a30"]
    },
    {
        "id": 1110010,
        "name": "Unknown",
        "size": 60,
        "posted_at": ["0x81646d"]
    },
    {
        "id": 1110011,
        "name": "Unknown",
        "size": 564,
        "posted_at": ["0x8237d3"]
    },
    {
        "id": 1110012,
        "name": "Unknown",
        "size": 40,
        "posted_at": ["0x82440e"]
    },
    {
        "id": 1110013,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x7fc4a5"]
    },
    {
        "id": 1110015,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x810f39"]
    },
    {
        "id": 1110017,
        "name": "Unknown",
        "size": 164,
        "posted_at": ["0x8269b2"]
    },
    {
        "id": 1110018,
        "name": "Unknown",
        "size": 128,
        "posted_at": ["0x8244d9"]
    },
    {
        "id": 1110019,
        "name": "RemoteDeviceAuthorizationRequested_t",
        "size": 128,
        "posted_at": ["0x805f99","0x8063f9"]
    },
    {
        "id": 1110020,
        "name": "RemoteDeviceAuthorizationCancelled_t",
        "size": 1,
        "posted_at": ["0x7fd7f4","0x8060ce"]
    },
    {
        "id": 1110022,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x810a45"]
    },
    {
        "id": 1110023,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x7ff4cd","0x804f1d"]
    },
    {
        "id": 1110024,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x7ff928","0x804722"]
    },
    {
        "id": 1110025,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x86bbb9"]
    },
    {
        "id": 1110026,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x8193b4"]
    },
    {
        "id": 1110028,
        "name": "RemotePlayInviteResult_t",
        "size": 1072,
        "posted_at": ["0x7fcf30"]
    },
    {
        "id": 1110029,
        "name": "Unknown",
        "size": 40,
        "posted_at": ["0x8254b5"]
    },
    {
        "id": 1110030,
        "name": "Unknown",
        "size": 40,
        "posted_at": ["0x80b5c5"]
    },
    {
        "id": 1110031,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x824a30"]
    },
    {
        "id": 1110032,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8262ad"]
    },
    {
        "id": 1110035,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8284f0"]
    },
    {
        "id": 1110036,
        "name": "Unknown",
        "size": 44,
        "posted_at": ["0x828646"]
    },
    {
        "id": 1110038,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x8286f0"]
    },
    {
        "id": 1110039,
        "name": "RemoteClientLaunchFailed_t",
        "size": 4,
        "posted_at": ["0x832036","0x8321dc","0x832349","0x832791","0x832919","0x83298b"]
    },
    {
        "id": 1130002,
        "name": "ShortcutRemoved_t",
        "size": 8,
        "posted_at": ["0x9e1bcf"]
    },
    {
        "id": 1150002,
        "name": "GameNotificationRemoved_t",
        "size": 12,
        "posted_at": ["0x986d20"]
    },
    {
        "id": 1150003,
        "name": "GameNotification_t",
        "size": 4124,
        "posted_at": ["0x987909"]
    },
    {
        "id": 1170001,
        "name": "30SharedConnectionMessageReady_t",
        "size": 4,
        "posted_at": ["0x2c8329"]
    },
    {
        "id": 1180001,
        "name": "ProcessShaderCacheResult_t",
        "size": 8,
        "posted_at": ["0x864d96"]
    },
    {
        "id": 1200002,
        "name": "Unknown",
        "size": 8,
        "posted_at": ["0x333369"]
    },
    {
        "id": 1210002,
        "name": "39ClientNetworking_RequestSDRPingLocation",
        "size": 1,
        "posted_at": ["0x787db2"]
    },
    {
        "id": 1250014,
        "name": "31RequestFriendsLobbiesResponse_t",
        "size": 24,
        "posted_at": ["0x98ee5b","0x98ef13"]
    },
    {
        "id": 1260001,
        "name": "30RemoteStorageAppSyncedClient_t",
        "size": 12,
        "posted_at": ["0x9c3a1c","0x289f21","0x2919ba","0x291cc6","0x29659b","0x29709b","0x2971c2","0x99fd2c","0x9c39f7"]
    },
    {
        "id": 1260002,
        "name": "30RemoteStorageAppSyncedServer_t",
        "size": 12,
        "posted_at": ["0x9c3d64","0x9c3e6d","0x289f95","0x291a77","0x291d44","0x291dbf","0x294a52","0x296e37","0x99fcc2","0x9c3d3f","0x9c3e45"]
    },
    {
        "id": 1260004,
        "name": "28RemoteStorageAppInfoLoaded_t",
        "size": 8,
        "posted_at": ["0x28c2b8"]
    },
    {
        "id": 1260005,
        "name": "33RemoteStorageAppSyncStatusCheck_t",
        "size": 8,
        "posted_at": ["0x9c328e","0x289fec","0x2918f3","0x291b24","0x99fd94","0x9c3210","0x9c326c"]
    },
    {
        "id": 1260006,
        "name": "33RemoteStorageConflictResolution_t",
        "size": 8,
        "posted_at": ["0x9c45f0","0x28a050","0x291049","0x291bde","0x293a3a","0x294d57","0x99fc60","0x9c45cd"]
    },
    {
        "id": 1260012,
        "name": "44RemoteStorage_SubscribedFileDownloadPaused_t",
        "size": 12,
        "posted_at": ["0x8c0b17"]
    },
    {
        "id": 1260013,
        "name": "47RemoteStorage_SubscribedFileDownloadCompleted_t",
        "size": 16,
        "posted_at": ["0x8bfbea"]
    },
    {
        "id": 1260014,
        "name": "44RemoteStorage_SubscribedFileDownloadQueued_t",
        "size": 16,
        "posted_at": ["0x8bea0f"]
    },
    {
        "id": 1260015,
        "name": "40RemoteStorage_SubscribedFilesProcessed_t",
        "size": 1,
        "posted_at": ["0x8bf4e9"]
    },
    {
        "id": 1260016,
        "name": "36RemoteStorage_AppCloudStateChanged_t",
        "size": 4,
        "posted_at": ["0x9a0bfa"]
    },
    {
        "id": 1260017,
        "name": "49RemoteStorage_AppPlatformOverrideBackupComplete_t",
        "size": 4,
        "posted_at": ["0x295d53"]
    },
    {
        "id": 1260018,
        "name": "50RemoteStorage_AppPlatformOverrideRestoreComplete_t",
        "size": 4,
        "posted_at": ["0x29606c"]
    },
    {
        "id": 1270001,
        "name": "BeginBroadcastSessionResult_t",
        "size": 12,
        "posted_at": ["0x3054d0"]
    },
    {
        "id": 1270002,
        "name": "EndBroadcastSessionResult_t",
        "size": 12,
        "posted_at": ["0x302fb2"]
    },
    {
        "id": 1270006,
        "name": "WatchBroadcastResult_t",
        "size": 4,
        "posted_at": ["0x3040c1"]
    },
    {
        "id": 1270007,
        "name": "InviteToBroadcastResult_t",
        "size": 4,
        "posted_at": ["0x304334"]
    },
    {
        "id": 1270008,
        "name": "BroadcastViewerState_t",
        "size": 16,
        "posted_at": ["0x306151"]
    },
    {
        "id": 1270009,
        "name": "ViewerNeedsApproval_t",
        "size": 8,
        "posted_at": ["0x3063d6"]
    },
    {
        "id": 1270010,
        "name": "ViewerApprovalResult_t",
        "size": 16,
        "posted_at": ["0x301a2f","0x302477"]
    },
    {
        "id": 1270012,
        "name": "BroadcastInvitedToView_t",
        "size": 8,
        "posted_at": ["0x2f97c7","0x2fa2fc"]
    },
    {
        "id": 1270013,
        "name": "BroadcastSettingsLoaded_t",
        "size": 1,
        "posted_at": ["0x2fbdf8"]
    },
    {
        "id": 1270014,
        "name": "BroadcastShowFirstTimeDlg_t",
        "size": 8,
        "posted_at": ["0x305f29"]
    },
    {
        "id": 1270015,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x2fb9d1"]
    },
    {
        "id": 1270016,
        "name": "Unknown",
        "size": 292,
        "posted_at": ["0x219b88","0x219c94","0x219da4","0x219eb4"]
    },
    {
        "id": 1270018,
        "name": "BroadcastUploadConfig_t",
        "size": 4,
        "posted_at": ["0x219376"]
    },
    {
        "id": 1270019,
        "name": "UnlockH264Result_t",
        "size": 260,
        "posted_at": ["0x2f9dc9","0x2fa4ea","0x8b98d1"]
    },
    {
        "id": 1270020,
        "name": "BroadcastRecorderError_t",
        "size": 4,
        "posted_at": ["0x2f9738"]
    },
    {
        "id": 1270022,
        "name": "Unknown",
        "size": 2,
        "posted_at": ["0x2f9695"]
    },
    {
        "id": 1270023,
        "name": "BroadcastCantStart_t",
        "size": 12,
        "posted_at": ["0x305db2"]
    },
    {
        "id": 1270025,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x21b388"]
    },
    {
        "id": 1270026,
        "name": "BroadcastThumbnailUploadStop_t",
        "size": 4,
        "posted_at": ["0x2187e7"]
    },
    {
        "id": 1270028,
        "name": "BroadcastWebRTCStop_t",
        "size": 1,
        "posted_at": ["0x30493a","0x306ae1"]
    },
    {
        "id": 1270029,
        "name": "BroadcastWebRTCAnswerReady_t",
        "size": 1,
        "posted_at": ["0x2f9922","0x2f9f80"]
    },
    {
        "id": 1270030,
        "name": "Unknown",
        "size": 1284,
        "posted_at": ["0x2fdcf5"]
    },
    {
        "id": 1270031,
        "name": "Unknown",
        "size": 264,
        "posted_at": ["0x2f9a2a"]
    },
    {
        "id": 1280001,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x1d766e","0x1da3fd"]
    },
    {
        "id": 1280003,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x894d51"]
    },
    {
        "id": 1280006,
        "name": "Unknown",
        "size": 16,
        "posted_at": ["0x1f35d5","0x1f71bd"]
    },
    {
        "id": 1280007,
        "name": "Unknown",
        "size": 32,
        "posted_at": ["0x2a109b"]
    },
    {
        "id": 1280009,
        "name": "Unknown",
        "size": 144,
        "posted_at": ["0x1fb6b0","0x1fb7d8"]
    },
    {
        "id": 1280010,
        "name": "Unknown",
        "size": 12,
        "posted_at": ["0x2a011d"]
    },
    {
        "id": 1280011,
        "name": "Unknown",
        "size": 32,
        "posted_at": ["0x8a91ff"]
    },
    {
        "id": 1280012,
        "name": "Unknown",
        "size": 40,
        "posted_at": ["0x283809"]
    },
    {
        "id": 1280016,
        "name": "AppAutoUpdateBehaviorChanged_t",
        "size": 8,
        "posted_at": ["0x1e9244"]
    },
    {
        "id": 1280017,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x1d64cd","0x1d6ecd","0x1dd6fa","0x1dd8ba"]
    },
    {
        "id": 1280019,
        "name": "AppConfigChanged_t",
        "size": 4,
        "posted_at": ["0x92a667","0x93eb40","0x94ce97","0x94d6b7","0x94d9e0","0x1d491b","0x337681"]
    },
    {
        "id": 1280020,
        "name": "CheckAppBetaPasswordResponse_t",
        "size": 200,
        "posted_at": ["0x26cab7"]
    },
    {
        "id": 1280024,
        "name": "Unknown",
        "size": 20,
        "posted_at": ["0x29c2ce"]
    },
    {
        "id": 1280025,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x1fc1eb"]
    },
    {
        "id": 1280027,
        "name": "AppLaunchResult_t",
        "size": 524,
        "posted_at": ["0x1f62af"]
    },
    {
        "id": 1280028,
        "name": "Unknown",
        "size": 1,
        "posted_at": ["0x1cf8ac","0x1d3ffb"]
    },
    {
        "id": 1280029,
        "name": "Unknown",
        "size": 24,
        "posted_at": ["0x8a910f"]
    },
    {
        "id": 1280031,
        "name": "Unknown",
        "size": 4,
        "posted_at": ["0x1e7ba3","0x1f2555","0x1f3aad","0x1f41b0"]
    },
    {
        "id": 1290004,
        "name": "18PurchaseResponse_t",
        "size": 4136,
        "posted_at": ["0x27ba20"]
    },
    {
        "id": 1290005,
        "name": "Unknown",
        "size": 1032,
        "posted_at": ["0x27b8a5"]
    },
    {
        "id": 1300001,
        "name": "StreamClientRaiseWindow_t",
        "size": 1,
        "posted_at": ["0x837727"]
    },
    {
        "id": 1300002,
        "name": "StreamClientResult_t",
        "size": 4,
        "posted_at": ["0x837efc","0x838297"]
    },
    {
        "id": 1300003,
        "name": "StreamClientConfigUpdated_t",
        "size": 1,
        "posted_at": ["0x837867"]
    },
    {
        "id": 1300005,
        "name": "StreamClientControllerMessageQueuedForRemote_t",
        "size": 1,
        "posted_at": ["0x837970"]
    },
    {
        "id": 1300006,
        "name": "StreamClientControllerMessageQueuedForLocal_t",
        "size": 1,
        "posted_at": ["0x837abc"]
    }
];

console.log("using System.Collections.Generic;");
console.log("");
console.log("namespace OpenSteamworks.Callbacks;");
console.log("");
console.log("internal static class KnownCallbackNames {");
console.log("    public readonly static Dictionary<int, string> CallbackNames = new Dictionary<int, string>");
console.log("    {");
callbacks.forEach((cb)=> {
	console.log(`       {${cb.id}, "${cb.name}"},`);
})
console.log("    };");
console.log("}");