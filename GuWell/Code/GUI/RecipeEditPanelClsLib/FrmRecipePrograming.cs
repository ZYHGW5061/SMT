using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTab;
using DevExpress.Utils;
using System.IO;
using DevExpress.XtraEditors;
using WestDragon.Framework.BaseLoggerClsLib;
using GlobalDataDefineClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using ConfigurationClsLib;
using RecipeClsLib;
using CommonPanelClsLib;
using GlobalToolClsLib;

namespace RecipeEditPanelClsLib
{
    public partial class FrmRecipePrograming : BaseForm
    {
        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_MOVE = 0xF010;
        /// <summary>
        /// Recipes管理树
        /// </summary>
        RecipeTree recipesManagerTree1 = null;
        private BondRecipe _currentRecipe;
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);

        //    if (m.Msg == WM_SYSCOMMAND && ((int)m.WParam == SC_MOVE))
        //    {
        //        // 不允许移动窗体
        //        // 可以在这里添加逻辑，比如弹出警告框
        //        return;
        //    }
        //}
        /// <summary>
        /// 系统配置
        /// </summary>
        protected SystemConfiguration _systemConfiguration
        {
            get { return SystemConfiguration.Instance; }
        }

        /// <summary>
        /// 存储创建的recipe编辑页面及recipe编辑控件对象
        /// </summary>
        private Dictionary<XtraTabPage, RecipeEditorView> _pageAndRcpControlDic;


        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmRecipePrograming()
        {
            //CreateWaitDialog();
            InitializeComponent();
            this._pageAndRcpControlDic = new Dictionary<XtraTabPage, RecipeEditorView>();
            //_systemConfiguration.SystemRunningModeChangedAct += OnSystemRuningModeChangedAct;
        }

        /// <summary>
        /// 初始化Recipe树
        /// </summary>
        void InitRecipeTree()
        {
            // 
            // recipesManagerTree1
            // 
            recipesManagerTree1 = new RecipeTree();
            this.recipesManagerTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recipesManagerTree1.Location = new System.Drawing.Point(0, 0);
            this.recipesManagerTree1.Name = "recipesManagerTree1";
            this.recipesManagerTree1.Size = new System.Drawing.Size(221, 339);
            this.recipesManagerTree1.TabIndex = 0;
            this.recipesManagerTree1.OnRecipeEditAct = new Action<BondRecipe>(OnRecipeEdited);
            this.panelControl1.Controls.Add(this.recipesManagerTree1);
        }

