namespace RecipeEditPanelClsLib
{
    partial class FrmRecipePrograming
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRecipePrograming));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barbtnNewRecipe = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnDeleteRecipe = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnEditRecipe = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCopy = new DevExpress.XtraBars.BarButtonItem();
            this.barReLoadRecipes = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.xtraTabControl1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1695, 863);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.barDockControlLeft);
            this.panelControl1.Controls.Add(this.barDockControlRight);
            this.panelControl1.Controls.Add(this.barDockControlBottom);
            this.panelControl1.Controls.Add(this.barDockControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(314, 857);
            this.panelControl1.TabIndex = 1;
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 833);
            // 
            // barManager1
            // 
            this.barManager1.AllowItemAnimatedHighlighting = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this.panelControl1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barbtnNewRecipe,
            this.barbtnDeleteRecipe,
            this.barbtnEditRecipe,
            this.barBtnCopy,
            this.barReLoadRecipes});
            this.barManager1.MaxItemId = 10;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnNewRecipe),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnDeleteRecipe),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnEditRecipe),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnCopy, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barReLoadRecipes, true)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 2";
            // 
            // barbtnNewRecipe
            // 
            this.barbtnNewRecipe.Caption = "New Recipe";
            this.barbtnNewRecipe.Id = 0;
            this.barbtnNewRecipe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnNewRecipe.ImageOptions.Image")));
            this.barbtnNewRecipe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnNewRecipe.ImageOptions.LargeImage")));
            this.barbtnNewRecipe.Name = "barbtnNewRecipe";
            this.barbtnNewRecipe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnNewRecipe_ItemClick);
            // 
            // barbtnDeleteRecipe
            // 
            this.barbtnDeleteRecipe.Caption = "Delete Recipe";
            this.barbtnDeleteRecipe.Id = 1;
            this.barbtnDeleteRecipe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnDeleteRecipe.ImageOptions.Image")));
            this.barbtnDeleteRecipe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnDeleteRecipe.ImageOptions.LargeImage")));
            this.barbtnDeleteRecipe.Name = "barbtnDeleteRecipe";
            this.barbtnDeleteRecipe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnDeleteRecipe_ItemClick);
            // 
            // barbtnEditRecipe
            // 
            this.barbtnEditRecipe.Caption = "Edit Recipe";
            this.barbtnEditRecipe.Id = 2;
            this.barbtnEditRecipe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnEditRecipe.ImageOptions.Image")));
            this.barbtnEditRecipe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnEditRecipe.ImageOptions.LargeImage")));
            this.barbtnEditRecipe.Name = "barbtnEditRecipe";
            this.barbtnEditRecipe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnEditRecipe_ItemClick);
            // 
            // barBtnCopy
            // 
            this.barBtnCopy.Caption = "Copy Recipe";
            this.barBtnCopy.Id = 3;
            this.barBtnCopy.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barBtnCopy.ImageOptions.Image")));
            this.barBtnCopy.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barBtnCopy.ImageOptions.LargeImage")));
            this.barBtnCopy.Name = "barBtnCopy";
            this.barBtnCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCopy_ItemClick);
            // 
            // barReLoadRecipes
            // 
            this.barReLoadRecipes.Caption = "Refresh";
            this.barReLoadRecipes.Id = 4;
            this.barReLoadRecipes.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barReLoadRecipes.ImageOptions.Image")));
            this.barReLoadRecipes.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barReLoadRecipes.ImageOptions.LargeImage")));
            this.barReLoadRecipes.Name = "barReLoadRecipes";
            this.barReLoadRecipes.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barReLoadRecipes_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(314, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 857);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(314, 0);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(314, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 833);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(323, 3);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabControl1.Size = new System.Drawing.Size(1369, 857);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.CloseButtonClick += new System.EventHandler(this.xtraTabControl1_CloseButtonClick);
            // 
            // FrmRecipePrograming
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1695, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRecipePrograming";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "配方编程";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barbtnNewRecipe;
        private DevExpress.XtraBars.BarButtonItem barbtnDeleteRecipe;
        private DevExpress.XtraBars.BarButtonItem barbtnEditRecipe;
        private DevExpress.XtraBars.BarButtonItem barBtnCopy;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraBars.BarButtonItem barReLoadRecipes;
    }
}
