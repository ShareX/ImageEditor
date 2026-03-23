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

using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShareX.ImageEditor.Presentation.Filters;

public abstract partial class FilterParameterState : ObservableObject
{
    protected FilterParameterState(FilterParameterDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }

    public FilterParameterDefinition Definition { get; }

    public string Key => Definition.Key;

    public string Label => Definition.Label;

    internal abstract object? GetValue();
}

public sealed partial class SliderFilterParameterState : FilterParameterState
{
    [ObservableProperty]
    private double _value;

    public SliderFilterParameterDefinition SliderDefinition => (SliderFilterParameterDefinition)Definition;

    public double Minimum => SliderDefinition.Minimum;

    public double Maximum => SliderDefinition.Maximum;

    public double TickFrequency => SliderDefinition.TickFrequency;

    public bool IsSnapToTickEnabled => SliderDefinition.IsSnapToTickEnabled;

    public string ValueStringFormat => SliderDefinition.ValueStringFormat;

    public SliderFilterParameterState(SliderFilterParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class CheckboxFilterParameterState : FilterParameterState
{
    [ObservableProperty]
    private bool _value;

    public CheckboxFilterParameterState(CheckboxFilterParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class EnumFilterParameterState : FilterParameterState
{
    [ObservableProperty]
    private FilterOptionDefinition _selectedOption;

    public EnumFilterParameterDefinition EnumDefinition => (EnumFilterParameterDefinition)Definition;

    public IReadOnlyList<FilterOptionDefinition> Options => EnumDefinition.Options;

    public EnumFilterParameterState(EnumFilterParameterDefinition definition)
        : base(definition)
    {
        _selectedOption = definition.Options[definition.DefaultIndex];
    }

    internal override object? GetValue() => SelectedOption.Value;
}

public sealed partial class ColorFilterParameterState : FilterParameterState
{
    [ObservableProperty]
    private Color _value;

    public ColorFilterParameterState(ColorFilterParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class NumericFilterParameterState : FilterParameterState
{
    [ObservableProperty]
    private decimal? _value;

    public NumericFilterParameterDefinition NumericDefinition => (NumericFilterParameterDefinition)Definition;

    public decimal Minimum => NumericDefinition.Minimum;

    public decimal Maximum => NumericDefinition.Maximum;

    public decimal Increment => NumericDefinition.Increment;

    public string FormatString => NumericDefinition.FormatString;

    public NumericFilterParameterState(NumericFilterParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class TextFilterParameterState : FilterParameterState
{
    [ObservableProperty]
    private string _value;

    public TextFilterParameterState(TextFilterParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}
