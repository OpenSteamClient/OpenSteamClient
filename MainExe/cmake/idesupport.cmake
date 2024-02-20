if (NOT DEFINED IDE_BUILD)
    set(CMAKE_BUILD_TYPE Release)
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
        set(BUILD_PLATFORM_TARGET "win")
    else()
        message( FATAL_ERROR "Couldn't set up Natives IDE support. Didn't detect your platform to be linux, osx or windows." )
    endif()
    set(NATIVE_OUTPUT_FOLDER "${PROJECT_SOURCE_DIR}/IDEBuild")
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