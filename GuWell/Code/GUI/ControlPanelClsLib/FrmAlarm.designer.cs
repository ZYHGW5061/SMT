namespace ControlPanelClsLib
{
    partial class FrmAlarm
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.backstageViewControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewControl();
            this.backstageViewClientControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.activeAlarmsView1 = new CommonPanelClsLib.ActiveAlarmsView();
            this.backstageViewClientControl2 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.alarmHistoryView1 = new CommonPanelClsLib.AlarmHistoryView();
            this.backstageViewTabItem1 = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.backstageViewTabItem2 = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            ((System.ComponentModel.ISupportInitialize)(this.backstageViewControl1)).BeginInit();
            this.backstageViewControl1.SuspendLayout();
            this.backstageViewClientControl1.SuspendLayout();
            this.backstageViewClientControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // backstageViewControl1
            // 
            this.backstageViewControl1.Controls.Add(this.backstageViewClientControl1);
            this.backstageViewControl1.Controls.Add(this.backstageViewClientControl2);
            this.backstageViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backstageViewControl1.Items.Add(this.backstageViewTabItem1);
            this.backstageViewControl1.Items.Add(this.backstageViewTabItem2);
            this.backstageViewControl1.Location = new System.Drawing.Point(0, 0);
            this.backstageViewControl1.Name = "backstageViewControl1";
            this.backstageViewControl1.SelectedTab = this.backstageViewTabItem1;
            this.backstageViewControl1.SelectedTabIndex = 0;
            this.backstageViewControl1.Size = new System.Drawing.Size(1091, 684);
            this.backstageViewControl1.TabIndex = 1;
            this.backstageViewControl1.Text = "backstageViewControl1";
            this.backstageViewControl1.VisibleInDesignTime = true;
            // 
            // backstageViewClientControl1
            // 
            this.backstageViewClientControl1.Controls.Add(this.activeAlarmsView1);
            this.backstageViewClientControl1.Location = new System.Drawing.Point(139, 0);
            this.backstageViewClientControl1.Name = "backstageViewClientControl1";
            this.backstageViewClientControl1.Size = new System.Drawing.Size(952, 684);
            this.backstageViewClientControl1.TabIndex = 1;
            // 
            // activeAlarmsView1
            // 
            this.activeAlarmsView1.Caption = null;
            this.activeAlarmsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeAlarmsView1.Location = new System.Drawing.Point(0, 0);
            this.activeAlarmsView1.Name = "activeAlarmsView1";
            this.activeAlarmsView1.OwnerTabPage = null;
            this.activeAlarmsView1.OwnerTabPageV2 = null;
            this.activeAlarmsView1.Size = new System.Drawing.Size(952, 684);
            this.activeAlarmsView1.TabIndex = 1;
            // 
            // backstageViewClientControl2
            // 
            this.backstageViewClientControl2.Controls.Add(this.alarmHistoryView1);
            this.backstageViewClientControl2.Location = new System.Drawing.Point(139, 0);
            this.backstageViewClientControl2.Name = "backstageViewClientControl2";
            this.backstageViewClientControl2.Size = new System.Drawing.Size(952, 684);
            this.backstageViewClientControl2.TabIndex = 2;
            // 
            // alarmHistoryView1
            // 
            this.alarmHistoryView1.Caption = null;
            this.alarmHistoryView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmHistoryView1.Location = new System.Drawing.Point(0, 0);
            this.alarmHistoryView1.Name = "alarmHistoryView1";
            this.alarmHistoryView1.OwnerTabPage = null;
            this.alarmHistoryView1.OwnerTabPageV2 = null;
            this.alarmHistoryView1.Size = new System.Drawing.Size(952, 684);
            this.alarmHistoryView1.TabIndex = 0;
            // 
            // backstageViewTabItem1
            // 
            this.backstageViewTabItem1.Caption = "当前报警";
            this.backstageViewTabItem1.ContentControl = this.backstageViewClientControl1;
            this.backstageViewTabItem1.Name = "backstageViewTabItem1";
            this.backstageViewTabItem1.Selected = true;
            // 
            // backstageViewTabItem2
            // 
            this.backstageViewTabItem2.Caption = "报警历史";
            this.backstageViewTabItem2.ContentControl = this.backstageViewClientControl2;
            this.backstageViewTabItem2.Name = "backstageViewTabItem2";
            // 
            // FrmAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 684);
            this.Controls.Add(this.backstageViewControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "FrmAlarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报警";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAlarm_FormClosing);
            this.Load += new System.EventHandler(this.IOCtrlPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.backstageViewControl1)).EndInit();
            this.backstageViewControl1.ResumeLayout(false);
            this.backstageViewClientControl1.ResumeLayout(false);
            this.backstageViewClientControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.BackstageViewControl backstageViewControl1;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl1;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl2;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem1;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem2;
        private CommonPanelClsLib.ActiveAlarmsView activeAlarmsView1;
        private CommonPanelClsLib.AlarmHistoryView alarmHistoryView1;
    }
}
