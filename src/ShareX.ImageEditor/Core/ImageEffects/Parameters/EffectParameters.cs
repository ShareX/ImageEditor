using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Parameters;

public static class EffectParameters
{
    public static SliderEffectParameter IntSlider<TEffect>(
        string key,
        string label,
        int minimum,
        int maximum,
        int defaultValue,
        Action<TEffect, int> applyValue,
        int tickFrequency = 1,
        bool isSnapToTickEnabled = true,
        string valueStringFormat = "{}{0:0}",
        string? description = null)
        where TEffect : ImageEffect
    {
        return new SliderEffectParameter(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            tickFrequency,
            isSnapToTickEnabled,
            valueStringFormat,
            (effect, value) => applyValue((TEffect)effect, (int)Math.Round(value)),
            description);
    }

    public static SliderEffectParameter FloatSlider<TEffect>(
        string key,
        string label,
        double minimum,
        double maximum,
        double defaultValue,
        Action<TEffect, float> applyValue,
        double tickFrequency = 1,
        bool isSnapToTickEnabled = true,
        string valueStringFormat = "{}{0:0}",
        string? description = null)
        where TEffect : ImageEffect
    {
        return new SliderEffectParameter(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            tickFrequency,
            isSnapToTickEnabled,
            valueStringFormat,
            (effect, value) => applyValue((TEffect)effect, (float)value),
            description);
    }

    public static CheckboxEffectParameter Bool<TEffect>(
        string key,
        string label,
        bool defaultValue,
        Action<TEffect, bool> applyValue,
        string? description = null)
        where TEffect : ImageEffect
    {
        return new CheckboxEffectParameter(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value),
            description);
    }

    public static EnumEffectParameter Enum<TEffect, TEnum>(
        string key,
        string label,
        TEnum defaultValue,
        Action<TEffect, TEnum> applyValue,
        IReadOnlyList<(string Label, TEnum Value)> options,
        string? description = null)
        where TEffect : ImageEffect
        where TEnum : notnull
    {
        int defaultIndex = -1;
        EffectOption[] effectOptions = new EffectOption[options.Count];

        for (int i = 0; i < options.Count; i++)
        {
            effectOptions[i] = new EffectOption(options[i].Label, options[i].Value!);
            if (EqualityComparer<TEnum>.Default.Equals(options[i].Value, defaultValue))
            {
                defaultIndex = i;
            }
        }

        if (defaultIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(defaultValue), defaultValue, "Enum default value must be present in the options list.");
        }

        return new EnumEffectParameter(
            key,
            label,
            effectOptions,
            defaultIndex,
            (effect, value) => applyValue((TEffect)effect, value is TEnum typedValue ? typedValue : defaultValue),
            description);
    }

    public static ColorEffectParameter Color<TEffect>(
        string key,
        string label,
        SKColor defaultValue,
        Action<TEffect, SKColor> applyValue,
        string? description = null)
        where TEffect : ImageEffect
    {
        return new ColorEffectParameter(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value),
            description);
    }

    public static NumericEffectParameter IntNumeric<TEffect>(
        string key,
        string label,
        int minimum,
        int maximum,
        int defaultValue,
        Action<TEffect, int> applyValue,
        int increment = 1,
        string formatString = "0",
        string? description = null)
        where TEffect : ImageEffect
    {
        return new NumericEffectParameter(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            increment,
            formatString,
            (effect, value) => applyValue((TEffect)effect, decimal.ToInt32(decimal.Round(value, 0, MidpointRounding.AwayFromZero))),
            description);
    }

    public static NumericEffectParameter DoubleNumeric<TEffect>(
        string key,
        string label,
        double minimum,
        double maximum,
        double defaultValue,
        Action<TEffect, double> applyValue,
        double increment = 1,
        string formatString = "0.0",
        string? description = null)
        where TEffect : ImageEffect
    {
        return new NumericEffectParameter(
            key,
            label,
            (decimal)minimum,
            (decimal)maximum,
            (decimal)defaultValue,
            (decimal)increment,
            formatString,
            (effect, value) => applyValue((TEffect)effect, (double)value),
            description);
    }

    public static TextEffectParameter Text<TEffect>(
        string key,
        string label,
        string defaultValue,
        Action<TEffect, string> applyValue,
        string? description = null)
        where TEffect : ImageEffect
    {
        return new TextEffectParameter(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value),
            description);
    }

    public static FilePathEffectParameter FilePath<TEffect>(
        string key,
        string label,
        string defaultValue,
        Action<TEffect, string> applyValue,
        string? fileFilter = null,
        string? description = null)
        where TEffect : ImageEffect
    {
        return new FilePathEffectParameter(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value),
            fileFilter,
            description);
    }
}
