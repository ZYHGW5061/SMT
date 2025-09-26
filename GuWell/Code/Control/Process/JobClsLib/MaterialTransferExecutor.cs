using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using PositioningSystemClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace JobClsLib
{
    public class MaterialTransferExecutor
    {
        /// <summary>
        /// 运行状态检查器
        /// </summary>
        private RunStatusController _statusCheckerForTransferMaterial;

        /// <summary>
        /// 清片运行状态检查器
        /// </summary>
        private RunStatusController _statusCheckerForClearMaterial;
        /// <summary>
        /// 传片流程规划器
        /// </summary>
        private WaferTransferPlanner _transferPlanner = null;
        /// <summary>
        /// 报告传片状态
        /// </summary>
        public Action<EnumTransferStatus,int, string> ReportTransferStatusAct { get; set; }
        /// <summary>
        /// 请求运行Wafer检测
        /// </summary>
        public Action<BondRecipe, int, bool> RequestWeldAct { get; set; }
        /// <summary>
        /// 确认检测是否完成
        /// </summary>
        public Action<RunStatusController> ConfirmWaferJobCompletedAct { get; set; }
        public Action ClearCompletedAct { get; set; }
        public Action QuickLoadCompletedAct { get; set; }

        /// <summary>
        /// 待检测的Wafer集合
        /// </summary>
        private Queue<JobMaterialInfo> _strips = new Queue<JobMaterialInfo>();
        /// <summary>
        /// 流程动作队列
        /// </summary>
        private Queue<WaferTransferInfo> _actionQueue;
        /// <summary>
        /// 确保安全的线程锁
        /// </summary>
        private object _lockerEnsureSafeForLoad = new object();
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positionSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        private bool _isClearMode = false;
        private bool _isQuickLoadMode = false;

        public MaterialTransferExecutor()
        {
            //初始化流程规划器
            _transferPlanner = new WaferTransferPlanner();

            //初始化运行状态器
            _statusCheckerForTransferMaterial = new RunStatusController();
            _statusCheckerForClearMaterial = new RunStatusController();

        }
        public void ExecuteTransfer(Queue<JobMaterialInfo> strips, bool isOnlyTransferWafer = false)
        {
            try
            {
                if (_statusCheckerForTransferMaterial.IsRunning)
                {
                    throw new NotSupportedException("Transfer system is running now.");
                }

                if (strips.Count == 0)
                {
                    throw new NotSupportedException("Number can not be zero.");
                }


                if (ReportTransferStatusAct != null)
                {
                    ReportTransferStatusAct(EnumTransferStatus.Transfering, 0, "Transfering Material...");
                }

                //------自动循环传片流程开始-------
                _statusCheckerForTransferMaterial.Start();

                //传片信息
                _strips = strips;
                //规划流程
                _actionQueue = _transferPlanner.PlanTransferActionQueue(strips);
                //执行流程
                RunTransferMaterialActions();

            }
            catch (Exception ex)
            {
                ExceptionAbort();

                LogRecorder.RecordLog(EnumLogContentType.Error, "ExecuteTransfer failed.", ex);

                if (ReportTransferStatusAct != null)
                {
                    ReportTransferStatusAct(EnumTransferStatus.ExceptionAborted, 0, "ExecuteTransferMaterial failed.");
                }
            }

        }
        /// <summary>
        /// 异常终止传片
        /// </summary>
        private void ExceptionAbort()
        {
            //终止传片流程
            _statusCheckerForTransferMaterial.Abort();
            _statusCheckerForClearMaterial.Abort();
            //清空传片流程信息
            if (_actionQueue != null)
            {
                _actionQueue.Clear();
            }

            if (_strips != null)
            {
                _strips.Clear();
            }
        }
        public bool IsAbort
        {
            get
            {
                return _statusCheckerForTransferMaterial.IsAborted;
            }
        }
        public bool IsRunning
        {
            get
            {
                return _statusCheckerForTransferMaterial.IsRunning;
            }
        }
        public void Abort()
        {
            try
            {
                //终止传片流程
                if(_statusCheckerForTransferMaterial.IsPaused)
                {
                    _statusCheckerForTransferMaterial.Resume();
                }
                _statusCheckerForTransferMaterial.Abort();
                _statusCheckerForClearMaterial.Abort();
                //清空传片流程信息
                if (_actionQueue != null)
                {
                    _actionQueue.Clear();
                }

                if (_strips != null)
                {
                    _strips.Clear();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        bool startFlag = true;
        /// <summary>
        /// 运行传片动作
        /// </summary>
        /// <param name="isAutoCycleMode">true，自动循环模式，false为手动上片模式</param>
        private void RunTransferMaterialActions(bool isAutoCycleMode = true)
        {
            #region 
            //Task.Factory.StartNew(new Action(() =>
            //{
            //    if (!PrepareToTransfer())
            //    {
            //        ExceptionAbort();
            //        if (ReportTransferStatusAct != null)
            //        {
            //            ReportTransferStatusAct(EnumTransferStatus.ExceptionAborted, "Prepare to load material failed.");
            //        }
            //        return;
            //    }
            //}));
            //Thread.Sleep(105);
            #endregion
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    bool isFirstStrip = true;
                    string useLotId = string.Empty;
                    string useRecipeName = string.Empty;
                    string waferID = string.Empty;

                    //循环执行流程队列
                    while (_actionQueue.Count > 0 && _statusCheckerForTransferMaterial.IsRunning)
                    {
                        var transferStatus = "";
                        WaferTransferInfo action = _actionQueue.Dequeue();

                        LogRecorder.RecordLog(EnumLogContentType.Error, $"RunTransferMaterialActions,Action:{action.TransferAction.ToString()}");



                        switch (action.TransferAction)
                        {                           
                            default:

                                break;
                        }

                    }
                    if(_isClearMode)
                    {
                        _isClearMode = false;
                        if(ClearCompletedAct!=null)
                        {
                            ClearCompletedAct();
                        }
                    }
                    if (_isQuickLoadMode)
                    {
                        _isQuickLoadMode = false;
                        if (QuickLoadCompletedAct != null)
                        {
                            QuickLoadCompletedAct();
                        }
                    }
                    if (_statusCheckerForTransferMaterial.IsRunning)
                    {
                        //----传片流程正常结束------
                        _statusCheckerForTransferMaterial.Accomplish();

                        //报告传片状态
                        if (ReportTransferStatusAct != null)
                        {
                            ReportTransferStatusAct(EnumTransferStatus.TransferCompleted,0, "Whole cassette material transfer completed.");
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }
        private AutoResetEvent _nextOneSafeEvent = new AutoResetEvent(false);
        /// <summary>
        /// 停止传片等待检测完成
        /// </summary>
        public void WaitWeldCompleted()
        {
            _statusCheckerForTransferMaterial.Pause();
            _statusCheckerForTransferMaterial.CheckStatus();
            //_nextOneSafeEvent.WaitOne();

        }

        /// <summary>
        /// 继续传片流程
        /// </summary>
        public void Resume()
        {
            _statusCheckerForTransferMaterial.Resume();
        }       
    }
}
