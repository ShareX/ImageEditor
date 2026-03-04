using SkiaSharp;

namespace ShareX.ImageEditor.Services;

/// <summary>
/// Host-provided GPU context for ImageEditor effects.
/// Implementations may use a persistent off-screen GRContext (Option B),
/// Avalonia's render-thread lease APIs, or any other backend-specific
/// mechanism. The library remains agnostic and falls back to CPU when
/// this provider is not set or returns null.
/// </summary>
public interface IEditorGpuContextProvider
{
    /// <summary>
    /// Attempts to run a color-filter effect on the GPU.
    /// </summary>
    /// <param name="source">Source bitmap (not owned by the provider).</param>
    /// <param name="filter">Color filter to apply (not owned by the provider).</param>
    /// <param name="pixelCount">Total pixel count (width * height) of the source.</param>
    /// <param name="effectName">Optional effect identifier for diagnostics.</param>
    /// <returns>
    /// A new <see cref="SKBitmap"/> containing the filtered result, or
    /// <c>null</c> if GPU processing is unavailable or fails and the caller
    /// should fall back to the CPU path.
    /// </returns>
    SKBitmap? TryRunColorFilter(SKBitmap source, SKColorFilter filter, int pixelCount, string effectName);
}

