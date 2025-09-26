using CameraControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using JobClsLib;
using LaserSensorManagerClsLib;
using LightControllerManagerClsLib;
using PositioningSystemClsLib;
using PowerClsLib;
using ProductRunClsLib;
using RecipeClsLib;
using StageControllerClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCalibrationClsLib;
using VisionClsLib;
using VisionControlAppClsLib;
using VisionGUI;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;
using static GlobalToolClsLib.GlobalCommFunc;

namespace ProductRunClsLib
{

    public enum EnumProductRunType
    {
        [Description("自动运行")]
        Auto,

        [Description("单步运行")]
        Step
    }

    

    public enum EnumActionStat
    {
        [Description("未运行")]
        undo,
        [Description("运行中")]
        running,
        [Description("完成")]
        done,
        [Description("出错")]
        error
    }
    public enum EnumJobRunStatus
    {
        Initial,
        PositionOneSubstrateSuccess,
        PositionOneSubstrateFail,
        PositionAllSubstrateSuccess,
        PositionAllSubstrateFail,
        PositionOneModuleSuccess,
        PositionOneModuleFail,
        PositionAllModuleSuccess,
        PositionAllModuleFail,
        PositionOneBondPosSuccess,
        PositionOneBondPosFail,
        PositionAllBondPosSuccess,
        PositionAllBondPosFail,
        DispenseOnePosSuccess,
        DispenseOnePosFail,
        DispenseAllPosSuccess,
        DispenseAllPosFail,
        WaitPositionChip,
        PositionChipSuccess,
        PositionChipFail,
        WaitPickupChip,
        PickupChipSuccess,
        PickupChipFail,
        AccuracyCalibrationChipSuccess,
        AccuracyCalibrationChipFail,
        BondChipSuccess,
        BondChipFail,
        AbandonChipSuccess,
        AbandonChipFail,
        PositionSubmonutSuccess,
        PositionSubmonutFail,
        PickupSubmonutSuccess,
        PickupSubmonutFail,
        AccuracyCalibrationSubmonutSuccess,
        AccuracyCalibrationSubmonutFail,
        BondSubmonutSuccess,
        BondSubmonutFail,
        BondSubmonutToEutecticSuccess,
        BondSubmonutToEutecticFail,
        PositionSubmonutToEutecticSuccess,
        PositionSubmonutToEutecticFail,
        EutecticSuccess,
        EutecticFail,
        BlankComponentSuccess,
        BlankComponentFail,
        AbandonSubmonutSuccess,
        AbandonSubmonutFail,
        StepDispenseComplete,
        StepBondDieComplete,
        StepAfterBondDieVisionComplete,
        Completed,
        Aborted

    }


    //***********************************************************************************************************************************
    //***********************************************************************************************************************************
    //
    //********************                                以下为动作的具体定义类                       *************************************
    //
    //***********************************************************************************************************************************
    //***********************************************************************************************************************************
    public class RunParameter
    {
        public RunParameter()
        {
            MaterialPos = new XYZTCoordinateConfig();
        }
        public XYZTCoordinateConfig MaterialPos { get; set; }
    }

    /*
     * EnumActionNo定义
     */
    public enum EnumActionNo
    {
        [Description("0 回到安全位置")]
        Action_MoveSafePos = 0,

        [Description("1 相机移动至基底上方")]
        Action_CamMoveToSubstratelPos = 1,

        [Description("2 相机识别基板位置")]
        Action_PositionSubstrate = 2,

        [Description("3 吸取基底")]
        Action_PositionModule = 3,

        [Description("4 放置基底")]
        Action_PutDownSubstrate = 4,

        [Description("5 相机移动至芯片上方")]
        Action_CamMoveToChipPos = 5,

        [Description("6 定位芯片")]
        Action_PositionChip = 6,

        [Description("7 吸取芯片")]
        Action_PickUpChip = 7,

        [Description("8 芯片移动至仰视相机")]
        Action_MoveToLookupCamPos = 8,

        [Description("9 芯片二次校准")]
        Action_AccuracyPositionChip = 9,

        [Description("10 相机移动至共晶台位置")]
        Action_CamMovToEutecnicPos = 10,

        [Description("11 放置芯片前识别贴装位置")]
        Action_RecognizeBondPos = 11,
        [Description("11 放置芯片前识别贴装位置")]
        Action_Dispense = 11,

        [Description("12 固晶")]
        Action_BondChip = 12,

        [Description("13 共晶")]
        Action_Eutectic = 13,

        [Description("14 下料")]
        Action_BlankComponent = 14,

        [Description("6 定位衬底")]
        Action_PositionSubmonut = 15,

        [Description("7 吸取衬底")]
        Action_PickUpSubmonut = 16,

        [Description("9 衬底二次校准")]
        Action_AccuracyPositionSubmonut = 17,

        [Description("9 衬底到共晶台")]
        Action_PositionSubmonutToEutectic = 18,

        [Description("9 芯片到共晶台")]
        Action_PositionChipToEutectic = 19,

        [Description("ex1 基底抛料")]
        Action_AbondonSubstrate = 101,

        [Description("ex2 芯片抛料")]
        Action_AbondonChip = 102,

        [Description("ex3 衬底抛料")]
        Action_AbondonSubmonut = 103

    }

    /*
     * 步骤Action类 基类
     */
    public abstract class StepActionBase
    {
        //动作所属Step
        public ProductStep SrcProductStep { get; set; }

        //动作序号
        public EnumActionNo ActionNo { get; set; }

        //动作描述
        public string ActionDesc { get; set; }

        //是否是一组基底操作的最后一步
        public bool IsLastActionPerSubstrate { get; set; }

        //动作完成状态
        private EnumActionStat actionStat;
        public EnumActionStat ActionStat
        {
            get
            {
                return actionStat;
            }
            set
            {
                actionStat = value;
            }
        }

        public string ActionStatDesc
        {
            get
            {
                return EnumHelper.GetEnumDescription(ActionStat);
            }
        }

        /*
         * 初始化方法
         * 
         * step 传入所属的步骤
         * actionNo 动作序号
         * actionDesc 动作描述
         * 
         */
        public StepActionBase(ProductStep step, EnumActionNo actionNo, string actionDesc)
        {
            SrcProductStep = step;
            ActionNo = actionNo;
            ActionDesc = actionDesc;
            ActionStat = EnumActionStat.undo;
            IsLastActionPerSubstrate = false;
        }

        /*
         * 待实现的具体动作方法
         */
        public abstract GWResult Run(RunParameter runParam = null);

        //轴控制核心
        protected StageCore stage = StageControllerClsLib.StageCore.Instance;

        protected PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }

