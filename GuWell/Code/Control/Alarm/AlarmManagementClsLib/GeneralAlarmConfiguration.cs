using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WestDragon.Framework.AlarmManagement;

namespace AlarmManagementClsLib
{
    public class GeneralAlarmConfiguration
    {
        public Alarm.EAlarmSeverity Level { get; set; }
        public SignalTowerType GreenType { get; set; }
        public SignalTowerType BlueType { get; set; }
        public SignalTowerType YellowType { get; set; }
        public SignalTowerType RedType { get; set; }
        public BuzzerType BuzzerType { get; set; }
        public int BuzzerDuration { get; set; }
        //public string StrGreenType
        //{
        //    get
        //    {
        //        return
        //        GreenType == SignalTowerType.None ? "-" : GreenType.ToString();
        //    }
        //    set
        //    {
        //        GreenType = value == "-" ? SignalTowerType.None : (SignalTowerType)Enum.Parse(typeof(SignalTowerType), value);
        //    }
        //}
        //public string StrBlueType
        //{
        //    get
        //    {
        //        return
        //        BlueType == SignalTowerType.None ? "-" : BlueType.ToString();
        //    }
        //    set
        //    {
        //        BlueType = value == "-" ? SignalTowerType.None : (SignalTowerType)Enum.Parse(typeof(SignalTowerType), value);
        //    }
        //}
        //public string StrYellowType
        //{
        //    get
        //    {
        //        return
        //        YellowType == SignalTowerType.None ? "-" : YellowType.ToString();
        //    }
        //    set
        //    {
        //        YellowType = value == "-" ? SignalTowerType.None : (SignalTowerType)Enum.Parse(typeof(SignalTowerType), value);
        //    }
        //}
        //public string StrRedType
        //{
        //    get
        //    {
        //        return
        //        RedType == SignalTowerType.None ? "-" : RedType.ToString();
        //    }
        //    set
        //    {
        //        RedType = value == "-" ? SignalTowerType.None : (SignalTowerType)Enum.Parse(typeof(SignalTowerType), value);
        //    }
        //}
        //public string StrBuzzerType
        //{
        //    get
        //    {
        //        return
        //        BuzzerType == BuzzerType.None ? "-" : BuzzerType.ToString();
        //    }
        //    set
        //    {
        //        BuzzerType = value == "-" ? BuzzerType.None : (BuzzerType)Enum.Parse(typeof(BuzzerType), value);
        //    }
        //}
    }

    public enum SignalTowerType
    {
        [XmlEnum(Name = "None")]
        None,
        [XmlEnum(Name = "On")]
        On,
        [XmlEnum(Name = "Blink")]
        Blink,
        [XmlEnum(Name = "Off")]
        Off
    }
    public enum BuzzerType
    {
        [XmlEnum(Name = "None")]
        None,
        [XmlEnum(Name = "Disontinuous")]
        Disontinuous,
        [XmlEnum(Name = "Continuous")]
        Continuous
    }
}
