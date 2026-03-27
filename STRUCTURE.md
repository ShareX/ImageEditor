# ShareX.ImageEditor - Structure Map

This document is the authoritative reference for navigating and extending the library.
It is written for coding agents and human contributors equally.

---

## Namespace-to-Path Contract

Most types in this repository follow a deterministic namespace-to-path mapping:

```text
namespace ShareX.ImageEditor.X.Y.Z
-> src/ShareX.ImageEditor/X/Y/Z/TypeName.cs
```

Examples:
- `ShareX.ImageEditor.Hosting.ImageEditorOptions` -> `Hosting/ImageEditorOptions.cs`
- `ShareX.ImageEditor.Core.ImageEffects.Filters.BlurImageEffect` -> `Core/ImageEffects/Filters/BlurImageEffect.cs`
- `ShareX.ImageEditor.Presentation.Views.Dialogs.SchemaDrivenEffectDialog` -> `Presentation/Views/Dialogs/SchemaDrivenEffectDialog.axaml.cs`
- `ShareX.ImageEditor.Presentation.Controllers.EditorInputController` -> `Presentation/Controllers/EditorInputController.cs`

Rules:
- AXAML views use `.axaml` plus `.axaml.cs`.
- Resource dictionaries and style includes use `.axaml` only.

Known exceptions:
- Effect dialogs are schema-driven from `Presentation/Effects/` metadata while bespoke dialog files stay under `Presentation/Views/Dialogs/`.
- Annotation visual partials live in `Presentation/Rendering/AnnotationVisuals/` while extending annotation types in the `ShareX.ImageEditor.Core.Annotations` namespace.

Use direct path computation first, then fall back to search only when one of the exceptions applies.

---

## Top-Level Buckets

```text
src/ShareX.ImageEditor/
|-- Assets/           Embedded cursors and icon font
|-- Core/             Platform-agnostic editor engine
|-- Hosting/          Host-facing API and service contracts
`-- Presentation/     Avalonia UI: views, controls, rendering, theming, viewmodels
```

---

## Hosting/

Entry points and contracts consumed by host applications such as ShareX.

| File | Role |
|------|------|
| `AvaloniaIntegration.cs` | Static launcher for opening the editor |
| `ImageEditorOptions.cs` | Options passed from host to editor |
| `EditorServices.cs` | Service wiring for DI |
| `IClipboardService.cs` | Clipboard abstraction for host integration |
| `IDesktopWallpaperService.cs` | Wallpaper abstraction for host-provided backgrounds |
| `WindowsDesktopWallpaperService.cs` | Default internal Windows wallpaper resolver |
| `DesktopWallpaperInfo.cs` | Resolved wallpaper metadata |
| `DesktopWallpaperLayout.cs` | Wallpaper layout enum |
| `EditorHostExample.cs` | Reference host integration example |
| `Diagnostics/EditorDiagnostics.cs` | Logging and tracing hooks |

If you are integrating the editor into a new host, start with `AvaloniaIntegration.cs` and `ImageEditorOptions.cs`.

---

## Core/

Platform-agnostic. No Avalonia references. Safe to unit-test without UI.

### Annotations

All concrete annotation types are enumerated in `Core/Annotations/Base/Annotation.cs` via
`[JsonDerivedType]` attributes. That file is the authoritative annotation inventory.

Current annotation folder layout:

```text
Core/Annotations/
|-- AnnotationCategory.cs
|-- Base/
|   |-- Annotation.cs
|   `-- IPointBasedAnnotation.cs
|-- Effects/
|   `-- BaseEffectAnnotation.cs + Blur, Highlight, Magnify, Pixelate, Spotlight
|-- Shapes/
|   `-- Arrow, Crop, CutOut, Ellipse, Freehand, Image, Line, Rectangle, SmartEraser
`-- Text/
    `-- Number, SpeechBalloon, Text
```

`AnnotationCategory.cs` defines the three categories: `Effects`, `Shapes`, and `Text`.
Each concrete annotation class returns its category through `Annotation.Category`.

To add a new annotation type:
1. Create the class in the appropriate `Core/Annotations/...` folder.
2. Add a `[JsonDerivedType]` entry in `Core/Annotations/Base/Annotation.cs`.
3. Add rendering support in `Presentation/Rendering/AnnotationVisuals/`.
4. Update `Presentation/Rendering/AnnotationVisuals/AnnotationVisualFactory.cs`.

### ImageEffects

All effect categories are enumerated in `Core/ImageEffects/ImageEffectCategory.cs`.

