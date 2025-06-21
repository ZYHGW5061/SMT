
namespace ControlPanelClsLib
{
    partial class PPESHeight
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
            this.btnVaccumOnOff = new System.Windows.Forms.Button();
            this.btnNeedleGoZero = new System.Windows.Forms.Button();
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.btnConfirmHeight = new System.Windows.Forms.Button();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.ctrlLight1 = new LightSourceCtrlPanelLib.CtrlLight();
            this.ctrlLight2 = new LightSourceCtrlPanelLib.CtrlLight();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartMeasureZ = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "用激光测距仪定位顶针座的高度";
            // 
            // btnVaccumOnOff
            // 
            this.btnVaccumOnOff.Location = new System.Drawing.Point(1041, 378);
            this.btnVaccumOnOff.Name = "btnVaccumOnOff";
            this.btnVaccumOnOff.Size = new System.Drawing.Size(113, 23);
            this.btnVaccumOnOff.TabIndex = 2;
            this.btnVaccumOnOff.Text = "真空 开/关";
            this.btnVaccumOnOff.UseVisualStyleBackColor = true;
            this.btnVaccumOnOff.Click += new System.EventHandler(this.btnVaccumOnOff_Click);
            // 
            // btnNeedleGoZero
            // 
            this.btnNeedleGoZero.Location = new System.Drawing.Point(1041, 420);
            this.btnNeedleGoZero.Name = "btnNeedleGoZero";
            this.btnNeedleGoZero.Size = new System.Drawing.Size(113, 23);
            this.btnNeedleGoZero.TabIndex = 2;
            this.btnNeedleGoZero.Text = "针头回初始原点";
            this.btnNeedleGoZero.UseVisualStyleBackColor = true;
            this.btnNeedleGoZero.Click += new System.EventHandler(this.btnNeedleGoZero_Click);
            // 
            // panelControlCameraAera
            // 
            this.panelControlCameraAera.Location = new System.Drawing.Point(20, 51);
            this.panelControlCameraAera.Name = "panelControlCameraAera";
            this.panelControlCameraAera.Size = new System.Drawing.Size(711, 485);
            this.panelControlCameraAera.TabIndex = 51;
            // 
            // btnConfirmHeight
            // 
            this.btnConfirmHeight.Location = new System.Drawing.Point(1041, 503);
            this.btnConfirmHeight.Name = "btnConfirmHeight";
            this.btnConfirmHeight.Size = new System.Drawing.Size(113, 23);
            this.btnConfirmHeight.TabIndex = 2;
            this.btnConfirmHeight.Text = "确认高度";
            this.btnConfirmHeight.UseVisualStyleBackColor = true;
            this.btnConfirmHeight.Click += new System.EventHandler(this.btnConfirmHeight_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.ctrlLight1);
            this.panelControl2.Controls.Add(this.ctrlLight2);
            this.panelControl2.Location = new System.Drawing.Point(753, 51);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(267, 149);
            this.panelControl2.TabIndex = 52;
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
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(753, 219);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(267, 317);
            this.stageQuickMove1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(267, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(341, 16);
            this.label2.TabIndex = 53;
            this.label2.Text = "--调整榜头Z轴使激光测距仪处于量程范围内";
            // 
            // btnStartMeasureZ
            // 
            this.btnStartMeasureZ.Location = new System.Drawing.Point(1041, 464);
            this.btnStartMeasureZ.Name = "btnStartMeasureZ";
            this.btnStartMeasureZ.Size = new System.Drawing.Size(113, 23);
            this.btnStartMeasureZ.TabIndex = 2;
            this.btnStartMeasureZ.Text = "开始测高";
            this.btnStartMeasureZ.UseVisualStyleBackColor = true;
            this.btnStartMeasureZ.Visible = false;
            this.btnStartMeasureZ.Click += new System.EventHandler(this.btnStartMeasureZ_Click);
            // 
            // PPESHeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControlCameraAera);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.btnStartMeasureZ);
            this.Controls.Add(this.btnConfirmHeight);
            this.Controls.Add(this.btnNeedleGoZero);
            this.Controls.Add(this.btnVaccumOnOff);
            this.Controls.Add(this.label1);
            this.Name = "PPESHeight";
            this.Size = new System.Drawing.Size(1172, 567);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private System.Windows.Forms.Button btnVaccumOnOff;
        private System.Windows.Forms.Button btnNeedleGoZero;
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private System.Windows.Forms.Button btnConfirmHeight;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight1;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStartMeasureZ;
    }
}
