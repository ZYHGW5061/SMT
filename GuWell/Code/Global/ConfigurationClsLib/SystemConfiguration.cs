using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace ConfigurationClsLib
{
    [Serializable]
    [XmlRoot("SystemConfiguration")]
    public class SystemConfiguration
    {
        private static string _configPath = Path.Combine($@"{System.Environment.CurrentDirectory}\Config", "SystemConfiguration.xml");
        private static readonly object _lockObj = new object();
        private static volatile SystemConfiguration _instance = LoadConfig();
        public static SystemConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new SystemConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }
        private SystemConfiguration()
        {
            SystemGUIType = new SystemGUIType();
            JobConfig = new JobConfig();
            PositioningConfig = new PositioningConfig();
            CalibrationConfig = new CalibrationConfig();
            PPToolSettings = new List<PPToolSettings>();
            ESToolSettings = new List<ESToolSettings>();
            SystemCalibrationConfig = new SystemCalibrationConfig();
        }
        private static SystemConfiguration LoadConfig()
        {
            SystemConfiguration ret = new SystemConfiguration();
            try
            {
                ret= XmlSerializeHelper.XmlDeserializeFromFile<SystemConfiguration>(_configPath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Load  SystemConfiguration Error.", ex);
            }
            return ret;

        }
        public void SaveConfig()
        {
            XmlSerializeHelper.XmlSerializeToFile(this, _configPath, Encoding.UTF8);
        }
        [XmlIgnore]
        public string RawDataSavePath
        {
            get
            {
                var ret = @"D:\GWData\JobResult\";
                if (!string.IsNullOrEmpty(JobConfig.RawDataSavingPath))
                {
                    ret = JobConfig.RawDataSavingPath;
                }
                return ret;
            }
        }
        #region XMLConfig
        [XmlElement("MachineID")]
        public string MachineID { get; set; }


        [XmlElement("SystemDefaultDirectory")]
        public string SystemDefaultDirectory { get; set; }

        [XmlElement("SystemGUIType")]
        public SystemGUIType SystemGUIType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("SystemRunningType")]
        public EnumRunningType SystemRunningType { get; set; }

        [XmlElement("SystemRole")]                
        public EnumSystemRole SystemRole { get; set; }
        [XmlElement("SystemMode")]
        public EnumSystemMode SystemMode { get; set; }

        [XmlElement("JobConfig")]
        public JobConfig JobConfig { get; set; }
        [XmlElement("PositioningConfig")]
        public PositioningConfig PositioningConfig { get; set; }
        [XmlElement("SystemCalibrationConfig")]
        //[XmlIgnore]
        public SystemCalibrationConfig SystemCalibrationConfig { get; set; }
        [XmlElement("CalibrationConfig")]
        public CalibrationConfig CalibrationConfig { get; set; }

        [XmlElement("ESUpPosition")]
        public float ESUpPosition { get; set; }
        /// <summary>
        /// BondZ低于此值时BondXY不能移动（出于装机保护）
        /// </summary>
        [XmlElement("BondZPos2ActiveXYSafeProtect")]
        public float BondZPos2ActiveXYSafeProtect { get; set; }
        /// <summary>
        /// ES高于于此值时BondXY不能随意移动（出于装机保护）
        /// </summary>
        [XmlElement("ESZPos2ActiveXYSafeProtect")]
        public float ESZPos2ActiveXYSafeProtect { get; set; }
        [XmlElement("ESDownPosition")]
        public float ESDownPosition { get; set; }
        [XmlArray("PPToolSettings"), XmlArrayItem(typeof(PPToolSettings))]
        //[XmlIgnore]
        public List<PPToolSettings> PPToolSettings { get; set; }
        [XmlArray("ESToolSettings"), XmlArrayItem(typeof(ESToolSettings))]
        //[XmlIgnore]
        public List<ESToolSettings> ESToolSettings { get; set; }

        [XmlElement("TuningTimeMS")]
        public int TuningTimeMS { get; set; }

        [XmlElement("OpenCoolAirDelayMS")]
        public int OpenCoolAirDelayMS { get; set; }
        [XmlElement("WaferSlotCount")]
        public int WaferSlotCount { get; set; }
        
        #endregion

    }

    [Serializable]
    public class SystemGUIType
    {
        [XmlElement("CameraWindowHeight")]
        public int CameraWindowHeight { get; set; }

        [XmlElement("CameraWindowWidth")]
        public int CameraWindowWidth { get; set; }

    }

    [Serializable]
    public class JobConfig
    {
        [XmlElement("RunningType")]
        public EnumRunningType RunningType { get; set; }

        [XmlElement("RawDataSavingPath")]
        public string RawDataSavingPath { get; set; }

        [XmlElement("EnableMarathon")]
        public bool EnableMarathon { get; set; }
        [XmlElement("RecogniseResulSaveOption")]
        public EnumRecogniseResulSaveOption RecogniseResulSaveOption { get; set; }
        [XmlElement("EnableVaccumConfirm")]
        public bool EnableVaccumConfirm { get; set; }
        [XmlElement("IsSlowSpeedRun")]
        public bool IsSlowSpeedRun { get; set; }

        [XmlElement("BreakVaccumDelayMsAfterEutectic")]
        public int BreakVaccumDelayMsAfterEutectic { get; set; }

        /// <summary>
        /// 找芯片时最多NG数量，超过NG数量之后提示确定起点位置
        /// </summary>
        [XmlElement("CurChipNGNumMax")]
        public int CurChipNGNumMax { get; set; }

    }
    /// <summary>
    /// 坐标系原点配置
    /// </summary>
    [Serializable]
    public class PositioningConfig
    {
        public PositioningConfig()
        {

            BondOrigion = new XYZTCoordinateConfig();

            LookupCameraOrigion = new XYZTCoordinateConfig();
            LookupLaserSensorOrigion = new XYZTCoordinateConfig();
            LookupChipPPOrigion = new XYZTCoordinateConfig();
            LookupSubmountPPOrigion = new XYZTCoordinateConfig();


            TrackOrigion = new XYZTCoordinateConfig();
            TrackLaserSensorOrigion = new XYZTCoordinateConfig();
            TrackChipPPOrigion = new XYZTCoordinateConfig();
            TrackSubmountPPOrigion = new XYZTCoordinateConfig();

            PP1AndBondCameraOffset = new XYZTCoordinateConfig();
            PP2AndBondCameraOffset = new XYZTCoordinateConfig();
            LaserSensorAndBondCameraOffset = new XYZTCoordinateConfig();


            WaferCameraOrigion = new XYZTCoordinateConfig();
            WaferLaserSensorOrigion = new XYZTCoordinateConfig();
            WaferChipPPOrigion = new XYZTCoordinateConfig();
            WaferSubmountPPOrigion = new XYZTCoordinateConfig();

            EutecticWeldingLocation = new XYZTCoordinateConfig();
            EutecticWeldingLaserSensorLocation = new XYZTCoordinateConfig();
            EutecticWeldingChipPPLocation = new XYZTCoordinateConfig();
            EutecticWeldingSubmountPPLocation = new XYZTCoordinateConfig();


            WaferOrigion = new XYZTCoordinateConfig();
            BondSafeLocation = new XYZTCoordinateConfig();



            ChipPPOrigion = new XYZTCoordinateConfig();
            SubmountPPOrigion = new XYZTCoordinateConfig();


            BondChipOrigion = new XYZTCoordinateConfig();
            BondSubmountOrigion = new XYZTCoordinateConfig();

            BondPosition = new XYZTCoordinateConfig();
            BondPosition4LoadPP = new XYZTCoordinateConfig();

            CuttingPosition = new XYZTCoordinateConfig();

            AbandonMaterialPosition = new XYZTCoordinateConfig();


            TrackEpoxtOrigion = new XYZTCoordinateConfig();
            TrackEpoxtSpotCoordinate = new XYZTCoordinateConfig();
            TrackBondCameraToEpoxtSpotCoordinate = new XYZTCoordinateConfig();

            EpoxtToDippingglueCoordinate = new XYZTCoordinateConfig();

            CalibrationTableOrigion = new XYZTCoordinateConfig();

            ESZSafeZoneofWaferTablePoint1 = new PointF();
            ESZSafeZoneofWaferTablePoint2 = new PointF();
            ESZSafeZoneofWaferTablePoint3 = new PointF();

            BondXYSafeRangeForBondZ = new PointF();
        }


        /// <summary>
        /// 榜头相机对焦系统原点坐标
        /// </summary>
        [XmlElement("BondOrigion")]
        public XYZTCoordinateConfig BondOrigion { get; set; }

        /// <summary>
        /// 榜头相机对焦轨道原点坐标
        /// </summary>
        [XmlElement("TrackOrigion")]
        public XYZTCoordinateConfig TrackOrigion { get; set; }
        /// <summary>
        /// 激光传感器对准轨道原点坐标
        /// </summary>
        [XmlElement("TrackLaserSensorOrigion")]
        public XYZTCoordinateConfig TrackLaserSensorOrigion { get; set; }
        /// <summary>
        /// 芯片吸嘴对准轨道原点坐标
        /// </summary>
        [XmlElement("TrackChipPPOrigion")]
        public XYZTCoordinateConfig TrackChipPPOrigion { get; set; }
        /// <summary>
        /// 基底吸嘴对准轨道原点坐标
        /// </summary>
        [XmlElement("TrackSubmountPPOrigion")]
        public XYZTCoordinateConfig TrackSubmountPPOrigion { get; set; }
        /// <summary>
        /// 点胶针对准轨道原点坐标
        /// </summary>
        [XmlElement("TrackEpoxtOrigion")]
        public XYZTCoordinateConfig TrackEpoxtOrigion { get; set; }

        /// <summary>
        /// 点胶针点胶坐标
        /// </summary>
        [XmlElement("TrackEpoxtSpotCoordinate")]
        public XYZTCoordinateConfig TrackEpoxtSpotCoordinate { get; set; }

        /// <summary>
        /// 相机对准点胶位置坐标
        /// </summary>
        [XmlElement("TrackBondCameraToEpoxtSpotCoordinate")]
        public XYZTCoordinateConfig TrackBondCameraToEpoxtSpotCoordinate { get; set; }

        /// <summary>
        /// 蘸胶位置坐标
        /// </summary>
        [XmlElement("EpoxtToDippingglueCoordinate")]
        public XYZTCoordinateConfig EpoxtToDippingglueCoordinate { get; set; }


        /// <summary>
        /// 相机对焦轨道时激光传感器测量的高度
        /// </summary>
        [XmlElement("TrackLaserSensorZ")]
        public double TrackLaserSensorZ { get; set; }


        /// <summary>
        /// 榜头相机对焦晶圆相机中心坐标
        /// </summary>
        [XmlElement("WaferCameraOrigion")]
        public XYZTCoordinateConfig WaferCameraOrigion { get; set; }
        /// <summary>
        /// 激光传感器对准晶圆相机中心坐标
        /// </summary>
        [XmlElement("WaferLaserSensorOrigion")]
        public XYZTCoordinateConfig WaferLaserSensorOrigion { get; set; }
        /// <summary>
        /// 芯片吸嘴对准晶圆相机中心坐标
        /// </summary>
        [XmlElement("WaferChipPPOrigion")]
        public XYZTCoordinateConfig WaferChipPPOrigion { get; set; }
        /// <summary>
        /// 基底吸嘴对准晶圆相机中心坐标
        /// </summary>
        [XmlElement("WaferSubmountPPOrigion")]
        public XYZTCoordinateConfig WaferSubmountPPOrigion { get; set; }

        /// <summary>
        /// 相机对焦晶圆时激光传感器测量的高度
        /// </summary>
        [XmlElement("WaferLaserSensorZ")]
        public double WaferLaserSensorZ { get; set; }



        /// <summary>
        /// 榜头相机对焦仰视相机中心坐标
        /// </summary>
        [XmlElement("LookupCameraOrigion")]
        public XYZTCoordinateConfig LookupCameraOrigion { get; set; }

        /// <summary>
        /// 榜头相机对二次校准台中心时的坐标,这里的Z记录的是校准台相对于mark台的高度
        /// </summary>
        [XmlElement("CalibrationTableOrigion")]
        public XYZTCoordinateConfig CalibrationTableOrigion { get; set; }

        /// <summary>
        /// 榜头1在仰视相机中心的坐标
        /// </summary>
        [XmlElement("LookupChipPPOrigion")]
        public XYZTCoordinateConfig LookupChipPPOrigion { get; set; }

        /// <summary>
        /// 榜头2在仰视相机中心的坐标
        /// </summary>
        [XmlElement("LookupSubmountPPOrigion")]
        public XYZTCoordinateConfig LookupSubmountPPOrigion { get; set; }

        /// <summary>
        /// 激光传感器在仰视相机中心的坐标
        /// </summary>
        [XmlElement("LookupLaserSensorOrigion")]
        public XYZTCoordinateConfig LookupLaserSensorOrigion { get; set; }
        /// <summary>
        /// 激光传感器在仰视相机中心的坐标
        /// </summary>
        [XmlElement("BondPosition4LoadPP")]
        public XYZTCoordinateConfig BondPosition4LoadPP { get; set; }
        /// <summary>
        /// 榜头1与榜头相机中心的偏移
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig PP1AndBondCameraOffset
        {
            get
            {
                XYZTCoordinateConfig Offset = new XYZTCoordinateConfig();
                Offset.X = LookupChipPPOrigion.X - LookupCameraOrigion.X;
                Offset.Y = LookupChipPPOrigion.Y - LookupCameraOrigion.Y;
                Offset.Z = TrackChipPPOrigion.Z - TrackOrigion.Z;
                return Offset;
            }
            set
            {

            }
        }

        /// <summary>
        /// 榜头2与榜头相机中心的偏移
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig PP2AndBondCameraOffset
        {
            get
            {
                XYZTCoordinateConfig Offset = new XYZTCoordinateConfig();
                Offset.X = LookupSubmountPPOrigion.X - LookupCameraOrigion.X;
                Offset.Y = LookupSubmountPPOrigion.Y - LookupCameraOrigion.Y;
                Offset.Z = TrackSubmountPPOrigion.Z - TrackOrigion.Z;
                return Offset;
            }
            set
            {

            }
        }

        /// <summary>
        /// 点胶针与榜头相机中心的偏移
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig EpoxtAndBondCameraOffset
        {
            get
            {
                XYZTCoordinateConfig Offset = new XYZTCoordinateConfig();
                Offset.X = TrackEpoxtSpotCoordinate.X - TrackBondCameraToEpoxtSpotCoordinate.X;
                Offset.Y = TrackEpoxtSpotCoordinate.Y - TrackBondCameraToEpoxtSpotCoordinate.Y;
                Offset.Z = TrackEpoxtSpotCoordinate.Z - TrackBondCameraToEpoxtSpotCoordinate.Z;
                return Offset;
            }
            set
            {

            }
        }

        /// <summary>
        /// 激光传感器与榜头相机中心的偏移
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig LaserSensorAndBondCameraOffset
        {
            get
            {
                XYZTCoordinateConfig Offset = new XYZTCoordinateConfig();
                Offset.X = LookupLaserSensorOrigion.X - LookupCameraOrigion.X;
                Offset.Y = LookupLaserSensorOrigion.Y - LookupCameraOrigion.Y;
                Offset.Z = TrackLaserSensorOrigion.Z - TrackOrigion.Z;
                return Offset;
            }
            set
            {

            }
        }


        /// <summary>
        /// 晶圆相机对晶圆盘mark中心坐标
        /// </summary>
        [XmlElement("WaferOrigion")]
        public XYZTCoordinateConfig WaferOrigion { get; set; }


        /// <summary>
        /// 榜头安全位置
        /// </summary>
        [XmlElement("BondSafeLocation")]
        public XYZTCoordinateConfig BondSafeLocation { get; set; }


        /// <summary>
        /// 榜头相机对准共晶焊接位置
        /// </summary>
        [XmlElement("EutecticWeldingLocation")]
        public XYZTCoordinateConfig EutecticWeldingLocation { get; set; }
        /// <summary>
        /// 芯片吸嘴对准共晶焊接位置
        /// </summary>
        [XmlElement("EutecticWeldingChipPPLocation")]
        public XYZTCoordinateConfig EutecticWeldingChipPPLocation { get; set; }
        /// <summary>
        /// 基底吸嘴对准共晶焊接位置
        /// </summary>
        [XmlElement("EutecticWeldingSubmountPPLocation")]
        public XYZTCoordinateConfig EutecticWeldingSubmountPPLocation { get; set; }
        /// <summary>
        /// 激光传感器对准共晶焊接位置
        /// </summary>
        [XmlElement("EutecticWeldingLaserSensorLocation")]
        public XYZTCoordinateConfig EutecticWeldingLaserSensorLocation { get; set; }



        /// <summary>
        /// 基底吸嘴Z空闲位置
        /// </summary>
        [XmlElement("SubmountPPFreeZ")]
        public double SubmountPPFreeZ { get; set; }
        /// <summary>
        /// 基底吸嘴Z工作位置
        /// </summary>
        [XmlElement("SubmountPPWorkZ")]
        public double SubmountPPWorkZ { get; set; }


        [XmlElement("ChipPPOrigion")]
        public XYZTCoordinateConfig ChipPPOrigion { get; set; }
        [XmlElement("SubmountPPOrigion")]
        public XYZTCoordinateConfig SubmountPPOrigion { get; set; }


        /// <summary>
        /// 芯片位置
        /// </summary>
        [XmlElement("BondChipOrigion")]
        public XYZTCoordinateConfig BondChipOrigion { get; set; }


        /// <summary>
        /// 衬底位置
        /// </summary>
        [XmlElement("BondSubmountOrigion")]
        public XYZTCoordinateConfig BondSubmountOrigion { get; set; }

        /// <summary>
        /// 贴装位置
        /// </summary>
        [XmlElement("BondPosition")]
        public XYZTCoordinateConfig BondPosition { get; set; }

        /// <summary>
        /// 下料位置
        /// </summary>
        [XmlElement("CuttingPosition")]
        public XYZTCoordinateConfig CuttingPosition { get; set; }

        /// <summary>
        /// 抛料位置(榜头相机对准抛料时的位置)
        /// </summary>
        [XmlElement("AbandonMaterialPosition")]
        public XYZTCoordinateConfig AbandonMaterialPosition { get; set; }
        [XmlElement("ESZSafeZoneofWaferTablePoint1")]
        public PointF ESZSafeZoneofWaferTablePoint1 { get; set; }
        [XmlElement("ESZSafeZoneofWaferTablePoint2")]
        public PointF ESZSafeZoneofWaferTablePoint2 { get; set; }

        [XmlElement("ESZSafeZoneofWaferTablePoint3")]
        public PointF ESZSafeZoneofWaferTablePoint3 { get; set; }
        [XmlElement("ESZWariningPos")]
        public float ESZWariningPos { get; set; }
        [XmlElement("BondZWariningPos")]
        public float BondZWariningPos { get; set; }
        /// <summary>
        /// 在此范围内BondZ可以去晶圆盘取料,此范围的中心时榜头相机中心对准晶圆相机中心时BONDXY的stage坐标
        /// </summary>
        [XmlElement("BondXYSafeRangeForBondZ")]
        public PointF BondXYSafeRangeForBondZ { get; set; }

    }

    /// <summary>
    /// 系统校准配置文件
    /// </summary>
    [Serializable]
    public class SystemCalibrationConfig
    {
        public SystemCalibrationConfig()
        {
            BondIdentifyBMCMatch = new MatchIdentificationParam();
            BondIdentifyBMCMatch2 = new MatchIdentificationParam();
            BondIdentifyBMCMatchoffset = new XYZTCoordinateConfig();
            UplookingIdentifyBMCLineFind = new LineFindIdentificationParam();
            UplookingIdentifyBMCMatch = new MatchIdentificationParam();
            UplookingIdentifyBMCMatch2 = new MatchIdentificationParam();
            BondIdentifyBMCSubstrateMatch = new MatchIdentificationParam();
            BondIdentifyBMCSubstrateMatch2 = new MatchIdentificationParam();
            BondIdentifyBMCSubstrateMatchoffset = new XYZTCoordinateConfig();

            BondIdentifyBondOrigionMatch = new MatchIdentificationParam();
            BondIdentifyTrackOrigionMatch = new MatchIdentificationParam();
            BondIdentifyWaferCameraOrigionMatch = new MatchIdentificationParam();
            BondIdentifyLookupCameraOrigionMatch = new MatchIdentificationParam();
            WaferIdentifyWaferOrigionMatch = new MatchIdentificationParam();
            UplookingIdentifyBond1Match = new MatchIdentificationParam();
            UplookingIdentifyBond2Match = new MatchIdentificationParam();
            UplookingIdentifyLaserSensorMatch = new MatchIdentificationParam();

            
            BondIdentifyEutecticWeldingMatch = new MatchIdentificationParam();
            

            BondIdentifySubmountMatch = new MatchIdentificationParam();
            BondIdentifyChipMatch = new MatchIdentificationParam();

            UplookingIdentifySubmountMatch = new MatchIdentificationParam();
            UplookingIdentifyChipMatch = new MatchIdentificationParam();

            BondIdentifyBondPositionMatch = new MatchIdentificationParam();
            BondIdentifyCuttingPositionMatch = new MatchIdentificationParam();
        }

        /// <summary>
        /// BMC次数
        /// </summary>
        [XmlElement("BMCtimes")]
        public int BMCtimes { get; set; }

        /// <summary>
        /// BMC间隔时间 ms
        /// </summary>
        [XmlElement("BMCdelaytime")]
        public int BMCdelaytime { get; set; }

        /// <summary>
        /// 榜头相机识别BMC特征点个数
        /// </summary>
        [XmlElement("BondIdentifyBMCSpotNum")]
        public int BondIdentifyBMCSpotNum { get; set; }
        /// <summary>
        /// 榜头相机识别BMC识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyBMCNum")]
        public int BondIdentifyBMCNum { get; set; }

        /// <summary>
        /// 榜头相机识别校准台
        /// </summary>
        [XmlElement("BondIdentifyCalibrationTableMatch")]
        public MatchIdentificationParam BondIdentifyCalibrationTableMatch { get; set; }

        /// <summary>
        /// 榜头相机识别BMC
        /// </summary>
        [XmlElement("BondIdentifyBMCMatch")]
        public MatchIdentificationParam BondIdentifyBMCMatch { get; set; }
        /// <summary>
        /// 榜头相机识别BMC
        /// </summary>
        [XmlElement("BondIdentifyBMCMatch2")]
        public MatchIdentificationParam BondIdentifyBMCMatch2 { get; set; }
        /// <summary>
        /// 相机对准吸取BMC的位置(相对于第一个特征点的偏移坐标 相对偏移) Theta为两个特征点连线相对于相机的角度
        /// </summary>
        [XmlElement("BondIdentifyBMCMatchoffset")]
        public XYZTCoordinateConfig BondIdentifyBMCMatchoffset { get; set; }



        /// <summary>
        /// 仰视相机识别BMC特征点个数
        /// </summary>
        [XmlElement("UplookingIdentifyBMCSpotNum")]
        public int UplookingIdentifyBMCSpotNum { get; set; }

        /// <summary>
        /// 仰视相机识别BMC识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("UplookingIdentifyBMCNum")]
        public int UplookingIdentifyBMCNum { get; set; }
        /// <summary>
        /// 仰视相机识别BMC 轮廓
        /// </summary>
        [XmlElement("UplookingIdentifyBMCMatch")]
        public MatchIdentificationParam UplookingIdentifyBMCMatch { get; set; }
        /// <summary>
        /// 仰视相机识别BMC 轮廓
        /// </summary>
        [XmlElement("UplookingIdentifyBMCMatch2")]
        public MatchIdentificationParam UplookingIdentifyBMCMatch2 { get; set; }
        /// <summary>
        /// 仰视相机识别BMC 边缘
        /// </summary>
        [XmlElement("UplookingIdentifyBMCLineFind")]
        public LineFindIdentificationParam UplookingIdentifyBMCLineFind { get; set; }
        /// <summary>
        /// 相机对准吸取BMC中心的位置(相对于第一个特征点的偏移坐标 相对偏移) Theta为两个特征点连线相对于相机的角度
        /// </summary>
        [XmlElement("UplookingIdentifyBMCMatchoffset")]
        public XYZTCoordinateConfig UplookingIdentifyBMCMatchoffset { get; set; }


        /// <summary>
        /// 榜头相机识别BMC基板特征点个数
        /// </summary>
        [XmlElement("BondIdentifyBMCSubstrateSpotNum")]
        public int BondIdentifyBMCSubstrateSpotNum { get; set; }
        /// <summary>
        /// 榜头相机识别BMC基板识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyBMCSubstrateNum")]
        public int BondIdentifyBMCSubstrateNum { get; set; }
        /// <summary>
        /// 榜头相机识别BMC基板
        /// </summary>
        [XmlElement("BondIdentifyBMCSubstrateMatch")]
        public MatchIdentificationParam BondIdentifyBMCSubstrateMatch { get; set; }
        /// <summary>
        /// 榜头相机识别BMC基板
        /// </summary>
        [XmlElement("BondIdentifyBMCSubstrateMatch2")]
        public MatchIdentificationParam BondIdentifyBMCSubstrateMatch2 { get; set; }
        /// <summary>
        /// 贴片位置(相对于第一个特征点的偏移坐标 相对偏移)
        /// </summary>
        [XmlElement("BondIdentifyBMCSubstrateMatchoffset")]
        public XYZTCoordinateConfig BondIdentifyBMCSubstrateMatchoffset { get; set; }


        /// <summary>
        /// 榜头相机识别轨道原点 移动阵列个数 行数
        /// </summary>
        [XmlElement("BondIdentifyTrackOrigionArrayRowNum")]
        public int BondIdentifyTrackOrigionArrayRowNum { get; set; }
        /// <summary>
        /// 榜头相机识别轨道原点 移动阵列个数 列数
        /// </summary>
        [XmlElement("BondIdentifyTrackOrigionArrayColNum")]
        public int BondIdentifyTrackOrigionArrayColNum { get; set; }
        /// <summary>
        /// 榜头相机识别轨道原点 移动范围 宽
        /// </summary>
        [XmlElement("BondIdentifyTrackOrigionArrayWidthRange")]
        public double BondIdentifyTrackOrigionArrayWidthRange { get; set; }
        /// <summary>
        /// 榜头相机识别轨道原点 移动范围 高
        /// </summary>
        [XmlElement("BondIdentifyTrackOrigionArrayHeightRange")]
        public double BondIdentifyTrackOrigionArrayHeightRange { get; set; }


        /// <summary>
        /// 榜头相机识别系统原点识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyBondOrigionNum")]
        public int BondIdentifyBondOrigionNum { get; set; }
        /// <summary>
        /// 榜头相机识别系统原点
        /// </summary>
        [XmlElement("BondIdentifyBondOrigionMatch")]
        public MatchIdentificationParam BondIdentifyBondOrigionMatch { get; set; }

        /// <summary>
        /// 榜头相机识别轨道原点识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyTrackOrigionNum")]
        public int BondIdentifyTrackOrigionNum { get; set; }
        /// <summary>
        /// 榜头相机识别轨道原点
        /// </summary>
        [XmlElement("BondIdentifyTrackOrigionMatch")]
        public MatchIdentificationParam BondIdentifyTrackOrigionMatch { get; set; }

        /// <summary>
        /// 榜头相机识别晶圆相机中心识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyWaferCameraOrigionNum")]
        public int BondIdentifyWaferCameraOrigionNum { get; set; }
        /// <summary>
        /// 榜头相机识别晶圆相机中心
        /// </summary>
        [XmlElement("BondIdentifyWaferCameraOrigionMatch")]
        public MatchIdentificationParam BondIdentifyWaferCameraOrigionMatch { get; set; }

        /// <summary>
        /// 榜头相机识别仰视相机中心识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyLookupCameraOrigionNum")]
        public int BondIdentifyLookupCameraOrigionNum { get; set; }
        /// <summary>
        /// 榜头相机识别仰视相机中心
        /// </summary>
        [XmlElement("BondIdentifyLookupCameraOrigionMatch")]
        public MatchIdentificationParam BondIdentifyLookupCameraOrigionMatch { get; set; }


        /// <summary>
        /// 晶圆相机识别晶圆盘mark中心 移动阵列个数 行数
        /// </summary>
        [XmlElement("WaferIdentifyWaferOrigionArrayRowNum")]
        public int WaferIdentifyWaferOrigionArrayRowNum { get; set; }
        /// <summary>
        /// 晶圆相机识别晶圆盘mark中心 移动阵列个数 列数
        /// </summary>
        [XmlElement("WaferIdentifyWaferOrigionArrayColNum")]
        public int WaferIdentifyWaferOrigionArrayColNum { get; set; }
        /// <summary>
        /// 晶圆相机识别晶圆盘mark中心 移动范围 宽
        /// </summary>
        [XmlElement("WaferIdentifyWaferOrigionArrayWidthRange")]
        public double WaferIdentifyWaferOrigionArrayWidthRange { get; set; }
        /// <summary>
        /// 晶圆相机识别晶圆盘mark中心 移动范围 高
        /// </summary>
        [XmlElement("WaferIdentifyWaferOrigionArrayHeightRange")]
        public double WaferIdentifyWaferOrigionArrayHeightRange { get; set; }

        /// <summary>
        /// 晶圆相机识别晶圆盘mark中心识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("WaferIdentifyWaferOrigionNum")]
        public int WaferIdentifyWaferOrigionNum { get; set; }
        /// <summary>
        /// 晶圆相机识别晶圆盘mark中心
        /// </summary>
        [XmlElement("WaferIdentifyWaferOrigionMatch")]
        public MatchIdentificationParam WaferIdentifyWaferOrigionMatch { get; set; }

        /// <summary>
        /// 仰视相机识别榜头1中心识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("UplookingIdentifyBond1Num")]
        public int UplookingIdentifyBond1Num { get; set; }
        /// <summary>
        /// 仰视相机识别榜头1中心
        /// </summary>
        [XmlElement("UplookingIdentifyBond1Match")]
        public MatchIdentificationParam UplookingIdentifyBond1Match { get; set; }


        /// <summary>
        /// 仰视相机识别榜头中心 移动阵列个数 行数
        /// </summary>
        [XmlElement("UplookingIdentifyBondArrayRowNum")]
        public int UplookingIdentifyBondArrayRowNum { get; set; }
        /// <summary>
        /// 仰视相机识别榜头中心 移动阵列个数 列数
        /// </summary>
        [XmlElement("UplookingIdentifyBondArrayColNum")]
        public int UplookingIdentifyBondArrayColNum { get; set; }
        /// <summary>
        /// 仰视相机识别榜头中心 移动范围 宽
        /// </summary>
        [XmlElement("UplookingIdentifyBondArrayWidthRange")]
        public double UplookingIdentifyBondArrayWidthRange { get; set; }
        /// <summary>
        /// 仰视相机识别榜头中心 移动范围 高
        /// </summary>
        [XmlElement("UplookingIdentifyBondArrayHeightRange")]
        public double UplookingIdentifyBondArrayHeightRange { get; set; }

        /// <summary>
        /// 仰视相机识别榜头2中心识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("UplookingIdentifyBond2Num")]
        public int UplookingIdentifyBond2Num { get; set; }
        /// <summary>
        /// 仰视相机识别榜头2中心
        /// </summary>
        [XmlElement("UplookingIdentifyBond2Match")]
        public MatchIdentificationParam UplookingIdentifyBond2Match { get; set; }

        /// <summary>
        /// 仰视相机识别激光传感器中心
        /// </summary>
        [XmlElement("UplookingIdentifyLaserSensorMatch")]
        public MatchIdentificationParam UplookingIdentifyLaserSensorMatch { get; set; }



        /// <summary>
        /// 榜头相机识别共晶焊接识别方式 0轮廓 1边缘 2圆
        /// </summary>
        [XmlElement("BondIdentifyEutecticWeldingNum")]
        public int BondIdentifyEutecticWeldingNum { get; set; }
        /// <summary>
        /// 榜头相机识别共晶焊接
        /// </summary>
        [XmlElement("BondIdentifyEutecticWeldingMatch")]
        public MatchIdentificationParam BondIdentifyEutecticWeldingMatch { get; set; }




        /// <summary>
        /// 榜头相机识别衬底参数
        /// </summary>
        [XmlElement("BondIdentifySubmountMatch")]
        public MatchIdentificationParam BondIdentifySubmountMatch { get; set; }

        /// <summary>
        /// 榜头相机识别芯片参数
        /// </summary>
        [XmlElement("BondIdentifyChipMatch")]
        public MatchIdentificationParam BondIdentifyChipMatch { get; set; }

        /// <summary>
        /// 仰视相机识别衬底参数
        /// </summary>
        [XmlElement("UplookingIdentifySubmountMatch")]
        public MatchIdentificationParam UplookingIdentifySubmountMatch { get; set; }

        /// <summary>
        /// 仰视相机识别芯片参数
        /// </summary>
        [XmlElement("UplookingIdentifyChipMatch")]
        public MatchIdentificationParam UplookingIdentifyChipMatch { get; set; }

        /// <summary>
        /// 榜头相机识别贴装位置
        /// </summary>
        [XmlElement("BondIdentifyBondPositionMatch")]
        public MatchIdentificationParam BondIdentifyBondPositionMatch { get; set; }

        /// <summary>
        /// 榜头相机识别下料位置
        /// </summary>
        [XmlElement("BondIdentifyCuttingPositionMatch")]
        public MatchIdentificationParam BondIdentifyCuttingPositionMatch { get; set; }

    }

    [Serializable]
    public class CalibrationConfig
    {
        public CalibrationConfig()
        {
            ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig();
            ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig();
            SubmountPPPosCompensateCoordinate1 = new XYZTCoordinateConfig();
            SubmountPPPosCompensateCoordinate2 = new XYZTCoordinateConfig();
        }
        /// <summary>
        /// 计算芯片吸嘴旋转补偿XY公式的坐标1，一般在T0度纪录
        /// </summary>
        [XmlElement("ChipPPPosCompensateCoordinate1")]
        public XYZTCoordinateConfig ChipPPPosCompensateCoordinate1 { get; set; }
        /// <summary>
        /// 计算芯片吸嘴旋转补偿XY公式的坐标2，一般在T180度纪录
        /// </summary>
        [XmlElement("ChipPPPosCompensateCoordinate2")]
        public XYZTCoordinateConfig ChipPPPosCompensateCoordinate2 { get; set; }

        /// <summary>
        /// 计算热衬吸嘴旋转补偿XY公式的坐标1，一般在T0度纪录
        /// </summary>
        [XmlElement("SubmountPPPosCompensateCoordinate1")]
        public XYZTCoordinateConfig SubmountPPPosCompensateCoordinate1 { get; set; }
        /// <summary>
        /// 计算热衬吸嘴旋转补偿XY公式的坐标2，一般在T180度纪录
        /// </summary>
        [XmlElement("SubmountPPPosCompensateCoordinate2")]
        public XYZTCoordinateConfig SubmountPPPosCompensateCoordinate2 { get; set; }
    }
}