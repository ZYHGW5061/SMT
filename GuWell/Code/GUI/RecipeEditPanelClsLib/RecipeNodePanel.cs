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
using DevExpress.XtraTab;
using System.Reflection;
using DevExpress.LookAndFeel;
using GlobalDataDefineClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using RecipeClsLib;
using CommonPanelClsLib;
using PositioningSystemClsLib;
using WestDragon.Framework.UtilityHelper;
using SystemCalibrationClsLib;
using JobClsLib;
using ConfigurationClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;

namespace RecipeEditPanelClsLib
{
    /// <summary>
    /// Recipe节点编辑控件容器
    /// </summary>
    public partial class RecipeNodeControl : BaseForm//XtraUserControl
    {
        /// <summary>
        /// 异常日志记录器
        /// </summary>
        private IBaseLogger _systemExceptionLogger
        {
            get { return LoggerManager.GetHandler().GetFileLogger(GlobalParameterSetting.SYSTEM_DEBUG_LOGGER_ID); }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 当前的Recipe对象
        /// </summary>
        public BondRecipe _editRecipe;

        /// <summary>
        /// 默认显示的页面
        /// </summary>
        private Type _defaultFirstPageViewType = null;

        /// <summary>
        /// 当前编辑的recipe root step
        /// </summary>
        private EnumRecipeRootStep _curRecipeStepOwner;

        /// <summary>
        /// 通知其他页面Recipe已保存
        /// </summary>
        public Action<BondRecipe, EnumRecipeStep> NotifyRecipeSaved { get; set; }

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public RecipeNodeControl(BondRecipe editRecipe, Type firstPageViewType, EnumRecipeRootStep rootStep = EnumRecipeRootStep.None)
        {
            InitializeComponent();

            this._editRecipe = editRecipe;
            this._defaultFirstPageViewType = firstPageViewType;
            this._curRecipeStepOwner = rootStep;

            //this.tabControl1.SetPXFHStyle();
            this.tabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        /// <summary>
        /// 第一次加载时执行页面初始化
        /// </summary>
        //protected override void OnFirstLoad()
        //{
        //    try
        //    {
        //        this.tabControl1.TabPages.Add();
        //        this.tabControl1.SelectedTabPageIndex = 0;
        //        //设置默认加载页面
        //        this.LoadSingleStepView(_defaultFirstPageViewType, this.tabControl1.SelectedTabPage);
        //    }
        //    finally
        //    {
        //        //base.OnFirstLoad();
        //    }
        //}

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            var defineRecipeView = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
            if (defineRecipeView != null)
            {
                bool enableGoto = false;
                defineRecipeView.GotoProviousStepPage(out enableGoto);
                if (!enableGoto)
                {
                    return;
                }
            }

            if (btnNextView.Text == "保存")
            {
                btnNextView.Text = "下一步>>";
            }

            if (this.tabControl1.SelectedTabPageIndex > 0)
            {
                this.btnNextView.Enabled = true;
                this.tabControl1.SelectedTabPageIndex -= 1;
                if (this.tabControl1.SelectedTabPageIndex == 0)
                {
                    this.btnPreviousView.Enabled = false;
                }
                else
                {
                    LoadSingleStepView(defineRecipeView.PrePageView, this.tabControl1.TabPages[tabControl1.SelectedTabPageIndex]);
                }
            }
        }


        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            var defineRecipeView = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
            EnumRecipeStep currentStep = EnumRecipeStep.Create;
            if (defineRecipeView != null)
            {
                bool defined = false;
                defineRecipeView.VertifyAndNotifySingleStepDefineFinished(out defined, out currentStep);
                if (!defined)
                {
                    return;
                }
            }
            if (btnNextView.Text == "保存")
            {
                NotifyRecipeDefined(_editRecipe, currentStep);
                return;
            }

            //int tabIndex = this.tabControl1.SelectedTabPageIndex + 1;
            //if (tabIndex == this.tabControl1.TabPages.Count)
            //{
            //    this.tabControl1.TabPages.Add();
            //}

            //if (defineRecipeView != null)
            //{
            //    this.tabControl1.SelectedTabPageIndex = this.tabControl1.SelectedTabPageIndex + 1;
            //    if (tabControl1.SelectedTabPage.Controls.Count > 0)
            //    {
            //        var defineRecipeView1 = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
            //        if (defineRecipeView1 != null && defineRecipeView1.GetType() != defineRecipeView.NextPageView)
            //        {
            //            while (tabControl1.SelectedTabPage.Controls.Count > 0)
            //            {
            //                tabControl1.SelectedTabPage.Controls[0].Dispose();
            //            }
            //            tabControl1.SelectedTabPage.Tag = false;
            //        }
            //    }

            //    //加载页面
            //    LoadSingleStepView(defineRecipeView.NextPageView, this.tabControl1.TabPages[tabIndex]);

            //    //判断当前页面是否是定义recipe的最后一个界面
            //    defineRecipeView = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
            //    if (defineRecipeView.NextPageView == null || defineRecipeView.NextPageViewDescription == string.Empty)
            //    {
            //        btnNextView.Text = "保存";
            //        btnNextView.Enabled = true;
            //    }
            //}

            //if (this.tabControl1.SelectedTabPageIndex != 0)
            //{
            //    btnPreviousView.Enabled = true;
            //}
        }

