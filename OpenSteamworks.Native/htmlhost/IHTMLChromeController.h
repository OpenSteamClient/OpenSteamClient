#pragma once

#include "EBrowserType.h"
#include "EHTMLCommands.h"
#include <utlbuffer.h>
#include "ghidradefines.h"


#pragma pack(push,1)

struct HTMLOptionsOld
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

struct HTMLOptions {
    const char *cacheDir;
    int universe;
    int realm;
    int language;
    int uiMode;
    bool enableGpuAcceleration;
    bool enableSmoothScrolling;
    bool enableGPUVideoDecode;
    bool enableHighDPI;
    const char *proxyServer;
    bool bypassProxyForLocalhost;
    // These 3 are probably packing
    undefined padding1;
    undefined padding2;
    undefined padding3;
    int composerMode;
    bool ignoreGPUBlocklist;
    bool allowWorkarounds;
    undefined padding4;
    undefined padding5;
    undefined padding6;
};

#pragma pack(pop)

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

// Most of the functions here don't work. Still figuring this out...
class IHTMLChromeController
{
public:
    virtual void Destructor1() = 0;
#ifdef __linux__
    virtual void Destructor2() = 0;
#endif
	virtual undefined4 StartWithOptions(void* pOptions) = 0;
	virtual unknown_ret Shutdown() = 0;
	virtual unknown_ret Start() = 0;
	virtual unknown_ret Init() = 0;
    // Checks for SSE2 or ARM
	virtual unknown_ret IsCPUCompatible() = 0;
	virtual unknown_ret FUN_10007400() = 0;
	virtual bool RunFrame() = 0;
	virtual unknown_ret FUN_10007500() = 0;
    virtual unknown_ret FUN_10007510() = 0;
    virtual unknown_ret FUN_10007680() = 0;
    virtual unknown_ret FUN_10007b10() = 0;
    virtual unknown_ret FUN_10007d90() = 0;
    virtual unknown_ret FUN_10007f30() = 0;
    virtual unknown_ret FUN_10007e60() = 0;
	virtual unknown_ret CreateBrowser(const char* param_1, const char *param_3, const char *param_4, undefined4 param_5, undefined4 param_6, EBrowserType param_7, undefined4 param_8) = 0;
	virtual unknown_ret CreateOffscreenBrowser(const char* param_1, const char *param_3, const char *param_4, undefined4 param_5, undefined4 param_6, EBrowserType param_7, undefined4 param_8) = 0;
	virtual unknown_ret CreateDirectRenderingBrowser(const char* param_1, const char *param_3, const char *param_4, undefined4 param_5, undefined4 param_6, EBrowserType param_7, undefined4 param_8) = 0;
	virtual unknown_ret CreateVRBrowser(const char* param_1, const char *param_3, const char *param_4, undefined4 param_5, undefined4 param_6, const char* pchVrOverlayKey, undefined4 param_8) = 0;
	virtual unknown_ret CreateBrowser2(const char* param_1, const char *param_3, const char *param_4, undefined4 param_5, undefined4 param_6, EBrowserType param_7, undefined4 param_8) = 0;
	virtual unknown_ret FUN_100083e0() = 0;
    // iUnk is usually 0xffffffff
	virtual HTMLCommandBuffer_t *AllocateCommand(EHTMLCommands command, int iUnk) = 0;
	virtual unknown_ret PushCommand(HTMLCommandBuffer_t*) = 0;
	virtual unknown_ret FreeCommand(HTMLCommandBuffer_t*) = 0;
	virtual unknown_ret FUN_1000d660() = 0;
	virtual unknown_ret FUN_1000d840() = 0;
    // Not necessary for hosting in this pid, as GetCurrentProcessID is used.
	virtual void SetHostingProcessPID(uint pid) = 0;
	virtual unknown_ret FUN_1000d910() = 0;
	virtual unknown_ret FUN_1000d920() = 0;
	virtual unknown_ret FUN_1000d9e0() = 0;
	virtual unknown_ret FUN_1000db80() = 0;
	virtual unknown_ret FUN_1000dc50() = 0;
	virtual char* GetCacheDir() = 0;
	// Gets a relative path to the Steam webhelper. 
	virtual const char* GetSteamWebhelperPath() = 0;

	virtual void Validate(void *validator, const char*) = 0;
	virtual bool ChromePrepareForValidate() = 0;
	virtual bool ChromeResumeFromValidate() = 0;

	virtual uint GetPIDOfWebhelperProcess() = 0;
};