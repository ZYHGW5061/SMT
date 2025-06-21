using WestDragon.Framework.AlarmUserControl;

namespace CommonPanelClsLib
{
    partial class ActiveAlarmsView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;     

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.m_alarmsManagementUserControl = new WestDragon.Framework.AlarmUserControl.AlarmsManagementUserControl();
            this.SuspendLayout();
            // 
            // m_alarmsManagementUserControl
            // 
            this.m_alarmsManagementUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_alarmsManagementUserControl.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.m_alarmsManagementUserControl.Appearance.Options.UseBackColor = true;
            this.m_alarmsManagementUserControl.Location = new System.Drawing.Point(0, 0);
            this.m_alarmsManagementUserControl.Name = "m_alarmsManagementUserControl";
            this.m_alarmsManagementUserControl.Size = new System.Drawing.Size(927, 632);
            this.m_alarmsManagementUserControl.TabIndex = 0;
            // 
            // ActiveAlarmsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.Controls.Add(this.m_alarmsManagementUserControl);
            this.Name = "ActiveAlarmsView";
            this.Size = new System.Drawing.Size(927, 632);
            this.ResumeLayout(false);

        }

        #endregion

        private AlarmsManagementUserControl m_alarmsManagementUserControl;
    }
}
