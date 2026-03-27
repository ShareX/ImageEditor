using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public enum SymmetryDirection
{
    LeftToRight,
    RightToLeft,
    TopToBottom,
    BottomToTop
}

public sealed class SymmetryImageEffect : ImageEffectBase
{
    public override string Id => "symmetry";
    public override string Name => "Symmetry";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => LucideIcons.flip_horizontal_2;
    public override string Description => "Creates bilateral symmetry by mirroring one half of the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.Enum<SymmetryImageEffect, SymmetryDirection>(
            "direction", "Direction", SymmetryDirection.LeftToRight, (e, v) => e.Direction = v,
            new (string, SymmetryDirection)[]
            {
                ("Left \u2192 Right", SymmetryDirection.LeftToRight),
                ("Right \u2192 Left", SymmetryDirection.RightToLeft),
                ("Top \u2192 Bottom", SymmetryDirection.TopToBottom),
                ("Bottom \u2192 Top", SymmetryDirection.BottomToTop)
            })
    ];

    public SymmetryDirection Direction { get; set; } = SymmetryDirection.LeftToRight;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);

        int w = source.Width, h = source.Height;
        SKRect srcRect, dstRect;

        switch (Direction)
        {
            case SymmetryDirection.LeftToRight:
                srcRect = new SKRect(0, 0, w / 2f, h);
                dstRect = new SKRect(w, 0, w / 2f, h);
                canvas.Save();
                canvas.Scale(-1, 1, w / 2f, 0);
                canvas.DrawBitmap(source, srcRect, srcRect);
                canvas.Restore();
                break;
            case SymmetryDirection.RightToLeft:
                srcRect = new SKRect(w / 2f, 0, w, h);
                canvas.Save();
                canvas.Scale(-1, 1, w / 2f, 0);
                canvas.DrawBitmap(source, srcRect, srcRect);
                canvas.Restore();
                break;
            case SymmetryDirection.TopToBottom:
                srcRect = new SKRect(0, 0, w, h / 2f);
                canvas.Save();
                canvas.Scale(1, -1, 0, h / 2f);
                canvas.DrawBitmap(source, srcRect, srcRect);
                canvas.Restore();
                break;
            case SymmetryDirection.BottomToTop:
                srcRect = new SKRect(0, h / 2f, w, h);
                canvas.Save();
                canvas.Scale(1, -1, 0, h / 2f);
                canvas.DrawBitmap(source, srcRect, srcRect);
                canvas.Restore();
                break;
        }

        return result;
    }
}
