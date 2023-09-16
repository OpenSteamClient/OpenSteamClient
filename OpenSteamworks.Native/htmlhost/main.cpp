#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>
#include <link.h>
#include <unistd.h>
#include <sys/prctl.h>

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

int main(int argc, char *argv[])
{
    // Kill process when parent dies
    prctl(PR_SET_PDEATHSIG, SIGKILL);

    if (argc < 2) {
        std::cerr << "Missing required argument cachedir" << std::endl;
        return 1;
    }

    auto dl_handle = dlopen("chromehtml.so", RTLD_NOW);
    if (dl_handle == nullptr)
    {
        std::cerr << "dl_handle == nullptr!!!" << std::endl;
        return 1;
    }

    createInterfaceFn createInterface = (createInterfaceFn)dlsym(dl_handle, "CreateInterface");
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
    options.cachedir = argv[1];
    options.universe = 1;
    options.field2_0x8 = -1;
    options.language = 0;
    options.uimode = 0;
    options.field5_0x14 = 1;
    options.argsFlags = 1;
    options.field7_0x16 = 0;
    options.field8_0x17 = 0;
    options.field9_0x18 = nullptr;
    options.field10_0x1c = 1;
    options.field11_0x20 = 1;
    options.field12_0x21 = 1;
    options.field13_0x22 = 1;
    options.field14_0x26 = 1;
    options.field15_0x30 = 1;

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
    std::cout << "Kill with CTRL+C" << std::endl;
    signal(SIGINT, handle_sigint);
    while (!done)
    {
        controller->RunFrame();
        usleep(50);
    }
    signal(SIGINT, SIG_DFL);
}