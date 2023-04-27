# Maintainer: Onni Kukkonen <onni.kukkonen77@gmail.com>

# Package name
pkgname=opensteam-git

# //TODO: auto updating this version
pkgver=0.0.1

# Increment every time this PKGBUILD is updated, if the software updates, reset this to 1
pkgrel=1

# Description
pkgdesc="Partially open-source alternative to the Steam Client application"

# We only support x86_64, since steamclient.so doesn't exist for ARM or other platforms
arch=('x86_64')

# Our github page
url="https://github.com/20PercentRendered/opensteamclient"

# Our license
license=('MIT')

# All our dependencies (there's a lot)
depends=(
    'qt6-base>=6.5'
    'libarchive>=3.6'
    'qrencode>=4.1'
    'openssl>=3.0.8'
    'protobuf>=21.12'
    'hicolor-icon-theme'
    'gcc-libs'
    'curl'
)

# Dependencies needed to build the package
makedepends=(
    'git'
    'cmake'
    'nlohmann-json'
    'extra-cmake-modules'
)

optdepends=(
    'lib32-gcc-libs: Steam Client Service (VAC) support'
)


source=("git+https://github.com/20PercentRendered/opensteamclient.git")
sha256sums=('SKIP')

pkgver() {
        cd "opensteamclient"
        git describe --long --tags | sed 's/\([^-]*-g\)/r\1/;s/-/./g'
}

prepare() {
        cd "opensteamclient"
        git submodule init
        git submodule update
}

build() {
        cd "opensteamclient"
        mkdir -p build
        cd build
        cmake .. -DREL_BUILD=1 -DCMAKE_INSTALL_PREFIX="/usr" -DCMAKE_BUILD_TYPE=None
        cmake --build .
}

check() {
        cd "opensteamclient/build"
        # We don't have a testing solution yet
}

package() {
        cd "opensteamclient"

        # MIT license
        install -Dm644 LICENSE "$pkgdir/usr/share/licenses/$pkgname/LICENSE"

        cd "build"
        DESTDIR="${pkgdir}" cmake --install .
}
