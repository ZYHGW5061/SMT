
namespace ControlPanelClsLib
{
    partial class EjectionSystemTool_Height
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
            this.btnESUpDown = new System.Windows.Forms.Button();
            this.btnVaccumOnOff = new System.Windows.Forms.Button();
            this.btnNeedleGoZero = new System.Windows.Forms.Button();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "用激光测距仪定位顶针座的高度";
            // 
            // btnESUpDown
            // 
            this.btnESUpDown.Location = new System.Drawing.Point(904, 404);
            this.btnESUpDown.Name = "btnESUpDown";
            this.btnESUpDown.Size = new System.Drawing.Size(216, 23);
            this.btnESUpDown.TabIndex = 2;
            this.btnESUpDown.Text = "顶针座  升/降";
            this.btnESUpDown.UseVisualStyleBackColor = true;
            this.btnESUpDown.Click += new System.EventHandler(this.btnESUpDown_Click);
            // 
            // btnVaccumOnOff
            // 
            this.btnVaccumOnOff.Location = new System.Drawing.Point(904, 444);
            this.btnVaccumOnOff.Name = "btnVaccumOnOff";
            this.btnVaccumOnOff.Size = new System.Drawing.Size(216, 23);
            this.btnVaccumOnOff.TabIndex = 2;
            this.btnVaccumOnOff.Text = "真空 开/关";
            this.btnVaccumOnOff.UseVisualStyleBackColor = true;
            this.btnVaccumOnOff.Click += new System.EventHandler(this.btnVaccumOnOff_Click);
            // 
            // btnNeedleGoZero
            // 
            this.btnNeedleGoZero.Location = new System.Drawing.Point(904, 486);
            this.btnNeedleGoZero.Name = "btnNeedleGoZero";
            this.btnNeedleGoZero.Size = new System.Drawing.Size(216, 23);
            this.btnNeedleGoZero.TabIndex = 2;
            this.btnNeedleGoZero.Text = "针头回初始原点";
            this.btnNeedleGoZero.UseVisualStyleBackColor = true;
            this.btnNeedleGoZero.Click += new System.EventHandler(this.btnNeedleGoZero_Click);
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(872, 64);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(267, 317);
            this.stageQuickMove1.TabIndex = 4;
            // 
            // panelControlCameraAera
            // 
            this.panelControlCameraAera.Location = new System.Drawing.Point(59, 64);
            this.panelControlCameraAera.Name = "panelControlCameraAera";
            this.panelControlCameraAera.Size = new System.Drawing.Size(711, 460);
            this.panelControlCameraAera.TabIndex = 51;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(1000, 547);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 30);
            this.btnOK.TabIndex = 52;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // EjectionSystemTool_Height
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panelControlCameraAera);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.btnNeedleGoZero);
            this.Controls.Add(this.btnVaccumOnOff);
            this.Controls.Add(this.btnESUpDown);
            this.Controls.Add(this.label1);
            this.Name = "EjectionSystemTool_Height";
            this.Size = new System.Drawing.Size(1217, 600);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private System.Windows.Forms.Button btnESUpDown;
        private System.Windows.Forms.Button btnVaccumOnOff;
        private System.Windows.Forms.Button btnNeedleGoZero;
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private System.Windows.Forms.Button btnOK;
    }
}
