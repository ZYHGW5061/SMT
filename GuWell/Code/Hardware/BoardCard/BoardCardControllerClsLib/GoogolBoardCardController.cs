using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ConfigurationClsLib;

namespace BoardCardControllerClsLib
{
    public class GoogolBoardCardController : IBoardCardController
    {
        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        public bool IsConnect => throw new NotImplementedException();
        const short CORE = 1;
        short EcatSts;
        static bool[] AxisClsStat = new bool[20];
        #region 开卡连接
        /// <summary>
        /// 开卡链接
        /// </summary>
        public void Connect()
        {
            var err = GTN.mc.GTN_Open(5, 2);
           
            StartEcatCommunication();
            err = GTN.mc.GTN_IsEcatReady(CORE, out EcatSts);
         
            while (EcatSts != 1)
            {
                Thread.Sleep(1000);
                err = GTN.mc.GTN_IsEcatReady(CORE, out EcatSts);
               
            }
        }
        #endregion 开卡连接

        #region 读连接状态
        public bool ReadConnect()
        {
            short Num = -1, Num2 = -1;
            var err = GTN.mc.GTN_GetEcatSlaves(CORE, out Num, out Num2);
            if(Num < 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        #endregion
        #region 开启总线通讯
        private void StartEcatCommunication()
        {
            short Success;
            try
            {
                var rtn = GTN.mc.GTN_TerminateEcatComm(CORE);
               
                rtn = GTN.mc.GTN_InitEcatComm(CORE);
              
                if (rtn != 0) { return; }
                Thread.Sleep(1000);
                do
                {
                    rtn = GTN.mc.GTN_IsEcatReady(CORE, out Success);
                  
                    Thread.Sleep(10);
                } while (Success == 0);
                rtn = GTN.mc.GTN_StartEcatComm(CORE);
                rtn = GTN.mc.GTN_Reset(CORE);
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion

        #region 关闭总线连接
        /// <summary>
        /// 关闭总线连接
        /// </summary>
        public void Disconnect()
        {
            CloseIOs();
            GTN.mc.GTN_TerminateEcatComm(CORE);
            GTN.mc.GTN_Close();
        }
        private void CloseIOs()
        {
            try
            {
                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, 0);

                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, 0);

                //IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.SubmountPPVaccumSwitch, 0);

                //IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.SubmountPPBlowSwitch, 0);

                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, 0);

                //IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch, 0);

                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 0);

                //IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.NitrogenValve, 0);

                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 0);
                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, 0);
                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                //IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.EutecticPlatformVaccumSwitch, 0);
                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 0);
                IO_WriteOutPut(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch1, 0);
            }
            catch(Exception ex)
            {

            }
        }
        /// <summary>
        /// 获取正极限状态
        /// </summary>
        public bool Get_SoftLimit_AxisStsPositive(EnumStageAxis axis)
        {
           return  AxisControl.mc.SoftLimit_AxisStsPositive((short)axis);
        }
        #endregion 关闭总线连接

        #region  获取负极限状态
        /// <summary>
        /// 获取负极限状态
        /// </summary>
        public bool Get_SoftLimit_AxisStsNegative(EnumStageAxis axis)
        {

            return AxisControl.mc.SoftLimit_AxisStsNegative((short)axis);

        }
        #endregion

        #region 获取轴使能状态
        /// <summary>
        /// 获取轴使能状态
        /// </summary>
        public bool Get_AxisSts_Enable(EnumStageAxis axis)
        {

            return AxisControl.mc.AxisSts_Enable((short)axis);

        }
        ///
        public bool Get_ALLAxisSts_Enable(int  axiscount)
        {

            return AxisControl.mc.AreAllAxesEnabled(axiscount);

        }
        #endregion


        #region  获取轴正忙
        /// <summary>
        /// 获取轴正忙
        /// </summary>
        public bool Get_AxisSts_Busy(EnumStageAxis axis)
        {
            return AxisControl.mc.AxisSts_Busy((short)axis);
        }




        /// <summary>
        /// 获取轴正忙
        /// </summary>
        public bool Get_AxisSts_Busy(EnumStageAxis axis,out short  err)
        {
            return AxisControl.mc.AxisSts_Busy((short)axis, out err);
        }
        #endregion

        #region  获取轴位置规划器状态
        /// <summary>
        /// 获取轴位置状态
        /// </summary>
        public bool Get_AxisSts_PosDone(EnumStageAxis axis)
        {
            //return AxisControl.mc.AxisSts_PosDone((short)axis);
            if(axis == EnumStageAxis.BondZ)
            {
                return AxisControl.mc.AxisSts_PosDone((short)axis);
            }
            int err = 0;
            return AxisControl.mc.AxisSts_PosReach((short)axis, out err);

        }

        #endregion

        #region  设置轴误差带
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="core"></param>
        /// <param name="band"></单位为脉冲param>
        /// <param name="time"></单位250USparam>
        /// <param name="err"></param>
        public void SetAxisErrPosBind(EnumStageAxis axis, out int err, int band = 10, int time = 10)
        {
            AxisControl.mc.SetAxisErrPosBind(Convert.ToInt16(axis), CORE, band, time, out err);

        }
        #endregion

        #region  获取轴误差带
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="core"></param>
        /// <param name="band"></单位为脉冲param>
        /// <param name="time"></单位250USparam>
        /// <param name="err"></param>
        public void GetAxisErrPosBind(EnumStageAxis axis, short core, out int band, out int time, out int err)
        {
            AxisControl.mc.GetAxisErrPosBind(Convert.ToInt16(axis), CORE, out band, out time, out err);

        }
        #endregion
        #region 电机到达设定误差带，使用前提是必须开始误差带监测功能
        /// <summary>
        /// 电机到达设定误差带，使用前提是必须开始误差带监测功能
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool AxisSts_PosReach(EnumStageAxis axis, out int err)
        {
            return AxisControl.mc.AxisSts_PosReach(Convert.ToInt16(axis), out err);
        }
        #endregion

        #region  获取回归原点成功状态
        /// <summary>
        /// 获取回归原点成功状态
        /// </summary>
        public bool Get_AxisSts_HomeDone(EnumStageAxis axis)
        {
            return AxisControl.mc.AxisSts_HomeDone((short)axis);

        }
        #endregion


        #region  获取轴错误状态
        /// <summary>
        /// 获取轴错误状态
        /// </summary>
        public bool Get_AxisEncArr(EnumStageAxis axis)
        {
            return AxisControl.mc.AxisEncArr((short)axis);
        }
        #endregion



        #region  电机模组配置参数初始化
        /// <summary>
        /// 电机模组配置参数初始化
        /// </summary>
        public void MotioParaInit()
        {
           // MotorPara.AxisMotionPara[1].EactID = 1;
           // MotorPara.AxisMotionPara[1].DynamicsParaIn.acc = 1;
           // MotorPara.AxisMotionPara[1].DynamicsParaIn.dec = 1;
           // MotorPara.AxisMotionPara[1].DynamicsParaIn.smoothTime = 20;
           // MotorPara.AxisMotionPara[1].DynamicsParaIn.smooth = 0.5;
           // MotorPara.AxisMotionPara[1].DynamicsParaIn.velStart = 10;
           // MotorPara.AxisMotionPara[1].DynamicsParaIn.circlePulse = 10000;
           // MotorPara.AxisMotionPara[1].AXISModule.lead = 1;
           // MotorPara.AxisMotionPara[2].EactID = 2;
           // MotorPara.AxisMotionPara[2].DynamicsParaIn.acc = 1;
           // MotorPara.AxisMotionPara[2].DynamicsParaIn.dec = 1;
           // MotorPara.AxisMotionPara[2].DynamicsParaIn.smoothTime = 20;
           // MotorPara.AxisMotionPara[2].DynamicsParaIn.smooth = 0.5;
           // MotorPara.AxisMotionPara[2].DynamicsParaIn.velStart = 10;
           // MotorPara.AxisMotionPara[2].DynamicsParaIn.circlePulse = 10000;
           // MotorPara.AxisMotionPara[2].AXISModule.lead = 1;
           //MotorPara.AxisMotionPara[3].EactID = 3;
           // MotorPara.AxisMotionPara[3].DynamicsParaIn.acc = 1;
           // MotorPara.AxisMotionPara[3].DynamicsParaIn.dec = 1;
           // MotorPara.AxisMotionPara[3].DynamicsParaIn.smoothTime = 20;
           // MotorPara.AxisMotionPara[3].DynamicsParaIn.smooth = 0.5;
           // MotorPara.AxisMotionPara[3].DynamicsParaIn.velStart = 10;
           // MotorPara.AxisMotionPara[3].DynamicsParaIn.circlePulse = 10000;
           // MotorPara.AxisMotionPara[3].AXISModule.lead = 1;



        }
        #endregion

        #region  电机模组配置参数初始化
        /// <summary>
        /// 电机模组配置参数设置
        /// </summary>
        /// <param name="axisParam"></param>
        public void SetAxisMotionParameters(AxisConfig axisParam)
        {
            var id = axisParam.Index;
            MotorPara.AxisMotionPara[id].EactID = (short)id;
            MotorPara.AxisMotionPara[id].DynamicsParaIn.acc = axisParam.Acceleration;
            MotorPara.AxisMotionPara[id].DynamicsParaIn.dec = axisParam.Deceleration;
            MotorPara.AxisMotionPara[id].DynamicsParaIn.smoothTime = (short)axisParam.SmoothTime;
            MotorPara.AxisMotionPara[id].DynamicsParaIn.smooth = axisParam.Smooth;
            //MotorPara.AxisMotionPara[id].DynamicsParaIn.velStart = axisParam.AxisSpeed;
            MotorPara.AxisMotionPara[id].DynamicsParaIn.circlePulse = axisParam.CirclePulse;
            MotorPara.AxisMotionPara[id].AXISModule.lead = axisParam.Lead;
            MotorPara.AxisMotionPara[id].DynamicsParaIn.velStart = (float)(axisParam.AxisSpeed *(axisParam.CirclePulse/ axisParam.Lead) / 1000);
        }
        #endregion


        #region 轴组使能
        /// <summary>
        /// 轴组使能
        /// </summary>
        public void Enable(EnumStageAxis axis)  //轴组使能
        {
            int pos;
            GTN.mc.GTN_AxisOn(CORE, (short)axis);

             GTN.mc.GTN_GetEcatEncPos(1, (short)axis, out pos);
           
            GTN.mc.GTN_SetPrfPos(1, (short)axis, pos);
            GTN.mc.GTN_SetEncPos(1, (short)axis, pos);
            GTN.mc.GTN_SynchAxisPos(1, 1 << (short)axis - 1);

        }
        #endregion

        #region 轴组下使能
        /// <summary>
        /// 轴组下使能
        /// </summary>
        public void Disable(EnumStageAxis axis)  //轴组下使能后无保持力
        {
            GTN.mc.GTN_AxisOff(CORE, (short)axis);
        }
      
        /// <summary>
        /// 轴组下使能
        /// </summary>
        public void Disable(EnumStageAxis axis,out short err)  //轴组下使能后无保持力
        {
            err= GTN.mc.GTN_AxisOff(CORE, (short)axis);
        }
        #endregion


        #region 获取轴加速度
        /// <summary>
        /// 获取轴加速度
        /// </summary>
        public double GetAcceleration(EnumStageAxis axis)
        {
            int OriPosPulse = 0;
            OriPosPulse = AxisControl.mc.MC_GetPrfAcc((short)axis);
            return (double)OriPosPulse * PulseToMM(axis) * 1000;
        }
        #endregion
        #region 获取轴速度
        /// <summary>
        /// 获取轴速度
        /// </summary>
        public double GetAxisSpeed(EnumStageAxis axis)
        {
            int OriPosPulse = 0;
            OriPosPulse =AxisControl.mc.MC_GetPrfVel((short)axis);
            return (double)OriPosPulse * PulseToMM(axis)*1000;
        }
        #endregion

        #region 获取轴当前位置
        /// <summary>
        /// 获取轴当前位置
        /// </summary>
        public double GetCurrentPosition(EnumStageAxis axis)
        {
            double OriPosPulse = 0;
            OriPosPulse =  AxisControl.mc. MC_GetEncPos((short) axis);
            return (double)OriPosPulse * PulseToMM(axis);

        }
        #endregion

        #region 获取轴减速度
        /// <summary>
        /// 获取轴减速度
        /// </summary>
        public double GetDeceleration(EnumStageAxis axis)
        {
            int OriPosPulse = 0;
            OriPosPulse = AxisControl.mc.MC_GetPrfAcc((short)axis);
            return (double)OriPosPulse * PulseToMM(axis) * 1000;
        }
