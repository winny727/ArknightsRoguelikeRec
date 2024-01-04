using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;

namespace ArknightsRoguelikeRec.Helper
{
    public static class UIHelper
    {

        public static void AddLayerBtn(Panel panel, string layerName, EventHandler onClick)
        {
            int gap = GlobalDefine.LAYER_BTN_GAP;
            int height = GlobalDefine.LAYER_BTN_HEIGHT;

            Button btn = new Button();
            btn.Text = layerName;
            btn.Font = GlobalDefine.TEXT_FONT;
            btn.Click += onClick;
            panel.Controls.Add(btn);

            btn.Size = new Size(panel.Width - 2 * gap, height);
            btn.Location = new Point(gap, gap + (panel.Controls.Count - 1) * height);
        }

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
        public static void AddNodeBtn(Panel panel, int colIndex, int rowIndex, int rowCount, Node node, LayerConfig layerConfig)
        {
            int gap = GlobalDefine.NODE_VIEW_GAP;
            int width = GlobalDefine.NODE_VIEW_WIDTH;
            int height = GlobalDefine.NODE_VIEW_HEIGHT;

            int nodeX = gap + colIndex * (width + gap);
            int nodeY = panel.Height / 2 - rowCount * gap + rowIndex * (height + gap) - height / 2;
            Panel nodeView = new Panel();
            panel.Controls.Add(nodeView);
            nodeView.BorderStyle = BorderStyle.FixedSingle;
            nodeView.Size = new Size(width, height);
            nodeView.Location = new Point(nodeX, nodeY);

            int btnGap = GlobalDefine.NODE_VIEW_BTN_GAP;
            int btnWidth = width - 2 * btnGap;
            int btnHeight = (height - 3 * btnGap) / 2;

            //Init Type
            Button btnType = new Button();
            nodeView.Controls.Add(btnType);
            btnType.Font = GlobalDefine.TEXT_FONT;
            btnType.Text = node.Type;
            btnType.Size = new Size(btnWidth, btnHeight);
            btnType.Location = new Point(btnGap, btnGap);

            //Init SubType
            Button btnSubType = new Button();
            nodeView.Controls.Add(btnSubType);
            btnSubType.Font = GlobalDefine.TEXT_FONT;
            btnSubType.Text = node.SubType;
            btnSubType.Size = new Size(btnWidth, btnHeight);
            btnSubType.Location = new Point(btnGap, 2 * btnGap + btnHeight);

            btnType.Click += (sender, e) =>
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                if (layerConfig != null)
                {
                    for (int i = 0; i < layerConfig.NodeTypes.Count; i++)
                    {
                        int nodeID = layerConfig.NodeTypes[i];
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
                            btnType.Tag = nodeConfig;
                        });
                    }
                }

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
                        btnType.Tag = null;

                        node.SubType = string.Empty;
                        btnSubType.Text = string.Empty;
                    });
                }

                if (contextMenuStrip.Items.Count > 0)
                {
                    contextMenuStrip.Show(Cursor.Position);
                }
            };

            if (layerConfig != null)
            {
                for (int i = 0; i < layerConfig.NodeTypes.Count; i++)
                {
                    int nodeID = layerConfig.NodeTypes[i];
                    NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                    if (nodeConfig != null && nodeConfig.Type == node.Type)
                    {
                        btnType.Tag = nodeConfig;
                        break;
                    }
                }
            }

            btnSubType.Click += (sender, e) =>
            {
                if (btnType.Tag is NodeConfig nodeConfig)
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

            //InitPort
            int portSize = GlobalDefine.NODE_VIEW_PORT_SIZE;
            Button btnPort1 = new Button();
            Button btnPort2 = new Button();
            Button btnPort3 = new Button();
            Button btnPort4 = new Button();
            panel.Controls.Add(btnPort1);
            panel.Controls.Add(btnPort2);
            panel.Controls.Add(btnPort3);
            panel.Controls.Add(btnPort4);
            btnPort1.Size = new Size(portSize, portSize);
            btnPort2.Size = new Size(portSize, portSize);
            btnPort3.Size = new Size(portSize, portSize);
            btnPort4.Size = new Size(portSize, portSize);
            btnPort1.Location = new Point(nodeX + nodeView.Size.Width / 2 - portSize / 2, nodeY - portSize);
            btnPort2.Location = new Point(nodeX - portSize, nodeY + nodeView.Size.Height / 2 - portSize / 2);
            btnPort3.Location = new Point(nodeX + nodeView.Size.Width, nodeY + nodeView.Size.Height / 2 - portSize / 2);
            btnPort4.Location = new Point(nodeX + nodeView.Size.Width / 2 - portSize / 2, nodeY + nodeView.Size.Height);
        }
    }
}