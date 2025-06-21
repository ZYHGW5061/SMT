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
using System.Diagnostics;
using System.IO;
using GlobalDataDefineClsLib;
using WestDragon.Framework;
using WestDragon.Framework.BaseLoggerClsLib;
using RecipeClsLib;
using WestDragon.Framework.UtilityHelper;
using ConfigurationClsLib;
using CameraControllerClsLib;
using System.Drawing.Imaging;
using System.Threading;
using CommonPanelClsLib;
using JobClsLib;
using GlobalToolClsLib;

namespace ControlPanelClsLib
{
    public partial class FrmExecuteRecipe : BaseForm
    {

        /// <summary>
        /// 是否人为点击Abort
        /// </summary>
        private bool _isAbort;

        string slotMap = "";
        public Action<EnumProcessStatus, string> OnShowProcessMessageAct { get; set; }
        public Action<string> OnRunningModeChanged { get; set; }
        public Action<bool> EnableNavigationButtonsAct { get; set; }
        private BondRecipe _currentRecipe;
        private string _currentLotID;
        private string _currentMaterialStripID;
        private string _currentSlotIndex;
        private DateTime _startTime;
        private int _marathonCounter = 0;
        /// <summary>
        /// 缓存队列机制,负责显示图片
        /// </summary>
        private BufferQueueMechanismOperation<byte[]> _dataBufferQueue;
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        private CameraConfig _cameraConfig
        {
            get { return CameraManager.Instance.CurrentCameraConfig; }
        }
        private ICameraController _currentCamera
        {
            get { return CameraManager.Instance.CurrentCamera; }
        }
        private bool _isMarathonJob;
        AutomaticJob _autoJob;
        private bool _isAutoJob = false;
        private List<JobMaterialInfo> _selectStripsForWeld = new List<JobMaterialInfo>();
        MaterialTransferExecutor _quickloadTransfer = new MaterialTransferExecutor();
        MaterialTransferExecutor _clearStripTransfer = new MaterialTransferExecutor();
        public FrmExecuteRecipe()
        {
            CreateWaitDialog();
            InitializeComponent();
            this.InitControls();
            base.SetStyle(ControlStyles.Selectable, true);
            base.UpdateStyles();
            InitializeEvents();
            InitProcessJob();

            if (_systemConfig.SystemRunningType == EnumRunningType.Actual)
            {
                InitializeIOStatus();
                RegisterIOChangedAct();
            }
            CloseWaitDialog();
        }
        private void InitControls()
        {


        }
        private void InitializeEvents()
        {
        }
        private void InitProcessJob()
        {
            try
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "Job is initializing.");
                _autoJob = AutomaticJob.Instance;
                _autoJob.InitializeJob();
                if (!_autoJob.WeldProgressReportAct.CheckDelegateRegistered((Action<int, int, int, int,int, int>)OnReportWeldProgressAct))
                {
                    _autoJob.WeldProgressReportAct += new Action<int, int, int, int, int, int>(OnReportWeldProgressAct);
                }
                if (!_autoJob.SendJobStatusAct.CheckDelegateRegistered((Action<EnumBondJobStatus, string>)OnReportJobStatus))
                {
                    _autoJob.SendJobStatusAct += OnReportJobStatus;

                }
                if (!_autoJob.SendTransferMaterialAct.CheckDelegateRegistered((Action<EnumTransferStatus,int, string>)OnReportTransferStatus))
                {
                    _autoJob.SendTransferMaterialAct += OnReportTransferStatus;

                }
                if (!_autoJob.JobStartAct.CheckDelegateRegistered((Action<BondRecipe, int>)OnJobStartAct))
                {
                    _autoJob.JobStartAct += OnJobStartAct;
                }
                if (!_autoJob.NotifyProcessCompletedAct.CheckDelegateRegistered((Action<JobResult>)OnNotifyJobCompleted))
                {
                    _autoJob.NotifyProcessCompletedAct += OnNotifyJobCompleted;
                }
                //if (!_autoJob.BeforeCameraChangeAct.CheckDelegateRegistered((Action)BeforeCameraChangeAct))
                //{
                //    _autoJob.BeforeCameraChangeAct += BeforeCameraChangeAct;
                //}
                //if (!_autoJob.AfterCameraChangeAct.CheckDelegateRegistered((Action)AfterCameraChangeAct))
                //{
                //    _autoJob.AfterCameraChangeAct += AfterCameraChangeAct;
                //}

