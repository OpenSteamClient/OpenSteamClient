#!/bin/bash
set -e
set -o pipefail

function build_platform () {
    rm -rf output/$PLATFORM_TARGET
    mkdir -p output/$PLATFORM_TARGET
    rm -rf build
    mkdir build
    cd build
    cmake .. -DBUILD_PLATFORM_TARGET=$PLATFORM_TARGET
    cmake --build .
}

mkdir -p build


if [ -n "$ALLBUILD" ]; then
  # Build all platforms
  BUILD_PLATFORM_TARGET=linux build_platform
  BUILD_PLATFORM_TARGET=windows build_platform
  BUILD_PLATFORM_TARGET=osx build_platform
else
    if [ -n "$PLATFORM_TARGET" ]; then
        build_platform
    else
        echo "PLATFORM_TARGET not set."
    fi
fi


