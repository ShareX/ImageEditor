#region License Information (GPL v3)

/*
    ShareX.ImageEditor - The UI-agnostic Editor library for ShareX
    Copyright (c) 2007-2026 ShareX Team

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

using ShareX.ImageEditor.Core.Annotations;
using SkiaSharp;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShareX.ImageEditor.Core.Persistence;

public static class XannProjectFileService
{
    public const int CurrentVersion = 1;
    public const string FileExtension = ".xann";

    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

    public static string GetDefaultSidecarPath(string imagePath) =>
        Path.Combine(
            Path.GetDirectoryName(imagePath) ?? string.Empty,
            Path.GetFileNameWithoutExtension(imagePath) + FileExtension);

    public static bool TryDeleteSidecar(string imagePath)
    {
        string sidecarPath = GetDefaultSidecarPath(imagePath);
        if (!File.Exists(sidecarPath))
        {
            return false;
        }

        File.Delete(sidecarPath);
        return true;
    }

    public static Task<string?> SaveAsync(
        string imagePath,
        SKBitmap sourceImage,
        IEnumerable<Annotation> annotations,
        DateTimeOffset? createdAt = null,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(
            () => Save(imagePath, sourceImage, annotations, createdAt, cancellationToken),
            cancellationToken);
    }

    public static Task<XannProjectLoadResult> LoadAsync(
        string sidecarPath,
        string? expectedImagePath = null,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(
            () => Load(sidecarPath, expectedImagePath, cancellationToken),
            cancellationToken);
    }

    private static string? Save(
        string imagePath,
        SKBitmap sourceImage,
        IEnumerable<Annotation> annotations,
        DateTimeOffset? createdAt,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || sourceImage == null)
        {
            return null;
        }

        var annotationList = annotations.Select(annotation => annotation.Clone()).ToList();
        if (annotationList.Count == 0)
        {
            TryDeleteSidecar(imagePath);
            return null;
        }

        string sidecarPath = GetDefaultSidecarPath(imagePath);
        string? directory = Path.GetDirectoryName(sidecarPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var project = new XannProjectFile
        {
            Version = CurrentVersion,
            ImagePath = Path.GetFileName(imagePath),
            ImageHash = File.Exists(imagePath) ? "sha256:" + ComputeSha256(imagePath) : string.Empty,
            CanvasWidth = sourceImage.Width,
            CanvasHeight = sourceImage.Height,
            CreatedAt = createdAt ?? DateTimeOffset.UtcNow,
            ModifiedAt = DateTimeOffset.UtcNow,
            SourceImagePngBase64 = EncodeBitmapPngBase64(sourceImage),
            Annotations = annotationList
        };

        foreach (var imageAnnotation in annotationList.OfType<ImageAnnotation>())
        {
            if (imageAnnotation.ImageBitmap != null)
            {
                project.EmbeddedImages[imageAnnotation.Id.ToString("D")] =
                    EncodeBitmapPngBase64(imageAnnotation.ImageBitmap);
            }
        }

        string tempPath = sidecarPath + ".tmp";
        try
        {
            using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var gzipStream = new GZipStream(fileStream, CompressionLevel.SmallestSize))
            {
                JsonSerializer.Serialize(gzipStream, project, SerializerOptions);
            }

            if (File.Exists(sidecarPath))
            {
                File.Delete(sidecarPath);
            }

            File.Move(tempPath, sidecarPath);
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        return sidecarPath;
    }

    private static XannProjectLoadResult Load(string sidecarPath, string? expectedImagePath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var fileStream = new FileStream(sidecarPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
        var project = JsonSerializer.Deserialize<XannProjectFile>(gzipStream, SerializerOptions)
            ?? throw new InvalidDataException("The .xann sidecar was empty or invalid.");

        if (project.Version > CurrentVersion)
        {
            throw new NotSupportedException($"Unsupported .xann version {project.Version}.");
        }

        var sourceImage = DecodeBitmapPngBase64(project.SourceImagePngBase64)
            ?? throw new InvalidDataException("The .xann sidecar does not contain a valid source image.");

        foreach (var imageAnnotation in project.Annotations.OfType<ImageAnnotation>())
        {
            if (project.EmbeddedImages.TryGetValue(imageAnnotation.Id.ToString("D"), out string? imageBase64))
            {
                var embeddedImage = DecodeBitmapPngBase64(imageBase64);
                if (embeddedImage != null)
                {
                    imageAnnotation.SetImage(embeddedImage);
                }
            }
            else if (!string.IsNullOrWhiteSpace(imageAnnotation.ImagePath) && File.Exists(imageAnnotation.ImagePath))
            {
                imageAnnotation.LoadImage(imageAnnotation.ImagePath);
            }
        }

        bool hashMatches = true;
        if (!string.IsNullOrWhiteSpace(expectedImagePath) &&
            File.Exists(expectedImagePath) &&
            !string.IsNullOrWhiteSpace(project.ImageHash))
        {
            hashMatches = string.Equals(
                project.ImageHash,
                "sha256:" + ComputeSha256(expectedImagePath),
                StringComparison.OrdinalIgnoreCase);
        }

        return new XannProjectLoadResult(project, sourceImage, hashMatches);
    }

    private static string EncodeBitmapPngBase64(SKBitmap bitmap)
    {
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return Convert.ToBase64String(data.ToArray());
    }

    private static SKBitmap? DecodeBitmapPngBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            return null;
        }

        byte[] bytes = Convert.FromBase64String(base64);
        return SKBitmap.Decode(bytes);
    }

    private static string ComputeSha256(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        byte[] hash = SHA256.HashData(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.Converters.Add(new SKPointJsonConverter());
        options.Converters.Add(new SKSizeJsonConverter());
        return options;
    }

    private sealed class SKPointJsonConverter : JsonConverter<SKPoint>
    {
        public override SKPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float x = 0;
            float y = 0;

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected SKPoint object.");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new SKPoint(x, y);
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected SKPoint property.");
                }

                string? propertyName = reader.GetString();
                reader.Read();

                if (string.Equals(propertyName, "x", StringComparison.OrdinalIgnoreCase))
                {
                    x = reader.GetSingle();
                }
                else if (string.Equals(propertyName, "y", StringComparison.OrdinalIgnoreCase))
                {
                    y = reader.GetSingle();
                }
                else
                {
                    reader.Skip();
                }
            }

            throw new JsonException("Incomplete SKPoint object.");
        }

        public override void Write(Utf8JsonWriter writer, SKPoint value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("x", value.X);
            writer.WriteNumber("y", value.Y);
            writer.WriteEndObject();
        }
    }

    private sealed class SKSizeJsonConverter : JsonConverter<SKSize>
    {
        public override SKSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float width = 0;
            float height = 0;

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected SKSize object.");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new SKSize(width, height);
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected SKSize property.");
                }

                string? propertyName = reader.GetString();
                reader.Read();

                if (string.Equals(propertyName, "width", StringComparison.OrdinalIgnoreCase))
                {
                    width = reader.GetSingle();
                }
                else if (string.Equals(propertyName, "height", StringComparison.OrdinalIgnoreCase))
                {
                    height = reader.GetSingle();
                }
                else
                {
                    reader.Skip();
                }
            }

            throw new JsonException("Incomplete SKSize object.");
        }

        public override void Write(Utf8JsonWriter writer, SKSize value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("width", value.Width);
            writer.WriteNumber("height", value.Height);
            writer.WriteEndObject();
        }
    }
}
