#if SKIA_SHARP
using System;
using System.Collections.Generic;
using System.Reflection;
using SkiaSharp;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public static class FontLoader
    {
        public const string FONT_PATH = "ArknightsRoguelikeRec.Assets.Fonts.HarmonyOS_Sans_SC_Regular.ttf";
        public static readonly SKTypeface SK_TEXT_FONT = LoadEmbeddedSKTypeface(FONT_PATH);

        public static SKTypeface LoadEmbeddedSKTypeface(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Font resource not found: {resourceName}");

            return SKTypeface.FromStream(stream);
        }
    }
}
#endif
