using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public interface ICanvas : IDisposable
    {
        void InitCanvas(float width, float height, Color? backgroundColor = null);
        Size GetCanvasDefaultSize();
        void RefreshCanvas();
        void ClearCanvas();
        void SetDrawStatic(); // 静态模式下绘制比较固定的内容
        void SetDrawDynamic(); // 动态模式下绘制需要频繁刷新的内容
        void ResetDynamicCanvas();
        void DrawLine(Point start, Point end, Color? color = null, float width = 1f);
        void DrawRectangle(Rect rect, Color? color = null, float width = 1f);
        void FillRectangle(Rect rect, Color? color = null);
        void DrawCircle(Point center, float radius, Color? color = null, float width = 1f);
        void FillCircle(Point center, float radius, Color? color = null);
        void DrawBezier(Point p1, Point p2, Point p3, Point p4, Color? color = null, float width = 1f);
        void DrawPoint(Point point, Color? color = null, float size = 1f);
        void DrawString(string text, Rect layoutRect, Color? color = null, TextAlignment alignment = TextAlignment.Center);
    }

    #region 辅助数据结构

    namespace DataStruct
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

            public static readonly Color Black = new Color(0, 0, 0);
            public static readonly Color White = new Color(255, 255, 255);
            public static readonly Color Gray = new Color(128, 128, 128);
            public static readonly Color LightGray = new Color(211, 211, 211);
            public static readonly Color Red = new Color(255, 0, 0);
            public static readonly Color Green = new Color(0, 255, 0);
            public static readonly Color Blue = new Color(0, 0, 255);
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

    #endregion
}