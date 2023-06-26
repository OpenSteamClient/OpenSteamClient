cmake_minimum_required(VERSION 3.25)
project(opensteamclient LANGUAGES CXX) 

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Optimization should be limited to O2 to avoid bugs and to be Arch-friendly
set(CMAKE_CXX_FLAGS_RELEASE "-O2")

# Arch linux wants "FULL RELRO"
set(LINK_OPTIONS "-Wl,-z,relro,-z,now")

# Build dir
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${PROJECT_BINARY_DIR})
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${PROJECT_BINARY_DIR})

# Extra cmake modules
find_package(ECM REQUIRED NO_MODULE)

# Qt modules

find_package(Qt6 COMPONENTS Core Widgets Network WebEngineWidgets QuickWidgets QUIET)

if (Qt6_FOUND) 
else()
    message(STATUS "Qt6 is missing WebEngineWidgets/QuickWidgets, webview support won't be built!")
    set(NOWEBVIEW true)
endif()


if (NOWEBVIEW)
    find_package(Qt6 REQUIRED COMPONENTS Core Widgets Network)
endif()

# App .desktop and logo (now hardcoded since using extra cmake modules breaks webengine)
install(FILES ${CMAKE_CURRENT_SOURCE_DIR}/package/opensteamclient.desktop DESTINATION /usr/share/applications)
install(FILES ${CMAKE_CURRENT_SOURCE_DIR}/package/opensteam.svg DESTINATION /usr/share/icons/hicolor/scalable/apps)

cmake_policy(SET CMP0060 NEW)
cmake_policy(SET CMP0003 NEW)

# libarchive for the updater (should always be installed atleast as dependency of cmake)
find_package(LibArchive REQUIRED)

# Various messages need protobuf, maybe the client will use protobuf only in the future?
# Static building unfortunately needs you to build protobuf from source
if (REL_BUILD OR STATICPROTOBUF)
include("cmake/protobuf_external.cmake")
else()
find_package(Protobuf REQUIRED)
endif()

# qrencode for the login QR code
# Static building unfortunately needs you to build qrencode from source
if (REL_BUILD OR STATICQRENCODE)
include("cmake/qrencode_external.cmake")
else()
find_library(qrencode_found NAMES qrencode REQUIRED)
set(qrencode_LIBRARIES ${qrencode_found})
endif()

# libcurl for downloading files
find_package(CURL REQUIRED)

# nlohmann json is good
find_package(nlohmann_json 3.2.0 REQUIRED)

# openssl is annoying but the only crypto library ever made
find_package(OpenSSL REQUIRED)

#TODO: there has to be a better way to do this
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_AUTOUIC ON)

# Set this variable off to not generate moc_predefs.h
# We need this so our protobufs can define linux()
# Because for some reason Qt devs thought it would be a good idea to #define linux 1
# NEVERMIND, this doesn't compile for some reason, see workaround below (Force include)
# set(CMAKE_AUTOMOC_COMPILER_PREDEFINES OFF)


if(CMAKE_VERSION VERSION_LESS "3.7.0")
    set(CMAKE_INCLUDE_CURRENT_DIR ON)
endif()


#set (FIND_LIBRARY_USE_LIB64_PATHS false)
if (NOWEBVIEW)
else()
file(GLOB_RECURSE dynamicwebview_sources CONFIGURE_DEPENDS "src/dynamicwebview/*.cpp")
endif()
file(GLOB_RECURSE opensteamclient_sources CONFIGURE_DEPENDS "src/client/*.cpp")
file(GLOB_RECURSE common_sources CONFIGURE_DEPENDS "src/common/*.cpp")
file(GLOB_RECURSE steamutl_sources CONFIGURE_DEPENDS "include/opensteamworks/steamutl/*.cpp")

# Get the git commit hash
execute_process(
    COMMAND git log -1 --format=%h
    WORKING_DIRECTORY ${CMAKE_CURRENT_LIST_DIR}
    OUTPUT_VARIABLE GIT_COMMIT_HASH
    OUTPUT_STRIP_TRAILING_WHITESPACE
)

# Get the git branch
execute_process(
    COMMAND git rev-parse --abbrev-ref HEAD
    WORKING_DIRECTORY ${CMAKE_CURRENT_LIST_DIR}
    OUTPUT_VARIABLE GIT_BRANCH_NAME
    OUTPUT_STRIP_TRAILING_WHITESPACE
)

# Save the git commit hash to the generated file
configure_file (
    ${PROJECT_SOURCE_DIR}/src/generated/gitinfo.h.in
    ${PROJECT_BINARY_DIR}/gitinfo.h
)

remove_definitions(linux)
add_executable(opensteam ${common_sources} ${steamutl_sources} ${opensteamclient_sources})

# Export all symbols from opensteam
# Used by steamclient.so to get info about the bootstrapper
# Would export only the needed ones, but it isn't an option...
set_property(TARGET opensteam PROPERTY ENABLE_EXPORTS ON)

# Force include protobufhack.cpp, which will #undef linux
target_compile_options(opensteam PUBLIC -include protobufhack.h)

#TODO: there has to be a better way to do this
set(CMAKE_AUTOMOC OFF)
set(CMAKE_AUTORCC OFF)
set(CMAKE_AUTOUIC OFF)

# Include the protobufs in the build
#NOTE: not using GLOB_RECURSE because rendermessages.proto breaks build
file(GLOB protobufs CONFIGURE_DEPENDS "ext/SteamDatabase/Protobufs/steam/*.proto")
protobuf_generate_cpp(PROTO_SRCS PROTO_HDRS ${protobufs})

target_include_directories(opensteam PRIVATE ${CMAKE_CURRENT_BINARY_DIR})
target_sources(opensteam PUBLIC ${PROTO_SRCS} ${PROTO_HDRS})

