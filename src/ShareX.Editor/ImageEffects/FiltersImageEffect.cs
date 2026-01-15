using SkiaSharp;

namespace ShareX.Editor.ImageEffects;

public abstract class FiltersImageEffect : ImageEffect
{
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
}
