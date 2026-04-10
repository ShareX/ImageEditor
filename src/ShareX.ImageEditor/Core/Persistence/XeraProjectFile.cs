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

namespace ShareX.ImageEditor.Core.Persistence;

/// <summary>
/// Serialized `.xera` sidecar document for re-editable annotations.
/// </summary>
public sealed class XeraProjectFile
{
    public int Version { get; set; } = 1;
    public string ImagePath { get; set; } = string.Empty;
    public string ImageHash { get; set; } = string.Empty;
    public int CanvasWidth { get; set; }
    public int CanvasHeight { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string SourceImagePngBase64 { get; set; } = string.Empty;
    public Dictionary<string, string> EmbeddedImages { get; set; } = new(StringComparer.Ordinal);
    public List<Annotation> Annotations { get; set; } = new();
}

public sealed class XeraProjectLoadResult
{
    public XeraProjectLoadResult(XeraProjectFile project, SkiaSharp.SKBitmap sourceImage, bool imageHashMatches)
    {
        Project = project;
        SourceImage = sourceImage;
        ImageHashMatches = imageHashMatches;
    }

    public XeraProjectFile Project { get; }
    public SkiaSharp.SKBitmap SourceImage { get; }
    public bool ImageHashMatches { get; }
}