                //if (!_autoJob.BeforeCameraGrabOneAct.CheckDelegateRegistered((Action)BeforeCameraGrabOneAct))
                //{
                //    _autoJob.BeforeCameraGrabOneAct += BeforeCameraGrabOneAct;
                //}

                //if (!_autoJob.AfterCameraGrabOneAct.CheckDelegateRegistered((Action)AfterCameraGrabOneAct))
                //{
                //    _autoJob.AfterCameraGrabOneAct += AfterCameraGrabOneAct;
                //}
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Excception occured while initializing Job.", ex);
            }
        }
        private void InitMonitorViewer()
        {
            try
            {
                _dataBufferQueue = new BufferQueueMechanismOperation<byte[]>();
                _dataBufferQueue.ContinuationAction = () => { };
                _dataBufferQueue.ProcessAction = x =>
                {
                    try
                    {
                        if (!this.IsHandleCreated || this.IsDisposed)
                        {
                            _dataBufferQueue.Stop();
                            UnregisterCameraEventHandler();
                            return;
                        }

                        if (!this.Visible)
                        {
                            return;
                        }
                        Bitmap bitmap;
                        if (_cameraConfig.ImageType == EnumImageData.Grey)
                        {
                            bitmap = BitmapFactory.BytesToBitmapFast(x, _cameraConfig.ImageSizeWidth, _cameraConfig.ImageSizeHeight, PixelFormat.Format8bppIndexed);
                        }
                        else
                        {
                            bitmap = BitmapFactory.BytesToBitmapFast(x, _cameraConfig.ImageSizeWidth, _cameraConfig.ImageSizeHeight, PixelFormat.Format24bppRgb);
                        }
                        ShowBitmap(bitmap);
                    }
                    catch (Exception ex)
                    {
                        //_systemGlobalLogger.AddErrorContent("显示图片过程异常", ex);
                    }
                };
                _dataBufferQueue.Start();

            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "InitMonitorViewer,Error.", ex);
            }
        }
        /// <summary>
        /// 报告当前Job运行的状态
        /// </summary>
        /// <param name="status">Job Status</param>
        /// <param name="progressinfo">进度信息</param>
        private void OnReportJobStatus(EnumBondJobStatus status, string progressinfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)delegate { OnReportJobStatus(status, progressinfo); });
                return;
            }
            switch (status)
            {
                case EnumBondJobStatus.Idle:                          //初始化为未开始的状态                   
                    NotifyJobStatusToView(EnumProcessStatus.Idle, progressinfo);
                    break;
                case EnumBondJobStatus.ProcessError:
                    RefreshControlsEnable(true);
                    UnregisterCameraEventHandler();
                    NotifyJobStatusToView(EnumProcessStatus.Aborted, progressinfo);
                    break;
                case EnumBondJobStatus.TransferError:
                    RefreshControlsEnable(true);
                    UnregisterCameraEventHandler();
                    NotifyJobStatusToView(EnumProcessStatus.Aborted, progressinfo);
                    break;
                case EnumBondJobStatus.VisionFailed:
                    NotifyJobStatusToView(EnumProcessStatus.Running, progressinfo);
                    break;
                case EnumBondJobStatus.UserAbort:
                    RefreshControlsEnable(true);
                    UnregisterCameraEventHandler();
                    NotifyJobStatusToView(EnumProcessStatus.Aborted, progressinfo);
                    break;
                case EnumBondJobStatus.ProcessCompleted:
                    if (!_isAutoJob)
                    {
                        RefreshControlsEnable(true);
                    }
                    UnregisterCameraEventHandler();
                    NotifyJobStatusToView(EnumProcessStatus.Completed, progressinfo);
                    if(_isMarathonJob)
                    {
                        //if (_systemConfig.IsCounterMarathon)
                        //{
                        //    if (_marathonCounter < _systemConfig.MarathonCount)
                        //    {
                        //        Thread.Sleep(10000);
                        //        StartMarathonProcedure();
                        //    }
                        //}
                        //else
                        //{
                        //    Thread.Sleep(10000);
                        //    StartMarathonProcedure();
                        //}
                    }
                    NotifyJobStatusToView(EnumProcessStatus.Idle, "");
                    break;
                case EnumBondJobStatus.TransferCompleted:
                    RefreshControlsEnable(true);
                    break;
                default:                                          
                    NotifyJobStatusToView(EnumProcessStatus.Running, progressinfo);
                    break;
            }

            Application.DoEvents();
        }
        private void OnReportTransferStatus(EnumTransferStatus status,int slotIndex, string progressinfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { OnReportTransferStatus(status, slotIndex, progressinfo); });
                return;
            }
            switch (status)
            {
                case EnumTransferStatus.TransferCompleted:
                    break;
                case EnumTransferStatus.StripTransferCompleted:
                    break;
                case EnumTransferStatus.Transfering:
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

                    break;
                case EnumTransferStatus.MaterialboxinExchangebox:

                    break;
                case EnumTransferStatus.MaterialboxinMaterialboxTrack:

                    break;
                case EnumTransferStatus.MaterialboxinOven:

                    break;
                case EnumTransferStatus.MaterialstripinMaterialstripTrack:

                    break;
                case EnumTransferStatus.MaterialstripinWeld:

                    break;
                case EnumTransferStatus.NoMateralboxNoMateralstrip:

                    break;
                default:
                    break;
            }
        }
        private void OnJobStartAct(BondRecipe recipe, int index)
        {
            this.Invoke(new Action(() =>
            {
                progressBarCtrl.Position = 0;
                progressBarCtrl.Visible = true;
            }));
        }
        /// <summary>
        /// 通知当前页面Job正在运行。
        /// </summary>
        private void NotifyJobStatusToView(EnumProcessStatus jobStatus, string info)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { NotifyJobStatusToView(jobStatus, info); });
                return;
            }
            if (jobStatus == EnumProcessStatus.Aborted)
            {
                //progressPanel1.Caption = "Preparing...";
                progressBarCtrl.Position = 0;
                //progressPanel1.Visible = false;
                progressBarCtrl.Visible = false;

            }
            else if (jobStatus == EnumProcessStatus.Completed)
            {

                progressBarCtrl.Position = 0;
                progressBarCtrl.Visible = false;

            }
            if (OnShowProcessMessageAct!=null)
            {
                LogRecorder.RecordModuleLog(EnumLogContentType.Info, "Process", info);
                OnShowProcessMessageAct(jobStatus, info);
            }
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {

        }

        /// <summary>
        /// 取消事件注册
        /// </summary>
        private void UnRegisterEvents()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lidorshell">1:shell,2:lid</param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="result"></param>
        /// <param name="totalRows"></param>
        /// <param name="totalColumns"></param>
        private void OnReportWeldProgressAct(int lidorshell, int rowIndex,int columnIndex,int result, int totalRows,int totalColumns)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    if (IsDisposed) return;  // 如果窗体已经关闭，直接返回。
                    this.Invoke((MethodInvoker)delegate { OnReportWeldProgressAct(lidorshell,rowIndex, columnIndex,result,totalRows,totalColumns); });
                    return;
                }
                LogRecorder.RecordLog(EnumLogContentType.Error, $"OnReportWeldProgressAct,lidorshell:{lidorshell},rowIndex:{rowIndex},columnIndex:{columnIndex},result:{result}.");
                //this.progressPanel2.Caption = string.Format("Analysising...{0}%", Math.Round(index * 100f / totalCount, 2).ToString("0.0"));
                var index=(columnIndex-1)* totalRows + rowIndex;
                var totalCount = totalRows * totalColumns;
                this.progressBarCtrl.Position = (int)Math.Round(index * 100f / totalCount, 2);
                if(lidorshell==1)
                {
                    //RefreshShellMapStatus(rowIndex, columnIndex, result);
                }
                else if(lidorshell==2)
                {
                    //RefreshLidMapStatus(rowIndex, columnIndex, result);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "OnReportWeldProgressAct,Error.", ex);
            }
        }
        /// <summary>
        /// 通知当前页面Job已完成
        /// </summary>
        /// <param name="resultPath"></param>
        /// <param name="measureResult"></param>
        private async void OnNotifyJobCompleted(JobResult jobResult)
        {
            try
            {
                if (jobResult != null)
                {
                    await Task.Factory.StartNew(new Action(() =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            CreateWaitDialog();
                            if (_autoJob != null)
                            {
                                if (_isAutoJob)
                                {
                                    _autoJob.RequestLoadNextMaterial();
                                }
                            }

                        }));
                    }));
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "OnNotifyJobCompleted,Error.", ex);
            }
            finally
            {
                CloseWaitDialog();
            }
        }


        private void EnableControlsByProcessStatus(bool enabled)
        {

        }

        

        /// <summary>
        /// 通知注册事件的回调
        /// </summary>
        private void OnNotifyRegisterEventsAct()
        {
            this.RegisterEvents();
        }

        /// <summary>
        /// 执行新的recipe步骤前的操作
        /// </summary>
        private void OnStartNewRecipeStepAct()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { OnStartNewRecipeStepAct(); });
                return;
            }
            //this.waferMapControl1.Clear();
            //this._defectPoistionInfoList.Clear();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {

            if(WarningBox.FormShow("动作确认", "将要终止当前流程...","提示") ==1)
            {
                if (_autoJob != null)
                {
                    _autoJob.Abort();
                    RefreshControlsEnable(true);
                    LogRecorder.RecordModuleLog(EnumLogContentType.Info, "Main", "用户手动终止流程.");
                    OnReportJobStatus(EnumBondJobStatus.UserAbort, "User Abort.");
                }
            }
        }

        private string _selectedRecipeName = string.Empty;
        private string _selectedHeatRecipeName = string.Empty;
        private void btnSelectRecipe_Click(object sender, EventArgs e)
        {
            //LogRecorder.RecordLog(EnumLogContentType.Info,string.Format("JobControlPanel: User clicked <{0}> Button", (sender as Control).Text));
            ////选择一个recipe
            //FrmRecipeSelect selectRecipeDialog = new FrmRecipeSelect(null, this.txtRecipeName.Text.ToUpper().Trim());
            //if (selectRecipeDialog.ShowDialog(this.FindForm()) == DialogResult.OK)
            //{
            //    try
            //    {
            //        _selectedRecipeName = selectRecipeDialog.SelectedRecipeName;
            //        //验证Recipe是否完整
            //        if (!ProcessRecipe.Validate(_selectedRecipeName, selectRecipeDialog.RecipeType))
            //        {
            //            WarningBox.FormShow("错误！", "配方无效！", "Error");
            //            return;
            //        }
            //        else
            //        {
            //            txtRecipeName.Text = selectRecipeDialog.SelectedRecipeName;
            //            var selectRecipe = ProcessRecipe.LoadRecipe(_selectedRecipeName, EnumRecipeType.Welder);
            //            seLongSideWeldPress.Text = selectRecipe.ProcessParameters.WeldParameters.LongSideWeldZPosPressure.ToString();
            //            seShortSideWeldPress.Text= selectRecipe.ProcessParameters.WeldParameters.ShortSideWeldZPosPressure.ToString();
            //            seLongSideWeldHeadCompensation.Text=selectRecipe.ProcessParameters.WeldParameters.YStartPosComponsationForLongSideWeld.ToString();
            //            labelLongSideWeldTailCompensation.Text=selectRecipe.ProcessParameters.WeldParameters.YEndPosComponsationForLongSideWeld.ToString();
            //            labelShortSideWeldHeadCompensation.Text=selectRecipe.ProcessParameters.WeldParameters.YStartPosComponsationForShortSideWeld.ToString();
            //            seShortSideWeldTailCompensation.Text=selectRecipe.ProcessParameters.WeldParameters.YEndPosComponsationForShortSideWeld.ToString();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogRecorder.RecordLog(EnumLogContentType.Error, "JobControlPanel: Exception occured while Loading Recipe.", ex);
            //    }
            //}
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedRecipeName))
            {
                WarningBox.FormShow("错误！", "所选配方为空！", "Error");
                return;
            }
            this.RegisterEvents();
            var currentRecipe = BondRecipe.LoadRecipe(_selectedRecipeName);
            _currentRecipe = currentRecipe;
            //FrmStartJob newStartJob = (FrmStartJob)Application.OpenForms["FrmStartJob"];
            //if (newStartJob == null)
            //{
            //    newStartJob = new FrmStartJob(false);
            //}
            //else
            //{
            //    newStartJob.Activate();
            //}

            //if (newStartJob.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {
            //        _marathonCounter = 1;
            //        RefreshControlsEnable(false);
            //        _isAutoJob = false;
            //        //状态增加当前slot index
            //        this.labelMainStatus.Text = $"当前配方:{_selectedRecipeName}; 料盒Id:{newStartJob.LotId}; 料条Id:{newStartJob.MaterialStripID}";
            //        LogRecorder.RecordModuleLog(EnumLogContentType.Info
            //            , "Main", $"手动开始工艺.配方:{_selectedRecipeName};料盒Id:{newStartJob.LotId}; 料条Id:{newStartJob.MaterialStripID}");
            //        LogRecorder.RecordLog(EnumLogContentType.Info, $"手动开始工艺.配方:{_selectedRecipeName};料盒Id:{newStartJob.LotId}; 料条Id:{newStartJob.MaterialStripID}");
            //        _isMarathonJob = newStartJob.IsMarathonJob;
            //        if (OnRunningModeChanged != null)
            //        {
            //            if (_isMarathonJob)
            //            {
            //                if (_systemConfig.IsCounterMarathon)
            //                {
            //                    OnRunningModeChanged($"马拉松运行模式：{_marathonCounter}/{_systemConfig.MarathonCount}");
            //                }
            //                else
            //                {
            //                    OnRunningModeChanged($"马拉松运行模式：{_marathonCounter}");
            //                }
            //            }
            //            else
            //            {
            //                OnRunningModeChanged($"正常运行模式");
            //            }
            //        }
            //        _marathonCounter++;
            //        //添加新的跑片信息
            //        var procedureInfo = new RunProcedureInfo();
            //        //procedureInfo.UseLoadPort = EnumLoadPort.LoadPort1;
            //        procedureInfo.isSelectAll = true;
            //        procedureInfo.LotID = newStartJob.LotId;
            //        procedureInfo.MaterialStripID = newStartJob.MaterialStripID;
            //        procedureInfo.SourceSlotIndex = "01";
            //        _currentLotID = newStartJob.LotId;
            //        _currentMaterialStripID = newStartJob.MaterialStripID;
            //        _currentSlotIndex = "01";
            //        JobMaterialStripInfo manualJobWaferInfo = new JobMaterialStripInfo()
            //        {
            //            Index = 1,
            //            ProcessRecipe = currentRecipe,
            //            RecipeName = currentRecipe.RecipeName,
            //            LotID = procedureInfo.LotID,
            //            Strip = new MaterialStripInfo() { StripID = newStartJob.MaterialStripID, StripIndex = "1", MaterialBoxID = "1" }
            //        };
            //        Queue<JobMaterialStripInfo> selectLids = new Queue<JobMaterialStripInfo>();
            //        selectLids.Enqueue(manualJobWaferInfo);
            //        procedureInfo.SelectMaterials = selectLids;
            //        AddProcedureInfo(procedureInfo);
            //        _startTime = DateTime.Now;
            //        _autoJob.RunProcedure(1);
            //    }
            //    catch (Exception ex)
            //    {
            //        LogRecorder.RecordLog(EnumLogContentType.Error, "Run Process Error.", ex);
            //        WarningBox.FormShow("错误", "下发Job失败！", "提示");
            //    }
            //    finally
            //    {
            //        //RefreshControlsEnable(true);
            //    }

            //}
        }
        /// <summary>
        /// 将要检测的RunProcedureInfo 添加到队列 并更新gridControlWaferResults
        /// </summary>
        /// <param name="runInfo"></param>
        private bool AddProcedureInfo(RunProcedureInfo runInfo)
        {
            if (this.InvokeRequired)
            {
                bool ret = false;
                this.Invoke((MethodInvoker)delegate { ret = AddProcedureInfo(runInfo); });
                return ret;
            }
            if (_autoJob.QueueRunProcedureInfo == null)
            {
                _autoJob.QueueRunProcedureInfo = new Queue<RunProcedureInfo>();
            }
            this._isAbort = false;
            //判断队列里是否已经包含待检测
            var runinfoList = runInfo.SelectMaterials.ToList();
            _selectStripsForWeld= runInfo.SelectMaterials.ToList();
            _autoJob.QueueRunProcedureInfo.Enqueue(runInfo);
            return true;
        }
        private static void ChangeImageName(string srcImageName, string destImageName)
        {
            while (true)
            {
                if (System.IO.File.Exists(srcImageName))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(srcImageName);
                    file.MoveTo(destImageName);
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        //private List<DefectPositionInfo> _defectPoistionInfoList = new List<DefectPositionInfo>();



        private void AddCrossHair(PointF point)
        {
            ////删除上次点击生成的十字星
            //this.waferMapControl1.DeleteLastCrossFigure();

            ////在选中缺陷的中心位置画十字星
            //this.waferMapControl1.AddCrossHairFigure(point.X, point.Y);
        }

        private void MoveStageToCaptureImage(PointF imageCenter)
        {

        }

        

        private void btnTest_Click(object sender, EventArgs e)
        {

            Task.Factory.StartNew(() =>
            {
                int coutner = 0;
                this._isAbort = false;
                while (true)
                {

                    System.Threading.Thread.Sleep(20000);
                    if (_isAbort) break;
                    if (coutner > 1000) break;
                }
            });

        }

        private void btnTestAbort_Click(object sender, EventArgs e)
        {
            this._isAbort = true;
        }










        private void JobControlPanel_Enter(object sender, EventArgs e)
        {
            //SystemMonitor.GetHandler().StartMonitoring();
        }

        private void JobControlPanel_Load(object sender, EventArgs e)
        {
            DefineMaterialStatusLabelColor();
        }
        private void DefineMaterialStatusLabelColor()
        {

        }
        private void btnResult_Click(object sender, EventArgs e)
        {
        }
        private void RefreshControlsEnable(bool enabled)
        {
            if(this.InvokeRequired)
            {
                this.Invoke((Action)delegate { RefreshControlsEnable(enabled); });
                return;
            }

            this.btnSelectRecipe.Enabled = enabled;
            if(EnableNavigationButtonsAct!=null)
            {
                EnableNavigationButtonsAct(enabled);
            }
        }
        /// <summary>
        /// 显示图像.
        /// </summary>
        /// <param name="bitmap"></param>
        public void ShowBitmap(Bitmap bitmap, bool showCrosshair = true)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() => ShowBitmap(bitmap, showCrosshair)));
                return;
            }

            if (!this.Visible)
            {
                return;
            }

            if (showCrosshair)
            {

                //this.canvasDisplay1.Image = bitmap;
                ////AddMarker();
                //this.canvasDisplay1.ZoomToRectangleWorld();
                //return;
            }
            //this.CanvasDisplay.Image = bitmap;
            //this.CanvasDisplay.ZoomToRectangleWorld();
        }
        /// <summary>
        /// 注册事件绑定.
        /// </summary>
        private void RegisterCameraEventHandler()
        {
            if (_currentCamera != null && !_currentCamera.SingleImageDataAcquiredAction.CheckDelegateRegistered((Action<byte[]>)ReceivedCameraData))
            {
                _currentCamera.SingleImageDataAcquiredAction += ReceivedCameraData;
            }
        }
        private void ReceivedCameraData(byte[] receiveImage)
        {
            //_dataBufferQueue.EnqueueNewData(receiveImage);

        }
        /// <summary>
        /// Unregister the camera handler method.
        /// </summary>
        private void UnregisterCameraEventHandler()
        {
            if (_currentCamera != null && _currentCamera.SingleImageDataAcquiredAction.CheckDelegateRegistered((Action<byte[]>)ReceivedCameraData))
            {
                _currentCamera.SingleImageDataAcquiredAction -= ReceivedCameraData;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //FrmRunProcedure prmPorcedure = new FrmRunProcedure(null);
            //// prmPorcedure.AddProcedureInfo += AddProcedureInfo;
            //prmPorcedure.AddRunProcedureInfoDelegate += AddProcedureInfo;
            //var show = prmPorcedure.ShowDialog();
            ////prmPorcedure.AddProcedureInfo -= AddProcedureInfo;
            //prmPorcedure.AddRunProcedureInfoDelegate -= AddProcedureInfo;
            //if (show == DialogResult.OK)
            //{
            //    //双Loadport执行时，只执行一次RunJob，后面的执行AddRunJob。
            //    //if (_autoJob.IsTransferingWafer || _autoJob.IsInspectingWafer)
            //    //{
            //    //    return;
            //    //}
            //    //选择wafer页面点击RUN
            //    UpdateSlotMap();
            //    RefreshControlsEnable(false);
            //    _isAutoJob = true;
            //    var currentRecipe = ProcessRecipe.LoadRecipe(prmPorcedure.SelectedRecipeName, EnumRecipeType.Welder);
            //    _currentRecipe = currentRecipe;
            //    if (_autoJob.QueueRunProcedureInfo.Count > 0)
            //        _autoJob.RunProcedure(0);
            //    else
            //        WarningBox.FormShow("异常!", "任务列表为空！","提示");
            //}
        }
        private void RegisterIOChangedAct()
        {
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.OuterDoorOpenStatus", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.OuterDoorCompactStatus", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.InnerDoorOpenStatus", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.InnerDoorCompactStatus", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.Pressure", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.VacuumDegree", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.TopTemp", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.BottomTemp", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.HeatTargetTemperature", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.HeatPreservationMinute", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.HeatPreservationResidueTime_Minute", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.OverTemperatureThreshold", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.PurgeTimes", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.PurgeInterval", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.ResiduePurgeTimes", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.PurgePressureUpperLimit", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("OvenBox.PurgePressureLowerLimit", IOChanged);


            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.OuterDoorOpenStatus", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.OuterDoorCompactStatus", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.Pressure", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.VacuumDegree", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.PurgeTimes", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.PurgeInterval", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.ResiduePurgeTimes", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.PurgePressureUpperLimit", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ExchangeBox.PurgePressureLowerLimit", IOChanged);

            IOManager.Instance.RegisterIOChannelChangedEvent("GloveBox.Pressure", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("GloveBox.DewPoint", IOChanged);
        }
        bool _bakeOvenInnerdoorPressSta;
        bool _bakeOvenInnerdoorDownSta;
        bool _bakeOvenOuterdoorPressSta;
        bool _bakeOvenOuterdoorDownSta;
        bool _exchangeBoxOuterdoorPressSta;
        bool _exchangeBoxOuterdoorCloseSta;
        private void IOChanged(string ioName, object preValue, object newValue)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() => IOChanged(ioName, preValue, newValue)));
                return;
            }
            try
            {
                
            }
            catch (Exception)
            {
            }
            finally
            {
            }
                                        
        }

        private void InitializeIOStatus()
        {
        }

    }


}
