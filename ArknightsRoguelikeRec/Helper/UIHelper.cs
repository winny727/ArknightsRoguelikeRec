using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.ViewModel;
using System.Drawing.Drawing2D;

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
            Color color = Color.DarkGray;

            for (int x = 0; x <= pictureBox.Width; x += step)
            {
                Point p1 = new Point(x, 0);
                Point p2 = new Point(x, pictureBox.Height);
                DrawUtil.DrawLine(bitmap, p1, p2, color);
            }

            for (int y = 0; y <= pictureBox.Height; y += step)
            {
                Point p1 = new Point(0, y);
                Point p2 = new Point(pictureBox.Width, y);
                DrawUtil.DrawLine(bitmap, p1, p2, color);
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
        public static NodeView CreateNodeView(Panel panel, int colIndex, int rowIndex, int rowCount, Node node, List<int> nodeTypes)
        {
            int hGap = GlobalDefine.NODE_VIEW_H_GAP;
            int vGap = GlobalDefine.NODE_VIEW_V_GAP;
            int width = GlobalDefine.NODE_VIEW_WIDTH;
            int height = GlobalDefine.NODE_VIEW_HEIGHT;

            //初始化节点
            int nodeX = hGap + colIndex * (width + hGap);
            int nodeY = panel.Height / 2 - rowCount * vGap + rowIndex * (height + vGap) - height / 2;
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
            btnSubType.Font = GlobalDefine.TEXT_FONT;
            btnSubType.Text = node.SubType;
            btnSubType.Size = new Size(btnWidth, btnHeight);
            btnSubType.Location = new Point(nodeX + btnGap, nodeY + 2 * btnGap + btnHeight);
            btnSubType.BringToFront();

            //选择节点类型
            btnType.Click += (sender, e) =>
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                if (nodeTypes != null)
                {
                    for (int i = 0; i < nodeTypes.Count; i++)
                    {
                        int nodeID = nodeTypes[i];
                        NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                        if (nodeConfig == null)
                        {
                            continue;
                        }

                        contextMenuStrip.Items.Add(nodeConfig.Type, null, (_sender, _e) =>
                        {
                            if (btnType.Text != nodeConfig.Type)
                            {
                                btnSubType.Text = string.Empty;
                                node.SubType = string.Empty;
                            }

                            node.Type = nodeConfig.Type;
                            btnType.Text = nodeConfig.Type;
                            btnView.Tag = nodeConfig;
                        });
                    }
                }

                //显示清除选项
                if (!string.IsNullOrEmpty(btnType.Text) || !string.IsNullOrEmpty(btnSubType.Text))
                {
                    if (contextMenuStrip.Items.Count > 0)
                    {
                        contextMenuStrip.Items.Add(new ToolStripSeparator());
                    }

                    contextMenuStrip.Items.Add("清除", null, (_sender, _e) =>
                    {
                        node.Type = string.Empty;
                        btnType.Text = string.Empty;
                        btnView.Tag = null;

                        node.SubType = string.Empty;
                        btnSubType.Text = string.Empty;
                    });
                }

                if (contextMenuStrip.Items.Count > 0)
                {
                    contextMenuStrip.Show(Cursor.Position);
                }
            };

            //初始化节点Tag
            if (nodeTypes != null)
            {
                for (int i = 0; i < nodeTypes.Count; i++)
                {
                    int nodeID = nodeTypes[i];
                    NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                    if (nodeConfig != null && nodeConfig.Type == node.Type)
                    {
                        btnView.Tag = nodeConfig;
                        break;
                    }
                }
            }

            //选择节点次级类型
            btnSubType.Click += (sender, e) =>
            {
                if (btnView.Tag is NodeConfig nodeConfig)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    for (int i = 0; i < nodeConfig.SubTypes.Count; i++)
                    {
                        string subType = nodeConfig.SubTypes[i];
                        contextMenuStrip.Items.Add(subType, null, (_sender, _e) =>
                        {
                            node.SubType = subType;
                            btnSubType.Text = subType;
                        });
                    }

                    //显示清除选项
                    if (!string.IsNullOrEmpty(btnSubType.Text))
                    {
                        if (contextMenuStrip.Items.Count > 0)
                        {
                            contextMenuStrip.Items.Add(new ToolStripSeparator());
                        }

                        contextMenuStrip.Items.Add("清除", null, (_sender, _e) =>
                        {
                            node.SubType = string.Empty;
                            btnSubType.Text = string.Empty;
                        });
                    }

                    if (contextMenuStrip.Items.Count > 0)
                    {
                        contextMenuStrip.Show(Cursor.Position);
                    }
                }
            };

            return new NodeView(node, colIndex, rowIndex, btnView, btnType, btnSubType);
        }

        public static void DrawConnection(PictureBox pictureBox, NodeView nodeView1, NodeView nodeView2)
        {
            Bitmap bitmap = (Bitmap)pictureBox.BackgroundImage;

            int offset = pictureBox.Location.X;
            int x1 = nodeView1.View.Location.X + nodeView1.View.Width / 2 - offset;
            int y1 = nodeView1.View.Location.Y + nodeView1.View.Height / 2;
            int x2 = nodeView2.View.Location.X + nodeView2.View.Width / 2 - offset;
            int y2 = nodeView2.View.Location.Y + nodeView2.View.Height / 2;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);

            DrawUtil.DrawBezier(bitmap, pt1, pt2, pt3, pt4, null, 2f);
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

            DrawUtil.DrawBezier(bitmap, pt1, pt2, pt3, pt4, null, 2f);
        }
    }
}