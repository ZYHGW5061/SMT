namespace CommonPanelClsLib
{
    partial class WaferControl
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
            this.textWaferID = new System.Windows.Forms.TextBox();
            this.LabWafer = new System.Windows.Forms.Label();
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // textWaferID
            // 
            this.textWaferID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textWaferID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textWaferID.Location = new System.Drawing.Point(0, 0);
            this.textWaferID.Margin = new System.Windows.Forms.Padding(0);
            this.textWaferID.Name = "textWaferID";
            this.textWaferID.Size = new System.Drawing.Size(112, 14);
            this.textWaferID.TabIndex = 3;
            this.textWaferID.Tag = "";
            this.textWaferID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabWafer
            // 
            this.LabWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabWafer.Location = new System.Drawing.Point(0, 0);
            this.LabWafer.Margin = new System.Windows.Forms.Padding(0);
            this.LabWafer.Name = "LabWafer";
            this.LabWafer.Size = new System.Drawing.Size(112, 27);
            this.LabWafer.TabIndex = 4;
            this.LabWafer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.LabWafer, 0, 1);
            this.tableMain.Controls.Add(this.textWaferID, 0, 0);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 2;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Size = new System.Drawing.Size(112, 27);
            this.tableMain.TabIndex = 1;
            // 
            // StripControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableMain);
            this.Name = "StripControl";
            this.Size = new System.Drawing.Size(112, 27);
            this.tableMain.ResumeLayout(false);
            this.tableMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textWaferID;
        private System.Windows.Forms.Label LabWafer;
        private System.Windows.Forms.TableLayoutPanel tableMain;
    }
}
