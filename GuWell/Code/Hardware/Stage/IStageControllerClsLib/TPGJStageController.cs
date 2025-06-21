using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WestDragon.Framework.UtilityHelper;

namespace StageControllerClsLib
{
    /// <summary>
    /// Stage控制器
    /// </summary>
    internal class TPGJStageController : IStageController
    {
        /*BondX
         * 





        */


        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        /// <summary>
        /// Stage的信息
        /// </summary>
        public AStageInfo StageInfo { get; set; }

        /// <summary>
        /// 多轴控制器
        /// </summary>
        private MultiAxisController _multiAxisController;

        /// <summary>
        /// Stage控制核心对象
        /// </summary>
        private StageCore StageCore = StageControllerClsLib.StageCore.Instance;

        /// <summary>
        /// 获取指定单轴
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public ISingleAxisController this[EnumStageAxis axis]
        {
            get
            {
                switch (axis)
                {
                    case EnumStageAxis.BondX:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as BondXSingleAxisController);
                    case EnumStageAxis.BondY:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as BondYSingleAxisController);
                    case EnumStageAxis.BondZ:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as BondZSingleAxisController);
                    case EnumStageAxis.ChipPPT:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as ChipPPTSingleAxisController);
                    case EnumStageAxis.PPtoolBankTheta:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as PPtoolBankThetaSingleAxisController);
                    case EnumStageAxis.DippingGlue:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as DippingGlueSingleAxisController);
                    case EnumStageAxis.TransportTrack1:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as TransportTrack1SingleAxisController);
                    case EnumStageAxis.TransportTrack2:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as TransportTrack2SingleAxisController);
                    case EnumStageAxis.TransportTrack3:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as TransportTrack3SingleAxisController);
                    case EnumStageAxis.WaferTableX:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as WaferTableXSingleAxisController);
                    case EnumStageAxis.WaferTableY:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as WaferTableYSingleAxisController);
                    case EnumStageAxis.WaferTableZ:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as WaferTableZSingleAxisController);
                    case EnumStageAxis.WaferFilm:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as WaferFilmSingleAxisController);
                    case EnumStageAxis.WaferFinger:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as WaferFingerSingleAxisController);
                    case EnumStageAxis.WaferCassetteLift:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as WaferCassetteLiftSingleAxisController);
                    case EnumStageAxis.ESZ:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as ESZSingleAxisController);
                    case EnumStageAxis.NeedleZ:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as NeedleZSingleAxisController);
                    case EnumStageAxis.NeedleSwitch:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as NeedleSwitchSingleAxisController);
                    case EnumStageAxis.FilpToolTheta:
                        return (StageCore.StageInfo.AxisControllerDic[axis] as FilpToolThetaSingleAxisController);
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// 获取指定多轴
        /// </summary>
        /// <param name="axises"></param>
        /// <returns></returns>
        public IMultiAxisController this[params EnumStageAxis[] axises]
        {
            get
            {
                _multiAxisController.Axises = axises;
                return _multiAxisController;
            }
        }

        /// <summary>
        /// 获取是否已连接
        /// </summary>
        public bool IsConnect
        {
            get { return StageCore.IsConnect; }
        }
        private bool _homeDoneFlag = false;
        public bool IsHomedone { get { return _homeDoneFlag; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TPGJStageController()
        {
            _multiAxisController = new MultiAxisController();
            _multiAxisController.StageCore = StageCore;
        }

        /// <summary>
        /// 初始化各轴的限位和速度
        /// </summary>
        public void InitialzeAllAxisParameter()
        {
            foreach(EnumStageAxis axis in Enum.GetValues(typeof(EnumStageAxis)))
            {
                if(axis == EnumStageAxis.None)
                {
                    continue;
                }
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                
                if (_axisConfig?.RunningType == EnumRunningType.Actual)
                {
                    this[axis].StageCore = StageCore;
                    this[axis].Enable();

                    if(axis == EnumStageAxis.ESZ || axis == EnumStageAxis.NeedleZ || axis == EnumStageAxis.WaferTableX || axis == EnumStageAxis.WaferTableY || axis == EnumStageAxis.WaferTableZ)
                    {
                        //this[axis].Home();
                    }

                    //if (axis == EnumStageAxis.WaferTableX || axis == EnumStageAxis.WaferTableY || axis == EnumStageAxis.WaferTableZ)
                    //{
                    //    this[axis].Home();
                    //}
                    this[axis].SetAxisMotionParameters(_axisConfig);
                    if (axis == EnumStageAxis.BondX || axis == EnumStageAxis.BondY || axis == EnumStageAxis.BondZ)
                    {
                        this[axis].SetSoftLeftAndRightLimit(_axisConfig.SoftRightLimit, _axisConfig.SoftLeftLimit);
                    }
                    //this[axis].SetSoftLeftAndRightLimit(_axisConfig.SoftRightLimit, _axisConfig.SoftLeftLimit);
                    this[axis].SetAxisErrPosBind();
                }
                
            }


            //this[EnumStageAxis.BondX].StageCore = StageCore;
            //this[EnumStageAxis.BondY].StageCore = StageCore;
            //this[EnumStageAxis.BondZ].StageCore = StageCore;
            //this[EnumStageAxis.ChipPPT].StageCore = StageCore;
            //this[EnumStageAxis.PPtoolBankTheta].StageCore = StageCore;
            //this[EnumStageAxis.DippingGlue].StageCore = StageCore;
            //this[EnumStageAxis.TransportTrack1].StageCore = StageCore;
            //this[EnumStageAxis.TransportTrack2].StageCore = StageCore;
            //this[EnumStageAxis.TransportTrack3].StageCore = StageCore;
            //this[EnumStageAxis.WaferTableX].StageCore = StageCore;
            //this[EnumStageAxis.WaferTableY].StageCore = StageCore;
            //this[EnumStageAxis.WaferTableZ].StageCore = StageCore;
            //this[EnumStageAxis.WaferFilm].StageCore = StageCore;
            //this[EnumStageAxis.WaferFinger].StageCore = StageCore;
            //this[EnumStageAxis.WaferCassetteLift].StageCore = StageCore;
            //this[EnumStageAxis.ESZ].StageCore = StageCore;
            //this[EnumStageAxis.NeedleZ].StageCore = StageCore;
            //this[EnumStageAxis.NeedleSwitch].StageCore = StageCore;
            //this[EnumStageAxis.FilpToolTheta].StageCore = StageCore;

            //this[EnumStageAxis.BondX].Enable();
            //this[EnumStageAxis.BondY].Enable();
            //this[EnumStageAxis.BondZ].Enable();
            //this[EnumStageAxis.ChipPPT].Enable();
            //this[EnumStageAxis.PPtoolBankTheta].Enable();
            //this[EnumStageAxis.DippingGlue].Enable();
            //this[EnumStageAxis.TransportTrack1].Enable();
            //this[EnumStageAxis.TransportTrack2].Enable();
            //this[EnumStageAxis.TransportTrack3].Enable();

            //this[EnumStageAxis.WaferTableX].Enable();
            //this[EnumStageAxis.WaferTableY].Enable();
            //this[EnumStageAxis.WaferTableZ].Enable();
            //this[EnumStageAxis.WaferTableZ].Home();

            //this[EnumStageAxis.WaferFilm].Enable();
            //this[EnumStageAxis.WaferFinger].Enable();
            //this[EnumStageAxis.WaferCassetteLift].Enable();

            //this[EnumStageAxis.ESZ].Enable();
            //this[EnumStageAxis.ESZ].Home();
            //this[EnumStageAxis.NeedleZ].Enable();
            //this[EnumStageAxis.NeedleZ].Home();

            //this[EnumStageAxis.NeedleSwitch].Enable();
            //this[EnumStageAxis.FilpToolTheta].Enable();

            //AxisConfig axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.BondX);
            //this[EnumStageAxis.BondX].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.BondX].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.BondX].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.BondY);
            //this[EnumStageAxis.BondY].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.BondY].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.BondY].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.BondZ);
            //this[EnumStageAxis.BondZ].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.BondZ].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.BondZ].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.ChipPPT);
            //this[EnumStageAxis.ChipPPT].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.ChipPPT].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.ChipPPT].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.PPtoolBankTheta);
            //this[EnumStageAxis.PPtoolBankTheta].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.PPtoolBankTheta].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.PPtoolBankTheta].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.DippingGlue);
            //this[EnumStageAxis.DippingGlue].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.DippingGlue].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.DippingGlue].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.TransportTrack1);
            //this[EnumStageAxis.TransportTrack1].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.TransportTrack1].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.TransportTrack1].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.TransportTrack2);
            //this[EnumStageAxis.TransportTrack2].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.TransportTrack2].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.TransportTrack2].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.TransportTrack3);
            //this[EnumStageAxis.TransportTrack3].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.TransportTrack3].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.TransportTrack3].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferTableX);
            //this[EnumStageAxis.WaferTableX].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.WaferTableX].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.WaferTableX].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferTableY);
            //this[EnumStageAxis.WaferTableY].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.WaferTableY].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.WaferTableY].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferTableZ);
            //this[EnumStageAxis.WaferTableZ].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.WaferTableZ].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.WaferTableZ].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferFilm);
            //this[EnumStageAxis.WaferFilm].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.WaferFilm].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.WaferFilm].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferFinger);
            //this[EnumStageAxis.WaferFinger].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.WaferFinger].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.WaferFinger].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferCassetteLift);
            //this[EnumStageAxis.WaferCassetteLift].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.WaferCassetteLift].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.WaferCassetteLift].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.ESZ);
            //this[EnumStageAxis.ESZ].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.ESZ].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.ESZ].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.NeedleZ);
            //this[EnumStageAxis.NeedleZ].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.NeedleZ].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.NeedleZ].SetAxisErrPosBind();

            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.NeedleSwitch);
            //this[EnumStageAxis.NeedleSwitch].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.NeedleSwitch].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.NeedleSwitch].SetAxisErrPosBind();
            //axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.FilpToolTheta);
            //this[EnumStageAxis.FilpToolTheta].SetAxisMotionParameters(axisConfig);
            //this[EnumStageAxis.FilpToolTheta].SetSoftLeftAndRightLimit(axisConfig.SoftRightLimit, axisConfig.SoftLeftLimit);
            //this[EnumStageAxis.FilpToolTheta].SetAxisErrPosBind();

        }

        /// <summary>
        /// 连接到控制器
        /// </summary>
        public void Connect()
        {
            StageCore.Connect();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {

            StageCore.Disconnect();
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// 执行Stage的Home操作
        /// </summary>
        public void Home()
        {
            foreach (EnumStageAxis axis in Enum.GetValues(typeof(EnumStageAxis)))
            {
                if (axis == EnumStageAxis.None)
                {
                    continue;
                }


                if (axis == EnumStageAxis.WaferTableX || axis == EnumStageAxis.WaferTableY || axis == EnumStageAxis.WaferTableZ || axis == EnumStageAxis.ESZ || axis == EnumStageAxis.NeedleZ)
                {
                    this[axis].Home();
                }

            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumVacuum"></param>
        public void OnVacuum(EnumVacuum enumVacuum)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumVacuum"></param>
        public void OffVacuum(EnumVacuum enumVacuum)
        {

        }




        public bool IsHomeDone()
        {
           return StageCore.IsHomedone;
        }

        public bool CheckHomeDone()
        {
            return true;
        }
    }
    public class TPGJStageInfo : AStageInfo
    {
        /// <summary>
        /// 控制器远程IP地址
        /// </summary>
        public string RemoteAddress { get; set; }

        /// <summary>
        /// Theta是否有限制360度移动
        /// </summary>
        public bool ThetaLimit { get; set; }
    }


}
