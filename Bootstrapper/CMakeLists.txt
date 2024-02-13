cmake_minimum_required(VERSION 3.25)
project(bootstrapper LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_BUILD_TYPE MinSizeRel)

if (MSVC)
    # Requires a "generator expression" to not create an extra Debug folder...
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${NATIVE_OUTPUT_FOLDER}$<0:>) 
endif()

add_executable(opensteam ${PROJECT_SOURCE_DIR}/src/main.cpp)
target_include_directories(opensteam PRIVATE ${PROJECT_SOURCE_DIR}/include)
target_link_libraries(opensteam -L/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.1/libcoreclr.so -L/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.1/libhostpolicy.so -L/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.1/libcoreclr.so)