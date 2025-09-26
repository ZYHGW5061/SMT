
namespace VisionGUI
{
    partial class CameraWindowGUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraWindowGUI));
            this.CameraWindow = new System.Windows.Forms.PictureBox();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barbtnCameraType = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuCameras = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barBtnSelBondCamera = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSelUplookCamera = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSelWaferCamera = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItemCross = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemGrid = new DevExpress.XtraBars.BarCheckItem();
            this.barBtnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonSaveImage = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.CameraWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuCameras)).BeginInit();
            this.SuspendLayout();
            // 
            // CameraWindow
            // 
            this.CameraWindow.Location = new System.Drawing.Point(0, 76);
            this.CameraWindow.Name = "CameraWindow";
            this.CameraWindow.Size = new System.Drawing.Size(682, 525);
            this.CameraWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CameraWindow.TabIndex = 0;
            this.CameraWindow.TabStop = false;
            this.CameraWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.CameraWindow_Paint);
            this.CameraWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CameraWindow_MouseMove);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AllowMinimizeRibbon = false;
            this.ribbonControl1.BackColor = System.Drawing.SystemColors.Control;
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
            this.ribbonControl1.DrawGroupsBorderMode = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.barbtnCameraType,
            this.barCheckItemCross,
            this.barCheckItemGrid,
            this.barBtnSelBondCamera,
            this.barBtnSelUplookCamera,
            this.barBtnSelWaferCamera,
            this.barBtnRefresh,
            this.barStaticItem1,
            this.barButtonSaveImage});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ribbonControl1.MaxItemId = 40;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(682, 77);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barbtnCameraType
            // 
            this.barbtnCameraType.ActAsDropDown = true;
            this.barbtnCameraType.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.barbtnCameraType.Caption = "选择相机";
            this.barbtnCameraType.DropDownControl = this.popupMenuCameras;
            this.barbtnCameraType.Id = 25;
            this.barbtnCameraType.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barbtnCameraType.ImageOptions.Image")));
            this.barbtnCameraType.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barbtnCameraType.ImageOptions.LargeImage")));
            this.barbtnCameraType.LargeWidth = 110;
            this.barbtnCameraType.Name = "barbtnCameraType";
            this.barbtnCameraType.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // popupMenuCameras
            // 
            this.popupMenuCameras.ItemLinks.Add(this.barBtnSelBondCamera);
            this.popupMenuCameras.ItemLinks.Add(this.barBtnSelUplookCamera);
            this.popupMenuCameras.ItemLinks.Add(this.barBtnSelWaferCamera);
            this.popupMenuCameras.Name = "popupMenuCameras";
            this.popupMenuCameras.Ribbon = this.ribbonControl1;
            // 
            // barBtnSelBondCamera
            // 
            this.barBtnSelBondCamera.Caption = "榜头相机";
            this.barBtnSelBondCamera.Id = 34;
            this.barBtnSelBondCamera.Name = "barBtnSelBondCamera";
            this.barBtnSelBondCamera.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSelBondCamera_ItemClick);
            // 
            // barBtnSelUplookCamera
            // 
            this.barBtnSelUplookCamera.Caption = "仰视相机";
            this.barBtnSelUplookCamera.Id = 35;
            this.barBtnSelUplookCamera.Name = "barBtnSelUplookCamera";
            this.barBtnSelUplookCamera.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSelUplookCamera_ItemClick);
            // 
            // barBtnSelWaferCamera
            // 
            this.barBtnSelWaferCamera.Caption = "晶圆相机";
            this.barBtnSelWaferCamera.Id = 36;
            this.barBtnSelWaferCamera.Name = "barBtnSelWaferCamera";
            this.barBtnSelWaferCamera.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSelWaferCamera_ItemClick);
            // 
            // barCheckItemCross
            // 
            this.barCheckItemCross.Caption = "十字";
            this.barCheckItemCross.Id = 32;
            this.barCheckItemCross.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barCheckItemCross.ImageOptions.Image")));
            this.barCheckItemCross.LargeWidth = 75;
            this.barCheckItemCross.Name = "barCheckItemCross";
            this.barCheckItemCross.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barCheckItemCross.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemCross_CheckedChanged);
            // 
            // barCheckItemGrid
            // 
            this.barCheckItemGrid.Caption = "网格";
            this.barCheckItemGrid.Id = 33;
            this.barCheckItemGrid.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barCheckItemGrid.ImageOptions.Image")));
            this.barCheckItemGrid.LargeWidth = 75;
            this.barCheckItemGrid.Name = "barCheckItemGrid";
            this.barCheckItemGrid.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barCheckItemGrid.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemGrid_CheckedChanged);
            // 
            // barBtnRefresh
            // 
            this.barBtnRefresh.Caption = "刷新";
            this.barBtnRefresh.Id = 37;
            this.barBtnRefresh.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barBtnRefresh.ImageOptions.Image")));
            this.barBtnRefresh.LargeWidth = 75;
            this.barBtnRefresh.Name = "barBtnRefresh";
            this.barBtnRefresh.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barBtnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnRefresh_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "(X:0,Y:0)(0)";
            this.barStaticItem1.Id = 38;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonPageGroup1.ItemLinks.Add(this.barbtnCameraType, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItemCross);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItemGrid);
            this.ribbonPageGroup1.ItemLinks.Add(this.barBtnRefresh);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonSaveImage);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.State = DevExpress.XtraBars.Ribbon.RibbonPageGroupState.Expanded;
            this.ribbonPageGroup1.Text = "File";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barStaticItem1);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // barButtonSaveImage
            // 
            this.barButtonSaveImage.Caption = "Save";
            this.barButtonSaveImage.Id = 39;
            this.barButtonSaveImage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonSaveImage.ImageOptions.Image")));
            this.barButtonSaveImage.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonSaveImage.ImageOptions.LargeImage")));
            this.barButtonSaveImage.LargeWidth = 75;
            this.barButtonSaveImage.Name = "barButtonSaveImage";
            this.barButtonSaveImage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSaveImage_ItemClick);
            // 
            // CameraWindowGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CameraWindow);
            this.Controls.Add(this.ribbonControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CameraWindowGUI";
            this.Size = new System.Drawing.Size(682, 604);
            ((System.ComponentModel.ISupportInitialize)(this.CameraWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuCameras)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CameraWindow;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem barbtnCameraType;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarCheckItem barCheckItemCross;
        private DevExpress.XtraBars.BarCheckItem barCheckItemGrid;
        private DevExpress.XtraBars.PopupMenu popupMenuCameras;
        private DevExpress.XtraBars.BarButtonItem barBtnSelBondCamera;
        private DevExpress.XtraBars.BarButtonItem barBtnSelUplookCamera;
        private DevExpress.XtraBars.BarButtonItem barBtnSelWaferCamera;
        private DevExpress.XtraBars.BarButtonItem barBtnRefresh;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonSaveImage;
    }
}
