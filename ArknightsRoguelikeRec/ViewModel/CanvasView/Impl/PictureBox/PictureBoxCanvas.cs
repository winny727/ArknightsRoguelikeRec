using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

using Size = ArknightsRoguelikeRec.ViewModel.DataStruct.Size;
using Color = ArknightsRoguelikeRec.ViewModel.DataStruct.Color;

using DrawingColor = System.Drawing.Color;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public class PictureBoxCanvas : ICanvas
    {
        private PictureBox mPictureBox;
        private Graphics mGraphics;
        private Font mTextFont;
        private Size mSize;

        private readonly List<ImageCanvasLayer> mCanvasLayers = new List<ImageCanvasLayer>();

        private bool mDisposed = false;

        public PictureBoxCanvas(PictureBox pictureBox, Font textFont = null)
        {
            mPictureBox = pictureBox ?? throw new ArgumentNullException(nameof(pictureBox));
            mTextFont = textFont ?? pictureBox.Font;
        }

        public void InitCanvas(float width, float height, Color? backgroundColor = null)
        {
            if (mDisposed) return;
            ClearCanvas();

            mSize = new Size(width, height);
            mPictureBox.BackgroundImage = CreateBitmap(width, height);
            mPictureBox.Image = CreateBitmap(width, height);
            mGraphics = Graphics.FromImage(mPictureBox.Image);
            mGraphics.CompositingMode = CompositingMode.SourceOver;
            mGraphics.CompositingQuality = CompositingQuality.HighSpeed;
            mGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            mGraphics.SmoothingMode = SmoothingMode.HighSpeed;

            Graphics backgroundGraphics = Graphics.FromImage(mPictureBox.BackgroundImage);
            mPictureBox.Size = PictureBoxHelper.ToDrawingSize(new Size(width, height));
            backgroundGraphics.Clear(PictureBoxHelper.ToDrawingColor(backgroundColor) ?? DrawingColor.White);
        }

        public void ApplyCanvas()
        {
            if (mDisposed) return;
            if (mGraphics != null)
            {
                mGraphics.Clear(DrawingColor.Transparent);
                foreach (var layer in mCanvasLayers)
                {
                    mGraphics.DrawImage(layer.Image, 0, 0);
                }
            }
            mPictureBox.Invalidate();
        }

        public void ClearCanvas()
        {
            if (mDisposed) return;

            ClearCanvasLayer();
            mGraphics?.Dispose();
            mPictureBox.BackgroundImage?.Dispose();
            mPictureBox.Image?.Dispose();
            mPictureBox.BackgroundImage = null;
            mPictureBox.Image = null;

            mGraphics = null;
        }

        public ICanvasLayer NewCanvasLayer()
        {
            ImageCanvasLayer layer = new ImageCanvasLayer(CreateBitmap(mSize.Width, mSize.Height), mTextFont);
            layer.Name = "Layer" + (mCanvasLayers.Count + 1);
            mCanvasLayers.Add(layer);
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
            ApplyCanvas();
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            ClearCanvas();
            mPictureBox = null;
            mTextFont = null;
            mDisposed = true;
        }

        private Bitmap CreateBitmap(float width, float height)
        {
            return new Bitmap(PictureBoxHelper.ConvertNumber(width), PictureBoxHelper.ConvertNumber(height));
        }
    }
}