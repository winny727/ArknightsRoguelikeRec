using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

public class BtnPictureBox : PictureBox
{
    public class Item
    {
        public Size Size { get; set; }
        public Point Location { get; set; }
        public string Text { get; set; }
        public Color BorderColor { get; set; } = Color.Black;
        public Color BackColor { get; set; } = Color.White;
        public Color TextColor { get; set; } = Color.Black;
        public Font Font { get; set; } = new Font("黑体", 10.0f);
        public Action OnClick { get; set; }
        public object Tag { get; set; }
    }

    public List<Item> Items = new List<Item>();

    public BtnPictureBox()
    {
        MouseClick += (sender, e) =>
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Item item = Items[i];
                Rectangle rectangle = new Rectangle(item.Location, item.Size);
                if (rectangle.Contains(e.Location))
                {
                    item.OnClick?.Invoke();
                }
            }
        };
    }

    public void UpdateView()
    {
        Image?.Dispose();
        Image = null;

        int maxX = -1;
        int maxY = -1;

        for (int i = 0; i < Items.Count; i++)
        {
            Item item = Items[i];
            if (item.Location.X + item.Size.Width > maxX)
            {
                maxX = item.Location.X + item.Size.Width;
            }
            if (item.Location.Y + item.Size.Height > maxY)
            {
                maxY = item.Location.Y + item.Size.Height;
            }
        }

        if (maxX <= 0 || maxY <= 0)
        {
            return;
        }

        Width = maxX + 1;
        Height = maxY + 1;
        Bitmap bitmap = new Bitmap(Width, Height);
        Image = bitmap;

        for (int i = 0; i < Items.Count; i++)
        {
            Item item = Items[i];
            Rectangle rectangle = new Rectangle(item.Location, item.Size);

            DrawUtil.FillRectangle(bitmap, rectangle, item.BackColor);
            DrawUtil.DrawRectangle(bitmap, rectangle, item.BorderColor);

            if (!string.IsNullOrEmpty(item.Text))
            {
                DrawUtil.DrawString(bitmap, item.Text, rectangle, item.Font, item.TextColor);
            }
        }
    }
}

