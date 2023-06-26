#include <string>

// We need to set the LD_LIBRARY_PATH because the libraries aren't installed globally
void SetLdLibraryPath(std::string steamroot) {
    std::string ldlibpath = "";
    if (getenv("LD_LIBRARY_PATH") != NULL) {
        // assume that LD_LIBRARY_PATH is previously already formatted correctly (: at end)
        ldlibpath.append(getenv("LD_LIBRARY_PATH"));
    }
#ifdef STEAMSERVICED
    ldlibpath.append(steamroot);
    ldlibpath.append("/ubuntu12_32:");
    
    ldlibpath.append(steamroot);
    ldlibpath.append("/ubuntu12_32/panorama:");
#endif

#ifdef OPENSTEAM_CLIENT
    ldlibpath.append(steamroot);
    ldlibpath.append("/linux64:");
#endif
    setenv("LD_LIBRARY_PATH", ldlibpath.c_str(), 1);

}