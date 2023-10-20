# the name of the target operating system
set(CMAKE_SYSTEM_NAME Darwin)
set(APPLE true)

# which compilers to use for C and C++
set(CMAKE_C_COMPILER x86_64-apple-${OSXCROSS_TARGET}-clang)
set(CMAKE_CXX_COMPILER x86_64-apple-${OSXCROSS_TARGET}-clang++)
set(CMAKE_RANLIB x86_64-apple-${OSXCROSS_TARGET}-ranlib)

set(TARGET x86_64-apple-${OSXCROSS_TARGET}) 
set(CMAKE_C_COMPILER_TARGET ${TARGET})
set(CMAKE_CXX_COMPILER_TARGET ${TARGET})

# here is the target environment located
set(CMAKE_FIND_ROOT_PATH ${OSXCROSS_TARGET_DIR})

# adjust the default behaviour of the FIND_XXX() commands:
# search headers and libraries in the target environment, search
# programs in the host environment
set(CMAKE_FIND_ROOT_PATH_MODE_PROGRAM NEVER)
set(CMAKE_FIND_ROOT_PATH_MODE_LIBRARY ONLY)
set(CMAKE_FIND_ROOT_PATH_MODE_INCLUDE ONLY)