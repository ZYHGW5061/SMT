using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StageManagerClsLib;
using CameraControllerClsLib;
using LightControllerManagerClsLib;
using LightControllerClsLib;
using StageControllerClsLib;
using PowerControllerManagerClsLib;
using BoardCardControllerClsLib;
using LaserSensorManagerClsLib;
using DynamometerManagerClsLib;
using LaserSensorControllerClsLib;
using JoyStickManagerClsLib;
using IJoyStickControllerClsLib;using System.IO.Ports;
using IOUtilityClsLib;
using DispensingMachineManagerClsLib;
using DispensingMachineControllerClsLib;

namespace HardwareManagerClsLib
{
    public class HardwareManager
    {
        private static readonly object _lockObj = new object();
        private static volatile HardwareManager _instance = null;
        public SerialPort PowerControl = new SerialPort();
        public static HardwareManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new HardwareManager();
                        }
                    }
                }
                return _instance;
            }
        }
        private HardwareManager()
        {
        }
        public void Initialize()
        {
        }
        protected HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        //private CameraManager _cameraManager
        //{
        //    get { return CameraManager.Instance; }
        //}
        private StageManager _stageManager
        {
            get { return StageManager.Instance; }
        }

        private BoardCardManager _boardCardManager
        {
            get { return BoardCardManager.Instance; }
        }


        private CameraManager _cameraManager
        {
            get { return CameraManager.Instance; }
        }
        private LightControllerManager _LightControllerManager
        {
            get { return LightControllerManager.Instance; }
        }
        

        public ILightSourceController WaferRingLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.WaferRingField);
            }
        }
        public ILightSourceController WaferDirectLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectField);
            }
        }
        public ILightSourceController WaferDirectRedLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField);
            }
        }
        public ILightSourceController WaferDirectGreenLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField);
            }
        }
        public ILightSourceController WaferDirectBlueLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField);
            }
        }
        public ILightSourceController BondRingLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondRingField);
            }
        }
        public ILightSourceController BondDirectLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectField);
            }
        }
        public ILightSourceController BondDirectRedLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField);
            }
        }
        public ILightSourceController BondDirectGreenLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField);
            }
        }
        public ILightSourceController BondDirectBlueLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField);
            }
        }
        public ILightSourceController LookupRingLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField);
            }
        }
        public ILightSourceController LookupDirectLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICameraController BondCamera
        {
            get { return _cameraManager.GetCameraByID(EnumCameraType.BondCamera); }
        }
        public ICameraController UplookingCamera
        {
            get { return _cameraManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        public ICameraController WaferCamera
        {
            get { return _cameraManager.GetCameraByID(EnumCameraType.WaferCamera); }
        }

        public ILaserSensorController LaserSensor
        {
            get { return LaserSensorManager.Instance.GetCurrentHardware(); }
        }

        public IJobStickController JobStick
        {
            get { return JoyStickManager.Instance.GetCurrentController(); }
        }

        public IPowerController PowerController
        {
            get { return PowerControllerManager.Instance.GetCurrentHardware(); }
        }



        public IDispensingMachineController DispensingMachineController
        {
            get { return DispensingMachineManager.Instance.GetCurrentHardware(); }
        }


        /// <summary>
        /// 根据硬件类型连接硬件的
        /// </summary>
        /// <param name="type"></param>
        public void ConnectHardware(EnumHardwareType type)
        {
            switch (type)
            {
                case EnumHardwareType.Stage:
                    _stageManager.Initialize();
                    //_boardCardManager.Initialize();
                    //JoyStickManager.Instance.Initialize();
                    IOUtilityHelper.Instance.Start();
                    //IOUtilityHelper.Instance.TurnonTowerYellowLight();
                    break;
                case EnumHardwareType.Camera:
                    _cameraManager.InitializeCameras();

                    break;
                case EnumHardwareType.Light:
                    _LightControllerManager.Initialize();
                    break;
                case EnumHardwareType.ControlBoard:
                    //PLCControllerManager.Instance.Initialize();
                    break;
                case EnumHardwareType.PowerController:
                    PowerControllerManager.Instance.Initialize();
                    break;
                case EnumHardwareType.LaserSensor:
                    LaserSensorManager.Instance.Initialize();
                    break;
                case EnumHardwareType.Dynamometer:
                    DynamometerManager.Instance.Initialize(PowerControl);
                    break;
                case EnumHardwareType.DispensingMachine:
                    DispensingMachineManager.Instance.Initialize();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据设备类型断开设备连接
        /// </summary>
        /// <param name="type"></param>
        public void DisconnectHardware(EnumHardwareType type)
        {
            try
            {
                switch (type)
                {
                    case EnumHardwareType.Stage:
                            _stageManager.Shutdown();
                        break;
                    case EnumHardwareType.Camera:
                            _cameraManager.Shutdown();
                        break;
                    case EnumHardwareType.Light:
                        _LightControllerManager.Shutdown();
                        break;
                    case EnumHardwareType.ControlBoard:
                        //PLCControllerManager.Instance.Shutdown();
                        break;
                    case EnumHardwareType.PowerController:
                        //PowerControllerManager.Instance.Shutdown();
                        break;
                    case EnumHardwareType.DispensingMachine:
                        DispensingMachineManager.Instance.Shutdown();
                        break;
                    default:
                        break;
                }
            }
            catch {
            }
        }

        /// <summary>
        /// 断开连接所有硬件
        /// </summary>
        public void DisconnectHardwares()
        {
            DisconnectHardware(EnumHardwareType.ControlBoard);

            DisconnectHardware(EnumHardwareType.Stage);

            DisconnectHardware(EnumHardwareType.Light);

            DisconnectHardware(EnumHardwareType.Camera);

            DisconnectHardware(EnumHardwareType.PowerController);

        }
        /// <summary>
        /// Stage运动平台
        /// </summary>
        public IStageController Stage
        {
            get { return StageManager.Instance.GetCurrentController(); }
        }
        
        //public IPowerController PowerController
        //{
        //    get { return PowerControllerManager.Instance.GetCurrentHardware(); }
        //}
        public CameraConfig GetCameraParametersByCameraType(string type)
        {
            CameraConfig ret = null;
            if (HardwareConfiguration.Instance.CameraConfigList.Any(i => i.CameraType == (EnumCameraType)Enum.Parse(typeof(EnumCameraType), type)))
            {
                ret = HardwareConfiguration.Instance.CameraConfigList.FirstOrDefault(i => i.CameraType == (EnumCameraType)Enum.Parse(typeof(EnumCameraType), type));
            }
            return ret;
        }
        public LensConfig CurrentLensConfig
        {
            get
            {
                LensConfig ret = null;
                //if (HardwareConfiguration.Instance.LensList.Any(i => i.UserCamera == _cameraManager.CurrentCameraType))
                //{
                //    ret = HardwareConfiguration.Instance.LensList.FirstOrDefault(i => i.UserCamera == _cameraManager.CurrentCameraType);
                //}
                return ret;
            }
        }
        /// <summary>
        /// 检测Camera的有效性
        /// </summary>
        /// <returns></returns>
        public bool CheckCameraEngineValid()
        {
            if (BondCamera != null && UplookingCamera != null && WaferCamera != null)
            {
                return BondCamera.IsConnect & UplookingCamera.IsConnect & WaferCamera.IsConnect;
            }
            return true;
        }
        public bool CheckPLCEngineValid()
        {
            return true;
        }
        public bool CheckStageEngineValid()
        {
            return true;
        }
        public bool CheckPowerControllerValid()
        {
            //if (PowerController != null)
            //{
            //    return PowerController.IsConnect;
            //}
            return true;
        }
        public bool CheckLaserSensorControllerValid()
        {
            //if (LaserSensorManager.Instance != null)
            //{
            //    return LaserSensorManager.Instance.IsConnect;
            //}
            return true;
        }

        public bool CheckDynamometerControllerValid()
        {
            //if (LaserSensorManager.Instance != null)
            //{
            //    return LaserSensorManager.Instance.IsConnect;
            //}
            return true;
        }
        public bool CheckDispensingMachineControllerValid()
        {
            //if (LaserSensorManager.Instance != null)
            //{
            //    return LaserSensorManager.Instance.IsConnect;
            //}
            return true;
        }
    }
}
