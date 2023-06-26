#pragma once

#include "SteamTypes.h"
#include "steamutl/utlmemory.h"
#include "steamutl/utlvector.h"
#include "steamutl/utlstring.h"
#include "steamutl/utlbuffer.h"


#include "steamutl/utlmap.h"

static bool putOverflow(CUtlBuffer *buf, int nSize) {
    return false;
}

static bool getOverflow(CUtlBuffer *buf, int nSize) {
    return false;
}

