#region License Information (GPL v3)

/*
    ShareX.Editor - The UI-agnostic Editor library for ShareX
    Copyright (c) 2007-2025 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using SkiaSharp;

namespace ShareX.Editor.Helpers;

/// <summary>
/// Image manipulation utilities for the Editor library.
/// </summary>
public static class ImageHelpers
{
    public static SKBitmap Crop(SKBitmap bitmap, SKRectI rect)
    {
        if (bitmap is null) throw new ArgumentNullException(nameof(bitmap));

        SKRectI bounded = new SKRectI(
            Math.Max(0, rect.Left),
            Math.Max(0, rect.Top),
            Math.Min(bitmap.Width, rect.Right),
            Math.Min(bitmap.Height, rect.Bottom));

        if (bounded.Width <= 0 || bounded.Height <= 0)
        {
            return new SKBitmap();
        }

        SKBitmap subset = new SKBitmap(bounded.Width, bounded.Height);
        return bitmap.ExtractSubset(subset, bounded) ? subset : new SKBitmap();
    }

    public static SKBitmap Crop(SKBitmap bitmap, int x, int y, int width, int height)
    {
        return Crop(bitmap, new SKRectI(x, y, x + width, y + height));
    }

    /// <summary>
    /// Cut out a horizontal or vertical section from the image and join the remaining parts
    /// </summary>
    /// <param name="bitmap">Source bitmap</param>
    /// <param name="startPos">Start position (X for vertical cut, Y for horizontal cut)</param>
    /// <param name="endPos">End position (X for vertical cut, Y for horizontal cut)</param>
    /// <param name="isVertical">True for vertical cut (left-right), false for horizontal cut (top-bottom)</param>
    /// <returns>New bitmap with the section cut out</returns>
    public static SKBitmap CutOut(SKBitmap bitmap, int startPos, int endPos, bool isVertical)
    {
        if (bitmap is null) throw new ArgumentNullException(nameof(bitmap));

        // Ensure start is before end
        if (startPos > endPos)
        {
            (startPos, endPos) = (endPos, startPos);
        }

        if (isVertical)
        {
            // Vertical cut: remove columns from startPos to endPos
            int cutWidth = endPos - startPos;
            if (cutWidth <= 0 || cutWidth >= bitmap.Width)
            {
                return bitmap; // Nothing to cut or invalid range
            }

            int newWidth = bitmap.Width - cutWidth;
            int newHeight = bitmap.Height;

            SKBitmap result = new SKBitmap(newWidth, newHeight);
            using SKCanvas canvas = new SKCanvas(result);

            // Draw left part (0 to startPos)
            if (startPos > 0)
            {
                SKRect srcLeft = new SKRect(0, 0, startPos, bitmap.Height);
                SKRect dstLeft = new SKRect(0, 0, startPos, newHeight);
                canvas.DrawBitmap(bitmap, srcLeft, dstLeft);
            }

            // Draw right part (endPos to width), positioned after left part
            if (endPos < bitmap.Width)
            {
                SKRect srcRight = new SKRect(endPos, 0, bitmap.Width, bitmap.Height);
                SKRect dstRight = new SKRect(startPos, 0, newWidth, newHeight);
                canvas.DrawBitmap(bitmap, srcRight, dstRight);
            }

            return result;
        }
        else
        {
            // Horizontal cut: remove rows from startPos to endPos
            int cutHeight = endPos - startPos;
            if (cutHeight <= 0 || cutHeight >= bitmap.Height)
            {
                return bitmap; // Nothing to cut or invalid range
            }

            int newWidth = bitmap.Width;
            int newHeight = bitmap.Height - cutHeight;

            SKBitmap result = new SKBitmap(newWidth, newHeight);
            using SKCanvas canvas = new SKCanvas(result);

            // Draw top part (0 to startPos)
            if (startPos > 0)
            {
                SKRect srcTop = new SKRect(0, 0, bitmap.Width, startPos);
                SKRect dstTop = new SKRect(0, 0, newWidth, startPos);
                canvas.DrawBitmap(bitmap, srcTop, dstTop);
            }

            // Draw bottom part (endPos to height), positioned after top part
            if (endPos < bitmap.Height)
            {
                SKRect srcBottom = new SKRect(0, endPos, bitmap.Width, bitmap.Height);
                SKRect dstBottom = new SKRect(0, startPos, newWidth, newHeight);
                canvas.DrawBitmap(bitmap, srcBottom, dstBottom);
            }

            return result;
        }
    }

    public static void SaveBitmap(SKBitmap bitmap, string filePath, int quality = 100)
    {
        if (bitmap is null) throw new ArgumentNullException(nameof(bitmap));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? string.Empty);

        SKEncodedImageFormat format = GetEncodedFormat(filePath);
        using SKImage image = SKImage.FromBitmap(bitmap);
        using SKData data = image.Encode(format, quality);
        using FileStream stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        data.SaveTo(stream);
    }

    private static SKEncodedImageFormat GetEncodedFormat(string filePath)
    {
        string extension = Path.GetExtension(filePath)?.TrimStart('.').ToLowerInvariant() ?? string.Empty;
        return extension switch
        {
            "jpg" or "jpeg" => SKEncodedImageFormat.Jpeg,
            "bmp" => SKEncodedImageFormat.Bmp,
            "gif" => SKEncodedImageFormat.Gif,
            "webp" => SKEncodedImageFormat.Webp,
            _ => SKEncodedImageFormat.Png
        };
    }

    /// <summary>
    /// Resize a bitmap to the specified dimensions with optional aspect ratio preservation
    /// </summary>
    /// <param name="source">Source bitmap to resize</param>
    /// <param name="width">Target width</param>
    /// <param name="height">Target height</param>
    /// <param name="maintainAspectRatio">If true, scales proportionally to fit within width/height</param>
    /// <param name="quality">Filter quality for scaling</param>
    /// <returns>New resized bitmap</returns>
    public static SKBitmap Resize(SKBitmap source, int width, int height, bool maintainAspectRatio = false, SKFilterQuality quality = SKFilterQuality.High)
    {
        // Quality ignored in Effect currently or hardcoded to High
        return new ShareX.Editor.ImageEffects.ManipulationsResizeImageEffect(width, height, maintainAspectRatio).Apply(source);
    }

    /// <summary>
    /// Rotate bitmap 90 degrees clockwise
    /// </summary>
    public static SKBitmap Rotate90Clockwise(SKBitmap source)
    {
        return ShareX.Editor.ImageEffects.ManipulationsRotateImageEffect.Clockwise90.Apply(source);
    }

    /// <summary>
    /// Rotate bitmap 90 degrees counter-clockwise
    /// </summary>
    public static SKBitmap Rotate90CounterClockwise(SKBitmap source)
    {
        return ShareX.Editor.ImageEffects.ManipulationsRotateImageEffect.CounterClockwise90.Apply(source);
    }

    /// <summary>
    /// Rotate bitmap 180 degrees
    /// </summary>
    public static SKBitmap Rotate180(SKBitmap source)
    {
        return ShareX.Editor.ImageEffects.ManipulationsRotateImageEffect.Rotate180.Apply(source);
    }

    /// <summary>
    /// Flip bitmap horizontally (mirror left-to-right)
    /// </summary>
    /// <summary>
    /// Flip bitmap horizontally (mirror left-to-right)
    /// </summary>
    public static SKBitmap FlipHorizontal(SKBitmap source)
    {
        return ShareX.Editor.ImageEffects.ManipulationsFlipImageEffect.Horizontal.Apply(source);
    }

    /// <summary>
    /// Flip bitmap vertically (mirror top-to-bottom)
    /// </summary>
    /// <summary>
    /// Flip bitmap vertically (mirror top-to-bottom)
    /// </summary>
    public static SKBitmap FlipVertical(SKBitmap source)
    {
        return ShareX.Editor.ImageEffects.ManipulationsFlipImageEffect.Vertical.Apply(source);
    }

    /// <summary>
    /// Resize canvas by adding padding around the image
    /// </summary>
    /// <param name="source">Source bitmap</param>
    /// <param name="left">Left padding in pixels</param>
    /// <param name="top">Top padding in pixels</param>
    /// <param name="right">Right padding in pixels</param>
    /// <param name="bottom">Bottom padding in pixels</param>
    /// <param name="backgroundColor">Background color for the padding</param>
    /// <returns>New bitmap with padding</returns>
    public static SKBitmap ResizeCanvas(SKBitmap source, int left, int top, int right, int bottom, SKColor backgroundColor)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int newWidth = source.Width + left + right;
        int newHeight = source.Height + top + bottom;

        if (newWidth <= 0 || newHeight <= 0)
        {
            throw new ArgumentException("Canvas size after padding must be greater than 0");
        }

        SKBitmap result = new SKBitmap(newWidth, newHeight, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            canvas.Clear(backgroundColor);
            canvas.DrawBitmap(source, left, top);
        }
        return result;
    }

    /// <summary>
    /// Auto-crops the image by removing edges that match the specified color within tolerance
    /// </summary>
    /// <param name="source">Source bitmap</param>
    /// <param name="color">Color to match for cropping</param>
    /// <param name="tolerance">Color tolerance (0-255)</param>
    /// <returns>Cropped bitmap</returns>
    public static SKBitmap AutoCrop(SKBitmap source, SKColor color, int tolerance = 0)
    {
        return new ShareX.Editor.ImageEffects.ManipulationsAutoCropImageEffect(color, tolerance).Apply(source);
    }

    internal static bool ColorsMatch(SKColor c1, SKColor c2, int tolerance)
    {
        return Math.Abs(c1.Red - c2.Red) <= tolerance &&
               Math.Abs(c1.Green - c2.Green) <= tolerance &&
               Math.Abs(c1.Blue - c2.Blue) <= tolerance &&
               Math.Abs(c1.Alpha - c2.Alpha) <= tolerance;
    }



    internal static SKBitmap ApplyColorMatrix(SKBitmap source, float[] matrix)
    {
        using var filter = SKColorFilter.CreateColorMatrix(matrix);
        return ApplyColorFilter(source, filter);
    }

    internal static SKBitmap ApplyColorFilter(SKBitmap source, SKColorFilter filter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            canvas.Clear(SKColors.Transparent);
            using (SKPaint paint = new SKPaint())
            {
                paint.ColorFilter = filter;
                canvas.DrawBitmap(source, 0, 0, paint);
            }
        }
        return result;
    }



    internal static SKBitmap ApplyPixelOperation(SKBitmap source, Func<SKColor, SKColor> operation)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);

        // Lock pixels for direct access (fast)
        IntPtr srcPtr = source.GetPixels();
        IntPtr dstPtr = result.GetPixels();

        int count = source.Width * source.Height;

        // Iterate assuming 32-bit (8888) format which is standard for SKBitmap usually
        // Unsafe block would be faster, but let's stick to safe GetPixel/SetPixel if possible 
        // or loop over buffer.
        // For simplicity and safety in this context:

        for (int x = 0; x < source.Width; x++)
        {
            for (int y = 0; y < source.Height; y++)
            {
                SKColor original = source.GetPixel(x, y);
                SKColor modified = operation(original);
                result.SetPixel(x, y, modified);
            }
        }

        return result;
    }

    // ============== FILTERS ==============

    public enum BorderType { Outside, Inside }
    public enum DashStyle { Solid, Dash, Dot, DashDot }

    public static SKBitmap ApplyBorder(SKBitmap source, BorderType type, int size, DashStyle dashStyle, SKColor color)
    {
        return new ShareX.Editor.ImageEffects.FiltersBorderImageEffect(type, size, dashStyle, color).Apply(source);
    }

    public static SKBitmap ApplyOutline(SKBitmap source, int size, int padding, SKColor color)
    {
        return new ShareX.Editor.ImageEffects.FiltersOutlineImageEffect(size, padding, color).Apply(source);
    }

    public static SKBitmap ApplyShadow(SKBitmap source, float opacity, int size, float darkness, SKColor color, int offsetX, int offsetY, bool autoResize)
    {
        return new ShareX.Editor.ImageEffects.FiltersShadowImageEffect(opacity, size, darkness, color, offsetX, offsetY, autoResize).Apply(source);
    }

    public static SKBitmap ApplyGlow(SKBitmap source, int size, float strength, SKColor color, int offsetX, int offsetY)
    {
        return new ShareX.Editor.ImageEffects.FiltersGlowImageEffect(size, strength, color, offsetX, offsetY).Apply(source);
    }

    public static SKBitmap ApplyReflection(SKBitmap source, int percentage, int maxAlpha, int minAlpha, int offset, bool skew, int skewSize)
    {
        return new ShareX.Editor.ImageEffects.FiltersReflectionImageEffect(percentage, maxAlpha, minAlpha, offset, skew, skewSize).Apply(source);
    }

    public static SKBitmap ApplyTornEdge(SKBitmap source, int depth, int range, bool top, bool right, bool bottom, bool left, bool curved)
    {
        return new ShareX.Editor.ImageEffects.FiltersTornEdgeImageEffect(depth, range, top, right, bottom, left, curved).Apply(source);
    }

    public static SKBitmap ApplySlice(SKBitmap source, int minHeight, int maxHeight, int minShift, int maxShift)
    {
        return new ShareX.Editor.ImageEffects.FiltersSliceImageEffect(minHeight, maxHeight, minShift, maxShift).Apply(source);
    }

    /// <summary>
    /// Apply box blur effect to the image
    /// </summary>
    public static SKBitmap BoxBlur(SKBitmap source, int radius)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (radius <= 0) return source.Copy();

        // Expand canvas to prevent edge transparency
        int padding = radius * 2;
        int expandedWidth = source.Width + padding * 2;
        int expandedHeight = source.Height + padding * 2;
        
        // Create expanded bitmap with edge pixels extended
        SKBitmap expanded = new SKBitmap(expandedWidth, expandedHeight, source.ColorType, source.AlphaType);
        using (SKCanvas expandCanvas = new SKCanvas(expanded))
        {
            // Fill with edge-extended version using clamp shader
            using var shader = SKShader.CreateBitmap(source, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, 
                SKMatrix.CreateTranslation(padding, padding));
            using var paint = new SKPaint { Shader = shader };
            expandCanvas.DrawRect(new SKRect(0, 0, expandedWidth, expandedHeight), paint);
        }
        
        // Apply blur to expanded bitmap
        SKBitmap blurred = new SKBitmap(expandedWidth, expandedHeight, source.ColorType, source.AlphaType);
        using (SKCanvas blurCanvas = new SKCanvas(blurred))
        {
            using SKPaint blurPaint = new SKPaint
            {
                ImageFilter = SKImageFilter.CreateBlur(radius, radius)
            };
            blurCanvas.DrawBitmap(expanded, 0, 0, blurPaint);
        }
        expanded.Dispose();
        
        // Crop back to original size
        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas resultCanvas = new SKCanvas(result))
        {
            resultCanvas.DrawBitmap(blurred, new SKRect(padding, padding, padding + source.Width, padding + source.Height),
                new SKRect(0, 0, source.Width, source.Height));
        }
        blurred.Dispose();
        
        return result;
    }

    /// <summary>
    /// Apply pixelate effect to the image
    /// </summary>
    public static SKBitmap Pixelate(SKBitmap source, int size)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (size <= 1) return source.Copy();

        // Downscale then upscale to create pixelation effect
        int smallWidth = Math.Max(1, source.Width / size);
        int smallHeight = Math.Max(1, source.Height / size);

        SKBitmap small = new SKBitmap(smallWidth, smallHeight);
        using (SKCanvas smallCanvas = new SKCanvas(small))
        {
            smallCanvas.DrawBitmap(source, new SKRect(0, 0, smallWidth, smallHeight));
        }

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            using SKPaint paint = new SKPaint { FilterQuality = SKFilterQuality.None };
            canvas.DrawBitmap(small, new SKRect(0, 0, source.Width, source.Height), paint);
        }
        small.Dispose();
        return result;
    }

    /// <summary>
    /// Apply sharpen effect to the image
    /// </summary>
    public static SKBitmap Sharpen(SKBitmap source, float strength)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (strength <= 0) return source.Copy();

        // Sharpen using unsharp mask: original + (original - blurred) * strength
        float s = Math.Clamp(strength, 0f, 1f);
        
        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        
        // Draw original
        canvas.DrawBitmap(source, 0, 0);
        
        // Create sharpening convolution kernel
        float center = 1 + 4 * s;
        float edge = -s;
        float[] kernel = new float[]
        {
            0, edge, 0,
            edge, center, edge,
            0, edge, 0
        };
        
        // Apply convolution
        using SKImageFilter sharpenFilter = SKImageFilter.CreateMatrixConvolution(
            new SKSizeI(3, 3),
            kernel,
            1f, // gain
            0f, // bias
            new SKPointI(1, 1), // kernel offset
            SKShaderTileMode.Clamp,
            true // convolve alpha
        );
        
        using SKPaint paint = new SKPaint { ImageFilter = sharpenFilter };
        result.Dispose();
        
        SKBitmap sharpened = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using SKCanvas sharpCanvas = new SKCanvas(sharpened);
        sharpCanvas.DrawBitmap(source, 0, 0, paint);
        
        return sharpened;
    }

    /// <summary>
    /// Apply rounded corners to the image
    /// </summary>
    public static SKBitmap RoundedCorners(SKBitmap source, int radius)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (radius <= 0) return source.Copy();

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Clear(SKColors.Transparent);

        using SKPath clipPath = new SKPath();
        clipPath.AddRoundRect(new SKRect(0, 0, source.Width, source.Height), radius, radius);
        canvas.ClipPath(clipPath, SKClipOperation.Intersect, true);
        
        canvas.DrawBitmap(source, 0, 0);
        return result;
    }

}
