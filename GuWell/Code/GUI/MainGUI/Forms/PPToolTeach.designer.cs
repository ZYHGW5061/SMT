
namespace MainGUI.Forms
{
    partial class PPToolTeach
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
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbSelRecipe = new System.Windows.Forms.ComboBox();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.seAbandonPosZ = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.seAbandonPosY = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.seAbandonPosX = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.seAbandonPosZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seAbandonPosY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seAbandonPosX.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.Location = new System.Drawing.Point(356, 85);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(115, 36);
            this.simpleButton3.TabIndex = 20;
            this.simpleButton3.Text = "上料";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(356, 142);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(115, 36);
            this.simpleButton1.TabIndex = 20;
            this.simpleButton1.Text = "下料";
            // 
            // cmbSelRecipe
            // 
            this.cmbSelRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelRecipe.FormattingEnabled = true;
            this.cmbSelRecipe.Items.AddRange(new object[] {
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
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25"});
            this.cmbSelRecipe.Location = new System.Drawing.Point(86, 85);
            this.cmbSelRecipe.Name = "cmbSelRecipe";
            this.cmbSelRecipe.Size = new System.Drawing.Size(125, 22);
            this.cmbSelRecipe.TabIndex = 33;
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(28, 88);
            this.labelControl14.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(52, 14);
            this.labelControl14.TabIndex = 32;
            this.labelControl14.Text = "选择工具:";
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(814, 74);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 34;
            // 
            // seAbandonPosZ
            // 
            this.seAbandonPosZ.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seAbandonPosZ.Location = new System.Drawing.Point(356, 302);
            this.seAbandonPosZ.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seAbandonPosZ.Name = "seAbandonPosZ";
            this.seAbandonPosZ.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seAbandonPosZ.Properties.AutoHeight = false;
            this.seAbandonPosZ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seAbandonPosZ.Properties.DisplayFormat.FormatString = "0.0";
            this.seAbandonPosZ.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seAbandonPosZ.Properties.EditFormat.FormatString = "0.0";
            this.seAbandonPosZ.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seAbandonPosZ.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seAbandonPosZ.Properties.MaskSettings.Set("mask", "f1");
            this.seAbandonPosZ.Size = new System.Drawing.Size(94, 23);
            this.seAbandonPosZ.TabIndex = 39;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Options.UseTextOptions = true;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(385, 271);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(26, 29);
            this.labelControl4.TabIndex = 35;
            this.labelControl4.Text = "Z";
            // 
            // seAbandonPosY
            // 
            this.seAbandonPosY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seAbandonPosY.Location = new System.Drawing.Point(246, 302);
            this.seAbandonPosY.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seAbandonPosY.Name = "seAbandonPosY";
            this.seAbandonPosY.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seAbandonPosY.Properties.AutoHeight = false;
            this.seAbandonPosY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seAbandonPosY.Properties.DisplayFormat.FormatString = "0.0";
            this.seAbandonPosY.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seAbandonPosY.Properties.EditFormat.FormatString = "0.0";
            this.seAbandonPosY.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seAbandonPosY.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seAbandonPosY.Properties.MaskSettings.Set("mask", "f1");
            this.seAbandonPosY.Size = new System.Drawing.Size(94, 23);
            this.seAbandonPosY.TabIndex = 40;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(275, 271);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(26, 29);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Text = "Y";
            // 
            // seAbandonPosX
            // 
            this.seAbandonPosX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seAbandonPosX.Location = new System.Drawing.Point(135, 302);
            this.seAbandonPosX.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seAbandonPosX.Name = "seAbandonPosX";
            this.seAbandonPosX.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seAbandonPosX.Properties.AutoHeight = false;
            this.seAbandonPosX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seAbandonPosX.Properties.DisplayFormat.FormatString = "0.0";
            this.seAbandonPosX.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seAbandonPosX.Properties.EditFormat.FormatString = "0.0";
            this.seAbandonPosX.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seAbandonPosX.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.seAbandonPosX.Properties.MaskSettings.Set("mask", "f1");
            this.seAbandonPosX.Size = new System.Drawing.Size(94, 23);
            this.seAbandonPosX.TabIndex = 41;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(164, 271);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(26, 29);
            this.labelControl1.TabIndex = 37;
            this.labelControl1.Text = "X";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(70, 299);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(59, 29);
            this.labelControl3.TabIndex = 38;
            this.labelControl3.Text = "抛料位置:";
            // 
            // PPToolTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 633);
            this.Controls.Add(this.seAbandonPosZ);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.seAbandonPosY);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.seAbandonPosX);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.cmbSelRecipe);
            this.Controls.Add(this.labelControl14);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton3);
            this.Name = "PPToolTeach";
            this.Text = "吸嘴工具示教";
            ((System.ComponentModel.ISupportInitialize)(this.seAbandonPosZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seAbandonPosY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seAbandonPosX.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.ComboBox cmbSelRecipe;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.SpinEdit seAbandonPosZ;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SpinEdit seAbandonPosY;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit seAbandonPosX;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}