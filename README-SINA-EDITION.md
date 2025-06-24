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

### Preventing Overscroll

Overscroll can occur, if you scroll really fast (for example using a Logitech MX
Master mouse with MagSpeed Electromagnetic scrolling) and then suddenly stop
scrolling (by stopping the scroll wheel with your finger).  If the scrolling
itself doesn't stop immediately, you experience overscroll.  It could also be
described as lagging.

Overscroll is prevented in the history view of SourceGit SE.  This is tested
on Fedora only so far.

Overscroll was only seen on Linux so far.
