using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using static GTN.glink;

namespace AxisControl
{

    public class mc
    {
        const short AxisCrd = 1;
        private short rtn;
        private static uint clk;
        /// <summary>
        /// 按位获取轴状态
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static bool AxisSts(short axis, short bit)
        {
            int pSts;
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }
            return false;
        }

        public static void  AxisSts(short axis,out int pSts)
        {
           
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            
        }


        /// <summary>
        /// 获取轴的正极限软限位状态
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool SoftLimit_AxisStsPositive(short axis)
        {
            int pSts;
            short bit = 5;
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取轴的负极限软限位状态
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool SoftLimit_AxisStsNegative(short axis)
        {
            int pSts;
            short bit = 6;
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取轴的使能标志
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisSts_Enable(short axis)
        {
            int pSts;
            short bit = 9;
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取全部轴状态
        /// </summary>
        /// <returns></returns>
        public static bool AreAllAxesEnabled(int  AxisNum)
        {
            // 假设轴的总数量是 8  
             int totalAxes = AxisNum;
            for (short i = 1; i <= AxisNum; i++)
            {
                if (!AxisSts_Enable(i))
                {
                    return false; // 只要有一个轴未启用，立即返回 false  
                }
            }
            return true; // 所有轴都启用，返回 true  
        }

        /// <summary>
        /// 获取轴正忙标志
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisSts_Busy(short axis)
        {
            int pSts;
            short bit = 10;
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取轴正忙标志
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisSts_Busy(short axis,out short err)
        {
            int pSts;
            short bit = 10;
            err = GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取轴到位标志
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisSts_PosDone(short axis)
        {
            int pSts;
            short bit = 10;//11;
            GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 电机到达设定误差带，使用前提是必须开始误差带监测功能
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisSts_PosReach(short axis,out int err)
        {
            int pSts;
            short bit = 11;
            err = GTN.mc.GTN_GetSts(AxisCrd, axis, out pSts, 1, out clk);
            if ((pSts & (1 << bit)) !=0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置轴运动误差带
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="core"></param>
        /// <param name="band"></单位为脉冲param>
        /// <param name="time"></单位250USparam>
        /// <param name="err"></param>
        public static void SetAxisErrPosBind(short axis, short core,int band,int time,out int err)
        {
           
            err  = GTN.mc.GTN_SetAxisBand(core, axis, band, time);
            GTN.mc.GTN_ClrSts(core, Convert.ToInt16(axis), 1);

        }
        /// <summary>
        /// 获取轴运动误差带
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="core"></param>
        /// <param name="band"></单位为脉冲param>
        /// <param name="time"></单位250USparam>
        /// <param name="err"></param>
        public static void GetAxisErrPosBind(short axis, short core, out int band, out int time, out int err)
        {

            err = GTN.mc.GTN_GetAxisBand(core,  axis, out band, out time);
            

        }


        /// <summary>
        /// 获取回原点到位标志
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisSts_HomeDone(short axis)
        {
            ushort sHomeSts;
            GTN.mc.GTN_GetEcatHomingStatus(1, axis, out sHomeSts);
            if (3 == sHomeSts)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 规划到位信号
        /// </summary>
        /// <param name="axis">轴号</param>
        public static bool AxisArr(short axis)
        {
            bool run;
            do
            {
                run = AxisSts(axis, 10);
            } while (run);
            return !run;
        }

        /// <summary>
        /// 轴错误信号
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static bool AxisEncArr(short axis)
        {
            bool run;
            do
            {
                run = AxisSts(axis, 11);

            } while (!run);
            return run;
        }
        /// <summary>
        /// 绝对运动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Trap">运动参数</param>
        /// <param name="Vel">速度</param>
        /// <param name="Pos">位置</param>
        public static short MC_MoveAbsolute(short Axis, double acc, double dec, double Vel, short sm, int Pos)
        {
            short rtn = 0;
            GTN.mc.TTrapPrm trap;
            rtn = GTN.mc.GTN_PrfTrap(AxisCrd, Axis);
            //FileData.Log.Commandhder("GTN_PrfTrap", rtn);
            rtn = GTN.mc.GTN_GetTrapPrm(AxisCrd, Axis, out trap);
            //FileData.Log.Commandhder("GTN_GetTrapPrm", rtn);
            trap.acc = acc;
            trap.dec = dec;
            trap.smoothTime = sm;
            rtn = GTN.mc.GTN_SetTrapPrm(AxisCrd, Axis, ref trap);
            //FileData.Log.Commandhder("GTN_SetTrapPrm", rtn);
            rtn = GTN.mc.GTN_SetVel(AxisCrd, Axis, Vel);
            //FileData.Log.Commandhder("GTN_SetVel", rtn);
            rtn = GTN.mc.GTN_SetPos(AxisCrd, Axis, Pos);
            //FileData.Log.Commandhder("GTN_SetPos", rtn);
            rtn = GTN.mc.GTN_Update(AxisCrd, 1 << Axis - 1);
            //FileData.Log.Commandhder("GTN_Update", rtn);

            return rtn;
        }
        /// <summary>
        /// 相对运动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Trap">运动参数</param>
        /// <param name="Vel">速度</param>
        /// <param name="Pos">相对位置</param>
        public static short MC_MoveRelative(short Axis, double Acc, double Dec, double Vel, short Sm, int Pos)
        {
            short rtn = 0;
            rtn = GTN.mc.GTN_PrfTrap(AxisCrd, Axis);
            //FileData.Log.Commandhder("GTN_PrfTrap", rtn);
            GTN.mc.TTrapPrm trap;
            double pPos;
            uint clk;
            rtn = GTN.mc.GTN_GetTrapPrm(AxisCrd, Axis, out trap);
            // FileData.Log.Commandhder("GTN_GetTrapPrm", rtn);
            trap.acc = Acc;
            trap.dec = Dec;
            trap.smoothTime = Sm;
            trap.velStart = 0;
            rtn = GTN.mc.GTN_SetTrapPrm(AxisCrd, Axis, ref trap);
            //FileData.Log.Commandhder("GTN_SetTrapPrm", rtn);
            rtn = GTN.mc.GTN_SetVel(AxisCrd, Axis, Vel);
            //FileData.Log.Commandhder("GTN_SetVel", rtn);
            rtn = GTN.mc.GTN_GetPrfPos(AxisCrd, Axis, out pPos, 1, out clk);
            //FileData.Log.Commandhder("GTN_GetPrfPos", rtn);
            rtn = GTN.mc.GTN_SetPos(AxisCrd, Axis, (int)(pPos + Pos));
            //FileData.Log.Commandhder("GTN_SetPos", rtn);
            rtn = GTN.mc.GTN_Update(AxisCrd, 1 << Axis - 1);
            //FileData.Log.Commandhder("GTN_Update", rtn);
            return rtn;
        }
        /// <summary>
        /// Jog运动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Jog">运动参数</param>
        /// <param name="Vel">速度</param>
        public static short MC_MoveJog(short Axis, double Acc, double Dec, double Sm, double Vel)
        {
            GTN.mc.TJogPrm jog;
            short rtn = 0;
            rtn = GTN.mc.GTN_PrfJog(AxisCrd, Axis);
            //FileData.Log.Commandhder("GTN_PrfJog", rtn);
            rtn = GTN.mc.GTN_GetJogPrm(AxisCrd, Axis, out jog);
            //FileData.Log.Commandhder("GTN_GetJogPrm", rtn);
            jog.acc = Acc;
            jog.dec = Dec;
            jog.smooth = Sm;
            rtn = GTN.mc.GTN_SetJogPrm(AxisCrd, Axis, ref jog);
            //FileData.Log.Commandhder("GTN_SetJogPrm", rtn);
            rtn = GTN.mc.GTN_SetVel(AxisCrd, Axis, Vel);
            //FileData.Log.Commandhder("GTN_SetVel", rtn);
            rtn = GTN.mc.GTN_Update(AxisCrd, 1 << Axis - 1);
            //FileData.Log.Commandhder("GTN_Update", rtn);
            return rtn;
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="Axis">轴号</param>
        public static void MC_Stop(short Axis)
        {
            GTN.mc.GTN_Stop(AxisCrd, 1 << Axis - 1, 1 << Axis - 1);
        }

        /// <summary>
        /// 回零方法
        /// </summary>
        /// <param name="Axis"></param>
        /// <param name="mode">4 正向回零 6 反向回零</param>
        /// <returns></returns>
        public static bool MC_Home(short Axis, short mode = 3)
        {
            short rtn;
            ushort HomeSts;


            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);//清除当前错误
            rtn = GTN.mc.GTN_SetHomingMode(AxisCrd, Axis, 6);  //切换到回零模式
            rtn = GTN.mc.GTN_SetEcatHomingPrm(AxisCrd, Axis, mode, 10000, 500, 20000, 0, 0);  //速度加速度参数填入

            rtn = GTN.mc.GTN_StartEcatHoming(AxisCrd, Axis);//启动回零


            do
            {
                GTN.mc.GTN_GetEcatHomingStatus(AxisCrd, Axis, out HomeSts);
                if ((HomeSts & 1 << 2) != 0 || AxisSts(Axis, 11)) //失败
                {


                 //   return false;
                }

            } while (HomeSts != 3);//成功
            rtn = GTN.mc.GTN_SetHomingMode(AxisCrd, Axis, 8);//切换到位置控制模式
            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);//清除发生过的回原点错误、
            return true;

        }
        /// <summary>
        /// 步进回零
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Home">回零参数</param>
        public static bool MC_Home(short Axis)
        {
            short rtn;
            ushort HomeSts;


            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);//清除当前错误
            rtn = GTN.mc.GTN_SetHomingMode(AxisCrd, Axis, 6);  //切换到回零模式
            rtn = GTN.mc.GTN_SetEcatHomingPrm(AxisCrd, Axis, 35, 500, 300, 1000, 0, 0);  //速度加速度参数填入

            rtn = GTN.mc.GTN_StartEcatHoming(AxisCrd, Axis);//启动回零


            do
            {
                GTN.mc.GTN_GetEcatHomingStatus(AxisCrd, Axis, out HomeSts);
                if ((HomeSts & 1 << 2) != 0) //失败
                {


                    return false;
                }

            } while (HomeSts != 3);//成功
            rtn = GTN.mc.GTN_SetHomingMode(AxisCrd, Axis, 8);//切换到位置控制模式
            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);//清除发生过的回原点错误、
            return true;

        }
        /// <summary>
        /// 步进回零
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Home">回零参数</param>
        /// 
        public static  bool Step_Go_Home(short Axis)
        {
            short rtn;
            ushort HomeSts;


            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);//清除当前错误
            rtn = GTN.mc.GTN_SetHomingMode(AxisCrd, Axis, 6);  //切换到回零模式
            if (Axis == 8|| Axis == 10)
            {
                rtn = GTN.mc.GTN_SetEcatHomingPrm(AxisCrd, Axis, 18, 3000, 500, 1000, 0, 0);  //速度加速度参数填入
            }
            else
            {
                rtn = GTN.mc.GTN_SetEcatHomingPrm(AxisCrd, Axis, 18, 500, 300, 1000, 0, 0);  //速度加速度参数填入
            }
            LogRecorder.RecordLog(EnumLogContentType.Debug, $"Home:{Axis}-Start.");
            rtn = GTN.mc.GTN_StartEcatHoming(AxisCrd, Axis);//启动回零


            do
            {
                GTN.mc.GTN_GetEcatHomingStatus(AxisCrd, Axis, out HomeSts);
                if ((HomeSts & 1 << 2) != 0) //失败
                {


                    return false;
                }

            } while (HomeSts != 3);//成功
            LogRecorder.RecordLog(EnumLogContentType.Debug, $"Home:{Axis}-End.");
            rtn = GTN.mc.GTN_SetHomingMode(AxisCrd, Axis, 8);//切换到位置控制模式
            //Thread.Sleep(3000);
            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);//清除发生过的回原点错误、
            return true;




        }




        /// <summary>
        /// 使能
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Enable">使能</param>
        public static short MC_Power(short Axis, bool Enable)
        {
            short rtn = 0;
            if (Enable)
            {
                rtn += GTN.mc.GTN_AxisOn(AxisCrd, Axis);
            }
            else
            {
                rtn += GTN.mc.GTN_AxisOff(AxisCrd, Axis);
            }
            return rtn;
        }
        /// <summary>
        /// 获取编码器位置(pulse)
        /// </summary>
        /// <param name="Axis"></param>
        /// <returns></returns>
        public static double MC_GetEncPos(short Axis)
        {
            
            double encpos;
            uint clk;
            GTN.mc.GTN_GetEncPos(AxisCrd, Axis, out encpos,1,out clk);
          //  GTN.mc.GTN_GetEncPos(1, Form_Msg.MAxis, out encpos, 1, out clk);
            return encpos;


        }
        /// <summary>
        /// 获取规划位置(pulse)
        /// </summary>
        /// <param name="Axis"></param>
        /// <returns></returns>
        public static int MC_GetPrfPos(short Axis)
        {
            uint clk;
            double prfpos;
            GTN.mc.GTN_GetPrfPos(AxisCrd, Axis, out prfpos, 1, out clk);
            return (int)Math.Round(prfpos);
        }
        public static int MC_GetPrfVel(short Axis)
        {
            uint clk;
            double prfvel;
            GTN.mc.GTN_GetPrfVel(AxisCrd, Axis, out prfvel, 1, out clk);
            return (int)Math.Round(prfvel);
        }
        public static int MC_GetPrfAcc(short Axis)
        {
            uint clk;
            double prfvel;
            GTN.mc.GTN_GetPrfAcc(AxisCrd, Axis, out prfvel, 1, out clk);
            return (int)Math.Round(prfvel);
        }

        public static int MC_GetSoftLimitPositive(short Axis)
        {
            Int32 Postive;
            Int32 Negative;
            GTN.mc.GTN_GetSoftLimit(AxisCrd, Axis, out Postive, out Negative);
            return Postive;

        }

        public static int MC_GetSoftLimitNegative(short Axis)
        {
            Int32 Postive;
            Int32 Negative;
            GTN.mc.GTN_GetSoftLimit(AxisCrd, Axis, out Postive, out Negative);
            return Negative;

        }

        public static void MC_SetSoftLimitNegativeAndPostive(short Axis, Int32 P,Int32  N)
        {
  
            GTN.mc.GTN_SetSoftLimitMode(AxisCrd, Axis, 1);
            GTN.mc.GTN_SetSoftLimit(AxisCrd, Axis, P, N);
        }

        public static void CloseSoftLimit(short Axis)
        {

            GTN.mc.GTN_SetSoftLimitMode(AxisCrd, Axis, 1);
         
            GTN.mc.GTN_SetSoftLimit(AxisCrd, Axis, 0x7fffffff,unchecked((int)0x80000000) );
        }

        public static void Axis_Stop(short Axis)
        {

            GTN.mc.GTN_Stop(AxisCrd, 1 << (Axis - 1), 1 << (Axis - 1));
        }

        public static void Axis_Stop(short Axis,out short err)
        {

            err= GTN.mc.GTN_Stop(AxisCrd, 1 << (Axis - 1), 1 << (Axis - 1));
        }

        public static void Axis_ClearSt(short Axis)
        {

            GTN.mc.GTN_ClrSts(AxisCrd, Axis, 1);
        }
       
        public static void ConCardClose()
        {

            GTN.mc.GTN_TerminateEcatComm(AxisCrd);
            GTN.mc.GTN_Close();
        }

        public static void ConCardClose(out short err)
        {

            err= GTN.mc.GTN_TerminateEcatComm(AxisCrd);
            err = GTN.mc.GTN_Close();
        }
        public static void  CardReset()
        {

            GTN.mc.GTN_Reset(AxisCrd);


        }

        public static int MC_GetEncVel(short Axis)
        {
            uint clk;
            double prfvel;
            GTN.mc.GTN_GetEncVel(AxisCrd, Axis, out prfvel, 1, out clk);
            return (int)Math.Round(prfvel);
        }

        public static double Int2DoubleDivide(double numerator, int denominator)
        {
            // 确保分母不为零
            if (denominator == 0)
            {
                return 0;
                throw new DivideByZeroException("分母不能为零。");
            }

            // 将其中一个操作数转换为 double
            return (double)(numerator / denominator);
        }


        public static void GetAxisTorque(short core, short axis, out short pTorque)
        {

            GTN.mc.GTN_GetEcatAxisAtlTorque(core, axis, out pTorque);
        }

        public static void GetAxisCurrent(short core, short axis, out short current)
        {

            GTN.mc.GTN_GetEcatAxisAtlCurrent(core, axis, out current);
        }



    }

    class IOFun
    {
        const short AxisCrd = 1;
        private short rtn;

        public static void IO_Init()
        {
            short rtn;
            byte count;
            //rtn = GTN.glink.GT_GLinkInitEx(0, 1);
            rtn = GTN.glink.GT_GLinkInitEx(0, 3);

            rtn = GTN.glink.GT_GetGLinkOnlineSlaveNum(out count);

        }
        /// <summary>
        /// IO初始化 IOmode 1 代表跟随线程刷新， 3代表退出软件后保持IO输出
        /// </summary>
        public static void IO_Init( short  IOMode)
        {
            short rtn;
            byte count;
            //rtn = GTN.glink.GT_GLinkInitEx(0, 1);
            rtn = GTN.glink.GT_GLinkInitEx(0, IOMode);

            rtn = GTN.glink.GT_GetGLinkOnlineSlaveNum(out count);

        }

        public static void IO_WriteOutPut(ushort EcatID, short number,int Value)
        {

            short rtn = 0;
            number = (short)(number - 1);
            // 确定字节和位的位置
            int ioIndex = number;
            int byteIndex = ioIndex / 8; // 每个字节包含8个位
            int bitIndex = ioIndex % 8;  // 确定在字节中的位位置
            byte[] BUFF = new byte[2];
            rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, 0, 2, out BUFF[0]); //读取当前IO状态
            BUFF[byteIndex] = (byte)(BUFF[byteIndex] | (Value << bitIndex));
            if (1 == Value)
            {
                BUFF[byteIndex] = (byte)(BUFF[byteIndex] | (Value << bitIndex));
            }
            else if (0 == Value)
            {
                BUFF[byteIndex] = (byte)(BUFF[byteIndex] & (~(1 << bitIndex)));

            }


            //  rtn = GTN.mc.GTN_EcatIOBitWriteOutput(1, EcatID, 0, number,(byte) Value);
            GTN.mc.GTN_EcatIOWriteOutput(1, EcatID,0,2, ref BUFF[0]);
            GTN.mc.GTN_EcatIOSynch(1);


        }

        public static void IO_WriteOutPut_2(ushort slaveno, short doIndex, int Value)
        {
            byte v = 0;
            if(Value == 0)
            {
                v = 0;
            }
            else if(Value == 1)
            {
                v = 1;
            }
            short rtn = GTN.glink.GT_SetGLinkDoBit((short)slaveno, (short)doIndex, v);

        }


        public static void IO_ReadInput(ushort EcatID, int ioIndex,out int Value)
        {
            // 确定字节和位的位置
            ioIndex = ioIndex - 1;//偏移位从1开始
            int byteIndex = ioIndex / 8; // 每个字节包含8个位
            int bitIndex = ioIndex % 8;  // 确定在字节中的位位置
            byte[] BUFF = new byte[2];

            short rtn = 0;
            rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, 6, 2, out BUFF[0]);
            int isSet = (BUFF[byteIndex] & (1 << bitIndex)) != 0 ? 1 : 0;
            Value = isSet;


        }

        public static short IO_ReadInput_2(ushort slaveno, int doIndex, out int Value)
        {
            // 确定字节和位的位置
            //doIndex = doIndex - 1;//偏移位从1开始
            short err = 0;
            int byteIndex = doIndex / 8; // 每个字节包含8个位
            int bitIndex = doIndex % 8;  // 确定在字节中的位位置
            byte[] BUFF = new byte[2];

            short rtn = 0;
            byte[] di0 = new byte[4];

            err= GTN.glink.GT_GetGLinkDi((short)slaveno, 0, out di0[0], 2);

            int isSet = (di0[byteIndex] & (1 << bitIndex)) != 0 ? 1 : 0;
            Value = isSet;

            return err;


        }


        public static void IO_ReadOutput(ushort EcatID, int ioIndex, out int Value)
        {
            // 确定字节和位的位置
            ioIndex = ioIndex - 1;//偏移位从1开始
            int byteIndex = ioIndex / 8; // 每个字节包含8个位
            int bitIndex = ioIndex % 8;  // 确定在字节中的位位置
            byte[] BUFF = new byte[2];

            short rtn = 0;
            rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, 0, 2, out BUFF[0]);
            int isSet = (BUFF[byteIndex] & (1 << bitIndex)) != 0 ? 1 : 0;
            Value = isSet;


        }

        public static void IO_ReadOutput_2(ushort slaveno, int doIndex, out int Value)
        {
            // 确定字节和位的位置
            doIndex = doIndex - 1;//偏移位从1开始
            int byteIndex = doIndex / 8; // 每个字节包含8个位
            int bitIndex = doIndex % 8;  // 确定在字节中的位位置
            byte[] BUFF = new byte[2];

            short rtn = 0;
            byte[] do0 = new byte[4];

            GTN.glink.GT_GetGLinkDo((short)slaveno, 0, ref do0[0], 2);

            int isSet = (do0[byteIndex] & (1 << bitIndex)) != 0 ? 1 : 0;
            Value = isSet;


        }

        public static void IO_ReadOutput_2(ushort slaveno, out int Value)
        {
            
            byte[] do0 = new byte[4];

            GTN.glink.GT_GetGLinkDo((short)slaveno, 0, ref do0[0], 2);

           
            Value = (do0[1] << 8) | do0[0];


        }


        static byte[] ShortArrayToByteArray(short[] shortArray)
        {
            // 创建一个 byte 数组，大小是 short 数组大小的 2 倍  
            byte[] byteArray = new byte[shortArray.Length * 2];

            // 将每个 short 转换成对应的 byte 数组  
            for (int i = 0; i < shortArray.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(shortArray[i]);
                Array.Copy(bytes, 0, byteArray, i * 2, 2);
            }

            return byteArray;
        }

        static short[] ByteArrayToShortArray(byte[] byteArray)
        {
            if (byteArray.Length % 2 != 0)
            {
                throw new ArgumentException("字节数组的长度必须是2的倍数。");
            }

            // 创建一个 short 数组，大小为 byte 数组长度的一半  
            short[] shortArray = new short[byteArray.Length / 2];

            // 将每 2 个字节转换为一个 short  
            for (int i = 0; i < shortArray.Length; i++)
            {
                shortArray[i] = BitConverter.ToInt16(byteArray, i * 2);
            }

            return shortArray;
        }

        public static void IO_WriteOutPut_D(ushort EcatID, ushort offset, short[] Value)
        {
            ushort Num = 2;
            Num = (ushort)(Value.Length * 2);

            byte[] byteArray = ShortArrayToByteArray(Value);

            short rtn = 0;

            byte[] BUFF = new byte[Num];
            rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, offset, Num, out BUFF[0]); //读取当前IO状态

            BUFF = byteArray;


            //  rtn = GTN.mc.GTN_EcatIOBitWriteOutput(1, EcatID, 0, number,(byte) Value);
            GTN.mc.GTN_EcatIOWriteOutput(1, EcatID, offset, Num, ref BUFF[0]);
            GTN.mc.GTN_EcatIOSynch(1);


        }

