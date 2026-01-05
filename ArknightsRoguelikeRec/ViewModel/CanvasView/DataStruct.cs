using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel.DataStruct
{
    public readonly struct Point
    {
        public float X { get; }
        public float Y { get; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

    public readonly struct Rect
    {
        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }

        public Rect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Contains(Point point)
        {
            return point.X >= X && point.X <= X + Width &&
                   point.Y >= Y && point.Y <= Y + Height;
        }

        public override string ToString()
        {
            return $"({X},{Y},{Width},{Height})";
        }
    }

    public readonly struct Size
    {
        public float Width { get; }
        public float Height { get; }
        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"({Width},{Height})";
        }
    }

    public readonly struct Color
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color(r, g, b);
        }

        public override string ToString()
        {
            return $"({R},{G},{B},{A})";
        }

        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color Gray = new Color(128, 128, 128);
        public static readonly Color LightGray = new Color(211, 211, 211);
        public static readonly Color DarkGray = new Color(64, 64, 64);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color DarkRed = new Color(139, 0, 0);
        public static readonly Color DarkGreen = new Color(0, 139, 0);
        public static readonly Color DarkBlue = new Color(0, 0, 139);
        public static readonly Color Transparent = new Color(0, 0, 0, 0);
    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right,
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
}