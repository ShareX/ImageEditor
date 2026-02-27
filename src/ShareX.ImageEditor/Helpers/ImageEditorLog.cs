using System.Diagnostics;

namespace ShareX.ImageEditor.Helpers;

internal static class ImageEditorLog
{
    public static void WriteLine(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        Debug.WriteLine(message);
        Trace.WriteLine(message);
        Console.WriteLine(message);
    }
}
