using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects;

public abstract class ImageEffectBase : ImageEffect
{
    public abstract string Id { get; }

    public virtual string Description => string.Empty;

    public virtual string BrowserLabel => ExecutionMode == EffectExecutionMode.Immediate ? Name : $"{Name}...";

    public virtual IReadOnlyList<EffectParameter> Parameters => [];

    public virtual EffectExecutionMode ExecutionMode => EffectExecutionMode.Previewable;

    public virtual string? EditorKey => null;

    public sealed override bool HasParameters => Parameters.Count > 0;

    public virtual SKBitmap Apply(SKBitmap source, ImageEffectContext context) => Apply(source);
}
