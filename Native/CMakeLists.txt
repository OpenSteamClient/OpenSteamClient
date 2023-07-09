cmake_minimum_required(VERSION 3.25)
project(opensteamclient LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 20)

set(CMAKE_BUILD_TYPE MinSizeRel)

set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/output)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/output)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/output)

if (BUILD_PLATFORM_TARGET STREQUAL "linux")
    add_executable(reaper ${PROJECT_SOURCE_DIR}/reaper/main.cpp)
    add_executable(steam-launch-wrapper ${PROJECT_SOURCE_DIR}/launchwrapper/main.cpp)

    add_executable(steamserviced ${PROJECT_SOURCE_DIR}/serviced/main.cpp)
    set_target_properties(steamserviced PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")

    # Fake steam service
    add_library(steamservice SHARED ${PROJECT_SOURCE_DIR}/fakeservice/main.cpp)
    set_target_properties(steamservice PROPERTIES PREFIX "")
endif()

if (BUILD_PLATFORM_TARGET STREQUAL "windows")
    
endif()
