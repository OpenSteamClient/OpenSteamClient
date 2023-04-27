#include "startup/startup.h"

int main(int argc, char **argv) {

    // InitQT looks at our args  
    Startup::SetLaunchArgs(argc, argv);
    
    // Qt needs to be initialized early for:
    // - QObjects to work
    // - QThreads to work
    Startup::InitQT();
    Startup::MaxOpenDescriptorsCheck();
    Startup::SetUpdaterFilesDir();
    Startup::InitGlobals();
    Startup::SingleInstanceChecks();
    Startup::RunBootstrapper();
    Startup::InitApplication();
    return Startup::UIMain();
}
