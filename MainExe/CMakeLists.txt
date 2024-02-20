cmake_minimum_required(VERSION 3.25)
project(mainexe LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 20)

include("cmake/idesupport.cmake")

set(NetHostOutputPath "" CACHE STRING "")

if (BUILD_ARCH STREQUAL "x64" OR DEFINED IDE_BUILD)
    file(GLOB_RECURSE mainexe_sources CONFIGURE_DEPENDS "src/*.cpp")

    add_executable(opensteam ${mainexe_sources})
    if (BUILD_PLATFORM_TARGET STREQUAL "win")
        target_link_options(opensteam PRIVATE -municode)
    endif()

    target_link_directories(opensteam PRIVATE ${NetHostOutputPath}/${BUILD_PLATFORM_TARGET}-${BUILD_ARCH})
    target_include_directories(opensteam PUBLIC ${PROJECT_SOURCE_DIR}/include)
    target_link_libraries(opensteam nethost)
endif()