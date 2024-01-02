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

        public static void AddNodeBtn(Panel panel, int colIndex, int rowIndex, int rowCount, Node node, LayerConfig layerConfig)
        {
            int gap = 50;
            int width = 150;
            int height = 80;

            Panel nodeView = new Panel();
            panel.Controls.Add(nodeView);
            nodeView.BorderStyle = BorderStyle.FixedSingle;
            nodeView.Size = new Size(width, height);
            nodeView.Location = new Point(gap + colIndex * (width + gap), (int)((panel.Height - 2 * gap) * ((float)(rowIndex + 1) / (rowCount + 1))));

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
                        if (nodeConfig != null)
                        {
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
                NodeConfig nodeConfig = btnType.Tag as NodeConfig;
                if(nodeConfig != null)
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

        }
    }
}
