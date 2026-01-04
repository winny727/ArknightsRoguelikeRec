using System;
using System.Collections.Generic;
using Color = ArknightsRoguelikeRec.ViewModel.DataStruct.Color;
using DrawingColor = System.Drawing.Color;
using DrawingPoint = System.Drawing.Point;
using DrawingRect = System.Drawing.Rectangle;
using DrawingSize = System.Drawing.Size;
using Point = ArknightsRoguelikeRec.ViewModel.DataStruct.Point;
using Rect = ArknightsRoguelikeRec.ViewModel.DataStruct.Rect;
using Size = ArknightsRoguelikeRec.ViewModel.DataStruct.Size;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public static class ImplHelper
    {
        public static int ConvertNumber(float value)
        {
            return (int)value;
        }

        public static DrawingPoint ToDrawingPoint(Point point)
        {
            return new DrawingPoint((int)point.X, (int)point.Y);
        }

        public static DrawingRect ToDrawingRect(Rect rect)
        {
            return new DrawingRect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static DrawingSize ToDrawingSize(Size size)
        {
            return new DrawingSize((int)size.Width, (int)size.Height);
        }

        public static DrawingColor ToDrawingColor(Color color)
        {
            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static DrawingColor? ToDrawingColor(Color? color)
        {
            if (color == null)
            {
                return null;
            }

            Color c = color.Value;
            return DrawingColor.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}
