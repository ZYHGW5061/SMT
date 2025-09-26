using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainGUI.Forms
{
    public partial class FrmAddESTool : BaseForm
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        /// <summary>
        /// 页面构造函数，新建Recipe
        /// </summary>
        public FrmAddESTool()
        {
            InitializeComponent();
            LoadExistESTool();
        }
        private void LoadExistESTool()
        {
            cbPPName.Items.Clear();

            foreach (var PPTool in _systemConfig.PPToolSettings)
            {
                cbPPName.Items.Add(PPTool.Name);
            }

            cbPPName.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PPtoolName;
            teChipPPPress.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PickupStress.ToString();
            teChipPPPickPos.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.WorkHeight.ToString();
            teChipPPPlacePos.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.WorkHeight.ToString();
            teVaccumDelayMS.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.DelayMSForVaccum.ToString();
            sePlaceDelayMs.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.DelayMSForPlace.ToString();
            sePlaceStress.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PlaceStress.ToString();
            seBreakVaccumTimespanMs.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.BreakVaccumTimespanMS.ToString();
            teSlowTravelBeforePickupMM.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowTravelBeforePickupMM.ToString();
            teSlowSpeedBeforePickup.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowSpeedBeforePickup.ToString();
            teSlowTravelAfterPickupMM.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowTravelAfterPickupMM.ToString();
            teSlowSpeedAfterPickup.Text = SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowSpeedAfterPickup.ToString();


        }
        /// <summary>
        /// 输入完毕，点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbPPName.Text.Trim()))
            {
                WarningBox.FormShow("错误","请选择吸嘴工具。","提示");
                return;
            }

            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PPtoolName = cbPPName.Text;
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PickupStress = float.Parse(teChipPPPress.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.WorkHeight = float.Parse(teChipPPPickPos.Text);
            //SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.WorkHeight = float.Parse(teChipPPPlacePos.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.DelayMSForVaccum = float.Parse(teVaccumDelayMS.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.DelayMSForPlace = float.Parse(sePlaceDelayMs.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PlaceStress = float.Parse(sePlaceStress.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.BreakVaccumTimespanMS = float.Parse(seBreakVaccumTimespanMs.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowTravelBeforePickupMM = float.Parse(teSlowTravelBeforePickupMM.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowSpeedBeforePickup = float.Parse(teSlowSpeedBeforePickup.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowTravelAfterPickupMM = float.Parse(teSlowTravelAfterPickupMM.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowSpeedAfterPickup = float.Parse(teSlowSpeedAfterPickup.Text);

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// 自动过滤字符串中非法的字符
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetValidFileName(string fileName)
        {
            StringBuilder fileNameBuilder = new StringBuilder(fileName);
            foreach (char inValidChar in Path.GetInvalidFileNameChars ())
            {
                fileNameBuilder.Replace(inValidChar.ToString (), string.Empty);
            }
            return fileNameBuilder.ToString ();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbPPName.Text.Trim()))
            {
                WarningBox.FormShow("错误", "请选择吸嘴工具。", "提示");
                return;
            }

            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PPtoolName = cbPPName.Text;
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PickupStress = float.Parse(teChipPPPress.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.WorkHeight = float.Parse(teChipPPPickPos.Text);
            //SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.WorkHeight = float.Parse(teChipPPPlacePos.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.DelayMSForVaccum = float.Parse(teVaccumDelayMS.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.DelayMSForPlace = float.Parse(sePlaceDelayMs.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.PlaceStress = float.Parse(sePlaceStress.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.BreakVaccumTimespanMS = float.Parse(seBreakVaccumTimespanMs.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowTravelBeforePickupMM = float.Parse(teSlowTravelBeforePickupMM.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowSpeedBeforePickup = float.Parse(teSlowSpeedBeforePickup.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowTravelAfterPickupMM = float.Parse(teSlowTravelAfterPickupMM.Text);
            SystemConfiguration.Instance.SystemCalibrationConfig.BMCPPtoolParam.SlowSpeedAfterPickup = float.Parse(teSlowSpeedAfterPickup.Text);

            this.DialogResult = DialogResult.Cancel;
        }
    }
}