        public static void IO_ReadInput_D(ushort EcatID, ushort offset, out int Value)
        {
            byte[] BUFF = new byte[2];
            short[] value;

            short rtn = 0;
            rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, (ushort)(offset + 6), 2, out BUFF[0]);
            value = ByteArrayToShortArray(BUFF);



            Value = value[0];


        }

        public static void IO_ReadOutput_D(ushort EcatID, ushort offset, out int Value)
        {
            byte[] BUFF = new byte[2];
            short[] value;

            short rtn = 0;
            rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, offset, 2, out BUFF[0]); //读取当前IO状态
            value = ByteArrayToShortArray(BUFF);
            Value = value[0];

        }

        public static void IO_ReadInput_A(ushort slaveno, ushort offset, out int Value)
        {
            short[] Ai = new short[4];
            GTN.glink.GT_GetGLinkAi((short)slaveno, 0, out Ai[0], 4);

            Value = Ai[offset];


        }

        public static void IO_ReadOutput_A(ushort slaveno, ushort offset, out int Value)
        {
            short[] Ai = new short[4];
            GTN.glink.GT_GetGLinkAo((short)slaveno, 0, out Ai[0], 4);

            Value = Ai[offset];

        }


        public static void IO_ReadAllInput(ushort EcatID, out List<int> Values)
        {
            Values = new List<int>();
            byte[] BUFF = new byte[2];
            short rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, 6, 2, out BUFF[0]);

            for (int i = 0; i < 8; i++)
            {
                Values.Add((BUFF[0] >> i) & 1);
            }
            for (int i = 0; i < 8; i++)
            {
                Values.Add((BUFF[1] >> i) & 1);
            }
        }
        public static void IO_ReadAllOutput(ushort EcatID, out List<int> Values)
        {
            Values = new List<int>();
            byte[] BUFF = new byte[2];
            short rtn = GTN.mc.GTN_EcatIOReadInput(1, EcatID, 0, 2, out BUFF[0]);

            for (int i = 0; i < 8; i++)
            {
                Values.Add((BUFF[0] >> i) & 1);
            }
            for (int i = 0; i < 8; i++)
            {
                Values.Add((BUFF[1] >> i) & 1);
            }
        }

        public static void IO_ReadAllInput_2(ushort slaveno, out List<int> Values)
        {
            Values = new List<int>();
            byte[] di0 = new byte[4];
            short rtn = GTN.glink.GT_GetGLinkDi((short)slaveno, 0, out di0[0], 2);

            for (int i = 0; i < 8; i++)
            {
                Values.Add((di0[0] >> i) & 1);
            }
            for (int i = 0; i < 8; i++)
            {
                Values.Add((di0[1] >> i) & 1);
            }
        }
        public static void IO_ReadAllOutput_2(ushort slaveno, out List<int> Values)
        {
            Values = new List<int>();
            byte[] do0 = new byte[4];
            short rtn = GTN.glink.GT_GetGLinkDo((short)slaveno, 0, ref do0[0], 2);

            for (int i = 0; i < 8; i++)
            {
                Values.Add((do0[0] >> i) & 1);
            }
            for (int i = 0; i < 8; i++)
            {
                Values.Add((do0[1] >> i) & 1);
            }
        }


        public static bool MC_GetGLinkCommStatus()
        {
            GLINK_COMM_STS G;
            GTN.glink.GT_GetGLinkCommStatus(out G);
            if (G.commStatus == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }

  public  class AxisPvt
    {
        public short Axis;
        public const short CORE = 1;
        short rtn;
        //PVT转盘测试
        public double velBegin = 0;
        public int PvtDataNum = 0;
        public double[] Pos = new double[1024];
        public double[] Time = new double[1024];
        public double[] Percent = new double[1024];
        List<double> Math_Pos = new List<double>();
        List<double> Math_Vel = new List<double>();
        List<double> Math_Percent = new List<double>();
        /// <summary>
        /// 解析PVT数据
        /// </summary>
        /// <param name="strdata">数据</param>
        private void ReadData(string strdata)
        {
            try
            {
                double[] PvtPos = new double[1024];
                double[] PvtTime = new double[1024];
                double[] PvtPercent = new double[1024];
                int count = 0;
                double startvel = 0;
                string[] data = strdata.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0] != "type=PvtPercent")
                {
                    throw new Exception("数据第一行错误误");
                }
                if (!data[1].Contains("velBegin"))
                {
                    throw new Exception("数据第二行错误");
                }
                else
                {
                    startvel = Convert.ToDouble(data[1].Split('=')[1].Trim());
                }
                if (!data[2].Contains("count"))
                {
                    throw new Exception("数据第三行错误");
                }
                else
                {
                    count = Convert.ToInt32(data[2].Split('=')[1].Trim());
                }
                if (count != data.Length - 3)
                {
                    throw new Exception("数据数量错误");
                }

                for (int i = 3; i < data.Length; i++)
                {
                    string[] param = data[i].Split(',');
                    PvtTime[i - 3] = Convert.ToDouble(param[0].Trim());
                    PvtPos[i - 3] = Convert.ToDouble(param[1].Trim());
                    PvtPercent[i - 3] = Convert.ToDouble(param[2].Trim());
                }
                PvtPos.CopyTo(this.Pos, 0);
                PvtTime.CopyTo(this.Time, 0);
                PvtPercent.CopyTo(this.Percent, 0);
                this.velBegin = startvel;
                this.PvtDataNum = count;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 读取TXT文件信息
        /// </summary>
        /// <param name="fileName"></param>
        public void File_ReadData(string fileName)
        {
            string strdata = System.IO.File.ReadAllText(fileName);
            ReadData(strdata);

        }
        public void m_Pvt()
        {
            rtn = GTN.mc.GTN_PrfPvt(CORE, Axis);

            rtn = GTN.mc.GTN_PvtTablePercent(CORE, Axis, this.PvtDataNum, ref this.Time[0], ref Pos[0], ref Percent[0], this.velBegin);

            rtn = GTN.mc.GTN_PvtTableSelect(CORE, Axis, Axis);

            rtn = GTN.mc.GTN_SetPvtLoop(CORE, Axis, 1);
        }
        public void PvtData(double s, double Maxvel, double acc)
        {
            double VelRun;//运行速度
            double t, t0, t1, t2;
            double s0, s1, s2;
            VelRun = Math.Min(Math.Pow(s * acc, 0.5), Maxvel);
            t0 = VelRun / acc;
            t2 = VelRun / acc;
            s0 = t0 * VelRun / 2;
            s2 = t2 * VelRun / 2;
            s1 = s - s0 - s2;
            t1 = s1 / VelRun;

        }



        public static (double[] q, double[] qd, double[] time,int numPoints) GenerateTrajectory(double q0, double q1, double v0, double v1, double vmax, double amax, double jmax, double softdt=0.01)
        {
            int sigma = Math.Sign(q1 - q0);

            // 得到规划参数  
            double[] para = STrajectoryPara(q0, q1, v0, v1, vmax, amax, jmax);
            double T = para[0] + para[1] + para[2]; // Ta + Tv + Td  

            // 时间向量  
            double dt = softdt; // 时间步长  
            int numPoints = (int)(T / dt) + 1; // 计算点的数量  
            double[] time = new double[numPoints];
            double[] q = new double[numPoints];
            double[] qd = new double[numPoints];
            double[] qdd = new double[numPoints];
            double[] qddd = new double[numPoints];
           
            // 计算位置、速度、加速度和加加速度  
            for (int i = 0; i < numPoints; i++)
            {
                time[i] = i * dt;
                q[i] = S_position(time[i], para);
                qd[i] = S_velocity(time[i], para);
                qdd[i] = S_acceleration(time[i], para);
                qddd[i] = S_jerk(time[i], para);
            }

            // 应用方向符号  
            for (int i = 0; i < numPoints; i++)
            {
                q[i] *= sigma;
                qd[i] *= sigma;
                qdd[i] *= sigma;
                qddd[i] *= sigma;
            }

            return (q, qd, time, numPoints);
        }
        #region 7段式速度规划时间分段算法
        /// <summary>
        /// 分段时间计算
        /// </summary>
        /// <param name="q0"></param>
        /// <param name="q1"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="vmax"></param>
        /// <param name="amax"></param>
        /// <param name="jmax"></param>
        /// <returns></returns>
        public static double[] STrajectoryPara(double q0, double q1, double v0, double v1, double vmax, double amax, double jmax)
        {
            // 计算规划参数  
            int sigma = Math.Sign(q1 - q0);
            double q_0 = sigma * q0;
            double q_1 = sigma * q1;
            double v_0 = sigma * v0;
            double v_1 = sigma * v1;

            // 速度和加速度限制  
            double v_min = -vmax;
            double v_max = ((sigma + 1) / 2) * vmax + ((sigma - 1) / 2) * v_min;
            double a_min = -amax;
            double a_max = ((sigma + 1) / 2) * amax + ((sigma - 1) / 2) * a_min;
            double j_min = -jmax;
            double j_max = ((sigma + 1) / 2) * jmax + ((sigma - 1) / 2) * j_min;

            // 计算加速和减速时间  
            var (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd) = CalculateAccelerationTimes(v_0, v_1, v_max, a_max, j_max, q_0, q_1);

            // 计算匀速段时间  
            double Tv = (q_1 - q_0) / v_max - (Ta / 2) * (1 + v_0 / v_max) - (Td / 2) * (1 + v_1 / v_max);

            double[] para;
            if (Tv > 0)
            {
                // 存在匀速阶段  
                para = new double[] { Ta, Tv, Td, Tj1, Tj2, q_0, q_1, v_0, v_1, v_max, a_max, a_min, a_lima, a_limd, j_max, j_min };
            }
            else
            {
                // 不存在匀速阶段  
                (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd) = HandleNoConstantSpeed(v_0, v_1, a_max, j_max, q_0, q_1);
                para = new double[] { Ta, 0, Td, Tj1, Tj2, q_0, q_1, v_0, v_1, vlim, a_max, a_min, a_lima, a_limd, j_max, j_min };
            }

            return para;
        }

        public static (double Ta, double Td, double Tj1, double Tj2, double vlim, double a_lima, double a_limd) CalculateAccelerationTimes(double v_0, double v_1, double v_max, double a_max, double j_max, double q_0, double q_1)
        {
            double Tj1, Ta, a_lima;
            if ((v_max - v_0) * j_max < a_max * a_max)
            {
                Tj1 = Math.Sqrt((v_max - v_0) / j_max); // 达不到a_max  
                Ta = 2 * Tj1;
                a_lima = j_max * Tj1;
            }
            else
            {
                Tj1 = a_max / j_max; // 能够达到a_max  
                Ta = Tj1 + (v_max - v_0) / a_max;
                a_lima = a_max;
            }

            double Tj2, Td, a_limd;
            if ((v_max - v_1) * j_max < a_max * a_max)
            {
                Tj2 = Math.Sqrt((v_max - v_1) / j_max); // 达不到a_min  
                Td = 2 * Tj2;
                a_limd = -j_max * Tj2;
            }
            else
            {
                Tj2 = a_max / j_max; // 能够达到a_min  
                Td = Tj2 + (v_max - v_1) / a_max;
                a_limd = -a_max;
            }

            // 确保 vlim 被赋值  
            double vlim = v_max; // 默认值  

            return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
        }

        public static (double Ta, double Td, double Tj1, double Tj2, double vlim, double a_lima, double a_limd) HandleNoConstantSpeed(double v_0, double v_1, double a_max, double j_max, double q_0, double q_1)
        {
            double Tv = 0;
            double Tj = a_max / j_max;
            double Tj1 = Tj;
            double Tj2 = Tj;
            double delta = (Math.Pow(a_max, 4) / Math.Pow(j_max, 2)) + 2 * (Math.Pow(v_0, 2) + Math.Pow(v_1, 2)) + a_max * (4 * (q_1 - q_0) - 2 * (a_max / j_max) * (v_0 + v_1));
            double Ta = ((Math.Pow(a_max, 2) / j_max) - 2.0 * v_0 + Math.Sqrt(delta)) / (2.0 * a_max);
            double Td = ((Math.Pow(a_max, 2) / j_max) - 2.0 * v_1 + Math.Sqrt(delta)) / (2.0 * a_max);

            double a_lima, a_limd, vlim;

            if (Ta < 0 || Td < 0)
            {
                if (Ta < 0)
                {
                    Ta = 0; Tj1 = 0;
                    Td = 2 * (q_1 - q_0) / (v_0 + v_1);
                    Tj2 = (j_max * (q_1 - q_0) - Math.Sqrt(j_max * (j_max * Math.Pow(q_1 - q_0, 2) + Math.Pow(v_1 + v_0, 2) * (v_1 - v_0)))) / (j_max * (v_1 + v_0));
                    a_lima = 0;
                    a_limd = -j_max * Tj2;
                    vlim = v_0;
                    return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
                }
                else if (Td < 0)
                {
                    Td = 0; Tj2 = 0;
                    Ta = 2 * (q_1 - q_0) / (v_0 + v_1);
                    Tj1 = (j_max * (q_1 - q_0) - Math.Sqrt(j_max * (j_max * Math.Pow(q_1 - q_0, 2)) - Math.Pow(v_1 + v_0, 2) * (v_1 - v_0))) / (j_max * (v_1 + v_0));
                    a_lima = j_max * Tj1;
                    a_limd = 0;
                    vlim = v_0 + a_lima * (Ta - Tj1);
                    return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
                }
            }
            else if (Ta >= 2 * Tj && Td >= 2 * Tj)
            {
                a_lima = a_max;
                a_limd = -a_max;
                vlim = v_0 + a_lima * (Ta - Tj);
                return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
            }
            else
            {
                double lambda = 0.99; // 系统取0<lambda<1  
                while (Ta < 2 * Tj || Td < 2 * Tj)
                {
                    a_max = lambda * a_max;
                    Tv = 0;
                    Tj = a_max / j_max;
                    Tj1 = Tj;
                    Tj2 = Tj;
                    delta = (Math.Pow(a_max, 4) / Math.Pow(j_max, 2)) + 2 * (Math.Pow(v_0, 2) + Math.Pow(v_1, 2)) + a_max * (4 * (q_1 - q_0) - 2 * (a_max / j_max) * (v_0 + v_1));
                    Ta = ((Math.Pow(a_max, 2) / j_max) - 2.0 * v_0 + Math.Sqrt(delta)) / (2.0 * a_max);
                    Td = ((Math.Pow(a_max, 2) / j_max) - 2.0 * v_1 + Math.Sqrt(delta)) / (2.0 * a_max);
                    if (Ta < 0 || Td < 0)
                    {
                        if (Ta < 0)
                        {
                            Ta = 0; Tj1 = 0;
                            Td = 2 * (q_1 - q_0) / (v_0 + v_1);
                            Tj2 = (j_max * (q_1 - q_0) - Math.Sqrt(j_max * (j_max * Math.Pow(q_1 - q_0, 2) + Math.Pow(v_1 + v_0, 2) * (v_1 - v_0)))) / (j_max * (v_1 + v_0));
                            a_lima = 0;
                            a_limd = -j_max * Tj2;
                            vlim = v_0;
                            return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
                        }
                        else if (Td < 0)
                        {
                            Td = 0; Tj2 = 0;
                            Ta = 2 * (q_1 - q_0) / (v_0 + v_1);
                            Tj1 = (j_max * (q_1 - q_0) - Math.Sqrt(j_max * (j_max * Math.Pow(q_1 - q_0, 2)) - Math.Pow(v_1 + v_0, 2) * (v_1 - v_0))) / (j_max * (v_1 + v_0));
                            a_lima = j_max * Tj1;
                            a_limd = 0;
                            vlim = v_0 + a_lima * (Ta - Tj1);
                            return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
                        }
                    }
                    else if (Ta >= 2 * Tj && Td >= 2 * Tj)
                    {
                        a_lima = a_max;
                        a_limd = -a_max;
                        vlim = v_0 + a_lima * (Ta - Tj);
                        return (Ta, Td, Tj1, Tj2, vlim, a_lima, a_limd);
                    }
                }
            }

            return (Ta, Td, Tj1, Tj2, 0, 0, 0); // 默认返回值  
        }
        #endregion
        #region 速度规划位置获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static double S_position(double t, double[] para)
        {
            // 提取参数  
            double Ta = para[0];
            double Tv = para[1];
            double Td = para[2];
            double Tj1 = para[3];
            double Tj2 = para[4];
            double q0 = para[5];
            double q1 = para[6];
            double v0 = para[7];
            double v1 = para[8];
            double vlim = para[9];
            double amax = para[10];
            double amin = para[11];
            double alima = para[12];
            double alimd = para[13];
            double jmax = para[14];
            double jmin = para[15];

            double T = Ta + Tv + Td;

            // 加速段  
            if (t < Tj1)
            {
                return q0 + v0 * t + jmax * Math.Pow(t, 3) / 6;
            }
            else if (t < Ta - Tj1)
            {
                return q0 + v0 * t + alima / 6 * (3 * Math.Pow(t, 2) - 3 * Tj1 * t + Math.Pow(Tj1, 2));
            }
            else if (t < Ta)
            {
                return q0 + (vlim + v0) * (Ta / 2) - vlim * (Ta - t) - jmin * Math.Pow((Ta - t), 3) / 6;
            }
            // 匀速段  
            else if (t < Ta + Tv)
            {
                return q0 + (vlim + v0) * (Ta / 2) + vlim * (t - Ta);
            }
            // 减速段  
            else if (t < T - Td + Tj2)
            {
                return q1 - (vlim + v1) * (Td / 2) + vlim * (t - T + Td) - jmax * Math.Pow((t - T + Td), 3) / 6;
            }
            else if (t < T - Tj2)
            {
                return q1 - (vlim + v1) * (Td / 2) + vlim * (t - T + Td) + (alimd / 6) * (3 * Math.Pow((t - T + Td), 2) - 3 * Tj2 * (t - T + Td) + Math.Pow(Tj2, 2));
            }
            else
            {
                return q1 - v1 * (T - t) - jmax * Math.Pow((T - t), 3) / 6;
            }
        }
        #endregion
        #region 速度规划速度获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static double S_velocity(double t, double[] para)
        {
            // 提取参数  
            double Ta = para[0];
            double Tv = para[1];
            double Td = para[2];
            double Tj1 = para[3];
            double Tj2 = para[4];
            double q0 = para[5];
            double q1 = para[6];
            double v0 = para[7];
            double v1 = para[8];
            double vlim = para[9];
            double amax = para[10];
            double amin = para[11];
            double alima = para[12];
            double alimd = para[13];
            double jmax = para[14];
            double jmin = para[15];

            double T = Ta + Tv + Td;

            // 计算速度  
            if (t < Tj1)
            {
                return v0 + jmax * Math.Pow(t, 2) / 2;
            }
            else if (t < Ta - Tj1)
            {
                return v0 + alima * (t - Tj1 / 2);
            }
            else if (t < Ta)
            {
                return vlim + jmin * Math.Pow((Ta - t), 2) / 2;
            }
            // 匀速段  
            else if (t < Ta + Tv)
            {
                return vlim;
            }
            // 减速段  
            else if (t < T - Td + Tj2)
            {
                return vlim - jmax * Math.Pow((t - T + Td), 2) / 2;
            }
            else if (t < T - Tj2)
            {
                return vlim + alimd * (t - T + Td - Tj2 / 2);
            }
            else
            {
                return v1 + jmax * Math.Pow((t - T), 2) / 2;
            }
        }
        #endregion

        #region 速度规划加速度获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static double S_acceleration(double t, double[] para)
        {
            // 提取参数  
            double Ta = para[0];
            double Tv = para[1];
            double Td = para[2];
            double Tj1 = para[3];
            double Tj2 = para[4];
            double q0 = para[5];
            double q1 = para[6];
            double v0 = para[7];
            double v1 = para[8];
            double vlim = para[9];
            double amax = para[10];
            double amin = para[11];
            double alima = para[12];
            double alimd = para[13];
            double jmax = para[14];
            double jmin = para[15];

            double T = Ta + Tv + Td;

            // 计算加速度  
            if (t < Tj1)
            {
                return jmax * t;
            }
            else if (t < Ta - Tj1)
            {
                return alima;
            }
            else if (t < Ta)
            {
                return -jmin * (Ta - t);
            }
            // 匀速段  
            else if (t < Ta + Tv)
            {
                return 0;
            }
            // 减速段  
            else if (t < T - Td + Tj2)
            {
                return -jmax * (t - T + Td);
            }
            else if (t < T - Tj2)
            {
                return alimd;
            }
            else
            {
                return -jmax * (T - t);
            }
        }
        #endregion
        #region 规划速度加加速度获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static double S_jerk(double t, double[] para)
        {
            // 提取参数  
            double Ta = para[0];
            double Tv = para[1];
            double Td = para[2];
            double Tj1 = para[3];
            double Tj2 = para[4];
            double q0 = para[5];
            double q1 = para[6];
            double v0 = para[7];
            double v1 = para[8];
            double vlim = para[9];
            double amax = para[10];
            double amin = para[11];
            double alima = para[12];
            double alimd = para[13];
            double jmax = para[14];
            double jmin = para[15];

            double T = Ta + Tv + Td;

            // 计算加加速度  
            if (t < Tj1)
            {
                return jmax;
            }
            else if (t < Ta - Tj1)
            {
                return 0;
            }
            else if (t < Ta)
            {
                return jmin;
            }
            // 匀速段  
            else if (t < Ta + Tv)
            {
                return 0;
            }
            // 减速段  
            else if (t < T - Td + Tj2)
            {
                return -jmax;
            }
            else if (t < T - Tj2)
            {
                return 0;
            }
            else
            {
                return jmax;
            }
        }
        #endregion

    }

    public class Interpolation  //插补部分
    {

        /// <summary>
        /// 建立二维插补坐标系
        /// </summary>
        /// <param name="synVelMax"></插补速度 单位脉冲毫秒param>
        /// <param name="synAccMax"></插补加速度 单位脉冲毫秒param>
        /// <param name="profile1"></插补轴1param>
        /// <param name="profile2"></插补轴1param>
        /// <param name="originPos1"></插补起始坐标1param>
        /// <param name="originPos2"></插补起始坐标2param>
        /// <param name="err"></param>
        public static bool  Set2DCoordinate(double synVelMax,double synAccMax  ,short profile1, short profile2, int originPos1,int originPos2,  out short error)
        {
            

            try
            {
                ///建立插补坐标系
                GTN.mc.TCrdPrm prm = new GTN.mc.TCrdPrm();
                // 定义为二维坐标系
                prm.dimension = 2;

                //最大合成速度  单位是pulse/ms
                prm.synVelMax = synVelMax;

                //最大合成加速度
                prm.synAccMax = synAccMax;

                //最小匀速时间
                prm.evenTime = 20;

                //轴的对应关系
                prm.profile1 = 0;
                prm.profile2 = 1;
                prm.profile3 = 2;
                prm.profile4 = 0;
                prm.profile5 = 0;
                prm.profile6 = 0;
                prm.profile7 = 0;
                prm.profile8 = 0; 

                //设置原点
                prm.setOriginFlag = 1;
                //原点坐标
                prm.originPos1 = originPos1;
                prm.originPos2 = originPos2;

                //建立1号坐标系
                error = GTN.mc.GTN_SetCrdPrm(1, 1, ref prm);
                if (error < 0)
                {
                    return false;
                    throw new Exception();
                
                }

            }
            catch (Exception )
            {
                error = -1;


            }

            return true;
        }


        /// <summary>
        /// 建立三维插补坐标系
        /// </summary>
        /// <param name="synVelMax"></插补速度 单位脉冲毫秒param>
        /// <param name="synAccMax"></插补加速度 单位脉冲毫秒param>
        /// <param name="profile1"></插补轴1param>
        /// <param name="profile2"></插补轴1param>
        /// <param name="originPos1"></插补起始坐标1param>
        /// <param name="originPos2"></插补起始坐标2param>
        /// <param name="err"></param>
        public static bool Set3DCoordinate(double synVelMax, double synAccMax, short profile1, short profile2, short profile3, int originPos1, int originPos2, int originPos3, out short error)
        {


            try
            {
                ///建立插补坐标系
                GTN.mc.TCrdPrm prm = new GTN.mc.TCrdPrm();
                // 定义为三维坐标系
                prm.dimension = 3;

                //最大合成速度  单位是pulse/ms
                prm.synVelMax = synVelMax;

                //最大合成加速度
                prm.synAccMax = synAccMax;

                //最小匀速时间
                prm.evenTime = 500;

                //轴的对应关系
                prm.profile1 = profile1;

                prm.profile2 = profile2;
                prm.profile3 = profile3;
                //设置原点
                prm.setOriginFlag = 1;
                //原点坐标
                prm.originPos1 = originPos1;
                prm.originPos2 = originPos2;
                prm.originPos3 = originPos3;

                //建立1号坐标系
                error = GTN.mc.GTN_SetCrdPrm(1, 1, ref prm);
                if (error < 0)
                {
                    return false;
                    throw new Exception();
                

                }

            }
            catch (Exception)
            {
                error = -1;


            }
            return true;

        }

        /// <summary>
        /// 清除缓存区内指令缓存
        /// </summary>
        /// <param name="core"></卡号param>
        /// <param name="crd"></坐标系号param>
        /// <param name="fifonum"></缓存区号param>
        /// <returns></returns>
        public static bool ClearCrdFifo( short core,short crd,short fifonum)
        {
            short err;
            err =  GTN.mc.GTN_CrdClear(core, crd, fifonum);
            if (err == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        /// <summary>
        /// 二维插补数据写入
        /// </summary>
        /// <param name="crd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="synVel"></param>
        /// <param name="synAcc"></param>
        /// <returns></returns>
        public static bool LnXYDataWrite(short crd, Int32 x, Int32 y, double synVel, double synAcc)
        {
            short err;
            err = GTN.mc.GTN_LnXY(1,crd, x,y, synVel, synAcc, 0,0);
            if (err == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// 三维插补数据写入
        /// </summary>
        /// <param name="crd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="synVel"></param>
        /// <param name="synAcc"></param>
        /// <returns></returns>
        public static bool LnXYZDataWrite(short crd, Int32 x, Int32 y, Int32 z, double synVel, double synAcc)
        {
            short err;
            err = GTN.mc.GTN_LnXYZ(1,crd, x, y,z, synVel, synAcc, 0, 0);
            if (err == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        /// <summary>
        /// 获取插补剩余空间
        /// </summary>
        /// <param name="crd"></param>
        /// <param name="pSpace"></param>
        public static void GetCrdSpace(short crd, out Int32 pSpace)
        {

            GTN.mc.GTN_CrdSpace(1, crd,out pSpace,0);

        }

        /// <summary>
        /// 启动坐标系运动
        /// </summary>
        /// <param name="crd"></param>
        /// <param name="fifo"></param>
        /// <returns></returns>
        public static bool StartCrdMove( )
        {
            short err;
            err =  GTN.mc.GTN_CrdStart(1, 1, 0);
            if (err == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取运动状态
        /// </summary>
        /// <param name="crd"></param>
        /// <param name="sts"></param>
        /// <param name="Finsh"></param>
        public static void GetCrdStatus(short crd, out short sts, out int  Finsh)
        {

            GTN.mc.GTN_CrdStatus(1, crd,out sts,out Finsh, 0);


        }


    }

}
