using SkiaSharp;

namespace ShareX.ImageEditor.ImageEffects.Drawings;

public enum DrawingPlacement
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}

public enum DrawingImageSizeMode
{
    DontResize,
    AbsoluteSize,
    PercentageOfWatermark,
    PercentageOfCanvas
}

public enum DrawingImageRotateFlipType
{
    None = 0,
    Rotate90 = 1,
    Rotate180 = 2,
    Rotate270 = 3,
    FlipX = 4,
    Rotate90FlipX = 5,
    FlipY = 6,
    Rotate90FlipY = 7
}

public enum DrawingInterpolationMode
{
    HighQualityBicubic,
    Bicubic,
    HighQualityBilinear,
    Bilinear,
    NearestNeighbor
}

public enum DrawingCompositingMode
{
    SourceOver,
    SourceCopy
}

public enum DrawingGradientMode
{
    Vertical,
    Horizontal,
    ForwardDiagonal,
    BackwardDiagonal
}

public sealed class DrawingGradientStop
{
    private float _location;

    public SKColor Color { get; set; } = SKColors.Black;

    public float Location
    {
        get => _location;
        set => _location = Math.Clamp(value, 0f, 100f);
    }

    public DrawingGradientStop()
    {
    }

    public DrawingGradientStop(SKColor color, float location)
    {
        Color = color;
        Location = location;
    }
}

