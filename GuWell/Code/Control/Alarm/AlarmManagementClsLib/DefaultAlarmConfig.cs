using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlarmManagementClsLib
{
    public class DefaultAlarmConfig
    {
        public DefaultAlarmConfig()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public int DefineId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AlarmType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否自动恢复
        /// </summary>
        public bool IsAutoRecovery { get; set; }
        /// <summary>
        /// 自动恢复选项，随配置
        /// </summary>
        public RecoveryOptions AutoRecoveryOption { get; set; }
        /// <summary>
        /// 恢复选项列表
        /// </summary>
        [XmlIgnore]
        protected RecoveryOptions[] m_defineRecoveryOptions;
        /// <summary>
        /// 获取或设置恢复选项列表
        /// </summary>
        public RecoveryOptions[] DefineRecoveryOptions
        {
            get
            {
                return this.m_defineRecoveryOptions;
            }
            set
            {
                this.m_defineRecoveryOptions = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public SignalTowerType GreenType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SignalTowerType BlueType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SignalTowerType YellowType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SignalTowerType RedType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BuzzerType BuzzerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BuzzerDuration { get; set; }

    }
    /// <summary>
    /// 告警等级
    /// </summary>
    public enum AlarmType
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "Alarm")]
        Alarm = 1,
        [XmlEnum(Name = "Error")]
        Error = 2,
        [XmlEnum(Name = "Warnning")]
        Warnning = 3,
        [XmlEnum(Name = "Notification")]
        Notification = 4
    }
    /// <summary>
    /// 告警恢复方式
    /// </summary>
    public enum RecoveryOptions
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "Retry")]
        Retry = 1,
        [XmlEnum(Name = "Abort")]
        Abort = 2,
        [XmlEnum(Name = "Ignore")]
        Ignore = 3,
        [XmlEnum(Name = "Initial")]
        Initial = 4,
    }
    /// <summary>
    /// 
    /// </summary>
    public enum AlarmSource
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "Master")]
        Master = 1,
        [XmlEnum(Name = "BackSide")]
        BackSide = 2,
        [XmlEnum(Name = "FrontSide")]
        FrontSide = 3,
        [XmlEnum(Name = "EdgeSide")]
        EdgeSide = 4,
    }
    public enum AlarmState
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "Active")]
        Active = 1,
        [XmlEnum(Name = "Acknowledged")]
        Acknowledged = 2,
        [XmlEnum(Name = "Recovered")]
        Recovered = 3,
        [XmlEnum(Name = "Cleared")]
        Cleared = 4,
        [XmlEnum(Name = "Reset")]
        Reset = 5
    }
}
