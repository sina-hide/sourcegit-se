#!/usr/bin/env sh

# Prerequisite Fedora: sudo dnf install clang zlib-devel

set -e

cd ..

repo=$PWD
inst="$HOME"/.local/share

dotnet publish -r linux-x64 -c Release

rm -rf "$inst"/sourcegit
mkdir -p "$inst"/sourcegit

cp -pr "$repo"/src/bin/Release/net9.0/linux-x64/publish/* "$inst"/sourcegit
mv "$inst"/sourcegit/SourceGit "$inst"/sourcegit/sourcegit
cp -p "$repo"/build/resources/_common/icons/sourcegit.png "$inst"/sourcegit

desktop="$inst"/applications/sourcegit.desktop 

cat > "$desktop" << END
[Desktop Entry]
Type = Application
Name = SourceGit
GenericName = Opensource Git GUI client
END

echo >> "$desktop" "Icon = $inst/sourcegit/sourcegit.png"

useenv=false
if [ -e "$repo"/install/scale-factors.local.conf ]
then
    scale=$(cat "$repo"/install/scale-factors.local.conf)
    if [ -n "$scale" ]
    then
        useenv=true
    fi
fi

if [[ $useenv = true ]]
then
    echo >> "$desktop" '# Find the name of displays by using `xrandr --listactivemonitors`.'
    echo >> "$desktop" "Exec = /usr/bin/env AVALONIA_SCREEN_SCALE_FACTORS='$scale' $inst/sourcegit/sourcegit"
else
    echo >> "$desktop" "Exec = $inst/sourcegit/sourcegit"
fi
