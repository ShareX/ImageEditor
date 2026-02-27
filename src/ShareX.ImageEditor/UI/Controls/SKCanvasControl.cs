using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using ShareX.ImageEditor.ImageEffects.Adjustments;
using ShareX.ImageEditor.Services;
using SkiaSharp;

namespace ShareX.ImageEditor.Controls;

/// <summary>
/// A control that allows direct SkiaSharp rendering into a WriteableBitmap.
/// This acts as the high-performance raster layer.
/// </summary>
public class SKCanvasControl : Control
{
    private WriteableBitmap? _bitmap;
    private object _lock = new object();

    // Inserted into the scene graph each frame until GPU registration succeeds.
    private readonly GpuLeaseCapture _gpuCapture = new GpuLeaseCapture();

    /// <summary>
    /// Initializes or resizes the backing store.
    /// </summary>
    public void Initialize(int width, int height)
    {
        if (width <= 0 || height <= 0) return;

        lock (_lock)
        {
            if (_bitmap?.PixelSize.Width == width && _bitmap?.PixelSize.Height == height)
                return;

            _bitmap?.Dispose();
            _bitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);
        }

        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        // Control.Render() receives a *recording* DrawingContext in Avalonia's deferred
        // renderer — it does NOT expose ISkiaSharpApiLeaseFeature.
        // ICustomDrawOperation.Render(ImmediateDrawingContext) IS called on the render
        // thread with the real GPU-backed context. We use it to capture the feature once.
        if (!_gpuCapture.IsRegistered)
            context.Custom(_gpuCapture);

        if (_bitmap != null)
            context.DrawImage(_bitmap, new Rect(0, 0, Bounds.Width, Bounds.Height));
    }

    /// <summary>
    /// Update the canvas using a SkiaSharp drawing action.
    /// </summary>
    public void Draw(Action<SKCanvas> drawAction)
    {
        if (_bitmap == null) return;

        lock (_lock)
        {
            using (var buffer = _bitmap.Lock())
            {
                var info = new SKImageInfo(
                    _bitmap.PixelSize.Width,
                    _bitmap.PixelSize.Height,
                    SKColorType.Bgra8888,
                    SKAlphaType.Premul);

                using (var surface = SKSurface.Create(info, buffer.Address, buffer.RowBytes))
                {
                    if (surface != null)
                        drawAction(surface.Canvas);
                }
            }
        }

        Avalonia.Threading.Dispatcher.UIThread.Post(InvalidateVisual, Avalonia.Threading.DispatcherPriority.Render);
    }

    /// <summary>
    /// Releases resources.
    /// </summary>
    public void Dispose()
    {
        _gpuCapture.Dispose();
        _bitmap?.Dispose();
        _bitmap = null;
    }

    /// <summary>
    /// Scene-graph node that captures <see cref="ISkiaSharpApiLeaseFeature"/> on the first
    /// render-thread invocation. Once captured, registers a <see cref="SkiaSharpLeaseProvider"/>
    /// with <see cref="ImageEffect"/> so that large-image color-filter effects use the GPU path.
    /// Becomes a no-op after successful registration.
    /// </summary>
    private sealed class GpuLeaseCapture : ICustomDrawOperation
    {
        // Written once on render thread; read on UI thread. Volatile prevents stale reads.
        internal volatile bool IsRegistered;

        // Non-empty so Avalonia's scene graph does not cull this node.
        public Rect Bounds => new Rect(0, 0, 1, 1);

        public bool HitTest(Point p) => false;

        public bool Equals(ICustomDrawOperation? other) => ReferenceEquals(this, other);

        public void Render(ImmediateDrawingContext context)
        {
            if (IsRegistered) return;

            // ImmediateDrawingContext implements IOptionalFeatureProvider and exposes
            // ISkiaSharpApiLeaseFeature when a GPU backend (GL/Vulkan) is active.
            var feature = (context as IOptionalFeatureProvider)?.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            if (feature != null)
            {
                ImageEffect.SetGpuLeaseProvider(new SkiaSharpLeaseProvider(feature));
                IsRegistered = true;
            }
        }

        public void Dispose() { }
    }

    /// <summary>
    /// Adapts <see cref="ISkiaSharpApiLeaseFeature"/> to <see cref="IEffectGpuLeaseProvider"/>.
    /// Each <see cref="TryWithGrContext"/> call acquires a fresh GL context lease, making the
    /// context current on the calling thread and releasing it on dispose.
    /// </summary>
    private sealed class SkiaSharpLeaseProvider : IEffectGpuLeaseProvider
    {
        private readonly ISkiaSharpApiLeaseFeature _leaseFeature;

        public SkiaSharpLeaseProvider(ISkiaSharpApiLeaseFeature leaseFeature)
            => _leaseFeature = leaseFeature;

        public SKBitmap? TryWithGrContext(Func<GRContext, SKBitmap?> gpuWork)
        {
            using var lease = _leaseFeature.Lease();
            var grContext = lease?.GrContext;
            if (grContext == null || grContext.IsAbandoned)
                return null;

            return gpuWork(grContext);
        }
    }
}
