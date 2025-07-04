using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageControllerClsLib
{
    public interface IStageController
    {
        ISingleAxisController this[EnumStageAxis axis] { get; }

        IMultiAxisController this[params EnumStageAxis[] axis] { get; }

        /// <summary
        /// 链接状态
        /// </summary
        bool IsConnect { get; }
        bool IsHomedone { get; }
        /// <summary
        /// 建立连接
        /// </summary
        void Connect();
        /// <summary
        /// 断开连接
        /// </summary
        void Disconnect();
        /// <summary
        /// 初始化各轴速度等参数
        /// </summary
        void InitialzeAllAxisParameter();
        /// <summary
        /// Home操作
        /// </summary
        void Home();
        bool CheckHomeDone();
        /// <summary
        /// 开启真空  压力强度从XML里面读取
        /// </summary
        /// <param name="enumVacuum"</param
        void OnVacuum(EnumVacuum enumVacuum);
        /// <summary
        /// 关闭真空
        /// </summary
        /// <param name="enumVacuum"</param
        void OffVacuum(EnumVacuum enumVacuum);
        /// <summary
        /// 停止
        /// </summary
        void Stop();
    }

    public interface ISingleAxisController
    {
        StageCore StageCore { get; set; }
        /// <summary
        /// 是否启用
        /// </summary
        bool IsEnabled { get; }
        /// <summary
        /// 最大轴速度
        /// </summary
        double GetMaxAxisSpeed();
        /// <summary
        /// 最大轴速度
        /// </summary
        void SetMaxAxisSpeed(double value);
        /// <summary
        /// 最大加速度
        /// </summary
        double GetMaxAcceleration();
        /// <summary
        /// 最大加速度
        /// </summary
        void SetMaxAcceleration(double value);

        /// <summary
        /// JERK
        /// </summary
        /// <param name="value"</param
        void SetAxisMotionParameters(AxisConfig axisParam);

        /// <summary
        /// 软左限位
        /// </summary
        double GetSoftLeftLimit();
        /// <summary
        /// 软左限位
        /// </summary
        void SetSoftLeftLimit(double value);
        /// <summary
        /// 软右限位
        /// </summary
        double GetSoftRightLimit();
        /// <summary
        /// 软右限位
        /// </summary
        void SetSoftRightLimit(double value);
        /// <summary
        /// 启用
        /// </summary
        void Enable();
        /// <summary
        /// 禁用
        /// </summary
        void Disable();
        void SetSoftLeftAndRightLimit(double Pvalue, double Nvalue);
        /// <summary>
        /// 取消轴正负软限位
        /// </summary>
        void CloseSoftLeftAndRightLimit();
        /// <summary
        /// Home
        /// </summary
        void Home();
        /// <summary
        /// 设置轴速度
        /// </summary
        /// <param name="speed"</param
        void SetAxisSpeed(double speed);
        /// <summary
        /// 获取轴速度
        /// </summary
        /// <returns</returns
        double GetAxisSpeed();
        /// <summary
        /// 设置轴的加速度
        /// </summary
        /// <param name="acceleration"</param
        void SetAcceleration(double acceleration);
        /// <summary
        /// 获取轴的加速度
        /// </summary
        /// <returns</returns
        double GetAcceleration();
        /// <summary
        /// 获取轴减速度
        /// </summary
        /// <returns</returns
        double GetDeceleration();
        /// <summary
        /// 设置轴减速度
        /// </summary
        /// <returns</returns
        void SetDeceleration(double deceleration);
        /// <summary
        /// 设置紧急停止减速度
        /// </summary
        /// <param name="deceleration"</param
        void SetKillDeceleration(double deceleration);
        /// <summary
        /// 获取轴的紧急停止减速度
        /// </summary
        /// <returns</returns
        double GetKillDeceleration();

        /// <summary
        /// 绝对移动
        /// </summary
        /// <param name="targetPos"</param
        /// <param name="millisecondsTimeout"超时时间  毫秒</param
        void MoveAbsoluteSync(double targetPos, int millisecondsTimeout = -1);

        /// <summary
        /// 相对移动
        /// </summary
        /// <param name="distance"</param
        /// <param name="millisecondsTimeout"超时时间  毫秒</param
        void MoveRelativeSync(double distance, int millisecondsTimeout = -1);

        /// <summary
        /// 获得当前位置
        /// </summary
        /// <returns</returns
        double GetCurrentPosition();
        /// <summary
        /// 获取所有轴位置
        /// </summary
        /// <returns</returns
        double[] GetCurrentStagePosition();

        void JogPositive(float speed);

        void StopJogPositive();
        void JogNegative(float speed);

        void StopJogNegative();

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
        int GetAxisState();

        void SetAxisErrPosBind(int band = 50, int time = 50);

        /// <summary>
        /// 报警清除
        /// </summary>
        void ClrAlarm();

        /// <summary>
        /// 报警 / 限位无效
        /// action动作 ： 1 为生效，0为失效
        /// </summary>
        void DisableAlarmLimit();


        #region  ZR控制部分


        #region 获取ZR的Z轴是否处于力控模式（等待触发状态）
        /// <summary>
        /// 检测是否处于力控模式（等待触发状态）
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>false：位置模式 true：力控模式</returns>
        bool Get_ZRAxisForceMode();

        #endregion

        #region 切换ZR的Z轴工作状态状态（上电默认为位置状态）
        /// <summary>
        /// 切换ZR的Z轴工作状态状态（上电默认为位置状态）
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="WorkMode"> 0:位置模式 1：力控模式</param>
        void Set_ZRAxisWorkMode(short WorkMode);
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
        ushort Get_ZRAxisForceStation();
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
        void Enable_ZRAxisForeceMode();




        #endregion






        #endregion


    }

    public interface IMultiAxisController
    {
        StageCore StageCore { get; set; }
        /// <summary
        /// 最大轴速度
        /// </summary
        double[] GetMaxAxisSpeed();
        /// <summary
        /// 最大轴速度
        /// </summary
        void SetMaxAxisSpeed(double[] value);
        /// <summary
        /// 最大加速度
        /// </summary
        double[] GetMaxAcceleration();
        /// <summary
        /// 最大加速度
        /// </summary
        void SetMaxAcceleration(double[] value);
        /// <summary
        /// 软左限位
        /// </summary
        double[] GetSoftLeftLimit();
        /// <summary
        /// 软左限位
        /// </summary
        void SetSoftLeftLimit(double[] value);
        /// <summary
        /// 软右限位
        /// </summary
        double[] GetSoftRightLimit();
        /// <summary
        /// 软右限位
        /// </summary
        void SetSoftRightLimit(double[] value);
        /// <summary
        /// 启用
        /// </summary
        void Enable();
        /// <summary
        /// 禁用
        /// </summary
        void Disable();
        /// <summary
        /// 获得当前位置
        /// </summary
        /// <returns</returns
        double[] GetCurrentPosition();
        /// <summary
        /// 设置轴速度
        /// </summary
        /// <param name="speed"</param
        void SetAxisSpeed(double[] speed);
        /// <summary
        /// 获取轴加速度
        /// </summary
        /// <returns</returns
        double[] GetAxisSpeed();
        /// <summary
        /// 设置轴的加速度
        /// </summary
        /// <param name="speed"</param
        void SetAcceleration(double[] speed);
        /// <summary
        /// 获取加速度
        /// </summary
        /// <returns</returns
        double[] GetAcceleration();
        /// <summary
        /// 获取轴减速度
        /// </summary
        /// <returns</returns
        double[] GetDeceleration();
        /// <summary
        /// 设置轴减速度
        /// </summary
        /// <returns</returns
        void SetDeceleration(double[] deceleration);
        /// <summary
        /// 设置紧急停止减速度
        /// </summary
        /// <param name="deceleration"</param
        void SetKillDeceleration(double[] deceleration);
        /// <summary
        /// 获取轴的紧急停止减速度
        /// </summary
        /// <returns</returns
        double[] GetKillDeceleration();
        /// <summary
        /// 绝对移动
        /// </summary
        /// <param name="targetPos"</param
        /// <param name="millisecondsTimeout"超时时间  毫秒</param
        void MoveAbsoluteSync(double[] targetPos, int millisecondsTimeout = -1);
        /// <summary
        /// 相对移动
        /// </summary
        /// <param name="distance"</param
        /// <param name="millisecondsTimeout"超时时间  毫秒</param
        void MoveRelativeSync(double[] distance, int millisecondsTimeout = -1);
    }


    public enum EnumVacuum
    {
        InnerRing, OuterRing
    }



    public abstract class AStageInfo
    {
        /// <summary
        /// 轴控制器集合
        /// </summary
        public Dictionary<EnumStageAxis, ISingleAxisController> AxisControllerDic { get; set; }
    }
}
