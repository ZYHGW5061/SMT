
namespace ControlPanelClsLib
{
    partial class PPTool_Alignment
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
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.panelStepOperate = new DevExpress.XtraEditors.PanelControl();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.step2Sign = new System.Windows.Forms.PictureBox();
            this.step1Sign = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelStepOperate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.step2Sign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.step1Sign)).BeginInit();
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
            // panelControlCameraAera
            // 
            this.panelControlCameraAera.Location = new System.Drawing.Point(18, 13);
            this.panelControlCameraAera.Name = "panelControlCameraAera";
            this.panelControlCameraAera.Size = new System.Drawing.Size(711, 460);
            this.panelControlCameraAera.TabIndex = 50;
            // 
            // panelStepOperate
            // 
            this.panelStepOperate.Location = new System.Drawing.Point(813, 13);
            this.panelStepOperate.Name = "panelStepOperate";
            this.panelStepOperate.Size = new System.Drawing.Size(330, 510);
            this.panelStepOperate.TabIndex = 49;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(813, 529);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(116, 30);
            this.btnPrevious.TabIndex = 47;
            this.btnPrevious.Text = "上一步";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1027, 529);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(116, 30);
            this.btnNext.TabIndex = 48;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // step2Sign
            // 
            this.step2Sign.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.step2Sign.Location = new System.Drawing.Point(104, 489);
            this.step2Sign.Name = "step2Sign";
            this.step2Sign.Size = new System.Drawing.Size(70, 70);
            this.step2Sign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.step2Sign.TabIndex = 51;
            this.step2Sign.TabStop = false;
            // 
            // step1Sign
            // 
            this.step1Sign.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.step1Sign.Location = new System.Drawing.Point(18, 489);
            this.step1Sign.Name = "step1Sign";
            this.step1Sign.Size = new System.Drawing.Size(70, 70);
            this.step1Sign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.step1Sign.TabIndex = 52;
            this.step1Sign.TabStop = false;
            // 
            // PPTool_Alignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControlCameraAera);
            this.Controls.Add(this.panelStepOperate);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.step2Sign);
            this.Controls.Add(this.step1Sign);
            this.Name = "PPTool_Alignment";
            this.Size = new System.Drawing.Size(1171, 575);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelStepOperate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.step2Sign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.step1Sign)).EndInit();
            this.ResumeLayout(false);

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
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private DevExpress.XtraEditors.PanelControl panelStepOperate;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.PictureBox step2Sign;
        private System.Windows.Forms.PictureBox step1Sign;
    }
}
