
namespace ControlPanelClsLib
{
    partial class EjectionSystemTool_XYMeasuring
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAutoFocus = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInitializeNeedle = new System.Windows.Forms.Button();
            this.btnESUpDown = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.ctrlLight1 = new LightSourceCtrlPanelLib.CtrlLight();
            this.ctrlLight2 = new LightSourceCtrlPanelLib.CtrlLight();
            this.btnNeedleGoZero = new System.Windows.Forms.Button();
            this.btnVaccumOnOff = new System.Windows.Forms.Button();
            this.stageQuickMove2 = new StageCtrlPanelLib.StageQuickMove();
            this.seNeedlePixelCoorX = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.seNeedlePixelCoorY = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seNeedlePixelCoorX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seNeedlePixelCoorY.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "用Wafer相机确认顶针系统的中心的位置";
            // 
            // btnAutoFocus
            // 
            this.btnAutoFocus.Location = new System.Drawing.Point(679, 541);
            this.btnAutoFocus.Name = "btnAutoFocus";
            this.btnAutoFocus.Size = new System.Drawing.Size(216, 23);
            this.btnAutoFocus.TabIndex = 2;
            this.btnAutoFocus.Text = "自动聚焦";
            this.btnAutoFocus.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(18, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(614, 591);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机";
            // 
            // btnInitializeNeedle
            // 
            this.btnInitializeNeedle.Location = new System.Drawing.Point(679, 570);
            this.btnInitializeNeedle.Name = "btnInitializeNeedle";
            this.btnInitializeNeedle.Size = new System.Drawing.Size(216, 23);
            this.btnInitializeNeedle.TabIndex = 3;
            this.btnInitializeNeedle.Text = "针头回初始原点";
            this.btnInitializeNeedle.UseVisualStyleBackColor = true;
            // 
            // btnESUpDown
            // 
            this.btnESUpDown.Location = new System.Drawing.Point(679, 512);
            this.btnESUpDown.Name = "btnESUpDown";
            this.btnESUpDown.Size = new System.Drawing.Size(216, 23);
            this.btnESUpDown.TabIndex = 4;
            this.btnESUpDown.Text = "顶针座  升/降";
            this.btnESUpDown.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(820, 614);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(679, 614);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 6;
            this.btnDone.Text = "完成";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(15, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "确认针头的XY零点位置";
            // 
            // panelControlCameraAera
            // 
            this.panelControlCameraAera.Location = new System.Drawing.Point(22, 64);
            this.panelControlCameraAera.Name = "panelControlCameraAera";
            this.panelControlCameraAera.Size = new System.Drawing.Size(711, 472);
            this.panelControlCameraAera.TabIndex = 51;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.ctrlLight1);
            this.panelControl2.Controls.Add(this.ctrlLight2);
            this.panelControl2.Location = new System.Drawing.Point(759, 64);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(267, 149);
            this.panelControl2.TabIndex = 57;
            // 
            // ctrlLight1
            // 
            this.ctrlLight1.ApplyIntensityToHardware = true;
            this.ctrlLight1.BrightFieldBrightnessChanged = null;
            this.ctrlLight1.Brightness = 0F;
            this.ctrlLight1.CurrentLightType = GlobalDataDefineClsLib.EnumLightSourceType.WaferDirectField;
            this.ctrlLight1.Location = new System.Drawing.Point(8, 6);
            this.ctrlLight1.Name = "ctrlLight1";
            this.ctrlLight1.Size = new System.Drawing.Size(246, 56);
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
            this.ctrlLight2.Size = new System.Drawing.Size(246, 56);
            this.ctrlLight2.TabIndex = 39;
            // 
            // btnNeedleGoZero
            // 
            this.btnNeedleGoZero.Location = new System.Drawing.Point(1040, 343);
            this.btnNeedleGoZero.Name = "btnNeedleGoZero";
            this.btnNeedleGoZero.Size = new System.Drawing.Size(113, 23);
            this.btnNeedleGoZero.TabIndex = 54;
            this.btnNeedleGoZero.Text = "针头回初始原点";
            this.btnNeedleGoZero.UseVisualStyleBackColor = true;
            this.btnNeedleGoZero.Click += new System.EventHandler(this.btnNeedleGoZero_Click);
            // 
            // btnVaccumOnOff
            // 
            this.btnVaccumOnOff.Location = new System.Drawing.Point(1040, 301);
            this.btnVaccumOnOff.Name = "btnVaccumOnOff";
            this.btnVaccumOnOff.Size = new System.Drawing.Size(113, 23);
            this.btnVaccumOnOff.TabIndex = 55;
            this.btnVaccumOnOff.Text = "真空 开/关";
            this.btnVaccumOnOff.UseVisualStyleBackColor = true;
            this.btnVaccumOnOff.Click += new System.EventHandler(this.btnVaccumOnOff_Click);
            // 
            // stageQuickMove2
            // 
            this.stageQuickMove2.Location = new System.Drawing.Point(759, 220);
            this.stageQuickMove2.Name = "stageQuickMove2";
            this.stageQuickMove2.PositiveQucikMoveAct = null;
            this.stageQuickMove2.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove2.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove2.Size = new System.Drawing.Size(267, 317);
            this.stageQuickMove2.TabIndex = 58;
            // 
            // seNeedlePixelCoorX
            // 
            this.seNeedlePixelCoorX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seNeedlePixelCoorX.Location = new System.Drawing.Point(1062, 423);
            this.seNeedlePixelCoorX.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seNeedlePixelCoorX.Name = "seNeedlePixelCoorX";
            this.seNeedlePixelCoorX.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seNeedlePixelCoorX.Properties.AutoHeight = false;
            this.seNeedlePixelCoorX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seNeedlePixelCoorX.Properties.DisplayFormat.FormatString = "0.0";
            this.seNeedlePixelCoorX.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seNeedlePixelCoorX.Properties.EditFormat.FormatString = "0.0";
            this.seNeedlePixelCoorX.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seNeedlePixelCoorX.Properties.IsFloatValue = false;
            this.seNeedlePixelCoorX.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.seNeedlePixelCoorX.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.seNeedlePixelCoorX.Properties.MaxValue = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.seNeedlePixelCoorX.Size = new System.Drawing.Size(80, 24);
            this.seNeedlePixelCoorX.TabIndex = 59;
            this.seNeedlePixelCoorX.EditValueChanged += new System.EventHandler(this.seMoveDistance_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(1042, 427);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(11, 14);
            this.labelControl1.TabIndex = 60;
            this.labelControl1.Text = "X:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(1042, 477);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 61;
            this.labelControl2.Text = "Y:";
            // 
            // seNeedlePixelCoorY
            // 
            this.seNeedlePixelCoorY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seNeedlePixelCoorY.Location = new System.Drawing.Point(1063, 472);
            this.seNeedlePixelCoorY.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seNeedlePixelCoorY.Name = "seNeedlePixelCoorY";
            this.seNeedlePixelCoorY.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seNeedlePixelCoorY.Properties.AutoHeight = false;
            this.seNeedlePixelCoorY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seNeedlePixelCoorY.Properties.DisplayFormat.FormatString = "0.0";
            this.seNeedlePixelCoorY.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seNeedlePixelCoorY.Properties.EditFormat.FormatString = "0.0";
            this.seNeedlePixelCoorY.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seNeedlePixelCoorY.Properties.IsFloatValue = false;
            this.seNeedlePixelCoorY.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.seNeedlePixelCoorY.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.seNeedlePixelCoorY.Properties.MaxValue = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.seNeedlePixelCoorY.Size = new System.Drawing.Size(80, 24);
            this.seNeedlePixelCoorY.TabIndex = 59;
            this.seNeedlePixelCoorY.EditValueChanged += new System.EventHandler(this.seMoveDistance_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(1148, 427);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 60;
            this.labelControl3.Text = "像素";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(1148, 477);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 60;
            this.labelControl4.Text = "像素";
            // 
            // EjectionSystemTool_XYMeasuring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.seNeedlePixelCoorY);
            this.Controls.Add(this.seNeedlePixelCoorX);
            this.Controls.Add(this.stageQuickMove2);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.btnNeedleGoZero);
            this.Controls.Add(this.btnVaccumOnOff);
            this.Controls.Add(this.panelControlCameraAera);
            this.Controls.Add(this.label2);
            this.Name = "EjectionSystemTool_XYMeasuring";
            this.Size = new System.Drawing.Size(1177, 567);
            this.Load += new System.EventHandler(this.EjectionSystemTool_XYMeasuring_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seNeedlePixelCoorX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seNeedlePixelCoorY.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private System.Windows.Forms.Button btnAutoFocus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnInitializeNeedle;
        private System.Windows.Forms.Button btnESUpDown;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight1;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight2;
        private System.Windows.Forms.Button btnNeedleGoZero;
        private System.Windows.Forms.Button btnVaccumOnOff;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove2;
        private DevExpress.XtraEditors.SpinEdit seNeedlePixelCoorX;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit seNeedlePixelCoorY;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}
