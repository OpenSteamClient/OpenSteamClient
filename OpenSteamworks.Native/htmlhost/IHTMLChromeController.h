#pragma once

#include "EBrowserType.h"
#include "EHTMLCommands.h"
#include <utlbuffer.h>

typedef unsigned char   undefined;
typedef unsigned char    undefined1;
typedef unsigned short    undefined2;
typedef unsigned int    undefined4;
typedef unsigned long long    undefined8;

struct HTMLOptions
{
	const char * strHTMLCacheDir;
    uint universe;
    const char* strProxy;
    undefined4 language;
    int uimode;
    undefined field5_0x14;
    undefined argsFlags;
    undefined field7_0x16;
    undefined field8_0x17;
    undefined **field9_0x18;
    int field10_0x1c;
    undefined field11_0x20;
    undefined field12_0x21;
    uint field13_0x22;
    uint field14_0x26;
    uint field15_0x30;
};

//-----------------------------------------------------------------------------
// Purpose: a single IPC packet for the html thread (in and out)
//-----------------------------------------------------------------------------
struct HTMLCommandBuffer_t
{
	EHTMLCommands m_eCmd;
	int m_iBrowser;
	CUtlBuffer m_Buffer;

    HTMLCommandBuffer_t() : m_Buffer(1, 256, 0) {
        
    }
};

struct StructThatHasCallback {
    undefined field0_0x0;
    undefined field1_0x1;
    undefined field2_0x2;
    undefined field3_0x3;
    undefined field4_0x4;
    undefined field5_0x5;
    undefined field6_0x6;
    undefined field7_0x7;
    void (* func)(undefined4, struct StructThatHasCallback *, int);
    undefined4 field9_0xc;
};

// Most of the functions here don't work. Still figuring this out...
class IHTMLChromeController
{
public:
    virtual void Destructor() = 0;
    virtual void Destructor2() = 0;
	virtual void SetOptions(HTMLOptions*) = 0;
	virtual void Shutdown() = 0;
	virtual void Start() = 0;
	virtual void StartThread() = 0;
	virtual void BUsesCPUInfo() = 0;
	virtual void Exit() = 0;
	virtual int RunFrame() = 0;
	virtual int ReturnsField0x160() = 0;
	virtual void RegisterSomeFunc(StructThatHasCallback*) = 0;
	virtual void SetCookie() = 0;
	virtual void GetCookieForURL() = 0;
	virtual void ProtobufFUN_000308c0() = 0;
	virtual void ProtobufFUN_00030d00() = 0;
	virtual void ProtobufFUN_00030a20() = 0;
	virtual int CreateBrowser() = 0;
	virtual int CreateOffscreenBrowser(char* param_1,char *param_3,char *param_4, undefined4 param_5, undefined4 param_6, EBrowserType param_7, undefined4 param_8) = 0;
	virtual int CreateDirectRenderingBrowser(char* param_1,char *param_3,char *param_4, undefined4 param_5, undefined4 param_6, EBrowserType param_7, undefined4 param_8) = 0;
	virtual int CreateVRBrowser(char* param_1,char *param_3,char *param_4, undefined4 param_5, undefined4 param_6, const char* pchVrOverlayKey, undefined4 param_8) = 0;
	virtual int CreateBrowser2(char* param_1,char *param_3,char *param_4, undefined4 param_5, undefined4 param_6, undefined4 param_7, undefined4 param_8) = 0;
	virtual void SerializeFUN_0003e2b0(int handle) = 0;
	virtual HTMLCommandBuffer_t *PackHTMLMessage(EHTMLCommands command, int iBrowser) = 0;
	virtual void PushCommand(HTMLCommandBuffer_t*) = 0;
	virtual void FUN_0002e6e0() = 0;
	virtual int ReadsEnvSTEAMVIDEOTOKEN() = 0;
	virtual void SerializeFUN_00030770() = 0;
	virtual void Assigns0x80() = 0;
	virtual void Assigns0x2c0() = 0;
	virtual void SerializeFUN_000304b0() = 0;
	virtual void SerializeFUN_00031470() = 0;
	virtual void SerializeFUN_00030b80() = 0;
	virtual void SerializeFUN_00030610() = 0;
	virtual char* AssignsDefaultStringTo0x84IfNotSetAndReturnsIt() = 0;
	// Gets a relative path to the Steam webhelper. Hardcoded.
	virtual const char* GetSteamWebhelperPath() = 0;

