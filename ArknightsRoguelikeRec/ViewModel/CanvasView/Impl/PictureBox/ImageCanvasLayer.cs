using ArknightsRoguelikeRec.ViewModel.DataStruct;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Color = ArknightsRoguelikeRec.ViewModel.DataStruct.Color;
using DrawingColor = System.Drawing.Color;
using Point = ArknightsRoguelikeRec.ViewModel.DataStruct.Point;
using Rect = ArknightsRoguelikeRec.ViewModel.DataStruct.Rect;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public class ImageCanvasLayer : ICanvasLayer
    {
        public string Name { get; set; }
        public Image Image { get; private set; }
        private Graphics mGraphics;
        private Font mTextFont;

        private bool mDisposed = false;

        public ImageCanvasLayer(Image image, Font textFont)
        {
            Image = image ?? throw new ArgumentNullException(nameof(Image));
            mGraphics = Graphics.FromImage(image) ?? throw new ArgumentNullException(nameof(Graphics));
            mTextFont = textFont ?? throw new ArgumentNullException(nameof(textFont));

            mGraphics.CompositingMode = CompositingMode.SourceOver;
            mGraphics.CompositingQuality = CompositingQuality.HighSpeed;
            mGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            mGraphics.SmoothingMode = SmoothingMode.HighSpeed;
        }

        public void Clear(Color? color = null)
        {
            if (mDisposed) return;
            mGraphics.Clear(ImplHelper.ToDrawingColor(color) ?? DrawingColor.Transparent);
        }

        public void DrawLine(Point start, Point end, Color? color = null, float width = 1f)
        {
            if (mDisposed) return;

            var startPoint = ImplHelper.ToDrawingPoint(start);
            var endPoint = ImplHelper.ToDrawingPoint(end);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.DrawLine(mGraphics, startPoint, endPoint, drawingColor, width);
        }

        public void DrawRectangle(Rect rect, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var drawingRect = ImplHelper.ToDrawingRect(rect);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.DrawRectangle(mGraphics, drawingRect, drawingColor, width);
        }

        public void FillRectangle(Rect rect, Color? color)
        {
            if (mDisposed) return;

            var drawingRect = ImplHelper.ToDrawingRect(rect);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.FillRectangle(mGraphics, drawingRect, drawingColor);
        }

        public void DrawCircle(Point center, float radius, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var centerPoint = ImplHelper.ToDrawingPoint(center);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.DrawCircle(mGraphics, centerPoint, ImplHelper.ConvertNumber(radius), drawingColor, width);
        }

        public void FillCircle(Point center, float radius, Color? color)
        {
            if (mDisposed) return;

            var centerPoint = ImplHelper.ToDrawingPoint(center);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.FillCircle(mGraphics, centerPoint, ImplHelper.ConvertNumber(radius), drawingColor);
        }

        public void DrawBezier(Point p1, Point p2, Point p3, Point p4, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var point1 = ImplHelper.ToDrawingPoint(p1);
            var point2 = ImplHelper.ToDrawingPoint(p2);
            var point3 = ImplHelper.ToDrawingPoint(p3);
            var point4 = ImplHelper.ToDrawingPoint(p4);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.DrawBezier(mGraphics, point1, point2, point3, point4, drawingColor, width);
        }

        public void DrawPoint(Point point, Color? color, float size = 1f)
        {
            if (mDisposed) return;

            var drawingPoint = ImplHelper.ToDrawingPoint(point);
            var drawingColor = ImplHelper.ToDrawingColor(color);
            DrawUtil.DrawPoint(mGraphics, drawingPoint, drawingColor, ImplHelper.ConvertNumber(size));
        }

        public void DrawString(string text, Rect layoutRect, Color? color, TextAlignment alignment = TextAlignment.Center)
        {
            if (mDisposed) return;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var drawingRect = ImplHelper.ToDrawingRect(layoutRect);
            var drawingColor = ImplHelper.ToDrawingColor(color) ?? DrawingColor.Black;
            var textFormatFlags = alignment switch
            {
                TextAlignment.Left => TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                TextAlignment.Right => TextFormatFlags.Right | TextFormatFlags.VerticalCenter,
                TextAlignment.Center => TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                TextAlignment.TopLeft => TextFormatFlags.Left | TextFormatFlags.Top,
                TextAlignment.TopRight => TextFormatFlags.Right | TextFormatFlags.Top,
                TextAlignment.TopCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.Top,
                TextAlignment.BottomLeft => TextFormatFlags.Left | TextFormatFlags.Bottom,
                TextAlignment.BottomRight => TextFormatFlags.Right | TextFormatFlags.Bottom,
                TextAlignment.BottomCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom,
                _ => TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
            };
            DrawUtil.DrawString(mGraphics, text, drawingRect, mTextFont, drawingColor, textFormatFlags);
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            mDisposed = true;
            mGraphics?.Dispose();
            mTextFont?.Dispose();
            mGraphics = null;
            mTextFont = null;
        }

        public void SetTextFont(Font textFont)
        {
            if (mDisposed) return;
            mTextFont = textFont ?? throw new ArgumentNullException(nameof(Font));
        }
    }
}