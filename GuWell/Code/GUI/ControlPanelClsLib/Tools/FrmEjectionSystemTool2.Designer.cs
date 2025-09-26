
namespace ControlPanelClsLib.Tools
{
    partial class FrmEjectionSystemTool2
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
            this.cmbExistESTool = new System.Windows.Forms.ComboBox();
            this.btnNewESTool = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.NeedleZeorPosition = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.LookuptoPPOrigionZ = new DevExpress.XtraEditors.SpinEdit();
            this.LookuptoPPOrigionY = new DevExpress.XtraEditors.SpinEdit();
            this.LookuptoPPOrigionX = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.ChipPPPosBracketY = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ChipPPPosBracketX = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnSetup = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.NeedleZeorPosition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookuptoPPOrigionZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookuptoPPOrigionY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookuptoPPOrigionX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChipPPPosBracketY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChipPPPosBracketX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbExistESTool
            // 
            this.cmbExistESTool.FormattingEnabled = true;
            this.cmbExistESTool.Location = new System.Drawing.Point(99, 12);
            this.cmbExistESTool.Name = "cmbExistESTool";
            this.cmbExistESTool.Size = new System.Drawing.Size(162, 22);
            this.cmbExistESTool.TabIndex = 28;
            this.cmbExistESTool.SelectedIndexChanged += new System.EventHandler(this.cmbExistESTool_SelectedIndexChanged);
            // 
            // btnNewESTool
            // 
            this.btnNewESTool.Location = new System.Drawing.Point(335, 14);
            this.btnNewESTool.Name = "btnNewESTool";
            this.btnNewESTool.Size = new System.Drawing.Size(71, 22);
            this.btnNewESTool.TabIndex = 27;
            this.btnNewESTool.Text = "新增";
            this.btnNewESTool.Click += new System.EventHandler(this.btnNewESTool_Click);
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(29, 17);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(52, 14);
            this.labelControl21.TabIndex = 26;
            this.labelControl21.Text = "已有工具:";
            // 
            // NeedleZeorPosition
            // 
            this.NeedleZeorPosition.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NeedleZeorPosition.Location = new System.Drawing.Point(412, 154);
            this.NeedleZeorPosition.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.NeedleZeorPosition.Name = "NeedleZeorPosition";
            this.NeedleZeorPosition.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.NeedleZeorPosition.Properties.AutoHeight = false;
            this.NeedleZeorPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NeedleZeorPosition.Properties.DisplayFormat.FormatString = "0.0";
            this.NeedleZeorPosition.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.NeedleZeorPosition.Properties.EditFormat.FormatString = "0.0";
            this.NeedleZeorPosition.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.NeedleZeorPosition.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NeedleZeorPosition.Properties.MaskSettings.Set("mask", "f1");
            this.NeedleZeorPosition.Size = new System.Drawing.Size(94, 23);
            this.NeedleZeorPosition.TabIndex = 83;
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.Options.UseTextOptions = true;
            this.labelControl12.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl12.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl12.Location = new System.Drawing.Point(92, 151);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(85, 29);
            this.labelControl12.TabIndex = 82;
            this.labelControl12.Text = "顶针原点高度:";
            // 
            // LookuptoPPOrigionZ
            // 
            this.LookuptoPPOrigionZ.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.LookuptoPPOrigionZ.Location = new System.Drawing.Point(412, 119);
            this.LookuptoPPOrigionZ.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.LookuptoPPOrigionZ.Name = "LookuptoPPOrigionZ";
            this.LookuptoPPOrigionZ.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.LookuptoPPOrigionZ.Properties.AutoHeight = false;
            this.LookuptoPPOrigionZ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LookuptoPPOrigionZ.Properties.DisplayFormat.FormatString = "0.0";
            this.LookuptoPPOrigionZ.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LookuptoPPOrigionZ.Properties.EditFormat.FormatString = "0.0";
            this.LookuptoPPOrigionZ.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LookuptoPPOrigionZ.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.LookuptoPPOrigionZ.Properties.MaskSettings.Set("mask", "f1");
            this.LookuptoPPOrigionZ.Size = new System.Drawing.Size(94, 23);
            this.LookuptoPPOrigionZ.TabIndex = 79;
            // 
            // LookuptoPPOrigionY
            // 
            this.LookuptoPPOrigionY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.LookuptoPPOrigionY.Location = new System.Drawing.Point(302, 119);
            this.LookuptoPPOrigionY.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.LookuptoPPOrigionY.Name = "LookuptoPPOrigionY";
            this.LookuptoPPOrigionY.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.LookuptoPPOrigionY.Properties.AutoHeight = false;
            this.LookuptoPPOrigionY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LookuptoPPOrigionY.Properties.DisplayFormat.FormatString = "0.0";
            this.LookuptoPPOrigionY.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LookuptoPPOrigionY.Properties.EditFormat.FormatString = "0.0";
            this.LookuptoPPOrigionY.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LookuptoPPOrigionY.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.LookuptoPPOrigionY.Properties.MaskSettings.Set("mask", "f1");
            this.LookuptoPPOrigionY.Size = new System.Drawing.Size(94, 23);
            this.LookuptoPPOrigionY.TabIndex = 80;
            // 
            // LookuptoPPOrigionX
            // 
            this.LookuptoPPOrigionX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.LookuptoPPOrigionX.Location = new System.Drawing.Point(191, 119);
            this.LookuptoPPOrigionX.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.LookuptoPPOrigionX.Name = "LookuptoPPOrigionX";
            this.LookuptoPPOrigionX.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.LookuptoPPOrigionX.Properties.AutoHeight = false;
            this.LookuptoPPOrigionX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LookuptoPPOrigionX.Properties.DisplayFormat.FormatString = "0.0";
            this.LookuptoPPOrigionX.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LookuptoPPOrigionX.Properties.EditFormat.FormatString = "0.0";
            this.LookuptoPPOrigionX.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LookuptoPPOrigionX.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.LookuptoPPOrigionX.Properties.MaskSettings.Set("mask", "f1");
            this.LookuptoPPOrigionX.Size = new System.Drawing.Size(94, 23);
            this.LookuptoPPOrigionX.TabIndex = 81;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Options.UseTextOptions = true;
            this.labelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl8.Location = new System.Drawing.Point(69, 116);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(108, 29);
            this.labelControl8.TabIndex = 78;
            this.labelControl8.Text = "榜头相机对准顶针:";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Options.UseTextOptions = true;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(441, 53);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(26, 29);
            this.labelControl4.TabIndex = 71;
            this.labelControl4.Text = "Z";
            // 
            // ChipPPPosBracketY
            // 
            this.ChipPPPosBracketY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChipPPPosBracketY.Location = new System.Drawing.Point(302, 84);
            this.ChipPPPosBracketY.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ChipPPPosBracketY.Name = "ChipPPPosBracketY";
            this.ChipPPPosBracketY.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.ChipPPPosBracketY.Properties.AutoHeight = false;
            this.ChipPPPosBracketY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ChipPPPosBracketY.Properties.DisplayFormat.FormatString = "0.0";
            this.ChipPPPosBracketY.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ChipPPPosBracketY.Properties.EditFormat.FormatString = "0.0";
            this.ChipPPPosBracketY.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ChipPPPosBracketY.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ChipPPPosBracketY.Properties.MaskSettings.Set("mask", "f1");
            this.ChipPPPosBracketY.Size = new System.Drawing.Size(94, 23);
            this.ChipPPPosBracketY.TabIndex = 76;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(331, 53);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(26, 29);
            this.labelControl2.TabIndex = 72;
            this.labelControl2.Text = "Y";
            // 
            // ChipPPPosBracketX
            // 
            this.ChipPPPosBracketX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChipPPPosBracketX.Location = new System.Drawing.Point(191, 84);
            this.ChipPPPosBracketX.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ChipPPPosBracketX.Name = "ChipPPPosBracketX";
            this.ChipPPPosBracketX.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.ChipPPPosBracketX.Properties.AutoHeight = false;
            this.ChipPPPosBracketX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ChipPPPosBracketX.Properties.DisplayFormat.FormatString = "0.0";
            this.ChipPPPosBracketX.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ChipPPPosBracketX.Properties.EditFormat.FormatString = "0.0";
            this.ChipPPPosBracketX.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ChipPPPosBracketX.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ChipPPPosBracketX.Properties.MaskSettings.Set("mask", "f1");
            this.ChipPPPosBracketX.Size = new System.Drawing.Size(94, 23);
            this.ChipPPPosBracketX.TabIndex = 77;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(220, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(26, 29);
            this.labelControl1.TabIndex = 73;
            this.labelControl1.Text = "X";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(7, 81);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(170, 29);
            this.labelControl3.TabIndex = 74;
            this.labelControl3.Text = "顶针中心在晶圆相机中的坐标:";
            // 
            // btnSetup
            // 
            this.btnSetup.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetup.Appearance.Options.UseFont = true;
            this.btnSetup.Location = new System.Drawing.Point(803, 82);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(115, 26);
            this.btnSetup.TabIndex = 84;
            this.btnSetup.Text = "设置";
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(561, 118);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(115, 26);
            this.simpleButton1.TabIndex = 85;
            this.simpleButton1.Text = "设置";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(561, 153);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(115, 26);
            this.simpleButton2.TabIndex = 86;
            this.simpleButton2.Text = "设置";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(693, 84);
            this.spinEdit1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.spinEdit1.Properties.AutoHeight = false;
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Properties.DisplayFormat.FormatString = "0.0";
            this.spinEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit1.Properties.EditFormat.FormatString = "0.0";
            this.spinEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit1.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.spinEdit1.Properties.MaskSettings.Set("mask", "f1");
            this.spinEdit1.Size = new System.Drawing.Size(94, 23);
            this.spinEdit1.TabIndex = 89;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Options.UseTextOptions = true;
            this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(722, 53);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(26, 29);
            this.labelControl5.TabIndex = 87;
            this.labelControl5.Text = "Y";
            // 
            // spinEdit2
            // 
            this.spinEdit2.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(582, 84);
            this.spinEdit2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.spinEdit2.Properties.AutoHeight = false;
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit2.Properties.DisplayFormat.FormatString = "0.0";
            this.spinEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit2.Properties.EditFormat.FormatString = "0.0";
            this.spinEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit2.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.spinEdit2.Properties.MaskSettings.Set("mask", "f1");
            this.spinEdit2.Size = new System.Drawing.Size(94, 23);
            this.spinEdit2.TabIndex = 90;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Options.UseTextOptions = true;
            this.labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.Location = new System.Drawing.Point(549, 53);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(94, 29);
            this.labelControl6.TabIndex = 88;
            this.labelControl6.Text = "X";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Options.UseTextOptions = true;
            this.labelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl7.Location = new System.Drawing.Point(582, 18);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(192, 29);
            this.labelControl7.TabIndex = 91;
            this.labelControl7.Text = "顶针中心在晶圆相机中的像素坐标:";
            // 
            // simpleButton6
            // 
            this.simpleButton6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.simpleButton6.Location = new System.Drawing.Point(803, 460);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(80, 26);
            this.simpleButton6.TabIndex = 92;
            this.simpleButton6.Text = "保存";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(422, 14);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(71, 22);
            this.simpleButton3.TabIndex = 93;
            this.simpleButton3.Text = "删除";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // FrmEjectionSystemTool2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 546);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.spinEdit1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.spinEdit2);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.NeedleZeorPosition);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.LookuptoPPOrigionZ);
            this.Controls.Add(this.LookuptoPPOrigionY);
            this.Controls.Add(this.LookuptoPPOrigionX);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.ChipPPPosBracketY);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.ChipPPPosBracketX);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cmbExistESTool);
            this.Controls.Add(this.btnNewESTool);
            this.Controls.Add(this.labelControl21);
            this.Name = "FrmEjectionSystemTool2";
            this.Text = "顶针工具";
            ((System.ComponentModel.ISupportInitialize)(this.NeedleZeorPosition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookuptoPPOrigionZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookuptoPPOrigionY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookuptoPPOrigionX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChipPPPosBracketY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChipPPPosBracketX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbExistESTool;
        private DevExpress.XtraEditors.SimpleButton btnNewESTool;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.SpinEdit NeedleZeorPosition;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.SpinEdit LookuptoPPOrigionZ;
        private DevExpress.XtraEditors.SpinEdit LookuptoPPOrigionY;
        private DevExpress.XtraEditors.SpinEdit LookuptoPPOrigionX;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SpinEdit ChipPPPosBracketY;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit ChipPPPosBracketX;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnSetup;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
    }
}