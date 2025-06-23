#!/usr/bin/env sh

# Script to install SourceGit SE.  Should be the only script directly called by
# the user for installation.

# When starting some of the scripts, the current directory has to be the
# directory where the scripts reside.  We assure this here.
cd "$(dirname "$0")" || exit 1

./sourcegit-install.sh
