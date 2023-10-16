cmake_minimum_required(VERSION 3.25)
project(opensteamclient LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 20)

if (NOT DEFINED IDE_BUILD)
    set(CMAKE_BUILD_TYPE MinSizeRel)
    if ("${BUILD_PLATFORM_TARGET}" STREQUAL "") 
        message( FATAL_ERROR "BUILD_PLATFORM_TARGET not set. Do not build this project outside of dotnet build." )
    endif()
    if ("${NATIVE_OUTPUT_FOLDER}" STREQUAL "") 
        message( FATAL_ERROR "NATIVE_OUTPUT_FOLDER not set. Do not build this project outside of dotnet build." )
    endif()
else()
    # Linux
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

if (BUILD_PLATFORM_TARGET STREQUAL "linux")
    add_executable(reaper ${PROJECT_SOURCE_DIR}/reaper/main.cpp)
    add_executable(steam-launch-wrapper ${PROJECT_SOURCE_DIR}/launchwrapper/main.cpp)

    add_executable(steamserviced ${PROJECT_SOURCE_DIR}/serviced/main.cpp)
    set_target_properties(steamserviced PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")

    # htmlhost hosts steamwebhelper and chromehtml if the user wants it. Some games use it to display MOTDs
    file(GLOB_RECURSE htmlhost_sources CONFIGURE_DEPENDS "htmlhost/*.cpp")

    add_executable(htmlhost ${htmlhost_sources} ${miniutl_sources})
    set_target_properties(htmlhost PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")
    target_include_directories(htmlhost PRIVATE "MiniUTL")

    # We hook getpid for htmlhost so it thinks our steam process is the steam process and not itself (not really meant to be hosted)
    add_library(htmlhost_fakepid SHARED ${PROJECT_SOURCE_DIR}/htmlhost_fakepid/main.cpp)
    set_target_properties(htmlhost_fakepid PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")

    # Fake steam service
    add_library(steamservice SHARED ${PROJECT_SOURCE_DIR}/fakeservice/main.cpp)
    set_target_properties(steamservice PROPERTIES PREFIX "")

    add_library(bootstrappershim32 SHARED ${PROJECT_SOURCE_DIR}/bootstrappershim/main.cpp)
    set_target_properties(bootstrappershim32 PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")

    add_library(bootstrappershim64 SHARED ${PROJECT_SOURCE_DIR}/bootstrappershim/main.cpp)
endif()

if (BUILD_PLATFORM_TARGET STREQUAL "windows")
    
endif()

if (NOT DEFINED IDE_BUILD)
    # Save build timestamp to file
    string(TIMESTAMP UNIX_TIMESTAMP "%s") 
    file(WRITE ${NATIVE_OUTPUT_FOLDER}/../build_timestamp ${UNIX_TIMESTAMP})
endif()