#endregion

        public double GetKillDeceleration(EnumStageAxis axis)
        {
            throw new NotImplementedException();
        }

        public double GetMaxAcceleration(EnumStageAxis axis)
        {
            throw new NotImplementedException();
        }

        public double GetMaxAxisSpeed(EnumStageAxis axis)
        {
            throw new NotImplementedException();
        }

        #region 获取轴正向限位
        /// <summary>
        /// 获取轴正向限位
        /// </summary>
        public double GetSoftLeftLimit(EnumStageAxis axis)
        {
            int OriPosPulse = 0;
            OriPosPulse = AxisControl.mc.MC_GetSoftLimitNegative((short)axis);
            return (double)OriPosPulse * PulseToMM(axis);
        }
        #endregion

        #region 获取轴反向限位
        /// <summary>
        /// 获取轴反向限位
        /// </summary>
        public double GetSoftRightLimit(EnumStageAxis axis)
        {
            int OriPosPulse = 0;
            OriPosPulse = AxisControl.mc.MC_GetSoftLimitPositive((short)axis);
            return (double)OriPosPulse * PulseToMM(axis);
        }
        #endregion

        #region 设置轴回原点模式
        /// <summary>
        /// 设置轴当前位置为原点
        /// </summary>
        public void Home(EnumStageAxis axis)
        {
            ClrAlarm(axis);
            if (axis == EnumStageAxis.WaferTableY || axis == EnumStageAxis.WaferTableX)
            {
                int pos = 0;
                AxisControl.mc.MC_Home((short)axis);
                GTN.mc.GTN_GetEcatEncPos(1, (short)axis, out pos);

                GTN.mc.GTN_SetPrfPos(1, (short)axis, pos);
                GTN.mc.GTN_SetEncPos(1, (short)axis, pos);
                GTN.mc.GTN_SynchAxisPos(1, 1 << (short)axis - 1);

            }
            else if(axis == EnumStageAxis.SubmountPPT|| axis == EnumStageAxis.SubmountPPZ || axis == EnumStageAxis.ESZ || axis == EnumStageAxis.NeedleZ || axis == EnumStageAxis.WaferTableZ)
            {
                int pos = 0;
                AxisControl.mc.Step_Go_Home((short)axis);
                GTN.mc.GTN_GetEcatEncPos(1, (short)axis, out pos);

                GTN.mc.GTN_SetPrfPos(1, (short)axis, pos);
                GTN.mc.GTN_SetEncPos(1, (short)axis, pos);
                GTN.mc.GTN_SynchAxisPos(1, 1 << (short)axis - 1);
            }
           
        }
        /// <summary>
        /// 回原点，并指定回原点的方法
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="homemode"></param>
        /// <returns></returns>
        public bool Home(EnumStageAxis axis ,short homemode)
        {
            bool Done = false;
            ClrAlarm(axis);
            int pos = 0;
            Done = AxisControl.mc.MC_Home((short)axis, homemode);
 
            if (!Done)
            {
                return false;
            }
            GTN.mc.GTN_GetEcatEncPos(1, (short)axis, out pos);

            GTN.mc.GTN_SetPrfPos(1, (short)axis, pos);
            GTN.mc.GTN_SetEncPos(1, (short)axis, pos);
            GTN.mc.GTN_SynchAxisPos(1, 1 << (short)axis - 1);
            return true;

        }
        #endregion

        #region 设置轴反向点动
        /// <summary>
        /// 设置轴反向点动
        /// </summary>
        public void JogNegative(EnumStageAxis axis, float speed)
        {
            if (axis == EnumStageAxis.BondZ || axis == EnumStageAxis.NeedleZ)
            {
                speed = -speed;
            }
            ClrAlarm(axis);
            //speed= (float)(speed*MMToPulse(axis)/1000);
            //MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = -Math.Abs(speed);
            //AxisControl.mc.MC_MoveJog(MotorPara.AxisMotionPara[(int)axis].EactID, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc,
            //                          MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smooth,
            //                          MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart);
            speed = (float)(speed * MMToPulse(axis) / 1000);
            //MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = -Math.Abs(speed);
            AxisControl.mc.MC_MoveJog(MotorPara.AxisMotionPara[(int)axis].EactID, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc,
                                      MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smooth,
                                      -speed);
        }
        #endregion

        #region  设置轴正向点动
        /// <summary>
        /// 设置轴正向点动
        /// </summary>
        public void JogPositive(EnumStageAxis axis, float speed)
        {
            if (axis == EnumStageAxis.BondZ|| axis==EnumStageAxis.NeedleZ)
            {
                speed = -speed;
            }
            //speed = (float)(speed * MMToPulse(axis) / 1000);
            //MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = speed;
            //AxisControl.mc.MC_MoveJog(MotorPara.AxisMotionPara[(int)axis].EactID, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc , 
            //                          MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smooth,
            //                          MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart);
            ClrAlarm(axis);
            speed = (float)(speed * MMToPulse(axis) / 1000);
            //MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = speed;
            AxisControl.mc.MC_MoveJog(MotorPara.AxisMotionPara[(int)axis].EactID, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc,
                                      MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smooth,
                                      speed);
        }
        #endregion

        #region 设置轴点动
        /// <summary>
        /// 设置轴点动
        /// </summary>
        public void JogMove(EnumStageAxis axis, double speed,double  acc,out short err)
        {
            //speed = (float)(speed * MMToPulse(axis) / 1000);
            //MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = speed;
            //AxisControl.mc.MC_MoveJog(MotorPara.AxisMotionPara[(int)axis].EactID, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc , 
            //                          MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smooth,
            //                          MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart);
            ClrAlarm(axis);
            speed = (speed * MMToPulse(axis) / 1000);
            acc = (acc * MMToPulse(axis) / 1000);
            //MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = speed;
            err = AxisControl.mc.MC_MoveJog(MotorPara.AxisMotionPara[(int)axis].EactID, acc,
                                      acc, MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smooth,
                                      speed);
        }
        #endregion


        #region 绝对运动
        /// <summary>
        /// 绝对运动（同步方式）
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Trap">运动参数</param>
        /// <param name="Vel">速度</param>
        /// <param name="Pos">位置</param>
        public void MoveAbsoluteSync(EnumStageAxis axis, double targetPos, double Speed, int millisecondsTimeout = -1)
        {
            ClrAlarm(axis);
            if (axis == EnumStageAxis.BondY)
            {
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                S_Movetion(axis, targetPos, _axisConfig.AxisSpeed, 10000, 50000);
            }
            else if (axis == EnumStageAxis.BondX)
            {
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                S_Movetion(axis, targetPos, _axisConfig.AxisSpeed, 10000, 50000);
            }
            else if (axis == EnumStageAxis.BondZ)
            {
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                S_Movetion(axis, targetPos, _axisConfig.AxisSpeed, 2500, 25000);
            }
            //else if (axis == EnumStageAxis.ChipPPT)
            //{
            //    S_Movetion(axis, targetPos, 20, 2500, 25000);
            //}
            else
            {
                Speed = Speed * MMToPulse(axis) / 1000;
                targetPos = targetPos * MMToPulse(axis);
                AxisControl.mc.MC_MoveAbsolute(MotorPara.AxisMotionPara[(int)axis].EactID,
                                               MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc,
                                              MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec,
                                            //Speed,
                                            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart,
                                            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smoothTime,
                                            (int)targetPos);
            }
        }

        public void MoveAbsoluteSync(EnumStageAxis axis, double targetPos, double Speed, double acc, out short err)
        {
            ClrAlarm(axis);
            Speed = Speed * MMToPulse(axis) / 1000;
            targetPos = targetPos * MMToPulse(axis);
            acc = acc * MMToPulse(axis)/1000;
            err =  AxisControl.mc.MC_MoveAbsolute(MotorPara.AxisMotionPara[(int)axis].EactID,acc, acc, Speed, 
                                                  MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smoothTime, (int)targetPos);

        }
        #endregion
        #region 相对运动
        /// <summary>
        /// 相对运动（同步方式）
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Trap">运动参数</param>
        /// <param name="Vel">速度</param>
        /// <param name="Pos">位置</param>
        public void MoveRelativeSync(EnumStageAxis axis, double distance, double Speed, int millisecondsTimeout = -1)
        {
            if(axis == EnumStageAxis.BondZ)
            {
                distance = -distance;
            }

            ClrAlarm(axis);

            

            if (axis == EnumStageAxis.BondY)
            {
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                double targetPos = GetCurrentPosition(axis) + distance;
                S_Movetion(axis, targetPos, _axisConfig.AxisSpeed, 10000, 50000);
            }
            else if (axis == EnumStageAxis.BondX)
            {
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                double targetPos = GetCurrentPosition(axis) + distance;
                S_Movetion(axis, targetPos, _axisConfig.AxisSpeed, 10000, 50000);
            }
            else if (axis == EnumStageAxis.BondZ)
            {
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);
                double targetPos = GetCurrentPosition(axis) + distance;
                S_Movetion(axis, targetPos, 5, 2500, 2500);
            }
            else
            {
                Speed = Speed * MMToPulse(axis) / 1000;
                distance = distance * MMToPulse(axis);
                AxisControl.mc.MC_MoveRelative(MotorPara.AxisMotionPara[(int)axis].EactID,
                                               MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc,
                                              MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec,
                                             //Speed,
                                             MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart,
                                            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smoothTime,
                                            (int)distance);
            }

                
        }


        public void MoveRelativeSync(EnumStageAxis axis, double distance, double Speed,double acc, out short err)
        {
            ClrAlarm(axis);
            Speed = Speed * MMToPulse(axis) / 1000;
            distance = distance * MMToPulse(axis);
            acc = acc * MMToPulse(axis);
            err = AxisControl.mc.MC_MoveRelative(MotorPara.AxisMotionPara[(int)axis].EactID, acc, acc, Speed,
                                                    MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.smoothTime, (int)distance);

        }
        #endregion
        #region 设置轴加速度
        /// <summary>
        /// 设置轴加速度
        /// </summary>
        public void SetAcceleration(EnumStageAxis axis, double acceleration)
        {
            acceleration = acceleration * MMToPulse(axis) / 1000;
            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.acc = acceleration;
        }
        #endregion

        #region 设置轴速度
        /// <summary>
        /// 设置轴速度
        /// </summary>
        public void SetAxisSpeed(EnumStageAxis axis, double speed)
        {
            speed = speed * MMToPulse(axis) / 1000;
            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.velStart = speed;
        }
        #endregion
        #region 设置轴减速度
        /// <summary>
        /// 设置轴减速度
        /// </summary>
        public void SetDeceleration(EnumStageAxis axis, double deceleration)
        {
            deceleration = deceleration * MMToPulse(axis) / 1000;
            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.dec = deceleration;
        }
