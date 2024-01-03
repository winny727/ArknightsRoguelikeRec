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
            int gap = 5;
            int height = 50;

            Button btn = new Button();
            btn.Text = layerName;
            btn.Font = new Font("黑体", 10.0f);
            btn.Click += onClick;
            panel.Controls.Add(btn);

            btn.Size = new Size(panel.Width - 2 * gap, height);
            btn.Location = new Point(gap, gap + (panel.Controls.Count - 1) * height);
        }

        public static void DrawGrid(Panel panel)
        {
            int step = 25;
            Graphics graphics = panel.CreateGraphics();
            Color color = Color.DarkGray;
            Pen pen = new Pen(color);

            for (int x = 0; x <= panel.Width; x += step)
            {
                PointF p1 = new PointF(x, 0);
                PointF p2 = new PointF(x, panel.Height);
                graphics.DrawLine(pen, p1, p2);
            }

            for (int y = 0; y <= panel.Height; y += step)
            {
                PointF p1 = new PointF(0, y);
                PointF p2 = new PointF(panel.Width, y);
                graphics.DrawLine(pen, p1, p2);
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
            int gap = 60;
            int width = 150;
            int height = 80;

            int nodeX = gap + colIndex * (width + gap);
            //int nodeY = (int)(panel.Height * ((float)(rowIndex + 1) / (rowCount + 1))) - height / 2;
            int nodeY = panel.Height / 2 - rowCount * gap + rowIndex * (height + gap) - height / 2;
            Panel nodeView = new Panel();
            panel.Controls.Add(nodeView);
            nodeView.BorderStyle = BorderStyle.FixedSingle;
            nodeView.Size = new Size(width, height);
            nodeView.Location = new Point(nodeX, nodeY);

            int btnGap = 2;
            int btnWidth = width - 2 * btnGap;
            int btnHeight = (height - 3 * btnGap) / 2;

            //Init Type
            Button btnType = new Button();
            nodeView.Controls.Add(btnType);
            btnType.Font = new Font("黑体", 10.0f);
            btnType.Text = node.Type;
            btnType.Size = new Size(btnWidth, btnHeight);
            btnType.Location = new Point(btnGap, btnGap);

            //Init SubType
            Button btnSubType = new Button();
            nodeView.Controls.Add(btnSubType);
            btnSubType.Font = new Font("黑体", 10.0f);
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
            int portSize = 20;
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

        /// <summary>
        /// 在PictureBox中绘制节点
        /// </summary>
        /// <param name="btnPictureBox"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowCount"></param>
        /// <param name="panelHeight"></param>
        /// <param name="node"></param>
        /// <param name="layerConfig"></param>
        public static void AddNodeBtn(BtnPictureBox btnPictureBox, int colIndex, int rowIndex, int rowCount, int panelHeight, Node node, LayerConfig layerConfig)
        {
            int gap = 50;
            int width = 150;
            int height = 80;

            int nodeX = gap + colIndex * (width + gap);
            //int nodeY = (int)((panelHeight - 2 * gap) * ((float)(rowIndex + 1) / (rowCount + 1))) - height / 2;
            int nodeY = panelHeight / 2 - rowCount * gap + rowIndex * (height + gap) - height / 2;
            BtnPictureBox.Item nodeView = new BtnPictureBox.Item()
            {
                Size = new Size(width, height),
                Location = new Point(nodeX, nodeY),
            };

            int btnGap = 2;
            int btnWidth = width - 2 * btnGap;
            int btnHeight = (height - 3 * btnGap) / 2;

            //Init Type
            BtnPictureBox.Item typeView = new BtnPictureBox.Item()
            {
                Text = node.Type,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(nodeX + btnGap, nodeY + btnGap),
            };

            //Init SubType
            BtnPictureBox.Item subTypeView = new BtnPictureBox.Item()
            {
                Text = node.SubType,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(nodeX + btnGap, nodeY + 2 * btnGap + btnHeight),
            };

            typeView.OnClick += () =>
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
                            if (typeView.Text != nodeConfig.Type)
                            {
                                subTypeView.Text = string.Empty;
                                node.SubType = string.Empty;
                            }

                            node.Type = nodeConfig.Type;
                            typeView.Text = nodeConfig.Type;
                            typeView.Tag = nodeConfig;
                            btnPictureBox.UpdateView();
                        });
                    }
                }

                if (!string.IsNullOrEmpty(typeView.Text) || !string.IsNullOrEmpty(subTypeView.Text))
                {
                    if (contextMenuStrip.Items.Count > 0)
                    {
                        contextMenuStrip.Items.Add(new ToolStripSeparator());
                    }

                    contextMenuStrip.Items.Add("清除", null, (_sender, _e) =>
                    {
                        node.Type = string.Empty;
                        typeView.Text = string.Empty;
                        typeView.Tag = null;

                        node.SubType = string.Empty;
                        subTypeView.Text = string.Empty;
                        btnPictureBox.UpdateView();
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
                        typeView.Tag = nodeConfig;
                        break;
                    }
                }
            }

            subTypeView.OnClick += () =>
            {
                if (typeView.Tag is NodeConfig nodeConfig)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    for (int i = 0; i < nodeConfig.SubTypes.Count; i++)
                    {
                        string subType = nodeConfig.SubTypes[i];
                        contextMenuStrip.Items.Add(subType, null, (_sender, _e) =>
                        {
                            node.SubType = subType;
                            subTypeView.Text = subType;
                            btnPictureBox.UpdateView();
                        });
                    }

                    if (!string.IsNullOrEmpty(subTypeView.Text))
                    {
                        if (contextMenuStrip.Items.Count > 0)
                        {
                            contextMenuStrip.Items.Add(new ToolStripSeparator());
                        }

                        contextMenuStrip.Items.Add("清除", null, (_sender, _e) =>
                        {
                            node.SubType = string.Empty;
                            subTypeView.Text = string.Empty;
                            btnPictureBox.UpdateView();
                        });
                    }

                    if (contextMenuStrip.Items.Count > 0)
                    {
                        contextMenuStrip.Show(Cursor.Position);
                    }
                }
            };

            void AddConnect()
            {
                //TODO
            }

            //Init Port
            int portSize = 20;
            BtnPictureBox.Item portView1 = new BtnPictureBox.Item()
            {
                Size = new Size(portSize, portSize),
                Location = new Point(nodeX + nodeView.Size.Width / 2 - portSize / 2, nodeY - portSize),
            };
            BtnPictureBox.Item portView2 = new BtnPictureBox.Item()
            {
                Size = new Size(portSize, portSize),
                Location = new Point(nodeX - portSize, nodeY + nodeView.Size.Height / 2 - portSize / 2),
            };
            BtnPictureBox.Item portView3 = new BtnPictureBox.Item()
            {
                Size = new Size(portSize, portSize),
                Location = new Point(nodeX + nodeView.Size.Width, nodeY + nodeView.Size.Height / 2 - portSize / 2),
            };
            BtnPictureBox.Item portView4 = new BtnPictureBox.Item()
            {
                Size = new Size(portSize, portSize),
                Location = new Point(nodeX + nodeView.Size.Width / 2 - portSize / 2, nodeY + nodeView.Size.Height),
            };
            portView1.OnClick += AddConnect;
            portView2.OnClick += AddConnect;
            portView3.OnClick += AddConnect;
            portView4.OnClick += AddConnect;

            btnPictureBox.Items.Add(nodeView);
            btnPictureBox.Items.Add(typeView);
            btnPictureBox.Items.Add(subTypeView);
            btnPictureBox.Items.Add(portView1);
            btnPictureBox.Items.Add(portView2);
            btnPictureBox.Items.Add(portView3);
            btnPictureBox.Items.Add(portView4);

            btnPictureBox.UpdateView();
        }
    }
}