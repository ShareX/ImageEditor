using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ShareX.ImageEditor.UI.Adapters.Converters;

public class MathMultiplyConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count != 2) return null;

        double result = 1.0;
        foreach (var val in values)
        {
            if (val is double d) result *= d;
            else if (val is int i) result *= i;
            else if (val is float f) result *= f;
            else if (val is decimal dec) result *= (double)dec;
            else return null;
        }

        return result;
    }
}
