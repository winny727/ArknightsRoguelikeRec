using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Helper;
using ArknightsRoguelikeRec.ViewModel;
using ArknightsRoguelikeRec.ViewModel.Impl;
#if SKIA_SHARP
using SkiaSharp.Views.Desktop;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ArknightsRoguelikeRec
{
    public partial class MainForm : Form
    {
        public string SavePath = Environment.CurrentDirectory + "\\SaveData";

        public SaveData SaveData { get; private set; }
        public int SelectedLayer { get; private set; }
        public bool IsDirty { get; private set; }

        public NodeView CurNodeView { get; private set; }

        private readonly InputForm mInputForm = new InputForm();

        private bool mIsDragging = false;
        private Point mLastMousePos;

        private readonly CanvasView mCanvasView;
#if SKIA_SHARP
        private readonly SKGLControl mNodeViewControl;
#else
        private readonly PictureBox mNodeViewControl;
#endif

        public MainForm()
        {
            InitializeComponent();

            textBoxNode.MaxLength = GlobalDefine.MAX_COLUMU;
            comboBoxLayerType.DisplayMember = "Key";
            comboBoxLayerType.ValueMember = "Value";

#if SKIA_SHARP
            mNodeViewControl = new SKGLControl
            {
                Width = panelNodeView.Width - 2,
                Height = panelNodeView.Height - 2,
            };

            mCanvasView = new CanvasView(
                new SKGLCanvas(mNodeViewControl, FontLoader.SK_TEXT_FONT),
                new ControlMouseHandler(mNodeViewControl),
                new MenuBuilder(mInputForm, () =>
                {
                    mCanvasView?.UpdateNodes();
                    mCanvasView?.ApplyCanvas();
                }),
                new NodeConfigInitializer());
#else
            mNodeViewControl = new PictureBox
            {
                Width = panelNodeView.Width - 2,
                Height = panelNodeView.Height - 2,
            };

            mCanvasView = new CanvasView(
                new PictureBoxCanvas(mNodeViewControl, GlobalDefine.TEXT_FONT),
                new ControlMouseHandler(mNodeViewControl),
                new MenuBuilder(mInputForm, () =>
                {
                    mCanvasView?.UpdateNodes();
                    mCanvasView?.ApplyCanvas();
                }),
                new NodeConfigInitializer());
#endif

            panelNodeView.Controls.Add(mNodeViewControl);

            mCanvasView.DefaultSize = new ViewModel.DataStruct.Size(
                mNodeViewControl.Width, mNodeViewControl.Height - 20f); // 预留部分高度给滚动条

            mNodeViewControl.MouseDown += NodeViewControl_MouseDown;
            mNodeViewControl.MouseUp += NodeViewControl_MouseUp;
            mNodeViewControl.MouseMove += NodeViewControl_MouseMove;
#if SKIA_SHARP
            mNodeViewControl.PaintSurface += (s, e) =>
            {
                panelNodeView.Refresh();
            };
#endif

#if SKIA_SHARP
            var timer = new System.Timers.Timer(8);
            timer.Elapsed += (s, e) =>
            {
                if (mNodeViewControl.InvokeRequired)
                {
                    mNodeViewControl.BeginInvoke((Action)(() => mCanvasView.Tick()));
                }
                else
                {
                    mNodeViewControl.Invalidate();
                }
            };
            timer.Start();
#else
            Timer timer = new Timer();
            timer.Interval = 8;
            timer.Tick += (s, e) => mCanvasView.Tick();
            timer.Start();
#endif
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
        4. 由密文板进行的节点转换记录（如将某节点通过板-子变成树洞）；
         */

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigHelper.InitConfig();

            ////测试用
            //string layerConfig = Newtonsoft.Json.JsonConvert.SerializeObject(GlobalDefine.LayerConfigDict.AsList(), Newtonsoft.Json.Formatting.Indented);
            //string nodeConfig = Newtonsoft.Json.JsonConvert.SerializeObject(GlobalDefine.NodeConfigDict.AsList(), Newtonsoft.Json.Formatting.Indented);

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
            mNodeViewControl.Enabled = isEnabled;
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

            int gap = GlobalDefine.LAYER_BTN_GAP;
            int height = GlobalDefine.LAYER_BTN_HEIGHT;

            for (int i = 0; i < SaveData.Layers.Count; i++)
            {
                int index = i;
                Button btnLayer = new Button();
                panelLayer.Controls.Add(btnLayer);
                btnLayer.Text = SaveData.Layers[i].CustomName;
                //btnLayer.Font = GlobalDefine.TEXT_FONT;

                btnLayer.Size = new Size(panelLayer.Width - 2 * gap, height);
                btnLayer.Location = new Point(gap, gap + (panelLayer.Controls.Count - 1) * height);

                btnLayer.Click += (sender, e) =>
                {
                    SelectedLayer = index;
                    UpdateLayerSelectedHighlight();
                    UpdateCurLayerView();
                };
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
            var layerList = GlobalDefine.LayerConfigDict.AsList();
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
                if (!char.IsDigit(e.KeyChar))
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.Handled = true;
                    return;
                }

                int count = e.KeyChar - '0';
                if (count < GlobalDefine.COLUMN_MIN_NODE ||
                    count > GlobalDefine.COLUMN_MAX_NODE)
                {
                    System.Media.SystemSounds.Exclamation.Play();
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

            List<List<Node>> tempNodes = null;
            if (layer.Nodes.Count > 0 || layer.Connections.Count > 0)
            {
                var result = MessageBox.Show("更改节点分布后是否清空当前层已选择的节点类型？", "更改节点分布", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.No)
                {
                    tempNodes = new List<List<Node>>(layer.Nodes);
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

            if (tempNodes != null)
            {
                for (int colIndex = 0; colIndex < layer.Nodes.Count; colIndex++)
                {
                    List<Node> colNodes = layer.Nodes[colIndex];
                    List<Node> tempColNodes = tempNodes[colIndex];
                    for (int rowIndex = 0; rowIndex < colNodes.Count && rowIndex < tempColNodes.Count; rowIndex++)
                    {
                        colNodes[rowIndex] = tempColNodes[rowIndex];
                    }
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

        private void NodeViewControl_MouseDown(object sender, MouseEventArgs e)
        {
            mLastMousePos = mNodeViewControl.PointToScreen(e.Location);
            mIsDragging = true;
        }

        private void NodeViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mIsDragging)
            {
                return;
            }

            Point curMousePos = mNodeViewControl.PointToScreen(e.Location);
            int deltaX = curMousePos.X - mLastMousePos.X;
            int deltaY = curMousePos.Y - mLastMousePos.Y;
            panelNodeView.AutoScrollPosition = new Point(-(panelNodeView.AutoScrollPosition.X + deltaX), -(panelNodeView.AutoScrollPosition.Y + deltaY));
            mLastMousePos = curMousePos;
            panelNodeView.Refresh();
        }

        private void NodeViewControl_MouseUp(object sender, MouseEventArgs e)
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

            if (e.KeyCode == Keys.E && btnEdit.Enabled)
            {
                btnEdit_Click(sender, e);
            }

            if (e.KeyCode == Keys.G && btnComment.Enabled)
            {
                btnComment_Click(sender, e);
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            mCanvasView.IsEditMode = !mCanvasView.IsEditMode;
            btnEdit.Text = mCanvasView.IsEditMode ? "退出编辑(E)" : "编辑连线(E)";
            btnEdit.BackColor = mCanvasView.IsEditMode ? Color.LightGreen : SystemColors.Control;
        }
    }
}