#endregion
        public void SetJERK(EnumStageAxis axis, double value)
        {
            throw new NotImplementedException();
        }

        public void SetKillDeceleration(EnumStageAxis axis, double deceleration)
        {
            throw new NotImplementedException();
        }

        public void SetMaxAcceleration(EnumStageAxis axis, double value)
        {
            throw new NotImplementedException();
        }

        public void SetMaxAxisSpeed(EnumStageAxis axis, double value)
        {
            throw new NotImplementedException();
        }
        #region 设置轴正负软限位
        /// <summary>
        /// 设置轴正负软限位
        /// </summary>
        public void SetSoftLeftAndRightLimit(EnumStageAxis axis, double Pvalue, double Nvalue)
        {
            Pvalue= Pvalue * MMToPulse(axis);
            Nvalue = Nvalue * MMToPulse(axis);
            AxisControl.mc.MC_SetSoftLimitNegativeAndPostive((short)axis, (Int32)Pvalue, (Int32)Nvalue);

        }
        #endregion
        #region 取消轴正负软限位
        /// <summary>
        /// 取消轴正负软限位
        /// </summary>
        public void CloseSoftLeftAndRightLimit(EnumStageAxis axis)
        {
            AxisControl.mc.CloseSoftLimit((short)axis);


        }
        #endregion

        #region 停止反向点动
        /// <summary>
        /// 停止反向点动
        /// </summary>
        /// <param name="axis"></param>
        public void StopJogNegative(EnumStageAxis axis)
        {
            GTN.mc.GTN_Stop(CORE, 1 << (short)axis - 1, 1 << (short)axis - 1);
        }
        #endregion
        #region 停止正向点动
        /// <summary>
        /// 停止正向点动
        /// </summary>
        /// <param name="axis"></param>
        public void StopJogPositive(EnumStageAxis axis)
        {
            GTN.mc.GTN_Stop(CORE, 1 << (short)axis - 1, 1 << (short)axis - 1);
        }
        #endregion
        #region 轴停止命令
        /// <summary>
        /// 轴停止命令
        /// </summary>
        public void StopMotion(EnumStageAxis axis)
        {
            AxisControl.mc.Axis_Stop((short)axis);


        }
        public void StopMotion(EnumStageAxis axis ,out short err)
        {
            AxisControl.mc.Axis_Stop((short)axis,out err);


        }

        #endregion
        #region 清除轴错误状态
        /// <summary>
        /// 清除轴错误状态
        /// </summary>
        public void Axis_ClearStatus(EnumStageAxis axis)
        {

            AxisControl.mc.Axis_ClearSt((short)axis);
        }
        #endregion
        #region 关闭板卡
        /// <summary>
        /// 关闭板卡
        /// </summary>
        public void BoardCardClose()
        {
            AxisControl.mc.ConCardClose();


        }

        /// <summary>
        /// 关闭板卡
        /// </summary>
        public void BoardCardClose(out short err)
        {
            AxisControl.mc.ConCardClose(out err);


        }
        #endregion
        #region 板卡复位
        /// <summary>
        /// 板卡复位
        /// </summary>
        public void BoardCardReset()
        {
            AxisControl.mc.CardReset();
        }

        #endregion

        # region 将脉冲单位转换为mm
        //将脉冲单位转换为mm
        public double PulseToMM(EnumStageAxis axis)
        {

            return (double)(AxisControl.mc.Int2DoubleDivide(MotorPara.AxisMotionPara[(int)axis].AXISModule.lead,
                                                            MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.circlePulse));

        
        }
        #endregion
        #region 将毫米转换成脉冲单位
        //将毫米转换成脉冲单位
        public double MMToPulse(EnumStageAxis axis)
        {
            return (MotorPara.AxisMotionPara[(int)axis].DynamicsParaIn.circlePulse 
                    / MotorPara.AxisMotionPara[(int)axis].AXISModule.lead);
        
        
        }
        #endregion
        #region  Ethercat远程IO输出设置 number（1-16） Value 1真 0假
        /// <summary>
        /// 远程IO输出设置 number（1-16） Value 1真 0假
        /// </summary>
        public void IO_WriteOutPut(ushort EcatID, short number, int  Value)
        {
            Thread.Sleep(10);
            AxisControl.IOFun.IO_WriteOutPut( EcatID,  number,  Value);


        }
        #endregion
        #region  Glink远程IO输出设置 number（1-32） Value 1真 0假
        /// <summary>
        /// 远程IO输出设置 number（1-16） Value 1真 0假
        /// </summary>
        public void IO_WriteOutPut_2(ushort slaveno, short doIndex, int Value)
        {
            Thread.Sleep(10);
            if(doIndex<16)
            {
                slaveno = 0;
            }
            else if(doIndex<32)
            {
                slaveno = 1;
                doIndex = (short)(doIndex - 16);
            }
            AxisControl.IOFun.IO_WriteOutPut_2(slaveno, doIndex, Value);


        }
        #endregion

        #region Ethercat输入IO读取
        /// <summary>
        /// 远程IO输入设置 number（1-16） Value 1真 0假
        /// </summary>
        public void IO_ReadInput(ushort EcatID, int ioIndex, out int Value)
        {
            AxisControl.IOFun.IO_ReadInput(EcatID, ioIndex, out  Value);

        }
