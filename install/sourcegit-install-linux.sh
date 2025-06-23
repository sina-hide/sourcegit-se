#!/usr/bin/env sh

# Prerequisite Fedora: sudo dnf install clang zlib-devel

set -e

# Constants.
install_base="${XDG_DATA_HOME:-$HOME/.local/share}"
sourcegit_dir="$install_base"/sourcegit
applications_dir="$install_base"/applications

# Build SourceGit as Native AOT.
./sourcegit-build-linux.sh

# Create desktop file.
cat > "$applications_dir"/SourceGit.desktop <<- END
	[Desktop Entry]
	Type = Application
	Name = SourceGit SE
	GenericName = Opensource Git GUI client (Sina Edition)
	Icon = $install_base/sourcegit/sourcegit.png
	Exec = $install_base/sourcegit/sourcegit-start-linux.sh
END

# Create conf file, but don't silently overwrite existing conf file.
cp -pi "$sourcegit_dir"/sourcegit-start-linux.template.conf "$applications_dir"/SourceGit.desktop.conf
