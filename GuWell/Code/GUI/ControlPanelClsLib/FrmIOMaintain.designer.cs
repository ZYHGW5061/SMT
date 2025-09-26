namespace ControlPanelClsLib
{
    partial class FrmIOMaintain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIOMaintain));
            this.ioMaintainPanel1 = new ControlPanelClsLib.IOMaintainPanel();
            this.SuspendLayout();
            // 
            // ioMaintainPanel1
            // 
            this.ioMaintainPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ioMaintainPanel1.Location = new System.Drawing.Point(0, 0);
            this.ioMaintainPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.ioMaintainPanel1.Name = "ioMaintainPanel1";
            this.ioMaintainPanel1.Size = new System.Drawing.Size(1083, 661);
            this.ioMaintainPanel1.TabIndex = 0;
            // 
            // FrmIOMaintain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1083, 661);
            this.Controls.Add(this.ioMaintainPanel1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmIOMaintain.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmIOMaintain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IO";
            this.ResumeLayout(false);

        }

        #endregion

        private IOMaintainPanel ioMaintainPanel1;
    }
}
