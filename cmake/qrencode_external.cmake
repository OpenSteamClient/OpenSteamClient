set(qrencode_TARGET external.qrencode)
set(qrencode_INSTALL_DIR ${CMAKE_CURRENT_BINARY_DIR}/${qrencode_TARGET})

set(qrencode_INCLUDE_DIRS ${qrencode_INSTALL_DIR}/include)
include_directories(${qrencode_INCLUDE_DIRS})
include_directories(${CMAKE_CURRENT_BINARY_DIR})

set(qrencode_LIBRARIES qrencode)

foreach(lib ${qrencode_LIBRARIES})
  if (MSVC)
    set(LIB_PATH ${qrencode_INSTALL_DIR}/lib/lib${lib}.lib)
  else()
    set(LIB_PATH ${qrencode_INSTALL_DIR}/lib/lib${lib}.a)
  endif()
  list(APPEND qrencode_BUILD_BYPRODUCTS ${LIB_PATH})
  add_library(${lib} STATIC IMPORTED)
  set_property(TARGET ${lib} PROPERTY IMPORTED_LOCATION
               ${LIB_PATH})
  add_dependencies(${lib} ${qrencode_TARGET})
endforeach(lib)

include (ExternalProject)
ExternalProject_Add(${qrencode_TARGET}
    PREFIX ${qrencode_TARGET}
    GIT_REPOSITORY https://github.com/fukuchi/libqrencode.git
    GIT_TAG v4.1.1
    UPDATE_COMMAND ""
    CONFIGURE_COMMAND ${CMAKE_COMMAND} ${qrencode_INSTALL_DIR}/src/${qrencode_TARGET}
        -G${CMAKE_GENERATOR}
        -DCMAKE_INSTALL_PREFIX=${qrencode_INSTALL_DIR}
        -DCMAKE_INSTALL_LIBDIR=lib
        -DCMAKE_BUILD_TYPE=${CMAKE_BUILD_TYPE}
        -DCMAKE_POSITION_INDEPENDENT_CODE=ON
        -DCMAKE_CXX_STANDARD=${CMAKE_CXX_STANDARD}
        -DWITH_TOOLS=NO
    BUILD_BYPRODUCTS ${qrencode_BUILD_BYPRODUCTS}
)