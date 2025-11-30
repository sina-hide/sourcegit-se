# SourceGit SE (Sina Edition)

This is an unofficial fork of the upstream project
https://github.com/sourcegit-scm/sourcegit with the goal of experimenting and
testing new ideas before they eventually land upstream.  Development is mainly
done on Fedora Workstation (Gnome), testing is additionally done on Windows.

The goal is to keep differences between this unofficial fork and the upstream
project minimal and mergeable.

## Differences

This is an overview of the differences between this unofficial fork and the
upstream project.

### About Box

The program is called SourceGit SE instead of SourceGit in the about box.

### Installation

This unofficial fork contains an installation directory with scripts for
installing on Linux.  The installation is local (no root required) with
configurable scaling and IM module prevention.

### Mise-en-place

For installing an exact version of dotnet, mise-en-place (or mise for short,
see https://mise.jdx.dev for details) is used.  By executing `mise install` in
the root directory of the repository it can be installed.  As an alternative it
will be installed when using the installation scripts to build and install
SourceGit SE.

The downside is that **Rider doesn't auto-detect** the .NET CLI executable path.  It
has to be configured accordingly under *Settings » Build, Execution, Deployment
» Toolset and Build*.  The MSBuild version should then be auto-detected.

### Focus filter

There is an additional filter to focus on a part of the complete commit graph.

This is useful for example when working on a long-living feature branch which
itself may have sub feature branches.  If you create a branch or tag at the
first commit of the feature branch and focus this branch or tag, then you see
all parents and all children of the commit corresponding to this focused branch
or tag.  This is the history before starting the branch and the complete branch
itself including all sub-branches.

While the other filters (include and exclude) use git options to do the
filtering, the focused commits are determined afterward by removing commits to
use for the commit graph.
