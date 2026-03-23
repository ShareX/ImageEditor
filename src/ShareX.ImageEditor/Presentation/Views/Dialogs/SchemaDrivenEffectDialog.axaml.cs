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
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ShareX.ImageEditor.Presentation.Effects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShareX.ImageEditor.Presentation.Views.Dialogs;

public partial class SchemaDrivenEffectDialog : UserControl, IEffectDialog
{
    private bool _isReady;

    public EffectDefinition Definition { get; }

    public ObservableCollection<EffectParameterState> ParameterStates { get; }

    public string Title => Definition.Name;

    public event EventHandler<EffectEventArgs>? ApplyRequested;

    public event EventHandler<EffectEventArgs>? PreviewRequested;

    public event EventHandler? CancelRequested;

    public SchemaDrivenEffectDialog()
        : this(ImageEffectCatalog.Definitions[0])
    {
    }

    public SchemaDrivenEffectDialog(EffectDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        ParameterStates = new ObservableCollection<EffectParameterState>(definition.Parameters.Select(parameter => parameter.CreateState()));

        foreach (EffectParameterState parameterState in ParameterStates)
        {
            parameterState.PropertyChanged += OnParameterStateChanged;
        }

        AvaloniaXamlLoader.Load(this);
        DataContext = this;

        AttachedToVisualTree += OnAttachedToVisualTree;
        DetachedFromVisualTree += OnDetachedFromVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        // ContentControl's ContentPresenter can replace an inherited DataContext with the parent's
        // (e.g. MainViewModel). Parameter bindings then fail silently and preview/apply keep defaults.
        // Old bespoke dialogs used FindControl and did not depend on DataContext; schema-driven UI does.
        DataContext = this;
        _isReady = true;
        RequestPreview();
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _isReady = false;
    }

    private void OnParameterStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!_isReady)
        {
            return;
        }

        RequestPreview();
    }

    private void RequestPreview()
    {
        PreviewRequested?.Invoke(this, BuildEffectEventArgs(Definition.Name));
    }

    private void OnApplyClick(object? sender, RoutedEventArgs e)
    {
        ApplyRequested?.Invoke(this, BuildEffectEventArgs($"Applied {Definition.Name}"));
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }

    private EffectEventArgs BuildEffectEventArgs(string statusMessage)
    {
        return new EffectEventArgs(
            img => Definition.CreateConfiguredEffect(ParameterStates).Apply(img),
            statusMessage);
    }

    private async void OnBrowseFilePathClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not FilePathParameterState parameterState)
        {
            return;
        }

        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider == null)
        {
            return;
        }

        IReadOnlyList<FilePickerFileType> fileTypes = ParseFileTypes(parameterState.FileFilter);
        IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = $"Select {parameterState.Label}",
            AllowMultiple = false,
            FileTypeFilter = fileTypes
        });

        if (files.Count > 0)
        {
            parameterState.Value = files[0].Path.LocalPath;
        }
    }

    private static IReadOnlyList<FilePickerFileType> ParseFileTypes(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return [FilePickerFileTypes.All];
        }

        string[] parts = filter.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            return [FilePickerFileTypes.All];
        }

        string[] patterns = parts[1].Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (patterns.Length == 0)
        {
            return [FilePickerFileTypes.All];
        }

        return
        [
            new FilePickerFileType(parts[0])
            {
                Patterns = patterns
            }
        ];
    }
}
