#!/bin/bash

if [[ -z "${IS_GH_WORKFLOW}" ]]; then
  echo "Not running in Github workflow, aborting"
else
  mkdir build
  cd build
  cmake .. -DREL_BUILD=1
  make -j8
  # .deb
  cpack -G DEB
  # .tar.gz
  cpack -G TGZ
fi
