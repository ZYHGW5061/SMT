using BoardCardControllerClsLib;
using ConfigurationClsLib;
using DispensingMachineManagerClsLib;
using DynamometerManagerClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using LaserSensorManagerClsLib;
using LightControllerManagerClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace IOUtilityClsLib
{
    public class IOUtilityHelper
    {
        private static readonly object _lockObj = new object();
        private static volatile IOUtilityHelper _instance = null;
        private bool _enablePollingIO;
        private bool _enablePollingIO1;
        private bool _enablePollingIO2;
        private bool _enablePollingIO3;
        private bool _enablePollingIO4;
        private bool _enablePollingIO5;
        IBoardCardController _boardCardController;
        private Dictionary<EnumBoardcardDefineInputIO, string> _inputIOList;
        private Dictionary<EnumBoardcardDefineOutputIO, string> _outputIOList;

        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        private LightControllerManager _LightControllerManager
        {
            get { return LightControllerManager.Instance; }
        }

        private LaserSensorManager _LaserSensorManager
        {
            get { return LaserSensorManager.Instance; }
        }
        private DynamometerManager _DynamometerManager
        {
            get { return DynamometerManager.Instance; }
        }

        private DispensingMachineManager _DispensingMachineManager
        {
            get { return DispensingMachineManager.Instance; }
        }


        /// <summary>
        /// 重试机制
        /// </summary>
        RetryMechanismOperation _retryMechanismOperation = new RetryMechanismOperation();
        public static IOUtilityHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new IOUtilityHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        private IOUtilityHelper()
        {
            _inputIOList = new Dictionary<EnumBoardcardDefineInputIO, string>();
            _inputIOList.Add(EnumBoardcardDefineInputIO.ChipPPVaccumNormally, "ChipPPVaccumNormally");
            _inputIOList.Add(EnumBoardcardDefineInputIO.SubmountPPVaccumNormally, "SubmountPPVaccumNormally");
            _inputIOList.Add(EnumBoardcardDefineInputIO.EutecticError, "EutecticError");
            _inputIOList.Add(EnumBoardcardDefineInputIO.EutecticComplete, "EutecticComplete");
            _inputIOList.Add(EnumBoardcardDefineInputIO.SafeDoorSensor1, "SafeDoorSensor1");
            _inputIOList.Add(EnumBoardcardDefineInputIO.SafeDoorSensor2, "SafeDoorSensor2");


            _outputIOList = new Dictionary<EnumBoardcardDefineOutputIO, string>();
            _outputIOList.Add(EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch, "SubmountPPVaccumSwitch");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch, "SubmountPPBlowSwitch");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, "ChipPPVaccumSwitch");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, "ChipPPBlowSwitch");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, "EjectionSystemVaccumSwitch");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch, "EutecticPlatformVaccumSwitch");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, "MaterialPlatformVaccumSwitch");
            //_outputIOList.Add(EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, "WaferTableVaccumSwitch");

            _outputIOList.Add(EnumBoardcardDefineOutputIO.NitrogenValve, "NitrogenValve");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.StartEctectic, "StartEctectic");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.TowerRedLight, "TowerRedLight");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.TowerYellowLight, "TowerYellowLight");
            _outputIOList.Add(EnumBoardcardDefineOutputIO.TowerGreenLight, "TowerGreenLight");

            _boardCardController = BoardCardManager.Instance.GetCurrentController();
        }
        public void Start()
        {
            _enablePollingIO = true;
            Task.Run(new Action(ReadIOTask));

            //_enablePollingIO1 = true;
            //Task.Run(new Action(ReadSerialPortTask1));

            //_enablePollingIO2 = true;
            //Task.Run(new Action(ReadSerialPortTask2));

            //_enablePollingIO3 = true;
            //Task.Run(new Action(ReadSerialPortTask3));

            //_enablePollingIO4 = true;
            //Task.Run(new Action(ReadSerialPortTask4));

            //_enablePollingIO5 = true;
            //Task.Run(new Action(ReadSerialPortTask5));
        }
        public bool IsChipPPVaccumOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开芯片吸嘴真空
        /// </summary>
        public bool OpenChipPPVaccum()
        {
            _retryMechanismOperation = new RetryMechanismOperation()
            {
                MaxRetryCount = 10,
                ProcessFunc = () =>
                {
                    if (GetChipPPVaccumStatus()==0)
                    {
                        Thread.Sleep(300);
                        return false;
                    }
                    return true;
                }
            };
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, 1);
            if (SystemConfiguration.Instance.JobConfig.EnableVaccumConfirm)
            {
                _retryMechanismOperation.Run();
                if (_retryMechanismOperation.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 关闭芯片吸嘴真空
        /// </summary>
        public bool CloseChipPPVaccum()
        {
            _retryMechanismOperation = new RetryMechanismOperation()
            {
                MaxRetryCount = 10,
                ProcessFunc = () =>
                {
                    if (GetChipPPVaccumStatus() == 1)
                    {
                        Thread.Sleep(300);
                        return false;
                    }
                    return true;
                }
            };
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, 0);
            _retryMechanismOperation.Run();
            if (_retryMechanismOperation.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsSubmountPPVaccumOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开衬底吸嘴真空
        /// </summary>
        public bool OpenSubmountPPVaccum()
        {
            _retryMechanismOperation = new RetryMechanismOperation()
            {
                MaxRetryCount = 10,
                ProcessFunc = () =>
                {
                    if (GetSubmountPPVaccumStatus() == 0)
                    {
                        Thread.Sleep(300);
                        return false;
                    }
                    return true;
                }
            };
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch, 1);
            if (SystemConfiguration.Instance.JobConfig.EnableVaccumConfirm)
            {
                _retryMechanismOperation.Run();
                if (_retryMechanismOperation.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 关闭衬底吸嘴真空
        /// </summary>
        public bool CloseSubmountPPVaccum()
        {


            _retryMechanismOperation = new RetryMechanismOperation()
            {
                MaxRetryCount = 10,
                ProcessFunc = () =>
                {
                    if (GetSubmountPPVaccumStatus() == 1)
                    {
                        Thread.Sleep(300);
                        return false;
                    }
                    return true;
                }
            };
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch, 0);
            _retryMechanismOperation.Run();
            if (_retryMechanismOperation.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsEutecticPlatformPPVaccumOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch, out status);
            ret = status == 1;
            return ret;
        }


        /// <summary>
        /// 打开共晶平台真空
        /// </summary>
        public void OpenEutecticPlatformPPVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch, 1);
        }
        /// <summary>
        /// 关闭共晶平台真空
        /// </summary>
        public void CloseEutecticPlatformPPVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch, 0);
        }
        public bool IsESBaseVaccumOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开顶针座真空
        /// </summary>
        public void OpenESBaseVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, 1);
        }
        /// <summary>
        /// 关闭顶针座真空
        /// </summary>
        public void CloseESBaseVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, 0);
        }

        public bool IsMaterailPlatformVaccumOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开物料平台真空
        /// </summary>
        public void OpenMaterailPlatformVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 1);
        }
        /// <summary>
        /// 关闭物料平台真空
        /// </summary>
        public void CloseMaterailPlatformVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 0);
        }
        /// <summary>
        /// 打开传送轨道真空1
        /// </summary>
        public void OpenTransportVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch1, 1);
        }
        /// <summary>
        /// 关闭传送轨道真空1
        /// </summary>
        public void CloseTransportVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch1, 0);
        }
        /// <summary>
        /// 打开传送轨道气缸1
        /// </summary>
        public void OpenTransportCylinder1()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportCylinder1, 1);
        }
        /// <summary>
        /// 关闭传送轨道气缸1
        /// </summary>
        public void CloseTransportCylinder1()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportCylinder1, 0);
        }
        public bool IsWaferTableVaccumOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开wafertable真空
        /// </summary>
        public void OpenWaferTableVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 1);
        }
        /// <summary>
        /// 关闭wafertable真空
        /// </summary>
        public void CloseWaferTableVaccum()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 0);
        }
        /// <summary>
        /// 获取芯片真空状态
        /// </summary>
        /// <returns>0：关闭，1：打开</returns>
        public int GetChipPPVaccumStatus()
        {
            int isOpen = 0;
            _boardCardController.IO_ReadInput_2(11, (int)EnumBoardcardDefineInputIO.ChipPPVaccumNormally, out isOpen);
            return isOpen;
        }

        /// <summary>
        /// 获取衬底真空状态
        /// </summary>
        /// <returns>0：关闭，1：打开</returns>
        public int GetSubmountPPVaccumStatus()
        {
            int isOpen = 0;
            _boardCardController.IO_ReadInput_2(11, (int)EnumBoardcardDefineInputIO.SubmountPPVaccumNormally, out isOpen);
            return isOpen;
        }
        public bool IsChipPPBlowOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开芯片吹气
        /// </summary>
        public void OpenChipPPBlow()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, 1);
        }
        /// <summary>
        /// 关闭芯片吹气
        /// </summary>
        public void CloseChipPPBlow()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, 0);
        }
        public bool IsSubmountPPBlowOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 打开衬底吹气
        /// </summary>
        public void OpenSubmountPPBlow()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch, 1);
        }
        /// <summary>
        /// 关闭衬底吹气
        /// </summary>
        public void CloseSubmountPPBlow()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch, 0);
        }

        public bool IsTowerRedLightOn()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.TowerRedLight, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 亮红灯
        /// </summary>
        public void TurnonTowerRedLight()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerRedLight, 1);
        }
        /// <summary>
        /// 灭红灯
        /// </summary>
        public void TurnoffTowerRedLight()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerRedLight, 0);
        }

        public bool IsTowerYellowLightOn()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 亮黄灯
        /// </summary>
        public void TurnonTowerYellowLight()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 1);
        }
        /// <summary>
        /// 灭黄灯
        /// </summary>
        public void TurnoffTowerYellowLight()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 0);
        }
        public bool IsTowerGreenLightOn()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.TowerGreenLight, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 亮绿灯
        /// </summary>
        public void TurnonTowerGreenLight()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerGreenLight, 1);
        }
        /// <summary>
        /// 灭绿灯
        /// </summary>
        public void TurnoffTowerGreenLight()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerGreenLight, 0);
        }
        public bool IsNitrogenValveOpened()
        {
            var ret = false;
            var status = 0;
            _boardCardController.IO_ReadOutput(11, (int)EnumBoardcardDefineOutputIO.NitrogenValve, out status);
            ret = status == 1;
            return ret;
        }
        /// <summary>
        /// 开氮气
        /// </summary>
        public void OpenNitrogen()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.NitrogenValve, 1);
        }
        /// <summary>
        /// 关氮气
        /// </summary>
        public void CloseNitrogen()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.NitrogenValve, 0);
        }
        public void UpDispenserCylinder()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
        }
        public void DownDispenserCylinder()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 1);
        }
        /// <summary>
        /// 开加热
        /// </summary>
        public void OpenEctectic()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StartEctectic, 1);
        }
        /// <summary>
        /// 关加热
        /// </summary>
        public void CloseEctectic()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StartEctectic, 0);
        }

        public bool IsEctecticComplete()
        {
            var ret = false;
            int status = 0;
            _boardCardController.IO_ReadInput_2(11, (int)EnumBoardcardDefineInputIO.EutecticComplete, out status);
            ret = status == 1;
            return ret;
        }

        public void ResetEctecticComplete()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineInputIO.EutecticComplete, 0);
        }

        /// <summary>
        /// 复位
        /// </summary>
        public void ResetEctectic()
        {
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ResetEutectic, 1);
            Thread.Sleep(200);
            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ResetEutectic, 0);
        }

        public bool IsEctecticFault()
        {
            var ret = false;
            int status = 0;
            _boardCardController.IO_ReadInput_2(11, (int)EnumBoardcardDefineInputIO.EutecticError, out status);
            ret = status == 1;
            return ret;
        }


        /// <summary>
        /// 读取IO状态的任务
        /// </summary>
        private void ReadIOTask()
        {
            while (_enablePollingIO)
            {
                Thread.Sleep(100);
                if(_boardCardController != null)
                {
                    List<int> data = new List<int>();
                    _boardCardController.IO_ReadAllInput_2(11, out data);
                    if (data.Count > 0)
                    {
                        ParseDataAndUpdateInputIOValue(data);
                    }
                    _boardCardController.IO_ReadAllOutput_2(11, out data);
                    if (data.Count > 0)
                    {
                        ParseDataAndUpdateOutputIOValue(data);
                    }
                    ReadAxisesPos();
                }
                
            }
        }
        /// <summary>
        /// 解析字符串并更新该IO点Value值
        /// </summary>
        internal void ParseDataAndUpdateInputIOValue(List<int> msg)
        {
            //IOManager.Instance.ChangeIOValue(_inputIOList[EnumBoardcardDefineInputIO.ChipPPVaccumNormally], msg[(int)EnumBoardcardDefineInputIO.ChipPPVaccumNormally - 1] ==1? true : false);
            //IOManager.Instance.ChangeIOValue(_inputIOList[EnumBoardcardDefineInputIO.SubmountPPVaccumNormally], msg[(int)EnumBoardcardDefineInputIO.SubmountPPVaccumNormally - 1] ==1? true : false);
            //IOManager.Instance.ChangeIOValue(_inputIOList[EnumBoardcardDefineInputIO.EutecticError], msg[(int)EnumBoardcardDefineInputIO.EutecticError - 1] ==1? true : false);
            //IOManager.Instance.ChangeIOValue(_inputIOList[EnumBoardcardDefineInputIO.EutecticComplete], msg[(int)EnumBoardcardDefineInputIO.EutecticComplete - 1] ==1? true : false);

            //IOManager.Instance.ChangeIOValue(_inputIOList[EnumBoardcardDefineInputIO.SafeDoorSensor1], msg[(int)EnumBoardcardDefineInputIO.SafeDoorSensor1 - 1] ==1? true : false);
            //IOManager.Instance.ChangeIOValue(_inputIOList[EnumBoardcardDefineInputIO.SafeDoorSensor2], msg[(int)EnumBoardcardDefineInputIO.SafeDoorSensor2 - 1] ==1? true : false);
            ////IOManager.Instance.ChangeIOValue(_IOList[EnumBoardcardDefineIO.EjectionSystemVaccumNormally], msg[(int)EnumBoardcardDefineIO.EjectionSystemVaccumNormally] ==1? true : false);
            ////IOManager.Instance.ChangeIOValue(_IOList[EnumBoardcardDefineIO.EutecticPlatformVaccumNormally], msg[(int)EnumBoardcardDefineIO.EutecticPlatformVaccumNormally] ==1? true : false);

            DataModel.Instance.ChipPPVaccumNormally = msg[(int)EnumBoardcardDefineInputIO.ChipPPVaccumNormally] == 1 ? true : false;
            //DataModel.Instance.SubmountPPVaccumNormally = msg[(int)EnumBoardcardDefineInputIO.SubmountPPVaccumNormally - 1] == 1 ? true : false;
            DataModel.Instance.EpoxtPON = msg[(int)EnumBoardcardDefineInputIO.EpoxtPON] == 1 ? true : false;
            DataModel.Instance.EpoxtDSO = msg[(int)EnumBoardcardDefineInputIO.EpoxtDSO] == 1 ? true : false;
            DataModel.Instance.EpoxtEND = msg[(int)EnumBoardcardDefineInputIO.EpoxtEND] == 1 ? true : false;
            DataModel.Instance.EpoxtERROR = msg[(int)EnumBoardcardDefineInputIO.EpoxtERROR] == 1 ? true : false;
            DataModel.Instance.EpoxtALARM = msg[(int)EnumBoardcardDefineInputIO.EpoxtALARM] == 1 ? true : false;
            DataModel.Instance.EpoxtALARM2 = msg[(int)EnumBoardcardDefineInputIO.EpoxtALARM2] == 1 ? true : false;
            DataModel.Instance.EpoxtRSM = msg[(int)EnumBoardcardDefineInputIO.EpoxtRSM] == 1 ? true : false;
            DataModel.Instance.EpoxtREADY = msg[(int)EnumBoardcardDefineInputIO.EpoxtREADY] == 1 ? true : false;

            DataModel.Instance.TransportInPlaceSignal1 = msg[(int)EnumBoardcardDefineInputIO.TransportInPlaceSignal1] == 1 ? true : false;
            DataModel.Instance.TransportInPlaceSignal2 = msg[(int)EnumBoardcardDefineInputIO.TransportInPlaceSignal2] == 1 ? true : false;
            DataModel.Instance.TransportInPlaceSignal3 = msg[(int)EnumBoardcardDefineInputIO.TransportInPlaceSignal3] == 1 ? true : false;
            //DataModel.Instance.EutecticError = msg[(int)EnumBoardcardDefineInputIO.EutecticError - 1] == 1 ? true : false;
            //DataModel.Instance.EutecticComplete = msg[(int)EnumBoardcardDefineInputIO.EutecticComplete - 1] == 1 ? true : false;

            DataModel.Instance.WaferInPlaceSignal1 = msg[(int)EnumBoardcardDefineInputIO.WaferInPlaceSignal1] == 1 ? true : false;

            DataModel.Instance.SafeDoorSensor1 = msg[(int)EnumBoardcardDefineInputIO.SafeDoorSensor1] == 1 ? true : false;
            DataModel.Instance.SafeDoorSensor2 = msg[(int)EnumBoardcardDefineInputIO.SafeDoorSensor2] == 1 ? true : false;


        }
        internal void ParseDataAndUpdateOutputIOValue(List<int> msg)
        {
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch], msg[(int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch-1] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.ChipPPBlowSwitch], msg[(int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch ] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch], msg[(int)EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch - 1] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch], msg[(int)EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch - 1] == 1 ? true : false);

            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.WaferTableVaccumSwitch], msg[(int)EnumBoardcardDefineOutputIO.WaferTableVaccumSwitch - 1] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.MaterialPlatformVaccumSwitch], msg[(int)EnumBoardcardDefineOutputIO.MaterialPlatformVaccumSwitch - 1] == 1 ? true : false); 
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch], msg[(int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch - 1] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch], msg[(int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch - 1] == 1 ? true : false);

            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.NitrogenValve], msg[(int)EnumBoardcardDefineOutputIO.NitrogenValve - 1] == 1 ? true : false);

            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.TowerRedLight], msg[(int)EnumBoardcardDefineOutputIO.TowerRedLight - 1] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.TowerYellowLight], msg[(int)EnumBoardcardDefineOutputIO.TowerYellowLight - 1] == 1 ? true : false);
            //IOManager.Instance.ChangeIOValue(_outputIOList[EnumBoardcardDefineOutputIO.TowerGreenLight], msg[(int)EnumBoardcardDefineOutputIO.TowerGreenLight - 1] == 1 ? true : false);

            DataModel.Instance.ChipPPVaccumSwitch = msg[(int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch] == 1 ? true : false;
            DataModel.Instance.ChipPPBlowSwitch = msg[(int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch] == 1 ? true : false;
            DataModel.Instance.EpoxtliftCylinder = msg[(int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder ] == 1 ? true : false;
            DataModel.Instance.EpoxtDIS = msg[(int)EnumBoardcardDefineOutputIO.EpoxtDIS ] == 1 ? true : false;
            DataModel.Instance.EpoxtENABLE = msg[(int)EnumBoardcardDefineOutputIO.EpoxtENABLE ] == 1 ? true : false;
            DataModel.Instance.EpoxtTMD = msg[(int)EnumBoardcardDefineOutputIO.EpoxtTMD ] == 1 ? true : false;
            DataModel.Instance.EpoxtTMS = msg[(int)EnumBoardcardDefineOutputIO.EpoxtTMS ] == 1 ? true : false;
            DataModel.Instance.EpoxtC_CNT = msg[(int)EnumBoardcardDefineOutputIO.EpoxtC_CNT ] == 1 ? true : false;
            DataModel.Instance.EpoxtRESET = msg[(int)EnumBoardcardDefineOutputIO.EpoxtRESET ] == 1 ? true : false;
            //DataModel.Instance.SubmountPPVaccumSwitch = msg[(int)EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch ] == 1 ? true : false;
            //DataModel.Instance.SubmountPPBlowSwitch = msg[(int)EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch ] == 1 ? true : false;

            DataModel.Instance.TransportCylinder1 = msg[(int)EnumBoardcardDefineOutputIO.TransportCylinder1 ] == 1 ? true : false;
            DataModel.Instance.TransportCylinder2 = msg[(int)EnumBoardcardDefineOutputIO.TransportCylinder2 ] == 1 ? true : false;
            DataModel.Instance.TransportVaccumSwitch1 = msg[(int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch1 ] == 1 ? true : false;
            DataModel.Instance.TransportVaccumSwitch2 = msg[(int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch2 ] == 1 ? true : false;
            //DataModel.Instance.EutecticPlatformVaccumSwitch = msg[(int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch ] == 1 ? true : false;
            //DataModel.Instance.NitrogenValve = msg[(int)EnumBoardcardDefineOutputIO.NitrogenValve ] == 1 ? true : false;
            //DataModel.Instance.StartEctectic = msg[(int)EnumBoardcardDefineOutputIO.StartEctectic ] == 1 ? true : false;
            //DataModel.Instance.ResetEutectic = msg[(int)EnumBoardcardDefineOutputIO.ResetEutectic ] == 1 ? true : false;

            
            DataModel.Instance.EjectionSystemVaccumSwitch = msg[(int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch ] == 1 ? true : false;
            DataModel.Instance.EjectionSystemBlowSwitch = msg[(int)EnumBoardcardDefineOutputIO.EjectionSystemBlowSwitch ] == 1 ? true : false;
            DataModel.Instance.WaferFingerCylinder = msg[(int)EnumBoardcardDefineOutputIO.WaferFingerCylinder ] == 1 ? true : false;
            DataModel.Instance.WaferClampCylinder = msg[(int)EnumBoardcardDefineOutputIO.WaferClampCylinder ] == 1 ? true : false;
            DataModel.Instance.WaferCassetteCylinder = msg[(int)EnumBoardcardDefineOutputIO.WaferCassetteCylinder ] == 1 ? true : false;
            DataModel.Instance.WaferTableVaccumSwitch = msg[(int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch ] == 1 ? true : false;
            //DataModel.Instance.MaterialPlatformVaccumSwitch = msg[(int)EnumBoardcardDefineOutputIO.TowerYellowLight ] == 1 ? true : false;

            DataModel.Instance.TowerRedLight = msg[(int)EnumBoardcardDefineOutputIO.TowerRedLight ] == 1 ? true : false;
            DataModel.Instance.TowerYellowLight = msg[(int)EnumBoardcardDefineOutputIO.TowerYellowLight ] == 1 ? true : false;
            DataModel.Instance.TowerGreenLight = msg[(int)EnumBoardcardDefineOutputIO.TowerGreenLight ] == 1 ? true : false;

            DataModel.Instance.WaferCassetteLiftMotorBrake = msg[(int)EnumBoardcardDefineOutputIO.WaferCassetteLiftMotorBrake ] == 1 ? true : false;
            DataModel.Instance.EjectionLiftMotorBrake = msg[(int)EnumBoardcardDefineOutputIO.EjectionLiftMotorBrake ] == 1 ? true : false;

        }
        internal void ReadAxisesPos()
        {
            foreach (EnumStageAxis axis in Enum.GetValues(typeof(EnumStageAxis)))
            {
                if (axis == EnumStageAxis.None)
                {
                    continue;
                }
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);

                if (_axisConfig?.RunningType == EnumRunningType.Actual)
                {
                    var pos = (float)_boardCardController.GetCurrentPosition(axis);
                    var sta = (int)_boardCardController.GetAxisState(axis);
                    //IOManager.Instance.ChangeIOValue("Stage.BondXSta", sta);
                    

                    if (axis == EnumStageAxis.BondX)
                    {
                        DataModel.Instance.BondX = pos;
                        DataModel.Instance.BondXSta = sta;
                    }
                    else if (axis == EnumStageAxis.BondY)
                    {
                        DataModel.Instance.BondY = pos;
                        DataModel.Instance.BondYSta = sta;
                    }
                    else if (axis == EnumStageAxis.BondZ)
                    {
                        DataModel.Instance.BondZ = pos;
                        DataModel.Instance.BondZSta = sta;
                    }
                    else if (axis == EnumStageAxis.ChipPPT)
                    {
                        DataModel.Instance.ChipPPT = pos;
                        DataModel.Instance.ChipPPTSta = sta;
                    }
                    else if (axis == EnumStageAxis.PPtoolBankTheta)
                    {
                        DataModel.Instance.PPtoolBankTheta = pos;
                        DataModel.Instance.PPtoolBankThetaSta = sta;
                    }
                    else if (axis == EnumStageAxis.DippingGlue)
                    {
                        DataModel.Instance.DippingGlue = pos;
                        DataModel.Instance.DippingGlueSta = sta;
                    }
                    else if (axis == EnumStageAxis.TransportTrack1)
                    {
                        DataModel.Instance.TransportTrack1 = pos;
                        DataModel.Instance.TransportTrack1Sta = sta;
                    }
                    else if (axis == EnumStageAxis.TransportTrack2)
                    {
                        DataModel.Instance.TransportTrack2 = pos;
                        DataModel.Instance.TransportTrack2Sta = sta;
                    }
                    else if (axis == EnumStageAxis.TransportTrack3)
                    {
                        DataModel.Instance.TransportTrack3 = pos;
                        DataModel.Instance.TransportTrack3Sta = sta;
                    }
                    else if (axis == EnumStageAxis.WaferTableX)
                    {
                        DataModel.Instance.WaferTableX = pos;
                        DataModel.Instance.WaferTableXSta = sta;
                    }
                    else if (axis == EnumStageAxis.WaferTableY)
                    {
                        DataModel.Instance.WaferTableY = pos;
                        DataModel.Instance.WaferTableYSta = sta;
                    }
                    else if (axis == EnumStageAxis.WaferTableZ)
                    {
                        DataModel.Instance.WaferTableZ = pos;
                        DataModel.Instance.WaferTableZSta = sta;
                    }
                    else if (axis == EnumStageAxis.WaferFilm)
                    {
                        DataModel.Instance.WaferFilm = pos;
                        DataModel.Instance.WaferFilmSta = sta;
                    }
                    else if (axis == EnumStageAxis.WaferFinger)
                    {
                        DataModel.Instance.WaferFinger = pos;
                        DataModel.Instance.WaferFingerSta = sta;
                    }
                    else if (axis == EnumStageAxis.WaferCassetteLift)
                    {
                        DataModel.Instance.WaferCassetteLift = pos;
                        DataModel.Instance.WaferCassetteLiftSta = sta;
                    }
                    else if (axis == EnumStageAxis.ESZ)
                    {
                        DataModel.Instance.ESZ = pos;
                        DataModel.Instance.ESZSta = sta;
                    }
                    else if (axis == EnumStageAxis.NeedleZ)
                    {
                        DataModel.Instance.NeedleZ = pos;
                        DataModel.Instance.NeedleZSta = sta;
                    }
                    else if (axis == EnumStageAxis.NeedleSwitch)
                    {
                        DataModel.Instance.NeedleSwitch = pos;
                        DataModel.Instance.NeedleSwitchSta = sta;
                    }
                    else if (axis == EnumStageAxis.FilpToolTheta)
                    {
                        DataModel.Instance.FilpToolTheta = pos;
                        DataModel.Instance.FilpToolThetaSta = sta;
                    }
                }

            }
            //var pos=(float)_boardCardController.GetCurrentPosition(EnumStageAxis.BondX);
            ////IOManager.Instance.ChangeIOValue("Stage.BondXPos", pos);
            //DataModel.Instance.BondX = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.BondY);
            ////IOManager.Instance.ChangeIOValue("Stage.BondYPos", pos);
            //DataModel.Instance.BondY = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.BondZ);
            ////IOManager.Instance.ChangeIOValue("Stage.BondZPos", pos);
            //DataModel.Instance.BondZ = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.ChipPPT);
            ////IOManager.Instance.ChangeIOValue("Stage.ChipTPos", pos);
            //DataModel.Instance.ChipPPT = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.PPtoolBankTheta);
            ////IOManager.Instance.ChangeIOValue("Stage.PPtoolBankTheta", pos);
            //DataModel.Instance.PPtoolBankTheta = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.DippingGlue);
            ////IOManager.Instance.ChangeIOValue("Stage.DippingGlue", pos);
            //DataModel.Instance.DippingGlue = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.TransportTrack1);
            ////IOManager.Instance.ChangeIOValue("Stage.TransportTrack1", pos);
            //DataModel.Instance.TransportTrack1 = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.TransportTrack2);
            ////IOManager.Instance.ChangeIOValue("Stage.TransportTrack2", pos);
            //DataModel.Instance.TransportTrack2 = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.TransportTrack3);
            ////IOManager.Instance.ChangeIOValue("Stage.TransportTrack3", pos);
            //DataModel.Instance.TransportTrack3 = pos;

            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.WaferTableX);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferTableXPos", pos);
            //DataModel.Instance.WaferTableX = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.WaferTableY);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferTableYPos", pos);
            //DataModel.Instance.WaferTableY = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.WaferTableZ);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferTableZPos", pos);
            //DataModel.Instance.WaferTableZ = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.WaferFilm);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferFilmPos", pos);
            //DataModel.Instance.WaferFilm = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.WaferFinger);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferFingerPos", pos);
            //DataModel.Instance.WaferFinger = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.WaferCassetteLift);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferCassetteLiftPos", pos);
            //DataModel.Instance.WaferCassetteLift = pos;

            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.ESZ);
            ////IOManager.Instance.ChangeIOValue("Stage.ESBaseZPos", pos);
            //DataModel.Instance.ESZ = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.NeedleZ);
            ////IOManager.Instance.ChangeIOValue("Stage.NeedleZPos", pos);
            //DataModel.Instance.NeedleZ = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.NeedleSwitch);
            ////IOManager.Instance.ChangeIOValue("Stage.NeedleSwitchPos", pos);
            //DataModel.Instance.NeedleSwitch = pos;
            //pos = (float)_boardCardController.GetCurrentPosition(EnumStageAxis.FilpToolTheta);
            ////IOManager.Instance.ChangeIOValue("Stage.FilpToolThetaPos", pos);
            //DataModel.Instance.FilpToolTheta = pos;


            //var sta = (int)_boardCardController.GetAxisState(EnumStageAxis.BondX);
            ////IOManager.Instance.ChangeIOValue("Stage.BondXSta", sta);
            //DataModel.Instance.BondXSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.BondY);
            ////IOManager.Instance.ChangeIOValue("Stage.BondYSta", sta);
            //DataModel.Instance.BondYSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.BondZ);
            ////IOManager.Instance.ChangeIOValue("Stage.BondZSta", sta);
            //DataModel.Instance.BondZSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.ChipPPT);
            ////IOManager.Instance.ChangeIOValue("Stage.ChipTSta", sta);
            //DataModel.Instance.ChipPPTSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.PPtoolBankTheta);
            ////IOManager.Instance.ChangeIOValue("Stage.PPtoolBankTheta", sta);
            //DataModel.Instance.PPtoolBankThetaSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.DippingGlue);
            ////IOManager.Instance.ChangeIOValue("Stage.DippingGlue", sta);
            //DataModel.Instance.DippingGlueSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.TransportTrack1);
            ////IOManager.Instance.ChangeIOValue("Stage.TransportTrack1", sta);
            //DataModel.Instance.TransportTrack1Sta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.TransportTrack2);
            ////IOManager.Instance.ChangeIOValue("Stage.TransportTrack2", sta);
            //DataModel.Instance.TransportTrack2Sta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.TransportTrack3);
            ////IOManager.Instance.ChangeIOValue("Stage.TransportTrack3", sta);
            //DataModel.Instance.TransportTrack3Sta = sta;

            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.WaferTableX);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferTableXSta", sta);
            //DataModel.Instance.WaferTableXSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.WaferTableY);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferTableYSta", sta);
            //DataModel.Instance.WaferTableYSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.WaferTableZ);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferTableZSta", sta);
            //DataModel.Instance.WaferTableZSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.WaferFilm);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferFilmSta", sta);
            //DataModel.Instance.WaferFilmSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.WaferFinger);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferFingerSta", sta);
            //DataModel.Instance.WaferFingerSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.WaferCassetteLift);
            ////IOManager.Instance.ChangeIOValue("Stage.WaferCassetteLiftSta", sta);
            //DataModel.Instance.WaferCassetteLiftSta = sta;

            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.ESZ);
            ////IOManager.Instance.ChangeIOValue("Stage.ESBaseZSta", sta);
            //DataModel.Instance.ESZSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.NeedleZ);
            ////IOManager.Instance.ChangeIOValue("Stage.NeedleZSta", sta);
            //DataModel.Instance.NeedleZSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.NeedleSwitch);
            ////IOManager.Instance.ChangeIOValue("Stage.NeedleSwitchSta", sta);
            //DataModel.Instance.NeedleSwitchSta = sta;
            //sta = (int)_boardCardController.GetAxisState(EnumStageAxis.FilpToolTheta);
            ////IOManager.Instance.ChangeIOValue("Stage.FilpToolThetaSta", sta);
            //DataModel.Instance.FilpToolThetaSta = sta;


        }


        private void ReadSerialPortTask1()
        {
            while (_enablePollingIO1)
            {
                Thread.Sleep(50);
                try
                {
                    ParseDataAndUpdateSerialPortLight();
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortLight,Error.",ex);
                }
                //try
                //{

                //    ParseDataAndUpdateSerialPortLaser();
                //}
                //catch (Exception ex)
                //{
                //    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortLaser,Error.", ex);
                //}
                //try
                //{
                //    ParseDataAndUpdateSerialPortDynameter();

                //}
                //catch (Exception ex)
                //{
                //    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortDynameter,Error.", ex);
                //}
                //try
                //{

                //    ParseDataAndUpdateSerialPortEpoxt();
                //}
                //catch (Exception ex)
                //{
                //    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortEpoxt,Error.", ex);
                //}




            }
        }

        private void ReadSerialPortTask2()
        {
            while (_enablePollingIO2)
            {
                Thread.Sleep(100);
                try
                {

                    ParseDataAndUpdateSerialPortLaser();
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortLaser,Error.", ex);
                }
            }
        }
        private void ReadSerialPortTask3()
        {
            while (_enablePollingIO3)
            {
                Thread.Sleep(100);
                try
                {
                    ParseDataAndUpdateSerialPortDynameter();

                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortDynameter,Error.", ex);
                }
            }
        }
        private void ReadSerialPortTask4()
        {
            while (_enablePollingIO4)
            {
                Thread.Sleep(100);
                try
                {

                    ParseDataAndUpdateSerialPortEpoxt();
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortEpoxt,Error.", ex);
                }
            }
        }
        private void ReadSerialPortTask5()
        {
            while (_enablePollingIO5)
            {
                Thread.Sleep(100);
                
            }
        }

        internal void ParseDataAndUpdateSerialPortLight()
        {
            if (_LightControllerManager.AllLights.Count > 0)
            {
                if (!DataModel.Instance.LightsIsWriting)
                {
                    DataModel.Instance.LightsIsReading = true;
                    bool BondDirectFieldconnect = true, BondRingFieldconnect = true, WaferDirectFieldconnect = true, WaferRingFieldconnect = true, UpLookingDirectFieldconnect = true, UpLookingRingFieldconnect = true;

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.BondRed = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        BondDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.BondGreen = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        BondDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.BondBlue = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        BondDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.BondRingField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.BondRing = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        BondRingFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.WaferRed = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        WaferDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.WaferGreen = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        WaferDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.WaferBlue = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        WaferDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferRingField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.WaferRing = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        WaferRingFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.UpLookingRed = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        UpLookingDirectFieldconnect = false;
                    }

                    if (_LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField).IsConnected)
                    {
                        int brightness = -1;
                        brightness = (int)_LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField).GetIntensity(_LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField).Channel);
                        if (brightness >= 0 && brightness < 256)
                        {
                            DataModel.Instance.UpLookingRing = brightness;
                        }


                        Thread.Sleep(100);
                    }
                    else
                    {
                        UpLookingRingFieldconnect = false;
                    }

                    if (BondDirectFieldconnect && BondRingFieldconnect
                        && WaferDirectFieldconnect && WaferRingFieldconnect
                        && UpLookingDirectFieldconnect && UpLookingRingFieldconnect)
                    {
                        DataModel.Instance.LightsIsconnect = true;
                    }
                    else
                    {
                        DataModel.Instance.LightsIsconnect = false;
                    }

                    DataModel.Instance.LightsIsReading = false;
                }


            }

        }

        internal void ParseDataAndUpdateSerialPortLaser()
        {

            if (_LaserSensorManager.GetCurrentHardware()!= null && _LaserSensorManager.GetCurrentHardware().IsConnect)
            {
                double distance = -1;
                distance = (double)_LaserSensorManager.GetCurrentHardware().ReadDistance();
                if (distance >= 0)
                {
                    DataModel.Instance.LaserValue = distance/10000.0f;
                }
                else
                {
                    DataModel.Instance.LaserValue = 0;
                }

                DataModel.Instance.LaserIsconnect = true;
                Thread.Sleep(50);
            }
            else
            {
                DataModel.Instance.LaserIsconnect = false;
            }

        }
        internal void ParseDataAndUpdateSerialPortDynameter()
        {

            if (_DynamometerManager.GetCurrentHardware() != null && _DynamometerManager.GetCurrentHardware().IsConnect)
            {
                double[] pressure;
                pressure = _DynamometerManager.GetCurrentHardware().ReadAllValue();
                if (pressure?.Length > 1)
                {
                    DataModel.Instance.PressureValue1 = pressure[0];
                    DataModel.Instance.PressureValue2 = pressure[1];
                }

                DataModel.Instance.PressureIsconnect = true;
                Thread.Sleep(50);
            }
            else
            {
                DataModel.Instance.PressureIsconnect = false;
            }

        }

        internal void ParseDataAndUpdateSerialPortEpoxt()
        {

            if (_DispensingMachineManager.GetCurrentHardware() != null && _DispensingMachineManager.GetCurrentHardware().IsConnect)
            {
                List<decimal> data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取供气压力);
                if(data?.Count > 0)
                {
                    DataModel.Instance.EpoxtCurrentPressure = (double)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取秒表);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtCurrentTime = (double)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取当前通道);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtCH = (int)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取吐出条件, DataModel.Instance.EpoxtCH.ToString());
                if (data?.Count > 4)
                {
                    DataModel.Instance.EpoxtMode = (int)data[1];
                    DataModel.Instance.EpoxtTime = (double)data[2];
                    DataModel.Instance.EpoxtPressure = (double)data[3];
                    DataModel.Instance.EpoxtVacuum = (double)data[4];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取计数器);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtSHOT = (int)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取Σ功能水头差自动校正);
                if (data?.Count > 1)
                {
                    DataModel.Instance.EpoxtMode2 = (int)data[1];
                }
                Thread.Sleep(20);
                DataModel.Instance.PressureIsconnect = true;
                Thread.Sleep(50);
            }
            else
            {
                DataModel.Instance.PressureIsconnect = false;
            }

        }


    }


}
