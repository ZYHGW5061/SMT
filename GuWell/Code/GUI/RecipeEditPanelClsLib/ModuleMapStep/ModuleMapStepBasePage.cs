using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using RecipeClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using GlobalDataDefineClsLib;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    /// <summary>
    /// 单步定义recipe的UI交互基类
    /// </summary>
    public class ModuleMapStepBasePage : XtraUserControl
    {

        /// <summary>
        /// Recipe对象
        /// </summary>
        public BondRecipe EditRecipe { get; set; }
        public virtual EnumDefineSetupRecipeSubstrateMapStep CurrentStep { get; }

        public PointF FirstSubmountPosition { get; set; }
        public PointF LastColumnSubmountPosition { get; set; }
        public PointF LastRowSubmountPosition { get; set; }
        public int FirstSubmountRowIndex { get; set; }
        public int FirstSubmountColumnIndex { get; set; }
        public int RowCount { get; set; }
        public float RowPitchMM { get; set; }
        public int ColumnCount { get; set; }
        public float ColumnPitchMM { get; set; }

        public XYZTCoordinateConfig FirstPointDeterminWaferRange { get; set; }
        public XYZTCoordinateConfig SecondPointDeterminWaferRange { get; set; }
        public XYZTCoordinateConfig ThirdPointDeterminWaferRange { get; set; }

        /// <summary>
        /// 上一步的页面Id
        /// </summary>
        public virtual Type PrePageView { get; set; }
        /// <summary>
        /// 页面描述
        /// </summary>
        public virtual string StepDescription { get; set; }

        /// <summary>
        /// 下一步的页面Id
        /// </summary>
        public virtual Type NextPageView { get; set; }
        /// <summary>
        /// 下一步的的页面描述
        /// </summary>
        public virtual string NextPageViewDescription { get; set; }

        /// <summary>
        /// 当前配方步骤所属的主step
        /// </summary>
        public EnumRecipeRootStep CurRecipeStepOwner { get; set; }

        /// <summary>
        /// Recipe定义完成
        /// </summary>
        public virtual Action<BondRecipe, int[], int[]> NotifySingleStepDefineFinished { get; set; }

        public virtual Action BeforeInvokeCameraAct { get; set; }
        public virtual Action<RunStatusController> BeforeSetTemplateAct { get; set; }
        public virtual Action RequestSetTemplateAct { get; set; }
        public virtual Func<bool> RequestRecogniseAct { get; set; }
        /// <summary>
        /// 加载Recipe内容
        /// </summary>
        /// <param name="recipe"></param>
        public virtual void LoadEditedRecipe(BondRecipe recipe) { EditRecipe = recipe; }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public virtual void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeSubstrateMapStep currengStep) 
        { finished = false; currengStep = EnumDefineSetupRecipeSubstrateMapStep.None; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableGoto"></param>
        public virtual void GotoProviousStepPage(out bool enableGoto)
        {
            enableGoto = true;
        }

        /// <summary>
        /// 初始化UI控件
        /// </summary>
        protected virtual void InitControls() { }

        /// <summary>
        /// 通知页面运动系统运动过程已完成。
        /// </summary>
        protected virtual void NotifyTargetPositionReached() { }

        /// <summary>
        /// 加载时执行
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            this.InitControls();
            base.OnLoad(e);
        }

        /// <summary>
        /// 显示等待窗口
        /// </summary>
        protected void CreateWaitDialog(string description = null)
        {
            if (SplashScreenManager.Default == null)
            {
                SplashScreenManager.ShowForm(this.FindForm(), typeof(DemoWaitForm), false, true);
            }
            //SplashScreenManager.Default.Properties.ParentForm = this.FindForm();
            SplashScreenManager.Default.SetWaitFormCaption("");
            SplashScreenManager.Default.SetWaitFormDescription(string.IsNullOrEmpty(description) ? "Loading..." : description);
        }

        /// <summary>
        /// 关闭等待窗口
        /// </summary>
        protected void CloseWaitDialog()
        {
            if (SplashScreenManager.Default != null)
            {
                SplashScreenManager.CloseForm();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DefineRecipeControl
            // 
            this.Name = "DefineRecipeControl";
            this.Size = new System.Drawing.Size(240, 214);
            this.ResumeLayout(false);

        }

    }
}
