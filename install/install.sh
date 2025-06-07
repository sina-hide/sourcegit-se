#!/usr/bin/env sh

set -e

sys=$(uname)

case "$sys" in
    Linux)  exec ./install-linux.sh;;
    *)      echo "Can't install on unknown system '$sys'.";;
esac

exit 1
