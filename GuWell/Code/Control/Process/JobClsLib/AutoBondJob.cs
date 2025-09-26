using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Linq;
using RecipeClsLib;
using WestDragon.Framework.UtilityHelper;
using GlobalDataDefineClsLib;
using System.Diagnostics;
using ConfigurationClsLib;
using CameraControllerClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using PowerControllerManagerClsLib;
using VisionDesigner;
using GlobalToolClsLib;
using CommonPanelClsLib;
using CameraControllerWrapperClsLib;

namespace JobClsLib
{
    /// <summary>
    /// 
    /// </summary>
    internal class AutoBondJob : ABondJob
    {

        private int _processedLidCounter = 0;
        private int _processedShellCounter = 0;
        private bool _pointWeldFinish = false;
        private int _currentShellRow = 0;
        private int _currentShellColumn = 0;
        private int _currentLidRow = 0;
        private int _currentLidColumn = 0;


        /// <summary>
        /// Recipe存放系统默认路径
        /// </summary>
        public static string SystemDefaultDirectory = GlobalParamSetting.CONFIG_FILE_DEFAULT_DIR;
        public JobResult JobResult { get; set; }
        private GWPowerControl _powerController
        {
            get
            {
                var controller = PowerControllerManager.Instance.GetCurrentHardware();
                if (controller != null)
                {
                    return controller as GWPowerControl;
                }
                return null;
            }
        }
        private CameraConfig _cameraConfig
        {
            get { return CameraManager.Instance.CurrentCameraConfig; }
        }
        /// <summary>
        /// 传片流程规划器
        /// </summary>
        private MaterialTranferPlanner _transferPlanner = null;
        /// <summary>
        /// 
        /// </summary>
        public AutoBondJob()
        {
            _statusChecker = new RunStatusController();
            _transferPlanner = new MaterialTranferPlanner();
            try
            {
                //var lidShapeMatcher = new GWShapeMatch();
                //_lidPositionDetector = new PositionDetectProcesser(lidShapeMatcher);
                //var ShellShapeMatcher = new GWShapeMatch();
                //_shellPositionDetector = new PositionDetectProcesser(ShellShapeMatcher);
            }
            catch (MvdException ex)
            {
                WarningBox.FormShow("运行环境异常。", ex.Message, "提示");
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Contruct WeldProcessJob Fail.", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void RunBeforeJob()
        {
            _processedLidCounter = 0;
            _processedShellCounter = 0;
            _pointWeldFinish = false;
            RawDataSavingPath = GenerateDataSavingPath();
            JobResult = new JobResult();
            Queue<MaterialMapInformation> components = new Queue<MaterialMapInformation>();
            foreach (var item in _currentRecipe.CurrentComponent.ComponentMapInfos)
            {
                components.Enqueue(item);
            }
            _actionForComponentQueue = _transferPlanner.PlanComponentTransferActionQueue(components);
            Queue<MaterialMapInformation> submounts = new Queue<MaterialMapInformation>();
            foreach (var item in _currentRecipe.SubstrateInfos.SubstrateMapInfos)
            {
                submounts.Enqueue(item);
            }
            _actionForSubmountQueue = _transferPlanner.PlanSubmountTransferActionQueue(submounts);
            //if (_lidPositionDetector == null)
            //{
            //    var lidShapeMatcher = new GWShapeMatch();
            //    _lidPositionDetector = new PositionDetectProcesser(lidShapeMatcher);
            //}
            //if (_shellPositionDetector == null)
            //{
            //    var ShellShapeMatcher = new GWShapeMatch();
            //    _shellPositionDetector = new PositionDetectProcesser(ShellShapeMatcher);
            //}
            SetRumParameters();
        }
        public override void PrepareJob(BondRecipe recipe, bool isFirst)
        {

        }

        private float CorrectRecogniseAngle(float rawAngle)
        {

            var correctAngle = rawAngle % 90;

            // If the actual angle is greater than 45 or less than -45, 
            // map it to the equivalent angle in the range -45 to 45
            if (correctAngle > 45)
            {
                correctAngle = correctAngle - 90;
            }
            else if (correctAngle < -45)
            {
                correctAngle = correctAngle + 90;
            }
            LogRecorder.RecordLog(EnumLogContentType.Debug, $"CorrectRecogniseAngle.<rawAngle:{rawAngle},correctAngle:{correctAngle}.>");
            return correctAngle;
        }
        

        private void TurnOffLights()
        {
            try
            {
                //if (!_statusChecker.IsRunning) { return; }
                //_hardWareManager.ShellDarkField.SetIntensity(0);
                //_hardWareManager.ShellBrightField.SetIntensity(0);
                //_hardWareManager.LidDarkField.SetIntensity(0);
                //_hardWareManager.LidBrightField.SetIntensity(0);
            }
            catch (Exception ex)
            {
                //应用检测照明参数异常，退出检测流程
                //this.Abort();
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured While TurnOff Lights.",ex);
            }

        }
        /// <summary>
        /// 生成数据存储目录
        /// </summary>
        /// <returns></returns>
        protected override string GenerateDataSavingPath()
        {
            try
            {
                string dateString = System.DateTime.Now.ToString("yyyyMMdd");
                string timeString = System.DateTime.Now.ToString("HHmmss");
                string path = _systemConfig.RawDataSavePath;
                string lotId = JobInfosManager.Instance.CurrentJobMaterialStripSourceInfo.LotId;
                string materialStripId = JobInfosManager.Instance.CurrentJobMaterialStripSourceInfo.MaterialStripId;
                string slotId = JobInfosManager.Instance.CurrentJobMaterialStripSourceInfo.SourceSlotIndex;
                path = Path.Combine(path, _currentRecipe.RecipeName, lotId);
                string parentPath = Path.Combine(path, dateString, "L" + slotId + "-" + materialStripId + "-" + timeString);
                var rawDataSavingPath = Path.Combine(parentPath, @"Data");
                CommonProcess.EnsureFolderExist(rawDataSavingPath);
                return rawDataSavingPath;
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Create Saving FilePath failed.", ex);
                throw (ex);
            }
        }




        /// <summary>
        /// 初始化WaferJob
        /// </summary>
        public override void InitialJob()
        {
            try
            {
                _actionForSubmountQueue = new Queue<SubmountTransferInfo>();
                _actionForSubmountQueue = new Queue<SubmountTransferInfo>();
                //初始化完成
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                IsInitialized = false;
                LogRecorder.RecordLog(EnumLogContentType.Error, "Initial Job failed.", ex);
            }
        }
        /// <summary>
        /// 流程动作队列
        /// </summary>
        private Queue<ComponentTransferInfo> _actionForComponentQueue;
        /// <summary>
        /// 流程动作队列
        /// </summary>
        private Queue<SubmountTransferInfo> _actionForSubmountQueue;
        /// <summary>
        /// 运行WaferJob
        /// </summary>
        /// <param name="recipe"></param>
        public override void RunProcessJob(BondRecipe recipe, int index = 0, bool isTestRecipe = false)
        {
            ReportJobRunningStatus(EnumBondJobStatus.Running, "准备开始...");
            //------流程开始---------------
            this._statusChecker.Start();
            this._isCompleted = false;
            List<Tuple<int, int, PointF, float>> shellPositionInfo = new List<Tuple<int, int, PointF, float>>();
            this._currentRecipe = recipe;
            RunBeforeJob();

            Task.Factory.StartNew(new Action(() =>
            {
                this._currentRecipe = recipe;
                try
                {
                    if (UIOperation != null)
                    {
                        UIOperation();
                    }
                    //循环执行流程队列-根据状态和物料数量判断
                    while (_statusChecker.IsRunning)
                    {
                        switch (JobInfosManager.Instance.JobCurrentStatus)
                        {
                            case EnumJobRunStatus.None:
                                break;
                            case EnumJobRunStatus.SubstrateIncoming:
                                ExecuteBondJobAction(EnumBonderAction.LoadSubstrate);
                                break;
                            case EnumJobRunStatus.LoadingSubstrate:
                                break;
                            case EnumJobRunStatus.LoadSubstrateSuccess:
                                ExecuteBondJobAction(EnumBonderAction.PositionSubstrate);
                                break;
                            case EnumJobRunStatus.LoadSubstrateFailed:
                                break;
                            case EnumJobRunStatus.PositioningSubstrate:
                                break;
                            case EnumJobRunStatus.PositionSubstrateSuccess:
                                ExecuteBondJobAction(EnumBonderAction.SearchBondPosition);
                                break;
                            case EnumJobRunStatus.PositionSubstrateFailed:
                                break;
                            case EnumJobRunStatus.SearchingBondPosition:
                                break;
                            case EnumJobRunStatus.SearchBondPositionSuccess:
                                ExecuteBondJobAction(EnumBonderAction.DispenseGlue);
                                break;
                            case EnumJobRunStatus.SearchBondPositionFailed:
                                break;
                            case EnumJobRunStatus.DispensingGlue:
                                break;
                            case EnumJobRunStatus.DispenseGlueSuccess:
                                ExecuteBondJobAction(EnumBonderAction.PositionComponent);
                                break;
                            case EnumJobRunStatus.DispenseGlueFailed:
                                break;
                            case EnumJobRunStatus.PositioningComponent:
                                break;
                            case EnumJobRunStatus.PositionComponentSuccess:
                                ExecuteBondJobAction(EnumBonderAction.PickComponent);
                                break;
                            case EnumJobRunStatus.PositionComponentFailed:
                                //如果已搜索芯片的个数已超过配方Map里的物料个数，且贴装位置还有剩余，则支持人工介入重新指定新的Map初始位置
                                break;
                            case EnumJobRunStatus.PickingupComponent:
                                break;
                            case EnumJobRunStatus.PickComponentSuccess:
                                ExecuteBondJobAction(EnumBonderAction.AccuracyComponent);
                                break;
                            case EnumJobRunStatus.PickComponentFailed:
                                break;
                            case EnumJobRunStatus.AccuracyComponent:
                                break;
                            case EnumJobRunStatus.AccuracyComponentSuccess:
                                ExecuteBondJobAction(EnumBonderAction.Bond);
                                break;
                            case EnumJobRunStatus.AccuracyComponentFailed:
                                break;
                            case EnumJobRunStatus.Bonding:
                                break;
                            case EnumJobRunStatus.BondSuccess:
                                //贴装位置已贴满则执行基板下料
                                if(true)
                                {
                                    ExecuteBondJobAction(EnumBonderAction.UnloadSubstrate);
                                }
                                break;
                            case EnumJobRunStatus.BondFailed:
                                break;
                            case EnumJobRunStatus.AbandonComponent:
                                break;
                            case EnumJobRunStatus.AbandonComponentSuccess:
                                break;
                            case EnumJobRunStatus.AbandonComponentFailed:
                                break;
                            case EnumJobRunStatus.UnloadingSubstrate:
                                break;
                            case EnumJobRunStatus.UnloadSubstrateSuccess:
                                ExecuteBondJobAction(EnumBonderAction.UnloadSubstrate);
                                break;
                            case EnumJobRunStatus.UnloadSubstrateFailed:
                                break;
                            default:
                                break;
                        }



                        if(_actionForSubmountQueue.Count==0)
                        {
                            break;
                        }
                    }
                    MarkJobCompleted();
                    ReportJobRunningStatus(EnumBondJobStatus.ProcessCompleted, "流程完成.");
                    if (NotifyProcessCompleted != null)
                    {
                        NotifyProcessCompleted(JobResult);
                    }


                }
                catch (Exception ex)
                {
                    this.Abort();
                    LogRecorder.RecordLog(EnumLogContentType.Error, "Exception occured during job.", ex);
                    ReportJobRunningStatus(EnumBondJobStatus.ProcessError, "启动流程异常！");
                }
            }));
        }
        private void ManualSelectComponent()
        {

        }
        private void PlanComponentPath()
        {

        }
        public override void Abort()
        {
            base.Abort();

            TurnOffLights();
        }

        private void SetRumParameters()
        {
            
        }

        private void DoWeld()
        {

        }



        /// <summary>
        /// 等待下位发出的识别盖板的请求
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>



        private void PickLid()
        {
        }
        private void PlaceLid()
        {
        }
        private void SetPointWeldPowerParameter()
        {
            if(_powerController!=null)
            {
            }
            else
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Power Controller is NULL!");
            }
        }

        private object _visionSafeObj = new object();
        private AutoResetEvent _visionSafeEvent = new AutoResetEvent(true);
        public void ChangeCurrentCamera(EnumCameraType type)
        {
            if(BeforeCameraChangeAct!=null)
            {
                BeforeCameraChangeAct();
            }
            CameraManager.Instance.CurrentCameraType = type;
            if (AfterCameraChangeAct != null)
            {
                AfterCameraChangeAct();
            }
        }

        /// <returns></returns>
        private bool ExecuteBondJobAction(EnumBonderAction cmd)
        {
            switch (cmd)
            {
                case EnumBonderAction.LoadSubstrate:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.LoadingSubstrate;
                    //执行基板传送，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.LoadSubstrateSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.LoadSubstrateFailed;
                    break;
                case EnumBonderAction.PositionSubstrate:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.PositioningSubstrate;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.PositionSubstrateSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.PositionSubstrateFailed;
                    break;
                case EnumBonderAction.SearchBondPosition:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.SearchingBondPosition;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.SearchBondPositionSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.SearchBondPositionFailed;
                    break;
                case EnumBonderAction.DispenseGlue:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.DispensingGlue;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.DispenseGlueSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.DispenseGlueFailed;
                    break;
                case EnumBonderAction.PositionComponent:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.PositioningComponent;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.PositionComponentSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.PositionComponentFailed;
                    break;
                case EnumBonderAction.AccuracyComponent:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.AccuracyComponent;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.AccuracyComponentSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.AccuracyComponentFailed;
                    break;
                case EnumBonderAction.Bond:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.Bonding;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.BondSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.BondFailed;
                    break;
                case EnumBonderAction.AbandonComponent:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.AccuracyComponent;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.AbandonComponentSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.AbandonComponentFailed;
                    break;
                case EnumBonderAction.UnloadSubstrate:
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.UnloadingSubstrate;
                    //执行基板定位，
                    //执行成功 
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.UnloadSubstrateSuccess;
                    JobInfosManager.Instance.JobCurrentStatus = EnumJobRunStatus.UnloadSubstrateFailed;
                    break;
                default:
                    break;
            }
            return true;
        }
    }

}
