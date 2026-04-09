# ShareX.ImageEditor Port Status

Last updated: 2026-04-08

## Port Source
- ShareX.ImageEditor commit: `9bad8ddd9` (HEAD as of 2026-04-08)
- XerahS submodule last synced to: `9bad8ddd9`
- XerahS submodule current HEAD: `6aac31b` (submodule's own commit history after porting)

## Port Activity (2026-04-08)
The following ShareX commits were ported in this session:

### High Risk — Interaction Cache System (a9d829b9f + 10433d15e)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| BaseEffectAnnotation.cs | a9d829b9f | Core/Annotations/Effects/ | High | ✅ Ported | Added GetInteractionCacheKey, CreateInteractionCacheBitmap, UpdateEffectFromInteractionCache, UpdateEffectFromAlignedCache |
| BlurAnnotation.cs | a9d829b9f + 10433d15e | Core/Annotations/Effects/ | High | ✅ Ported | Added CreateBlurredSourceCache, interaction cache methods |
| PixelateAnnotation.cs | a9d829b9f | Core/Annotations/Effects/ | High | ✅ Ported | Added CreatePixelatedSourceCache, interaction cache methods |
| HighlightAnnotation.cs | a9d829b9f | Core/Annotations/Effects/ | High | ✅ Ported | Added CreateHighlightedSourceCache, ApplyHighlightToBitmap, interaction cache methods |
| MagnifyAnnotation.cs | a9d829b9f | Core/Annotations/Effects/ | High | ✅ Ported | Added interaction cache methods, refactored to UpdateEffectCore |
| EditorView.CoreBridge.cs | a9d829b9f | Presentation/Views/ | High | ✅ Ported | Added _cachedEffectPreview*, TryUpdateCachedEffectVisual, EnsureEffectPreviewCache, ClearEffectPreviewCache, ClearInteractiveEffectPreviewCache, UpdateInteractiveEffectVisual |
| EditorInputController.cs | a9d829b9f | Presentation/Controllers/ | Medium | ✅ Ported | UpdateEffectVisual now delegates to _view.UpdateInteractiveEffectVisual |
| AnnotationEffectVisualUpdater.cs | a9d829b9f | Presentation/Rendering/ | Medium | ✅ Verified | Already had ApplyEffectBrush and UpdateEffectVisual methods |

### Medium Risk — Spotlight Overlay (86369123f)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| SpotlightControl.cs | 86369123f | Presentation/Controls/ | Medium | ✅ Ported | Simplified to selection shell only; darkening overlay moved to SpotlightOverlayControl |
| SpotlightOverlayControl.cs | 86369123f | Presentation/Controls/ | High | ✅ Ported | NEW file — renders darkening overlay via shared SKCanvasControl |
| EditorView.axaml | 86369123f | Presentation/Views/ | High | ✅ Ported | Added SpotlightOverlayControl to canvas |
| EditorView.CoreBridge.cs | 86369123f | Presentation/Views/ | High | ✅ Ported | Added RefreshSpotlightOverlay() method |
| EditorView.ToolbarHandlers.cs | 86369123f | Presentation/Views/ | Medium | ✅ Ported | ApplySelectedEffectStrength now calls RefreshSpotlightOverlay() |
| EditorInputController.cs | 86369123f | Presentation/Controllers/ | Medium | ✅ Ported | 3× spotlightControl.InvalidateVisual() replaced with _view.RefreshSpotlightOverlay() |

### Medium Risk — Core SKBitmap Reuse (dc794cd6b)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| EditorView.CoreBridge.cs | dc794cd6b | Presentation/Views/ | Medium | ✅ Ported | OnRequestUpdateEffect uses _editorCore.SourceImage when available; temporarySource disposed in finally |

### Medium Risk — Host Copy/Save Handlers (9bad64d52)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| MainViewModel.cs | 9bad64d52 | Presentation/ViewModels/ | Medium | ✅ Ported | Added HasHostCopyHandler, HasHostSaveHandler, HasHostSaveAsHandler flags |
| AvaloniaIntegration.cs | 9bad64d52 | Hosting/ | Medium | ✅ Ported | Set host handler flags after wiring events |
| EditorView.axaml.cs | 9bad64d52 | Presentation/Views/ | Medium | ✅ Ported | OnCopyImageRequested handler with SkiaSharp snapshot → clipboard; early returns for host handlers |

### Low Risk — Use ImageFilePath Instead of LastSavedPath (879f2b5e1)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| MainViewModel.cs | 879f2b5e1 | Presentation/ViewModels/ | Low | ✅ Ported | Removed _lastSavedPath field; CanSave() uses ImageFilePath |
| EditorView.axaml.cs | 879f2b5e1 | Presentation/Views/ | Low | ✅ Ported | OnSaveRequested/OnSaveAsRequested use ImageFilePath |
| EditorWindow.axaml.cs | 879f2b5e1 | Presentation/Views/ | Low | ✅ Ported | Removed LastSavedPath assignment |
| AvaloniaUIService.cs | 879f2b5e1 | Services/ | Low | ✅ Ported | Use ImageFilePath instead of LastSavedPath |
| MainViewModelHelper.cs | 879f2b5e1 | Services/ | Low | ✅ Ported | All LastSavedPath → ImageFilePath |
| MainWindow.axaml.cs (TEMP2) | 879f2b5e1 | Views_TEMP2/ | Low | ✅ Ported | LastSavedPath → ImageFilePath |
| EditorCloseConfirmationTests.cs | 879f2b5e1 | Tests/ | Low | ✅ Ported | LastSavedPath removed from test |

### Medium Risk — Tool-Specific Shape Selection and Hover (846eee26a)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| EditorSelectionController.cs | 846eee26a | Presentation/Controllers/ | Medium | ✅ Ported | Added GetControlToolType helper; updated hit-testing logic |
| EditorInputController.cs | 846eee26a | Presentation/Controllers/ | Medium | ✅ Ported | Tool-specific filtering for annotation creation |

### Low Risk — Reset IsDirty After Saving (f7e4029b1)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| EditorView.axaml.cs | f7e4029b1 | Presentation/Views/ | Low | ✅ Ported | Added vm.IsDirty = false in OnSaveRequested and OnSaveAsRequested |
| AvaloniaUIService.cs | f7e4029b1 | Hosting/ | Low | ✅ Ported | Added vm.IsDirty = false after setting ImageFilePath |

### Low Risk — Disabled ContentPresenter Style (64ab3590d)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| ImageEditorStyles.axaml | 64ab3590d | Presentation/Styles/ | Low | ✅ Ported | Added disabled ContentPresenter style |

### Medium Risk — Text Editor KeyUp and Caret Fix (59f48dfba)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| EditorInputController.cs | 59f48dfba | Presentation/Controllers/ | Medium | ✅ Ported | Changed KeyDown → KeyUp; added Dispatcher.UIThread.Post for caret reset |

### Low Risk — Preserve Scroll Offset When Focusing Text Box (bbfd59cd8)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| EditorInputController.cs | bbfd59cd8 | Presentation/Controllers/ | Low | ✅ Ported | Added Dispatcher.UIThread.Post with preserved scroll offset |

### Medium Risk — Snap Rotation to 45° with Shift (adb34c82b)
| File | ShareX Commit | XerahS Location | Risk | Status | Notes |
|------|--------------|-----------------|------|--------|-------|
| EditorSelectionController.cs | adb34c82b | Presentation/Controllers/ | Medium | ✅ Ported | Shift key snaps rotation to 45° increments |
| EditorSelectionController.cs | adb34c82b | Presentation/Controllers/ | Medium | ✅ Ported | Hover outline fix: TextBox → OutlinedTextControl cast |

## Remaining ShareX Commits (not yet reviewed)
The following ShareX commits after 8a51a9a may still need review:
- (List any commits not covered in the table above)

## Notes
- All ShareX.ImageEditor code uses WPF; XerahS uses Avalonia + SkiaSharp
- Interaction cache system (a9d829b9f + 10433d15e) was ported together as they are interdependent
- Spotlight overlay (86369123f) was already partially ported before this session (SpotlightOverlayControl existed)