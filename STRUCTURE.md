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
- `ShareX.ImageEditor.Presentation.Views.Dialogs.BlurDialog` -> `Presentation/Views/Dialogs/Filters/BlurDialog.axaml.cs`
- `ShareX.ImageEditor.Presentation.Controllers.EditorInputController` -> `Presentation/Controllers/EditorInputController.cs`

Rules:
- AXAML views use `.axaml` plus `.axaml.cs`.
- Resource dictionaries and style includes use `.axaml` only.

Known exceptions:
- Effect dialog files stay in the `ShareX.ImageEditor.Presentation.Views.Dialogs` namespace while their files are grouped under `Presentation/Views/Dialogs/<Category>/`.
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
|-- ImageEffect.cs
|-- ImageEffectCategory.cs
|-- Adjustments/      AdjustmentImageEffect.cs + 26 concrete effects
|-- Drawings/         8 drawing effects plus drawing helpers/enums
|-- Filters/          FilterImageEffect.cs + 65 concrete effects
|-- Helpers/          ConvolutionHelper, ImageHelpers, ProceduralEffectHelper, TypeExtensions
`-- Manipulations/    13 concrete effects
```

Base class note:
- `ImageEffect.cs` is the root abstraction.
- `AdjustmentImageEffect` and `FilterImageEffect` are per-category abstract bases.
- Drawing and manipulation effects derive directly from `ImageEffect`.

To add a new image effect:
1. Create the effect class in the matching `Core/ImageEffects/<Category>/` folder.
2. If the effect needs a dialog, add it under the matching `Presentation/Views/Dialogs/<Category>/` folder.
3. If the dialog is registry-backed, register it in `Presentation/Views/Dialogs/EffectDialogRegistry.cs`.
4. Add the effect entry to `Presentation/Controls/EffectBrowserPanel.axaml.cs`.

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
| `Presentation/Views/Dialogs/EffectDialogRegistry.cs` | Registry-backed effect dialog factories (currently 104 entries) |
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
|-- Rendering/        Bitmap/cursor helpers plus annotation visual support
|   `-- AnnotationVisuals/
|       |-- AnnotationVisualFactory.cs
|       `-- Annotation-specific .Visual.cs partials
|-- Theming/          ThemeManager, icon constants, ShareXStyles.axaml, ShareXTheme.axaml
|-- ViewModels/       MainViewModel, its partial companions, adapter/viewmodel base types
`-- Views/
    |-- Dialogs/
    |   |-- Adjustments/    23 dialogs
    |   |-- Drawings/       9 dialogs
    |   |-- Filters/        60 dialogs
    |   |-- Manipulations/  14 dialogs
    |   |-- EffectDialogRegistry.cs
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
- `Views/Dialogs/` contains 106 dialogs in total. `EffectDialogRegistry.cs` currently covers 104 of them; `CropImageDialog` and `RotateCustomAngleDialog` are opened directly from `EditorView.EffectsHost.cs`.
- `BorderDialog` is grouped under `Views/Dialogs/Drawings/`, even though its underlying effect class lives in `Core/ImageEffects/Filters/`.
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
- `closedhand.cur`
- `Crosshair.cur`
- `openhand.cur`

Reference patterns:
- Font family: `avares://ShareX.ImageEditor/Assets#lucide`
- File assets: `avares://ShareX.ImageEditor/Assets/<file>`
