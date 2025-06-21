
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;

namespace RecipeEditPanelClsLib
{
    partial class BondPositionStep_SetBondPosition
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.ctrlLight1 = new LightSourceCtrlPanelLib.CtrlLight();
            this.ctrlLight2 = new LightSourceCtrlPanelLib.CtrlLight();
            this.btnConfirmPos = new System.Windows.Forms.Button();
            this.seBPRotateTheta = new DevExpress.XtraEditors.SpinEdit();
            this.seBPCompensationY = new DevExpress.XtraEditors.SpinEdit();
            this.seBPCompensationT = new DevExpress.XtraEditors.SpinEdit();
            this.seBPCompensationX = new DevExpress.XtraEditors.SpinEdit();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seBPRotateTheta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBPCompensationY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBPCompensationT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBPCompensationX.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelStepInfo);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(353, 42);
            this.panelControl1.TabIndex = 38;
            // 
            // labelStepInfo
            // 
            this.labelStepInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepInfo.Appearance.Options.UseFont = true;
            this.labelStepInfo.Location = new System.Drawing.Point(5, 11);
            this.labelStepInfo.Name = "labelStepInfo";
            this.labelStepInfo.Size = new System.Drawing.Size(238, 19);
            this.labelStepInfo.TabIndex = 4;
            this.labelStepInfo.Text = "步骤 7/7：确定贴装位置和角度";
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.503607F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.49639F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(359, 647);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Controls.Add(this.btnConfirmPos);
            this.panelControl2.Controls.Add(this.seBPRotateTheta);
            this.panelControl2.Controls.Add(this.seBPCompensationY);
            this.panelControl2.Controls.Add(this.seBPCompensationT);
            this.panelControl2.Controls.Add(this.seBPCompensationX);
            this.panelControl2.Controls.Add(this.stageQuickMove1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 51);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(353, 593);
            this.panelControl2.TabIndex = 39;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(207, 508);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(56, 14);
            this.labelControl4.TabIndex = 40;
            this.labelControl4.Text = "偏转角(°):";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(30, 559);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(76, 14);
            this.labelControl2.TabIndex = 40;
            this.labelControl2.Text = "位置补偿T(°):";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 533);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 14);
            this.labelControl1.TabIndex = 40;
            this.labelControl1.Text = "位置补偿Y(mm):";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(17, 508);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(89, 14);
            this.labelControl3.TabIndex = 40;
            this.labelControl3.Text = "位置补偿X(mm):";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.ctrlLight1);
            this.panelControl3.Controls.Add(this.ctrlLight2);
            this.panelControl3.Location = new System.Drawing.Point(5, 5);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(338, 149);
            this.panelControl3.TabIndex = 41;
            // 
            // ctrlLight1
            // 
            this.ctrlLight1.ApplyIntensityToHardware = true;
            this.ctrlLight1.BrightFieldBrightnessChanged = null;
            this.ctrlLight1.Brightness = 0F;
            this.ctrlLight1.CurrentLightType = GlobalDataDefineClsLib.EnumLightSourceType.WaferRingField;
            this.ctrlLight1.Location = new System.Drawing.Point(8, 6);
            this.ctrlLight1.Name = "ctrlLight1";
            this.ctrlLight1.Size = new System.Drawing.Size(325, 56);
            this.ctrlLight1.TabIndex = 39;
            // 
            // ctrlLight2
            // 
            this.ctrlLight2.ApplyIntensityToHardware = true;
            this.ctrlLight2.BrightFieldBrightnessChanged = null;
            this.ctrlLight2.Brightness = 0F;
            this.ctrlLight2.CurrentLightType = GlobalDataDefineClsLib.EnumLightSourceType.WaferRingField;
            this.ctrlLight2.Location = new System.Drawing.Point(8, 85);
            this.ctrlLight2.Name = "ctrlLight2";
            this.ctrlLight2.Size = new System.Drawing.Size(325, 56);
            this.ctrlLight2.TabIndex = 39;
            // 
            // btnConfirmPos
            // 
            this.btnConfirmPos.Location = new System.Drawing.Point(277, 551);
            this.btnConfirmPos.Name = "btnConfirmPos";
            this.btnConfirmPos.Size = new System.Drawing.Size(61, 30);
            this.btnConfirmPos.TabIndex = 40;
            this.btnConfirmPos.Text = "确认";
            this.btnConfirmPos.UseVisualStyleBackColor = true;
            this.btnConfirmPos.Click += new System.EventHandler(this.btnConfirmPos_Click);
            // 
            // seBPRotateTheta
            // 
            this.seBPRotateTheta.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBPRotateTheta.Location = new System.Drawing.Point(267, 505);
            this.seBPRotateTheta.Name = "seBPRotateTheta";
            this.seBPRotateTheta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBPRotateTheta.Properties.DisplayFormat.FormatString = "0.000";
            this.seBPRotateTheta.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPRotateTheta.Properties.EditFormat.FormatString = "0.000";
            this.seBPRotateTheta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPRotateTheta.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.seBPRotateTheta.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seBPRotateTheta.Properties.MaskSettings.Set("mask", "f3");
            this.seBPRotateTheta.Properties.MaxValue = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.seBPRotateTheta.Properties.MinValue = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.seBPRotateTheta.Size = new System.Drawing.Size(71, 20);
            this.seBPRotateTheta.TabIndex = 12;
            // 
            // seBPCompensationY
            // 
            this.seBPCompensationY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBPCompensationY.Location = new System.Drawing.Point(111, 530);
            this.seBPCompensationY.Name = "seBPCompensationY";
            this.seBPCompensationY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBPCompensationY.Properties.DisplayFormat.FormatString = "0.000";
            this.seBPCompensationY.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPCompensationY.Properties.EditFormat.FormatString = "0.000";
            this.seBPCompensationY.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPCompensationY.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seBPCompensationY.Properties.MaskSettings.Set("mask", "f3");
            this.seBPCompensationY.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seBPCompensationY.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seBPCompensationY.Size = new System.Drawing.Size(76, 20);
            this.seBPCompensationY.TabIndex = 12;
            // 
            // seBPCompensationT
            // 
            this.seBPCompensationT.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBPCompensationT.Location = new System.Drawing.Point(111, 556);
            this.seBPCompensationT.Name = "seBPCompensationT";
            this.seBPCompensationT.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBPCompensationT.Properties.DisplayFormat.FormatString = "0.000";
            this.seBPCompensationT.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPCompensationT.Properties.EditFormat.FormatString = "0.000";
            this.seBPCompensationT.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPCompensationT.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seBPCompensationT.Properties.MaskSettings.Set("mask", "f3");
            this.seBPCompensationT.Properties.MaxValue = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.seBPCompensationT.Properties.MinValue = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.seBPCompensationT.Size = new System.Drawing.Size(76, 20);
            this.seBPCompensationT.TabIndex = 12;
            // 
            // seBPCompensationX
            // 
            this.seBPCompensationX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBPCompensationX.Location = new System.Drawing.Point(111, 505);
            this.seBPCompensationX.Name = "seBPCompensationX";
            this.seBPCompensationX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBPCompensationX.Properties.DisplayFormat.FormatString = "0.000";
            this.seBPCompensationX.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPCompensationX.Properties.EditFormat.FormatString = "0.000";
            this.seBPCompensationX.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBPCompensationX.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seBPCompensationX.Properties.MaskSettings.Set("mask", "f3");
            this.seBPCompensationX.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seBPCompensationX.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seBPCompensationX.Size = new System.Drawing.Size(76, 20);
            this.seBPCompensationX.TabIndex = 12;
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(38, 173);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 0;
            // 
            // BondPositionStep_SetBondPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BondPositionStep_SetBondPosition";
            this.Size = new System.Drawing.Size(359, 647);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seBPRotateTheta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBPCompensationY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBPCompensationT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBPCompensationX.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelStepInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private PanelControl panelControl2;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private SpinEdit seBPRotateTheta;
        private SpinEdit seBPCompensationY;
        private SpinEdit seBPCompensationX;
        private System.Windows.Forms.Button btnConfirmPos;
        private PanelControl panelControl3;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight1;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight2;
        private SpinEdit seBPCompensationT;
        private LabelControl labelControl3;
        private LabelControl labelControl4;
        private LabelControl labelControl2;
        private LabelControl labelControl1;
    }
}
