using AlarmManagementClsLib;
using AlarmManagementClsLib.DB;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemGUILib.Alarm
{
    public partial class AlarmHistoryView : DevExpress.XtraEditors.XtraUserControl
    {
        private List<AlarmEntity> _alarmHistoryList = new List<AlarmEntity>();
        public AlarmHistoryView()
        {
            InitializeComponent();
            InitializeAlarmDataToGridView();
            //if (CommandPanelOrientation == E95ControlClsLib.CommandPanelOrientation.Right)
            //{
            //    this.m_commandPanel.Dock = System.Windows.Forms.DockStyle.Right;
            //}
            //else
            //{
            //    this.m_commandPanel.Dock = System.Windows.Forms.DockStyle.Left;
            //}
        }

        private void AlarmHistoryView_Load(object sender, EventArgs e)
        {
            this.gridControl1.Size = new System.Drawing.Size(1850, 600);
        }
        /// <summary>
        /// 绑定活动告警列表到UI
        /// </summary>
        private void InitializeAlarmDataToGridView()
        {
            _alarmHistoryList = AlarmManager.Instance.LoadAlarmsHistoryFromDB();
            this.gridControl1.BeginUpdate();
            this.gridControl1.DataSource = _alarmHistoryList;
            this.gridControl1.EndUpdate();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _alarmHistoryList = AlarmManager.Instance.LoadAlarmsHistoryFromDB();
            this.gridControl1.BeginUpdate();
            this.gridControl1.DataSource = _alarmHistoryList;
            this.gridControl1.EndUpdate();
        }
    }
}
