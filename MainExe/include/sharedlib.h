#pragma once
#include <netcore/coreclr_delegates.h>

void *load_library(const char_t *);
void *get_export(void *, const char *);