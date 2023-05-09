#pragma once

#include <iostream>
#ifdef DEV_BUILD
#define DEBUG_MSG std::cout
#else
#include <fstream>
extern std::ofstream debug_null_logger;
// Using an empty ofstream is good enough in most cases
#define DEBUG_MSG debug_null_logger
#endif