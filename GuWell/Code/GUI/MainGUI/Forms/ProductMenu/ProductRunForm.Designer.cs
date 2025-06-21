
namespace MainGUI.Forms.ProductMenu
{
    partial class ProductRunForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductRunForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAutoStart = new DevExpress.XtraEditors.SimpleButton();
            this.btnAutoPause = new DevExpress.XtraEditors.SimpleButton();
            this.btnStop = new DevExpress.XtraEditors.SimpleButton();
            this.btnAutoContinue = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cbProductList = new System.Windows.Forms.ComboBox();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.seStartIndex = new DevExpress.XtraEditors.SpinEdit();
            this.seProcessCount = new DevExpress.XtraEditors.SpinEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ckeIsProcessPart = new DevExpress.XtraEditors.CheckEdit();
            this.labelCounter = new System.Windows.Forms.Label();
            this.seStartChipIndex = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seStartIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seProcessCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeIsProcessPart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seStartChipIndex.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAutoStart);
            this.groupBox1.Controls.Add(this.btnAutoPause);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnAutoContinue);
            this.groupBox1.Location = new System.Drawing.Point(12, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 332);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生产操作";
            // 
            // btnAutoStart
            // 
            this.btnAutoStart.AllowFocus = false;
            this.btnAutoStart.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.next1;
            this.btnAutoStart.ImageOptions.SvgImageSize = new System.Drawing.Size(30, 30);
            this.btnAutoStart.Location = new System.Drawing.Point(28, 37);
            this.btnAutoStart.Name = "btnAutoStart";
            this.btnAutoStart.Size = new System.Drawing.Size(102, 36);
            this.btnAutoStart.TabIndex = 28;
            this.btnAutoStart.Text = "自动生产";
            this.btnAutoStart.Click += new System.EventHandler(this.btnAutoStart_Click);
            // 
            // btnAutoPause
            // 
            this.btnAutoPause.AllowFocus = false;
            this.btnAutoPause.Enabled = false;
            this.btnAutoPause.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.pause;
            this.btnAutoPause.Location = new System.Drawing.Point(28, 96);
            this.btnAutoPause.Name = "btnAutoPause";
            this.btnAutoPause.Size = new System.Drawing.Size(102, 36);
            this.btnAutoPause.TabIndex = 29;
            this.btnAutoPause.Text = "暂停";
            // 
            // btnStop
            // 
            this.btnStop.AllowFocus = false;
            this.btnStop.Enabled = false;
            this.btnStop.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point(28, 269);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(102, 36);
            this.btnStop.TabIndex = 31;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnAutoContinue
            // 
            this.btnAutoContinue.AllowFocus = false;
            this.btnAutoContinue.Enabled = false;
            this.btnAutoContinue.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.last;
            this.btnAutoContinue.Location = new System.Drawing.Point(28, 156);
            this.btnAutoContinue.Name = "btnAutoContinue";
            this.btnAutoContinue.Size = new System.Drawing.Size(102, 36);
            this.btnAutoContinue.TabIndex = 30;
            this.btnAutoContinue.Text = "继续";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择生产配方";
            // 
            // cbProductList
            // 
            this.cbProductList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProductList.FormattingEnabled = true;
            this.cbProductList.Location = new System.Drawing.Point(99, 13);
            this.cbProductList.Name = "cbProductList";
            this.cbProductList.Size = new System.Drawing.Size(227, 22);
            this.cbProductList.TabIndex = 2;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "名称";
            this.treeListColumn1.FieldName = "ActionDesc";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 246;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "状态";
            this.treeListColumn2.FieldName = "ActionStatDesc";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 1;
            this.treeListColumn2.Width = 364;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "undo_画板 1.png");
            this.imageList1.Images.SetKeyName(1, "run_画板 1.png");
            this.imageList1.Images.SetKeyName(2, "done_画板 1.png");
            this.imageList1.Images.SetKeyName(3, "error_画板 1.png");
            // 
            // seStartIndex
            // 
            this.seStartIndex.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartIndex.Location = new System.Drawing.Point(330, 190);
            this.seStartIndex.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seStartIndex.Name = "seStartIndex";
            this.seStartIndex.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seStartIndex.Properties.AutoHeight = false;
            this.seStartIndex.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seStartIndex.Properties.DisplayFormat.FormatString = "0";
            this.seStartIndex.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartIndex.Properties.EditFormat.FormatString = "0";
            this.seStartIndex.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartIndex.Properties.IsFloatValue = false;
            this.seStartIndex.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seStartIndex.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seStartIndex.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seStartIndex.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seStartIndex.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartIndex.Size = new System.Drawing.Size(76, 29);
            this.seStartIndex.TabIndex = 10;
            // 
            // seProcessCount
            // 
            this.seProcessCount.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seProcessCount.Location = new System.Drawing.Point(330, 231);
            this.seProcessCount.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seProcessCount.Name = "seProcessCount";
            this.seProcessCount.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seProcessCount.Properties.AutoHeight = false;
            this.seProcessCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seProcessCount.Properties.DisplayFormat.FormatString = "0";
            this.seProcessCount.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seProcessCount.Properties.EditFormat.FormatString = "0";
            this.seProcessCount.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seProcessCount.Properties.IsFloatValue = false;
            this.seProcessCount.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seProcessCount.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seProcessCount.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seProcessCount.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seProcessCount.Size = new System.Drawing.Size(76, 29);
            this.seProcessCount.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "基板从第";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(409, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "颗开始";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(307, 239);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "共";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(410, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 14);
            this.label8.TabIndex = 9;
            this.label8.Text = "颗";
            // 
            // ckeIsProcessPart
            // 
            this.ckeIsProcessPart.Location = new System.Drawing.Point(477, 237);
            this.ckeIsProcessPart.Name = "ckeIsProcessPart";
            this.ckeIsProcessPart.Properties.Caption = "部分工艺";
            this.ckeIsProcessPart.Size = new System.Drawing.Size(75, 20);
            this.ckeIsProcessPart.TabIndex = 11;
            // 
            // labelCounter
            // 
            this.labelCounter.AutoSize = true;
            this.labelCounter.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCounter.Location = new System.Drawing.Point(353, 84);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(67, 39);
            this.labelCounter.TabIndex = 5;
            this.labelCounter.Text = "0/0";
            // 
            // seStartChipIndex
            // 
            this.seStartChipIndex.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartChipIndex.Location = new System.Drawing.Point(330, 272);
            this.seStartChipIndex.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seStartChipIndex.Name = "seStartChipIndex";
            this.seStartChipIndex.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seStartChipIndex.Properties.AutoHeight = false;
            this.seStartChipIndex.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seStartChipIndex.Properties.DisplayFormat.FormatString = "0";
            this.seStartChipIndex.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartChipIndex.Properties.EditFormat.FormatString = "0";
            this.seStartChipIndex.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartChipIndex.Properties.IsFloatValue = false;
            this.seStartChipIndex.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seStartChipIndex.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seStartChipIndex.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seStartChipIndex.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seStartChipIndex.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartChipIndex.Size = new System.Drawing.Size(76, 29);
            this.seStartChipIndex.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(409, 279);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "颗开始";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "芯片从第";
            // 
            // ProductRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 396);
            this.Controls.Add(this.seStartChipIndex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ckeIsProcessPart);
            this.Controls.Add(this.seProcessCount);
            this.Controls.Add(this.seStartIndex);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelCounter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbProductList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProductRunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动生产";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProductRunForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seStartIndex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seProcessCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeIsProcessPart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seStartChipIndex.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProductList;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraEditors.SpinEdit seStartIndex;
        private DevExpress.XtraEditors.SpinEdit seProcessCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.SimpleButton btnAutoStart;
        private DevExpress.XtraEditors.SimpleButton btnAutoPause;
        private DevExpress.XtraEditors.SimpleButton btnStop;
        private DevExpress.XtraEditors.SimpleButton btnAutoContinue;
        private DevExpress.XtraEditors.CheckEdit ckeIsProcessPart;
        private System.Windows.Forms.Label labelCounter;
        private DevExpress.XtraEditors.SpinEdit seStartChipIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}