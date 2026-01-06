using ArknightsRoguelikeRec.DataStruct;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Color = ArknightsRoguelikeRec.DataStruct.Color;
using DrawingColor = System.Drawing.Color;
using Point = ArknightsRoguelikeRec.DataStruct.Point;
using Rect = ArknightsRoguelikeRec.DataStruct.Rect;

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

            var c = PictureBoxHelper.ToDrawingColor(color) ?? DrawingColor.Transparent;
            mGraphics.Clear(c);
        }

        public void DrawLine(Point start, Point end, Color? color = null, float width = 1f)
        {
            if (mDisposed) return;

            var startPoint = PictureBoxHelper.ToDrawingPoint(start);
            var endPoint = PictureBoxHelper.ToDrawingPoint(end);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.DrawLine(mGraphics, startPoint, endPoint, drawingColor, width);
        }

        public void DrawRectangle(Rect rect, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var drawingRect = PictureBoxHelper.ToDrawingRect(rect);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.DrawRectangle(mGraphics, drawingRect, drawingColor, width);
        }

        public void FillRectangle(Rect rect, Color? color)
        {
            if (mDisposed) return;

            var drawingRect = PictureBoxHelper.ToDrawingRect(rect);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.FillRectangle(mGraphics, drawingRect, drawingColor);
        }

        public void DrawCircle(Point center, float radius, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var centerPoint = PictureBoxHelper.ToDrawingPoint(center);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.DrawCircle(mGraphics, centerPoint, PictureBoxHelper.ConvertNumber(radius), drawingColor, width);
        }

        public void FillCircle(Point center, float radius, Color? color)
        {
            if (mDisposed) return;

            var centerPoint = PictureBoxHelper.ToDrawingPoint(center);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.FillCircle(mGraphics, centerPoint, PictureBoxHelper.ConvertNumber(radius), drawingColor);
        }

        public void DrawBezier(Point p1, Point p2, Point p3, Point p4, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var point1 = PictureBoxHelper.ToDrawingPoint(p1);
            var point2 = PictureBoxHelper.ToDrawingPoint(p2);
            var point3 = PictureBoxHelper.ToDrawingPoint(p3);
            var point4 = PictureBoxHelper.ToDrawingPoint(p4);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.DrawBezier(mGraphics, point1, point2, point3, point4, drawingColor, width);
        }

        public void DrawPoint(Point point, Color? color, float size = 1f)
        {
            if (mDisposed) return;

            var drawingPoint = PictureBoxHelper.ToDrawingPoint(point);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color);
            DrawUtil.DrawPoint(mGraphics, drawingPoint, drawingColor, PictureBoxHelper.ConvertNumber(size));
        }

        public void DrawString(string text, Rect layoutRect, Color? color, TextAlignment alignment = TextAlignment.Center)
        {
            if (mDisposed) return;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var drawingRect = PictureBoxHelper.ToDrawingRect(layoutRect);
            var drawingColor = PictureBoxHelper.ToDrawingColor(color) ?? DrawingColor.Black;
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