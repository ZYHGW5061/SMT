using CameraControllerClsLib;
using CameraControllerWrapperClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using LightControllerClsLib;
using LightControllerManagerClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionClsLib;
using VisionControlAppClsLib;

namespace VisionControlAppClsLib
{
    public class VisualControlManager
    {
        private static readonly object _lockObj = new object();
        private static volatile VisualControlManager _instance = null;
        public static VisualControlManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new VisualControlManager();
                        }
                    }
                }
                return _instance;
            }
        }
        private VisualControlManager()
        {
            AllVisualControl = new Dictionary<EnumCameraType, VisualControlApplications>();
            //Initialize();
        }




        public Dictionary<EnumCameraType, VisualControlApplications> AllVisualControl { get; private set; }
        public EnumCameraType CurrentCameraType { get; set; }
        //public ICameraController CurrentCamera { get; set; }



        /// <summary>
        /// 硬件配置
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        private CameraControllerClsLib.CameraManager _cameraManager
        {
            get { return CameraControllerClsLib.CameraManager.Instance; }
        }
        private LightControllerManager _LightControllerManager
        {
            get { return LightControllerManager.Instance; }
        }
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
        public ILightSourceController SubstrateRingLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondRingField);
            }
        }
        public ILightSourceController SubstrateDirectLightController
        {
            get
            {
                return _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectField);
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
        private CameraConfig _BondcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.BondCamera); }
        }
        private CameraConfig _UplookingcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.UplookingCamera); }
        }
        private CameraConfig _WafercameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.WaferCamera); }
        }

        VisualAlgorithms Substratevisual = new VisualAlgorithms();

        VisualAlgorithms Wafervisual = new VisualAlgorithms();

        VisualAlgorithms Uplookvisual = new VisualAlgorithms();


        public bool InitializeVisualControls()
        {
            

            bool S = Substratevisual.Init();
            bool W = Wafervisual.Init();
            bool U = Uplookvisual.Init();
            VisualControlApplications App;
            var configs = HardwareConfiguration.Instance.CameraConfigList;
            foreach (var item in configs)
            {
                var camera = CameraControllerClsLib.CameraFactory.CreateCamera(item);

               


                var cameraIndexName = (EnumCameraType)Enum.Parse(typeof(EnumCameraType), item.CameraName);
                if (cameraIndexName == EnumCameraType.BondCamera)
                {
                    if(HardwareConfiguration.Instance.IsBondDirectLightMultiColor)
                    {
                        App = new VisualControlApplications(BondCamera, EnumCameraType.BondCamera, _hardwareConfig.SubstrateDirectRedLightConfig.ChannelNumber, _hardwareConfig.SubstrateDirectGreenLightConfig.ChannelNumber, _hardwareConfig.SubstrateDirectBlueLightConfig.ChannelNumber,
                        _hardwareConfig.SubstrateRingLightConfig.ChannelNumber, Substratevisual);
                        
                    }
                    else
                    {
                        App = new VisualControlApplications(BondCamera, EnumCameraType.BondCamera, _hardwareConfig.SubstrateDirectLightConfig.ChannelNumber,
                            _hardwareConfig.SubstrateRingLightConfig.ChannelNumber, Substratevisual);
                    }
                    
                    App.ImageWidth = _BondcameraConfig.ImageSizeWidth;
                    App.ImageHeight = _BondcameraConfig.ImageSizeHeight;
                    //Add camera to camera dic.
                    AllVisualControl.Add(item.CameraType, App);
                    S = true;
                }
                if (cameraIndexName == EnumCameraType.UplookingCamera)
                {
                    if (HardwareConfiguration.Instance.IsUplookDirectLightMultiColor)
                    {
                        //App = new VisualControlApplications(UplookingCamera, EnumCameraType.UplookingCamera, _hardwareConfig.LookupDirectRedLightConfig.ChannelNumber, _hardwareConfig.SubstrateDirectGreenLightConfig.ChannelNumber, _hardwareConfig.SubstrateDirectBlueLightConfig.ChannelNumber,
                        //_hardwareConfig.SubstrateRingLightConfig.ChannelNumber, Substratevisual);
                        App = new VisualControlApplications(UplookingCamera, EnumCameraType.UplookingCamera, _hardwareConfig.LookupDirectLightConfig.ChannelNumber, _hardwareConfig.LookupRingLightConfig.ChannelNumber, Uplookvisual);
                    }
                    else
                    {
                        App = new VisualControlApplications(UplookingCamera, EnumCameraType.UplookingCamera, _hardwareConfig.LookupDirectLightConfig.ChannelNumber, _hardwareConfig.LookupRingLightConfig.ChannelNumber, Uplookvisual);
                    }
                    
                    
                    App.ImageWidth = _UplookingcameraConfig.ImageSizeWidth;
                    App.ImageHeight = _UplookingcameraConfig.ImageSizeHeight;
                    //Add camera to camera dic.
                    AllVisualControl.Add(item.CameraType, App);
                    U = true;
                }
                if (cameraIndexName == EnumCameraType.WaferCamera)
                {
                    if(HardwareConfiguration.Instance.IsWaferDirectLightMultiColor)
                    {
                        App = new VisualControlApplications(WaferCamera, EnumCameraType.WaferCamera, _hardwareConfig.WaferDirectRedLightConfig.ChannelNumber, _hardwareConfig.WaferDirectGreenLightConfig.ChannelNumber, _hardwareConfig.WaferDirectBlueLightConfig.ChannelNumber,
                        _hardwareConfig.WaferRingLightConfig.ChannelNumber, Wafervisual);
                    }
                    else
                    {
                        App = new VisualControlApplications(WaferCamera, EnumCameraType.WaferCamera, _hardwareConfig.WaferDirectLightConfig.ChannelNumber,
                        _hardwareConfig.WaferRingLightConfig.ChannelNumber, Wafervisual);
                    }
                    //App = new VisualControlApplications(WaferCamera, WaferRingLightController, _hardwareConfig.WaferDirectRedLightConfig.ChannelNumber, _hardwareConfig.WaferDirectGreenLightConfig.ChannelNumber, _hardwareConfig.WaferDirectBlueLightConfig.ChannelNumber,
                    //    _hardwareConfig.WaferRingLightConfig.ChannelNumber, Wafervisual);
                    
                    App.ImageWidth = _WafercameraConfig.ImageSizeWidth;
                    App.ImageHeight = _WafercameraConfig.ImageSizeHeight;
                    //Add camera to camera dic.
                    AllVisualControl.Add(item.CameraType, App);
                    W = true;
                }


            }




            return S & U & W;
        }

        public VisualControlApplications GetCameraByID(EnumCameraType cameraIndex)
        {
            VisualControlApplications ret = null;
            if (AllVisualControl.ContainsKey(cameraIndex))
            {
                ret = AllVisualControl[cameraIndex];
            }
            return ret;
        }
    }
}
