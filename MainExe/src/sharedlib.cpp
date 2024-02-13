#include <sharedlib.h>
#include <assert.h>

#ifdef _WIN32
    #include <windows.h>

    void *load_library(const char_t *path)
    {
        HMODULE h = ::LoadLibraryW(path);
        assert(h != nullptr);
        return (void*)h;
    }
    void *get_export(void *h, const char *name)
    {
        void *f = (void*)::GetProcAddress((HMODULE)h, name);
        assert(f != nullptr);
        return f;
    }
#else
    #include <dlfcn.h>

    void *load_library(const char_t *path)
    {
        void *h = dlopen(path, RTLD_LAZY | RTLD_LOCAL);
        assert(h != nullptr);
        return h;
    }
    void *get_export(void *h, const char *name)
    {
        void *f = dlsym(h, name);
        assert(f != nullptr);
        return f;
    }
#endif