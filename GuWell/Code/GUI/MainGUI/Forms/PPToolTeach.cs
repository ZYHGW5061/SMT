using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using PositioningSystemClsLib;
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
using SystemCalibrationClsLib;
using UserManagerClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace MainGUI.Forms
{
    public partial class PPToolTeach : DevExpress.XtraEditors.XtraForm
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
            get { return LoggerManager.GetHandler().GetFileLogger("SystemGlobalLogger"); }
        }

        private PPToolSettings curPPtool;

        public PPToolTeach()
        {
            InitializeComponent();

            cmbSelRecipe.Items.Clear();

            foreach (var pptool in _systemConfig.PPToolSettings)
            {
                cmbSelRecipe.Items.Add(pptool.Name);
            }

            //LoadSystemSettingsFromConfig();
        }


        private void LoadSystemSettingsFromConfig()
        {
            try
            {
                string PPtoolname = cmbSelRecipe.Text;

                curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                ChipPPPosBracketX.Text = curPPtool.ChipPPPosBracket.X.ToString();
                ChipPPPosBracketY.Text = curPPtool.ChipPPPosBracket.Y.ToString();
                ChipPPPosBracketZ.Text = curPPtool.ChipPPPosBracket.Z.ToString();
                ChipPPPosBracketT.Text = curPPtool.ChipPPPosBracket.Theta.ToString();

                LookuptoPPOrigionX.Text = curPPtool.LookuptoPPOrigion.X.ToString();
                LookuptoPPOrigionY.Text = curPPtool.LookuptoPPOrigion.Y.ToString();
                LookuptoPPOrigionZ.Text = curPPtool.LookuptoPPOrigion.Z.ToString();

                PP1AndBondCameraOffsetX.Text = curPPtool.PP1AndBondCameraOffset.X.ToString();
                PP1AndBondCameraOffsetY.Text = curPPtool.PP1AndBondCameraOffset.Y.ToString();
                PP1AndBondCameraOffsetZ.Text = curPPtool.PP1AndBondCameraOffset.Z.ToString();

                PPosCompensateCoordinate1X.Text = curPPtool.ChipPPPosCompensateCoordinate1.X.ToString();
                PPosCompensateCoordinate1Y.Text = curPPtool.ChipPPPosCompensateCoordinate1.Y.ToString();
                PPosCompensateCoordinate1Z.Text = curPPtool.ChipPPPosCompensateCoordinate1.Z.ToString();

                PPosCompensateCoordinate2X.Text = curPPtool.ChipPPPosCompensateCoordinate2.X.ToString();
                PPosCompensateCoordinate2Y.Text = curPPtool.ChipPPPosCompensateCoordinate2.Y.ToString();
                PPosCompensateCoordinate2Z.Text = curPPtool.ChipPPPosCompensateCoordinate2.Z.ToString();

                AltimetryOnMark.Text = curPPtool.AltimetryOnMark.ToString();


            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while loading system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }

        private void btnAutomatic_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否自动校准吸嘴？", "提示") == 1)
            {
                string PPtoolname = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}自动校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.ChipRun(PPtoolname, 0);
                //LoadSystemSettingsFromConfig();
            }
            
        }

        private void btnSemiAutomatic_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否手动校准吸嘴？", "提示") == 1)
            {
                string PPtoolname = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}手动校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.ChipRun(PPtoolname, 1);
                //LoadSystemSettingsFromConfig();
            }
            
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否全手动校准吸嘴？", "提示") == 1)
            {
                string PPtoolname = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}全手动校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.ChipRun(PPtoolname, 2);
                //LoadSystemSettingsFromConfig();
            }
            
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string PPtoolname = cmbSelRecipe.Text;

                curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                curPPtool.ChipPPPosBracket.X = double.Parse(ChipPPPosBracketX.Text);
                curPPtool.ChipPPPosBracket.Y = double.Parse(ChipPPPosBracketY.Text);
                curPPtool.ChipPPPosBracket.Z = double.Parse(ChipPPPosBracketZ.Text);
                curPPtool.ChipPPPosBracket.Theta = double.Parse(ChipPPPosBracketT.Text);

                curPPtool.LookuptoPPOrigion.X = double.Parse(LookuptoPPOrigionX.Text);
                curPPtool.LookuptoPPOrigion.Y = double.Parse(LookuptoPPOrigionY.Text);
                curPPtool.LookuptoPPOrigion.Z = double.Parse(LookuptoPPOrigionZ.Text);

                curPPtool.PP1AndBondCameraOffset.X = double.Parse(PP1AndBondCameraOffsetX.Text);
                curPPtool.PP1AndBondCameraOffset.Y = double.Parse(PP1AndBondCameraOffsetY.Text);
                curPPtool.PP1AndBondCameraOffset.Z = double.Parse(PP1AndBondCameraOffsetZ.Text);

                curPPtool.ChipPPPosCompensateCoordinate1.X = double.Parse(PPosCompensateCoordinate1X.Text);
                curPPtool.ChipPPPosCompensateCoordinate1.Y = double.Parse(PPosCompensateCoordinate1Y.Text);
                curPPtool.ChipPPPosCompensateCoordinate1.Z = double.Parse(PPosCompensateCoordinate1Z.Text);

                curPPtool.ChipPPPosCompensateCoordinate2.X = double.Parse(PPosCompensateCoordinate2X.Text);
                curPPtool.ChipPPPosCompensateCoordinate2.Y = double.Parse(PPosCompensateCoordinate2Y.Text);
                curPPtool.ChipPPPosCompensateCoordinate2.Z = double.Parse(PPosCompensateCoordinate2Z.Text);

                curPPtool.AltimetryOnMark = float.Parse(AltimetryOnMark.Text);


                _systemConfig.SaveConfig();
                HardwareConfiguration.Instance.SaveConfig();

                //WarningBox.FormShow("成功。", "设置完成!", "提示");
            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }

        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    curPPtool.ChipPPPosBracket.X = BondX;
                    curPPtool.ChipPPPosBracket.Y = BondY;
                    curPPtool.ChipPPPosBracket.Z = BondZ;
                    curPPtool.ChipPPPosBracket.Theta = BondT;

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置吸嘴架位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }

                

            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }

        private void btnLoader_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否加载吸嘴？请确认榜头上目前无吸嘴！", "提示") == 1)
            {
                string PPtoolname = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}到吸嘴架位置加载吸嘴", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, double.Parse(ChipPPPosBracketT.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(ChipPPPosBracketX.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(ChipPPPosBracketY.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(ChipPPPosBracketZ.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(ChipPPPosBracketY.Text) - 45, EnumCoordSetType.Absolute);

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, 410.2687, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 88.1397, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 128.4747, EnumCoordSetType.Absolute);



            }
        }

        private void btnCutting_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否卸载吸嘴？", "提示") == 1)
            {
                string PPtoolname = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}到吸嘴架位置卸载吸嘴", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);


                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, double.Parse(ChipPPPosBracketT.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(ChipPPPosBracketX.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(ChipPPPosBracketY.Text) - 45, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(ChipPPPosBracketZ.Text), EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(ChipPPPosBracketY.Text), EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FrmAddPPTool frm = new FrmAddPPTool();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                PPToolSettings newTool = new PPToolSettings() { Name = frm.NewName };

                if (!string.IsNullOrEmpty(frm.TemplateName))
                {
                    var templateTool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == frm.TemplateName);
                    
                    newTool = templateTool;
                    newTool.Name = frm.NewName;
                }
                var templateFolderName = $@"Config\SystemConfiguration\PPTool\{newTool.Name}\UplookingIdentifyPPtoolMatch\";
                var templateTrainFileName = Path.Combine(templateFolderName, $"Template.contourmxml");
                var templateTrainParamName = Path.Combine(templateFolderName, $"TrainParam.xml");
                var templateRunFileName = Path.Combine(templateFolderName, $"Run.xml");
                newTool.UplookingIdentifyPPtoolMatch = new MatchIdentificationParam();
                newTool.UplookingIdentifyPPtoolMatch.DirectLightType = EnumDirectLightSourceType.SingleR;
                newTool.UplookingIdentifyPPtoolMatch.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.LookupRingLightConfig.ChannelNumber);
                newTool.UplookingIdentifyPPtoolMatch.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.LookupDirectLightConfig.ChannelNumber);
                newTool.UplookingIdentifyPPtoolMatch.Templatexml = templateTrainFileName;
                newTool.UplookingIdentifyPPtoolMatch.TemplateParamxml = templateTrainParamName;
                newTool.UplookingIdentifyPPtoolMatch.Runxml = templateRunFileName;

                newTool.LookupCameraOrigion = _systemConfig.PositioningConfig.LookupCameraOrigion;
                _systemConfig.PPToolSettings.Add(newTool);
                _systemConfig.SaveConfig();

                cmbSelRecipe.Items.Clear();

                foreach (var pptool in _systemConfig.PPToolSettings)
                {
                    cmbSelRecipe.Items.Add(pptool.Name);
                }
                cmbSelRecipe.Text = frm.NewName;

                LoadSystemSettingsFromConfig();

               
                WarningBox.FormShow("成功", "添加完成。", "提示");

            }
            frm.Dispose();
        }

        private void cmbSelRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSystemSettingsFromConfig();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == cmbSelRecipe.Text);
            if (cmbSelRecipe.Text != null && curPPtool!=null)
            {
                DataModel.Instance.CurPPtoolName = cmbSelRecipe.Text;
            }
            else
            {
                DataModel.Instance.CurPPtoolName = "";
            }
            
        }
    }
}