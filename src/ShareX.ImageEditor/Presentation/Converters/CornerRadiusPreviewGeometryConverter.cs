using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace ShareX.ImageEditor.Presentation.Converters
{
    public class CornerRadiusPreviewGeometryConverter : IValueConverter
    {
        public static readonly CornerRadiusPreviewGeometryConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double size = 30;

            if (parameter is string sizeText && double.TryParse(sizeText, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedSize) && parsedSize > 0)
            {
                size = parsedSize;
            }

            if (!TryGetRadius(value, out double radius))
            {
                radius = 0;
            }

            double clampedRadius = Math.Clamp(radius, 0, size);
            string sizeValue = size.ToString(CultureInfo.InvariantCulture);

            if (clampedRadius <= 0.01d)
            {
                return Geometry.Parse($"M 0,{sizeValue} L 0,0 L {sizeValue},0");
            }

            string radiusValue = clampedRadius.ToString(CultureInfo.InvariantCulture);
            return Geometry.Parse($"M 0,{sizeValue} L 0,{radiusValue} A {radiusValue},{radiusValue} 0 0 1 {radiusValue},0 L {sizeValue},0");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
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
