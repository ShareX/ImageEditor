namespace ShareX.ImageEditor.Core.ImageEffects;

public sealed record ImageEffectContext(
    bool IsPreview,
    IServiceProvider? Services = null,
    CancellationToken CancellationToken = default);
