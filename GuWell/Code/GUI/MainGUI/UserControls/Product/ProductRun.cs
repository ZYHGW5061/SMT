using CommonPanelClsLib;
using GlobalToolClsLib;
using ProductRunClsLib;
using RecipeClsLib;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainGUI.UserControls.Product
{
    public partial class ProductRun : UserControl
    {
        private ProductExecutor productExecutor = ProductExecutor.Instance;

        public ProductRun()
        {
            InitializeComponent();
            productExecutor.PropertyChanged += new PropertyChangedEventHandler(chgRunStat);
        }

        public void LoadProductConfig(BondRecipe config)
        {
            productExecutor.LoadProductRecipe(config);
        }

        /*
         * 自动运行开始
         */
        private void btnAutoStart_Click(object sender, EventArgs e)
        {
            if(ProductExecutor.Instance.ProductRecipe==null)
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
                Task.Factory.StartNew(new Action(() =>
                {
                    //productExecutor.MoveToSafePos();
                    productExecutor.StartRun();
                }));
            }
        }

        /*
         * 自动运行-暂停
         */
        private void btnAutoPause_Click(object sender, EventArgs e)
        {
            productExecutor.RunStat = EnumProductRunStat.AutoPause;
        }

        /*
         * 自动运行-继续
         */
        private void btnAutoContinue_Click(object sender, EventArgs e)
        {
            if (ProductExecutor.Instance.IsActionRunning)
            {
                WarningBox.FormShow("动作未终止，请等停止后操作", "警告");
                return;
            }
            productExecutor.RunStat = EnumProductRunStat.AutoRun;
            //Task.Factory.StartNew(new Action(() =>
            //{
            //    productExecutor.Execute();
            //}));
        }

        /*
         * 手动运行开始
         */
        private void btnManualStart_Click(object sender, EventArgs e)
        {
            if (ProductExecutor.Instance.IsActionRunning)
            {
                WarningBox.FormShow("动作未终止，请等停止后操作", "警告");
                return;
            }
            productExecutor.RunStat = EnumProductRunStat.StepPause;
            productExecutor.RunningActionIndex = 0;
            productExecutor.CurSubstrateNum = 1;
            productExecutor.ResetActionStat();
        }

        /*
        * 手动运行-下一步执行
        */
        private void btnManualNext_Click(object sender, EventArgs e)
        {
            if (ProductExecutor.Instance.IsActionRunning)
            {
                WarningBox.FormShow("动作未终止，请等停止后操作", "警告");
                return;
            }
            productExecutor.RunStat = EnumProductRunStat.StepRun;
            productExecutor.ExecuteStep();
        }

        /*
       * 手动运行-重做当前步
       */
        private void btnRedoStep_Click(object sender, EventArgs e)
        {
            //if (ProductExecutor.Instance.IsActionRunning)
            //{
            //    WarningBox.FormShow("动作未终止，请等停止后操作", "警告");
            //    return;
            //}
            //productExecutor.RunStat = EnumProductRunStat.StepRun;
            ////productExecutor.RunningStepIndex--;
            //productExecutor.ExecuteSelectedStep(productExecutor.RedoStepIndex);
        }

        /*
       * 停止运行
       */
        private void btnStop_Click(object sender, EventArgs e)
        {
            productExecutor.RunStat = EnumProductRunStat.UserAbort;
            productExecutor.RunningActionIndex = 0;
            productExecutor.ResetActionStat();
        }

        public void chgRunStat(object sender, PropertyChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, PropertyChangedEventArgs>(chgRunStat), sender, e);
                return;
            }

            if (e.PropertyName == "RunStat")
            {
               if (productExecutor.RunStat == EnumProductRunStat.Stop|| productExecutor.RunStat == EnumProductRunStat.UserAbort)
                {
                    btnAutoStart.Enabled = true;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;
                    btnManualStart.Enabled = true;
                    btnManualNext.Enabled = false;
                    btnRedoStep.Enabled = false;
                    btnStop.Enabled = false;
                }
               else if (productExecutor.RunStat == EnumProductRunStat.AutoRun)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = true;
                    btnAutoContinue.Enabled = false;
                    btnManualStart.Enabled = false;
                    btnManualNext.Enabled = false;
                    btnRedoStep.Enabled = false;
                    btnStop.Enabled = true;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.AutoPause)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = true;
                    btnManualStart.Enabled = false;
                    btnManualNext.Enabled = false;
                    btnRedoStep.Enabled = false;
                    btnStop.Enabled = true;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.StepRun)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;
                    btnManualStart.Enabled = false;
                    btnManualNext.Enabled = false;
                    btnRedoStep.Enabled = false;
                    btnStop.Enabled = true;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.StepPause)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;
                    btnManualStart.Enabled = false;
                    btnManualNext.Enabled = true;
                    btnRedoStep.Enabled = true;
                    btnStop.Enabled = true;
                }
                else if (productExecutor.RunStat == EnumProductRunStat.NoProd)
                {
                    btnAutoStart.Enabled = false;
                    btnAutoPause.Enabled = false;
                    btnAutoContinue.Enabled = false;
                    btnManualStart.Enabled = false;
                    btnManualNext.Enabled = false;
                    btnRedoStep.Enabled = false;
                    btnStop.Enabled = false;
                }
            }
        }
    }
}