        /// <summary>
        /// 页面加载时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            InitRecipeTree();
            LoadRecipes();
        }
        protected void Initialize()
        {
            InitRecipeTree();
        }
        /// <summary>
        /// 异步加载Recipes
        /// </summary>
        private void LoadRecipes()
        {

            try
            {
                //加载所有 Recipes
                if (recipesManagerTree1 != null)
                {
                    recipesManagerTree1.LoadRecipesFromSystem();
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while loading recipe list！", ex);
            }
        }

        /// <summary>
        /// 在切换运行模式时更改快速上片控件的可见性
        /// </summary>
        /// <param name="mode"></param>
        //private void OnSystemRuningModeChangedAct(EnumSystemRunningMode mode)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke((MethodInvoker)delegate { this.OnSystemRuningModeChangedAct(mode); });
        //        return;
        //    }
        //}


        /// <summary>
        /// 新建一个Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnNewRecipe_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (recipesManagerTree1.OpenedRecipe != null && recipesManagerTree1.OpenedRecipe.Count >= 1)
            {
                XtraMessageBox.Show("Another recipe is editing now.", "Tips"); return;
            }

            //新建Recipe
            var recipeNew = this.recipesManagerTree1.AddNewRecipe();

            OnRecipeEdited(recipeNew);
        }

        /// <summary>
        /// 删除一个Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnDeleteRecipe_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //删除
                this.recipesManagerTree1.DeleteSelectedRecipe();
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while delete recipe！", ex);
            }

        }

        /// <summary>
        /// 编辑一个Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnEditRecipe_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.recipesManagerTree1.IsRecipeNodeSelected)
            {
                if (recipesManagerTree1.OpenedRecipe != null && recipesManagerTree1.OpenedRecipe.Count >= 1)
                {
                    XtraMessageBox.Show("Another recipe is editing now.", "Tips"); return;
                }

                OnRecipeEdited(this.recipesManagerTree1.RecipeSelected);
            }
        }

        /// <summary>
        /// 当编辑Recipe时
        /// </summary>
        /// <param name="recipe"></param>
        private void OnRecipeEdited(BondRecipe recipe)
        {
            if (recipe != null)
            {
                var pageText = string.Format("编辑配方 - 当前配方: {0}", recipe.RecipeName);
                var filterPage = xtraTabControl1.TabPages.FirstOrDefault(r => r.Text == pageText);
                if (filterPage != null)
                {
                    xtraTabControl1.SelectedTabPage = filterPage;
                    return;
                }

                XtraTabPage page = new XtraTabPage();
                page.ShowCloseButton = DefaultBoolean.True;
                page.Text = pageText;
                RecipeEditorView editRcpCtrl = new RecipeEditorView(recipe) { Dock = DockStyle.Fill };
                page.Controls.Add(editRcpCtrl);
                xtraTabControl1.TabPages.Add(page);
                xtraTabControl1.SelectedTabPage = page;
                this._pageAndRcpControlDic.Add(page, editRcpCtrl);
                recipesManagerTree1.OpenedRecipe = GetCurrentOpendRecipeName();
                _currentRecipe = recipe;
                //editRcpCtrl.ChangetoRecipeNode("配置", EnumRecipeRootStep.None);
                if (_systemConfiguration.SystemRunningType == EnumRunningType.Actual)
                {
                    PositioningSystemClsLib.PositioningSystem.Instance.SetAxisSpeedForProgram();
                }

            }
        }

        /// <summary>
        /// 复制选中的Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.recipesManagerTree1.CopySelectedRecipe();
        }

        /// <summary>
        /// 结束编辑当前Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage != null)
            {
                if (WarningBox.FormShow("动作确认", "是否确认结束配方编辑？", "提示") == 1)
                {
                    //当页面关闭时，取消此页面对wafer align状态变化的订阅,针对同时打开多个recipe的情况
                    if (this._pageAndRcpControlDic.Keys.Contains(xtraTabControl1.SelectedTabPage))
                    {
                        this._pageAndRcpControlDic.Remove(xtraTabControl1.SelectedTabPage);
                    }


                    xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                    recipesManagerTree1.OpenedRecipe = GetCurrentOpendRecipeName();
                }
            }
        }

        /// <summary>
        /// 重新加载Recipe树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barReLoadRecipes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadRecipes();
        }
        private List<string> GetCurrentOpendRecipeName()
        {
            List<string> recipeNames = new List<string>();
            if (xtraTabControl1.TabPages.Count > 0)
            {
                foreach (XtraTabPage page in xtraTabControl1.TabPages)
                {
                    recipeNames.Add(page.Text.Substring(13, page.Text.Length - 13));
                }
            }
            return recipeNames;
        }

        //private void barQuickLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    LoadPortControl quickLoad = new LoadPortControl();
        //    quickLoad.isMultipleSelection = false;
        //    if (quickLoad.ShowDialog() == DialogResult.OK)
        //    {
        //        //WarningFormCL.WarningBox.FormShow();
        //        var selectStripInfos = quickLoad.GetSelectedStrips();
        //        try
        //        {
        //            CreateWaitDialog();
        //            MaterialTransferExecutor executor = new MaterialTransferExecutor();

        //            if (_currentRecipe != null)
        //            {
        //                if (WarningFormCL.WarningBox.FormShow("动作确认", "请确认配方已设定并保存升降台的上料位置！", "提示") == 1)
        //                {
        //                    if (selectStripInfos.Count > 0)
        //                    {
        //                        var info = new JobMaterialStripInfo()
        //                        {
        //                            Strip = selectStripInfos[0],
        //                            RecipeName = _currentRecipe.RecipeName,
        //                            ProcessRecipe = _currentRecipe,
        //                            //LotID = ProcedureInfo.LotID,
        //                            StripID = selectStripInfos[0].StripID,
        //                            Index = Convert.ToInt32(selectStripInfos[0].StripIndex)
        //                        };
        //                        executor.ExecuteQuickTransferStrip(info);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                WarningFormCL.WarningBox.FormShow("错误", "请先选择配方，并确定已设定、保存升降台的上料位置！", "提示");
        //            }

        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //        finally
        //        {
        //            CloseWaitDlg();
        //            quickLoad.Dispose();
        //            quickLoad = null;
        //        }
        //    }
        //}

        //private void barBtnClearStrip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        MaterialTransferExecutor executor = new MaterialTransferExecutor();

        //        if (_currentRecipe != null)
        //        {
        //            if (WarningFormCL.WarningBox.FormShow("动作确认", "请确认料条在焊台上！", "提示") == 1)
        //            {
        //                CreateWaitDialog();
        //                JobMaterialStripInfo manualJobWaferInfo = new JobMaterialStripInfo()
        //                {
        //                    Index = 1,
        //                    //ProcessRecipe = currentRecipe,
        //                    //RecipeName = currentRecipe.RecipeName,
        //                    //LotID = procedureInfo.LotID,
        //                    Strip = new MaterialStripInfo() { StripID = "", StripIndex = "1", MaterialBoxID = "1" }
        //                };
        //                executor.ClearCompletedAct += ClearComplete;
        //                executor.ExecuteClearStrips(manualJobWaferInfo);
        //            }
        //            else
        //            {
        //                WarningFormCL.WarningBox.FormShow("错误", "请先选择配方，并确定已设定、保存升降台的上料位置！", "提示");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //}
        //private void ClearComplete()
        //{
        //    CloseWaitDlg();
        //}
    }
}
