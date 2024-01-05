namespace ArknightsRoguelikeRec
{
    partial class Form1
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
            this.btnEditConnection = new System.Windows.Forms.Button();
            this.panelNodeView = new System.Windows.Forms.Panel();
            this.pictureBoxNode = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.labelNode = new System.Windows.Forms.Label();
            this.textBoxNode = new System.Windows.Forms.TextBox();
            this.panelAllLayer = new System.Windows.Forms.Panel();
            this.btnRemoveLayer = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelInfo.SuspendLayout();
            this.panelCurLayer.SuspendLayout();
            this.panelNodeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNode)).BeginInit();
            this.panelAllLayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitleLayer
            // 
            this.labelTitleLayer.AutoSize = true;
            this.labelTitleLayer.Font = new System.Drawing.Font("黑体", 14F);
            this.labelTitleLayer.Location = new System.Drawing.Point(3, 17);
            this.labelTitleLayer.Name = "labelTitleLayer";
            this.labelTitleLayer.Size = new System.Drawing.Size(49, 19);
            this.labelTitleLayer.TabIndex = 1;
            this.labelTitleLayer.Text = "层数";
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.Font = new System.Drawing.Font("黑体", 14F);
            this.btnAddLayer.Location = new System.Drawing.Point(305, 3);
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(56, 56);
            this.btnAddLayer.TabIndex = 3;
            this.btnAddLayer.Text = "+";
            this.btnAddLayer.UseVisualStyleBackColor = true;
            this.btnAddLayer.Click += new System.EventHandler(this.btnAddLayer_Click);
            // 
            // panelLayer
            // 
            this.panelLayer.AutoScroll = true;
            this.panelLayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLayer.Location = new System.Drawing.Point(3, 65);
            this.panelLayer.Name = "panelLayer";
            this.panelLayer.Size = new System.Drawing.Size(420, 846);
            this.panelLayer.TabIndex = 2;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.labelNumber);
            this.panelInfo.Controls.Add(this.textBoxNumber);
            this.panelInfo.Controls.Add(this.labelUserID);
            this.panelInfo.Controls.Add(this.textBoxUserID);
            this.panelInfo.Location = new System.Drawing.Point(444, 12);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(586, 169);
            this.panelInfo.TabIndex = 1;
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Font = new System.Drawing.Font("黑体", 10F);
            this.labelNumber.Location = new System.Drawing.Point(36, 62);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(63, 14);
            this.labelNumber.TabIndex = 2;
            this.labelNumber.Text = "记录编号";
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Font = new System.Drawing.Font("黑体", 10F);
            this.textBoxNumber.Location = new System.Drawing.Point(141, 59);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(354, 23);
            this.textBoxNumber.TabIndex = 3;
            this.textBoxNumber.TextChanged += new System.EventHandler(this.textBoxNumber_TextChanged);
            // 
            // labelUserID
            // 
            this.labelUserID.AutoSize = true;
            this.labelUserID.Font = new System.Drawing.Font("黑体", 10F);
            this.labelUserID.Location = new System.Drawing.Point(36, 22);
            this.labelUserID.Name = "labelUserID";
            this.labelUserID.Size = new System.Drawing.Size(63, 14);
            this.labelUserID.TabIndex = 0;
            this.labelUserID.Text = "探索者ID";
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Font = new System.Drawing.Font("黑体", 10F);
            this.textBoxUserID.Location = new System.Drawing.Point(141, 19);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(354, 23);
            this.textBoxUserID.TabIndex = 1;
            this.textBoxUserID.TextChanged += new System.EventHandler(this.textBoxUserID_TextChanged);
            // 
            // linkLabelOpenSaveFolder
            // 
            this.linkLabelOpenSaveFolder.AutoSize = true;
            this.linkLabelOpenSaveFolder.Font = new System.Drawing.Font("黑体", 9F);
            this.linkLabelOpenSaveFolder.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelOpenSaveFolder.Location = new System.Drawing.Point(1145, 60);
            this.linkLabelOpenSaveFolder.Name = "linkLabelOpenSaveFolder";
            this.linkLabelOpenSaveFolder.Size = new System.Drawing.Size(101, 12);
            this.linkLabelOpenSaveFolder.TabIndex = 11;
            this.linkLabelOpenSaveFolder.TabStop = true;
            this.linkLabelOpenSaveFolder.Text = "打开文件保存位置";
            this.linkLabelOpenSaveFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenSaveFolder_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("黑体", 9F);
            this.btnSave.Location = new System.Drawing.Point(1214, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("黑体", 9F);
            this.btnLoad.Location = new System.Drawing.Point(1125, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(83, 30);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "读取";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("黑体", 9F);
            this.btnNew.Location = new System.Drawing.Point(1036, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(83, 30);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "新建";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panelCurLayer
            // 
            this.panelCurLayer.Controls.Add(this.btnEditConnection);
            this.panelCurLayer.Controls.Add(this.panelNodeView);
            this.panelCurLayer.Controls.Add(this.btnCancel);
            this.panelCurLayer.Controls.Add(this.btnApply);
            this.panelCurLayer.Controls.Add(this.labelNode);
            this.panelCurLayer.Controls.Add(this.textBoxNode);
            this.panelCurLayer.Location = new System.Drawing.Point(444, 187);
            this.panelCurLayer.Name = "panelCurLayer";
            this.panelCurLayer.Size = new System.Drawing.Size(853, 739);
            this.panelCurLayer.TabIndex = 0;
            // 
            // btnEditConnection
            // 
            this.btnEditConnection.Font = new System.Drawing.Font("黑体", 9F);
            this.btnEditConnection.Location = new System.Drawing.Point(766, 19);
            this.btnEditConnection.Name = "btnEditConnection";
            this.btnEditConnection.Size = new System.Drawing.Size(83, 30);
            this.btnEditConnection.TabIndex = 10;
            this.btnEditConnection.Text = "编辑连接";
            this.btnEditConnection.UseVisualStyleBackColor = true;
            this.btnEditConnection.Click += new System.EventHandler(this.btnEditConnection_Click);
            // 
            // panelNodeView
            // 
            this.panelNodeView.AutoScroll = true;
            this.panelNodeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNodeView.Controls.Add(this.pictureBoxNode);
            this.panelNodeView.Location = new System.Drawing.Point(3, 55);
            this.panelNodeView.Name = "panelNodeView";
            this.panelNodeView.Size = new System.Drawing.Size(847, 681);
            this.panelNodeView.TabIndex = 9;
            this.panelNodeView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelNodeView_Scroll);
            // 
            // pictureBoxNode
            // 
            this.pictureBoxNode.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBoxNode.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxNode.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxNode.Name = "pictureBoxNode";
            this.pictureBoxNode.Size = new System.Drawing.Size(845, 679);
            this.pictureBoxNode.TabIndex = 8;
            this.pictureBoxNode.TabStop = false;
            this.pictureBoxNode.Click += new System.EventHandler(this.pictureBoxNode_Click);
            this.pictureBoxNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNode_MouseDown);
            this.pictureBoxNode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNode_MouseMove);
            this.pictureBoxNode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNode_MouseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("黑体", 9F);
            this.btnCancel.Location = new System.Drawing.Point(590, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("黑体", 9F);
            this.btnApply.Location = new System.Drawing.Point(501, 19);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(83, 30);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "确定";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // labelNode
            // 
            this.labelNode.AutoSize = true;
            this.labelNode.Font = new System.Drawing.Font("黑体", 10F);
            this.labelNode.Location = new System.Drawing.Point(36, 22);
            this.labelNode.Name = "labelNode";
            this.labelNode.Size = new System.Drawing.Size(63, 14);
            this.labelNode.TabIndex = 4;
            this.labelNode.Text = "节点分布";
            // 
            // textBoxNode
            // 
            this.textBoxNode.Font = new System.Drawing.Font("黑体", 10F);
            this.textBoxNode.Location = new System.Drawing.Point(141, 19);
            this.textBoxNode.MaxLength = 16;
            this.textBoxNode.Name = "textBoxNode";
            this.textBoxNode.Size = new System.Drawing.Size(354, 23);
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
            this.panelAllLayer.Size = new System.Drawing.Size(426, 914);
            this.panelAllLayer.TabIndex = 12;
            // 
            // btnRemoveLayer
            // 
            this.btnRemoveLayer.Font = new System.Drawing.Font("黑体", 14F);
            this.btnRemoveLayer.Location = new System.Drawing.Point(367, 3);
            this.btnRemoveLayer.Name = "btnRemoveLayer";
            this.btnRemoveLayer.Size = new System.Drawing.Size(56, 56);
            this.btnRemoveLayer.TabIndex = 4;
            this.btnRemoveLayer.Text = "-";
            this.btnRemoveLayer.UseVisualStyleBackColor = true;
            this.btnRemoveLayer.Click += new System.EventHandler(this.btnRemoveLayer_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1309, 938);
            this.Controls.Add(this.panelAllLayer);
            this.Controls.Add(this.panelCurLayer);
            this.Controls.Add(this.linkLabelOpenSaveFolder);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.btnLoad);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("黑体", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "明日方舟肉鸽节点记录工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelCurLayer.ResumeLayout(false);
            this.panelCurLayer.PerformLayout();
            this.panelNodeView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNode)).EndInit();
            this.panelAllLayer.ResumeLayout(false);
            this.panelAllLayer.PerformLayout();
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
        private System.Windows.Forms.PictureBox pictureBoxNode;
        private System.Windows.Forms.Button btnEditConnection;
        private System.Windows.Forms.Timer timer1;
    }
}

