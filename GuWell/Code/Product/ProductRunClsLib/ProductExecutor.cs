using CameraControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using JobClsLib;
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
    public class ProductExecutor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static volatile ProductExecutor _instance = new ProductExecutor();
        private static readonly object _lockObj = new object();
        internal EventWaitHandle _eventWaitAsysncPositionChipSignal = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal EventWaitHandle _eventWaitAsysncPositionChipComplete = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal EventWaitHandle _eventWaitForNextAction = new EventWaitHandle(false, EventResetMode.AutoReset);

        public static ProductExecutor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ProductExecutor();
                        }
                    }
                }
                return _instance;
            }
        }
        protected PositioningSystem _positionSystem
        {
            get
            {
                return PositioningSystem.Instance;
            }
        }
        public bool SingleStepRun { get; set; }
        //主配方
        public BondRecipe ProductRecipe { get; set; }

        //该配方所有的基板
        public List<MaterialMapInformation> AllSubstrate;

        public int MaxSubNum 
        { 
            get {
                return AllSubstrate.Count;
            } 
        }

        //基板Map中当前选的那一个的信息
        private int curSubstrateNum = 1;
        public int CurSubstrateNum
        {
            get
            {
                return curSubstrateNum;
            }
            set
            {
                curSubstrateNum = value;
                //this.SetSubNumActions();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }
        private int curChipNum = 1;
        public int CurChipNum
        {
            get
            {
                return curChipNum;
            }
            set
            {
                curChipNum = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }

        private int curChipNGNum = 0;
        /// <summary>
        /// 当前NG芯片数量
        /// </summary>
        public int CurChipNGNum
        {
            get
            {
                return curChipNGNum;
            }
            set
            {
                curChipNGNum = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }

        private double materialLocationOffsetX = 0;
        /// <summary>
        /// 物料起点X偏移
        /// </summary>
        public double MaterialLocationOffsetX
        {
            get
            {
                return materialLocationOffsetX;
            }
            set
            {
                materialLocationOffsetX = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }

        private double materialLocationOffsetY = 0;
        /// <summary>
        /// 物料起点Y偏移
        /// </summary>
        public double MaterialLocationOffsetY
        {
            get
            {
                return materialLocationOffsetY;
            }
            set
            {
                materialLocationOffsetY = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }

        private double curmaterialLocationX = 0;
        /// <summary>
        /// 当前物料起点X
        /// </summary>
        public double curMaterialLocationX
        {
            get
            {
                return curmaterialLocationX;
            }
            set
            {
                curmaterialLocationX = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }

        private double curmaterialLocationY = 0;
        /// <summary>
        /// 当前物料起点Y
        /// </summary>
        public double curMaterialLocationY
        {
            get
            {
                return curmaterialLocationY;
            }
            set
            {
                curmaterialLocationY = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }

        private int curModuleNum = 1;
        public int CurModuleNum
        {
            get
            {
                return curModuleNum;
            }
            set
            {
                curModuleNum = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurSubNum"));
            }
        }
        private int _bondDieCounter = 1;
        public int BondDieCounter
        {
            get
            {
                return _bondDieCounter;
            }
            set
            {
                _bondDieCounter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BondCounter"));
            }
        }
        public int ManualSettedStartIndex
        {
            get;
            set;
        }
        public int ManualSettedProcessCount
        {
            get;
            set;
        }
        public bool IsProcessPart
        {
            get;
            set;
        }
        
        //当前基底号的信息
        public MaterialMapInformation CurSubstrateInfo;

        //该配方用的基板组的通用参数
        public ProgramSubstrateSettings Substrate { get; set; }

        public XYZTCoordinateConfig SubstrateCoordinateHomePoint { get; set; }
        public XYZTCoordinateConfig SubstrateCoordinateHomeSecondPoint { get; set; }

        //配方中所含的步骤（可包含多芯片和一个共晶）
        public List<ProductStep> ProductSteps { get; set; }

        //选择中的基底对应的动作组
        public List<StepActionBase> StepActions { get; set; }

        //正在运行或下一步运行的 动作indecx
        public int RunningActionIndex;

        //当前动作组中指定运行的动作
        public int RedoStepIndex = -1;

        //选择不同基底时对应的动作组
        public Dictionary<int, List<StepActionBase>> ActionGroups;
        //所有基底组成的所有动作List
        public List<StepActionBase> AllActions;

        //运行状态
        private EnumProductRunStat runStat;
        public EnumProductRunStat RunStat 
        { get
            {
                return runStat;
            }

            set
            {
                runStat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RunStat"));
            }
        }

        //吸取基底前校准结果
        public XYZTCoordinateConfig OffsetBeforePickupSubstrate { get; set; }

        //吸取芯片前校准结果
        public XYZTCoordinateConfig OffsetBeforePickupChip { get; set; }
        public float CompensateXAfterPickupChip { get; set; }
        public float CompensateYAfterPickupChip { get; set; }

        //贴装芯片前校准结果
        public XYZTCoordinateConfig OffsetBeforeBondChip { get; set; }

        //贴装芯片前校准结果
        public XYZTCoordinateConfig OffsetAfterChipAccuracy { get; set; }

        public int PickingChipNo = 0;
        public int NextChipNo = 0;

        public bool IsActionRunning = false;

        public ProductExecutor()
        {
            AllSubstrate = null;
            CurSubstrateInfo = null;
            ProductRecipe = null;
            Substrate = null;
            ProductSteps = new List<ProductStep>();
            RunStat = EnumProductRunStat.Stop;
            StepActions = new List<StepActionBase>();
            RunningActionIndex = 0;
            AllActions = new List<StepActionBase>();
            OffsetAfterChipAccuracy = new XYZTCoordinateConfig();
            OffsetBeforeBondChip = new XYZTCoordinateConfig();
            OffsetBeforePickupChip = new XYZTCoordinateConfig();

            SubstrateCoordinateHomePoint = new XYZTCoordinateConfig();
            SubstrateCoordinateHomeSecondPoint = new XYZTCoordinateConfig();
        }

        //加载要运行的生产配方
        public void LoadProductRecipe(BondRecipe recipe)
        {
            RunningActionIndex = 0;
            RedoStepIndex = -1;
            ProductSteps.Clear();
            StepActions.Clear();

            if (recipe == null)
            {
                RunStat = EnumProductRunStat.NoProd;
                return;
            }
            
            ProductRecipe = recipe;
            Substrate = ProductRecipe.SubstrateInfos;

            //AllSubstrate = ProductRecipe.SubstrateInfos.SubstrateMapInfos;
            //CurSubstrateInfo = AllSubstrate[0];

            ProductSteps = recipe.ProductSteps;

            //if (ProductSteps == null || ProductSteps.Count == 0)
            //{
            //    return;
            //}

            //RunStat = EnumProductRunStat.Stop;

            //SetSubNumActions();
        }

        //***********************************************************************************************************************************
        //***********************************************************************************************************************************
        //
        //********************                                以下为不同基底Num 的动作选择                       *******************************
        //
        //***********************************************************************************************************************************
        //***********************************************************************************************************************************
        public void SetSubNumActions()
        {
            if (curSubstrateNum < 1 || curSubstrateNum > MaxSubNum)
            {
                return;
            }

            int perActNum = AllActions.Count / AllSubstrate.Count;
            int sNum = (this.curSubstrateNum - 1) * perActNum;

            StepActions.Clear();
            StepActions = AllActions.GetRange(sNum, perActNum);
            return;
        }

        /*
         * 开始前先移动安全位
         */
        public void MoveToSafePos()
        {

            StepAction_MoveSafePos stepAction_MoveSafePos = new StepAction_MoveSafePos(null, EnumActionNo.Action_MoveSafePos, "移动到安全位置");
            stepAction_MoveSafePos.Run();
            IOUtilityHelper.Instance.CloseChipPPVaccum();
            Thread.Sleep(10);
            IOUtilityHelper.Instance.CloseSubmountPPVaccum();
            Thread.Sleep(10);
            IOUtilityHelper.Instance.OpenChipPPBlow();
            Thread.Sleep(100);
            IOUtilityHelper.Instance.CloseChipPPBlow();
            Thread.Sleep(10);
            IOUtilityHelper.Instance.OpenSubmountPPBlow();
            Thread.Sleep(100);
            IOUtilityHelper.Instance.CloseSubmountPPBlow();
            Thread.Sleep(10);
            IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();
        }

        //***********************************************************************************************************************************
        //***********************************************************************************************************************************
        //
        //********************                                以下执行动作                       *********************************************
        //
        //***********************************************************************************************************************************
        //***********************************************************************************************************************************
        public void StartRun()
        {
            PrepareRun();
            BeforeRun();
            
        }
        private bool _isBPInvalid = false;
        public void PrepareRun()
        {
            ProductExecutor.Instance.MaterialLocationOffsetX = 0;
            ProductExecutor.Instance.MaterialLocationOffsetY = 0;
            _positionSystem.StageMovetoPrepareLocationForJob();
        }

        //public void Transpotmothed()
        //{
        //    IOUtilityHelper.Instance.TurnonTowerGreenLight();
        //    IOUtilityHelper.Instance.TurnoffTowerYellowLight();
        //    if (_positionSystem.BondMovetoSafeLocation())
        //    {
        //        curSubstrateNum = 1;
        //        curModuleNum = 1;
        //        BondDieCounter = 0;
        //        ProductExecutor.Instance.CurChipNum = 1;
        //        Task.Factory.StartNew(new Action(() =>
        //        {
        //            EnumJobRunStatus currentJobStatus = EnumJobRunStatus.Initial;
        //            //while (true)
        //            //{

        //            //填充 芯片相关的动作 （有可能是多芯片）
        //            foreach (ProductStep step in ProductSteps)
        //            {
        //                ProductRecipe.CurrentComponentInfosName = step.ComponentName;
        //                ProductRecipe.CurrentBondPositionSettingsName = step.BondingPositionName;
        //                if (step.productStepType == EnumProductStepType.Dispense)
        //                {
        //                    while (true && RunStat != EnumProductRunStat.Stop && RunStat != EnumProductRunStat.UserAbort)
        //                    {
        //                        var ret = GlobalGWResultDefine.RET_SUCCESS;
        //                        if (currentJobStatus == EnumJobRunStatus.DispenseAllPosSuccess || currentJobStatus == EnumJobRunStatus.DispenseAllPosFail)
        //                        {
        //                            break;
        //                        }

        //                        switch (currentJobStatus)
        //                        {

        //                            case EnumJobRunStatus.Initial:
        //                                StepAction_PositionSubstrate stepAction_PositionSubstrate = new StepAction_PositionSubstrate(step, EnumActionNo.Action_PositionSubstrate, "基板定位");
        //                                ret = stepAction_PositionSubstrate.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllSubstrateSuccess : EnumJobRunStatus.PositionAllSubstrateFail;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.PositionAllSubstrateSuccess:
        //                                if (ProductRecipe.SubstrateInfos.IsPositionModules)
        //                                {
        //                                    ResetEventWaitForNext();
        //                                    StepAction_PositionModule stepAction_PositionModule = new StepAction_PositionModule(step, EnumActionNo.Action_PositionModule, "芯片定位");
        //                                    ret = stepAction_PositionModule.Run();
        //                                    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllModuleSuccess : EnumJobRunStatus.PositionAllModuleFail;
        //                                    WaitForNext();
        //                                }
        //                                else
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
        //                                }
        //                                break;
        //                            case EnumJobRunStatus.PositionAllSubstrateFail:
        //                                //弹窗提示
        //                                if (WarningBox.FormShow("异常发生！", "基板对位失败！", "警报") == 1)
        //                                {
        //                                    //重试
        //                                    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                }
        //                                else
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                }
        //                                break;
        //                            case EnumJobRunStatus.PositionOneModuleSuccess:
        //                                break;
        //                            case EnumJobRunStatus.PositionOneModuleFail:
        //                                break;
        //                            case EnumJobRunStatus.PositionAllModuleSuccess:
        //                                ResetEventWaitForNext();
        //                                StepAction_PositionBondPos stepAction_PositionBP = new StepAction_PositionBondPos(step, EnumActionNo.Action_RecognizeBondPos, "定位贴装位置");
        //                                ret = stepAction_PositionBP.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllBondPosSuccess : EnumJobRunStatus.PositionAllBondPosFail;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.PositionAllModuleFail:
        //                                //弹窗提示
        //                                if (WarningBox.FormShow("异常发生！", "模组对位失败！", "警报") == 1)
        //                                {
        //                                    //重试
        //                                    currentJobStatus = EnumJobRunStatus.PositionAllSubstrateSuccess;
        //                                }
        //                                else
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                }
        //                                break;
        //                            case EnumJobRunStatus.PositionOneBondPosSuccess:
        //                                break;
        //                            case EnumJobRunStatus.PositionOneBondPosFail:
        //                                break;
        //                            case EnumJobRunStatus.PositionAllBondPosSuccess:
        //                                this.CompletedJob();
        //                                WarningBox.FormShow("流程完成！", "流程正常结束！", "提示");
        //                                return;

        //                                //ResetEventWaitForNext();
        //                                //StepAction_Dispense stepAction_Dispense = new StepAction_Dispense(step, EnumActionNo.Action_Dispense, "划胶");
        //                                //ret = stepAction_Dispense.Run();
        //                                //currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.DispenseAllPosSuccess : EnumJobRunStatus.DispenseAllPosFail;
        //                                //WaitForNext();
        //                                //break;
        //                            case EnumJobRunStatus.PositionAllBondPosFail:
        //                                //弹窗提示
        //                                if (WarningBox.FormShow("异常发生！", "贴装位置对位失败！", "警报") == 1)
        //                                {
        //                                    //重试
        //                                    currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
        //                                }
        //                                else
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                }
        //                                break;
        //                            case EnumJobRunStatus.DispenseOnePosSuccess:
        //                                break;
        //                            case EnumJobRunStatus.DispenseOnePosFail:
        //                                break;
        //                            case EnumJobRunStatus.Aborted:
        //                                this.AbortJob();
        //                                WarningBox.FormShow("流程终止！", "流程异常终止！", "提示");
        //                                return;
        //                                //case EnumJobRunStatus.DispenseAllPosSuccess:
        //                                //    ResetEventWaitForNext();
        //                                //    StepAction_PositionComponent stepAction_PositionChip = new StepAction_PositionComponent(null, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                //    ret = stepAction_PositionChip.Run();
        //                                //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
        //                                //    WaitForNext();
        //                                //    break;
        //                                //case EnumJobRunStatus.DispenseAllPosFail:
        //                                //    //弹窗提示
        //                                //    if (WarningBox.FormShow("异常发生！", "点胶失败！", "警报") == 1)
        //                                //    {
        //                                //        //重试
        //                                //        currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
        //                                //    }
        //                                //    else
        //                                //    {
        //                                //        currentJobStatus = EnumJobRunStatus.Aborted;
        //                                //    }
        //                                //    break;
        //                                //case EnumJobRunStatus.PositionChipSuccess:
        //                                //    ResetEventWaitForNext();
        //                                //    StepAction_PickUpChip stepAction_PickChip = new StepAction_PickUpChip(null, EnumActionNo.Action_PositionChip, "拾取芯片");
        //                                //    ret = stepAction_PickChip.Run();
        //                                //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
        //                                //    ProductExecutor.Instance.CurChipNum++;
        //                                //    WaitForNext();
        //                                //    break;
        //                                //case EnumJobRunStatus.PositionChipFail:
        //                                //    //芯片对位失败时自动跳到下一颗
        //                                //    ProductExecutor.Instance.CurChipNum++;
        //                                //    break;
        //                                //case EnumJobRunStatus.PickupChipSuccess:
        //                                //    //进行二次校准同时识别下一颗芯片
        //                                //    Task.Factory.StartNew(() => {
        //                                //        _eventWaitAsysncPositionChipSignal.WaitOne();
        //                                //        ResetEventWaitForNext();
        //                                //        StepAction_PositionComponent stepAction_AsyncPositionChip = new StepAction_PositionComponent(null, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                //        ret = stepAction_AsyncPositionChip.Run();
        //                                //        WaitForNext();
        //                                //        _eventWaitAsysncPositionChipComplete.Set();

        //                                //    });
        //                                //    ResetEventWaitForNext();
        //                                //    StepAction_AccuracyPositionChip stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionChip(null, EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
        //                                //    ret = stepAction_AccuracyCalibrationChip.Run();
        //                                //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.AccuracyCalibrationChipSuccess : EnumJobRunStatus.AccuracyCalibrationChipFail;
        //                                //    WaitForNext();
        //                                //    break;
        //                                //case EnumJobRunStatus.PickupChipFail:
        //                                //    break;
        //                                //case EnumJobRunStatus.AccuracyCalibrationChipSuccess:
        //                                //    ResetEventWaitForNext();
        //                                //    StepAction_BondChip stepAction_BondChip = new StepAction_BondChip(null, EnumActionNo.Action_BondChip, "固晶");
        //                                //    ret = stepAction_BondChip.Run();
        //                                //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.BondChipSuccess : EnumJobRunStatus.BondChipFail;
        //                                //    WaitForNext();
        //                                //    break;
        //                                //case EnumJobRunStatus.AccuracyCalibrationChipFail:
        //                                //    break;
        //                                //case EnumJobRunStatus.BondChipSuccess:
        //                                //    _eventWaitAsysncPositionChipComplete.WaitOne();
        //                                //    ResetEventWaitForNext();
        //                                //    StepAction_PickUpChip stepAction_PickChip2 = new StepAction_PickUpChip(null, EnumActionNo.Action_PositionChip, "拾取芯片");
        //                                //    ret = stepAction_PickChip2.Run();
        //                                //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
        //                                //    WaitForNext();
        //                                //    break;
        //                                //case EnumJobRunStatus.BondChipFail:
        //                                //    //弹窗提示
        //                                //    if (WarningBox.FormShow("异常发生！", "点胶失败！", "警报") == 1)
        //                                //    {
        //                                //        //抛料
        //                                //        currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
        //                                //    }
        //                                //    else
        //                                //    {
        //                                //        currentJobStatus = EnumJobRunStatus.Aborted;
        //                                //    }
        //                                //    break;
        //                                //case EnumJobRunStatus.AbandonChipSuccess:
        //                                //    break;
        //                                //case EnumJobRunStatus.AbandonChipFail:
        //                                //    break;
        //                                //case EnumJobRunStatus.Aborted:
        //                                //    WarningBox.FormShow("流程终止！","流程异常终止！","提示");
        //                                //    return;
        //                                //case EnumJobRunStatus.Completed:
        //                                //    WarningBox.FormShow("流程完成！", "流程正常结束！", "提示");
        //                                //    return;
        //                                //default:
        //                                //    break;
        //                        }
        //                    }

        //                }
        //                else if (step.productStepType == EnumProductStepType.BondDie)
        //                {
        //                    while (true && RunStat != EnumProductRunStat.Stop && RunStat != EnumProductRunStat.UserAbort)
        //                    {
        //                        //if (currentJobStatus == EnumJobRunStatus.Completed || currentJobStatus == EnumJobRunStatus.Aborted)
        //                        //{
        //                        //    break;
        //                        //}
        //                        var ret = GlobalGWResultDefine.RET_SUCCESS;
        //                        switch (currentJobStatus)
        //                        {

        //                            //case EnumJobRunStatus.Initial:
        //                            //    StepAction_PositionSubstrate stepAction_PositionSubstrate = new StepAction_PositionSubstrate(null, EnumActionNo.Action_PositionSubstrate, "基板定位");
        //                            //    ret = stepAction_PositionSubstrate.Run();
        //                            //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllSubstrateSuccess : EnumJobRunStatus.PositionAllSubstrateFail;
        //                            //    WaitForNext();
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionAllSubstrateSuccess:
        //                            //    if (ProductRecipe.SubstrateInfos.IsPositionModules)
        //                            //    {
        //                            //        ResetEventWaitForNext();
        //                            //        StepAction_PositionModule stepAction_PositionModule = new StepAction_PositionModule(null, EnumActionNo.Action_PositionModule, "芯片定位");
        //                            //        ret = stepAction_PositionModule.Run();
        //                            //        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllModuleSuccess : EnumJobRunStatus.PositionAllModuleFail;
        //                            //        WaitForNext();
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
        //                            //    }
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionAllSubstrateFail:
        //                            //    //弹窗提示
        //                            //    if (WarningBox.FormShow("异常发生！", "基板对位失败！", "警报") == 1)
        //                            //    {
        //                            //        //重试
        //                            //        currentJobStatus = EnumJobRunStatus.Initial;
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        currentJobStatus = EnumJobRunStatus.Aborted;
        //                            //    }
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionOneModuleSuccess:
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionOneModuleFail:
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionAllModuleSuccess:
        //                            //    ResetEventWaitForNext();
        //                            //    StepAction_PositionBondPos stepAction_PositionBP = new StepAction_PositionBondPos(null, EnumActionNo.Action_RecognizeBondPos, "定位贴装位置");
        //                            //    ret = stepAction_PositionBP.Run();
        //                            //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllBondPosSuccess : EnumJobRunStatus.PositionAllBondPosFail;
        //                            //    WaitForNext();
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionAllModuleFail:
        //                            //    //弹窗提示
        //                            //    if (WarningBox.FormShow("异常发生！", "模组对位失败！", "警报") == 1)
        //                            //    {
        //                            //        //重试
        //                            //        currentJobStatus = EnumJobRunStatus.PositionAllSubstrateSuccess;
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        currentJobStatus = EnumJobRunStatus.Aborted;
        //                            //    }
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionOneBondPosSuccess:
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionOneBondPosFail:
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionAllBondPosSuccess:
        //                            //    ResetEventWaitForNext();
        //                            //    StepAction_Dispense stepAction_Dispense = new StepAction_Dispense(null, EnumActionNo.Action_Dispense, "划胶");
        //                            //    ret = stepAction_Dispense.Run();
        //                            //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.DispenseAllPosSuccess : EnumJobRunStatus.DispenseAllPosFail;
        //                            //    WaitForNext();
        //                            //    break;
        //                            //case EnumJobRunStatus.PositionAllBondPosFail:
        //                            //    //弹窗提示
        //                            //    if (WarningBox.FormShow("异常发生！", "贴装位置对位失败！", "警报") == 1)
        //                            //    {
        //                            //        //重试
        //                            //        currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        currentJobStatus = EnumJobRunStatus.Aborted;
        //                            //    }
        //                            //    break;
        //                            case EnumJobRunStatus.DispenseOnePosSuccess:
        //                                break;
        //                            case EnumJobRunStatus.DispenseOnePosFail:
        //                                break;
        //                            case EnumJobRunStatus.DispenseAllPosSuccess:
        //                                ResetEventWaitForNext();

        //                                StepAction_PositionComponent stepAction_PositionChip = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                ret = stepAction_PositionChip.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.DispenseAllPosFail:
        //                                //弹窗提示
        //                                if (WarningBox.FormShow("异常发生！", "点胶失败！", "警报") == 1)
        //                                {
        //                                    //重试
        //                                    //currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
        //                                    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                }
        //                                else
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                }
        //                                break;
        //                            case EnumJobRunStatus.PositionChipSuccess:
        //                                ResetEventWaitForNext();
        //                                StepAction_PickUpChipWithRotate stepAction_PickChip = new StepAction_PickUpChipWithRotate(step, EnumActionNo.Action_PositionChip, "拾取芯片");
        //                                ret = stepAction_PickChip.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
        //                                ProductExecutor.Instance.CurChipNum++;
        //                                ProductExecutor.Instance.CurChipNGNum = 0;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.PositionChipFail:
        //                                //芯片对位失败时自动跳到下一颗
        //                                ProductExecutor.Instance.CurChipNum++;

        //                                if (curChipNum > ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Completed;
        //                                    break;
        //                                }
        //                                ProductExecutor.Instance.CurChipNGNum++;
        //                                if (ProductExecutor.Instance.CurChipNGNum > SystemConfiguration.Instance.JobConfig.CurChipNGNumMax)
        //                                {
        //                                    if (CameraWindowGUI.Instance != null)
        //                                    {
        //                                        CameraWindowGUI.Instance.SelectCamera(2);
        //                                        CameraWindowGUI.Instance.ClearGraphicDraw();
        //                                    }
        //                                    if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
        //                                    {
        //                                        CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
        //                                        CameraWindowForm.Instance.Show();
        //                                    }
        //                                    //弹窗提示
        //                                    if (ShowMessage2("异常发生！", "搜寻芯片失败，请重新确定搜寻芯片起点位置！", "警报") == 1)
        //                                    {
        //                                        ProductExecutor.Instance.CurChipNum = 1;
        //                                        ProductExecutor.Instance.CurChipNGNum = 0;
        //                                        double X = _positionSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
        //                                        double Y = _positionSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);
        //                                        ProductExecutor.Instance.MaterialLocationOffsetX = X - ProductRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.X;
        //                                        ProductExecutor.Instance.MaterialLocationOffsetY = Y - ProductRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.Y;
        //                                    }
        //                                    else
        //                                    {
        //                                        currentJobStatus = EnumJobRunStatus.Aborted;
        //                                    }
        //                                }
        //                                ResetEventWaitForNext();
        //                                StepAction_PositionComponent stepAction_PositionChip3 = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                ret = stepAction_PositionChip3.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.PickupChipSuccess:
        //                                //进行二次校准同时识别下一颗芯片
        //                                //Task.Factory.StartNew(() =>
        //                                //{
        //                                //    _eventWaitAsysncPositionChipSignal.WaitOne();
        //                                //    ResetEventWaitForNext();
        //                                //    StepAction_PositionComponent stepAction_AsyncPositionChip = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                //    ret = stepAction_AsyncPositionChip.Run();
        //                                //    WaitForNext();
        //                                //    _eventWaitAsysncPositionChipComplete.Set();

        //                                //});
        //                                ResetEventWaitForNext();
        //                                if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
        //                                {
        //                                    StepAction_AccuracyPositionWithUplookCamera stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionWithUplookCamera(step, EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
        //                                    ret = stepAction_AccuracyCalibrationChip.Run();
        //                                }
        //                                else if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
        //                                {
        //                                    StepAction_AccuracyPositionChipInCalibrationTable stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionChipInCalibrationTable(step, EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
        //                                    ret = stepAction_AccuracyCalibrationChip.Run();
        //                                }
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.AccuracyCalibrationChipSuccess : EnumJobRunStatus.AccuracyCalibrationChipFail;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.PickupChipFail:
        //                                //芯片拾取失败时自动抛料跳到下一颗TBD需增加自动抛料
        //                                //ProductExecutor.Instance.CurChipNum++;
        //                                if (curChipNum > ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Completed;
        //                                    break;
        //                                }
        //                                currentJobStatus = EnumJobRunStatus.BondChipSuccess;
        //                                break;
        //                            case EnumJobRunStatus.AccuracyCalibrationChipSuccess:
        //                                ResetEventWaitForNext();
        //                                //StepAction_BondChip stepAction_BondChip = new StepAction_BondChip(step, EnumActionNo.Action_BondChip, "固晶");
        //                                StepAction_OnlyBondChip stepAction_BondChip = new StepAction_OnlyBondChip(step, EnumActionNo.Action_BondChip, "固晶");
        //                                ret = stepAction_BondChip.Run();
        //                                if (ret == GlobalGWResultDefine.RET_BPInvalid)
        //                                {
        //                                    _isBPInvalid = true;
        //                                }
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.BondChipSuccess : EnumJobRunStatus.BondChipFail;
        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.AccuracyCalibrationChipFail:
        //                                //芯片二次对位失败时自动抛料跳到下一颗TBD需增加自动抛料
        //                                //ProductExecutor.Instance.CurChipNum++;
        //                                if (curChipNum > ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Completed;
        //                                    break;
        //                                }
        //                                if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
        //                                {
        //                                    StepAction_MaterialThrowingAction stepAction_MaterialThrowingAction = new StepAction_MaterialThrowingAction(step, EnumActionNo.Action_AbondonChip, "抛料");
        //                                    ret = stepAction_MaterialThrowingAction.Run();
        //                                }
        //                                else if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
        //                                {
        //                                    StepAction_CalibrationTableMaterialThrowingAction stepAction_MaterialThrowingAction = new StepAction_CalibrationTableMaterialThrowingAction(step, EnumActionNo.Action_AbondonChip, "抛料");
        //                                    ret = stepAction_MaterialThrowingAction.Run();
        //                                }

        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.AbandonChipSuccess : EnumJobRunStatus.AbandonChipFail;
        //                                if (currentJobStatus == EnumJobRunStatus.AbandonChipFail)
        //                                {
        //                                    if (ShowMessage2("异常发生！", "抛料失败，请手动去除吸嘴上的芯片！清除无误后点击<确认>按钮后流程继续。", "警报") == 1)
        //                                    {

        //                                    }
        //                                    else
        //                                    {
        //                                        currentJobStatus = EnumJobRunStatus.Aborted;
        //                                    }
        //                                }

        //                                //抛料成功后
        //                                StepAction_PositionComponent stepAction_PositionChip2 = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                ret = stepAction_PositionChip2.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
        //                                //currentJobStatus = EnumJobRunStatus.BondChipSuccess;
        //                                break;
        //                            case EnumJobRunStatus.BondChipSuccess:
        //                                //_eventWaitAsysncPositionChipComplete.WaitOne();
        //                                //成功之后更新当前SubstrateNumber和ModuleNumber
        //                                BondDieCounter++;
        //                                var moudleCountInOneSubstrate = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count;
        //                                var bpCountInOneModule = ProductRecipe.StepBondingPositionList.Count;
        //                                var allMoudleCounts = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count;
        //                                var allBPCounts = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count * bpCountInOneModule;
        //                                curModuleNum = _bondDieCounter % moudleCountInOneSubstrate + 1;
        //                                curSubstrateNum = _bondDieCounter / moudleCountInOneSubstrate + 1;

        //                                if (_bondDieCounter >= allBPCounts)
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Completed;
        //                                    break;
        //                                }
        //                                //多芯片场景
        //                                if (_bondDieCounter == allMoudleCounts && ProductRecipe.StepBondingPositionList.Count > 1)
        //                                {
        //                                    _bondDieCounter = 0;
        //                                    curModuleNum = 1;
        //                                    curSubstrateNum = 1;
        //                                }
        //                                ResetEventWaitForNext();
        //                                //StepAction_PickUpChip stepAction_PickChip2 = new StepAction_PickUpChip(step, EnumActionNo.Action_PositionChip, "拾取芯片");
        //                                //ret = stepAction_PickChip2.Run();
        //                                //currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
        //                                if (curChipNum >= ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.Completed;
        //                                    break;
        //                                }
        //                                if (ProductExecutor.Instance.IsProcessPart)
        //                                {
        //                                    if (_bondDieCounter >= ProductExecutor.Instance.ManualSettedProcessCount)
        //                                    {
        //                                        currentJobStatus = EnumJobRunStatus.Completed;
        //                                        break;
        //                                    }
        //                                }
        //                                StepAction_PositionComponent stepAction_PositionChip4 = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
        //                                ret = stepAction_PositionChip4.Run();
        //                                currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;

        //                                WaitForNext();
        //                                break;
        //                            case EnumJobRunStatus.BondChipFail:

        //                                BondDieCounter++;
        //                                var moudleCountInOneSubstrate1 = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count;
        //                                var bpCountInOneModule1 = ProductRecipe.StepBondingPositionList.Count;
        //                                var allMoudleCounts1 = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count;
        //                                var allBPCounts1 = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count + bpCountInOneModule1;
        //                                curModuleNum = _bondDieCounter % moudleCountInOneSubstrate1 + 1;
        //                                curSubstrateNum = _bondDieCounter / moudleCountInOneSubstrate1 + 1;
        //                                if (_bondDieCounter == allMoudleCounts1)
        //                                {
        //                                    _bondDieCounter = 0;
        //                                    curModuleNum = 1;
        //                                    curSubstrateNum = 1;
        //                                }
        //                                if (IsProcessPart)
        //                                {
        //                                    if (_bondDieCounter >= ManualSettedProcessCount)
        //                                    {
        //                                        currentJobStatus = EnumJobRunStatus.Completed;
        //                                        break;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (_bondDieCounter >= allBPCounts1)
        //                                    {
        //                                        currentJobStatus = EnumJobRunStatus.Completed;
        //                                        break;
        //                                    }
        //                                }
        //                                if (_isBPInvalid)
        //                                {
        //                                    currentJobStatus = EnumJobRunStatus.AccuracyCalibrationChipSuccess;
        //                                    _isBPInvalid = false;
        //                                }
        //                                else
        //                                {
        //                                    //抛料后识别下一颗芯片
        //                                    currentJobStatus = EnumJobRunStatus.BondChipSuccess;
        //                                }
        //                                //弹窗提示
        //                                //if (WarningBox.FormShow("异常发生！", "贴装失败！", "警报") == 1)
        //                                //{
        //                                //    //抛料
        //                                //    currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
        //                                //}
        //                                //else
        //                                //{
        //                                //    currentJobStatus = EnumJobRunStatus.Aborted;
        //                                //}
        //                                break;
        //                            case EnumJobRunStatus.AbandonChipSuccess:
        //                                break;
        //                            case EnumJobRunStatus.AbandonChipFail:
        //                                break;
        //                            case EnumJobRunStatus.Aborted:
        //                                this.AbortJob();
        //                                WarningBox.FormShow("流程终止！", "流程异常终止！", "提示");
        //                                return;
        //                            case EnumJobRunStatus.Completed:
        //                                this.CompletedJob();
        //                                WarningBox.FormShow("流程完成！", "流程正常结束！", "提示");
        //                                return;
        //                            default:
        //                                break;
        //                        }
        //                    }

        //                }
        //            }
        //            if (RunStat == EnumProductRunStat.UserAbort)
        //            {
        //                ResetAction();
        //                WarningBox.FormShow("流程异常结束！", "用户终止流程！", "提示");
        //            }
        //            //}
        //        }));
        //    }
        //    else
        //    {
        //        LogRecorder.RecordLog(EnumLogContentType.Error, "Bond Move to Safe Position Failed.");
        //        this.AbortJob();
        //        WarningBox.FormShow("流程终止！", "流程异常终止！", "提示");
        //    }
        //}

        public void Execute()
        {
            IOUtilityHelper.Instance.TurnonTowerGreenLight();
            IOUtilityHelper.Instance.TurnoffTowerYellowLight();
            if (_positionSystem.BondMovetoSafeLocation())
            {
                curSubstrateNum = 1;
                curModuleNum = 1;
                BondDieCounter = 0;
                ProductExecutor.Instance.CurChipNum = 1;
                Task.Factory.StartNew(new Action(() =>
                {
                    EnumJobRunStatus currentJobStatus = EnumJobRunStatus.Initial;
                    //while (true)
                    //{

                    //填充 芯片相关的动作 （有可能是多芯片）
                    foreach (ProductStep step in ProductSteps)
                    {
                        ProductRecipe.CurrentComponentInfosName = step.ComponentName;
                        ProductRecipe.CurrentBondPositionSettingsName = step.BondingPositionName;
                        if (step.productStepType == EnumProductStepType.Dispense)
                        {
                            while (true && RunStat != EnumProductRunStat.Stop && RunStat != EnumProductRunStat.UserAbort)
                            {
                                var ret = GlobalGWResultDefine.RET_SUCCESS;
                                if (currentJobStatus == EnumJobRunStatus.DispenseAllPosSuccess || currentJobStatus == EnumJobRunStatus.DispenseAllPosFail)
                                {
                                    break;
                                }

                                switch (currentJobStatus)
                                {

                                    case EnumJobRunStatus.Initial:
                                        StepAction_PositionSubstrate stepAction_PositionSubstrate = new StepAction_PositionSubstrate(step, EnumActionNo.Action_PositionSubstrate, "基板定位");
                                        ret = stepAction_PositionSubstrate.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllSubstrateSuccess : EnumJobRunStatus.PositionAllSubstrateFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.PositionAllSubstrateSuccess:
                                        if (ProductRecipe.SubstrateInfos.IsPositionModules)
                                        {
                                            ResetEventWaitForNext();
                                            StepAction_PositionModule stepAction_PositionModule = new StepAction_PositionModule(step, EnumActionNo.Action_PositionModule, "芯片定位");
                                            ret = stepAction_PositionModule.Run();
                                            currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllModuleSuccess : EnumJobRunStatus.PositionAllModuleFail;
                                            WaitForNext();
                                        }
                                        else
                                        {
                                            currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
                                        }
                                        break;
                                    case EnumJobRunStatus.PositionAllSubstrateFail:
                                        //弹窗提示
                                        if (WarningBox.FormShow("异常发生！", "基板对位失败！", "警报") == 1)
                                        {
                                            //重试
                                            currentJobStatus = EnumJobRunStatus.Aborted;
                                        }
                                        else
                                        {
                                            currentJobStatus = EnumJobRunStatus.Aborted;
                                        }
                                        break;
                                    case EnumJobRunStatus.PositionOneModuleSuccess:
                                        break;
                                    case EnumJobRunStatus.PositionOneModuleFail:
                                        break;
                                    case EnumJobRunStatus.PositionAllModuleSuccess:
                                        ResetEventWaitForNext();
                                        StepAction_PositionBondPos stepAction_PositionBP = new StepAction_PositionBondPos(step, EnumActionNo.Action_RecognizeBondPos, "定位贴装位置");
                                        ret = stepAction_PositionBP.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllBondPosSuccess : EnumJobRunStatus.PositionAllBondPosFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.PositionAllModuleFail:
                                        //弹窗提示
                                        if (WarningBox.FormShow("异常发生！", "模组对位失败！", "警报") == 1)
                                        {
                                            //重试
                                            currentJobStatus = EnumJobRunStatus.PositionAllSubstrateSuccess;
                                        }
                                        else
                                        {
                                            currentJobStatus = EnumJobRunStatus.Aborted;
                                        }
                                        break;
                                    case EnumJobRunStatus.PositionOneBondPosSuccess:
                                        break;
                                    case EnumJobRunStatus.PositionOneBondPosFail:
                                        break;
                                    case EnumJobRunStatus.PositionAllBondPosSuccess:
                                        ResetEventWaitForNext();
                                        StepAction_Dispense stepAction_Dispense = new StepAction_Dispense(step, EnumActionNo.Action_Dispense, "划胶");
                                        ret = stepAction_Dispense.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.DispenseAllPosSuccess : EnumJobRunStatus.DispenseAllPosFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.PositionAllBondPosFail:
                                        //弹窗提示
                                        if (WarningBox.FormShow("异常发生！", "贴装位置对位失败！", "警报") == 1)
                                        {
                                            //重试
                                            currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
                                        }
                                        else
                                        {
                                            currentJobStatus = EnumJobRunStatus.Aborted;
                                        }
                                        break;
                                    case EnumJobRunStatus.DispenseOnePosSuccess:
                                        break;
                                    case EnumJobRunStatus.DispenseOnePosFail:
                                        break;
                                    case EnumJobRunStatus.Aborted:
                                        this.AbortJob();
                                        WarningBox.FormShow("流程终止！", "流程异常终止！", "提示");
                                        return;
                                        //case EnumJobRunStatus.DispenseAllPosSuccess:
                                        //    ResetEventWaitForNext();
                                        //    StepAction_PositionComponent stepAction_PositionChip = new StepAction_PositionComponent(null, EnumActionNo.Action_PositionChip, "定位芯片");
                                        //    ret = stepAction_PositionChip.Run();
                                        //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
                                        //    WaitForNext();
                                        //    break;
                                        //case EnumJobRunStatus.DispenseAllPosFail:
                                        //    //弹窗提示
                                        //    if (WarningBox.FormShow("异常发生！", "点胶失败！", "警报") == 1)
                                        //    {
                                        //        //重试
                                        //        currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
                                        //    }
                                        //    else
                                        //    {
                                        //        currentJobStatus = EnumJobRunStatus.Aborted;
                                        //    }
                                        //    break;
                                        //case EnumJobRunStatus.PositionChipSuccess:
                                        //    ResetEventWaitForNext();
                                        //    StepAction_PickUpChip stepAction_PickChip = new StepAction_PickUpChip(null, EnumActionNo.Action_PositionChip, "拾取芯片");
                                        //    ret = stepAction_PickChip.Run();
                                        //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
                                        //    ProductExecutor.Instance.CurChipNum++;
                                        //    WaitForNext();
                                        //    break;
                                        //case EnumJobRunStatus.PositionChipFail:
                                        //    //芯片对位失败时自动跳到下一颗
                                        //    ProductExecutor.Instance.CurChipNum++;
                                        //    break;
                                        //case EnumJobRunStatus.PickupChipSuccess:
                                        //    //进行二次校准同时识别下一颗芯片
                                        //    Task.Factory.StartNew(() => {
                                        //        _eventWaitAsysncPositionChipSignal.WaitOne();
                                        //        ResetEventWaitForNext();
                                        //        StepAction_PositionComponent stepAction_AsyncPositionChip = new StepAction_PositionComponent(null, EnumActionNo.Action_PositionChip, "定位芯片");
                                        //        ret = stepAction_AsyncPositionChip.Run();
                                        //        WaitForNext();
                                        //        _eventWaitAsysncPositionChipComplete.Set();

                                        //    });
                                        //    ResetEventWaitForNext();
                                        //    StepAction_AccuracyPositionChip stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionChip(null, EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
                                        //    ret = stepAction_AccuracyCalibrationChip.Run();
                                        //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.AccuracyCalibrationChipSuccess : EnumJobRunStatus.AccuracyCalibrationChipFail;
                                        //    WaitForNext();
                                        //    break;
                                        //case EnumJobRunStatus.PickupChipFail:
                                        //    break;
                                        //case EnumJobRunStatus.AccuracyCalibrationChipSuccess:
                                        //    ResetEventWaitForNext();
                                        //    StepAction_BondChip stepAction_BondChip = new StepAction_BondChip(null, EnumActionNo.Action_BondChip, "固晶");
                                        //    ret = stepAction_BondChip.Run();
                                        //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.BondChipSuccess : EnumJobRunStatus.BondChipFail;
                                        //    WaitForNext();
                                        //    break;
                                        //case EnumJobRunStatus.AccuracyCalibrationChipFail:
                                        //    break;
                                        //case EnumJobRunStatus.BondChipSuccess:
                                        //    _eventWaitAsysncPositionChipComplete.WaitOne();
                                        //    ResetEventWaitForNext();
                                        //    StepAction_PickUpChip stepAction_PickChip2 = new StepAction_PickUpChip(null, EnumActionNo.Action_PositionChip, "拾取芯片");
                                        //    ret = stepAction_PickChip2.Run();
                                        //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
                                        //    WaitForNext();
                                        //    break;
                                        //case EnumJobRunStatus.BondChipFail:
                                        //    //弹窗提示
                                        //    if (WarningBox.FormShow("异常发生！", "点胶失败！", "警报") == 1)
                                        //    {
                                        //        //抛料
                                        //        currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
                                        //    }
                                        //    else
                                        //    {
                                        //        currentJobStatus = EnumJobRunStatus.Aborted;
                                        //    }
                                        //    break;
                                        //case EnumJobRunStatus.AbandonChipSuccess:
                                        //    break;
                                        //case EnumJobRunStatus.AbandonChipFail:
                                        //    break;
                                        //case EnumJobRunStatus.Aborted:
                                        //    WarningBox.FormShow("流程终止！","流程异常终止！","提示");
                                        //    return;
                                        //case EnumJobRunStatus.Completed:
                                        //    WarningBox.FormShow("流程完成！", "流程正常结束！", "提示");
                                        //    return;
                                        //default:
                                        //    break;
                                }
                            }

                        }
                        else if (step.productStepType == EnumProductStepType.BondDie)
                        {
                            while (true && RunStat != EnumProductRunStat.Stop && RunStat != EnumProductRunStat.UserAbort)
                            {
                                //if (currentJobStatus == EnumJobRunStatus.Completed || currentJobStatus == EnumJobRunStatus.Aborted)
                                //{
                                //    break;
                                //}
                                var ret = GlobalGWResultDefine.RET_SUCCESS;
                                switch (currentJobStatus)
                                {

                                    //case EnumJobRunStatus.Initial:
                                    //    StepAction_PositionSubstrate stepAction_PositionSubstrate = new StepAction_PositionSubstrate(null, EnumActionNo.Action_PositionSubstrate, "基板定位");
                                    //    ret = stepAction_PositionSubstrate.Run();
                                    //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllSubstrateSuccess : EnumJobRunStatus.PositionAllSubstrateFail;
                                    //    WaitForNext();
                                    //    break;
                                    //case EnumJobRunStatus.PositionAllSubstrateSuccess:
                                    //    if (ProductRecipe.SubstrateInfos.IsPositionModules)
                                    //    {
                                    //        ResetEventWaitForNext();
                                    //        StepAction_PositionModule stepAction_PositionModule = new StepAction_PositionModule(null, EnumActionNo.Action_PositionModule, "芯片定位");
                                    //        ret = stepAction_PositionModule.Run();
                                    //        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllModuleSuccess : EnumJobRunStatus.PositionAllModuleFail;
                                    //        WaitForNext();
                                    //    }
                                    //    else
                                    //    {
                                    //        currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
                                    //    }
                                    //    break;
                                    //case EnumJobRunStatus.PositionAllSubstrateFail:
                                    //    //弹窗提示
                                    //    if (WarningBox.FormShow("异常发生！", "基板对位失败！", "警报") == 1)
                                    //    {
                                    //        //重试
                                    //        currentJobStatus = EnumJobRunStatus.Initial;
                                    //    }
                                    //    else
                                    //    {
                                    //        currentJobStatus = EnumJobRunStatus.Aborted;
                                    //    }
                                    //    break;
                                    //case EnumJobRunStatus.PositionOneModuleSuccess:
                                    //    break;
                                    //case EnumJobRunStatus.PositionOneModuleFail:
                                    //    break;
                                    //case EnumJobRunStatus.PositionAllModuleSuccess:
                                    //    ResetEventWaitForNext();
                                    //    StepAction_PositionBondPos stepAction_PositionBP = new StepAction_PositionBondPos(null, EnumActionNo.Action_RecognizeBondPos, "定位贴装位置");
                                    //    ret = stepAction_PositionBP.Run();
                                    //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionAllBondPosSuccess : EnumJobRunStatus.PositionAllBondPosFail;
                                    //    WaitForNext();
                                    //    break;
                                    //case EnumJobRunStatus.PositionAllModuleFail:
                                    //    //弹窗提示
                                    //    if (WarningBox.FormShow("异常发生！", "模组对位失败！", "警报") == 1)
                                    //    {
                                    //        //重试
                                    //        currentJobStatus = EnumJobRunStatus.PositionAllSubstrateSuccess;
                                    //    }
                                    //    else
                                    //    {
                                    //        currentJobStatus = EnumJobRunStatus.Aborted;
                                    //    }
                                    //    break;
                                    //case EnumJobRunStatus.PositionOneBondPosSuccess:
                                    //    break;
                                    //case EnumJobRunStatus.PositionOneBondPosFail:
                                    //    break;
                                    //case EnumJobRunStatus.PositionAllBondPosSuccess:
                                    //    ResetEventWaitForNext();
                                    //    StepAction_Dispense stepAction_Dispense = new StepAction_Dispense(null, EnumActionNo.Action_Dispense, "划胶");
                                    //    ret = stepAction_Dispense.Run();
                                    //    currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.DispenseAllPosSuccess : EnumJobRunStatus.DispenseAllPosFail;
                                    //    WaitForNext();
                                    //    break;
                                    //case EnumJobRunStatus.PositionAllBondPosFail:
                                    //    //弹窗提示
                                    //    if (WarningBox.FormShow("异常发生！", "贴装位置对位失败！", "警报") == 1)
                                    //    {
                                    //        //重试
                                    //        currentJobStatus = EnumJobRunStatus.PositionAllModuleSuccess;
                                    //    }
                                    //    else
                                    //    {
                                    //        currentJobStatus = EnumJobRunStatus.Aborted;
                                    //    }
                                    //    break;
                                    case EnumJobRunStatus.DispenseOnePosSuccess:
                                        break;
                                    case EnumJobRunStatus.DispenseOnePosFail:
                                        break;
                                    case EnumJobRunStatus.DispenseAllPosSuccess:
                                        ResetEventWaitForNext();

                                        StepAction_PositionComponent stepAction_PositionChip = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
                                        ret = stepAction_PositionChip.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.DispenseAllPosFail:
                                        //弹窗提示
                                        if (WarningBox.FormShow("异常发生！", "点胶失败！", "警报") == 1)
                                        {
                                            //重试
                                            //currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
                                            currentJobStatus = EnumJobRunStatus.Aborted;
                                        }
                                        else
                                        {
                                            currentJobStatus = EnumJobRunStatus.Aborted;
                                        }
                                        break;
                                    case EnumJobRunStatus.PositionChipSuccess:
                                        ResetEventWaitForNext();
                                        StepAction_PickUpChipWithRotate stepAction_PickChip = new StepAction_PickUpChipWithRotate(step, EnumActionNo.Action_PositionChip, "拾取芯片");
                                        ret = stepAction_PickChip.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
                                        ProductExecutor.Instance.CurChipNum++;
                                        ProductExecutor.Instance.CurChipNGNum = 0;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.PositionChipFail:
                                        //芯片对位失败时自动跳到下一颗
                                        ProductExecutor.Instance.CurChipNum++;
                                        
                                        if (curChipNum > ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
                                        {
                                            currentJobStatus = EnumJobRunStatus.Completed;
                                            break;
                                        }
                                        ProductExecutor.Instance.CurChipNGNum++;
                                        if(ProductExecutor.Instance.CurChipNGNum > SystemConfiguration.Instance.JobConfig.CurChipNGNumMax)
                                        {
                                            if (CameraWindowGUI.Instance != null)
                                            {
                                                CameraWindowGUI.Instance.SelectCamera(2);
                                                CameraWindowGUI.Instance.ClearGraphicDraw();
                                            }
                                            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                                            {
                                                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                                                CameraWindowForm.Instance.Show();
                                            }
                                            //弹窗提示
                                            if (ShowMessage2("异常发生！", "搜寻芯片失败，请重新确定搜寻芯片起点位置！", "警报") == 1)
                                            {
                                                ProductExecutor.Instance.CurChipNum = 1;
                                                ProductExecutor.Instance.CurChipNGNum = 0;
                                                double X = _positionSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                                                double Y = _positionSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);
                                                ProductExecutor.Instance.MaterialLocationOffsetX = X - ProductRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.X;
                                                ProductExecutor.Instance.MaterialLocationOffsetY = Y - ProductRecipe.CurrentComponent.ComponentMapInfos[0].MaterialLocation.Y;
                                            }
                                            else
                                            {
                                                currentJobStatus = EnumJobRunStatus.Aborted;
                                            }
                                        }
                                        ResetEventWaitForNext();
                                        StepAction_PositionComponent stepAction_PositionChip3 = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
                                        ret = stepAction_PositionChip3.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.PickupChipSuccess:
                                        //进行二次校准同时识别下一颗芯片
                                        //Task.Factory.StartNew(() =>
                                        //{
                                        //    _eventWaitAsysncPositionChipSignal.WaitOne();
                                        //    ResetEventWaitForNext();
                                        //    StepAction_PositionComponent stepAction_AsyncPositionChip = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
                                        //    ret = stepAction_AsyncPositionChip.Run();
                                        //    WaitForNext();
                                        //    _eventWaitAsysncPositionChipComplete.Set();

                                        //});
                                        ResetEventWaitForNext();
                                        if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                                        {
                                            StepAction_AccuracyPositionWithUplookCamera stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionWithUplookCamera(step, EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
                                            ret = stepAction_AccuracyCalibrationChip.Run();
                                        }
                                        else if(ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                                        {
                                            StepAction_AccuracyPositionChipInCalibrationTable stepAction_AccuracyCalibrationChip = new StepAction_AccuracyPositionChipInCalibrationTable(step, EnumActionNo.Action_AccuracyPositionChip, "二次校准芯片");
                                            ret = stepAction_AccuracyCalibrationChip.Run();
                                        }
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.AccuracyCalibrationChipSuccess : EnumJobRunStatus.AccuracyCalibrationChipFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.PickupChipFail:
                                        //芯片拾取失败时自动抛料跳到下一颗TBD需增加自动抛料
                                        //ProductExecutor.Instance.CurChipNum++;
                                        if (curChipNum > ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
                                        {
                                            currentJobStatus = EnumJobRunStatus.Completed;
                                            break;
                                        }
                                        currentJobStatus = EnumJobRunStatus.BondChipSuccess;
                                        break;
                                    case EnumJobRunStatus.AccuracyCalibrationChipSuccess:
                                        ResetEventWaitForNext();
                                        //StepAction_BondChip stepAction_BondChip = new StepAction_BondChip(step, EnumActionNo.Action_BondChip, "固晶");
                                        StepAction_OnlyBondChip stepAction_BondChip = new StepAction_OnlyBondChip(step, EnumActionNo.Action_BondChip, "固晶");
                                        ret = stepAction_BondChip.Run();
                                        if (ret == GlobalGWResultDefine.RET_BPInvalid)
                                        {
                                            _isBPInvalid = true;
                                        }
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.BondChipSuccess : EnumJobRunStatus.BondChipFail;
                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.AccuracyCalibrationChipFail:
                                        //芯片二次对位失败时自动抛料跳到下一颗TBD需增加自动抛料
                                        //ProductExecutor.Instance.CurChipNum++;
                                        if (curChipNum > ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
                                        {
                                            currentJobStatus = EnumJobRunStatus.Completed;
                                            break;
                                        }
                                        if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                                        {
                                            StepAction_MaterialThrowingAction stepAction_MaterialThrowingAction = new StepAction_MaterialThrowingAction(step, EnumActionNo.Action_AbondonChip, "抛料");
                                            ret = stepAction_MaterialThrowingAction.Run();
                                        }
                                        else if (ProductRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                                        {
                                            StepAction_CalibrationTableMaterialThrowingAction stepAction_MaterialThrowingAction = new StepAction_CalibrationTableMaterialThrowingAction(step, EnumActionNo.Action_AbondonChip, "抛料");
                                            ret = stepAction_MaterialThrowingAction.Run();
                                        }
                                        
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.AbandonChipSuccess : EnumJobRunStatus.AbandonChipFail;
                                        if (currentJobStatus == EnumJobRunStatus.AbandonChipFail)
                                        {
                                            if (ShowMessage2("异常发生！", "抛料失败，请手动去除吸嘴上的芯片！清除无误后点击<确认>按钮后流程继续。", "警报") == 1)
                                            {

                                            }
                                            else
                                            {
                                                currentJobStatus = EnumJobRunStatus.Aborted;
                                            }
                                        }

                                        //抛料成功后
                                        StepAction_PositionComponent stepAction_PositionChip2 = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
                                        ret = stepAction_PositionChip2.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;
                                        //currentJobStatus = EnumJobRunStatus.BondChipSuccess;
                                        break;
                                    case EnumJobRunStatus.BondChipSuccess:
                                        //_eventWaitAsysncPositionChipComplete.WaitOne();
                                        //成功之后更新当前SubstrateNumber和ModuleNumber
                                        BondDieCounter++;
                                        var moudleCountInOneSubstrate = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count;
                                        var bpCountInOneModule = ProductRecipe.StepBondingPositionList.Count;
                                        var allMoudleCounts = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count;
                                        var allBPCounts = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count * bpCountInOneModule;
                                        curModuleNum = _bondDieCounter % moudleCountInOneSubstrate + 1;
                                        curSubstrateNum = _bondDieCounter / moudleCountInOneSubstrate + 1;

                                        if (_bondDieCounter >= allBPCounts)
                                        {
                                            currentJobStatus = EnumJobRunStatus.Completed;
                                            break;
                                        }
                                        //多芯片场景
                                        if (_bondDieCounter == allMoudleCounts&& ProductRecipe.StepBondingPositionList.Count>1)
                                        {
                                            _bondDieCounter = 0;
                                            curModuleNum = 1;
                                            curSubstrateNum = 1;
                                        }
                                        ResetEventWaitForNext();
                                        //StepAction_PickUpChip stepAction_PickChip2 = new StepAction_PickUpChip(step, EnumActionNo.Action_PositionChip, "拾取芯片");
                                        //ret = stepAction_PickChip2.Run();
                                        //currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PickupChipSuccess : EnumJobRunStatus.PickupChipFail;
                                        if(curChipNum>= ProductRecipe.CurrentComponent.ComponentMapInfos.Count)
                                        {
                                            currentJobStatus = EnumJobRunStatus.Completed;
                                            break;
                                        }
                                        if (ProductExecutor.Instance.IsProcessPart)
                                        {
                                            if (_bondDieCounter >= ProductExecutor.Instance.ManualSettedProcessCount)
                                            {
                                                currentJobStatus = EnumJobRunStatus.Completed;
                                                break;
                                            }
                                        }
                                        StepAction_PositionComponent stepAction_PositionChip4 = new StepAction_PositionComponent(step, EnumActionNo.Action_PositionChip, "定位芯片");
                                        ret = stepAction_PositionChip4.Run();
                                        currentJobStatus = ret == GlobalGWResultDefine.RET_SUCCESS ? EnumJobRunStatus.PositionChipSuccess : EnumJobRunStatus.PositionChipFail;

                                        WaitForNext();
                                        break;
                                    case EnumJobRunStatus.BondChipFail:

                                        BondDieCounter++;
                                        var moudleCountInOneSubstrate1 = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count;
                                        var bpCountInOneModule1 = ProductRecipe.StepBondingPositionList.Count;
                                        var allMoudleCounts1 = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count;
                                        var allBPCounts1 = ProductRecipe.SubstrateInfos.ModuleMapInfos.FirstOrDefault().Count * ProductRecipe.SubstrateInfos.SubstrateMapInfos.Count + bpCountInOneModule1;
                                        curModuleNum = _bondDieCounter % moudleCountInOneSubstrate1 + 1;
                                        curSubstrateNum = _bondDieCounter / moudleCountInOneSubstrate1 + 1;
                                        if (_bondDieCounter == allMoudleCounts1)
                                        {
                                            _bondDieCounter = 0;
                                            curModuleNum = 1;
                                            curSubstrateNum = 1;
                                        }
                                        if (IsProcessPart)
                                        {
                                            if (_bondDieCounter >= ManualSettedProcessCount)
                                            {
                                                currentJobStatus = EnumJobRunStatus.Completed;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (_bondDieCounter >= allBPCounts1)
                                            {
                                                currentJobStatus = EnumJobRunStatus.Completed;
                                                break;
                                            }
                                        }
                                        if (_isBPInvalid)
                                        {
                                            currentJobStatus = EnumJobRunStatus.AccuracyCalibrationChipSuccess;
                                            _isBPInvalid = false;
                                        }
                                        else
                                        {
                                            //抛料后识别下一颗芯片
                                            currentJobStatus = EnumJobRunStatus.BondChipSuccess;
                                        }
                                        //弹窗提示
                                        //if (WarningBox.FormShow("异常发生！", "贴装失败！", "警报") == 1)
                                        //{
                                        //    //抛料
                                        //    currentJobStatus = EnumJobRunStatus.PositionAllBondPosSuccess;
                                        //}
                                        //else
                                        //{
                                        //    currentJobStatus = EnumJobRunStatus.Aborted;
                                        //}
                                        break;
                                    case EnumJobRunStatus.AbandonChipSuccess:
                                        break;
                                    case EnumJobRunStatus.AbandonChipFail:
                                        break;
                                    case EnumJobRunStatus.Aborted:
                                        this.AbortJob();
                                        WarningBox.FormShow("流程终止！", "流程异常终止！", "提示");
                                        return;
                                    case EnumJobRunStatus.Completed:
                                        this.CompletedJob();
                                        WarningBox.FormShow("流程完成！", "流程正常结束！", "提示");
                                        return;
                                    default:
                                        break;
                                }
                            }

                        }
                    }
                    if (RunStat == EnumProductRunStat.UserAbort)
                    {
                        ResetAction();
                        WarningBox.FormShow("流程异常结束！", "用户终止流程！", "提示");
                    }
                    //}
                }));
            }
            else
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Bond Move to Safe Position Failed.");
                this.AbortJob();
                WarningBox.FormShow("流程终止！", "流程异常终止！", "提示");
            }


            //this.StopRunStat();
        }

        public void ExecuteStep()
        {
            RunStat = EnumProductRunStat.StepRun;
            StepActionBase act = AllActions[RunningActionIndex];
            act.ActionStat = EnumActionStat.running;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));
            GWResult ret = act.Run();
            if (ret != GlobalGWResultDefine.RET_SUCCESS)
            {
                WarningBox.FormShow("错误", ret.ResultMsg, ret.ResultCode.ToString());
                act.ActionStat = EnumActionStat.error;
            }
            else
            {
                act.ActionStat = EnumActionStat.done;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));

            RunningActionIndex ++;

            if (act.IsLastActionPerSubstrate && RunningActionIndex < AllActions.Count - 1)
            {
                CurSubstrateNum += 1;
            }

            if (RunningActionIndex == AllActions.Count)
            {
                RunStat = EnumProductRunStat.Stop;
            }
            else
            {
                RunStat = EnumProductRunStat.StepPause;
            }
        }
        private void WaitForNext()
        {
            if (SingleStepRun)
            {
                _eventWaitForNextAction.WaitOne();
            }
        }
        private void ResetEventWaitForNext()
        {
            if(SingleStepRun)
            {
                _eventWaitForNextAction.Reset();
            }
        }
        public void SetEventWaitForNext()
        {
            if (SingleStepRun)
            {
                _eventWaitForNextAction.Set();
            }
        }
        public GWResult ExecuteStep(StepActionBase act)
        {
            RunStat = EnumProductRunStat.StepRun;
            act.ActionStat = EnumActionStat.running;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));
            GWResult ret = act.Run();
            if (ret != GlobalGWResultDefine.RET_SUCCESS)
            {
                WarningBox.FormShow("错误", ret.ResultMsg, ret.ResultCode.ToString());
                act.ActionStat = EnumActionStat.error;
            }
            else
            {
                act.ActionStat = EnumActionStat.done;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));

            return ret;
        }

        public void ExecuteSelectedStep(int index)
        {
            RunStat = EnumProductRunStat.StepRun;
            StepActionBase act = StepActions[index];
            act.ActionStat = EnumActionStat.running;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));
            GWResult ret = act.Run();
            if (ret != GlobalGWResultDefine.RET_SUCCESS)
            {
                WarningBox.FormShow("错误", ret.ResultMsg, ret.ResultCode.ToString());
                act.ActionStat = EnumActionStat.error;
            }
            else
            {
                act.ActionStat = EnumActionStat.done;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat")); 
            RunStat = EnumProductRunStat.StepPause;
        }

        //复位动作状态
        public void ResetActionStat()
        {
            ProcedureComplete();
            foreach (StepActionBase act in AllActions)
            {
                act.ActionStat = EnumActionStat.undo;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));
        }

        /*
         * 跳步
         * 
         * direct 跳步方向：1向上  2向下
         * actNo 跳到动作类型 EnumActionNo
         */
        public bool JumpAct(int direct, EnumActionNo actNo)
        {
            /*this.CurSubstrateNum = subIndex;
            this.RunningActionIndex = actIndex;
            SetSubNumActions();*/

            int cur = RunningActionIndex;
            bool find = false;
            List<StepActionBase> needChgStatList = new List<StepActionBase>();

            //跳步前基底num
            int substrateNo = CurSubstrateNum;

            while (RunningActionIndex >=0 && RunningActionIndex < AllActions.Count - 1)
            {
                //向下先加
                if (direct == 2)
                {
                    cur++;
                }

                if (cur >= AllActions.Count - 1)
                {
                    break;
                }

                StepActionBase act = AllActions[cur];

                if (act.ActionNo == actNo)
                {
                    find = true;
                    needChgStatList.Add(act);
                    break;
                }
                else
                {
                    if (act.IsLastActionPerSubstrate)
                    {
                        substrateNo = (direct == 1) ? substrateNo - 1 : substrateNo + 1;
                    }
                    needChgStatList.Add(act);
                }

                //向上后减
                if (direct == 1)
                {
                    cur--;
                }
            }

            if (!find)
            {
                WarningBox.FormShow("错误", "跳步失败！");
                return false;
            }
            else
            {
                for(int i=0; i> needChgStatList.Count; i++)
                {
                    AllActions[i].ActionStat = EnumActionStat.undo;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionStat"));

                this.CurSubstrateNum = substrateNo;
                this.RunningActionIndex = cur;
                //SetSubNumActions();
                IsActionRunning = true;
                this.Execute();
                IsActionRunning = false;
                return true;
            }

        }

        private void BeforeRun()
        {
            InTranspotoneMaterial();
            DataModel.Instance.PropertyChanged += ActBeforeRun;
        }
        private void ProcedureComplete()
        {
            
            Thread.Sleep(2000);
            DataModel.Instance.PropertyChanged -= ActBeforeRun;
        }

        private void InTranspotoneMaterial()
        {
            Task.Run(()=> {
                while(DataModel.Instance.StartTranspot == false)
                {

                }
                while (true)
                {
                    if(DataModel.Instance.StartTranspot)
                    {
                        if (DataModel.Instance.TransportInPlaceSignal1 && DataModel.Instance.TransportInPlaceSignal2 == false)
                        {
                            IOUtilityClsLib.IOUtilityHelper.Instance.OpenTransportCylinder1();
                            //开始传片
                            _positionSystem.JogNegative(EnumStageAxis.TransportTrack1, 5f);
                            _positionSystem.JogNegative(EnumStageAxis.TransportTrack2, 5f);
                            while (true)
                            {
                                if (DataModel.Instance.TransportInPlaceSignal2 == true)
                                {
                                    break;
                                }
                            }
                            while (true)
                            {
                                if (DataModel.Instance.TransportInPlaceSignal2 == false)
                                {
                                    //超过第二个光电
                                    _positionSystem.StopJogNegative(EnumStageAxis.TransportTrack1);
                                    _positionSystem.StopJogNegative(EnumStageAxis.TransportTrack2);

                                    _positionSystem.JogPositive(EnumStageAxis.TransportTrack2, 0.3f);
                                    break;
                                }
                            }
                            while (true)
                            {
                                if (DataModel.Instance.TransportInPlaceSignal2 == true)
                                {
                                    //到达第二个光电
                                    _positionSystem.StopJogPositive(EnumStageAxis.TransportTrack2);
                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseTransportCylinder1();
                                    break;
                                }
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    }
                }
                DataModel.Instance.StartTranspot = false;
                Execute();
                //Transpotmothed();

            });
            
        }

        private void OutTranspotoneMaterial()
        {
            Task.Run(() => {
                if (DataModel.Instance.EndTranspot == false)
                {
                    if (DataModel.Instance.TransportInPlaceSignal2 && DataModel.Instance.TransportInPlaceSignal3 == false)
                    {
                        //开始传片
                        IOUtilityClsLib.IOUtilityHelper.Instance.OpenTransportCylinder1();
                        _positionSystem.JogNegative(EnumStageAxis.TransportTrack2, 5f);
                        _positionSystem.JogNegative(EnumStageAxis.TransportTrack3, 5f);
                        while (true)
                        {
                            if (DataModel.Instance.TransportInPlaceSignal3)
                            {
                                //超过第二个光电
                                _positionSystem.StopJogNegative(EnumStageAxis.TransportTrack2);
                                _positionSystem.StopJogNegative(EnumStageAxis.TransportTrack3);
                                break;
                            }
                        }
                    }
                    DataModel.Instance.EndTranspot = true;
                }
            });
        }

        private void ActBeforeRun(object sender, PropertyChangedEventArgs e)
        {
            //开始传片流程
            if (e.PropertyName == nameof(DataModel.TransportInPlaceSignal1))
            {
                DataModel.Instance.StartTranspot = true;
            }


            //if (e.PropertyName == nameof(DataModel.TransportInPlaceSignal1))
            //{
            //    if(DataModel.Instance.StartTranspot)
            //    {
            //        if (DataModel.Instance.TransportInPlaceSignal1 && DataModel.Instance.TransportInPlaceSignal2 == false)
            //        {
            //            //开始传片
            //            _positionSystem.JogNegative(EnumStageAxis.TransportTrack1, 5f);
            //            _positionSystem.JogNegative(EnumStageAxis.TransportTrack2, 5f);
            //        }
            //        else
            //        {
            //            //停止传片
            //        }
            //    }
                
            //}
            //if (e.PropertyName == nameof(DataModel.TransportInPlaceSignal2))
            //{
            //    if (DataModel.Instance.TransportInPlaceSignal2)
            //    {
            //        //上料到位之后
            //        //开始检测
            //        //Execute();
            //    }
            //    else
            //    {
            //        //停止传片
            //        _positionSystem.StopJogNegative(EnumStageAxis.TransportTrack1);
            //        _positionSystem.StopJogNegative(EnumStageAxis.TransportTrack2);

            //        _positionSystem.JogPositive(EnumStageAxis.TransportTrack2, 1f);

            //    }
            //}
        }

        //停止将动作和运行状态归位
        public void AbortJob()
        {
            RunStat = EnumProductRunStat.Stop;
            ResetActionStat();
            RunningActionIndex = 0;
            CurSubstrateNum = 1;
            CurChipNum = 1;
            _bondDieCounter = 0;
            curModuleNum = 1;
            ResetAction();
        }

        //停止将动作和运行状态归位
        public void CompletedJob()
        {
            RunStat = EnumProductRunStat.Completed;
            ResetActionStat();
            RunningActionIndex = 0;
            CurSubstrateNum = 1;
            CurChipNum = 1;
            _bondDieCounter = 0;
            curModuleNum = 1;
            ResetAction();
            //推片
            DataModel.Instance.EndTranspot = false;
            Thread.Sleep(500);
            OutTranspotoneMaterial();
        }

        //停止后需要机器完成的复位动作
        public void ResetAction()
        {
            IOUtilityHelper.Instance.TurnoffTowerGreenLight();
            IOUtilityHelper.Instance.TurnonTowerYellowLight();

            IOUtilityHelper.Instance.CloseChipPPVaccum();
            IOUtilityHelper.Instance.CloseChipPPBlow();

            IOUtilityHelper.Instance.CloseSubmountPPVaccum();
            IOUtilityHelper.Instance.CloseSubmountPPBlow();

            IOUtilityHelper.Instance.CloseESBaseVaccum();
            IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();
            IOUtilityHelper.Instance.CloseWaferTableVaccum();
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
            IOUtilityHelper.Instance.CloseNitrogen();

            LightControllerManager.Instance.CloseAllLight();

            StepAction_MoveSafePos stepAction_MoveSafePos = new StepAction_MoveSafePos(null, EnumActionNo.Action_MoveSafePos, "移动到安全位置");
            stepAction_MoveSafePos.Run();



            //StageControllerClsLib.StageCore.Instance.AbloluteMoveSync(EnumStageAxis.ChipPPT, 0);
            //StageControllerClsLib.StageCore.Instance.AbloluteMoveSync(EnumStageAxis.SubmountPPT, 0);

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



}
