namespace MainGUI.Forms
{
    partial class FrmAddESTool
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
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sePlaceStress = new DevExpress.XtraEditors.SpinEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.seBreakVaccumTimespanMs = new DevExpress.XtraEditors.SpinEdit();
            this.sePlaceDelayMs = new DevExpress.XtraEditors.SpinEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.teChipPPPickPos = new DevExpress.XtraEditors.TextEdit();
            this.label23 = new System.Windows.Forms.Label();
            this.teChipPPPlacePos = new DevExpress.XtraEditors.TextEdit();
            this.label20 = new System.Windows.Forms.Label();
            this.cbPPName = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.teChipPPPress = new DevExpress.XtraEditors.TextEdit();
            this.teSlowSpeedAfterPickup = new DevExpress.XtraEditors.TextEdit();
            this.label28 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.teVaccumDelayMS = new DevExpress.XtraEditors.TextEdit();
            this.teSlowTravelAfterPickupMM = new DevExpress.XtraEditors.TextEdit();
            this.label38 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.teSlowSpeedBeforePickup = new DevExpress.XtraEditors.TextEdit();
            this.label34 = new System.Windows.Forms.Label();
            this.teSlowTravelBeforePickupMM = new DevExpress.XtraEditors.TextEdit();
            this.label17 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sePlaceStress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBreakVaccumTimespanMs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sePlaceDelayMs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teChipPPPickPos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teChipPPPlacePos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teChipPPPress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowSpeedAfterPickup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teVaccumDelayMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowTravelAfterPickupMM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowSpeedBeforePickup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowTravelBeforePickupMM.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(102, 268);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 42);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确认";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(446, 268);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 42);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sePlaceStress);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.seBreakVaccumTimespanMs);
            this.groupBox2.Controls.Add(this.sePlaceDelayMs);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.teChipPPPickPos);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.teChipPPPlacePos);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.cbPPName);
            this.groupBox2.Controls.Add(this.label36);
            this.groupBox2.Controls.Add(this.teChipPPPress);
            this.groupBox2.Controls.Add(this.teSlowSpeedAfterPickup);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label37);
            this.groupBox2.Controls.Add(this.teVaccumDelayMS);
            this.groupBox2.Controls.Add(this.teSlowTravelAfterPickupMM);
            this.groupBox2.Controls.Add(this.label38);
            this.groupBox2.Controls.Add(this.label35);
            this.groupBox2.Controls.Add(this.teSlowSpeedBeforePickup);
            this.groupBox2.Controls.Add(this.label34);
            this.groupBox2.Controls.Add(this.teSlowTravelBeforePickupMM);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(637, 187);
            this.groupBox2.TabIndex = 111;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "吸嘴参数";
            // 
            // sePlaceStress
            // 
            this.sePlaceStress.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sePlaceStress.Location = new System.Drawing.Point(529, 82);
            this.sePlaceStress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sePlaceStress.Name = "sePlaceStress";
            this.sePlaceStress.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sePlaceStress.Properties.DisplayFormat.FormatString = "0.000";
            this.sePlaceStress.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePlaceStress.Properties.EditFormat.FormatString = "0.000";
            this.sePlaceStress.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePlaceStress.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.sePlaceStress.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.sePlaceStress.Properties.MaskSettings.Set("mask", "f3");
            this.sePlaceStress.Properties.MaxLength = -100;
            this.sePlaceStress.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.sePlaceStress.Size = new System.Drawing.Size(95, 20);
            this.sePlaceStress.TabIndex = 111;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(451, 86);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 14);
            this.label6.TabIndex = 106;
            this.label6.Text = "放置压力：";
            // 
            // seBreakVaccumTimespanMs
            // 
            this.seBreakVaccumTimespanMs.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBreakVaccumTimespanMs.Location = new System.Drawing.Point(529, 108);
            this.seBreakVaccumTimespanMs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.seBreakVaccumTimespanMs.Name = "seBreakVaccumTimespanMs";
            this.seBreakVaccumTimespanMs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBreakVaccumTimespanMs.Size = new System.Drawing.Size(95, 20);
            this.seBreakVaccumTimespanMs.TabIndex = 109;
            // 
            // sePlaceDelayMs
            // 
            this.sePlaceDelayMs.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sePlaceDelayMs.Location = new System.Drawing.Point(529, 54);
            this.sePlaceDelayMs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sePlaceDelayMs.Name = "sePlaceDelayMs";
            this.sePlaceDelayMs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sePlaceDelayMs.Size = new System.Drawing.Size(95, 20);
            this.sePlaceDelayMs.TabIndex = 110;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(430, 112);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 14);
            this.label8.TabIndex = 107;
            this.label8.Text = "破空时长/ms：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(428, 58);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(87, 14);
            this.label14.TabIndex = 108;
            this.label14.Text = "放置延时/ms：";
            // 
            // teChipPPPickPos
            // 
            this.teChipPPPickPos.Location = new System.Drawing.Point(116, 81);
            this.teChipPPPickPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teChipPPPickPos.Name = "teChipPPPickPos";
            this.teChipPPPickPos.Size = new System.Drawing.Size(95, 20);
            this.teChipPPPickPos.TabIndex = 64;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(36, 86);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(73, 14);
            this.label23.TabIndex = 65;
            this.label23.Text = "拾芯片(mm)";
            // 
            // teChipPPPlacePos
            // 
            this.teChipPPPlacePos.Location = new System.Drawing.Point(116, 108);
            this.teChipPPPlacePos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teChipPPPlacePos.Name = "teChipPPPlacePos";
            this.teChipPPPlacePos.Size = new System.Drawing.Size(95, 20);
            this.teChipPPPlacePos.TabIndex = 66;
            this.teChipPPPlacePos.Visible = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(36, 112);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(73, 14);
            this.label20.TabIndex = 71;
            this.label20.Text = "放芯片(mm)";
            this.label20.Visible = false;
            // 
            // cbPPName
            // 
            this.cbPPName.FormattingEnabled = true;
            this.cbPPName.Location = new System.Drawing.Point(112, 19);
            this.cbPPName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbPPName.Name = "cbPPName";
            this.cbPPName.Size = new System.Drawing.Size(204, 22);
            this.cbPPName.TabIndex = 105;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(339, 163);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(167, 14);
            this.label36.TabIndex = 97;
            this.label36.Text = "拾取后慢速移动的速度(mm/s)";
            // 
            // teChipPPPress
            // 
            this.teChipPPPress.Location = new System.Drawing.Point(116, 54);
            this.teChipPPPress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teChipPPPress.Name = "teChipPPPress";
            this.teChipPPPress.Size = new System.Drawing.Size(95, 20);
            this.teChipPPPress.TabIndex = 99;
            // 
            // teSlowSpeedAfterPickup
            // 
            this.teSlowSpeedAfterPickup.Location = new System.Drawing.Point(529, 159);
            this.teSlowSpeedAfterPickup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teSlowSpeedAfterPickup.Name = "teSlowSpeedAfterPickup";
            this.teSlowSpeedAfterPickup.Size = new System.Drawing.Size(95, 20);
            this.teSlowSpeedAfterPickup.TabIndex = 96;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(393, 31);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(116, 14);
            this.label28.TabIndex = 95;
            this.label28.Text = "拾取真空延时(ms)：";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(14, 163);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(157, 14);
            this.label37.TabIndex = 95;
            this.label37.Text = "拾取后慢速移动的距离(mm)";
            // 
            // teVaccumDelayMS
            // 
            this.teVaccumDelayMS.Location = new System.Drawing.Point(529, 27);
            this.teVaccumDelayMS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teVaccumDelayMS.Name = "teVaccumDelayMS";
            this.teVaccumDelayMS.Size = new System.Drawing.Size(95, 20);
            this.teVaccumDelayMS.TabIndex = 94;
            // 
            // teSlowTravelAfterPickupMM
            // 
            this.teSlowTravelAfterPickupMM.Location = new System.Drawing.Point(192, 159);
            this.teSlowTravelAfterPickupMM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teSlowTravelAfterPickupMM.Name = "teSlowTravelAfterPickupMM";
            this.teSlowTravelAfterPickupMM.Size = new System.Drawing.Size(95, 20);
            this.teSlowTravelAfterPickupMM.TabIndex = 94;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(22, 58);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(85, 14);
            this.label38.TabIndex = 101;
            this.label38.Text = "压力偏移(mm)";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(340, 137);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(167, 14);
            this.label35.TabIndex = 93;
            this.label35.Text = "拾取前慢速移动的速度(mm/s)";
            // 
            // teSlowSpeedBeforePickup
            // 
            this.teSlowSpeedBeforePickup.Location = new System.Drawing.Point(529, 133);
            this.teSlowSpeedBeforePickup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teSlowSpeedBeforePickup.Name = "teSlowSpeedBeforePickup";
            this.teSlowSpeedBeforePickup.Size = new System.Drawing.Size(95, 20);
            this.teSlowSpeedBeforePickup.TabIndex = 92;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(14, 138);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(157, 14);
            this.label34.TabIndex = 91;
            this.label34.Text = "拾取前慢速移动的距离(mm)";
            // 
            // teSlowTravelBeforePickupMM
            // 
            this.teSlowTravelBeforePickupMM.Location = new System.Drawing.Point(192, 135);
            this.teSlowTravelBeforePickupMM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.teSlowTravelBeforePickupMM.Name = "teSlowTravelBeforePickupMM";
            this.teSlowTravelBeforePickupMM.Size = new System.Drawing.Size(95, 20);
            this.teSlowTravelBeforePickupMM.TabIndex = 90;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(18, 23);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(79, 14);
            this.label17.TabIndex = 61;
            this.label17.Text = "芯片吸嘴工具";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(276, 268);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(100, 42);
            this.simpleButton1.TabIndex = 112;
            this.simpleButton1.Text = "只修改吸嘴参数";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmAddESTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(666, 346);
            this.ControlBox = false;
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddESTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建BMC";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sePlaceStress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBreakVaccumTimespanMs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sePlaceDelayMs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teChipPPPickPos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teChipPPPlacePos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teChipPPPress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowSpeedAfterPickup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teVaccumDelayMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowTravelAfterPickupMM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowSpeedBeforePickup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSlowTravelBeforePickupMM.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SpinEdit sePlaceStress;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.SpinEdit seBreakVaccumTimespanMs;
        private DevExpress.XtraEditors.SpinEdit sePlaceDelayMs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.TextEdit teChipPPPickPos;
        private System.Windows.Forms.Label label23;
        private DevExpress.XtraEditors.TextEdit teChipPPPlacePos;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cbPPName;
        private System.Windows.Forms.Label label36;
        private DevExpress.XtraEditors.TextEdit teChipPPPress;
        private DevExpress.XtraEditors.TextEdit teSlowSpeedAfterPickup;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label37;
        private DevExpress.XtraEditors.TextEdit teVaccumDelayMS;
        private DevExpress.XtraEditors.TextEdit teSlowTravelAfterPickupMM;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label35;
        private DevExpress.XtraEditors.TextEdit teSlowSpeedBeforePickup;
        private System.Windows.Forms.Label label34;
        private DevExpress.XtraEditors.TextEdit teSlowTravelBeforePickupMM;
        private System.Windows.Forms.Label label17;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}