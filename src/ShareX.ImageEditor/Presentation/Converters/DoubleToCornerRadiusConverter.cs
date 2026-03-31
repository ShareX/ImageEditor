using Avalonia;
using Avalonia.Data.Converters;
using System.Globalization;

namespace ShareX.ImageEditor.Presentation.Converters
{
    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        public static readonly DoubleToCornerRadiusConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (TryGetRadius(value, out double radius))
            {
                var clamped = Math.Max(0, radius);
                return new CornerRadius(clamped);
            }

            return new CornerRadius(0);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
            {
                return cornerRadius.TopLeft;
            }

            return 0d;
        }

        private static bool TryGetRadius(object? value, out double radius)
        {
            switch (value)
            {
                case double doubleRadius when !double.IsNaN(doubleRadius) && !double.IsInfinity(doubleRadius):
                    radius = doubleRadius;
                    return true;
                case float floatRadius when !float.IsNaN(floatRadius) && !float.IsInfinity(floatRadius):
                    radius = floatRadius;
                    return true;
                case int intRadius:
                    radius = intRadius;
                    return true;
                case long longRadius:
                    radius = longRadius;
                    return true;
                case decimal decimalRadius:
                    radius = (double)decimalRadius;
                    return true;
                default:
                    radius = 0;
                    return false;
            }
        }
    }
}