#endregion
        #region Ethercat输出IO读取
        public void IO_ReadOutput(ushort EcatID, int ioIndex, out int Value)
        {
            AxisControl.IOFun.IO_ReadOutput(EcatID, ioIndex, out Value);

        }
#endregion
        #region Ethercat输入IO读取
        /// <summary>
        /// 远程数字量IO输入设置 number（1-16） Value 1真 0假
        /// </summary>
        public void IO_ReadInput_D(ushort EcatID, int offset, out int Value)
        {
            AxisControl.IOFun.IO_ReadInput_D(EcatID, (ushort)offset, out Value);

        }
#endregion
        #region Ethercat输出IO读取
        public void IO_ReadOutput_D(ushort EcatID, int offset, out int Value)
        {
            AxisControl.IOFun.IO_ReadOutput_D(EcatID, (ushort)offset, out Value);

        }
#endregion

        #region 读取Glink模拟输入量
        public void IO_ReadInput_A(ushort slaveno, ushort offset, out int Value)
        {
            AxisControl.IOFun.IO_ReadInput_A(slaveno, (ushort)offset, out Value);

        }
#endregion
        #region 读取Glink模拟输出量
        public void IO_ReadOutput_A(ushort slaveno, ushort offset, out int Value)
        {
            AxisControl.IOFun.IO_ReadOutput_A(slaveno, (ushort)offset, out Value);

        }
