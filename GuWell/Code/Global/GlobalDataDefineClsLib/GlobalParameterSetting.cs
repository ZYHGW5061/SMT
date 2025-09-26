using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GlobalDataDefineClsLib
{
    public static class GlobalParameterSetting
    {
        public const string SYSTEM_DEBUG_LOGGER_ID = "SystemGlobalLogger";
        public const double GOLDEN_POINT = 0.618;
        public const string RIGHT_BRACE = "}";
        public const string LEFT_BRACE = "{";
        public const string RIGHT_SQUARE_BRACKET = "]";
        public const string RIGHT_ANGLE_BRACKET = ">";
        public const string LEFT_ANGLE_BRACKET = "<";
        public const string LEFT_SQUARE_BRACKET = "[";
        public const string LEFT_PARENTTHESIS = "(";
        public const string SYSTEM_MONITOR_LOGGER_ID = "SystemMonitorLogger";
        public const string HARDWARE_PLC_LOGGER_ID = "HardwarePLCLogger";
        public const string SYSTEM_PLC_LOGGER_ID = "SystemPLCLogger";
        public const string SYSTEM_OPERATION_DBLOGGER_ID = "UserOperationLogger";
        public const string RIGHT_PARENTTHESIS = ")";
        public static readonly string CONFIG_FILE_FULLNAME;
        public static readonly string SYSTEM_STARTUP_INFO_FILE_FULLNAME;
        public static readonly string WAFERJOB_LOG_FILE_DEFAULT_DIR;
        public static readonly string LOG_FILE_OPERATION_DIR;
        public static readonly string LOG_FILE_EXCEPTION_DIR;
        public static readonly string CONFIG_FILE_FULLNAME_MASTERCONTROL;
        public static readonly string CHANNEL_CONFIG_FILE_DEFAULT_DIR;
        public static readonly string BUSINESS_GOLDEN_TEMP_DIR;
        public static readonly string CONFIG_FILE_DEFAULT_DIR;
        public static readonly string JOB_REPORT_TEMPLATE_PATH_HISTORY;
        public static readonly string JOB_REPORT_TEMPLATE_PATH;
        public static readonly string JOB_REPORT_IMAGE_PATH;
        public static readonly string DEBUG_OUTPUT_DIR;
        public static readonly string SYSTEM_DEFAULT_DIR;
        public static readonly string CONFIG_FILE_DEFAULT_DIR_MASTERCONTROL;
        public static bool ENABLE_SAVE_DEFECT_RAWDATA;
        public static bool ENABLE_SAVE_RAWDATA;
        public static Bitmap MAP_NAVIGATION;


        //public static readonly Dictionary<EnumWaferMissionState, Color> WaferMissionStateColorDic = new Dictionary<EnumWaferMissionState, Color>()
        //{
        //    {EnumWaferMissionState.Unknow,Color.SandyBrown},
        //    {EnumWaferMissionState.NoWafer,Color.LightGray},
        //    {EnumWaferMissionState.ErrorWafer, Color.Red},
        //    {EnumWaferMissionState.Unselect, Color.White},
        //    {EnumWaferMissionState.Selected, Color.LawnGreen},
        //    {EnumWaferMissionState.PreAlign, Color.SkyBlue},
        //    {EnumWaferMissionState.Aligning, Color.Cyan},
        //    {EnumWaferMissionState.Aligned, Color.Cyan},
        //    {EnumWaferMissionState.Measuring, Color.Yellow},
        //    {EnumWaferMissionState.Measured, Color.DarkOrange},
        //    {EnumWaferMissionState.CompletePass, Color.Green},
        //    {EnumWaferMissionState.CompleteNotPass, Color.Brown}
        //};
    }
    



}
