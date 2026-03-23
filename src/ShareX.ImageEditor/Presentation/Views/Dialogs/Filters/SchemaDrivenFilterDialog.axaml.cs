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
using ShareX.ImageEditor.Presentation.Filters;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShareX.ImageEditor.Presentation.Views.Dialogs;

public partial class SchemaDrivenFilterDialog : UserControl, IEffectDialog
{
    private bool _isReady;

    public FilterDefinition Definition { get; }

    public ObservableCollection<FilterParameterState> ParameterStates { get; }

    public string Title => Definition.Name;

    public event EventHandler<EffectEventArgs>? ApplyRequested;

    public event EventHandler<EffectEventArgs>? PreviewRequested;

    public event EventHandler? CancelRequested;

    public SchemaDrivenFilterDialog()
        : this(FilterCatalog.Definitions[0])
    {
    }

    public SchemaDrivenFilterDialog(FilterDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        ParameterStates = new ObservableCollection<FilterParameterState>(definition.Parameters.Select(parameter => parameter.CreateState()));

        foreach (FilterParameterState parameterState in ParameterStates)
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
}
