# Copyright 2016 Google Inc. All rights reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# We only need Protobuf_generate_cpp from FindProtobuf, and we are going to
# override the rest with ExternalProject version.
include (FindProtobuf)

set(PROTOBUF_TARGET external.protobuf)
set(PROTOBUF_INSTALL_DIR ${CMAKE_CURRENT_BINARY_DIR}/${PROTOBUF_TARGET})

set(PROTOBUF_INCLUDE_DIRS ${PROTOBUF_INSTALL_DIR}/include)
include_directories(${PROTOBUF_INCLUDE_DIRS})
include_directories(${CMAKE_CURRENT_BINARY_DIR})

# The protobuf libraries are named differently in a debug configuration BUT only on Windows. WTF?
IF(CMAKE_BUILD_TYPE MATCHES Debug)
  set(PROTOBUF_LIBRARIES protobufd)
ELSE()
  set(PROTOBUF_LIBRARIES protobuf)
ENDIF()

foreach(lib ${PROTOBUF_LIBRARIES})
  if (MSVC)
    set(LIB_PATH ${PROTOBUF_INSTALL_DIR}/lib/lib${lib}.lib)
  else()
    set(LIB_PATH ${PROTOBUF_INSTALL_DIR}/lib/lib${lib}.a)
  endif()
  list(APPEND PROTOBUF_BUILD_BYPRODUCTS ${LIB_PATH})
  add_library(${lib} STATIC IMPORTED)
  set_property(TARGET ${lib} PROPERTY IMPORTED_LOCATION
               ${LIB_PATH})
  add_dependencies(${lib} ${PROTOBUF_TARGET})
endforeach(lib)

set(PROTOBUF_PROTOC_EXECUTABLE ${PROTOBUF_INSTALL_DIR}/bin/protoc)
list(APPEND PROTOBUF_BUILD_BYPRODUCTS ${PROTOBUF_PROTOC_EXECUTABLE})

if(${CMAKE_VERSION} VERSION_LESS "3.10.0")
  set(PROTOBUF_PROTOC_TARGET protoc)
else()
  set(PROTOBUF_PROTOC_TARGET protobuf::protoc)
endif()


if(NOT TARGET ${PROTOBUF_PROTOC_TARGET})
  add_executable(${PROTOBUF_PROTOC_TARGET} IMPORTED)
endif()

set_property(TARGET ${PROTOBUF_PROTOC_TARGET} PROPERTY IMPORTED_LOCATION
             ${PROTOBUF_PROTOC_EXECUTABLE})
add_dependencies(${PROTOBUF_PROTOC_TARGET} ${PROTOBUF_TARGET})

include (ExternalProject)
ExternalProject_Add(${PROTOBUF_TARGET}
    PREFIX ${PROTOBUF_TARGET}
    GIT_REPOSITORY https://github.com/google/protobuf.git
    GIT_TAG v3.15.3
    UPDATE_COMMAND ""
    CONFIGURE_COMMAND ${CMAKE_COMMAND} ${PROTOBUF_INSTALL_DIR}/src/${PROTOBUF_TARGET}/cmake/CMakeLists.txt
        -G${CMAKE_GENERATOR}
        -DCMAKE_INSTALL_PREFIX=${PROTOBUF_INSTALL_DIR}
        -DCMAKE_INSTALL_LIBDIR=lib
        #-DCMAKE_BUILD_TYPE=${CMAKE_BUILD_TYPE}
        # Force this to create smaller executables
        -DCMAKE_BUILD_TYPE=MinSizeRel
        -DCMAKE_POSITION_INDEPENDENT_CODE=ON
        -DCMAKE_C_FLAGS=${PROTOBUF_CFLAGS}
        -DCMAKE_CXX_FLAGS=${PROTOBUF_CXXFLAGS}
        -DCMAKE_CXX_STANDARD=${CMAKE_CXX_STANDARD}
        -DCMAKE_TOOLCHAIN_FILE=${CMAKE_TOOLCHAIN_FILE}
        -Dprotobuf_BUILD_TESTS=OFF
        -Dprotobuf_BUILD_PROTOC_BINARIES=ON
        -Dprotobuf_BUILD_SHARED_LIBS=OFF
        -Dprotobuf_BUILD_STATIC_LIBS=OFF
        -Dprotobuf_WITH_ZLIB=OFF
        -Dprotobuf_BUILD_CONFORMANCE=OFF
    BUILD_BYPRODUCTS ${PROTOBUF_BUILD_BYPRODUCTS}
)

set(CMAKE_LINK_GROUP_USING_cross_refs_SUPPORTED TRUE)
set(CMAKE_LINK_GROUP_USING_cross_refs
  "LINKER:--start-group"
  "LINKER:--end-group"
)
# cmake 3.7 uses Protobuf_ when 3.5 PROTOBUF_ prefixes.
set(Protobuf_INCLUDE_DIRS ${PROTOBUF_INCLUDE_DIRS})
set(Protobuf_LIBRARIES "$<LINK_GROUP:cross_refs,${PROTOBUF_LIBRARIES}>")
set(Protobuf_PROTOC_EXECUTABLE ${PROTOBUF_PROTOC_EXECUTABLE})