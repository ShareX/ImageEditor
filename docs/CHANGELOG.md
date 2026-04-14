# Changelog

All notable changes to the ShareX.ImageEditor project will be documented in this file.

## v0.2.0

### Features
- **Annotations**: Add editable annotation sidecar support with `.xann` extension for preserving editable annotation data alongside exported images `(45cf1de, 42f27c9)`
- **Annotations**: Add StepTailStyle option for Step and SpeechBalloon annotations — defer tail rendering until dragged, render with shaft geometry `(db98c80, 1064b35, 9c65902, e35a1c4, 44ea622)`
- **Crop Tool**: Add auto-crop detection — editor automatically detects and selects crop region based on content `(c6fd309)`
- **Image Effects**: Add decorative border effects including Windows11/Crimson borders and multiple procedural border styles `(a6c31d5, f8307a7, f1b2c19, bceb4e1)`
- **Image Effects**: Add FakeHairImageEffect, Candy Cane Border, five new effects, six new effects — all schema-driven `(dcb8162, 06b2c91, cb9e176, 5fa8e79)`
- **UI**: Add host accent styling hooks for confirmation dialog, aligned with host application theme `(2aa9725)`
- **Startup**: Add Sample.png asset for default editor pre-load on startup `(2cb0ca6)`

### Build
- **Platform**: Upgrade to Avalonia 12 and streamline to .NET 10.0 single target (x64/ARM64) `(197e772, fc1ae5a, 46a5dd7, a60937e)`
- **Bindings**: Align toolbar adapter contract with Avalonia 12 compiled bindings; reduce reflection bindings `(b34c473, 0738ca9, 841e388)`
- **SkiaSharp**: Migrate obsolete SkiaSharp APIs to current versions `(6a4192f)`
- **Line Endings**: Reapply strict LF line ending enforcement `(c00dca5)`

### Fixes
- **Crop UI**: Improve crop confirm button layout, cursor feedback, and canvas focus `(fc228fe, fcfc4ea)`
- **Annotations**: Fix StepControl arrow tail rendering and initialization gating `(44ea622, 9c65902)`
- **Dialog**: Use host application name for close confirmation dialog title `(6aac31b)`
- **Dialog**: Replace corrupted warning icon and wrap confirmation text for readability `(b7cd498)`
- **Toolbar**: Keep shared toolbar bindings dynamic across adapters `(841e388)`

### Refactor
- **Architecture**: Remove unnecessary Platforms restriction from project file `(aeb3e98)`
- **Port**: Catch up through ShareX@c6e3c5260 — interaction cache for image effects, shared SKCanvasControl for spotlight overlay rendering `(b168ea8, 952dc05, 098cddf)`

### Changed
- **Platform**: Drop BMP save option; add AVIF save support `(b45fc08)`

## v0.1.0

*Initial release of the modernized ShareX ImageEditor built on Avalonia UI and SkiaSharp.*

### Added
- **Core Engine**: Implemented an entirely new `EditorCore` host-agnostic image editing engine driven by `SkiaSharp`.
- **Annotation Tools**: Introduced a comprehensive suite of drawing tools, including Arrows (with tapered shafts), Ellipses, Rectangles, Freehand lines (with bezier smoothing), Highlighters, Step/Number counters, and Spotlight overlays.
- **Speech Balloons**: Added fully interactive speech balloon annotations featuring draggable tails with intelligent edge-snapping and in-place text editing overlays.
- **Advanced Tools**: Added an interactive Crop tool with draggable corner/edge handles, and a Cut Out tool allowing users to precisely remove horizontal or vertical logical sections of an image.
- **Image Effects System**: Developed a powerful `ImageEffects` pipeline with over 70 schema-driven filters, adjustments (Brightness, Hue, Gamma, Saturation), distortions (Twirl, Pinch, Perspective Warp), and visual styles (Clouds, Stained Glass, Oil Paint, Neon Glow, Blood Splash, Candy Cane).
- **Effect Browser UI**: Built a modular, schema-driven Effect Browser and registry with search filtering, categories, and recent/favorite effects tracking.
- **User Interface**: Created a modern, unified Avalonia-based UI toolbar integrating zoom controls, task modes, colour pickers, stroke thickness controls, and context menus.
- **Theming**: Integrated robust system theming with specific `ShareXLight` and `ShareXDark` theme resources, supporting system accent colours and dynamic popups.
- **Editor Options**: Supported configurable initial settings such as auto-copy to clipboard, exit confirmations, task-mode visibility, and default tool styles.
- **File & Navigation Extensions**: Added support for loading images via drag-and-drop, command-line arguments, and direct clipboard pasting. Enabled saving to AVIF alongside other standard formats.
- **Debugging & Diagnostics**: Added development tools including VS Code Model Context Protocol (MCP) integrations, visual-tree diagnostics, and host problem logging extensions.

### Changed
- **Icons & Typography**: Migrated all toolbar and effect visuals to the Lucide icon set and modernized UI typographies (including fallback Segoe UI support).
- **Input & Selection Flow**: Rewritten interaction handling using decoupled controller architectures (`EditorInputController`, `EditorSelectionController`) replacing monolithic canvas event handlers.
- **Platform Packaging**: Moved to `.NET 9.0` and `.NET 10.0` target frameworks ensuring broad compatibility across Windows, Linux, and macOS for x64/ARM64 architectures.
- **Selection Handling**: Completely redesigned selection grips, adding large, high-visibility L-shaped corner handles and dashed outlines for superior usability.
- **Rendering Architecture**: Phased out legacy Avalonia geometric primitives in favour of native SkiaSharp rendering matrices (`SKRect`, `SKPoint`) minimizing rendering anomalies during rotation and zooming.

### Fixed
- **Memory Management**: Resolved major use-after-free bugs, bounding box calculation memory leaks, and redundant `SKBitmap` copying paths in the effect preview cycles. 
- **Hit Testing**: Corrected invalid geometric hit-testing for tools under active rotation/skew, enabling flawless selection of thin lines, curved paths, and hollow ellipses.
- **Zoom Integrity**: Fixed DPI scaling issues and offset bugs that misaligned user drawings from the mouse cursor at zoom scales other than 100%. Addressed edge collision in Zoom-to-Fit operations.
- **Platform Integrity**: Handled Linux wallpaper pre-warming bugs and abstraction anomalies when resolving host backgrounds.
- **UI State Races**: Resolved race conditions locking users out of task modes. Prevented duplicate confirmation dialogs upon rapid exit keyboard events.

### Refactored
- **Effect Schemas**: Standardized all filter definitions under `ImageEffectBase`. Deleted over 150 redundant ad-hoc filter configuration dialogs in favour of automated schema-driven layouts with two-way parameter binding.
- **History Tracking**: Overhauled the Undo/Redo historical cache using lightweight state mementos instead of massive deep copies per interaction, achieving huge performance gains. 
- **Subsystem Interfaces**: Stripped the editor of isolated host dependencies, relying rigidly on `IClipboardService` and `IEditorServices` dependency injection bindings to remain functionally independent of its host application.
