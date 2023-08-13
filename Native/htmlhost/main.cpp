#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>
#include <link.h>
#include <unistd.h>

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