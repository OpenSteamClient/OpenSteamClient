# Package name
pkgname=opensteamclient-git

# //TODO: auto updating this version
pkgver=0.0.1

# Increment every time this PKGBUILD is updated, if the software updates, reset this to 1
pkgrel=1

# Description
pkgdesc="Partially open-source alternative to the Steam Client application"

# We only support x86_64, since steamclient.so doesn't exist for ARM or otherwise
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
    'openssl'
    'protobuf'
)

# Dependencies needed to build the package
makedepends=(
    'git'
    'cmake'
    'nlohmann-json'
    'extra-cmake-modules'
)

checkdepends=()
optdepends=(
    'lib32-glibc: Steam Client Service (VAC) support'
)

provides=()

# We can exist side-by-side with the official app
conflicts=()
replaces=()

# Put global config files here that pacman should backup
backup=()

# Override default makepkg options
options=()

# Install script
install=

# Changelog, too much work to include
changelog=

source=("git+https://github.com/20PercentRendered/opensteamclient.git")
sha256sums=('SKIP')

noextract=()
md5sums=()
validpgpkeys=()

pkgver() {
        cd "opensteamclient"
        git describe --long --tags | sed 's/\([^-]*-g\)/r\1/;s/-/./g'
}

prepare() {
        pwd
        cd "opensteamclient"
        pwd
        git submodule init
        git submodule update
}

build() {
        cd "opensteamclient"
        pwd
        mkdir -p build
        cd build
        pwd
        cmake .. -DREL_BUILD=1 -DCMAKE_INSTALL_PREFIX="/usr" -DCMAKE_BUILD_TYPE=
        cmake --build .
}

check() {
        pwd
        cd "opensteamclient/build"
        pwd
        # We don't have a testing solution yet
}

package() {
        pwd
        cd "opensteamclient/build"
        pwd
        DESTDIR="${pkgdir}" cmake --install .
}
