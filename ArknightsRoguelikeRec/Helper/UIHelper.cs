using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.ViewModel;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace ArknightsRoguelikeRec.Helper
{
    public static class UIHelper
    {
        /// <summary>
        /// 添加层数按钮
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="layerName"></param>
        /// <param name="onClick"></param>
        public static Button CreateLayerBtn(Panel panel, string layerName, Action onClick = null)
        {
            int gap = GlobalDefine.LAYER_BTN_GAP;
            int height = GlobalDefine.LAYER_BTN_HEIGHT;

            Button btnLayer = new Button();
            panel.Controls.Add(btnLayer);
            btnLayer.Text = layerName;
            btnLayer.Font = GlobalDefine.TEXT_FONT;

            btnLayer.Size = new Size(panel.Width - 2 * gap, height);
            btnLayer.Location = new Point(gap, gap + (panel.Controls.Count - 1) * height);

            btnLayer.Click += (sender, e) => onClick?.Invoke();

            return btnLayer;
        }

        /// <summary>
        /// 绘制背景网格
        /// </summary>
        /// <param name="pictureBox"></param>
        public static void DrawGrid(PictureBox pictureBox)
        {
            Bitmap bitmap = (Bitmap)pictureBox.BackgroundImage;

            int step = GlobalDefine.BACKGROUND_GRID_STEP;
            Color color = Color.LightGray;

            for (int x = 0; x <= pictureBox.Width; x += step)
            {
                Point p1 = new Point(x, 0);
                Point p2 = new Point(x, pictureBox.Height);
                float width = (x / step) % 5 == 0 ? 2f : 1f;
                DrawUtil.DrawLine(bitmap, p1, p2, color, width);
            }

            for (int y = 0; y <= pictureBox.Height; y += step)
            {
                Point p1 = new Point(0, y);
                Point p2 = new Point(pictureBox.Width, y);
                float width = (y / step) % 5 == 0 ? 2f : 1f;
                DrawUtil.DrawLine(bitmap, p1, p2, color, width);
            }
        }

        /// <summary>
        /// 将节点绘制为按钮
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowCount"></param>
        /// <param name="node"></param>
        /// <param name="layerConfig"></param>
        public static NodeView CreateNodeView(Panel panel, int colIndex, int rowIndex, int rowCount, Node node)
        {
            int hGap = GlobalDefine.NODE_VIEW_H_GAP;
            int vGap = GlobalDefine.NODE_VIEW_V_GAP;
            int width = GlobalDefine.NODE_VIEW_WIDTH;
            int height = GlobalDefine.NODE_VIEW_HEIGHT;

            //初始化节点
            int nodeX = hGap + colIndex * (width + hGap);
            int nodeY = panel.Height / 2 - (rowCount * height + (rowCount - 1) * vGap) / 2 + rowIndex * (height + vGap) - GlobalDefine.NODE_VIEW_SCROLL_GAP;
            Button btnView = new Button();
            panel.Controls.Add(btnView);
            btnView.Size = new Size(width, height);
            btnView.Location = new Point(nodeX, nodeY);

            int btnGap = GlobalDefine.NODE_VIEW_BTN_GAP;
            int btnWidth = width - 2 * btnGap;
            int btnHeight = (height - 3 * btnGap) / 2;

            //初始化节点类型选择按钮
            Button btnType = new Button();
            panel.Controls.Add(btnType);
            btnType.Font = GlobalDefine.TEXT_FONT;
            btnType.Text = node.Type;
            btnType.Size = new Size(btnWidth, btnHeight);
            btnType.Location = new Point(nodeX + btnGap, nodeY + btnGap);
            btnType.BringToFront();

            //初始化节点次级类型选择按钮
            Button btnSubType = new Button();
            panel.Controls.Add(btnSubType);
            btnSubType.Font = GlobalDefine.SUB_TEXT_FONT;
            btnSubType.Text = node.SubType;
            btnSubType.Size = new Size(btnWidth, btnHeight);
            btnSubType.Location = new Point(nodeX + btnGap, nodeY + 2 * btnGap + btnHeight);
            btnSubType.BringToFront();

            return new NodeView(node, colIndex, rowIndex, btnView, btnType, btnSubType);
        }

        public static void DrawConnection(PictureBox pictureBox, NodeView nodeView1, NodeView nodeView2)
        {
            Bitmap bitmap = (Bitmap)pictureBox.BackgroundImage;
            Color color = Color.Black;

            int offset = pictureBox.Location.X;
            int x1 = nodeView1.View.Location.X + nodeView1.View.Width / 2 - offset;
            int y1 = nodeView1.View.Location.Y + nodeView1.View.Height / 2;
            int x2 = nodeView2.View.Location.X + nodeView2.View.Width / 2 - offset;
            int y2 = nodeView2.View.Location.Y + nodeView2.View.Height / 2;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);

            DrawUtil.DrawBezier(bitmap, pt1, pt2, pt3, pt4, color, 2f);
        }

        public static Button CreateDelConnectionBtn(Panel panel, NodeView nodeView1, NodeView nodeView2, Action onClick = null)
        {
            Button btnDel = new Button();
            panel.Controls.Add(btnDel);
            btnDel.Text = "X";
            btnDel.Font = GlobalDefine.TEXT_FONT;

            int btnSize = GlobalDefine.CONNECTION_DELETE_BTN_SIZE;

            int x1 = nodeView1.View.Location.X + nodeView1.View.Width / 2;
            int y1 = nodeView1.View.Location.Y + nodeView1.View.Height / 2;
            int x2 = nodeView2.View.Location.X + nodeView2.View.Width / 2;
            int y2 = nodeView2.View.Location.Y + nodeView2.View.Height / 2;

            btnDel.Size = new Size(btnSize, btnSize);
            btnDel.Location = new Point(x1 + (x2 - x1) / 2 - btnSize / 2, y1 + (y2 - y1) / 2 - btnSize / 2);
            btnDel.BringToFront();

            btnDel.Click += (sender, e) => onClick?.Invoke();

            return btnDel;
        }

        public static void ClearConnectionPreview(PictureBox pictureBox)
        {
            pictureBox.Image?.Dispose();
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        public static void DrawConnectionPreview(PictureBox pictureBox, NodeView nodeView)
        {
            Bitmap bitmap = (Bitmap)pictureBox.Image;
            Color color = Color.Black;

            Point mousePos = Cursor.Position;
            Point locaction = pictureBox.PointToClient(mousePos);

            int offset = pictureBox.Location.X;
            int x1 = nodeView.View.Location.X + nodeView.View.Width / 2 - offset;
            int y1 = nodeView.View.Location.Y + nodeView.View.Height / 2;
            int x2 = locaction.X;
            int y2 = locaction.Y;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);

            DrawUtil.DrawBezier(bitmap, pt1, pt2, pt3, pt4, color, 2f);
        }

        public static void AddSeparator(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Items.Add(new ToolStripSeparator());
            }
        }

        public static void AddSeparatedMenuItem(ContextMenuStrip contextMenuStrip, string name, Action onAction)
        {
            AddSeparator(contextMenuStrip);
            contextMenuStrip.Items.Add(name, null, (_sender, _e) =>
            {
                onAction?.Invoke();
            });
        }

        public static void ShowMenu(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }
    }
}