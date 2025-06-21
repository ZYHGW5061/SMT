
namespace ControlPanelClsLib
{
    partial class FrmEjectionSystemTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEjectionSystemTool));
            this.tabPane = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPageNeedleZero = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ejectionSystemTool_NeedleZero1 = new ControlPanelClsLib.EjectionSystemTool_NeedleZero();
            this.tabNavigationPageNeedleAlign = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ejectionSystemTool_XYMeasuring1 = new ControlPanelClsLib.EjectionSystemTool_XYMeasuring();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbExistESTool = new System.Windows.Forms.ComboBox();
            this.btnNewESTool = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.tabNavigationPagePPESH = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ppesHeight1 = new ControlPanelClsLib.PPESHeight();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).BeginInit();
            this.tabPane.SuspendLayout();
            this.tabNavigationPageNeedleZero.SuspendLayout();
            this.tabNavigationPageNeedleAlign.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tabNavigationPagePPESH.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPane
            // 
            this.tabPane.Controls.Add(this.tabNavigationPageNeedleZero);
            this.tabPane.Controls.Add(this.tabNavigationPageNeedleAlign);
            this.tabPane.Controls.Add(this.tabNavigationPagePPESH);
            this.tabPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane.Location = new System.Drawing.Point(3, 53);
            this.tabPane.Name = "tabPane";
            this.tabPane.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPageNeedleZero,
            this.tabNavigationPagePPESH,
            this.tabNavigationPageNeedleAlign});
            this.tabPane.RegularSize = new System.Drawing.Size(1365, 686);
            this.tabPane.SelectedPage = this.tabNavigationPageNeedleZero;
            this.tabPane.Size = new System.Drawing.Size(1365, 686);
            this.tabPane.TabIndex = 15;
            this.tabPane.Text = "1";
            // 
            // tabNavigationPageNeedleZero
            // 
            this.tabNavigationPageNeedleZero.Caption = "    顶针Z原点确定    ";
            this.tabNavigationPageNeedleZero.Controls.Add(this.ejectionSystemTool_NeedleZero1);
            this.tabNavigationPageNeedleZero.Name = "tabNavigationPageNeedleZero";
            this.tabNavigationPageNeedleZero.Size = new System.Drawing.Size(1365, 653);
            // 
            // ejectionSystemTool_NeedleZero1
            // 
            this.ejectionSystemTool_NeedleZero1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ejectionSystemTool_NeedleZero1.Location = new System.Drawing.Point(0, 0);
            this.ejectionSystemTool_NeedleZero1.Name = "ejectionSystemTool_NeedleZero1";
            this.ejectionSystemTool_NeedleZero1.Size = new System.Drawing.Size(1365, 653);
            this.ejectionSystemTool_NeedleZero1.TabIndex = 0;
            // 
            // tabNavigationPageNeedleAlign
            // 
            this.tabNavigationPageNeedleAlign.Caption = "    顶针XY原点确定    ";
            this.tabNavigationPageNeedleAlign.Controls.Add(this.ejectionSystemTool_XYMeasuring1);
            this.tabNavigationPageNeedleAlign.Name = "tabNavigationPageNeedleAlign";
            this.tabNavigationPageNeedleAlign.Size = new System.Drawing.Size(1365, 686);
            // 
            // ejectionSystemTool_XYMeasuring1
            // 
            this.ejectionSystemTool_XYMeasuring1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ejectionSystemTool_XYMeasuring1.Location = new System.Drawing.Point(0, 0);
            this.ejectionSystemTool_XYMeasuring1.Name = "ejectionSystemTool_XYMeasuring1";
            this.ejectionSystemTool_XYMeasuring1.Size = new System.Drawing.Size(1365, 686);
            this.ejectionSystemTool_XYMeasuring1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabPane, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
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
            this.panelControl1.Controls.Add(this.cmbExistESTool);
            this.panelControl1.Controls.Add(this.btnNewESTool);
            this.panelControl1.Controls.Add(this.labelControl21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1365, 44);
            this.panelControl1.TabIndex = 38;
            // 
            // cmbExistESTool
            // 
            this.cmbExistESTool.FormattingEnabled = true;
            this.cmbExistESTool.Location = new System.Drawing.Point(102, 10);
            this.cmbExistESTool.Name = "cmbExistESTool";
            this.cmbExistESTool.Size = new System.Drawing.Size(162, 22);
            this.cmbExistESTool.TabIndex = 25;
            this.cmbExistESTool.SelectedIndexChanged += new System.EventHandler(this.cmbExistESTool_SelectedIndexChanged);
            // 
            // btnNewESTool
            // 
            this.btnNewESTool.Location = new System.Drawing.Point(338, 12);
            this.btnNewESTool.Name = "btnNewESTool";
            this.btnNewESTool.Size = new System.Drawing.Size(71, 22);
            this.btnNewESTool.TabIndex = 24;
            this.btnNewESTool.Text = "新增";
            this.btnNewESTool.Click += new System.EventHandler(this.btnNewESTool_Click);
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(32, 15);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(52, 14);
            this.labelControl21.TabIndex = 22;
            this.labelControl21.Text = "已有工具:";
            // 
            // tabNavigationPagePPESH
            // 
            this.tabNavigationPagePPESH.Caption = "顶针座测高";
            this.tabNavigationPagePPESH.Controls.Add(this.ppesHeight1);
            this.tabNavigationPagePPESH.Name = "tabNavigationPagePPESH";
            this.tabNavigationPagePPESH.Size = new System.Drawing.Size(1365, 686);
            // 
            // ppesHeight1
            // 
            this.ppesHeight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppesHeight1.ESStageHeight = 0F;
            this.ppesHeight1.Location = new System.Drawing.Point(0, 0);
            this.ppesHeight1.Name = "ppesHeight1";
            this.ppesHeight1.ESZSystemHeight = 0F;
            this.ppesHeight1.Size = new System.Drawing.Size(1365, 686);
            this.ppesHeight1.TabIndex = 0;
            // 
            // FrmEjectionSystemTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 792);
            this.Controls.Add(this.tableLayoutPanel1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmEjectionSystemTool.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmEjectionSystemTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "顶针工具设置";
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).EndInit();
            this.tabPane.ResumeLayout(false);
            this.tabNavigationPageNeedleZero.ResumeLayout(false);
            this.tabNavigationPageNeedleAlign.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.tabNavigationPagePPESH.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TabPane tabPane;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPageNeedleZero;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPageNeedleAlign;
        private EjectionSystemTool_NeedleZero ejectionSystemTool_NeedleZero1;
        private EjectionSystemTool_XYMeasuring ejectionSystemTool_XYMeasuring1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnNewESTool;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbExistESTool;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPagePPESH;
        private PPESHeight ppesHeight1;
    }
}
