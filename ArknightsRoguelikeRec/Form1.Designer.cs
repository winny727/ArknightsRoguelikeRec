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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openSaveFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelTitleLayer = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelLayer = new System.Windows.Forms.Panel();
            this.btnAddLayer = new System.Windows.Forms.Button();
            this.contextMenuStripLayer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelCurLayer = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panelLayerView = new System.Windows.Forms.Panel();
            this.labelUserID = new System.Windows.Forms.Label();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.labelNumber = new System.Windows.Forms.Label();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.labelNode = new System.Windows.Forms.Label();
            this.textBoxNode = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.linkLabelOpenSaveFolder = new System.Windows.Forms.LinkLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelCurLayer.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(10, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1309, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.openSaveFolderToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(76, 28);
            this.menuToolStripMenuItem.Text = "&Menu";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(267, 6);
            // 
            // openSaveFolderToolStripMenuItem
            // 
            this.openSaveFolderToolStripMenuItem.Name = "openSaveFolderToolStripMenuItem";
            this.openSaveFolderToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.openSaveFolderToolStripMenuItem.Text = "Open Save Folder";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(267, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.exitToolStripMenuItem.Text = "&Exit";
            // 
            // labelTitleLayer
            // 
            this.labelTitleLayer.AutoSize = true;
            this.labelTitleLayer.Font = new System.Drawing.Font("黑体", 14F);
            this.labelTitleLayer.Location = new System.Drawing.Point(17, 22);
            this.labelTitleLayer.Name = "labelTitleLayer";
            this.labelTitleLayer.Size = new System.Drawing.Size(68, 28);
            this.labelTitleLayer.TabIndex = 1;
            this.labelTitleLayer.Text = "层数";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(13, 39);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnAddLayer);
            this.splitContainer1.Panel1.Controls.Add(this.panelLayer);
            this.splitContainer1.Panel1.Controls.Add(this.labelTitleLayer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelInfo);
            this.splitContainer1.Panel2.Controls.Add(this.panelCurLayer);
            this.splitContainer1.Size = new System.Drawing.Size(1282, 886);
            this.splitContainer1.SplitterDistance = 427;
            this.splitContainer1.TabIndex = 2;
            // 
            // panelLayer
            // 
            this.panelLayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLayer.Location = new System.Drawing.Point(3, 68);
            this.panelLayer.Name = "panelLayer";
            this.panelLayer.Size = new System.Drawing.Size(420, 814);
            this.panelLayer.TabIndex = 2;
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.Font = new System.Drawing.Font("黑体", 14F);
            this.btnAddLayer.Location = new System.Drawing.Point(364, 7);
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(56, 56);
            this.btnAddLayer.TabIndex = 3;
            this.btnAddLayer.Text = "+";
            this.btnAddLayer.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripLayer
            // 
            this.contextMenuStripLayer.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripLayer.Name = "contextMenuStripLayer";
            this.contextMenuStripLayer.Size = new System.Drawing.Size(61, 4);
            // 
            // panelCurLayer
            // 
            this.panelCurLayer.Controls.Add(this.btnCancel);
            this.panelCurLayer.Controls.Add(this.btnApply);
            this.panelCurLayer.Controls.Add(this.panelLayerView);
            this.panelCurLayer.Controls.Add(this.labelNode);
            this.panelCurLayer.Controls.Add(this.textBoxNode);
            this.panelCurLayer.Location = new System.Drawing.Point(3, 213);
            this.panelCurLayer.Name = "panelCurLayer";
            this.panelCurLayer.Size = new System.Drawing.Size(844, 667);
            this.panelCurLayer.TabIndex = 0;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.linkLabelOpenSaveFolder);
            this.panelInfo.Controls.Add(this.btnSave);
            this.panelInfo.Controls.Add(this.btnLoad);
            this.panelInfo.Controls.Add(this.btnNew);
            this.panelInfo.Controls.Add(this.labelNumber);
            this.panelInfo.Controls.Add(this.textBoxNumber);
            this.panelInfo.Controls.Add(this.labelUserID);
            this.panelInfo.Controls.Add(this.textBoxUserID);
            this.panelInfo.Location = new System.Drawing.Point(3, 3);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(844, 203);
            this.panelInfo.TabIndex = 1;
            // 
            // panelLayerView
            // 
            this.panelLayerView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLayerView.Location = new System.Drawing.Point(3, 59);
            this.panelLayerView.Name = "panelLayerView";
            this.panelLayerView.Size = new System.Drawing.Size(838, 604);
            this.panelLayerView.TabIndex = 0;
            // 
            // labelUserID
            // 
            this.labelUserID.AutoSize = true;
            this.labelUserID.Font = new System.Drawing.Font("黑体", 10F);
            this.labelUserID.Location = new System.Drawing.Point(36, 22);
            this.labelUserID.Name = "labelUserID";
            this.labelUserID.Size = new System.Drawing.Size(89, 20);
            this.labelUserID.TabIndex = 0;
            this.labelUserID.Text = "探索者ID";
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Font = new System.Drawing.Font("黑体", 10F);
            this.textBoxUserID.Location = new System.Drawing.Point(141, 19);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(354, 30);
            this.textBoxUserID.TabIndex = 1;
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Font = new System.Drawing.Font("黑体", 10F);
            this.labelNumber.Location = new System.Drawing.Point(36, 62);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(89, 20);
            this.labelNumber.TabIndex = 2;
            this.labelNumber.Text = "记录编号";
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Font = new System.Drawing.Font("黑体", 10F);
            this.textBoxNumber.Location = new System.Drawing.Point(141, 59);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(354, 30);
            this.textBoxNumber.TabIndex = 3;
            // 
            // labelNode
            // 
            this.labelNode.AutoSize = true;
            this.labelNode.Font = new System.Drawing.Font("黑体", 10F);
            this.labelNode.Location = new System.Drawing.Point(36, 22);
            this.labelNode.Name = "labelNode";
            this.labelNode.Size = new System.Drawing.Size(89, 20);
            this.labelNode.TabIndex = 4;
            this.labelNode.Text = "节点分布";
            // 
            // textBoxNode
            // 
            this.textBoxNode.Font = new System.Drawing.Font("黑体", 10F);
            this.textBoxNode.Location = new System.Drawing.Point(141, 19);
            this.textBoxNode.Name = "textBoxNode";
            this.textBoxNode.Size = new System.Drawing.Size(354, 30);
            this.textBoxNode.TabIndex = 5;
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
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("黑体", 9F);
            this.btnNew.Location = new System.Drawing.Point(569, 59);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(83, 30);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "新建";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("黑体", 9F);
            this.btnLoad.Location = new System.Drawing.Point(658, 59);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(83, 30);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "读取";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("黑体", 9F);
            this.btnSave.Location = new System.Drawing.Point(747, 59);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // linkLabelOpenSaveFolder
            // 
            this.linkLabelOpenSaveFolder.AutoSize = true;
            this.linkLabelOpenSaveFolder.Font = new System.Drawing.Font("黑体", 9F);
            this.linkLabelOpenSaveFolder.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelOpenSaveFolder.Location = new System.Drawing.Point(678, 24);
            this.linkLabelOpenSaveFolder.Name = "linkLabelOpenSaveFolder";
            this.linkLabelOpenSaveFolder.Size = new System.Drawing.Size(152, 18);
            this.linkLabelOpenSaveFolder.TabIndex = 11;
            this.linkLabelOpenSaveFolder.TabStop = true;
            this.linkLabelOpenSaveFolder.Text = "打开文件保存位置";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 938);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("黑体", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "明日方舟肉鸽节点记录工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelCurLayer.ResumeLayout(false);
            this.panelCurLayer.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openSaveFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label labelTitleLayer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelLayer;
        private System.Windows.Forms.Button btnAddLayer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLayer;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelCurLayer;
        private System.Windows.Forms.Panel panelLayerView;
        private System.Windows.Forms.Label labelUserID;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Label labelNode;
        private System.Windows.Forms.TextBox textBoxNode;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.LinkLabel linkLabelOpenSaveFolder;
    }
}

