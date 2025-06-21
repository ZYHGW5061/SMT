
namespace StageCtrlPanelLib
{
    partial class FrmStageMaintain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStageMaintain));
            this.btnStartPPMove2Uplook = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.stageAxisParameterPanel1 = new StageCtrlPanelLib.StageAxisParameterPanel();
            this.stageAxisMoveControlPanel1 = new StageCtrlPanelLib.StageAxisMoveControlPanel();
            this.SuspendLayout();
            // 
            // btnStartPPMove2Uplook
            // 
            this.btnStartPPMove2Uplook.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnStartAuto.ImageOptions.SvgImage")));
            this.btnStartPPMove2Uplook.Location = new System.Drawing.Point(26, 454);
            this.btnStartPPMove2Uplook.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartPPMove2Uplook.Name = "btnStartPPMove2Uplook";
            this.btnStartPPMove2Uplook.Size = new System.Drawing.Size(184, 42);
            this.btnStartPPMove2Uplook.TabIndex = 24;
            this.btnStartPPMove2Uplook.Text = "芯片吸嘴-->榜头相机";
            this.btnStartPPMove2Uplook.Click += new System.EventHandler(this.btnStartPPMove2Uplook_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.simpleButton1.Location = new System.Drawing.Point(26, 513);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(184, 42);
            this.simpleButton1.TabIndex = 24;
            this.simpleButton1.Text = "衬底吸嘴-->榜头相机";
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton2.ImageOptions.SvgImage")));
            this.simpleButton2.Location = new System.Drawing.Point(226, 454);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(184, 42);
            this.simpleButton2.TabIndex = 24;
            this.simpleButton2.Text = "榜头相机-->芯片吸嘴";
            // 
            // simpleButton3
            // 
            this.simpleButton3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton3.ImageOptions.SvgImage")));
            this.simpleButton3.Location = new System.Drawing.Point(226, 513);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(184, 42);
            this.simpleButton3.TabIndex = 24;
            this.simpleButton3.Text = "榜头相机-->衬底吸嘴";
            // 
            // simpleButton4
            // 
            this.simpleButton4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton4.ImageOptions.SvgImage")));
            this.simpleButton4.Location = new System.Drawing.Point(540, 454);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(184, 42);
            this.simpleButton4.TabIndex = 24;
            this.simpleButton4.Text = "芯片吸嘴-->仰视相机";
            // 
            // simpleButton5
            // 
            this.simpleButton5.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton5.ImageOptions.SvgImage")));
            this.simpleButton5.Location = new System.Drawing.Point(740, 454);
            this.simpleButton5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(184, 42);
            this.simpleButton5.TabIndex = 24;
            this.simpleButton5.Text = "仰视相机-->芯片吸嘴";
            // 
            // simpleButton6
            // 
            this.simpleButton6.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton6.ImageOptions.SvgImage")));
            this.simpleButton6.Location = new System.Drawing.Point(740, 513);
            this.simpleButton6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(184, 42);
            this.simpleButton6.TabIndex = 24;
            this.simpleButton6.Text = "仰视相机-->衬底吸嘴";
            // 
            // simpleButton7
            // 
            this.simpleButton7.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton7.ImageOptions.SvgImage")));
            this.simpleButton7.Location = new System.Drawing.Point(540, 513);
            this.simpleButton7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(184, 42);
            this.simpleButton7.TabIndex = 24;
            this.simpleButton7.Text = "衬底吸嘴-->仰视相机";
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(632, 38);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 2;
            // 
            // stageAxisParameterPanel1
            // 
            this.stageAxisParameterPanel1.CurrentStageAxis = GlobalDataDefineClsLib.EnumStageAxis.BondX;
            this.stageAxisParameterPanel1.Location = new System.Drawing.Point(348, 3);
            this.stageAxisParameterPanel1.Name = "stageAxisParameterPanel1";
            this.stageAxisParameterPanel1.Size = new System.Drawing.Size(242, 388);
            this.stageAxisParameterPanel1.TabIndex = 1;
            // 
            // stageAxisMoveControlPanel1
            // 
            this.stageAxisMoveControlPanel1.CurrentStageAxis = GlobalDataDefineClsLib.EnumStageAxis.BondX;
            this.stageAxisMoveControlPanel1.Location = new System.Drawing.Point(2, 3);
            this.stageAxisMoveControlPanel1.Name = "stageAxisMoveControlPanel1";
            this.stageAxisMoveControlPanel1.Size = new System.Drawing.Size(340, 388);
            this.stageAxisMoveControlPanel1.TabIndex = 0;
            // 
            // FrmStageMaintain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 607);
            this.Controls.Add(this.simpleButton7);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.btnStartPPMove2Uplook);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.stageAxisParameterPanel1);
            this.Controls.Add(this.stageAxisMoveControlPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStageMaintain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "运动控制";
            this.ResumeLayout(false);

        }

        #endregion

        private StageAxisMoveControlPanel stageAxisMoveControlPanel1;
        private StageAxisParameterPanel stageAxisParameterPanel1;
        private StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.SimpleButton btnStartPPMove2Uplook;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
    }
}