using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalToolClsLib
{
    public class DataModel : INotifyPropertyChanged
    {
        private static readonly Lazy<DataModel> instance = new Lazy<DataModel>(() => new DataModel());
        public static DataModel Instance => instance.Value;
        private DataModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;


        #region 内部参数

        #region System


        private string sysdatetime;
        private int sysRuntime;

        private bool iolocken = true;

        private float bondY;
        private float bondX;
        private float bondZ;
        private float chipPPT;
        private float pPtoolBankTheta;
        private float dippingGlue;
        private float transportTrack1;
        private float transportTrack2;
        private float transportTrack3;
        private float waferTableY;
        private float waferTableX;
        private float waferTableZ;
        private float waferFilm;
        private float waferFinger;
        private float waferCassetteLift;
        private float eSZ;
        private float needleZ;
        private float needleSwitch;
        private float filpToolTheta;
        private float submountPPT;
        private float submountPPZ;

        private int bondYSta;
        private int bondXSta;
        private int bondZSta;
        private int chipPPTSta;
        private int pPtoolBankThetaSta;
        private int dippingGlueSta;
        private int transportTrack1Sta;
        private int transportTrack2Sta;
        private int transportTrack3Sta;
        private int waferTableYSta;
        private int waferTableXSta;
        private int waferTableZSta;
        private int waferFilmSta;
        private int waferFingerSta;
        private int waferCassetteLiftSta;
        private int eSZSta;
        private int needleZSta;
        private int needleSwitchSta;
        private int filpToolThetaSta;
        private int submountPPTSta;
        private int submountPPZSta;




        private int weldMaterialNumber = 0;
        private int pressWorkNumber = 0;
        private int equipmentOperatingTime = 0;
        private int thisequipmentOperatingTime = 0;

        private bool run = false;
        private bool error = false;

        private bool stageAxisIsconnect = true;
        private bool stageIOIsconnect = true;
        private bool cameraIsconnect = true;
        private bool temperatureIsconnect = true;
        private bool vacuumIsconnect = true;
        private bool turboMolecularPumpIsconnect = true;
        private bool lightsIsconnect = true;
        private bool laserIsconnect = true;
        private bool pressureIsconnect = true;

        private bool temperatureIsReading = false;
        private bool vacuumIsReading = false;
        private bool turboMolecularPumpIsReading = false;
        private bool temperatureIsWriting = false;
        private bool vacuumIsWriting = false;
        private bool turboMolecularPumpIsWriting = false;
        private bool lightsIsReading = false;
        private bool lightsIsWriting = false;

        private bool linkstatusofmodules = true;

        private string joblogText;


        #endregion

        #region OutputIO

        private bool chipPPVaccumSwitch = false;
        private bool chipPPBlowSwitch = false;
        private bool epoxtliftCylinder = false;
        private bool epoxtDIS = false;
        private bool epoxtENABLE = false;
        private bool epoxtTMD = false;
        private bool epoxtTMS = false;
        private bool epoxtC_CNT = false;
        private bool epoxtRESET = false;
        private bool submountPPVaccumSwitch = false;
        private bool submountPPBlowSwitch = false;

        private bool transportCylinder1 = false;
        private bool transportCylinder2 = false;
        private bool transportVaccumSwitch1 = false;
        private bool transportVaccumSwitch2 = false;
        private bool eutecticPlatformVaccumSwitch = false;
        private bool nitrogenValve = false;
        private bool startEctectic = false;
        private bool resetEutectic = false;

        private bool ejectionSystemVaccumSwitch = false;
        private bool ejectionSystemBlowSwitch = false;
        private bool waferFingerCylinder = false;
        private bool waferClampCylinder = false;
        private bool waferCassetteCylinder = false;
        private bool materialPlatformVaccumSwitch = false;
        private bool waferTableVaccumSwitch = false;
        

        private bool towerYellowLight = false;
        private bool towerGreenLight = false;
        private bool towerRedLight = false;

        private bool waferCassetteLiftMotorBrake = false;
        private bool ejectionLiftMotorBrake = false;




        #endregion

        #region InputIO


        private bool chipPPVaccumNormally = false;
        private bool submountPPVaccumNormally = false;
        private bool epoxtPON = false;
        private bool epoxtDSO = false;
        private bool epoxtEND = false;
        private bool epoxtERROR = false;
        private bool epoxtALARM = false;
        private bool epoxtEXE1 = false;
        private bool epoxtALARM2 = false;
        private bool epoxtRSM = false;
        private bool epoxtREADY = false;

        private bool transportInPlaceSignal1 = false;
        private bool transportInPlaceSignal2 = false;
        private bool transportInPlaceSignal3 = false;
        private bool eutecticError = false;
        private bool eutecticComplete = false;

        private bool waferInPlaceSignal1 = false;

        private bool safeDoorSensor1 = false;
        private bool safeDoorSensor2 = false;



        #endregion

        #region SerialPort

        private int bondRed;
        private int bondGreen;
        private int bondBlue;
        private int bondRing;
        private int waferRed;
        private int waferGreen;
        private int waferBlue;
        private int waferRing;
        private int upLookingRed;
        private int upLookingGreen;
        private int upLookingBlue;
        private int upLookingRing;
        private double laserValue;
        private double pressureValue1;
        private double pressureValue2;


        private int epoxtCH;
        private int epoxtMode;
        private double epoxtTime;
        private double epoxtPressure;
        private double epoxtVacuum;
        private int epoxtMode2;
        private int epoxtSHOT;
        private double epoxtCurrentPressure;
        private double epoxtCurrentTime;


        #endregion



        #endregion


        #region 全局参数


        #region 流程

        private bool startTranspot = false;
        /// <summary>
        /// 开始传片
        /// </summary>
        public bool StartTranspot
        {
            get { return startTranspot; }
            set
            {
                if (startTranspot != value)
                {
                    startTranspot = value;
                    OnPropertyChanged(nameof(StartTranspot));
                }
            }
        }

        private bool endTranspot = false;
        /// <summary>
        /// 开始退片
        /// </summary>
        public bool EndTranspot
        {
            get { return endTranspot; }
            set
            {
                if (endTranspot != value)
                {
                    endTranspot = value;
                    OnPropertyChanged(nameof(EndTranspot));
                }
            }
        }




        #endregion

        #region System


        /// <summary>
        /// 系统时间
        /// </summary>
        public string Sysdatetime
        {
            get { return sysdatetime; }
            set
            {
                if (sysdatetime != value)
                {
                    sysdatetime = value;
                    OnPropertyChanged(nameof(Sysdatetime));
                }
            }

        }

        /// <summary>
        /// 系统运行时间
        /// </summary>
        public int SysRuntime
        {
            get { return sysRuntime; }
            set
            {
                if (sysRuntime != value)
                {
                    sysRuntime = value;
                    OnPropertyChanged(nameof(SysRuntime));
                }
            }

        }


        /// <summary>
        /// IO互锁开关
        /// </summary>
        public bool IOlocken
        {
            get { return iolocken; }
            set
            {
                if (iolocken != value)
                {
                    iolocken = value;
                }
            }
        }

        /// <summary>
        /// Bond头Y轴
        /// </summary>
        public float BondY
        {
            get { return bondY; }
            set
            {
                if (bondY != value)
                {
                    bondY = value;
                    OnPropertyChanged(nameof(BondY));
                }
            }
        }

        /// <summary>
        /// Bond头X轴
        /// </summary>
        public float BondX
        {
            get { return bondX; }
            set
            {
                if (bondX != value)
                {
                    bondX = value;
                    OnPropertyChanged(nameof(BondX));
                }
            }
        }

        /// <summary>
        /// Bond头Z轴
        /// </summary>
        public float BondZ
        {
            get { return bondZ; }
            set
            {
                if (bondZ != value)
                {
                    bondZ = value;
                    OnPropertyChanged(nameof(BondZ));
                }
            }
        }

        /// <summary>
        /// 芯片吸嘴Theta轴
        /// </summary>
        public float ChipPPT
        {
            get { return chipPPT; }
            set
            {
                if (chipPPT != value)
                {
                    chipPPT = value;
                    OnPropertyChanged(nameof(ChipPPT));
                }
            }
        }
        /// <summary>
        /// 吸嘴库Theta轴
        /// </summary>
        public float PPtoolBankTheta
        {
            get { return pPtoolBankTheta; }
            set
            {
                if (pPtoolBankTheta != value)
                {
                    pPtoolBankTheta = value;
                    OnPropertyChanged(nameof(PPtoolBankTheta));
                }
            }
        }
        /// <summary>
        /// 蘸胶轴
        /// </summary>
        public float DippingGlue
        {
            get { return dippingGlue; }
            set
            {
                if (dippingGlue != value)
                {
                    dippingGlue = value;
                    OnPropertyChanged(nameof(DippingGlue));
                }
            }
        }
        /// <summary>
        /// 传送轨道1轴
        /// </summary>
        public float TransportTrack1
        {
            get { return transportTrack1; }
            set
            {
                if (transportTrack1 != value)
                {
                    transportTrack1 = value;
                    OnPropertyChanged(nameof(TransportTrack1));
                }
            }
        }
        /// <summary>
        /// 传送轨道2轴
        /// </summary>
        public float TransportTrack2
        {
            get { return transportTrack2; }
            set
            {
                if (transportTrack2 != value)
                {
                    transportTrack2 = value;
                    OnPropertyChanged(nameof(TransportTrack2));
                }
            }
        }
        /// <summary>
        /// 传送轨道3轴
        /// </summary>
        public float TransportTrack3
        {
            get { return transportTrack3; }
            set
            {
                if (transportTrack3 != value)
                {
                    transportTrack3 = value;
                    OnPropertyChanged(nameof(TransportTrack3));
                }
            }
        }


        /// <summary>
        /// 晶圆盘Y轴
        /// </summary>
        public float WaferTableY
        {
            get { return waferTableY; }
            set
            {
                if (waferTableY != value)
                {
                    waferTableY = value;
                    OnPropertyChanged(nameof(WaferTableY));
                }
            }
        }
        /// <summary>
        /// 晶圆盘X轴
        /// </summary>
        public float WaferTableX
        {
            get { return waferTableX; }
            set
            {
                if (waferTableX != value)
                {
                    waferTableX = value;
                    OnPropertyChanged(nameof(WaferTableX));
                }
            }
        }

        /// <summary>
        /// 晶圆相机Z
        /// </summary>
        public float WaferTableZ
        {
            get { return waferTableZ; }
            set
            {
                if (waferTableZ != value)
                {
                    waferTableZ = value;
                    OnPropertyChanged(nameof(WaferTableZ));
                }
            }
        }
        /// <summary>
        /// 晶圆阔膜轴
        /// </summary>
        public float WaferFilm
        {
            get { return waferFilm; }
            set
            {
                if (waferFilm != value)
                {
                    waferFilm = value;
                    OnPropertyChanged(nameof(WaferFilm));
                }
            }
        }

        /// <summary>
        /// 晶圆夹爪轴
        /// </summary>
        public float WaferFinger
        {
            get { return waferFinger; }
            set
            {
                if (waferFinger != value)
                {
                    waferFinger = value;
                    OnPropertyChanged(nameof(WaferFinger));
                }
            }
        }

        /// <summary>
        /// 晶圆抽匣升降轴
        /// </summary>
        public float WaferCassetteLift
        {
            get { return waferCassetteLift; }
            set
            {
                if (waferCassetteLift != value)
                {
                    waferCassetteLift = value;
                    OnPropertyChanged(nameof(WaferCassetteLift));
                }
            }
        }


        /// <summary>
        /// 顶针帽Z
        /// </summary>
        public float ESZ
        {
            get { return eSZ; }
            set
            {
                if (eSZ != value)
                {
                    eSZ = value;
                    OnPropertyChanged(nameof(ESZ));
                }
            }
        }

        /// <summary>
        /// 顶针Z
        /// </summary>
        public float NeedleZ
        {
            get { return needleZ; }
            set
            {
                if (needleZ != value)
                {
                    needleZ = value;
                    OnPropertyChanged(nameof(NeedleZ));
                }
            }
        }


        /// <summary>
        /// 顶针切换轴
        /// </summary>
        public float NeedleSwitch
        {
            get { return needleSwitch; }
            set
            {
                if (needleSwitch != value)
                {
                    needleSwitch = value;
                    OnPropertyChanged(nameof(NeedleSwitch));
                }
            }
        }
        /// <summary>
        /// 翻转工具Theta轴
        /// </summary>
        public float FilpToolTheta
        {
            get { return filpToolTheta; }
            set
            {
                if (filpToolTheta != value)
                {
                    filpToolTheta = value;
                    OnPropertyChanged(nameof(FilpToolTheta));
                }
            }
        }

        /// <summary>
        /// 衬底吸嘴T轴
        /// </summary>
        public float SubmountPPT
        {
            get { return submountPPT; }
            set
            {
                if (submountPPT != value)
                {
                    submountPPT = value;
                    OnPropertyChanged(nameof(SubmountPPT));
                }
            }
        }
        /// <summary>
        /// 衬底吸嘴Z轴
        /// </summary>
        public float SubmountPPZ
        {
            get { return submountPPZ; }
            set
            {
                if (submountPPZ != value)
                {
                    submountPPZ = value;
                    OnPropertyChanged(nameof(SubmountPPZ));
                }
            }
        }



        /// <summary>
        /// Bond头Y轴
        /// </summary>
        public int BondYSta
        {
            get { return bondYSta; }
            set
            {
                if (bondYSta != value)
                {
                    bondYSta = value;
                    OnPropertyChanged(nameof(BondYSta));
                }
            }
        }

        /// <summary>
        /// Bond头X轴
        /// </summary>
        public int BondXSta
        {
            get { return bondXSta; }
            set
            {
                if (bondXSta != value)
                {
                    bondXSta = value;
                    OnPropertyChanged(nameof(BondXSta));
                }
            }
        }

        /// <summary>
        /// Bond头Z轴
        /// </summary>
        public int BondZSta
        {
            get { return bondZSta; }
            set
            {
                if (bondZSta != value)
                {
                    bondZSta = value;
                    OnPropertyChanged(nameof(BondZSta));
                }
            }
        }

        /// <summary>
        /// 芯片吸嘴Theta轴
        /// </summary>
        public int ChipPPTSta
        {
            get { return chipPPTSta; }
            set
            {
                if (chipPPTSta != value)
                {
                    chipPPTSta = value;
                    OnPropertyChanged(nameof(ChipPPTSta));
                }
            }
        }
        /// <summary>
        /// 吸嘴库Theta轴
        /// </summary>
        public int PPtoolBankThetaSta
        {
            get { return pPtoolBankThetaSta; }
            set
            {
                if (pPtoolBankThetaSta != value)
                {
                    pPtoolBankThetaSta = value;
                    OnPropertyChanged(nameof(PPtoolBankThetaSta));
                }
            }
        }
        /// <summary>
        /// 蘸胶轴
        /// </summary>
        public int DippingGlueSta
        {
            get { return dippingGlueSta; }
            set
            {
                if (dippingGlueSta != value)
                {
                    dippingGlueSta = value;
                    OnPropertyChanged(nameof(DippingGlueSta));
                }
            }
        }
        /// <summary>
        /// 传送轨道1轴
        /// </summary>
        public int TransportTrack1Sta
        {
            get { return transportTrack1Sta; }
            set
            {
                if (transportTrack1Sta != value)
                {
                    transportTrack1Sta = value;
                    OnPropertyChanged(nameof(TransportTrack1Sta));
                }
            }
        }
        /// <summary>
        /// 传送轨道2轴
        /// </summary>
        public int TransportTrack2Sta
        {
            get { return transportTrack2Sta; }
            set
            {
                if (transportTrack2Sta != value)
                {
                    transportTrack2Sta = value;
                    OnPropertyChanged(nameof(TransportTrack2Sta));
                }
            }
        }
        /// <summary>
        /// 传送轨道3轴
        /// </summary>
        public int TransportTrack3Sta
        {
            get { return transportTrack3Sta; }
            set
            {
                if (transportTrack3Sta != value)
                {
                    transportTrack3Sta = value;
                    OnPropertyChanged(nameof(TransportTrack3Sta));
                }
            }
        }


        /// <summary>
        /// 晶圆盘Y轴
        /// </summary>
        public int WaferTableYSta
        {
            get { return waferTableYSta; }
            set
            {
                if (waferTableYSta != value)
                {
                    waferTableYSta = value;
                    OnPropertyChanged(nameof(WaferTableYSta));
                }
            }
        }
        /// <summary>
        /// 晶圆盘X轴
        /// </summary>
        public int WaferTableXSta
        {
            get { return waferTableXSta; }
            set
            {
                if (waferTableXSta != value)
                {
                    waferTableXSta = value;
                    OnPropertyChanged(nameof(WaferTableXSta));
                }
            }
        }

        /// <summary>
        /// 晶圆相机Z
        /// </summary>
        public int WaferTableZSta
        {
            get { return waferTableZSta; }
            set
            {
                if (waferTableZSta != value)
                {
                    waferTableZSta = value;
                    OnPropertyChanged(nameof(WaferTableZSta));
                }
            }
        }
        /// <summary>
        /// 晶圆阔膜轴
        /// </summary>
        public int WaferFilmSta
        {
            get { return waferFilmSta; }
            set
            {
                if (waferFilmSta != value)
                {
                    waferFilmSta = value;
                    OnPropertyChanged(nameof(WaferFilmSta));
                }
            }
        }

        /// <summary>
        /// 晶圆夹爪轴
        /// </summary>
        public int WaferFingerSta
        {
            get { return waferFingerSta; }
            set
            {
                if (waferFingerSta != value)
                {
                    waferFingerSta = value;
                    OnPropertyChanged(nameof(WaferFingerSta));
                }
            }
        }

        /// <summary>
        /// 晶圆抽匣升降轴
        /// </summary>
        public int WaferCassetteLiftSta
        {
            get { return waferCassetteLiftSta; }
            set
            {
                if (waferCassetteLiftSta != value)
                {
                    waferCassetteLiftSta = value;
                    OnPropertyChanged(nameof(WaferCassetteLiftSta));
                }
            }
        }


        /// <summary>
        /// 顶针帽Z
        /// </summary>
        public int ESZSta
        {
            get { return eSZSta; }
            set
            {
                if (eSZSta != value)
                {
                    eSZSta = value;
                    OnPropertyChanged(nameof(ESZSta));
                }
            }
        }

        /// <summary>
        /// 顶针Z
        /// </summary>
        public int NeedleZSta
        {
            get { return needleZSta; }
            set
            {
                if (needleZSta != value)
                {
                    needleZSta = value;
                    OnPropertyChanged(nameof(NeedleZSta));
                }
            }
        }


        /// <summary>
        /// 顶针切换轴
        /// </summary>
        public int NeedleSwitchSta
        {
            get { return needleSwitchSta; }
            set
            {
                if (needleSwitchSta != value)
                {
                    needleSwitchSta = value;
                    OnPropertyChanged(nameof(NeedleSwitchSta));
                }
            }
        }
        /// <summary>
        /// 翻转工具Theta轴
        /// </summary>
        public int FilpToolThetaSta
        {
            get { return filpToolThetaSta; }
            set
            {
                if (filpToolThetaSta != value)
                {
                    filpToolThetaSta = value;
                    OnPropertyChanged(nameof(FilpToolThetaSta));
                }
            }
        }

        /// <summary>
        /// 衬底吸嘴T
        /// </summary>
        public int SubmountPPTSta
        {
            get { return submountPPTSta; }
            set
            {
                if (submountPPTSta != value)
                {
                    submountPPTSta = value;
                    OnPropertyChanged(nameof(SubmountPPTSta));
                }
            }
        }
        /// <summary>
        /// 衬底吸嘴Z
        /// </summary>
        public int SubmountPPZSta
        {
            get { return submountPPZSta; }
            set
            {
                if (submountPPZSta != value)
                {
                    submountPPZSta = value;
                    OnPropertyChanged(nameof(SubmountPPZSta));
                }
            }
        }





        /// <summary>
        /// 已焊接物料个数
        /// </summary>
        public int WeldMaterialNumber
        {
            get { return weldMaterialNumber; }
            set
            {
                if (weldMaterialNumber != value)
                {
                    weldMaterialNumber = value;
                    OnPropertyChanged(nameof(WeldMaterialNumber));
                }
            }
        }

        /// <summary>
        /// 压机工作次数
        /// </summary>
        public int PressWorkNumber
        {
            get { return pressWorkNumber; }
            set
            {
                if (pressWorkNumber != value)
                {
                    pressWorkNumber = value;
                    OnPropertyChanged(nameof(PressWorkNumber));
                }
            }
        }

        /// <summary>
        /// 设备运行时间 分钟
        /// </summary>
        public int EquipmentOperatingTime
        {
            get { return equipmentOperatingTime; }
            set
            {
                if (equipmentOperatingTime != value)
                {
                    equipmentOperatingTime = value;
                    OnPropertyChanged(nameof(EquipmentOperatingTime));
                }
            }
        }

        /// <summary>
        /// 本次设备运行时间 分钟
        /// </summary>
        public int ThisEquipmentOperatingTime
        {
            get { return thisequipmentOperatingTime; }
            set
            {
                if (thisequipmentOperatingTime != value)
                {
                    thisequipmentOperatingTime = value;
                    OnPropertyChanged(nameof(ThisEquipmentOperatingTime));
                }
            }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public bool Run
        {
            get { return run; }
            set
            {
                if (run != value)
                {
                    run = value;
                    OnPropertyChanged(nameof(Run));
                }
            }
        }

        /// <summary>
        /// 错误状态
        /// </summary>
        public bool Error
        {
            get { return error; }
            set
            {
                if (error != value)
                {
                    error = value;
                    OnPropertyChanged(nameof(Error));
                }
            }
        }




        /// <summary>
        /// 轴连接状态
        /// </summary>
        public bool StageAxisIsconnect
        {
            get { return stageAxisIsconnect; }
            set
            {
                if (stageAxisIsconnect != value)
                {
                    stageAxisIsconnect = value;
                    OnPropertyChanged(nameof(StageAxisIsconnect));
                }
            }
        }
        /// <summary>
        /// 轴IO模块连接状态
        /// </summary>
        public bool StageIOIsconnect
        {
            get { return stageIOIsconnect; }
            set
            {
                if (stageIOIsconnect != value)
                {
                    stageIOIsconnect = value;
                    OnPropertyChanged(nameof(StageIOIsconnect));
                }
            }
        }

        /// <summary>
        /// 相机连接状态
        /// </summary>
        public bool CameraIsconnect
        {
            get { return cameraIsconnect; }
            set
            {
                if (cameraIsconnect != value)
                {
                    cameraIsconnect = value;
                    OnPropertyChanged(nameof(CameraIsconnect));
                }
            }
        }

        /// <summary>
        /// 温控表连接状态
        /// </summary>
        public bool TemperatureIsconnect
        {
            get { return temperatureIsconnect; }
            set
            {
                if (temperatureIsconnect != value)
                {
                    temperatureIsconnect = value;
                    OnPropertyChanged(nameof(TemperatureIsconnect));
                }
            }
        }

        /// <summary>
        /// 真空计连接状态
        /// </summary>
        public bool VacuumIsconnect
        {
            get { return vacuumIsconnect; }
            set
            {
                if (vacuumIsconnect != value)
                {
                    vacuumIsconnect = value;
                    OnPropertyChanged(nameof(VacuumIsconnect));
                }
            }
        }

        /// <summary>
        /// 分子泵连接状态
        /// </summary>
        public bool TurboMolecularPumpIsconnect
        {
            get { return turboMolecularPumpIsconnect; }
            set
            {
                if (turboMolecularPumpIsconnect != value)
                {
                    turboMolecularPumpIsconnect = value;
                    OnPropertyChanged(nameof(TurboMolecularPumpIsconnect));
                }
            }
        }


        /// <summary>
        /// 温控表正在读
        /// </summary>
        public bool TemperatureIsReading
        {
            get { return temperatureIsReading; }
            set
            {
                if (temperatureIsReading != value)
                {
                    temperatureIsReading = value;
                    OnPropertyChanged(nameof(TemperatureIsReading));
                }
            }
        }

        /// <summary>
        /// 真空计正在读
        /// </summary>
        public bool VacuumIsReading
        {
            get { return vacuumIsReading; }
            set
            {
                if (vacuumIsReading != value)
                {
                    vacuumIsReading = value;
                    OnPropertyChanged(nameof(VacuumIsReading));
                }
            }
        }

        /// <summary>
        /// 分子泵正在读
        /// </summary>
        public bool TurboMolecularPumpIsReading
        {
            get { return turboMolecularPumpIsReading; }
            set
            {
                if (turboMolecularPumpIsReading != value)
                {
                    turboMolecularPumpIsReading = value;
                    OnPropertyChanged(nameof(TurboMolecularPumpIsReading));
                }
            }
        }


        /// <summary>
        /// 温控表正在写
        /// </summary>
        public bool TemperatureIsWriting
        {
            get { return temperatureIsWriting; }
            set
            {
                if (temperatureIsWriting != value)
                {
                    temperatureIsWriting = value;
                    OnPropertyChanged(nameof(TemperatureIsWriting));
                }
            }
        }

        /// <summary>
        /// 真空计正在写
        /// </summary>
        public bool VacuumIsWriting
        {
            get { return vacuumIsWriting; }
            set
            {
                if (vacuumIsWriting != value)
                {
                    vacuumIsWriting = value;
                    OnPropertyChanged(nameof(VacuumIsWriting));
                }
            }
        }

        /// <summary>
        /// 分子泵正在写
        /// </summary>
        public bool TurboMolecularPumpIsWriting
        {
            get { return turboMolecularPumpIsWriting; }
            set
            {
                if (turboMolecularPumpIsWriting != value)
                {
                    turboMolecularPumpIsWriting = value;
                    OnPropertyChanged(nameof(TurboMolecularPumpIsWriting));
                }
            }
        }


        /// <summary>
        /// 光源连接状态
        /// </summary>
        public bool LightsIsconnect
        {
            get { return lightsIsconnect; }
            set
            {
                if (lightsIsconnect != value)
                {
                    lightsIsconnect = value;
                    OnPropertyChanged(nameof(LightsIsconnect));
                }
            }
        }

        /// <summary>
        /// 光源正在读
        /// </summary>
        public bool LightsIsReading
        {
            get { return lightsIsReading; }
            set
            {
                if (lightsIsReading != value)
                {
                    lightsIsReading = value;
                    OnPropertyChanged(nameof(LightsIsReading));
                }
            }
        }
        /// <summary>
        /// 光源正在写
        /// </summary>
        public bool LightsIsWriting
        {
            get { return lightsIsWriting; }
            set
            {
                if (lightsIsWriting != value)
                {
                    lightsIsWriting = value;
                    OnPropertyChanged(nameof(LightsIsWriting));
                }
            }
        }

        /// <summary>
        /// 点胶机正在读
        /// </summary>
        public bool EpoxtsIsReading { get; set; }
        /// <summary>
        /// 点胶机正在写
        /// </summary>
        public bool EpoxtsIsWriting { get; set; }


        /// <summary>
        /// 激光连接状态
        /// </summary>
        public bool LaserIsconnect
        {
            get { return laserIsconnect; }
            set
            {
                if (laserIsconnect != value)
                {
                    laserIsconnect = value;
                    OnPropertyChanged(nameof(LaserIsconnect));
                }
            }
        }

        /// <summary>
        /// 压力传感器连接状态
        /// </summary>
        public bool PressureIsconnect
        {
            get { return pressureIsconnect; }
            set
            {
                if (pressureIsconnect != value)
                {
                    pressureIsconnect = value;
                    OnPropertyChanged(nameof(PressureIsconnect));
                }
            }
        }


        /// <summary>
        /// IO模块链接状态
        /// </summary>
        public bool Linkstatusofmodules
        {
            get { return linkstatusofmodules; }
            set
            {
                if (linkstatusofmodules != value)
                {
                    linkstatusofmodules = value;
                    OnPropertyChanged(nameof(Linkstatusofmodules));
                }
            }
        }




        public string JobLogText
        {
            get { return joblogText; }
            set
            {
                if (joblogText != value)
                {
                    joblogText = value;
                    OnPropertyChanged(nameof(JobLogText));
                }
            }
        }

        #endregion

        #region OutputIO

        /// <summary>
        /// 芯片吸嘴真空
        /// </summary>
        public bool ChipPPVaccumSwitch
        {
            get { return chipPPVaccumSwitch; }
            set
            {
                if (chipPPVaccumSwitch != value)
                {
                    chipPPVaccumSwitch = value;
                    OnPropertyChanged(nameof(ChipPPVaccumSwitch));
                }
            }
        }
        /// <summary>
        /// 芯片吸嘴破空
        /// </summary>
        public bool ChipPPBlowSwitch
        {
            get { return chipPPBlowSwitch; }
            set
            {
                if (chipPPBlowSwitch != value)
                {
                    chipPPBlowSwitch = value;
                    OnPropertyChanged(nameof(ChipPPBlowSwitch));
                }
            }
        }
        /// <summary>
        /// 胶针升降气缸
        /// </summary>
        public bool EpoxtliftCylinder
        {
            get { return epoxtliftCylinder; }
            set
            {
                if (epoxtliftCylinder != value)
                {
                    epoxtliftCylinder = value;
                    OnPropertyChanged(nameof(EpoxtliftCylinder));
                }
            }
        }
        /// <summary>
        /// 胶针吐出
        /// </summary>
        public bool EpoxtDIS
        {
            get { return epoxtDIS; }
            set
            {
                if (epoxtDIS != value)
                {
                    epoxtDIS = value;
                    OnPropertyChanged(nameof(EpoxtDIS));
                }
            }
        }
        /// <summary>
        /// 胶针吐出有效
        /// </summary>
        public bool EpoxtENABLE
        {
            get { return epoxtENABLE; }
            set
            {
                if (epoxtENABLE != value)
                {
                    epoxtENABLE = value;
                    OnPropertyChanged(nameof(EpoxtENABLE));
                }
            }
        }

        /// <summary>
        /// 胶针指定模式
        /// </summary>
        public bool EpoxtTMD
        {
            get { return epoxtTMD; }
            set
            {
                if (epoxtTMD != value)
                {
                    epoxtTMD = value;
                    OnPropertyChanged(nameof(EpoxtTMD));
                }
            }
        }

        /// <summary>
        /// 胶针吐出模式切换
        /// </summary>
        public bool EpoxtTMS
        {
            get { return epoxtTMS; }
            set
            {
                if (epoxtTMS != value)
                {
                    epoxtTMS = value;
                    OnPropertyChanged(nameof(EpoxtTMS));
                }
            }
        }

        /// <summary>
        /// 胶针计数器清除
        /// </summary>
        public bool EpoxtC_CNT
        {
            get { return epoxtC_CNT; }
            set
            {
                if (epoxtC_CNT != value)
                {
                    epoxtC_CNT = value;
                    OnPropertyChanged(nameof(EpoxtC_CNT));
                }
            }
        }

        /// <summary>
        /// 胶针重置
        /// </summary>
        public bool EpoxtRESET
        {
            get { return epoxtRESET; }
            set
            {
                if (epoxtRESET != value)
                {
                    epoxtRESET = value;
                    OnPropertyChanged(nameof(EpoxtRESET));
                }
            }
        }

        /// <summary>
        /// 衬底吸嘴真空
        /// </summary>
        public bool SubmountPPVaccumSwitch
        {
            get { return submountPPVaccumSwitch; }
            set
            {
                if (submountPPVaccumSwitch != value)
                {
                    submountPPVaccumSwitch = value;
                    OnPropertyChanged(nameof(SubmountPPVaccumSwitch));
                }
            }
        }
        /// <summary>
        /// 衬底吸嘴破空
        /// </summary>
        public bool SubmountPPBlowSwitch
        {
            get { return submountPPBlowSwitch; }
            set
            {
                if (submountPPBlowSwitch != value)
                {
                    submountPPBlowSwitch = value;
                    OnPropertyChanged(nameof(SubmountPPBlowSwitch));
                }
            }
        }

        /// <summary>
        /// 传送轨道气缸1
        /// </summary>
        public bool TransportCylinder1
        {
            get { return transportCylinder1; }
            set
            {
                if (transportCylinder1 != value)
                {
                    transportCylinder1 = value;
                    OnPropertyChanged(nameof(TransportCylinder1));
                }
            }
        }
        /// <summary>
        /// 传送轨道气缸2
        /// </summary>
        public bool TransportCylinder2
        {
            get { return transportCylinder2; }
            set
            {
                if (transportCylinder2 != value)
                {
                    transportCylinder2 = value;
                    OnPropertyChanged(nameof(TransportCylinder2));
                }
            }
        }
        /// <summary>
        /// 传送轨道真空开关1
        /// </summary>
        public bool TransportVaccumSwitch1
        {
            get { return transportVaccumSwitch1; }
            set
            {
                if (transportVaccumSwitch1 != value)
                {
                    transportVaccumSwitch1 = value;
                    OnPropertyChanged(nameof(TransportVaccumSwitch1));
                }
            }
        }
        /// <summary>
        /// 传送轨道真空开关2
        /// </summary>
        public bool TransportVaccumSwitch2
        {
            get { return transportVaccumSwitch2; }
            set
            {
                if (transportVaccumSwitch2 != value)
                {
                    transportVaccumSwitch2 = value;
                    OnPropertyChanged(nameof(TransportVaccumSwitch2));
                }
            }
        }

        /// <summary>
        /// 共晶台真空
        /// </summary>
        public bool EutecticPlatformVaccumSwitch
        {
            get { return eutecticPlatformVaccumSwitch; }
            set
            {
                if (eutecticPlatformVaccumSwitch != value)
                {
                    eutecticPlatformVaccumSwitch = value;
                    OnPropertyChanged(nameof(EutecticPlatformVaccumSwitch));
                }
            }
        }
        /// <summary>
        /// 氮气阀
        /// </summary>
        public bool NitrogenValve
        {
            get { return nitrogenValve; }
            set
            {
                if (nitrogenValve != value)
                {
                    nitrogenValve = value;
                    OnPropertyChanged(nameof(NitrogenValve));
                }
            }
        }

        /// <summary>
        /// 开始共晶
        /// </summary>
        public bool StartEctectic
        {
            get { return startEctectic; }
            set
            {
                if (startEctectic != value)
                {
                    startEctectic = value;
                    OnPropertyChanged(nameof(StartEctectic));
                }
            }
        }
        /// <summary>
        /// 重置共晶
        /// </summary>
        public bool ResetEutectic
        {
            get { return resetEutectic; }
            set
            {
                if (resetEutectic != value)
                {
                    resetEutectic = value;
                    OnPropertyChanged(nameof(ResetEutectic));
                }
            }
        }



        /// <summary>
        /// 顶针系统真空
        /// </summary>
        public bool EjectionSystemVaccumSwitch
        {
            get { return ejectionSystemVaccumSwitch; }
            set
            {
                if (ejectionSystemVaccumSwitch != value)
                {
                    ejectionSystemVaccumSwitch = value;
                    OnPropertyChanged(nameof(EjectionSystemVaccumSwitch));
                }
            }
        }
        /// <summary>
        /// 顶针系统破空
        /// </summary>
        public bool EjectionSystemBlowSwitch
        {
            get { return ejectionSystemBlowSwitch; }
            set
            {
                if (ejectionSystemBlowSwitch != value)
                {
                    ejectionSystemBlowSwitch = value;
                    OnPropertyChanged(nameof(EjectionSystemBlowSwitch));
                }
            }
        }

        /// <summary>
        /// 晶圆夹爪气缸
        /// </summary>
        public bool WaferFingerCylinder
        {
            get { return waferFingerCylinder; }
            set
            {
                if (waferFingerCylinder != value)
                {
                    waferFingerCylinder = value;
                    OnPropertyChanged(nameof(WaferFingerCylinder));
                }
            }
        }

        /// <summary>
        /// 晶圆夹持气缸
        /// </summary>
        public bool WaferClampCylinder
        {
            get { return waferClampCylinder; }
            set
            {
                if (waferClampCylinder != value)
                {
                    waferClampCylinder = value;
                    OnPropertyChanged(nameof(WaferClampCylinder));
                }
            }
        }

        /// <summary>
        /// 晶圆抽匣气缸
        /// </summary>
        public bool WaferCassetteCylinder
        {
            get { return waferCassetteCylinder; }
            set
            {
                if (waferCassetteCylinder != value)
                {
                    waferCassetteCylinder = value;
                    OnPropertyChanged(nameof(WaferCassetteCylinder));
                }
            }
        }


        /// <summary>
        /// 晶圆真空
        /// </summary>
        public bool WaferTableVaccumSwitch
        {
            get { return waferTableVaccumSwitch; }
            set
            {
                if (waferTableVaccumSwitch != value)
                {
                    waferTableVaccumSwitch = value;
                    OnPropertyChanged(nameof(WaferTableVaccumSwitch));
                }
            }
        }

        /// <summary>
        /// 华夫盒真空
        /// </summary>
        public bool MaterialPlatformVaccumSwitch
        {
            get { return materialPlatformVaccumSwitch; }
            set
            {
                if (materialPlatformVaccumSwitch != value)
                {
                    materialPlatformVaccumSwitch = value;
                    OnPropertyChanged(nameof(MaterialPlatformVaccumSwitch));
                }
            }
        }





        /// <summary>
        /// 塔灯黄灯
        /// </summary>
        public bool TowerYellowLight
        {
            get { return towerYellowLight; }
            set
            {
                if (towerYellowLight != value)
                {
                    towerYellowLight = value;
                    OnPropertyChanged(nameof(TowerYellowLight));
                }
            }
        }
        /// <summary>
        /// 塔灯绿灯
        /// </summary>
        public bool TowerGreenLight
        {
            get { return towerGreenLight; }
            set
            {
                if (towerGreenLight != value)
                {
                    towerGreenLight = value;
                    OnPropertyChanged(nameof(TowerGreenLight));
                }
            }
        }
        /// <summary>
        /// 塔灯红灯
        /// </summary>
        public bool TowerRedLight
        {
            get { return towerRedLight; }
            set
            {
                if (towerRedLight != value)
                {
                    towerRedLight = value;
                    OnPropertyChanged(nameof(TowerRedLight));
                }
            }
        }


        /// <summary>
        /// 晶圆抽匣轴抱闸
        /// </summary>
        public bool WaferCassetteLiftMotorBrake
        {
            get { return waferCassetteLiftMotorBrake; }
            set
            {
                if (waferCassetteLiftMotorBrake != value)
                {
                    waferCassetteLiftMotorBrake = value;
                    OnPropertyChanged(nameof(WaferCassetteLiftMotorBrake));
                }
            }
        }

        /// <summary>
        /// 顶针升降轴抱闸
        /// </summary>
        public bool EjectionLiftMotorBrake
        {
            get { return ejectionLiftMotorBrake; }
            set
            {
                if (ejectionLiftMotorBrake != value)
                {
                    ejectionLiftMotorBrake = value;
                    OnPropertyChanged(nameof(EjectionLiftMotorBrake));
                }
            }
        }


        #endregion

        #region InputIO

        /// <summary>
        /// 芯片吸嘴真空到位
        /// </summary>
        public bool ChipPPVaccumNormally
        {
            get { return chipPPVaccumNormally; }
            set
            {
                if (chipPPVaccumNormally != value)
                {
                    chipPPVaccumNormally = value;
                    OnPropertyChanged(nameof(ChipPPVaccumNormally));
                }
            }
        }
        /// <summary>
        /// 衬底吸嘴真空到位
        /// </summary>
        public bool SubmountPPVaccumNormally
        {
            get { return submountPPVaccumNormally; }
            set
            {
                if (submountPPVaccumNormally != value)
                {
                    submountPPVaccumNormally = value;
                    OnPropertyChanged(nameof(SubmountPPVaccumNormally));
                }
            }
        }

        /// <summary>
        /// 胶针通电中
        /// </summary>
        public bool EpoxtPON
        {
            get { return epoxtPON; }
            set
            {
                if (epoxtPON != value)
                {
                    epoxtPON = value;
                    OnPropertyChanged(nameof(EpoxtPON));
                }
            }
        }

        /// <summary>
        /// 胶针吐出中
        /// </summary>
        public bool EpoxtDSO
        {
            get { return epoxtDSO; }
            set
            {
                if (epoxtDSO != value)
                {
                    epoxtDSO = value;
                    OnPropertyChanged(nameof(EpoxtDSO));
                }
            }
        }
        /// <summary>
        /// 胶针吐出完成
        /// </summary>
        public bool EpoxtEND
        {
            get { return epoxtEND; }
            set
            {
                if (epoxtEND != value)
                {
                    epoxtEND = value;
                    OnPropertyChanged(nameof(EpoxtEND));
                }
            }
        }

        /// <summary>
        /// 胶针错误
        /// </summary>
        public bool EpoxtERROR
        {
            get { return epoxtERROR; }
            set
            {
                if (epoxtERROR != value)
                {
                    epoxtERROR = value;
                    OnPropertyChanged(nameof(EpoxtERROR));
                }
            }
        }

        /// <summary>
        /// 胶针报警
        /// </summary>
        public bool EpoxtALARM
        {
            get { return epoxtALARM; }
            set
            {
                if (epoxtALARM != value)
                {
                    epoxtALARM = value;
                    OnPropertyChanged(nameof(EpoxtALARM));
                }
            }
        }

        /// <summary>
        /// 胶针报警2
        /// </summary>
        public bool EpoxtALARM2
        {
            get { return epoxtALARM2; }
            set
            {
                if (epoxtALARM2 != value)
                {
                    epoxtALARM2 = value;
                    OnPropertyChanged(nameof(EpoxtALARM2));
                }
            }
        }

        /// <summary>
        /// 胶针余量报警
        /// </summary>
        public bool EpoxtRSM
        {
            get { return epoxtRSM; }
            set
            {
                if (epoxtRSM != value)
                {
                    epoxtRSM = value;
                    OnPropertyChanged(nameof(EpoxtRSM));
                }
            }
        }

        /// <summary>
        /// 胶针准备
        /// </summary>
        public bool EpoxtREADY
        {
            get { return epoxtREADY; }
            set
            {
                if (epoxtREADY != value)
                {
                    epoxtREADY = value;
                    OnPropertyChanged(nameof(EpoxtREADY));
                }
            }
        }


        /// <summary>
        /// 传送轨道到位信号1
        /// </summary>
        public bool TransportInPlaceSignal1
        {
            get { return transportInPlaceSignal1; }
            set
            {
                if (transportInPlaceSignal1 != value)
                {
                    transportInPlaceSignal1 = value;
                    OnPropertyChanged(nameof(TransportInPlaceSignal1));
                }
            }
        }

        /// <summary>
        /// 传送轨道到位信号2
        /// </summary>
        public bool TransportInPlaceSignal2
        {
            get { return transportInPlaceSignal2; }
            set
            {
                if (transportInPlaceSignal2 != value)
                {
                    transportInPlaceSignal2 = value;
                    OnPropertyChanged(nameof(TransportInPlaceSignal2));
                }
            }
        }

        /// <summary>
        /// 传送轨道到位信号3
        /// </summary>
        public bool TransportInPlaceSignal3
        {
            get { return transportInPlaceSignal3; }
            set
            {
                if (transportInPlaceSignal3 != value)
                {
                    transportInPlaceSignal3 = value;
                    OnPropertyChanged(nameof(TransportInPlaceSignal3));
                }
            }
        }

        /// <summary>
        /// 晶圆盘到位信号
        /// </summary>
        public bool WaferInPlaceSignal1
        {
            get { return waferInPlaceSignal1; }
            set
            {
                if (waferInPlaceSignal1 != value)
                {
                    waferInPlaceSignal1 = value;
                    OnPropertyChanged(nameof(WaferInPlaceSignal1));
                }
            }
        }


        /// <summary>
        /// 安全门传感器1
        /// </summary>
        public bool SafeDoorSensor1
        {
            get { return safeDoorSensor1; }
            set
            {
                if (safeDoorSensor1 != value)
                {
                    safeDoorSensor1 = value;
                    OnPropertyChanged(nameof(SafeDoorSensor1));
                }
            }
        }

        /// <summary>
        /// 安全门传感器2
        /// </summary>
        public bool SafeDoorSensor2
        {
            get { return safeDoorSensor2; }
            set
            {
                if (safeDoorSensor2 != value)
                {
                    safeDoorSensor2 = value;
                    OnPropertyChanged(nameof(SafeDoorSensor2));
                }
            }
        }


        /// <summary>
        /// 共晶错误
        /// </summary>
        public bool EutecticError
        {
            get { return eutecticError; }
            set
            {
                if (eutecticError != value)
                {
                    eutecticError = value;
                    OnPropertyChanged(nameof(EutecticError));
                }
            }
        }

        /// <summary>
        /// 共晶完成
        /// </summary>
        public bool EutecticComplete
        {
            get { return eutecticComplete; }
            set
            {
                if (eutecticComplete != value)
                {
                    eutecticComplete = value;
                    OnPropertyChanged(nameof(EutecticComplete));
                }
            }
        }




        #endregion


        #region SerialPort

        /// <summary>
        /// 榜头直光（红）
        /// </summary>
        public int BondRed
        {
            get { return bondRed; }
            set
            {
                if (bondRed != value)
                {
                    bondRed = value;
                    OnPropertyChanged(nameof(BondRed));
                }
            }
        }
        /// <summary>
        /// 榜头直光（绿）
        /// </summary>
        public int BondGreen
        {
            get { return bondGreen; }
            set
            {
                if (bondGreen != value)
                {
                    bondGreen = value;
                    OnPropertyChanged(nameof(BondGreen));
                }
            }
        }
        /// <summary>
        /// 榜头直光（蓝）
        /// </summary>
        public int BondBlue
        {
            get { return bondBlue; }
            set
            {
                if (bondBlue != value)
                {
                    bondBlue = value;
                    OnPropertyChanged(nameof(BondBlue));
                }
            }
        }
        /// <summary>
        /// 榜头环光
        /// </summary>
        public int BondRing
        {
            get { return bondRing; }
            set
            {
                if (bondRing != value)
                {
                    bondRing = value;
                    OnPropertyChanged(nameof(BondRing));
                }
            }
        }


        /// <summary>
        /// 晶圆直光（红）
        /// </summary>
        public int WaferRed
        {
            get { return waferRed; }
            set
            {
                if (waferRed != value)
                {
                    waferRed = value;
                    OnPropertyChanged(nameof(WaferRed));
                }
            }
        }
        /// <summary>
        /// 晶圆直光（绿）
        /// </summary>
        public int WaferGreen
        {
            get { return waferGreen; }
            set
            {
                if (waferGreen != value)
                {
                    waferGreen = value;
                    OnPropertyChanged(nameof(WaferGreen));
                }
            }
        }
        /// <summary>
        /// 晶圆直光（蓝）
        /// </summary>
        public int WaferBlue
        {
            get { return waferBlue; }
            set
            {
                if (waferBlue != value)
                {
                    waferBlue = value;
                    OnPropertyChanged(nameof(WaferBlue));
                }
            }
        }
        /// <summary>
        /// 晶圆环光
        /// </summary>
        public int WaferRing
        {
            get { return waferRing; }
            set
            {
                if (waferRing != value)
                {
                    waferRing = value;
                    OnPropertyChanged(nameof(WaferRing));
                }
            }
        }

        /// <summary>
        /// 仰视直光（红）
        /// </summary>
        public int UpLookingRed
        {
            get { return upLookingRed; }
            set
            {
                if (upLookingRed != value)
                {
                    upLookingRed = value;
                    OnPropertyChanged(nameof(UpLookingRed));
                }
            }
        }
        /// <summary>
        /// 仰视直光（绿）
        /// </summary>
        public int UpLookingGreen
        {
            get { return upLookingGreen; }
            set
            {
                if (upLookingGreen != value)
                {
                    upLookingGreen = value;
                    OnPropertyChanged(nameof(UpLookingGreen));
                }
            }
        }
        /// <summary>
        /// 仰视直光（蓝）
        /// </summary>
        public int UpLookingBlue
        {
            get { return upLookingBlue; }
            set
            {
                if (upLookingBlue != value)
                {
                    upLookingBlue = value;
                    OnPropertyChanged(nameof(UpLookingBlue));
                }
            }
        }
        /// <summary>
        /// 仰视环光
        /// </summary>
        public int UpLookingRing
        {
            get { return upLookingRing; }
            set
            {
                if (upLookingRing != value)
                {
                    upLookingRing = value;
                    OnPropertyChanged(nameof(UpLookingRing));
                }
            }
        }


        /// <summary>
        /// 激光传感器读数
        /// </summary>
        public double LaserValue
        {
            get { return laserValue; }
            set
            {
                if (laserValue != value)
                {
                    laserValue = value;
                    OnPropertyChanged(nameof(LaserValue));
                }
            }
        }
        /// <summary>
        /// 压力传感器读数
        /// </summary>
        public double PressureValue1
        {
            get { return pressureValue1; }
            set
            {
                if (pressureValue1 != value)
                {
                    pressureValue1 = value;
                    OnPropertyChanged(nameof(PressureValue1));
                }
            }
        }
        /// <summary>
        /// 压力传感器读数
        /// </summary>
        public double PressureValue2
        {
            get { return pressureValue2; }
            set
            {
                if (pressureValue2 != value)
                {
                    pressureValue2 = value;
                    OnPropertyChanged(nameof(PressureValue2));
                }
            }
        }


        /// <summary>
        /// 点胶机当前通道
        /// </summary>
        public int EpoxtCH
        {
            get { return epoxtCH; }
            set
            {
                if (epoxtCH != value)
                {
                    epoxtCH = value;
                    OnPropertyChanged(nameof(epoxtCH));
                }
            }
        }
        /// <summary>
        /// 点胶机当前模式
        /// </summary>
        public int EpoxtMode
        {
            get { return epoxtMode; }
            set
            {
                if (epoxtMode != value)
                {
                    epoxtMode = value;
                    OnPropertyChanged(nameof(EpoxtMode));
                }
            }
        }

        /// <summary>
        /// 点胶机点胶时间
        /// </summary>
        public double EpoxtTime
        {
            get { return epoxtTime; }
            set
            {
                if (epoxtTime != value)
                {
                    epoxtTime = value;
                    OnPropertyChanged(nameof(EpoxtTime));
                }
            }
        }

        /// <summary>
        /// 点胶机点胶压力
        /// </summary>
        public double EpoxtPressure
        {
            get { return epoxtPressure; }
            set
            {
                if (epoxtPressure != value)
                {
                    epoxtPressure = value;
                    OnPropertyChanged(nameof(EpoxtPressure));
                }
            }
        }

        /// <summary>
        /// 点胶机点胶真空
        /// </summary>
        public double EpoxtVacuum
        {
            get { return epoxtVacuum; }
            set
            {
                if (epoxtVacuum != value)
                {
                    epoxtVacuum = value;
                    OnPropertyChanged(nameof(EpoxtVacuum));
                }
            }
        }

        /// <summary>
        /// 点胶机Σ模式
        /// </summary>
        public int EpoxtMode2
        {
            get { return epoxtMode2; }
            set
            {
                if (epoxtMode2 != value)
                {
                    epoxtMode2 = value;
                    OnPropertyChanged(nameof(EpoxtMode2));
                }
            }
        }

        /// <summary>
        /// 点胶机计数值
        /// </summary>
        public int EpoxtSHOT
        {
            get { return epoxtSHOT; }
            set
            {
                if (epoxtSHOT != value)
                {
                    epoxtSHOT = value;
                    OnPropertyChanged(nameof(EpoxtSHOT));
                }
            }
        }

        /// <summary>
        /// 点胶机供气压力
        /// </summary>
        public double EpoxtCurrentPressure
        {
            get { return epoxtCurrentPressure; }
            set
            {
                if (epoxtCurrentPressure != value)
                {
                    epoxtCurrentPressure = value;
                    OnPropertyChanged(nameof(EpoxtCurrentPressure));
                }
            }
        }

        /// <summary>
        /// 点胶机上一次点胶时间
        /// </summary>
        public double EpoxtCurrentTime
        {
            get { return epoxtCurrentTime; }
            set
            {
                if (epoxtCurrentTime != value)
                {
                    epoxtCurrentTime = value;
                    OnPropertyChanged(nameof(EpoxtCurrentTime));
                }
            }
        }




        #endregion


        

        #endregion

        #region 订阅事件

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnInnerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged($"MaterialMapLog.{e.PropertyName}");
        }

        #endregion


        #region IO锁定


        #endregion

    }
}
