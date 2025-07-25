using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VisionClsLib;
using WestDragon.Framework.UtilityHelper;

namespace GlobalDataDefineClsLib
{
    [Serializable]
    public enum EnumRunningType { Actual, Simulated }
    [Serializable]
    public enum EnumSystemRole { System_Master, System_Slave1, System_Slave2, System_Slave3, System_Slave4, System_Slave5, System_Slave6, System_Slave7, System_Slave8 }
    public enum EnumSystemMode { Attach=1,Eutectic=0}

    [Serializable]
    public enum EnumStageAxis
    {
        [Description("无")]
        None = -1,

        [Description("Bond头Y轴")]
        BondY = 1,

        [Description("Bond头X轴")]
        BondX = 2,

        [Description("Bond头Z轴")]
        BondZ = 3,

        [Description("芯片吸嘴Theta轴")]
        ChipPPT = 6,

        [Description("芯片吸嘴Z轴")]
        ChipPPZ = 101,

        [Description("基板吸嘴Theta轴")]
        SubmountPPT = 14,

        [Description("基板吸嘴Z轴")]
        SubmountPPZ = 13,

        [Description("吸嘴库Theta轴")]
        PPtoolBankTheta = 50,

        [Description("蘸胶轴")]
        DippingGlue = 60,

        [Description("传送轨道1轴")]
        TransportTrack1 = 12,

        [Description("传送轨道2轴")]
        TransportTrack2 = 11,

        [Description("传送轨道3轴")]
        TransportTrack3 = 10,


        [Description("晶圆盘Y轴")]
        WaferTableY = 7,

        [Description("晶圆盘X轴")]
        WaferTableX = 9,

        [Description("俯视相机轴")]
        WaferTableZ = 4,

        [Description("晶圆阔膜轴")]
        WaferFilm = 130,

        [Description("晶圆夹爪轴")]
        WaferFinger = 140,

        [Description("晶圆抽匣升降轴")]
        WaferCassetteLift = 150,

        [Description("顶针底座轴")]
        ESZ = 8,

        [Description("顶针轴")]
        NeedleZ = 5,

        [Description("顶针切换轴")]
        NeedleSwitch = 180,

        [Description("翻转工具Theta轴")]
        FilpToolTheta = 190,





    }

    [Serializable]
    public enum EnumStageSystem
    {
        BondTable,
        Transport,
        WaferTable,
        WaferCassette,
        ChipPP,
        SubmountPP,
        ES
    }
    [Serializable]
    public enum EnumSystemAxis
    {
        XY,
        X,
        Z,
        NeedleZ,
        Focus,
        Theta,
        WaferFilm,
        Stage1,
        Stage2,
        Stage3,
    }
    [Serializable]
    public enum EnumAxisStatus
    {
        Alarm = 2,
        Enable = 3,
        PlanMotion = 4,
        SmoothStop = 5,
        PositiveLimit = 6,
        NegativeLimit = 7,
        NotorInPlace = 8,
        EmergencyStop = 9,
    }
    [Serializable]
    public enum EnumFindBondPositionMethod
    {
        NoAccuracy,
        Accuracy,
    }
    [Serializable]
    public enum EnumCameraProducer { HIK, DaHeng }
    [Serializable]
    public enum EnumCameraType
    {
        None = 0,
        BondCamera = 1,
        UplookingCamera = 2,
        WaferCamera = 3,
    }
    [Serializable]
    public enum EnumImageData
    {
        Grey, RGB
    }
    [Serializable]
    public enum EnumLightProducer { KOMA, HIK, OPT }
    [Serializable]
    public enum EnumLightSourceType
    {
        WaferRingField, BondRingField, LookupRingField
        , WaferDirectField, BondDirectField, LookupDirectField, 
        WaferDirectRedField, WaferDirectGreenField, WaferDirectBlueField,
        BondDirectRedField, BondDirectGreenField, BondDirectBlueField
    }

    [Serializable]
    public enum EnumDirectLightSourceType
    {
        SingleR, RGB,
    }
    [Serializable]
    public enum EnumLaserProducer { BOJKESensor, PHOSKEY }
    [Serializable]
    public enum DynamometerProducer { JNSENSOR, RLD }
    [Serializable]
    public enum EnumCommunicationType { SerialPort, Ethernet }



    public enum EnumRecogniseResulSaveOption
    {
        NotSave = 0, SaveOK = 1, SaveNG = 2, AllSave = 3
    }
    /// <summary>
    /// Recipe根节点步骤
    /// </summary>
    public enum EnumRecipeRootStep 
    { 
        None,
        Substrate,
        Dispenser, 
        BondPosition,
        Component,
        EpoxySettings
    }
    /// <summary>
    /// Recipe步骤
    /// </summary>
    [Serializable]
    public enum EnumRecipeStep
    {
        Create,
        Configuration,

        Substrate_InformationSettings,
        Substrate_PositionSettings,
        Substrate_MaterialMap,
        Substrate_PPSettings,
        Substrate_Accuracy,

        Module_PositionSettings,
        Module_MaterialMap,

        BondPosition,

        Component_InformationSettings,
        Component_PositionSettings,
        Component_MaterialMap,
        Component_PPSettings,
        Component_Accuracy,

        EutecticSettings,

        ProductStepList,
        BlankingSetting,
        EpoxyApplications,
        DispenserSettings,

        None
    }
    /// <summary>
    /// Recipe步骤
    /// </summary>
    [Serializable]
    public enum EnumDefinedSingleStep
    {
        CHipPPPick,
        ChipPPPlace,
        SubmountPPPick,
        SubmountPPPlace,

        ComponentVisionPosition,
        ComponentUplookingAccuracy,

        SubmountVisionPosition,
        SubmountUplookingAccuracy,

        SubmountVisionPositionBeforeBond,

        Bond,
        Eutectic,

        SubmountVisionPositionBeforeBlanking,

        None
    }
    [Serializable]
    public enum EnumDefineSetupRecipeComponentPositionStep
    {
        None,
        SetWorkHeight,
        SetComponentLeftUpperCorner,
        SetComponentRightUpperCorner,
        SetComponentRightLowerCorner,
        SetComponentLeftLowerCorner,
        VisionPosition
    }



    [Serializable]
    public enum EnumDefineSetupRecipeComponentMapStep
    {
        None,
        SetFirstComponentPos,
        SetColumnMap,   //包括列数和列间距
        SetRowMap,    //包括行数和行间距
        SetDeterminWaferRangeFirstPoint,
        SetDeterminWaferRangeSecondPoint,
        SetDeterminWaferRangeThirdPoint
    }
    [Serializable]
    public enum EnumDefineSetupRecipeComponentAccuracyStep
    {
        None,
        SetComponentLeftUpperCorner,
        SetComponentRightUpperCorner,
        SetComponentRightLowerCorner,
        SetComponentLeftLowerCorner,
        AccuracyPosition,
        LeftUpperEdgeSearch,
        RighLowerEdgeSearch
    }

    [Serializable]
    public enum EnumDefineSetupRecipeSubstratePositionStep
    {
        None,
        SetWorkHeight,
        SetLeftUpperCorner,
        SetRightUpperCorner,
        SetRightLowerCorner,
        SetLeftLowerCorner,
        //VisionPosition,
        SetMark1VisionParam,
        SetMark2VisionParam
    }
    [Serializable]
    public enum EnumDefineSetupRecipeModulePositionStep
    {
        None,
        SetWorkHeight,
        SetLeftUpperCorner,
        SetRightUpperCorner,
        SetRightLowerCorner,
        SetLeftLowerCorner,
        VisionPosition
    }

    [Serializable]
    public enum EnumDefineSetupRecipeSubstrateMapStep
    {
        None,
        SetFirstMaterialPos,
        SetColumnMap,   //包括列数和列间距
        SetRowMap,    //包括行数和行间距
        SetDeterminWaferRangeFirstPoint,
        SetDeterminWaferRangeSecondPoint,
        SetDeterminWaferRangeThirdPoint
    }
    [Serializable]
    public enum EnumDefineSetupRecipeSubmountAccuracyStep
    {
        None,
        SetSubmountLeftUpperCorner,
        SetSubmountRightUpperCorner,
        SetSubmountRightLowerCorner,
        SetSubmountLeftLowerCorner,
        EdgeSearch,
        LeftUpperEdgeSearch,
        RighLowerEdgeSearch
    }

    [Serializable]
    public enum EnumDefineSetupRecipeBondPositionStep
    {
        None,
        SetPPHeight,
        SetSubmountLeftUpperCorner,
        SetSubmountRightUpperCorner,
        SetSubmountRightLowerCorner,
        SetSubmountLeftLowerCorner,
        VisionPosition,
        SetBondPosition
    }
    [Serializable]
    public enum EnumDefineSetupRecipeBlankingSettingsStep
    {
        None,
        SetPickParameters,
        BlankingMethod
    }

    [Serializable]
    public enum EnumDefineDispenserSettingsStep
    {
        None,
        DispenserInfo,
        DispenserPosition,
        DispenserWorkHeight
    }

    [Serializable]
    public enum EnumDefineEpoxyApplicationStep
    {
        None,
        ExpoxySettings
    }
    [Serializable]
    public enum EnumDefinePPAlignStep
    {
        None,
        PositionCenterFirst,
        PositionCenterSecond
    }
    [Serializable]
    public enum EnumHardwareType
    {
        Stage,
        Camera,
        Light,
        ControlBoard,
        PowerController,
        LaserSensor,
        Dynamometer,
        DispensingMachine,
    }
    [Serializable]
    public enum EnumCarrierType
    {
        [Description("蓝膜")]
        Wafer,
        [Description("华夫盒")]
        WafflePack,
        [Description("蓝膜华夫盒")]
        WaferWafflePack
    }

    public enum EnumComponentType
    {
        [Description("芯片")]
        Component,
        [Description("衬底")]
        Submonut,
    }

    [Serializable]
    public enum EnumYesOrNo
    {
        [Description("是")]
        Yes,
        [Description("否")]
        No
    }
    [Serializable]
    public enum EnumBlankingMethod
    {
        OriginalRoad,
        BlankingArea
    }
    /// <summary>
    /// 加热段参数
    /// </summary>
    [Serializable]
    public class HeatSegmentParam
    {
        public int segmentNo { get; set; }

        public double TimespanMinite { get; set; }

        public double TargetTemprerature { get; set; }

    }
    [Serializable]
    public enum EnumUsedPP
    {
        [Description("芯片吸嘴")]
        ChipPP,

        [Description("基底吸嘴")]
        SubmountPP
    }
    [Serializable]
    public enum EnumPPtool
    {
        [Description("芯片吸嘴")]
        PPtool1,

