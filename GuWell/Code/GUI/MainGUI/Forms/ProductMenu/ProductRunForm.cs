using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using ProductRunClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WestDragon.Framework.UtilityHelper;
using static GlobalToolClsLib.GlobalCommFunc;

namespace MainGUI.Forms.ProductMenu
{
    public partial class ProductRunForm : DevExpress.XtraEditors.XtraForm
    {

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        // 定义键盘钩子类型
        private const int WH_KEYBOARD_LL = 13;
        // 定义键盘按下消息
        private const int WM_KEYDOWN = 0x0100;
        // 定义键盘释放消息
        private const int WM_KEYUP = 0x0101;
        //生产配方列表
        public List<string> ProdRecipeNameList;
        public BondRecipe curRecipe = null;

        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        private ProductExecutor productExecutor = ProductExecutor.Instance;

        public ProductRunForm()
        {
            InitializeComponent();
            productExecutor.PropertyChanged += new PropertyChangedEventHandler(chgRunStat);
            ProdRecipeNameList = getRecipeNameList();
            fillProductList();
            _hookID = SetHook(_proc);

        }
        private void ProductRunForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
        }
        private void fillProductList()
        {

            cbProductList.DataSource = ProdRecipeNameList;
            cbProductList.SelectedIndex = -1;
            this.cbProductList.SelectedValueChanged += new System.EventHandler(this.cbProductList_SelectedValueChanged);

        }

        private void fillTreeAction()
        {            
            //treeActionStat.OptionsView.RowImagesShowMode = DevExpress.XtraTreeList.RowImagesShowMode.Default;
            //treeActionStat.SelectImageList = imageList1;
            //treeActionStat.ImageIndexFieldName = "ActionStat";

            //treeActionStat.DataSource = productExecutor.StepActions;
            //treeActionStat.RefreshDataSource();
        }

        //生产配方下拉变更
        private void cbProductList_SelectedValueChanged(object sender, System.EventArgs e)
        {

            if (cbProductList.SelectedIndex == -1)
            {
                //productExecutor.LoadProductConfig(null);
                //fillTreeAction();
                //treeActionStat.Refresh();
                return;
            }

            productExecutor.AllActions.Clear();
            productExecutor.StepActions.Clear();
            productExecutor.RunningActionIndex = 0;

            string recipeName = cbProductList.SelectedItem.ToString();
            curRecipe = BondRecipe.LoadRecipe(recipeName);

            productExecutor.LoadProductRecipe(curRecipe);
            fillTreeAction();
            //treeActionStat.Refresh();
            //btnNextSubNum.Enabled = true;
            //btnPreSubNum.Enabled = true;
            //lbCurSubNum.Text = productExecutor.CurSubstrateNum.ToString();
            //lbMaxSubNum.Text = productExecutor.MaxSubNum.ToString();
        }

