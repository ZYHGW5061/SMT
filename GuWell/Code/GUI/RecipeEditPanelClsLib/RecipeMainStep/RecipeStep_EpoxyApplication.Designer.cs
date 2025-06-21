
namespace RecipeEditPanelClsLib
{
    partial class RecipeStep_EpoxyApplication
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
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSetDispenseParam = new DevExpress.XtraEditors.SimpleButton();
            this.seDispenseTimeS = new DevExpress.XtraEditors.SpinEdit();
            this.出胶时长 = new DevExpress.XtraEditors.LabelControl();
            this.seDispenseVaccumPressure = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.seDispensePressure = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.seDispensePatternHeight = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.seDispensePatternWidth = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.cmbSelDispenserRecipe = new System.Windows.Forms.ComboBox();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.cmbDispensePattern = new System.Windows.Forms.ComboBox();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seDispenseTimeS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispenseVaccumPressure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispensePressure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispensePatternHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispensePatternWidth.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.groupBox4);
            this.panelControl2.Controls.Add(this.seDispensePatternHeight);
            this.panelControl2.Controls.Add(this.labelControl9);
            this.panelControl2.Controls.Add(this.seDispensePatternWidth);
            this.panelControl2.Controls.Add(this.labelControl7);
            this.panelControl2.Controls.Add(this.cmbSelDispenserRecipe);
            this.panelControl2.Controls.Add(this.labelControl11);
            this.panelControl2.Controls.Add(this.labelControl8);
            this.panelControl2.Controls.Add(this.cmbDispensePattern);
            this.panelControl2.Controls.Add(this.labelControl6);
            this.panelControl2.Location = new System.Drawing.Point(13, 17);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(898, 608);
            this.panelControl2.TabIndex = 42;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(68, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 19;
            this.labelControl1.Text = "出胶配方:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSetDispenseParam);
            this.groupBox4.Controls.Add(this.seDispenseTimeS);
            this.groupBox4.Controls.Add(this.出胶时长);
            this.groupBox4.Controls.Add(this.seDispenseVaccumPressure);
            this.groupBox4.Controls.Add(this.labelControl10);
            this.groupBox4.Controls.Add(this.labelControl3);
            this.groupBox4.Controls.Add(this.seDispensePressure);
            this.groupBox4.Controls.Add(this.labelControl2);
            this.groupBox4.Controls.Add(this.labelControl4);
            this.groupBox4.Controls.Add(this.labelControl5);
            this.groupBox4.Location = new System.Drawing.Point(37, 92);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(273, 210);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "参数设置";
            // 
            // btnSetDispenseParam
            // 
            this.btnSetDispenseParam.Location = new System.Drawing.Point(98, 154);
            this.btnSetDispenseParam.Name = "btnSetDispenseParam";
            this.btnSetDispenseParam.Size = new System.Drawing.Size(75, 23);
            this.btnSetDispenseParam.TabIndex = 20;
            this.btnSetDispenseParam.Text = "设置";
            this.btnSetDispenseParam.Click += new System.EventHandler(this.btnSetDispenseParam_Click);
            // 
            // seDispenseTimeS
            // 
            this.seDispenseTimeS.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seDispenseTimeS.Location = new System.Drawing.Point(91, 100);
            this.seDispenseTimeS.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seDispenseTimeS.Name = "seDispenseTimeS";
            this.seDispenseTimeS.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seDispenseTimeS.Properties.AutoHeight = false;
            this.seDispenseTimeS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seDispenseTimeS.Properties.DisplayFormat.FormatString = "0.000";
            this.seDispenseTimeS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispenseTimeS.Properties.EditFormat.FormatString = "0.000";
            this.seDispenseTimeS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispenseTimeS.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seDispenseTimeS.Properties.MaskSettings.Set("mask", "f3");
            this.seDispenseTimeS.Properties.ReadOnly = true;
            this.seDispenseTimeS.Size = new System.Drawing.Size(94, 23);
            this.seDispenseTimeS.TabIndex = 17;
            // 
            // 出胶时长
            // 
            this.出胶时长.Location = new System.Drawing.Point(32, 104);
            this.出胶时长.Name = "出胶时长";
            this.出胶时长.Size = new System.Drawing.Size(52, 14);
            this.出胶时长.TabIndex = 14;
            this.出胶时长.Text = "出胶时长:";
            // 
            // seDispenseVaccumPressure
            // 
            this.seDispenseVaccumPressure.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seDispenseVaccumPressure.Location = new System.Drawing.Point(91, 65);
            this.seDispenseVaccumPressure.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seDispenseVaccumPressure.Name = "seDispenseVaccumPressure";
            this.seDispenseVaccumPressure.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seDispenseVaccumPressure.Properties.AutoHeight = false;
            this.seDispenseVaccumPressure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seDispenseVaccumPressure.Properties.DisplayFormat.FormatString = "0.000";
            this.seDispenseVaccumPressure.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispenseVaccumPressure.Properties.EditFormat.FormatString = "0.000";
            this.seDispenseVaccumPressure.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispenseVaccumPressure.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seDispenseVaccumPressure.Properties.MaskSettings.Set("mask", "f3");
            this.seDispenseVaccumPressure.Properties.ReadOnly = true;
            this.seDispenseVaccumPressure.Size = new System.Drawing.Size(94, 23);
            this.seDispenseVaccumPressure.TabIndex = 17;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(55, 69);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(28, 14);
            this.labelControl10.TabIndex = 14;
            this.labelControl10.Text = "负压:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(56, 34);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(28, 14);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "正压:";
            // 
            // seDispensePressure
            // 
            this.seDispensePressure.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seDispensePressure.Location = new System.Drawing.Point(91, 30);
            this.seDispensePressure.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seDispensePressure.Name = "seDispensePressure";
            this.seDispensePressure.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seDispensePressure.Properties.AutoHeight = false;
            this.seDispensePressure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seDispensePressure.Properties.DisplayFormat.FormatString = "0.000";
            this.seDispensePressure.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispensePressure.Properties.EditFormat.FormatString = "0.000";
            this.seDispensePressure.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispensePressure.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seDispensePressure.Properties.MaskSettings.Set("mask", "f3");
            this.seDispensePressure.Properties.ReadOnly = true;
            this.seDispensePressure.Size = new System.Drawing.Size(94, 23);
            this.seDispensePressure.TabIndex = 17;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(189, 104);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(5, 14);
            this.labelControl2.TabIndex = 14;
            this.labelControl2.Text = "s";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(189, 34);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(19, 14);
            this.labelControl4.TabIndex = 14;
            this.labelControl4.Text = "kPa";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(189, 69);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(19, 14);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "kPa";
            // 
            // seDispensePatternHeight
            // 
            this.seDispensePatternHeight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seDispensePatternHeight.Location = new System.Drawing.Point(123, 464);
            this.seDispensePatternHeight.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seDispensePatternHeight.Name = "seDispensePatternHeight";
            this.seDispensePatternHeight.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seDispensePatternHeight.Properties.AutoHeight = false;
            this.seDispensePatternHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seDispensePatternHeight.Properties.DisplayFormat.FormatString = "0.000";
            this.seDispensePatternHeight.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispensePatternHeight.Properties.EditFormat.FormatString = "0.000";
            this.seDispensePatternHeight.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispensePatternHeight.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seDispensePatternHeight.Properties.MaskSettings.Set("mask", "f3");
            this.seDispensePatternHeight.Size = new System.Drawing.Size(94, 23);
            this.seDispensePatternHeight.TabIndex = 17;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(87, 468);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(16, 14);
            this.labelControl9.TabIndex = 14;
            this.labelControl9.Text = "高:";
            // 
            // seDispensePatternWidth
            // 
            this.seDispensePatternWidth.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seDispensePatternWidth.Location = new System.Drawing.Point(123, 429);
            this.seDispensePatternWidth.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seDispensePatternWidth.Name = "seDispensePatternWidth";
            this.seDispensePatternWidth.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seDispensePatternWidth.Properties.AutoHeight = false;
            this.seDispensePatternWidth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seDispensePatternWidth.Properties.DisplayFormat.FormatString = "0.000";
            this.seDispensePatternWidth.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispensePatternWidth.Properties.EditFormat.FormatString = "0.000";
            this.seDispensePatternWidth.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seDispensePatternWidth.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seDispensePatternWidth.Properties.MaskSettings.Set("mask", "f3");
            this.seDispensePatternWidth.Size = new System.Drawing.Size(94, 23);
            this.seDispensePatternWidth.TabIndex = 17;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(87, 433);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(16, 14);
            this.labelControl7.TabIndex = 14;
            this.labelControl7.Text = "宽:";
            // 
            // cmbSelDispenserRecipe
            // 
            this.cmbSelDispenserRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelDispenserRecipe.FormattingEnabled = true;
            this.cmbSelDispenserRecipe.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cmbSelDispenserRecipe.Location = new System.Drawing.Point(128, 32);
            this.cmbSelDispenserRecipe.Name = "cmbSelDispenserRecipe";
            this.cmbSelDispenserRecipe.Size = new System.Drawing.Size(94, 22);
            this.cmbSelDispenserRecipe.TabIndex = 18;
            this.cmbSelDispenserRecipe.SelectedIndexChanged += new System.EventHandler(this.cmbSelDispenserRecipe_SelectedIndexChanged);
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(52, 387);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(52, 14);
            this.labelControl11.TabIndex = 14;
            this.labelControl11.Text = "画胶模板:";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(221, 468);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(20, 14);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "mm";
            // 
            // cmbDispensePattern
            // 
            this.cmbDispensePattern.FormattingEnabled = true;
            this.cmbDispensePattern.Location = new System.Drawing.Point(123, 384);
            this.cmbDispensePattern.Name = "cmbDispensePattern";
            this.cmbDispensePattern.Size = new System.Drawing.Size(94, 22);
            this.cmbDispensePattern.TabIndex = 13;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(221, 433);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(20, 14);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "mm";
            // 
            // RecipeStep_EpoxyApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Name = "RecipeStep_EpoxyApplication";
            this.Size = new System.Drawing.Size(1105, 719);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seDispenseTimeS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispenseVaccumPressure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispensePressure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispensePatternHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDispensePatternWidth.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.SpinEdit seDispenseVaccumPressure;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SpinEdit seDispensePressure;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private System.Windows.Forms.ComboBox cmbDispensePattern;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit seDispenseTimeS;
        private DevExpress.XtraEditors.LabelControl 出胶时长;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.ComboBox cmbSelDispenserRecipe;
        private DevExpress.XtraEditors.SpinEdit seDispensePatternHeight;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SpinEdit seDispensePatternWidth;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btnSetDispenseParam;
    }
}
