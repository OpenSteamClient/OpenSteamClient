// .NET headers
#include <netbridge.h>


#if defined(_WIN32)
int __cdecl wmain(int argc, wchar_t *argv[])
#else
int main(int argc, char *argv[])
#endif
{
    auto bridge = new CNetBridge(argc, argv);
    return bridge->Run();
}