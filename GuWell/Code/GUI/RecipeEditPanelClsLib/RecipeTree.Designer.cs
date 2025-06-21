namespace RecipeEditPanelClsLib
{
    partial class RecipeTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecipeTree));
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barbtnNewProduct = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnEditRecipe = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnDeleteRecipe = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnRecipeCopy = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnRecipeRename = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            this.SuspendLayout();
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnNewProduct, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnEditRecipe),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnDeleteRecipe),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnRecipeCopy, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnRecipeRename, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barbtnNewProduct
            // 
            this.barbtnNewProduct.Caption = "新建";
            this.barbtnNewProduct.Id = 0;
            this.barbtnNewProduct.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnNewProduct.ImageOptions.Image")));
            this.barbtnNewProduct.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnNewProduct.ImageOptions.LargeImage")));
            this.barbtnNewProduct.Name = "barbtnNewProduct";
            this.barbtnNewProduct.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnNewProduct_ItemClick);
            // 
            // barbtnEditRecipe
            // 
            this.barbtnEditRecipe.Caption = "编辑";
            this.barbtnEditRecipe.Id = 1;
            this.barbtnEditRecipe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnEditRecipe.ImageOptions.Image")));
            this.barbtnEditRecipe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnEditRecipe.ImageOptions.LargeImage")));
            this.barbtnEditRecipe.Name = "barbtnEditRecipe";
            this.barbtnEditRecipe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnEditRecipe_ItemClick);
            // 
            // barbtnDeleteRecipe
            // 
            this.barbtnDeleteRecipe.Caption = "删除";
            this.barbtnDeleteRecipe.Id = 2;
            this.barbtnDeleteRecipe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnDeleteRecipe.ImageOptions.Image")));
            this.barbtnDeleteRecipe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnDeleteRecipe.ImageOptions.LargeImage")));
            this.barbtnDeleteRecipe.Name = "barbtnDeleteRecipe";
            this.barbtnDeleteRecipe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnDeleteRecipe_ItemClick);
            // 
            // barbtnRecipeCopy
            // 
            this.barbtnRecipeCopy.Caption = "复制";
            this.barbtnRecipeCopy.Id = 3;
            this.barbtnRecipeCopy.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnRecipeCopy.ImageOptions.Image")));
            this.barbtnRecipeCopy.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnRecipeCopy.ImageOptions.LargeImage")));
            this.barbtnRecipeCopy.Name = "barbtnRecipeCopy";
            this.barbtnRecipeCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnRecipeCopy_ItemClick);
            // 
            // barbtnRecipeRename
            // 
            this.barbtnRecipeRename.Caption = "重命名";
            this.barbtnRecipeRename.Id = 3;
            this.barbtnRecipeRename.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnRecipeRename.ImageOptions.Image")));
            this.barbtnRecipeRename.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnRecipeRename.ImageOptions.LargeImage")));
            this.barbtnRecipeRename.Name = "barbtnRecipeRename";
            this.barbtnRecipeRename.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnRecipeRename_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barbtnNewProduct,
            this.barbtnEditRecipe,
            this.barbtnDeleteRecipe,
            this.barbtnRecipeCopy,
            this.barbtnRecipeRename,
            this.barButtonItem1});
            this.barManager1.MaxItemId = 5;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(221, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 684);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(221, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 684);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(221, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 684);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 4;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "recipes.png");
            this.imageList1.Images.SetKeyName(1, "recipe.png");
            // 
            // treeList1
            // 
            this.treeList1.Appearance.FocusedCell.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.treeList1.Appearance.FocusedCell.Options.UseFont = true;
            this.treeList1.Appearance.FocusedRow.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.treeList1.Appearance.FocusedRow.Options.UseFont = true;
            this.treeList1.Appearance.Row.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.treeList1.Appearance.Row.Options.UseFont = true;
            this.treeList1.Appearance.SelectedRow.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.treeList1.Appearance.SelectedRow.Options.UseFont = true;
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeList1.MenuManager = this.barManager1;
            this.treeList1.Name = "treeList1";
            this.treeList1.BeginUnboundLoad();
            this.treeList1.AppendNode(new object[] {
            "GW"}, -1);
            this.treeList1.AppendNode(new object[] {
            "Bonder"}, 0);
            this.treeList1.EndUnboundLoad();
            this.treeList1.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList1.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Matches;
            this.treeList1.OptionsFind.AlwaysVisible = true;
            this.treeList1.OptionsFind.FindMode = DevExpress.XtraTreeList.FindMode.Always;
            this.treeList1.OptionsFind.ShowFindButton = false;
            this.treeList1.OptionsMenu.EnableColumnMenu = false;
            this.treeList1.OptionsMenu.ShowExpandCollapseItems = false;
            this.treeList1.OptionsView.ShowColumns = false;
            this.treeList1.OptionsView.ShowHorzLines = false;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.OptionsView.ShowVertLines = false;
            this.treeList1.SelectImageList = this.imageList1;
            this.treeList1.Size = new System.Drawing.Size(221, 684);
            this.treeList1.TabIndex = 6;
            this.treeList1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseClick);
            this.treeList1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseDoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "Name";
            this.treeListColumn1.FieldName = "RecipeName";
            this.treeListColumn1.MinWidth = 69;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // RecipeTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "RecipeTree";
            this.Size = new System.Drawing.Size(221, 684);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarButtonItem barbtnNewProduct;
        private DevExpress.XtraBars.BarButtonItem barbtnEditRecipe;
        private DevExpress.XtraBars.BarButtonItem barbtnDeleteRecipe;
        private DevExpress.XtraBars.BarButtonItem barbtnRecipeCopy;
        private DevExpress.XtraBars.BarButtonItem barbtnRecipeRename;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}
