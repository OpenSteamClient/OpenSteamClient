cmake_minimum_required(VERSION 3.25)
project(opensteamclient LANGUAGES CXX C) 

set(CMAKE_ASM_NASM_FLAGS_DEBUG "-g -Fdwarf")
set(CMAKE_ASM_NASM_OBJECT_FORMAT elf64)

set(CMAKE_CXX_STANDARD 14)
set(CMAKE_BUILD_TYPE Debug)

set(THREADS_PREFER_PTHREAD_FLAG ON)
find_package(Threads REQUIRED)


set(CMAKE_POSITION_INDEPENDENT_CODE ON)
include_directories(${PROJECT_SOURCE_DIR}/include/)

file(GLOB_RECURSE nativetest_sources CONFIGURE_DEPENDS "src/**.cpp" "src/**.c")

add_executable(nativetest ${nativetest_sources})
if(MSVC)
  target_compile_options(nativetest PRIVATE /W4 /WX)
else()
  target_compile_options(nativetest PRIVATE -O0 -Wall -Wextra -Wpedantic)
endif()

add_library(steamclient IMPORTED SHARED)
set_property(TARGET steamclient PROPERTY
             IMPORTED_LOCATION "/home/onni/.local/share/OpenSteam/linux64/steamclient.so")

target_link_libraries(nativetest dl Threads::Threads steamclient)