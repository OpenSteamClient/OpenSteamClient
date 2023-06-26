#include "startup.h"
#include "../globals.h"
#include "../gui/application.h"
#include "../interop/appmanager.h"
#include "../urlprotocol.h"
#include "updater.h"
#include "bootstrapper.h"
#include "../ext/steamservice.h"
#include "../misc_logic/singleinstance.h"
#include <sys/resource.h>
#include <QMainWindow>
#include <QApplication>
#include <QThread>
#include <QDialog>
#include <QProgressDialog>

SteamClientMgr     *Global_SteamClientMgr = nullptr;
SteamServiceMgr    *Global_SteamServiceMgr = nullptr;
bool                Global_debugCbLogging = false;
ThreadController   *Global_ThreadController = nullptr;

URLProtocolHandler *Global_URLProtocolHandler = nullptr;

CommandLine        *Global_CommandLine = nullptr;

Bootstrapper       *Global_Bootstrapper = nullptr;
Updater            *Global_Updater = nullptr;

void Startup::InitQT() {
    // This creates the Application class if it didn't already exist
    Application::GetApplication();
}

void Startup::SetLaunchArgs(int argc, char *argv[]) {
    Global_CommandLine = new CommandLine();
    Global_CommandLine->SetLaunchCommandLine(argc, argv);
}

void Startup::InitGlobals() {
    Global_Updater = new Updater();
    Global_URLProtocolHandler = new URLProtocolHandler();
    Global_debugCbLogging = Global_CommandLine->HasOption("-log-callbacks");
}

void Startup::MaxOpenDescriptorsCheck() {
    rlimit limit;
    rlim_t was;
    getrlimit(RLIMIT_NOFILE, &limit);
    was = limit.rlim_cur;
    if (limit.rlim_cur < limit.rlim_max) {
        limit.rlim_cur = limit.rlim_max;
        setrlimit(RLIMIT_NOFILE, &limit);
    }
    if (Global_debugCbLogging) {
        std::cout << "[Startup] RLIMIT_NOFILE set to " << limit.rlim_cur << ", was " << was << std::endl;
    }
}

void Startup::SetUpdaterFilesDir() {
//TODO: this should be moved somewhere more appropriate
// When this is packaged all our binaries should reside in the same folder so the bootstrapper can copy them (like in /usr/lib/opensteam/)

    if (getenv("UPDATER_FILES_DIR") == NULL)
    {
        auto newpath = std::filesystem::canonical(std::filesystem::read_symlink("/proc/self/exe")).parent_path();

// Our packaged files are in updater_files on release builds
#ifndef DEV_BUILD
    newpath = newpath / "updater_files";
#endif

    setenv("UPDATER_FILES_DIR", newpath.string().c_str(), 0);
    DEBUG_MSG << "[Startup] Setting UPDATER_FILES_DIR to " << newpath << std::endl;
    }

}
void Startup::SingleInstanceChecks() {
    auto thispid = PIDUtils::GetPid();
    DEBUG_MSG << "[Startup] Our PID is " << thispid << std::endl;

    //TODO: implement signals and make global
    auto singleinstancemgr = new SingleInstanceMgr(thispid);
    if (singleinstancemgr->BCheckForInstance()) {
        std::cout << "[Startup] OpenSteamClient is already running; sending args to instance [PID:" << singleinstancemgr->GetInstancePid() << "]" << std::endl;
        singleinstancemgr->SendArgvToInstance(Global_CommandLine->argc, Global_CommandLine->argv);
        exit(EXIT_SUCCESS);
    } else {
        singleinstancemgr->SetThisProcessAsInstance();
        DEBUG_MSG << "[Startup] Registered this process as the instance." << std::endl;
    }
}
void Startup::RunBootstrapper() {
    DEBUG_MSG << "[Startup] RunBootstrapper" << std::endl;
    Global_Bootstrapper = new Bootstrapper();
    if (Global_CommandLine->HasOption("--bootstrapper-restore-valvesteam")) {
        try
        {
            Global_Bootstrapper->SetValveSteamActive();
        }
        catch(const std::exception& e)
        {
            std::cerr << "[Startup] Failed to restore ValveSteam: " << e.what() << std::endl;
            exit(EXIT_FAILURE);
        }

        std::cout << "[Startup] ValveSteam has been restored, you can now safely start it. " << std::endl;
        exit(EXIT_SUCCESS);
    }

    DEBUG_MSG << "[Startup] RunBootstrap" << std::endl;
    Global_Bootstrapper->RunBootstrap();
    Global_Updater->Verify();

    Global_SteamClientMgr = new SteamClientMgr();
    Global_SteamServiceMgr = new SteamServiceMgr();

    if (Global_SteamClientMgr->BFailedLoad()) {
        if (getenv("UPDATER_RAN_ONCE") == NULL)
        {
            std::cerr << "[Startup] Failed to load steamclient.so; verifying and restarting. " << std::endl;
            Global_Updater->Verify(true);
            Global_Bootstrapper->Restart(true);
        } else {
            std::cerr << "[Startup] Failed to load steamclient.so after verification. " << std::endl;
            exit(EXIT_FAILURE);
        }
    }
}

int Startup::UIMain() {
    return Application::GetApplication()->StartApplication();
}

void Startup::InitApplication() {
    Application::GetApplication()->InitApplication();
}
