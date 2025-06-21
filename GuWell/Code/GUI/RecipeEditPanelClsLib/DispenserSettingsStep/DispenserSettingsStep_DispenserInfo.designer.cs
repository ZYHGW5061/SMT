
using DevExpress.XtraEditors;

namespace RecipeEditPanelClsLib
{
    partial class DispenserSettingsStep_DispenserInfo
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelStepInfo = new DevExpress.XtraEditors.LabelControl();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox5 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sesePredispensingOffsetY = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.sesePredispensingOffsetX = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.sePredispensingIntervelSeconds = new DevExpress.XtraEditors.SpinEdit();
            this.sePredispensingIntervelMinutes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.sePredispensingTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.teAbsoluteMoveTarget = new DevExpress.XtraEditors.TextEdit();
            this.cmbPredispensingMode = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sesePredispensingOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sesePredispensingOffsetX.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sePredispensingIntervelSeconds.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sePredispensingIntervelMinutes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sePredispensingTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teAbsoluteMoveTarget.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelStepInfo);
            this.panelControl1.Location = new System.Drawing.Point(10, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(338, 42);
            this.panelControl1.TabIndex = 38;
            // 
            // labelStepInfo
            // 
            this.labelStepInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepInfo.Appearance.Options.UseFont = true;
            this.labelStepInfo.Location = new System.Drawing.Point(5, 7);
            this.labelStepInfo.Name = "labelStepInfo";
            this.labelStepInfo.Size = new System.Drawing.Size(170, 19);
            this.labelStepInfo.TabIndex = 4;
            this.labelStepInfo.Text = "步骤 1/3：画胶器设置";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // repositoryItemComboBox5
            // 
            this.repositoryItemComboBox5.AutoHeight = false;
            this.repositoryItemComboBox5.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox5.Name = "repositoryItemComboBox5";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupBox2);
            this.panelControl2.Controls.Add(this.groupBox1);
            this.panelControl2.Controls.Add(this.sePredispensingTimes);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.teAbsoluteMoveTarget);
            this.panelControl2.Controls.Add(this.cmbPredispensingMode);
            this.panelControl2.Location = new System.Drawing.Point(10, 53);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(338, 565);
            this.panelControl2.TabIndex = 41;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sesePredispensingOffsetY);
            this.groupBox2.Controls.Add(this.labelControl6);
            this.groupBox2.Controls.Add(this.sesePredispensingOffsetX);
            this.groupBox2.Controls.Add(this.labelControl7);
            this.groupBox2.Controls.Add(this.labelControl8);
            this.groupBox2.Controls.Add(this.labelControl9);
            this.groupBox2.Location = new System.Drawing.Point(19, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 119);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "预划胶偏移";
            // 
            // sesePredispensingOffsetY
            // 
            this.sesePredispensingOffsetY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sesePredispensingOffsetY.Location = new System.Drawing.Point(86, 68);
            this.sesePredispensingOffsetY.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.sesePredispensingOffsetY.Name = "sesePredispensingOffsetY";
            this.sesePredispensingOffsetY.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.sesePredispensingOffsetY.Properties.AutoHeight = false;
            this.sesePredispensingOffsetY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sesePredispensingOffsetY.Properties.DisplayFormat.FormatString = "0.000";
            this.sesePredispensingOffsetY.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sesePredispensingOffsetY.Properties.EditFormat.FormatString = "0.000";
            this.sesePredispensingOffsetY.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sesePredispensingOffsetY.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sesePredispensingOffsetY.Properties.MaskSettings.Set("mask", "f3");
            this.sesePredispensingOffsetY.Size = new System.Drawing.Size(94, 23);
            this.sesePredispensingOffsetY.TabIndex = 17;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(67, 72);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(12, 14);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "Y:";
            // 
            // sesePredispensingOffsetX
            // 
            this.sesePredispensingOffsetX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sesePredispensingOffsetX.Location = new System.Drawing.Point(87, 33);
            this.sesePredispensingOffsetX.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.sesePredispensingOffsetX.Name = "sesePredispensingOffsetX";
            this.sesePredispensingOffsetX.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.sesePredispensingOffsetX.Properties.AutoHeight = false;
            this.sesePredispensingOffsetX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sesePredispensingOffsetX.Properties.DisplayFormat.FormatString = "0.000";
            this.sesePredispensingOffsetX.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sesePredispensingOffsetX.Properties.EditFormat.FormatString = "0.000";
            this.sesePredispensingOffsetX.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sesePredispensingOffsetX.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sesePredispensingOffsetX.Properties.MaskSettings.Set("mask", "f3");
            this.sesePredispensingOffsetX.Size = new System.Drawing.Size(94, 23);
            this.sesePredispensingOffsetX.TabIndex = 17;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(187, 72);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(20, 14);
            this.labelControl7.TabIndex = 14;
            this.labelControl7.Text = "mm";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(187, 37);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(20, 14);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "mm";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(69, 37);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(11, 14);
            this.labelControl9.TabIndex = 14;
            this.labelControl9.Text = "X:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelControl5);
            this.groupBox1.Controls.Add(this.sePredispensingIntervelSeconds);
            this.groupBox1.Controls.Add(this.sePredispensingIntervelMinutes);
            this.groupBox1.Controls.Add(this.labelControl4);
            this.groupBox1.Location = new System.Drawing.Point(19, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 115);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预划胶时间间隔";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(187, 72);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(12, 14);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "秒";
            // 
            // sePredispensingIntervelSeconds
            // 
            this.sePredispensingIntervelSeconds.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sePredispensingIntervelSeconds.Location = new System.Drawing.Point(86, 68);
            this.sePredispensingIntervelSeconds.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.sePredispensingIntervelSeconds.Name = "sePredispensingIntervelSeconds";
            this.sePredispensingIntervelSeconds.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.sePredispensingIntervelSeconds.Properties.AutoHeight = false;
            this.sePredispensingIntervelSeconds.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sePredispensingIntervelSeconds.Properties.DisplayFormat.FormatString = "0";
            this.sePredispensingIntervelSeconds.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePredispensingIntervelSeconds.Properties.EditFormat.FormatString = "0";
            this.sePredispensingIntervelSeconds.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePredispensingIntervelSeconds.Properties.IsFloatValue = false;
            this.sePredispensingIntervelSeconds.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.sePredispensingIntervelSeconds.Properties.MaskSettings.Set("allowBlankInput", true);
            this.sePredispensingIntervelSeconds.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.sePredispensingIntervelSeconds.Properties.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.sePredispensingIntervelSeconds.Size = new System.Drawing.Size(94, 23);
            this.sePredispensingIntervelSeconds.TabIndex = 15;
            // 
            // sePredispensingIntervelMinutes
            // 
            this.sePredispensingIntervelMinutes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sePredispensingIntervelMinutes.Location = new System.Drawing.Point(87, 32);
            this.sePredispensingIntervelMinutes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.sePredispensingIntervelMinutes.Name = "sePredispensingIntervelMinutes";
            this.sePredispensingIntervelMinutes.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.sePredispensingIntervelMinutes.Properties.AutoHeight = false;
            this.sePredispensingIntervelMinutes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sePredispensingIntervelMinutes.Properties.DisplayFormat.FormatString = "0";
            this.sePredispensingIntervelMinutes.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePredispensingIntervelMinutes.Properties.EditFormat.FormatString = "0";
            this.sePredispensingIntervelMinutes.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePredispensingIntervelMinutes.Properties.IsFloatValue = false;
            this.sePredispensingIntervelMinutes.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.sePredispensingIntervelMinutes.Properties.MaskSettings.Set("allowBlankInput", true);
            this.sePredispensingIntervelMinutes.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.sePredispensingIntervelMinutes.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sePredispensingIntervelMinutes.Size = new System.Drawing.Size(94, 23);
            this.sePredispensingIntervelMinutes.TabIndex = 15;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(187, 37);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 14;
            this.labelControl4.Text = "分钟";
            // 
            // sePredispensingTimes
            // 
            this.sePredispensingTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sePredispensingTimes.Location = new System.Drawing.Point(91, 63);
            this.sePredispensingTimes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.sePredispensingTimes.Name = "sePredispensingTimes";
            this.sePredispensingTimes.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.sePredispensingTimes.Properties.AutoHeight = false;
            this.sePredispensingTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sePredispensingTimes.Properties.DisplayFormat.FormatString = "0";
            this.sePredispensingTimes.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePredispensingTimes.Properties.EditFormat.FormatString = "0";
            this.sePredispensingTimes.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.sePredispensingTimes.Properties.IsFloatValue = false;
            this.sePredispensingTimes.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.sePredispensingTimes.Properties.MaskSettings.Set("allowBlankInput", true);
            this.sePredispensingTimes.Properties.MaskSettings.Set("mask", "\\d{1,3}?");
            this.sePredispensingTimes.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sePredispensingTimes.Size = new System.Drawing.Size(94, 23);
            this.sePredispensingTimes.TabIndex = 15;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 67);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(64, 14);
            this.labelControl2.TabIndex = 14;
            this.labelControl2.Text = "预划胶次数:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 28);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 14;
            this.labelControl1.Text = "预划胶方式:";
            // 
            // teAbsoluteMoveTarget
            // 
            this.teAbsoluteMoveTarget.Location = new System.Drawing.Point(91, 442);
            this.teAbsoluteMoveTarget.Name = "teAbsoluteMoveTarget";
            this.teAbsoluteMoveTarget.Size = new System.Drawing.Size(94, 20);
            this.teAbsoluteMoveTarget.TabIndex = 16;
            // 
            // cmbPredispensingMode
            // 
            this.cmbPredispensingMode.FormattingEnabled = true;
            this.cmbPredispensingMode.Location = new System.Drawing.Point(91, 25);
            this.cmbPredispensingMode.Name = "cmbPredispensingMode";
            this.cmbPredispensingMode.Size = new System.Drawing.Size(94, 22);
            this.cmbPredispensingMode.TabIndex = 13;
            this.cmbPredispensingMode.Text = "Off";
            // 
            // DispenserSettingsStep_DispenserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "DispenserSettingsStep_DispenserInfo";
            this.Size = new System.Drawing.Size(359, 629);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sesePredispensingOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sesePredispensingOffsetX.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sePredispensingIntervelSeconds.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sePredispensingIntervelMinutes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sePredispensingTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teAbsoluteMoveTarget.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelStepInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox5;
        private PanelControl panelControl2;
        private SpinEdit sePredispensingIntervelMinutes;
        private LabelControl labelControl1;
        private TextEdit teAbsoluteMoveTarget;
        private System.Windows.Forms.ComboBox cmbPredispensingMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private SpinEdit sesePredispensingOffsetY;
        private LabelControl labelControl6;
        private SpinEdit sesePredispensingOffsetX;
        private LabelControl labelControl7;
        private LabelControl labelControl8;
        private LabelControl labelControl9;
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelControl labelControl5;
        private SpinEdit sePredispensingIntervelSeconds;
        private LabelControl labelControl4;
        private SpinEdit sePredispensingTimes;
        private LabelControl labelControl2;
    }
}
