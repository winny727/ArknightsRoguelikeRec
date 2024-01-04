using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.ViewModel;

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
        public static void CreateLayerBtn(Panel panel, string layerName, Action onClick)
        {
            int gap = GlobalDefine.LAYER_BTN_GAP;
            int height = GlobalDefine.LAYER_BTN_HEIGHT;

            Button btn = new Button();
            btn.Text = layerName;
            btn.Font = GlobalDefine.TEXT_FONT;
            btn.Click += (sender, e) => onClick?.Invoke();
            panel.Controls.Add(btn);

            btn.Size = new Size(panel.Width - 2 * gap, height);
            btn.Location = new Point(gap, gap + (panel.Controls.Count - 1) * height);
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
            Panel viewPanel = new Panel();
            panel.Controls.Add(viewPanel);
            viewPanel.BorderStyle = BorderStyle.FixedSingle;
            viewPanel.Size = new Size(width, height);
            viewPanel.Location = new Point(nodeX, nodeY);

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
                            viewPanel.Tag = nodeConfig;
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
                        viewPanel.Tag = null;

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
                        viewPanel.Tag = nodeConfig;
                        break;
                    }
                }
            }

            //选择节点次级类型
            btnSubType.Click += (sender, e) =>
            {
                if (viewPanel.Tag is NodeConfig nodeConfig)
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

            return new NodeView(node, colIndex, rowIndex, viewPanel);
        }

        public static Control CreateNodePort(Panel panel, NodeView nodeView, Direction direction)
        {
            int portSize = GlobalDefine.NODE_VIEW_PORT_SIZE;
            Button btnPort = new Button();
            panel.Controls.Add(btnPort);
            btnPort.Size = new Size(portSize, portSize);
            Point offset = GetPortOffset(direction);
            Point nodeViewLocation = nodeView.View.Location;
            btnPort.Location = new Point(nodeViewLocation.X + offset.X, nodeViewLocation.Y + offset.Y);

            nodeView.Ports.Add(direction, btnPort);

            return btnPort;
        }

        private static Point GetPortOffset(Direction direction)
        {
            int nodeWidth = GlobalDefine.NODE_VIEW_WIDTH;
            int nodeHeight = GlobalDefine.NODE_VIEW_HEIGHT;
            int portSize = GlobalDefine.NODE_VIEW_PORT_SIZE;
            switch (direction)
            {
                case Direction.Top:
                    return new Point(nodeWidth / 2 - portSize / 2, -portSize);
                case Direction.Bottom:
                    return new Point(nodeWidth / 2 - portSize / 2, nodeHeight);
                case Direction.Left:
                    return new Point(-portSize, nodeHeight / 2 - portSize / 2);
                case Direction.Right:
                    return new Point(nodeWidth, nodeHeight / 2 - portSize / 2);
            }
            return default;
        }

        public static void DrawConnection(PictureBox pictureBox, NodeView nodeView1, NodeView nodeView2)
        {
            Control port1 = null;
            Control port2 = null;
            if (nodeView1.ColIndex == nodeView2.ColIndex)
            {
                if (nodeView1.RowIndex > nodeView2.RowIndex)
                {
                    port1 = nodeView1.Ports[Direction.Bottom];
                    port2 = nodeView2.Ports[Direction.Top];
                }
                else if (nodeView1.RowIndex < nodeView2.RowIndex)
                {
                    port1 = nodeView1.Ports[Direction.Top];
                    port2 = nodeView2.Ports[Direction.Bottom];
                }
            }
            else if (nodeView1.ColIndex < nodeView2.ColIndex)
            {
                port1 = nodeView1.Ports[Direction.Right];
                port2 = nodeView2.Ports[Direction.Left];
            }
            else if (nodeView1.ColIndex > nodeView2.ColIndex)
            {
                port1 = nodeView1.Ports[Direction.Left];
                port2 = nodeView2.Ports[Direction.Right];
            }

            if (port1 == null || port2 == null)
            {
                return;
            }

            Bitmap bitmap = (Bitmap)pictureBox.BackgroundImage;
            int portSize = GlobalDefine.NODE_VIEW_PORT_SIZE;

            int x1 = port1.Location.X + portSize / 2;
            int y1 = port1.Location.Y + portSize / 2;
            int x2 = port2.Location.X + portSize / 2;
            int y2 = port2.Location.Y + portSize / 2;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x1 + (x2 - x1) / 2, y1);
            Point pt3 = new Point(x2 - (x2 - x1) / 2, y2);
            Point pt4 = new Point(x2, y2);

            DrawUtil.DrawBezier(bitmap, pt1, pt2, pt3, pt4, null, 2f);
        }

        public static void DrawConnectionPreview(PictureBox pictureBox, Control port)
        {
            Bitmap bitmap = (Bitmap)pictureBox.BackgroundImage;
            int portSize = GlobalDefine.NODE_VIEW_PORT_SIZE;

            Point mousePos = Cursor.Position;
            Point locaction = pictureBox.PointToClient(mousePos);

            int x1 = port.Location.X + portSize / 2;
            int y1 = port.Location.Y + portSize / 2;
            int x2 = locaction.X;
            int y2 = locaction.Y;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x1 + (x2 - x1) / 2, y1);
            Point pt3 = new Point(x2 - (x2 - x1) / 2, y2);
            Point pt4 = new Point(x2, y2);

            DrawUtil.DrawBezier(bitmap, pt1, pt2, pt3, pt4, null, 2f);
        }
    }
}