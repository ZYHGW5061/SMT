
namespace MainGUI.UserControls.Product
{
    partial class ProductRun
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAutoStart = new DevExpress.XtraEditors.SimpleButton();
            this.btnManualStart = new DevExpress.XtraEditors.SimpleButton();
            this.btnManualNext = new DevExpress.XtraEditors.SimpleButton();
            this.btnAutoPause = new DevExpress.XtraEditors.SimpleButton();
            this.btnAutoContinue = new DevExpress.XtraEditors.SimpleButton();
            this.btnStop = new DevExpress.XtraEditors.SimpleButton();
            this.separatorControl3 = new DevExpress.XtraEditors.SeparatorControl();
            this.btnRedoStep = new DevExpress.XtraEditors.SimpleButton();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAutoStart
            // 
            this.btnAutoStart.AllowFocus = false;
            this.btnAutoStart.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.next1;
            this.btnAutoStart.ImageOptions.SvgImageSize = new System.Drawing.Size(30, 30);
            this.btnAutoStart.Location = new System.Drawing.Point(13, 14);
            this.btnAutoStart.Name = "btnAutoStart";
            this.btnAutoStart.Size = new System.Drawing.Size(102, 36);
            this.btnAutoStart.TabIndex = 22;
            this.btnAutoStart.Text = "自动生产";
            this.btnAutoStart.Click += new System.EventHandler(this.btnAutoStart_Click);
            // 
            // btnManualStart
            // 
            this.btnManualStart.AllowFocus = false;
            this.btnManualStart.Enabled = false;
            this.btnManualStart.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.doublenext;
            this.btnManualStart.Location = new System.Drawing.Point(13, 159);
            this.btnManualStart.Name = "btnManualStart";
            this.btnManualStart.Size = new System.Drawing.Size(102, 36);
            this.btnManualStart.TabIndex = 23;
            this.btnManualStart.Text = "手动开始";
            this.btnManualStart.Visible = false;
            this.btnManualStart.Click += new System.EventHandler(this.btnManualStart_Click);
            // 
            // btnManualNext
            // 
            this.btnManualNext.AllowFocus = false;
            this.btnManualNext.Enabled = false;
            this.btnManualNext.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.actions_arrow1right;
            this.btnManualNext.Location = new System.Drawing.Point(13, 198);
            this.btnManualNext.Name = "btnManualNext";
            this.btnManualNext.Size = new System.Drawing.Size(102, 36);
            this.btnManualNext.TabIndex = 24;
            this.btnManualNext.Text = "下一步";
            this.btnManualNext.Visible = false;
            this.btnManualNext.Click += new System.EventHandler(this.btnManualNext_Click);
            // 
            // btnAutoPause
            // 
            this.btnAutoPause.AllowFocus = false;
            this.btnAutoPause.Enabled = false;
            this.btnAutoPause.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.pause;
            this.btnAutoPause.Location = new System.Drawing.Point(13, 53);
            this.btnAutoPause.Name = "btnAutoPause";
            this.btnAutoPause.Size = new System.Drawing.Size(102, 36);
            this.btnAutoPause.TabIndex = 25;
            this.btnAutoPause.Text = "暂停";
            this.btnAutoPause.Click += new System.EventHandler(this.btnAutoPause_Click);
            // 
            // btnAutoContinue
            // 
            this.btnAutoContinue.AllowFocus = false;
            this.btnAutoContinue.Enabled = false;
            this.btnAutoContinue.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.last;
            this.btnAutoContinue.Location = new System.Drawing.Point(13, 92);
            this.btnAutoContinue.Name = "btnAutoContinue";
            this.btnAutoContinue.Size = new System.Drawing.Size(102, 36);
            this.btnAutoContinue.TabIndex = 26;
            this.btnAutoContinue.Text = "继续";
            this.btnAutoContinue.Click += new System.EventHandler(this.btnAutoContinue_Click);
            // 
            // btnStop
            // 
            this.btnStop.AllowFocus = false;
            this.btnStop.Enabled = false;
            this.btnStop.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point(13, 321);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(102, 36);
            this.btnStop.TabIndex = 27;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // separatorControl3
            // 
            this.separatorControl3.LineColor = System.Drawing.Color.Silver;
            this.separatorControl3.Location = new System.Drawing.Point(13, 130);
            this.separatorControl3.Name = "separatorControl3";
            this.separatorControl3.Size = new System.Drawing.Size(102, 23);
            this.separatorControl3.TabIndex = 42;
            // 
            // btnRedoStep
            // 
            this.btnRedoStep.AllowFocus = false;
            this.btnRedoStep.Enabled = false;
            this.btnRedoStep.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.convertto;
            this.btnRedoStep.Location = new System.Drawing.Point(13, 237);
            this.btnRedoStep.Name = "btnRedoStep";
            this.btnRedoStep.Size = new System.Drawing.Size(102, 36);
            this.btnRedoStep.TabIndex = 43;
            this.btnRedoStep.Text = "重做步";
            this.btnRedoStep.Visible = false;
            this.btnRedoStep.Click += new System.EventHandler(this.btnRedoStep_Click);
            // 
            // separatorControl1
            // 
            this.separatorControl1.LineColor = System.Drawing.Color.Silver;
            this.separatorControl1.Location = new System.Drawing.Point(13, 279);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Size = new System.Drawing.Size(102, 23);
            this.separatorControl1.TabIndex = 44;
            // 
            // ProductRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAutoStart);
            this.Controls.Add(this.separatorControl1);
            this.Controls.Add(this.btnAutoPause);
            this.Controls.Add(this.btnManualStart);
            this.Controls.Add(this.btnRedoStep);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnAutoContinue);
            this.Controls.Add(this.separatorControl3);
            this.Controls.Add(this.btnManualNext);
            this.Name = "ProductRun";
            this.Size = new System.Drawing.Size(127, 373);
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnAutoStart;
        private DevExpress.XtraEditors.SimpleButton btnManualStart;
        private DevExpress.XtraEditors.SimpleButton btnManualNext;
        private DevExpress.XtraEditors.SimpleButton btnAutoPause;
        private DevExpress.XtraEditors.SimpleButton btnAutoContinue;
        private DevExpress.XtraEditors.SimpleButton btnStop;
        private DevExpress.XtraEditors.SeparatorControl separatorControl3;
        private DevExpress.XtraEditors.SimpleButton btnRedoStep;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
    }
}