        //相机设置参数
        protected CameraConfig _BondcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.BondCamera); }
        }
        protected CameraConfig _UplookingcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.UplookingCamera); }
        }
        protected CameraConfig _WafercameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.WaferCamera); }
        }

        //视觉管理
        protected VisionControlAppClsLib.VisualControlManager _VisualManager
        {
            get { return VisionControlAppClsLib.VisualControlManager.Instance; }
        }

        //相机视觉设置
        protected VisualControlApplications BondCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.BondCamera); }
        }
        protected VisualControlApplications UplookingCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        protected VisualControlApplications WaferCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.WaferCamera); }
        }

        //系统设置类
        protected SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        protected ProgramComponentSettings CurSubmonutParam
        {
            get
            {
                string SubmonutName = this.SrcProductStep.SubmonutName;
                return BondRecipe.LoadComponentByName(SubmonutName);
            }
        }

        protected ProgramComponentSettings CurChipParam
        {
            get
            {
                string chipName = this.SrcProductStep.ComponentName;
                return BondRecipe.LoadComponentByName(chipName);
            }
        }

        protected void SaveCurChipParam()
        {
            BondRecipe.SaveComponent(CurChipParam, this.SrcProductStep.ComponentName);
        }

        protected BondingPositionSettings CurBondPosition
        {
            get
            {
                string name = this.SrcProductStep.BondingPositionName;
                return BondRecipe.LoadBondPositionByName(name);
            }
        }
        protected EpoxyApplication CurEpoxyApplication
        {
            get
            {
                string name = this.SrcProductStep.EpoxyApplicationName;
                return BondRecipe.LoadEpoxyApplicationByName(name);
            }
        }
        //绝对位置Z向移动
        protected void AxisAbsoluteMove(EnumStageAxis axis, double target)
        {
            stage.AbloluteMoveSync(axis, target);
        }

        /// <summary>
        /// 榜头移动
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        protected void BondXYZAbsoluteMove(double X, double Y, double Z)
        {
            stage.AbloluteMoveSync(new EnumStageAxis[3] { EnumStageAxis.BondX, EnumStageAxis.BondY, EnumStageAxis.BondZ }, new double[3] { X, Y, Z });
        }
    }

    //----------------------- 基类定义完成 -----------------------------------------------------------------------



    //------------------------------------------ 步骤类定义 ------------------------------------------------------------

    class StepAction_Test : StepActionBase
    {
        public StepAction_Test(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }
        public override GWResult Run(RunParameter runParam = null)
        {
            Thread.Sleep(1000);
            if (this.ActionNo == EnumActionNo.Action_PutDownSubstrate)
            {
                return GlobalGWResultDefine.RET_FAILED;
            }
            return GlobalGWResultDefine.RET_SUCCESS;
        }
    }

    /*
     * 移至安全位置步骤Action
     */
    class StepAction_MoveSafePos : StepActionBase 
    {
        public StepAction_MoveSafePos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                //AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ);

                double BondX = _systemConfig.PositioningConfig.BondSafeLocation.X;
                double BondY = _systemConfig.PositioningConfig.BondSafeLocation.Y;
                double BondZ = _systemConfig.PositioningConfig.BondSafeLocation.Z;

                if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ);
                    
                }
                else
                {
                    IOUtilityHelper.Instance.UpDispenserCylinder();
                }
                
                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);
                BondXYZAbsoluteMove(BondX, BondY, BondZ);
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
                AxisAbsoluteMove(EnumStageAxis.ESZ, 0);
                if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);

                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_MoveSafePos,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }

    /*
     * 物料Step 1 - 相机移至基底上方位置 步骤Action
     */
    class StepAction_CamMoveToSubstratelPos : StepActionBase
    {
        public StepAction_CamMoveToSubstratelPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            if (ProductExecutor.Instance.Substrate == null)
            {
                WarningBox.FormShow("错误", "没取到基底信息！", "提示");
            }
            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMoveToSubstratelPos-Start.");
            _positioningSystem.PPMovetoSafeLocation();
            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

            //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----
            //double X = ProductExecutor.Instance.Substrate.FirstSubmountLocation.X;
            //double Y = ProductExecutor.Instance.Substrate.FirstSubmountLocation.Y;            
            double X = ProductExecutor.Instance.AllSubstrate[ProductExecutor.Instance.CurSubstrateNum - 1].MaterialLocation.X;
            double Y = ProductExecutor.Instance.AllSubstrate[ProductExecutor.Instance.CurSubstrateNum - 1].MaterialLocation.Y;

            double Z = ProductExecutor.Instance.Substrate.PositionSustrateVisionParameters.ShapeMatchParameters[0].CameraZWorkPosition;
            //-----通过物料名取物料对象，取物料首位置 X Y Z    end -----

            //移动bond头
            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);

            //XY联动移动到物料上方
            //EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            //multiAxis[0] = EnumStageAxis.BondX;
            //multiAxis[1] = EnumStageAxis.BondY;
            //double[] targets = new double[2];
            //targets[0] = X;
            //targets[1] = Y;
            //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
            _positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

            _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);


            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMoveToSubstratelPos-End.");
            return GlobalGWResultDefine.RET_SUCCESS;
        }
    }

    /*
     * 相机取 基底 前识别校准 步骤Action
     */
    public class StepAction_PositionSubstrate : StepActionBase
    {
        public StepAction_PositionSubstrate(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                
                //CameraWindowGUI.Instance?.SelectCamera(0);
                if (_curRecipe.CurrentSubstrate == null)
                {
                    WarningBox.FormShow("错误", "基底信息为空！", "提示");
                    return GlobalGWResultDefine.RET_FAILED;
                }
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CalibrationBeforePickSubstrate-Start.");
                var ret = GlobalGWResultDefine.RET_FAILED;
                ExecutionController.Instance.WaitIfPaused();
                ProductExecutor.Instance.CurSubstrateNum = 1;
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    foreach (var item in ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.SubstrateMapInfos)
                    {
                        
                        if(ProductExecutor.Instance.CurSubstrateNum < ProductExecutor.Instance.StartSubstrateNum)
                        {
                            ProductExecutor.Instance.CurSubstrateNum++;
                            continue;
                        }

                        //double X = item.MaterialLocation.X - _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[0].PatternOffsetWithMaterialCenter.X;
                        //double Y = item.MaterialLocation.Y - _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[0].PatternOffsetWithMaterialCenter.Y;

                        //double Z = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[0].CameraZWorkPosition;
                        //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);
                        //item.PositionSubstrateResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                        //item.IsPositionSuccess = item.PositionSubstrateResult == null ? false : true;

                        //识别substrate的Mark1和Mark2

                        VisionIdentificationParam visionParam = new VisionIdentificationParam();
                        int ParametersCount = 0;
                        if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[0];
                            ParametersCount = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters.Count;
                        }
                        else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.LineSearchParams[0];
                            ParametersCount = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.LineSearchParams.Count;
                        }
                        else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                        {
                            visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.CircleSearchParameters[0];
                            ParametersCount = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.CircleSearchParameters.Count;
                        }

                        double X = visionParam.BondTablePositionOfCreatePattern.X + item.MaterialLocation.X - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.X;
                        double Y = visionParam.BondTablePositionOfCreatePattern.Y + item.MaterialLocation.Y - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.Y;
                        double Z = visionParam.CameraZWorkPosition;
                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            double BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                            double BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                            XYZTCoordinateConfig org_SubstrateCoordinateHomePoint1 = new XYZTCoordinateConfig()
                            {
                                X = X,
                                Y = Y,
                            };
                            ExecutionController.Instance.WaitIfPaused();
                            if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Debug
                                                   , $"SubstrateCoordinateHomePoint:PosX:{_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X},PosY:{_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y}.");
                                XYZTCoordinateConfig visionRet = new XYZTCoordinateConfig();
                                if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                }
                                else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                }
                                else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                {
                                    visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                }

                                if (visionRet != null)
                                {
                                    item.PositionSubstrateMark1Result = visionRet;
                                    ExecutionController.Instance.WaitIfPaused();
                                    //移动到视野中心
                                    if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet.X, visionRet.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                    {
                                        BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                        BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                        XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint1 = new XYZTCoordinateConfig()
                                        {
                                            X = BondX,
                                            Y = BondY,
                                        };
                                        //更新substrate坐标原点
                                        //_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                        //_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                        ProductExecutor.Instance.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                        ProductExecutor.Instance.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);

                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet1:PosX:{visionRet.X},PosY:{visionRet.Y}.");
                                        item.IsPositionSuccess = true;

                                        if (_curRecipe.CurrentSubstrate.PositionSubstratePointCount == 2 && ParametersCount > 1)
                                        {
                                            if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                            {
                                                visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[1];
                                            }
                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                            {
                                                visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.LineSearchParams[1];
                                            }
                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                            {
                                                visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.CircleSearchParameters[1];
                                            }
                                            X = visionParam.BondTablePositionOfCreatePattern.X + item.MaterialLocation.X - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.X;
                                            Y = visionParam.BondTablePositionOfCreatePattern.Y + item.MaterialLocation.Y - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.Y;
                                            Z = visionParam.CameraZWorkPosition;
                                            ExecutionController.Instance.WaitIfPaused();
                                            if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                            {
                                                XYZTCoordinateConfig org_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                {
                                                    X = X,
                                                    Y = Y,
                                                };

                                                ExecutionController.Instance.WaitIfPaused();
                                                if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                {
                                                    XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                                    if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                    {
                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                    }
                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                    {
                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                    }
                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                    {
                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                    }
                                                    if (visionRet2 != null)
                                                    {
                                                        ExecutionController.Instance.WaitIfPaused();
                                                        //移动到视野中心
                                                        if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet2.X, visionRet2.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                        {
                                                            BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                            BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                            XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                            {
                                                                X = BondX,
                                                                Y = BondY,
                                                            };

                                                            double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                            double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                            double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                            double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                            double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                            double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                            double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                            double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                            double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                            double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                            double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                            double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                            double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                            double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                            double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                            double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                            XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                            {
                                                                X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                Theta = BMCsubangleInDegrees,
                                                            };

                                                            XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                            {
                                                                X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                Theta = org_BMCsubangleInDegrees,
                                                            };

                                                            //更新substrate坐标原点
                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                            item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                            {
                                                                X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                            };
                                                            item.IsPositionSuccess = true;
                                                            ret = GlobalGWResultDefine.RET_SUCCESS;
                                                        }
                                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                    }
                                                    else
                                                    {
                                                        if (ShowMessage2("异常发生！", "搜寻基板Mark2失败，请重新定位到基板Mark2位置，进行识别！", "警报") == 1)
                                                        {
                                                            if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                            }
                                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                            }
                                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                            }
                                                            if (visionRet2 != null)
                                                            {
                                                                ExecutionController.Instance.WaitIfPaused();
                                                                //移动到视野中心
                                                                if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet2.X, visionRet2.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                                {
                                                                    BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = BondX,
                                                                        Y = BondY,
                                                                    };

                                                                    double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                    double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                    double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                    double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                    double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                    double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                    double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                    double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                    //更新substrate坐标原点
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = BMCsubangleInDegrees,
                                                                    };

                                                                    XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                        Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                        Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.IsPositionSuccess = true;
                                                                    ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                }
                                                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                            }
                                                            else
                                                            {
                                                                if(ShowMessage2("异常发生！", "搜寻基板Mark2失败，是否手动对准该基板特征位置继续生产？", "警报") == 1)
                                                                {
                                                                    BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = BondX,
                                                                        Y = BondY,
                                                                    };

                                                                    double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                    double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                    double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                    double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                    double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                    double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                    double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                    double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                    //更新substrate坐标原点
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = BMCsubangleInDegrees,
                                                                    };

                                                                    XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                        Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                        Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.IsPositionSuccess = true;
                                                                    ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                                }
                                                                else
                                                                {
                                                                    item.IsPositionSuccess = false;
                                                                }
                                                                
                                                            }

                                                        }
                                                        else
                                                        {
                                                            item.IsPositionSuccess = false;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            item.IsPositionSuccess = false;
                                            ret = GlobalGWResultDefine.RET_SUCCESS;
                                        }



                                    }
                                }
                                else
                                {
                                    if (ShowMessage2("异常发生！", "搜寻基板Mark1失败，请重新定位到基板Mark1位置，进行识别！", "警报") == 1)
                                    {
                                        if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                        {
                                            visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                        }
                                        else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                        {
                                            visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                        }
                                        else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                        {
                                            visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                        }

                                        if (visionRet != null)
                                        {
                                            item.PositionSubstrateMark1Result = visionRet;
                                            ExecutionController.Instance.WaitIfPaused();
                                            //移动到视野中心
                                            if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet.X, visionRet.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                            {
                                                BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint1 = new XYZTCoordinateConfig()
                                                {
                                                    X = BondX,
                                                    Y = BondY,
                                                };
                                                //更新substrate坐标原点
                                                //_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                //_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                ProductExecutor.Instance.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                ProductExecutor.Instance.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);

                                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet1:PosX:{visionRet.X},PosY:{visionRet.Y}.");
                                                item.IsPositionSuccess = true;

                                                if (_curRecipe.CurrentSubstrate.PositionSubstratePointCount == 2 && ParametersCount > 1)
                                                {
                                                    if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                    {
                                                        visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[1];
                                                    }
                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                    {
                                                        visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.LineSearchParams[1];
                                                    }
                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                    {
                                                        visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.CircleSearchParameters[1];
                                                    }
                                                    X = visionParam.BondTablePositionOfCreatePattern.X + item.MaterialLocation.X - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.X;
                                                    Y = visionParam.BondTablePositionOfCreatePattern.Y + item.MaterialLocation.Y - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.Y;
                                                    Z = visionParam.CameraZWorkPosition;
                                                    ExecutionController.Instance.WaitIfPaused();
                                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                    {
                                                        XYZTCoordinateConfig org_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                        {
                                                            X = X,
                                                            Y = Y,
                                                        };
                                                        ExecutionController.Instance.WaitIfPaused();
                                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                        {
                                                            XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                                            if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                            }
                                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                            }
                                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                            }
                                                            if (visionRet2 != null)
                                                            {
                                                                ExecutionController.Instance.WaitIfPaused();
                                                                //移动到视野中心
                                                                if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet2.X, visionRet2.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                                {
                                                                    BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = BondX,
                                                                        Y = BondY,
                                                                    };

                                                                    double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                    double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                    double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                    double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                    double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                    double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                    double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                    double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                    //更新substrate坐标原点
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = BMCsubangleInDegrees,
                                                                    };

                                                                    XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                        Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                        Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.IsPositionSuccess = true;
                                                                    ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                }
                                                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                            }
                                                            else
                                                            {
                                                                if (ShowMessage2("异常发生！", "搜寻基板Mark2失败，请重新定位到基板Mark2位置！", "警报") == 1)
                                                                {
                                                                    if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                                    }
                                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                                    }
                                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                                    }
                                                                    if (visionRet2 != null)
                                                                    {
                                                                        ExecutionController.Instance.WaitIfPaused();
                                                                        //移动到视野中心
                                                                        if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet2.X, visionRet2.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                                        {
                                                                            BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = BondX,
                                                                                Y = BondY,
                                                                            };

                                                                            double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                            double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                            double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                            double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                            double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                            double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                            double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                            double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                            //更新substrate坐标原点
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = BMCsubangleInDegrees,
                                                                            };

                                                                            XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                                Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                                Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.IsPositionSuccess = true;
                                                                            ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                        }
                                                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ShowMessage2("异常发生！", "搜寻基板Mark2失败，是否手动对准该基板特征位置继续生产？", "警报") == 1)
                                                                        {
                                                                            BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = BondX,
                                                                                Y = BondY,
                                                                            };

                                                                            double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                            double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                            double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                            double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                            double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                            double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                            double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                            double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                            //更新substrate坐标原点
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = BMCsubangleInDegrees,
                                                                            };

                                                                            XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                                Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                                Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.IsPositionSuccess = true;
                                                                            ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                                        }
                                                                        else
                                                                        {
                                                                            item.IsPositionSuccess = false;
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    item.IsPositionSuccess = false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    item.IsPositionSuccess = false;
                                                    ret = GlobalGWResultDefine.RET_SUCCESS;
                                                }



                                            }
                                        }
                                        else
                                        {
                                            if (ShowMessage2("异常发生！", "搜寻基板Mark1失败，是否手动对准该基板特征位置继续生产？", "警报") == 1)
                                            {
                                                BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint1 = new XYZTCoordinateConfig()
                                                {
                                                    X = BondX,
                                                    Y = BondY,
                                                };
                                                //更新substrate坐标原点
                                                //_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                //_curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                ProductExecutor.Instance.SubstrateCoordinateHomePoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                ProductExecutor.Instance.SubstrateCoordinateHomePoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);

                                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet1:PosX:{visionRet.X},PosY:{visionRet.Y}.");
                                                item.IsPositionSuccess = true;

                                                if (_curRecipe.CurrentSubstrate.PositionSubstratePointCount == 2 && ParametersCount > 1)
                                                {
                                                    if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                    {
                                                        visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.ShapeMatchParameters[1];
                                                    }
                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                    {
                                                        visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.LineSearchParams[1];
                                                    }
                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                    {
                                                        visionParam = _curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.CircleSearchParameters[1];
                                                    }
                                                    X = visionParam.BondTablePositionOfCreatePattern.X + item.MaterialLocation.X - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.X;
                                                    Y = visionParam.BondTablePositionOfCreatePattern.Y + item.MaterialLocation.Y - ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.FirstSubstrateHomeSystemLocation.Y;
                                                    Z = visionParam.CameraZWorkPosition;
                                                    ExecutionController.Instance.WaitIfPaused();
                                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                    {
                                                        XYZTCoordinateConfig org_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                        {
                                                            X = X,
                                                            Y = Y,
                                                        };
                                                        ExecutionController.Instance.WaitIfPaused();
                                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                        {
                                                            XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                                            if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                            }
                                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                            }
                                                            else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                            {
                                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                            }
                                                            if (visionRet2 != null)
                                                            {
                                                                ExecutionController.Instance.WaitIfPaused();
                                                                //移动到视野中心
                                                                if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet2.X, visionRet2.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                                {
                                                                    BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = BondX,
                                                                        Y = BondY,
                                                                    };

                                                                    double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                    double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                    double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                    double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                    double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                    double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                    double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                    double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                    double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                    double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                    double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                    double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                    //更新substrate坐标原点
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                    ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                    XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = BMCsubangleInDegrees,
                                                                    };

                                                                    XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                        Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                        Theta = org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                    {
                                                                        X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                        Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                        Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                    };
                                                                    item.IsPositionSuccess = true;
                                                                    ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                }
                                                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                            }
                                                            else
                                                            {
                                                                if (ShowMessage2("异常发生！", "搜寻基板Mark2失败，请重新定位到基板Mark2位置！", "警报") == 1)
                                                                {
                                                                    if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                                    }
                                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                                    }
                                                                    else if (_curRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                                    }
                                                                    if (visionRet2 != null)
                                                                    {
                                                                        ExecutionController.Instance.WaitIfPaused();
                                                                        //移动到视野中心
                                                                        if (_positioningSystem.BondXYUnionMovetoStageCoor(visionRet2.X, visionRet2.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                                                        {
                                                                            BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = BondX,
                                                                                Y = BondY,
                                                                            };

                                                                            double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                            double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                            double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                            double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                            double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                            double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                            double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                            double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                            //更新substrate坐标原点
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = BMCsubangleInDegrees,
                                                                            };

                                                                            XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                                Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                                Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.IsPositionSuccess = true;
                                                                            ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                        }
                                                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ShowMessage2("异常发生！", "搜寻基板Mark2失败，是否手动对准该基板特征位置继续生产？", "警报") == 1)
                                                                        {
                                                                            BondX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            BondY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig cur_SubstrateCoordinateHomePoint2 = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = BondX,
                                                                                Y = BondY,
                                                                            };

                                                                            double org_BMCsubx1 = org_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double org_BMCsuby1 = org_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double org_BMCsubx2 = org_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double org_BMCsuby2 = org_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double org_BMCsubdeltaX = org_BMCsubx2 - org_BMCsubx1;
                                                                            double org_BMCsubdeltaY = org_BMCsuby2 - org_BMCsuby1;
                                                                            double org_BMCsubangleInRadians = Math.Atan2(org_BMCsubdeltaY, org_BMCsubdeltaX);
                                                                            double org_BMCsubangleInDegrees = org_BMCsubangleInRadians * (180.0 / Math.PI);


                                                                            double BMCsubx1 = cur_SubstrateCoordinateHomePoint1.X; // 第一个点的X坐标  
                                                                            double BMCsuby1 = cur_SubstrateCoordinateHomePoint1.Y; // 第一个点的Y坐标  
                                                                            double BMCsubx2 = cur_SubstrateCoordinateHomePoint2.X; // 第二个点的X坐标  
                                                                            double BMCsuby2 = cur_SubstrateCoordinateHomePoint2.Y; // 第二个点的Y坐标  
                                                                            double BMCsubdeltaX = BMCsubx2 - BMCsubx1;
                                                                            double BMCsubdeltaY = BMCsuby2 - BMCsuby1;
                                                                            double BMCsubangleInRadians = Math.Atan2(BMCsubdeltaY, BMCsubdeltaX);
                                                                            double BMCsubangleInDegrees = BMCsubangleInRadians * (180.0 / Math.PI);

                                                                            //更新substrate坐标原点
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            //ProductExecutor.Instance.Substrate.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                                            ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                                                                            XYZTCoordinateConfig BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (cur_SubstrateCoordinateHomePoint1.X + cur_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (cur_SubstrateCoordinateHomePoint1.Y + cur_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = BMCsubangleInDegrees,
                                                                            };

                                                                            XYZTCoordinateConfig org_BondtoBMCsubcenter = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = (org_SubstrateCoordinateHomePoint1.X + org_SubstrateCoordinateHomePoint2.X) / 2,
                                                                                Y = (org_SubstrateCoordinateHomePoint1.Y + org_SubstrateCoordinateHomePoint2.Y) / 2,
                                                                                Theta = org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.PositionSubstrateMark1Result = new XYZTCoordinateConfig()
                                                                            {
                                                                                X = -(BondtoBMCsubcenter.X - org_BondtoBMCsubcenter.X),
                                                                                Y = (BondtoBMCsubcenter.Y - org_BondtoBMCsubcenter.Y),
                                                                                Theta = BMCsubangleInDegrees - org_BMCsubangleInDegrees,
                                                                            };
                                                                            item.IsPositionSuccess = true;
                                                                            ret = GlobalGWResultDefine.RET_SUCCESS;
                                                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"visionRet2:PosX:{visionRet2.X},PosY:{visionRet2.Y}.");
                                                                        }
                                                                        else
                                                                        {
                                                                            item.IsPositionSuccess = false;
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    item.IsPositionSuccess = false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    item.IsPositionSuccess = false;
                                                    ret = GlobalGWResultDefine.RET_SUCCESS;
                                                }

                                            }
                                            else
                                            {
                                                item.IsPositionSuccess = false;
                                            }
                                            
                                        }

                                    }
                                    else
                                    {
                                        
                                        item.IsPositionSuccess = false;
                                    }
                                    
                                }
                            }
                        }

                        ProductExecutor.Instance.CurSubstrateNum++;
                        if (ProductExecutor.Instance.CurSubstrateNum > ProductExecutor.Instance.EndSubstrateNum)
                        {
                            break;
                        }
                    }

                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_CalibrationBeforePickSubstrate Fail.");
                    ret = GlobalGWResultDefine.RET_FAILED;
                }


                return ret;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionSubstrate,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }

        private int ShowMessage2(string title, string content, string type)
        {
            int result = -1;
            var formReadyEvent = new ManualResetEvent(false);
            //WarningBox1.FormShow(title, content, type);
            MessageBox1 myMessageBox1 = new MessageBox1();
            myMessageBox1.OnButtonClicked += (buttonResult) =>
            {
                result = buttonResult == "confirm" ? 1 : 0;
                formReadyEvent.Set();
            };
            myMessageBox1.showMessage(title, content, type);

            while (!formReadyEvent.WaitOne(100))
            {
                Application.DoEvents();
            }

            return result;
        }
    }
    class StepAction_PositionModule : StepActionBase
    {
        public StepAction_PositionModule(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                ProductExecutor.Instance.CurSubstrateNum = 1;
                if (_curRecipe.CurrentSubstrate == null)
                {
                    WarningBox.FormShow("错误", "基板信息为空！", "提示");
                    return GlobalGWResultDefine.RET_FAILED;
                }
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionModule-Start.");
                ExecutionController.Instance.WaitIfPaused();
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    int substrateIndex = 0;

                    foreach (var substrateModules in _curRecipe.CurrentSubstrate.ModuleMapInfos)
                    {
                        // _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].MaterialLocation 第一个基板的坐标（第一个特征点的坐标）
                        var homeX = _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].MaterialLocation.X - _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].PositionSubstrateMark1Result?.X;
                        var homeY = _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].MaterialLocation.Y + _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].PositionSubstrateMark1Result?.Y;
                        List<MaterialMapInformation> temp = new List<MaterialMapInformation>();
                        var positionBondChipCounter = 0;
                        List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>> tempList = new List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>>();
                        foreach (var itemModule in substrateModules)
                        {
                            //移动到识别Module位置
                            //double X = itemSub.k.MaterialLocation.X - visionParam.PatternOffsetWithMaterialCenter.X;
                            //double Y = itemSub.Item1.MaterialLocation.Y - visionParam.PatternOffsetWithMaterialCenter.Y;

                            //double Z = visionParam.CameraZWorkPosition;
                            //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);
                            //itemSub.Item1.PositionModuleResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                            //itemSub.Item1.IsPositionSuccess = itemSub.Item1.PositionModuleResult == null ? false : true;

                            if (ProductExecutor.Instance.RunStat != EnumProductRunStat.UserAbort)
                            {
                                if (ProductExecutor.Instance.IsProcessPart)
                                {
                                    if (positionBondChipCounter >= ProductExecutor.Instance.ManualSettedProcessCount)
                                    {
                                        break;
                                    }
                                }

                                VisionIdentificationParam visionParam = new VisionIdentificationParam();
                                int ParametersCount = 0;
                                if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    visionParam = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.ShapeMatchParameters[0];
                                    ParametersCount = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.ShapeMatchParameters.Count;
                                }
                                else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    visionParam = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.LineSearchParams[0];
                                    ParametersCount = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.LineSearchParams.Count;
                                }
                                else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                {
                                    visionParam = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.CircleSearchParameters[0];
                                    ParametersCount = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.CircleSearchParameters.Count;
                                }

                                double X = itemModule.MaterialLocation.X - visionParam.PatternOffsetWithMaterialCenter.X + (double)homeX;
                                double Y = itemModule.MaterialLocation.Y - visionParam.PatternOffsetWithMaterialCenter.Y + (double)homeY;
                                double Z = visionParam.CameraZWorkPosition;
                                ExecutionController.Instance.WaitIfPaused();
                                if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                    && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                {
                                    XYZTCoordinateConfig PositionBondChipResult = new XYZTCoordinateConfig();
                                    if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                    {
                                        PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                    }
                                    else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                    {
                                        PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                    }
                                    else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                    {
                                        PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                    }

                                    if (PositionBondChipResult != null)
                                    {


                                        if (ProductExecutor.Instance.ProductRecipe.CurrentSubstrate.PositionModulePointCount == 2 && ParametersCount > 1)
                                        {
                                            VisionIdentificationParam visionParam2 = new VisionIdentificationParam();
                                            if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                            {
                                                visionParam2 = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.ShapeMatchParameters[1];
                                            }
                                            else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                            {
                                                visionParam2 = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.LineSearchParams[1];
                                            }
                                            else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                            {
                                                visionParam2 = _curRecipe.CurrentSubstrate.PositionModuleVisionParameters.CircleSearchParameters[1];
                                            }

                                            double X2 = itemModule.MaterialLocation.X - visionParam2.PatternOffsetWithMaterialCenter.X + (double)homeX;
                                            double Y2 = itemModule.MaterialLocation.Y - visionParam2.PatternOffsetWithMaterialCenter.Y + (double)homeY;
                                            double Z2 = visionParam2.CameraZWorkPosition;
                                            ExecutionController.Instance.WaitIfPaused();
                                            if (_positioningSystem.BondXYUnionMovetoSystemCoor(X2, Y2, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z2, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                            {
                                                XYZTCoordinateConfig PositionBondChipResult2 = new XYZTCoordinateConfig();
                                                if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                {
                                                    PositionBondChipResult2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam2);
                                                }
                                                else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                {
                                                    PositionBondChipResult2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam2);
                                                }
                                                else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                {
                                                    PositionBondChipResult2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam2);
                                                }

                                                if (PositionBondChipResult2 != null)
                                                {
                                                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PositionModule:{itemModule.MaterialNumber};VisionX:{PositionBondChipResult.X},VisionY:{PositionBondChipResult.Y}.");
                                                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PositionModule:{itemModule.MaterialNumber};VisionX2:{PositionBondChipResult2.X},VisionY2:{PositionBondChipResult2.Y}.");
                                                    itemModule.MaterialLocation.X = (float)(itemModule.MaterialLocation.X - (PositionBondChipResult.X / 2 + PositionBondChipResult2.X / 2));
                                                    itemModule.MaterialLocation.Y = (float)(itemModule.MaterialLocation.Y + (PositionBondChipResult.Y / 2 + PositionBondChipResult2.Y / 2));

                                                    positionBondChipCounter++;
                                                }
                                                else
                                                {
                                                    LogRecorder.RecordLog(EnumLogContentType.Warn, $"RecognizeModule Failed.MaterialNumber:{itemModule.MaterialNumber}");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PositionModule:{itemModule.MaterialNumber};VisionX:{PositionBondChipResult.X},VisionY:{PositionBondChipResult.Y}.");
                                            itemModule.MaterialLocation.X = (float)(itemModule.MaterialLocation.X - PositionBondChipResult.X);
                                            itemModule.MaterialLocation.Y = (float)(itemModule.MaterialLocation.Y + PositionBondChipResult.Y);
                                            positionBondChipCounter++;
                                        }



                                    }
                                    else
                                    {
                                        LogRecorder.RecordLog(EnumLogContentType.Warn, $"RecognizeModule Failed.MaterialNumber:{itemModule.MaterialNumber}");
                                    }
                                }

                            }


                        }
                        substrateIndex++;
                    }
                    //ProductExecutor.Instance.OffsetBeforePickupSubstrate = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                    //if (ProductExecutor.Instance.OffsetBeforePickupSubstrate != null)
                    //{
                    //    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionModule-VisionEnd.");
                    //    _positioningSystem.PPMovetoSafeLocation();
                    //    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionModule-End.");
                    //    return GlobalGWResultDefine.RET_SUCCESS;
                    //}

                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_CalibrationBeforePickSubstrate Fail.");
                    return GlobalGWResultDefine.RET_FAILED;
                }



                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionModule,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }
    
    /*
     * 相机取 芯片 前识别校准 步骤Action
     */
    public class StepAction_PositionComponent : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_PositionComponent(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        {
           
            if (actionNo == EnumActionNo.Action_PositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if(actionNo == EnumActionNo.Action_PositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if(componentType == EnumComponentType.Component)
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionComponent-Start.");

                    VisionIdentificationParam visionParam = new VisionIdentificationParam();
                    int ParametersCount = 0;
                    if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    {
                        visionParam = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[0];
                        ParametersCount = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.Count;
                    }
                    else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    {
                        visionParam = CurChipParam.PositionComponentVisionParameters.LineSearchParams[0];
                        ParametersCount = CurChipParam.PositionComponentVisionParameters.LineSearchParams.Count;
                    }
                    else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                    {
                        visionParam = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[0];
                        ParametersCount = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters.Count;
                    }
                    double X = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X + ProductExecutor.Instance.MaterialLocationOffsetX;
                    double Y = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y + ProductExecutor.Instance.MaterialLocationOffsetY;
                    double Z = visionParam.CameraZWorkPosition;

                    if (CurChipParam.CarrierType == EnumCarrierType.WafflePack)
                    {
                        if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.Count;
                        }
                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.LineSearchParams[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.LineSearchParams.Count;
                        }
                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters.Count;
                        }
                        X = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X + visionParam.PatternOffsetWithMaterialCenter.X + ProductExecutor.Instance.MaterialLocationOffsetX;
                        Y = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y + visionParam.PatternOffsetWithMaterialCenter.Y + ProductExecutor.Instance.MaterialLocationOffsetY;
                        Z = visionParam.CameraZWorkPosition;
                        //CameraWindowGUI.Instance?.SelectCamera(0);
                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            XYZTCoordinateConfig visionRet = new XYZTCoordinateConfig();
                            if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                            }
                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                            }
                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                            {
                                visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                            }

                            CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].PositionModuleResult = visionRet;
                            ProductExecutor.Instance.OffsetBeforePickupChip = visionRet;
                            ProductExecutor.Instance.OffsetBeforePickupChip = new XYZTCoordinateConfig()
                            {
                                X = visionRet.X + visionParam.PatternOffsetWithMaterialCenter.X,
                                Y = visionRet.Y - visionParam.PatternOffsetWithMaterialCenter.Y,
                                Theta = visionRet.Theta,
                            };
                            if (visionRet == null)
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            else
                            {
                                if(CurChipParam.PositionMarkPointCount == 2 && ParametersCount > 1)
                                {
                                    VisionIdentificationParam visionParam2 = new VisionIdentificationParam();
                                    if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                    {
                                        visionParam2 = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[1];
                                    }
                                    else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                    {
                                        visionParam2 = CurChipParam.PositionComponentVisionParameters.LineSearchParams[1];
                                    }
                                    else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                    {
                                        visionParam2 = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[1];
                                    }
                                    double X2 = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X + visionParam2.PatternOffsetWithMaterialCenter.X + ProductExecutor.Instance.MaterialLocationOffsetX;
                                    double Y2 = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y + visionParam2.PatternOffsetWithMaterialCenter.Y + ProductExecutor.Instance.MaterialLocationOffsetY;
                                    double Z2 = visionParam2.CameraZWorkPosition;
                                    //CameraWindowGUI.Instance?.SelectCamera(0);
                                    ExecutionController.Instance.WaitIfPaused();
                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(X2, Y2, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z2, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                    {
                                        XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                        if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                        {
                                            visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam2);
                                        }
                                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                        {
                                            visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam2);
                                        }
                                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                        {
                                            visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam2);
                                        }

                                        //CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].PositionModuleResult = visionRet;
                                        //ProductExecutor.Instance.OffsetBeforePickupChip = visionRet;
                                        ProductExecutor.Instance.OffsetBeforePickupChip = new XYZTCoordinateConfig()
                                        {
                                            X = (visionRet.X + visionRet2.X) / 2 + visionParam2.PatternOffsetWithMaterialCenter.X,
                                            Y = (visionRet.Y + visionRet2.Y) / 2 - visionParam2.PatternOffsetWithMaterialCenter.Y,
                                            Theta = (visionRet.Theta + visionRet.Theta)/2,
                                        };
                                        if (visionRet == null)
                                        {
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                    }
                                    else
                                    {
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }

                            }
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                    }
                    else if (CurChipParam.CarrierType == EnumCarrierType.Wafer)
                    {
                        if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.Count;
                        }
                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.LineSearchParams[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.LineSearchParams.Count;
                        }
                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters.Count;
                        }
                        //CameraWindowGUI.Instance?.SelectCamera(2);
                        var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == CurChipParam.RelatedESToolName);
                        if (usedESTool != null)
                        {
                            var xx = 0f;
                            var yy = 0f;
                            Z = visionParam.CameraZWorkPosition;
                            var curComp = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1];

                            xx = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X - CurChipParam.ComponentMapInfos[0].MaterialLocation.X - (float)ProductExecutor.Instance.MaterialLocationOffsetX
                                + (float)visionParam.PatternOffsetWithMaterialCenter.X;
                            yy = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y - CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - (float)ProductExecutor.Instance.MaterialLocationOffsetY 
                                + (float)visionParam.PatternOffsetWithMaterialCenter.Y;
                            ExecutionController.Instance.WaitIfPaused();
                            if (
                            //    _positioningSystem.BondMovetoSafeLocation()
                            ////顶针移动到零点
                            //&& 
                            _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, CurChipParam.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //顶针座升起
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurChipParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                XYZTCoordinateConfig visionRet = new XYZTCoordinateConfig();
                                if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (MatchIdentificationParam)visionParam);
                                }
                                else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (LineFindIdentificationParam)visionParam);
                                }
                                else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                {
                                    visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (CircleFindIdentificationParam)visionParam);
                                }
                                CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].PositionModuleResult = visionRet;
                                ProductExecutor.Instance.OffsetBeforePickupChip = visionRet;
                                if (visionRet == null)
                                {
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                                else
                                {
                                    if (CurChipParam.PositionMarkPointCount == 2 && ParametersCount > 1)
                                    {
                                        VisionIdentificationParam visionParam2 = new VisionIdentificationParam();
                                        if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                        {
                                            visionParam2 = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[1];
                                        }
                                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                        {
                                            visionParam2 = CurChipParam.PositionComponentVisionParameters.LineSearchParams[1];
                                        }
                                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                        {
                                            visionParam2 = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[1];
                                        }
                                        var xx2 = 0f;
                                        var yy2 = 0f;
                                        double Z2 = visionParam2.CameraZWorkPosition;

                                        xx2 = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X - CurChipParam.ComponentMapInfos[0].MaterialLocation.X - (float)ProductExecutor.Instance.MaterialLocationOffsetX
                                            + (float)visionParam2.PatternOffsetWithMaterialCenter.X;
                                        yy2 = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y - CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - (float)ProductExecutor.Instance.MaterialLocationOffsetY
                                            + (float)visionParam2.PatternOffsetWithMaterialCenter.Y;

                                        ExecutionController.Instance.WaitIfPaused();
                                        //CameraWindowGUI.Instance?.SelectCamera(0);
                                        if ( _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, CurChipParam.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                        //顶针座升起
                                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurChipParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z2, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                        {
                                            XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                            if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                            {
                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (MatchIdentificationParam)visionParam2);
                                            }
                                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                            {
                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (LineFindIdentificationParam)visionParam2);
                                            }
                                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                            {
                                                visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (CircleFindIdentificationParam)visionParam2);
                                            }
                                            
                                            ProductExecutor.Instance.OffsetBeforePickupChip = visionRet;
                                            if (visionRet2 == null)
                                            {
                                                return GlobalGWResultDefine.RET_FAILED;
                                            }
                                            else
                                            {
                                                ExecutionController.Instance.WaitIfPaused();
                                                //物料移动到视野中心
                                                if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, (visionRet.X + visionRet2.X) / 2 
                                                    - (float)visionParam2.PatternOffsetWithMaterialCenter.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                                || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, (visionRet.X + visionRet2.X) / 2 
                                                - (float)visionParam2.PatternOffsetWithMaterialCenter.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                                {
                                                    //移至中心失败
                                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                                                    return GlobalGWResultDefine.RET_FAILED;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                    }
                                    else
                                    {
                                        ExecutionController.Instance.WaitIfPaused();
                                        //物料移动到视野中心
                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X - (float)visionParam.PatternOffsetWithMaterialCenter.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                        || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y - (float)visionParam.PatternOffsetWithMaterialCenter.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                        {
                                            //移至中心失败
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                    }



                                }
                            }
                            else
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-顶针工具无效.");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (CurChipParam.CarrierType == EnumCarrierType.WaferWafflePack)
                    {
                        if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.Count;
                        }
                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.LineSearchParams[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.LineSearchParams.Count;
                        }
                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                        {
                            visionParam = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[0];
                            ParametersCount = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters.Count;
                        }
                        //CameraWindowGUI.Instance?.SelectCamera(2);
                        var xx = 0f;
                        var yy = 0f;
                        var curComp = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1];

                        xx = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X - CurChipParam.ComponentMapInfos[0].MaterialLocation.X - (float)ProductExecutor.Instance.MaterialLocationOffsetX 
                            + (float)visionParam.PatternOffsetWithMaterialCenter.X;
                        yy = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y - CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - (float)ProductExecutor.Instance.MaterialLocationOffsetY 
                            + (float)visionParam.PatternOffsetWithMaterialCenter.Y;
                        ExecutionController.Instance.WaitIfPaused();
                        //移动wafertable
                        if (
                        //    _positioningSystem.BondMovetoSafeLocation()
                        //&& 
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, CurChipParam.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            XYZTCoordinateConfig visionRet = new XYZTCoordinateConfig();
                            if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (MatchIdentificationParam)visionParam);
                            }
                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (LineFindIdentificationParam)visionParam);
                            }
                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                            {
                                visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (CircleFindIdentificationParam)visionParam);
                            }

                            CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].PositionModuleResult = visionRet;
                            ProductExecutor.Instance.OffsetBeforePickupChip = visionRet;
                            if (visionRet == null)
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            else
                            {
                                


                                if (CurChipParam.PositionMarkPointCount == 2 && CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.Count > 1)
                                {
                                    VisionIdentificationParam visionParam2 = new VisionIdentificationParam();
                                    if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                    {
                                        visionParam2 = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters[1];
                                    }
                                    else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                    {
                                        visionParam2 = CurChipParam.PositionComponentVisionParameters.LineSearchParams[1];
                                    }
                                    else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                    {
                                        visionParam2 = CurChipParam.PositionComponentVisionParameters.CircleSearchParameters[1];
                                    }
                                    var xx2 = 0f;
                                    var yy2 = 0f;
                                    double Z2 = visionParam2.CameraZWorkPosition;

                                    xx2 = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X - CurChipParam.ComponentMapInfos[0].MaterialLocation.X - (float)ProductExecutor.Instance.MaterialLocationOffsetX
                                        + (float)visionParam2.PatternOffsetWithMaterialCenter.X;
                                    yy2 = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y - CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - (float)ProductExecutor.Instance.MaterialLocationOffsetY
                                        + (float)visionParam2.PatternOffsetWithMaterialCenter.Y;

                                    ExecutionController.Instance.WaitIfPaused();
                                    //CameraWindowGUI.Instance?.SelectCamera(0);
                                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, CurChipParam.ComponentMapInfos[0].MaterialLocation.X - xx2, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                    && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, CurChipParam.ComponentMapInfos[0].MaterialLocation.Y - yy2, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                    && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z2, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                    {
                                        XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                        if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                        {
                                            visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (MatchIdentificationParam)visionParam2);
                                        }
                                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                        {
                                            visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (LineFindIdentificationParam)visionParam2);
                                        }
                                        else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                        {
                                            visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, (CircleFindIdentificationParam)visionParam2);
                                        }

                                        ProductExecutor.Instance.OffsetBeforePickupChip = visionRet;
                                        if (visionRet2 == null)
                                        {
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                        else
                                        {
                                            ExecutionController.Instance.WaitIfPaused();
                                            //物料移动到视野中心
                                            if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, (visionRet.X + visionRet2.X) / 2
                                                - (float)visionParam2.PatternOffsetWithMaterialCenter.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                            || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, (visionRet.X + visionRet2.X) / 2
                                            - (float)visionParam2.PatternOffsetWithMaterialCenter.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                            {
                                                //移至中心失败
                                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                                                return GlobalGWResultDefine.RET_FAILED;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }
                                else
                                {
                                    ExecutionController.Instance.WaitIfPaused();
                                    //物料移动到视野中心
                                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X - (float)visionParam.PatternOffsetWithMaterialCenter.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                    || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y - (float)visionParam.PatternOffsetWithMaterialCenter.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                    {
                                        //移至中心失败
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }


                            }
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }

                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionComponent-End.");
                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionComponent-Start.");
                    MatchIdentificationParam visionParam = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault();

                    double X = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.X + ProductExecutor.Instance.MaterialSubmonutLocationOffsetX;
                    double Y = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.Y + ProductExecutor.Instance.MaterialSubmonutLocationOffsetY;
                    double Z = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;

                    if (CurSubmonutParam.CarrierType == EnumCarrierType.WafflePack)
                    {
                        CameraWindowGUI.Instance?.SelectCamera(0);
                        if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {


                            var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                            CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].PositionModuleResult = visionRet;
                            ProductExecutor.Instance.OffsetBeforePickupSubmonut = visionRet;
                            if (visionRet == null)
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                    }
                    else if (CurSubmonutParam.CarrierType == EnumCarrierType.Wafer)
                    {
                        CameraWindowGUI.Instance?.SelectCamera(2);
                        var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedESToolName);
                        if (usedESTool != null)
                        {
                            var xx = 0f;
                            var yy = 0f;
                            var curComp = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1];

                            xx = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.X - CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.X - (float)ProductExecutor.Instance.MaterialSubmonutLocationOffsetX;
                            yy = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.Y - CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.Y - (float)ProductExecutor.Instance.MaterialSubmonutLocationOffsetY;
                            if (_positioningSystem.BondMovetoSafeLocation()
                            //顶针移动到零点
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //顶针座升起
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurSubmonutParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, visionParam);
                                CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].PositionModuleResult = visionRet;
                                ProductExecutor.Instance.OffsetBeforePickupSubmonut = visionRet;
                                if (visionRet == null)
                                {
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                                else
                                {
                                    //物料移动到视野中心
                                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                    || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                    {
                                        //移至中心失败
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                    ////物料移动到顶针中心
                                    //if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X + usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                    //|| _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y - usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                    //{
                                    //    //移至中心失败
                                    //    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至顶针中心失败.");
                                    //    return GlobalGWResultDefine.RET_FAILED;
                                    //}
                                    //for(int i_0 = 0; i_0 < 3; i_0++)
                                    //{
                                    //    var visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, visionParam);
                                    //    if (visionRet2 == null)
                                    //    {
                                    //        return GlobalGWResultDefine.RET_FAILED;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X - usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                    //|| _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y + usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                    //        {
                                    //            //移至中心失败
                                    //            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至顶针中心失败.");
                                    //            return GlobalGWResultDefine.RET_FAILED;
                                    //        }
                                    //    }
                                    //}

                                }
                            }
                            else
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-顶针工具无效.");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (CurSubmonutParam.CarrierType == EnumCarrierType.WaferWafflePack)
                    {
                        CameraWindowGUI.Instance?.SelectCamera(2);
                        var xx = 0f;
                        var yy = 0f;
                        var curComp = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1];

                        xx = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.X - CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.X - (float)ProductExecutor.Instance.MaterialSubmonutLocationOffsetX;
                        yy = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.Y - CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.Y - (float)ProductExecutor.Instance.MaterialSubmonutLocationOffsetY;
                        //移动wafertable
                        if (_positioningSystem.BondMovetoSafeLocation()
                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.X - xx, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, CurSubmonutParam.ComponentMapInfos[0].MaterialLocation.Y - yy, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            CameraWindowGUI.Instance.SelectCamera(2);
                            var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, visionParam);
                            CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].PositionModuleResult = visionRet;
                            ProductExecutor.Instance.OffsetBeforePickupSubmonut = visionRet;
                            if (visionRet == null)
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            else
                            {
                                //物料移动到视野中心
                                if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, visionRet.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, visionRet.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                                {
                                    //移至中心失败
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }

                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PositionComponent-End.");
                }
                
                
                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionComponent,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }

    /*
     * 物料Step 3 - 吸取物料 步骤Action
     */
    class StepAction_PickUpSubstrate : StepActionBase
    {
        public StepAction_PickUpSubstrate(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }
        BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                //if (ProductExecutor.Instance.OffsetBeforePickupSubstrate != null)
                //{
                //    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpSubstrate-Start.");
                //    var materialOrigionA = _curRecipe.SubstrateInfos.PositionSubmountVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //    var targetA = ProductExecutor.Instance.OffsetBeforePickupSubstrate.Theta - materialOrigionA;
                //    //衬底吸嘴复位
                //    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                //    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                //    _positioningSystem.PPMovetoSafeLocation();

                //    //衬底角度补偿
                //    if (targetA > 0)
                //    {
                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, -targetA, EnumCoordSetType.Relative);
                //    }
                //    else
                //    {
                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                //    }

                //    //Thread.Sleep(500);



                //    //吸嘴移动到衬底中心上方
                //    var offset = _systemConfig.PositioningConfig.PP2AndBondCameraOffset;
                //    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ProductExecutor.Instance.OffsetBeforePickupSubstrate.X + offset.X, EnumCoordSetType.Relative);
                //    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ProductExecutor.Instance.OffsetBeforePickupSubstrate.Y + offset.Y, EnumCoordSetType.Relative);
                //    _positioningSystem.BondXYUnionMovetoStageCoor(ProductExecutor.Instance.OffsetBeforePickupSubstrate.X + offset.X
                //        , ProductExecutor.Instance.OffsetBeforePickupSubstrate.Y + offset.Y, EnumCoordSetType.Relative);



                //    //衬底吸嘴拾取衬底，TBD-此处的高度应该用吸嘴工具和物料参数计算
                //    _curRecipe.SubstrateInfos.PPSettings.WorkHeight = _curRecipe.SubstrateInfos.SubmountPPPickPos;
                //    if (PPUtility.Instance.PickViaSystemCoor(_curRecipe.SubstrateInfos.PPSettings))
                //    {
                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, targetA, EnumCoordSetType.Relative);
                //    }
                //    else
                //    {
                //        LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取衬底失败！");
                //        WarningBox.FormShow("错误", "拾取衬底失败！");
                //        return GlobalGWResultDefine.RET_FAILED;
                //    }
                //    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpSubstrate-End.");
                //    return GlobalGWResultDefine.RET_SUCCESS;

                //}
                return GlobalGWResultDefine.RET_FAILED;
                //Thread.Sleep(2000);

            }
            catch (Exception ex)
            {

                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PickUpSubstrate,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }

    /// <summary>
    /// 芯片二次校准，二次校准完成后，这个动作没有移动XY到贴装位置上方，移动动作还是放在了Bond动作里
    /// </summary>
    public class StepAction_AccuracyPositionChip : StepActionBase
    {
        public StepAction_AccuracyPositionChip(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        { 

        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_MoveToLookupCamPos-Start.");
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    {
                        var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;

                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                        //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                        double X = 0d;
                        double Y = 0d;
                        double Z = 0d;
                        var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {


                            if (pptool != null)
                            {
                                X = pptool.ChipPPPosCompensateCoordinate1.X;
                                Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                            }
                            else
                            {
                                X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            //Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                            Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZChipSystemWorkPosition;
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            if (pptool != null)
                            {
                                X = pptool.ChipPPPosCompensateCoordinate1.X;
                                Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                            }
                            else
                            {
                                X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            //Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                            Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZChipSystemWorkPosition;
                        }


                        //吸嘴旋转补偿
                        //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

                        //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                        //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

                        //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                        //double angle = targetA;
                        //double angle0 = 0;

                        //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);

                        //X += point3.X;
                        //Y += point3.Y;

                        //-----通过物料名取物料对象，取物料首位置 X Y Z    end -----

                        //移动bond头
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);


                        //XY联动移动到物料上方
                        EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                        multiAxis[0] = EnumStageAxis.BondX;
                        multiAxis[1] = EnumStageAxis.BondY;
                        double[] targets = new double[2];
                        targets[0] = X;
                        targets[1] = Y;
                        //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                        //拾取之后移动到仰视相机上方

                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);
                        //按芯片吸嘴的系统坐标系移动
                        var PPToolZero = 0f;
                        if (pptool != null)
                        {
                            PPToolZero = pptool.AltimetryOnMark;
                        }
                        else
                        {
                            PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        }
                        
                        if(
                            //_positioningSystem.ChipPPMovetoUplookingCameraCenter()
                            _positioningSystem.PPtoolMovetoUplookingCameraCenter()
                        && _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute)==StageMotionResult.Success)
                        {
                            if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                            {
                                if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                                {
                                    _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute);
                                }
                            }
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                    }


                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-Start.");
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                    {
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                            ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                return GlobalGWResultDefine.RET_SUCCESS;
                            }
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();

                            ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (_positioningSystem.BondZMovetoSafeLocation())
                            {
                                if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                    return GlobalGWResultDefine.RET_SUCCESS;
                                }
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Failed.");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                    }
                    else
                    {
                        ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                    }
                }

                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }
    /// <summary>
    /// 二次校准前先把偏移角度旋转到位，依据二次校准的芯片中心结果直接进行贴装（一次识别），二次校准完成后，这个动作没有移动XY到贴装位置上方，移动动作还是放在了Bond动作里
    /// </summary>
    public class StepAction_AccuracyPositionChipOptNoAngleCalibration : StepActionBase
    {
        public StepAction_AccuracyPositionChipOptNoAngleCalibration(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_MoveToLookupCamPos-Start.");
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    var curSubstrate = ProductExecutor.Instance.ProductRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                    var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                    var curDealBP = CurBondPosition;
                    if (CurBondPosition != null)
                    {
                        curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                    }
                    //if (!curDealBP.IsPositionSuccess)
                    //{
                    //    return GlobalGWResultDefine.RET_BPInvalid;
                    //}
                    //吸嘴旋转补偿
                    //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //var targetB = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionB;
                    //配方设置的贴装位置偏转角
                    var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                    var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //二次校准时模板初始角度
                    var angleofPosChipPattern = 0f;
                    if (CurChipParam.PositionComponentVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    {
                        angleofPosChipPattern = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    }
                    else if (CurChipParam.PositionComponentVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    {
                        angleofPosChipPattern = CurChipParam.PositionComponentVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                    }
                    //贴装补偿的角度
                    var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                    //var targetA = targetB + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                    //    + materialOrigionA + compensateT - materialOrigionC;curDealBP
                    //var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                    //            + materialOrigionA + compensateT - materialOrigionC;
                    //Theta轴移动此角度后将chip转正
                    var finalAngle = ProductExecutor.Instance.OffsetBeforePickupChip.Theta+ angleofPosChipPattern + bondPosOffsetTheta - curDealBP.PositionBondChipResult.Theta
                                  + bondPosOrigionAngle + compensateT;

                    LogRecorder.ProductionLog(EnumLogContentType.Info, $"StepAction_AccuracyPositionChipOptNoAngleCalibration-FinalAngle:{finalAngle}.");
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Absolute) != StageMotionResult.Success)
                    {
                        var curAngle = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"StepAction_AccuracyPositionChipOptNoAngleCalibration-CurAngle after FinalAngle:{curAngle}.");
                        //LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Error.");
                        //return GlobalGWResultDefine.RET_FAILED;
                    }



                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    {
                        var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;

                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                        //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                        double X = 0d;
                        double Y = 0d;
                        double Z = 0d;
                        var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {


                            if (pptool != null)
                            {
                                X = pptool.ChipPPPosCompensateCoordinate1.X;
                                Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                            }
                            else
                            {
                                X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            //Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                            Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZChipSystemWorkPosition;
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            if (pptool != null)
                            {
                                X = pptool.ChipPPPosCompensateCoordinate1.X;
                                Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                            }
                            else
                            {
                                X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            //Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                            Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZChipSystemWorkPosition;
                        }


                        //吸嘴旋转补偿
                        //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

                        //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                        //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

                        //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                        //double angle = targetA;
                        //double angle0 = 0;

                        //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);

                        //X += point3.X;
                        //Y += point3.Y;

                        //-----通过物料名取物料对象，取物料首位置 X Y Z    end -----

                        //移动bond头
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);


                        //XY联动移动到物料上方
                        EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                        multiAxis[0] = EnumStageAxis.BondX;
                        multiAxis[1] = EnumStageAxis.BondY;
                        double[] targets = new double[2];
                        targets[0] = X;
                        targets[1] = Y;
                        //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                        //拾取之后移动到仰视相机上方

                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);
                        //按芯片吸嘴的系统坐标系移动
                        var PPToolZero = 0f;
                        if (pptool != null)
                        {
                            PPToolZero = pptool.AltimetryOnMark;
                        }
                        else
                        {
                            PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        }
                        if (_positioningSystem.ChipPPMovetoUplookingCameraCenter()
                        && _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {

                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                    }


                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-Start.");
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                    {
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                            ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                return GlobalGWResultDefine.RET_SUCCESS;
                            }
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();

                            ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (_positioningSystem.BondZMovetoSafeLocation())
                            {
                                if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                    return GlobalGWResultDefine.RET_SUCCESS;
                                }
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Failed.");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                    }
                    else
                    {
                        ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                    }
                }

                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }
    /// <summary>
    /// s二次校准时识别两次，一次补角度，另外一次找中心，没有使用旋转补偿，二次校准完成后，这个动作没有移动XY到贴装位置上方，移动动作还是放在了Bond动作里
    /// </summary>
    public class StepAction_AccuracyPositionChipMultiVision : StepActionBase
    {
        public StepAction_AccuracyPositionChipMultiVision(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionChipMultiVision-Start.");
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    {
                        var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;

                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                        //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                        double X = 0d;
                        double Y = 0d;
                        double Z = 0d;
                        var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {


                            if (pptool != null)
                            {
                                X = pptool.ChipPPPosCompensateCoordinate1.X;
                                Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                            }
                            else
                            {
                                X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            //Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                            Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZChipSystemWorkPosition;
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            if (pptool != null)
                            {
                                X = pptool.ChipPPPosCompensateCoordinate1.X;
                                Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                            }
                            else
                            {
                                X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            //Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                            Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZChipSystemWorkPosition;
                        }

                        //XY联动移动到物料上方
                        EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                        multiAxis[0] = EnumStageAxis.BondX;
                        multiAxis[1] = EnumStageAxis.BondY;
                        double[] targets = new double[2];
                        targets[0] = X;
                        targets[1] = Y;
                        //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                        //拾取之后移动到仰视相机上方

                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);
                        //按芯片吸嘴的系统坐标系移动
                        var PPToolZero = 0f;
                        if (pptool != null)
                        {
                            PPToolZero = pptool.AltimetryOnMark;
                        }
                        else
                        {
                            PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        }
                        if (_positioningSystem.ChipPPMovetoUplookingCameraCenter()
                        && _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {

                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                    }


                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-Start.");
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                    {
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                            ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                return GlobalGWResultDefine.RET_SUCCESS;
                            }
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                            //Thread.Sleep(10000);

                            var firstVisionResult  = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (firstVisionResult == null)
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipMultiVision,First Vision Failed.");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipMultiVision,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                            ProductExecutor.Instance.OffsetAfterChipAccuracy = firstVisionResult;
                            #region 计算最终的贴装角度
                            BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                            var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                            var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                            var curDealBP = CurBondPosition;
                            if (CurBondPosition != null)
                            {
                                curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);

                                var tempCounter = ProductExecutor.Instance.CurModuleNum - 1;
                                while (!curDealBP.IsPositionSuccess)
                                {
                                    tempCounter++;
                                    if (tempCounter >= curSubstrate.Count)
                                    {
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipMultiVision Failed.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                    curModule = curSubstrate[tempCounter];
                                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                                }
                            }
                            var bondPosOffsetTheta = curDealBP.BondPositionWithPatternOffset.Theta;
                            var bondPosOrigionAngle = curDealBP.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //二次校准时模板初始角度
                            var angleofChipAccuracyPattern = 0f;
                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            }
                            else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                            }
                            //贴装补偿的角度
                            var compensateT = curDealBP.BondPositionCompensation.Theta;
                            //Theta轴移动此角度后将chip转正
                            var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta - angleofChipAccuracyPattern - curDealBP.PositionBondChipResult.Theta
                                          + bondPosOrigionAngle + compensateT + bondPosOffsetTheta;
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipMultiVision,FinalAngle:{finalAngle}");
                            #endregion
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipMultiVision,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                            //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Relative) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipMultiVision,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                                //Thread.Sleep(10000);
                                ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);

                                if (_positioningSystem.BondZMovetoSafeLocation())
                                {
                                    if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                                    {
                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipMultiVision,IdentificationAsyncSecond:{ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta}");
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                        return GlobalGWResultDefine.RET_SUCCESS;
                                    }
                                }
                                else
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Failed.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                        }
                    }
                    else
                    {
                        ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                    }
                }

                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyCalibrationChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }
    /// <summary>
    /// 使用仰视相机二次校准时识别两次，一次补角度，另外一次找中心，没有使用旋转补偿，校准完成之后，移动到贴装位置上方
    /// </summary>
    public class StepAction_AccuracyPositionWithUplookCamera : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_AccuracyPositionWithUplookCamera(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        {
            if (actionNo == EnumActionNo.Action_PositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_PositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if(componentType == EnumComponentType.Component)
                {
                    VisionIdentificationParam visionParam = new VisionIdentificationParam();
                    int ParametersCount = 0;
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    {
                        visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters[0];
                        ParametersCount = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.Count;
                    }
                    else if (CurChipParam.AccuracyComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    {
                        visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams[0];
                        ParametersCount = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.Count;
                    }
                    else if (CurChipParam.AccuracyComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                    {
                        visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.CircleSearchParameters[0];
                        ParametersCount = CurChipParam.AccuracyComponentPositionVisionParameters.CircleSearchParameters.Count;
                    }
                    CameraWindowGUI.Instance?.SelectCamera(1);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionWithUplookCamera-Start.");
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    ExecutionController.Instance.WaitIfPaused();
                    if (_positioningSystem.BondZMovetoSafeLocation())
                    {
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                        {
                            var materialOrigionA = visionParam.OrigionAngle;
                            //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;

                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                            //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                            double X = 0d;
                            double Y = 0d;
                            double Z = 0d;

                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {


                                if (pptool != null)
                                {
                                    X = pptool.LookuptoPPOrigion.X;
                                    Y = pptool.LookuptoPPOrigion.Y;
                                }
                                else
                                {
                                    X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                    Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                                }
                                //Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                                Z = visionParam.CameraZChipSystemWorkPosition;
                            }
                            else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                if (pptool != null)
                                {
                                    X = pptool.LookuptoPPOrigion.X;
                                    Y = pptool.LookuptoPPOrigion.Y;
                                }
                                else
                                {
                                    X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                    Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                                }
                                //Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                                Z = visionParam.CameraZChipSystemWorkPosition;
                            }

                            var materialOrigionA_init = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA_init;
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupChip.Theta}");
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-targetAngle:{targetA}");
                            //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);
                            //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");


                            //XY联动移动到物料上方
                            EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                            multiAxis[0] = EnumStageAxis.BondX;
                            multiAxis[1] = EnumStageAxis.BondY;
                            multiAxis[2] = pptool.StageAxisTheta;
                            //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                            //拾取之后移动到仰视相机上方

                            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);


                            double[] Target = new double[3];
                            Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                            Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            if (pptool != null)
                            {
                                Target[0] = pptool.LookuptoPPOrigion.X;
                                Target[1] = pptool.LookuptoPPOrigion.Y;
                            }
                            else
                            {
                                Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }
                            
                            Target[2] = -targetA;
                            ExecutionController.Instance.WaitIfPaused();
                            StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis, Target, EnumCoordSetType.Absolute);

                            //按芯片吸嘴的系统坐标系移动
                            var PPToolZero = 0f;
                            if (pptool != null)
                            {
                                PPToolZero = pptool.AltimetryOnMark;
                            }
                            else
                            {
                                PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            }
                            ExecutionController.Instance.WaitIfPaused();
                            if (
                            //_positioningSystem.ChipPPMovetoUplookingCameraCenter()
                            result == StageMotionResult.Success
                            &&
                            _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                                {
                                    if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                                    {
                                        _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute);
                                    }
                                }
                            }
                            else
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                        }

                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                        {
                            //var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                            //Thread.Sleep(10000);
                            XYZTCoordinateConfig firstVisionResult = new XYZTCoordinateConfig();
                            if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, (MatchIdentificationParam)visionParam);
                            }
                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, (LineFindIdentificationParam)visionParam);
                            }
                            else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                            {
                                firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, (CircleFindIdentificationParam)visionParam);
                            }
                            //var firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                            if (firstVisionResult == null)
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,First Vision Failed.");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                            ProductExecutor.Instance.OffsetAfterChipAccuracy = firstVisionResult;
                            #region 计算最终的贴装角度
                            BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                            var curSubstrate = _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                            var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                            var curDealBP = CurBondPosition;
                            if (CurBondPosition != null)
                            {
                                curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);

                                var tempCounter = ProductExecutor.Instance.CurModuleNum - 1;
                                while (!curDealBP.IsPositionSuccess)
                                {
                                    tempCounter++;
                                    if (tempCounter >= curSubstrate.Count)
                                    {
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera Failed.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                    curModule = curSubstrate[tempCounter];
                                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                                }
                            }
                            var bondPosOffsetTheta = curDealBP.BondPositionWithPatternOffset.Theta;
                            var bondPosOrigionAngle = curDealBP.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //二次校准时模板初始角度
                            var angleofChipAccuracyPattern = 0f;
                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            }
                            else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                            }
                            //贴装补偿的角度
                            var compensateT = curDealBP.BondPositionCompensation.Theta;
                            //Theta轴移动贴装角度
                            //var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta - angleofChipAccuracyPattern+ bondPosOrigionAngle - curDealBP.PositionBondChipResult.Theta
                            //               + compensateT + bondPosOffsetTheta;
                            var finalAngle = bondPosOrigionAngle - curDealBP.PositionBondChipResult.Theta + compensateT + bondPosOffsetTheta + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta;
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,FinalAngle:{finalAngle}");
                            if (finalAngle > 90 || finalAngle < -90)
                            {
                                finalAngle = 0;
                            }
                            #endregion
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                            //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Relative) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            ExecutionController.Instance.WaitIfPaused();
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                                Thread.Sleep(500);
                                //ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                                ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig();
                                if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, (MatchIdentificationParam)visionParam);
                                }
                                else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, (LineFindIdentificationParam)visionParam);
                                }
                                else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                {
                                    ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, (CircleFindIdentificationParam)visionParam);
                                }
                                ExecutionController.Instance.WaitIfPaused();
                                if (_positioningSystem.BondZMovetoSafeLocation())
                                {
                                    if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                                    {
                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,IdentificationAsyncSecond:{ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta}");

                                        #region 二次识别完成之后，移动到贴装位置上方

                                        //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                                        var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                                        var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;


                                        //贴装补偿的XY,向右向上补偿为正
                                        var compensateX = CurBondPosition.BondPositionCompensation.X;
                                        var compensateY = CurBondPosition.BondPositionCompensation.Y;
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"CurBondPosition.BondPositionCompensation.X: {compensateX}");
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"CurBondPosition.BondPositionCompensation.Y: {compensateY}");
                                        float thetaRadians = -(float)((-bondPosOrigionAngle + curDealBP.PositionBondChipResult.Theta) * Math.PI / 180.0);
                                        //float thetaRadians = (float)((bondPosOrigionAngle) * Math.PI / 180.0);
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"VisionParametersForFindBondPosition_.OrigionAngle: {bondPosOrigionAngle}");
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"PositionBondChipResult.Theta: {curDealBP.PositionBondChipResult.Theta}");
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetAngle: {(-bondPosOrigionAngle + curDealBP.PositionBondChipResult.Theta)}");


                                        //if (thetaRadians > 0)
                                        //{
                                        //    compensateX = Math.Cos(thetaRadians) * compensateX;
                                        //    compensateY = Math.Cos(thetaRadians) * compensateY;
                                        //}
                                        //else
                                        //{
                                        //    compensateX = Math.Cos(-thetaRadians) * compensateX;
                                        //    compensateY = Math.Cos(-thetaRadians) * compensateY;
                                        //}

                                        // 计算旋转后的坐标
                                        double compensateX_1 = compensateX * Math.Cos(thetaRadians) - compensateY * Math.Sin(thetaRadians);
                                        double compensateY_1 = compensateX * Math.Sin(thetaRadians) + compensateY * Math.Cos(thetaRadians);

                                        compensateX = compensateX_1;
                                        compensateY = compensateY_1;

                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetX: {compensateX}");
                                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetY: {compensateY}");

                                        //将记录的贴装位置的系统坐标系转换为Stage坐标系
                                        #region 计算贴装芯片时需要移动到的Stage位置
                                        var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                                        {
                                            X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                                            Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                                        });

                                        #endregion

                                        #region 计算贴装芯片时需要移动到的stage位置

                                        //var curChipStagePosXAfterCorrect = curChipCenterStagePosX - compensateX;
                                        //var curChipStagePosYAfterCorrect = curChipCenterStagePosY + compensateY;
                                        var curChipStagePosXAfterCorrect = curChipCenterStagePosX - compensateX;
                                        var curChipStagePosYAfterCorrect = curChipCenterStagePosY + compensateY;


                                        //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                                        var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                        var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;

                                        var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                                        var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                                        #endregion
                                        ExecutionController.Instance.WaitIfPaused();
                                        if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionWithUplookCamera-End.");
                                            return GlobalGWResultDefine.RET_SUCCESS;
                                        }
                                        else
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionWithUplookCamera,移动到贴装位失败.");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Second Vision Failed.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }
                                else
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Failed.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                        }
                        else
                        {
                            ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                        }
                    }
                }
                else
                {

                }

                

                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }

    /// <summary>
    /// 芯片移动到贴装位置上方
    /// </summary>
    public class StepAction_ChipToBondPosition : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_ChipToBondPosition(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc)
        {
            if (actionNo == EnumActionNo.Action_PositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_PositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if (componentType == EnumComponentType.Component)
                {
                    //CameraWindowGUI.Instance?.SelectCamera(1);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_ChipToBondPosition-Start.");
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    ExecutionController.Instance.WaitIfPaused();
                    if (_positioningSystem.BondZMovetoSafeLocation())
                    {
                        #region 移动到贴装位置上方

                        BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                        var curSubstrate = _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                        var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                        var curDealBP = CurBondPosition;
                        if (CurBondPosition != null)
                        {
                            curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);

                            var tempCounter = ProductExecutor.Instance.CurModuleNum - 1;
                            while (!curDealBP.IsPositionSuccess)
                            {
                                tempCounter++;
                                if (tempCounter >= curSubstrate.Count)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_ChipToBondPosition Failed.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                                curModule = curSubstrate[tempCounter];
                                curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                            }
                        }
                        var bondPosOffsetTheta = curDealBP.BondPositionWithPatternOffset.Theta;
                        var bondPosOrigionAngle = curDealBP.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //二次校准时模板初始角度
                        var angleofChipAccuracyPattern = 0f;
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                        }
                        //贴装补偿的角度
                        var compensateT = curDealBP.BondPositionCompensation.Theta;

                        var materialOrigionA_init = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA_init + compensateT;
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);

                        //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                        var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                        var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;


                        //贴装补偿的XY,向右向上补偿为正
                        var compensateX = CurBondPosition.BondPositionCompensation.X;
                        var compensateY = CurBondPosition.BondPositionCompensation.Y;
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"CurBondPosition.BondPositionCompensation.X: {compensateX}");
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"CurBondPosition.BondPositionCompensation.Y: {compensateY}");
                        float thetaRadians = -(float)((-bondPosOrigionAngle + curDealBP.PositionBondChipResult.Theta) * Math.PI / 180.0);
                        //float thetaRadians = (float)((bondPosOrigionAngle) * Math.PI / 180.0);
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"VisionParametersForFindBondPosition_.OrigionAngle: {bondPosOrigionAngle}");
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"PositionBondChipResult.Theta: {curDealBP.PositionBondChipResult.Theta}");
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetAngle: {(-bondPosOrigionAngle + curDealBP.PositionBondChipResult.Theta)}");


                        //if (thetaRadians > 0)
                        //{
                        //    compensateX = Math.Cos(thetaRadians) * compensateX;
                        //    compensateY = Math.Cos(thetaRadians) * compensateY;
                        //}
                        //else
                        //{
                        //    compensateX = Math.Cos(-thetaRadians) * compensateX;
                        //    compensateY = Math.Cos(-thetaRadians) * compensateY;
                        //}

                        // 计算旋转后的坐标
                        double compensateX_1 = compensateX * Math.Cos(thetaRadians) - compensateY * Math.Sin(thetaRadians);
                        double compensateY_1 = compensateX * Math.Sin(thetaRadians) + compensateY * Math.Cos(thetaRadians);

                        compensateX = compensateX_1;
                        compensateY = compensateY_1;

                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetX: {compensateX}");
                        LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetY: {compensateY}");

                        //将记录的贴装位置的系统坐标系转换为Stage坐标系
                        #region 计算贴装芯片时需要移动到的Stage位置
                        var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                        {
                            X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                            Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                        });

                        #endregion

                        #region 计算贴装芯片时需要移动到的stage位置

                        //var curChipStagePosXAfterCorrect = pptool.PP1AndBondCameraOffset.X - compensateX;
                        //var curChipStagePosYAfterCorrect = pptool.PP1AndBondCameraOffset.Y + compensateY;

                        var curChipStagePosXAfterCorrect = pptool.PP1AndBondCameraOffset.X - compensateX;
                        var curChipStagePosYAfterCorrect = pptool.PP1AndBondCameraOffset.Y + compensateY;

                        //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                        var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect;
                        var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect;

                        var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                        var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                        #endregion
                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, -targetA, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_ChipToBondPosition-End.");
                            return GlobalGWResultDefine.RET_SUCCESS;
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_ChipToBondPosition,移动到贴装位失败.");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                        #endregion

                    }
                    else
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_ChipToBondPosition,Failed.");
                        return GlobalGWResultDefine.RET_FAILED;
                    }

                }
                else
                {

                }



                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }


        /// <summary>
        /// 使用仰视相机二次校准时识别两次，一次补角度，另外一次找中心，没有使用旋转补偿
        /// </summary>
    public class StepAction_AccuracyPositionWithUplookCameraNoBond : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_AccuracyPositionWithUplookCameraNoBond(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc)
        {
            if (actionNo == EnumActionNo.Action_AccuracyPositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_AccuracyPositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if (componentType == EnumComponentType.Component)
                {
                    CameraWindowGUI.Instance?.SelectCamera(1);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionWithUplookCamera-Start.");
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    if (_positioningSystem.BondZMovetoSafeLocation())
                    {
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                        {
                            var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;

                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                            //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                            double X = 0d;
                            double Y = 0d;
                            double Z = 0d;

                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {


                                if (pptool != null)
                                {
                                    X = pptool.LookuptoPPOrigion.X;
                                    Y = pptool.LookuptoPPOrigion.Y;
                                }
                                else
                                {
                                    X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                    Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                                }
                                //Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                                Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZChipSystemWorkPosition;
                            }
                            else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                if (pptool != null)
                                {
                                    X = pptool.LookuptoPPOrigion.X;
                                    Y = pptool.LookuptoPPOrigion.Y;
                                }
                                else
                                {
                                    X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                    Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                                }
                                //Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                                Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZChipSystemWorkPosition;
                            }

                            var materialOrigionA_init = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA_init;
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupChip.Theta}");
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-targetAngle:{targetA}");
                            //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);
                            //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");


                            //XY联动移动到物料上方
                            EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                            multiAxis[0] = EnumStageAxis.BondX;
                            multiAxis[1] = EnumStageAxis.BondY;
                            multiAxis[2] = pptool.StageAxisTheta;
                            //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                            //拾取之后移动到仰视相机上方

                            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);


                            double[] Target = new double[3];
                            Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                            Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            if (pptool != null)
                            {
                                Target[0] = pptool.LookuptoPPOrigion.X;
                                Target[1] = pptool.LookuptoPPOrigion.Y;
                            }
                            else
                            {
                                Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }

                            Target[2] = -targetA;
                            StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis, Target, EnumCoordSetType.Absolute);

                            //按芯片吸嘴的系统坐标系移动
                            var PPToolZero = 0f;
                            if (pptool != null)
                            {
                                PPToolZero = pptool.AltimetryOnMark;
                            }
                            else
                            {
                                PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            }
                            if (
                            //_positioningSystem.ChipPPMovetoUplookingCameraCenter()
                            result == StageMotionResult.Success
                            &&
                            _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                                {
                                    if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                                    {
                                        _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute);
                                    }
                                }
                            }
                            else
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                        }

                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                        {
                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                                ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                                if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationChip-End.");
                                    return GlobalGWResultDefine.RET_SUCCESS;
                                }
                            }
                            else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                                //Thread.Sleep(10000);

                                var firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                                if (firstVisionResult == null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,First Vision Failed.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                                ProductExecutor.Instance.OffsetAfterChipAccuracy = firstVisionResult;
                                #region 计算最终的贴装角度

                                if (CurBondPosition != null)
                                {
                                    
                                }
                                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                //二次校准时模板初始角度
                                var angleofChipAccuracyPattern = 0f;
                                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                }
                                else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                                }
                                //贴装补偿的角度
                                var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                                //Theta轴移动贴装角度
                                //var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta - angleofChipAccuracyPattern+ bondPosOrigionAngle - curDealBP.PositionBondChipResult.Theta
                                //               + compensateT + bondPosOffsetTheta;
                                var finalAngle = bondPosOrigionAngle - CurBondPosition.PositionBondChipResult.Theta + compensateT + bondPosOffsetTheta;
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,FinalAngle:{finalAngle}");
                                #endregion
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                                //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Relative) == StageMotionResult.Success
                                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                {
                                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                                    Thread.Sleep(500);
                                    ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);

                                    if (_positioningSystem.BondZMovetoSafeLocation())
                                    {
                                        if (ProductExecutor.Instance.OffsetAfterChipAccuracy != null)
                                        {
                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,IdentificationAsyncSecond:{ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta}");

                                            return GlobalGWResultDefine.RET_SUCCESS;
                                        }
                                    }
                                    else
                                    {
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Failed.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                        }
                    }
                }
                else
                {
                    CameraWindowGUI.Instance?.SelectCamera(1);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionWithUplookCamera-Start.");
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                    if (_positioningSystem.BondZMovetoSafeLocation())
                    {
                        if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                        {
                            var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;

                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                            //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                            double X = 0d;
                            double Y = 0d;
                            double Z = 0d;

                            if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {


                                if (pptool != null)
                                {
                                    X = pptool.LookuptoPPOrigion.X;
                                    Y = pptool.LookuptoPPOrigion.Y;
                                }
                                else
                                {
                                    X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                    Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                                }
                                //Z = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                                Z = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZChipSystemWorkPosition;
                            }
                            else if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                if (pptool != null)
                                {
                                    X = pptool.LookuptoPPOrigion.X;
                                    Y = pptool.LookuptoPPOrigion.Y;
                                }
                                else
                                {
                                    X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                    Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                                }
                                //Z = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                                Z = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZChipSystemWorkPosition;
                            }

                            var materialOrigionA_init = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA_init;
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpSubmonutWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta}");
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpSubmonutWithRotate-targetAngle:{targetA}");
                            //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT)}");
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -targetA, EnumCoordSetType.Relative);
                            //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT)}");


                            //XY联动移动到物料上方
                            EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                            multiAxis[0] = EnumStageAxis.BondX;
                            multiAxis[1] = EnumStageAxis.BondY;
                            multiAxis[2] = pptool.StageAxisTheta;
                            //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                            //拾取之后移动到仰视相机上方

                            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);


                            double[] Target = new double[3];
                            Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                            Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            if (pptool != null)
                            {
                                Target[0] = pptool.LookuptoPPOrigion.X;
                                Target[1] = pptool.LookuptoPPOrigion.Y;
                            }
                            else
                            {
                                Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                                Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                            }

                            Target[2] = -targetA;
                            StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis, Target, EnumCoordSetType.Absolute);

                            //按芯片吸嘴的系统坐标系移动
                            var PPToolZero = 0f;
                            if (pptool != null)
                            {
                                PPToolZero = pptool.AltimetryOnMark;
                            }
                            else
                            {
                                PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            }
                            if (
                            //_positioningSystem.SubmonutPPMovetoUplookingCameraCenter()
                            result == StageMotionResult.Success
                            &&
                            _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                                {
                                    if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                                    {
                                        _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute);
                                    }
                                }
                            }
                            else
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                        }

                        if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                        {
                            if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                var visionParam = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                                ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                                if (ProductExecutor.Instance.OffsetAfterSubmonutAccuracy != null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyCalibrationSubmonut-End.");
                                    return GlobalGWResultDefine.RET_SUCCESS;
                                }
                            }
                            else if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                var visionParam = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                                //Thread.Sleep(10000);

                                var firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);
                                if (firstVisionResult == null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,First Vision Failed.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                                ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = firstVisionResult;
                                #region 计算最终的贴装角度

                                if (CurBondPosition != null)
                                {

                                }
                                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                //二次校准时模板初始角度
                                var angleofSubmonutAccuracyPattern = 0f;
                                if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    angleofSubmonutAccuracyPattern = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                }
                                else if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    angleofSubmonutAccuracyPattern = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                                }
                                //贴装补偿的角度
                                var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                                //Theta轴移动贴装角度
                                //var finalAngle = ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.Theta - angleofSubmonutAccuracyPattern+ bondPosOrigionAngle - curDealBP.PositionBondSubmonutResult.Theta
                                //               + compensateT + bondPosOffsetTheta;
                                var finalAngle = bondPosOrigionAngle - CurBondPosition.PositionBondChipResult.Theta + compensateT + bondPosOffsetTheta;
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,FinalAngle:{finalAngle}");
                                #endregion
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                                //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -finalAngle + 50, EnumCoordSetType.Relative) == StageMotionResult.Success
                                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.Theta, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                {
                                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                                    Thread.Sleep(500);
                                    ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, visionParam);

                                    if (_positioningSystem.BondZMovetoSafeLocation())
                                    {
                                        if (ProductExecutor.Instance.OffsetAfterSubmonutAccuracy != null)
                                        {
                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionWithUplookCamera,IdentificationAsyncSecond:{ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.Theta}");

                                            return GlobalGWResultDefine.RET_SUCCESS;
                                        }
                                    }
                                    else
                                    {
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Failed.");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                        }
                    }


                }



                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionWithUplookCamera,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
    }



    /// <summary>
    /// 芯片吸嘴抛料
    /// </summary>
    public class StepAction_MaterialThrowingAction : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_MaterialThrowingAction(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        {
            if (actionNo == EnumActionNo.Action_AbondonChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_AbondonSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if(componentType == EnumComponentType.Component)
                {
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    return AbandonMaterialUtility.Instance.Abandon(pptool);
                }
                else
                {
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                    return AbandonMaterialUtility.Instance.Abandon(pptool);
                }


                //return AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.ChipPP);
            }
            catch (Exception)
            {

                //throw;
            }

            return GlobalGWResultDefine.RET_SUCCESS;
        }
    }

    /// <summary>
    /// 中转台芯片吸嘴抛料
    /// </summary>
    public class StepAction_CalibrationTableMaterialThrowingAction : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_CalibrationTableMaterialThrowingAction(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        {
            if (actionNo == EnumActionNo.Action_AbondonChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_AbondonSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                var ppParam = CurChipParam.PPSettings;

                if (componentType == EnumComponentType.Component)
                {
                    pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    ppParam = CurChipParam.PPSettings;
                }
                else
                {
                    pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                    ppParam = CurSubmonutParam.PPSettings;
                }

                

                if (pptool != null)
                {
                    var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                    if (componentType == EnumComponentType.Component)
                    {
                        systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                    }
                    else
                    {
                        systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurSubmonutParam.ThicknessMM;
                    }
                        
                    ppParam.PPToolZero = pptool.AltimetryOnMark;
                    ppParam.WorkHeight = (float)systemPos;
                }
                else
                {
                    var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                    if (componentType == EnumComponentType.Component)
                    {
                        systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                    }
                    else
                    {
                        systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurSubmonutParam.ThicknessMM;
                    }
                    ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                    ppParam.WorkHeight = (float)systemPos;
                }
                ExecutionController.Instance.WaitIfPaused();
                if (_positioningSystem.BondZMovetoSafeLocation() && _positioningSystem.PPtoolMovetoCalibrationTableCenter(pptool))
                {
                    ExecutionController.Instance.WaitIfPaused();
                    //拾取芯片
                    if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromCalibrationTable))
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);  


                        //return AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.ChipPP);
                        return AbandonMaterialUtility.Instance.Abandon(pptool);
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }

                
            }
            catch (Exception)
            {

                //throw;
            }

            return GlobalGWResultDefine.RET_SUCCESS;
        }

        public void AfterPlaceChipOnCalibrationTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromCalibrationTable()
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }

    /// <summary>
    /// 共晶台芯片吸嘴抛料
    /// </summary>
    public class StepAction_EutecticTableMaterialThrowingAction : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_EutecticTableMaterialThrowingAction(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc)
        {
            if (actionNo == EnumActionNo.Action_AbondonChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_AbondonSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                var ppParam = CurChipParam.PPSettings;

                if (componentType == EnumComponentType.Component)
                {
                    pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    ppParam = CurChipParam.PPSettings;
                }
                else
                {
                    pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                    ppParam = CurSubmonutParam.PPSettings;
                }



                if (pptool != null)
                {
                    var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurChipParam.ThicknessMM;
                    if (componentType == EnumComponentType.Component)
                    {
                        systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurChipParam.ThicknessMM;
                    }
                    else
                    {
                        systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurSubmonutParam.ThicknessMM;
                    }

                    ppParam.PPToolZero = pptool.AltimetryOnMark;
                    ppParam.WorkHeight = (float)systemPos;
                }
                else
                {
                    var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurChipParam.ThicknessMM;
                    if (componentType == EnumComponentType.Component)
                    {
                        systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurChipParam.ThicknessMM;
                    }
                    else
                    {
                        systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurSubmonutParam.ThicknessMM;
                    }
                    ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                    ppParam.WorkHeight = (float)systemPos;
                }
                if (_positioningSystem.BondZMovetoSafeLocation() && _positioningSystem.PPtoolMovetoEutecticTableCenter(pptool))
                {
                    //拾取芯片
                    if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromEutecticTable))
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);  


                        //return AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.ChipPP);
                        return AbandonMaterialUtility.Instance.Abandon(pptool);
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }


            }
            catch (Exception)
            {

                //throw;
            }

            return GlobalGWResultDefine.RET_SUCCESS;
        }

        public void AfterPlaceChipOnEutecticTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromEutecticTable()
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }



    public class StepAction_AccuracyPositionChipInCalibrationTable : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_AccuracyPositionChipInCalibrationTable(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        {
            if (actionNo == EnumActionNo.Action_PositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_PositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                //CameraWindowGUI.Instance?.SelectCamera(0);
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionChipInCalibrationTable-Start.");

                VisionIdentificationParam visionParam = new VisionIdentificationParam();
                int ParametersCount = 0;
                if (CurChipParam.AccuracyComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters[0];
                    ParametersCount = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.Count;
                }
                else if (CurChipParam.AccuracyComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams[0];
                    ParametersCount = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.Count;
                }
                else if (CurChipParam.AccuracyComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                {
                    visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.CircleSearchParameters[0];
                    ParametersCount = CurChipParam.AccuracyComponentPositionVisionParameters.CircleSearchParameters.Count;
                }

                var pptool2 = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);

                var materialOrigionA_init = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA_init;
                ExecutionController.Instance.WaitIfPaused();
                if (_positioningSystem.BondZMovetoSafeLocation()
                    && _positioningSystem.PPtoolMovetoCalibrationTableCenter(pptool2)
                    //&& _positioningSystem.ChipPPMovetoCalibrationTableCenter(_systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName))
                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative) == StageMotionResult.Success)
                {
                    //使用的吸嘴工具
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    //转动吸嘴直接到贴装角度
                    #region 计算最终的贴装角度
                    BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                    var curSubstrate = _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                    var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                    var curDealBP = CurBondPosition;
                    if (CurBondPosition != null)
                    {
                        curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);

                        var tempCounter = ProductExecutor.Instance.CurModuleNum - 1;
                        while (!curDealBP.IsPositionSuccess)
                        {
                            tempCounter++;
                            if (tempCounter >= curSubstrate.Count)
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipInCalibrationTable Failed.");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            curModule = curSubstrate[tempCounter];
                            curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                        }
                    }
                    var bondPosOffsetTheta = curDealBP.BondPositionWithPatternOffset.Theta;
                    var bondPosOrigionAngle = curDealBP.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //二次校准时模板初始角度
                    var angleofChipAccuracyPattern = 0f;
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    {
                        angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    }
                    else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    {
                        angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                    }
                    //贴装补偿的角度
                    var compensateT = curDealBP.BondPositionCompensation.Theta;
                    //Theta轴移动此角度后将chip转正
                    var finalAngle = - curDealBP.PositionBondChipResult.Theta + bondPosOrigionAngle + compensateT + bondPosOffsetTheta;
                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,FinalAngle:{finalAngle}");
                    #endregion
                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                    ExecutionController.Instance.WaitIfPaused();
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    {
                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                        //放芯片
                        var ppParam = CurChipParam.PPSettings;

                        if (pptool != null)
                        {
                            var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                            ppParam.PPToolZero = pptool.AltimetryOnMark;
                            ppParam.WorkHeight = (float)systemPos;
                        }
                        else
                        {
                            var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                            ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            ppParam.WorkHeight = (float)systemPos;
                        }
                        ExecutionController.Instance.WaitIfPaused();
                        if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnCalibrationTable, true))
                        {
                            _positioningSystem.PPMovetoSafeLocation();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "放置芯片到校准台失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                        var workZ = 0f;
                        ////移动榜头相机到校准台上方
                        //if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        //{
                        //    workZ = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                        //}
                        //else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        //{
                        //    workZ = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                        //}
                        workZ = visionParam.CameraZWorkPosition;
                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondCameraMovetoCalibrationTableCenter()
                            && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, workZ, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {

                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                            {
                                //var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                                //Thread.Sleep(10000);
                                XYZTCoordinateConfig firstVisionResult = new XYZTCoordinateConfig();
                                if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                {
                                    firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                }
                                else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                {
                                    firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                }
                                else if (CurChipParam.PositionComponentVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                {
                                    firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                }
                                //var firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                if (firstVisionResult == null)
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipInCalibrationTable,Vision Failed.");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                                else
                                {
                                    LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                                    ProductExecutor.Instance.OffsetAfterChipAccuracy = firstVisionResult;
                                    ExecutionController.Instance.WaitIfPaused();
                                    if (_positioningSystem.BondZMovetoSafeLocation()
                                        && _positioningSystem.PPtoolMovetoCalibrationTableCenter(pptool2)
                                        //&& _positioningSystem.ChipPPMovetoCalibrationTableCenter(_systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName))
                                        )
                                    {
                                        ExecutionController.Instance.WaitIfPaused();
                                        //拾取芯片
                                        if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromCalibrationTable))
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                                            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                                            //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);  

                                            //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                                            var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) - ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                                            var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) - ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;


                                            //贴装补偿的XY,向右向上补偿为正
                                            var compensateX = CurBondPosition.BondPositionCompensation.X;
                                            var compensateY = CurBondPosition.BondPositionCompensation.Y;
                                            //float thetaRadians = (float)((bondPosOrigionAngle - curDealBP.PositionBondChipResult.Theta) * Math.PI / 180.0);
                                            //if (thetaRadians > 0)
                                            //{
                                            //    compensateX = Math.Cos(thetaRadians) * compensateX;
                                            //    compensateY = Math.Cos(thetaRadians) * compensateY;
                                            //}
                                            //else
                                            //{
                                            //    compensateX = Math.Cos(-thetaRadians) * compensateX;
                                            //    compensateY = Math.Cos(-thetaRadians) * compensateY;
                                            //}


                                            LogRecorder.ProductionLog(EnumLogContentType.Info, $"CurBondPosition.BondPositionCompensation.X: {compensateX}");
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, $"CurBondPosition.BondPositionCompensation.Y: {compensateY}");
                                            float thetaRadians = -(float)((-bondPosOrigionAngle + curDealBP.PositionBondChipResult.Theta) * Math.PI / 180.0);
                                            //float thetaRadians = (float)((bondPosOrigionAngle) * Math.PI / 180.0);
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, $"VisionParametersForFindBondPosition_.OrigionAngle: {bondPosOrigionAngle}");
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, $"PositionBondChipResult.Theta: {curDealBP.PositionBondChipResult.Theta}");
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, $"offsetAngle: {(-bondPosOrigionAngle + curDealBP.PositionBondChipResult.Theta)}");


                                            //if (thetaRadians > 0)
                                            //{
                                            //    compensateX = Math.Cos(thetaRadians) * compensateX;
                                            //    compensateY = Math.Cos(thetaRadians) * compensateY;
                                            //}
                                            //else
                                            //{
                                            //    compensateX = Math.Cos(-thetaRadians) * compensateX;
                                            //    compensateY = Math.Cos(-thetaRadians) * compensateY;
                                            //}

                                            // 计算旋转后的坐标
                                            double compensateX_1 = compensateX * Math.Cos(thetaRadians) - compensateY * Math.Sin(thetaRadians);
                                            double compensateY_1 = compensateX * Math.Sin(thetaRadians) + compensateY * Math.Cos(thetaRadians);

                                            compensateX = compensateX_1;
                                            compensateY = compensateY_1;


                                            //将记录的贴装位置的系统坐标系转换为Stage坐标系
                                            #region 计算贴装芯片时需要移动到的Stage位置
                                            var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                                            {
                                                X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                                                Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                                            });

                                            #endregion

                                            #region 计算贴装芯片时需要移动到的stage位置

                                            //var curChipStagePosXAfterCorrect = curChipCenterStagePosX - compensateX;
                                            //var curChipStagePosYAfterCorrect = curChipCenterStagePosY + compensateY;
                                            var curChipStagePosXAfterCorrect = curChipCenterStagePosX - compensateX;
                                            var curChipStagePosYAfterCorrect = curChipCenterStagePosY + compensateY;


                                            //计算贴装补偿之后的芯片和榜头相机的偏移
                                            var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.CalibrationTableOrigion.X;
                                            var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.CalibrationTableOrigion.Y;




                                            var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                                            var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                                            #endregion
                                            //X = baseX;
                                            //Y = baseY;
                                            ExecutionController.Instance.WaitIfPaused();
                                            //if (_positioningSystem.BondXYUnionMovetoStageCoor(finalX, finalY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                            if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) != StageMotionResult.Success)
                                            {
                                                _positioningSystem.PPMovetoSafeLocation();
                                                LogRecorder.ProductionLog(EnumLogContentType.Error, "移动到贴装位失败！");
                                                return GlobalGWResultDefine.RET_FAILED;
                                            }
                                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionChipInCalibrationTable-End.");
                                            return GlobalGWResultDefine.RET_SUCCESS;
                                        }
                                        else
                                        {
                                            _positioningSystem.PPMovetoSafeLocation();
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                            }
                        }
                    }


                    
                }

                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipInCalibrationTable,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
        public void AfterPlaceChipOnCalibrationTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromCalibrationTable()
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }

    public class StepAction_AccuracyPositionChipInCalibrationTableNoBond : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_AccuracyPositionChipInCalibrationTableNoBond(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc)
        {
            if (actionNo == EnumActionNo.Action_AccuracyPositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_AccuracyPositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if(componentType == EnumComponentType.Component)
                {
                    CameraWindowGUI.Instance?.SelectCamera(0);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionChipInCalibrationTable-Start.");
                    var materialOrigionA_init = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA_init;
                    var PPtool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    if (_positioningSystem.BondZMovetoSafeLocation()
                        && _positioningSystem.PPtoolMovetoCalibrationTableCenter(PPtool)
                        && _positioningSystem.MoveAixsToStageCoord(PPtool.StageAxisTheta, -targetA, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    {
                        //使用的吸嘴工具
                        var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                        //转动吸嘴直接到贴装角度
                        #region 计算最终的贴装角度
                        var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                        var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //二次校准时模板初始角度
                        var angleofChipAccuracyPattern = 0f;
                        if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        }
                        else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                        }
                        //贴装补偿的角度
                        var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                        //Theta轴移动此角度后将chip转正
                        var finalAngle = -CurBondPosition.PositionBondChipResult.Theta + bondPosOrigionAngle + compensateT + bondPosOffsetTheta;
                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,FinalAngle:{finalAngle}");
                        #endregion
                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                        if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                            //放芯片
                            var ppParam = CurChipParam.PPSettings;

                            if (pptool != null)
                            {
                                var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                                ppParam.PPToolZero = pptool.AltimetryOnMark;
                                ppParam.WorkHeight = (float)systemPos;
                            }
                            else
                            {
                                var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurChipParam.ThicknessMM;
                                ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                ppParam.WorkHeight = (float)systemPos;
                            }

                            if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnCalibrationTable, true))
                            {
                                _positioningSystem.PPMovetoSafeLocation();
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "放置芯片到校准台失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            var workZ = 0f;
                            //移动榜头相机到校准台上方
                            if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                workZ = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                            }
                            else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                workZ = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                            }
                            if (_positioningSystem.BondCameraMovetoCalibrationTableCenter()
                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, workZ, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {

                                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                                {
                                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                    {
                                        var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                                        ProductExecutor.Instance.OffsetAfterChipAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                        if (ProductExecutor.Instance.OffsetAfterChipAccuracy == null)
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipInCalibrationTable,Vision Failed.");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                        else
                                        {
                                            //拾取芯片
                                            if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromCalibrationTable))
                                            {
                                                _positioningSystem.PPMovetoSafeLocation();
                                                return GlobalGWResultDefine.RET_SUCCESS;
                                            }
                                            else
                                            {
                                                _positioningSystem.PPMovetoSafeLocation();
                                                LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                                                return GlobalGWResultDefine.RET_FAILED;
                                            }
                                        }
                                    }
                                    else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                    {

                                        var visionParam = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                                        //Thread.Sleep(10000);

                                        var firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                        if (firstVisionResult == null)
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipInCalibrationTable,Vision Failed.");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                        else
                                        {
                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionChipInCalibrationTable,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                                            ProductExecutor.Instance.OffsetAfterChipAccuracy = firstVisionResult;
                                            if (_positioningSystem.BondZMovetoSafeLocation()
                                                && _positioningSystem.PPtoolMovetoCalibrationTableCenter(PPtool))
                                            {
                                                //拾取芯片
                                                if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromCalibrationTable))
                                                {
                                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");

                                                    _positioningSystem.PPMovetoSafeLocation();
                                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionChipInCalibrationTable-End.");
                                                    return GlobalGWResultDefine.RET_SUCCESS;
                                                }
                                                else
                                                {
                                                    _positioningSystem.PPMovetoSafeLocation();
                                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                                                    return GlobalGWResultDefine.RET_FAILED;
                                                }
                                            }

                                        }


                                    }
                                }
                                else
                                {
                                    ProductExecutor.Instance.OffsetAfterChipAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                                }
                            }
                        }



                    }

                }
                else
                {
                    CameraWindowGUI.Instance?.SelectCamera(0);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionSubmonutInCalibrationTable-Start.");
                    var materialOrigionA_init = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA_init;
                    var PPtool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                    if (_positioningSystem.BondZMovetoSafeLocation()
                        && _positioningSystem.PPtoolMovetoCalibrationTableCenter(PPtool)
                        && _positioningSystem.MoveAixsToStageCoord(PPtool.StageAxisTheta, -targetA, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    {
                        //使用的吸嘴工具
                        var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                        //转动吸嘴直接到贴装角度
                        #region 计算最终的贴装角度
                        var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                        var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //二次校准时模板初始角度
                        var angleofSubmonutAccuracyPattern = 0f;
                        if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                        {
                            angleofSubmonutAccuracyPattern = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        }
                        else if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                        {
                            angleofSubmonutAccuracyPattern = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                        }
                        //贴装补偿的角度
                        var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                        //Theta轴移动此角度后将chip转正
                        var finalAngle = -CurBondPosition.PositionBondChipResult.Theta + bondPosOrigionAngle + compensateT + bondPosOffsetTheta;
                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionSubmonutInCalibrationTable,FinalAngle:{finalAngle}");
                        #endregion
                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionSubmonutInCalibrationTable,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                        if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -finalAngle, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionSubmonutInCalibrationTable,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(pptool.StageAxisTheta)}");
                            //放芯片
                            var ppParam = CurSubmonutParam.PPSettings;

                            if (pptool != null)
                            {
                                var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurSubmonutParam.ThicknessMM;
                                ppParam.PPToolZero = pptool.AltimetryOnMark;
                                ppParam.WorkHeight = (float)systemPos;
                            }
                            else
                            {
                                var systemPos = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z + CurSubmonutParam.ThicknessMM;
                                ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                ppParam.WorkHeight = (float)systemPos;
                            }

                            if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnCalibrationTable, true))
                            {
                                _positioningSystem.PPMovetoSafeLocation();
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "放置芯片到校准台失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                            var workZ = 0f;
                            //移动榜头相机到校准台上方
                            if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                            {
                                workZ = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                            }
                            else if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                            {
                                workZ = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                            }
                            if (_positioningSystem.BondCameraMovetoCalibrationTableCenter()
                                && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, workZ, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {

                                if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                                {
                                    if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                    {
                                        var visionParam = CurSubmonutParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();

                                        ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                        if (ProductExecutor.Instance.OffsetAfterSubmonutAccuracy == null)
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionSubmonutInCalibrationTable,Vision Failed.");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                        else
                                        {
                                            //拾取芯片
                                            if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromCalibrationTable))
                                            {
                                                _positioningSystem.PPMovetoSafeLocation();
                                                return GlobalGWResultDefine.RET_SUCCESS;
                                            }
                                            else
                                            {
                                                _positioningSystem.PPMovetoSafeLocation();
                                                LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                                                return GlobalGWResultDefine.RET_FAILED;
                                            }
                                        }
                                    }
                                    else if (CurSubmonutParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                    {

                                        var visionParam = CurSubmonutParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                                        //Thread.Sleep(10000);

                                        var firstVisionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                        if (firstVisionResult == null)
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionSubmonutInCalibrationTable,Vision Failed.");
                                            return GlobalGWResultDefine.RET_FAILED;
                                        }
                                        else
                                        {
                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_AccuracyPositionSubmonutInCalibrationTable,IdentificationAsyncFirst:{firstVisionResult.Theta}");
                                            ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = firstVisionResult;
                                            if (_positioningSystem.BondZMovetoSafeLocation()
                                                && _positioningSystem.PPtoolMovetoCalibrationTableCenter(PPtool))
                                            {
                                                //拾取芯片
                                                if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromCalibrationTable))
                                                {
                                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondSubmonutOpt-Start.");

                                                    _positioningSystem.PPMovetoSafeLocation();
                                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionSubmonutInCalibrationTable-End.");
                                                    return GlobalGWResultDefine.RET_SUCCESS;
                                                }
                                                else
                                                {
                                                    _positioningSystem.PPMovetoSafeLocation();
                                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "从校准台拾取芯片失败！");
                                                    return GlobalGWResultDefine.RET_FAILED;
                                                }
                                            }

                                        }


                                    }
                                }
                                else
                                {
                                    ProductExecutor.Instance.OffsetAfterSubmonutAccuracy = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                                }
                            }
                        }



                    }

                }


                return GlobalGWResultDefine.RET_FAILED;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_AccuracyPositionChipInCalibrationTable,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
        }
        public void AfterPlaceChipOnCalibrationTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromCalibrationTable()
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }

    /*
     * 物料Step 5 - 放置基底 步骤Action
     */
    class StepAction_PutDownSubstrate : StepActionBase
    {
        public StepAction_PutDownSubstrate(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {               
                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PutDownSubstrate,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
        /// <summary>
        /// 放衬底时开启共晶台吸气
        /// </summary>
        private void PlaceSubmountAction()
        {
            IOUtilityHelper.Instance.OpenEutecticPlatformPPVaccum();
        }
    }

    /*
    * 相机移动至芯片上方 步骤Action
    */
    class StepAction_CamMoveToComponentPos : StepActionBase
    {
        public StepAction_CamMoveToComponentPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMoveToChipPos-Start.");
                //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----
                //double X = CurChipParam.FirstComponentLocation.X;
                //double Y = CurChipParam.FirstComponentLocation.Y;
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();

                double X = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.X;
                double Y = CurChipParam.ComponentMapInfos[ProductExecutor.Instance.CurChipNum - 1].MaterialLocation.Y;
                double Z = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                //-----通过物料名取物料对象，取物料首位置 X Y Z    end -----
                if (CurChipParam.CarrierType == EnumCarrierType.WafflePack)
                {
                    //移动bond头
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);

                    //XY联动移动到物料上方
                    //EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                    //multiAxis[0] = EnumStageAxis.BondX;
                    //multiAxis[1] = EnumStageAxis.BondY;
                    //double[] targets = new double[2];
                    //targets[0] = X;
                    //targets[1] = Y;
                    //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                    _positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);


                }
                else if(CurChipParam.CarrierType == EnumCarrierType.Wafer)
                {
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute);
                    //移动bond头
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, X, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, Y, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z, EnumCoordSetType.Absolute);
                }
                else if (CurChipParam.CarrierType == EnumCarrierType.WaferWafflePack)
                {
                    //移动bond头
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, X, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, Y, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableZ, Z, EnumCoordSetType.Absolute);
                }
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMoveToChipPos-End.");
                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_CamMoveToChipPos,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }

    
    /*
   * 吸取芯片 步骤Action
   */
    public class StepAction_PickUpChipWithRotate : StepActionBase
    {
        public EnumComponentType componentType { get; set; } = EnumComponentType.Component;
        public StepAction_PickUpChipWithRotate(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) 
        {
            if (actionNo == EnumActionNo.Action_PositionChip)
            {
                componentType = EnumComponentType.Component;
            }
            else if (actionNo == EnumActionNo.Action_PositionSubmonut)
            {
                componentType = EnumComponentType.Submonut;
            }
        }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                if(componentType == EnumComponentType.Component)
                {
                    BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                    var curDealBP = CurBondPosition;
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    if(DataModel.Instance.CurPPtoolName == null || DataModel.Instance.CurPPtoolName == "")
                    {
                        int Done = 0;
                        Done = WarningBox.FormShow("错误", $"检测不到吸嘴{pptool.Name}，是否需要加载吸嘴！");
                        if (Done == 1)
                        {
                            if (!PPUtility.Instance.LoadPPTool(pptool.Name))
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, $"加载吸嘴{pptool.Name}失败！");
                                WarningBox.FormShow("错误", $"加载吸嘴{pptool.Name}失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            Done = WarningBox.FormShow("警告", $"吸嘴{pptool.Name}是否已经在帮头上！");
                            if (Done == 1)
                            {
                                DataModel.Instance.CurPPtoolName = pptool.Name;
                            }
                            else
                            {
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        
                    }
                    else if (DataModel.Instance.CurPPtoolName != pptool.Name)
                    {
                        if (!PPUtility.Instance.UnloadPPTool(DataModel.Instance.CurPPtoolName))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"卸载吸嘴{DataModel.Instance.CurPPtoolName}失败！");
                            WarningBox.FormShow("错误", $"卸载吸嘴{DataModel.Instance.CurPPtoolName}失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                        if (!PPUtility.Instance.LoadPPTool(pptool.Name))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"加载吸嘴{pptool.Name}失败！");
                            WarningBox.FormShow("错误", $"加载吸嘴{pptool.Name}失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (pptool.Name == DataModel.Instance.CurPPtoolName)
                    {

                    }

                    if (CurChipParam.CarrierType == EnumCarrierType.WafflePack)
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpChip-Start.");
                        //BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                        var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                        //安全位
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                        //吸嘴移动到芯片中心上方

                        var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        offset = pptool.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            offset.X = usedPPandBondCameraOffsetX;
                            offset.Y = usedPPandBondCameraOffsetY;
                        }
                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //芯片吸嘴T复位
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.BondXYUnionMovetoStageCoor(ProductExecutor.Instance.OffsetBeforePickupChip.X + offset.X + curDealBP.chipPositionCompensation.X
                            , ProductExecutor.Instance.OffsetBeforePickupChip.Y + offset.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {
                           
                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            var pp = CurChipParam.PPSettings;


                            if (pptool != null)
                            {
                                var systemPos = CurChipParam.ChipPPPickSystemPos;
                                pp.PPToolZero = pptool.AltimetryOnMark;
                                pp.WorkHeight = (float)(systemPos + curDealBP.chipPositionCompensation.Z);
                            }
                            else
                            {
                                var systemPos = CurChipParam.ChipPPPickSystemPos;
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                pp.WorkHeight = (float)(systemPos + curDealBP.chipPositionCompensation.Z);
                            }

                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);

                                //_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -targetA, EnumCoordSetType.Relative);



                                //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                                //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                                //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                                //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                                //double angle0 = CurrA;
                                //double angle = CurrA+ targetA;
                                //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                                //ProductExecutor.Instance.CompensateXAfterPickupChip = point3.X;
                                //ProductExecutor.Instance.CompensateYAfterPickupChip = point3.Y;
                                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpChip-End.");
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (CurChipParam.CarrierType == EnumCarrierType.Wafer)
                    {
                        var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == CurChipParam.RelatedESToolName);
                        if (usedESTool != null)
                        {
                            var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                            offset = pptool.PP1AndBondCameraOffset;
                            if (pptool != null)
                            {
                                var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                                offset.X = usedPPandBondCameraOffsetX;
                                offset.Y = usedPPandBondCameraOffsetY;
                            }
                            var offsetBCAndWC = _systemConfig.PositioningConfig.WaferCameraOrigion;
                            var offsetBCAndWC2 = usedESTool.BondIdentifyNeedleCenter;
                            ExecutionController.Instance.WaitIfPaused();
                            if (_positioningSystem.BondZMovetoSafeLocation()
                            //顶针移动到零点
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //物料中心移动到顶针上方
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                            //芯片吸嘴物料中心上方
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC.X - usedESTool.NeedleCenter.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC.Y - usedESTool.NeedleCenter.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC2.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC2.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //顶针座升起
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurChipParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //拾取芯片
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            )
                            {
                                //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                                var pp = CurChipParam.PPSettings;
                                pp.WorkHeight = CurChipParam.ChipPPPickSystemPos + (float)curDealBP.chipPositionCompensation.Z; ;
                                //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                                if (pptool != null)
                                {
                                    pp.PPToolZero = pptool.AltimetryOnMark;
                                }
                                else
                                {
                                    //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                    //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                    //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                    //var ppWorkSystemPos = 0f;
                                    pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                    //pp.WorkHeight = ppWorkSystemPos;
                                }

                                IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();
                                //Thread.Sleep(3000);
                                var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                                ExecutionController.Instance.WaitIfPaused();
                                if (PPUtility.Instance.PickViaSystemCoor(pp))
                                {
                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);
                                    //_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -targetA, EnumCoordSetType.Relative);
                                }
                                else
                                {
                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                    WarningBox.FormShow("错误", "拾取芯片失败！");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                            else
                            {
                                IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (CurChipParam.CarrierType == EnumCarrierType.WaferWafflePack)
                    {
                        //芯片吸嘴物料中心上方
                        var ppSystemOffset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        ppSystemOffset = pptool.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            ppSystemOffset.X = usedPPandBondCameraOffsetX;
                            ppSystemOffset.Y = usedPPandBondCameraOffsetY;
                        }
                        var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;


                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //芯片吸嘴物料中心上方
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X+ ProductExecutor.Instance.OffsetBeforePickupChip.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y- ProductExecutor.Instance.OffsetBeforePickupChip.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        ////拾取芯片
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        )
                        {
                            //XY联动移动到物料上方
                            EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                            multiAxis[0] = EnumStageAxis.BondX;
                            multiAxis[1] = EnumStageAxis.BondY;
                            //multiAxis[2] = EnumStageAxis.ChipPPT;
                            multiAxis[2] = pptool.StageAxisTheta;
                            double[] targets = new double[3];
                            targets[0] = ppSystemOffset.X + bondcamera2wafercamera.X + curDealBP.chipPositionCompensation.X;
                            targets[1] = ppSystemOffset.Y + bondcamera2wafercamera.Y + curDealBP.chipPositionCompensation.Y;
                            targets[2] = 0;
                            ExecutionController.Instance.WaitIfPaused();
                            StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                            if (result == StageMotionResult.Success)
                            {

                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,BondXTarget:{ppSystemOffset.X + bondcamera2wafercamera.X}");
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,BondXCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX)}");
                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            var pp = CurChipParam.PPSettings;
                            pp.WorkHeight = CurChipParam.ChipPPPickSystemPos + (float)curDealBP.chipPositionCompensation.Z;

                            //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                            if (pptool != null)
                            {
                                //吸嘴工具原点
                                pp.PPToolZero = pptool.AltimetryOnMark;

                            }
                            else
                            {
                                //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                //var ppWorkSystemPos = 0f;
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                //pp.WorkHeight = ppWorkSystemPos;
                            }


                            //var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                            //LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupChip.Theta}");
                            //LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-targetAngle:{targetA}");
                            ExecutionController.Instance.WaitIfPaused();
                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);
                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }

                }
                else
                {
                    ProductExecutor.Instance.CurrectPickupSubmonut = new XYZTCoordinateConfig();

                    BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                    var curDealBP = CurBondPosition;
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                    if (CurSubmonutParam.CarrierType == EnumCarrierType.WafflePack)
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpSubmonut-Start.");
                        //BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                        var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;
                        //安全位
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                        //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                        //吸嘴移动到芯片中心上方

                        var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        offset = pptool.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            offset.X = usedPPandBondCameraOffsetX;
                            offset.Y = usedPPandBondCameraOffsetY;
                        }


                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //芯片吸嘴T复位
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.BondXYUnionMovetoStageCoor(ProductExecutor.Instance.OffsetBeforePickupSubmonut.X + offset.X
                            , ProductExecutor.Instance.OffsetBeforePickupSubmonut.Y + offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {
                            
                            ProductExecutor.Instance.CurrectPickupSubmonut.X = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                            ProductExecutor.Instance.CurrectPickupSubmonut.Y = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);

                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            var pp = CurSubmonutParam.PPSettings;


                            if (pptool != null)
                            {
                                var systemPos = CurSubmonutParam.ChipPPPickSystemPos;
                                pp.PPToolZero = pptool.AltimetryOnMark;
                                pp.WorkHeight = systemPos;
                            }
                            else
                            {
                                var systemPos = CurSubmonutParam.ChipPPPickSystemPos;
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                pp.WorkHeight = systemPos;
                            }

                            ExecutionController.Instance.WaitIfPaused();
                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT);
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -targetA, EnumCoordSetType.Relative);

                                _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -targetA, EnumCoordSetType.Relative);



                                //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                                //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.SubmonutPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.SubmonutPPPosCompensateCoordinate1.Y);
                                //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.SubmonutPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.SubmonutPPPosCompensateCoordinate2.Y);
                                //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                                //double angle0 = CurrA;
                                //double angle = CurrA+ targetA;
                                //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                                //ProductExecutor.Instance.CompensateXAfterPickupSubmonut = point3.X;
                                //ProductExecutor.Instance.CompensateYAfterPickupSubmonut = point3.Y;
                                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpSubmonut-End.");
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (CurSubmonutParam.CarrierType == EnumCarrierType.Wafer)
                    {
                        var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedESToolName);
                        if (usedESTool != null)
                        {
                            var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                            offset = pptool.PP1AndBondCameraOffset;
                            if (pptool != null)
                            {
                                var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                                offset.X = usedPPandBondCameraOffsetX;
                                offset.Y = usedPPandBondCameraOffsetY;
                            }
                            var offsetBCAndWC = _systemConfig.PositioningConfig.WaferCameraOrigion;
                            var offsetBCAndWC2 = usedESTool.BondIdentifyNeedleCenter;
                            ExecutionController.Instance.WaitIfPaused();
                            if (_positioningSystem.BondZMovetoSafeLocation()
                            //顶针移动到零点
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //物料中心移动到顶针上方
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                            //芯片吸嘴物料中心上方
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC.X - usedESTool.NeedleCenter.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC.Y - usedESTool.NeedleCenter.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC2.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC2.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //顶针座升起
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurSubmonutParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //拾取芯片
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            )
                            {
                                ProductExecutor.Instance.CurrectPickupSubmonut.X = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                                ProductExecutor.Instance.CurrectPickupSubmonut.Y = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);

                                //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                                var pp = CurSubmonutParam.PPSettings;
                                pp.WorkHeight = CurSubmonutParam.ChipPPPickSystemPos + (float)curDealBP.chipPositionCompensation.Z; ;
                                //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedPPToolName);
                                if (pptool != null)
                                {
                                    pp.PPToolZero = pptool.AltimetryOnMark;
                                }
                                else
                                {
                                    //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                    //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                    //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                    //var ppWorkSystemPos = 0f;
                                    pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                    //pp.WorkHeight = ppWorkSystemPos;
                                }

                                IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();
                                //Thread.Sleep(3000);
                                var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;
                                ExecutionController.Instance.WaitIfPaused();
                                if (PPUtility.Instance.PickViaSystemCoor(pp))
                                {
                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -targetA, EnumCoordSetType.Relative);
                                    _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -targetA, EnumCoordSetType.Relative);
                                }
                                else
                                {
                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                    WarningBox.FormShow("错误", "拾取芯片失败！");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                            else
                            {
                                IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else if (CurSubmonutParam.CarrierType == EnumCarrierType.WaferWafflePack)
                    {
                        //芯片吸嘴物料中心上方
                        var ppSystemOffset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        ppSystemOffset = pptool.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            ppSystemOffset.X = usedPPandBondCameraOffsetX;
                            ppSystemOffset.Y = usedPPandBondCameraOffsetY;
                        }
                        var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;


                        ExecutionController.Instance.WaitIfPaused();
                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //芯片吸嘴物料中心上方
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X+ ProductExecutor.Instance.OffsetBeforePickupSubmonut.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y- ProductExecutor.Instance.OffsetBeforePickupSubmonut.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        ////拾取芯片
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        )
                        {
                            //XY联动移动到物料上方
                            EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                            multiAxis[0] = EnumStageAxis.BondX;
                            multiAxis[1] = EnumStageAxis.BondY;
                            //multiAxis[2] = EnumStageAxis.SubmonutPPT;
                            multiAxis[2] = pptool.StageAxisTheta;
                            double[] targets = new double[3];
                            targets[0] = ppSystemOffset.X + bondcamera2wafercamera.X + curDealBP.chipPositionCompensation.X;
                            targets[1] = ppSystemOffset.Y + bondcamera2wafercamera.Y + curDealBP.chipPositionCompensation.Y;
                            targets[2] = 0;
                            ExecutionController.Instance.WaitIfPaused();
                            StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                            if (result == StageMotionResult.Success)
                            {

                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }

                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,BondXTarget:{ppSystemOffset.X + bondcamera2wafercamera.X}");
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,BondXCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX)}");
                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            var pp = CurSubmonutParam.PPSettings;
                            pp.WorkHeight = CurSubmonutParam.ChipPPPickSystemPos + (float)curDealBP.chipPositionCompensation.Z;

                            //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedPPToolName);
                            if (pptool != null)
                            {
                                //吸嘴工具原点
                                pp.PPToolZero = pptool.AltimetryOnMark;

                            }
                            else
                            {
                                //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                //var ppWorkSystemPos = 0f;
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                //pp.WorkHeight = ppWorkSystemPos;
                            }

                            ProductExecutor.Instance.CurrectPickupSubmonut.X = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                            ProductExecutor.Instance.CurrectPickupSubmonut.Y = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);

                            //var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;
                            //LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpSubmonutWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta}");
                            //LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpSubmonutWithRotate-targetAngle:{targetA}");
                            ExecutionController.Instance.WaitIfPaused();
                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT)}");
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -targetA, EnumCoordSetType.Relative);
                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT)}");
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }

                }

                

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PickUpSubstrate,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }
    public class StepAction_PickUpChip : StepActionBase
    {
        public StepAction_PickUpChip(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                if (CurChipParam.CarrierType == EnumCarrierType.WafflePack)
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpChip-Start.");
                    //BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                    var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                    //安全位
                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                    //吸嘴移动到芯片中心上方

                    var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.ChipPPPosCompensateCoordinate1.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.ChipPPPosCompensateCoordinate1.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        offset.X = usedPPandBondCameraOffsetX;
                        offset.Y = usedPPandBondCameraOffsetY;
                    }
                    if (_positioningSystem.BondZMovetoSafeLocation()
                    //芯片吸嘴T复位
                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                    && _positioningSystem.BondXYUnionMovetoStageCoor(ProductExecutor.Instance.OffsetBeforePickupChip.X + offset.X
                        , ProductExecutor.Instance.OffsetBeforePickupChip.Y + offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    {
                        //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                        var pp = CurChipParam.PPSettings;


                        if (pptool != null)
                        {
                            var systemPos = CurChipParam.ChipPPPickSystemPos;
                            pp.PPToolZero = pptool.AltimetryOnMark;
                            pp.WorkHeight = systemPos;
                        }
                        else
                        {
                            var systemPos = CurChipParam.ChipPPPickSystemPos;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            pp.WorkHeight = systemPos;
                        }

                        if (PPUtility.Instance.PickViaSystemCoor(pp))
                        {
                            //double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, targetA, EnumCoordSetType.Relative);



                            //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                            //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                            //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                            //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                            //double angle0 = CurrA;
                            //double angle = CurrA+ targetA;
                            //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                            //ProductExecutor.Instance.CompensateXAfterPickupChip = point3.X;
                            //ProductExecutor.Instance.CompensateYAfterPickupChip = point3.Y;
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpChip-End.");
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                        WarningBox.FormShow("错误", "拾取芯片失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else if (CurChipParam.CarrierType == EnumCarrierType.Wafer)
                {
                    var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == CurChipParam.RelatedESToolName);
                    if (usedESTool != null)
                    {
                        var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.ChipPPPosCompensateCoordinate1.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.ChipPPPosCompensateCoordinate1.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            offset.X = usedPPandBondCameraOffsetX;
                            offset.Y = usedPPandBondCameraOffsetY;
                        }
                        var offsetBCAndWC = _systemConfig.PositioningConfig.WaferCameraOrigion;
                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //顶针移动到零点
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //物料中心移动到顶针上方
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                        //芯片吸嘴物料中心上方
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC.X - usedESTool.NeedleCenter.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC.Y - usedESTool.NeedleCenter.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //顶针座升起
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurChipParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //拾取芯片
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            var pp = CurChipParam.PPSettings;
                            pp.WorkHeight = CurChipParam.ChipPPPickSystemPos;
                            //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                            if (pptool != null)
                            {
                                pp.PPToolZero = pptool.AltimetryOnMark;
                            }
                            else
                            {
                                //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                //var ppWorkSystemPos = 0f;
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                //pp.WorkHeight = ppWorkSystemPos;
                            }

                            IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();
                            Thread.Sleep(3000);
                            var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, targetA, EnumCoordSetType.Relative);
                            }
                            else
                            {
                                IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else
                        {
                            IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                    }
                    else
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                        WarningBox.FormShow("错误", "拾取芯片失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else if (CurChipParam.CarrierType == EnumCarrierType.WaferWafflePack)
                {
                    //芯片吸嘴物料中心上方
                    var ppSystemOffset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;

                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.ChipPPPosCompensateCoordinate1.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.ChipPPPosCompensateCoordinate1.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        ppSystemOffset.X = usedPPandBondCameraOffsetX;
                        ppSystemOffset.Y = usedPPandBondCameraOffsetY;
                    }
                    var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;
                    if (_positioningSystem.BondZMovetoSafeLocation()
                    //芯片吸嘴物料中心上方
                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                    //拾取芯片
                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                        var pp = CurChipParam.PPSettings;
                        pp.WorkHeight = CurChipParam.ChipPPPickSystemPos;

                        //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                        if (pptool != null)
                        {
                            //吸嘴工具原点
                            pp.PPToolZero = pptool.AltimetryOnMark;

                        }
                        else
                        {
                            //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                            //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                            //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                            //var ppWorkSystemPos = 0f;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            //pp.WorkHeight = ppWorkSystemPos;
                        }


                        var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                        //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                        if (PPUtility.Instance.PickViaSystemCoor(pp))
                        {
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, targetA, EnumCoordSetType.Relative);
                        }
                        else
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PickUpSubstrate,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }

    /*
     * 移动至仰视相机 步骤Action
     */
    public class StepAction_MoveToLookupCamPos : StepActionBase
    {
        public StepAction_MoveToLookupCamPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_MoveToLookupCamPos-Start.");
                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                {
                    var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;

                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                    _positioningSystem.PPMovetoSafeLocation();
                    //-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----

                    double X = 0d;
                    double Y = 0d;
                    double Z = 0d;
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    {


                        if (pptool != null)
                        {
                            X = pptool.ChipPPPosCompensateCoordinate1.X;
                            Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                        }
                        else
                        {
                            X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                            Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                        }
                        //Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZWorkPosition;
                        Z = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().CameraZChipSystemWorkPosition;
                    }
                    else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    {
                        if (pptool != null)
                        {
                            X = pptool.ChipPPPosCompensateCoordinate1.X;
                            Y = pptool.ChipPPPosCompensateCoordinate1.Y;
                        }
                        else
                        {
                            X = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                            Y = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                        }
                        //Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                        Z = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().CameraZChipSystemWorkPosition;
                    }


                    //吸嘴旋转补偿
                    //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

                    //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                    //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

                    //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                    //double angle = targetA;
                    //double angle0 = 0;

                    //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);

                    //X += point3.X;
                    //Y += point3.Y;

                    //-----通过物料名取物料对象，取物料首位置 X Y Z    end -----

                    //移动bond头
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);


                    //XY联动移动到物料上方
                    EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                    multiAxis[0] = EnumStageAxis.BondX;
                    multiAxis[1] = EnumStageAxis.BondY;
                    double[] targets = new double[2];
                    targets[0] = X;
                    targets[1] = Y;
                    //_positioningSystem.MoveAxisToSystemCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                    //拾取之后移动到仰视相机上方
                    _positioningSystem.ChipPPMovetoUplookingCameraCenter();
                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute);
                    //按芯片吸嘴的系统坐标系移动
                    var PPToolZero = 0f;
                    if (pptool != null)
                    {
                        PPToolZero = pptool.AltimetryOnMark;
                    }
                    else
                    {
                        PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                    }
                    _positioningSystem.MoveChipPPToSystemCoord(PPToolZero, Z, EnumCoordSetType.Absolute);

                }

                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_MoveToLookupCamPos-End.");
                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_CamMoveToChipPos,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }

    /*
      * 相机移动至共晶位置 步骤Action
      */
    class StepAction_CamMovToEutecnicPos : StepActionBase
    {
        public StepAction_CamMovToEutecnicPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMovToEutecnicPos-Start.");
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                _positioningSystem.PPMovetoSafeLocation();


                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, _systemConfig.PositioningConfig.EutecticWeldingLocation.Y, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, _systemConfig.PositioningConfig.EutecticWeldingLocation.X, EnumCoordSetType.Absolute);
                //_positioningSystem.BondXYUnionMovetoStageCoor(_systemConfig.PositioningConfig.EutecticWeldingLocation.X, _systemConfig.PositioningConfig.EutecticWeldingLocation.Y, EnumCoordSetType.Absolute);

                //BondZ移动到相机识别位置
                var z = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + _curRecipe.SubstrateInfos.ThicknessMM;
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, z, EnumCoordSetType.Absolute);
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMovToEutecnicPos-End.");
                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_CalibrationBeforePickChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {

            }
        }
    }

    /*
  * 相机移动至共晶位置识别贴装位置 步骤Action
  */
    class StepAction_CamMovToEutecnicVisionPositionPos : StepActionBase
    {
        public StepAction_CamMovToEutecnicVisionPositionPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMovToEutecnicPos-Start.");
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                if(_positioningSystem.PPMovetoSafeLocation() == StageMotionResult.Success)
                {
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, _systemConfig.PositioningConfig.EutecticWeldingLocation.Y, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, _systemConfig.PositioningConfig.EutecticWeldingLocation.X, EnumCoordSetType.Absolute);
                    //_positioningSystem.BondXYUnionMovetoStageCoor(_systemConfig.PositioningConfig.EutecticWeldingLocation.X, _systemConfig.PositioningConfig.EutecticWeldingLocation.Y, EnumCoordSetType.Absolute);

                    //BondZ移动到相机识别位置
                    var z = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + _curRecipe.SubmonutInfos.ThicknessMM;
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, z, EnumCoordSetType.Absolute);
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_CamMovToEutecnicPos-End.");

                    MatchIdentificationParam visionParam = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault();
                    var PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                    if (PositionBondChipResult != null)
                    {
                        ProductExecutor.Instance.OffsetBeforeEutecticChip = PositionBondChipResult;
                    }
                    else
                    {
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else
                {
                    return GlobalGWResultDefine.RET_FAILED;
                }


                


                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_CalibrationBeforePickChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {

            }
        }
    }


    /*
     * 物料Step 6 - 放置芯片前识别贴装位置 步骤Action
     */
    public class StepAction_PositionBondPos : StepActionBase
    {
        public StepAction_PositionBondPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_RecognizeBondPos-Start.");
                //CameraWindowGUI.Instance?.SelectCamera(0);
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                ProductExecutor.Instance.CurSubstrateNum = 1;
                ProductExecutor.Instance.CurModuleNum = 1;
                var ret= GlobalGWResultDefine.RET_SUCCESS;
                int substrateIndex = 0;
                _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos.Clear();
                foreach (var substrateModules in _curRecipe.CurrentSubstrate.ModuleMapInfos)
                {
                    
                    var homeX = _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].MaterialLocation.X - _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].PositionSubstrateMark1Result?.X;
                    var homeY = _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].MaterialLocation.Y + _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].PositionSubstrateMark1Result?.Y;
                    List<MaterialMapInformation> temp = new List<MaterialMapInformation>();
                    var positionBondChipCounter = 0;
                    List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>> tempList = new List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>>();

                    ProductExecutor.Instance.CurModuleNum = 1;
                    if(_curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].IsPositionSuccess)
                    {
                        if (ProductExecutor.Instance.CurSubstrateNum >= ProductExecutor.Instance.StartSubstrateNum &&
    ProductExecutor.Instance.CurSubstrateNum <= ProductExecutor.Instance.EndSubstrateNum)
                        {
                            foreach (var itemModule in substrateModules)
                            {
                                ProductExecutor.Instance.CurBondingPositionNum = 1;

                                var dd = new Tuple<MaterialMapInformation, List<BondingPositionSettings>>(itemModule, new List<BondingPositionSettings>());

                                if (ProductExecutor.Instance.CurModuleNum >= ProductExecutor.Instance.StartModuleNum &&
                            ProductExecutor.Instance.CurModuleNum <= ProductExecutor.Instance.EndModuleNum)
                                {
                                    foreach (var itemStepBP in _curRecipe.StepBondingPositionList_2)
                                    {
                                        if (ProductExecutor.Instance.CurBondingPositionNum >= ProductExecutor.Instance.StartBondingPositionNum &&
                            ProductExecutor.Instance.CurBondingPositionNum <= ProductExecutor.Instance.EndBondingPositionNum)
                                        {
                                            if (ProductExecutor.Instance.RunStat != EnumProductRunStat.UserAbort)
                                            {
                                                if (ProductExecutor.Instance.IsProcessPart)
                                                {
                                                    if (positionBondChipCounter >= ProductExecutor.Instance.ManualSettedProcessCount)
                                                    {
                                                        break;
                                                    }
                                                }

                                                BondingPositionSettings newBondPosObj = new BondingPositionSettings();
                                                newBondPosObj = itemStepBP.DeepCopy();
                                                VisionIdentificationParam visionParam = new VisionIdentificationParam();
                                                if (itemStepBP.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                {
                                                    visionParam = itemStepBP.VisionParametersForFindBondPosition.ShapeMatchParameters[0];
                                                }
                                                else if (itemStepBP.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                {
                                                    visionParam = itemStepBP.VisionParametersForFindBondPosition.LineSearchParams[0];
                                                }
                                                else if (itemStepBP.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                {
                                                    visionParam = itemStepBP.VisionParametersForFindBondPosition.CircleSearchParameters[0];
                                                }

                                                // 将角度转换为弧度
                                                double angleRadians = 0;
                                                if (_curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].PositionSubstrateMark1Result != null)
                                                {
                                                    angleRadians = _curRecipe.CurrentSubstrate.SubstrateMapInfos[substrateIndex].PositionSubstrateMark1Result.Theta * Math.PI / 180.0;
                                                }
                                                double x = itemModule.MaterialLocation.X - visionParam.PatternOffsetWithMaterialCenter.X;
                                                double y = itemModule.MaterialLocation.Y - visionParam.PatternOffsetWithMaterialCenter.Y;

                                                // 计算正弦和余弦值
                                                double cosTheta = Math.Cos(angleRadians);
                                                double sinTheta = Math.Sin(angleRadians);

                                                // 应用旋转矩阵公式：
                                                // x' = x * cosθ - y * sinθ
                                                // y' = x * sinθ + y * cosθ
                                                double newX = x * cosTheta - y * sinTheta;
                                                double newY = x * sinTheta + y * cosTheta;

                                                double X = newX + (double)homeX;
                                                double Y = newY + (double)homeY;
                                                double Z = visionParam.CameraZWorkPosition;
                                                ExecutionController.Instance.WaitIfPaused();
                                                if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                    && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                {
                                                    XYZTCoordinateConfig PositionBondChipResult = new XYZTCoordinateConfig();
                                                    if (itemStepBP.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                    {
                                                        newBondPosObj.PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                    }
                                                    else if (itemStepBP.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                    {
                                                        newBondPosObj.PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                    }
                                                    else if (itemStepBP.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                    {
                                                        newBondPosObj.PositionBondChipResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                    }

                                                    if (newBondPosObj.PositionBondChipResult != null)
                                                    {
                                                        LogRecorder.RecordLog(EnumLogContentType.Debug
                                                            , $"StepAction_PositionBondPos:{itemModule.MaterialNumber},BPName:{itemStepBP.Name};VisionX:{newBondPosObj.PositionBondChipResult.X},VisionY:{newBondPosObj.PositionBondChipResult.Y},VisionT:{newBondPosObj.PositionBondChipResult.Theta}.");
                                                        //识别在视野中心右边时resultX为负
                                                        //if (itemModule.MaterialCoordIndex.Y < 6)
                                                        //{
                                                        //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) - newBondPosObj.PositionBondChipResult.X;
                                                        //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + newBondPosObj.PositionBondChipResult.Y;
                                                        //}
                                                        //else
                                                        //{
                                                        //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) - newBondPosObj.PositionBondChipResult.X - (itemModule.MaterialCoordIndex.Y - 5) * 0.0005;
                                                        //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + newBondPosObj.PositionBondChipResult.Y;
                                                        //}
                                                        newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) - newBondPosObj.PositionBondChipResult.X;
                                                        newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + newBondPosObj.PositionBondChipResult.Y;


                                                        LogRecorder.RecordLog(EnumLogContentType.Debug
                                                            , $"StepAction_PositionBondPos:{itemModule.MaterialNumber},BPName:{itemStepBP.Name};PosX:{newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X},PosY:{newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y}.");
                                                        positionBondChipCounter++;
                                                    }
                                                    else
                                                    {
                                                        if(ShowMessage2("异常发生！", "搜寻贴片位置特征失败，是否手动对准该特征位置继续生产？", "警报") == 1)
                                                        {
                                                            newBondPosObj.PositionBondChipResult = new XYZTCoordinateConfig()
                                                            { 
                                                                X=0,
                                                                Y=0,
                                                            };

                                                            //识别在视野中心右边时resultX为负
                                                            //if (itemModule.MaterialCoordIndex.Y < 6)
                                                            //{
                                                            //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) - newBondPosObj.PositionBondChipResult.X;
                                                            //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + newBondPosObj.PositionBondChipResult.Y;
                                                            //}
                                                            //else
                                                            //{
                                                            //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) - newBondPosObj.PositionBondChipResult.X - (itemModule.MaterialCoordIndex.Y - 5) * 0.0005;
                                                            //    newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + newBondPosObj.PositionBondChipResult.Y;
                                                            //}
                                                            newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                                                            newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);


                                                            LogRecorder.RecordLog(EnumLogContentType.Debug
                                                                , $"StepAction_PositionBondPos:{itemModule.MaterialNumber},BPName:{itemStepBP.Name};PosX:{newBondPosObj.BondPositionSystemPosAfterVisionCalibration.X},PosY:{newBondPosObj.BondPositionSystemPosAfterVisionCalibration.Y}.");
                                                            positionBondChipCounter++;
                                                        }
                                                        else
                                                        {
                                                            newBondPosObj.PositionBondChipResult = null;
                                                            LogRecorder.RecordLog(EnumLogContentType.Warn, $"RecognizeBondPos Failed.MaterialNumber:{itemModule.MaterialNumber},BPName:{itemStepBP.Name}");
                                                        }
                                                    }
                                                    newBondPosObj.IsPositionSuccess = newBondPosObj.PositionBondChipResult == null ? false : true;

                                                }
                                                dd.Item2.Add(newBondPosObj);
                                            }

                                        }
                                        else
                                        {
                                            dd.Item2.Add(null);
                                        }

                                        ProductExecutor.Instance.CurBondingPositionNum++;
                                    }

                                }


                                tempList.Add(dd);

                                ProductExecutor.Instance.CurModuleNum++;

                            }
                            if (!_curRecipe.CurrentSubstrate.IsPositionModules)
                            {

                                //如果module不需要定位，贴装位置角度使用大板角度
                                var angleOrigon = Math.Atan((_curRecipe.CurrentSubstrate.SubstrateCoordinateHomeSecondPoint.Y - _curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y)
                                    / (_curRecipe.CurrentSubstrate.SubstrateCoordinateHomeSecondPoint.X - _curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X));
                                var curAngle = Math.Atan((ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y - ProductExecutor.Instance.SubstrateCoordinateHomePoint.Y)
                                    / (ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X - ProductExecutor.Instance.SubstrateCoordinateHomePoint.X));
                                var diffThetaRadians = angleOrigon - curAngle;
                                float diffThetaDegrees = (float)(diffThetaRadians * 180.0 / Math.PI);
                                if (diffThetaDegrees < -170 && diffThetaDegrees > -190)
                                {
                                    diffThetaDegrees = diffThetaDegrees + 180;
                                }
                                if (diffThetaDegrees > 170 && diffThetaDegrees < 190)
                                {
                                    diffThetaDegrees = diffThetaDegrees - 180;
                                }
                                if (diffThetaDegrees > 90 || diffThetaDegrees < -90)
                                {
                                    diffThetaDegrees = 0;
                                }
                                foreach (var tuple in tempList)
                                {
                                    foreach (var bondPos in tuple.Item2)
                                    {
                                        if (bondPos.PositionBondChipResult != null)
                                        {
                                            bondPos.PositionBondChipResult.Theta = diffThetaDegrees;
                                            LogRecorder.RecordLog(EnumLogContentType.Debug
                                                    , $"StepAction_PositionBondPos:BPName:{bondPos.Name};VisionX:{bondPos.PositionBondChipResult.X},VisionY:{bondPos.PositionBondChipResult.Y},VisionT:{bondPos.PositionBondChipResult.Theta}.");

                                        }
                                    }
                                }


                            }
                            else
                            {

                            }

                            if (SystemConfiguration.Instance.JobConfig.IsLaserScanningHeight)
                            {

                                //激光扫描贴片高度
                                var angleOrigon = Math.Atan((_curRecipe.CurrentSubstrate.SubstrateCoordinateHomeSecondPoint.Y - _curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y)
                                    / (_curRecipe.CurrentSubstrate.SubstrateCoordinateHomeSecondPoint.X - _curRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X));
                                var curAngle = Math.Atan((ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.Y - ProductExecutor.Instance.SubstrateCoordinateHomePoint.Y)
                                    / (ProductExecutor.Instance.SubstrateCoordinateHomeSecondPoint.X - ProductExecutor.Instance.SubstrateCoordinateHomePoint.X));
                                var diffThetaRadians = angleOrigon - curAngle;
                                float diffThetaDegrees = (float)(diffThetaRadians * 180.0 / Math.PI);
                                foreach (var tuple in tempList)
                                {
                                    foreach (var bondPos in tuple.Item2)
                                    {
                                        if (bondPos.BondPositionSystemPosAfterVisionCalibration != null && bondPos.BondPositionSystemPosAfterVisionCalibration.X != 0
                                             && bondPos.BondPositionSystemPosAfterVisionCalibration.Y != 0)
                                        {

                                            var LaserSensorAndBondCameraOffsetX = (float)-_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X;
                                            var LaserSensorAndBondCameraOffsetY = (float)_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y;

                                            var X = bondPos.BondPositionSystemPosAfterVisionCalibration.X + bondPos.BondPositionCompensation.X + LaserSensorAndBondCameraOffsetX;
                                            var Y = bondPos.BondPositionSystemPosAfterVisionCalibration.Y + bondPos.BondPositionCompensation.Y + LaserSensorAndBondCameraOffsetY;
                                            double Z = bondPos.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().CameraZWorkPosition;
                                            ExecutionController.Instance.WaitIfPaused();
                                            if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                            {
                                                ExecutionController.Instance.WaitIfPaused();
                                                if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                {
                                                    ExecutionController.Instance.WaitIfPaused();
                                                    //读取激光测高仪读数
                                                    Thread.Sleep(500);
                                                    double distance = -1;
                                                    while(distance < 0)
                                                    {
                                                        distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                                                        if (distance >= 0)
                                                        {
                                                            DataModel.Instance.LaserValue = distance / 10000.0f;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            if(ShowMessage2("异常发生！", "贴片位置测高失败，是否继续测高？", "警报") == 1)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                bondPos.IsPositionSuccess = false;
                                                                break;
                                                            }
                                                            DataModel.Instance.LaserValue = 0;
                                                        }
                                                    }
                                                    
                                                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 10000;
                                                    var curLaserMeasureH = DataModel.Instance.LaserValue;
                                                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 1000;
                                                    //根据校准数据及当前的激光测高仪读数计算吸嘴工作高度
                                                    var curBondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                                                    //var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z;
                                                    var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackOrigion.Z;
                                                    var offsetMeasureZ = -(curLaserMeasureH - _systemConfig.PositioningConfig.TrackLaserSensorZ);
                                                    var componentZ = offsetMeasureZ - offsetZ;
                                                    bondPos.BondPositionSystemPosAfterVisionCalibration.Z = (float)-componentZ;

                                                    LogRecorder.RecordLog(EnumLogContentType.Debug
                                                    , $"StepAction_PositionBondPos:BPName:{bondPos.Name};AfterVisionX:{bondPos.BondPositionSystemPosAfterVisionCalibration.X},AfterVisionY:{bondPos.BondPositionSystemPosAfterVisionCalibration.Y},AfterVisionZ:{bondPos.BondPositionSystemPosAfterVisionCalibration.Z}.");
                                                }


                                            }


                                        }
                                    }
                                }


                            }
                            else
                            {

                            }
                        }

                    }




                    _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos.Add(tempList);
                    substrateIndex++;
                    ProductExecutor.Instance.CurSubstrateNum++;
                }

                if(!_positioningSystem.BondMovetoSafeLocation())
                {
                    return GlobalGWResultDefine.RET_FAILED;
                }


                return ret;
            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PositionBondPos,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }

        private int ShowMessage2(string title, string content, string type)
        {
            int result = -1;
            var formReadyEvent = new ManualResetEvent(false);
            //WarningBox1.FormShow(title, content, type);
            MessageBox1 myMessageBox1 = new MessageBox1();
            myMessageBox1.OnButtonClicked += (buttonResult) =>
            {
                result = buttonResult == "confirm" ? 1 : 0;
                formReadyEvent.Set();
            };
            myMessageBox1.showMessage(title, content, type);

            while (!formReadyEvent.WaitOne(100))
            {
                Application.DoEvents();
            }

            return result;
        }

    }
    public class StepAction_Dispense : StepActionBase
    {
        public StepAction_Dispense(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_Dispense-Start.");
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                ProductExecutor.Instance.CurSubstrateNum = 1;
                ProductExecutor.Instance.CurModuleNum = 1;
                ExecutionController.Instance.WaitIfPaused();
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    var dispenseCounter = 0;
                    foreach (var substrateModules in _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos)
                    {
                        foreach (var itemModule in substrateModules)
                        {
                            if (ProductExecutor.Instance.RunStat != EnumProductRunStat.UserAbort)
                            {
                                if (ProductExecutor.Instance.IsProcessPart)
                                {
                                    if (dispenseCounter >= ProductExecutor.Instance.ManualSettedProcessCount)
                                    {
                                        IOUtilityHelper.Instance.UpDispenserCylinder();
                                        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                                        return GlobalGWResultDefine.RET_SUCCESS;
                                    }
                                }
                                if (CurBondPosition != null)
                                {
                                    var curBP = itemModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                                    if (curBP != null)
                                    {
                                        if (curBP.IsPositionSuccess)
                                        {
                                            for(int i=0;i< _curRecipe.CurrentDispenser.DispensingCount;i++)
                                            {
                                                if (_curRecipe.CurrentDispenser.DispensingMode == EnumDispensingMode.Dipping)
                                                {
                                                    var DippingX = (float)_curRecipe.CurrentDispenser.EpoxtToDippingglueCoordinate.X;
                                                    var DippingY = (float)_curRecipe.CurrentDispenser.EpoxtToDippingglueCoordinate.Y;
                                                    ExecutionController.Instance.WaitIfPaused();
                                                    if (_positioningSystem.BondXYUnionMovetoStageCoor(DippingX, DippingY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                    {
                                                        IOUtilityHelper.Instance.DownDispenserCylinder();
                                                        var Z = (float)SystemConfiguration.Instance.PositioningConfig.EpoxtToDippingglueCoordinate.Z;
                                                        ExecutionController.Instance.WaitIfPaused();
                                                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                        {
                                                            Thread.Sleep(50);
                                                            ExecutionController.Instance.WaitIfPaused();
                                                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                IOUtilityHelper.Instance.UpDispenserCylinder();
                                                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                                                                return GlobalGWResultDefine.RET_FAILED;
                                                            }
                                                        }


                                                    }
                                                    else
                                                    {
                                                        IOUtilityHelper.Instance.UpDispenserCylinder();
                                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                                                        return GlobalGWResultDefine.RET_FAILED;
                                                    }
                                                }


                                                var despenserAndBondCameraOffsetX = float.IsNaN(_curRecipe.CurrentDispenser.DispenserPosOffsetXWithBondCamera)
                                                    ? -_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X : _curRecipe.CurrentDispenser.DispenserPosOffsetXWithBondCamera;
                                                var despenserAndBondCameraOffsetY = float.IsNaN(_curRecipe.CurrentDispenser.DispenserPosOffsetYWithBondCamera)
                                                    ? _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y : _curRecipe.CurrentDispenser.DispenserPosOffsetYWithBondCamera;
                                                var X = curBP.BondPositionSystemPosAfterVisionCalibration.X + despenserAndBondCameraOffsetX + curBP.BondPositionCompensation.X + curBP.DispenserPositionCompensation.X;
                                                var Y = curBP.BondPositionSystemPosAfterVisionCalibration.Y + despenserAndBondCameraOffsetY + curBP.BondPositionCompensation.Y + curBP.DispenserPositionCompensation.Y;
                                                ExecutionController.Instance.WaitIfPaused();
                                                if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                {
                                                    IOUtilityHelper.Instance.DownDispenserCylinder();

                                                    var Z = CurEpoxyApplication.DispenserSystemPosZMM + curBP.DispenserPositionCompensation.Z - curBP.SystemHeight + curBP.BondPositionSystemPosAfterVisionCalibration.Z;
                                                    ExecutionController.Instance.WaitIfPaused();
                                                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                    {
                                                        ExecutionController.Instance.WaitIfPaused();
                                                        if (_curRecipe.CurrentDispenser.DispensingMode != EnumDispensingMode.Dipping)
                                                        {
                                                            if (CurEpoxyApplication.DispensePattern == EnumDispensePattern.Point)
                                                            {
                                                                DispenserUtility.Instance.ExecutePointRecipe(CurEpoxyApplication.DispenserRecipeName);
                                                            }
                                                            else
                                                            {
                                                                if(CurEpoxyApplication.DispensePattern ==  EnumDispensePattern.GreekCross)
                                                                {
                                                                    DispenserUtility.Instance.DrawCross(CurEpoxyApplication.DispensePatternWidthMM, CurEpoxyApplication.DispensePatternHeightMM, CurEpoxyApplication.DispenserSpeed);
                                                                }
                                                                else if(CurEpoxyApplication.DispensePattern == EnumDispensePattern.DiagonalCross)
                                                                {
                                                                    DispenserUtility.Instance.DrawGreekCross(CurEpoxyApplication.DispensePatternWidthMM, CurEpoxyApplication.DispensePatternHeightMM, CurEpoxyApplication.DispenserSpeed);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                IOUtilityHelper.Instance.UpDispenserCylinder();
                                                                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                                                                return GlobalGWResultDefine.RET_FAILED;
                                                            }

                                                        }

                                                    }
                                                    

                                                }
                                                SystemConfiguration.Instance.JobConfig.DispenserCounter++;
                                                


                                            }
                                            dispenseCounter++;

                                        }
                                        var despenserAndBondCameraOffsetX2 = float.IsNaN(_curRecipe.CurrentDispenser.DispenserPosOffsetXWithBondCamera)
                                                    ? -_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X : _curRecipe.CurrentDispenser.DispenserPosOffsetXWithBondCamera;
                                        var despenserAndBondCameraOffsetY2 = float.IsNaN(_curRecipe.CurrentDispenser.DispenserPosOffsetYWithBondCamera)
                                            ? _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y : _curRecipe.CurrentDispenser.DispenserPosOffsetYWithBondCamera;
                                        var X2 = curBP.BondPositionSystemPosAfterVisionCalibration.X + despenserAndBondCameraOffsetX2 + curBP.BondPositionCompensation.X + curBP.DispenserPositionCompensation.X;
                                        var Y2 = curBP.BondPositionSystemPosAfterVisionCalibration.Y + despenserAndBondCameraOffsetY2 + curBP.BondPositionCompensation.Y + curBP.DispenserPositionCompensation.Y;
                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_Dispense: Index{dispenseCounter};ExpotName:{CurEpoxyApplication.Name}; BPName:{CurBondPosition.Name};despenserX:{X2},despenserY:{Y2}.");
                                    }
                                }
                            }

                            ProductExecutor.Instance.CurModuleNum++;
                        }
                        ProductExecutor.Instance.CurSubstrateNum++;
                    }
                }
                else
                {
                    IOUtilityHelper.Instance.UpDispenserCylinder();
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                    return GlobalGWResultDefine.RET_FAILED;
                }
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                IOUtilityHelper.Instance.UpDispenserCylinder();
                if (!_positioningSystem.BondMovetoSafeLocation())
                {
                    return GlobalGWResultDefine.RET_FAILED;
                }
                return GlobalGWResultDefine.RET_SUCCESS;
            }
            catch (Exception ex)
            {
                IOUtilityHelper.Instance.UpDispenserCylinder();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_Dispense,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }

    /*
 * 物料Step 6 - 放置芯片后识别贴装位置 步骤Action
 */
    public class StepAction_ComponentCalibrationAfterPPPos : StepActionBase
    {
        public StepAction_ComponentCalibrationAfterPPPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_ComponentCalibrationAfterPPPos-Start.");
                //CameraWindowGUI.Instance?.SelectCamera(0);
                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                int Moduleindex = 0;
                if (_positioningSystem.BondZMovetoSafeLocation())
                {
                    var positionBondChipCounter = 0;
                    foreach (var substrateModules in _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos)
                    {
                        foreach (var itemModule in substrateModules)
                        {
                            if (CurChipParam != null)
                            {
                                if (ProductExecutor.Instance.RunStat != EnumProductRunStat.UserAbort)
                                {
                                    if (ProductExecutor.Instance.IsProcessPart)
                                    {
                                        if (positionBondChipCounter >= ProductExecutor.Instance.ManualSettedProcessCount)
                                        {
                                            IOUtilityHelper.Instance.UpDispenserCylinder();
                                            _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                                            return GlobalGWResultDefine.RET_SUCCESS;
                                        }
                                    }

                                    if (CurBondPosition != null)
                                    {
                                        var curBP = itemModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                                        if (curBP != null)
                                        {
                                            if (curBP.IsPositionSuccess)
                                            {

                                                VisionIdentificationParam visionParam = new VisionIdentificationParam();
                                                int ParametersCount = 0;
                                                if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                {
                                                    visionParam = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.ShapeMatchParameters[0];
                                                    ParametersCount = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.ShapeMatchParameters.Count;
                                                }
                                                else if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                {
                                                    visionParam = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.LineSearchParams[0];
                                                    ParametersCount = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.LineSearchParams.Count;
                                                }
                                                else if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                {
                                                    visionParam = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.CircleSearchParameters[0];
                                                    ParametersCount = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.CircleSearchParameters.Count;
                                                }
                                                //MatchIdentificationParam visionParam = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();

                                                double X = curBP.BondPositionSystemPosAfterVisionCalibration.X + curBP.BondPositionCompensation.X + visionParam.PatternOffsetWithMaterialCenter.X;
                                                double Y = curBP.BondPositionSystemPosAfterVisionCalibration.Y + curBP.BondPositionCompensation.Y + visionParam.PatternOffsetWithMaterialCenter.Y;
                                                double Z = visionParam.CameraZWorkPosition;

                                                if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                                                {
                                                    //CameraWindowGUI.Instance?.SelectCamera(0);
                                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                        && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                    {
                                                        XYZTCoordinateConfig visionRet = new XYZTCoordinateConfig();
                                                        if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                        {
                                                            visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam);
                                                        }
                                                        else if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                        {
                                                            visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam);
                                                        }
                                                        else if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                        {
                                                            visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam);
                                                        }
                                                        //var visionRet = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, visionParam);
                                                        if (visionRet == null)
                                                        {
                                                            if (CurChipParam.CalibrationAfterPPMarkPointCount == 2 && ParametersCount > 1)
                                                            {
                                                                VisionIdentificationParam visionParam2 = new VisionIdentificationParam();
                                                                if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                                {
                                                                    visionParam2 = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.ShapeMatchParameters[1];
                                                                }
                                                                else if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                                {
                                                                    visionParam2 = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.LineSearchParams[1];
                                                                }
                                                                else if (CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                                {
                                                                    visionParam2 = CurChipParam.CalibrationAfterPPComponentPositionVisionParameters.CircleSearchParameters[1];
                                                                }

                                                                double X2 = CurBondPosition.BondPositionSystemPosAfterVisionCalibration.X + CurBondPosition.BondPositionCompensation.X + visionParam2.PatternOffsetWithMaterialCenter.X;
                                                                double Y2 = CurBondPosition.BondPositionSystemPosAfterVisionCalibration.Y + CurBondPosition.BondPositionCompensation.Y + visionParam2.PatternOffsetWithMaterialCenter.Y;
                                                                double Z2 = visionParam2.CameraZWorkPosition;
                                                                ExecutionController.Instance.WaitIfPaused();
                                                                if (_positioningSystem.BondXYUnionMovetoSystemCoor(X2, Y2, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                                                    && _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z2, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                                                {
                                                                    XYZTCoordinateConfig visionRet2 = new XYZTCoordinateConfig();
                                                                    if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (MatchIdentificationParam)visionParam2);
                                                                    }
                                                                    else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (LineFindIdentificationParam)visionParam2);
                                                                    }
                                                                    else if (_curRecipe.CurrentSubstrate.PositionModuleVisionParameters.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                                                                    {
                                                                        visionRet2 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, (CircleFindIdentificationParam)visionParam2);
                                                                    }

                                                                    if (visionRet2 != null)
                                                                    {
                                                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_ComponentCalibrationAfterPPPos:{itemModule.Item1.MaterialNumber}" +
                                                                $",BPName:{CurBondPosition.Name},ComponentName:{CurChipParam.Name} OffsetPosX2:{visionRet.X},OffsetPosY2:{visionRet.Y}.,OffsetPosT2:{visionRet.Theta}.");
                                                                    }
                                                                    else
                                                                    {
                                                                        LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_ComponentCalibrationAfterPPPos:{itemModule.Item1.MaterialNumber}" +
                                                                $",BPName:{CurBondPosition.Name},ComponentName:{CurChipParam.Name};Vision Fail.");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    return GlobalGWResultDefine.RET_FAILED;
                                                                }
                                                            }
                                                            else
                                                            {

                                                            }


                                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_ComponentCalibrationAfterPPPos:{itemModule.Item1.MaterialNumber}" +
                                                                $",BPName:{CurBondPosition.Name},ComponentName:{CurChipParam.Name};Vision Fail.");
                                                            //return GlobalGWResultDefine.RET_FAILED;
                                                        }
                                                        else
                                                        {
                                                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_ComponentCalibrationAfterPPPos:{itemModule.Item1.MaterialNumber}" +
                                                                $",BPName:{CurBondPosition.Name},ComponentName:{CurChipParam.Name} OffsetPosX:{visionRet.X},OffsetPosY:{visionRet.Y}.,OffsetPosT:{visionRet.Theta}.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return GlobalGWResultDefine.RET_FAILED;
                                                    }

                                                }
                                                else
                                                {
                                                    LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_ComponentCalibrationAfterPPPos:ComponentName:{CurChipParam.Name}芯片未创建贴片后识别");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_ComponentCalibrationAfterPPPos:没有贴片位置信息");
                                        }
                                    }
                                    else
                                    {
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_ComponentCalibrationAfterPPPos:没有贴片位置信息");
                                    }
                                    positionBondChipCounter++;
                                }

                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_ComponentCalibrationAfterPPPos:没有芯片信息");
                            }

                        }
                    }
                }
                else
                {
                    IOUtilityHelper.Instance.UpDispenserCylinder();
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_ComponentCalibrationAfterPPPos,Fail.");
                    return GlobalGWResultDefine.RET_FAILED;
                }
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                IOUtilityHelper.Instance.UpDispenserCylinder();

                if (!_positioningSystem.BondMovetoSafeLocation())
                {
                    return GlobalGWResultDefine.RET_FAILED;
                }
                return GlobalGWResultDefine.RET_SUCCESS;


            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_ComponentCalibrationAfterPPPos,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
        }
    }


    /*
     * 吸嘴移动至贴片位置 步骤Action
     */
    class StepAction_PPMovToBondPos : StepActionBase
    {
        public StepAction_PPMovToBondPos(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            Thread.Sleep(2000);
            return GlobalGWResultDefine.RET_SUCCESS;
        }
    }

    /*
     * 物料Step 7 - 放置芯片 步骤Action
     */
    public class StepAction_BondChip : StepActionBase
    {
        public StepAction_BondChip(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if(!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                //
                //吸嘴旋转补偿
                //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //var targetB = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionB;

                var bpPosAngleOffsetWithPattern = CurBondPosition.BondPositionWithPatternOffset.Theta;
                var rotateAngleAfterVisionBondPos = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;


                //二次校准时模板初始角度
                var angleofChipAccuracyPattern = 0f;
                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                }
                else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                }
                double currPPT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                //贴装补偿的角度
                var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                //var targetA = targetB + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //    + materialOrigionA + compensateT - materialOrigionC;curDealBP
                //var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //            + materialOrigionA + compensateT - materialOrigionC;
                var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + bpPosAngleOffsetWithPattern - curDealBP.PositionBondChipResult.Theta
                              + rotateAngleAfterVisionBondPos + compensateT - angleofChipAccuracyPattern;

                if(_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA + 50, EnumCoordSetType.Absolute) == StageMotionResult.Success
                &&_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                {

                    double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                    CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                    PointF point1 = new PointF();
                    PointF point2 = new PointF();
                    if (pptool != null)
                    {
                        point1 = new PointF((float)pptool.ChipPPPosCompensateCoordinate1.X, (float)pptool.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)pptool.ChipPPPosCompensateCoordinate2.X, (float)pptool.ChipPPPosCompensateCoordinate2.Y);
                    }
                    else
                    {
                        point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                    }

                    PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);
                    double angle = CurrA;
                    double angle0 = 0;
                    PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                    double angleX = point3.X;
                    double angleY = point3.Y;
                    //
                    //吸嘴拾取芯片的旋转角度
                    //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //var rotateAngleAfterVisionComponent = ProductExecutor.Instance.OffsetBeforePickupChip?.Theta - materialOrigionB;
                    ////贴装位置视觉识别的旋转角度
                    //var bondPosWithPatternOffsetT = CurBondPosition.BondPositionWithPatternOffset.Theta;


                    //var materialOrigionA = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //var rotateAngleAfterVisionBondPos = ProductExecutor.Instance.OffsetBeforeBondChip?.Theta - materialOrigionA;
                    ////贴装补偿的角度
                    //var compensateT = CurBondPosition.BondPositionCompensation.Theta;

                    ////二次校准时模板初始角度
                    //var materialOrigionC = 0f;
                    //if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod != EnumAccuracyMethod.None)
                    //{
                    //    if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    //    {
                    //        materialOrigionC = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                    //    }
                    //    else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    //    {
                    //        materialOrigionC = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                    //    }
                    //}
                    //var rotateAngleAfterAccuracy = ProductExecutor.Instance.OffsetAfterChipAccuracy?.Theta - materialOrigionC;

                    //var targetA = rotateAngleAfterVisionComponent + rotateAngleAfterAccuracy + bondPosWithPatternOffsetT - rotateAngleAfterVisionBondPos + compensateT;


                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, (double)targetA, EnumCoordSetType.Absolute);

                    //double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                    //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                    //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                    //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);
                    //double angle = CurrA;
                    //double angle0 = 0;
                    //if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    //{
                    //    angle0 = currPPT;
                    //}
                    //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                    //double angleX = point3.X;
                    //double angleY = point3.Y;


                    //贴装补偿的XY,向右向上补偿为正
                    var compensateX = CurBondPosition.BondPositionCompensation.X;
                    var compensateY = CurBondPosition.BondPositionCompensation.Y;

                    //将记录的贴装位置的系统坐标系转换为Stage坐标系
                    var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                    {
                        X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                        Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                    });

                    var baseX = baseStageCoor.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X;
                    var baseY = baseStageCoor.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y;
                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.ChipPPPosCompensateCoordinate1.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.ChipPPPosCompensateCoordinate1.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        baseX = baseStageCoor.X + usedPPandBondCameraOffsetX;
                        baseY = baseStageCoor.Y + usedPPandBondCameraOffsetY;
                    }

                    double X = baseX + ProductExecutor.Instance.OffsetAfterChipAccuracy.X  + angleX - compensateX;
                    double Y = baseY + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y  + angleY + compensateY;

                    //X = baseX;
                    //Y = baseY;

                    if (_positioningSystem.BondXYUnionMovetoStageCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                        Thread.Sleep(_systemConfig.TuningTimeMS);
                        var pp = CurChipParam.PPSettings;

                        if (pptool != null)
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = pptool.AltimetryOnMark;
                            pp.WorkHeight = systemPos;
                        }
                        else
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            pp.WorkHeight = systemPos;
                        }

                        if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-End.");
                        }
                        else
                        {
                            _positioningSystem.PPMovetoSafeLocation();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "放芯片失败！");
                            WarningBox.FormShow("错误", "放芯片失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "放芯片失败！");
                        WarningBox.FormShow("错误", "放芯片失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "放芯片失败！");
                    WarningBox.FormShow("错误", "放芯片失败！");
                    return GlobalGWResultDefine.RET_FAILED;
                }

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_BondChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }
    /// <summary>
    /// 贴片流程优化，不以吸嘴中心为基准计算贴装位置，以仰视实际识别到的中心计算贴装位(有角度补偿)
    /// </summary>
    public class StepAction_BondChipOpt : StepActionBase
    {
        public StepAction_BondChipOpt(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if (!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                //
                //吸嘴旋转补偿
                //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //var targetB = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionB;
                //配方设置的贴装位置偏转角
                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;

                //二次校准时模板初始角度
                var angleofChipAccuracyPattern = 0f;
                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                }
                else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                }
                double currPPT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                //贴装补偿的角度
                var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                //var targetA = targetB + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //    + materialOrigionA + compensateT - materialOrigionC;curDealBP
                //var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //            + materialOrigionA + compensateT - materialOrigionC;
                //Theta轴移动此角度后将chip转正
                var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + bondPosOffsetTheta - curDealBP.PositionBondChipResult.Theta
                              + bondPosOrigionAngle + compensateT - angleofChipAccuracyPattern;

                //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;

//                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Absolute) == StageMotionResult.Success
//&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                {

                    double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                    CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                    PointF point1 = new PointF();
                    PointF point2 = new PointF();
                    if (pptool != null)
                    {
                        point1 = new PointF((float)pptool.ChipPPPosCompensateCoordinate1.X, (float)pptool.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)pptool.ChipPPPosCompensateCoordinate2.X, (float)pptool.ChipPPPosCompensateCoordinate2.Y);
                    }
                    else
                    {
                        point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                    }
                    //计算Theta转正带来的XY偏移
                    PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);
                    double angle = CurrA;
                    double angle0 = 0;
                    PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                    double angleX = point3.X;
                    double angleY = point3.Y;

                    //贴装补偿的XY,向右向上补偿为正
                    var compensateX = CurBondPosition.BondPositionCompensation.X;
                    var compensateY = CurBondPosition.BondPositionCompensation.Y;

                    //将记录的贴装位置的系统坐标系转换为Stage坐标系
                    #region 计算贴装芯片时需要移动到的Stage位置
                    var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                    {
                        X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                        Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                    });

                    var baseX = baseStageCoor.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X;
                    var baseY = baseStageCoor.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y;
                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.ChipPPPosCompensateCoordinate1.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.ChipPPPosCompensateCoordinate1.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        baseX = baseStageCoor.X + usedPPandBondCameraOffsetX;
                        baseY = baseStageCoor.Y + usedPPandBondCameraOffsetY;
                    }

                    double finalX = baseX + ProductExecutor.Instance.OffsetAfterChipAccuracy.X + angleX - compensateX;
                    double finalY = baseY + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y + angleY + compensateY;
                    #endregion

                    #region 计算贴装芯片时需要移动到的stage位置

                    var curChipStagePosXAfterCorrect = curChipCenterStagePosX + angleX - compensateX;
                    var curChipStagePosYAfterCorrect = curChipCenterStagePosY + angleY + compensateY;


                    //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                    var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                    var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;

                    var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                    var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                    #endregion
                    //X = baseX;
                    //Y = baseY;

                    //if (_positioningSystem.BondXYUnionMovetoStageCoor(finalX, finalY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                        Thread.Sleep(_systemConfig.TuningTimeMS);
                        var pp = CurChipParam.PPSettings;

                        if (pptool != null)
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = pptool.AltimetryOnMark;
                            pp.WorkHeight = systemPos;
                        }
                        else
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            pp.WorkHeight = systemPos;
                        }

                        if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-End.");
                        }
                        else
                        {
                            _positioningSystem.PPMovetoSafeLocation();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                            WarningBox.FormShow("错误", "芯片贴装失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                        WarningBox.FormShow("错误", "芯片贴装失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                    WarningBox.FormShow("错误", "芯片贴装失败！");
                    return GlobalGWResultDefine.RET_FAILED;
                }

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_BondChipOpt,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }
    /// <summary>
    /// 贴片流程优化，不以吸嘴中心为基准计算贴装位置，以仰视实际识别到的中心计算贴装位(拾取芯片之后已经做了一次角度补偿，在这里会进行角度旋转补偿)
    /// </summary>
    public class StepAction_BondChipOptTwoTimesRotate : StepActionBase
    {
        public StepAction_BondChipOptTwoTimesRotate(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if (!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                //
                //吸嘴旋转补偿
                //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //var targetB = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionB;
                //配方设置的贴装位置偏转角
                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;

                //二次校准时模板初始角度
                var angleofChipAccuracyPattern = 0f;
                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                }
                else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                }
                double currPPT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                //贴装补偿的角度
                var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                //var targetA = targetB + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //    + materialOrigionA + compensateT - materialOrigionC;curDealBP
                //var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //            + materialOrigionA + compensateT - materialOrigionC;
                //Theta轴移动此角度后将chip转正
                var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta - angleofChipAccuracyPattern  - curDealBP.PositionBondChipResult.Theta
                              + bondPosOrigionAngle + compensateT + bondPosOffsetTheta;
                double initialChipPPTheta = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;

                //                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Absolute) == StageMotionResult.Success
                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                {

                    double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                    CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                    PointF point1 = new PointF();
                    PointF point2 = new PointF();
                    if (pptool != null)
                    {
                        point1 = new PointF((float)pptool.ChipPPPosCompensateCoordinate1.X, (float)pptool.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)pptool.ChipPPPosCompensateCoordinate2.X, (float)pptool.ChipPPPosCompensateCoordinate2.Y);
                    }
                    else
                    {
                        point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                    }
                    //计算Theta转正带来的XY偏移
                    PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);
                    double angle = CurrA;
                    //double angle0 = 0;
                    double angle0 = initialChipPPTheta;
                    PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                    double angleX = point3.X;
                    double angleY = point3.Y;

                    //贴装补偿的XY,向右向上补偿为正
                    var compensateX = CurBondPosition.BondPositionCompensation.X;
                    var compensateY = CurBondPosition.BondPositionCompensation.Y;

                    //将记录的贴装位置的系统坐标系转换为Stage坐标系
                    #region 计算贴装芯片时需要移动到的Stage位置
                    var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                    {
                        X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                        Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                    });

                    var baseX = baseStageCoor.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X;
                    var baseY = baseStageCoor.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y;
                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.ChipPPPosCompensateCoordinate1.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.ChipPPPosCompensateCoordinate1.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        baseX = baseStageCoor.X + usedPPandBondCameraOffsetX;
                        baseY = baseStageCoor.Y + usedPPandBondCameraOffsetY;
                    }

                    double finalX = baseX + ProductExecutor.Instance.OffsetAfterChipAccuracy.X + angleX - compensateX;
                    double finalY = baseY + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y + angleY + compensateY;
                    #endregion

                    #region 计算贴装芯片时需要移动到的stage位置

                    var curChipStagePosXAfterCorrect = curChipCenterStagePosX + angleX - compensateX;
                    var curChipStagePosYAfterCorrect = curChipCenterStagePosY + angleY + compensateY;


                    //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                    var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                    var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;

                    var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                    var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                    #endregion
                    //X = baseX;
                    //Y = baseY;

                    //if (_positioningSystem.BondXYUnionMovetoStageCoor(finalX, finalY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                        Thread.Sleep(_systemConfig.TuningTimeMS);
                        var pp = CurChipParam.PPSettings;

                        if (pptool != null)
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = pptool.AltimetryOnMark;
                            pp.WorkHeight = systemPos;
                        }
                        else
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            pp.WorkHeight = systemPos;
                        }

                        if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-End.");
                        }
                        else
                        {
                            _positioningSystem.PPMovetoSafeLocation();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                            WarningBox.FormShow("错误", "芯片贴装失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                        WarningBox.FormShow("错误", "芯片贴装失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                    WarningBox.FormShow("错误", "芯片贴装失败！");
                    return GlobalGWResultDefine.RET_FAILED;
                }

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_BondChipOpt,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }
    /// <summary>
    /// 芯片二次识别后不再进行角度补偿，只找center
    /// </summary>
    public class StepAction_BondChipOptNoAngleCalibration : StepActionBase
    {
        public StepAction_BondChipOptNoAngleCalibration(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if (!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                //
                //吸嘴旋转补偿
                //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //var targetB = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionB;
                //配方设置的贴装位置偏转角
                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;

                //二次校准时模板初始角度
                //var angleofChipAccuracyPattern = 0f;
                //if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                //{
                //    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //}
                //else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                //{
                //    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                //}
                //double currPPT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                //贴装补偿的角度
                //var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                ////var targetA = targetB + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                ////    + materialOrigionA + compensateT - materialOrigionC;curDealBP
                ////var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                ////            + materialOrigionA + compensateT - materialOrigionC;
                ////Theta轴移动此角度后将chip转正
                //var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + bondPosOffsetTheta - curDealBP.PositionBondChipResult.Theta
                //              + bondPosOrigionAngle + compensateT - angleofChipAccuracyPattern;

                //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;

                //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                //{

                //    double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                    //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                    //PointF point1 = new PointF();
                    //PointF point2 = new PointF();
                    //if (pptool != null)
                    //{
                    //    point1 = new PointF((float)pptool.ChipPPPosCompensateCoordinate1.X, (float)pptool.ChipPPPosCompensateCoordinate1.Y);
                    //    point2 = new PointF((float)pptool.ChipPPPosCompensateCoordinate2.X, (float)pptool.ChipPPPosCompensateCoordinate2.Y);
                    //}
                    //else
                    //{
                    //    point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                    //    point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                    //}
                    ////计算Theta转正带来的XY偏移
                    //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);
                    //double angle = CurrA;
                    //double angle0 = 0;
                    //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                    //double angleX = point3.X;
                    //double angleY = point3.Y;

                    //贴装补偿的XY,向右向上补偿为正
                    var compensateX = CurBondPosition.BondPositionCompensation.X;
                    var compensateY = CurBondPosition.BondPositionCompensation.Y;

                    //将记录的贴装位置的系统坐标系转换为Stage坐标系
                    #region 计算贴装芯片时需要移动到的Stage位置
                    var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                    {
                        X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                        Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                    });

                    #endregion

                    #region 计算贴装芯片时需要移动到的stage位置

                    var curChipStagePosXAfterCorrect = curChipCenterStagePosX  - compensateX;
                    var curChipStagePosYAfterCorrect = curChipCenterStagePosY  + compensateY;


                    //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                    var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                    var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;

                    var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                    var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                    #endregion
                    //X = baseX;
                    //Y = baseY;

                    //if (_positioningSystem.BondXYUnionMovetoStageCoor(finalX, finalY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                        Thread.Sleep(_systemConfig.TuningTimeMS);
                        var pp = CurChipParam.PPSettings;

                        if (pptool != null)
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = pptool.AltimetryOnMark;
                            pp.WorkHeight = systemPos;
                        }
                        else
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            pp.WorkHeight = systemPos;
                        }

                        if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-End.");
                        }
                        else
                        {
                            _positioningSystem.PPMovetoSafeLocation();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                            WarningBox.FormShow("错误", "芯片贴装失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                        WarningBox.FormShow("错误", "芯片贴装失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                //}
                //else
                //{
                //    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                //    WarningBox.FormShow("错误", "芯片贴装失败！");
                //    return GlobalGWResultDefine.RET_FAILED;
                //}

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_BondChipOpt,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }
    /// <summary>
    /// 芯片二次识别后不再进行角度补偿，只找center,且芯贴装位置做XY位置补偿时，考虑角度
    /// </summary>
    public class StepAction_BondChipNewCompensation : StepActionBase
    {
        public StepAction_BondChipNewCompensation(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if (!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
    
                //配方设置的贴装位置偏转角
                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;



                //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;


                //贴装补偿的XY,向右向上补偿为正
                var compensateX = CurBondPosition.BondPositionCompensation.X;
                var compensateY = CurBondPosition.BondPositionCompensation.Y;
                float thetaRadians = (float)((bondPosOrigionAngle - curDealBP.PositionBondChipResult.Theta) * Math.PI / 180.0);
                if (thetaRadians > 0)
                {
                    compensateX = Math.Cos(thetaRadians) * compensateX;
                    compensateY = Math.Cos(thetaRadians) * compensateY;
                }
                else
                {
                    compensateX = Math.Cos(-thetaRadians) * compensateX;
                    compensateY =  Math.Cos(-thetaRadians) * compensateY; 
                }

                //将记录的贴装位置的系统坐标系转换为Stage坐标系
                #region 计算贴装芯片时需要移动到的Stage位置
                var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                {
                    X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                    Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                });

                #endregion

                #region 计算贴装芯片时需要移动到的stage位置

                var curChipStagePosXAfterCorrect = curChipCenterStagePosX - compensateX;
                var curChipStagePosYAfterCorrect = curChipCenterStagePosY + compensateY;


                //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;

                var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                #endregion
                //X = baseX;
                //Y = baseY;

                //if (_positioningSystem.BondXYUnionMovetoStageCoor(finalX, finalY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                {
                    //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                    Thread.Sleep(_systemConfig.TuningTimeMS);
                    var pp = CurChipParam.PPSettings;

                    if (pptool != null)
                    {
                        var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                        pp.PPToolZero = pptool.AltimetryOnMark;
                        pp.WorkHeight = systemPos;
                    }
                    else
                    {
                        var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                        pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        pp.WorkHeight = systemPos;
                    }

                    if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                    {
                        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-End.");
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                        WarningBox.FormShow("错误", "芯片贴装失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else
                {
                    _positioningSystem.PPMovetoSafeLocation();
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                    WarningBox.FormShow("错误", "芯片贴装失败！");
                    return GlobalGWResultDefine.RET_FAILED;
                }
                //}
                //else
                //{
                //    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                //    WarningBox.FormShow("错误", "芯片贴装失败！");
                //    return GlobalGWResultDefine.RET_FAILED;
                //}

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_BondChipOpt,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }
    /// <summary>
    /// 使用新的Theta旋转补偿XY公式（第一次拾取芯片之后，没有进行角度补偿，在这里一次性转到位）
    /// </summary>
    public class StepAction_BondChipOptNewXYDeviationCal : StepActionBase
    {
        public StepAction_BondChipOptNewXYDeviationCal(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipOpt-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.SubstrateInfos.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if (!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                //
                //吸嘴旋转补偿
                //var materialOrigionB = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //var targetB = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionB;
                //配方设置的贴装位置偏转角
                var bondPosOffsetTheta = CurBondPosition.BondPositionWithPatternOffset.Theta;
                var bondPosOrigionAngle = CurBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault().OrigionAngle;

                //二次校准时模板初始角度
                var angleofChipAccuracyPattern = 0f;
                if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                }
                else if (CurChipParam.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    angleofChipAccuracyPattern = CurChipParam.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault().OrigionAngle;
                }
                double currPPT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                //贴装补偿的角度
                var compensateT = CurBondPosition.BondPositionCompensation.Theta;
                //var targetA = targetB + ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //    + materialOrigionA + compensateT - materialOrigionC;curDealBP
                //var targetA = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + materialOrigionAB - ProductExecutor.Instance.OffsetBeforeBondChip.Theta
                //            + materialOrigionA + compensateT - materialOrigionC;
                //Theta轴移动此角度后将chip转正
                var finalAngle = ProductExecutor.Instance.OffsetAfterChipAccuracy.Theta + bondPosOffsetTheta - curDealBP.PositionBondChipResult.Theta
                              + bondPosOrigionAngle + compensateT - angleofChipAccuracyPattern;

                //计算二次识别后芯片中心所处的位置的Stage位置(拾取芯片后没有补偿角度)
                var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + ProductExecutor.Instance.OffsetAfterChipAccuracy.X;
                var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + ProductExecutor.Instance.OffsetAfterChipAccuracy.Y;

                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle + 50, EnumCoordSetType.Absolute) == StageMotionResult.Success
                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -50, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -finalAngle, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                {
                    #region 计算贴装芯片时需要移动到的Stage位置
                    var baseStageCoor = _positioningSystem.ConvertBondCameraSystemCoordToStageCoord(new XYZTCoordinate
                    {
                        X = curDealBP.BondPositionSystemPosAfterVisionCalibration.X,
                        Y = curDealBP.BondPositionSystemPosAfterVisionCalibration.Y
                    });

                    #endregion
                    double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                    CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                    PointF point1 = new PointF();
                    PointF point2 = new PointF();
                    if (pptool != null)
                    {
                        point1 = new PointF((float)pptool.ChipPPPosCompensateCoordinate1.X, (float)pptool.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)pptool.ChipPPPosCompensateCoordinate2.X, (float)pptool.ChipPPPosCompensateCoordinate2.Y);
                    }
                    else
                    {
                        point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                        point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                    }
                    //计算Theta转正带来的XY偏移
                    PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);
                    double angle = CurrA;
                    double angle0 = 0;
                    //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                    PointF centerOffset = new PointF();
                    PointF point3 = PPCalibration.PPXYDeviationCal((float)curChipCenterStagePosX, (float)curChipCenterStagePosY, (float)-CurrA,out centerOffset);
                    //double angleX = point3.X- curChipCenterStagePosX;
                    //double angleY = point3.Y- curChipCenterStagePosY;

                    var chipCenterOffsetXAfterRotate = centerOffset.X;
                    var chipCenterOffsetYAfterRotate = centerOffset.Y;

                    //贴装补偿的XY,向右向上补偿为正
                    var compensateX = CurBondPosition.BondPositionCompensation.X;
                    var compensateY = CurBondPosition.BondPositionCompensation.Y;

                    //将记录的贴装位置的系统坐标系转换为Stage坐标系

                    #region 计算贴装芯片时需要移动到的stage位置

                    var curChipStagePosXAfterCorrect = curChipCenterStagePosX + chipCenterOffsetXAfterRotate - compensateX;
                    var curChipStagePosYAfterCorrect = curChipCenterStagePosY + chipCenterOffsetYAfterRotate + compensateY;


                    //计算旋转、贴装补偿之后的芯片和榜头相机的偏移
                    var chipCentetAndBondCameraCenterOffsetX = curChipStagePosXAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                    var chipCentetAndBondCameraCenterOffsetY = curChipStagePosYAfterCorrect - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;

                    var finalXOpt = baseStageCoor.X + chipCentetAndBondCameraCenterOffsetX;
                    var finalYOpt = baseStageCoor.Y + chipCentetAndBondCameraCenterOffsetY;
                    #endregion
                    //X = baseX;
                    //Y = baseY;

                    //if (_positioningSystem.BondXYUnionMovetoStageCoor(finalX, finalY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    if (_positioningSystem.BondXYUnionMovetoStageCoor(finalXOpt, finalYOpt, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        //_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute);

                        Thread.Sleep(_systemConfig.TuningTimeMS);
                        var pp = CurChipParam.PPSettings;

                        if (pptool != null)
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = pptool.AltimetryOnMark;
                            pp.WorkHeight = systemPos;
                        }
                        else
                        {
                            var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                            pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                            pp.WorkHeight = systemPos;
                        }

                        if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChip-End.");
                        }
                        else
                        {
                            _positioningSystem.PPMovetoSafeLocation();
                            LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                            WarningBox.FormShow("错误", "芯片贴装失败！");
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                        WarningBox.FormShow("错误", "芯片贴装失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                }
                else
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                    WarningBox.FormShow("错误", "芯片贴装失败！");
                    return GlobalGWResultDefine.RET_FAILED;
                }

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_BondChipOpt,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }
    /// <summary>
    /// 芯片二次识别后不再进行角度补偿，只找center,且芯贴装位置做XY位置补偿时，考虑角度
    /// </summary>
    public class StepAction_OnlyBondChip : StepActionBase
    {
        public StepAction_OnlyBondChip(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_OnlyBondChip-Start.");
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                var curSubstrate = _curRecipe.CurrentSubstrate.ModuleMapInfosWithBondPositionInfos[ProductExecutor.Instance.CurSubstrateNum - 1];
                var curModule = curSubstrate[ProductExecutor.Instance.CurModuleNum - 1];
                var curDealBP = CurBondPosition;
                if (CurBondPosition != null)
                {
                    curDealBP = curModule.Item2.FirstOrDefault(i => i.Name == CurBondPosition.Name);
                }
                if (!curDealBP.IsPositionSuccess)
                {
                    return GlobalGWResultDefine.RET_BPInvalid;
                }
                //使用的吸嘴工具
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);



                Thread.Sleep(_systemConfig.TuningTimeMS);
                var pp = CurChipParam.PPSettings;

                if (pptool != null)
                {
                    var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                    if (SystemConfiguration.Instance.JobConfig.IsLaserScanningHeight)
                    {
                        systemPos = (float)(curDealBP.BondPositionSystemPosAfterVisionCalibration.Z + CurChipParam.ThicknessMM);
                        
                    }
                    
                    pp.PPToolZero = pptool.AltimetryOnMark;
                    pp.WorkHeight = systemPos + (float)curDealBP.BondPositionCompensation.Z;
                }
                else
                {
                    var systemPos = curDealBP.SystemHeight + CurChipParam.ThicknessMM;
                    if (SystemConfiguration.Instance.JobConfig.IsLaserScanningHeight)
                    {
                        systemPos = (float)(curDealBP.BondPositionSystemPosAfterVisionCalibration.Z + CurChipParam.ThicknessMM);

                    }
                    pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                    pp.WorkHeight = systemPos + (float)curDealBP.BondPositionCompensation.Z;
                }
                ExecutionController.Instance.WaitIfPaused();
                if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_OnlyBondChip-End.");
                }
                else
                {
                    _positioningSystem.PPMovetoSafeLocation();
                    LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片贴装失败！");
                    WarningBox.FormShow("错误", "芯片贴装失败！");
                    return GlobalGWResultDefine.RET_FAILED;
                }
                //CurChipParam.ProduceCount++;
                //SaveCurChipParam();
                SystemConfiguration.Instance.JobConfig.TotalBondCounter++;

                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_OnlyBondChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }
    }

    /// <summary>
    /// 贴衬底到共晶台位置 芯片二次识别后不再进行角度补偿，只找center,且芯贴装位置做XY位置补偿时，考虑角度
    /// </summary>
    public class StepAction_BondSubmonutToEutectic : StepActionBase
    {
        public StepAction_BondSubmonutToEutectic(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondSubmonutToEutectic-Start.");

                CameraWindowGUI.Instance?.SelectCamera(0);
                //LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondSubmonutToEutectic-Start.");
                var materialOrigionA_init = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA_init;
                var PPtool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                if (_positioningSystem.BondZMovetoSafeLocation()
                    //&& _positioningSystem.PPtoolMovetoEutecticTableCenter(PPtool)
                    && _positioningSystem.MoveAixsToStageCoord(PPtool.StageAxisTheta, -targetA, EnumCoordSetType.Relative) == StageMotionResult.Success)
                {
                    double[] Target = new double[2];
                    Target[0] = _systemConfig.PositioningConfig.EutecticWeldingLocation.X + ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.X;
                    Target[1] = _systemConfig.PositioningConfig.EutecticWeldingLocation.Y + ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.Y;
                    if (PPtool != null)
                    {
                        Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X - PPtool.LookuptoPPOrigion.X);
                        Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y - PPtool.LookuptoPPOrigion.Y);
                    }
                    else
                    {
                        Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X - _systemConfig.PositioningConfig.LookupChipPPOrigion.X);
                        Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y - _systemConfig.PositioningConfig.LookupChipPPOrigion.Y);
                    }
                    EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                    multiAxis[0] = EnumStageAxis.BondX;
                    multiAxis[1] = EnumStageAxis.BondY;
                    _positioningSystem.MoveAixsToStageCoord(multiAxis, Target, EnumCoordSetType.Absolute);


                    //放芯片
                    var ppParam = CurSubmonutParam.PPSettings;

                    if (PPtool != null)
                    {
                        var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurSubmonutParam.ThicknessMM;
                        ppParam.PPToolZero = PPtool.AltimetryOnMark;
                        ppParam.WorkHeight = (float)systemPos;
                    }
                    else
                    {
                        var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurSubmonutParam.ThicknessMM;
                        ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        ppParam.WorkHeight = (float)systemPos;
                    }

                    if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnEutecticTable, true))
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "放置衬底到共晶台失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                    }


                }



                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_OnlyBondChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }

        public void AfterPlaceChipOnEutecticTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromEutecticTable()
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }

    /// <summary>
    /// 贴芯片到贴装位置 芯片二次识别后不再进行角度补偿，只找center,且芯贴装位置做XY位置补偿时，考虑角度
    /// </summary>
    public class StepAction_BondChipToEutectic : StepActionBase
    {
        public StepAction_BondChipToEutectic(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipToEutectic-Start.");

                CameraWindowGUI.Instance?.SelectCamera(0);
                //LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondChipToEutectic-Start.");
                var materialOrigionA_init = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                var targetA = ProductExecutor.Instance.OffsetBeforeEutecticChip.Theta - materialOrigionA_init + CurBondPosition.BondPositionCompensation.Theta;
                var PPtool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.PPSettings.PPtoolName);
                if (_positioningSystem.BondZMovetoSafeLocation()
                    //&& _positioningSystem.PPtoolMovetoEutecticTableCenter(PPtool)
                    && _positioningSystem.MoveAixsToStageCoord(PPtool.StageAxisTheta, -targetA, EnumCoordSetType.Relative) == StageMotionResult.Success)
                {
                    double[] Target = new double[2];
                    Target[0] = _systemConfig.PositioningConfig.EutecticWeldingLocation.X + ProductExecutor.Instance.OffsetBeforeEutecticChip.X;
                    Target[1] = _systemConfig.PositioningConfig.EutecticWeldingLocation.Y + ProductExecutor.Instance.OffsetBeforeEutecticChip.Y;
                    if (PPtool != null)
                    {
                        Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X - PPtool.LookuptoPPOrigion.X);
                        Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y - PPtool.LookuptoPPOrigion.Y);
                    }
                    else
                    {
                        Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X - _systemConfig.PositioningConfig.LookupChipPPOrigion.X);
                        Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y - _systemConfig.PositioningConfig.LookupChipPPOrigion.Y);
                    }
                    EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                    multiAxis[0] = EnumStageAxis.BondX;
                    multiAxis[1] = EnumStageAxis.BondY;
                    _positioningSystem.MoveAixsToStageCoord(multiAxis, Target, EnumCoordSetType.Absolute);


                    //放芯片
                    var ppParam = CurChipParam.PPSettings;

                    if (PPtool != null)
                    {
                        var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurChipParam.ThicknessMM + CurSubmonutParam.ThicknessMM;
                        ppParam.PPToolZero = PPtool.AltimetryOnMark;
                        ppParam.WorkHeight = (float)systemPos;
                    }
                    else
                    {
                        var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurChipParam.ThicknessMM + CurSubmonutParam.ThicknessMM;
                        ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        ppParam.WorkHeight = (float)systemPos;
                    }

                    if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, null, false))
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "放置衬底到校准台失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                    else
                    {
                        //_positioningSystem.PPMovetoSafeLocation();
                    }


                }



                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_OnlyBondChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }

        }

        public void AfterPlaceChipOnEutecticTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromEutecticTable()
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }


    /*
     * 物料Step 8 - 共晶 步骤Action
     */
    class StepAction_Eutectic : StepActionBase
    {
        public StepAction_Eutectic(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                //LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_Eutectic-Start.");

                //BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                //if (PowerManager.Instance.GetFault())
                //{
                //    PowerManager.Instance.Reset();
                //}
                //IOUtilityHelper.Instance.CloseChipPPVaccum();
                //IOUtilityHelper.Instance.OpenNitrogen();
                //PowerManager.Instance.SetGP(_curRecipe.EutecticParameters.ParameterIndex);
                //PowerManager.Instance.PowerRun();

                //Stopwatch sw = new Stopwatch();

                //int WeldNum = 0;
                //WeldNum = PowerManager.Instance.GetWeldNum();

                //sw.Start();
                ////Thread.Sleep(6000);
                ////Thread.Sleep(22000);
                ////IOUtilityHelper.Instance.OpenNitrogen();
                //while ((!PowerManager.Instance.GetStopSignal()) && (!(PowerManager.Instance.GetWeldNum() > WeldNum)))
                //{
                //    sw.Stop();
                //    if (sw.ElapsedMilliseconds > 60000)
                //    {
                //        PowerManager.Instance.PowerStop();
                //        LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_Eutectic-Timeout.");
                //        return GlobalGWResultDefine.RET_FAILED;
                //    }

                //    sw.Start();
                //    Thread.Sleep(500);

                //}
                //sw.Stop();
                //PowerManager.Instance.PowerStop();
                //IOUtilityHelper.Instance.CloseNitrogen();
                ////Thread.Sleep(5000);
                //IOUtilityHelper.Instance.CloseChipPPVaccum();
                ////IOUtilityHelper.Instance.OpenChipPPBlow();
                ////Thread.Sleep(500);
                ////IOUtilityHelper.Instance.CloseChipPPBlow();
                //LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_Eutectic-End.");


                BeforePickChipFromEutecticTable();

                Thread.Sleep(5000);


                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                IOUtilityHelper.Instance.CloseNitrogen();
                IOUtilityHelper.Instance.CloseChipPPBlow();
                PowerManager.Instance.PowerStop();
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PubDownChip,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {

            }
        }

        public void AfterPlaceChipOnEutecticTable()
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }
        public void BeforePickChipFromEutecticTable()
        {
            IOUtilityHelper.Instance.CloseChipPPVaccum();
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }
    }

    /*
     * 下料 步骤Action
     */
    class StepAction_BlankComponent : StepActionBase
    {
        public StepAction_BlankComponent(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }
        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                //LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BlankComponent-Start.");
                //BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                //IOUtilityHelper.Instance.CloseChipPPVaccum();
                //IOUtilityHelper.Instance.OpenChipPPBlow();
                //Thread.Sleep(_systemConfig.JobConfig.BreakVaccumDelayMsAfterEutectic);
                //IOUtilityHelper.Instance.CloseChipPPBlow();
                ////Thread.Sleep(2000);
                //BondRecipe curRecipe = ProductExecutor.Instance.ProductRecipe;


                ////先慢速上升3mm
                //var axisConfig = HardwareConfiguration.Instance.StageConfig.AxisConfigList.FirstOrDefault(i => i.Type == EnumStageAxis.BondZ);
                //_positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, 5);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 3, EnumCoordSetType.Relative);

                ////快速上升到安全位
                //_positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, (float)axisConfig.AxisSpeed);

                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                //_positioningSystem.PPMovetoSafeLocation();

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                ////改成联动

                //var tempX = _systemConfig.PositioningConfig.EutecticWeldingLocation.X + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.X - curRecipe.BlankingParameters.PickPosition.X;
                //var tempY = _systemConfig.PositioningConfig.EutecticWeldingLocation.Y + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y + curRecipe.BlankingParameters.PickPosition.Y;
                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, tempX, EnumCoordSetType.Absolute);
                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, tempY, EnumCoordSetType.Absolute);

                //_positioningSystem.BondXYUnionMovetoStageCoor(tempX, tempY, EnumCoordSetType.Absolute);
                ////衬底吸嘴放下衬底
                //ProductExecutor.Instance.ProductRecipe.SubstrateInfos.PPSettings.WorkHeight = ProductExecutor.Instance.ProductRecipe.SubstrateInfos.SubmountPPPlacePos;
                ////增加共晶台开真空
                //if (PPUtility.Instance.PickViaSystemCoor(ProductExecutor.Instance.ProductRecipe.SubstrateInfos.PPSettings, false, new Action(() => BlankingSubmountAction())))
                //{
                //    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, targetA, EnumCoordSetType.Relative);
                //}
                //else
                //{
                //    IOUtilityHelper.Instance.CloseSubmountPPVaccum();
                //    LogRecorder.ProductionLog(EnumLogContentType.Error, "下料失败！");
                //    WarningBox.FormShow("错误", "下料失败！");
                //    return GlobalGWResultDefine.RET_FAILED;
                //}
                ////衬底吸嘴复位
                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                //_positioningSystem.PPMovetoSafeLocation();
                ////-----通过物料名取物料对象，取物料首位置 X Y Z  begin -----
                ////double X = ProductExecutor.Instance.Substrate.FirstSubmountLocation.X;
                ////double Y = ProductExecutor.Instance.Substrate.FirstSubmountLocation.Y;

                //double X = ProductExecutor.Instance.AllSubstrate[ProductExecutor.Instance.CurSubstrateNum - 1].MaterialLocation.X;
                //double Y = ProductExecutor.Instance.AllSubstrate[ProductExecutor.Instance.CurSubstrateNum - 1].MaterialLocation.Y;
                ////-----通过物料名取物料对象，取物料首位置 X Y Z    end -----
                ////吸嘴移动到衬底中心上方
                //var offset = _systemConfig.PositioningConfig.PP2AndBondCameraOffset;
                ////移动bond头
                ////_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, X - offset.X, EnumCoordSetType.Absolute);
                ////_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, Y + offset.Y, EnumCoordSetType.Absolute);
                //_positioningSystem.BondXYUnionMovetoSystemCoor(X - offset.X + curRecipe.BlankingParameters.PickPosition.X, Y + offset.Y + curRecipe.BlankingParameters.PickPosition.Y, EnumCoordSetType.Absolute);

                ////放衬底，TBD-此处的高度应该用吸嘴工具和物料参数计算（SubmountPPPickPos高度为跟吸嘴工具相关的Z坐标系的值，所以在PPutility里面，移动方法应调用ChipMove）
                //_curRecipe.SubstrateInfos.PPSettings.WorkHeight = _curRecipe.SubstrateInfos.SubmountPPPickPos;
                //if (PPUtility.Instance.PlaceViaSystemCoor(_curRecipe.SubstrateInfos.PPSettings))
                //{
                //    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BlankComponent-End.");
                //}
                //else
                //{
                //    IOUtilityHelper.Instance.CloseSubmountPPVaccum();
                //    _positioningSystem.PPMovetoSafeLocation();
                //    LogRecorder.ProductionLog(EnumLogContentType.Error, "放衬底失败！");
                //    WarningBox.FormShow("错误", "放衬底失败！");
                //    return GlobalGWResultDefine.RET_FAILED;
                //}

                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_BondSubmonutToEutectic-Start.");

                CameraWindowGUI.Instance?.SelectCamera(0);
                LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_AccuracyPositionSubmonutInCalibrationTable-Start.");
                //var materialOrigionA_init = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                //var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA_init;
                var PPtool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                if (_positioningSystem.BondZMovetoSafeLocation()
                    ////&& _positioningSystem.PPtoolMovetoEutecticTableCenter(PPtool)
                    //&& _positioningSystem.MoveAixsToStageCoord(PPtool.StageAxisTheta, -targetA, EnumCoordSetType.Relative) == StageMotionResult.Success
                    )
                {
                    double[] Target = new double[2];
                    Target[0] = _systemConfig.PositioningConfig.EutecticWeldingLocation.X + ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.X;
                    Target[1] = _systemConfig.PositioningConfig.EutecticWeldingLocation.Y + ProductExecutor.Instance.OffsetAfterSubmonutAccuracy.Y;
                    if (PPtool != null)
                    {
                        Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X - PPtool.LookuptoPPOrigion.X);
                        Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y - PPtool.LookuptoPPOrigion.Y);
                    }
                    else
                    {
                        Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X - _systemConfig.PositioningConfig.LookupChipPPOrigion.X);
                        Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y - _systemConfig.PositioningConfig.LookupChipPPOrigion.Y);
                    }
                    EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                    multiAxis[0] = EnumStageAxis.BondX;
                    multiAxis[1] = EnumStageAxis.BondY;
                    _positioningSystem.MoveAixsToStageCoord(multiAxis, Target, EnumCoordSetType.Absolute);


                    //放芯片
                    var ppParam = CurSubmonutParam.PPSettings;

                    if (PPtool != null)
                    {
                        var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurSubmonutParam.ThicknessMM;
                        ppParam.PPToolZero = PPtool.AltimetryOnMark;
                        ppParam.WorkHeight = (float)systemPos;
                    }
                    else
                    {
                        var systemPos = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z + CurSubmonutParam.ThicknessMM;
                        ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        ppParam.WorkHeight = (float)systemPos;
                    }

                    if (!PPUtility.Instance.PickViaSystemCoor(ppParam, BlankingSubmountAction))
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.ProductionLog(EnumLogContentType.Error, "从共晶台吸取衬底失败！");
                        return GlobalGWResultDefine.RET_FAILED;
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();

                        BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;
                        var curDealBP = CurBondPosition;
                        var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.PPSettings.PPtoolName);
                        if (CurSubmonutParam.CarrierType == EnumCarrierType.WafflePack)
                        {
                            LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickUpSubmonut-Start.");
                            //BondRecipe _curRecipe = ProductExecutor.Instance.ProductRecipe;

                            var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;
                            //安全位
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                            //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                            //吸嘴移动到芯片中心上方

                            var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                            offset = pptool.PP1AndBondCameraOffset;
                            if (pptool != null)
                            {
                                var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                                offset.X = usedPPandBondCameraOffsetX;
                                offset.Y = usedPPandBondCameraOffsetY;
                            }
                            double X = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.X + ProductExecutor.Instance.MaterialSubmonutLocationOffsetX;
                            double Y = CurSubmonutParam.ComponentMapInfos[ProductExecutor.Instance.CurSubmonutNum - 1].MaterialLocation.Y + ProductExecutor.Instance.MaterialSubmonutLocationOffsetY;

                            if (_positioningSystem.BondZMovetoSafeLocation()
                            //芯片吸嘴T复位
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.BondXYUnionMovetoStageCoor(ProductExecutor.Instance.OffsetBeforePickupSubmonut.X + offset.X
                            //    , ProductExecutor.Instance.OffsetBeforePickupSubmonut.Y + offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                            &&  _positioningSystem.BondXYUnionMovetoStageCoor(ProductExecutor.Instance.CurrectPickupSubmonut.X,ProductExecutor.Instance.CurrectPickupSubmonut.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                                var pp = CurSubmonutParam.PPSettings;


                                if (pptool != null)
                                {
                                    var systemPos = CurSubmonutParam.ChipPPPickSystemPos;
                                    pp.PPToolZero = pptool.AltimetryOnMark;
                                    pp.WorkHeight = systemPos;
                                }
                                else
                                {
                                    var systemPos = CurSubmonutParam.ChipPPPickSystemPos;
                                    pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                    pp.WorkHeight = systemPos;
                                }

                                if (PPUtility.Instance.PlaceViaSystemCoor(pp,null,null,true))
                                {
                                    
                                    LogRecorder.ProductionLog(EnumLogContentType.Info, "StepAction_PickDownSubmonut-End.");
                                }
                                else
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "放下衬底失败！");
                                    WarningBox.FormShow("错误", "放下衬底失败！");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "放下衬底失败！");
                                WarningBox.FormShow("错误", "放下衬底失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else if (CurSubmonutParam.CarrierType == EnumCarrierType.Wafer)
                        {
                            var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedESToolName);
                            if (usedESTool != null)
                            {
                                var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                                offset = pptool.PP1AndBondCameraOffset;
                                if (pptool != null)
                                {
                                    var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                    var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                                    offset.X = usedPPandBondCameraOffsetX;
                                    offset.Y = usedPPandBondCameraOffsetY;
                                }
                                var offsetBCAndWC = _systemConfig.PositioningConfig.WaferCameraOrigion;
                                var offsetBCAndWC2 = usedESTool.BondIdentifyNeedleCenter;
                                if (_positioningSystem.BondZMovetoSafeLocation()
                                //顶针移动到零点
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                //物料中心移动到顶针上方
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) == StageMotionResult.Success
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                                //芯片吸嘴物料中心上方
                                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC.X - usedESTool.NeedleCenter.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC.Y - usedESTool.NeedleCenter.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC2.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC2.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                //顶针座升起
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, CurSubmonutParam.ESBaseWorkPos, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                //拾取芯片
                                //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                )
                                {
                                    //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                                    var pp = CurSubmonutParam.PPSettings;
                                    pp.WorkHeight = CurSubmonutParam.ChipPPPickSystemPos + (float)curDealBP.chipPositionCompensation.Z; ;
                                    //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedPPToolName);
                                    if (pptool != null)
                                    {
                                        pp.PPToolZero = pptool.AltimetryOnMark;
                                    }
                                    else
                                    {
                                        //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                        //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                        //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                        //var ppWorkSystemPos = 0f;
                                        pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                        //pp.WorkHeight = ppWorkSystemPos;
                                    }

                                    IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();
                                    //Thread.Sleep(3000);
                                    var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                    var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;
                                    if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                                    {
                                    }
                                    else
                                    {
                                        IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                        LogRecorder.ProductionLog(EnumLogContentType.Error, "放下芯片失败！");
                                        WarningBox.FormShow("错误", "放下芯片失败！");
                                        return GlobalGWResultDefine.RET_FAILED;
                                    }
                                }
                                else
                                {
                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "放下芯片失败！");
                                    WarningBox.FormShow("错误", "放下芯片失败！");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }

                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }
                        else if (CurSubmonutParam.CarrierType == EnumCarrierType.WaferWafflePack)
                        {
                            //芯片吸嘴物料中心上方
                            var ppSystemOffset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                            ppSystemOffset = pptool.PP1AndBondCameraOffset;
                            if (pptool != null)
                            {
                                var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                                ppSystemOffset.X = usedPPandBondCameraOffsetX;
                                ppSystemOffset.Y = usedPPandBondCameraOffsetY;
                            }
                            var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;



                            if (_positioningSystem.BondZMovetoSafeLocation()
                            //芯片吸嘴物料中心上方
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X+ ProductExecutor.Instance.OffsetBeforePickupSubmonut.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y- ProductExecutor.Instance.OffsetBeforePickupSubmonut.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            ////拾取芯片
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            )
                            {
                                //XY联动移动到物料上方
                                EnumStageAxis[] multiAxis2 = new EnumStageAxis[3];
                                multiAxis2[0] = EnumStageAxis.BondX;
                                multiAxis2[1] = EnumStageAxis.BondY;
                                //multiAxis[2] = EnumStageAxis.SubmonutPPT;
                                multiAxis2[2] = pptool.StageAxisTheta;
                                double[] targets = new double[3];
                                targets[0] = ppSystemOffset.X + bondcamera2wafercamera.X + curDealBP.chipPositionCompensation.X;
                                targets[1] = ppSystemOffset.Y + bondcamera2wafercamera.Y + curDealBP.chipPositionCompensation.Y;
                                targets[2] = 0;
                                StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis2, targets, EnumCoordSetType.Absolute);
                                if (result == StageMotionResult.Success)
                                {

                                }
                                else
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "拾取芯片失败！");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }

                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,BondXTarget:{ppSystemOffset.X + bondcamera2wafercamera.X}");
                                LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,BondXCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX)}");
                                //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                                var pp = CurSubmonutParam.PPSettings;
                                pp.WorkHeight = CurSubmonutParam.ChipPPPickSystemPos + (float)curDealBP.chipPositionCompensation.Z;

                                //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurSubmonutParam.RelatedPPToolName);
                                if (pptool != null)
                                {
                                    //吸嘴工具原点
                                    pp.PPToolZero = pptool.AltimetryOnMark;

                                }
                                else
                                {
                                    //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                    //var ppWorkSystemPos = _systemConfig.PositioningConfig.PPESAltimetryParameter.PPSystemPosition - (ProductExecutor.Instance.ProductRecipe.CurrentComponent.ESBaseWorkPos - pptool.PPESAltimetryParameter.ESStagePosition)
                                    //+ ProductExecutor.Instance.ProductRecipe.CurrentComponent.ThicknessMM + ProductExecutor.Instance.ProductRecipe.CurrentComponent.CarrierThicknessMM;
                                    //var ppWorkSystemPos = 0f;
                                    pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                    //pp.WorkHeight = ppWorkSystemPos;
                                }


                                //var materialOrigionA = CurSubmonutParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                                //var targetA = ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta - materialOrigionA;
                                //LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpSubmonutWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupSubmonut.Theta}");
                                //LogRecorder.ProductionLog(EnumLogContentType.Error, $"StepAction_PickUpSubmonutWithRotate-targetAngle:{targetA}");
                                if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
                                {
                                    //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT)}");
                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmonutPPT, -targetA, EnumCoordSetType.Relative);
                                    //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpSubmonutWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.SubmonutPPT)}");
                                }
                                else
                                {
                                    LogRecorder.ProductionLog(EnumLogContentType.Error, "放下芯片失败！");
                                    return GlobalGWResultDefine.RET_FAILED;
                                }
                            }
                            else
                            {
                                LogRecorder.ProductionLog(EnumLogContentType.Error, "放下芯片失败！");
                                return GlobalGWResultDefine.RET_FAILED;
                            }
                        }



                    }


                }


                return GlobalGWResultDefine.RET_SUCCESS;

            }
            catch (Exception ex)
            {
                LogRecorder.ProductionLog(EnumLogContentType.Error, "StepAction_PutDownSubstrate,Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //PositioningSystem.Instance.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, SystemConfiguration.Instance.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //PositioningSystem.Instance.MoveAxisToSystemCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.PPMovetoSafeLocation();
            }
            //抬起
        }

        /// <summary>
        /// 放衬底时开启共晶台吸气
        /// </summary>
        private void BlankingSubmountAction()
        {
            IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();
        }
        
    }

    //------------------------------  不在步骤内的一些处理特殊情况的Action ------------------------------------

    /*
     * 抛料substrate动作
     */
    class ExAction_AbondonSubstrate : StepActionBase
    {
        public ExAction_AbondonSubstrate(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.SubmountPP);
            }
            catch (Exception)
            {

                //throw;
            }

            return GlobalGWResultDefine.RET_SUCCESS;
        }
    }

    /*
     * 抛料chip动作
     */
    class ExAction_AbondonChip : StepActionBase
    {
        public ExAction_AbondonChip(ProductStep step, EnumActionNo actionNo, string actionDesc) : base(step, actionNo, actionDesc) { }

        public override GWResult Run(RunParameter runParam = null)
        {
            try
            {
                AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.ChipPP);
            }
            catch (Exception)
            {

                //throw;
            }

            return GlobalGWResultDefine.RET_SUCCESS;
        }
    }

}