        [Description("基底吸嘴")]
        PPtool2
    }
    [Serializable]
    public enum EnumVisionPositioningMethod
    {
        PatternSearch,
        CircleSearch,
        EdgeSearch
    }
    [Serializable]
    public enum EnumAccuracyMethod
    {
        None,
        UplookingCamera,
        CalibrationTable
    }
    [Serializable]
    public enum EnumDispensePattern
    {
        Point,
        GreekCross,
        DiagonalCross
    }
    [Serializable]
    public class ShapeMatchParameters
    {
        public ShapeMatchParameters()
        {
            MaskSetting = new List<RecogniseMaskSetting>();
            BondTablePositionOfCreatePattern = new XYZTCoordinateConfig();
            WaferTablePositionOfCreatePattern = new XYZTCoordinateConfig();
            PositionOfMaterialCenter = new XYZTCoordinateConfig();
            RingLightSetting = new OpticalSettings();
            DirectLightSetting = new OpticalSettings();
            ROILeftUpperPoint = new PointF();
        }
        [XmlElement("Name")]
        public string Name { get; set; }


        [XmlElement("CameraZWorkPosition")]
        public float CameraZWorkPosition { get; set; }
        [XmlElement("OrigionAngle")]
        public float OrigionAngle { get; set; }
        /// <summary>
        /// 创建模板时BondTable的位置
        /// </summary>
        [XmlElement("BondTablePositionOfCreatePattern")]
        public XYZTCoordinateConfig BondTablePositionOfCreatePattern { get; set; }
        /// <summary>
        /// 创建模板时物料中心的系统坐标位
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig PositionOfMaterialCenter { get; set; }
        /// <summary>
        /// 创建模板时WaferTable的位置
        /// </summary>
        [XmlElement("WaferTablePositionOfCreatePattern")]
        public XYZTCoordinateConfig WaferTablePositionOfCreatePattern { get; set; }

        [XmlElement("RingLightSetting")]
        public OpticalSettings RingLightSetting { get; set; }
        [XmlElement("DirectLightSetting")]
        public OpticalSettings DirectLightSetting { get; set; }
        [XmlElement("ScoreThreshold")]
        public double ScoreThreshold { get; set; }
        [XmlElement("AcceptAngleThreshold")]
        public double AcceptAngleThreshold { get; set; }




        /// <summary>
        /// 
        /// </summary>
        [XmlElement("TrainFileFullName")]
        public string TrainFileFullName { get; set; }
        [XmlElement("TemplateFileFullName")]
        public string TemplateFileFullName { get; set; }
        [XmlElement("TemplateRegionLeftUpperPoint")]
        public PointF TemplateRegionLeftUpperPoint { get; set; }
        [XmlElement("TemplateRegionWidth")]
        public float TemplateRegionWidth { get; set; }
        [XmlElement("TemplateRegionHeight")]
        public float TemplateRegionHeight { get; set; }

        [XmlElement("ROILeftUpperPoint")]
        public PointF ROILeftUpperPoint { get; set; }
        [XmlElement("ROIWidth")]
        public float ROIWidth { get; set; }
        [XmlElement("ROIHeight")]
        public float ROIHeight { get; set; }
        [XmlArray("MaskSetting"), XmlArrayItem(typeof(RecogniseMaskSetting))]
        public List<RecogniseMaskSetting> MaskSetting { get; set; }

    }
    [Serializable]
    public enum EnumMaskType
    {
        None = 0,
        BondOrigion = 1,
        TrackOrigion = 2,
        WaferCameraOrigion = 3,
        LookupCameraOrigion = 4,
        WaferOrigion = 5,
        LookupUCtoolOrigion = 6,
        LookupChipPPOrigion = 7,
        LookupSubmountPPOrigion = 8,
        LookupLaserSensorOrigion = 9,
        EutecticWeldingOrigion = 10,
        CalibrationTableOrigion = 11,
    }
    [Serializable]
    [XmlType("Mask")]
    public class RecogniseMaskSetting
    {
        [XmlElement("MaskRegionLeftUpperPoint")]
        public PointF MaskRegionLeftUpperPoint { get; set; }
        [XmlElement("MaskRegionWidth")]
        public float MaskRegionWidth { get; set; }
        [XmlElement("MaskRegionHeight")]
        public float MaskRegionHeight { get; set; }
        public RecogniseMaskSetting()
        {
            MaskRegionLeftUpperPoint = new PointF();
        }
    }

    [Serializable]
    public class CircleSearchParameters
    {
        public CircleSearchParameters()
        {
            MaskSetting = new List<RecogniseMaskSetting>();
            BondTablePosition = new XYZTCoordinateConfig();
            WaferTablePosition = new XYZTCoordinateConfig();
        }
        [XmlElement("Name")]
        public string Name { get; set; }

        //[XmlElement("UsedCamera")]
        //public EnumCameraType UsedCamera { get; set; }
        [XmlElement("BondTablePosition")]
        public XYZTCoordinateConfig BondTablePosition { get; set; }
        [XmlElement("WaferTablePosition")]
        public XYZTCoordinateConfig WaferTablePosition { get; set; }
        [XmlElement("CameraZWorkPosition")]
        public float CameraZWorkPosition { get; set; }
        [XmlElement("RingLightSetting")]
        public OpticalSettings RingLightSetting { get; set; }
        [XmlElement("DirectLightSetting")]
        public OpticalSettings DirectLightSetting { get; set; }
        [XmlElement("ScoreThreshold")]
        public double ScoreThreshold { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("TrainFileFullName")]
        public string TrainFileFullName { get; set; }
        [XmlElement("TemplateFileFullName")]
        public string TemplateFileFullName { get; set; }
        [XmlElement("TemplateRegionLeftUpperPoint")]
        public PointF TemplateRegionLeftUpperPoint { get; set; }
        [XmlElement("TemplateRegionWidth")]
        public float TemplateRegionWidth { get; set; }
        [XmlElement("TemplateRegionHeight")]
        public float TemplateRegionHeight { get; set; }

        [XmlElement("ROILeftUpperPoint")]
        public PointF ROILeftUpperPoint { get; set; }
        [XmlElement("ROIWidth")]
        public float ROIWidth { get; set; }
        [XmlElement("ROIHeight")]
        public float ROIHeight { get; set; }
        [XmlArray("MaskSetting"), XmlArrayItem(typeof(RecogniseMaskSetting))]
        public List<RecogniseMaskSetting> MaskSetting { get; set; }
    }
    [Serializable]
    public class EdgeSearchParameters
    {
        public EdgeSearchParameters()
        {
            MaskSetting = new List<RecogniseMaskSetting>();
            BondTablePosition = new XYZTCoordinateConfig();
            WaferTablePosition = new XYZTCoordinateConfig();
            RingLightSetting = new OpticalSettings();
            DirectLightSetting = new OpticalSettings();
        }
        [XmlElement("Name")]
        public string Name { get; set; }

        //[XmlElement("UsedCamera")]
        //public EnumCameraType UsedCamera { get; set; }
        [XmlElement("BondTablePosition")]
        public XYZTCoordinateConfig BondTablePosition { get; set; }
        [XmlElement("WaferTablePosition")]
        public XYZTCoordinateConfig WaferTablePosition { get; set; }
        [XmlElement("CameraZWorkPosition")]
        public float CameraZWorkPosition { get; set; }
        [XmlElement("OrigionAngle")]
        public float OrigionAngle { get; set; }
        [XmlElement("RingLightSetting")]
        public OpticalSettings RingLightSetting { get; set; }
        [XmlElement("DirectLightSetting")]
        public OpticalSettings DirectLightSetting { get; set; }
        [XmlElement("ScoreThreshold")]
        public double ScoreThreshold { get; set; }
        [XmlElement("AcceptAngleThreshold")]
        public double AcceptAngleThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("TrainFileFullName")]
        public string TrainFileFullName { get; set; }
        [XmlElement("TemplateFileFullName")]
        public string TemplateFileFullName { get; set; }
        [XmlElement("UpEdgeSearchTemplateFileFullName")]
        public string UpEdgeSearchTemplateFileFullName { get; set; }
        [XmlElement("RightEdgeSearchTemplateFileFullName")]
        public string RightEdgeSearchTemplateFileFullName { get; set; }
        [XmlElement("DownEdgeSearchTemplateFileFullName")]
        public string DownEdgeSearchTemplateFileFullName { get; set; }
        [XmlElement("LeftEdgeSearchTemplateFileFullName")]
        public string LeftEdgeSearchTemplateFileFullName { get; set; }
        [XmlElement("TemplateRegionLeftUpperPoint")]
        public PointF TemplateRegionLeftUpperPoint { get; set; }
        [XmlElement("TemplateRegionWidth")]
        public float TemplateRegionWidth { get; set; }
        [XmlElement("TemplateRegionHeight")]
        public float TemplateRegionHeight { get; set; }

