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
using ShareX.ImageEditor.Core.ImageEffects;

namespace ShareX.ImageEditor.Presentation.Effects;

public abstract class EffectParameterDefinition
{
    protected EffectParameterDefinition(string key, string label)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Label = label ?? throw new ArgumentNullException(nameof(label));
    }

    public string Key { get; }

    public string Label { get; }

    internal abstract EffectParameterState CreateState();

    internal abstract void ApplyValue(ImageEffect effect, object? value);
}

public sealed class SliderParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, double> _applyValue;

    public double Minimum { get; }

    public double Maximum { get; }

    public double DefaultValue { get; }

    public double TickFrequency { get; }

    public bool IsSnapToTickEnabled { get; }

    public string ValueStringFormat { get; }

    public SliderParameterDefinition(
        string key,
        string label,
        double minimum,
        double maximum,
        double defaultValue,
        double tickFrequency,
        bool isSnapToTickEnabled,
        string valueStringFormat,
        Action<ImageEffect, double> applyValue)
        : base(key, label)
    {
        Minimum = minimum;
        Maximum = maximum;
        DefaultValue = defaultValue;
        TickFrequency = tickFrequency;
        IsSnapToTickEnabled = isSnapToTickEnabled;
        ValueStringFormat = valueStringFormat ?? throw new ArgumentNullException(nameof(valueStringFormat));
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
    }

    internal override EffectParameterState CreateState() => new SliderParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        _applyValue(effect, value is double numericValue ? numericValue : DefaultValue);
    }
}

public sealed class CheckboxParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, bool> _applyValue;

    public bool DefaultValue { get; }

    public CheckboxParameterDefinition(
        string key,
        string label,
        bool defaultValue,
        Action<ImageEffect, bool> applyValue)
        : base(key, label)
    {
        DefaultValue = defaultValue;
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
    }

    internal override EffectParameterState CreateState() => new CheckboxParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        _applyValue(effect, value is bool booleanValue ? booleanValue : DefaultValue);
    }
}

public sealed class EnumParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, object> _applyValue;

    public IReadOnlyList<EffectOptionDefinition> Options { get; }

    public int DefaultIndex { get; }

    public EnumParameterDefinition(
        string key,
        string label,
        IReadOnlyList<EffectOptionDefinition> options,
        int defaultIndex,
        Action<ImageEffect, object> applyValue)
        : base(key, label)
    {
        if (options is null || options.Count == 0)
        {
            throw new ArgumentException("Enum options must not be empty.", nameof(options));
        }

        if (defaultIndex < 0 || defaultIndex >= options.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(defaultIndex));
        }

        Options = options;
        DefaultIndex = defaultIndex;
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
    }

    internal override EffectParameterState CreateState() => new EnumParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        object resolvedValue = value ?? Options[DefaultIndex].Value;
        _applyValue(effect, resolvedValue);
    }
}

public sealed class ColorParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, Color> _applyValue;

    public Color DefaultValue { get; }

    public ColorParameterDefinition(
        string key,
        string label,
        Color defaultValue,
        Action<ImageEffect, Color> applyValue)
        : base(key, label)
    {
        DefaultValue = defaultValue;
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
    }

    internal override EffectParameterState CreateState() => new ColorParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        _applyValue(effect, value is Color colorValue ? colorValue : DefaultValue);
    }
}

public sealed class NumericParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, decimal> _applyValue;

    public decimal Minimum { get; }

    public decimal Maximum { get; }

    public decimal DefaultValue { get; }

    public decimal Increment { get; }

    public string FormatString { get; }

    public NumericParameterDefinition(
        string key,
        string label,
        decimal minimum,
        decimal maximum,
        decimal defaultValue,
        decimal increment,
        string formatString,
        Action<ImageEffect, decimal> applyValue)
        : base(key, label)
    {
        Minimum = minimum;
        Maximum = maximum;
        DefaultValue = defaultValue;
        Increment = increment;
        FormatString = formatString ?? throw new ArgumentNullException(nameof(formatString));
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
    }

    internal override EffectParameterState CreateState() => new NumericParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        decimal resolvedValue = value switch
        {
            decimal decimalValue => decimalValue,
            double doubleValue => (decimal)doubleValue,
            int intValue => intValue,
            null => DefaultValue,
            _ => DefaultValue
        };

        _applyValue(effect, resolvedValue);
    }
}

public sealed class TextParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, string> _applyValue;

    public string DefaultValue { get; }

    public TextParameterDefinition(
        string key,
        string label,
        string defaultValue,
        Action<ImageEffect, string> applyValue)
        : base(key, label)
    {
        DefaultValue = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
    }

    internal override EffectParameterState CreateState() => new TextParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        _applyValue(effect, value as string ?? DefaultValue);
    }
}

public sealed class FilePathParameterDefinition : EffectParameterDefinition
{
    private readonly Action<ImageEffect, string> _applyValue;

    public string DefaultValue { get; }

    /// <summary>
    /// Optional file type filter string for the file picker dialog.
    /// Example: "Image files|*.png;*.jpg;*.bmp"
    /// </summary>
    public string? FileFilter { get; }

    public FilePathParameterDefinition(
        string key,
        string label,
        string defaultValue,
        Action<ImageEffect, string> applyValue,
        string? fileFilter = null)
        : base(key, label)
    {
        DefaultValue = defaultValue ?? "";
        _applyValue = applyValue ?? throw new ArgumentNullException(nameof(applyValue));
        FileFilter = fileFilter;
    }

    internal override EffectParameterState CreateState() => new FilePathParameterState(this);

    internal override void ApplyValue(ImageEffect effect, object? value)
    {
        _applyValue(effect, value as string ?? DefaultValue);
    }
}

public sealed class EffectOptionDefinition
{
    public string Label { get; }

    public object Value { get; }

    public EffectOptionDefinition(string label, object value)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString() => Label;
}
