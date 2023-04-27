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

# We don't use CMAKE_MODULE_PATH anywhere else so this is fine
set(CMAKE_MODULE_PATH ${ECM_MODULE_PATH})

# App .desktop and logo
include(KDEInstallDirs)
install(FILES ${CMAKE_CURRENT_SOURCE_DIR}/package/opensteamclient.desktop DESTINATION ${KDE_INSTALL_APPDIR})
install(FILES ${CMAKE_CURRENT_SOURCE_DIR}/package/opensteam.svg DESTINATION ${KDE_INSTALL_ICONDIR}/hicolor/scalable/apps)

cmake_policy(SET CMP0060 NEW)
cmake_policy(SET CMP0003 NEW)

# Qt modules
find_package(Qt6 COMPONENTS Core Widgets Network REQUIRED)

# libarchive for the updater (should always be installed atleast as dependency of cmake)
find_package(LibArchive REQUIRED)

# Various messages need protobuf, maybe the client will use protobuf only in the future?
find_package(Protobuf REQUIRED)

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
file(GLOB_RECURSE opensteamclient_sources CONFIGURE_DEPENDS "src/client/*.cpp")
file(GLOB_RECURSE common_sources CONFIGURE_DEPENDS "src/common/*.cpp")
file(GLOB_RECURSE steamutl_sources CONFIGURE_DEPENDS "include/opensteamworks/steamutl/*.cpp")

remove_definitions(linux)
add_executable(opensteam ${common_sources} ${steamutl_sources} ${opensteamclient_sources})

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


target_compile_definitions(steamserviced PRIVATE
            # flag sets behaviour of shared code
            STEAMSERVICED=1
)

# 32-bit components
set_target_properties(steamserviced PROPERTIES COMPILE_FLAGS "-m32" LINK_FLAGS "-m32")

target_include_directories(opensteam PRIVATE ${PROJECT_SOURCE_DIR}/include)
target_include_directories(steamserviced PRIVATE ${PROJECT_SOURCE_DIR}/include)

# Improves build performance
set_target_properties(Qt6::Core PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")
set_target_properties(Qt6::Widgets PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")
set_target_properties(Qt6::Network PROPERTIES MAP_IMPORTED_CONFIG_COVERAGE "RELEASE")

target_link_libraries(opensteam -lqrencode OpenSSL::SSL nlohmann_json::nlohmann_json Qt6::Widgets Qt6::Core Qt6::Network ${LibArchive_LIBRARIES} ${Protobuf_LIBRARIES} ${CURL_LIBRARIES})
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

# DEB files

SET(CPACK_DEBIAN_PACKAGE_MAINTAINER "Onni Kukkonen")
include(CPack)