        [XmlElement("ROILeftUpperPoint")]
        public PointF ROILeftUpperPoint { get; set; }
        [XmlElement("ROIWidth")]
        public float ROIWidth { get; set; }
        [XmlElement("ROIHeight")]
        public float ROIHeight { get; set; }
        [XmlArray("MaskSetting"), XmlArrayItem(typeof(RecogniseMaskSetting))]
        public List<RecogniseMaskSetting> MaskSetting { get; set; }
    }
    [Serializable]
    public class OpticalSettings
    {
        [XmlAttribute("Active")]
        public bool Active { get; set; }
        [XmlAttribute("LightColor")]
        public EnumLightColor LightColor { get; set; }

        [XmlAttribute("Brightness")]
        public float Brightness { get; set; }


    }
    /// <summary>
    /// 自定义回调动作抽象基类
    /// 用于向特定的过程返回数据并进行自定义操作。
    /// </summary>
    public abstract class ACustomActionBaseClass { }

    [Serializable]
    public enum EnumRecipeType { INVALID = 0, Bonder = 1, Heat = 2, Transport = 3 }
    /// <summary>
    /// 用于保存Job分析结果的类
    /// </summary>
    [Serializable]
    public sealed class JobResult
    {
        /// <summary>
        /// 最终结果
        /// </summary>
        //public List<ProcessResult> LidFinalResults { get; set; }
        //public List<ProcessResult> ShellFinalResults { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProcessJobName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobResult()
        {
            //LidFinalResults = new List<ProcessResult>();
            //ShellFinalResults = new List<ProcessResult>();
        }
    }
    [Serializable]
    public enum EnumBonderAction
    {
        LoadSubstrate,                       //加载基板
        PositionSubstrate,                   //基板定位
        SearchBondPosition,                  //搜索贴装位置
        DispenseGlue,                        //划胶
        PositionComponent,                   //芯片定位
        PickComponent,
        AccuracyComponent,                   //芯片二次校准
        Bond,                                //贴片
        AbandonComponent,                    //抛料
        UnloadSubstrate                      //基板退料
    }
    [Serializable]
    public enum EnumTransferStatus
    {
        TransferCompleted, //整盒传输完成
        StripTransferCompleted, //整盒传输完成
        Transfering,       //正在传输
        ExceptionAborted,  //传输异常停止
        UserAbort,         //用户停止传输
        StripCleared,      //清片完成
        StripLoaded,        //一键上片完成
        /// <summary>
        /// 料盒在buffer
        /// </summary>
        MaterialboxinBuffer,
        /// <summary>
        /// 料盒在交换箱
        /// </summary>
        MaterialboxinExchangebox,
        /// <summary>
        /// 料盒在料盒轨道
        /// </summary>
        MaterialboxinMaterialboxTrack,
        /// <summary>
        /// 料盒在烘箱
        /// </summary>
        MaterialboxinOven,
        /// <summary>
        /// 料条在料条轨道
        /// </summary>
        MaterialstripinMaterialstripTrack,
        /// <summary>
        /// 料条在焊台
        /// </summary>
        MaterialstripinWeld,
        /// <summary>
        /// 无料盒无料条
        /// </summary>
        NoMateralboxNoMateralstrip
    }
    [Serializable]
    public enum EnumProcessingState
    {
        INIT = 0,
        IDLE = 2,
        SETUP = 3,
        READY = 4,
        EXECUTING = 5,
        PAUSE = 6
    }
    [Serializable]
    public enum EnumProcessJobState
    {
        Created = 255,
        Queued = 0,
        SettingUp = 1,
        WaitingForStart = 2,
        Processing = 3,
        ProcessComplete = 4,
        Pausing = 6,
        Paused = 7,
        Stopping = 8,
        Aborting = 9,
        Stopped = 10,
        Aborted = 11
    }
    [Serializable]
    public enum EnumBondJobStatus
    {
        Idle = 0,
        Running = 1,
        UserAbort = 2,
        ProcessCompleted = 3,
        ProcessError = 4,
        TransferError = 5,
        TransferCompleted = 6,
        VisionFailed = 7,
        NeedManualLoadSubstrate,
        NeedManualFindComponent
    }
    /// <summary>
    /// 支持权重对象的随机对象接口
    /// </summary>
    public interface IRandomObject
    {
        /// <summary>
        /// 权重
        /// </summary>
        int Weight
        {
            set;
            get;
        }
    }
    [Serializable]
    public class MaterialMapInformation : ACloneable<MaterialMapInformation>, IComparable, IEqualityComparer<MaterialMapInformation>, IRandomObject
    {
        #region IRandomObject
        /// <summary>
        /// 权重
        /// </summary>
        [XmlAttribute("Weight")]
        public int Weight { set; get; }
        #endregion

        /// <summary>
        /// Wafer坐标系的坐标值
        /// </summary>
        public PointF MaterialLocation;
        /// <summary>
        /// 索引坐标。
        /// </summary>
        public Point MaterialCoordIndex;
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig PositionSubstrateMark1Result { get; set; }
        [XmlIgnore]
        public XYZTCoordinateConfig PositionSubstrateMark2Result { get; set; }
        [XmlIgnore]
        public XYZTCoordinateConfig PositionModuleResult { get; set; }
        [XmlIgnore]
        public bool IsPositionSuccess { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("MaterialNumber")]
        public int MaterialNumber;

        /// <summary>
        /// material是否存在
        /// </summary>
        [XmlAttribute("IsMaterialExist")]
        public bool IsMaterialExist;

        /// <summary>
        /// 是否是Mark
        /// </summary>
        [XmlAttribute("IsMark")]
        public bool IsMark;

        /// <summary>
        /// 属性
        /// </summary>
        [XmlAttribute("Properties")]
        public MaterialProperties Properties { get; set; }
        [XmlIgnore]
        MaterialProcessResult ProcessResult { get; set; }

        /// <summary>
        /// 是否需要检测
        /// </summary>
        [XmlAttribute("IsProcess")]
        public bool IsProcess;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public MaterialMapInformation()
        {
            IsMaterialExist = true;
            PositionSubstrateMark1Result = new XYZTCoordinateConfig();
            PositionSubstrateMark2Result = new XYZTCoordinateConfig();
            PositionModuleResult = new XYZTCoordinateConfig();
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="coordIndex"></param>
        /// <param name="location"></param>
        /// <param name="patternAngle"></param>
        /// <param name="toBePorcessed"></param>
        public MaterialMapInformation(int number, Point coordIndex, PointF location, double patternAngle, bool toBePorcessed)
        {
            IsMaterialExist = true;
            MaterialNumber = number;
            MaterialCoordIndex = coordIndex;
            MaterialLocation = location;
            PositionSubstrateMark1Result = new XYZTCoordinateConfig();
            PositionSubstrateMark2Result = new XYZTCoordinateConfig();
            PositionModuleResult = new XYZTCoordinateConfig();
        }

        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            try
            {
                int positive = 0;
                if (obj is MaterialMapInformation)
                {
                    MaterialMapInformation compareObject = obj as MaterialMapInformation;
                    if (this.MaterialCoordIndex.X < compareObject.MaterialCoordIndex.X)
                    {
                        positive = this.MaterialCoordIndex.Y <= compareObject.MaterialCoordIndex.Y ? -1 : 1;
                    }
                    else if (this.MaterialCoordIndex.X > compareObject.MaterialCoordIndex.X)
                    {
                        positive = this.MaterialCoordIndex.Y < compareObject.MaterialCoordIndex.Y ? -1 : 1;
                    }
                    else
                    {
                        positive = this.MaterialCoordIndex.Y.CompareTo(compareObject.MaterialCoordIndex.Y);
                    }
                }
                return -positive;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " <- " + ex.TargetSite.Name);
                return 0;
            }
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(MaterialMapInformation obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public bool Equals(MaterialMapInformation obj1, MaterialMapInformation obj2)
        {
            return obj1.MaterialCoordIndex.Equals(obj2.MaterialCoordIndex);
        }
    }
    /// <summary>
    /// 机台工艺检测状态
    /// </summary>
    [Serializable]
    public enum EnumProcessStatus
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError,
        /// <summary>
        /// 空闲状态
        /// </summary>
        Idle,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 用户停止
        /// </summary>
        Aborted,
        /// <summary>
        ///
        /// </summary>
        Completed,
        /// <summary>
        /// 暂停
        /// </summary>
        Paused,
        /// <summary>
        /// 
        /// </summary>
        WaferInspectError,
        /// <summary>
        /// Wafer传送异常
        /// </summary>
        WaferTransferError,
        /// <summary>
        /// 这里指一盒扫描完成，指一次循环跑片
        /// </summary>
        TransferCompleted
    }
    [Serializable]
    public enum EnumLightColor
    {
        Red,
        Green,
        Blue,
        White
    }
    [Serializable]
    public class ESToolSettings
    {
        public ESToolSettings()
        {
            NeedleCenter = new XYZTCoordinateConfig();
            BondIdentifyNeedleCenter = new XYZTCoordinateConfig();
        }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("NeedleName")]
        public string NeedleName { get; set; }
        [XmlElement("ESBasePosition")]
        public float ESBasePosition { get; set; }
        /// <summary>
        /// 针头刚好移动到表面时的绝对高度
        /// </summary>

        [XmlElement("NeedleZeorPosition")]
        public float NeedleZeorPosition { get; set; }
        /// <summary>
        /// 顶针XY和wafer视野中心的偏移（正方向向右向下）
        /// </summary>
        [XmlElement("NeedleCenter")]
        public XYZTCoordinateConfig NeedleCenter { get; set; }

        /// <summary>
        /// Bond相机对准顶针XY坐标 stge
        /// </summary>
        [XmlElement("BondIdentifyNeedleCenter")]
        public XYZTCoordinateConfig BondIdentifyNeedleCenter { get; set; }

        [XmlElement("ESZStagePosWhenMeasureHeight")]
        public float ESZStagePosWhenMeasureHeight { get; set; }
        [XmlElement("ESZToplateHigherValueThanMarkTopplateWhenMeasureESZHeight")]
        public float ESZToplateHigherValueThanMarkTopplateWhenMeasureESZHeight { get; set; }
        [XmlElement("LaserSensorValueWhenMeasureESZHeight")]
        public float LaserSensorValueWhenMeasureESZHeight { get; set; }

    }
    [Serializable]
    public class PPToolSettings
    {
        public PPToolSettings()
        {
            EnumPPtool = EnumPPtool.PPtool1;

            StageAxisTheta = EnumStageAxis.ChipPPT;
            StageAxisZ = EnumStageAxis.None;

            UplookingIdentifyPPtoolMatch = new MatchIdentificationParam();
            PPAndUCtoolOffset = new XYZTCoordinateConfig();

            LookupCameraOrigion = new XYZTCoordinateConfig();
            LookuptoPPOrigion = new XYZTCoordinateConfig();
            PP1AndBondCameraOffset = new XYZTCoordinateConfig();
            ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig();
            ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig();
            PPESAltimetryParameter = new PPESAltimetryParameters();

        }
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("EnumPPtool")]
        public EnumPPtool EnumPPtool { get; set; }

        [XmlElement("PPName")]
        public string PPName { get; set; }
        /// <summary>
        ///吸嘴接触Mark是的Z高度（吸嘴工具的系统原点）
        /// </summary>
        [XmlElement("AltimetryOnMark")]
        public float AltimetryOnMark { get; set; }

        /// <summary>
        ///吸嘴工具旋转轴
        /// </summary>
        [XmlElement("StageAxisTheta")]
        public EnumStageAxis StageAxisTheta { get; set; }

        /// <summary>
        ///吸嘴工具独立Z轴
        /// </summary>
        [XmlElement("StageAxisZ")]
        public EnumStageAxis StageAxisZ { get; set; }

        /// <summary>
        /// 吸嘴独立Z空闲位置
        /// </summary>
        [XmlElement("PPFreeZ")]
        public double PPFreeZ { get; set; }
        /// <summary>
        /// 吸嘴独立Z工作位置
        /// </summary>
        [XmlElement("PPWorkZ")]
        public double PPWorkZ { get; set; }

        [XmlElement("PPVaccumSwitch")]
        public EnumBoardcardDefineOutputIO PPVaccumSwitch { get; set; }

        [XmlElement("PPBlowSwitch")]
        public EnumBoardcardDefineOutputIO PPBlowSwitch { get; set; }

        [XmlElement("PPVaccumNormally")]
        public EnumBoardcardDefineInputIO PPVaccumNormally { get; set; }


        /// <summary>
        /// 仰视相机识别吸嘴 轮廓
        /// </summary>
        [XmlElement("UplookingIdentifyPPtoolMatch")]
        public MatchIdentificationParam UplookingIdentifyPPtoolMatch { get; set; }
        /// <summary>
        /// 吸嘴与uc工具的中心偏移
        /// </summary>
        [XmlElement("PPAndUCtoolOffset")]
        public XYZTCoordinateConfig PPAndUCtoolOffset { get; set; }

        /// <summary>
        /// 榜头相机对准仰视相机原点坐标
        /// </summary>
        [XmlElement("LookupCameraOrigion")]
        public XYZTCoordinateConfig LookupCameraOrigion { get; set; }

        /// <summary>
        /// 榜头在仰视相机中心的坐标
        /// </summary>
        [XmlElement("LookuptoPPOrigion")]
        public XYZTCoordinateConfig LookuptoPPOrigion { get; set; }

        /// <summary>
        /// 计算吸嘴旋转补偿XY公式的坐标1，一般在T0度纪录
        /// </summary>
        [XmlElement("PPosCompensateCoordinate1")]
        public XYZTCoordinateConfig ChipPPPosCompensateCoordinate1 { get; set; }
        /// <summary>
        /// 计算吸嘴旋转补偿XY公式的坐标2，一般在T180度纪录
        /// </summary>
        [XmlElement("PPosCompensateCoordinate2")]
        public XYZTCoordinateConfig ChipPPPosCompensateCoordinate2 { get; set; }

        /// <summary>
        /// 榜头与榜头相机中心的偏移
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig PP1AndBondCameraOffset
        {
            get
            {
                XYZTCoordinateConfig Offset = new XYZTCoordinateConfig();
                Offset.X = LookuptoPPOrigion.X - LookupCameraOrigion.X;
                Offset.Y = LookuptoPPOrigion.Y - LookupCameraOrigion.Y;
                return Offset;
            }
            set
            {

            }
        }

        /// <summary>
        /// 吸嘴和ES测高时的参数
        /// </summary>
        [XmlElement("PPESAltimetryParameter")]
        public PPESAltimetryParameters PPESAltimetryParameter { get; set; }
        
        [XmlElement("RotationTablePos4LoadPP")]
        public float RotationTablePos4LoadPP { get; set; }

    }
    [Serializable]
    public class PPESAltimetryParameters
    {
        public PPESAltimetryParameters()
        {
        }
        [XmlElement("PPName")]
        public string PPName { get; set; }

        [XmlElement("PPSystemPosition")]
        public float PPSystemPosition { get; set; }
        [XmlElement("ESStagePosition")]
        public float ESStagePosition { get; set; }
    }
    [Serializable]
    public class BaseCoordinateConfig
    {
        [XmlAttribute("X"), DataMember(Order = 1)]
        public double X { get; set; }

        [XmlAttribute("Y"), DataMember(Order = 2)]
        public double Y { get; set; }
    }

    [Serializable]
    public class XYThetaCoordinateConfig : BaseCoordinateConfig
    {
        [XmlAttribute("Theta")]
        public float Theta { get; set; }
    }

    [Serializable]
    public class XYZCoordinateConfig : BaseCoordinateConfig
    {
        [XmlAttribute("Z")]
        public double Z { get; set; }
    }

    [Serializable]
    public class XYZTCoordinateConfig : BaseCoordinateConfig
    {
        [XmlAttribute("Z")]
        public double Z { get; set; }

        [XmlAttribute("Theta")]
        public double Theta { get; set; }
    }

    [Serializable]
    public class XYZTOffsetConfig : BaseCoordinateConfig
    {
        [XmlAttribute("Z")]
        public double Z { get; set; }

        [XmlAttribute("Theta")]
        public double Theta { get; set; }
    }

    [Serializable]
    public class MatchIdentificationParam
    {
        public MatchIdentificationParam()
        {
            //Templatexml = "";
            //Runxml = "";
            TemplateRoi = new RectangleFV();
            SearchRoi = new RectangleFV();
            MaskSetting = new List<RecogniseMaskSetting>();
            BondTablePositionOfCreatePattern = new XYZTCoordinateConfig();
            PositionOfMaterialCenter = new XYZTCoordinateConfig();
            WaferTablePositionOfCreatePattern = new XYZTCoordinateConfig();
            PatternOffsetWithMaterialCenter = new XYZTCoordinateConfig();
            //Templateresult = new MatchTemplateResult();
        }

        [XmlElement("RingLightintensity")]
        public int RingLightintensity { get; set; }

        [XmlElement("DirectLightType")]
        public EnumDirectLightSourceType DirectLightType { get; set; }

        [XmlElement("DirectLightintensity")]
        public int DirectLightintensity { get; set; }

        [XmlElement("DirectRedLightintensity")]
        public int DirectRedLightintensity { get; set; }
        [XmlElement("DirectGreenLightintensity")]
        public int DirectGreenLightintensity { get; set; }
        [XmlElement("DirectBlueLightintensity")]
        public int DirectBlueLightintensity { get; set; }

        [XmlElement("Templatexml")]
        public string Templatexml { get; set; }
        [XmlElement("TemplateParamxml")]
        public string TemplateParamxml { get; set; }
        [XmlElement("Runxml")]
        public string Runxml { get; set; }

        [XmlElement("Score")]
        public float Score { get; set; }

        [XmlElement("MinAngle")]
        public int MinAngle { get; set; }

        [XmlElement("MaxAngle")]
        public int MaxAngle { get; set; }

        [XmlElement("TemplateRoi")]
        public RectangleFV TemplateRoi { get; set; }

        [XmlElement("MaskRoi")]
        public RectangleFV MaskRoi { get; set; }

        [XmlElement("SearchRoi")]
        public RectangleFV SearchRoi { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }


        [XmlElement("CameraZWorkPosition")]
        public float CameraZWorkPosition { get; set; }
        [XmlElement("CameraZChipSystemWorkPosition")]
        public float CameraZChipSystemWorkPosition { get; set; }
        [XmlElement("OrigionAngle")]
        public float OrigionAngle { get; set; }
        /// <summary>
        /// 创建模板时BondTable的位置
        /// </summary>
        [XmlElement("BondTablePositionOfCreatePattern")]
        public XYZTCoordinateConfig BondTablePositionOfCreatePattern { get; set; }
        /// <summary>
        /// 创建模板时物料中心的系统坐标位
        /// </summary>
        [XmlElement("PositionOfMaterialCenter")]
        public XYZTCoordinateConfig PositionOfMaterialCenter { get; set; }
        /// <summary>
        /// 创建Pattern时的坐标系和物料中心的偏移（系统坐标系）
        /// </summary>
        [XmlElement("PatternOffsetWithMaterialCenter")]
        public XYZTCoordinateConfig PatternOffsetWithMaterialCenter { get; set; }
        /// <summary>
        /// 创建模板时WaferTable的位置
        /// </summary>
        [XmlElement("WaferTablePositionOfCreatePattern")]
        public XYZTCoordinateConfig WaferTablePositionOfCreatePattern { get; set; }

        [XmlArray("MaskSetting"), XmlArrayItem(typeof(RecogniseMaskSetting))]
        public List<RecogniseMaskSetting> MaskSetting { get; set; }
        //[XmlElement("Templateresult")]
        //public MatchTemplateResult Templateresult { get; set; }
    }

    [Serializable]
    public class LineFindIdentificationParam
    {
        public LineFindIdentificationParam()
        {
            //UpEdgefilepath = "";
            //DownEdgefilepath = "";
            //LeftEdgefilepath = "";
            //RightEdgefilepath = "";
            UpEdgeRoi = new RectangleFV();
            DownEdgeRoi = new RectangleFV();
            LeftEdgeRoi = new RectangleFV();
            RightEdgeRoi = new RectangleFV();
            MaskSetting = new List<RecogniseMaskSetting>();
            BondTablePosition = new XYZTCoordinateConfig();
            WaferTablePosition = new XYZTCoordinateConfig();
        }
        [XmlElement("RingLightintensity")]
        public int RingLightintensity { get; set; }
        [XmlElement("DirectLightintensity")]
        public int DirectLightintensity { get; set; }

        [XmlElement("UpEdgefilepath")]
        public string UpEdgefilepath { get; set; }
        [XmlElement("DownEdgefilepath")]
        public string DownEdgefilepath { get; set; }
        [XmlElement("LeftEdgefilepath")]
        public string LeftEdgefilepath { get; set; }
        [XmlElement("RightEdgefilepath")]
        public string RightEdgefilepath { get; set; }


        [XmlElement("UpEdgeScore")]
        public int UpEdgeScore { get; set; }
        [XmlElement("DownEdgeScore")]
        public int DownEdgeScore { get; set; }
        [XmlElement("LeftEdgeScore")]
        public int LeftEdgeScore { get; set; }
        [XmlElement("RightEdgeScore")]
        public int RightEdgeScore { get; set; }


        [XmlElement("UpEdgeRoi")]
        public RectangleFV UpEdgeRoi { get; set; }
        [XmlElement("DownEdgeRoi")]
        public RectangleFV DownEdgeRoi { get; set; }
        [XmlElement("LeftEdgeRoi")]
        public RectangleFV LeftEdgeRoi { get; set; }

        [XmlElement("RightEdgeRoi")]
        public RectangleFV RightEdgeRoi { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        //[XmlElement("UsedCamera")]
        //public EnumCameraType UsedCamera { get; set; }
        [XmlElement("BondTablePosition")]
        public XYZTCoordinateConfig BondTablePosition { get; set; }
        [XmlElement("WaferTablePosition")]
        public XYZTCoordinateConfig WaferTablePosition { get; set; }
        [XmlElement("CameraZWorkPosition")]
        public float CameraZWorkPosition { get; set; }
        [XmlElement("CameraZChipSystemWorkPosition")]
        public float CameraZChipSystemWorkPosition { get; set; }
        [XmlElement("OrigionAngle")]
        public float OrigionAngle { get; set; }
        [XmlArray("MaskSetting"), XmlArrayItem(typeof(RecogniseMaskSetting))]
        //[XmlIgnore]
        public List<RecogniseMaskSetting> MaskSetting { get; set; }
    }

    [Serializable]
    public class CircleFindIdentificationParam
    {
        public CircleFindIdentificationParam()
        {
            //CircleFindTemplatefilepath = "";
            TemplateRoiCenter = new PointF();
            SearchRoi = new RectangleFV();
            MaskSetting = new List<RecogniseMaskSetting>();
            BondTablePosition = new XYZTCoordinateConfig();
            WaferTablePosition = new XYZTCoordinateConfig();
        }
        [XmlElement("RingLightintensity")]
        public int RingLightintensity { get; set; }
        [XmlElement("DirectLightintensity")]
        public int DirectLightintensity { get; set; }
        [XmlElement("CircleFindTemplatefilepath")]
        public string CircleFindTemplatefilepath { get; set; }

        [XmlElement("Score")]
        public int Score { get; set; }

        [XmlElement("MinR")]
        public int MinR { get; set; }

        [XmlElement("MaxR")]
        public int MaxR { get; set; }

        [XmlElement("TemplateRoiCenter")]
        public PointF TemplateRoiCenter { get; set; }
        [XmlElement("TemplateRoiR")]
        public float TemplateRoiR { get; set; }

        [XmlElement("SearchRoi")]
        public RectangleFV SearchRoi { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        //[XmlElement("UsedCamera")]
        //public EnumCameraType UsedCamera { get; set; }
        [XmlElement("BondTablePosition")]
        public XYZTCoordinateConfig BondTablePosition { get; set; }
        [XmlElement("WaferTablePosition")]
        public XYZTCoordinateConfig WaferTablePosition { get; set; }
        [XmlElement("CameraZWorkPosition")]
        public float CameraZWorkPosition { get; set; }
        [XmlElement("OrigionAngle")]
        public float OrigionAngle { get; set; }
        [XmlArray("MaskSetting"), XmlArrayItem(typeof(RecogniseMaskSetting))]
        public List<RecogniseMaskSetting> MaskSetting { get; set; }
    }
    [Serializable]
    public enum EnumBoardcardDefineOutputIO
    {
        #region 榜头

        ChipPPVaccumSwitch = 0,
        ChipPPBlowSwitch = 1,
        EpoxtliftCylinder = 2,

        

        

        #endregion

        #region 传送轨道

        TransportCylinder1 = 3,
        TransportCylinder2 = 4,
        TransportVaccumSwitch1 = 5,
        TransportVaccumSwitch2 = 6,



        
        

        #endregion

        #region 晶圆系统

        EjectionSystemVaccumSwitch = 7,
        EjectionSystemBlowSwitch = 8,
        WaferFingerCylinder = 9,
        WaferClampCylinder = 10,
        WaferCassetteCylinder = 11,
        
        StatisticWaffleVaccumSwitch = 12,

        TowerGreenLight = 13,

        #endregion

        #region 塔灯


        TowerYellowLight = 14,
        TowerRedLight = 15,
        //TowerRedLight = 16,


        #endregion

        #region 电机

        WaferCassetteLiftMotorBrake = 17,
        EjectionLiftMotorBrake = 18,

        #endregion

        #region 点胶

        EpoxtDIS = 19,
        EpoxtENABLE = 20,
        EpoxtTMD = 21,
        EpoxtTMS = 22,
        EpoxtC_CNT = 23,
        EpoxtRESET = 24,


        #endregion

        #region 其他

        SubmountPPVaccumSwitch,
        SubmountPPBlowSwitch,

        EutecticPlatformVaccumSwitch,

        NitrogenValve,
        StartEctectic,
        ResetEutectic,

        #endregion


    }
    [Serializable]
    public enum EnumBoardcardDefineInputIO
    {
        #region 榜头

        ChipPPVaccumNormally = 0,
        
        

        #endregion

        #region 传送轨道

        TransportInPlaceSignal1 = 3,
        TransportInPlaceSignal2 = 2,
        TransportInPlaceSignal3 = 1,


        

        #endregion

        #region 晶圆系统

        WaferInPlaceSignal1 = 4,


        #endregion


        #region 点胶

        EpoxtPON = 5,
        EpoxtDSO = 6,
        EpoxtEND = 7,
        EpoxtERROR = 8,
        EpoxtALARM = 9,
        EpoxtALARM2 = 10,
        EpoxtRSM = 11,
        EpoxtREADY = 12,


        #endregion

        #region 其他

        SubmountPPVaccumNormally = 13,
        EutecticError = 14,
        EutecticComplete,

        SafeDoorSensor1,
        SafeDoorSensor2,

        #endregion


    }

    [Serializable]
    public enum EnumProductStepType
    {
        [Description("无")]
        None,

        [Description("物料")]
        Material,
        Dispense,
        BondDie,
        BondDie2,

        [Description("共晶")]
        Eutectic
    }

    [Serializable]
    public enum EnumTransSrc
    {
        [Description("无")]
        None,

        [Description("物料")]
        Component,

        [Description("校准平台")]
        CalibrationTable,

        [Description("共晶平台")]
        EutecticTable
    }

    [Serializable]
    public enum EnumTransDest
    {
        [Description("无")]
        None,

        [Description("校准平台")]
        CalibrationTable,

        [Description("共晶平台")]
        EutecticTable,

        [Description("成品区")]
        FinishedProduct
    }


    [Serializable]
    public class PowerParam
    {
        [XmlAttribute("T1")]
        public int T1 { get; set; }
        [XmlAttribute("T2")]
        public int T2 { get; set; }
        [XmlAttribute("T3")]
        public int T3 { get; set; }
        [XmlAttribute("T4")]
        public int T4 { get; set; }
        [XmlAttribute("T5")]
        public int T5 { get; set; }
        [XmlAttribute("t1")]
        public int t1 { get; set; }
        [XmlAttribute("t2")]
        public int t2 { get; set; }
        [XmlAttribute("t3")]
        public int t3 { get; set; }
        [XmlAttribute("t4")]
        public int t4 { get; set; }
        [XmlAttribute("t5")]
        public int t5 { get; set; }
        [XmlAttribute("t6")]
        public int t6 { get; set; }

        [XmlAttribute("H1")]
        public int H1 { get; set; }
        [XmlAttribute("H2")]
        public int H2 { get; set; }
        [XmlAttribute("H3")]
        public int H3 { get; set; }
        [XmlAttribute("H4")]
        public int H4 { get; set; }

        [XmlAttribute("L1")]
        public int L1 { get; set; }
        [XmlAttribute("L2")]
        public int L2 { get; set; }
        [XmlAttribute("L3")]
        public int L3 { get; set; }
        [XmlAttribute("L4")]
        public int L4 { get; set; }



        [XmlAttribute("G1")]
        public int G1 { get; set; }
        [XmlAttribute("G2")]
        public int G2 { get; set; }
        [XmlAttribute("G3")]
        public int G3 { get; set; }
        [XmlAttribute("G4")]
        public int G4 { get; set; }



        [XmlAttribute("t0")]
        public int t0 { get; set; }

        [XmlAttribute("T0")]
        public int T0 { get; set; }



        [XmlAttribute("MH")]
        public int MH { get; set; }
        [XmlAttribute("ML")]
        public int ML { get; set; }

        [XmlAttribute("HD")]
        public int HD { get; set; }

        [XmlAttribute("CNTLLimit")]
        public int CNTLLimit { get; set; }

        [XmlAttribute("MP")]
        public int MP { get; set; }

        [XmlAttribute("TC")]
        public int TC { get; set; }

        [XmlAttribute("IOUT")]
        public int IOUT { get; set; }

        [XmlAttribute("OPMD")]
        public int OPMD { get; set; }

        [XmlAttribute("DM")]
        public int DM { get; set; }

        [XmlAttribute("GP")]
        public int GP { get; set; }

        [XmlAttribute("DN")]
        public int DN { get; set; }

        [XmlAttribute("ERRC")]
        public int ERRC { get; set; }


        [XmlAttribute("CNTH")]
        public int CNTH { get; set; }
        [XmlAttribute("CNTL")]
        public int CNTL { get; set; }

        [XmlAttribute("TM1")]
        public int TM1 { get; set; }
        [XmlAttribute("TM2")]
        public int TM2 { get; set; }
        [XmlAttribute("TM3")]
        public int TM3 { get; set; }
        [XmlAttribute("TM4")]
        public int TM4 { get; set; }
        [XmlAttribute("TMC")]
        public int TMC { get; set; }
        [XmlAttribute("DataTime")]
        public int DataTime { get; set; }
        [XmlAttribute("tcys")]
        public int tcys { get; set; }
        [XmlAttribute("IM1")]
        public int IM1 { get; set; }
        [XmlAttribute("IM2")]
        public int IM2 { get; set; }
        [XmlAttribute("IM3")]
        public int IM3 { get; set; }
        [XmlAttribute("IM4")]
        public int IM4 { get; set; }
        [XmlAttribute("UM1")]
        public int UM1 { get; set; }
        [XmlAttribute("UM2")]
        public int UM2 { get; set; }
        [XmlAttribute("UM3")]
        public int UM3 { get; set; }
        [XmlAttribute("UM4")]
        public int UM4 { get; set; }

        [XmlAttribute("TData1")]
        public int TData1 { get; set; }



    }

    [Serializable]
    public class RectangleFV
    {
        [XmlAttribute("X")]
        public float X { get; set; }
        [XmlAttribute("Y")]
        public float Y { get; set; }
        [XmlAttribute("Width")]
        public float Width { get; set; }
        [XmlAttribute("Height")]
        public float Height { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PPWorkParameters
    {
        /// <summary>
        /// 使用的吸嘴
        /// </summary>
        [XmlElement("UsedPP")]
        public EnumUsedPP UsedPP;

        /// <summary>
        /// 使用的吸嘴
        /// </summary>
        [XmlElement("PPtoolName")]
        public string PPtoolName;

        /// <summary>
        /// 工作高度，不包括压力偏移和其他偏移
        /// </summary>
        [XmlElement("WorkHeight")]
        public float WorkHeight { get; set; }

        /// <summary>
        /// 使用的吸嘴工具的
        /// </summary>
        [XmlElement("PPToolZero")]
        public float PPToolZero { get; set; }
        /// <summary>
        /// 压力偏移
        /// </summary>

        [XmlElement("PickupStress")]
        public float PickupStress { get; set; }
        [XmlElement("PlaceStress")]
        public float PlaceStress { get; set; }
        /// <summary>
        /// 是否需要顶针
        /// </summary>
        [XmlElement("IsUseNeedle")]
        public bool IsUseNeedle { get; set; }
        /// <summary>
        /// 顶针上升高度
        /// </summary>
        [XmlElement("NeedleUpHeight")]
        public float NeedleUpHeight { get; set; }
        /// <summary>
        /// 顶针上升速度
        /// </summary>
        [XmlElement("NeedleSpeed")]
        public float NeedleSpeed { get; set; }

        //[XmlElement("PickedDelayMS")]
        //public float DelayMSAfterPicked { get; set; }
        /// <summary>
        /// 开真空后等待的延时，延时过后才开始上升（即拾取延时）
        /// </summary>

        [XmlElement("DelayMSForVaccum")]
        public float DelayMSForVaccum { get; set; }
        /// <summary>
        /// 放置后的等待延时
        /// </summary>
        [XmlElement("DelayMSForPlace")]
        public float DelayMSForPlace { get; set; }
        /// <summary>
        /// 破空时长
        /// </summary>
        [XmlElement("BreakVaccumTimespanMS")]
        public float BreakVaccumTimespanMS { get; set; }
        
        /// <summary>
        /// 拾取前慢速移动的距离(也可用作放置时的慢速区间)
        /// </summary>

        [XmlElement("SlowTravelBeforePickupMM")]
        public float SlowTravelBeforePickupMM { get; set; }
        /// <summary>
        /// 拾取前慢速移动的速度
        /// </summary>

        [XmlElement("SlowSpeedBeforePickup")]
        public float SlowSpeedBeforePickup { get; set; }
        /// <summary>
        /// 拾取后慢速移动的距离
        /// </summary>

        [XmlElement("SlowTravelAfterPickupMM")]
        public float SlowTravelAfterPickupMM { get; set; }
        /// <summary>
        /// 拾取前慢速移动的速度
        /// </summary>

        [XmlElement("SlowSpeedAfterPickup")]
        public float SlowSpeedAfterPickup { get; set; }
        /// <summary>
        /// 拾取后上升的高度
        /// </summary>

        [XmlElement("UpDistanceMMAfterPicked")]
        public float UpDistanceMMAfterPicked { get; set; }
        public PPWorkParameters()
        {

        }
    }

    //生产步骤
    [Serializable]
    public class ProductStep
    {
        [XmlElement("StepName")]
        public string StepName { get; set; }

        /// <summary>
        /// 步骤类型
        /// </summary>
        [XmlElement("ProductStepType")]
        public EnumProductStepType productStepType { get; set; }

        /// <summary>
        /// 基板名称
        /// </summary>
        [XmlElement("SubstrateName")]
        public string SubstrateName { get; set; }

        /// <summary>
        /// 热沉名称
        /// </summary>
        [XmlElement("SubmonutName")]
        public string SubmonutName { get; set; }

        /// <summary>
        /// 芯片名称
        /// </summary>
        [XmlElement("ComponentName")]
        public string ComponentName { get; set; }

        /// <summary>
        /// 贴装位置名称
        /// </summary>
        [XmlElement("BondingPositionName")]
        public string BondingPositionName { get; set; }

        /// <summary>
        /// 共晶名称
        /// </summary>
        [XmlElement("EutecticName")]
        public string EutecticName { get; set; }

        /// <summary>
        /// 点胶工具名称
        /// </summary>
        [XmlElement("EpoxyApplicationName")]
        public string EpoxyApplicationName { get; set; }
    }

    //共晶配置
    [Serializable]
    public class EutecticConfig
    {
        [XmlElement("ConfigName")]
        public string ConfigName { get; set; }

        [XmlElement("GroupNo")]
        public string GroupNo { get; set; }
    }

    [Serializable]
    public enum EnumLoginResult
    {
        None = 0, Success = 1, UserNotExist = 2, PasswordWrong = 3
    }
    [Serializable]
    public class RightsInfo
    {
        public RightsInfo() { }

        public int RightsID { get; set; }
        public string RightsType { get; set; }
    }
    [Serializable]
    public class UserInfos
    {
        public UserInfos() { }

        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string usertype { get; set; }
        public string description { get; set; }
    }

    public class FunctionInfo
    {
        public FunctionInfo()
        {

        }

        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public int ParentID { get; set; }
    }
    /// <summary>
    /// 功能权限
    /// </summary>
    public class FunctionRightsInfo
    {
        public int FunctionInfoID { get; set; }
        public int RightsID { get; set; }
        public bool Visible { get; set; }
        public string FunctionName { get; set; }
    }

    [Serializable]
    public enum EnumJobRunStatus
    {
        None = 0, 
        SubstrateIncoming = 1,
        LoadingSubstrate = 2,
        LoadSubstrateSuccess = 3,
        LoadSubstrateFailed = 4,
        PositioningSubstrate = 5,
        PositionSubstrateSuccess = 6, 
        PositionSubstrateFailed = 7,
        SearchingBondPosition = 8,
        SearchBondPositionSuccess = 9,
        SearchBondPositionFailed = 10,
        DispensingGlue = 11,
        DispenseGlueSuccess = 12,
        DispenseGlueFailed = 13,
        PositioningComponent = 14,
        PositionComponentSuccess = 15,
        PositionComponentFailed = 16,
        PickingupComponent = 17,
        PickComponentSuccess = 18,
        PickComponentFailed = 19,
        AccuracyComponent = 20,
        AccuracyComponentSuccess = 21,
        AccuracyComponentFailed = 22,
        Bonding=23,
        BondSuccess=24,
        BondFailed=25,
        AbandonComponent = 26,
        AbandonComponentSuccess = 27,
        AbandonComponentFailed = 28,
        UnloadingSubstrate = 29,
        UnloadSubstrateSuccess = 30,
        UnloadSubstrateFailed = 31
    }
    [Serializable]
    public class MaterialWaferInfo
    {
        public string WaferID { get; set; }
        public string SlotIndex { get; set; }
        public string MaterialBoxID { get; set; }
        public EnumLoadPort AtLoadPort { get; set; }
        public EnumWaferSize WaferSize { get; set; }

    }
    /// <summary>
    /// 上料口LoadPort
    /// </summary>
    public enum EnumLoadPort
    {
        LoadPort1, LoadPort2, LoadPort3
    }
    /// <summary>
    /// Wafer的尺寸类型。
    /// </summary>
    [Serializable]
    public enum EnumWaferSize { INVALID = 0, INCH8 = 8, INCH12 = 12 }

    public enum EnumWaferType
    {
        Closed = -1,
        None = 0,
        Normal = 1,
        Thickness = 2,
        Slotted = 3,
        Bow = 4,
        Multiple = 7,
        Others = 9
    }
    public enum EnumPreDisoensingMode
    {
        Off = 1,
        BeforeDispenseCycle = 2,
        BeforeFirstDispensing = 3
    }
    [Serializable]
    public enum EnumPredispensingMode
    {
        Off,
        BeforeDispenseCycle,
        Before1stDispensingCycyleonTS
    }

    [Serializable]
    public enum EnumDispensingMode
    {
        /// <summary>
        /// 点胶
        /// </summary>
        Dispensing,
        /// <summary>
        /// 画胶
        /// </summary>
        GluePatternDispensing,
        /// <summary>
        /// 压胶
        /// </summary>
        AdhesivePressing,
        /// <summary>
        /// 蘸胶
        /// </summary>
        Dipping

    }
    [Serializable]
    public enum EnumEpoxyExudationMode
    {
        SpeedConstant,
        VolumeConstant
    }
    /// <summary>
    /// Lid的属性，检测Lid，边缘Lid，非检测Lid(Shell同样适用)
    /// </summary>
    [Serializable]
    public enum MaterialProperties { Testable, Edge, NonTestable }
    /// <summary>
    /// Lid的属性，检测Lid，边缘Lid，非检测Lid(Shell同样适用)
    /// </summary>
    [Serializable]
    public enum MaterialProcessResult { None = 0, NotProcess = 1, RecognisePass = 2, LSSuccess = 4, SSSuccess = 5, PointWeldFinished = 3, Fail = 8, Abandoned = 7 }
    /// <summary>
    /// 自动聚焦的方法
    /// </summary>
    [Serializable]
    public enum EnumAutoFocusMethod { Normal, Fast, CurveFit, Maximum }
    /// <summary>
    /// 自动聚焦的方法
    /// </summary>
    [Serializable]
    public enum EnumImageSharpnessMode { MeanStdDev, Laplance, Sobel }
    /// <summary>
    /// 物料参数
    /// </summary>

    [Serializable]
    /// <summary>
    /// 物料类型
    /// </summary>
    public enum EnumMaterialType
    {
        [Description("基底")]
        Substrate,

        [Description("芯片")]
        Chip
    }

    //取料方式
    public enum EnumPickupOrder
    {
        [Description("左起蛇形")]
        LeftSnakeShape,
        [Description("右起蛇形")]
        RightSnakeShape
    }

    public enum EnumBlankType
    {
        [Description("放置料区")]
        FinishPos,
        [Description("放回基底盘")]
        SubstratePos
    }

    [Serializable]
    public class ProgramComponentSettings
    {
        public ProgramComponentSettings()
        {
            MarkPoint = new XYZTCoordinateConfig();
            PositionComponentVisionParameters = new VisionParameters();
            AccuracyComponentPositionVisionParameters = new VisionParameters();

            FirstComponentLocation = new XYZTCoordinateConfig();
            FirstSubmountLocation = new XYZTCoordinateConfig();
            ComponentMapInfos = new List<MaterialMapInformation>();
            SubmountMapInfos = new List<MaterialMapInformation>();

            PPSettings = new PPWorkParameters();
            PPSettingsForBlanking = new PPWorkParameters();

            WaferEdgePos1 = new XYZTCoordinateConfig();
            WaferEdgePos2 = new XYZTCoordinateConfig();
            WaferEdgePos3 = new XYZTCoordinateConfig();

            ProcessWafers = new List<MaterialWaferInfo>();
        }
        #region 
        [XmlElement("IsMaterialInfoSettingsComplete")]
        public bool IsMaterialInfoSettingsComplete { get; set; }
        [XmlElement("IsMaterialPositionSettingsComplete")]
        public bool IsMaterialPositionSettingsComplete { get; set; }
        [XmlElement("IsMaterialPPSettingsComplete")]
        public bool IsMaterialPPSettingsComplete { get; set; }
        [XmlElement("IsMaterialMapSettingsComplete")]
        public bool IsMaterialMapSettingsComplete { get; set; }
        [XmlElement("IsMaterialAccuracySettingsComplete")]
        public bool IsMaterialAccuracySettingsComplete { get; set; }
        [XmlElement("MarkPoint")]
        public XYZTCoordinateConfig MarkPoint { get; set; }
        //按系统坐标系记录
        [XmlElement("FirstComponentLocation")]
        //按系统坐标系记录
        public XYZTCoordinateConfig FirstComponentLocation { get; set; }
        [XmlElement("FirstSubmountLocation")]
        public XYZTCoordinateConfig FirstSubmountLocation { get; set; }

        [XmlElement("Name")]
        //物料名称
        public string Name { get; set; }
        [XmlElement("MaterialType")]
        //物料类型
        public EnumMaterialType MaterialType { get; set; }

        [XmlElement("WidthMM")]
        //物料横宽
        public float WidthMM { get; set; }

        [XmlElement("HeightMM")]
        //物料竖高
        public float HeightMM { get; set; }

        [XmlElement("PitchColumnMM")]
        //列间距
        public float PitchColumnMM { get; set; }

        [XmlElement("PitchRowMM")]
        //行间距
        public float PitchRowMM { get; set; }

        [XmlElement("ThicknessMM")]
        //物料厚度
        public float ThicknessMM { get; set; }
        [XmlElement("RowCount")]
        //行数
        public int RowCount { get; set; }
        [XmlElement("ColumnCount")]
        //列数
        public int ColumnCount { get; set; }
        //[XmlElement("OrigionAngle")]
        ////初始角度
        //public float OrigionAngle { get; set; }

        [XmlElement("CarrierType")]
        //容器类型
        public EnumCarrierType CarrierType { get; set; }

        [XmlElement("CarrierThicknessMM")]
        //容器厚度
        public float CarrierThicknessMM { get; set; }

        [XmlElement("RelatedESToolName")]
        public string RelatedESToolName { get; set; }
        [XmlElement("RelatedPPToolName")]
        public string RelatedPPToolName { get; set; }
        [XmlElement("RelatedMaterialBoxToolName")]
        public string RelatedMaterialBoxToolName { get; set; }
        [XmlArray("ProcessWafers"), XmlArrayItem(typeof(MaterialWaferInfo))]
        public List<MaterialWaferInfo> ProcessWafers { get; set; }
        /// <summary>
        /// Chip吸嘴吸取时bondz的高度
        /// </summary>
        [XmlElement("ChipPPPickPos")]
        public float ChipPPPickPos { get; set; }
        /// <summary>
        /// Chip吸嘴吸取时bondz的高度偏差（此处是基于吸嘴工具校准的偏差，实际工作高度为吸嘴工具校准值减去此偏差）
        /// </summary>
        [XmlElement("ChipPPPickPosOffset")]
        public float ChipPPPickPosOffset { get; set; }
        /// <summary>
        /// Chip吸嘴吸取时bondz的系统坐标系高度（系统坐标系的原点为吸嘴在Mark上校准的高度）
        /// </summary>
        [XmlElement("ChipPPPickSystemPos")]
        public float ChipPPPickSystemPos { get; set; }
        /// <summary>
        /// Chip吸嘴放置时bondz的高度
        /// </summary>
        [XmlElement("ChipPPPlacePos")]
        public float ChipPPPlacePos { get; set; }

        /// <summary>
        /// 顶针座工作高度
        /// </summary>
        [XmlElement("ESBaseWorkPos")]
        public float ESBaseWorkPos { get; set; }

        [XmlElement("PPSettings")]
        public PPWorkParameters PPSettings { get; set; }
        [XmlElement("PPSettingsForBlanking")]
        public PPWorkParameters PPSettingsForBlanking { get; set; }
        /// <summary>
        /// Submount吸嘴吸取时bondz的高度
        /// </summary>
        [XmlElement("SubmountPPPickPos")]
        public float SubmountPPPickPos { get; set; }

        /// <summary>
        /// Submount吸嘴吸取时bondz的高度偏差（此处是基于吸嘴工具校准的偏差，实际工作高度为吸嘴工具校准值减去此偏差）
        /// </summary>
        [XmlElement("SubstrateMeasureZPosOffset")]
        public float SubstrateMeasureZPosOffset { get; set; }
        [XmlElement("SubstrateMeasureZSystemPos")]
        public float SubstrateMeasureZSystemPos { get; set; }

        [XmlElement("WaferEdgePos1")]
        public XYZTCoordinateConfig WaferEdgePos1 { get; set; }

        [XmlElement("WaferEdgePos2")]
        public XYZTCoordinateConfig WaferEdgePos2 { get; set; }

        [XmlElement("WaferEdgePos3")]
        public XYZTCoordinateConfig WaferEdgePos3 { get; set; }

        [XmlElement("WaferThickness")]
        //蓝膜厚度
        public float WaferThickness { get; set; }

        /// <summary>
        /// 视觉定位芯片时的参数
        /// </summary>
        [XmlElement("PositionComponentVisionParameters")]
        //[XmlArray("PositionComponentVisionParameters"), XmlArrayItem(typeof(VisionParameters))]
        public VisionParameters PositionComponentVisionParameters { get; set; }
        /// <summary>
        /// 芯片二次定位的视觉参数
        /// </summary>
        [XmlElement("AccuracyComponentPositionVisionParameters")]
        public VisionParameters AccuracyComponentPositionVisionParameters { get; set; }




        //Bond头相机工作高度
        //wafer相机工作高度
        //仰视相机工作高度
        //芯片吸嘴工作高度
        //substrate吸嘴工作高度
        //顶针座高度
        //顶针高度
        #endregion

        [XmlIgnore]
        public List<MaterialMapInformation> ComponentMapInfos { get; set; }
        [XmlIgnore]
        public List<MaterialMapInformation> SubmountMapInfos { get; set; }



    }
    /// <summary>
    /// 编程中的基板基本设置
    /// </summary>
    [Serializable]
    public class ProgramSubstrateSettings
    {
        public ProgramSubstrateSettings()
        {
            MarkPoint = new XYZTCoordinateConfig();
            PositionSustrateVisionParameters = new VisionParameters();
            PositionSustrateVisionParameters.VisionPositionUsedCamera = EnumCameraType.BondCamera;
            PositionModuleVisionParameters = new VisionParameters();
            PositionModuleVisionParameters.VisionPositionUsedCamera = EnumCameraType.BondCamera;
            FirstSubstrateHomeSystemLocation = new XYZTCoordinateConfig();
            SubstrateMapInfos = new List<MaterialMapInformation>();
            //ModuleMapInfos = new List<List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>>>();
            FirstModuleHomeSystemLocation = new XYZTCoordinateConfig();
            ModuleMapInfos = new List<List<MaterialMapInformation>>();
            PositionSustrateMarkVisionParameters = new List<VisionParameters>();
            SubstrateCoordinateHomePoint = new XYZTCoordinateConfig();
            SubstrateCoordinateHomeSecondPoint = new XYZTCoordinateConfig();
            BondingPositionInfos = new List<List<List<BondingPositionSettings>>>();
            ModuleMapInfosWithBondPositionInfos = new List<List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>>>();
            FirstSubstrateCenterSystemLocation = new XYZTCoordinateConfig();
            FirstModuleCenterSystemLocation = new XYZTCoordinateConfig();
        }
        #region 
        [XmlElement("IsMultiSubstrates")]
        public bool IsMultiSubstrates { get; set; }
        [XmlElement("IsMultiModules")]
        public bool IsMultiModules { get; set; }
        [XmlElement("IsPositionModules")]
        public bool IsPositionModules { get; set; }
        [XmlElement("PositionModulePointCount")]
        public int PositionModulePointCount { get; set; }

        [XmlElement("PositionSubstratePointCount")]
        public int PositionSubstratePointCount { get; set; }


        [XmlElement("IsMaterialInfoSettingsComplete")]
        public bool IsMaterialInfoSettingsComplete { get; set; }
        [XmlElement("IsMaterialPositionSettingsComplete")]
        public bool IsMaterialPositionSettingsComplete { get; set; }
        [XmlElement("IsMaterialPPSettingsComplete")]
        public bool IsMaterialPPSettingsComplete { get; set; }
        [XmlElement("IsMaterialMapSettingsComplete")]
        public bool IsMaterialMapSettingsComplete { get; set; }
        [XmlElement("IsModulePositionSettingsComplete")]
        public bool IsModulePositionSettingsComplete { get; set; }
        [XmlElement("IsModuleMapSettingsComplete")]
        public bool IsModuleMapSettingsComplete { get; set; }
        [XmlElement("IsMaterialAccuracySettingsComplete")]
        public bool IsMaterialAccuracySettingsComplete { get; set; }
        [XmlElement("MarkPoint")]
        public XYZTCoordinateConfig MarkPoint { get; set; }

        [XmlElement("FirstSubstrateHomeSystemLocation")]
        public XYZTCoordinateConfig FirstSubstrateHomeSystemLocation { get; set; }

        [XmlElement("FirstModuleHomeSystemLocation")]
        public XYZTCoordinateConfig FirstModuleHomeSystemLocation { get; set; }
        [XmlElement("FirstSubstrateCenterSystemLocation")]
        public XYZTCoordinateConfig FirstSubstrateCenterSystemLocation { get; set; }
        [XmlElement("FirstModuleCenterSystemLocation")]
        public XYZTCoordinateConfig FirstModuleCenterSystemLocation { get; set; }

        [XmlElement("SubstrateCoordinateHomePoint")]
        public XYZTCoordinateConfig SubstrateCoordinateHomePoint { get; set; }
        [XmlElement("SubstrateCoordinateHomeSecondPoint")]
        public XYZTCoordinateConfig SubstrateCoordinateHomeSecondPoint { get; set; }

        [XmlElement("Name")]
        //物料名称
        public string Name { get; set; }
        [XmlElement("MaterialType")]
        //物料类型
        public EnumMaterialType MaterialType { get; set; }

        ///// <summary>
        ///// 模块对位用到的参数
        ///// </summary>

        //[XmlElement("ModuleAlignmentConfig")]
        //public VisionParameters ModuleAlignmentConfig { get; set; }

        [XmlElement("WidthMM")]
        //物料横宽
        public float WidthMM { get; set; }

        [XmlElement("HeightMM")]
        //物料竖高
        public float HeightMM { get; set; }

        [XmlElement("PitchColumnMM")]
        //列间距
        public float PitchColumnMM { get; set; }

        [XmlElement("PitchRowMM")]
        //行间距
        public float PitchRowMM { get; set; }

        [XmlElement("ThicknessMM")]
        //物料厚度
        public float ThicknessMM { get; set; }
        [XmlElement("RowCount")]
        //行数
        public int RowCount { get; set; }
        [XmlElement("ColumnCount")]
        //列数
        public int ColumnCount { get; set; }

        [XmlElement("ModuleWidthMM")]
        //物料横宽
        public float ModuleWidthMM { get; set; }

        [XmlElement("ModuleHeightMM")]
        //物料竖高
        public float ModuleHeightMM { get; set; }

        [XmlElement("ModulePitchColumnMM")]
        //列间距
        public float ModulePitchColumnMM { get; set; }

        [XmlElement("ModulePitchRowMM")]
        //行间距
        public float ModulePitchRowMM { get; set; }
        [XmlElement("ModuleRowCount")]
        //行数
        public int ModuleRowCount { get; set; }
        [XmlElement("ModuleColumnCount")]
        //列数
        public int ModuleColumnCount { get; set; }
        //[XmlElement("OrigionAngle")]
        ////初始角度
        //public float OrigionAngle { get; set; }

        [XmlElement("CarrierType")]
        //容器类型
        public EnumCarrierType CarrierType { get; set; }

        [XmlElement("CarrierThicknessMM")]
        //容器厚度
        public float CarrierThicknessMM { get; set; }

        [XmlElement("RelatedESToolName")]
        public string RelatedESToolName { get; set; }
        [XmlElement("RelatedPPToolName")]
        public string RelatedPPToolName { get; set; }

        [XmlElement("WaferThickness")]
        //蓝膜厚度
        public float WaferThickness { get; set; }

        /// <summary>
        /// 衬底视觉定位时的参数
        /// </summary>
        [XmlElement("PositionSustrateVisionParameters")]
        //[XmlArray("PositionSustrateVisionParameters"), XmlArrayItem(typeof(VisionParameters))]
        public VisionParameters PositionSustrateVisionParameters { get; set; }
        [XmlArray("PositionSustrateMarkVisionParameters"), XmlArrayItem(typeof(VisionParameters))]
        public List<VisionParameters> PositionSustrateMarkVisionParameters { get; set; }
        /// <summary>
        /// 衬底视觉定位时的参数
        /// </summary>
        [XmlElement("PositionModuleVisionParameters")]
        //[XmlArray("PositionSustrateVisionParameters"), XmlArrayItem(typeof(VisionParameters))]
        public VisionParameters PositionModuleVisionParameters { get; set; }

        [XmlElement("SubstrateMeasureZPosOffset")]
        public float SubstrateMeasureZPosOffset { get; set; }
        [XmlElement("SubstrateTopZSystemPos")]
        public float SubstrateTopZSystemPos { get; set; }
        [XmlElement("ModuleTopZSystemPos")]
        public float ModuleTopZSystemPos { get; set; }



        //Bond头相机工作高度
        //wafer相机工作高度
        //仰视相机工作高度
        //芯片吸嘴工作高度
        //substrate吸嘴工作高度
        //顶针座高度
        //顶针高度
        #endregion

        [XmlIgnore]
        public List<List<MaterialMapInformation>> ModuleMapInfos { get; set; }
        //public List<List<Tuple<MaterialMapInformation,List<BondingPositionSettings>>>> ModuleMapInfos { get; set; }
        [XmlIgnore]
        public List<List<Tuple<MaterialMapInformation, List<BondingPositionSettings>>>> ModuleMapInfosWithBondPositionInfos { get; set; }
        [XmlIgnore]
        public List<MaterialMapInformation> SubstrateMapInfos { get; set; }
        [XmlIgnore]
        public List<List<List<BondingPositionSettings>>> BondingPositionInfos { get; set; }


    }
    /// <summary>
    /// 下料时的参数,下料的吸嘴复用衬底的吸嘴，只是高度不同
    /// </summary>
    [Serializable]
    public class BlankingParameters
    {
        [XmlElement("IsCompleted")]
        public bool IsCompleted { get; set; }
        [XmlElement("BlankingMethod")]
        public EnumBlankingMethod BlankingMethod { get; set; }

        /// <summary>
        /// XY为下料时拾取位置和物料中心的偏移，Z为拾取高度
        /// </summary>
        [XmlElement("PickPosition")]
        public XYZTCoordinateConfig PickPosition { get; set; }

        [XmlElement("PlacePositionForFirstSumbount")]
        public XYZTCoordinateConfig PlacePositionForFirstSumbount { get; set; }
        public BlankingParameters()
        {
            PickPosition = new XYZTCoordinateConfig();
            PlacePositionForFirstSumbount = new XYZTCoordinateConfig();
        }
    }
    [Serializable]
    public class AlignmentConfig
    {
        public AlignmentConfig()
        {
            AlignmentPosition = new XYZTCoordinateConfig();
            AlignmentVisionParameters = new VisionParameters();
        }
        [XmlElement("AlignmentPosition")]
        public XYZTCoordinateConfig AlignmentPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>

        [XmlElement("AlignmentVisionParameters")]
        public VisionParameters AlignmentVisionParameters { get; set; }
    }

    /// <summary>
    /// 视觉参数类
    /// </summary>
    [Serializable]
    public class VisionParameters
    {
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }
        [XmlElement("IsCompleted")]
        public bool IsCompleted { get; set; }
        [XmlElement("VisionPositionUsedCamera")]
        public EnumCameraType VisionPositionUsedCamera { get; set; }

        [XmlElement("VisionPositionMethod")]
        public EnumVisionPositioningMethod VisionPositionMethod { get; set; }
        [XmlElement("AccuracyMethod")]
        public EnumAccuracyMethod AccuracyMethod { get; set; }
        [XmlElement("AccuracyVisionPositionMethod")]
        public EnumVisionPositioningMethod AccuracyVisionPositionMethod { get; set; }



        [XmlArray("ShapeMatchConfigs"), XmlArrayItem(typeof(MatchIdentificationParam))]
        public List<MatchIdentificationParam> ShapeMatchParameters { get; set; }
        [XmlArray("CircleSearchConfigs"), XmlArrayItem(typeof(CircleFindIdentificationParam))]
        public List<CircleFindIdentificationParam> CircleSearchParameters { get; set; }
        [XmlArray("EdgeSearchConfigs"), XmlArrayItem(typeof(LineFindIdentificationParam))]
        public List<LineFindIdentificationParam> LineSearchParams { get; set; }
        public VisionParameters()
        {
            ShapeMatchParameters = new List<MatchIdentificationParam>();
            CircleSearchParameters = new List<CircleFindIdentificationParam>();
            LineSearchParams = new List<LineFindIdentificationParam>();
        }
    }
    [Serializable]
    public class BondingPositionSettings
    {
        public BondingPositionSettings()
        {
            BondPositionWithPatternOffset = new XYZTCoordinateConfig();
            BondPositionWithMaterialCenterOffset = new XYZTCoordinateConfig();
            VisionParametersForFindBondPosition = new VisionParameters();
            BondPositionCompensation = new XYZTCoordinateConfig();
            DispenserPositionCompensation = new XYZTCoordinateConfig();
            chipPositionCompensation = new XYZTCoordinateConfig();
            PositionBondChipResult = new XYZTCoordinateConfig();
            BondPositionSystemPosAfterVisionCalibration = new XYZTCoordinateConfig();
        }
        [XmlElement("IsComplete")]
        public bool IsComplete { get; set; }
        [XmlElement("FindBondPositionMethod")]
        public EnumFindBondPositionMethod FindBondPositionMethod { get; set; }
        /// <summary>
        /// 贴片前的视觉参数
        /// </summary>

        [XmlElement("VisionParametersForFindBondPosition")]
        public VisionParameters VisionParametersForFindBondPosition { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("PPWorkHeight")]
        public float PPWorkHeight { get; set; }
        [XmlElement("SystemHeight")]
        public float SystemHeight { get; set; }
        [XmlElement("MeasureEveryHeightWithLaser")]
        public bool MeasureEveryHeightWithLaser { get; set; }
        /// <summary>
        /// 贴装位置和识别特征的偏移，精准贴装模式下使用此参数
        /// </summary>
        [XmlElement("BondPositionWithPatternOffset")]
        public XYZTCoordinateConfig BondPositionWithPatternOffset { get; set; }
        /// <summary>
        /// 贴装位置和物料中心的偏移，非精准贴装模式下使用此参数
        /// </summary>
        [XmlElement("BondPositionWithMaterialCenterOffset")]
        public XYZTCoordinateConfig BondPositionWithMaterialCenterOffset { get; set; }
        /// <summary>
        /// 贴装位置补偿
        /// </summary>
        [XmlElement("BondPositionCompensation")]
        public XYZTCoordinateConfig BondPositionCompensation { get; set; }
        /// <summary>
        /// 点胶位置补偿
        /// </summary>
        [XmlElement("DispenserPositionCompensation")]
        public XYZTCoordinateConfig DispenserPositionCompensation { get; set; }
        /// <summary>
        /// 吸芯片位置补偿
        /// </summary>
        [XmlElement("chipPositionCompensation")]
        public XYZTCoordinateConfig chipPositionCompensation { get; set; }
        [XmlIgnore]
        public XYZTCoordinateConfig PositionBondChipResult { get; set; }
        /// <summary>
        /// 贴装位置校准后的系统坐标系位置
        /// </summary>
        [XmlIgnore]
        public XYZTCoordinateConfig BondPositionSystemPosAfterVisionCalibration { get; set; }
        [XmlIgnore]
        public bool IsPositionSuccess { get; set; }
    }

    [Serializable]
    public class EutecticParameters
    {
        [XmlElement("IsCompleted")]
        public bool IsCompleted { get; set; }
        [XmlElement("ParameterIndex")]
        public int ParameterIndex { get; set; }

        [XmlElement("EutecticPress")]
        public float EutecticPress { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("HeatSegmentParam")]
        public List<HeatSegmentParam> HeatSegmentParam { get; set; }
        public EutecticParameters()
        {
            HeatSegmentParam = new List<HeatSegmentParam>();
        }
    }

    [Serializable]
    public class DispenserSettings
    {
        [XmlElement("IsCompleted")]
        public bool IsCompleted { get; set; }
        [XmlElement("PredispensingMode")]
        public EnumPredispensingMode PredispensingMode { get; set; }

        [XmlElement("DispensingMode")]
        public EnumDispensingMode DispensingMode { get; set; }

        [XmlElement("PredispensingCount")]
        public int PredispensingCount { get; set; }
        [XmlElement("PredispensingIntervalMinute")]
        public int PredispensingIntervalMinute { get; set; }
        [XmlElement("PredispensingIntervalSecond")]
        public int PredispensingIntervalSecond { get; set; }
        [XmlElement("PredispensingOffsetXMM")]
        public float PredispensingOffsetXMM { get; set; }
        [XmlElement("PredispensingOffsetYMM")]
        public float PredispensingOffsetYMM { get; set; }

        [XmlElement("DispenserSystemPosXMM")]
        public float DispenserSystemPosXMM { get; set; }
        [XmlElement("DispenserSystemPosYMM")]
        public float DispenserSystemPosYMM { get; set; }

        [XmlElement("DispenserSystemPosZMM")]
        public float DispenserSystemPosZMM { get; set; }

        [XmlElement("DispenserPosOffsetXWithBondCamera")]
        public float DispenserPosOffsetXWithBondCamera { get; set; }
        [XmlElement("DispenserPosOffsetYWithBondCamera")]
        public float DispenserPosOffsetYWithBondCamera { get; set; }
        public DispenserSettings()
        {

        }
    }

    [Serializable]
    public class EpoxyApplication
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("IsCompleted")]
        public bool IsCompleted { get; set; }
        [XmlElement("EpoxyExudationMode")]
        public EnumEpoxyExudationMode EpoxyExudationMode { get; set; }
        [XmlElement("EpoxyExudationXMM")]
        public float EpoxyExudationXMM { get; set; }
        [XmlElement("EpoxyExudationSizeYMM")]
        public float EpoxyExudationSizeYMM { get; set; }

        [XmlElement("TearOffHeightMM")]
        public float TearOffHeightMM { get; set; }


        [XmlElement("TearOffSpeedMM")]
        public float TearOffSpeedMM { get; set; }

        [XmlElement("DispenserRecipeName")]
        public string DispenserRecipeName { get; set; }
        [XmlElement("DispensePattern")]
        public EnumDispensePattern DispensePattern { get; set; }
        [XmlElement("DispensePatternWidthMM")]
        public float DispensePatternWidthMM { get; set; }
        [XmlElement("DispensePatternHeightMM")]
        public float DispensePatternHeightMM { get; set; }
        public EpoxyApplication()
        {

        }
    }

    [Serializable]
    public class MaterialBoxToolSettings
    {
        public MaterialBoxToolSettings()
        {
        }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("FirstSlotPosition")]
        public float FirstSlotPosition { get; set; }

        [XmlElement("SlotCount")]
        public int SlotCount { get; set; }


    }
    [Serializable]
    public class DefineRecipeCounter
    {
        public DefineRecipeCounter()
        {
        }
        [XmlElement("RecipeName")]
        public string RecipeName { get; set; }

        [XmlElement("Count")]
        public int Count { get; set; }


    }
    public class DispenseRecipeInfo
    {
        public DispenseRecipeInfo()
        {

        }
        public float Pressure { get; set; }
        public float Vaccum { get; set; }
        public float ShotTimespan { get; set; }
    }
}
