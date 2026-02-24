#region License Information (GPL v3)

/*
    ShareX.ImageEditor - The UI-agnostic Editor library for ShareX
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ShareX.ImageEditor.Annotations;
using ShareX.ImageEditor.Controls;
using ShareX.ImageEditor.Helpers;
using ShareX.ImageEditor.ViewModels;
using ShareX.ImageEditor.Views.Controllers;
using ShareX.ImageEditor.Views.Dialogs;
using SkiaSharp;
using System.ComponentModel;

namespace ShareX.ImageEditor.Views
{
    public partial class EditorView : UserControl
    {
        // --- Edit Menu Event Handlers ---

        private void OnResizeImageRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.PreviewImage != null)
            {
                var dialog = new ResizeImageDialog();
                dialog.Initialize((int)vm.ImageWidth, (int)vm.ImageHeight);

                dialog.ApplyRequested += (s, args) =>
                {
                    vm.ResizeImage(args.NewWidth, args.NewHeight, args.Quality);
                    vm.CloseEffectsPanelCommand.Execute(null);
                };

                dialog.CancelRequested += (s, args) =>
                {
                    vm.CloseEffectsPanelCommand.Execute(null);
                };

                vm.EffectsPanelContent = dialog;
                vm.IsEffectsPanelOpen = true;
            }
        }

        private void OnResizeCanvasRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.PreviewImage != null)
            {
                var dialog = new ResizeCanvasDialog();
                // Get edge color from image for "Match image edge" option
                SKColor? edgeColor = null;
                try
                {
                    using var skBitmap = BitmapConversionHelpers.ToSKBitmap(vm.PreviewImage);
                    if (skBitmap != null)
                    {
                        edgeColor = skBitmap.GetPixel(0, 0);
                    }
                }
                catch { }

                dialog.Initialize(edgeColor);

                dialog.ApplyRequested += (s, args) =>
                {
                    vm.ResizeCanvas(args.Top, args.Right, args.Bottom, args.Left, args.BackgroundColor);
                    vm.CloseEffectsPanelCommand.Execute(null);
                };

                dialog.CancelRequested += (s, args) =>
                {
                    vm.CloseEffectsPanelCommand.Execute(null);
                };

                vm.EffectsPanelContent = dialog;
                vm.IsEffectsPanelOpen = true;
            }
        }

        private void OnCropImageRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.PreviewImage != null)
            {
                var dialog = new CropImageDialog();
                dialog.Initialize((int)vm.ImageWidth, (int)vm.ImageHeight);

                dialog.ApplyRequested += (s, args) =>
                {
                    // vm.CropImage(args.X, args.Y, args.Width, args.Height);
                    // SIP-FIX: Use Core crop to handle annotation adjustment and history unified
                    _editorCore.Crop(new SKRect(args.X, args.Y, args.X + args.Width, args.Y + args.Height));

                    vm.CloseEffectsPanelCommand.Execute(null);
                };

                dialog.CancelRequested += (s, args) =>
                {
                    vm.CloseEffectsPanelCommand.Execute(null);
                };

                vm.EffectsPanelContent = dialog;
                vm.IsEffectsPanelOpen = true;
            }
        }

        private void OnAutoCropImageRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.AutoCropImageCommand.Execute(null);
            }
        }

        private void OnRotate90CWRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Rotate90ClockwiseCommand.Execute(null);
            }
        }

        private void OnRotate90CCWRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Rotate90CounterClockwiseCommand.Execute(null);
            }
        }

        private void OnRotate180Requested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Rotate180Command.Execute(null);
            }
        }

        private void OnRotateCustomAngleRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.OpenRotateCustomAngleDialogCommand.Execute(null);
                var dialog = new RotateCustomAngleDialog();
                vm.EffectsPanelContent = dialog;
                vm.IsEffectsPanelOpen = true;
            }
        }

        private void OnFlipHorizontalRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.FlipHorizontalCommand.Execute(null);
            }
        }

        private void OnFlipVerticalRequested(object? sender, EventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.FlipVerticalCommand.Execute(null);
            }
        }

        // --- Effects Menu Handlers ---

        private void OnBrightnessRequested(object? sender, EventArgs e) => ShowEffectDialog(new BrightnessDialog());
        private void OnContrastRequested(object? sender, EventArgs e) => ShowEffectDialog(new ContrastDialog());
        private void OnHueRequested(object? sender, EventArgs e) => ShowEffectDialog(new HueDialog());
        private void OnSaturationRequested(object? sender, EventArgs e) => ShowEffectDialog(new SaturationDialog());
        private void OnGammaRequested(object? sender, EventArgs e) => ShowEffectDialog(new GammaDialog());
        private void OnAlphaRequested(object? sender, EventArgs e) => ShowEffectDialog(new AlphaDialog());
        private void OnColorizeRequested(object? sender, EventArgs e) => ShowEffectDialog(new ColorizeDialog());
        private void OnSelectiveColorRequested(object? sender, EventArgs e) => ShowEffectDialog(new SelectiveColorDialog());
        private void OnReplaceColorRequested(object? sender, EventArgs e) => ShowEffectDialog(new ReplaceColorDialog());
        private void OnGrayscaleRequested(object? sender, EventArgs e) => ShowEffectDialog(new GrayscaleDialog());

        private void OnInvertRequested(object? sender, EventArgs e) { if (DataContext is MainViewModel vm) vm.InvertColorsCommand.Execute(null); }
        private void OnBlackAndWhiteRequested(object? sender, EventArgs e) { if (DataContext is MainViewModel vm) vm.BlackAndWhiteCommand.Execute(null); }
        private void OnSepiaRequested(object? sender, EventArgs e) => ShowEffectDialog(new SepiaDialog());
        private void OnPolaroidRequested(object? sender, EventArgs e) { if (DataContext is MainViewModel vm) vm.PolaroidCommand.Execute(null); }

        // Filter handlers
        private void OnBorderRequested(object? sender, EventArgs e) => ShowEffectDialog(new BorderDialog());
        private void OnOutlineRequested(object? sender, EventArgs e) => ShowEffectDialog(new OutlineDialog());
        private void OnShadowRequested(object? sender, EventArgs e) => ShowEffectDialog(new ShadowDialog());
        private void OnGlowRequested(object? sender, EventArgs e) => ShowEffectDialog(new GlowDialog());
        private void OnReflectionRequested(object? sender, EventArgs e) => ShowEffectDialog(new ReflectionDialog());
        private void OnTornEdgeRequested(object? sender, EventArgs e) => ShowEffectDialog(new TornEdgeDialog());
        private void OnSliceRequested(object? sender, EventArgs e) => ShowEffectDialog(new SliceDialog());
        private void OnRoundedCornersRequested(object? sender, EventArgs e) => ShowEffectDialog(new RoundedCornersDialog());
        private void OnSkewRequested(object? sender, EventArgs e) => ShowEffectDialog(new SkewDialog());
        private void OnRotate3DRequested(object? sender, EventArgs e) => ShowEffectDialog(new Rotate3DDialog());
        private void OnRotate3DBoxRequested(object? sender, EventArgs e) => ShowEffectDialog(new Rotate3DBoxDialog());
        private void OnBlurRequested(object? sender, EventArgs e) => ShowEffectDialog(new BlurDialog());
        private void OnPixelateRequested(object? sender, EventArgs e) => ShowEffectDialog(new PixelateDialog());
        private void OnSharpenRequested(object? sender, EventArgs e) => ShowEffectDialog(new SharpenDialog());

        private void ShowEffectDialog<T>(T dialog) where T : UserControl, IEffectDialog
        {
            var vm = DataContext as MainViewModel;
            if (vm == null) return;

            // Initialize logic
            vm.StartEffectPreview();

            // Wire events using interface instead of dynamic
            dialog.PreviewRequested += (s, e) => vm.PreviewEffect(e.EffectOperation);
            dialog.ApplyRequested += (s, e) =>
            {
                vm.ApplyEffect(e.EffectOperation, e.StatusMessage);
                vm.CloseEffectsPanelCommand.Execute(null);
            };
            dialog.CancelRequested += (s, e) =>
            {
                vm.CancelEffectPreview();
                vm.CloseEffectsPanelCommand.Execute(null);
            };

            // If left sidebar is open, close it to avoid clutter? 
            // The request says "Side bar at right side won't cover the image preview at center".
            // So we can keep left sidebar open or close it. 
            // Usually only one "main" panel is active or both sidebars. 
            // Let's keep existing behavior for SettingsPanel (left) but ensure EffectsPanel (right) opens.

            vm.EffectsPanelContent = dialog;
            vm.IsEffectsPanelOpen = true;
        }

        private void OnModalBackgroundPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            // Only close if clicking on the background, not the dialog content
            if (e.Source == sender && DataContext is MainViewModel vm)
            {
                vm.CancelEffectPreview();
                vm.CloseModalCommand.Execute(null);
            }
        }

        /// <summary>
        /// Validates that UI annotation state is synchronized with EditorCore state.
        /// ISSUE-001 mitigation: Detect annotation count mismatches in dual-state architecture.
        /// </summary>
        private void ValidateAnnotationSync()
        {
            var canvas = this.FindControl<Canvas>("AnnotationCanvas");
            if (canvas == null) return;

            // Count UI annotations (exclude non-annotation controls like CropOverlay)
            int uiAnnotationCount = 0;
            foreach (var child in canvas.Children)
            {
                if (child is Control control && control.Tag is Annotation &&
                    control.Name != "CropOverlay" && control.Name != "CutOutOverlay")
                {
                    uiAnnotationCount++;
                }
            }

            int coreAnnotationCount = _editorCore.Annotations.Count;
        }
    }
}
