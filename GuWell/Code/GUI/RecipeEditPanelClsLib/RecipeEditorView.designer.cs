namespace RecipeEditPanelClsLib
{
    partial class RecipeEditorView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecipeEditorView));
            this.barbtnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeRecipeNodes = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lblsubViewCaption = new DevExpress.XtraEditors.LabelControl();
            this.recipeNodeContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeChildNodes = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.popupMenu2 = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeRecipeNodes)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeChildNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            this.SuspendLayout();
            // 
            // barbtnAdd
            // 
            this.barbtnAdd.Caption = "添加";
            this.barbtnAdd.Id = 0;
            this.barbtnAdd.Name = "barbtnAdd";
            this.barbtnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnAdd_ItemClick);
            // 
            // barbtnDelete
            // 
            this.barbtnDelete.Caption = "删除";
            this.barbtnDelete.Id = 0;
            this.barbtnDelete.Name = "barbtnDelete";
            this.barbtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnDelete_ItemClick);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnAdd)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barManager1
            // 
            this.barManager1.AllowItemAnimatedHighlighting = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barbtnAdd,
            this.barbtnDelete});
            this.barManager1.MaxItemId = 10;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(924, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 540);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(924, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 540);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(924, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 540);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "2.bmp");
            this.imageList1.Images.SetKeyName(1, "3.bmp");
            this.imageList1.Images.SetKeyName(2, "1.bmp");
            this.imageList1.Images.SetKeyName(3, "icons8-complete-48.png");
            this.imageList1.Images.SetKeyName(4, "icons8-error-48.png");
            // 
            // treeRecipeNodes
            // 
            this.treeRecipeNodes.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeRecipeNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeRecipeNodes.Location = new System.Drawing.Point(2, 2);
            this.treeRecipeNodes.Margin = new System.Windows.Forms.Padding(2);
            this.treeRecipeNodes.Name = "treeRecipeNodes";
            this.treeRecipeNodes.BeginUnboundLoad();
            this.treeRecipeNodes.AppendNode(new object[] {
            "基板"}, -1);
            this.treeRecipeNodes.AppendNode(new object[] {
            "贴装位置"}, -1);
            this.treeRecipeNodes.AppendNode(new object[] {
            "芯片"}, -1);
            this.treeRecipeNodes.AppendNode(new object[] {
            "划胶器"}, -1);
            this.treeRecipeNodes.AppendNode(new object[] {
            "胶水设置"}, -1);
            this.treeRecipeNodes.AppendNode(new object[] {
            "工艺列表"}, -1);
            this.treeRecipeNodes.EndUnboundLoad();
            this.treeRecipeNodes.OptionsBehavior.PopulateServiceColumns = true;
            this.treeRecipeNodes.OptionsDragAndDrop.ExpandNodeOnDrag = false;
            this.treeRecipeNodes.OptionsLayout.StoreAppearance = true;
            this.treeRecipeNodes.OptionsMenu.EnableColumnMenu = false;
            this.treeRecipeNodes.OptionsMenu.EnableFooterMenu = false;
            this.treeRecipeNodes.OptionsMenu.EnableNodeMenu = false;
            this.treeRecipeNodes.OptionsMenu.ShowAutoFilterRowItem = false;
            this.treeRecipeNodes.OptionsView.ShowColumns = false;
            this.treeRecipeNodes.OptionsView.ShowHorzLines = false;
            this.treeRecipeNodes.OptionsView.ShowIndicator = false;
            this.treeRecipeNodes.OptionsView.ShowSummaryFooter = true;
            this.treeRecipeNodes.OptionsView.ShowVertLines = false;
            this.treeRecipeNodes.RowHeight = 40;
            this.tableLayoutPanel1.SetRowSpan(this.treeRecipeNodes, 2);
            this.treeRecipeNodes.SelectImageList = this.imageList1;
            this.treeRecipeNodes.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.Default;
            this.treeRecipeNodes.Size = new System.Drawing.Size(258, 251);
            this.treeRecipeNodes.TabIndex = 3;
            this.treeRecipeNodes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeRecipeNodes_MouseClick);
            this.treeRecipeNodes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeRecipeNodes_MouseDoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColumn1.AppearanceCell.Options.UseFont = true;
            this.treeListColumn1.Caption = "treeListColumn1";
            this.treeListColumn1.FieldName = "RecipeNodeName";
            this.treeListColumn1.MinWidth = 100;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowMove = false;
            this.treeListColumn1.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn1.OptionsColumn.AllowSize = false;
            this.treeListColumn1.OptionsColumn.AllowSort = false;
            this.treeListColumn1.OptionsColumn.FixedWidth = true;
            this.treeListColumn1.OptionsColumn.ReadOnly = true;
            this.treeListColumn1.OptionsColumn.ShowInCustomizationForm = false;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 185;
            // 
            // lblsubViewCaption
            // 
            this.lblsubViewCaption.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblsubViewCaption.Appearance.Options.UseFont = true;
            this.lblsubViewCaption.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblsubViewCaption.Location = new System.Drawing.Point(264, 2);
            this.lblsubViewCaption.Margin = new System.Windows.Forms.Padding(2);
            this.lblsubViewCaption.Name = "lblsubViewCaption";
            this.lblsubViewCaption.Size = new System.Drawing.Size(658, 33);
            this.lblsubViewCaption.TabIndex = 9;
            // 
            // recipeNodeContainer
            // 
            this.recipeNodeContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recipeNodeContainer.Location = new System.Drawing.Point(264, 39);
            this.recipeNodeContainer.Margin = new System.Windows.Forms.Padding(2);
            this.recipeNodeContainer.Name = "recipeNodeContainer";
            this.tableLayoutPanel1.SetRowSpan(this.recipeNodeContainer, 2);
            this.recipeNodeContainer.Size = new System.Drawing.Size(658, 499);
            this.recipeNodeContainer.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 262F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.treeChildNodes, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.recipeNodeContainer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblsubViewCaption, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.treeRecipeNodes, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 285F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(924, 540);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeChildNodes
            // 
            this.treeChildNodes.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeChildNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeChildNodes.Location = new System.Drawing.Point(2, 257);
            this.treeChildNodes.Margin = new System.Windows.Forms.Padding(2);
            this.treeChildNodes.Name = "treeChildNodes";
            this.treeChildNodes.OptionsBehavior.PopulateServiceColumns = true;
            this.treeChildNodes.OptionsDragAndDrop.ExpandNodeOnDrag = false;
            this.treeChildNodes.OptionsLayout.StoreAppearance = true;
            this.treeChildNodes.OptionsMenu.EnableColumnMenu = false;
            this.treeChildNodes.OptionsMenu.EnableFooterMenu = false;
            this.treeChildNodes.OptionsMenu.ShowAutoFilterRowItem = false;
            this.treeChildNodes.OptionsView.ShowColumns = false;
            this.treeChildNodes.OptionsView.ShowHorzLines = false;
            this.treeChildNodes.OptionsView.ShowIndicator = false;
            this.treeChildNodes.OptionsView.ShowSummaryFooter = true;
            this.treeChildNodes.OptionsView.ShowVertLines = false;
            this.treeChildNodes.RowHeight = 40;
            this.treeChildNodes.SelectImageList = this.imageList1;
            this.treeChildNodes.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.Default;
            this.treeChildNodes.Size = new System.Drawing.Size(258, 281);
            this.treeChildNodes.TabIndex = 12;
            this.treeChildNodes.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeChildNodes_FocusedNodeChanged);
            this.treeChildNodes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeChildNodes_MouseDoubleClick);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColumn2.AppearanceCell.Options.UseFont = true;
            this.treeListColumn2.Caption = "treeListColumn1";
            this.treeListColumn2.FieldName = "RecipeNodeName";
            this.treeListColumn2.MinWidth = 100;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowMove = false;
            this.treeListColumn2.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn2.OptionsColumn.AllowSize = false;
            this.treeListColumn2.OptionsColumn.AllowSort = false;
            this.treeListColumn2.OptionsColumn.FixedWidth = true;
            this.treeListColumn2.OptionsColumn.ReadOnly = true;
            this.treeListColumn2.OptionsColumn.ShowInCustomizationForm = false;
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            this.treeListColumn2.Width = 185;
            // 
            // popupMenu2
            // 
            this.popupMenu2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnDelete)});
            this.popupMenu2.Manager = this.barManager1;
            this.popupMenu2.Name = "popupMenu2";
            // 
            // RecipeEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "RecipeEditorView";
            this.Size = new System.Drawing.Size(924, 540);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeRecipeNodes)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeChildNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarButtonItem barbtnAdd;
        private DevExpress.XtraBars.BarButtonItem barbtnDelete;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel recipeNodeContainer;
        private DevExpress.XtraEditors.LabelControl lblsubViewCaption;
        private DevExpress.XtraTreeList.TreeList treeRecipeNodes;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraTreeList.TreeList treeChildNodes;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraBars.PopupMenu popupMenu2;
    }
}
