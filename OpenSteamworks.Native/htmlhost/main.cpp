#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>
#include <chrono>
#include <thread>

#if __linux__
#define CHROMEHTML_LIB "chromehtml.so"
#define TIER0_LIB "libtier0_s.so"
#include <link.h>
#include <unistd.h>
#include <sys/prctl.h>
#endif 

#if _WIN32
#define CHROMEHTML_LIB "bin\\chromehtml.dll"
#define TIER0_LIB "tier0_s.dll"
#include <windows.h>
#include "../windowssupport.h"
#include "windowshooks.h"
#include "../bootstrappershim/windowsshim.h"
#endif

// DEBUGGER
#include <signal.h>
#include <csignal> // or C++ style alternative

#include "IHTMLChromeController.h"

bool done = false;
void handle_sigint(int sig)
{
    done = true;
}

void WaitForControlSignalToContinue() {
  std::cout << "Waiting for CTRL+C" << std::endl;
  signal(SIGINT, handle_sigint);
  while (!done) {
    std::this_thread::sleep_for(std::chrono::milliseconds(1000));
  }
  done = false;
  signal(SIGINT, SIG_DFL);
}

typedef void *(*createInterfaceFn)(const char *, int *);
typedef void *(*overrideArgvFn)(char *argv[], int argc);

// Notes:
// buildid is determined with tier0's GetMiniDumpBuildID
// steamid is determined with tier0's GetMiniDumpSteamID
// The main interface (IHTMLChromeController, CChromeIPClient) is also duplicated within steamclient.so. Yep. Even in the 64-bit one. Don't know how to access it though, shit...
int main(int argc, char *argv[])
{
    // Kill process when parent dies (windows doesn't support this, might leave lingering processes)
#if __linux__
    prctl(PR_SET_PDEATHSIG, SIGKILL);
#endif 

    if (argc < 16) {
        std::cerr << "Missing required arguments [cachedir, steampath, universe, realm, language, uimode, enablegpuacceleration, enablesmoothscrolling, enablegpuvideodecode, enablehighdpi, proxyserver, bypassproxyforlocalhost, composermode, ignoregpublocklist, allowworkarounds]" << std::endl;
        return 1;
    }

    auto dl_handle_tier0 = dlopen(TIER0_LIB, RTLD_NOW);
    if (dl_handle_tier0 == nullptr)
    {
        std::cerr << "dl_handle_tier0 == nullptr!!!" << std::endl;
        return 1;
    }

// Create hooks on Windows
#if _WIN32
    WindowsHookFunc((HMODULE)dl_handle_tier0, "ThreadGetCurrentProcessId", (void*)&GetCurrentProcessIdHook);
    
    WindowsHookFunc((HMODULE)dl_handle_tier0, "CreateSimpleProcess", (void*)&CreateSimpleProcessHook);
    // This is a hack and is very terrible, but chromehtml will not use the correct binary (64-bit one) if we don't override tier0's and vstdlib's detections.
    WindowsHookFunc((HMODULE)dl_handle_tier0, "Is64BitOS", (void*)&Is64BitOSHook);
    WindowsHookFunc("vstdlib_s.dll", "GetOSType", (void*)&GetOSTypeHook);
    // This feels wrong, but isn't?
    // WindowsHookFunc((HMODULE)dl_handle_tier0, "Plat_GetExecutablePath", &Plat_GetExecutablePathHook);
    // WindowsHookFunc((HMODULE)dl_handle_tier0, "Plat_GetExecutablePathUTF8", &Plat_GetExecutablePathHook);
#endif

    auto dl_handle_chromehtml = dlopen(CHROMEHTML_LIB, RTLD_NOW);
    if (dl_handle_chromehtml == nullptr)
    {
        std::cerr << "dl_handle_chromehtml == nullptr!!!" << std::endl;
        return 1;
    }

    createInterfaceFn createInterface = (createInterfaceFn)dlsym(dl_handle_chromehtml, "CreateInterface");
    if (createInterface == nullptr)
    {
        std::cerr << "createInterface == nullptr!!!" << std::endl;
        return 1;
    }

    int returnCode;
    IHTMLChromeController* controller = (IHTMLChromeController*)createInterface("ChromeHTML_Controller_003", &returnCode);
    if (controller == nullptr)
    {
        std::cerr << "controller == nullptr!!!" << std::endl;
        return 1;
    }

    if (returnCode != 0) {
        std::cerr << "returnCode != 0!!!" << std::endl;
        return 1;
    }

    HTMLOptions* options = new HTMLOptions();
    options->cacheDir = argv[1];
    options->universe = std::stoi(argv[3]);
    options->realm = std::stoi(argv[4]);
    options->language = std::stoi(argv[5]);
    options->uiMode = std::stoi(argv[6]);
    options->enableGpuAcceleration = std::stoi(argv[7]);
    options->enableSmoothScrolling = std::stoi(argv[8]);
    options->enableGPUVideoDecode = std::stoi(argv[9]);
    options->enableHighDPI = std::stoi(argv[10]);
    options->proxyServer = argv[11];
    options->bypassProxyForLocalhost = std::stoi(argv[12]);
    options->padding1 = 0;
    options->padding2 = 0;
    options->padding3 = 0;
    options->composerMode = std::stoi(argv[13]);
    options->ignoreGPUBlocklist = std::stoi(argv[14]);
    options->allowWorkarounds = std::stoi(argv[15]);
    options->padding4 = 0;
    options->padding5 = 0;
    options->padding6 = 0;

    // This isn't enough. Hooking is needed. Stupid.
    //controller->SetHostingProcessPID(std::stoi(getenv("OPENSTEAM_PID")));
    // controller->SetOptions(&options);
    // controller->StartThread();
    // controller->Start();
    std::cout << "startwithoptions " << controller->StartWithOptions(options) << std::endl;
    std::cout << "init " << controller->Init() << std::endl;
    // std::cout << "WTF " << std::endl;
    // std::cout << "FUN_10007400 " << controller->FUN_10007400() << std::endl;
    std::cout << "start " << controller->Start() << std::endl;

   
    //std::cout << "CreateBrowser2 " << controller->CreateBrowser2("Valve Steam Client", nullptr, "https://google.com", 0, 0, EBrowserType::Transparent_Toplevel, 0) << std::endl;
    std::cout << "Webhelper PID " << controller->GetPIDOfWebhelperProcess() << std::endl;
    std::cout << "Kill with CTRL+C" << std::endl;
    signal(SIGINT, handle_sigint);
    while (!done)
    {
        controller->RunFrame();
        std::this_thread::sleep_for(std::chrono::microseconds(1));
    }
    signal(SIGINT, SIG_DFL);
}