using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardCardControllerClsLib
{
    public interface IBoardCardController
    {
        void Connect();
        bool IsConnect { get; }
        void Disconnect();

        void MotioParaInit();  //电机参数初始化

        void IO_Init();

        bool Get_SoftLimit_AxisStsPositive(EnumStageAxis axis);
        bool Get_SoftLimit_AxisStsNegative(EnumStageAxis axis);
        bool Get_AxisSts_Enable(EnumStageAxis axis);
        bool Get_AxisSts_Busy(EnumStageAxis axis);
        bool Get_AxisSts_PosDone(EnumStageAxis axis);
        bool Get_AxisSts_HomeDone(EnumStageAxis axis);

        bool Get_AxisEncArr(EnumStageAxis axis);




        /// <summary
        /// 最大轴速度
        /// </summary
        double GetMaxAxisSpeed(EnumStageAxis axis);
        /// <summary
        /// 最大轴速度
        /// </summary
        void SetMaxAxisSpeed(EnumStageAxis axis, double value);
        /// <summary
        /// 最大加速度
        /// </summary
        double GetMaxAcceleration(EnumStageAxis axis);
        /// <summary
        /// 最大加速度
        /// </summary
        void SetMaxAcceleration(EnumStageAxis axis, double value);

        /// <summary
        /// JERK
        /// </summary
        /// <param name="value"</param
        void SetJERK(EnumStageAxis axis, double value);

        /// <summary
        /// 软左限位
        /// </summary
        double GetSoftLeftLimit(EnumStageAxis axis);
        /// <summary
        /// 软左限位
        /// </summary
        void SetSoftLeftAndRightLimit(EnumStageAxis axis, double Pvalue, double Nvalue);
        void CloseSoftLeftAndRightLimit(EnumStageAxis axis);

        /// <summary
        /// 软右限位
        /// </summary
        double GetSoftRightLimit(EnumStageAxis axis);


        /// <summary
        /// 软右限位
        /// </summary
        /// <summary
        /// 启用
        /// </summary
        void Enable(EnumStageAxis axis);
        /// <summary
        /// 禁用
        /// </summary
        void Disable(EnumStageAxis axis);
        /// <summary
        /// Home
        /// </summary
        bool Home(EnumStageAxis axis, short homemode);
        /// <summary
        /// 设置轴速度
        /// </summary
        /// <param name="speed"</param
        void SetAxisSpeed(EnumStageAxis axis, double speed);
        /// <summary
        /// 获取轴速度
        /// </summary
        /// <returns</returns
        double GetAxisSpeed(EnumStageAxis axis);
        /// <summary
        /// 设置轴的加速度
        /// </summary
        /// <param name="acceleration"</param
        void SetAcceleration(EnumStageAxis axis, double acceleration);
        /// <summary
        /// 获取轴的加速度
        /// </summary
        /// <returns</returns
        double GetAcceleration(EnumStageAxis axis);
        /// <summary
        /// 获取轴减速度
        /// </summary
        /// <returns</returns
        double GetDeceleration(EnumStageAxis axis);
        /// <summary
        /// 设置轴减速度
        /// </summary
        /// <returns</returns
        void SetDeceleration(EnumStageAxis axis, double deceleration);
        /// <summary
        /// 设置紧急停止减速度
        /// </summary
        /// <param name="deceleration"</param
        void SetKillDeceleration(EnumStageAxis axis, double deceleration);
        /// <summary
        /// 获取轴的紧急停止减速度
        /// </summary
        /// <returns</returns
        double GetKillDeceleration(EnumStageAxis axis);
        void SetAxisMotionParameters(AxisConfig axisParam);
        /// <summary>
        /// 绝对移动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="targetPos"></param>
        /// <param name="Speed"></param>
        /// <param name="millisecondsTimeout"></param>
        void MoveAbsoluteSync(EnumStageAxis axis, double targetPos, double Speed, int millisecondsTimeout = -1);

        /// <summary>
        /// 相对移动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="Speed"></param>
        /// <param name="millisecondsTimeout">超时时间</param>
        void MoveRelativeSync(EnumStageAxis axis, double distance, double Speed, int millisecondsTimeout = -1);

        /// <summary>
        /// 获得当前位置
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        double GetCurrentPosition(EnumStageAxis axis);

        void JogPositive(EnumStageAxis axis, float speed);

        void StopJogPositive(EnumStageAxis axis);
        void JogNegative(EnumStageAxis axis, float speed);

        void StopJogNegative(EnumStageAxis axis);

        //脉冲当量转换：1mm对应多少个脉冲
        double MMToPulse(EnumStageAxis axis);


        //将脉冲转化成mm
        double PulseToMM(EnumStageAxis axis);
        //停止某一轴的运动
        void StopMotion(EnumStageAxis axis);

        //清除轴错误状态
        void Axis_ClearStatus(EnumStageAxis axis);

        void BoardCardClose();
        void BoardCardReset();

        //将某一个IO拉高
        void IO_WriteOutPut(ushort EcatID, short number, int Value);
        //读取某一个IO的输入信号
        void IO_ReadInput(ushort EcatID, int ioIndex, out int Value);
        //读取某一个IO的输出信号
        void IO_ReadOutput(ushort EcatID, int ioIndex, out int Value);
        //将某一个IO拉高
        void IO_WriteOutPut_2(ushort EcatID, short number, int Value);
        //读取某一个IO的输入信号
        void IO_ReadInput_2(ushort EcatID, int ioIndex, out int Value);
        //读取某一个IO的输出信号
        void IO_ReadOutput_2(ushort EcatID, int ioIndex, out int Value);
        //读取某一个IO的输入信号
        void IO_ReadInput_D(ushort EcatID, int offset, out int Value);
        //读取某一个IO的输出信号
        void IO_ReadOutput_D(ushort EcatID, int offset, out int Value);

        //读取某一个IO的输入信号
        void IO_ReadInput_A(ushort slaveno, ushort offset, out int Value);
        //读取某一个IO的输出信号
        void IO_ReadOutput_A(ushort slaveno, ushort offset, out int Value);

        //读所有输入信号
        void IO_ReadAllInput(ushort EcatID, out List<int> Value);
        //读所有输出信号状态
        void IO_ReadAllOutput(ushort EcatID, out List<int> Value);

        //读所有输入信号
        void IO_ReadAllInput_2(ushort EcatID, out List<int> Value);
        //读所有输出信号状态
        void IO_ReadAllOutput_2(ushort EcatID, out List<int> Value);

        //读IO模块状态
        bool IO_GetGLinkCommStatus();

        //读取轴状态
        int GetAxisState(EnumStageAxis axis);

        void SetAxisErrPosBind(EnumStageAxis axis, out int err, int band = 10, int time = 10);
        // 报警清除
        void ClrAlarm(EnumStageAxis axis);

        // 报警 / 限位无效
        void DisableAlarmLimit(EnumStageAxis axis);




        #region  ZR控制部分


        #region 获取ZR的Z轴是否处于力控模式（等待触发状态）
        /// <summary>
        /// 检测是否处于力控模式（等待触发状态）
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>false：位置模式 true：力控模式</returns>
        bool Get_ZRAxisForceMode(EnumStageAxis axis);

        #endregion

        #region 切换ZR的Z轴工作状态状态（上电默认为位置状态）
        /// <summary>
        /// 切换ZR的Z轴工作状态状态（上电默认为位置状态）
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="WorkMode"> 0:位置模式 1：力控模式</param>
        void Set_ZRAxisWorkMode(EnumStageAxis axis, short WorkMode);
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
        ushort Get_ZRAxisForceStation(EnumStageAxis axis);
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
        void Get_ZRForceParamters(
            EnumStageAxis axis,
            out double speedPos,
            out double keepTime,
            out double switchPos,
            out double backPos,
            out double speed,
            out double firstSpeed,
            out double secondSpeed,
            out double currentLimit);
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
        void Set_ZRForceParamters(
     EnumStageAxis axis,
     double speedPos,
     double keepTime,
     double switchPos,
     double backPos,
     double speed,
     double firstSpeed,
     double secondSpeed,
     double currentLimit);
        #endregion

        #region    触发力控开始

        /// <summary>
        /// 触发力控开始
        /// </summary>
        /// <param name="axis"></param>
        void Enable_ZRAxisForeceMode(EnumStageAxis axis);




        #endregion


        #region   快速返回
        void GoBack_ZRAxisForeceMode(EnumStageAxis axis);//保压中立即回退 0x2016 给2 上升沿触发

        #endregion 


        #endregion


    }
}
