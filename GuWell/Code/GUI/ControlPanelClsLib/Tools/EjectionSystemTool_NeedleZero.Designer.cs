
namespace ControlPanelClsLib
{
    partial class EjectionSystemTool_NeedleZero
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
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.btnNeedleGoZero = new System.Windows.Forms.Button();
            this.btnVaccumOnOff = new System.Windows.Forms.Button();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.ctrlLight1 = new LightSourceCtrlPanelLib.CtrlLight();
            this.ctrlLight2 = new LightSourceCtrlPanelLib.CtrlLight();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "确认针头的Z零点位置";
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(759, 220);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(267, 316);
            this.stageQuickMove1.TabIndex = 4;
            // 
            // panelControlCameraAera
            // 
            this.panelControlCameraAera.Location = new System.Drawing.Point(22, 64);
            this.panelControlCameraAera.Name = "panelControlCameraAera";
            this.panelControlCameraAera.Size = new System.Drawing.Size(711, 472);
            this.panelControlCameraAera.TabIndex = 51;
            // 
            // btnNeedleGoZero
            // 
            this.btnNeedleGoZero.Location = new System.Drawing.Point(1040, 466);
            this.btnNeedleGoZero.Name = "btnNeedleGoZero";
            this.btnNeedleGoZero.Size = new System.Drawing.Size(116, 23);
            this.btnNeedleGoZero.TabIndex = 14;
            this.btnNeedleGoZero.Text = "针头回初始原点";
            this.btnNeedleGoZero.UseVisualStyleBackColor = true;
            this.btnNeedleGoZero.Click += new System.EventHandler(this.btnNeedleGoZero_Click);
            // 
            // btnVaccumOnOff
            // 
            this.btnVaccumOnOff.Location = new System.Drawing.Point(1040, 424);
            this.btnVaccumOnOff.Name = "btnVaccumOnOff";
            this.btnVaccumOnOff.Size = new System.Drawing.Size(116, 23);
            this.btnVaccumOnOff.TabIndex = 15;
            this.btnVaccumOnOff.Text = "真空 开/关";
            this.btnVaccumOnOff.UseVisualStyleBackColor = true;
            this.btnVaccumOnOff.Click += new System.EventHandler(this.btnVaccumOnOff_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.ctrlLight1);
            this.panelControl2.Controls.Add(this.ctrlLight2);
            this.panelControl2.Location = new System.Drawing.Point(759, 64);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(267, 149);
            this.panelControl2.TabIndex = 58;
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
            // EjectionSystemTool_NeedleZero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.btnNeedleGoZero);
            this.Controls.Add(this.panelControlCameraAera);
            this.Controls.Add(this.btnVaccumOnOff);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.label1);
            this.Name = "EjectionSystemTool_NeedleZero";
            this.Size = new System.Drawing.Size(1177, 567);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private System.Windows.Forms.Button btnNeedleGoZero;
        private System.Windows.Forms.Button btnVaccumOnOff;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight1;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight2;
    }
}
