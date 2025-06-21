
using DevExpress.XtraEditors;

namespace RecipeEditPanelClsLib
{
    partial class ComponentPositionStep_WorkHeight
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
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelStepInfo = new DevExpress.XtraEditors.LabelControl();
            this.ctrlLight1 = new LightSourceCtrlPanelLib.CtrlLight();
            this.ctrlLight2 = new LightSourceCtrlPanelLib.CtrlLight();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox5 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btnGotoPPCenter = new System.Windows.Forms.Button();
            this.btnAutoFocus = new System.Windows.Forms.Button();
            this.btnLaserMeasureHeight = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(39, 215);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(275, 317);
            this.stageQuickMove1.TabIndex = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelStepInfo);
            this.panelControl1.Location = new System.Drawing.Point(10, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(338, 42);
            this.panelControl1.TabIndex = 38;
            // 
            // labelStepInfo
            // 
            this.labelStepInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepInfo.Appearance.Options.UseFont = true;
            this.labelStepInfo.Location = new System.Drawing.Point(5, 11);
            this.labelStepInfo.Name = "labelStepInfo";
            this.labelStepInfo.Size = new System.Drawing.Size(187, 19);
            this.labelStepInfo.TabIndex = 4;
            this.labelStepInfo.Text = "步骤 1/6：确认工作高度";
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
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.ctrlLight1);
            this.panelControl2.Controls.Add(this.ctrlLight2);
            this.panelControl2.Location = new System.Drawing.Point(10, 56);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(338, 149);
            this.panelControl2.TabIndex = 38;
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
            // btnGotoPPCenter
            // 
            this.btnGotoPPCenter.Location = new System.Drawing.Point(196, 554);
            this.btnGotoPPCenter.Name = "btnGotoPPCenter";
            this.btnGotoPPCenter.Size = new System.Drawing.Size(107, 30);
            this.btnGotoPPCenter.TabIndex = 39;
            this.btnGotoPPCenter.Text = "去吸嘴中心";
            this.btnGotoPPCenter.UseVisualStyleBackColor = true;
            this.btnGotoPPCenter.Click += new System.EventHandler(this.btnGotoPPCenter_Click);
            // 
            // btnAutoFocus
            // 
            this.btnAutoFocus.Location = new System.Drawing.Point(50, 554);
            this.btnAutoFocus.Name = "btnAutoFocus";
            this.btnAutoFocus.Size = new System.Drawing.Size(107, 30);
            this.btnAutoFocus.TabIndex = 40;
            this.btnAutoFocus.Text = "自动聚焦";
            this.btnAutoFocus.UseVisualStyleBackColor = true;
            this.btnAutoFocus.Click += new System.EventHandler(this.btnAutoFocus_Click);
            // 
            // btnLaserMeasureHeight
            // 
            this.btnLaserMeasureHeight.Location = new System.Drawing.Point(196, 601);
            this.btnLaserMeasureHeight.Name = "btnLaserMeasureHeight";
            this.btnLaserMeasureHeight.Size = new System.Drawing.Size(107, 30);
            this.btnLaserMeasureHeight.TabIndex = 41;
            this.btnLaserMeasureHeight.Text = "高度确认";
            this.btnLaserMeasureHeight.UseVisualStyleBackColor = true;
            this.btnLaserMeasureHeight.Click += new System.EventHandler(this.btnLaserMeasureHeight_Click);
            // 
            // ComponentPositionStep_WorkHeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAutoFocus);
            this.Controls.Add(this.btnLaserMeasureHeight);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.btnGotoPPCenter);
            this.Name = "ComponentPositionStep_WorkHeight";
            this.Size = new System.Drawing.Size(359, 647);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelStepInfo;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight1;
        private LightSourceCtrlPanelLib.CtrlLight ctrlLight2;
        private PanelControl panelControl2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox5;
        private System.Windows.Forms.Button btnGotoPPCenter;
        private System.Windows.Forms.Button btnAutoFocus;
        private System.Windows.Forms.Button btnLaserMeasureHeight;
    }
}
