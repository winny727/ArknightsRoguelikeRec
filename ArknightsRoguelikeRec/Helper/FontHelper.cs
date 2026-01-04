using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

public static class FontHelper
{
#if SKIA_SHARP
    public static SkiaSharp.SKTypeface LoadEmbeddedSKTypeface(string resourceName)
    {
        var asm = Assembly.GetExecutingAssembly();
        using var stream = asm.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new Exception($"Font resource not found: {resourceName}");

        return SkiaSharp.SKTypeface.FromStream(stream);
    }
#endif
}
