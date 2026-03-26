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

namespace ShareX.ImageEditor.Presentation.Effects;

public abstract partial class EffectParameterState : ObservableObject
{
    protected EffectParameterState(EffectParameterDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }

    public EffectParameterDefinition Definition { get; }

    public string Key => Definition.Key;

    public string Label => Definition.Label;

    internal abstract object? GetValue();
}

public sealed partial class SliderParameterState : EffectParameterState
{
    [ObservableProperty]
    private double _value;

    public SliderParameterDefinition SliderDefinition => (SliderParameterDefinition)Definition;

    public double Minimum => SliderDefinition.Minimum;

    public double Maximum => SliderDefinition.Maximum;

    public double TickFrequency => SliderDefinition.TickFrequency;

    public bool IsSnapToTickEnabled => SliderDefinition.IsSnapToTickEnabled;

    public string ValueStringFormat => SliderDefinition.ValueStringFormat;

    public SliderParameterState(SliderParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class CheckboxParameterState : EffectParameterState
{
    [ObservableProperty]
    private bool _value;

    public CheckboxParameterState(CheckboxParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class EnumParameterState : EffectParameterState
{
    [ObservableProperty]
    private EffectOptionDefinition _selectedOption;

    public EnumParameterDefinition EnumDefinition => (EnumParameterDefinition)Definition;

    public IReadOnlyList<EffectOptionDefinition> Options => EnumDefinition.Options;

    public EnumParameterState(EnumParameterDefinition definition)
        : base(definition)
    {
        _selectedOption = definition.Options[definition.DefaultIndex];
    }

    internal override object? GetValue() => SelectedOption.Value;
}

public sealed partial class ColorParameterState : EffectParameterState
{
    [ObservableProperty]
    private Color _value;

    public ColorParameterState(ColorParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class NumericParameterState : EffectParameterState
{
    [ObservableProperty]
    private decimal? _value;

    public NumericParameterDefinition NumericDefinition => (NumericParameterDefinition)Definition;

    public decimal Minimum => NumericDefinition.Minimum;

    public decimal Maximum => NumericDefinition.Maximum;

    public decimal Increment => NumericDefinition.Increment;

    public string FormatString => NumericDefinition.FormatString;

    public NumericParameterState(NumericParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class TextParameterState : EffectParameterState
{
    [ObservableProperty]
    private string _value;

    public TextParameterState(TextParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}

public sealed partial class FilePathParameterState : EffectParameterState
{
    [ObservableProperty]
    private string _value;

    public FilePathParameterDefinition FilePathDefinition => (FilePathParameterDefinition)Definition;

    public string? FileFilter => FilePathDefinition.FileFilter;

    public FilePathParameterState(FilePathParameterDefinition definition)
        : base(definition)
    {
        _value = definition.DefaultValue;
    }

    internal override object? GetValue() => Value;
}
