# ShareX.ImageEditor

ShareX.ImageEditor is the image editor library used by ShareX and XerahS. It combines a UI-agnostic editing core with an Avalonia-based presentation layer and a small host-facing integration surface for embedding the editor into desktop applications.

The solution contains two main projects:
- `src/ShareX.ImageEditor`: the reusable editor library
- `src/ShareX.ImageEditor.Loader`: a standalone development host for running and testing the editor locally

## What This Project Provides

ShareX.ImageEditor is not just a single Avalonia window. It is split into distinct layers so host applications can reuse the editor without taking a dependency on the full UI internals:

- `Hosting` exposes the public integration surface, editor options, diagnostics hooks, clipboard/save/upload callbacks, and wallpaper services.
- `Core` contains the editor engine, annotation model, history stack, and image effect implementations.
- `Presentation` contains the Avalonia UI, controls, rendering helpers, dialogs, themes, and view models.

For a detailed folder-by-folder map, see [STRUCTURE.md](STRUCTURE.md).

## Feature Highlights

The editor currently includes:

- A reusable host API for opening the editor from a file path or stream.
- Dialog-based and immediate-action image effects, exposed through an effect browser with favorites support.
- A non-UI core that keeps editor state, image effects, and history separate from Avalonia views.
- Rich annotation tooling for screenshots, tutorials, bug reports, and quick image markups.
- Host callbacks for copy, save, save as, upload, and pin-to-screen flows.
- Task-mode support for capture workflows where the editor returns either the edited image or the original source image.
- Avalonia + SkiaSharp rendering with a standalone loader project for local development.

## Verbose Feature List

### Annotation and Editing Tools

The editor supports a broad set of markup and region tools designed for screenshot workflows:

- Selection and manipulation of existing annotations.
- Rectangle, ellipse, line, and arrow drawing tools.
- Freehand pen drawing.
- Text annotations with configurable styling.
- Speech balloon annotations.
- Numbered step markers for tutorials and documentation.
- Smart eraser support.
- Region-based blur, pixelate, magnify, highlight, and spotlight annotations.
- Crop and cut-out style editing operations.
- Image/sticker insertion as annotations.

### Image Effects

The effect system is organized around four user-facing categories in the effect browser:

- `Manipulations`: crop, auto-crop, flip, rotate, resize image, resize canvas, rounded corners, perspective warp, skew, twirl, displacement map, pinch/bulge, and 3D transforms.
- `Adjustments`: alpha, auto contrast, brightness, contrast, hue, saturation, gamma, levels, color matrix, colorize, selective color, replace color, grayscale, sepia, solarize, threshold, vibrance, temperature/tint, shadows/highlights, duotone gradient map, exposure, film emulation, plus immediate actions like invert, black and white, and polaroid.
- `Filters`: blur, sharpen, pixelate, noise, bloom, glow, halation, gaussian blur, lens blur, motion blur, spin blur, zoom blur, median filter, surface blur, unsharp mask, CRT, dithering, color depth, convolution matrix, edge-detection-style effects, and a long list of stylized looks such as ASCII art, anime speed lines, blueprint drawing, cartoon sticker cutout, claymation texture, hologram scan, holographic foil shimmer, matrix digital rain, neon edge glow, old camera flash burn, rainy window, stained glass, watercolor / Kuwahara, thermal vision, vintage print damage, and more.
- `Drawings`: background fill, background image, checkerboard, border, image overlay, particles, styled text, text watermark, and wooden frame effects.

At the code level the repository currently contains more than 100 image effects across the core effect categories, with both registry-backed dialogs and direct actions for common one-click operations.

### Workflow and Host Integration Features

The library is built to plug into a host application rather than only run as a standalone editor:

- Open the editor from a file path or any image stream.
- Supply `ImageEditorOptions` to control defaults such as colors, thickness, text styling, background presets, effect strengths, and favorite effects.
- Wire `EditorEvents` callbacks for copy, save, save as, pin, upload, and diagnostics reporting.
- Use task mode when the host wants the editor to participate in a larger capture pipeline.
- Return encoded image bytes from the editor window for host-side interoperability.
- Use the built-in Windows desktop wallpaper service or provide a custom implementation through the hosting layer.

### UI and Architecture Features

- Avalonia-based desktop UI with theming and custom controls.
- SkiaSharp-backed rendering and export pipeline.
- View-model-driven editor state with explicit partial classes for canvas state, image state, effect preview, tool options, and background state.
- Registry-driven dialog dispatch for most effect dialogs.
- A separate standalone loader project for local UI development and smoke testing.

## Key Entry Points

If you are new to the codebase, these are the fastest files to start from:

- `src/ShareX.ImageEditor/Hosting/AvaloniaIntegration.cs`
  Main entry point for embedding and showing the editor.
- `src/ShareX.ImageEditor/Hosting/ImageEditorOptions.cs`
  Host-configurable editor defaults.
- `src/ShareX.ImageEditor/Presentation/Controls/EffectBrowserPanel.axaml.cs`
  Current effect-browser inventory and user-facing effect organization.
- `src/ShareX.ImageEditor/Presentation/Views/Dialogs/EffectDialogRegistry.cs`
  Registry-backed dialog factory map.
- `src/ShareX.ImageEditor/Core/Annotations/Base/Annotation.cs`
  Authoritative annotation type inventory.
- `src/ShareX.ImageEditor/Presentation/Views/EditorView.EffectsHost.cs`
  Where dialog dispatch and direct effect actions are wired into the editor view.

## Running Locally

Build the solution:

```powershell
dotnet build .\ShareX.ImageEditor.sln
```

Run the standalone loader:

```powershell
dotnet run --project .\src\ShareX.ImageEditor.Loader\ShareX.ImageEditor.Loader.csproj
```

## Embedding the Editor

Hosts typically go through `AvaloniaIntegration`. Common options include:

- `ShowEditor(string filePath)` to open from disk.
- `ShowEditor(Stream imageStream)` to open from an in-memory image.
- `ShowEditorDialog(Stream imageStream, ImageEditorOptions options, EditorEvents? events = null, bool taskMode = false, string? imageFilePath = null)` for host-controlled workflows.

Typical host responsibilities:

- Provide the source image or file path.
- Decide how save/copy/upload/pin actions should be handled.
- Optionally customize defaults through `ImageEditorOptions`.
- Optionally consume diagnostics through `EditorEvents.DiagnosticReported`.

## Repository Navigation

- Start in `Hosting` when you need the public API or host integration surface.
- Start in `Core` when you need editor behavior, history, annotation types, or image effects.
- Start in `Presentation` when you need Avalonia UI, dialogs, rendering, input handling, or theming.
- Start in `src/ShareX.ImageEditor.Loader` when you want to run the editor as a standalone desktop app during development.
