namespace ControlPanelClsLib
{
    partial class FrmCheck
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCheck));
            this.teAngle = new DevExpress.XtraEditors.TextEdit();
            this.panelControlCameraAera = new DevExpress.XtraEditors.PanelControl();
            this.lightControl1 = new LightSourceCtrlPanelLib.LightControl();
            this.btnCalOffset = new DevExpress.XtraEditors.SimpleButton();
            this.btnRecognise = new DevExpress.XtraEditors.SimpleButton();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.teNewCenterX = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.teNewCenterY = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnMove2Center = new DevExpress.XtraEditors.SimpleButton();
            this.teRotateCenterX = new DevExpress.XtraEditors.TextEdit();
            this.teRotateCenterY = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnRecogniseTwo = new DevExpress.XtraEditors.SimpleButton();
            this.teCameraOffsetX = new DevExpress.XtraEditors.TextEdit();
            this.teCameraOffsetY = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.teCameraOffsetYBeforeR = new DevExpress.XtraEditors.TextEdit();
            this.teCameraOffsetXBeforeR = new DevExpress.XtraEditors.TextEdit();
            this.teNewCenterOffsetX = new DevExpress.XtraEditors.TextEdit();
            this.teNewCenterOffsetY = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.teActualCenterOffsetX = new DevExpress.XtraEditors.TextEdit();
            this.teActualCenterOffsetY = new DevExpress.XtraEditors.TextEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.teAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teRotateCenterX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teRotateCenterY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetYBeforeR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetXBeforeR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teActualCenterOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teActualCenterOffsetY.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teAngle
            // 
            this.teAngle.Location = new System.Drawing.Point(1134, 22);
            this.teAngle.Name = "teAngle";
            this.teAngle.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teAngle.Properties.Appearance.Options.UseFont = true;
            this.teAngle.Properties.ReadOnly = true;
            this.teAngle.Size = new System.Drawing.Size(135, 30);
            this.teAngle.TabIndex = 17;
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
            this.lightControl1.Location = new System.Drawing.Point(12, 531);
            this.lightControl1.Name = "lightControl1";
            this.lightControl1.Size = new System.Drawing.Size(332, 164);
            this.lightControl1.TabIndex = 22;
            // 
            // btnCalOffset
            // 
            this.btnCalOffset.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCalOffset.ImageOptions.SvgImage")));
            this.btnCalOffset.Location = new System.Drawing.Point(766, 566);
            this.btnCalOffset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCalOffset.Name = "btnCalOffset";
            this.btnCalOffset.Size = new System.Drawing.Size(99, 42);
            this.btnCalOffset.TabIndex = 23;
            this.btnCalOffset.Text = "计算偏移";
            this.btnCalOffset.Click += new System.EventHandler(this.btnCalOffset_Click);
            // 
            // btnRecognise
            // 
            this.btnRecognise.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRecognise.ImageOptions.SvgImage")));
            this.btnRecognise.Location = new System.Drawing.Point(533, 630);
            this.btnRecognise.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRecognise.Name = "btnRecognise";
            this.btnRecognise.Size = new System.Drawing.Size(99, 42);
            this.btnRecognise.TabIndex = 23;
            this.btnRecognise.Text = "识别";
            this.btnRecognise.Click += new System.EventHandler(this.btnStartAuto_Click);
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(644, 12);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 20;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(1094, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 24;
            this.labelControl1.Text = "角度：";
            // 
            // teNewCenterX
            // 
            this.teNewCenterX.Location = new System.Drawing.Point(1134, 258);
            this.teNewCenterX.Name = "teNewCenterX";
            this.teNewCenterX.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teNewCenterX.Properties.Appearance.Options.UseFont = true;
            this.teNewCenterX.Properties.ReadOnly = true;
            this.teNewCenterX.Size = new System.Drawing.Size(135, 30);
            this.teNewCenterX.TabIndex = 17;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(978, 266);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(151, 14);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "旋转后芯片中心X（计算）：";
            // 
            // teNewCenterY
            // 
            this.teNewCenterY.Location = new System.Drawing.Point(1134, 297);
            this.teNewCenterY.Name = "teNewCenterY";
            this.teNewCenterY.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teNewCenterY.Properties.Appearance.Options.UseFont = true;
            this.teNewCenterY.Properties.ReadOnly = true;
            this.teNewCenterY.Size = new System.Drawing.Size(135, 30);
            this.teNewCenterY.TabIndex = 17;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(977, 304);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(152, 14);
            this.labelControl3.TabIndex = 24;
            this.labelControl3.Text = "旋转后芯片中心Y（计算）：";
            // 
            // btnMove2Center
            // 
            this.btnMove2Center.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnMove2Center.ImageOptions.SvgImage")));
            this.btnMove2Center.Location = new System.Drawing.Point(533, 566);
            this.btnMove2Center.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMove2Center.Name = "btnMove2Center";
            this.btnMove2Center.Size = new System.Drawing.Size(159, 42);
            this.btnMove2Center.TabIndex = 23;
            this.btnMove2Center.Text = "移动到视野中心";
            this.btnMove2Center.Click += new System.EventHandler(this.btnMove2Center_Click);
            // 
            // teRotateCenterX
            // 
            this.teRotateCenterX.Location = new System.Drawing.Point(1134, 85);
            this.teRotateCenterX.Name = "teRotateCenterX";
            this.teRotateCenterX.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teRotateCenterX.Properties.Appearance.Options.UseFont = true;
            this.teRotateCenterX.Properties.ReadOnly = true;
            this.teRotateCenterX.Size = new System.Drawing.Size(135, 30);
            this.teRotateCenterX.TabIndex = 17;
            // 
            // teRotateCenterY
            // 
            this.teRotateCenterY.Location = new System.Drawing.Point(1134, 124);
            this.teRotateCenterY.Name = "teRotateCenterY";
            this.teRotateCenterY.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teRotateCenterY.Properties.Appearance.Options.UseFont = true;
            this.teRotateCenterY.Properties.ReadOnly = true;
            this.teRotateCenterY.Size = new System.Drawing.Size(135, 30);
            this.teRotateCenterY.TabIndex = 17;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(1074, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 14);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "旋中心X：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(1062, 132);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(68, 14);
            this.labelControl5.TabIndex = 24;
            this.labelControl5.Text = "旋转中心Y：";
            // 
            // btnRecogniseTwo
            // 
            this.btnRecogniseTwo.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRecogniseTwo.ImageOptions.SvgImage")));
            this.btnRecogniseTwo.Location = new System.Drawing.Point(766, 625);
            this.btnRecogniseTwo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRecogniseTwo.Name = "btnRecogniseTwo";
            this.btnRecogniseTwo.Size = new System.Drawing.Size(99, 42);
            this.btnRecogniseTwo.TabIndex = 23;
            this.btnRecogniseTwo.Text = "再次识别";
            this.btnRecogniseTwo.Click += new System.EventHandler(this.btnRecogniseTwo_Click);
            // 
            // teCameraOffsetX
            // 
            this.teCameraOffsetX.Location = new System.Drawing.Point(1134, 351);
            this.teCameraOffsetX.Name = "teCameraOffsetX";
            this.teCameraOffsetX.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teCameraOffsetX.Properties.Appearance.Options.UseFont = true;
            this.teCameraOffsetX.Properties.ReadOnly = true;
            this.teCameraOffsetX.Size = new System.Drawing.Size(135, 30);
            this.teCameraOffsetX.TabIndex = 17;
            // 
            // teCameraOffsetY
            // 
            this.teCameraOffsetY.Location = new System.Drawing.Point(1134, 390);
            this.teCameraOffsetY.Name = "teCameraOffsetY";
            this.teCameraOffsetY.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teCameraOffsetY.Properties.Appearance.Options.UseFont = true;
            this.teCameraOffsetY.Properties.ReadOnly = true;
            this.teCameraOffsetY.Size = new System.Drawing.Size(135, 30);
            this.teCameraOffsetY.TabIndex = 17;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(990, 359);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(139, 14);
            this.labelControl6.TabIndex = 24;
            this.labelControl6.Text = "旋转后相对于相机偏移X：";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(991, 397);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(140, 14);
            this.labelControl7.TabIndex = 24;
            this.labelControl7.Text = "旋转后相对于相机偏移Y：";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(991, 206);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(140, 14);
            this.labelControl8.TabIndex = 27;
            this.labelControl8.Text = "旋转前相对于相机偏移Y：";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(990, 168);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(139, 14);
            this.labelControl9.TabIndex = 28;
            this.labelControl9.Text = "旋转前相对于相机偏移X：";
            // 
            // teCameraOffsetYBeforeR
            // 
            this.teCameraOffsetYBeforeR.Location = new System.Drawing.Point(1134, 199);
            this.teCameraOffsetYBeforeR.Name = "teCameraOffsetYBeforeR";
            this.teCameraOffsetYBeforeR.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teCameraOffsetYBeforeR.Properties.Appearance.Options.UseFont = true;
            this.teCameraOffsetYBeforeR.Properties.ReadOnly = true;
            this.teCameraOffsetYBeforeR.Size = new System.Drawing.Size(135, 30);
            this.teCameraOffsetYBeforeR.TabIndex = 25;
            // 
            // teCameraOffsetXBeforeR
            // 
            this.teCameraOffsetXBeforeR.Location = new System.Drawing.Point(1134, 160);
            this.teCameraOffsetXBeforeR.Name = "teCameraOffsetXBeforeR";
            this.teCameraOffsetXBeforeR.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teCameraOffsetXBeforeR.Properties.Appearance.Options.UseFont = true;
            this.teCameraOffsetXBeforeR.Properties.ReadOnly = true;
            this.teCameraOffsetXBeforeR.Size = new System.Drawing.Size(135, 30);
            this.teCameraOffsetXBeforeR.TabIndex = 26;
            // 
            // teNewCenterOffsetX
            // 
            this.teNewCenterOffsetX.Location = new System.Drawing.Point(1134, 501);
            this.teNewCenterOffsetX.Name = "teNewCenterOffsetX";
            this.teNewCenterOffsetX.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teNewCenterOffsetX.Properties.Appearance.Options.UseFont = true;
            this.teNewCenterOffsetX.Properties.ReadOnly = true;
            this.teNewCenterOffsetX.Size = new System.Drawing.Size(135, 30);
            this.teNewCenterOffsetX.TabIndex = 17;
            // 
            // teNewCenterOffsetY
            // 
            this.teNewCenterOffsetY.Location = new System.Drawing.Point(1134, 540);
            this.teNewCenterOffsetY.Name = "teNewCenterOffsetY";
            this.teNewCenterOffsetY.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teNewCenterOffsetY.Properties.Appearance.Options.UseFont = true;
            this.teNewCenterOffsetY.Properties.ReadOnly = true;
            this.teNewCenterOffsetY.Size = new System.Drawing.Size(135, 30);
            this.teNewCenterOffsetY.TabIndex = 17;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(944, 509);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(185, 14);
            this.labelControl10.TabIndex = 24;
            this.labelControl10.Text = "旋转后芯片中心OffsetX（计算）：";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(943, 547);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(186, 14);
            this.labelControl11.TabIndex = 24;
            this.labelControl11.Text = "旋转后芯片中心OffsetY（计算）：";
            // 
            // teActualCenterOffsetX
            // 
            this.teActualCenterOffsetX.Location = new System.Drawing.Point(1134, 583);
            this.teActualCenterOffsetX.Name = "teActualCenterOffsetX";
            this.teActualCenterOffsetX.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teActualCenterOffsetX.Properties.Appearance.Options.UseFont = true;
            this.teActualCenterOffsetX.Properties.ReadOnly = true;
            this.teActualCenterOffsetX.Size = new System.Drawing.Size(135, 30);
            this.teActualCenterOffsetX.TabIndex = 17;
            // 
            // teActualCenterOffsetY
            // 
            this.teActualCenterOffsetY.Location = new System.Drawing.Point(1134, 622);
            this.teActualCenterOffsetY.Name = "teActualCenterOffsetY";
            this.teActualCenterOffsetY.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.teActualCenterOffsetY.Properties.Appearance.Options.UseFont = true;
            this.teActualCenterOffsetY.Properties.ReadOnly = true;
            this.teActualCenterOffsetY.Size = new System.Drawing.Size(135, 30);
            this.teActualCenterOffsetY.TabIndex = 17;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(944, 591);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(185, 14);
            this.labelControl12.TabIndex = 24;
            this.labelControl12.Text = "旋转后芯片中心OffsetX（实际）：";
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(943, 629);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(186, 14);
            this.labelControl13.TabIndex = 24;
            this.labelControl13.Text = "旋转后芯片中心OffsetY（实际）：";
            // 
            // FrmCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1323, 707);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.teCameraOffsetYBeforeR);
            this.Controls.Add(this.teCameraOffsetXBeforeR);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.btnRecogniseTwo);
            this.Controls.Add(this.btnRecognise);
            this.Controls.Add(this.btnMove2Center);
            this.Controls.Add(this.btnCalOffset);
            this.Controls.Add(this.lightControl1);
            this.Controls.Add(this.panelControlCameraAera);
            this.Controls.Add(this.teRotateCenterY);
            this.Controls.Add(this.teCameraOffsetY);
            this.Controls.Add(this.teActualCenterOffsetY);
            this.Controls.Add(this.teNewCenterOffsetY);
            this.Controls.Add(this.teNewCenterY);
            this.Controls.Add(this.teCameraOffsetX);
            this.Controls.Add(this.teActualCenterOffsetX);
            this.Controls.Add(this.teRotateCenterX);
            this.Controls.Add(this.teNewCenterOffsetX);
            this.Controls.Add(this.teNewCenterX);
            this.Controls.Add(this.teAngle);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmCheck.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "单步执行";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSingleStepRun_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.teAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCameraAera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teRotateCenterX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teRotateCenterY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetYBeforeR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCameraOffsetXBeforeR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teNewCenterOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teActualCenterOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teActualCenterOffsetY.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit teAngle;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.PanelControl panelControlCameraAera;
        private LightSourceCtrlPanelLib.LightControl lightControl1;
        private DevExpress.XtraEditors.SimpleButton btnCalOffset;
        private DevExpress.XtraEditors.SimpleButton btnRecognise;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit teNewCenterX;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit teNewCenterY;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnMove2Center;
        private DevExpress.XtraEditors.TextEdit teRotateCenterX;
        private DevExpress.XtraEditors.TextEdit teRotateCenterY;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnRecogniseTwo;
        private DevExpress.XtraEditors.TextEdit teCameraOffsetX;
        private DevExpress.XtraEditors.TextEdit teCameraOffsetY;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit teCameraOffsetYBeforeR;
        private DevExpress.XtraEditors.TextEdit teCameraOffsetXBeforeR;
        private DevExpress.XtraEditors.TextEdit teNewCenterOffsetX;
        private DevExpress.XtraEditors.TextEdit teNewCenterOffsetY;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.TextEdit teActualCenterOffsetX;
        private DevExpress.XtraEditors.TextEdit teActualCenterOffsetY;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl13;
    }
}
