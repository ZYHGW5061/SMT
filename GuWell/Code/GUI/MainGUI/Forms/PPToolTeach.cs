using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using JobClsLib;
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

                BracketIndex.Text = curPPtool.BracketIndex.ToString();

                

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

                if (_systemConfig.PositioningConfig.ChipPPPosBracket.Count > 5)
                {
                    ChipPPPosBracketX1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].X.ToString();
                    ChipPPPosBracketY1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].Y.ToString();
                    ChipPPPosBracketZ1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].Z.ToString();
                    ChipPPPosBracketT1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].Theta.ToString();

                    ChipPPPosBracketX2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].X.ToString();
                    ChipPPPosBracketY2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].Y.ToString();
                    ChipPPPosBracketZ2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].Z.ToString();
                    ChipPPPosBracketT2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].Theta.ToString();

                    ChipPPPosBracketX3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].X.ToString();
                    ChipPPPosBracketY3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].Y.ToString();
                    ChipPPPosBracketZ3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].Z.ToString();
                    ChipPPPosBracketT3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].Theta.ToString();

                    ChipPPPosBracketX4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].X.ToString();
                    ChipPPPosBracketY4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].Y.ToString();
                    ChipPPPosBracketZ4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].Z.ToString();
                    ChipPPPosBracketT4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].Theta.ToString();

                    ChipPPPosBracketX5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].X.ToString();
                    ChipPPPosBracketY5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].Y.ToString();
                    ChipPPPosBracketZ5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].Z.ToString();
                    ChipPPPosBracketT5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].Theta.ToString();

                    ChipPPPosBracketX6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].X.ToString();
                    ChipPPPosBracketY6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].Y.ToString();
                    ChipPPPosBracketZ6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].Z.ToString();
                    ChipPPPosBracketT6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].Theta.ToString();
                }

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
                if (PPtoolname != null && PPtoolname != "")
                {
                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    curPPtool.BracketIndex = int.Parse(BracketIndex.Text);


                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].X = double.Parse(ChipPPPosBracketX1.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].Y = double.Parse(ChipPPPosBracketY1.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].Z = double.Parse(ChipPPPosBracketZ1.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].Theta = double.Parse(ChipPPPosBracketT1.Text);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].X = double.Parse(ChipPPPosBracketX2.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].Y = double.Parse(ChipPPPosBracketY2.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].Z = double.Parse(ChipPPPosBracketZ2.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].Theta = double.Parse(ChipPPPosBracketT2.Text);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].X = double.Parse(ChipPPPosBracketX3.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].Y = double.Parse(ChipPPPosBracketY3.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].Z = double.Parse(ChipPPPosBracketZ3.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].Theta = double.Parse(ChipPPPosBracketT3.Text);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].X = double.Parse(ChipPPPosBracketX4.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].Y = double.Parse(ChipPPPosBracketY4.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].Z = double.Parse(ChipPPPosBracketZ4.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].Theta = double.Parse(ChipPPPosBracketT4.Text);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].X = double.Parse(ChipPPPosBracketX5.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].Y = double.Parse(ChipPPPosBracketY5.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].Z = double.Parse(ChipPPPosBracketZ5.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].Theta = double.Parse(ChipPPPosBracketT5.Text);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].X = double.Parse(ChipPPPosBracketX6.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].Y = double.Parse(ChipPPPosBracketY6.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].Z = double.Parse(ChipPPPosBracketZ6.Text);
                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].Theta = double.Parse(ChipPPPosBracketT6.Text);

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



            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }

        }

        private void btnSetup1_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架1位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].X = BondX;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].Y = BondY;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].Z = BondZ;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[0].Theta = BondT;

                    ChipPPPosBracketX1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].X.ToString();
                    ChipPPPosBracketY1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].Y.ToString();
                    ChipPPPosBracketZ1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].Z.ToString();
                    ChipPPPosBracketT1.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[0].Theta.ToString();

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置吸嘴架位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }

            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }
        private void btnSetup2_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架2位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].X = BondX;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].Y = BondY;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].Z = BondZ;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[1].Theta = BondT;

                    ChipPPPosBracketX2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].X.ToString();
                    ChipPPPosBracketY2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].Y.ToString();
                    ChipPPPosBracketZ2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].Z.ToString();
                    ChipPPPosBracketT2.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[1].Theta.ToString();

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置吸嘴架位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }

            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }

        private void btnSetup3_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架3位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].X = BondX;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].Y = BondY;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].Z = BondZ;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[2].Theta = BondT;

                    ChipPPPosBracketX3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].X.ToString();
                    ChipPPPosBracketY3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].Y.ToString();
                    ChipPPPosBracketZ3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].Z.ToString();
                    ChipPPPosBracketT3.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[2].Theta.ToString();

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置吸嘴架位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }

            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }

        private void btnSetup4_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架4位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].X = BondX;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].Y = BondY;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].Z = BondZ;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[3].Theta = BondT;

                    ChipPPPosBracketX4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].X.ToString();
                    ChipPPPosBracketY4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].Y.ToString();
                    ChipPPPosBracketZ4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].Z.ToString();
                    ChipPPPosBracketT4.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[3].Theta.ToString();

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置吸嘴架位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }

            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }

        private void btnSetup5_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架5位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].X = BondX;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].Y = BondY;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].Z = BondZ;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[4].Theta = BondT;

                    ChipPPPosBracketX5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].X.ToString();
                    ChipPPPosBracketY5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].Y.ToString();
                    ChipPPPosBracketZ5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].Z.ToString();
                    ChipPPPosBracketT5.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[4].Theta.ToString();

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置吸嘴架位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }

            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.RecordLog(EnumLogContentType.Error, "Errors occured while saving system Configure", ex);
            }
        }

        private void btnSetup6_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "是否已经移动到吸嘴架6位置？", "提示") == 1)
                {
                    string PPtoolname = cmbSelRecipe.Text;

                    curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == PPtoolname);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    double BondT = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].X = BondX;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].Y = BondY;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].Z = BondZ;
                    _systemConfig.PositioningConfig.ChipPPPosBracket[5].Theta = BondT;

                    ChipPPPosBracketX6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].X.ToString();
                    ChipPPPosBracketY6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].Y.ToString();
                    ChipPPPosBracketZ6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].Z.ToString();
                    ChipPPPosBracketT6.Text = _systemConfig.PositioningConfig.ChipPPPosBracket[5].Theta.ToString();

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

                if (!PPUtility.Instance.LoadPPTool(PPtoolname))
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, $"加载吸嘴{PPtoolname}失败！");
                    WarningBox.FormShow("错误", $"加载吸嘴{PPtoolname}失败！");
                }



            }
        }

        private void btnCutting_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否卸载吸嘴？", "提示") == 1)
            {
                string PPtoolname = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}到吸嘴架位置卸载吸嘴", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                if (!PPUtility.Instance.UnloadPPTool(PPtoolname))
                {
                    LogRecorder.ProductionLog(EnumLogContentType.Error, $"卸载吸嘴{PPtoolname}失败！");
                    WarningBox.FormShow("错误", $"卸载吸嘴{PPtoolname}失败！");
                }
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

                var templateFolder0 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Config\SystemConfiguration\PPTool\{newTool.Name}");
                if (!Directory.Exists(templateFolder0))
                {
                    Directory.CreateDirectory(templateFolder0);
                    Console.WriteLine($"文件夹已创建: {templateFolder0}");
                }
                var templateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Config\SystemConfiguration\PPTool\{newTool.Name}\UplookingIdentifyPPtoolMatch");
                if (!Directory.Exists(templateFolder))
                {
                    Directory.CreateDirectory(templateFolder);
                    Console.WriteLine($"文件夹已创建: {templateFolder}");
                }

                var templateFolderName = $@"Config\SystemConfiguration\PPTool\{newTool.Name}\UplookingIdentifyPPtoolMatch\";
                var templateTrainFileName = Path.Combine(templateFolderName, $"Template.contourmxml");
                var templateTrainParamName = Path.Combine(templateFolderName, $"TrainParam.xml");
                var templateRunFileName = Path.Combine(templateFolderName, $"Run.xml");

                newTool.PPName = newTool.Name;
                newTool.EnumPPtool = EnumPPtool.PPtool1;
                newTool.StageAxisTheta = EnumStageAxis.ChipPPT;
                newTool.StageAxisZ = EnumStageAxis.None;
                newTool.PPFreeZ = 0;
                newTool.PPWorkZ = 0;
                newTool.PPVaccumSwitch =  EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch;
                newTool.PPBlowSwitch =  EnumBoardcardDefineOutputIO.ChipPPBlowSwitch;
                newTool.PPVaccumNormally =  EnumBoardcardDefineInputIO.ChipPPVaccumNormally;

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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否删除吸嘴？", "提示") == 1)
            {
                var curPPtool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.Name == cmbSelRecipe.Text);
                _systemConfig.PPToolSettings.Remove(curPPtool);
                LogRecorder.RecordUserOperationLog($"删除吸嘴工具{cmbSelRecipe.Text}", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                cmbSelRecipe.Items.Clear();

                foreach (var pptool in _systemConfig.PPToolSettings)
                {
                    cmbSelRecipe.Items.Add(pptool.Name);
                }
                cmbSelRecipe.Text = "";

                
            }
                
            
        }
    }
}