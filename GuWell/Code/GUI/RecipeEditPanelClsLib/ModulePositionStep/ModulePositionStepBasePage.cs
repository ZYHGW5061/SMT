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
using PositioningSystemClsLib;
using VisionGUI;

namespace RecipeEditPanelClsLib
{
    /// <summary>
    /// 单步定义recipe的UI交互基类
    /// </summary>
    public class ModulePositionStepBasePage : XtraUserControl
    {
        /// <summary>
        /// 定位系统
        /// </summary>
        protected PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        /// Recipe对象
        /// </summary>
        public BondRecipe EditRecipe { get; set; }
        public virtual EnumDefineSetupRecipeModulePositionStep CurrentStep { get; }

        public PointF LeftUpperCornerCoor { get; set; }
        public PointF RightUpperCornerCoor { get; set; }
        public PointF RightLowerCornerCoor { get; set; }
        public PointF LeftLowerCornerCoor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float PPWorkHeight { get; set; }
        //基板表面比Mark表面高了多少
        public float ModuleTopplateHigherValueThanMarkTopplate { get; set; }
        /// <summary>
        /// 特征的系统坐标系位置
        /// </summary>
        public PointF PositionOfPattern { get; set; }
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
        protected CameraWindowGUI UsedCameraWnd { get; set; }
        /// <summary>
        /// 加载Recipe内容
        /// </summary>
        /// <param name="recipe"></param>
        public virtual void LoadEditedRecipe(BondRecipe recipe) { EditRecipe = recipe; }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public virtual void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeModulePositionStep currengStep) 
        { finished = false; currengStep = EnumDefineSetupRecipeModulePositionStep.None; }


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
