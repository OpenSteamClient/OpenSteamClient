#pragma once
#include <map>
#include <QObject>


class AbstractDynamicWebViewInterface;
typedef AbstractDynamicWebViewInterface *(*GetWebViewInterface_ptr)(int index);

class DynamicWebViewLibraryMgr
{
private:
    void *dl_handle = nullptr;

    // Use this instead of a vector since it's easier
    // NOTE: the void* doesn't mean anything
    std::map<AbstractDynamicWebViewInterface*, void*> loadedInterfaces;

    // This function gets the interfaces
    GetWebViewInterface_ptr getterFunc = nullptr;

    // Initializes and loads the shared library.
    bool initialize();

    // Whether we are allowed to initialize
    bool enabled = true;
public:
    // Creates a new interface and returns it's pointer.
    AbstractDynamicWebViewInterface *GetInterface();

    // Unloads an interface
    void Unload(AbstractDynamicWebViewInterface *interface);

    // Enables/disables registering of new interfaces and library initialization.
    // Doesn't kill existing webviews.
    void SetEnabled(bool enable);

    DynamicWebViewLibraryMgr(/* args */);
    ~DynamicWebViewLibraryMgr();
};