using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public static class ButtonColorHelper
    {
        public static Color GetPressedColor(Color color)
        {
            byte r = (byte)Math.Max(color.R - 40, 0);
            byte g = (byte)Math.Max(color.G - 40, 0);
            byte b = (byte)Math.Max(color.B - 40, 0);
            return Color.FromRgb(r, g, b);
        }

        public static Color GetHoverColor(Color color)
        {
            byte r = (byte)Math.Min(color.R + 30, 255);
            byte g = (byte)Math.Min(color.G + 30, 255);
            byte b = (byte)Math.Min(color.B + 30, 255);
            return Color.FromRgb(r, g, b);
        }

        public static Color GetDisabledColor(Color color)
        {
            // 灰度化：常用公式 0.3*R + 0.59*G + 0.11*B
            byte gray = (byte)Math.Min(255, (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11));
            return Color.FromRgb(gray, gray, gray);
        }

        public static Color GetHighlightColor(Color color)
        {
            byte r = (byte)Math.Min(color.R + 60, 255);
            byte g = (byte)Math.Min(color.G + 60, 255);
            byte b = (byte)Math.Min(color.B + 60, 255);
            return Color.FromRgb(r, g, b);
        }
    }
}
