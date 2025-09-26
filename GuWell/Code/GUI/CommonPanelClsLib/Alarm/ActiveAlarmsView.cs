using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlarmManagementClsLib.DB;
using AlarmManagementClsLib;
using WestDragon.Framework.AlarmManagement;
using WestDragon.Framework.UserControls;
using WestDragon.Framework;
using WestDragon.Framework.AlarmUserControl;
using WestDragon.Framework.UtilityHelper;

namespace CommonPanelClsLib
{
    public partial class ActiveAlarmsView : ViewBase
    {
        private int m_nPreviousAlarmCount;

        private AlarmSubscriber m_alarmSubscriber;

        private ATBalloonTip m_alarmTip = new ATBalloonTip();

        private object m_alarmTipLock = new object();

        private int m_nPreviousAlarmNotificationCount;

        public ActiveAlarmsView()
        {
            InitializeComponent();

            Initialize();
            this.CreateControl(); //创建句柄
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
                if (this.m_alarmSubscriber != null)
                {
                    this.m_alarmSubscriber.Dispose();
                }
                if (this.m_alarmTip != null)
                {
                    this.m_alarmTip.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public void Initialize()
        {
            Task.Factory.StartNew(new Action(() =>
            {
                List<EventSourceID> eventSourceIDList = new List<EventSourceID>();
                EventSourceID eventSourceID = new EventSourceID(EventSourceID.EEventSourceCategory.eMasterGUI);
                eventSourceIDList.Add(eventSourceID);
                this.m_alarmsManagementUserControl.Initialize(eventSourceIDList.ToArray());
                this.m_alarmsManagementUserControl.ActiveAlarmCountChangedEvent += new AlarmsListUserControl.ActiveAlarmCountChangedDelegate(this.m_alarmsManagementUserControl_ActiveAlarmCountChangedEvent);
                if (this.m_alarmSubscriber != null)
                {
                    this.m_alarmSubscriber.Dispose();
                    this.m_alarmSubscriber = null;
                }
                this.m_alarmSubscriber = new AlarmSubscriber(eventSourceIDList.ToArray());
                this.m_alarmSubscriber.ActiveAlarmListChanged += new AlarmSubscriber.ActiveAlarmListChangedDelegate(this.m_alarmSubscriber_ActiveAlarmListChanged);
                this.m_alarmSubscriber.AlarmAddedEvent += new AlarmSubscriber.AlarmEventDelegate(this.m_alarmSubscriber_AlarmAdd);
                this.m_alarmSubscriber.AlarmRecoveryEvent += new AlarmSubscriber.AlarmEventDelegate(this.m_alarmSubscriber_AlarmRecovery);
            }));
        }

        public bool IsAtLeastOneAlarmRecoverable()
        {
            return this.m_alarmSubscriber != null && this.m_alarmSubscriber.IsAtLeastOneAlarmRecoverable();
        }

        private void m_alarmSubscriber_ActiveAlarmListChanged(System.Collections.Generic.List<Alarm> activeAlarmList)
        {
            this.OnUIThread(new Action(() => {
                //if (base.ParentFunctionalArea != base.ParentFunctionalArea.ParentE95Main.CurrentFunctionalArea)
                //{
                    bool bDisplayAlarmNotification = false;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (activeAlarmList.Count > 0 && activeAlarmList.Count > this.m_nPreviousAlarmNotificationCount)
                    {
                        int nVisibleAlarmCount = 0;
                        foreach (Alarm activeAlarm in activeAlarmList)
                        {
                            sb.AppendFormat("{0}, {1}, {2}\n", activeAlarm.LastPostedTime.ToShortTimeString(), activeAlarm.sDisplayName, activeAlarm.SummaryInfo());
                            if (++nVisibleAlarmCount >= 1)
                            {
                                break;
                            }
                        }
                        int nHiddenAlarmCount = activeAlarmList.Count - nVisibleAlarmCount;
                        if (nHiddenAlarmCount > 0)
                        {
                            sb.AppendFormat("And {0} more alarms ...", nHiddenAlarmCount);
                        }
                        bDisplayAlarmNotification = true;
                    }
                    this.m_nPreviousAlarmNotificationCount = activeAlarmList.Count;
                    lock (this.m_alarmTipLock)
                    {
                        //if (bDisplayAlarmNotification)
                        //{
                        //    this.m_alarmTip.Show("Recent alarms", sb.ToString(), this.ParentFunctionalArea.NavigationButton, ATBalloonTip.EIcon.Error, 1500);
                        //}
                        //else
                        //{
                        //    this.m_alarmTip.CloseWindow();
                        //}
                    }
                //}
            }), true);
           
        }

        private void m_alarmsManagementUserControl_ActiveAlarmCountChangedEvent(int nCount)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)delegate { m_alarmsManagementUserControl_ActiveAlarmCountChangedEvent(nCount); });
                return;
            }
            //根据ActiveAlarm的数量改变导航栏
            //if (nCount > 0)
            //{
            //    if (base.Visible || nCount <= this.m_nPreviousAlarmCount)
            //    {
            //        this.ParentFunctionalArea.NavigationButton.AlarmState = ButtonAlarmState.Alarm;
            //        this.ParentFunctionalArea.NavigationButton.ForeColor = Color.Red;
            //    }
            //    else if(this.IsHandleCreated)
            //    {
            //        if (this.ParentFunctionalArea.NavigationButton.InvokeRequired)
            //        {
            //            this.Invoke(new Action(() =>
            //            {
            //                this.ParentFunctionalArea.NavigationButton.AlarmCount = nCount;
            //                this.ParentFunctionalArea.NavigationButton.AlarmState = ButtonAlarmState.FlashAlarm;
            //                this.ParentFunctionalArea.NavigationButton.ForeColor = Color.Red;
            //            }));
            //        }
            //        else
            //        {
            //            this.ParentFunctionalArea.NavigationButton.AlarmCount = nCount;
            //            this.ParentFunctionalArea.NavigationButton.AlarmState = ButtonAlarmState.FlashAlarm;
            //            this.ParentFunctionalArea.NavigationButton.ForeColor = Color.Red;
            //        }
            //    }
            //}
            //else
            //{
            //    this.ParentFunctionalArea.NavigationButton.AlarmState = ButtonAlarmState.Normal;
            //    this.ParentFunctionalArea.NavigationButton.Text = "告警";
            //    this.ParentFunctionalArea.NavigationButton.ForeColor = Color.Black;
            //}
            this.m_nPreviousAlarmCount = nCount;
        }
        /// <summary>
        /// 告警发生时计入数据库
        /// </summary>
        /// <param name="alm"></param>
        private void m_alarmSubscriber_AlarmAdd(Alarm alm)
        {
            var entity = new AlarmEntity();
            //entity.DefineId = newAlarm.DefineId;
            entity.Name = alm.sDisplayName;
            //entity.Description = newAlarm.Description;
            entity.Type = (int)alm.AlarmSeverityLevel;
            entity.Source = (int)alm.SourceID.eCategory;
            entity.RecoveryTime = new DateTime(1753, 1, 1);
            entity.OccurrenceTime = DateTime.Now;
            entity.AcknowledgeTime = new DateTime(1753, 1, 1);
            entity.ClearTime = new DateTime(1753, 1, 1);
            entity.ResetTime = new DateTime(1753, 1, 1);
            //区分是不是自动恢复型的报警
            //entity.State = (int)newAlarm.State;
            //AlarmManager.GetHandler().AddAlarm2DB(entity);
            //this.RefreshTowerLightState(alm);
        }
        #region 塔灯状态
        //private void RefreshTowerLightState(Alarm alm)
        //{
        //    Task.Run(() =>
        //    {
        //        if (alm.DefineId != -1)
        //        {
        //            if (AlarmManager.GetHandler().DefaultAlarmsConfig.ContainsKey(alm.DefineId))
        //            {
        //                var tempDefaultAlarm = AlarmManager.GetHandler().DefaultAlarmsConfig[alm.DefineId];
        //                EnumSignalTowerStates redType;
        //                EnumSignalTowerStates yellowType;
        //                EnumSignalTowerStates blueType;
        //                EnumSignalTowerStates greenType;
        //                EnumSignalTowerType buzzerType = EnumSignalTowerType.Buzzer1;
        //                if (Enum.TryParse<EnumSignalTowerStates>(tempDefaultAlarm.RedType.ToString(), out redType))
        //                {
        //                    if (Enum.TryParse<EnumSignalTowerStates>(tempDefaultAlarm.YellowType.ToString(), out yellowType))
        //                    {
        //                        if (Enum.TryParse<EnumSignalTowerStates>(tempDefaultAlarm.BlueType.ToString(), out blueType))
        //                        {
        //                            if (Enum.TryParse<EnumSignalTowerStates>(tempDefaultAlarm.GreenType.ToString(), out greenType))
        //                            {
        //                                switch (tempDefaultAlarm.BuzzerType)
        //                                {
        //                                    case BuzzerType.Continuous:
        //                                        buzzerType = EnumSignalTowerType.Buzzer1;
        //                                        break;
        //                                    case BuzzerType.Disontinuous:
        //                                        buzzerType = EnumSignalTowerType.Buzzer2;
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Red, redType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Yellow, yellowType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Blue, blueType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Green, greenType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(buzzerType, EnumSignalTowerStates.On);
        //                                System.Threading.Thread.Sleep(tempDefaultAlarm.BuzzerDuration);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Red, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Yellow, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Blue, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Green, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(buzzerType, EnumSignalTowerStates.Off);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (AlarmManager.GetHandler().GeneralAlarmsConfiguration.ContainsKey(alm.AlarmSeverityLevel))
        //                {
        //                    var config = AlarmManager.GetHandler().GeneralAlarmsConfiguration[alm.AlarmSeverityLevel];
        //                    EnumSignalTowerStates redType;
        //                    EnumSignalTowerStates yellowType;
        //                    EnumSignalTowerStates blueType;
        //                    EnumSignalTowerStates greenType;
        //                    EnumSignalTowerType buzzerType = EnumSignalTowerType.Buzzer1;
        //                    if (Enum.TryParse<EnumSignalTowerStates>(config.RedType.ToString(), out redType))
        //                    {
        //                        if (Enum.TryParse<EnumSignalTowerStates>(config.YellowType.ToString(), out yellowType))
        //                        {
        //                            if (Enum.TryParse<EnumSignalTowerStates>(config.BlueType.ToString(), out blueType))
        //                            {
        //                                if (Enum.TryParse<EnumSignalTowerStates>(config.GreenType.ToString(), out greenType))
        //                                {
        //                                    switch (config.BuzzerType)
        //                                    {
        //                                        case BuzzerType.Continuous:
        //                                            buzzerType = EnumSignalTowerType.Buzzer1;
        //                                            break;
        //                                        case BuzzerType.Disontinuous:
        //                                            buzzerType = EnumSignalTowerType.Buzzer2;
        //                                            break;
        //                                        default:
        //                                            break;
        //                                    }
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Red, redType);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Yellow, yellowType);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Blue, blueType);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Green, greenType);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(buzzerType, EnumSignalTowerStates.On);
        //                                    System.Threading.Thread.Sleep(config.BuzzerDuration);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Red, EnumSignalTowerStates.Off);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Yellow, EnumSignalTowerStates.Off);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Blue, EnumSignalTowerStates.Off);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Green, EnumSignalTowerStates.Off);
        //                                    SignalTowerManager.GetHandler().SetSignalTower(buzzerType, EnumSignalTowerStates.Off);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (AlarmManager.GetHandler().GeneralAlarmsConfiguration.ContainsKey(alm.AlarmSeverityLevel))
        //            {
        //                var config = AlarmManager.GetHandler().GeneralAlarmsConfiguration[alm.AlarmSeverityLevel];
        //                EnumSignalTowerStates redType;
        //                EnumSignalTowerStates yellowType;
        //                EnumSignalTowerStates blueType;
        //                EnumSignalTowerStates greenType;
        //                EnumSignalTowerType buzzerType = EnumSignalTowerType.Buzzer1;
        //                if (Enum.TryParse<EnumSignalTowerStates>(config.RedType.ToString(), out redType))
        //                {
        //                    if (Enum.TryParse<EnumSignalTowerStates>(config.YellowType.ToString(), out yellowType))
        //                    {
        //                        if (Enum.TryParse<EnumSignalTowerStates>(config.BlueType.ToString(), out blueType))
        //                        {
        //                            if (Enum.TryParse<EnumSignalTowerStates>(config.GreenType.ToString(), out greenType))
        //                            {
        //                                switch (config.BuzzerType)
        //                                {
        //                                    case BuzzerType.Continuous:
        //                                        buzzerType = EnumSignalTowerType.Buzzer1;
        //                                        break;
        //                                    case BuzzerType.Disontinuous:
        //                                        buzzerType = EnumSignalTowerType.Buzzer2;
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Red, redType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Yellow, yellowType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Blue, blueType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Green, greenType);
        //                                SignalTowerManager.GetHandler().SetSignalTower(buzzerType, EnumSignalTowerStates.On);
        //                                System.Threading.Thread.Sleep(config.BuzzerDuration);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Red, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Yellow, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Blue, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(EnumSignalTowerType.Green, EnumSignalTowerStates.Off);
        //                                SignalTowerManager.GetHandler().SetSignalTower(buzzerType, EnumSignalTowerStates.Off);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    });
        //}
        #endregion
        private void m_alarmSubscriber_AlarmRecovery(Alarm alm)
        {
            //更新数据库中告警的状态

        }
    }
}
