namespace ArknightsRoguelikeRec
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelTitleLayer = new System.Windows.Forms.Label();
            this.btnAddLayer = new System.Windows.Forms.Button();
            this.panelLayer = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelNumber = new System.Windows.Forms.Label();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.labelUserID = new System.Windows.Forms.Label();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.linkLabelOpenSaveFolder = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panelCurLayer = new System.Windows.Forms.Panel();
            this.checkBoxComplete = new System.Windows.Forms.CheckBox();
            this.btnEditConnection = new System.Windows.Forms.Button();
            this.btnComment = new System.Windows.Forms.Button();
            this.labelNodeTips = new System.Windows.Forms.Label();
            this.comboBoxLayerType = new System.Windows.Forms.ComboBox();
            this.labelLayerType = new System.Windows.Forms.Label();
            this.panelNodeView = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.labelNode = new System.Windows.Forms.Label();
            this.textBoxNode = new System.Windows.Forms.TextBox();
            this.panelAllLayer = new System.Windows.Forms.Panel();
            this.btnRemoveLayer = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnEditRoute = new System.Windows.Forms.Button();
            this.panelInfo.SuspendLayout();
            this.panelCurLayer.SuspendLayout();
            this.panelAllLayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitleLayer
            // 
            this.labelTitleLayer.AutoSize = true;
            this.labelTitleLayer.Location = new System.Drawing.Point(15, 15);
            this.labelTitleLayer.Name = "labelTitleLayer";
            this.labelTitleLayer.Size = new System.Drawing.Size(29, 12);
            this.labelTitleLayer.TabIndex = 1;
            this.labelTitleLayer.Text = "层级";
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.Location = new System.Drawing.Point(141, 10);
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(75, 23);
            this.btnAddLayer.TabIndex = 3;
            this.btnAddLayer.Text = "+";
            this.btnAddLayer.UseVisualStyleBackColor = true;
            this.btnAddLayer.Click += new System.EventHandler(this.btnAddLayer_Click);
            // 
            // panelLayer
            // 
            this.panelLayer.AutoScroll = true;
            this.panelLayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLayer.Location = new System.Drawing.Point(3, 39);
            this.panelLayer.Name = "panelLayer";
            this.panelLayer.Size = new System.Drawing.Size(294, 659);
            this.panelLayer.TabIndex = 2;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.labelNumber);
            this.panelInfo.Controls.Add(this.textBoxNumber);
            this.panelInfo.Controls.Add(this.labelUserID);
            this.panelInfo.Controls.Add(this.textBoxUserID);
            this.panelInfo.Location = new System.Drawing.Point(318, 12);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(289, 70);
            this.panelInfo.TabIndex = 1;
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Location = new System.Drawing.Point(15, 40);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(53, 12);
            this.labelNumber.TabIndex = 2;
            this.labelNumber.Text = "记录编号";
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Location = new System.Drawing.Point(74, 37);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(200, 21);
            this.textBoxNumber.TabIndex = 3;
            this.textBoxNumber.TextChanged += new System.EventHandler(this.textBoxNumber_TextChanged);
            // 
            // labelUserID
            // 
            this.labelUserID.AutoSize = true;
            this.labelUserID.Location = new System.Drawing.Point(15, 15);
            this.labelUserID.Name = "labelUserID";
            this.labelUserID.Size = new System.Drawing.Size(41, 12);
            this.labelUserID.TabIndex = 0;
            this.labelUserID.Text = "玩家ID";
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Location = new System.Drawing.Point(74, 10);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(200, 21);
            this.textBoxUserID.TabIndex = 1;
            this.textBoxUserID.TextChanged += new System.EventHandler(this.textBoxUserID_TextChanged);
            // 
            // linkLabelOpenSaveFolder
            // 
            this.linkLabelOpenSaveFolder.AutoSize = true;
            this.linkLabelOpenSaveFolder.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelOpenSaveFolder.Location = new System.Drawing.Point(719, 58);
            this.linkLabelOpenSaveFolder.Name = "linkLabelOpenSaveFolder";
            this.linkLabelOpenSaveFolder.Size = new System.Drawing.Size(101, 12);
            this.linkLabelOpenSaveFolder.TabIndex = 11;
            this.linkLabelOpenSaveFolder.TabStop = true;
            this.linkLabelOpenSaveFolder.Text = "打开文件保存位置";
            this.linkLabelOpenSaveFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenSaveFolder_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(613, 51);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存(Ctrl+S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(719, 22);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 23);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "读取(Ctrl+O)";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(613, 22);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 23);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "新建(Ctrl+N)";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panelCurLayer
            // 
            this.panelCurLayer.Controls.Add(this.btnEditRoute);
            this.panelCurLayer.Controls.Add(this.checkBoxComplete);
            this.panelCurLayer.Controls.Add(this.btnEditConnection);
            this.panelCurLayer.Controls.Add(this.btnComment);
            this.panelCurLayer.Controls.Add(this.labelNodeTips);
            this.panelCurLayer.Controls.Add(this.comboBoxLayerType);
            this.panelCurLayer.Controls.Add(this.labelLayerType);
            this.panelCurLayer.Controls.Add(this.panelNodeView);
            this.panelCurLayer.Controls.Add(this.btnCancel);
            this.panelCurLayer.Controls.Add(this.btnApply);
            this.panelCurLayer.Controls.Add(this.labelNode);
            this.panelCurLayer.Controls.Add(this.textBoxNode);
            this.panelCurLayer.Location = new System.Drawing.Point(318, 88);
            this.panelCurLayer.Name = "panelCurLayer";
            this.panelCurLayer.Size = new System.Drawing.Size(674, 625);
            this.panelCurLayer.TabIndex = 0;
            // 
            // checkBoxComplete
            // 
            this.checkBoxComplete.AutoSize = true;
            this.checkBoxComplete.Location = new System.Drawing.Point(505, 40);
            this.checkBoxComplete.Name = "checkBoxComplete";
            this.checkBoxComplete.Size = new System.Drawing.Size(60, 16);
            this.checkBoxComplete.TabIndex = 16;
            this.checkBoxComplete.Text = "已完成";
            this.checkBoxComplete.UseVisualStyleBackColor = true;
            this.checkBoxComplete.CheckedChanged += new System.EventHandler(this.checkBoxComplete_CheckedChanged);
            // 
            // btnEditConnection
            // 
            this.btnEditConnection.Location = new System.Drawing.Point(571, 7);
            this.btnEditConnection.Name = "btnEditConnection";
            this.btnEditConnection.Size = new System.Drawing.Size(100, 23);
            this.btnEditConnection.TabIndex = 15;
            this.btnEditConnection.Text = "编辑连线(E)";
            this.btnEditConnection.UseVisualStyleBackColor = true;
            this.btnEditConnection.Click += new System.EventHandler(this.btnEditConnection_Click);
            // 
            // btnComment
            // 
            this.btnComment.Location = new System.Drawing.Point(571, 36);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(100, 23);
            this.btnComment.TabIndex = 14;
            this.btnComment.Text = "层级备注(G)";
            this.btnComment.UseVisualStyleBackColor = true;
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // labelNodeTips
            // 
            this.labelNodeTips.AutoSize = true;
            this.labelNodeTips.ForeColor = System.Drawing.Color.Red;
            this.labelNodeTips.Location = new System.Drawing.Point(280, 41);
            this.labelNodeTips.Name = "labelNodeTips";
            this.labelNodeTips.Size = new System.Drawing.Size(149, 12);
            this.labelNodeTips.TabIndex = 13;
            this.labelNodeTips.Text = "请先输入当前层的节点分布";
            // 
            // comboBoxLayerType
            // 
            this.comboBoxLayerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLayerType.DropDownWidth = 500;
            this.comboBoxLayerType.FormattingEnabled = true;
            this.comboBoxLayerType.Location = new System.Drawing.Point(74, 12);
            this.comboBoxLayerType.Name = "comboBoxLayerType";
            this.comboBoxLayerType.Size = new System.Drawing.Size(200, 20);
            this.comboBoxLayerType.TabIndex = 12;
            this.comboBoxLayerType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayerType_SelectedIndexChanged);
            // 
            // labelLayerType
            // 
            this.labelLayerType.AutoSize = true;
            this.labelLayerType.Location = new System.Drawing.Point(15, 15);
            this.labelLayerType.Name = "labelLayerType";
            this.labelLayerType.Size = new System.Drawing.Size(53, 12);
            this.labelLayerType.TabIndex = 11;
            this.labelLayerType.Text = "层级类型";
            // 
            // panelNodeView
            // 
            this.panelNodeView.AutoScroll = true;
            this.panelNodeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNodeView.Location = new System.Drawing.Point(3, 65);
            this.panelNodeView.Name = "panelNodeView";
            this.panelNodeView.Size = new System.Drawing.Size(668, 557);
            this.panelNodeView.TabIndex = 9;
            this.panelNodeView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelNodeView_Scroll);
            this.panelNodeView.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panelNodeView_MouseWheel);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(361, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(280, 36);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "确定";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // labelNode
            // 
            this.labelNode.AutoSize = true;
            this.labelNode.Location = new System.Drawing.Point(15, 41);
            this.labelNode.Name = "labelNode";
            this.labelNode.Size = new System.Drawing.Size(53, 12);
            this.labelNode.TabIndex = 4;
            this.labelNode.Text = "节点分布";
            // 
            // textBoxNode
            // 
            this.textBoxNode.Location = new System.Drawing.Point(74, 38);
            this.textBoxNode.MaxLength = 16;
            this.textBoxNode.Name = "textBoxNode";
            this.textBoxNode.Size = new System.Drawing.Size(200, 21);
            this.textBoxNode.TabIndex = 5;
            this.textBoxNode.TextChanged += new System.EventHandler(this.textBoxNode_TextChanged);
            this.textBoxNode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNode_KeyPress);
            // 
            // panelAllLayer
            // 
            this.panelAllLayer.Controls.Add(this.btnRemoveLayer);
            this.panelAllLayer.Controls.Add(this.labelTitleLayer);
            this.panelAllLayer.Controls.Add(this.panelLayer);
            this.panelAllLayer.Controls.Add(this.btnAddLayer);
            this.panelAllLayer.Location = new System.Drawing.Point(12, 12);
            this.panelAllLayer.Name = "panelAllLayer";
            this.panelAllLayer.Size = new System.Drawing.Size(300, 701);
            this.panelAllLayer.TabIndex = 12;
            // 
            // btnRemoveLayer
            // 
            this.btnRemoveLayer.Location = new System.Drawing.Point(222, 10);
            this.btnRemoveLayer.Name = "btnRemoveLayer";
            this.btnRemoveLayer.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveLayer.TabIndex = 4;
            this.btnRemoveLayer.Text = "-";
            this.btnRemoveLayer.UseVisualStyleBackColor = true;
            this.btnRemoveLayer.Click += new System.EventHandler(this.btnRemoveLayer_Click);
            // 
            // btnEditRoute
            // 
            this.btnEditRoute.Location = new System.Drawing.Point(465, 7);
            this.btnEditRoute.Name = "btnEditRoute";
            this.btnEditRoute.Size = new System.Drawing.Size(100, 23);
            this.btnEditRoute.TabIndex = 17;
            this.btnEditRoute.Text = "编辑路线(R)";
            this.btnEditRoute.UseVisualStyleBackColor = true;
            this.btnEditRoute.Click += new System.EventHandler(this.btnEditRoute_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1004, 725);
            this.Controls.Add(this.panelAllLayer);
            this.Controls.Add(this.panelCurLayer);
            this.Controls.Add(this.linkLabelOpenSaveFolder);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.btnLoad);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "明日方舟肉鸽节点记录工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelCurLayer.ResumeLayout(false);
            this.panelCurLayer.PerformLayout();
            this.panelAllLayer.ResumeLayout(false);
            this.panelAllLayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelTitleLayer;
        private System.Windows.Forms.Panel panelLayer;
        private System.Windows.Forms.Button btnAddLayer;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelCurLayer;
        private System.Windows.Forms.Label labelUserID;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Label labelNode;
        private System.Windows.Forms.TextBox textBoxNode;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.LinkLabel linkLabelOpenSaveFolder;
        private System.Windows.Forms.Panel panelAllLayer;
        private System.Windows.Forms.Button btnRemoveLayer;
        private System.Windows.Forms.Panel panelNodeView;
        private System.Windows.Forms.ComboBox comboBoxLayerType;
        private System.Windows.Forms.Label labelLayerType;
        private System.Windows.Forms.Label labelNodeTips;
        private System.Windows.Forms.Button btnComment;
        private System.Windows.Forms.Button btnEditConnection;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.CheckBox checkBoxComplete;
        private System.Windows.Forms.Button btnEditRoute;
    }
}

