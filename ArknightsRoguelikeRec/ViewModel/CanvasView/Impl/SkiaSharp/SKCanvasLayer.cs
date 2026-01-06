#if SKIA_SHARP
using ArknightsRoguelikeRec.DataStruct;
using SkiaSharp;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public class SKCanvasLayer : ICanvasLayer
    {
        public string Name { get; set; }
        private SKGLCanvas mCanvas;
        private SKTypeface mSKTypeface;
        private readonly List<Action<SKCanvas>> mCommands = new List<Action<SKCanvas>>();

        private static readonly Dictionary<(Color, float, bool), SKPaint> mStrokeCache = new Dictionary<(Color, float, bool), SKPaint>();
        private static readonly Dictionary<(Color, bool), SKPaint> mFillCache = new Dictionary<(Color, bool), SKPaint>();
        private static readonly Dictionary<(SKTypeface, float), SKFont> mFontCache = new Dictionary<(SKTypeface, float), SKFont>();

        private bool mDisposed = false;

        public SKCanvasLayer(SKGLCanvas skglCanvas, SKTypeface skTypeface)
        {
            mCanvas = skglCanvas ?? throw new ArgumentNullException(nameof(skglCanvas));
            mSKTypeface = skTypeface ?? throw new ArgumentNullException(nameof(skTypeface));
        }

        internal void ExecuteRenderCommands(SKCanvas canvas)
        {
            if (mDisposed) return;

            foreach (var cmd in mCommands)
                cmd(canvas);
        }

        public void Clear(Color? color = null)
        {
            if (mDisposed) return;

            mCommands.Clear();

            if (color != null)
            {
                var skColor = ToSK(color.Value);
                mCommands.Add(c => c.Clear(skColor));
            }

            mCanvas.MarkDirty();
        }

        public void DrawLine(Point start, Point end, Color? color = null, float width = 1f)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                var paint = StrokePaint(c, width);
                canvas.DrawLine(start.X, start.Y, end.X, end.Y, paint);
            });

            mCanvas.MarkDirty();
        }

        public void DrawRectangle(Rect rect, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                var paint = StrokePaint(c, width);
                canvas.DrawRect(ToSKRect(rect), paint);
            });

            mCanvas.MarkDirty();
        }

        public void FillRectangle(Rect rect, Color? color)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                var paint = FillPaint(c);
                canvas.DrawRect(ToSKRect(rect), paint);
            });

            mCanvas.MarkDirty();
        }

        public void DrawCircle(Point origin, float radius, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                var paint = StrokePaint(c, width, true);
                canvas.DrawCircle(origin.X, origin.Y, radius, paint);
            });

            mCanvas.MarkDirty();
        }

        public void FillCircle(Point origin, float radius, Color? color)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                var paint = FillPaint(c, true);
                canvas.DrawCircle(origin.X, origin.Y, radius, paint);
            });

            mCanvas.MarkDirty();
        }

        public void DrawBezier(Point p1, Point p2, Point p3, Point p4, Color? color, float width = 1f)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                var path = new SKPath();
                path.MoveTo(p1.X, p1.Y);
                path.CubicTo(p2.X, p2.Y, p3.X, p3.Y, p4.X, p4.Y);

                var paint = StrokePaint(c, width, true);
                canvas.DrawPath(path, paint);
            });

            mCanvas.MarkDirty();
        }

        public void DrawPoint(Point point, Color? color, float size = 1f)
        {
            if (mDisposed) return;

            var c = color ?? Color.Black;
            float r = Math.Max(0.5f, size * 0.5f);

            mCommands.Add(canvas =>
            {
                var paint = FillPaint(c);
                canvas.DrawCircle(point.X, point.Y, r, paint);
            });

            mCanvas.MarkDirty();
        }

        public void DrawString(string text, Rect layoutRect, Color? color, TextAlignment alignment = TextAlignment.Center)
        {
            if (mDisposed) return;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var c = color ?? Color.Black;

            mCommands.Add(canvas =>
            {
                float fontSize = layoutRect.Height * 0.4f;
                var font = GetFont(mSKTypeface, fontSize);
                var paint = GetFill(c);

                float textWidth = font.MeasureText(text, paint);
                var metrics = font.Metrics;

                float layoutLeft = layoutRect.X;
                float layoutRight = layoutRect.X + layoutRect.Width - textWidth;
                float layoutHCenter = layoutRect.X + layoutRect.Width / 2;

                float x = alignment switch
                {
                    TextAlignment.Left => layoutLeft,
                    TextAlignment.TopLeft => layoutLeft,
                    TextAlignment.BottomLeft => layoutLeft,
                    TextAlignment.Right => layoutRight,
                    TextAlignment.TopRight => layoutRight,
                    TextAlignment.BottomRight => layoutRight,
                    _ => layoutHCenter - textWidth * 0.5f
                };

                float textHeight = metrics.Descent - metrics.Ascent;
                float layoutTop = layoutRect.Y - metrics.Ascent;
                float layoutBottom = layoutRect.Y + layoutRect.Height - metrics.Descent;
                float layoutVCenter = layoutRect.Y + layoutRect.Height * 0.5f
                        - (metrics.Ascent + metrics.Descent) * 0.5f;

                float y = alignment switch
                {
                    TextAlignment.TopLeft => layoutTop,
                    TextAlignment.TopCenter => layoutTop,
                    TextAlignment.TopRight => layoutTop,
                    TextAlignment.BottomLeft => layoutBottom,
                    TextAlignment.BottomCenter => layoutBottom,
                    TextAlignment.BottomRight => layoutBottom,
                    _ => layoutVCenter
                };

                canvas.DrawText(text, x, y, font, paint);
            });

            mCanvas.MarkDirty();
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            mDisposed = true;
            mCommands.Clear();
            mCanvas = null;
            mSKTypeface = null;
        }

        private static SKPaint StrokePaint(Color c, float width, bool isAntialias = false)
            => GetStroke(c, width, isAntialias);

        private static SKPaint FillPaint(Color c, bool isAntialias = false)
            => GetFill(c, isAntialias);

        private static SKRect ToSKRect(Rect r)
            => new SKRect(r.X, r.Y, r.X + r.Width, r.Y + r.Height);

        private static SKColor ToSK(Color c)
            => new SKColor(c.R, c.G, c.B, c.A);

        private static SKPaint GetStroke(Color c, float w, bool isAntialias = false)
        {
            var key = (c, w, isAntialias);
            if (!mStrokeCache.TryGetValue(key, out var p))
            {
                p = new SKPaint
                {
                    Color = ToSK(c),
                    StrokeWidth = w,
                    IsStroke = true,
                    IsAntialias = isAntialias,
                };
                mStrokeCache[key] = p;
            }
            return p;
        }

        private static SKPaint GetFill(Color c, bool isAntialias = false)
        {
            var key = (c, isAntialias);
            if (!mFillCache.TryGetValue(key, out var p))
            {
                p = new SKPaint
                {
                    Color = ToSK(c),
                    IsAntialias = isAntialias,
                };
                mFillCache[key] = p;
            }
            return p;
        }

        private static SKFont GetFont(SKTypeface tf, float size)
        {
            var key = (tf, size);
            if (!mFontCache.TryGetValue(key, out var font))
            {
                font = new SKFont
                {
                    Typeface = tf,
                    Size = size
                };
                mFontCache[key] = font;
            }
            return font;
        }
    }
}
#endif