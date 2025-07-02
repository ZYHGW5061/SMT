using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using StageControllerClsLib;


namespace StageManagerClsLib
{
    /// <summary>
    /// Stage管理类
    /// </summary>
    public class StageManager
    {
        #region 获取单例
        private static readonly object _lockObj = new object();
        private static volatile StageManager _instance = null;
        public static StageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new StageManager();
                        }
                    }
                }
                return _instance;
            }
        }
        private StageManager()
        {
        }
        #endregion

        /// <summary>
        /// 当前硬件
        /// </summary>
        IStageController _currentStageController = null;
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        /// <summary>
        /// 初始化,创建Stage控制对象及硬件连接
        /// </summary>
        public void Initialize()
        {
            var runningType = _hardwareConfig.StageConfig.RunningType;
            if (runningType == EnumRunningType.Actual)
            {

                TPGJStageInfo stageInfo = new TPGJStageInfo();
                stageInfo.AxisControllerDic = new Dictionary<EnumStageAxis, ISingleAxisController>();
                stageInfo.AxisControllerDic.Add(EnumStageAxis.BondX, new BondXSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.BondY, new BondYSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.BondZ, new BondZSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.ChipPPT, new ChipPPTSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.PPtoolBankTheta, new PPtoolBankThetaSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.DippingGlue, new DippingGlueSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.TransportTrack1, new TransportTrack1SingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.TransportTrack2, new TransportTrack2SingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.TransportTrack3, new TransportTrack3SingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.WaferTableX, new WaferTableXSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.WaferTableY, new WaferTableYSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.WaferTableZ, new WaferTableZSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.WaferFilm, new WaferFilmSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.WaferFinger, new WaferFingerSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.WaferCassetteLift, new WaferCassetteLiftSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.ESZ, new ESZSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.NeedleZ, new NeedleZSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.NeedleSwitch, new NeedleSwitchSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.FilpToolTheta, new FilpToolThetaSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.SubmountPPT, new SubmountPPTSingleAxisController());
                stageInfo.AxisControllerDic.Add(EnumStageAxis.SubmountPPZ, new SubmountPPZSingleAxisController());
                //添加其他轴

                StageCore.Instance.StageInfo = stageInfo;
                _currentStageController = new TPGJStageController();
                //(_currentStageController as HCFAStageController).StageInfo = stageInfo;
            }
            else
            {
                _currentStageController = new SimulateStageController();
            }

            if (!_currentStageController.IsConnect)
                _currentStageController.Connect();
            _currentStageController.CheckHomeDone();
            _currentStageController.InitialzeAllAxisParameter();

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Shutdown()
        {

            if (_currentStageController != null)
            {
                _currentStageController.Stop();
                _currentStageController.Disconnect();
                _currentStageController = null;
            }
        }

        /// <summary>
        /// 获取当前控制器
        /// </summary>
        /// <returns></returns>
        public IStageController GetCurrentController()
        {
            if (_currentStageController == null)
            {
            }
            return _currentStageController;
        }
    }
}
