namespace ControlPanelClsLib
{
    partial class FrmExecuteRecipe
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExecuteRecipe));
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.labelProcessStatus = new System.Windows.Forms.Label();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelMainStatus = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.teRecipeNmae = new DevExpress.XtraEditors.TextEdit();
            this.btnSelectRecipe = new DevExpress.XtraEditors.SimpleButton();
            this.btnAbort = new DevExpress.XtraEditors.SimpleButton();
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.progressBarCtrl = new DevExpress.XtraEditors.ProgressBarControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teRecipeNmae.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarCtrl.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.labelProcessStatus, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panelControl4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.panelControl2, 0, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1596, 728);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // labelProcessStatus
            // 
            this.labelProcessStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel3.SetColumnSpan(this.labelProcessStatus, 3);
            this.labelProcessStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessStatus.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labelProcessStatus.Location = new System.Drawing.Point(1, 41);
            this.labelProcessStatus.Margin = new System.Windows.Forms.Padding(1, 3, 1, 0);
            this.labelProcessStatus.Name = "labelProcessStatus";
            this.labelProcessStatus.Size = new System.Drawing.Size(1594, 47);
            this.labelProcessStatus.TabIndex = 21;
            this.labelProcessStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControl4
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.panelControl4, 3);
            this.panelControl4.Controls.Add(this.labelMainStatus);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(3, 3);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1590, 32);
            this.panelControl4.TabIndex = 5;
            // 
            // labelMainStatus
            // 
            this.labelMainStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMainStatus.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelMainStatus.Appearance.Options.UseFont = true;
            this.labelMainStatus.Appearance.Options.UseForeColor = true;
            this.labelMainStatus.Location = new System.Drawing.Point(5, 5);
            this.labelMainStatus.Name = "labelMainStatus";
            this.labelMainStatus.Size = new System.Drawing.Size(74, 19);
            this.labelMainStatus.TabIndex = 4;
            this.labelMainStatus.Text = "当前配方:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelControl26);
            this.groupBox1.Controls.Add(this.teRecipeNmae);
            this.groupBox1.Controls.Add(this.btnSelectRecipe);
            this.groupBox1.Controls.Add(this.btnAbort);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Location = new System.Drawing.Point(3, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 257);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生产";
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(35, 40);
            this.labelControl26.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(28, 14);
            this.labelControl26.TabIndex = 0;
            this.labelControl26.Text = "配方:";
            // 
            // teRecipeNmae
            // 
            this.teRecipeNmae.Location = new System.Drawing.Point(114, 33);
            this.teRecipeNmae.Name = "teRecipeNmae";
            this.teRecipeNmae.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teRecipeNmae.Properties.Appearance.Options.UseFont = true;
            this.teRecipeNmae.Properties.ReadOnly = true;
            this.teRecipeNmae.Size = new System.Drawing.Size(226, 30);
            this.teRecipeNmae.TabIndex = 9;
            // 
            // btnSelectRecipe
            // 
            this.btnSelectRecipe.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnSelectRecipe.Appearance.Options.UseFont = true;
            this.btnSelectRecipe.Location = new System.Drawing.Point(367, 35);
            this.btnSelectRecipe.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnSelectRecipe.Name = "btnSelectRecipe";
            this.btnSelectRecipe.Size = new System.Drawing.Size(43, 27);
            this.btnSelectRecipe.TabIndex = 10;
            this.btnSelectRecipe.Text = "....";
            // 
            // btnAbort
            // 
            this.btnAbort.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAbort.ImageOptions.Image")));
            this.btnAbort.Location = new System.Drawing.Point(326, 150);
            this.btnAbort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(84, 39);
            this.btnAbort.TabIndex = 12;
            this.btnAbort.Text = "终止";
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnStart
            // 
            this.btnStart.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.ImageOptions.Image")));
            this.btnStart.Location = new System.Drawing.Point(205, 150);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(99, 39);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "开始";
            // 
            // panelControl2
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.panelControl2, 3);
            this.panelControl2.Controls.Add(this.progressBarCtrl);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 694);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1590, 31);
            this.panelControl2.TabIndex = 7;
            // 
            // progressBarCtrl
            // 
            this.progressBarCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarCtrl.Location = new System.Drawing.Point(2, 2);
            this.progressBarCtrl.Name = "progressBarCtrl";
            this.progressBarCtrl.Size = new System.Drawing.Size(1586, 27);
            this.progressBarCtrl.TabIndex = 5;
            this.progressBarCtrl.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl6, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1606, 738);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.tableLayoutPanel3);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl6.Location = new System.Drawing.Point(3, 3);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(1600, 732);
            this.panelControl6.TabIndex = 14;
            // 
            // FrmExecuteRecipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1606, 738);
            this.Controls.Add(this.tableLayoutPanel1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmExecuteRecipe.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmExecuteRecipe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配方执行";
            this.Load += new System.EventHandler(this.JobControlPanel_Load);
            this.Enter += new System.EventHandler(this.JobControlPanel_Enter);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teRecipeNmae.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarCtrl.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labelMainStatus;
        private DevExpress.XtraEditors.ProgressBarControl progressBarCtrl;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.SimpleButton btnAbort;
        private DevExpress.XtraEditors.SimpleButton btnSelectRecipe;
        private DevExpress.XtraEditors.SimpleButton btnStart;
        private DevExpress.XtraEditors.TextEdit teRecipeNmae;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelProcessStatus;
    }
}
