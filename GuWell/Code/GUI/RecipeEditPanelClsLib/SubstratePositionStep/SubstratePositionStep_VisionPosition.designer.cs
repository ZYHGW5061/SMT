
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;

namespace RecipeEditPanelClsLib
{
    partial class SubstratePositionStep_VisionPosition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubstratePositionStep_VisionPosition));
            VisionClsLib.MatchTemplateResult matchTemplateResult1 = new VisionClsLib.MatchTemplateResult();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelStepInfo = new DevExpress.XtraEditors.LabelControl();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox5 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.visualMatchControlGUI1 = new VisionGUI.VisualMatchControlGUI();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.labelStepInfo.Size = new System.Drawing.Size(255, 19);
            this.labelStepInfo.TabIndex = 4;
            this.labelStepInfo.Text = "步骤 6/6：创建衬底视觉定位模板";
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
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.503607F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.49639F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(359, 647);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 51);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(353, 593);
            this.tabControl1.TabIndex = 39;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.visualMatchControlGUI1);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(345, 566);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置模板";
            // 
            // visualMatchControlGUI1
            // 
            this.visualMatchControlGUI1.AngleRange = 15;
            this.visualMatchControlGUI1.DirectLightintensity = 0;
            this.visualMatchControlGUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualMatchControlGUI1.Location = new System.Drawing.Point(3, 3);
            this.visualMatchControlGUI1.MatchRunfilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchRun.contourmxml";
            this.visualMatchControlGUI1.MatchTemplatefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchTemplate.contourmxml";
            this.visualMatchControlGUI1.Name = "visualMatchControlGUI1";
            this.visualMatchControlGUI1.OutlineAngle = 0F;
            this.visualMatchControlGUI1.OutlineDeviation = ((System.Drawing.PointF)(resources.GetObject("visualMatchControlGUI1.OutlineDeviation")));
            this.visualMatchControlGUI1.RingLightintensity = 0;
            this.visualMatchControlGUI1.Score = 0.5F;
            this.visualMatchControlGUI1.SearchRoi = ((GlobalDataDefineClsLib.RectangleFV)(resources.GetObject("visualMatchControlGUI1.SearchRoi")));
            this.visualMatchControlGUI1.Size = new System.Drawing.Size(339, 560);
            this.visualMatchControlGUI1.TabIndex = 0;
            this.visualMatchControlGUI1.Templateresult = matchTemplateResult1;
            this.visualMatchControlGUI1.TemplateRoi = ((GlobalDataDefineClsLib.RectangleFV)(resources.GetObject("visualMatchControlGUI1.TemplateRoi")));
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.stageQuickMove1);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(345, 566);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Stage移动";
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(35, 21);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 0;
            // 
            // SubmountPositionStep_VisionPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SubmountPositionStep_VisionPosition";
            this.Size = new System.Drawing.Size(359, 647);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelStepInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private VisionGUI.VisualMatchControlGUI visualMatchControlGUI1;
    }
}
