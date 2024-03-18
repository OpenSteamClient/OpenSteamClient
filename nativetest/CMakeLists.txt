cmake_minimum_required(VERSION 3.25)
project(opensteamclient LANGUAGES CXX C) 

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_BUILD_TYPE Debug)

set(THREADS_PREFER_PTHREAD_FLAG ON)
find_package(Threads REQUIRED)


set(CMAKE_POSITION_INDEPENDENT_CODE ON)
include_directories(${PROJECT_SOURCE_DIR}/include/)

file(GLOB_RECURSE nativetest_sources CONFIGURE_DEPENDS "src/**.cpp" "src/**.c")

add_executable(nativetest ${nativetest_sources})
target_link_libraries(nativetest dl Threads::Threads)