        public void chgRunStat(object sender, PropertyChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, PropertyChangedEventArgs>(chgRunStat), sender,e);
                return;
            }

            if (e.PropertyName == "ActionStat")
            {
                //treeActionStat.Refresh();
            }
            if (e.PropertyName == "BondCounter")
            {
                if (!ckeIsProcessPart.Checked)
                {
                    var bpCountInOneModule = curRecipe.StepBondingPositionList.Count;
                    var allMoudleCounts = curRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * curRecipe.SubstrateInfos.SubstrateMapInfos.Count;
                    var allBPCounts = curRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * curRecipe.SubstrateInfos.SubstrateMapInfos.Count * bpCountInOneModule;
                    labelCounter.Text = $"{productExecutor.BondDieCounter}/{allBPCounts}";
                }
                else
                {
                    labelCounter.Text = $"{productExecutor.BondDieCounter}/{Int32.Parse(seProcessCount.Text)}";
                }
            }
            if (e.PropertyName == "CurSubNum")
            {
                //lbCurSubNum.Text = productExecutor.CurSubstrateNum.ToString();
                //if (productExecutor.RunStat == EnumProductRunStat.AutoRun)
                //{
                    fillTreeAction();
                //}
                //treeActionStat.Refresh();

            }

            if (e.PropertyName == "RunStat")
            {
                if (productExecutor.RunStat == EnumProductRunStat.Stop || productExecutor.RunStat == EnumProductRunStat.Completed || productExecutor.RunStat == EnumProductRunStat.UserAbort)
                {
                    btnAutoStart.Enabled = true;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;

                    btnStop.Enabled = false;

                    cbProductList.Enabled = true;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.AutoRun)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = true;
                    btnAutoContinue.Enabled = false;

                    btnStop.Enabled = true;

                    cbProductList.Enabled = false;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.AutoPause)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = true;

                    btnStop.Enabled = true;

                    cbProductList.Enabled = false;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.StepRun)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;

                    btnStop.Enabled = true;

                    cbProductList.Enabled = false;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.StepPause)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;

                    btnStop.Enabled = true;

                    cbProductList.Enabled = false;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.NoProd)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;

                    btnStop.Enabled = false;

                    cbProductList.Enabled = true;
                }
            }
        }

        //获取生产配方列表
        private List<string> getRecipeNameList()
        {
            List<string> list = new List<string>();
            string recipeDir = _systemConfig.SystemDefaultDirectory + @"Recipes\Bonder";
            CommonProcess.EnsureFolderExist(recipeDir);
            CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\{1}\Components\", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString()));
            CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\{1}\BondPositions\", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString()));
            var recipeFiles = Directory.GetDirectories(recipeDir);
            for (int recipeIndex = 0; recipeIndex < recipeFiles.Length; recipeIndex++)
            {
                var recipeName = Path.GetFileName(recipeFiles[recipeIndex]);
                if (recipeName != "BondPositions" && recipeName != "Components" && recipeName != "EpoxyApplication")
                {
                    list.Add(recipeName);
                }
            }
            return list;
        }

        //点击行指定重做的行号
        private void treeActionStat_RowClick(object sender, DevExpress.XtraTreeList.RowClickEventArgs e)
        {
            productExecutor.RedoStepIndex = e.Node.Id;
        }

        private void btnNextSubNum_Click(object sender, System.EventArgs e)
        {
            if (productExecutor.CurSubstrateNum == productExecutor.MaxSubNum)
            {
                return;
            }
            productExecutor.CurSubstrateNum += 1;
            productExecutor.SetSubNumActions();
            fillTreeAction();
        }

        private void btnPreSubNum_Click(object sender, System.EventArgs e)
        {
            if (productExecutor.CurSubstrateNum == 1)
            {
                return;
            }
            productExecutor.CurSubstrateNum -= 1;
            productExecutor.SetSubNumActions();
            fillTreeAction();
        }

        // 设置钩子的方法
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // 钩子回调函数
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    // 键盘按下事件处理
                    int vkCode = Marshal.ReadInt32(lParam);
                    if (vkCode == 120)  //F9开启单步
                    {
                        ProductExecutor.Instance.SingleStepRun = true;
                        SingleStepRunUtility.Instance.EnableSingleStep = true;
                    }
                    else if(vkCode == 121)   //F10下一步
                    {
                        ProductExecutor.Instance.SetEventWaitForNext();
                        SingleStepRunUtility.Instance.Continue();
                    }
                    else if (vkCode == 119)   //F8取消单步
                    {
                        ProductExecutor.Instance.SingleStepRun = false;
                        SingleStepRunUtility.Instance.EnableSingleStep = false;
                        ProductExecutor.Instance.SetEventWaitForNext();
                    }
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    // 键盘释放事件处理
                    int vkCode = Marshal.ReadInt32(lParam);
                    Console.WriteLine($"Key Up: {vkCode}");
                }
            }
            // 将消息传递给下一个钩子或窗口过程
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // Windows API 函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private void btnPositionSubstrate_Click(object sender, EventArgs e)
        {
            StepAction_PositionSubstrate stepAction_PositionSubstrate = new StepAction_PositionSubstrate(null, EnumActionNo.Action_PositionSubstrate, "基板定位");
            var ret = stepAction_PositionSubstrate.Run();
        }

        private void btnPositionBondPosition_Click(object sender, EventArgs e)
        {
            StepAction_PositionBondPos stepAction_PositionBP = new StepAction_PositionBondPos(null, EnumActionNo.Action_RecognizeBondPos, "定位贴装位置");
            var ret = stepAction_PositionBP.Run();
        }

        private void btnPositionComponent_Click(object sender, EventArgs e)
        {
            PositioningSystemClsLib.PositioningSystem.Instance.BondMovetoSafeLocation();
            ProductExecutor.Instance.CurChipNum = 4;
            StepAction_PositionComponent stepAction_PositionChip = new StepAction_PositionComponent(ProductExecutor.Instance.ProductRecipe.ProductSteps[0], EnumActionNo.Action_PositionChip, "定位芯片");
            var ret = stepAction_PositionChip.Run();
        }

        private void btnPickUpComponent_Click(object sender, EventArgs e)
        {
            ProductExecutor.Instance.CurChipNum = 4;
            StepAction_PickUpChip stepAction_PickChip = new StepAction_PickUpChip(ProductExecutor.Instance.ProductRecipe.ProductSteps[0], EnumActionNo.Action_PositionChip, "拾取芯片");
            var ret = stepAction_PickChip.Run();
        }

        private void btnAccuracyComponent_Click(object sender, EventArgs e)
        {

            ProductExecutor.Instance.CurChipNum = 4;
            StepAction_MoveToLookupCamPos stepAction_MoveToLookupCamPos = new StepAction_MoveToLookupCamPos(ProductExecutor.Instance.ProductRecipe.ProductSteps[0], EnumActionNo.Action_MoveToLookupCamPos, "移动到仰视相机上方");
            var ret = stepAction_MoveToLookupCamPos.Run();

            StepAction_AccuracyPositionChip stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionChip(ProductExecutor.Instance.ProductRecipe.ProductSteps[0], EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
            ret = stepAction_AccuracyCalibrationChip.Run();
        }

        private void btnDispense_Click(object sender, EventArgs e)
        {
            PositioningSystemClsLib.PositioningSystem.Instance.BondMovetoSafeLocation();
            StepAction_Dispense stepAction_Dispense = new StepAction_Dispense(ProductExecutor.Instance.ProductRecipe.ProductSteps[0], EnumActionNo.Action_Dispense, "划胶");
            var ret = stepAction_Dispense.Run();
        }

        private void btnDondDie_Click(object sender, EventArgs e)
        {
            PositioningSystemClsLib.PositioningSystem.Instance.BondMovetoSafeLocation();
            ProductExecutor.Instance.CurSubstrateNum = 1;
            ProductExecutor.Instance.CurModuleNum = 1;
            StepAction_BondChip stepAction_BondChip = new StepAction_BondChip(ProductExecutor.Instance.ProductRecipe.ProductSteps[0], EnumActionNo.Action_BondChip, "固晶");
            var ret = stepAction_BondChip.Run();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lbCurSubNum_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnAutoStart_Click(object sender, EventArgs e)
        {
            string recipeName = cbProductList.Text.ToString();
            if(string.IsNullOrEmpty(recipeName))
            {
                WarningBox.FormShow("警告", "配方为空，请先选择配方！");
                return;
            }
            curRecipe = BondRecipe.LoadRecipe(recipeName);

            productExecutor.LoadProductRecipe(curRecipe);
            if (ProductExecutor.Instance.ProductRecipe == null)
            {
                WarningBox.FormShow("警告", "配方为空，请先选择配方！");
                return;
            }
            if (ProductExecutor.Instance.IsActionRunning)
            {
                WarningBox.FormShow("动作未终止，请等停止后操作", "警告");
                return;
            }
            if (WarningBox.FormShow("动作确认", "是否开始自动生产？") == 1)
            {
                //PositioningSystemClsLib.PositioningSystem.Instance.SetAxisSpeedForProduct();
                productExecutor.RunStat = EnumProductRunStat.AutoRun;
                ProductExecutor.Instance.ManualSettedProcessCount = Int32.Parse(seProcessCount.Text);
                ProductExecutor.Instance.CurChipNum = Int32.Parse(seStartChipIndex.Text);
                ProductExecutor.Instance.IsProcessPart = ckeIsProcessPart.Checked;
                Task.Factory.StartNew(new Action(() =>
                {
                    //productExecutor.MoveToSafePos();
                    productExecutor.StartRun();
                    chgRunStat(this, new PropertyChangedEventArgs("BondCounter"));
                }));
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            productExecutor.RunStat = EnumProductRunStat.UserAbort;
            productExecutor.RunningActionIndex = 0;
            productExecutor.ResetActionStat();
        }
    }
}