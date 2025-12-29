using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

using Point = ArknightsRoguelikeRec.ViewModel.DataStruct.Point;
using Rect = ArknightsRoguelikeRec.ViewModel.DataStruct.Rect;
using Size = ArknightsRoguelikeRec.ViewModel.DataStruct.Size;
using Color = ArknightsRoguelikeRec.ViewModel.DataStruct.Color;

using DrawingPoint = System.Drawing.Point;
using DrawingRect = System.Drawing.Rectangle;
using DrawingSize = System.Drawing.Size;
using DrawingColor = System.Drawing.Color;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class PictureBoxCanvas : ICanvas
    {
        private PictureBox mPictureBox;
        private Graphics mBackgroundGraphics;
        private Graphics mGraphics;
        private readonly Size mDefaultSize;
        private Font mTextFont;
        private bool mDrawBackground = false;
        private bool mDisposed = false;

        public PictureBoxCanvas(PictureBox pictureBox)
        {
            mPictureBox = pictureBox ?? throw new ArgumentNullException(nameof(PictureBox));
            mDefaultSize = new Size(pictureBox.Size.Width, pictureBox.Size.Height - 20f); // 考虑滚动条高度
        }

        public void InitCanvas(float width, float height, Color? backgroundColor = null)
        {
            EnsureNotDisposed();
            mPictureBox.BackgroundImage?.Dispose();
            mPictureBox.Image?.Dispose();

            var bgColor = ToDrawingColor(backgroundColor) ?? DrawingColor.White;
            mPictureBox.BackgroundImage = new Bitmap((int)width, (int)height);
            mPictureBox.Image = new Bitmap((int)width, (int)height);
            mBackgroundGraphics = Graphics.FromImage(mPictureBox.BackgroundImage);
            mGraphics = Graphics.FromImage(mPictureBox.Image);

            mPictureBox.Size = new DrawingSize((int)width, (int)height);

            using Graphics g = Graphics.FromImage(mPictureBox.BackgroundImage);
            g.Clear(bgColor);
        }

        public Size GetCanvasDefaultSize()
        {
            EnsureNotDisposed();
            return mDefaultSize;
        }

        public void RefreshCanvas()
        {
            EnsureNotDisposed();
            mPictureBox.Refresh();
        }

        public void ClearCanvas()
        {
            EnsureNotDisposed();
            mPictureBox.BackgroundImage?.Dispose();
            mPictureBox.Image?.Dispose();
            mPictureBox.BackgroundImage = null;
            mPictureBox.Image = null;
        }

        public void ResetDynamicCanvas()
        {
            EnsureNotDisposed();
            mPictureBox.Image?.Dispose();
            mPictureBox.Image = new Bitmap(mPictureBox.BackgroundImage.Width, mPictureBox.BackgroundImage.Height);
            mGraphics = Graphics.FromImage(mPictureBox.Image);
        }

        public void SetDrawStatic()
        {
            EnsureNotDisposed();
            mDrawBackground = true;
        }

        public void SetDrawDynamic()
        {
            EnsureNotDisposed();
            mDrawBackground = true;
        }

        public void DrawLine(Point start, Point end, Color? color = null, float width = 1f)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var startPoint = new DrawingPoint((int)start.X, (int)start.Y);
            var endPoint = new DrawingPoint((int)end.X, (int)end.Y);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.DrawLine(graphics, startPoint, endPoint, drawingColor, width);
        }

        public void DrawRectangle(Rect rect, Color? color, float width = 1f)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var drawingRect = new DrawingRect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.DrawRectangle(graphics, drawingRect, drawingColor, width);
        }

        public void FillRectangle(Rect rect, Color? color)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var drawingRect = new DrawingRect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.FillRectangle(graphics, drawingRect, drawingColor);
        }

        public void DrawCircle(Point center, float radius, Color? color, float width = 1f)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var centerPoint = new DrawingPoint((int)center.X, (int)center.Y);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.DrawCircle(graphics, centerPoint, (int)radius, drawingColor, width);
        }

        public void FillCircle(Point center, float radius, Color? color)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var centerPoint = new DrawingPoint((int)center.X, (int)center.Y);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.FillCircle(graphics, centerPoint, (int)radius, drawingColor);
        }

        public void DrawBezier(Point p1, Point p2, Point p3, Point p4, Color? color, float width = 1f)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var point1 = new DrawingPoint((int)p1.X, (int)p1.Y);
            var point2 = new DrawingPoint((int)p2.X, (int)p2.Y);
            var point3 = new DrawingPoint((int)p3.X, (int)p3.Y);
            var point4 = new DrawingPoint((int)p4.X, (int)p4.Y);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.DrawBezier(graphics, point1, point2, point3, point4, drawingColor, width);
        }

        public void DrawPoint(Point point, Color? color, float size = 1f)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var drawingPoint = new DrawingPoint((int)point.X, (int)point.Y);
            var drawingColor = ToDrawingColor(color);
            DrawUtil.DrawPoint(graphics, drawingPoint, drawingColor, (int)size);
        }

        public void DrawString(string text, Rect layoutRect, Color? color, TextAlignment alignment = TextAlignment.Center)
        {
            EnsureNotDisposed();
            Graphics graphics = GetCurrentGraphics();
            if (graphics == null)
            {
                return;
            }

            var drawingRect = new DrawingRect((int)layoutRect.X, (int)layoutRect.Y, (int)layoutRect.Width, (int)layoutRect.Height);
            var drawingColor = ToDrawingColor(color) ?? DrawingColor.Black;
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
            Font font = GetTextFont();
            DrawUtil.DrawString(graphics, text, drawingRect, font, drawingColor, textFormatFlags);
        }

        public void RegClickEvent(Action<Point, MouseButton> callback)
        {
            EnsureNotDisposed();
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            mPictureBox.MouseClick += (sender, e) =>
            {
                var point = new Point(e.X, e.Y);
                var button = (MouseButton)e.Button;
                callback(point, button);
            };
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            mDisposed = true;
            mPictureBox.BackgroundImage?.Dispose();
            mPictureBox.Image?.Dispose();
            mTextFont?.Dispose();
            mPictureBox = null;
            mTextFont = null;
        }

        private void EnsureNotDisposed()
        {
            if (mDisposed)
            {
                throw new ObjectDisposedException(nameof(PictureBoxCanvas));
            }
        }

        public void SetTextFont(Font font)
        {
            EnsureNotDisposed();
            mTextFont = font;
        }

        public Font GetTextFont()
        {
            EnsureNotDisposed();
            return mTextFont ?? mPictureBox.Font;
        }

        private Graphics GetCurrentGraphics()
        {
            EnsureNotDisposed();
            return mDrawBackground ? mBackgroundGraphics : mGraphics;
        }

        private DrawingColor? ToDrawingColor(Color? color)
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