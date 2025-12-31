using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Helper;
using ArknightsRoguelikeRec.ViewModel;
using ArknightsRoguelikeRec.ViewModel.Impl;

namespace ArknightsRoguelikeRec
{
    public partial class MainForm : Form
    {
        public string SavePath = Environment.CurrentDirectory + "\\SaveData";

        public SaveData SaveData { get; private set; }
        public int SelectedLayer { get; private set; }
        public bool IsDirty { get; private set; }

        public NodeView CurNodeView { get; private set; }

        private InputForm mInputForm = new InputForm();

        private bool mIsDragging = false;
        private Point mLastMousePos;

        private CanvasView mCanvasView;

        public MainForm()
        {
            InitializeComponent();

            textBoxNode.MaxLength = GlobalDefine.MAX_COLUMU;
            comboBoxLayerType.DisplayMember = "Key";
            comboBoxLayerType.ValueMember = "Value";

            mCanvasView = new CanvasView(
                new PictureBoxCanvas(pictureBoxNode, GlobalDefine.TEXT_FONT),
                new ControlMouseHandler(pictureBoxNode),
                new MenuBuilder(mInputForm, () =>
                {
                    mCanvasView?.UpdateNodes();
                    mCanvasView?.ApplyCanvas();
                }),
                new NodeConfigInitializer());
            mCanvasView.DefaultSize = new ViewModel.DataStruct.Size(
                pictureBoxNode.Width, pictureBoxNode.Height - 20f); // 预留部分高度给滚动条
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
            ConfigHelper.InitConfig();

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
                comboBoxLayerType.Items.Add(new MenuItem("（无）", null));
                for (int i = 0; i < layerConfig.LayerTypes.Count; i++)
                {
                    string layerType = layerConfig.LayerTypes[i];
                    comboBoxLayerType.Items.Add(new MenuItem(layerType, layerType));
                }

                int index = 0;
                for (int i = 0; i < comboBoxLayerType.Items.Count; i++)
                {
                    MenuItem item = (MenuItem)comboBoxLayerType.Items[i];
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
            panelNodeView.AutoScrollPosition = new Point(0, 0);

            Layer layer = GetCurLayer();
            mCanvasView.InitCanvas(SaveData, layer);
            mCanvasView.UpdateCanvas();
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
                    DataAPI.AddLayer(SaveData, layerConfig.Name);
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
            if (e.KeyChar != (char)8) //8: Backspace
            {
                if (!int.TryParse(e.KeyChar.ToString(), out int count) || count < GlobalDefine.COLUMN_MIN_NODE || count > GlobalDefine.COLUMN_MAX_NODE)
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
                DataAPI.AddColume(layer);
                int.TryParse(textBoxNode.Text[i].ToString(), out int num);
                for (int j = 0; j < num; j++)
                {
                    DataAPI.AddNode(layer, i);
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

                DataAPI.AddConnection(layer, curColFirstNode, nextColFirstNode); //已去重
                DataAPI.AddConnection(layer, curColLastNode, nextColLastNode);

                if (curColNodes.Count == 1)
                {
                    for (int j = 0; j < nextColNodes.Count; j++)
                    {
                        Node nextColNode = nextColNodes[j];
                        DataAPI.AddConnection(layer, curColFirstNode, nextColNode);
                    }
                }

                if (nextColNodes.Count == 1)
                {
                    for (int j = 0; j < curColNodes.Count; j++)
                    {
                        Node curColNode = curColNodes[j];
                        DataAPI.AddConnection(layer, curColNode, nextColFirstNode);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (!IsEditMode || CurNodeView == null)
            //{
            //    return;
            //}

            //UIHelper.ClearConnectionPreview(pictureBoxNode);
            //UIHelper.DrawConnectionPreview(pictureBoxNode, CurNodeView);
        }

        private void pictureBoxNode_MouseDown(object sender, MouseEventArgs e)
        {
            mLastMousePos = pictureBoxNode.PointToScreen(e.Location);
            mIsDragging = true;
        }

        private void pictureBoxNode_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mIsDragging)
            {
                return;
            }

            Point curMousePos = pictureBoxNode.PointToScreen(e.Location);
            int deltaX = curMousePos.X - mLastMousePos.X;
            int deltaY = curMousePos.Y - mLastMousePos.Y;
            panelNodeView.AutoScrollPosition = new Point(-(panelNodeView.AutoScrollPosition.X + deltaX), -(panelNodeView.AutoScrollPosition.Y + deltaY));
            mLastMousePos = curMousePos;
            panelNodeView.Refresh();
        }

        private void pictureBoxNode_MouseUp(object sender, MouseEventArgs e)
        {
            mIsDragging = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N && btnNew.Enabled)
            {
                btnNew_Click(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.O && btnLoad.Enabled)
            {
                btnLoad_Click(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.S && btnSave.Enabled)
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

            MenuItem item = (MenuItem)comboBoxLayerType.Items[index];
            layer.Type = item.Value;
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            Layer layer = GetCurLayer();
            if (layer == null)
            {
                return;
            }

            mInputForm.Title = "层级备注";
            mInputForm.Content = layer.Comment;
            if (mInputForm.ShowDialog() == DialogResult.OK)
            {
                layer.Comment = mInputForm.Content;
                IsDirty = true;
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            mCanvasView.IsEditMode = !mCanvasView.IsEditMode;
            buttonEdit.Text = mCanvasView.IsEditMode ? "退出编辑" : "编辑连线";
        }
    }
}
