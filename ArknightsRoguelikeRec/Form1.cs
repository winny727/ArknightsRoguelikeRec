using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.ViewModel;
using ArknightsRoguelikeRec.Helper;

namespace ArknightsRoguelikeRec
{
    public partial class Form1 : Form
    {
        public string SavePath = Environment.CurrentDirectory + "\\SaveData";

        public SaveData SaveData { get; private set; }
        public int SelectedLayer { get; private set; }
        public bool IsDirty { get; private set; }

        public NodeView CurNodeView { get; private set; }
        public bool IsEditMode { get; private set; }

        private List<NodeView> nodeViews = new List<NodeView>();
        private List<Button> delConnectionBtns = new List<Button>();

        private bool isDragging = false;
        private Point lastMousePos;

        public Form1()
        {
            InitializeComponent();

            comboBoxLayerType.DisplayMember = "Key";
            comboBoxLayerType.ValueMember = "Value";
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x02000000; //双缓冲
                return createParams;
            }
        }

        /*
         TODO：
        1. 可标记走过的节点（行动路径）；
        2. 可标记当前游戏状态完成/未完成；
        3. 存在未连接的节点时提醒；
        4. 由密文板进行的节点转换记录（如将某节点通过板子变成树洞）；
        5. 连接编辑状态高亮；
         */

        private void Form1_Load(object sender, EventArgs e)
        {
            DefineConfig.InitConfig();

            ////测试用
            //string layerConfig = Newtonsoft.Json.JsonConvert.SerializeObject(DefineConfig.LayerConfigDict.AsList(), Newtonsoft.Json.Formatting.Indented);
            //string nodeConfig = Newtonsoft.Json.JsonConvert.SerializeObject(DefineConfig.NodeConfigDict.AsList(), Newtonsoft.Json.Formatting.Indented);

            //Console.WriteLine(layerConfig);
            //Console.WriteLine(nodeConfig);

            UpdateViewState();
        }

