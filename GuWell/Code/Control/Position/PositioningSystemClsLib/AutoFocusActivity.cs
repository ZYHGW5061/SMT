using CameraControllerClsLib;
using CameraControllerWrapperClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using HardwareManagerClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UserControls;
using WestDragon.Framework.UtilityHelper;
using static PositioningSystemClsLib.AutoFocusProcessorFactory;

namespace PositioningSystemClsLib
{
    /// <summary>
    /// 自动聚焦功能实现类
    /// </summary>
    public class AutoFocusActivity
    {

        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
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


        /// <summary>
        /// 是否是自动聚焦
        /// </summary>
        public bool AutoFocus = true;

        /// <summary>
        /// 不自动聚焦时Z的绝对位置
        /// </summary>
        public double FocusPositionZ = 0;

        /// <summary>
        /// 执行AutoFocus的接口
        /// </summary>
        public IAutoFocus _autoFocusProcessor { get; set; }

        /// <summary>
        /// 聚焦器对象
        /// </summary>
        private static volatile  AutoFocusActivity _autoFocusActivity = null;
        private static readonly object _lockObj = new object();
        /// <summary>
        /// 获取聚焦器的唯一单实例
        /// </summary>
        /// <returns></returns>
        public static AutoFocusActivity Instance
        {
            get
            {
                if (_autoFocusActivity == null)
                {
                    lock (_lockObj)
                    {

                        if (_autoFocusActivity == null)
                        {
                            _autoFocusActivity = new AutoFocusActivity();
                        }
                    }
                }
                return _autoFocusActivity;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoFocusActivity()
        {
            _autoFocusProcessor = AutoFocusProcessorFactory.Create(AutoFocusType.OpencvAlgorithm);
        }

        /// <summary>
        /// 切换聚焦的类型
        /// </summary>
        /// <param name="type"></param>
        public void ChangeAutoFocusAlgoType(AutoFocusProcessorFactory.AutoFocusType type)
        {
            _autoFocusProcessor = AutoFocusProcessorFactory.Create(type);
        }
        private CameraConfig _cameraConfig
        {
            get
            {
                return CameraManager.Instance.CurrentCameraConfig;
            }
        }
        private EnumCameraType _currentCamera
        {
            get
            {
                return CameraManager.Instance.CurrentCameraType;
            }
        }
        private EnumStageAxis _focusZAxis
        {
            get
            {
                var ret= EnumStageAxis.BondZ;
                if(_currentCamera==EnumCameraType.WaferCamera)
                {
                    ret = EnumStageAxis.WaferTableZ;
                }
                return ret;
            }
        }
        /// <summary>
        /// 用户自定义回调事件
        /// </summary>
        public Action<AutoFocusActionCallback> UserCustomAction
        {
            get { return _autoFocusProcessor.UserCustomAction; }
            set { _autoFocusProcessor.UserCustomAction = value; }
        }

        /// <summary>
        /// 在当前点执行自动聚焦操作
        /// </summary>
        public void AutoFocusForCurrentPoint(double offsetZ = 0, CircleFigure focusRegion = null)
        {
            LogRecorder.RecordLog(EnumLogContentType.Info, "AutoFocusForCurrentPoint Start.");
            if (AutoFocus)
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "AutoFocusForCurrentPoint Start Step1.");
                var oldPositionZ = _positioningSystem.ReadCurrentStagePosition(_focusZAxis);
                try
                {
                    var coarseFocusValue = 0d;
                    if (focusRegion == null)
                    {
                        focusRegion = new CircleFigure(new System.Drawing.PointF(_cameraConfig.ImageSizeWidth / 2, _cameraConfig.ImageSizeHeight / 2)
                            , Math.Min(_cameraConfig.ImageSizeWidth, _cameraConfig.ImageSizeHeight) / 2);
                        //focusRegion.CenterX = _systemConfig.ImageSize.Width / 2;
                        //focusRegion.CenterY = _systemConfig.ImageSize.Height / 2;
                        //focusRegion.Radius = Math.Min(_systemConfig.ImageSize.Width, _systemConfig.ImageSize.Height) / 2;
                    }
                    //这里的起始位置是当前位置的上下5MM
                    _autoFocusProcessor.Param = new AutoFocusParams(oldPositionZ - 3, oldPositionZ + 3,0.05d, focusRegion, EnumAutoFocusMethod.Fast);
                    _autoFocusProcessor.DoAutoFocusSync_RelateiveMove();
                    oldPositionZ = _autoFocusProcessor.Result.X;
                    LogRecorder.RecordLog(EnumLogContentType.Info, "AutoFocusForCurrentPoint Start Step2.");

                }
                catch (Exception ex)
                {
                    //处理异常信息
                    LogRecorder.RecordLog(EnumLogContentType.Error, "Auto focus failed.", ex);
                }
            }
            else
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "AutoFocusForCurrentPoint Start Step3.");
            }
        }
    }
}
