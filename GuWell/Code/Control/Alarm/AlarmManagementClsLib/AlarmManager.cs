using AlarmManagementClsLib.DB;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestDragon.Framework;
using WestDragon.Framework.AlarmManagement;
using WestDragon.Framework.UtilityHelper;

namespace AlarmManagementClsLib
{
    public class AlarmManager
    {
        #region 单例
        private static AlarmManager _singleton = new AlarmManager();
        private readonly static object _LckObj = new object();
        private AlarmManager()
        {
            //_ioChannelManager.RegisterIOChannelEvt("System.VacuumSourcePressure1", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.VacuumSourcePressure2", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.CompressedAirPressure1", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.CompressedAirPressure2", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.DifferentialPressureSensorSetting1", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.DifferentialPressureSensorSetting2", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.FFUAlarm", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.IonizerAlarm", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.DrivePower", OnIOChanged);
            //_ioChannelManager.RegisterIOChannelEvt("System.AreaSensor", OnIOChanged);
        }
        public static AlarmManager Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_LckObj)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new AlarmManager();
                        }
                    }
                }
                return _singleton;
            }
        }

        #endregion

        /// <summary>
        /// IO通道管理器
        /// </summary>
        //private IOChannelManager _ioChannelManager
        //{
        //    get
        //    {
        //        return IOChannelManager.GetHandler();
        //    }
        //}

        /// <summary>
        /// 存放系统默认路径
        /// </summary>
        public static string SystemDefaultDirectory = GlobalParameterSetting.SYSTEM_DEFAULT_DIR;
        /// <summary>
        /// 缺省报警列表
        /// </summary>
        private Dictionary<int, DefaultAlarmConfig> _defaultAlarmsConfig = new Dictionary<int, DefaultAlarmConfig>();
        public Dictionary<int, DefaultAlarmConfig> DefaultAlarmsConfig { get { return _defaultAlarmsConfig; } }
        /// <summary>
        /// 根据Alarm等级配置的反馈信号列表
        /// </summary>
        private Dictionary<Alarm.EAlarmSeverity, GeneralAlarmConfiguration> _generalAlarmsConfiguration = new Dictionary<Alarm.EAlarmSeverity, GeneralAlarmConfiguration>();
        public Dictionary<Alarm.EAlarmSeverity, GeneralAlarmConfiguration> GeneralAlarmsConfiguration { get { return _generalAlarmsConfiguration; } }
        /// <summary>
        /// 加载根据Alarm等级配置的反馈信号列表
        /// </summary>
        public void LoadGeneralAlarmConfigList()
        {
            _generalAlarmsConfiguration.Clear();
            List<GeneralAlarmConfiguration> list = new List<GeneralAlarmConfiguration>();
            var folderPath = @"Alarms\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fullFilePath = @"Alarms\CommonAlarmConfigration.xml";
            if (File.Exists(fullFilePath))
            {
                list = XmlSerializeHelper.XmlDeserializeFromFile<List<GeneralAlarmConfiguration>>(fullFilePath, Encoding.UTF8);
            }
            list.ForEach(i =>
            {
                _generalAlarmsConfiguration.Add(i.Level, i);
            });
        }
        public List<GeneralAlarmConfiguration> GetGeneralAlarmConfigList()
        {
            List<GeneralAlarmConfiguration> list = new List<GeneralAlarmConfiguration>();
            var folderPath =  @"Alarms\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fullFilePath =  @"Alarms\CommonAlarmConfigration.xml";
            if (File.Exists(fullFilePath))
            {
                list = XmlSerializeHelper.XmlDeserializeFromFile<List<GeneralAlarmConfiguration>>(fullFilePath, Encoding.UTF8);
            }
            return list;
        }
        /// <summary>
        /// 保存根据Alarm等级配置的反馈信号列表
        /// </summary>
        public void SaveGeneralAlarmConfigList(List<GeneralAlarmConfiguration> list)
        {
            if (list != null)
            {
                var fullFilePath = @"Alarms\CommonAlarmConfigration.xml";
                XmlSerializeHelper.XmlSerializeToFile(list, fullFilePath, Encoding.UTF8);
                this.LoadGeneralAlarmConfigList();
            }
        }
        /// <summary>
        /// 加载缺省Alarm列表
        /// </summary>
        public void LoadDefaultAlarmConfigList()
        {
            _defaultAlarmsConfig.Clear();
            List<DefaultAlarmConfig> defaultAlarms = new List<DefaultAlarmConfig>();
            var folderPath = @"Alarms\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fullFilePath =  @"Alarms\DefaultAlarms.xml";
            if (File.Exists(fullFilePath))
            {
                defaultAlarms = XmlSerializeHelper.XmlDeserializeFromFile<List<DefaultAlarmConfig>>(fullFilePath, Encoding.UTF8);
            }
            defaultAlarms.ForEach(i =>
            {
                _defaultAlarmsConfig.Add(i.DefineId, i);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        public List<DefaultAlarmConfig> GetDefaultAlarmConfigList()
        {
            List<DefaultAlarmConfig> preDefinedAlarms = new List<DefaultAlarmConfig>();
            var folderPath =  @"Alarms\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fullFilePath =  @"Alarms\DefaultAlarms.xml";
            if (File.Exists(fullFilePath))
            {
                preDefinedAlarms = XmlSerializeHelper.XmlDeserializeFromFile<List<DefaultAlarmConfig>>(fullFilePath, Encoding.UTF8);
            }

            //List<DefaultAlarmConfig> list = new List<DefaultAlarmConfig>();
            //list.Add(new DefaultAlarmConfig
            //{
            //    DefineId = 10001,
            //    Description = "AAADDDD",
            //    Name = "FourRecoverableAlarm",
            //    Type = AlarmType.Error,
            //    IsAutoRecovery = false,
            //    AutoRecoveryOption = RecoveryOptions.Retry,
            //    DefineRecoveryOptions = new RecoveryOptions[2] { RecoveryOptions.Retry, RecoveryOptions.Abort },
            //    GreenType = SignalTowerType.None,
            //    BlueType = SignalTowerType.None,
            //    YellowType = SignalTowerType.None,
            //    RedType = SignalTowerType.None,
            //    BuzzerType = AlarmManagementClsLib.BuzzerType.Continuous,
            //    BuzzerDuration = 111
            //});
            //var fullFilePath = AlarmManager.SystemDefaultDirectory + @"Alarms\DefaultAlarms.xml";
            //XmlSerializeHelper.XmlSerializeToFile(list, fullFilePath, Encoding.UTF8);

            return preDefinedAlarms;
        }
        /// <summary>
        /// 保存缺省Alarm列表
        /// </summary>
        public void SaveDefaultAlarmConfigList(List<DefaultAlarmConfig> list)
        {
            if (list != null)
            {
                var fullFilePath = @"Alarms\DefaultAlarms.xml";
                XmlSerializeHelper.XmlSerializeToFile(list, fullFilePath, Encoding.UTF8);
                this.LoadDefaultAlarmConfigList();
            }
        }
        /// <summary>
        /// 增加Alarm历史记录
        /// </summary>
        /// <returns></returns>
        public int AddAlarm2DB(AlarmEntity alm)
        {
            var result = -1;
            try
            {
                int newId = new AlarmRepository().InsertAlarm(alm);
                result = newId;
            }
            catch (Exception ex)
            {
                //LogRecorder.RecordLog(EnumLogContentType.Error, "LoadAlarmsViewListFromDBException,Alarms:" + JsonConvert.SerializeObject(alm), ex);
            }
            return result;
        }
        /// <summary>
        /// 从数据库加载Alarm历史记录
        /// </summary>
        /// <returns></returns>
        public List<AlarmEntity> LoadAlarmsHistoryFromDB()
        {
            var result = new List<AlarmEntity>();
            try
            {
                result = new AlarmRepository().GetAlarmsHistoryList();              
            }
            catch (Exception ex)
            {
                //LogRecorder.RecordLog(EnumLogContentType.Error, "LoadAlarmsViewListFromDBException", ex);
            }
            return result;
        }
        public bool UpdateAlarmRecoverStateByInstanceId(string instanceId, int recoveryOption, string recoveryFailText)
        {
            var result = false;
            try
            {
                result = new AlarmRepository().UpdateAlarmRecoverStateByInstanceId(instanceId, recoveryOption,recoveryFailText);
            }
            catch (Exception ex)
            {
                //LogRecorder.RecordLog(EnumLogContentType.Error, "UpdateAlarmRecoverStateByInstanceIdException", ex);
            }
            return result;
        }

        /// <summary>
        /// 转换IO为事件回调
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="previousValue"></param>
        /// <param name="newValue"></param>
        private void OnIOChanged(string channelName, object previousValue, object newValue)
        {
            if (!(bool)previousValue && (bool)newValue)
            {
                var type = channelName.Split('.')[1];
                ProcessSystemIOChange(previousValue, newValue, type);
            }
        }

        /// <summary>
        /// 处理IO变化
        /// </summary>
        /// <param name="previousValue"></param>
        /// <param name="newValue"></param>
        /// <param name="loadport"></param>
        private void ProcessSystemIOChange(object previousValue, object newValue, string iochannel)
        {
            string alarmString = "";

            switch (iochannel)
            {
                case "VacuumSourcePressure1":
                case "VacuumSourcePressure2":
                case "CompressedAirPressure1":
                case "CompressedAirPressure2":
                case "DifferentialPressureSensorSetting1":
                case "DifferentialPressureSensorSetting2":
                case "DrivePower":
                case "DoorStatus":
                    alarmString = string.Format("EFEM:{0} Alarm Warnining!", iochannel);
                    NotificationAlarm notificationAlarm = new NotificationAlarm("", new EventSourceID(EventSourceID.EEventSourceCategory.eMasterGUI), alarmString, "Please check EFEM params.");
                    notificationAlarm.Raise();
                    break;
                case "FFUAlarm":
                case "AreaSensor":
                    alarmString = string.Format("EFEM:{0} Warnining!", iochannel);
                    SimpleAlarm simpleAlarm = new SimpleAlarm("", new EventSourceID(EventSourceID.EEventSourceCategory.eMasterGUI), alarmString, "Please check EFEM params.");
                    simpleAlarm.ReportActivation(alarmString);
                    break;
            }
        }

        public void PostAlarm(string name,string content)
        {
            SimpleAlarm simpleAlarm = new SimpleAlarm(name, new EventSourceID(EventSourceID.EEventSourceCategory.eMasterGUI), content, content);
            simpleAlarm.ReportActivation(content);
        }
    }
}