```text
Core/ImageEffects/
|-- ImageEffect.cs           Root abstraction
|-- ImageEffectBase.cs       Self-describing base (Id, Name, Category, Parameters, Apply)
|-- ImageEffectContext.cs    Execution context passed to effect application
|-- EffectExecutionMode.cs   Immediate vs dialog-driven execution mode
|-- ImageEffectCategory.cs
|-- Adjustments/             AdjustmentImageEffectBase.cs + concrete effects
|-- Drawings/                Drawing effects (line/text/shape/watermark/background/etc.)
|-- Filters/                 Filter effects
|-- Helpers/                 ConvolutionHelper, ImageHelpers, ProceduralEffectHelper, TypeExtensions
|-- Manipulations/           Geometric/transform effects
`-- Parameters/              Reusable parameter models (number, color, enum, bool, etc.)
```

Base class note:
- `ImageEffect.cs` is the root abstraction.
- `ImageEffectBase` extends `ImageEffect` with self-contained `Id`, `Name`, `Category`, `Description`, `IconKey`, `Parameters`, and `Apply()`. All concrete effects extend this.
- `AdjustmentImageEffectBase` is a category-specific base for adjustments, providing `ApplyColorMatrix()`, `ApplyColorFilter()`, and `ApplyPixelOperation()` helpers.
- Effects are auto-discovered at startup by `DiscoveredEffectRegistry` via reflection — no manual registration required.

To add a new image effect:
1. Create a single sealed class in the matching `Core/ImageEffects/<Category>/` folder extending `ImageEffectBase` (or `AdjustmentImageEffectBase` for adjustments).
2. Override `Id`, `Name`, `Category`, `Description`, `IconKey`, `Parameters`, and `Apply()`. That's it — the effect is auto-discovered.
3. If the effect needs a bespoke editor dialog, set `EditorKey` and register the control in `Presentation/Views/Dialogs/EffectDialogRegistry.cs`.

### Editor / History / Abstractions

```text
Core/Abstractions/    IAnnotationToolbarAdapter.cs
Core/Editor/          EditorCore.cs, EditorTool.cs
Core/History/         EditorHistory.cs, EditorMemento.cs
```

---

## Presentation/

All Avalonia UI code. Depends on Core; never referenced by Core.

### Registry / Inventory Files

These files are the authoritative source for what exists in their domain.
Read them before inspecting individual implementations.

| File | Inventory / Responsibility |
|------|----------------------------|
| `Core/Annotations/Base/Annotation.cs` | All serializable annotation types |
| `Presentation/Rendering/AnnotationVisuals/AnnotationVisualFactory.cs` | Persisted/preview annotation visual creation and synchronization |
| `Presentation/Effects/ImageEffectCatalog.cs` | Unified effect catalog used by browser, dialog routing, and metadata display |
| `Presentation/Effects/DiscoveredEffectRegistry.cs` | Reflection-based effect discovery and catalog seeding |
| `Presentation/Views/Dialogs/EffectDialogRegistry.cs` | Bespoke editor routing for effects that need custom UI |
| `Presentation/Controls/EffectBrowserPanel.axaml.cs` | Effect browser categories, menu items, and dialog/immediate-action wiring |

### Folder Map

```text
Presentation/
|-- Controllers/      EditorInputController, EditorSelectionController, EditorZoomController
|-- Controls/         Custom controls and templated controls
|   |-- EffectSlider.axaml
|   |-- EffectSlider.cs
|   `-- ColorPickerDropdown, StrengthSlider, ZoomPickerDropdown, etc.
|-- Converters/       XAML value converters
|-- Effects/          Effect metadata/catalog models and discovery adapters
|-- Rendering/        Bitmap/cursor helpers plus annotation visual support
|   `-- AnnotationVisuals/
|       |-- AnnotationVisualFactory.cs
|       `-- Annotation-specific .Visual.cs partials
|-- Theming/          ThemeManager, icon constants, ShareXStyles.axaml, ShareXTheme.axaml
|-- ViewModels/       MainViewModel, its partial companions, adapter/viewmodel base types
`-- Views/
    |-- Dialogs/
    |   |-- SchemaDrivenEffectDialog.axaml(.cs)
    |   |-- EffectDialogRegistry.cs
    |   |-- EffectEventArgs.cs
    |   `-- IEffectDialog.cs
    |-- ConfirmationDialogView.axaml(.cs)
    |-- EditorView.axaml(.cs)
    |-- EditorView.ClipboardHandler.cs
    |-- EditorView.CoreBridge.cs
    |-- EditorView.EffectsHost.cs
    |-- EditorView.ToolbarHandlers.cs
    `-- EditorWindow.axaml(.cs)
```

Notes:
- Dialog UI is now primarily schema-driven via `SchemaDrivenEffectDialog`; most effects do not require dedicated dialog classes.
- `EffectDialogRegistry.cs` only maps bespoke editors for effects with custom interaction requirements (for example perspective warp, resize, crop, and selected drawing effects).
- `EditorView.EffectsHost.cs` still contains host-side operation handling for editor operations that are not plain `ImageEffect` rows.
- `EffectSlider.axaml` is a style/template resource paired with the `EffectSlider.cs` control class.

### Partial Class Conventions

`EditorView` and `MainViewModel` are split into named partials.
Use `EditorView.*.cs` or `MainViewModel.*.cs` globs to find all companions.

| Partial suffix | Responsibility |
|----------------|----------------|
| `ClipboardHandler` | Cut/copy/paste from the host clipboard |
| `CoreBridge` | `EditorCore` event subscriptions and callbacks |
| `EffectsHost` | Effect dialog dispatch and effect-panel orchestration |
| `ToolbarHandlers` | Annotation toolbar button handlers |
| `BackgroundState` | Background and wallpaper state |
| `CanvasState` | Canvas pan/zoom/selection state |
| `EffectPreview` | Live effect preview pipeline |
| `ImageState` | Source bitmap and image lifecycle state |
| `ToolOptions` | Per-tool property bindings and options state |

---

## Assets/

`src/ShareX.ImageEditor/Assets/` currently contains:
- `lucide.ttf` for the icon font used by the UI
- `lucide-unicode.html` as icon glyph reference
- `closedhand.cur`
- `Crosshair.cur`
- `openhand.cur`

Reference patterns:
- Font family: `avares://ShareX.ImageEditor/Assets#lucide`
- File assets: `avares://ShareX.ImageEditor/Assets/<file>`