	virtual void Validate(void *validator, const char*) = 0;

	virtual bool ChromePrepareForValidate() = 0;
	virtual bool ChromeResumeFromValidate() = 0;

	virtual int GetPID() = 0;
};

        // 						PTR_~CHTMLController_00247df8                   XREF[6]:     ~CHTMLController:00031723(*), 
        //                                                                                   ~CHTMLController:00031729(*), 
        //                                                                                   ~CHTMLController:00031be3(*), 
        //                                                                                   ~CHTMLController:00031be9(*), 
        //                                                                                   FUN_00034930:0003494f(*), 
        //                                                                                   FUN_00034930:00034960(*)  
        // 00247df8 c0 1b 03 00     addr       CHTMLController::~CHTMLController
        // 00247dfc 00 17 03 00     addr       CHTMLController::~CHTMLController
        // 00247e00 20 01 03 00     addr       CHTMLController::SetOptions
        // 00247e04 b0 ff 02 00     addr       CHTMLController::Shutdown
        // 00247e08 e0 ff 02 00     addr       CHTMLController::Start
        // 00247e0c f0 03 03 00     addr       CHTMLController::StartThread
        // 00247e10 20 ff 02 00     addr       CHTMLController::BUsesCPUInfo
        // 00247e14 50 ff 02 00     addr       CHTMLController::Exit
        // 00247e18 20 6f 03 00     addr       CHTMLController::RunFrame
        // 00247e1c d0 d1 02 00     addr       CHTMLController::ReturnsField0x160
        // 00247e20 a0 e5 02 00     addr       CHTMLController::RegisterSomeFunc
        // 00247e24 80 20 03 00     addr       CHTMLController::SetCookie
        // 00247e28 d0 25 03 00     addr       CHTMLController::GetCookieForURL
        // 00247e2c c0 08 03 00     addr       CHTMLController::ProtobufFUN_000308c0
        // 00247e30 00 0d 03 00     addr       CHTMLController::ProtobufFUN_00030d00
        // 00247e34 20 0a 03 00     addr       CHTMLController::ProtobufFUN_00030a20
        // 00247e38 a0 59 03 00     addr       CHTMLController::CreateBrowser
        // 00247e3c 40 5b 03 00     addr       CHTMLController::CreateOffscreenBrowser
        // 00247e40 30 5d 03 00     addr       CHTMLController::CreateDirectRenderingBrowser
        // 00247e44 10 5f 03 00     addr       CHTMLController::CreateVRBrowser
        // 00247e48 00 61 03 00     addr       CHTMLController::CreateBrowser2
        // 00247e4c b0 e2 03 00     addr       CHTMLController::SerializeFUN_0003e2b0
        // 00247e50 80 e6 02 00     addr       CHTMLController::AssertFUN_0002e680
        // 00247e54 b0 e6 02 00     addr       CHTMLController::PushCommand
        // 00247e58 e0 e6 02 00     addr       CHTMLController::FreeSomething
        // 00247e5c 50 29 03 00     addr       CHTMLController::ReadsEnvSTEAMVIDEOTOKEN
        // 00247e60 70 07 03 00     addr       CHTMLController::SerializeFUN_00030770
        // 00247e64 e0 d1 02 00     addr       CHTMLController::Assigns0x80
        // 00247e68 40 d2 02 00     addr       CHTMLController::Assigns0x2c0
        // 00247e6c b0 04 03 00     addr       CHTMLController::SerializeFUN_000304b0
        // 00247e70 70 14 03 00     addr       CHTMLController::SerializeFUN_00031470
        // 00247e74 80 0b 03 00     addr       CHTMLController::SerializeFUN_00030b80
        // 00247e78 10 06 03 00     addr       CHTMLController::SerializeFUN_00030610
        // 00247e7c f0 d1 02 00     addr       CHTMLController::AssignsDefaultStringTo0x84IfN
        // 00247e80 b0 00 03 00     addr       CHTMLController::GetSteamWebhelperPath
        // 00247e84 30 0f 03 00     addr       CHTMLController::Validate
        // 00247e88 50 d2 02 00     addr       CHTMLController::AlwaysReturns1
        // 00247e8c 50 d2 02 00     addr       CHTMLController::AlwaysReturns1
        // 00247e90 80 00 03 00     addr       CHTMLController::GetPID