#endregion
        #region 读取Ethercat输入IO状态，OUT list输出数组
        public void IO_ReadAllInput(ushort EcatID, out List<int> Value)
        {
            AxisControl.IOFun.IO_ReadAllInput(EcatID, out Value);
        }
        #endregion
        #region 读取Ethercat输出IO状态，OUT list输出数组
        public void IO_ReadAllOutput(ushort EcatID, out List<int> Value)
        {
            AxisControl.IOFun.IO_ReadAllOutput(EcatID, out Value);
        }
#endregion
        #region 读取Glink输入IO状态，OUT list输出数组
        public void IO_ReadAllInput_2(ushort EcatID, out List<int> Value)
        {
            List<int> Value1, Value2;
            AxisControl.IOFun.IO_ReadAllInput_2(0, out Value1);
            AxisControl.IOFun.IO_ReadAllInput_2(1, out Value2);
            Value = Value1.Concat(Value2).ToList<int>();
        }
#endregion
        #region 读取Glink输出IO状态，OUT list输出数组
        public void IO_ReadAllOutput_2(ushort EcatID, out List<int> Value)
        {
            List<int> Value1, Value2;
            AxisControl.IOFun.IO_ReadAllOutput_2(0, out Value1);
            AxisControl.IOFun.IO_ReadAllOutput_2(1, out Value2);
            Value = Value1.Concat(Value2).ToList<int>();
        }
