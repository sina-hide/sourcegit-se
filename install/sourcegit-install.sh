#!/usr/bin/env sh

set -e

sys=$(uname)

case "$sys" in
	Linux)  exec ./sourcegit-install-linux.sh;;
	*)      echo "Can't install on unknown system '$sys'.";;
esac

exit 1