        /// <summary>
        /// 加载Recipe编辑的子页面
        /// </summary>
        /// <param name="subViewType"></param>
        /// <param name="tabPage"></param>
        private void LoadSingleStepView(Type subViewType, XtraTabPage tabPage)
        {
            try
            {
                this.LoadUIView(new Action(() =>
                {
                    ConstructorInfo constructor = subViewType.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw new ApplicationException(subViewType.FullName + " doesn't have public constructor with empty parameters");
                    }
                    var objView = constructor.Invoke(null);
                    if (objView is IRecipeStepBase) { (objView as IRecipeStepBase).NotifySingleStepDefineFinished += SingleStepDefined; }
                    UIFuncHelper.LoadUIView(objView as Control, tabPage);
                    tabPage.Tag = true;
                }), (bool)(tabPage.Tag ?? false), tabPage);

                if (tabControl1.SelectedTabPage.Controls.Count == 0) return;
                var defineRecipeView = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
                if (defineRecipeView != null)
                {
                    defineRecipeView.CurRecipeStepOwner = this._curRecipeStepOwner;
                    defineRecipeView.LoadEditedRecipe(_editRecipe);
                    //设置next按钮信息
                    if (defineRecipeView.NextPageView == null)
                        this.btnNextView.Text = "保存";
                    else
                        this.btnNextView.Text = "下一步>>";
                    //设置previouse按钮信息
                    if (defineRecipeView.PrePageView == null)
                        this.btnPreviousView.Enabled = false;
                    else
                        this.btnPreviousView.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _systemExceptionLogger.AddErrorContent("Load recipe subview：" + subViewType.ToString() + " enccount some exception。", ex);
            }
        }

        /// <summary>
        /// 单步Recipe定义完成
        /// </summary>
        /// <param name="recipeFinished"></param>
        private void SingleStepDefined(BondRecipe recipeFinished, int[] stepCount, int[] currentStep)
        {
            //将定义好的Recipe传回
            _editRecipe = recipeFinished;
        }

        /// <summary>
        /// 通知整个Recipe定义完成
        /// </summary>
        /// <param name="recipe"></param>
        private void NotifyRecipeDefined(BondRecipe recipe, EnumRecipeStep currentStep)
        {
            if (NotifyRecipeSaved != null && _editRecipe != null)
            {
                NotifyRecipeSaved(_editRecipe, currentStep);
            }
        }
        public void ReleaseResource()
        {
            //if (tabControl1.SelectedTabPage.Controls.Count == 0) return;
            //var defineRecipeView = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
            //if (defineRecipeView != null)
            //{
            //    //var tempView=defineRecipeView as InspectionIllumination;
            //    //if(tempView!=null)
            //    //{
            //    //    tempView.ReleaseResourse();
            //    //}
            //}
        }

