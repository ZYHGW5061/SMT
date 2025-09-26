using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DispensingMachineControllerClsLib
{
    public enum MUSASHICommandenum
    {
        吐出要求,
        STEP,
        DOWN,
        PRESET,
        TIMED模式切换,
        MANUAL模式切换,
        吐出压力监视测试,
        通道加载,
        通道复制,
        品种加载,
        品种删除,
        诊断功能,
        温度调节自动调谐,
        用户计数器清除,
        错误日志清除,
        仅初始化参数,
        消除所有记忆,
        满量采样,
        空采样,
        详细采样,
        解除Σ模式,
        ECO模式,
        电磁阀报警信号OFF,

        吐出条件,
        延迟时间,
        吐出压力监视功能,
        通道名,
        吐出辅助,
        Σ功能水头差自动校正,
        Σ功能自动防滴漏,
        Σ功能自动剩余量警告,
        Σ功能剩余量校正,
        品种,
        自动递增功能,
        品种详情,
        温度调节条件,
        吐出条件补正,
        间隔功能,
        I_O信号,
        环境,
        警告功能,
        密码锁定,
        输入信号分配,
        输出信号分配,
        日期时间,
        时区,
        机体名称,
        IP地址,
        子网掩码,
        默认网关,
        管理员密码,
        网页更新间隔,
        只读,

        获取吐出条件,
        获取延迟时间,
        获取吐出压力监视功能,
        获取通道名,
        获取吐出辅助,
        获取Σ功能水头差自动校正,
        获取Σ功能自动防滴漏,
        获取Σ功能自动剩余量警告,
        获取Σ功能剩余量校正,
        获取品种,
        获取自动递增功能,
        获取品种详情,
        获取温度调节条件,
        获取吐出条件补正,
        获取间隔功能,
        获取I_O信号,
        获取环境,
        获取警告功能,
        获取密码锁定,
        获取输入信号分配,
        获取输出信号分配,
        获取日期时间,
        获取时区,
        获取机体名称,
        获取IP地址,
        获取子网掩码,
        获取默认网关,
        获取管理员密码,
        获取网页更新间隔,
        获取只读,

        获取吐出状态,
        获取供气压力,
        获取秒表,
        获取当前通道,
        获取当前品种,
        获取自动递增状态,
        获取吐出压力监视测试状态,
        获取余量检测状态,
        获取温度调节状态,
        获取吐出条件补正后的值,
        获取间隔状态,
        获取计数器,
        获取诊断状态,
        获取温度调节自动调谐,
        获取错误日志总数,
        获取错误日志_主_,
        获取错误日志_子_,
        获取D_sub25输出状态,

        获取MAC地址,
        获取I_O规格,
        获取命令信息,
        获取版本信息
    }


    public static class MUSASHICommand
    {
        /// <summary>  
        /// 根据指令枚举获取对应的协议代码  
        /// </summary>  
        /// <param name="command">指令枚举值</param>  
        /// <returns>四位数字符串代码</returns>  
        /// <exception cref="KeyNotFoundException">当枚举值未在字典中注册时抛出</exception>  
        public static string GetCommandCode(MUSASHICommandenum command)
        {
            // 处理特殊符号替换（枚举命名规则限制）  
            var key = command.ToString()
                .Replace("_O", "/O")
                .Replace("_sub", "-sub")
                .Replace("_主_", "（主）")
                .Replace("_子_", "（子）");

            return MUSASHICommand.Codes[key];
        }
        public static readonly Dictionary<string, string> Codes = new Dictionary<string, string>
    {  
        // 各类操作命令 (0000-0022)  
        { "吐出要求", "0000" },
        { "STEP", "0001" },
        { "DOWN", "0002" },
        { "PRESET", "0003" },
        { "TIMED模式切换", "0004" },
        { "MANUAL模式切换", "0005" },
        { "吐出压力监视测试", "0006" },
        { "通道加载", "0007" },
        { "通道复制", "0008" },
        { "品种加载", "0009" },
        { "品种删除", "0010" },
        { "诊断功能", "0011" },
        { "温度调节自动调谐", "0012" },
        { "用户计数器清除", "0013" },
        { "错误日志清除", "0014" },
        { "仅初始化参数", "0015" },
        { "消除所有记忆", "0016" },
        { "满量采样", "0017" },
        { "空采样", "0018" },
        { "详细采样", "0019" },
        { "解除Σ模式", "0020" },
        { "ECO模式", "0021" },
        { "电磁阀报警信号OFF", "0022" },  

        // 参数设置命令 (1000-1029)  
        { "吐出条件", "1000" },
        { "延迟时间", "1001" },
        { "吐出压力监视功能", "1002" },
        { "通道名", "1003" },
        { "吐出辅助", "1004" },
        { "Σ功能水头差自动校正", "1005" },
        { "Σ功能自动防滴漏", "1006" },
        { "Σ功能自动剩余量警告", "1007" },
        { "Σ功能剩余量校正", "1008" },
        { "品种", "1009" },
        { "自动递增功能", "1010" },
        { "品种详情", "1011" },
        { "温度调节条件", "1012" },
        { "吐出条件补正", "1013" },
        { "间隔功能", "1014" },
        { "I/O信号", "1015" },
        { "环境", "1016" },
        { "警告功能", "1017" },
        { "密码锁定", "1018" },
        { "输入信号分配", "1019" },
        { "输出信号分配", "1020" },
        { "日期时间", "1021" },
        { "时区", "1022" },
        { "机体名称", "1023" },
        { "IP地址", "1024" },
        { "子网掩码", "1025" },
        { "默认网关", "1026" },
        { "管理员密码", "1027" },
        { "网页更新间隔", "1028" },
        { "只读", "1029" },  

        // 获取参数命令 (2000-2029)  
        { "获取吐出条件", "2000" },
        { "获取延迟时间", "2001" },
        { "获取吐出压力监视功能", "2002" },
        { "获取通道名", "2003" },
        { "获取吐出辅助", "2004" },
        { "获取Σ功能水头差自动校正", "2005" },
        { "获取Σ功能自动防滴漏", "2006" },
        { "获取Σ功能自动剩余量警告", "2007" },
        { "获取Σ功能剩余量校正", "2008" },
        { "获取品种", "2009" },
        { "获取自动递增功能", "2010" },
        { "获取品种详情", "2011" },
        { "获取温度调节条件", "2012" },
        { "获取吐出条件补正", "2013" },
        { "获取间隔功能", "2014" },
        { "获取I/O信号", "2015" },
        { "获取环境", "2016" },
        { "获取警告功能", "2017" },
        { "获取密码锁定", "2018" },
        { "获取输入信号分配", "2019" },
        { "获取输出信号分配", "2020" },
        { "获取日期时间", "2021" },
        { "获取时区", "2022" },
        { "获取机体名称", "2023" },
        { "获取IP地址", "2024" },
        { "获取子网掩码", "2025" },
        { "获取默认网关", "2026" },
        { "获取管理员密码", "2027" },
        { "获取网页更新间隔", "2028" },
        { "获取只读", "2029" },  

        // 获取设备信息命令 (3000-3017)  
        { "获取吐出状态", "3000" },
        { "获取供气压力", "3001" },
        { "获取秒表", "3002" },
        { "获取当前通道", "3003" },
        { "获取当前品种", "3004" },
        { "获取自动递增状态", "3005" },
        { "获取吐出压力监视测试状态", "3006" },
        { "获取余量检测状态", "3007" },
        { "获取温度调节状态", "3008" },
        { "获取吐出条件补正后的值", "3009" },
        { "获取间隔状态", "3010" },
        { "获取计数器", "3011" },
        { "获取诊断状态", "3012" },
        { "获取温度调节自动调谐", "3013" },
        { "获取错误日志总数", "3014" },
        { "获取错误日志（主）", "3015" },
        { "获取错误日志（子）", "3016" },
        { "获取D-sub25输出状态", "3017" },  

        // 获取其他信息命令 (9996-9999)  
        { "获取MAC地址", "9996" },
        { "获取I/O规格", "9997" },
        { "获取命令信息", "9998" },
        { "获取版本信息", "9999" }
    };
    }



    public class MUSASHI : IDispensingMachineController
    {
        SerialPort PowerControl = new SerialPort();
        private readonly object _lock = new object();
        private DispensingMachineControllerConfig _config = null;
        bool coms = false;
        public int PLCadd = 1;
        private const int Timeout = 1000;

        public MUSASHI(DispensingMachineControllerConfig config)
        {
            _config = config;
        }

        public bool IsConnect { get; set; } = false;



        #region Private Method


        private int ByteToInt(byte[] BTData)
        {
            try
            {
                if (BTData != null)
                {
                    int offset = BTData.Length;
                    int Data = 0;
                    for (int i = 0; i < offset; i++)
                    {
                        Data = Data + (int)((BTData[i] & 0xFF) << 8 * (offset - i - 1));
                    }
                    //Data = (int)((BTData[offset] & 0xFF)
                    //| ((BTData[offset + 1] & 0xFF) << 8)
                    //| ((BTData[offset + 2] & 0xFF) << 16)
                    //| ((BTData[offset + 3] & 0xFF) << 24));
                    return Data;
                }
                else
                    return -1;

            }
            catch (Exception ex) { return -1; }

        }
        private static byte[] CRC16(byte[] value, int Length, ushort poly = 0xA001, ushort crcInit = 0xFFFF)
        {
            if (value == null || !value.Any())
                throw new ArgumentException("");

            //运算
            ushort crc = crcInit;
            for (int i = 0; i < Length; i++)
            {
                crc = (ushort)(crc ^ (value[i]));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ poly) : (ushort)(crc >> 1);
                }
            }
            byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
            byte lo = (byte)(crc & 0x00FF);         //低位置

            byte[] buffer = new byte[2];
            //buffer.AddRange(value);
            buffer[0] = lo;//低字节
            buffer[1] = hi;//高字节

            return buffer;
        }
        /// <summary>
        /// 连接点胶机
        /// </summary>
        /// <param name="com"></param>
        /// <param name="baudrate"></param>
        /// <param name="Databits"></param>
        /// <param name="Stopbits"></param>
        /// <param name="parity"></param>
        /// <returns></returns>
        private int SerialConnect(string com, int baudrate = 115200, int Databits = 8, int Stopbits = 1, int parity = 0)
        {

            PowerControl.PortName = com;//设置端口名
            PowerControl.BaudRate = baudrate;//设置波特率
            PowerControl.DataBits = Databits;
            switch (Stopbits)
            {
                case 0:
                    PowerControl.StopBits = StopBits.None;
                    break;
                case 1:
                    PowerControl.StopBits = StopBits.One;
                    break;
                case 2:
                    PowerControl.StopBits = StopBits.Two;
                    break;
            }
            switch (parity)
            {
                case 0:
                    PowerControl.Parity = Parity.None;
                    break;
                case 1:
                    PowerControl.Parity = Parity.Odd;
                    break;
                case 2:
                    PowerControl.Parity = Parity.Even;
                    break;
                case 3:
                    PowerControl.Parity = Parity.Mark;
                    break;
            }

            PowerControl.ReadTimeout = 2000;
            PowerControl.WriteTimeout = 2000;
            try
            {
                if (!PowerControl.IsOpen)
                {
                    PowerControl.Open();//打开串口
                    coms = true;
                    //loop_back();
                    //PLC_state();
                }
                else
                {
                    PowerControl.Close();//关闭串口
                    PowerControl.Open();//打开串口
                    coms = true;
                }
                IsConnect = true;
            }
            catch
            {
                coms = false;
                IsConnect = false;
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// 连接点胶机
        /// </summary>
        /// <returns></returns>
        private int SerialConnect(SerialPort PowerControl)
        {
            if(PowerControl.PortName != $"COM{_config.SerialCommunicator.Port}")
            {
                PowerControl.PortName = $"COM{_config.SerialCommunicator.Port}";//设置端口名
                PowerControl.BaudRate = _config.SerialCommunicator.BaudRate;//设置波特率
                PowerControl.DataBits = _config.SerialCommunicator.DataBits;
                switch ((int)_config.SerialCommunicator.StopBits)
                {
                    case 0:
                        PowerControl.StopBits = StopBits.None;
                        break;
                    case 1:
                        PowerControl.StopBits = StopBits.One;
                        break;
                    case 2:
                        PowerControl.StopBits = StopBits.Two;
                        break;
                }
                switch ((int)_config.SerialCommunicator.Parity)
                {
                    case 0:
                        PowerControl.Parity = Parity.None;
                        break;
                    case 1:
                        PowerControl.Parity = Parity.Odd;
                        break;
                    case 2:
                        PowerControl.Parity = Parity.Even;
                        break;
                    case 3:
                        PowerControl.Parity = Parity.Mark;
                        break;
                }
            }

            PowerControl.ReadTimeout = 2000;
            PowerControl.WriteTimeout = 2000;
            this.PowerControl = PowerControl;
            try
            {
                if (!PowerControl.IsOpen)
                {
                    PowerControl.Open();//打开串口
                    coms = true;
                    //loop_back();
                    //PLC_state();
                }
                else
                {
                    //PowerControl.Close();//关闭串口
                    //PowerControl.Open();//打开串口
                    coms = true;
                }
                IsConnect = true;
            }
            catch
            {
                coms = false;
                IsConnect = false;
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// 断开点胶机
        /// </summary>
        /// <returns></returns>
        private int SerialDisconnect()
        {
            try
            {
                if (PowerControl.IsOpen)
                {
                    PowerControl.Close();//关闭串口
                    coms = false;
                    IsConnect = false;
                }
            }
            catch
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// 读取节点信息
        /// </summary>
        /// <param name="Add"></param>
        /// <param name="num"></param>
        private byte[] PCread(int PLCadd, int Add, int num)
        {
            try
            {
                if (coms)
                {
                    int length = 8;
                    byte[] data = new byte[length];

                    data[0] = Convert.ToByte(PLCadd);
                    data[1] = 0x04;
                    byte[] B0 = BitConverter.GetBytes(Add);
                    byte[] B1 = BitConverter.GetBytes(num);
                    data[2] = B0[1];
                    data[3] = B0[0];
                    data[4] = B1[1];
                    data[5] = B1[0];
                    byte[] data1 = CRC16(data, 6);

                    data[6] = data1[0];
                    data[7] = data1[1];


                    PowerControl.DiscardInBuffer();
                    PowerControl.DiscardOutBuffer();
                    PowerControl.Write(data, 0, length);
                    System.Threading.Thread.Sleep(100);
                    length = PowerControl.BytesToRead;
                    //length = 15;
                    data = new byte[length];
                    PowerControl.Read(data, 0, length);

                    if (length > 0)
                    {
                        if (data[1] == 0x04)
                        {
                            byte[] Data0 = data.Skip(3).Take(length - 5).ToArray();
                            return Data0;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 写入节点信息
        /// </summary>
        /// <param name="Add"></param>
        /// <param name="num"></param>
        private void PCwrite(int PLCadd, int Add, int Data)
        {
            try
            {
                if (coms)
                {
                    int length = 8;
                    byte[] data = new byte[length];

                    data[0] = Convert.ToByte(PLCadd);
                    data[1] = 0x06;
                    byte[] B0 = BitConverter.GetBytes(Add);
                    byte[] B1 = BitConverter.GetBytes(Data);
                    data[2] = B0[1];
                    data[3] = B0[0];
                    data[4] = B1[1];
                    data[5] = B1[0];
                    byte[] data1 = CRC16(data, 6);

                    data[6] = data1[0];
                    data[7] = data1[1];


                    PowerControl.DiscardInBuffer();
                    PowerControl.DiscardOutBuffer();
                    PowerControl.Write(data, 0, length);
                    System.Threading.Thread.Sleep(100);
                    length = PowerControl.BytesToRead;
                    //length = 15;
                    data = new byte[length];
                    PowerControl.Read(data, 0, length);
                }
                else
                {
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        // 校验和计算（Type A协议）  
        private string CalculateChecksum(string data)
        {
            byte sum = 0;
            foreach (char c in data)
            {
                sum += (byte)c;
            }
            return ((byte)~sum).ToString("X2");
        }

        // 构建请求数据包（Type A协议）  
        public string BuildCommand(string command, params string[] parameters)
        {
            var sb = new StringBuilder($"M,{command},");
            foreach (var param in parameters)
            {
                sb.Append($"{param},");
            }
            string data = sb.ToString();
            return $"{data}{CalculateChecksum(data)}\r\n";
        }

        // 发送命令并接收响应  
        public string SendCommand(string command)
        {
            lock (_lock)
            {
                if (!PowerControl.IsOpen) PowerControl.Open();

                PowerControl.DiscardInBuffer();
                PowerControl.Write(command);

                DateTime start = DateTime.Now;
                StringBuilder response = new StringBuilder();

                while ((DateTime.Now - start).TotalMilliseconds < Timeout)
                {
                    if (PowerControl.BytesToRead > 0)
                    {
                        response.Append(PowerControl.ReadExisting());
                        if (response.ToString().EndsWith("\r\n"))
                            break;
                    }
                    Thread.Sleep(10);
                }
                return response.ToString().Trim();
            }
        }






        #endregion

        /// <summary>
        /// 指令
        /// </summary>
        /// <returns></returns>
        public bool Set(MUSASHICommandenum command, params string[] parameters)
        {
            DataModel.Instance.EpoxtsIsWriting = true;
            if (DataModel.Instance.EpoxtsIsReading)
            {
                Thread.Sleep(20);
            }

            string command0 = BuildCommand(MUSASHICommand.GetCommandCode(command),
                parameters);

            string response = SendCommand(command0);
            DataModel.Instance.EpoxtsIsWriting = false;
            // 解析响应格式：A0,CS\r\n  
            return response.StartsWith("A0");
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public List<decimal> Get(MUSASHICommandenum command, params string[] parameters)
        {
            if (DataModel.Instance.EpoxtsIsWriting)
            {
                return null;
            }
            DataModel.Instance.EpoxtsIsReading = true;
            string command0 = BuildCommand(MUSASHICommand.GetCommandCode(command),
                parameters);

            string response = SendCommand(command0);

            DataModel.Instance.EpoxtsIsReading = false;

            // 响应格式示例：A0,1234,1111,CS\r\n  
            if (!response.StartsWith("A0"))
                return null;
            //throw new InvalidOperationException($"Error response: {response}");

            string[] parts = response.Split(',');
            string[] newParts = parts.Skip(1).ToArray();
            List<decimal> data = new List<decimal>();
            foreach (var part in newParts)
            {
                if (part != "")
                    data.Add(Convert.ToDecimal(part));
            }
            return data;
        }


        /// <summary>
        /// 吐出条件 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="time"></param>
        /// <param name="pressure"></param>
        /// <param name="vacuum"></param>
        /// <returns></returns>
        public bool SpittingOutConditions(int channel, decimal mode, decimal time, decimal pressure, decimal vacuum)
        {
            // 参数格式验证  
            if (channel < 1 || channel > 400) throw new ArgumentOutOfRangeException(nameof(channel));
            if (mode < 0 || mode > 1) throw new ArgumentOutOfRangeException(nameof(mode));
            if (time < 0.01m || time > 9999.999m) throw new ArgumentOutOfRangeException(nameof(time));
            if (pressure < 30 || pressure > 500.0m) throw new ArgumentOutOfRangeException(nameof(pressure));
            if (pressure > 0 || pressure < -5.0m) throw new ArgumentOutOfRangeException(nameof(vacuum));

            string command = BuildCommand(MUSASHICommand.Codes["吐出条件"],
                channel.ToString(),
                mode.ToString(),
                time.ToString("0.000"),
                pressure.ToString("0.0"),
                vacuum.ToString("0.0"));

            string response = SendCommand(command);

            // 解析响应格式：A0,CS\r\n  
            return response.StartsWith("A0");
        }

        /// <summary>
        /// 获取吐出条件
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public (decimal Time, decimal Pressure, decimal Vacuum) ReadDispensingParameters(int channel)
        {
            string command = BuildCommand(MUSASHICommand.Codes["获取吐出条件"], channel.ToString());
            string response = SendCommand(command);

            // 响应格式示例：A0,1234,1111,CS\r\n  
            if (!response.StartsWith("A0"))
                throw new InvalidOperationException($"Error response: {response}");

            string[] parts = response.Split(',');
            if(parts.Length > 6 && Convert.ToInt32(parts[1]) == channel)
            {
                return (
                Time: decimal.Parse(parts[3]),  // 将1234转换为1.234  
                Pressure: decimal.Parse(parts[4]), // 将1111转换为11.1  
                Vacuum: decimal.Parse(parts[5]));// 将1111转换为11.1  
            }
            else
            {
                return (-1,-1,-1);
            }
            
            
        }

        public void Connect()
        {
            SerialConnect($"COM{_config.SerialCommunicator.Port}", _config.SerialCommunicator.BaudRate
                , _config.SerialCommunicator.DataBits, (int)_config.SerialCommunicator.StopBits, (int)_config.SerialCommunicator.Parity);
            PLCadd = _config.SerialCommunicator.DeviceAddress;
        }
        public void Connect(SerialPort serial)
        {
            SerialConnect(serial);
            PLCadd = _config.SerialCommunicator.DeviceAddress;
        }

        public void Disconnect()
        {
            SerialDisconnect();
        }

        public void Write()
        {
            throw new NotImplementedException();
        }

        public void Shot()
        {
            Set(DispensingMachineControllerClsLib.MUSASHICommandenum.DOWN, "");
        }

        public List<string> GetRecipeList()
        {
            throw new NotImplementedException();
        }

        public void LoadRecipe(string recipeName)
        {
            throw new NotImplementedException();
        }

        public DispenseRecipeInfo GetRecipeInfo(string recipeName)
        {
            throw new NotImplementedException();
        }

        public int GetShotCounter()
        {
            throw new NotImplementedException();
        }

        public float GetRemainingQuantity()
        {
            throw new NotImplementedException();
        }
    }
}
