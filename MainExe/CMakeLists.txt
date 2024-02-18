cmake_minimum_required(VERSION 3.25)
project(mainexe LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_BUILD_TYPE Debug)

include("CustomBuildTask/custombuildtaskcommon.cmake")

if (BUILD_BITS STREQUAL "x64" OR DEFINED IDE_BUILD)
    file(GLOB_RECURSE mainexe_sources CONFIGURE_DEPENDS "src/*.cpp")

    add_executable(opensteam ${mainexe_sources})
    if (BUILD_PLATFORM_TARGET STREQUAL "windows")
        target_link_options(opensteam PRIVATE -municode)
        target_link_directories(opensteam PRIVATE "managedartifacts/publish/managed/release_win-x64/")
    endif()

    if (BUILD_PLATFORM_TARGET STREQUAL "macos")
        target_link_directories(opensteam PRIVATE "managedartifacts/publish/managed/release_osx-x64/")
    endif()

    if (BUILD_PLATFORM_TARGET STREQUAL "linux")
        target_link_directories(opensteam PRIVATE "managedartifacts/publish/managed/release_linux-x64/")
    endif()


    target_include_directories(opensteam PUBLIC ${PROJECT_SOURCE_DIR}/include)
    target_link_libraries(opensteam nethost)
endif()

if (NOT DEFINED IDE_BUILD)
    # Save build timestamp to file
    string(TIMESTAMP UNIX_TIMESTAMP "%s")
    file(WRITE ${NATIVE_OUTPUT_FOLDER}/../build_timestamp ${UNIX_TIMESTAMP})
endif()
