using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace GlobalToolClsLib
{
    // 错误类型枚举
    public enum ErrorType
    {
        System = 0,        // 系统错误
        Hardware = 1,      // 硬件设备错误
        Process = 2,       // 流程错误
        Operation = 3,     // 操作错误
        Communication = 4, // 通信错误
        Sensor = 5,        // 传感器错误
        Actuator = 6       // 执行器错误
    }

    // 错误严重级别
    public enum ErrorSeverity
    {
        Information,
        Warning,
        Error,
        Critical
    }

    // 错误信息类
    public class ErrorInfo
    {
        public string Code { get; set; }
        public ErrorType Type { get; set; }
        public ErrorSeverity Severity { get; set; }
        public DateTime OccurrenceTime { get; set; }
        public string AdditionalInfo { get; set; }
        public string Module { get; set; }

        public override string ToString()
        {
            return $"[{OccurrenceTime:yyyy-MM-dd HH:mm:ss}] {Code}: {ErrorManager.GetErrorDescription(Code)}";
        }
    }

    // 错误代码常量类
    public static class ErrorCodes
    {
        // 系统错误 (0xxxxx)
        public const string System_InitFailed = "000001";
        public const string System_MemoryAllocationFailed = "000002";
        public const string System_ConfigCorrupted = "000003";
        public const string System_DatabaseConnectionFailed = "000004";

        // 硬件设备错误 (1xxxxx)
        public const string Hardware_PowerSupplyFailed = "100001";
        public const string Hardware_MotorDriverFault = "100002";
        public const string Hardware_ControllerTimeout = "100003";

        // 流程错误 (2xxxxx)
        public const string Process_SequenceError = "200001";
        public const string Process_Timeout = "200002";
        public const string Process_InvalidState = "200003";

        // 操作错误 (3xxxxx)
        public const string Operation_InvalidParameter = "300001";
        public const string Operation_UnauthorizedAccess = "300002";
        public const string Operation_ManualInterventionRequired = "300003";

        // 通信错误 (4xxxxx)
        public const string Communication_PlcTimeout = "400001";
        public const string Communication_SerialPortError = "400002";
        public const string Communication_NetworkDisconnected = "400003";
        public const string Communication_ProtocolError = "400004";

        // 传感器错误 (5xxxxx)
        public const string Sensor_TemperatureAbnormal = "500001";
        public const string Sensor_PressureOutOfRange = "500002";
        public const string Sensor_DisplacementCalibrationFailed = "500003";
        public const string Sensor_LevelSensorFault = "500004";

        // 执行器错误 (6xxxxx)
        public const string Actuator_MotorOverload = "600001";
        public const string Actuator_CylinderTimeout = "600002";
        public const string Actuator_ValveMalfunction = "600003";
        public const string Actuator_RobotArmFault = "600004";
    }

    // 错误管理器
    public static class ErrorManager
    {
        private static readonly Dictionary<string, string> ErrorDescriptions;
        private static readonly Dictionary<string, string> ErrorSuggestions;
        private static readonly Dictionary<string, ErrorSeverity> ErrorSeverities;
        private static readonly Dictionary<string, ErrorType> ErrorTypes;

        private static List<ErrorInfo> _errorHistory = new List<ErrorInfo>();

        static ErrorManager()
        {
            // 初始化错误描述
            ErrorDescriptions = new Dictionary<string, string>
            {
                // 系统错误
                { ErrorCodes.System_InitFailed, "系统初始化过程中发生错误" },
                { ErrorCodes.System_MemoryAllocationFailed, "无法分配所需的内存资源" },
                { ErrorCodes.System_ConfigCorrupted, "配置文件损坏或格式不正确" },
                { ErrorCodes.System_DatabaseConnectionFailed, "数据库连接失败" },
                
                // 硬件设备错误
                { ErrorCodes.Hardware_PowerSupplyFailed, "电源供应异常" },
                { ErrorCodes.Hardware_MotorDriverFault, "电机驱动器故障" },
                { ErrorCodes.Hardware_ControllerTimeout, "控制器响应超时" },
                
                // 流程错误
                { ErrorCodes.Process_SequenceError, "工艺流程顺序错误" },
                { ErrorCodes.Process_Timeout, "流程执行超时" },
                { ErrorCodes.Process_InvalidState, "无效的系统状态" },
                
                // 操作错误
                { ErrorCodes.Operation_InvalidParameter, "参数设置无效" },
                { ErrorCodes.Operation_UnauthorizedAccess, "未授权访问尝试" },
                { ErrorCodes.Operation_ManualInterventionRequired, "需要手动干预" },
                
                // 通信错误
                { ErrorCodes.Communication_PlcTimeout, "与PLC的通信超时" },
                { ErrorCodes.Communication_SerialPortError, "串口通信发生错误" },
                { ErrorCodes.Communication_NetworkDisconnected, "网络连接已中断" },
                { ErrorCodes.Communication_ProtocolError, "通信协议错误" },
                
                // 传感器错误
                { ErrorCodes.Sensor_TemperatureAbnormal, "温度传感器读数异常" },
                { ErrorCodes.Sensor_PressureOutOfRange, "压力传感器读数超出正常范围" },
                { ErrorCodes.Sensor_DisplacementCalibrationFailed, "位移传感器校准失败" },
                { ErrorCodes.Sensor_LevelSensorFault, "液位传感器故障" },
                
                // 执行器错误
                { ErrorCodes.Actuator_MotorOverload, "电机检测到过载情况" },
                { ErrorCodes.Actuator_CylinderTimeout, "气缸动作超时未完成" },
                { ErrorCodes.Actuator_ValveMalfunction, "阀门响应异常或未按预期工作" },
                { ErrorCodes.Actuator_RobotArmFault, "机械臂运动异常" }
            };

            // 初始化错误处理建议
            ErrorSuggestions = new Dictionary<string, string>
            {
                // 系统错误
                { ErrorCodes.System_InitFailed, "检查系统依赖项并重新启动应用程序" },
                { ErrorCodes.System_MemoryAllocationFailed, "关闭不必要的应用程序并释放内存资源" },
                { ErrorCodes.System_ConfigCorrupted, "恢复备份配置文件或使用默认配置" },
                { ErrorCodes.System_DatabaseConnectionFailed, "检查数据库服务器状态和网络连接" },
                
                // 硬件设备错误
                { ErrorCodes.Hardware_PowerSupplyFailed, "检查电源线路和电源模块" },
                { ErrorCodes.Hardware_MotorDriverFault, "检查电机驱动器状态和连接" },
                { ErrorCodes.Hardware_ControllerTimeout, "检查控制器状态和重启控制器" },
                
                // 流程错误
                { ErrorCodes.Process_SequenceError, "检查工艺流程配置和顺序逻辑" },
                { ErrorCodes.Process_Timeout, "检查流程步骤是否正常完成" },
                { ErrorCodes.Process_InvalidState, "检查系统状态机逻辑" },
                
                // 操作错误
                { ErrorCodes.Operation_InvalidParameter, "检查输入参数的有效范围" },
                { ErrorCodes.Operation_UnauthorizedAccess, "验证用户权限和登录状态" },
                { ErrorCodes.Operation_ManualInterventionRequired, "操作员需要手动处理当前情况" },
                
                // 通信错误
                { ErrorCodes.Communication_PlcTimeout, "检查PLC电源和通信线路连接" },
                { ErrorCodes.Communication_SerialPortError, "检查串口连接和参数设置" },
                { ErrorCodes.Communication_NetworkDisconnected, "检查网络设备和电缆连接" },
                { ErrorCodes.Communication_ProtocolError, "检查通信协议配置和设备兼容性" },
                
                // 传感器错误
                { ErrorCodes.Sensor_TemperatureAbnormal, "检查传感器连接和环境温度" },
                { ErrorCodes.Sensor_PressureOutOfRange, "检查压力源和传感器校准" },
                { ErrorCodes.Sensor_DisplacementCalibrationFailed, "重新执行传感器校准程序" },
                { ErrorCodes.Sensor_LevelSensorFault, "检查传感器安装和清洁度" },
                
                // 执行器错误
                { ErrorCodes.Actuator_MotorOverload, "检查负载情况和电机驱动器设置" },
                { ErrorCodes.Actuator_CylinderTimeout, "检查气源压力和气缸机械结构" },
                { ErrorCodes.Actuator_ValveMalfunction, "检查电磁阀电源和气压供应" },
                { ErrorCodes.Actuator_RobotArmFault, "检查机械臂限位和运动轨迹" }
            };

            // 初始化错误严重级别
            ErrorSeverities = new Dictionary<string, ErrorSeverity>
            {
                // 系统错误
                { ErrorCodes.System_InitFailed, ErrorSeverity.Critical },
                { ErrorCodes.System_MemoryAllocationFailed, ErrorSeverity.Error },
                { ErrorCodes.System_ConfigCorrupted, ErrorSeverity.Error },
                { ErrorCodes.System_DatabaseConnectionFailed, ErrorSeverity.Error },
                
                // 硬件设备错误
                { ErrorCodes.Hardware_PowerSupplyFailed, ErrorSeverity.Critical },
                { ErrorCodes.Hardware_MotorDriverFault, ErrorSeverity.Error },
                { ErrorCodes.Hardware_ControllerTimeout, ErrorSeverity.Error },
                
                // 流程错误
                { ErrorCodes.Process_SequenceError, ErrorSeverity.Error },
                { ErrorCodes.Process_Timeout, ErrorSeverity.Warning },
                { ErrorCodes.Process_InvalidState, ErrorSeverity.Error },
                
                // 操作错误
                { ErrorCodes.Operation_InvalidParameter, ErrorSeverity.Warning },
                { ErrorCodes.Operation_UnauthorizedAccess, ErrorSeverity.Warning },
                { ErrorCodes.Operation_ManualInterventionRequired, ErrorSeverity.Information },
                
                // 通信错误
                { ErrorCodes.Communication_PlcTimeout, ErrorSeverity.Error },
                { ErrorCodes.Communication_SerialPortError, ErrorSeverity.Warning },
                { ErrorCodes.Communication_NetworkDisconnected, ErrorSeverity.Error },
                { ErrorCodes.Communication_ProtocolError, ErrorSeverity.Error },
                
                // 传感器错误
                { ErrorCodes.Sensor_TemperatureAbnormal, ErrorSeverity.Warning },
                { ErrorCodes.Sensor_PressureOutOfRange, ErrorSeverity.Error },
                { ErrorCodes.Sensor_DisplacementCalibrationFailed, ErrorSeverity.Warning },
                { ErrorCodes.Sensor_LevelSensorFault, ErrorSeverity.Error },
                
                // 执行器错误
                { ErrorCodes.Actuator_MotorOverload, ErrorSeverity.Error },
                { ErrorCodes.Actuator_CylinderTimeout, ErrorSeverity.Warning },
                { ErrorCodes.Actuator_ValveMalfunction, ErrorSeverity.Error },
                { ErrorCodes.Actuator_RobotArmFault, ErrorSeverity.Error }
            };

            // 初始化错误类型映射
            ErrorTypes = new Dictionary<string, ErrorType>
            {
                // 系统错误
                { ErrorCodes.System_InitFailed, ErrorType.System },
                { ErrorCodes.System_MemoryAllocationFailed, ErrorType.System },
                { ErrorCodes.System_ConfigCorrupted, ErrorType.System },
                { ErrorCodes.System_DatabaseConnectionFailed, ErrorType.System },
                
                // 硬件设备错误
                { ErrorCodes.Hardware_PowerSupplyFailed, ErrorType.Hardware },
                { ErrorCodes.Hardware_MotorDriverFault, ErrorType.Hardware },
                { ErrorCodes.Hardware_ControllerTimeout, ErrorType.Hardware },
                
                // 流程错误
                { ErrorCodes.Process_SequenceError, ErrorType.Process },
                { ErrorCodes.Process_Timeout, ErrorType.Process },
                { ErrorCodes.Process_InvalidState, ErrorType.Process },
                
                // 操作错误
                { ErrorCodes.Operation_InvalidParameter, ErrorType.Operation },
                { ErrorCodes.Operation_UnauthorizedAccess, ErrorType.Operation },
                { ErrorCodes.Operation_ManualInterventionRequired, ErrorType.Operation },
                
                // 通信错误
                { ErrorCodes.Communication_PlcTimeout, ErrorType.Communication },
                { ErrorCodes.Communication_SerialPortError, ErrorType.Communication },
                { ErrorCodes.Communication_NetworkDisconnected, ErrorType.Communication },
                { ErrorCodes.Communication_ProtocolError, ErrorType.Communication },
                
                // 传感器错误
                { ErrorCodes.Sensor_TemperatureAbnormal, ErrorType.Sensor },
                { ErrorCodes.Sensor_PressureOutOfRange, ErrorType.Sensor },
                { ErrorCodes.Sensor_DisplacementCalibrationFailed, ErrorType.Sensor },
                { ErrorCodes.Sensor_LevelSensorFault, ErrorType.Sensor },
                
                // 执行器错误
                { ErrorCodes.Actuator_MotorOverload, ErrorType.Actuator },
                { ErrorCodes.Actuator_CylinderTimeout, ErrorType.Actuator },
                { ErrorCodes.Actuator_ValveMalfunction, ErrorType.Actuator },
                { ErrorCodes.Actuator_RobotArmFault, ErrorType.Actuator }
            };
        }

        // 获取错误描述
        public static string GetErrorDescription(string code)
        {
            return ErrorDescriptions.TryGetValue(code, out var description)
                ? description
                : "未知错误";
        }

        // 获取错误处理建议
        public static string GetErrorSuggestion(string code)
        {
            return ErrorSuggestions.TryGetValue(code, out var suggestion)
                ? suggestion
                : "请查看系统日志获取更多信息";
        }

        // 获取错误严重级别
        public static ErrorSeverity GetErrorSeverity(string code)
        {
            return ErrorSeverities.TryGetValue(code, out var severity)
                ? severity
                : ErrorSeverity.Error;
        }

        // 获取错误类型
        public static ErrorType GetErrorType(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length < 1)
                return ErrorType.System;

            // 从错误码第一位获取错误类型
            char typeChar = code[0];
            if (char.IsDigit(typeChar))
            {
                int typeValue = int.Parse(typeChar.ToString());
                if (Enum.IsDefined(typeof(ErrorType), typeValue))
                    return (ErrorType)typeValue;
            }

            return ErrorType.System;
        }

        // 获取错误类型名称
        public static string GetErrorTypeName(ErrorType type)
        {
            switch (type)
            {
                case ErrorType.System: return "系统错误";
                case ErrorType.Hardware: return "硬件设备错误";
                case ErrorType.Process: return "流程错误";
                case ErrorType.Operation: return "操作错误";
                case ErrorType.Communication: return "通信错误";
                case ErrorType.Sensor: return "传感器错误";
                case ErrorType.Actuator: return "执行器错误";
                default: return "未知错误类型";
            }
        }

        // 记录错误
        public static void LogError(string code, string module, string additionalInfo = "")
        {
            var errorInfo = new ErrorInfo
            {
                Code = code,
                Type = GetErrorType(code),
                Severity = GetErrorSeverity(code),
                OccurrenceTime = DateTime.Now,
                AdditionalInfo = additionalInfo,
                Module = module
            };

            _errorHistory.Add(errorInfo);

            // 在实际应用中，这里还可以添加日志写入文件或数据库的逻辑
            Console.WriteLine($"错误记录: {errorInfo}");

            // 根据错误严重级别决定是否需要通知用户或执行紧急操作
            if (errorInfo.Severity >= ErrorSeverity.Error)
            {
                // 触发错误通知事件
                OnErrorOccurred?.Invoke(null, errorInfo);
            }
        }

        // 获取错误历史
        public static List<ErrorInfo> GetErrorHistory()
        {
            return _errorHistory;
        }

        // 按类型筛选错误
        public static List<ErrorInfo> GetErrorsByType(ErrorType type)
        {
            return _errorHistory.Where(e => e.Type == type).ToList();
        }

        // 按严重级别筛选错误
        public static List<ErrorInfo> GetErrorsBySeverity(ErrorSeverity severity)
        {
            return _errorHistory.Where(e => e.Severity == severity).ToList();
        }

        // 错误发生事件
        public static event EventHandler<ErrorInfo> OnErrorOccurred;
    }

}