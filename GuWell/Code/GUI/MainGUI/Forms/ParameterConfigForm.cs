using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using ConfigurationClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using GlobalDataDefineClsLib;
using CommonPanelClsLib;
using PositioningSystemClsLib;

namespace MainGUI.Forms
{
    public partial class ParameterConfigForm : XtraForm
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        protected SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 硬件配置
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        /// 系统日志
        /// </summary>
        private IBaseLogger _systemLogger
        {
            get { return LoggerManager.GetHandler().GetFileLogger("SystemGlobalLogger");}
        }
        /// <summary>
        /// 功能词典集合
        /// </summary>
        private Dictionary<int, BackstageViewTabItem> _dicFunctionView = new Dictionary<int, BackstageViewTabItem>();


        /// <summary>
        /// 构造函数
        /// </summary>
        public ParameterConfigForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 第一次加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                cmbSystemRole.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(EnumSystemRole)))
                {
                    cmbSystemRole.Items.Add(item);
                }
                LoadSystemSettingsFromConfig();
                LoadImageSavingParamsFromConfig();
                LoadFailedSettingsParamsFromConfig();

            }
            finally
            {
                base.OnLoad(e);
            }

        }

        #region Image Saving Settings
        /// <summary>
        /// 加载图片存储参数
        /// </summary>
        public void LoadImageSavingParamsFromConfig()
        {
            try
            {
                cmbRecogniseResulSaveOption.SelectedIndex= (int)_systemConfig.JobConfig.RecogniseResulSaveOption;
            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while loading Image Saving params from Configure", ex);
            }
        }
        /// <summary>
        /// 图片保存参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplyImageSaving_Click(object sender, EventArgs e)
        {
            try
            {
                _systemConfig.JobConfig.RecogniseResulSaveOption = (EnumRecogniseResulSaveOption)cmbRecogniseResulSaveOption.SelectedIndex;
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功。", "设置完成!", "提示");
            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while saving focus params to Configure", ex);
            }
        }

        #endregion

        #region Process Settings
        private void LoadSystemSettingsFromConfig()
        {
            try
            {
                teRawDataSavePath.Text = _systemConfig.JobConfig.RawDataSavingPath;
                seAbandonPosX.Text = _systemConfig.PositioningConfig.AbandonMaterialPosition.X.ToString("0.0");
                seAbandonPosY.Text = _systemConfig.PositioningConfig.AbandonMaterialPosition.Y.ToString("0.0");
                seAbandonPosZ.Text = _systemConfig.PositioningConfig.AbandonMaterialPosition.Z.ToString("0.0");
                ckeConfirmVaccum.Checked = _systemConfig.JobConfig.EnableVaccumConfirm;
                ckeIsSlowSpeedRun.Checked = _systemConfig.JobConfig.IsSlowSpeedRun;

                seTurningTimeMS.Text = _systemConfig.TuningTimeMS.ToString();
                seOpenCoolAirDelay.Text = _systemConfig.OpenCoolAirDelayMS.ToString();

                seBreakVaccumDelayMsAfterEutectic.Text = _systemConfig.JobConfig.BreakVaccumDelayMsAfterEutectic.ToString();

                cmbSystemRole.Text = _systemConfig.SystemRole.ToString();

                seESZSafeZoneofWaferTablePoint1X.Text = _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1.X.ToString();
                seESZSafeZoneofWaferTablePoint1Y.Text = _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1.Y.ToString();

                seESZSafeZoneofWaferTablePoint2X.Text = _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2.X.ToString();
                seESZSafeZoneofWaferTablePoint2Y.Text = _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2.Y.ToString();

                seESZSafeZoneofWaferTablePoint3X.Text = _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3.X.ToString();
                seESZSafeZoneofWaferTablePoint3Y.Text = _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3.Y.ToString();

                seBondXYSafeRangeForBondZWidth.Text = _systemConfig.PositioningConfig.BondXYSafeRangeForBondZ.X.ToString();
                seBondXYSafeRangeForBondZHeight.Text = _systemConfig.PositioningConfig.BondXYSafeRangeForBondZ.Y.ToString();

                seESZWariningPos.Text = _systemConfig.PositioningConfig.ESZWariningPos.ToString();
                seBondZWariningPos.Text = _systemConfig.PositioningConfig.BondZWariningPos.ToString();
            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while loading system Configure", ex);
            }
        }

        private void btnApplySystemSettings_Click(object sender, EventArgs e)
        {
            try
            {
                _systemConfig.JobConfig.RawDataSavingPath = teRawDataSavePath.Text.Trim();
                _systemConfig.PositioningConfig.AbandonMaterialPosition.X = double.Parse(seAbandonPosX.Text);
                _systemConfig.PositioningConfig.AbandonMaterialPosition.Y = double.Parse(seAbandonPosY.Text);
                _systemConfig.PositioningConfig.AbandonMaterialPosition.Z = double.Parse(seAbandonPosZ.Text);
                _systemConfig.JobConfig.EnableVaccumConfirm = ckeConfirmVaccum.Checked;
                _systemConfig.JobConfig.IsSlowSpeedRun = ckeIsSlowSpeedRun.Checked;

                _systemConfig.TuningTimeMS = int.Parse(seTurningTimeMS.Text);
                _systemConfig.OpenCoolAirDelayMS = int.Parse(seOpenCoolAirDelay.Text);
                _systemConfig.JobConfig.BreakVaccumDelayMsAfterEutectic = int.Parse(seBreakVaccumDelayMsAfterEutectic.Text);

                _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1 
                    = new PointF(float.Parse(seESZSafeZoneofWaferTablePoint1X.Text), float.Parse(seESZSafeZoneofWaferTablePoint1Y.Text));

                _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2
                    = new PointF(float.Parse(seESZSafeZoneofWaferTablePoint2X.Text), float.Parse(seESZSafeZoneofWaferTablePoint2Y.Text));

                _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3
                    = new PointF(float.Parse(seESZSafeZoneofWaferTablePoint3X.Text), float.Parse(seESZSafeZoneofWaferTablePoint3Y.Text));

                _systemConfig.PositioningConfig.BondXYSafeRangeForBondZ
    = new PointF(float.Parse(seBondXYSafeRangeForBondZWidth.Text), float.Parse(seBondXYSafeRangeForBondZHeight.Text));

                _systemConfig.PositioningConfig.ESZWariningPos = float.Parse(seESZWariningPos.Text);
                _systemConfig.PositioningConfig.BondZWariningPos = float.Parse(seBondZWariningPos.Text);

                _systemConfig.SystemRole = (EnumSystemRole)Enum.Parse(typeof(EnumSystemRole), cmbSystemRole.Text);
                _systemConfig.SaveConfig();
                HardwareConfiguration.Instance.SaveConfig();

                WarningBox.FormShow("成功。", "设置完成!", "提示");
            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
            }
        }
        #endregion

        #region Exception Settings

        private void LoadFailedSettingsParamsFromConfig()
        {
            //cbxReadWaferIDFailed.SelectedIndex = (int)_systemConfig.ReadWaferIDFailed;
            //cheReadWaferIDFailed.Checked = _systemConfig.retryReadWaferID;
            //cbxWaferTransmissionFailed.SelectedIndex = (int)_systemConfig.WaferTransmissionFailed;
            //cheWaferTransmissionFailed.Checked = _systemConfig.retryWaferTransmission;
            //cbxAlignmentFailed.SelectedIndex = (int)_systemConfig.AlignmentFailed;
            //cheAlignmentFailed.Checked = _systemConfig.retryAlignment;
            //cbxInspectFailed.SelectedIndex = (int)_systemConfig.InspectFailed;
            //cbxAutoFocusFailed.SelectedIndex = (int)_systemConfig.AutoFocusFailed;
            //cheAutoFocusFailed.Checked = _systemConfig.retryAutoFocus;
        }

        private void btnFailedSettingsAppay_Click(object sender, EventArgs e)
        {
            //_systemConfig.ReadWaferIDFailed = (EnumJobFailedProcessMode)cbxReadWaferIDFailed.SelectedIndex;
            //_systemConfig.WaferTransmissionFailed = (EnumJobFailedProcessMode)cbxWaferTransmissionFailed.SelectedIndex;
            //_systemConfig.AlignmentFailed = (EnumJobFailedProcessMode)cbxAlignmentFailed.SelectedIndex;
            //_systemConfig.InspectFailed = (EnumJobFailedProcessMode)cbxInspectFailed.SelectedIndex;
            //_systemConfig.AutoFocusFailed = (EnumJobFailedProcessMode)cbxAutoFocusFailed.SelectedIndex;
            //_systemConfig.retryReadWaferID = cheReadWaferIDFailed.Checked;
            //_systemConfig.retryWaferTransmission = cheWaferTransmissionFailed.Checked;
            //_systemConfig.retryAlignment = cheAlignmentFailed.Checked;
            //_systemConfig.retryAutoFocus = cheAutoFocusFailed.Checked;
            //_systemConfig.SaveToConfigFile();
            //XtraMessageBox.Show("设置完成!", "Success");

        }

        #endregion

        private void groupControl5_Paint(object sender, PaintEventArgs e)
        {

        }


        private void btnSelRawDataSavePath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择原始数据保存地址";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    WarningBox.FormShow("错误", "文件夹路径不能为空", "提示");
                    return;
                }
                teRawDataSavePath.Text = dialog.SelectedPath + @"\"; ;
            }
        }

        private void btnSaveESZSafeZoneofWaferTablePoint1_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否将晶圆盘当前位置定义为晶圆盘安全区域定义点-1 ？", "提示") == 1)
            {
                seESZSafeZoneofWaferTablePoint1X.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableX).ToString("0.000");
                seESZSafeZoneofWaferTablePoint1Y.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableY).ToString("0.000");
            }
        }

        private void btnSaveESZSafeZoneofWaferTablePoint2_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否将晶圆盘当前位置定义为晶圆盘安全区域定义点-2 ？", "提示") == 1)
            {
                seESZSafeZoneofWaferTablePoint2X.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableX).ToString("0.000");
                seESZSafeZoneofWaferTablePoint2Y.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableY).ToString("0.000");
            }
        }

        private void btnSaveESZSafeZoneofWaferTablePoint3_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否将晶圆盘当前位置定义为晶圆盘安全区域定义点-3 ？", "提示") == 1)
            {
                seESZSafeZoneofWaferTablePoint3X.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableX).ToString("0.000");
                seESZSafeZoneofWaferTablePoint3Y.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableY).ToString("0.000");
            }
        }

        private void btnSaveBondXYSafeRangeForBondZ_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否将晶圆盘当前位置定义为晶圆盘安全区域定义点-3 ？", "提示") == 1)
            {
                seESZSafeZoneofWaferTablePoint3X.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableX).ToString("0.000");
                seESZSafeZoneofWaferTablePoint3Y.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableY).ToString("0.000");
            }
        }

        private void btnSetESZWarningPos_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否将顶针座Z轴当前位置定义为顶针座危险位？", "提示") == 1)
            {
                seESZWariningPos.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ESZ).ToString("0.000");
            }
        }

        private void btnSetBondZWarningPos_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否将榜头Z轴当前位置定义为榜头Z危险位？", "提示") == 1)
            {
                seBondZWariningPos.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString("0.000");
            }
        }
    }
}
