# Changelog

All notable changes to the ShareX.ImageEditor project will be documented in this file.

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
