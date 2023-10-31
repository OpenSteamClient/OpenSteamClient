cmake_minimum_required(VERSION 3.25)
project(opensteamclient LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 20)

if (NOT DEFINED IDE_BUILD)
    set(CMAKE_BUILD_TYPE MinSizeRel)
    if ("${BUILD_PLATFORM_TARGET}" STREQUAL "") 
        message( FATAL_ERROR "BUILD_PLATFORM_TARGET not set. Do not build this project manually outside of dotnet build." )
    endif()
    if ("${NATIVE_OUTPUT_FOLDER}" STREQUAL "") 
        message( FATAL_ERROR "NATIVE_OUTPUT_FOLDER not set. Do not build this project manually outside of dotnet build." )
    endif()
else()
    if(UNIX AND NOT APPLE)
        set(BUILD_PLATFORM_TARGET "linux")
    elseif(APPLE) 
        set(BUILD_PLATFORM_TARGET "osx")
    elseif(WIN32)
        set(BUILD_PLATFORM_TARGET "windows")
    else()
        message( FATAL_ERROR "Couldn't set up Natives IDE support. Didn't detect your platform to be linux, osx or windows." )
    endif()
    set(NATIVE_OUTPUT_FOLDER "${PROJECT_SOURCE_DIR}/Build/IDE/Natives/IDE")
endif()

# Only for linux, since MSVC needs an entirely different compiler for 32-bit compiles
function(set_ide_32bit_build targetname)
    if (DEFINED IDE_BUILD)
        set_target_properties(${targetname} PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")
    endif()
endfunction()

# Gosh Microsoft, you really don't seem to get stuff right
if (MSVC)
    # Requires a "generator expression" to not create an extra Debug folder...
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${NATIVE_OUTPUT_FOLDER}$<0:>)
else()
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${NATIVE_OUTPUT_FOLDER})
endif()

set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${NATIVE_OUTPUT_FOLDER})
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${NATIVE_OUTPUT_FOLDER})

file(GLOB_RECURSE miniutl_sources CONFIGURE_DEPENDS "MiniUTL/*.cpp")
file(GLOB_RECURSE htmlhost_sources CONFIGURE_DEPENDS "htmlhost/*.cpp")

# Common SDL config options
set(SDL_TEST_LIBRARY OFF)

# Stop .exp, .lib, .dll.a and various other unneeded files from manifesting
# (Can't do this, it breaks the build due to MingW depending on a .lib existing even for a simple exe file. WTF?)
# Let's just filter these out in the bootstrap phase.
# unset(CMAKE_IMPORT_LIBRARY_SUFFIX)

if (BUILD_PLATFORM_TARGET STREQUAL "linux")
    if (BUILD_BITS STREQUAL "64" OR DEFINED IDE_BUILD)
        add_executable(reaper ${PROJECT_SOURCE_DIR}/reaper/main.cpp)
        add_executable(steam-launch-wrapper ${PROJECT_SOURCE_DIR}/launchwrapper/main.cpp)

        # 64-bit steam service stub
        add_library(steamservice SHARED ${PROJECT_SOURCE_DIR}/fakeservice/main.cpp)
        set_target_properties(steamservice PROPERTIES PREFIX "")

        add_library(bootstrappershim64 SHARED ${PROJECT_SOURCE_DIR}/bootstrappershim/main.cpp)

        add_subdirectory(SDL)
        add_subdirectory(SDL_ttf)
    endif()

    if (BUILD_BITS STREQUAL "32" OR DEFINED IDE_BUILD)
        # Needed for VAC, Proton
        add_executable(steamserviced ${PROJECT_SOURCE_DIR}/serviced/main.cpp)
        set_ide_32bit_build(steamserviced)

        # htmlhost hosts steamwebhelper and chromehtml if the user wants it. Some games use it to display MOTDs
        add_executable(htmlhost ${htmlhost_sources} ${miniutl_sources})
        target_include_directories(htmlhost PRIVATE "MiniUTL")
        set_ide_32bit_build(htmlhost)

        # We hook getpid for htmlhost (on linux with LD_PRELOAD) so it thinks our steam process is the steam process and not itself (not really meant to be hosted)
        add_library(htmlhost_fakepid SHARED ${PROJECT_SOURCE_DIR}/htmlhost_fakepid/main.cpp)
        set_ide_32bit_build(htmlhost_fakepid)

        add_library(bootstrappershim32 SHARED ${PROJECT_SOURCE_DIR}/bootstrappershim/main.cpp)
        set_ide_32bit_build(bootstrappershim32)
    endif()
endif()

if (BUILD_PLATFORM_TARGET STREQUAL "windows")
    if (BUILD_BITS STREQUAL "64" OR DEFINED IDE_BUILD)
        set(SDL3TTF_VENDORED ON)
        add_subdirectory(SDL)
        add_subdirectory(SDL_ttf)
    endif()

    if (BUILD_BITS STREQUAL "32" OR DEFINED IDE_BUILD)
        # Needed for VAC
        add_executable(steamserviced ${PROJECT_SOURCE_DIR}/serviced/main.cpp)

        # htmlhost hosts steamwebhelper and chromehtml if the user wants it. Some games use it to display MOTDs
        add_executable(htmlhost ${htmlhost_sources} ${miniutl_sources})
        target_include_directories(htmlhost PRIVATE "MiniUTL")
    endif()
endif()

if (BUILD_PLATFORM_TARGET STREQUAL "macos")
    if (BUILD_BITS STREQUAL "64" OR DEFINED IDE_BUILD)
        # MacOS steam has the luxury of having a 64-bit steamservice
        # add_executable(steamserviced ${PROJECT_SOURCE_DIR}/serviced/main.cpp)

    endif()

    if (BUILD_BITS STREQUAL "32" OR DEFINED IDE_BUILD)
        # No 32-bit natives on macos
    endif()

    if (BUILD_BITS STREQUAL "ARM" OR DEFINED IDE_BUILD)
        add_executable(steam-launch-wrapper ${PROJECT_SOURCE_DIR}/launchwrapper/main.cpp)
    endif()
endif()

if (NOT DEFINED IDE_BUILD)
    # Save build timestamp to file
    string(TIMESTAMP UNIX_TIMESTAMP "%s") 
    file(WRITE ${NATIVE_OUTPUT_FOLDER}/../build_timestamp ${UNIX_TIMESTAMP})
endif()
