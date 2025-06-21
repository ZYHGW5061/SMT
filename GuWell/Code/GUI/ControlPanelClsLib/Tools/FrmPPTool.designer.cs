
namespace ControlPanelClsLib
{
    partial class FrmPPTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPPTool));
            this.tabPane = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPageHeight = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ppTool_Height1 = new ControlPanelClsLib.PPTool_Height();
            this.tabNavigationPageAlign = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ppTool_Alignment1 = new ControlPanelClsLib.PPTool_Alignment();
            this.tabNavigationPagePPESH = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ppesHeight1 = new ControlPanelClsLib.PPESHeight();
            this.tabNavigationPageTeach = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.seRotationTablePos4LoadPP = new DevExpress.XtraEditors.SpinEdit();
            this.seBondPosX4LoadPP = new DevExpress.XtraEditors.SpinEdit();
            this.seBondPosY4LoadPP = new DevExpress.XtraEditors.SpinEdit();
            this.seBondPosZ4LoadPP = new DevExpress.XtraEditors.SpinEdit();
            this.seBondPosT4LoadPP = new DevExpress.XtraEditors.SpinEdit();
            this.btnSaveConfig = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbExistTool = new System.Windows.Forms.ComboBox();
            this.btnCalibratePP = new DevExpress.XtraEditors.SimpleButton();
            this.btnUnloadPP = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadPP = new DevExpress.XtraEditors.SimpleButton();
            this.btnNewTool = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).BeginInit();
            this.tabPane.SuspendLayout();
            this.tabNavigationPageHeight.SuspendLayout();
            this.tabNavigationPageAlign.SuspendLayout();
            this.tabNavigationPagePPESH.SuspendLayout();
            this.tabNavigationPageTeach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seRotationTablePos4LoadPP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosX4LoadPP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosY4LoadPP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosZ4LoadPP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosT4LoadPP.Properties)).BeginInit();
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
            this.tabPane.Controls.Add(this.tabNavigationPageAlign);
            this.tabPane.Controls.Add(this.tabNavigationPagePPESH);
            this.tabPane.Controls.Add(this.tabNavigationPageTeach);
            this.tabPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane.Location = new System.Drawing.Point(3, 53);
            this.tabPane.Name = "tabPane";
            this.tabPane.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPageHeight,
            this.tabNavigationPageAlign,
            this.tabNavigationPagePPESH,
            this.tabNavigationPageTeach});
            this.tabPane.RegularSize = new System.Drawing.Size(1365, 686);
            this.tabPane.SelectedPage = this.tabNavigationPageHeight;
            this.tabPane.Size = new System.Drawing.Size(1365, 686);
            this.tabPane.TabIndex = 15;
            this.tabPane.Text = "1";
            // 
            // tabNavigationPageHeight
            // 
            this.tabNavigationPageHeight.Caption = "    吸嘴测高    ";
            this.tabNavigationPageHeight.Controls.Add(this.ppTool_Height1);
            this.tabNavigationPageHeight.Name = "tabNavigationPageHeight";
            this.tabNavigationPageHeight.Size = new System.Drawing.Size(1365, 653);
            // 
            // ppTool_Height1
            // 
            this.ppTool_Height1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppTool_Height1.Location = new System.Drawing.Point(0, 0);
            this.ppTool_Height1.Name = "ppTool_Height1";
            this.ppTool_Height1.PPHeight = 0F;
            this.ppTool_Height1.Size = new System.Drawing.Size(1365, 653);
            this.ppTool_Height1.TabIndex = 0;
            // 
            // tabNavigationPageAlign
            // 
            this.tabNavigationPageAlign.Caption = "    XY原点确定    ";
            this.tabNavigationPageAlign.Controls.Add(this.ppTool_Alignment1);
            this.tabNavigationPageAlign.Name = "tabNavigationPageAlign";
            this.tabNavigationPageAlign.Size = new System.Drawing.Size(1365, 653);
            // 
            // ppTool_Alignment1
            // 
            this.ppTool_Alignment1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppTool_Alignment1.FirstCenterPosition = null;
            this.ppTool_Alignment1.Location = new System.Drawing.Point(0, 0);
            this.ppTool_Alignment1.Name = "ppTool_Alignment1";
            this.ppTool_Alignment1.SecondCenterPosition = null;
            this.ppTool_Alignment1.Size = new System.Drawing.Size(1365, 653);
            this.ppTool_Alignment1.TabIndex = 0;
            // 
            // tabNavigationPagePPESH
            // 
            this.tabNavigationPagePPESH.Caption = "  吸嘴-顶针座测高  ";
            this.tabNavigationPagePPESH.Controls.Add(this.ppesHeight1);
            this.tabNavigationPagePPESH.Name = "tabNavigationPagePPESH";
            this.tabNavigationPagePPESH.Size = new System.Drawing.Size(1365, 653);
            // 
            // ppesHeight1
            // 
            this.ppesHeight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppesHeight1.ESStageHeight = 0F;
            this.ppesHeight1.Location = new System.Drawing.Point(0, 0);
            this.ppesHeight1.Name = "ppesHeight1";
            this.ppesHeight1.ESZSystemHeight = 0F;
            this.ppesHeight1.Size = new System.Drawing.Size(1365, 653);
            this.ppesHeight1.TabIndex = 0;
            // 
            // tabNavigationPageTeach
            // 
            this.tabNavigationPageTeach.Caption = "示教";
            this.tabNavigationPageTeach.Controls.Add(this.seRotationTablePos4LoadPP);
            this.tabNavigationPageTeach.Controls.Add(this.seBondPosX4LoadPP);
            this.tabNavigationPageTeach.Controls.Add(this.seBondPosY4LoadPP);
            this.tabNavigationPageTeach.Controls.Add(this.seBondPosZ4LoadPP);
            this.tabNavigationPageTeach.Controls.Add(this.seBondPosT4LoadPP);
            this.tabNavigationPageTeach.Controls.Add(this.btnSaveConfig);
            this.tabNavigationPageTeach.Controls.Add(this.labelControl5);
            this.tabNavigationPageTeach.Controls.Add(this.labelControl4);
            this.tabNavigationPageTeach.Controls.Add(this.labelControl2);
            this.tabNavigationPageTeach.Controls.Add(this.labelControl6);
            this.tabNavigationPageTeach.Controls.Add(this.labelControl1);
            this.tabNavigationPageTeach.Controls.Add(this.labelControl3);
            this.tabNavigationPageTeach.Controls.Add(this.stageQuickMove1);
            this.tabNavigationPageTeach.Name = "tabNavigationPageTeach";
            this.tabNavigationPageTeach.Size = new System.Drawing.Size(1365, 653);
            // 
            // seRotationTablePos4LoadPP
            // 
            this.seRotationTablePos4LoadPP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seRotationTablePos4LoadPP.Location = new System.Drawing.Point(138, 211);
            this.seRotationTablePos4LoadPP.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seRotationTablePos4LoadPP.Name = "seRotationTablePos4LoadPP";
            this.seRotationTablePos4LoadPP.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seRotationTablePos4LoadPP.Properties.AutoHeight = false;
            this.seRotationTablePos4LoadPP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seRotationTablePos4LoadPP.Properties.DisplayFormat.FormatString = "0.0000";
            this.seRotationTablePos4LoadPP.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seRotationTablePos4LoadPP.Properties.EditFormat.FormatString = "0.0000";
            this.seRotationTablePos4LoadPP.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seRotationTablePos4LoadPP.Properties.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.seRotationTablePos4LoadPP.Properties.IsFloatValue = false;
            this.seRotationTablePos4LoadPP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seRotationTablePos4LoadPP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seRotationTablePos4LoadPP.Properties.MaskSettings.Set("mask", "f4");
            this.seRotationTablePos4LoadPP.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seRotationTablePos4LoadPP.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seRotationTablePos4LoadPP.Size = new System.Drawing.Size(94, 29);
            this.seRotationTablePos4LoadPP.TabIndex = 54;
            // 
            // seBondPosX4LoadPP
            // 
            this.seBondPosX4LoadPP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBondPosX4LoadPP.Location = new System.Drawing.Point(138, 136);
            this.seBondPosX4LoadPP.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seBondPosX4LoadPP.Name = "seBondPosX4LoadPP";
            this.seBondPosX4LoadPP.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seBondPosX4LoadPP.Properties.AutoHeight = false;
            this.seBondPosX4LoadPP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBondPosX4LoadPP.Properties.DisplayFormat.FormatString = "0.0000";
            this.seBondPosX4LoadPP.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosX4LoadPP.Properties.EditFormat.FormatString = "0.0000";
            this.seBondPosX4LoadPP.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosX4LoadPP.Properties.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.seBondPosX4LoadPP.Properties.IsFloatValue = false;
            this.seBondPosX4LoadPP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seBondPosX4LoadPP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seBondPosX4LoadPP.Properties.MaskSettings.Set("mask", "f4");
            this.seBondPosX4LoadPP.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seBondPosX4LoadPP.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seBondPosX4LoadPP.Size = new System.Drawing.Size(94, 29);
            this.seBondPosX4LoadPP.TabIndex = 54;
            // 
            // seBondPosY4LoadPP
            // 
            this.seBondPosY4LoadPP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBondPosY4LoadPP.Location = new System.Drawing.Point(249, 136);
            this.seBondPosY4LoadPP.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seBondPosY4LoadPP.Name = "seBondPosY4LoadPP";
            this.seBondPosY4LoadPP.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seBondPosY4LoadPP.Properties.AutoHeight = false;
            this.seBondPosY4LoadPP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBondPosY4LoadPP.Properties.DisplayFormat.FormatString = "0.0000";
            this.seBondPosY4LoadPP.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosY4LoadPP.Properties.EditFormat.FormatString = "0.0000";
            this.seBondPosY4LoadPP.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosY4LoadPP.Properties.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.seBondPosY4LoadPP.Properties.IsFloatValue = false;
            this.seBondPosY4LoadPP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seBondPosY4LoadPP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seBondPosY4LoadPP.Properties.MaskSettings.Set("mask", "f4");
            this.seBondPosY4LoadPP.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seBondPosY4LoadPP.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seBondPosY4LoadPP.Size = new System.Drawing.Size(94, 29);
            this.seBondPosY4LoadPP.TabIndex = 53;
            // 
            // seBondPosZ4LoadPP
            // 
            this.seBondPosZ4LoadPP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBondPosZ4LoadPP.Location = new System.Drawing.Point(358, 136);
            this.seBondPosZ4LoadPP.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seBondPosZ4LoadPP.Name = "seBondPosZ4LoadPP";
            this.seBondPosZ4LoadPP.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seBondPosZ4LoadPP.Properties.AutoHeight = false;
            this.seBondPosZ4LoadPP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBondPosZ4LoadPP.Properties.DisplayFormat.FormatString = "0.0000";
            this.seBondPosZ4LoadPP.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosZ4LoadPP.Properties.EditFormat.FormatString = "0.0000";
            this.seBondPosZ4LoadPP.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosZ4LoadPP.Properties.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.seBondPosZ4LoadPP.Properties.IsFloatValue = false;
            this.seBondPosZ4LoadPP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seBondPosZ4LoadPP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seBondPosZ4LoadPP.Properties.MaskSettings.Set("mask", "f4");
            this.seBondPosZ4LoadPP.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seBondPosZ4LoadPP.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seBondPosZ4LoadPP.Size = new System.Drawing.Size(94, 29);
            this.seBondPosZ4LoadPP.TabIndex = 52;
            // 
            // seBondPosT4LoadPP
            // 
            this.seBondPosT4LoadPP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seBondPosT4LoadPP.Location = new System.Drawing.Point(464, 136);
            this.seBondPosT4LoadPP.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.seBondPosT4LoadPP.Name = "seBondPosT4LoadPP";
            this.seBondPosT4LoadPP.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seBondPosT4LoadPP.Properties.AutoHeight = false;
            this.seBondPosT4LoadPP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seBondPosT4LoadPP.Properties.DisplayFormat.FormatString = "0.0000";
            this.seBondPosT4LoadPP.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosT4LoadPP.Properties.EditFormat.FormatString = "0.0000";
            this.seBondPosT4LoadPP.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seBondPosT4LoadPP.Properties.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.seBondPosT4LoadPP.Properties.IsFloatValue = false;
            this.seBondPosT4LoadPP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.seBondPosT4LoadPP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.seBondPosT4LoadPP.Properties.MaskSettings.Set("mask", "f4");
            this.seBondPosT4LoadPP.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.seBondPosT4LoadPP.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.seBondPosT4LoadPP.Size = new System.Drawing.Size(94, 29);
            this.seBondPosT4LoadPP.TabIndex = 51;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(596, 139);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(71, 22);
            this.btnSaveConfig.TabIndex = 50;
            this.btnSaveConfig.Text = "保存";
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Options.UseTextOptions = true;
            this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(490, 107);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(40, 29);
            this.labelControl5.TabIndex = 43;
            this.labelControl5.Text = "Theta";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Options.UseTextOptions = true;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(384, 107);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(26, 29);
            this.labelControl4.TabIndex = 43;
            this.labelControl4.Text = "Z";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(274, 107);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(26, 29);
            this.labelControl2.TabIndex = 44;
            this.labelControl2.Text = "Y";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Options.UseTextOptions = true;
            this.labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.Location = new System.Drawing.Point(23, 210);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(105, 29);
            this.labelControl6.TabIndex = 46;
            this.labelControl6.Text = "工具转盘Load位置:";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(163, 107);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(26, 29);
            this.labelControl1.TabIndex = 45;
            this.labelControl1.Text = "X";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(32, 135);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 29);
            this.labelControl3.TabIndex = 46;
            this.labelControl3.Text = "榜头Load位置:";
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(939, 76);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(269, 317);
            this.stageQuickMove1.TabIndex = 42;
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
            this.panelControl1.Controls.Add(this.btnCalibratePP);
            this.panelControl1.Controls.Add(this.btnUnloadPP);
            this.panelControl1.Controls.Add(this.btnLoadPP);
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
            // btnCalibratePP
            // 
            this.btnCalibratePP.Location = new System.Drawing.Point(685, 12);
            this.btnCalibratePP.Name = "btnCalibratePP";
            this.btnCalibratePP.Size = new System.Drawing.Size(94, 22);
            this.btnCalibratePP.TabIndex = 24;
            this.btnCalibratePP.Text = "自动校准";
            this.btnCalibratePP.Click += new System.EventHandler(this.btnCalibratePP_Click);
            // 
            // btnUnloadPP
            // 
            this.btnUnloadPP.Location = new System.Drawing.Point(569, 12);
            this.btnUnloadPP.Name = "btnUnloadPP";
            this.btnUnloadPP.Size = new System.Drawing.Size(71, 22);
            this.btnUnloadPP.TabIndex = 24;
            this.btnUnloadPP.Text = "Unload";
            this.btnUnloadPP.Click += new System.EventHandler(this.btnUnloadPP_Click);
            // 
            // btnLoadPP
            // 
            this.btnLoadPP.Location = new System.Drawing.Point(464, 12);
            this.btnLoadPP.Name = "btnLoadPP";
            this.btnLoadPP.Size = new System.Drawing.Size(71, 22);
            this.btnLoadPP.TabIndex = 24;
            this.btnLoadPP.Text = "Load";
            this.btnLoadPP.Click += new System.EventHandler(this.btnLoadPP_Click);
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
            this.labelControl21.Location = new System.Drawing.Point(32, 14);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(52, 14);
            this.labelControl21.TabIndex = 22;
            this.labelControl21.Text = "已有工具:";
            // 
            // FrmPPTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 792);
            this.Controls.Add(this.tableLayoutPanel1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmPPTool.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "FrmPPTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "吸嘴工具设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPPTool_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).EndInit();
            this.tabPane.ResumeLayout(false);
            this.tabNavigationPageHeight.ResumeLayout(false);
            this.tabNavigationPageAlign.ResumeLayout(false);
            this.tabNavigationPagePPESH.ResumeLayout(false);
            this.tabNavigationPageTeach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seRotationTablePos4LoadPP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosX4LoadPP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosY4LoadPP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosZ4LoadPP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seBondPosT4LoadPP.Properties)).EndInit();
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
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPageAlign;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnNewTool;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbExistTool;
        private PPTool_Alignment ppTool_Alignment1;
        private PPTool_Height ppTool_Height1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPagePPESH;
        private PPESHeight ppesHeight1;
        private DevExpress.XtraEditors.SimpleButton btnUnloadPP;
        private DevExpress.XtraEditors.SimpleButton btnLoadPP;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPageTeach;
        private DevExpress.XtraEditors.SimpleButton btnSaveConfig;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.SpinEdit seRotationTablePos4LoadPP;
        private DevExpress.XtraEditors.SpinEdit seBondPosX4LoadPP;
        private DevExpress.XtraEditors.SpinEdit seBondPosY4LoadPP;
        private DevExpress.XtraEditors.SpinEdit seBondPosZ4LoadPP;
        private DevExpress.XtraEditors.SpinEdit seBondPosT4LoadPP;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btnCalibratePP;
    }
}
