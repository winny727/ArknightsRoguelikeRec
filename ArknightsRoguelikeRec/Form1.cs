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

        public SaveData SaveData { get; set; }
        public int SelectedLayer { get; set; }
        public bool IsDirty { get; set; }

        public Form1()
        {
            InitializeComponent();
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
                UIHelper.CreateLayerBtn(panelLayer, SaveData.Layers[i].Name, () =>
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
            panelCurLayer.Enabled = false;
            UpdateNodeText();
            UpdateNodeView();

            if (SaveData == null)
            {
                return;
            }

            if (SelectedLayer < 0 || SelectedLayer >= SaveData.Layers.Count)
            {
                return;
            }

            panelCurLayer.Enabled = true;
        }

        private void UpdateNodeText()
        {
            textBoxNode.Text = string.Empty;
            btnApply.Visible = false;
            btnCancel.Visible = false;

            if (SaveData == null)
            {
                return;
            }

            if (SelectedLayer < 0 || SelectedLayer >= SaveData.Layers.Count)
            {
                return;
            }

            Layer layer = SaveData.Layers[SelectedLayer];

            //节点分布数字
            string nodeText = string.Empty;
            for (int i = 0; i < layer.Nodes.Count; i++)
            {
                nodeText += layer.Nodes[i].Count;
            }
            textBoxNode.Text = nodeText;

            btnApply.Visible = false;
            btnCancel.Visible = false;
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

            pictureBoxNode.BackgroundImage?.Dispose();
            pictureBoxNode.Image?.Dispose();
            pictureBoxNode.BackgroundImage = null;
            pictureBoxNode.Image = null;

            if (SaveData == null)
            {
                return;
            }

            if (SelectedLayer < 0 || SelectedLayer >= SaveData.Layers.Count)
            {
                return;
            }

            Layer layer = SaveData.Layers[SelectedLayer];
            LayerConfig layerConfig = ConfigHelper.GetLayerConfigByName(layer.Name);

            int width = layer.Nodes.Count * (GlobalDefine.NODE_VIEW_H_GAP + GlobalDefine.NODE_VIEW_WIDTH) + GlobalDefine.NODE_VIEW_H_GAP;
            pictureBoxNode.Width = Math.Max(width, panelNodeView.Width - 2);
            pictureBoxNode.Height = panelNodeView.HorizontalScroll.Visible ? panelNodeView.Height - GlobalDefine.NODE_VIEW_SCROLL_GAP : panelNodeView.Height - 2;
            pictureBoxNode.BackgroundImage = new Bitmap(pictureBoxNode.Width, pictureBoxNode.Height);
            pictureBoxNode.Image = new Bitmap(pictureBoxNode.Width, pictureBoxNode.Height);

            UIHelper.DrawGrid(pictureBoxNode); //绘制背景网格

            List<NodeView> nodeViews = new List<NodeView>();
            for (int i = 0; i < layer.Nodes.Count; i++)
            {
                int rowCount = layer.Nodes[i].Count;
                for (int j = 0; j < layer.Nodes[i].Count; j++)
                {
                    //创建节点
                    Node node = layer.Nodes[i][j];
                    NodeView nodeView = UIHelper.CreateNodeView(panelNodeView, i, j, rowCount, node, layerConfig.NodeTypes);

                    //创建节点端口
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        UIHelper.CreateNodePort(panelNodeView, nodeView, direction);
                    }

                    nodeViews.Add(nodeView);
                }
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                NodeView nodeView1 = nodeViews[connection.NodeIndex1];
                NodeView nodeView2 = nodeViews[connection.NodeIndex2];
                UIHelper.DrawConnection(pictureBoxNode, nodeView1, nodeView2);
            }

            pictureBoxNode.SendToBack(); //置于底层
            pictureBoxNode.Refresh();
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
            if (SaveData == null)
            {
                return;
            }

            if (SelectedLayer < 0 || SelectedLayer >= SaveData.Layers.Count)
            {
                return;
            }

            Layer layer = SaveData.Layers[SelectedLayer];

            if (MessageBox.Show($"是否确认删除选中层\n{layer.Name}", "删除", MessageBoxButtons.OKCancel) == DialogResult.OK)
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
        }

        private void textBoxNode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8)
            {
                if ((e.KeyChar < '1') || (e.KeyChar > '4'))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UpdateNodeText();
            btnApply.Visible = false;
            btnCancel.Visible = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveData == null)
            {
                return;
            }

            if (SelectedLayer < 0 || SelectedLayer >= SaveData.Layers.Count)
            {
                return;
            }

            Layer layer = SaveData.Layers[SelectedLayer];

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

            UpdateNodeView();
        }

        private void panelNodeView_Scroll(object sender, ScrollEventArgs e)
        {
            panelNodeView.Refresh();
        }
    }
}
