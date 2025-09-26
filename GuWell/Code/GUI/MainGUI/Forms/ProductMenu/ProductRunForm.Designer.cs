
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
            this.btnStep = new DevExpress.XtraEditors.SimpleButton();
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
            this.seStartModule = new DevExpress.XtraEditors.SpinEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.seCurModuleNum = new DevExpress.XtraEditors.SpinEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.seCurChipNum = new DevExpress.XtraEditors.SpinEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.seCurSubstrateNum = new DevExpress.XtraEditors.SpinEdit();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.seBondCounter = new DevExpress.XtraEditors.SpinEdit();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.seEndModule = new DevExpress.XtraEditors.SpinEdit();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.seEndIndex = new DevExpress.XtraEditors.SpinEdit();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.seEndBondingPosition = new DevExpress.XtraEditors.SpinEdit();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.seStartBondingPosition = new DevExpress.XtraEditors.SpinEdit();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.spinEdit3 = new DevExpress.XtraEditors.SpinEdit();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seStartIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seProcessCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeIsProcessPart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seStartChipIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seStartModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCurModuleNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCurChipNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCurSubstrateNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondCounter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seEndModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seEndIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seEndBondingPosition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seStartBondingPosition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStep);
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
            // btnStep
            // 
            this.btnStep.AllowFocus = false;
            this.btnStep.Enabled = false;
            this.btnStep.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.last;
            this.btnStep.Location = new System.Drawing.Point(28, 143);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(102, 36);
            this.btnStep.TabIndex = 32;
            this.btnStep.Text = "单步(F10)";
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
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
            this.btnAutoPause.Location = new System.Drawing.Point(28, 90);
            this.btnAutoPause.Name = "btnAutoPause";
            this.btnAutoPause.Size = new System.Drawing.Size(102, 36);
            this.btnAutoPause.TabIndex = 29;
            this.btnAutoPause.Text = "暂停(F9)";
            this.btnAutoPause.Click += new System.EventHandler(this.btnAutoPause_Click);
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
            this.btnAutoContinue.ImageOptions.SvgImage = global::MainGUI.Properties.Resources.doublenext;
            this.btnAutoContinue.Location = new System.Drawing.Point(28, 196);
            this.btnAutoContinue.Name = "btnAutoContinue";
            this.btnAutoContinue.Size = new System.Drawing.Size(102, 36);
            this.btnAutoContinue.TabIndex = 30;
            this.btnAutoContinue.Text = "继续(F8)";
            this.btnAutoContinue.Click += new System.EventHandler(this.btnAutoContinue_Click);
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
            this.cbProductList.SelectedIndexChanged += new System.EventHandler(this.cbProductList_SelectedIndexChanged);
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
            this.seStartIndex.Location = new System.Drawing.Point(274, 64);
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
            this.seProcessCount.Location = new System.Drawing.Point(476, 351);
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
            this.label4.Location = new System.Drawing.Point(215, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "基板从第";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(353, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "颗开始";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(453, 359);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "共";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(556, 358);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 14);
            this.label8.TabIndex = 9;
            this.label8.Text = "颗";
            // 
            // ckeIsProcessPart
            // 
            this.ckeIsProcessPart.Location = new System.Drawing.Point(275, 356);
            this.ckeIsProcessPart.Name = "ckeIsProcessPart";
            this.ckeIsProcessPart.Properties.Caption = "部分工艺";
            this.ckeIsProcessPart.Size = new System.Drawing.Size(75, 20);
            this.ckeIsProcessPart.TabIndex = 11;
            // 
            // labelCounter
            // 
            this.labelCounter.AutoSize = true;
            this.labelCounter.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCounter.Location = new System.Drawing.Point(579, 348);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(67, 39);
            this.labelCounter.TabIndex = 5;
            this.labelCounter.Text = "0/0";
            this.labelCounter.Visible = false;
            // 
            // seStartChipIndex
            // 
            this.seStartChipIndex.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartChipIndex.Location = new System.Drawing.Point(275, 187);
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
            this.label2.Location = new System.Drawing.Point(354, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "颗开始";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "芯片从第";
            // 
            // seStartModule
            // 
            this.seStartModule.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartModule.Location = new System.Drawing.Point(275, 105);
            this.seStartModule.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seStartModule.Name = "seStartModule";
            this.seStartModule.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seStartModule.Properties.AutoHeight = false;
            this.seStartModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seStartModule.Properties.DisplayFormat.FormatString = "0";
            this.seStartModule.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartModule.Properties.EditFormat.FormatString = "0";
            this.seStartModule.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartModule.Properties.IsFloatValue = false;
            this.seStartModule.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seStartModule.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seStartModule.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seStartModule.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seStartModule.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartModule.Size = new System.Drawing.Size(76, 29);
            this.seStartModule.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(354, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 14);
            this.label5.TabIndex = 15;
            this.label5.Text = "颗开始";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(216, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 14);
            this.label9.TabIndex = 16;
            this.label9.Text = "模块从第";
            // 
            // seCurModuleNum
            // 
            this.seCurModuleNum.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seCurModuleNum.Enabled = false;
            this.seCurModuleNum.Location = new System.Drawing.Point(477, 228);
            this.seCurModuleNum.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seCurModuleNum.Name = "seCurModuleNum";
            this.seCurModuleNum.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seCurModuleNum.Properties.AutoHeight = false;
            this.seCurModuleNum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seCurModuleNum.Properties.DisplayFormat.FormatString = "0";
            this.seCurModuleNum.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCurModuleNum.Properties.EditFormat.FormatString = "0";
            this.seCurModuleNum.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCurModuleNum.Properties.IsFloatValue = false;
            this.seCurModuleNum.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seCurModuleNum.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seCurModuleNum.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seCurModuleNum.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seCurModuleNum.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seCurModuleNum.Properties.ReadOnly = true;
            this.seCurModuleNum.Size = new System.Drawing.Size(76, 29);
            this.seCurModuleNum.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(556, 235);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 14);
            this.label10.TabIndex = 24;
            this.label10.Text = "颗";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(402, 235);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 14);
            this.label11.TabIndex = 25;
            this.label11.Text = "当前模块第";
            // 
            // seCurChipNum
            // 
            this.seCurChipNum.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seCurChipNum.Enabled = false;
            this.seCurChipNum.Location = new System.Drawing.Point(275, 310);
            this.seCurChipNum.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seCurChipNum.Name = "seCurChipNum";
            this.seCurChipNum.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seCurChipNum.Properties.AutoHeight = false;
            this.seCurChipNum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seCurChipNum.Properties.DisplayFormat.FormatString = "0";
            this.seCurChipNum.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCurChipNum.Properties.EditFormat.FormatString = "0";
            this.seCurChipNum.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCurChipNum.Properties.IsFloatValue = false;
            this.seCurChipNum.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seCurChipNum.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seCurChipNum.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seCurChipNum.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seCurChipNum.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seCurChipNum.Properties.ReadOnly = true;
            this.seCurChipNum.Size = new System.Drawing.Size(76, 29);
            this.seCurChipNum.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(354, 317);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 14);
            this.label12.TabIndex = 21;
            this.label12.Text = "颗";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(200, 317);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 14);
            this.label13.TabIndex = 22;
            this.label13.Text = "当前芯片第";
            // 
            // seCurSubstrateNum
            // 
            this.seCurSubstrateNum.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seCurSubstrateNum.Enabled = false;
            this.seCurSubstrateNum.Location = new System.Drawing.Point(275, 228);
            this.seCurSubstrateNum.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seCurSubstrateNum.Name = "seCurSubstrateNum";
            this.seCurSubstrateNum.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seCurSubstrateNum.Properties.AutoHeight = false;
            this.seCurSubstrateNum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seCurSubstrateNum.Properties.DisplayFormat.FormatString = "0";
            this.seCurSubstrateNum.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCurSubstrateNum.Properties.EditFormat.FormatString = "0";
            this.seCurSubstrateNum.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCurSubstrateNum.Properties.IsFloatValue = false;
            this.seCurSubstrateNum.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seCurSubstrateNum.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seCurSubstrateNum.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seCurSubstrateNum.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seCurSubstrateNum.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seCurSubstrateNum.Properties.ReadOnly = true;
            this.seCurSubstrateNum.Size = new System.Drawing.Size(76, 29);
            this.seCurSubstrateNum.TabIndex = 20;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(354, 235);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 14);
            this.label14.TabIndex = 18;
            this.label14.Text = "颗";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(200, 235);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 14);
            this.label15.TabIndex = 19;
            this.label15.Text = "当前基板第";
            // 
            // seBondCounter
            // 
            this.seBondCounter.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seBondCounter.Enabled = false;
            this.seBondCounter.Location = new System.Drawing.Point(477, 310);
            this.seBondCounter.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seBondCounter.Name = "seBondCounter";
            this.seBondCounter.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seBondCounter.Properties.AutoHeight = false;
            this.seBondCounter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBondCounter.Properties.DisplayFormat.FormatString = "0";
            this.seBondCounter.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondCounter.Properties.EditFormat.FormatString = "0";
            this.seBondCounter.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondCounter.Properties.IsFloatValue = false;
            this.seBondCounter.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seBondCounter.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seBondCounter.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seBondCounter.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seBondCounter.Properties.ReadOnly = true;
            this.seBondCounter.Size = new System.Drawing.Size(76, 29);
            this.seBondCounter.TabIndex = 29;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(557, 317);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 14);
            this.label16.TabIndex = 28;
            this.label16.Text = "颗";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(438, 317);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(31, 14);
            this.label17.TabIndex = 27;
            this.label17.Text = "已贴";
            // 
            // seEndModule
            // 
            this.seEndModule.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seEndModule.Location = new System.Drawing.Point(476, 105);
            this.seEndModule.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seEndModule.Name = "seEndModule";
            this.seEndModule.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seEndModule.Properties.AutoHeight = false;
            this.seEndModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seEndModule.Properties.DisplayFormat.FormatString = "0";
            this.seEndModule.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seEndModule.Properties.EditFormat.FormatString = "0";
            this.seEndModule.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seEndModule.Properties.IsFloatValue = false;
            this.seEndModule.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seEndModule.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seEndModule.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seEndModule.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seEndModule.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seEndModule.Size = new System.Drawing.Size(76, 29);
            this.seEndModule.TabIndex = 35;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(555, 112);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 14);
            this.label18.TabIndex = 33;
            this.label18.Text = "颗结束";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(438, 112);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 14);
            this.label19.TabIndex = 34;
            this.label19.Text = "到第";
            // 
            // seEndIndex
            // 
            this.seEndIndex.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seEndIndex.Location = new System.Drawing.Point(476, 64);
            this.seEndIndex.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seEndIndex.Name = "seEndIndex";
            this.seEndIndex.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seEndIndex.Properties.AutoHeight = false;
            this.seEndIndex.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seEndIndex.Properties.DisplayFormat.FormatString = "0";
            this.seEndIndex.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seEndIndex.Properties.EditFormat.FormatString = "0";
            this.seEndIndex.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seEndIndex.Properties.IsFloatValue = false;
            this.seEndIndex.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seEndIndex.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seEndIndex.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seEndIndex.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seEndIndex.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seEndIndex.Size = new System.Drawing.Size(76, 29);
            this.seEndIndex.TabIndex = 32;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(555, 71);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(43, 14);
            this.label20.TabIndex = 30;
            this.label20.Text = "颗结束";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(438, 71);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 14);
            this.label21.TabIndex = 31;
            this.label21.Text = "到第";
            // 
            // seEndBondingPosition
            // 
            this.seEndBondingPosition.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seEndBondingPosition.Location = new System.Drawing.Point(476, 146);
            this.seEndBondingPosition.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seEndBondingPosition.Name = "seEndBondingPosition";
            this.seEndBondingPosition.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seEndBondingPosition.Properties.AutoHeight = false;
            this.seEndBondingPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seEndBondingPosition.Properties.DisplayFormat.FormatString = "0";
            this.seEndBondingPosition.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seEndBondingPosition.Properties.EditFormat.FormatString = "0";
            this.seEndBondingPosition.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seEndBondingPosition.Properties.IsFloatValue = false;
            this.seEndBondingPosition.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seEndBondingPosition.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seEndBondingPosition.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seEndBondingPosition.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seEndBondingPosition.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seEndBondingPosition.Size = new System.Drawing.Size(76, 29);
            this.seEndBondingPosition.TabIndex = 41;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(555, 153);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(43, 14);
            this.label22.TabIndex = 39;
            this.label22.Text = "颗结束";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(438, 153);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(31, 14);
            this.label23.TabIndex = 40;
            this.label23.Text = "到第";
            // 
            // seStartBondingPosition
            // 
            this.seStartBondingPosition.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartBondingPosition.Location = new System.Drawing.Point(275, 146);
            this.seStartBondingPosition.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seStartBondingPosition.Name = "seStartBondingPosition";
            this.seStartBondingPosition.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seStartBondingPosition.Properties.AutoHeight = false;
            this.seStartBondingPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seStartBondingPosition.Properties.DisplayFormat.FormatString = "0";
            this.seStartBondingPosition.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartBondingPosition.Properties.EditFormat.FormatString = "0";
            this.seStartBondingPosition.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seStartBondingPosition.Properties.IsFloatValue = false;
            this.seStartBondingPosition.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seStartBondingPosition.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seStartBondingPosition.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.seStartBondingPosition.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.seStartBondingPosition.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seStartBondingPosition.Size = new System.Drawing.Size(76, 29);
            this.seStartBondingPosition.TabIndex = 38;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(354, 153);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(43, 14);
            this.label24.TabIndex = 36;
            this.label24.Text = "颗开始";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(192, 153);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(79, 14);
            this.label25.TabIndex = 37;
            this.label25.Text = "贴片位置从第";
            // 
            // spinEdit3
            // 
            this.spinEdit3.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit3.Enabled = false;
            this.spinEdit3.Location = new System.Drawing.Point(274, 269);
            this.spinEdit3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.spinEdit3.Name = "spinEdit3";
            this.spinEdit3.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.spinEdit3.Properties.AutoHeight = false;
            this.spinEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit3.Properties.DisplayFormat.FormatString = "0";
            this.spinEdit3.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit3.Properties.EditFormat.FormatString = "0";
            this.spinEdit3.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit3.Properties.IsFloatValue = false;
            this.spinEdit3.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.spinEdit3.Properties.MaskSettings.Set("allowBlankInput", true);
            this.spinEdit3.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.spinEdit3.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spinEdit3.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit3.Properties.ReadOnly = true;
            this.spinEdit3.Size = new System.Drawing.Size(76, 29);
            this.spinEdit3.TabIndex = 44;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(353, 276);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(19, 14);
            this.label26.TabIndex = 42;
            this.label26.Text = "颗";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(179, 276);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(91, 14);
            this.label27.TabIndex = 43;
            this.label27.Text = "当前贴片位置第";
            // 
            // ProductRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 396);
            this.Controls.Add(this.spinEdit3);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.seEndBondingPosition);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.seStartBondingPosition);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.seEndModule);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.seEndIndex);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.seBondCounter);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.seCurModuleNum);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.seCurChipNum);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.seCurSubstrateNum);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.seStartModule);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
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
            ((System.ComponentModel.ISupportInitialize)(this.seStartModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCurModuleNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCurChipNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCurSubstrateNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondCounter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seEndModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seEndIndex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seEndBondingPosition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seStartBondingPosition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).EndInit();
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
        private DevExpress.XtraEditors.SimpleButton btnStep;
        private DevExpress.XtraEditors.SpinEdit seStartModule;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.SpinEdit seCurModuleNum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.SpinEdit seCurChipNum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.SpinEdit seCurSubstrateNum;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private DevExpress.XtraEditors.SpinEdit seBondCounter;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private DevExpress.XtraEditors.SpinEdit seEndModule;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private DevExpress.XtraEditors.SpinEdit seEndIndex;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private DevExpress.XtraEditors.SpinEdit seEndBondingPosition;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private DevExpress.XtraEditors.SpinEdit seStartBondingPosition;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private DevExpress.XtraEditors.SpinEdit spinEdit3;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
    }
}