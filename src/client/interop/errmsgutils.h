#include <string>
#include "includesteamworks.h"

#pragma once

class ErrMsgUtils
{
private:

public:
    static std::string GetErrorMessageFromEAppUpdateError(EAppUpdateError error);
    static std::string GetErrorMessageFromEResult(EResult eResult);
};