﻿using BoardCardControllerClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;


namespace StageControllerClsLib
{
    public class StageCore
    {
        #region 单例模式
        private static volatile StageCore _instance;
        private static readonly object _lockObj = new object();
        private bool _enablePollingAxis;
        public static StageCore Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new StageCore();
                        }
                    }

                }
                return _instance;
            }
        }
        private StageCore()
        {

        }
        #endregion
        const short CORE = 1;
        short EcatSts;
        public AStageInfo StageInfo { get; set; }
        public bool IsConnect { get; set; }
        public bool IsHomedone { get; set; }
        IBoardCardController _boardCardController;
        public void AbloluteMoveSync(EnumStageAxis axis, double target)
        {
            if(_boardCardController != null)
            {
                _boardCardController.MoveAbsoluteSync(axis, target,10);
                WaitAbsoluteMoveDone(axis);
            }
        }

        public void AbloluteMoveSync(EnumStageAxis[] axises, double[] target)
        {
            if (_boardCardController != null)
            {
                for (int i = 0; i < axises.Length; i++)
                {
                    _boardCardController.MoveAbsoluteSync(axises[i], target[i], 10);
                }
                for (int i = 0; i < axises.Length; i++)
                {
                    WaitAbsoluteMoveDone(axises[i]);
                }
            }
        }

        public void Connect()
        {
            BoardCardManager.Instance.Initialize();

            _boardCardController = BoardCardManager.Instance.GetCurrentController();
        }

        public void Disconnect()
        {
            if (_boardCardController != null)
            {
                _boardCardController.Disconnect();
            }
        }

        public float GetAxisSpeed(EnumStageAxis axis)
        {
            var ret = 0d;
            if (_boardCardController != null)
            {
                ret = _boardCardController.GetAxisSpeed(axis);
            }
            return (float)ret;
        }

        public double GetCurrentPosition(EnumStageAxis axis)
        {
            var ret = 0d;
            if (_boardCardController != null)
            {
                ret = _boardCardController.GetCurrentPosition(axis);
            }
            return ret;
        }

        public double[] GetCurrentStagePosition()
        {
            throw new NotImplementedException();
        }

        public void JogNegative(EnumStageAxis axis, float jogSpeed = 0)
        {
            //AxisControl.mc.MC_MoveJog((short)axis, 1d, 1d, 0.1d, (double)-jogSpeed);
            if (_boardCardController != null)
            {
                _boardCardController.JogNegative(axis,jogSpeed);
            }
        }

        public void JogPositive(EnumStageAxis axis, float jogSpeed = 0)
        {
            //AxisControl.mc.MC_MoveJog((short)axis, 1d, 1d, 0.1d, (double)jogSpeed);
            if (_boardCardController != null)
            {
                _boardCardController.JogPositive(axis, jogSpeed);
            }
        }
        public void StopJogNegative(EnumStageAxis axis)
        {
            //GTN.mc.GTN_Stop(CORE, 1 << (short)axis - 1, 1 << (short)axis - 1);
            if (_boardCardController != null)
            {
                _boardCardController.StopJogNegative(axis);
            }
        }

        public void StopJogPositive(EnumStageAxis axis)
        {
            //GTN.mc.GTN_Stop(CORE, 1 << (short)axis - 1, 1 << (short)axis - 1);
            if (_boardCardController != null)
            {
                _boardCardController.StopJogPositive(axis);
            }
        }
        public void RelativeMoveSync(EnumStageAxis axis, float distance)
        {
            if (_boardCardController != null)
            {
                if(axis == EnumStageAxis.NeedleZ)
                {
                    AxisConfig _axisConfig = HardwareConfiguration.Instance.StageConfig.GetAixsConfigByType(axis);
                    _boardCardController.MoveRelativeSync(axis, distance, _axisConfig.AxisSpeed);
                }
                else
                {
                    _boardCardController.MoveRelativeSync(axis, distance, 10);
                }
                
                WaitRelativeMoveDone(axis);
            }
        }

        public void RelativeMoveSync(EnumStageAxis[] axises, double[] distance)
        {
            if (_boardCardController != null)
            {
                for (int i = 0; i < axises.Length; i++)
                {
                    _boardCardController.MoveRelativeSync(axises[i], distance[i], 10);
                }
                for (int i = 0; i < axises.Length; i++)
                {
                    WaitRelativeMoveDone(axises[i]);
                }
            }
        }

        public void SetAxisSpeed(EnumStageAxis axis, float speed)
        {
            if (_boardCardController != null)
            {
                _boardCardController.SetAxisSpeed(axis,speed);
            }
        }
        public void SetAxisMotionParameters(AxisConfig axisParam)
        {
            if (_boardCardController != null)
            {
                _boardCardController.SetAxisMotionParameters(axisParam);
            }
        }


        public void WaitAbsoluteMoveDone(EnumStageAxis axis, int timeout = 60000)
        {
            if (_boardCardController != null)
            {
                bool isEnd = false;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!isEnd)
                {
                    var ret = _boardCardController.Get_AxisSts_PosDone(axis);//true-Done
                    if (ret)
                    {
                        isEnd = true;
                        break;
                    }
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        sw.Stop();
                        throw new Exception("WaitAbsoluteMoveDone-Timeout.");
                        isEnd = true;
                        break;
                    }
                    Thread.Sleep(50);
                }
            }
        }

        public void WaitAbsoluteMoveDone(EnumStageAxis[] axis)
        {
            //throw new NotImplementedException();
        }

        public void WaitRelativeMoveDone(EnumStageAxis axis, int timeout = 60000)
        {
            if (_boardCardController != null)
            {
                bool isEnd = false;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!isEnd)
                {
                    var ret = _boardCardController.Get_AxisSts_PosDone(axis);//true-Done
                    if (ret)
                    {
                        isEnd = true;
                        break;
                    }
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        sw.Stop();
                        throw new Exception("WaitAbsoluteMoveDone-Timeout.");
                        isEnd = true;
                        break;
                    }
                    Thread.Sleep(50);
                }
            }
        }

        public void WaitRelativeMoveDone(EnumStageAxis[] axis)
        {
            //throw new NotImplementedException();
        }
        public void Enable(EnumStageAxis axis)
        {
            if (_boardCardController != null)
            {
                _boardCardController.Enable(axis);
            }
        }
        public void Disable(EnumStageAxis axis)
        {
            if (_boardCardController != null)
            {
                _boardCardController.Disable(axis);
            }
        }
        /// <summary>
        /// 设置轴正负软限位
        /// </summary>
        public void SetSoftLeftAndRightLimit(EnumStageAxis axis, double Pvalue, double Nvalue)
        {
            if (_boardCardController != null)
            {
                _boardCardController.SetSoftLeftAndRightLimit(axis,Pvalue, Nvalue);
            }

        }
        public double GetSoftLeftLimit(EnumStageAxis axis)
        {
            var ret = 0d;
            if (_boardCardController != null)
            {
                ret=_boardCardController.GetSoftLeftLimit(axis);
            }
            return ret;
        }
        public double GetSoftRightLimit(EnumStageAxis axis)
        {
            var ret = 0d;
            if (_boardCardController != null)
            {
                ret = _boardCardController.GetSoftRightLimit(axis);
            }
            return ret;
        }
        /// <summary>
        /// 取消轴正负软限位
        /// </summary>
        public void CloseSoftLeftAndRightLimit(EnumStageAxis axis)
        {
            if (_boardCardController != null)
            {
                _boardCardController.CloseSoftLeftAndRightLimit(axis);
            }

        }
        /// <summary>
        /// 设置轴当前位置为原点
        /// </summary>
        public void Home(EnumStageAxis axis)
        {
            if (_boardCardController != null)
            {
                if ( axis == EnumStageAxis.WaferTableZ || axis == EnumStageAxis.ESZ)
                {
                    _boardCardController.Home(axis, 18);
                }
                //else if (axis == EnumStageAxis.WaferTableX||axis==EnumStageAxis.NeedleZ)
                else if (axis==EnumStageAxis.NeedleZ)
                {
                    _boardCardController.Home(axis, 17);
                }
                else if(axis == EnumStageAxis.WaferTableY)
                {
                    _boardCardController.Home(axis, 18);
                }
                else if(axis == EnumStageAxis.WaferTableX)
                {
                    _boardCardController.Home(axis, 17);
                }
                else if (axis == EnumStageAxis.SubmountPPZ)
                {
                    _boardCardController.Home(axis, 101);
                }
                else if (axis == EnumStageAxis.SubmountPPT)
                {
                    _boardCardController.Home(axis, 33);
                }
            }
        }

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
            if (_boardCardController != null)
            {
                return _boardCardController.GetAxisState(axis);
            }
            return 0;
        }

        public void SetAxisErrPosBind(EnumStageAxis axis, int band = 50, int time = 50)
        {
            int err = 0;
            if (axis == EnumStageAxis.BondX || axis == EnumStageAxis.BondY || axis == EnumStageAxis.BondZ)
            {
                _boardCardController.SetAxisErrPosBind(axis, out err, 50, time);

            }
            else if (axis == EnumStageAxis.ChipPPT)
            {
                _boardCardController.SetAxisErrPosBind(axis, out err, 50, 50);
            }
            else
            {

                _boardCardController.SetAxisErrPosBind(axis, out err, 100, 100);
            }
            _boardCardController.ClrAlarm(axis);
        }

        /// <summary>
        /// 报警清除
        /// </summary>
        public void ClrAlarm(EnumStageAxis axis)
        {
            _boardCardController.ClrAlarm(axis);
        }

        /// <summary>
        /// 报警 / 限位无效
        /// action动作 ： 1 为生效，0为失效
        /// </summary>
        public void DisableAlarmLimit(EnumStageAxis axis)
        {
            _boardCardController.DisableAlarmLimit(axis);
        }
        public void StartReadAxisPositionTask()
        {
            _enablePollingAxis = true;
            Task.Run(new Action(ReadAxisPositionTask));
        }
        private void ReadAxisPositionTask()
        {
            //while (_enablePollingIO)
            //{
            //    Thread.Sleep(500);
            //    List<int> data = new List<int>(); ;
            //    _boardCardController.IO_ReadAll(11, out data);
            //    if (data.Count > 0)
            //    {
            //        //ParseDataAndUpdateIOValue(data);
            //    }
            //}
        }

        #region  ZR控制部分


        #region 获取ZR的Z轴是否处于力控模式（等待触发状态）
        /// <summary>
        /// 检测是否处于力控模式（等待触发状态）
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>false：位置模式 true：力控模式</returns>
        public bool Get_ZRAxisForceMode(EnumStageAxis axis)
        {
            return _boardCardController.Get_ZRAxisForceMode(axis);
        }

        #endregion

        #region 切换ZR的Z轴工作状态状态（上电默认为位置状态）
        /// <summary>
        /// 切换ZR的Z轴工作状态状态（上电默认为位置状态）
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="WorkMode"> 0:位置模式 1：力控模式</param>
        public void Set_ZRAxisWorkMode(EnumStageAxis axis, short WorkMode)
        {
            _boardCardController.Set_ZRAxisWorkMode(axis, WorkMode);
        }
        #endregion

        #region 获得力控流程状态机
        /// <summary>
        /// 获得力控流程状态机
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns> 
        /// 0 未激活
        /// 1 力控已完成
        /// 2 快进中
        /// 4 保压中
        /// 5 回退中
        /// 9 保留状态
        /// </returns>
        public ushort Get_ZRAxisForceStation(EnumStageAxis axis)
        {
            return _boardCardController.Get_ZRAxisForceStation(axis);
        }
        #endregion


        #region 获取力控流程参数
        /// <summary>
        /// 获取力控流程参数
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="speedPos">快进位置(mm)</param>
        /// <param name="keepTime">保压时间(ms)</param>
        /// <param name="switchPos">速度切换位置 (mm)</param>
        /// <param name="backPos">回退位置 (mm)</param>
        /// <param name="speed">快进速度 (mm/s)</param>
        /// <param name="firstSpeed">一段速度 (mm/s)</param>
        /// <param name="secondSpeed">二段速度 (mm/s)</param>
        /// <param name="currentLimit">扭矩限制 (%)</param>
        public void Get_ZRForceParamters(
            EnumStageAxis axis,
            out double speedPos,
            out double keepTime,
            out double switchPos,
            out double backPos,
            out double speed,
            out double firstSpeed,
            out double secondSpeed,
            out double currentLimit)
        {
            _boardCardController.Get_ZRForceParamters(
            axis,
            out speedPos,
            out keepTime,
            out switchPos,
            out backPos,
            out speed,
            out firstSpeed,
            out secondSpeed,
            out currentLimit);
        }
        #endregion

        #region 设置力控流程参数
        /// <summary>
        /// 设置力控流程参数
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="speedPos">快进位置(mm)</param>
        /// <param name="keepTime">保压时间(ms)</param>
        /// <param name="switchPos">速度切换位置 (mm)</param>
        /// <param name="backPos">回退位置 (mm)</param>
        /// <param name="speed">快进速度 (mm/s)</param>
        /// <param name="firstSpeed">一段速度 (mm/s)</param>
        /// <param name="secondSpeed">二段速度 (mm/s)</param>
        /// <param name="currentLimit">扭矩限制 (%)</param>
        public void Set_ZRForceParamters(
     EnumStageAxis axis,
     double speedPos,
     double keepTime,
     double switchPos,
     double backPos,
     double speed,
     double firstSpeed,
     double secondSpeed,
     double currentLimit)
        {
            _boardCardController.Set_ZRForceParamters(
            axis,
            speedPos,
            keepTime,
            switchPos,
            backPos,
            speed,
            firstSpeed,
            secondSpeed,
            currentLimit);
        }
        #endregion

        #region    触发力控开始

        /// <summary>
        /// 触发力控开始
        /// </summary>
        /// <param name="axis"></param>
        public void Enable_ZRAxisForeceMode(EnumStageAxis axis)
        {
            _boardCardController.Enable_ZRAxisForeceMode(axis);
        }




        #endregion

        #region  力控快速返回

        public void GoBack_ZRAxisForeceMode(EnumStageAxis axis)
        {


            _boardCardController.GoBack_ZRAxisForeceMode(axis);

        }



        #endregion







        #endregion



    }
}
