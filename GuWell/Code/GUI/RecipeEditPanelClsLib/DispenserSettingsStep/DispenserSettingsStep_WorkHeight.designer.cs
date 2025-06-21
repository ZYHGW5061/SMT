
using DevExpress.XtraEditors;

namespace RecipeEditPanelClsLib
{
    partial class DispenserSettingsStep_WorkHeight
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelStepInfo = new DevExpress.XtraEditors.LabelControl();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox5 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.btnDispenseGlue = new System.Windows.Forms.Button();
            this.btnMeasureHeight = new System.Windows.Forms.Button();
            this.btnDownDispenser = new System.Windows.Forms.Button();
            this.btnUpDispenser = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelStepInfo);
            this.panelControl1.Location = new System.Drawing.Point(10, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(338, 118);
            this.panelControl1.TabIndex = 38;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(90, 82);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 19);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "钮。";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(90, 48);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(230, 19);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "后，点击下方的<自动测高>按";
            // 
            // labelStepInfo
            // 
            this.labelStepInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepInfo.Appearance.Options.UseFont = true;
            this.labelStepInfo.Location = new System.Drawing.Point(5, 11);
            this.labelStepInfo.Name = "labelStepInfo";
            this.labelStepInfo.Size = new System.Drawing.Size(313, 19);
            this.labelStepInfo.TabIndex = 4;
            this.labelStepInfo.Text = "步骤 3/3：将划胶器移动到系统Mark上方";
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
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(39, 145);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(275, 317);
            this.stageQuickMove1.TabIndex = 40;
            // 
            // btnDispenseGlue
            // 
            this.btnDispenseGlue.Location = new System.Drawing.Point(53, 532);
            this.btnDispenseGlue.Name = "btnDispenseGlue";
            this.btnDispenseGlue.Size = new System.Drawing.Size(241, 30);
            this.btnDispenseGlue.TabIndex = 43;
            this.btnDispenseGlue.Text = "点胶";
            this.btnDispenseGlue.UseVisualStyleBackColor = true;
            this.btnDispenseGlue.Click += new System.EventHandler(this.btnDispenseGlue_Click);
            // 
            // btnMeasureHeight
            // 
            this.btnMeasureHeight.Location = new System.Drawing.Point(53, 582);
            this.btnMeasureHeight.Name = "btnMeasureHeight";
            this.btnMeasureHeight.Size = new System.Drawing.Size(241, 30);
            this.btnMeasureHeight.TabIndex = 42;
            this.btnMeasureHeight.Text = "高度确认";
            this.btnMeasureHeight.UseVisualStyleBackColor = true;
            this.btnMeasureHeight.Click += new System.EventHandler(this.btnMeasureHeight_Click);
            // 
            // btnDownDispenser
            // 
            this.btnDownDispenser.Location = new System.Drawing.Point(182, 480);
            this.btnDownDispenser.Name = "btnDownDispenser";
            this.btnDownDispenser.Size = new System.Drawing.Size(112, 30);
            this.btnDownDispenser.TabIndex = 45;
            this.btnDownDispenser.Text = "降胶座";
            this.btnDownDispenser.UseVisualStyleBackColor = true;
            this.btnDownDispenser.Click += new System.EventHandler(this.btnDownDispenser_Click);
            // 
            // btnUpDispenser
            // 
            this.btnUpDispenser.Location = new System.Drawing.Point(53, 480);
            this.btnUpDispenser.Name = "btnUpDispenser";
            this.btnUpDispenser.Size = new System.Drawing.Size(112, 30);
            this.btnUpDispenser.TabIndex = 46;
            this.btnUpDispenser.Text = "升胶座";
            this.btnUpDispenser.UseVisualStyleBackColor = true;
            this.btnUpDispenser.Click += new System.EventHandler(this.btnUpDispenser_Click);
            // 
            // DispenserSettingsStep_WorkHeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDownDispenser);
            this.Controls.Add(this.btnUpDispenser);
            this.Controls.Add(this.stageQuickMove1);
            this.Controls.Add(this.btnMeasureHeight);
            this.Controls.Add(this.btnDispenseGlue);
            this.Controls.Add(this.panelControl1);
            this.Name = "DispenserSettingsStep_WorkHeight";
            this.Size = new System.Drawing.Size(359, 628);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelStepInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox5;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private System.Windows.Forms.Button btnDispenseGlue;
        private LabelControl labelControl2;
        private LabelControl labelControl1;
        private System.Windows.Forms.Button btnMeasureHeight;
        private System.Windows.Forms.Button btnDownDispenser;
        private System.Windows.Forms.Button btnUpDispenser;
    }
}
