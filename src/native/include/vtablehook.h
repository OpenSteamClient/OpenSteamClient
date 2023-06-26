/*
The MIT License (MIT)

Copyright (c) 2013 Thordin

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#include <stdint.h>

#ifdef WIN32
#include <Windows.h>
#elif __linux__
#include <unistd.h>
#include <sys/mman.h>
int vtablehook_pagesize = sysconf(_SC_PAGE_SIZE);
int vtablehook_pagemask = ~(vtablehook_pagesize-1);
#endif

int vtablehook_unprotect(void* region) {
#ifdef WIN32
        MEMORY_BASIC_INFORMATION mbi;
        VirtualQuery((LPCVOID)region, &mbi, sizeof(mbi));
        VirtualProtect(mbi.BaseAddress, mbi.RegionSize, PAGE_READWRITE, &mbi.Protect);
        return mbi.Protect;
#elif __linux__
        mprotect((void*) ((intptr_t)region & vtablehook_pagemask), vtablehook_pagesize, PROT_READ|PROT_WRITE|PROT_EXEC);
        return PROT_READ|PROT_EXEC;
#endif
}

void vtablehook_protect(void* region, int protection) {
#ifdef WIN32
        MEMORY_BASIC_INFORMATION mbi;
        VirtualQuery((LPCVOID)region, &mbi, sizeof(mbi));
        VirtualProtect(mbi.BaseAddress, mbi.RegionSize, protection, &mbi.Protect);
#elif __linux__
        mprotect((void*) ((intptr_t)region & vtablehook_pagemask), vtablehook_pagesize, protection);
#endif
}

/*
* instance: pointer to an instance of a class
* hook: function to overwrite with
* offset: 0 = method 1, 1 = method 2 etc...
* return: original function
*/

void* vtablehook_hook(void* instance, void* hook, int offset) {
        intptr_t vtable = *((intptr_t*)instance);
        intptr_t entry = vtable + sizeof(intptr_t) * offset;
        intptr_t original = *((intptr_t*) entry);

        int original_protection = vtablehook_unprotect((void*)entry);
        *((intptr_t*)entry) = (intptr_t)hook;
        vtablehook_protect((void*)entry, original_protection);

        return (void*)original;
}