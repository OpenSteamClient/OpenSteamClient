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
#define CHROMEHTML_LIB "chromehtml.dll"
#define TIER0_LIB "..\\tier0_s.dll"
#include <windows.h>
#include "windowssupport.h"
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

int main(int argc, char *argv[])
{
    // Kill process when parent dies (windows doesn't support this)
#if __linux__
    prctl(PR_SET_PDEATHSIG, SIGKILL);
#endif 

    if (argc < 3) {
        std::cerr << "Missing required arguments [cachedir, steampath]" << std::endl;
        return 1;
    }

    auto dl_handle_chromehtml = dlopen(CHROMEHTML_LIB, RTLD_NOW);
    if (dl_handle_chromehtml == nullptr)
    {
        std::cerr << "dl_handle_chromehtml == nullptr!!!" << std::endl;
        return 1;
    }

    auto dl_handle_tier0 = dlopen(TIER0_LIB, RTLD_NOW);
    if (dl_handle_tier0 == nullptr)
    {
        std::cerr << "dl_handle_tier0 == nullptr!!!" << std::endl;
        return 1;
    }

    overrideArgvFn Plat_InternalOverrideArgv = (overrideArgvFn)dlsym(dl_handle_tier0, "Plat_InternalOverrideArgv");
    if (Plat_InternalOverrideArgv == nullptr)
    {
        std::cerr << "Plat_InternalOverrideArgv == nullptr!!!" << std::endl;
        return 1;
    }

    argv[0] = getenv("OPENSTEAM_EXE_PATH");
    Plat_InternalOverrideArgv(argv, argc);

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

    HTMLOptions options;
    options.strHTMLCacheDir = argv[1];
    options.universe = 1;
    options.strProxy = "";
    options.language = 0;
    options.uimode = 0;
    options.field5_0x14 = 0;
    options.argsFlags = 0;
    options.field7_0x16 = 0;
    options.field8_0x17 = 0;
    options.field9_0x18 = nullptr;
    options.field10_0x1c = 0;
    options.field11_0x20 = 0;
    options.field12_0x21 = 0;
    options.field13_0x22 = 0;
    options.field14_0x26 = 0;
    options.field15_0x30 = 0;

    // char *field0_0x0;
    // uint field1_0x4;
    // int field2_0x8;
    // undefined4 field3_0xc;
    // int field4_0x10;
    // undefined field5_0x14;
    // undefined field6_0x15;
    // undefined field7_0x16;
    // undefined field8_0x17;
    // undefined **field9_0x18;
    // int field10_0x1c;
    // undefined field11_0x20;
    // undefined field12_0x21;

    controller->SetOptions(&options);
    controller->StartThread();
    controller->Start();
    //std::cout << "CreateOffscreenBrowser" << controller->CreateOffscreenBrowser("Valve Steam Client", nullptr, "", 1, 1, EBrowserType::OffScreen, 1) << std::endl;
    std::cout << "Kill with CTRL+C" << std::endl;
    signal(SIGINT, handle_sigint);
    while (!done)
    {
        controller->RunFrame();
        std::this_thread::sleep_for(std::chrono::microseconds(50));
    }
    signal(SIGINT, SIG_DFL);
}