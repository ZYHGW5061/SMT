using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using HardwareManagerClsLib;
using JoyStickControllerClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PositioningSystemClsLib
{
    public class JoyStickControl
    {
        private static volatile JoyStickControl _instance = new JoyStickControl();
        private static readonly object _lockObj = new object();
        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static JoyStickControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new JoyStickControl();
                        }
                    }
                }
                return _instance;
            }
        }

        

        bool HeadMode = true;// true 机头模式 false 传送模式

        JoyStickController _joystickOBJ = null;
        int Mode = 1;//轴组
        bool YGMode = false;//摇杆
        EnumStageAxis Axis1 = EnumStageAxis.None;//摇杆水平轴号
        EnumStageAxis Axis2 = EnumStageAxis.None;//摇杆垂直轴号
        public float Lowspeed = 3;//低速
        public float Highspeed = 20;//高速

        public float[] Speeds = new float[20];

        private HardwareConfiguration _hardwareConfig { get { return HardwareConfiguration.Instance; } }
        protected PositioningSystem _positionSystem
        {
            get
            {
                return PositioningSystem.Instance;
            }
        }


        private JoyStickControl()
        {
            _joystickOBJ = (JoyStickController)HardwareManager.Instance.JobStick;


            _joystickOBJ.JoystickMoveEvent += _joystickOBJ_JoystickMoveEvent;
            _joystickOBJ.JoystickButtonEvent += _joystickOBJ_JoystickButtonEvent;

            AxisConfig axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.BondX);
            Speeds[(int)EnumStageAxis.BondX] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.BondY);
            Speeds[(int)EnumStageAxis.BondY] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.BondZ);
            Speeds[(int)EnumStageAxis.BondZ] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.ChipPPT);
            Speeds[(int)EnumStageAxis.ChipPPT] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.PPtoolBankTheta);
            Speeds[(int)EnumStageAxis.PPtoolBankTheta] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.DippingGlue);
            Speeds[(int)EnumStageAxis.DippingGlue] = (float)axisConfig.MediumAxisSpeed;

            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.TransportTrack1);
            Speeds[(int)EnumStageAxis.TransportTrack1] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.TransportTrack2);
            Speeds[(int)EnumStageAxis.TransportTrack2] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.TransportTrack3);
            Speeds[(int)EnumStageAxis.TransportTrack3] = (float)axisConfig.MediumAxisSpeed;

            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferTableX);
            Speeds[(int)EnumStageAxis.WaferTableX] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferTableY);
            Speeds[(int)EnumStageAxis.WaferTableY] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferTableZ);
            Speeds[(int)EnumStageAxis.WaferTableZ] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferFilm);
            Speeds[(int)EnumStageAxis.WaferFilm] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferFinger);
            Speeds[(int)EnumStageAxis.WaferFinger] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.WaferCassetteLift);
            Speeds[(int)EnumStageAxis.WaferCassetteLift] = (float)axisConfig.MediumAxisSpeed;

            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.ESZ);
            Speeds[(int)EnumStageAxis.ESZ] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.NeedleZ);
            Speeds[(int)EnumStageAxis.NeedleZ] = (float)axisConfig.MediumAxisSpeed;

            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.NeedleSwitch);
            Speeds[(int)EnumStageAxis.NeedleSwitch] = (float)axisConfig.MediumAxisSpeed;
            axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(EnumStageAxis.FilpToolTheta);
            Speeds[(int)EnumStageAxis.FilpToolTheta] = (float)axisConfig.MediumAxisSpeed;
        }

        public void JoyStickEnable(bool Enable)
        {
            _joystickOBJ.HandleEnable(Enable);

            foreach (EnumStageAxis axis in Enum.GetValues(typeof(EnumStageAxis)))
            {
                if(axis != EnumStageAxis.None)
                {
                    _positionSystem.StopJogPositive(axis);
                    Thread.Sleep(3);
                    _positionSystem.StopJogNegative(axis);
                    Thread.Sleep(3);
                }
                
            }
        }


        /// <summary>
        /// 手柄摇杆响应事件
        /// </summary>
        /// <param name="JoyX">左右</param>
        /// <param name="JoyY">上下</param>
        private void _joystickOBJ_JoystickMoveEvent(int JoyX, int JoyY)
        {
            try
            {


                if (YGMode == false)
                {
                    float ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * 30);
                    float ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * 30);
                    if ((int)Axis1 > -1 && (int)Axis1 < 12)
                    {
                        bool directionX = false;// false反转 true正转
                        switch (Axis1)
                        {
                            case EnumStageAxis.BondX:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.BondX]);
                                directionX = true;
                                break;
                            case EnumStageAxis.ChipPPT:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.ChipPPT]);
                                directionX = true;
                                break;
                            case EnumStageAxis.PPtoolBankTheta:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.PPtoolBankTheta]);
                                directionX = false;
                                break;
                            case EnumStageAxis.DippingGlue:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.DippingGlue]);
                                directionX = false;
                                break;
                            case EnumStageAxis.TransportTrack1:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.TransportTrack1]);
                                directionX = false;
                                break;
                            case EnumStageAxis.TransportTrack2:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.TransportTrack2]);
                                directionX = false;
                                break;
                            case EnumStageAxis.TransportTrack3:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.TransportTrack3]);
                                directionX = false;
                                break;
                            case EnumStageAxis.WaferTableX:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.WaferTableX]);
                                directionX = false;
                                break;
                            case EnumStageAxis.WaferFilm:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.WaferFilm]);
                                directionX = false;
                                break;
                            case EnumStageAxis.WaferFinger:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.WaferFinger]);
                                directionX = false;
                                break;
                            case EnumStageAxis.NeedleSwitch:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.NeedleSwitch]);
                                directionX = false;
                                break;
                            case EnumStageAxis.FilpToolTheta:
                                ValueX = Math.Abs((float)(JoyX - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.FilpToolTheta]);
                                directionX = false;
                                break;
                        }



                        if (JoyX < 31500)
                        {
                            if (directionX)
                            {
                                _positionSystem.JogPositive(Axis1, ValueX);
                            }
                            else
                            {
                                _positionSystem.JogNegative(Axis1, ValueX);
                            }
                        }
                        if (JoyX >= 31500 && JoyX <= 34034)
                        {
                            _positionSystem.StopJogPositive(Axis1);
                            Thread.Sleep(3);
                            _positionSystem.StopJogNegative(Axis1);
                        }
                        if (JoyX > 34034)
                        {
                            if (directionX)
                            {
                                _positionSystem.JogNegative(Axis1, ValueX);
                            }
                            else
                            {
                                _positionSystem.JogPositive(Axis1, ValueX);
                            }
                        }
                    }

                    if ((int)Axis2 > -1 && (int)Axis2 < 19)
                    {
                        bool directionY = false;// false反转 true正转
                        switch (Axis2)
                        {
                            case EnumStageAxis.BondY:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.BondY]);
                                directionY = true;
                                break;
                            case EnumStageAxis.BondZ:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.BondZ]);
                                directionY = false;
                                break;
                            case EnumStageAxis.WaferTableY:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.WaferTableY]);
                                directionY = true;
                                break;
                            case EnumStageAxis.WaferTableZ:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.WaferTableZ]);
                                directionY = true;
                                break;
                            case EnumStageAxis.WaferCassetteLift:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.WaferCassetteLift]);
                                directionY = true;
                                break;
                            case EnumStageAxis.ESZ:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.ESZ]);
                                directionY = true;
                                break;
                            case EnumStageAxis.NeedleZ:
                                ValueY = Math.Abs((float)(JoyY - 32767.0f) / 32767.0f * Speeds[(int)EnumStageAxis.NeedleZ]);
                                directionY = false;
                                break;
                        }

                        if (JoyY < 31500)
                        {
                            if (directionY)
                            {
                                _positionSystem.JogPositive(Axis2, ValueY);
                            }
                            else
                            {
                                _positionSystem.JogNegative(Axis2, ValueY);
                            }
                        }
                        if (JoyY >= 31500 && JoyY <= 34034)
                        {
                            _positionSystem.StopJogPositive(Axis2);
                            Thread.Sleep(3);
                            _positionSystem.StopJogNegative(Axis2);
                        }
                        if (JoyY > 34034)
                        {
                            if (directionY)
                            {
                                _positionSystem.JogNegative(Axis2, ValueY);
                            }
                            else
                            {
                                _positionSystem.JogPositive(Axis2, ValueY);
                            }

                        }
                    }
                }
                else
                {
                    if (JoyX >= 31500 && JoyX <= 34034)
                    {
                        _positionSystem.StopJogPositive(Axis1);
                        Thread.Sleep(3);
                        _positionSystem.StopJogNegative(Axis1);
                    }


                    if (JoyY >= 31500 && JoyY <= 34034)
                    {
                        _positionSystem.StopJogPositive(Axis2);
                        Thread.Sleep(3);
                        _positionSystem.StopJogNegative(Axis2);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 手柄按键响应事件
        /// </summary>
        /// <param name="KeyNum">1-12</param>
        private void _joystickOBJ_JoystickButtonEvent(int KeyNum)
        {
            try
            {

                //模式切换
                if (KeyNum > -1 && KeyNum < 11)
                {
                    Mode = KeyNum;

                    if (HeadMode)
                    {
                        switch (Mode)
                        {
                            case (int)Axisgroup.BondTableXY:
                                YGMode = false;
                                Axis1 = EnumStageAxis.BondX;
                                Axis2 = EnumStageAxis.BondY;
                                //stageSystem = EnumStageSystem.ShellTable;
                                //systemAxis = EnumSystemAxis.XY;
                                break;
                            case (int)Axisgroup.BondTableZ:
                                YGMode = false;
                                Axis1 = EnumStageAxis.SubmountPPZ;
                                Axis2 = EnumStageAxis.BondZ;
                                //stageSystem = EnumStageSystem.LidTable;
                                //systemAxis = EnumSystemAxis.XY;
                                break;
                            case (int)Axisgroup.WaferTableXY:
                                YGMode = false;
                                Axis1 = EnumStageAxis.WaferTableX;
                                Axis2 = EnumStageAxis.WaferTableY;
                                //stageSystem = EnumStageSystem.PP;
                                //systemAxis = EnumSystemAxis.YZ;
                                break;
                            case (int)Axisgroup.WaferTableZ:
                                YGMode = false;
                                Axis1 = EnumStageAxis.WaferTableZ;
                                Axis2 = EnumStageAxis.WaferTableZ;
                                //stageSystem = EnumStageSystem.Head;
                                //systemAxis = EnumSystemAxis.XZ;
                                break;
                            case (int)Axisgroup.Thera:
                                YGMode = false;
                                Axis1 = EnumStageAxis.ChipPPT;
                                Axis2 = EnumStageAxis.SubmountPPT;
                                //stageSystem = EnumStageSystem.None;
                                ////systemAxis = EnumSystemAxis.Z;
                                break;
                            case (int)Axisgroup.NeedleZ:
                                YGMode = false;
                                Axis1 = EnumStageAxis.ESZ;
                                Axis2 = EnumStageAxis.NeedleZ;
                                //stageSystem = EnumStageSystem.None;
                                ////systemAxis = EnumSystemAxis.XY;
                                break;
                            case 0:

                                //HC.手动_轴点动反转停止(Axis1);
                                //Thread.Sleep(10);
                                //HC.手动_轴点动正转停止(Axis1);
                                //Thread.Sleep(10);
                                //HC.手动_轴点动反转停止(Axis2);
                                //Thread.Sleep(10);
                                //HC.手动_轴点动正转停止(Axis2);

                                YGMode = true;
                                //Axis1 = (int)-1;
                                //Axis2 = (int)-1;
                                break;
                        }

                    }
                }


            }
            catch (Exception ex)
            {

            }
        }


    }

    public enum Axisgroup : int
    {
        /// <summary>
        /// (榜头X,榜头Y)。
        /// </summary>
        BondTableXY = 1,
        /// <summary>
        /// (榜头Z,基板吸嘴Z)。
        /// </summary>
        BondTableZ = 2,
        /// <summary>
        /// (晶圆盘X,晶圆盘Y)。
        /// </summary>
        WaferTableXY = 3,
        /// <summary>
        /// (晶圆相机Z)。
        /// </summary>
        WaferTableZ = 4,
        /// <summary>
        /// (芯片吸嘴Z,基地吸嘴Z)。
        /// </summary>
        Thera = 5,
        /// <summary>
        /// (料盒钩爪X,料盒钩爪Z)。
        /// </summary>
        NeedleZ = 6,

    }

}
