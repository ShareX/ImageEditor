using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace ShareX.ImageEditor.Helpers
{
    public static class ThemeManager
    {
        public static readonly ThemeVariant ShareX = new ThemeVariant("ShareX", ThemeVariant.Dark);

        public static event EventHandler<ThemeVariant>? ThemeChanged;

        public static void SetTheme(ThemeVariant theme, object? target = null)
        {
            if (target is Application app)
            {
                app.RequestedThemeVariant = theme;
            }
            else if (target is Window window)
            {
                window.RequestedThemeVariant = theme;
            }
            else if (target is ThemeVariantScope scope)
            {
                scope.RequestedThemeVariant = theme;
            }
            else if (target == null && Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = theme;
            }

            ThemeChanged?.Invoke(null, theme);
        }

        public static ThemeVariant GetCurrentTheme()
        {
            return Application.Current?.RequestedThemeVariant ?? ThemeVariant.Default;
        }
    }
}
