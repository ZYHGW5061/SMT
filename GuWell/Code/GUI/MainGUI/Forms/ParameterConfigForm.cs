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
using SystemCalibrationClsLib;
using GlobalToolClsLib;
using UserManagerClsLib;
using System.IO;
using WestDragon.Framework.UtilityHelper;

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

        List<ProgramSubstrateSettings> substrateList;
        List<ProgramComponentSettings> componentsList;
        List<ProgramComponentSettings> submonutList;
        List<BondingPositionSettings> bondPosList;
        List<EpoxyApplication> _epoxyApplicationList;
        //List<EutecticParameters> eutecticList;
        ProductStep curStep;
        List<ProductStep> productSteps;
        ProgramSubstrateSettings curStepSubstrate;
        ProgramComponentSettings curStepSubmonut;
        ProgramComponentSettings curStepComp;
        BondingPositionSettings curStepBondingPos;
        EpoxyApplication curStepepoxyApplication;
        //EutecticParameters curStepEutectic;
        private static string SystemDefaultDirectory = SystemConfiguration.Instance.JobConfig.RecipeSavingPath;
        private static string _substrateSavePath = string.Format(@"{0}Recipes\Substrate\", SystemDefaultDirectory);
        private static string _componentsSavePath = string.Format(@"{0}Recipes\Components\", SystemDefaultDirectory);
        private static string _bondPositionSavePath = string.Format(@"{0}Recipes\BondPositions\", SystemDefaultDirectory);
        private static string _epoxyApplicationSavePath = string.Format(@"{0}Recipes\EpoxyApplication\", SystemDefaultDirectory);

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
                LoadbackstageViewTabItem2FromConfig();

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
                _systemConfig.JobConfig.RecognizeSuccessSavingPath = teRecognizeSuccessSavingPath.Text;
                _systemConfig.JobConfig.RecognizeFailSavingPath = teRecognizeFailSavingPath.Text;
                _systemConfig.SaveConfig();
                LogRecorder.RecordUserOperationLog($"保存图片配置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
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
                teSQliteDataSavingPath.Text = _systemConfig.JobConfig.SQliteDataSavingPath;

                ckeConfirmVaccum.Checked = _systemConfig.JobConfig.EnableVaccumConfirm;
                ckeIsSlowSpeedRun.Checked = _systemConfig.JobConfig.IsSlowSpeedRun;

                checkWaferQuickMode.Checked = _systemConfig.JobConfig.WaferQuickMode;
                checkIsTransport.Checked = _systemConfig.JobConfig.IsTransport;
                checkIsLaserScanningHeight.Checked = _systemConfig.JobConfig.IsLaserScanningHeight;
                checkIsComponentCalibrationAfterPP.Checked = _systemConfig.JobConfig.IsComponentCalibrationAfterPP;

                seCurChipNGNumMax.Text = _systemConfig.JobConfig.CurChipNGNumMax.ToString();

                seBMCtimes.Text = _systemConfig.SystemCalibrationConfig.BMCtimes.ToString();
                seBMCdelaytime.Text = _systemConfig.SystemCalibrationConfig.BMCdelaytime.ToString();

                seTurningTimeMS.Text = _systemConfig.TuningTimeMS.ToString();
                seOpenCoolAirDelay.Text = _systemConfig.OpenCoolAirDelayMS.ToString();

                seBreakVaccumDelayMsAfterEutectic.Text = _systemConfig.JobConfig.BreakVaccumDelayMsAfterEutectic.ToString();

                cmbSystemRole.Text = _systemConfig.SystemRole.ToString();

                seAbandonPosX.Text = _systemConfig.PositioningConfig.AbandonMaterialPosition.X.ToString("0.0");
                seAbandonPosY.Text = _systemConfig.PositioningConfig.AbandonMaterialPosition.Y.ToString("0.0");
                seAbandonPosZ.Text = _systemConfig.PositioningConfig.AbandonMaterialPosition.Z.ToString("0.0");

                BondOrigionX.Text = _systemConfig.PositioningConfig.BondOrigion.X.ToString();
                BondOrigionY.Text = _systemConfig.PositioningConfig.BondOrigion.Y.ToString();
                BondOrigionZ.Text = _systemConfig.PositioningConfig.BondOrigion.Z.ToString();

                TrackOrigionX.Text = _systemConfig.PositioningConfig.TrackOrigion.X.ToString();
                TrackOrigionY.Text = _systemConfig.PositioningConfig.TrackOrigion.Y.ToString();
                TrackOrigionZ.Text = _systemConfig.PositioningConfig.TrackOrigion.Z.ToString();

                TrackLaserSensorOrigionX.Text = _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString();
                TrackLaserSensorOrigionY.Text = _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString();
                TrackLaserSensorOrigionZ.Text = _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString();

                TrackLaserSensorZ.Text = _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString();

                WaferCameraOrigionX.Text = _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString();
                WaferCameraOrigionY.Text = _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString();
                WaferCameraOrigionZ.Text = _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString();

                WaferOrigionX.Text = _systemConfig.PositioningConfig.WaferOrigion.X.ToString();
                WaferOrigionY.Text = _systemConfig.PositioningConfig.WaferOrigion.Y.ToString();
                WaferOrigionZ.Text = _systemConfig.PositioningConfig.WaferOrigion.Z.ToString();

                LookupCameraOrigionX.Text = _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString();
                LookupCameraOrigionY.Text = _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString();
                LookupCameraOrigionZ.Text = _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString();

                CalibrationTableOrigionX.Text = _systemConfig.PositioningConfig.CalibrationTableOrigion.X.ToString();
                CalibrationTableOrigionY.Text = _systemConfig.PositioningConfig.CalibrationTableOrigion.Y.ToString();
                CalibrationTableOrigionZ.Text = _systemConfig.PositioningConfig.CalibrationTableOrigion.Z.ToString();

                LookupLaserSensorOrigionX.Text = _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString();
                LookupLaserSensorOrigionY.Text = _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString();
                LookupLaserSensorOrigionZ.Text = _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString();

                BondSafeLocationX.Text = _systemConfig.PositioningConfig.BondSafeLocation.X.ToString();
                BondSafeLocationY.Text = _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString();
                BondSafeLocationZ.Text = _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString();


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

                teRecognizeSuccessSavingPath.Text = _systemConfig.JobConfig.RecognizeSuccessSavingPath;
                teRecognizeFailSavingPath.Text = _systemConfig.JobConfig.RecognizeFailSavingPath;


            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while loading system Configure", ex);
            }
        }
        private void LoadSubstrateList()
        {

            substrateList = new List<ProgramSubstrateSettings>();
            cmbSelRecipe.Items.Clear();
            //cbComponentList.Items.Add(_editRecipe.SubstrateInfos.Name);
            var childs = Directory.GetDirectories(_substrateSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                cmbSelRecipe.Items.Add(childName);
                var xmlFile = $@"{_substrateSavePath}\{childName}\{childName}.xml";
                var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramSubstrateSettings>(xmlFile, Encoding.UTF8);
                substrateList.Add(comp);
            }
            cmbSelRecipe.SelectedIndex = -1;
        }

        private void LoadComponentList()
        {

            componentsList = new List<ProgramComponentSettings>();
            cmbSelRecipe2.Items.Clear();
            //cbComponentList.Items.Add(_editRecipe.SubstrateInfos.Name);
            var childs = Directory.GetDirectories(_componentsSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                cmbSelRecipe2.Items.Add(childName);
                var xmlFile = $@"{_componentsSavePath}\{childName}\{childName}.xml";
                var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramComponentSettings>(xmlFile, Encoding.UTF8);
                componentsList.Add(comp);
            }
            cmbSelRecipe2.SelectedIndex = -1;
        }

        private void LoadbackstageViewTabItem2FromConfig()
        {
            try
            {
                LoadSubstrateList();
                LoadComponentList();

                teTotalBondCounter.Text = _systemConfig.JobConfig.TotalBondCounter.ToString();

                teDispenserCounter.Text = _systemConfig.JobConfig.DispenserCounter.ToString();
                

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
                
    //            _systemConfig.PositioningConfig.BondOrigion.X = double.Parse(BondOrigionX.Text);
    //            _systemConfig.PositioningConfig.BondOrigion.Y = double.Parse(BondOrigionY.Text);
    //            _systemConfig.PositioningConfig.BondOrigion.Z = double.Parse(BondOrigionZ.Text);

    //            _systemConfig.PositioningConfig.TrackOrigion.X = double.Parse(TrackOrigionX.Text);
    //            _systemConfig.PositioningConfig.TrackOrigion.Y = double.Parse(TrackOrigionY.Text);
    //            _systemConfig.PositioningConfig.TrackOrigion.Z = double.Parse(TrackOrigionZ.Text);

    //            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X = double.Parse(TrackLaserSensorOrigionX.Text);
    //            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y = double.Parse(TrackLaserSensorOrigionY.Text);
    //            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z = double.Parse(TrackLaserSensorOrigionZ.Text);

    //            _systemConfig.PositioningConfig.TrackLaserSensorZ = double.Parse(TrackLaserSensorZ.Text);

    //            _systemConfig.PositioningConfig.WaferCameraOrigion.X = double.Parse(WaferCameraOrigionX.Text);
    //            _systemConfig.PositioningConfig.WaferCameraOrigion.Y = double.Parse(WaferCameraOrigionY.Text);
    //            _systemConfig.PositioningConfig.WaferCameraOrigion.Z = double.Parse(WaferCameraOrigionZ.Text);

    //            _systemConfig.PositioningConfig.WaferOrigion.X = double.Parse(WaferOrigionX.Text);
    //            _systemConfig.PositioningConfig.WaferOrigion.Y = double.Parse(WaferOrigionY.Text);
    //            _systemConfig.PositioningConfig.WaferOrigion.Z = double.Parse(WaferOrigionZ.Text);

    //            _systemConfig.PositioningConfig.LookupCameraOrigion.X = double.Parse(LookupCameraOrigionX.Text);
    //            _systemConfig.PositioningConfig.LookupCameraOrigion.Y = double.Parse(LookupCameraOrigionY.Text);
    //            _systemConfig.PositioningConfig.LookupCameraOrigion.Z = double.Parse(LookupCameraOrigionZ.Text);

    //            _systemConfig.PositioningConfig.CalibrationTableOrigion.X = double.Parse(CalibrationTableOrigionX.Text);
    //            _systemConfig.PositioningConfig.CalibrationTableOrigion.Y = double.Parse(CalibrationTableOrigionY.Text);
    //            _systemConfig.PositioningConfig.CalibrationTableOrigion.Z = double.Parse(CalibrationTableOrigionZ.Text);

    //            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X = double.Parse(LookupLaserSensorOrigionX.Text);
    //            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y = double.Parse(LookupLaserSensorOrigionY.Text);
    //            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z = double.Parse(LookupLaserSensorOrigionZ.Text);

    //            _systemConfig.PositioningConfig.BondSafeLocation.X = double.Parse(BondSafeLocationX.Text);
    //            _systemConfig.PositioningConfig.BondSafeLocation.Y = double.Parse(BondSafeLocationY.Text);
    //            _systemConfig.PositioningConfig.BondSafeLocation.Z = double.Parse(BondSafeLocationZ.Text);

    //            _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1
    //                = new PointF(float.Parse(seESZSafeZoneofWaferTablePoint1X.Text), float.Parse(seESZSafeZoneofWaferTablePoint1Y.Text));

    //            _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2
    //                = new PointF(float.Parse(seESZSafeZoneofWaferTablePoint2X.Text), float.Parse(seESZSafeZoneofWaferTablePoint2Y.Text));

    //            _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3
    //                = new PointF(float.Parse(seESZSafeZoneofWaferTablePoint3X.Text), float.Parse(seESZSafeZoneofWaferTablePoint3Y.Text));

    //            _systemConfig.PositioningConfig.BondXYSafeRangeForBondZ
    //= new PointF(float.Parse(seBondXYSafeRangeForBondZWidth.Text), float.Parse(seBondXYSafeRangeForBondZHeight.Text));

    //            _systemConfig.PositioningConfig.ESZWariningPos = float.Parse(seESZWariningPos.Text);
    //            _systemConfig.PositioningConfig.BondZWariningPos = float.Parse(seBondZWariningPos.Text);

                _systemConfig.JobConfig.RawDataSavingPath = teRawDataSavePath.Text.Trim();
                _systemConfig.JobConfig.SQliteDataSavingPath = teSQliteDataSavingPath.Text.Trim();

                _systemConfig.JobConfig.EnableVaccumConfirm = ckeConfirmVaccum.Checked;
                _systemConfig.JobConfig.IsSlowSpeedRun = ckeIsSlowSpeedRun.Checked;
                _systemConfig.JobConfig.WaferQuickMode = checkWaferQuickMode.Checked;
                _systemConfig.JobConfig.IsTransport = checkIsTransport.Checked;
                _systemConfig.JobConfig.IsLaserScanningHeight = checkIsLaserScanningHeight.Checked;
                _systemConfig.JobConfig.IsComponentCalibrationAfterPP = checkIsComponentCalibrationAfterPP.Checked;

                _systemConfig.JobConfig.CurChipNGNumMax = int.Parse(seCurChipNGNumMax.Text);

                _systemConfig.SystemCalibrationConfig.BMCtimes = int.Parse(seBMCtimes.Text);
                _systemConfig.SystemCalibrationConfig.BMCdelaytime = int.Parse(seBMCdelaytime.Text);

                _systemConfig.TuningTimeMS = int.Parse(seTurningTimeMS.Text);
                _systemConfig.OpenCoolAirDelayMS = int.Parse(seOpenCoolAirDelay.Text);
                _systemConfig.JobConfig.BreakVaccumDelayMsAfterEutectic = int.Parse(seBreakVaccumDelayMsAfterEutectic.Text);

                

                _systemConfig.SystemRole = (EnumSystemRole)Enum.Parse(typeof(EnumSystemRole), cmbSystemRole.Text);
                _systemConfig.SaveConfig();
                HardwareConfiguration.Instance.SaveConfig();

                LogRecorder.RecordUserOperationLog($"保存系统配置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

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
                teRawDataSavePath.Text = dialog.SelectedPath + @"\";
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

        private void btnRecognizeSuccessSavingPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择识别成功图像保存地址";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    WarningBox.FormShow("错误", "文件夹路径不能为空", "提示");
                    return;
                }
                teRecognizeSuccessSavingPath.Text = dialog.SelectedPath + @"\";
            }
        }

        private void btnRecognizeFailSavingPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择识别失败图像保存地址";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    WarningBox.FormShow("错误", "文件夹路径不能为空", "提示");
                    return;
                }
                teRecognizeFailSavingPath.Text = dialog.SelectedPath + @"\";
            }
        }

        private void btnAutomatic_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否进行系统自动校准？", "提示") == 1)
            {
                LogRecorder.RecordUserOperationLog($"系统自动校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.AutoRun(0);
                //LoadSystemSettingsFromConfig();
            }
            
        }

        private void btnSemiAutomatic_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否进行系统手动校准？", "提示") == 1)
            {
                LogRecorder.RecordUserOperationLog($"系统手动校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.AutoRun(1);
                //LoadSystemSettingsFromConfig();
            }
            
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否进行系统全手动校准？", "提示") == 1)
            {
                LogRecorder.RecordUserOperationLog($"系统全手动校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.AutoRun(2);
                //LoadSystemSettingsFromConfig();
            }
            
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                _systemConfig.PositioningConfig.BondOrigion.X = double.Parse(BondOrigionX.Text);
                _systemConfig.PositioningConfig.BondOrigion.Y = double.Parse(BondOrigionY.Text);
                _systemConfig.PositioningConfig.BondOrigion.Z = double.Parse(BondOrigionZ.Text);

                _systemConfig.PositioningConfig.TrackOrigion.X = double.Parse(TrackOrigionX.Text);
                _systemConfig.PositioningConfig.TrackOrigion.Y = double.Parse(TrackOrigionY.Text);
                _systemConfig.PositioningConfig.TrackOrigion.Z = double.Parse(TrackOrigionZ.Text);

                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X = double.Parse(TrackLaserSensorOrigionX.Text);
                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y = double.Parse(TrackLaserSensorOrigionY.Text);
                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z = double.Parse(TrackLaserSensorOrigionZ.Text);

                _systemConfig.PositioningConfig.TrackLaserSensorZ = double.Parse(TrackLaserSensorZ.Text);

                _systemConfig.PositioningConfig.WaferCameraOrigion.X = double.Parse(WaferCameraOrigionX.Text);
                _systemConfig.PositioningConfig.WaferCameraOrigion.Y = double.Parse(WaferCameraOrigionY.Text);
                _systemConfig.PositioningConfig.WaferCameraOrigion.Z = double.Parse(WaferCameraOrigionZ.Text);

                _systemConfig.PositioningConfig.WaferOrigion.X = double.Parse(WaferOrigionX.Text);
                _systemConfig.PositioningConfig.WaferOrigion.Y = double.Parse(WaferOrigionY.Text);
                _systemConfig.PositioningConfig.WaferOrigion.Z = double.Parse(WaferOrigionZ.Text);

                _systemConfig.PositioningConfig.LookupCameraOrigion.X = double.Parse(LookupCameraOrigionX.Text);
                _systemConfig.PositioningConfig.LookupCameraOrigion.Y = double.Parse(LookupCameraOrigionY.Text);
                _systemConfig.PositioningConfig.LookupCameraOrigion.Z = double.Parse(LookupCameraOrigionZ.Text);

                _systemConfig.PositioningConfig.CalibrationTableOrigion.X = double.Parse(CalibrationTableOrigionX.Text);
                _systemConfig.PositioningConfig.CalibrationTableOrigion.Y = double.Parse(CalibrationTableOrigionY.Text);
                _systemConfig.PositioningConfig.CalibrationTableOrigion.Z = double.Parse(CalibrationTableOrigionZ.Text);

                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X = double.Parse(LookupLaserSensorOrigionX.Text);
                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y = double.Parse(LookupLaserSensorOrigionY.Text);
                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z = double.Parse(LookupLaserSensorOrigionZ.Text);

                _systemConfig.PositioningConfig.BondSafeLocation.X = double.Parse(BondSafeLocationX.Text);
                _systemConfig.PositioningConfig.BondSafeLocation.Y = double.Parse(BondSafeLocationY.Text);
                _systemConfig.PositioningConfig.BondSafeLocation.Z = double.Parse(BondSafeLocationZ.Text);

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

                //_systemConfig.JobConfig.RawDataSavingPath = teRawDataSavePath.Text.Trim();
                //_systemConfig.JobConfig.SQliteDataSavingPath = teSQliteDataSavingPath.Text.Trim();

                //_systemConfig.JobConfig.EnableVaccumConfirm = ckeConfirmVaccum.Checked;
                //_systemConfig.JobConfig.IsSlowSpeedRun = ckeIsSlowSpeedRun.Checked;
                //_systemConfig.JobConfig.WaferQuickMode = checkWaferQuickMode.Checked;
                //_systemConfig.JobConfig.IsTransport = checkIsTransport.Checked;

                //_systemConfig.JobConfig.CurChipNGNumMax = int.Parse(seCurChipNGNumMax.Text);

                //_systemConfig.SystemCalibrationConfig.BMCtimes = int.Parse(seBMCtimes.Text);
                //_systemConfig.SystemCalibrationConfig.BMCdelaytime = int.Parse(seBMCdelaytime.Text);

                //_systemConfig.TuningTimeMS = int.Parse(seTurningTimeMS.Text);
                //_systemConfig.OpenCoolAirDelayMS = int.Parse(seOpenCoolAirDelay.Text);
                //_systemConfig.JobConfig.BreakVaccumDelayMsAfterEutectic = int.Parse(seBreakVaccumDelayMsAfterEutectic.Text);



                _systemConfig.SystemRole = (EnumSystemRole)Enum.Parse(typeof(EnumSystemRole), cmbSystemRole.Text);
                _systemConfig.SaveConfig();
                HardwareConfiguration.Instance.SaveConfig();

                LogRecorder.RecordUserOperationLog($"保存系统坐标", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                WarningBox.FormShow("成功。", "设置完成!", "提示");
            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
            }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            BMCProcess.Instance.StopBMC();

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否运行BMC？", "提示") == 1)
            {
                BMCProcess.Instance.Run(_systemConfig.SystemCalibrationConfig.BMCtimes, _systemConfig.SystemCalibrationConfig.BMCdelaytime);
            }
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否创建BMC？", "提示") == 1)
            {
                FrmAddESTool frm = new FrmAddESTool();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    BMCProcess.Instance.CreationProcess();

                }
                frm.Dispose();
                
            }
            
        }

        private void btnResetTotalBondCounter_Click(object sender, EventArgs e)
        {
            teTotalBondCounter.Text = "0";
            _systemConfig.JobConfig.TotalBondCounter = 0;
        }

        private void btnResetDispenserCounter_Click(object sender, EventArgs e)
        {
            teDispenserCounter.Text = "0";
            _systemConfig.JobConfig.DispenserCounter = 0;
        }

        private void btnSQliteDataSavingPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择数据库保存地址";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    WarningBox.FormShow("错误", "文件夹路径不能为空", "提示");
                    return;
                }
                teSQliteDataSavingPath.Text = dialog.SelectedPath + @"\";
            }
        }

        private void btnFailedSettingsAppay_Click_1(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"保存流程配置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
        }

        private void cmbSelRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbSelRecipe.Text))
            {
                curStepComp = componentsList.Find(t => t.Name == cmbSelRecipe.Text);
                teProductCounter.Text = curStepComp?.ProduceCount.ToString();
            }
        }

        private void cmbSelRecipe2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbSelRecipe2.Text))
            {
                curStepSubstrate = substrateList.Find(t => t.Name == cmbSelRecipe2.Text);
                teProductCounter2.Text = curStepSubstrate?.ProduceCount.ToString();
            }
        }

        private void btnResetRecipeCounter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbSelRecipe.Text))
            {
                curStepComp = componentsList.Find(t => t.Name == cmbSelRecipe.Text);
                if(curStepComp != null)
                {
                    curStepComp.ProduceCount = 0;
                }
                
                teProductCounter.Text = "0";
            }
        }

        private void btnResetRecipeCounter2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbSelRecipe2.Text))
            {
                curStepSubstrate = substrateList.Find(t => t.Name == cmbSelRecipe2.Text);
                if (curStepSubstrate != null)
                {
                    curStepSubstrate.ProduceCount = 0;
                }

                teProductCounter2.Text = "0";
            }
        }

        private void checkIsLaserScanningHeight_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkIsTransport_CheckedChanged(object sender, EventArgs e)
        {

        }

        
    }
}