add_executable(reaper ${PROJECT_SOURCE_DIR}/src/utils/reaper/main.cpp)
add_executable(steam-launch-wrapper ${PROJECT_SOURCE_DIR}/src/utils/launchwrapper/main.cpp)
add_executable(steamserviced ${common_sources} ${PROJECT_SOURCE_DIR}/src/service/main.cpp)
add_library(mocksteamservice SHARED ${PROJECT_SOURCE_DIR}/src/service/fakeservice.cpp)

# Dynamic web view
# Everyone might not need this, so this is optional
if (NOWEBVIEW)
else()
add_library(dynamicwebview SHARED ${dynamicwebview_sources})
endif()

# flags

if (REL_BUILD)

# Release build
message("Building release build")

target_compile_definitions(opensteam PRIVATE 
    STEAMWORKS_CLIENT_INTERFACES=1
    OPENSTEAM_CLIENT=1
)

else()

# Dev build
message("Building dev build")

target_compile_definitions(opensteam PRIVATE 
    # Silences warnings
    STEAMWORKS_CLIENT_INTERFACES=1

    # flag sets behaviour of shared code
    OPENSTEAM_CLIENT=1

    # copies files over every run, enables debug logging, lots of other stuff. remove before building release
    DEV_BUILD=1 

    # Re-parses all protobuf messages sent, remove before building release
    PROTOMSG_REPARSE=1

    # Enable debug logging all protomsg activities (sending, receiving, de-(serialization))
    PRINT_PROTOMSG=1
)

endif()

if (NOWEBVIEW) 
# This isn't ideal, ideally we would use some sort of add_target_compile_definitions
add_compile_definitions(NOWEBVIEW)
endif()

target_compile_definitions(steamserviced PRIVATE
    # flag sets behaviour of shared code
    STEAMSERVICED=1
)

# 32-bit components
set_target_properties(steamserviced PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")

# We add Qt6WebEngineWidgets to the include but we don't link to them
# This is so we can use QtWebEngine without it eating RAM by loading it dynamically only when needed
target_include_directories(opensteam PRIVATE ${PROJECT_SOURCE_DIR}/include ${Qt6WebEngineWidgets_INCLUDE_DIRS})
target_include_directories(steamserviced PRIVATE ${PROJECT_SOURCE_DIR}/include)

# Improves build performance
set_target_properties(Qt6::Core PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")
set_target_properties(Qt6::Widgets PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")
set_target_properties(Qt6::Network PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")

if (NOWEBVIEW)
else()
set_target_properties(Qt6::QuickWidgets PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")
set_target_properties(Qt6::WebEngineWidgets PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")
endif()

target_link_libraries(opensteam ${qrencode_LIBRARIES} OpenSSL::SSL nlohmann_json::nlohmann_json Qt6::Widgets Qt6::Core Qt6::Network ${LibArchive_LIBRARIES} ${Protobuf_LIBRARIES} ${CURL_LIBRARIES})

if (NOWEBVIEW)
else()
target_link_libraries(dynamicwebview PUBLIC Qt6::Widgets Qt6::Core Qt6::WebEngineWidgets Qt6::QuickWidgets)
endif()

target_link_libraries(reaper)
target_link_libraries(steam-launch-wrapper)
target_link_libraries(steamserviced)
target_link_libraries(mocksteamservice)

install(TARGETS opensteam DESTINATION lib/opensteam/)
install(PROGRAMS ${CMAKE_CURRENT_SOURCE_DIR}/package/opensteam.sh RENAME opensteam DESTINATION bin)
install(TARGETS reaper DESTINATION lib/opensteam/updater_files/)
install(TARGETS steam-launch-wrapper DESTINATION lib/opensteam/updater_files/)
install(TARGETS steamserviced DESTINATION lib/opensteam/updater_files/)
install(TARGETS mocksteamservice DESTINATION lib/opensteam/updater_files/)

if (NOWEBVIEW)
else()
install(TARGETS dynamicwebview DESTINATION lib/opensteam/updater_files/)
endif()

# DEB files

# //TODO: this is not the correct naming scheme for debian
set(CPACK_PACKAGE_NAME "opensteam-git")

# What happens if this is not a git clone?
SET(CPACK_PACKAGE_VERSION ${GIT_COMMIT_HASH})

# These descriptions are not auto-parsed from the readme. Would like if changing the readme would update this too, otherwise these will most likely be forgotten about.
set(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Partially open-source Steam client for Linux.")
set(CPACK_PACKAGE_DESCRIPTION "OpenSteamClient is an open-source alternative frontend to the official steamclient binary.")

set(CPACK_PACKAGE_VENDOR "Onni Kukkonen")
set(CPACK_PACKAGE_CONTACT "Contact through Github at <20PercentRendered/opensteam>, security issues at <onni.kukkonen77@gmail.com>")	
set(CPACK_DEBIAN_PACKAGE_DESCRIPTION_SUMMARY ${CPACK_PACKAGE_DESCRIPTION_SUMMARY})
set(CPACK_DEBIAN_PACKAGE_DESCRIPTION ${CPACK_PACKAGE_DESCRIPTION})
set(CPACK_DEBIAN_PACKAGE_SECTION "games")

SET(CPACK_DEBIAN_PACKAGE_MAINTAINER "Onni Kukkonen")


# TGZ files

# Set by default to 1, which uses 1 core only. Use 0 for all cores. (This should be a default...)
set(CPACK_ARCHIVE_THREADS 0)

# This needs to be set since otherwise it'll try and install the files into the root folder (why is this not default...)
SET(CPACK_SET_DESTDIR "ON")


include(CPack)
