#include "dynamicwebviewinterface.h"

std::map<int, DynamicWebViewInterface*> interfaces;
extern "C" DynamicWebViewInterface *GetWebViewInterface(int index)
{
    if (interfaces.contains(index)) {
        return interfaces.at(index);
    }
    auto interface = new DynamicWebViewInterface();
    interfaces.insert({index, interface});
    return interface;
}