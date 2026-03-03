using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.ImageEditor.ImageEffects.Filters;

namespace ShareX.ImageEditor.Views.Dialogs;

public partial class RainReflectionPuddlesDialog : UserControl, IEffectDialog
{
    public event EventHandler<EffectEventArgs>? ApplyRequested;
    public event EventHandler<EffectEventArgs>? PreviewRequested;
    public event EventHandler? CancelRequested;

    public RainReflectionPuddlesDialog()
    {
        AvaloniaXamlLoader.Load(this);
        AttachedToVisualTree += (s, e) => RequestPreview();
    }

    private float GetValue(string controlName, double fallback)
    {
        return (float)(this.FindControl<Slider>(controlName)?.Value ?? fallback);
    }

    private RainReflectionPuddlesImageEffect CreateEffect()
    {
        return new RainReflectionPuddlesImageEffect
        {
            PuddleAmount = GetValue("PuddleAmountSlider", 55d),
            ReflectionStrength = GetValue("ReflectionStrengthSlider", 65d),
            RippleStrength = GetValue("RippleStrengthSlider", 30d),
            Wetness = GetValue("WetnessSlider", 35d),
            Horizon = GetValue("HorizonSlider", 58d)
        };
    }

    private void OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (!IsLoaded) return;
        RequestPreview();
    }

    private void RequestPreview()
    {
        PreviewRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Rain reflection puddles"));
    }

    private void OnApplyClick(object? sender, RoutedEventArgs e)
    {
        ApplyRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Applied Rain reflection puddles"));
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }
}
