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

namespace ShareX.ImageEditor.Presentation.Theming
{
    public static class EditorIcons
    {
        public const string ToolSelect = LucideIcons.MousePointer2;
        public const string ToolRectangle = LucideIcons.Square;
        public const string ToolEllipse = LucideIcons.Circle;
        public const string ToolLine = LucideIcons.Minus;
        public const string ToolArrow = LucideIcons.ArrowRight;
        public const string ToolFreehand = LucideIcons.Pencil;
        public const string ToolHighlight = LucideIcons.Highlighter;
        public const string ToolText = LucideIcons.Type;
        public const string ToolSpeechBalloon = LucideIcons.MessageSquare;
        public const string ToolStep = LucideIcons.Hash;
        public const string ToolBlur = LucideIcons.Droplet;
        public const string ToolPixelate = LucideIcons.Grid2X2;
        public const string ToolMagnify = LucideIcons.Search;
        public const string ToolSpotlight = LucideIcons.Lightbulb;
        public const string ToolSmartEraser = LucideIcons.Eraser;
        public const string ToolCrop = LucideIcons.Crop;
        public const string ToolCutOut = LucideIcons.Scissors;

        public const string ActionUndo = LucideIcons.Undo2;
        public const string ActionRedo = LucideIcons.Redo2;
        public const string ActionDelete = LucideIcons.Trash2;
        public const string ActionClearAll = LucideIcons.BrushCleaning;
        public const string ActionCopy = LucideIcons.Clipboard;
        public const string ActionPaste = LucideIcons.ClipboardPaste;
        public const string ActionDuplicate = LucideIcons.CopyPlus;
        public const string ActionBringToFront = LucideIcons.BringToFront;
        public const string ActionBringForward = LucideIcons.MoveUp;
        public const string ActionSendBackward = LucideIcons.MoveDown;
        public const string ActionSendToBack = LucideIcons.SendToBack;
        public const string ActionSave = LucideIcons.Save;
        public const string ActionSaveAs = LucideIcons.SaveAll;
        public const string ActionDownload = LucideIcons.Download;
        public const string ActionPinToScreen = LucideIcons.Pin;
        public const string ActionUpload = LucideIcons.CloudUpload;
        public const string ActionCancel = LucideIcons.X;
        public const string ActionContinue = LucideIcons.Play;
        public const string ActionReset = LucideIcons.RefreshCw;
        public const string ActionRotateLeft = LucideIcons.RotateCcwSquare;
        public const string ActionRotateRight = LucideIcons.RotateCwSquare;

        public const string FormatBold = LucideIcons.Bold;
        public const string FormatItalic = LucideIcons.Italic;
        public const string FormatUnderline = LucideIcons.Underline;
        public const string MenuTheme = LucideIcons.MoonStar;
        public const string PanelBackground = LucideIcons.Image;
        public const string PanelEffects = LucideIcons.Sparkles;
        public const string LayerFlatten = LucideIcons.Layers2;
        public const string ChevronDown = LucideIcons.ChevronDown;

        public static string ForTool(EditorTool tool) => tool switch
        {
            EditorTool.Select => ToolSelect,
            EditorTool.Rectangle => ToolRectangle,
            EditorTool.Ellipse => ToolEllipse,
            EditorTool.Line => ToolLine,
            EditorTool.Arrow => ToolArrow,
            EditorTool.Freehand => ToolFreehand,
            EditorTool.Text => ToolText,
            EditorTool.SpeechBalloon => ToolSpeechBalloon,
            EditorTool.Step => ToolStep,
            EditorTool.Blur => ToolBlur,
            EditorTool.Pixelate => ToolPixelate,
            EditorTool.Magnify => ToolMagnify,
            EditorTool.Spotlight => ToolSpotlight,
            EditorTool.SmartEraser => ToolSmartEraser,
            EditorTool.Highlight => ToolHighlight,
            EditorTool.Crop => ToolCrop,
            EditorTool.CutOut => ToolCutOut,
            _ => ToolSelect
        };
    }
}
