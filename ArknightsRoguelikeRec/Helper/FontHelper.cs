using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class FontHelper
{
    private static PrivateFontCollection mFontCollection = new PrivateFontCollection();

    public static FontFamily LoadFont(string resourceName)
    {
        // 如果已经加载过，直接返回
        foreach (var ff in mFontCollection.Families)
        {
            if (ff.Name == resourceName)
                return ff;
        }

        var asm = Assembly.GetExecutingAssembly();
        using Stream fontStream = asm.GetManifestResourceStream(resourceName);
        if (fontStream == null)
            throw new Exception($"Embedded font not found: {resourceName}");

        // 复制到 byte[]，然后添加到 PrivateFontCollection
        byte[] fontData = new byte[fontStream.Length];
        fontStream.Read(fontData, 0, fontData.Length);

        unsafe
        {
            fixed (byte* pFontData = fontData)
            {
                mFontCollection.AddMemoryFont((IntPtr)pFontData, fontData.Length);
            }
        }

        return mFontCollection.Families[mFontCollection.Families.Length - 1]; // 返回最后添加的字体
    }

    public static Font CreateFont(string resourceName, float size, FontStyle style = FontStyle.Regular)
    {
        var family = LoadFont(resourceName);
        return new Font(family, size, style, GraphicsUnit.Pixel);
    }

    public static SKTypeface LoadEmbeddedSKTypeface(string resourceName)
    {
        var asm = Assembly.GetExecutingAssembly();
        using var stream = asm.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new Exception($"Font resource not found: {resourceName}");

        return SKTypeface.FromStream(stream);
    }
}
