# Maintainer: Onni Kukkonen <onni.kukkonen77@gmail.com>

pkgname=opensteam-git
pkgver=0
pkgrel=2
pkgdesc="Partially open-source alternative to the Steam Client application"
arch=('x86_64')
url="https://github.com/20PercentRendered/opensteamclient"
license=('MIT')

depends=(
    'qt6-base>=6.5'
    'libarchive>=3.6'
    'qrencode>=4.1'
    'openssl>=3.0.8'
    'protobuf>=21.12'
    'hicolor-icon-theme'
    'gcc-libs'
    'curl'
    'lib32-gcc-libs'
)

makedepends=(
    'git'
    'cmake'
    'nlohmann-json'
    'extra-cmake-modules'
)

source=("git+https://github.com/20PercentRendered/opensteamclient.git"
        "git+https://github.com/SteamDatabase/Protobufs.git")
sha256sums=('SKIP' 'SKIP')

pkgver() {
  cd "opensteamclient"
  git describe --long --tags --abbrev=7 | sed 's/^v-//;s/\([^-]*-g\)/r\1/;s/-/./g'
}

prepare() {
  cd "opensteamclient"
  git submodule init
  git submodule init
  git config submodule.ext/SteamDatabase/Protobufs.url "$srcdir/Protobufs"
  git -c protocol.file.allow=always submodule update
}

build() {
  cmake -B build -S opensteamclient -DREL_BUILD=1 \
        -DCMAKE_INSTALL_PREFIX="/usr" \
        -DCMAKE_BUILD_TYPE=None \
        -Wno-dev
  cmake --build build
}

check() {
  ctest --test-dir build --output-on-failure
}

package() {
  DESTDIR="${pkgdir}" cmake --install build

  cd "opensteamclient"
  install -Dm644 LICENSE "$pkgdir/usr/share/licenses/$pkgname/LICENSE"
}