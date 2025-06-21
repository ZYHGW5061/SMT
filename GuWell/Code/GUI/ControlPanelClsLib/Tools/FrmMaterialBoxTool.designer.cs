
namespace ControlPanelClsLib
{
    partial class FrmMaterialBoxTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMaterialBoxTool));
            this.tabPane = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPageHeight = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbSlotCount = new System.Windows.Forms.ComboBox();
            this.seFirstSlotPosMM = new DevExpress.XtraEditors.SpinEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbExistTool = new System.Windows.Forms.ComboBox();
            this.btnNewTool = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).BeginInit();
            this.tabPane.SuspendLayout();
            this.tabNavigationPageHeight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seFirstSlotPosMM.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPane
            // 
            this.tabPane.Controls.Add(this.tabNavigationPageHeight);
            this.tabPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane.Location = new System.Drawing.Point(3, 53);
            this.tabPane.Name = "tabPane";
            this.tabPane.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPageHeight});
            this.tabPane.RegularSize = new System.Drawing.Size(1365, 686);
            this.tabPane.SelectedPage = this.tabNavigationPageHeight;
            this.tabPane.Size = new System.Drawing.Size(1365, 686);
            this.tabPane.TabIndex = 15;
            this.tabPane.Text = "1";
            // 
            // tabNavigationPageHeight
            // 
            this.tabNavigationPageHeight.Caption = "    基本参数    ";
            this.tabNavigationPageHeight.Controls.Add(this.panelControl3);
            this.tabNavigationPageHeight.Name = "tabNavigationPageHeight";
            this.tabNavigationPageHeight.Size = new System.Drawing.Size(1365, 653);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.simpleButton1);
            this.panelControl3.Controls.Add(this.simpleButton3);
            this.panelControl3.Controls.Add(this.label8);
            this.panelControl3.Controls.Add(this.cmbSlotCount);
            this.panelControl3.Controls.Add(this.seFirstSlotPosMM);
            this.panelControl3.Controls.Add(this.label5);
            this.panelControl3.Controls.Add(this.stageQuickMove1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1365, 653);
            this.panelControl3.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(155, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 14);
            this.label8.TabIndex = 16;
            this.label8.Text = "层数：";
            // 
            // cmbSlotCount
            // 
            this.cmbSlotCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSlotCount.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSlotCount.FormattingEnabled = true;
            this.cmbSlotCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25"});
            this.cmbSlotCount.Location = new System.Drawing.Point(202, 194);
            this.cmbSlotCount.Name = "cmbSlotCount";
            this.cmbSlotCount.Size = new System.Drawing.Size(121, 20);
            this.cmbSlotCount.TabIndex = 15;
            // 
            // seFirstSlotPosMM
            // 
            this.seFirstSlotPosMM.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seFirstSlotPosMM.Location = new System.Drawing.Point(202, 78);
            this.seFirstSlotPosMM.Name = "seFirstSlotPosMM";
            this.seFirstSlotPosMM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seFirstSlotPosMM.Properties.DisplayFormat.FormatString = "0.000";
            this.seFirstSlotPosMM.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seFirstSlotPosMM.Properties.EditFormat.FormatString = "0.000";
            this.seFirstSlotPosMM.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seFirstSlotPosMM.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.seFirstSlotPosMM.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seFirstSlotPosMM.Properties.MaskSettings.Set("mask", "f2");
            this.seFirstSlotPosMM.Properties.MaxValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.seFirstSlotPosMM.Size = new System.Drawing.Size(121, 20);
            this.seFirstSlotPosMM.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 14);
            this.label5.TabIndex = 13;
            this.label5.Text = "第一层高度/mm：";
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(1022, 81);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(267, 317);
            this.stageQuickMove1.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabPane, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1371, 792);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 745);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1365, 44);
            this.panelControl2.TabIndex = 39;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1240, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmbExistTool);
            this.panelControl1.Controls.Add(this.btnNewTool);
            this.panelControl1.Controls.Add(this.labelControl21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1365, 44);
            this.panelControl1.TabIndex = 38;
            // 
            // cmbExistTool
            // 
            this.cmbExistTool.FormattingEnabled = true;
            this.cmbExistTool.Location = new System.Drawing.Point(102, 11);
            this.cmbExistTool.Name = "cmbExistTool";
            this.cmbExistTool.Size = new System.Drawing.Size(162, 22);
            this.cmbExistTool.TabIndex = 25;
            this.cmbExistTool.SelectedIndexChanged += new System.EventHandler(this.cmbExistTool_SelectedIndexChanged);
            // 
            // btnNewTool
            // 
            this.btnNewTool.Location = new System.Drawing.Point(338, 12);
            this.btnNewTool.Name = "btnNewTool";
            this.btnNewTool.Size = new System.Drawing.Size(71, 22);
            this.btnNewTool.TabIndex = 24;
            this.btnNewTool.Text = "新增";
            this.btnNewTool.Click += new System.EventHandler(this.btnNewTool_Click);
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(32, 15);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(52, 14);
            this.labelControl21.TabIndex = 22;
            this.labelControl21.Text = "已有工具:";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(202, 419);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(115, 36);
            this.simpleButton1.TabIndex = 21;
            this.simpleButton1.Text = "下料";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.Location = new System.Drawing.Point(202, 362);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(115, 36);
            this.simpleButton3.TabIndex = 22;
            this.simpleButton3.Text = "上料";
            // 
            // FrmMaterialBoxTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 792);
            this.Controls.Add(this.tableLayoutPanel1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmMaterialBoxTool.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmMaterialBoxTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "料盒工具管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPPTool_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).EndInit();
            this.tabPane.ResumeLayout(false);
            this.tabNavigationPageHeight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seFirstSlotPosMM.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TabPane tabPane;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPageHeight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnNewTool;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbExistTool;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbSlotCount;
        private DevExpress.XtraEditors.SpinEdit seFirstSlotPosMM;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
    }
}
