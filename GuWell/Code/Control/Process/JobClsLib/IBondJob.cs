using CameraControllerClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using HardwareManagerClsLib;
using PositioningSystemClsLib;
using RecipeClsLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
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
    /// ProcessJob Interface.
    /// </summary>
    public interface IBondJob
    {
        /// <summary>
        /// 原始数据存储路径
        /// </summary>
        string RawDataSavingPath { get; set; }

        /// <summary>
        /// 运行job任务.
        /// </summary>
        /// <param name="recipe"></param>
        void RunProcessJob(BondRecipe recipe, int index = 0, bool isTestRecipe = false);

        /// <summary>
        /// 停止任务
        /// </summary>
        void Pause();

        /// <summary>
        /// 继续任务
        /// </summary>
        void Resume();

        /// <summary>
        /// 终止任务
        /// </summary>
        void Abort();

        /// <summary>
        /// 获取分析结果
        /// </summary>
        //MeasureResult GetFinalResult();
    }
    public abstract class ABondJob : IBondJob
    {
        public string JobId { get; set; }
        /// <summary>
        /// 当前Job操作的Wafer对应的Lot id
        /// </summary>
        public string LotId { get; set; }

        /// <summary>
        /// 当前Job操作的Wafer对应的Id
        /// </summary>
        public string MaterialStripId { get; set; }

        /// <summary>
        /// 当前Job操作Wafer对应的层号
        /// </summary>
        public int MaterialStripIndex { get; set; }

        /// <summary>
        /// 是否是当前Lot的第一个Wafer
        /// </summary>
        public bool IsFirstMaterialStrip { get; set; }

        /// <summary>
        /// 当前的Job运行模式，测试模式 跑片模式
        /// </summary>
        public bool IsTestingJob { get; set; }

        /// <summary>
        /// 原始数据存储路径
        /// </summary>
        public string RawDataSavingPath { get; set; }


        /// <summary>
        /// 获取当前Job是否已经初始化
        /// </summary>
        public bool IsInitialized { get; protected set; }



        /// <summary>
        /// 运行状态检查器
        /// </summary>
        protected RunStatusController _statusChecker { get; set; }

        /// <summary>
        /// 报告检测进度的委托
        /// </summary>
        public Action<int, int, int[]> ReportMeasureProgressAct { get; set; }
        /// <summary>
        /// 报告检测进度的委托
        /// </summary>
        public Action<JobResult> NotifyProcessCompleted { get; set; }

        /// <summary>
        /// 报告分析进度的委托
        /// </summary>
        public Action<int, int, int, int, int, int> WeldProgressReportAct { get; set; }
        public Action<int[,]> UpdateLidsProcessStatus { get; set; }
        public Action<int[,]> UpdateShellsProcessStatus { get; set; }


        /// <summary>
        /// 发送Job状态的委托
        /// </summary>
        public Action<EnumBondJobStatus, string> SendJobStatusAct { get; set; }


        public Action BeforeCameraChangeAct
        { get; set; }
        public Action AfterCameraChangeAct
        { get; set; }
        public Action BeforeCameraGrabOneAct
        { get; set; }
        public Action AfterCameraGrabOneAct
        { get; set; }
        /// <summary>
        /// 切换Pass时更新主界面Map
        /// </summary>
        public Action<BondRecipe> NotifyChangeMap { get; set; }

        /// <summary>
        /// 硬件配置
        /// </summary>
        protected HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        protected SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }


        /// <summary>
        /// 硬件系统
        /// </summary>
        protected HardwareManager _hardWareManager
        {
            get { return HardwareManager.Instance; }
        }


        /// <summary>
        /// 当前Job操作的Recipe对象
        /// </summary>
        protected BondRecipe _currentRecipe;


        /// <summary>
        /// 委托操作UI
        /// </summary>
        public Action UIOperation { get; set; }

        /// <summary>
        /// 当前Job是否已经完成
        /// </summary>
        protected bool _isCompleted = false;

        /// <summary>
        /// 
        /// </summary>
        protected PositioningSystem _positioningSystem
        {
            get
            {
                return PositioningSystem.Instance;
            }
        }

        /// <summary>
        /// 蛇形路径规划器
        /// </summary>
        //private PathPlaner _pathPlaner = new PathPlaner();

        /// <summary>
        /// 记录上次recipe的名字
        /// </summary>
        public static string _lastRecipeName = string.Empty;

        ///<summary>
        /// 初始化WaferJob
        /// </summary>
        public abstract void InitialJob();

        public abstract void RunProcessJob(BondRecipe recipe, int index = 0, bool isTestRecipe = false);




        /// <summary>
        /// 停止任务
        /// </summary>
        public void Pause() { _statusChecker.Pause(); PauseSubScheduler(); }

        /// <summary>
        /// 继续任务
        /// </summary>
        public void Resume() { _statusChecker.Resume(); ResumeSubScheduler(); }

        /// <summary>
        /// 终止任务 & 重置坐标系到校准前的状态
        /// </summary>
        public virtual void Abort()
        {
            //停止当前检测过程
            _statusChecker.Abort();
        }

        /// <summary>
        /// 终止任务, 不重置坐标系到校准前的状态
        /// </summary>
        public virtual void AbortWithoutReset()
        {
            //停止当前检测过程
            _statusChecker.Abort();
        }


        /// <summary>
        /// 测量前运行
        /// </summary>
        protected abstract void RunBeforeJob();

        /// <summary>
        /// 生成数据存储目录
        /// </summary>
        /// <returns></returns>
        protected abstract string GenerateDataSavingPath();

        /// <summary>
        /// 吸真空时准备工作
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="isFirst"></param>
        public virtual void PrepareJob(BondRecipe recipe, bool isFirst)
        {

        }

        /// <summary>
        /// 报告Job运行状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="statusOfRunning"></param>
        protected void ReportJobRunningStatus(EnumBondJobStatus status, string statusOfRunning)
        {
            if (SendJobStatusAct != null)
            {
                SendJobStatusAct(status, statusOfRunning);
            }
        }


        /// <summary>
        /// 暂停子任务，数据采集
        /// </summary>
        protected void PauseSubScheduler()
        {
        }

        /// <summary>
        /// 恢复子任务，数据采集
        /// </summary>
        protected void ResumeSubScheduler()
        {
        }

        /// <summary>
        /// 获取当前Job是否正在运行
        /// </summary>
        /// <returns></returns>
        public bool IsJobRunning()
        {
            return _statusChecker.IsRunning;
        }

        /// <summary>
        /// 获取当前Job是否正在运行
        /// </summary>
        /// <returns></returns>
        public bool IsJobCompleted()
        {
            return _isCompleted;
        }

        /// <summary>
        /// 标记Job已完成状态
        /// </summary>
        public void MarkJobCompleted()
        {
            this._isCompleted = true;
            this._statusChecker.Accomplish();
        }


        private bool _bStitchFinised = false;

    }
}