        private void btnTestRecipe_Click(object sender, EventArgs e)
        {

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {

            var defineRecipeView = tabControl1.SelectedTabPage.Controls[0] as IRecipeStepBase;
            EnumRecipeStep currentStep = EnumRecipeStep.Create;
            if (defineRecipeView != null)
            {
                bool defined = false;
                defineRecipeView.VertifyAndNotifySingleStepDefineFinished(out defined, out currentStep);
                if (currentStep == EnumRecipeStep.Component_PPSettings)
                {
                    if (WarningBox.FormShow("动作确认！", "将要拾取芯片并移动到二次校准位置。", "提示") == 1)
                    {
                        try
                        {
                            CreateWaitDialog();
                            //此处执行自动识别第一课芯片、拾取、移动到仰视相机
                            foreach (var item in _editRecipe.CurrentComponent.ComponentMapInfos)
                            {
                                //if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                                //{
                                var visionParam = _editRecipe.CurrentComponent.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault();
                                if (visionParam != null)
                                {
                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                                    if (_positioningSystem.BondMovetoSafeLocation())
                                    {
                                        if (_editRecipe.CurrentComponent.CarrierType == EnumCarrierType.WafflePack)
                                        {
                                            _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, item.MaterialLocation.X, EnumCoordSetType.Absolute);
                                            _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, item.MaterialLocation.Y, EnumCoordSetType.Absolute);
                                            _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, visionParam.CameraZWorkPosition, EnumCoordSetType.Absolute);

                                            //视觉识别，并将物料中心移动到视觉中心
                                            var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                            if (visionRet != null)
                                            {
                                                var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;

                                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, visionRet.X + offset.X, EnumCoordSetType.Relative);
                                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, visionRet.Y + offset.Y, EnumCoordSetType.Relative);

                                                ////吸嘴移动到相机中心（此时也是物料中心）(只移XY)
                                                ////_positioningSystem.ChipPPMovetoBondCameraCenter();
                                                ////拾取芯片
                                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute);

                                                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.PPSettings.PPtoolName);
                                                if (pptool != null)
                                                {
                                                    offset = pptool.PP1AndBondCameraOffset;

                                                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, visionRet.X + offset.X, EnumCoordSetType.Relative);
                                                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, visionRet.Y + offset.Y, EnumCoordSetType.Relative);

                                                    //吸嘴移动到相机中心（此时也是物料中心）(只移XY)
                                                    //_positioningSystem.ChipPPMovetoBondCameraCenter();
                                                    //拾取芯片
                                                    _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute);

                                                    _editRecipe.CurrentComponent.PPSettings.PPToolZero = pptool.AltimetryOnMark;

                                                }
                                                else
                                                {

                                                    _editRecipe.CurrentComponent.PPSettings.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;

                                                }
                                                _editRecipe.CurrentComponent.PPSettings.WorkHeight = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                _editRecipe.CurrentComponent.PPSettings.UpDistanceMMAfterPicked = 10;
                                                if (PPUtility.Instance.PickViaSystemCoor(_editRecipe.CurrentComponent.PPSettings))
                                                {
                                                    if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                                                    {


                                                        //拾取完成之后进行角度补偿和旋转后的XY补偿
                                                        if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -(visionRet.Theta - visionParam.OrigionAngle), EnumCoordSetType.Relative) == StageMotionResult.Success
                                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                    //拾取之后移动到仰视相机上方
                                                    && _positioningSystem.PPtoolMovetoUplookingCameraCenter(pptool)
                                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, pptool.LookuptoPPOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                        {
                                                            if(pptool.EnumPPtool == EnumPPtool.PPtool2)
                                                            {
                                                                _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute);
                                                            }

                                                            JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                            JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                            JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("动作完成。", "已拾取芯片到仰视相机上方！");
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("错误", "拾取芯片失败！");
                                                            return;
                                                        }
                                                    }
                                                    else if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                                                    {
                                                        //移动到校准台上方并放芯片
                                                        //拾取完成之后进行角度补偿和旋转后的XY补偿
                                                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -(visionRet.Theta - visionParam.OrigionAngle), EnumCoordSetType.Relative) == StageMotionResult.Success
                                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                    && _positioningSystem.PPtoolMovetoCalibrationTableCenter(pptool))
                                                        {

                                                            //放芯片
                                                            var ppParam = _editRecipe.CurrentComponent.PPSettings;

                                                            if (pptool != null)
                                                            {
                                                                var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM;
                                                                ppParam.PPToolZero = pptool.AltimetryOnMark;
                                                                ppParam.WorkHeight = (float)systemPos;
                                                            }
                                                            else
                                                            {
                                                                var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM;
                                                                ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                                                ppParam.WorkHeight = (float)systemPos;
                                                            }

                                                            if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnCalibrationTable, true))
                                                            {
                                                                _positioningSystem.PPMovetoSafeLocation();
                                                                LogRecorder.RecordLog(EnumLogContentType.Error, "放置芯片到校准台失败！");
                                                                WarningBox.FormShow("错误", "放置芯片到校准台失败！");

                                                            }
                                                            JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                            JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                            JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("动作完成。", "已拾取芯片到校准台！");
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时移动到校准台失败！");
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("错误", "移动到校准台失败！");
                                                            return;
                                                        }
                                                    }



                                                    ////拾取完成之后进行角度补偿和旋转后的XY补偿
                                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, visionRet.Theta - visionParam.OrigionAngle, EnumCoordSetType.Relative);
                                                    ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                                                    //////拾取之后移动到仰视相机上方
                                                    ////_positioningSystem.ChipPPMovetoUplookingCameraCenter();
                                                    ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ
                                                    ////    , _systemConfig.PositioningConfig.LookupChipPPOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM, EnumCoordSetType.Absolute);

                                                    ////拾取之后移动到仰视相机上方
                                                    //_positioningSystem.PPtoolMovetoUplookingCameraCenter(pptool);
                                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ
                                                    //    , pptool.LookuptoPPOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM, EnumCoordSetType.Absolute);



                                                    //JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                    //JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                    //JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                    //CloseWaitDialog();
                                                    //WarningBox.FormShow("动作完成。", "已拾取芯片到仰视相机上方！");
                                                    //break;
                                                }
                                                else
                                                {
                                                    LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                    CloseWaitDialog();
                                                    WarningBox.FormShow("错误", "拾取芯片失败！");
                                                    return;
                                                }
                                            }
                                        }
                                        else if (_editRecipe.CurrentComponent.CarrierType == EnumCarrierType.Wafer)
                                        {
                                            var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.RelatedESToolName);
                                            var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.PPSettings.PPtoolName);
                                            if (usedESTool != null)
                                            {

                                                var xx = 0f;
                                                var yy = 0f;

                                                xx = item.MaterialLocation.X - _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.X;
                                                yy = item.MaterialLocation.Y - _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.Y;
                                                if (//顶针移动到零点
                                                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                //物料移动到wafer相机中心
                                                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, item.MaterialLocation.X + usedESTool.NeedleCenter.X, EnumCoordSetType.Absolute);
                                                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, item.MaterialLocation.Y - usedESTool.NeedleCenter.Y, EnumCoordSetType.Absolute);
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, visionParam.CameraZWorkPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                //顶针座升起
                                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, _editRecipe.CurrentComponent.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                {


                                                    //视觉识别
                                                    var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, visionParam);
                                                    if (visionRet != null)
                                                    {

                                                        //物料移动到视野中心
                                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                                        || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                                        {
                                                            LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("错误", "拾取芯片失败！");
                                                            return;
                                                        }
                                                        var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;

                                                        offset = pptool.PP1AndBondCameraOffset;

                                                        var offsetBCAndWC = _systemConfig.PositioningConfig.WaferCameraOrigion;


                                                        //物料中心移动到顶针上方
                                                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X , EnumCoordSetType.Relative) == StageMotionResult.Success
                                                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                                                        //芯片吸嘴物料中心上方
                                                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC.X - usedESTool.NeedleCenter.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC.Y - usedESTool.NeedleCenter.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success

                                                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                        {

                                                            //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.RelatedPPToolName);
                                                            if (pptool != null)
                                                            {
                                                                _editRecipe.CurrentComponent.PPSettings.PPToolZero = pptool.AltimetryOnMark;

                                                            }
                                                            else
                                                            {

                                                                _editRecipe.CurrentComponent.PPSettings.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;

                                                            }

                                                            _editRecipe.CurrentComponent.PPSettings.WorkHeight = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                            IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();
                                                            if (PPUtility.Instance.PickViaSystemCoor(_editRecipe.CurrentComponent.PPSettings))
                                                            {

                                                                if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                                                                {


                                                                    //拾取完成之后进行角度补偿和旋转后的XY补偿
                                                                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -(visionRet.Theta - visionParam.OrigionAngle), EnumCoordSetType.Relative) == StageMotionResult.Success
                                                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                                //拾取之后移动到仰视相机上方
                                                                && _positioningSystem.PPtoolMovetoUplookingCameraCenter(pptool)
                                                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, pptool.LookuptoPPOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                                    {


                                                                        IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                        JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                                        JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                                        JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                                        CloseWaitDialog();
                                                                        WarningBox.FormShow("动作完成。", "已拾取芯片到仰视相机上方！");
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                        LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                                        CloseWaitDialog();
                                                                        WarningBox.FormShow("错误", "拾取芯片失败！");
                                                                        return;
                                                                    }
                                                                }
                                                                else if(_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                                                                {
                                                                    //移动到校准台上方并放芯片
                                                                    //拾取完成之后进行角度补偿和旋转后的XY补偿
                                                                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -(visionRet.Theta - visionParam.OrigionAngle), EnumCoordSetType.Relative) == StageMotionResult.Success
                                                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success   
                                                                && _positioningSystem.PPtoolMovetoCalibrationTableCenter(pptool))
                                                                    {

                                                                        //放芯片
                                                                        var ppParam = _editRecipe.CurrentComponent.PPSettings;

                                                                        if (pptool != null)
                                                                        {
                                                                            var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM;
                                                                            ppParam.PPToolZero = pptool.AltimetryOnMark;
                                                                            ppParam.WorkHeight = (float)systemPos;
                                                                        }
                                                                        else
                                                                        {
                                                                            var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM;
                                                                            ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                                                            ppParam.WorkHeight = (float)systemPos;
                                                                        }

                                                                        if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnCalibrationTable, true))
                                                                        {
                                                                            _positioningSystem.PPMovetoSafeLocation();
                                                                            LogRecorder.RecordLog(EnumLogContentType.Error, "放置芯片到校准台失败！");
                                                                            WarningBox.FormShow("错误", "放置芯片到校准台失败！");

                                                                        }
                                                                        IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                        JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                                        JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                                        JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                                        CloseWaitDialog();
                                                                        WarningBox.FormShow("动作完成。", "已拾取芯片到校准台！");
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                        LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时移动到校准台失败！");
                                                                        CloseWaitDialog();
                                                                        WarningBox.FormShow("错误", "移动到校准台失败！");
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                                CloseWaitDialog();
                                                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                            LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("错误", "拾取芯片失败！");
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                        //LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                        //CloseWaitDialog();
                                                        //WarningBox.FormShow("错误", "拾取芯片失败！");
                                                        continue;
                                                    }

                                                }
                                                else
                                                {
                                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                    LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                    CloseWaitDialog();
                                                    WarningBox.FormShow("错误", "拾取芯片失败！");
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                LogRecorder.RecordLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                                                CloseWaitDialog();
                                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                                return;
                                            }
                                        }
                                        else if (_editRecipe.CurrentComponent.CarrierType == EnumCarrierType.WaferWafflePack)
                                        {

                                            var xx = 0f;
                                            var yy = 0f;

                                            var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.PPSettings.PPtoolName);

                                            xx = item.MaterialLocation.X - _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.X;
                                            yy = item.MaterialLocation.Y - _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.Y;
                                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, _editRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, visionParam.CameraZWorkPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                            {
                                                //视觉识别
                                                var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, visionParam);
                                                if (visionRet != null)
                                                {
                                                    var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;

                                                    offset = pptool.PP1AndBondCameraOffset;

                                                    var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;

                                                    //物料移动到视野中心
                                                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                                    || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                                    {
                                                        LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                        CloseWaitDialog();
                                                        WarningBox.FormShow("错误", "拾取芯片失败！");
                                                        return;
                                                    }

                                                    //物料中心移动到顶针上方
                                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X - visionRet.X, EnumCoordSetType.Relative);
                                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y + visionRet.Y, EnumCoordSetType.Relative);
                                                    //芯片吸嘴物料中心上方
                                                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                    {

                                                        //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.RelatedPPToolName);
                                                        if (pptool != null)
                                                        {
                                                            _editRecipe.CurrentComponent.PPSettings.PPToolZero = pptool.AltimetryOnMark;

                                                        }
                                                        else
                                                        {

                                                            _editRecipe.CurrentComponent.PPSettings.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;

                                                        }

                                                        _editRecipe.CurrentComponent.PPSettings.WorkHeight = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                        //IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();
                                                        if (PPUtility.Instance.PickViaSystemCoor(_editRecipe.CurrentComponent.PPSettings))
                                                        {
                                                            if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                                                            {
                                                                //拾取完成之后进行角度补偿和旋转后的XY补偿
                                                                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -(visionRet.Theta - visionParam.OrigionAngle), EnumCoordSetType.Relative) == StageMotionResult.Success
                                                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                            //拾取之后移动到仰视相机上方
                                                            && _positioningSystem.PPtoolMovetoUplookingCameraCenter(pptool)
                                                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ
                                                                , _systemConfig.PositioningConfig.LookupChipPPOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                                {

                                                                    //IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                    JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                                    JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                                    JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                                    CloseWaitDialog();
                                                                    WarningBox.FormShow("动作完成。", "已拾取芯片到仰视相机上方！");
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                                    CloseWaitDialog();
                                                                    WarningBox.FormShow("错误", "拾取芯片失败！");
                                                                    return;
                                                                }
                                                            }
                                                            else if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                                                            {
                                                                //移动到校准台上方并放芯片
                                                                //拾取完成之后进行角度补偿和旋转后的XY补偿
                                                                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -(visionRet.Theta - visionParam.OrigionAngle), EnumCoordSetType.Relative) == StageMotionResult.Success
                                                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                            && _positioningSystem.PPtoolMovetoCalibrationTableCenter(pptool))
                                                                {

                                                                    //放芯片
                                                                    var ppParam = _editRecipe.CurrentComponent.PPSettings;

                                                                    if (pptool != null)
                                                                    {
                                                                        var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM;
                                                                        ppParam.PPToolZero = pptool.AltimetryOnMark;
                                                                        ppParam.WorkHeight = (float)systemPos;
                                                                    }
                                                                    else
                                                                    {
                                                                        var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + _editRecipe.CurrentComponent.ThicknessMM;
                                                                        ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                                                        ppParam.WorkHeight = (float)systemPos;
                                                                    }

                                                                    if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, null, true))
                                                                    {
                                                                        _positioningSystem.PPMovetoSafeLocation();
                                                                        LogRecorder.RecordLog(EnumLogContentType.Error, "放置芯片到校准台失败！");
                                                                        WarningBox.FormShow("错误", "放置芯片到校准台失败！");

                                                                    }
                                                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                    JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X = item.MaterialLocation.X;
                                                                    JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y = item.MaterialLocation.Y;
                                                                    JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickSystemPos;
                                                                    CloseWaitDialog();
                                                                    WarningBox.FormShow("动作完成。", "已拾取芯片到校准台！");
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                                    LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时移动到校准台失败！");
                                                                    CloseWaitDialog();
                                                                    WarningBox.FormShow("错误", "移动到校准台失败！");
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                                            LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                            CloseWaitDialog();
                                                            WarningBox.FormShow("错误", "拾取芯片失败！");
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                        CloseWaitDialog();
                                                        WarningBox.FormShow("错误", "拾取芯片失败！");
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                                CloseWaitDialog();
                                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            LogRecorder.RecordLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                                            CloseWaitDialog();
                                            WarningBox.FormShow("错误", "拾取芯片失败！");
                                            return;
                                        }

                                        CloseWaitDialog();
                                        WarningBox.FormShow("成功！", "拾取芯片到仰视相机上方完成！");
                                    }
                                    else
                                    {
                                        LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！");
                                        CloseWaitDialog();
                                        WarningBox.FormShow("错误", "拾取芯片失败！");
                                        return;
                                    }
                                }
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                            LogRecorder.RecordLog(EnumLogContentType.Error, "二次校准时拾取芯片失败！", ex);
                            CloseWaitDialog();
                        }
                        finally
                        {
                            CloseWaitDialog();
                        }

                    }
                }
                else if (currentStep == EnumRecipeStep.Substrate_PositionSettings)
                {
                    //识别substrate的Mark1和Mark2
                    MatchIdentificationParam visionParam = _editRecipe.SubstrateInfos.PositionSustrateMarkVisionParameters[0].ShapeMatchParameters[0];
                    double X = visionParam.BondTablePositionOfCreatePattern.X;
                    double Y = visionParam.BondTablePositionOfCreatePattern.Y;
                    double Z = visionParam.CameraZWorkPosition;
                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                            if (visionRet != null)
                            {
                                //移动到视野中心
                                if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet.X, visionRet.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                {
                                    //更新substrate坐标原点
                                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);

                                    visionParam = _editRecipe.SubstrateInfos.PositionSustrateMarkVisionParameters[1].ShapeMatchParameters[0];
                                    X = visionParam.BondTablePositionOfCreatePattern.X;
                                    Y = visionParam.BondTablePositionOfCreatePattern.Y;
                                    Z = visionParam.CameraZWorkPosition;
                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                    {
                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                        {
                                            var visionSecondTime = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                            if (visionSecondTime != null)
                                            {
                                                //移动到视野中心
                                                if (_positioningSystem.BondXYUnionMovetoStageCoor(visionSecondTime.X, visionSecondTime.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                {
                                                    //更新substrate第二基准点坐标
                                                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                    NotifyRecipeDefined(_editRecipe, currentStep);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //搜索完榜头相机移动到第一个Mark位置
                    _positioningSystem.BondXYUnionMovetoSystemCoor(_editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.X, _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.Y, EnumCoordSetType.Absolute);


                }
            }
        }
        public void AfterPlaceChipOnCalibrationTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        private void RecipeNodeControl_Load(object sender, EventArgs e)
        {
            try
            {
                this.tabControl1.TabPages.Add();
                this.tabControl1.SelectedTabPageIndex = 0;
                //设置默认加载页面
                this.LoadSingleStepView(_defaultFirstPageViewType, this.tabControl1.SelectedTabPage);
            }
            finally
            {
                //base.OnFirstLoad();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "确认结束配方编辑？", "提示") == 1)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void RecipeNodeControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "将要结束配方编辑，当前配方是否已保存？", "提示") == 0)
            {
                e.Cancel = true;
            }
        }
    }


    /// <summary>
    /// 界面上常用到的功能
    /// </summary>
    public static class UIFuncHelper
    {
        /// <summary>
        /// 准备加载功能界面
        /// </summary>
        /// <param name="ctrlView"></param>
        /// <param name="tabPage"></param>
        public static void LoadUIView(Control ctrlView, XtraTabPage tabPage)
        {
            ctrlView.Visible = false;
            ctrlView.Dock = DockStyle.Fill;
            ctrlView.Bounds = tabPage.DisplayRectangle;
            tabPage.Controls.Add(ctrlView);
            ctrlView.Visible = true;
        }

        /// <summary>
        /// 加载并初始化一个功能页面
        /// </summary>
        /// <param name="loadAction">功能</param>
        /// <param name="isLoaded">首页面是否已经加载过</param>
        public static void LoadUIView(this Control moduleUi, Action loadAction, bool isLoaded, XtraTabPage tabPage)
        {
            if (isLoaded)
            {
                return;
            }
            moduleUi.Invoke(new Action(() =>
            {
                /* WaitFormManager.ShowWaitForm(moduleUi.FindForm(), "Please Wait", "Loading...");*/
                tabPage.SuspendLayout();
                if (loadAction != null)
                {
                    loadAction();
                }
                tabPage.ResumeLayout(true);
                //关闭等待画面  
                /*WaitFormManager.CloseWaitForm();*/
            }));
        }

    }  
}
