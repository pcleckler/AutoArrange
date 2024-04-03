# AutoArrange
A Windows 10 utility for dynamically arranging application windows based on desktop layout, ensuring optimal positioning for enhanced productivity.

## Operation
1. AutoArrange sits in the SysTray and monitors locations of configured windows.
1. Configured windows are periodically relocated to their preferred positions.
1. When desktop layout changes, AutoArrange begins using the configured list of windows for that layout.

## Configuration
1. Detects desktop layout geometry and creates a configuration entry for it.
1. Allows adding/modifying/removing of window references from the desktop layout.
1. Allows visual selection of windows for arrangement configuration.
1. Allows customization of the window reference to facilitate changes in window class, title, etc.
1. Window configuration can be copied between desktop layout.