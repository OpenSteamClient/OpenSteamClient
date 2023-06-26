#include <string>

#pragma once

// We need to set the LD_LIBRARY_PATH because the libraries aren't installed globally
void SetLdLibraryPath(std::string steamroot);