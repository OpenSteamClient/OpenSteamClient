#ifdef _WIN32
#define EXPORT extern "C" __declspec(dllexport)
#endif

#if __linux__
#define EXPORT extern "C"
#endif