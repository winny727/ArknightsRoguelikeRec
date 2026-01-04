using ArknightsRoguelikeRec.ViewModel.DataStruct;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public class SKGLCanvas : ICanvas
    {
        private SKGLControl mSKGLControl;
        private SKTypeface mSKTypeface;
        private Size mSize;

        private readonly List<SKCanvasLayer> mCanvasLayers = new List<SKCanvasLayer>();
        private SKPicture mCachedPicture;
        private bool mDirty = true;
        private SKColor mBackgroundColor = SKColors.Transparent;

        private bool mDisposed = false;

        public SKGLCanvas(SKGLControl skglControl, SKTypeface skTypeface)
        {
            mSKGLControl = skglControl ?? throw new ArgumentNullException(nameof(skglControl));
            mSKTypeface = skTypeface ?? throw new ArgumentNullException(nameof(skTypeface));

            mSKGLControl.PaintSurface += OnPaintSurface;
            mSize = new Size(mSKGLControl.Width, mSKGLControl.Height);
        }

        private void OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (mDisposed) return;

            var canvas = e.Surface.Canvas;
            canvas.Clear(mBackgroundColor);

            if (mDirty)
            {
                mCachedPicture?.Dispose();
                mCachedPicture = GeneratePicture();
                mDirty = false;
            }

            if (mCachedPicture != null)
                canvas.DrawPicture(mCachedPicture);
        }

        private SKPicture GeneratePicture()
        {
            if (mDisposed) return null;

            var recorder = new SKPictureRecorder();
            var canvas = recorder.BeginRecording(
                new SKRect(0, 0, mSize.Width, mSize.Height));

            foreach (var layer in mCanvasLayers)
                layer.ExecuteRenderCommands(canvas);

            return recorder.EndRecording();
        }

        public void InitCanvas(float width, float height, Color? backgroundColor = null)
        {
            if (mDisposed) return;
            ClearCanvas();

            mSize = new Size(width, height);
            mBackgroundColor = ToSK(backgroundColor ?? Color.Transparent);
            mSKGLControl.Width = (int)width;
            mSKGLControl.Height = (int)height;

            MarkDirty();
            ApplyCanvas();
        }

        public void ApplyCanvas()
        {
            mSKGLControl.Validate();
        }

        public void ClearCanvas()
        {
            if (mDisposed) return;

            ClearCanvasLayer();
            mCachedPicture?.Dispose();
            mCachedPicture = null;
            MarkDirty();
            ApplyCanvas();
        }

        public ICanvasLayer NewCanvasLayer()
        {
            var layer = new SKCanvasLayer(this, mSKTypeface);
            mCanvasLayers.Add(layer);
            MarkDirty();
            return layer;
        }

        public void ClearCanvasLayer()
        {
            if (mDisposed) return;
            foreach (var layer in mCanvasLayers)
            {
                layer?.Dispose();
            }
            mCanvasLayers.Clear();
            MarkDirty();
            ApplyCanvas();
        }

        internal void MarkDirty()
        {
            mDirty = true;
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            mSKGLControl.PaintSurface -= OnPaintSurface;
            ClearCanvas();
            mSKGLControl = null;
            mSKTypeface = null;
            mDisposed = true;
        }

        private static SKColor ToSK(Color c)
            => new SKColor(c.R, c.G, c.B, c.A);
    }
}