#!/usr/bin/env sh

# Prerequisite Fedora: sudo dnf install clang zlib-devel
# Additional prererquisite: mise has to be installed (https://mise.jdx.dev)

set -e

# Change into root directory of repository.
cd ..

# Constants.
repo=$PWD
install_base="${XDG_DATA_HOME:-$HOME/.local/share}"
sourcegit_dir="$install_base"/sourcegit

machine=$(uname -m)
case $machine in
	x86_64)  runtime_identifier=linux-x64;;
	*)       echo unsupported machine "$machine"; exit 1;;
esac

# Install configured dotnet version if not yet installed.
mise trust 2> /dev/null
mise install

dotnet publish --runtime "$runtime_identifier" --configuration Release

rm -rf "$sourcegit_dir"
mkdir -p "$sourcegit_dir"

# Copy binaries.
cp -pr "$repo"/src/bin/Release/net9.0/linux-x64/publish/* "$sourcegit_dir"

# Copy icon.
cp -p "$repo"/build/resources/_common/icons/sourcegit.png "$sourcegit_dir"

# Copy start scripts.
cp -p "$repo"/install/sourcegit-start-linux.template.conf "$sourcegit_dir"
cp -p "$repo"/install/sourcegit-start-linux.sh "$sourcegit_dir"
