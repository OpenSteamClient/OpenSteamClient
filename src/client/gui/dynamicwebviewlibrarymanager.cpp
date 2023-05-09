#include "dynamicwebviewlibrarymanager.h"
#include "../temporary_logging_solution.h"
#include "../../dynamicwebview/dynamicwebviewinterface.h"
#include <dlfcn.h>
#include <QCoreApplication>
#include "application.h"

DynamicWebViewLibraryMgr::DynamicWebViewLibraryMgr(/* args */)
{
}

DynamicWebViewLibraryMgr::~DynamicWebViewLibraryMgr()
{
    assert(loadedInterfaces.size() == 0);
}

bool DynamicWebViewLibraryMgr::initialize() {
    if (!enabled) {
        DEBUG_MSG << "[DynamicWebViewLibraryMgr] initialize() failed, WebViews are disabled" << std::endl;
        return false;
    }

    DEBUG_MSG << "[DynamicWebViewLibraryMgr] Loading libdynamicwebview.so" << std::endl;
    dl_handle = dlopen("libdynamicwebview.so", RTLD_LAZY);
    if (dl_handle == 0x0)
    {
        DEBUG_MSG << "[DynamicWebViewLibraryMgr] Error occurred while loading libdynamicwebview.so: " << dlerror() << std::endl;
        return false;
    }
    getterFunc = (GetWebViewInterface_ptr)dlsym(dl_handle, "GetWebViewInterface");
    if (getterFunc == 0x0) {
        DEBUG_MSG << "[DynamicWebViewLibraryMgr] Error occurred while finding GetWebViewInterface: " << dlerror() << std::endl;
        dlclose(dl_handle);
        dl_handle = nullptr;
        return false;
    }
    DEBUG_MSG << "[DynamicWebViewLibraryMgr] Found GetWebViewInterface" << std::endl;
    return true;
}

AbstractDynamicWebViewInterface *DynamicWebViewLibraryMgr::GetInterface() {
    if (!enabled) {
        DEBUG_MSG << "[DynamicWebViewLibraryMgr] GetInterface() failed, WebViews are disabled" << std::endl;
        return nullptr;
    }
    if (getterFunc == nullptr) {
        bool result = initialize();
        if (!result) {
            DEBUG_MSG << "[DynamicWebViewLibraryMgr] GetInterface() failed, initialization failed" << std::endl;
            return nullptr;
        }
    }
    AbstractDynamicWebViewInterface *interface = getterFunc(loadedInterfaces.size());
    loadedInterfaces.insert({interface, nullptr});
    return interface;
}

void DynamicWebViewLibraryMgr::Unload(AbstractDynamicWebViewInterface *interface) {
    if (loadedInterfaces.contains(interface)) {
        loadedInterfaces.erase(interface);
        interface->DeleteWebEngine();
    }
    if (loadedInterfaces.size() == 0) {
        if (dl_handle != nullptr)
        {
            dlclose(dl_handle);
        }
        dl_handle = nullptr;
        getterFunc = nullptr;
    }
}

void DynamicWebViewLibraryMgr::SetEnabled(bool enable) {
    enabled = enable;
}