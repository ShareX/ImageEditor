using ShareX.ImageEditor.Core.ImageEffects;

namespace ShareX.ImageEditor.Presentation.Effects;

internal sealed record EditorOperationDefinition(
    string Id,
    string BrowserLabel,
    string Icon,
    string Description,
    ImageEffectCategory Category,
    EditorOperationKind Kind);
