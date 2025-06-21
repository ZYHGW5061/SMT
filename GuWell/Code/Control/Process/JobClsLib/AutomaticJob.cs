using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using JobClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace JobClsLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AutomaticJob
    {
        #region 获取Job的唯一单例
        private static volatile AutomaticJob _instance;
        private static readonly object _lockObj = new object();
        public static AutomaticJob Instance
        {
            get
            {
                if(_instance==null)
                {
                    lock(_lockObj)
                    {
                        if(_instance==null)
                        {
                            _instance = new AutomaticJob();
                        }
                    }
                }
                return _instance;
            }

        }
        #endregion

        #region 成员

        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }


        /// <summary>
        /// 传片执行器
        /// </summary>
        private MaterialTransferExecutor _transferExecutor = null;
        /// <summary>
        /// WaferJob
        /// </summary>
        private ABondJob _currentBondJob = null;

        /// <summary>
        /// 获取当前Job
        /// </summary>
        public ABondJob CurrentBondJob
        {
            get { return _currentBondJob; }
            set { _currentBondJob = value; }
        }

        /// <summary>
        /// false:传片+检测模式 true:仅包含传片
        /// </summary>
        public bool IsOnlyTransferMode { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool IsCompleted
        {
            get { return _currentBondJob.IsJobCompleted(); }
        }


        /// <summary>
        /// 获取或设置当前WaferJob的LotId
        /// </summary>
        public string LotId
        {
            get { return _currentBondJob.LotId; }
            set { _currentBondJob.LotId = value; }
        }

        /// <summary>
        /// 获取或设置当前WaferJob的WaferId
        /// </summary>
        public string WaferId
        {
            get { return _currentBondJob.MaterialStripId; }
            set { _currentBondJob.MaterialStripId = value; }
        }

        /// <summary>
        /// 获取或设置当前WaferJob的SlotNumber
        /// </summary>
        public int WaferIndex
        {
            get { return _currentBondJob.MaterialStripIndex; }
            set { _currentBondJob.MaterialStripIndex = value; }
        }

        /// <summary>
        /// 获取当前WaferJob的原始图片存储路径
        /// </summary>
        public string RawDataSavingPath
        {
            get { return _currentBondJob.RawDataSavingPath; }
        }
        public string ProcessJobId
        {
            get { return _currentBondJob.JobId; }
        }
        /// <summary>
        /// 当前已传送的Wafer的数量
        /// </summary>
        public int TransferedWaferCount { get; set; }

        /// <summary>
        /// 检测信息集合
        /// </summary>
        public Queue<RunProcedureInfo> QueueRunProcedureInfo { get; set; }

        #endregion

        #region 委托
        /// <summary>
        /// 报告单片检测分析进度的委托
        /// </summary>
        public Action<int, int, int, int, int, int> WeldProgressReportAct
        {
            get { return _currentBondJob.WeldProgressReportAct; }
            set { _currentBondJob.WeldProgressReportAct = value; }
        }
        public Action<int[,]> UpdateLidsProcessStatus
        {
            get { return _currentBondJob.UpdateLidsProcessStatus; }
            set { _currentBondJob.UpdateLidsProcessStatus = value; }
        }
        
        public Action<int[,]> UpdateShellsProcessStatus
        {
            get { return _currentBondJob.UpdateShellsProcessStatus; }
            set { _currentBondJob.UpdateShellsProcessStatus = value; }
        }
        /// <summary>
        /// 报告单片检测扫描进度的委托
        /// </summary>
        public Action<int, int, int[]> ReportScanningProgressAct
        {
            get { return _currentBondJob.ReportMeasureProgressAct; }
            set { _currentBondJob.ReportMeasureProgressAct = value; }
        }
        public Action<JobResult> NotifyProcessCompletedAct
        {
            get { return _currentBondJob.NotifyProcessCompleted; }
            set { _currentBondJob.NotifyProcessCompleted = value; }
        }
        /// <summary>
        /// 报告Job运行状态的委托
        /// </summary>
        public Action<EnumBondJobStatus, string> SendJobStatusAct { get; set; }

        public Action<EnumTransferStatus,int, string> SendTransferMaterialAct { get; set; }

        /// <summary>
        /// 报告当前已传送的Wafer的数量
        /// </summary>
        public Action<int> ReportTransferedWaferCount { get; set; }


        /// <summary>
        /// 报告当前检测完的wafer结果
        /// </summary>
        //public Action<WaferResultReportData> ReportWaferResultAct { get; set; }

        public Action<BondRecipe, int> JobStartAct
        {
            get;
            set;
        }


        /// <summary>
        /// ProcessingState状态事件改变
        /// </summary>
        public Action<EnumProcessingState> ReportProcessingStateChanged { get; set; }

		/// <summary>
        /// 添加一条job到队列回调
        /// </summary>
        public Func<RunProcedureInfo,Boolean> AddProcedureInfoAction { get; set; }


        /// <summary>
        /// Job开始执行时启动计时器
        /// </summary>
        public Action StartLotjobCostedtimerAct { get; set; }

        public Action BeforeCameraChangeAct
        {
                get { return _currentBondJob.BeforeCameraChangeAct; }
                set { _currentBondJob.BeforeCameraChangeAct = value; }
        }
        public Action AfterCameraChangeAct
        {
            get { return _currentBondJob.AfterCameraChangeAct; }
            set { _currentBondJob.AfterCameraChangeAct = value; }
        }
        public Action BeforeCameraGrabOneAct
        {
            get { return _currentBondJob.BeforeCameraGrabOneAct; }
            set { _currentBondJob.BeforeCameraGrabOneAct = value; }
        }
        public Action AfterCameraGrabOneAct
        {
            get { return _currentBondJob.AfterCameraGrabOneAct; }
            set { _currentBondJob.AfterCameraGrabOneAct = value; }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        private AutomaticJob()
        {
            _transferExecutor = new MaterialTransferExecutor();
            //RunWaferJob的回调
            _transferExecutor.RequestWeldAct += new Action<BondRecipe, int, bool>(OnRequestStartJobAct);
            _transferExecutor.ReportTransferStatusAct += new Action<EnumTransferStatus,int, string>(OnHandleJobTransferStatus);
            //确认检测完成的回调
            _transferExecutor.ConfirmWaferJobCompletedAct += new Action<RunStatusController>(OnConfirmWeldJobCompletedAct);
            //执行wafer信息队列
            if (QueueRunProcedureInfo == null)
            {
                QueueRunProcedureInfo = new Queue<RunProcedureInfo>();
            }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 开始自动化扫描检测流程，包含自动循环传片 + 工艺。
        /// </summary>
        /// <param name="wafers">Wafer集合</param>
        /// <param name="currentRecipe"></param>
        /// <param name="lotId"></param>
        public void StartAutoCycleJobs(Queue<JobMaterialInfo> wafers, bool isOnlyTransferWafer = false)
        {
            //是否包含检测过程
            IsOnlyTransferMode = isOnlyTransferWafer;
            if (_transferExecutor != null)
            {
                _transferExecutor.ExecuteTransfer(wafers, isOnlyTransferWafer);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialStrips"></param>
        /// <param name="index"></param>
        /// <param name="isFirstMaterial"></param>
        public void StartManualJob(Queue<JobMaterialInfo> materialStrips, int index, bool isTestRecipe)
        {
            //传递当前Recipe
            OnRequestStartJobAct(materialStrips.Dequeue().ProcessRecipe, index, isTestRecipe);
        }

        /// <summary>
        /// 人为终止检测流程
        /// </summary>
        public void Abort()
        {
            //人为终止当前检测Job
            if (_currentBondJob != null && _currentBondJob.IsJobRunning())
            {
                _currentBondJob.Abort();
            }
            if(_transferExecutor != null && _transferExecutor.IsRunning)
            {
                _transferExecutor.Abort();
            }
        }





        /// <summary>
        /// 初始化Job
        /// </summary>
        public void InitializeJob()
        {
            try
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "Job is initializing.");
                _currentBondJob = new AutoBondJob();
                _currentBondJob.InitialJob();
                if (!_currentBondJob.SendJobStatusAct.CheckDelegateRegistered((Action<EnumBondJobStatus, string>)OnHandleJobStatus))
                {
                    _currentBondJob.SendJobStatusAct += new Action<EnumBondJobStatus, string>(OnHandleJobStatus);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Excception occured while initializing Job.", ex);
            }
        }       

        /// <summary>
        /// 请求下一片，直到取完
        /// </summary>
        /// <returns>请求过程中是否停止了流程</returns>
        public bool RequestLoadNextMaterial()
        {
            if (_currentBondJob != null)
            {
                _currentBondJob.MarkJobCompleted();
            }

            //报告状态
            if (SendJobStatusAct != null) { SendJobStatusAct(EnumBondJobStatus.Running, "Unloading Material..."); }

            if (_transferExecutor != null)
            {

                if (!_transferExecutor.IsAbort)
                {
                    //通知传片器取当前片并开始请求下一片，直至取完
                    _transferExecutor.Resume();
                }
                else
                {
                    //_systemLogger.AddDebugContent("User Abort Job While PrepareToUnLoadWafer.");
                }
                return true;
            }
            else
            {
                throw new NotSupportedException("No transferExecutor exist.");
            }
        }

        /// <summary>
        /// 添加到队列
        /// </summary>
        /// <param name="info"></param>
        public bool AddProcedureInfo(RunProcedureInfo info)
        {
            if (QueueRunProcedureInfo == null)
            {
                QueueRunProcedureInfo = new Queue<RunProcedureInfo>();
            }
            QueueRunProcedureInfo.Enqueue(info);
            return true;
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void ClearProcedureInfo()
        {
            QueueRunProcedureInfo.Clear();
        }

        public void RunProcedure(int isManual = 0, bool isTestRecipe = false)
        {
            Queue<JobMaterialInfo> selectStrips = new Queue<JobMaterialInfo>();
            if (isManual == 0)
            {
                while (QueueRunProcedureInfo.Count > 0)
                {
                    var info = QueueRunProcedureInfo.Dequeue();
                    while (info.SelectMaterials.Count > 0)
                    {
                        selectStrips.Enqueue(info.SelectMaterials.Dequeue());
                    }
                    JobInfosManager.Instance.CurrentJobMaterialStripSourceInfo = new MaterialStripSourceInfo
                    {
                        LotId = info.LotID,
                        MaterialStripId = info.MaterialStripID,
                        SourceSlotIndex = info.SourceSlotIndex
                    };
                }
                StartManualJob(selectStrips, isManual, isTestRecipe);
            }
            else
            {
                while (QueueRunProcedureInfo.Count > 0)
                {
                    var info = QueueRunProcedureInfo.Dequeue();
                    while (info.SelectMaterials.Count > 0)
                    {
                        //selectStrip.Enqueue(info.SelectMaterials.Dequeue());
                        selectStrips.Enqueue(info.SelectMaterials.Dequeue());
                    }
                    JobInfosManager.Instance.CurrentJobMaterialStripSourceInfo = new MaterialStripSourceInfo
                    {
                        LotId = info.LotID,
                        MaterialStripId = info.MaterialStripID,
                        SourceSlotIndex = info.SourceSlotIndex
                    };
                }
                StartAutoCycleJobs(selectStrips, false);
            }

        }

        #endregion

        

        /// <summary>
        /// 处理检测过程中返回的状态并报告到上层
        /// </summary>
        /// <param name="status"></param>
        /// <param name="progress"></param>
        private void OnHandleJobStatus(EnumBondJobStatus status, string progress)
        {
            switch (status)
            {
                case EnumBondJobStatus.ProcessError:             //检测失败终止传输过程  
                case EnumBondJobStatus.VisionFailed:          //调平失败
                case EnumBondJobStatus.ProcessCompleted:           //一片完成，不需要弹框提示，进度栏提示
                case EnumBondJobStatus.Running:                       //UI通知，进度栏提示
                    break;
            }
            if (SendJobStatusAct != null)
            {
                SendJobStatusAct(status, progress);
            }
        }
        private void OnHandleJobTransferStatus(EnumTransferStatus status,int slotIndex, string progress)
        {
            var jobStatus = EnumBondJobStatus.Idle;
            switch (status)
            {
                case EnumTransferStatus.TransferCompleted:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.Transfering:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.ExceptionAborted:
                    break;
                case EnumTransferStatus.UserAbort:
                    break;
                case EnumTransferStatus.StripCleared:
                    break;
                case EnumTransferStatus.StripLoaded:
                    break;
                case EnumTransferStatus.MaterialboxinBuffer:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.MaterialboxinExchangebox:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.MaterialboxinMaterialboxTrack:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.MaterialboxinOven:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.MaterialstripinMaterialstripTrack:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.MaterialstripinWeld:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                case EnumTransferStatus.NoMateralboxNoMateralstrip:
                    jobStatus = EnumBondJobStatus.Running;
                    break;
                default:
                    break;
            }

            if (SendJobStatusAct != null)
            {
                SendJobStatusAct(jobStatus, progress);
            }
            if (SendTransferMaterialAct != null)
            {
                SendTransferMaterialAct(status, slotIndex, progress);
            }
        }


        /// <summary>
        /// Wafer传输完成时
        /// </summary>
        /// <param name="runStatusController"></param>
        private void OnWaferTransferCompletedAct()
        {
            if (ReportTransferedWaferCount != null)
            {
                TransferedWaferCount++;
                ReportTransferedWaferCount(TransferedWaferCount);
            }
        }

        /// <summary>
        /// 请求开始
        /// </summary>
        private void OnRequestStartJobAct(BondRecipe currentRecipe,int index,bool isTestRecipe)
        {
            if (IsOnlyTransferMode)
            {
                //模拟检测
                Sleep10000();
            }
            else
            {
                if (_currentBondJob != null)
                {
                    if (JobStartAct != null)
                    {
                        JobStartAct(currentRecipe, index);
                    }
                    _currentBondJob.RunProcessJob(currentRecipe, index, isTestRecipe);
                    _currentBondJob.JobId = $"WeldJob{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                }
                else
                {
                    WarningBox.FormShow("错误", "Job 对象为空！", "提示");
                }
            }
        }

        /// <summary>
        /// 模拟检测过程
        /// </summary>
        private async void Sleep10000()
        {
            await Task.Run(new Action(() =>
            {
                if (SendJobStatusAct != null) { SendJobStatusAct(EnumBondJobStatus.Running, "Simulate Inspecting..."); }

                //睡眠10秒
                Thread.Sleep(4000);

                //请求下一片
                RequestLoadNextMaterial();
            }));
        }

        private void OnRequestPrepareJobAct(BondRecipe currentRecipe, bool isFirstWafer)
        {
            if (IsOnlyTransferMode) return;
            _currentBondJob.PrepareJob(currentRecipe, isFirstWafer);
        }
        /// <summary>
        /// 获取当前Wafer是否已完成检测
        /// </summary>
        public bool IsJobCompleted
        {
            get { return _currentBondJob.IsJobCompleted(); }
        }
        /// <summary>
        /// 取片前确认当前片检测是否完成
        /// </summary>
        /// <param name="runStatusController"></param>
        private void OnConfirmWeldJobCompletedAct(RunStatusController runStatusController)
        {
            if (!IsJobCompleted)
            {
                LogRecorder.RecordLog(EnumLogContentType.Debug, "Working now ,waiting..");
            }
            else
            {
                //to do: 检测已完成则开始取片
                LogRecorder.RecordLog(EnumLogContentType.Debug, "Job completed , continue..");
            }
        }

    }
}
