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
