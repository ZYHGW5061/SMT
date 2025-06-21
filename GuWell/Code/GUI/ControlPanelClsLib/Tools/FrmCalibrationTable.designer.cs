namespace ControlPanelClsLib
{
    partial class FrmCalibrationTable
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCalibrationTable));
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.lightControl1 = new LightSourceCtrlPanelLib.LightControl();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.btnSetPos = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlCameraAera
            // 
            this.panelControlCameraAera.Location = new System.Drawing.Point(12, 12);
            this.panelControlCameraAera.Name = "panelControlCameraAera";
            this.panelControlCameraAera.Size = new System.Drawing.Size(586, 516);
            this.panelControlCameraAera.TabIndex = 15;
            // 
            // lightControl1
            // 
            this.lightControl1.Location = new System.Drawing.Point(604, 12);
            this.lightControl1.Name = "lightControl1";
            this.lightControl1.Size = new System.Drawing.Size(332, 164);
            this.lightControl1.TabIndex = 22;
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(629, 211);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 20;
            // 
            // btnSetPos
            // 
            this.btnSetPos.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSetPos.ImageOptions.SvgImage")));
            this.btnSetPos.Location = new System.Drawing.Point(708, 571);
            this.btnSetPos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSetPos.Name = "btnSetPos";
            this.btnSetPos.Size = new System.Drawing.Size(139, 42);
            this.btnSetPos.TabIndex = 23;
            this.btnSetPos.Text = "位置确认";
            this.btnSetPos.Click += new System.EventHandler(this.btnSetPos_Click);
            // 
            // FrmCalibrationTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(951, 648);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.btnSetPos);
            this.Controls.Add(this.lightControl1);
            this.Controls.Add(this.panelControlCameraAera);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmCalibrationTable.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmCalibrationTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "校准台操作";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSingleStepRun_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private LightSourceCtrlPanelLib.LightControl lightControl1;
        private DevExpress.XtraEditors.SimpleButton btnSetPos;
    }
}