#endregion
        #region 读取Glink连接状态
        /// <summary>
        /// 读取Glink连接状态
        /// </summary>
        /// <returns></returns>
        public bool IO_GetGLinkCommStatus()
        {
            return AxisControl.IOFun.MC_GetGLinkCommStatus();
        }
        #endregion

        #region  获取轴的当前状态
        /// <summary>
        /// 读取轴状态
        /// 1 报警
        /// 5 正限位
        /// 6 负限位 
        /// 7 平滑停止 
        /// 8 急停 
        /// 9 使能 
        /// 10 规划运动 
        /// 11 电机到位
        /// </summary>
        public int GetAxisState(EnumStageAxis axis)
        {
            int stats = 0;
            uint clk;
            
            GTN.mc.GTN_GetSts(CORE, Convert.ToInt16(axis), out stats, 1, out clk);
            return stats;
        }


        #endregion

        #region  报警清除
        /// <summary>
        /// 报警清除
        /// </summary>
        public void ClrAlarm(EnumStageAxis axis)
        {
            GTN.mc.GTN_ClrSts(CORE, Convert.ToInt16(axis), 1);
        }
        #endregion

        #region 清除所有轴报警
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        public void ALLAxisClrAlarm(short axiscount, out short  err)
        {
            err = GTN.mc.GTN_ClrSts(CORE, axiscount, 1);
        }
        #endregion


        #region 关闭所有轴报警
        /// <summary>
        /// 报警 / 限位无效
        /// action动作 ： 1 为生效，0为失效
        /// </summary>
        public void DisableAlarmLimit(EnumStageAxis axis)
        {
            if (axis == EnumStageAxis.None)
            {
                return;
            }

            if (AxisClsStat[(int)axis] == true)
            {
                GTN.mc.GTN_AlarmOff(CORE, Convert.ToInt16(axis));
                GTN.mc.GTN_LmtsOff(CORE, Convert.ToInt16(axis), -1);
                GTN.mc.GTN_ClrSts(CORE, Convert.ToInt16(axis), 1);
            }
            else if (AxisClsStat[(int)axis] == false)
            {
                GTN.mc.GTN_AlarmOn(CORE, Convert.ToInt16(axis));
                GTN.mc.GTN_LmtsOn(CORE, Convert.ToInt16(axis), -1);
            }

            AxisClsStat[(int)axis] = !AxisClsStat[(int)axis];
        }
        #endregion
        #region Glink的初始化操作，默认退出软件后保持输出
        /// <summary>
        /// 
        /// </summary>
        public void IO_Init()
        {
            AxisControl.IOFun.IO_Init();
        }
        #endregion

        #region Glink的初始化操作，输出状态可选
        /// <summary>
        /// IO初始化 IOmode 1 代表跟随线程刷新， 3代表退出软件后保持IO输出
        /// </summary>
        public void IO_Init(short IOMode)
        {
            AxisControl.IOFun.IO_Init(IOMode);
        }
        #endregion
        #region Glink获取输入状态 int型输出
        public void IO_ReadInput_2(ushort slaveno, int doIndex, out int Value)
        {
            if (doIndex < 16)
            {
                slaveno = 0;
            }
            else if (doIndex < 32)
            {
                slaveno = 1;
                doIndex = (short)(doIndex - 16);
            }
            AxisControl.IOFun.IO_ReadInput_2(slaveno, doIndex, out Value);
        }
        public short GlinkIO_ReadInput( int doIndex, out int Value)
        {
            ushort slaveno =0;
            short err = 0;
            if (doIndex < 16)
            {
                slaveno = 0;
            }
            else if (doIndex < 32)
            {
                slaveno = 1;
                doIndex = (short)(doIndex - 16);
            }
            else if (doIndex < 48)
            {
                slaveno = 2;
                doIndex = (short)(doIndex - 32);
            }
            else if (doIndex < 64)
            {
                slaveno = 1;
                doIndex = (short)(doIndex - 48);
            }

            err= AxisControl.IOFun.IO_ReadInput_2(slaveno, doIndex, out Value);
            return err;
        }
        #endregion
        #region Glink获取输出状态 int型输出
        public void IO_ReadOutput_2(ushort slaveno, int doIndex, out int Value)
        {
            if (doIndex < 16)
            {
                slaveno = 0;
            }
            else if (doIndex < 32)
            {
                slaveno = 1;
                doIndex = (short)(doIndex - 16);
            }
            AxisControl.IOFun.IO_ReadOutput_2(slaveno, doIndex, out Value);
        }
        public void IO_ReadOutput_2(ushort slaveno, out int Value)
        {
            
            AxisControl.IOFun.IO_ReadOutput_2(slaveno, out Value);
        }



        #endregion





        #region  S型速度规划的实现

        public void S_Movetion(EnumStageAxis axis,double TargetPos)
        {
            double pPos;
            uint clk;
            int mask;
            GTN.mc.GTN_GetEncPos(1, Convert.ToInt16(axis), out pPos, 1, out clk);//获取当前位置


            double q0 = pPos;
            double q1 = TargetPos *10000;  //默认转换关系为1mm对应10000脉冲
            double v0 = 0;
            double v1 = 0;
            double vmax = 200*10000;
            double amax = 5000*10000;
            double jmax = 50000*10000;
            var (q, qd, time, numPoints) = AxisControl.AxisPvt.GenerateTrajectory(q0, q1, v0, v1, vmax, amax, jmax); //执行规划，并生成规划数组

            double[] S_pos = new double[numPoints];
            double[] S_vel = new double[numPoints];
            double[] S_time = new double[numPoints];
            Array.Copy(q, S_pos, numPoints);
            Array.Copy(qd, S_vel, numPoints);
            Array.Copy(time, S_time, numPoints);
            if (numPoints > 0)
            {
                S_pos[numPoints-1] = TargetPos * 10000;


            }
            // 对 S_vel 的每个元素除以 1000  ,转换成ms单位
            for (int i = 0; i < numPoints; i++)
            {
                S_vel[i] /= 1000; // 除以 1000  
            }

            // 对 S_time 的每个元素乘以 1000   转化成毫秒单位
            for (int i = 0; i < numPoints; i++)
            {
                S_time[i] *= 1000; // 乘以 1000  
            }
            GTN.mc.GTN_PrfPvt(1, Convert.ToInt16(axis));//设置此轴为PVT模式
            GTN.mc.GTN_PvtTable(1, 1, numPoints,ref  S_time[0],ref  S_pos[0], ref S_vel[0]); //压入数据
            GTN.mc.GTN_PvtTableSelect(1, Convert.ToInt16(axis), 1);
            mask = 1 << (Convert.ToInt16(axis) -1);
            GTN.mc.GTN_PvtStart(1, mask);

        }


        public void S_Movetion(EnumStageAxis axis, double TargetPos, double s_v, double s_a, double s_j)
        {
            double pPos;
            uint clk;
            int mask;
            GTN.mc.GTN_GetEncPos(1, Convert.ToInt16(axis), out pPos, 1, out clk);//获取当前位置


            double q0 = pPos;
            double q1 = TargetPos * 10000;  //默认转换关系为1mm对应10000脉冲 丝杠导程为5
            double v0 = 0;
            double v1 = 0;
            // double vmax = 200 * 10000;
            //  double amax = 5000 * 10000;
            //  double jmax = 2500 * 10000;

            double vmax = s_v * 10000;
            double amax = s_a * 10000;
            double jmax = s_j * 10000;
            if (!q0.Equals(q1))
            {
                var (q, qd, time, numPoints) = AxisControl.AxisPvt.GenerateTrajectory(q0, q1, v0, v1, vmax, amax, jmax, 0.005); //执行规划，并生成规划数组
                if (numPoints > 1000)
                {

                    (q, qd, time, numPoints) = AxisControl.AxisPvt.GenerateTrajectory(q0, q1, v0, v1, vmax, amax, jmax, 0.01); //执行规划，并生成规划数组

                }
                if (numPoints > 1000)
                {

                    (q, qd, time, numPoints) = AxisControl.AxisPvt.GenerateTrajectory(q0, q1, v0, v1, vmax, amax, jmax, 0.1); //执行规划，并生成规划数组

                }
                if (numPoints > 1000)
                {

                    (q, qd, time, numPoints) = AxisControl.AxisPvt.GenerateTrajectory(q0, q1, v0, v1, vmax, amax, jmax, 0.5); //执行规划，并生成规划数组

                }


                double[] S_pos = new double[numPoints];
                double[] S_vel = new double[numPoints];
                double[] S_time = new double[numPoints];
                if (numPoints > 1)
                {
                    Array.Copy(q, S_pos, numPoints);
                    Array.Copy(qd, S_vel, numPoints);
                    Array.Copy(time, S_time, numPoints);
                    // 对 S_vel 的每个元素除以 1000  ,转换成ms单位
                    if (numPoints > 0)
                    {
                        S_pos[numPoints - 1] = q1;


                    }
                    for (int i = 0; i < numPoints; i++)
                    {
                        S_vel[i] /= 1000; // 除以 1000  
                    }

                    // 对 S_time 的每个元素乘以 1000   转化成毫秒单位
                    for (int i = 0; i < numPoints; i++)
                    {
                        S_time[i] *= 1000; // 乘以 1000  
                    }
                    GTN.mc.GTN_PrfPvt(1, Convert.ToInt16(axis));//设置此轴为PVT模式
                    GTN.mc.GTN_PvtTable(1, Convert.ToInt16(axis), numPoints, ref S_time[0], ref S_pos[0], ref S_vel[0]); //压入数据
                    GTN.mc.GTN_PvtTableSelect(1, Convert.ToInt16(axis), Convert.ToInt16(axis));
                    mask = 1 << (Convert.ToInt16(axis) - 1);
                    GTN.mc.GTN_PvtStart(1, mask);
                }
            }

        }


        #endregion


        #region  XY的直线插补


        public  bool Set2DCoordinate(double synVelMax, double synAccMax, short profile1, short profile2, int originPos1, int originPos2)
        {

           return   AxisControl.Interpolation.Set2DCoordinate(synVelMax, synAccMax, profile1, profile2, originPos1, originPos2, out short error);

        }

        public  bool ClearCrdFifo(short core, short crd, short fifonum)
        {

            return AxisControl.Interpolation.ClearCrdFifo( core,  crd,  fifonum);



        }
        public  bool LnXYDataWrite(short crd, Int32 x, Int32 y, double synVel, double synAcc)
        {

            return AxisControl.Interpolation.LnXYDataWrite( crd,  x,  y,  synVel,  synAcc);
        }




        public  void GetCrdSpace(short crd, out Int32 pSpace)

        {
             AxisControl.Interpolation.GetCrdSpace( crd, out   pSpace);


        }


        public  bool StartCrdMove()

        {
           return  AxisControl.Interpolation.StartCrdMove();


        }


        public  void GetCrdStatus(short crd, out short sts, out int Finsh)
        {
            AxisControl.Interpolation.GetCrdStatus( crd, out  sts, out  Finsh);

        }

        #endregion
    }
}