        private void InitSaveDirectory()
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
        }

        private void UpdateViewState()
        {
            bool isEnabled = SaveData != null;
            panelAllLayer.Enabled = isEnabled;
            panelLayer.Enabled = isEnabled;
            panelInfo.Enabled = isEnabled;
            panelCurLayer.Enabled = isEnabled;
            pictureBoxNode.Enabled = isEnabled;
            btnSave.Enabled = isEnabled;

            UpdateInfoView();
            UpdateLayerView();
            UpdateLayerSelectedHighlight();
            UpdateCurLayerView();
        }

        private void UpdateInfoView()
        {
            textBoxUserID.Text = SaveData?.UserName ?? string.Empty;
            textBoxNumber.Text = SaveData?.DataID ?? string.Empty;
        }

        private void UpdateLayerView()
        {
            panelLayer.Controls.Clear();
            if (SaveData == null)
            {
                return;
            }

            for (int i = 0; i < SaveData.Layers.Count; i++)
            {
                int index = i;
                UIHelper.CreateLayerBtn(panelLayer, SaveData.Layers[i].CustomName, () =>
                {
                    SelectedLayer = index;
                    UpdateLayerSelectedHighlight();
                    UpdateCurLayerView();
                });
            }
        }

        private void UpdateLayerSelectedHighlight()
        {
            for (int i = 0; i < panelLayer.Controls.Count; i++)
            {
                KnownColor knownColor = i == SelectedLayer ? KnownColor.Highlight : KnownColor.Control;
                panelLayer.Controls[i].BackColor = Color.FromKnownColor(knownColor);
            }
        }

        private void UpdateCurLayerView()
        {
            UpdateLayerData();
            UpdateNodeView();

            Layer layer = GetCurLayer();
            panelCurLayer.Enabled = layer != null;
        }

        private Layer GetCurLayer()
        {
            if (SaveData != null && SelectedLayer >= 0 && SelectedLayer < SaveData.Layers.Count)
            {
                return SaveData.Layers[SelectedLayer];
            }
            return null;
        }

        private void UpdateLayerData()
        {
            comboBoxLayerType.Items.Clear();
            comboBoxLayerType.Visible = false;
            labelLayerType.Visible = false;
            textBoxNode.Text = string.Empty;
            btnApply.Visible = false;
            btnCancel.Visible = false;
            labelNodeTips.Visible = false;

            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            //层级类型
            LayerConfig layerConfig = ConfigHelper.GetLayerConfigByName(layer.Name);
            if (layerConfig?.LayerTypes != null && layerConfig.LayerTypes.Count > 0)
            {
                comboBoxLayerType.Items.Add(new Item("（无）", null));
                for (int i = 0; i < layerConfig.LayerTypes.Count; i++)
                {
                    string layerType = layerConfig.LayerTypes[i];
                    comboBoxLayerType.Items.Add(new Item(layerType, layerType));
                }

                int index = 0;
                for (int i = 0; i < comboBoxLayerType.Items.Count; i++)
                {
                    Item item = (Item)comboBoxLayerType.Items[i];
                    if (item.Value == layer.Type)
                    {
                        index = i;
                        break;
                    }
                }

                comboBoxLayerType.SelectedIndex = index;
                comboBoxLayerType.Visible = true;
                labelLayerType.Visible = true;
            }


            //节点分布数字
            string nodeText = string.Empty;
            for (int i = 0; i < layer.Nodes.Count; i++)
            {
                nodeText += layer.Nodes[i].Count;
            }
            textBoxNode.Text = nodeText;

            btnApply.Visible = false;
            btnCancel.Visible = false;
            labelNodeTips.Visible = layer.Nodes.Count <= 0;
        }

        private void UpdateNodeView()
        {
            for (int i = panelNodeView.Controls.Count - 1; i >= 0; i--)
            {
                if (panelNodeView.Controls[i] != pictureBoxNode)
                {
                    panelNodeView.Controls.RemoveAt(i);
                }
            }

            panelNodeView.AutoScrollPosition = new Point(0, 0);

            nodeViews.Clear();
            pictureBoxNode.BackgroundImage?.Dispose();
            pictureBoxNode.Image?.Dispose();
            pictureBoxNode.BackgroundImage = null;
            pictureBoxNode.Image = null;

            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            LayerConfig layerConfig = ConfigHelper.GetLayerConfigByName(layer.Name);

            int width = layer.Nodes.Count * (GlobalDefine.NODE_VIEW_H_GAP + GlobalDefine.NODE_VIEW_WIDTH) + GlobalDefine.NODE_VIEW_H_GAP;
            pictureBoxNode.Width = Math.Max(width, panelNodeView.Width - 2);
            pictureBoxNode.Height = panelNodeView.HorizontalScroll.Visible ? panelNodeView.Height - GlobalDefine.NODE_VIEW_SCROLL_GAP : panelNodeView.Height - 2;
            pictureBoxNode.BackgroundImage = new Bitmap(pictureBoxNode.Width, pictureBoxNode.Height);

            UIHelper.DrawGrid(pictureBoxNode); //绘制背景网格

            for (int i = 0; i < layer.Nodes.Count; i++)
            {
                int rowCount = layer.Nodes[i].Count;
                for (int j = 0; j < layer.Nodes[i].Count; j++)
                {
                    //创建节点
                    Node node = layer.Nodes[i][j];
                    NodeView nodeView = UIHelper.CreateNodeView(panelNodeView, i, j, rowCount, node);

                    void OnNodeClick(object sender, EventArgs e)
                    {
                        nodeView.View.Focus();
                        if (!IsEditMode)
                        {
                            return;
                        }

                        UIHelper.ClearConnectionPreview(pictureBoxNode);
                        if (CurNodeView == null)
                        {
                            CurNodeView = nodeView;
                            UIHelper.DrawConnectionPreview(pictureBoxNode, nodeView);
                        }
                        else
                        {
                            NodeView tempNodeView = CurNodeView;
                            CurNodeView = null;
                            if (CheckConnectionValid(layer, tempNodeView, nodeView))
                            {
                                DataHelper.AddConnection(layer, tempNodeView.Node, nodeView.Node);
                                UpdateConnection();
                                UpdateEditMode();
                            }
                        }
                    }

                    void OnNodeTypeClick(object sender, EventArgs e)
                    {
                        if (IsEditMode)
                        {
                            return;
                        }

                        //选择节点类型
                        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                        if (layerConfig?.NodeTypes != null)
                        {
                            for (int i = 0; i < layerConfig.NodeTypes.Count; i++)
                            {
                                int nodeID = layerConfig.NodeTypes[i];
                                if (nodeID == 0)
                                {
                                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                                    continue;
                                }

                                NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                                if (nodeConfig == null)
                                {
                                    continue;
                                }

                                contextMenuStrip.Items.Add(nodeConfig.Type, null, (_sender, _e) =>
                                {
                                    if (nodeView.TypeView.Text != nodeConfig.Type)
                                    {
                                        nodeView.SubTypeView.Text = string.Empty;
                                        node.SubType = string.Empty;
                                    }

                                    node.Type = nodeConfig.Type;
                                    nodeView.TypeView.Text = nodeConfig.Type;
                                    nodeView.NodeConfig = nodeConfig;
                                });
                            }
                        }

                        //显示清除选项
                        if (!string.IsNullOrEmpty(nodeView.TypeView.Text) || !string.IsNullOrEmpty(nodeView.SubTypeView.Text))
                        {
                            if (contextMenuStrip.Items.Count > 0)
                            {
                                contextMenuStrip.Items.Add(new ToolStripSeparator());
                            }

                            contextMenuStrip.Items.Add("清除", null, (_sender, _e) =>
                            {
                                node.Type = string.Empty;
                                nodeView.TypeView.Text = string.Empty;
                                nodeView.NodeConfig = null;

                                node.SubType = string.Empty;
                                nodeView.SubTypeView.Text = string.Empty;
                            });
                        }

                        if (contextMenuStrip.Items.Count > 0)
                        {
                            contextMenuStrip.Show(Cursor.Position);
                        }
                    }

                    void OnNodeSubTypeClick(object sender, EventArgs e)
                    {
                        if (IsEditMode)
                        {
                            return;
                        }

                        //选择节点次级类型
                        if (nodeView.NodeConfig != null)
                        {
                            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                            for (int i = 0; i < nodeView.NodeConfig.SubTypes.Count; i++)
                            {
                                string subType = nodeView.NodeConfig.SubTypes[i];

                                if (string.IsNullOrEmpty(subType))
                                {
                                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                                    continue;
                                }

                                contextMenuStrip.Items.Add(subType, null, (_sender, _e) =>
                                {
                                    node.SubType = subType;
                                    nodeView.SubTypeView.Text = subType;
                                });
                            }

                            //树洞层类型
                            if (nodeView.NodeConfig.ExtraLayer > 0)
                            {
                                for (int i = 0; i < SaveData.Layers.Count; i++)
                                {
                                    Layer curLayer = SaveData.Layers[i];
                                    if (curLayer == layer)
                                    {
                                        continue;
                                    }

                                    LayerConfig curLayerConfig = ConfigHelper.GetLayerConfigByName(curLayer.Name);
                                    if (curLayerConfig != null && curLayerConfig.ID == nodeView.NodeConfig.ExtraLayer)
                                    {
                                        contextMenuStrip.Items.Add(curLayer.CustomName, null, (_sender, _e) =>
                                        {
                                            node.SubType = curLayer.CustomName;
                                            nodeView.SubTypeView.Text = curLayer.CustomName;
                                        });
                                    }
                                }
                            }

                            //显示清除选项
                            if (!string.IsNullOrEmpty(nodeView.SubTypeView.Text))
                            {
                                if (contextMenuStrip.Items.Count > 0)
                                {
                                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                                }

                                contextMenuStrip.Items.Add("清除", null, (_sender, _e) =>
                                {
                                    node.SubType = string.Empty;
                                    nodeView.SubTypeView.Text = string.Empty;
                                });
                            }

                            if (contextMenuStrip.Items.Count > 0)
                            {
                                contextMenuStrip.Show(Cursor.Position);
                            }
                        }
                    }

                    nodeView.View.Click += OnNodeClick;
                    nodeView.TypeView.Click += OnNodeClick;
                    nodeView.SubTypeView.Click += OnNodeClick;
                    nodeView.TypeView.Click += OnNodeTypeClick;
                    nodeView.SubTypeView.Click += OnNodeSubTypeClick;

                    //初始化NodeConfig
                    if (layerConfig?.NodeTypes != null)
                    {
                        for (int k = 0; k < layerConfig.NodeTypes.Count; k++)
                        {
                            int nodeID = layerConfig.NodeTypes[k];
                            NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                            if (nodeConfig != null && nodeConfig.Type == node.Type)
                            {
                                nodeView.NodeConfig = nodeConfig;
                                break;
                            }
                        }
                    }

                    nodeViews.Add(nodeView);
                }
            }

            //绘制连接
            UpdateConnection();

            //编辑模式状态更新
            UpdateEditMode();

            pictureBoxNode.SendToBack(); //置于底层
            pictureBoxNode.Refresh();
        }

        private void UpdateConnection()
        {
            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                NodeView nodeView1 = nodeViews[connection.Idx1];
                NodeView nodeView2 = nodeViews[connection.Idx2];
                UIHelper.DrawConnection(pictureBoxNode, nodeView1, nodeView2);
            }
        }

        private bool CheckConnectionValid(Layer layer, NodeView nodeView1, NodeView nodeView2)
        {
            if (nodeView1.ColIndex == nodeView2.ColIndex && nodeView1.RowIndex == nodeView2.RowIndex)
            {
                return false;
            }

            int index1 = nodeViews.IndexOf(nodeView1);
            int index2 = nodeViews.IndexOf(nodeView2);

            if (index1 < 0 || index2 < 0)
            {
                return false;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                if ((connection.Idx1 == index1 && connection.Idx2 == index2) || (connection.Idx1 == index2 && connection.Idx2 == index1))
                {
                    MessageBox.Show("选中节点间已存在连接", "连接失败");
                    return false;
                }
            }

            int colDelta = Math.Abs(nodeView1.ColIndex - nodeView2.ColIndex);
            int rowDelta = Math.Abs(nodeView1.RowIndex - nodeView2.RowIndex);
            if ((colDelta == 0 && rowDelta > 1) || colDelta > 1)
            {
                MessageBox.Show("选中节点无法连接", "连接失败");
                return false;
            }

            return true;
        }

        private bool CheckSaveData()
        {
            if (SaveData == null)
            {
                return true;
            }

            if (!IsDirty)
            {
                return true;
            }

            DialogResult dialogResult = MessageBox.Show("是否保存当前数据？", "保存", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                SaveCurrentData();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return false;
            }

            return true;
        }

        private string SaveCurrentData()
        {
            if (SaveData == null)
            {
                return null;
            }

            SaveData.UserName = textBoxUserID.Text;
            SaveData.DataID = textBoxNumber.Text;

            InitSaveDirectory();
            string savePath = DataHelper.SaveData(SaveData, SavePath);
            IsDirty = false;

            return savePath;
        }

        private void UpdateEditMode()
        {
            CurNodeView = null;
            for (int i = 0; i < delConnectionBtns.Count; i++)
            {
                delConnectionBtns[i].Dispose();
            }
            delConnectionBtns.Clear();
            UIHelper.ClearConnectionPreview(pictureBoxNode);

            if (IsEditMode)
            {
                btnEditConnection.BackColor = Color.FromKnownColor(KnownColor.Highlight);
                btnEditConnection.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);
                btnEditConnection.Text = "退出编辑";
                Layer layer = GetCurLayer();
                if (layer != null)
                {
                    for (int i = 0; i < layer.Connections.Count; i++)
                    {
                        var connection = layer.Connections[i];
                        NodeView nodeView1 = nodeViews[connection.Idx1];
                        NodeView nodeView2 = nodeViews[connection.Idx2];
                        Button btnDel = null;
                        btnDel = UIHelper.CreateDelConnectionBtn(panelNodeView, nodeView1, nodeView2, () =>
                        {
                            btnDel.Dispose();
                            delConnectionBtns.Remove(btnDel);

                            DataHelper.RemoveConnection(layer, connection);

                            pictureBoxNode.BackgroundImage = new Bitmap(pictureBoxNode.Width, pictureBoxNode.Height);
                            UIHelper.DrawGrid(pictureBoxNode); //绘制背景网格

                            UpdateConnection();
                            pictureBoxNode.Refresh();
                        });
                        delConnectionBtns.Add(btnDel);
                    }
                }
            }
            else
            {
                btnEditConnection.BackColor = Color.FromKnownColor(KnownColor.Control);
                btnEditConnection.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                btnEditConnection.Text = "编辑连接";
            }

            ////隐藏类型选择按钮
            //for (int i = 0; i < nodeViews.Count; i++)
            //{
            //    NodeView nodeView = nodeViews[i];
            //    nodeView.TypeView.Visible = !IsEditMode;
            //    nodeView.SubTypeView.Visible = !IsEditMode;
            //}
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            var layerList = DefineConfig.LayerConfigDict.AsList();
            for (int i = 0; i < layerList.Count; i++)
            {
                LayerConfig layerConfig = layerList[i];
                contextMenuStrip.Items.Add(layerConfig.Name, null, (_sender, _e) =>
                {
                    DataHelper.AddLayer(SaveData, layerConfig.Name);
                    SelectedLayer = panelLayer.Controls.Count;
                    IsDirty = true;
                    UpdateLayerView();
                    UpdateLayerSelectedHighlight();
                    UpdateCurLayerView();
                });
            }

            contextMenuStrip.Show(Cursor.Position);
        }

        private void btnRemoveLayer_Click(object sender, EventArgs e)
        {
            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            if (MessageBox.Show($"是否确认删除选中层\n{layer.CustomName}", "删除", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SaveData.Layers.RemoveAt(SelectedLayer);
                if (SelectedLayer >= SaveData.Layers.Count)
                {
                    SelectedLayer = SaveData.Layers.Count - 1;
                }
                IsDirty = true;
                UpdateLayerView();
                UpdateLayerSelectedHighlight();
                UpdateCurLayerView();
            }
        }

        private void linkLabelOpenSaveFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InitSaveDirectory();
            Process.Start("Explorer.exe", SavePath);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (CheckSaveData())
            {
                string userName = !string.IsNullOrEmpty(textBoxUserID.Text) ? textBoxUserID.Text : "Player";
                if (int.TryParse(textBoxNumber.Text, out int dataID))
                {
                    dataID++;
                }
                SaveData = DataHelper.CreateData(userName, dataID.ToString());
                SelectedLayer = 0;
                IsDirty = false;
                UpdateViewState();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData == null)
            {
                return;
            }

            string savePath = SaveCurrentData();
            if (!string.IsNullOrEmpty(savePath))
            {
                MessageBox.Show($"保存成功\n{savePath}", "保存");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            InitSaveDirectory();
            if (CheckSaveData())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = SavePath;
                dialog.Multiselect = false;
                dialog.Filter = "Json Files|*.json";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveData = DataHelper.LoadData(dialog.FileName);
                    SelectedLayer = 0;
                    IsDirty = false;
                    UpdateViewState();
                }
            }
        }

        private void textBoxUserID_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void textBoxNumber_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void textBoxNode_TextChanged(object sender, EventArgs e)
        {
            btnApply.Visible = true;
            btnCancel.Visible = true;
            labelNodeTips.Visible = false;
        }

        private void textBoxNode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8)
            {
                if (!int.TryParse(e.KeyChar.ToString(), out int count) || count < GlobalDefine.ROW_MIN_NODE || count > GlobalDefine.ROW_MAX_NODE)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UpdateLayerData();
            btnApply.Visible = false;
            btnCancel.Visible = false;
            labelNodeTips.Visible = false;

            Layer layer = GetCurLayer();
            if (layer != null)
            {
                labelNodeTips.Visible = layer.Nodes.Count <= 0;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            if (layer.Nodes.Count > 0 || layer.Connections.Count > 0)
            {
                if (MessageBox.Show("更改节点分布将会清空当前层已选择的节点类型，是否继续？", "更改节点分布", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }

            layer.Nodes.Clear();
            layer.Connections.Clear();
            for (int i = 0; i < textBoxNode.Text.Length; i++)
            {
                DataHelper.AddColume(layer);
                int.TryParse(textBoxNode.Text[i].ToString(), out int num);
                for (int j = 0; j < num; j++)
                {
                    DataHelper.AddNode(layer, i);
                }
            }

            //初始化部分固定（只有一种连法）连接
            for (int i = 0; i < layer.Nodes.Count - 1; i++)
            {
                var curColNodes = layer.Nodes[i];
                var nextColNodes = layer.Nodes[i + 1];

                if (curColNodes.Count <= 0 || nextColNodes.Count <= 0)
                {
                    continue;
                }

                Node curColFirstNode = curColNodes[0];
                Node curColLastNode = curColNodes[curColNodes.Count - 1];
                Node nextColFirstNode = nextColNodes[0];
                Node nextColLastNode = nextColNodes[nextColNodes.Count - 1];

                DataHelper.AddConnection(layer, curColFirstNode, nextColFirstNode); //已去重
                DataHelper.AddConnection(layer, curColLastNode, nextColLastNode);

                if (curColNodes.Count == 1)
                {
                    for (int j = 0; j < nextColNodes.Count; j++)
                    {
                        Node nextColNode = nextColNodes[j];
                        DataHelper.AddConnection(layer, curColFirstNode, nextColNode);
                    }
                }

                if (nextColNodes.Count == 1)
                {
                    for (int j = 0; j < curColNodes.Count; j++)
                    {
                        Node curColNode = curColNodes[j];
                        DataHelper.AddConnection(layer, curColNode, nextColFirstNode);
                    }
                }
            }

            UpdateNodeView();

            btnApply.Visible = false;
            btnCancel.Visible = false;
            labelNodeTips.Visible = layer.Nodes.Count <= 0;
        }

        private void panelNodeView_Scroll(object sender, ScrollEventArgs e)
        {
            panelNodeView.Refresh();
        }

        private void btnEditConnection_Click(object sender, EventArgs e)
        {
            IsEditMode = !IsEditMode;
            UpdateEditMode();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsEditMode || CurNodeView == null)
            {
                return;
            }

            UIHelper.ClearConnectionPreview(pictureBoxNode);
            UIHelper.DrawConnectionPreview(pictureBoxNode, CurNodeView);
        }

        private void pictureBoxNode_Click(object sender, EventArgs e)
        {
            if (!IsEditMode)
            {
                return;
            }

            CurNodeView = null;
            UIHelper.ClearConnectionPreview(pictureBoxNode);
        }

        private void pictureBoxNode_MouseDown(object sender, MouseEventArgs e)
        {
            lastMousePos = pictureBoxNode.PointToScreen(e.Location);
            isDragging = true;
        }

        private void pictureBoxNode_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                return;
            }

            Point curMousePos = pictureBoxNode.PointToScreen(e.Location);
            int deltaX = curMousePos.X - lastMousePos.X;
            int deltaY = curMousePos.Y - lastMousePos.Y;
            panelNodeView.AutoScrollPosition = new Point(-(panelNodeView.AutoScrollPosition.X + deltaX), -(panelNodeView.AutoScrollPosition.Y + deltaY));
            lastMousePos = curMousePos;
            panelNodeView.Refresh();
        }

        private void pictureBoxNode_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnSave_Click(sender, e);
            }
        }

        private void comboBoxLayerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            int index = comboBoxLayerType.SelectedIndex;
            if (index < 0 || index >= comboBoxLayerType.Items.Count)
            {
                return;
            }

            Item item = (Item)comboBoxLayerType.Items[index];
            layer.Type = item.Value;
        }
    }
}
