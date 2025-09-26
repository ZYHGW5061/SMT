using BoardCardControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using DispensingMachineManagerClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using JobClsLib;
using LaserSensorManagerClsLib;
using PositioningSystemClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCalibrationClsLib;
using UserManagerClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace ControlPanelClsLib.Tools
{
    public partial class FrmEpoxtTool2 : DevExpress.XtraEditors.XtraForm
    {
        private bool _enablePollingIO4;

        private SynchronizationContext _syncContext;

        private DispensingMachineManager _DispensingMachineManager
        {
            get { return DispensingMachineManager.Instance; }
        }

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

        private DispenserSettings curDispenser;

        public FrmEpoxtTool2()
        {
            InitializeComponent();

            cmbSelRecipe.Items.Clear();

            foreach (var dispenser in _systemConfig.DispenserSettings)
            {
                cmbSelRecipe.Items.Add(dispenser.Name);
            }


            combDispensingMode.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumDispensingMode)))
            {
                combDispensingMode.Items.Add(item);
            }
            

            InitController();

            BindLabelClickEvents();

            DataModel.Instance.PropertyChanged += DataModel_PropertyChanged;
            _syncContext = SynchronizationContext.Current;
        }

        private void LoadSystemSettingsFromConfig()
        {
            try
            {
                string dispensername = cmbSelRecipe.Text;

                curDispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == dispensername);

                combDispensingMode.Text = curDispenser.DispensingMode.ToString();

                TrackEpoxtSpotCoordinateX.Text = curDispenser.TrackEpoxtSpotCoordinate.X.ToString();
                TrackEpoxtSpotCoordinateY.Text = curDispenser.TrackEpoxtSpotCoordinate.Y.ToString();
                TrackEpoxtSpotCoordinateZ.Text = curDispenser.TrackEpoxtSpotCoordinate.Z.ToString();

                TrackBondCameraToEpoxtSpotCoordinateX.Text = curDispenser.TrackBondCameraToEpoxtSpotCoordinate.X.ToString();
                TrackBondCameraToEpoxtSpotCoordinateY.Text = curDispenser.TrackBondCameraToEpoxtSpotCoordinate.Y.ToString();
                TrackBondCameraToEpoxtSpotCoordinateZ.Text = curDispenser.TrackBondCameraToEpoxtSpotCoordinate.Z.ToString();

                EpoxtToDippingglueCoordinateX.Text = curDispenser.EpoxtToDippingglueCoordinate.X.ToString();
                EpoxtToDippingglueCoordinateY.Text = curDispenser.EpoxtToDippingglueCoordinate.Y.ToString();
                EpoxtToDippingglueCoordinateZ.Text = curDispenser.EpoxtToDippingglueCoordinate.Z.ToString();

                DispenserPosOffsetXWithBondCamera.Text = curDispenser.DispenserPosOffsetXWithBondCamera.ToString();
                DispenserPosOffsetYWithBondCamera.Text = curDispenser.DispenserPosOffsetYWithBondCamera.ToString();

                DispenserSystemPosZMM.Text = curDispenser.DispenserSystemPosZMM.ToString();


            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while loading system Configure", ex);
            }
        }


        private void InitController()
        {
            #region SerialPort

            if (DataModel.Instance.EpoxtCH > 0)
            {
                EpoxtCH.Value = DataModel.Instance.EpoxtCH;
            }

            EpoxtTime.Value = (decimal)DataModel.Instance.EpoxtTime;
            EpoxtPressure.Value = (decimal)DataModel.Instance.EpoxtPressure;
            EpoxtVacuum.Value = (decimal)DataModel.Instance.EpoxtVacuum;
            EpoxtSHOT.Value = DataModel.Instance.EpoxtSHOT;
            EpoxtCurrentPressure.Value = (decimal)DataModel.Instance.EpoxtCurrentPressure;
            EpoxtCurrentTime.Value = (decimal)DataModel.Instance.EpoxtCurrentTime;
            if (DataModel.Instance.EpoxtMode > -1 && DataModel.Instance.EpoxtMode < 2)
            {
                EpoxtMode.SelectedIndex = DataModel.Instance.EpoxtMode;
            };
            if (DataModel.Instance.EpoxtMode2 > -1 && DataModel.Instance.EpoxtMode2 < 2)
            {
                EpoxtMode2.SelectedIndex = DataModel.Instance.EpoxtMode2;
            };

            #endregion

        }

        private void BindLabelClickEvents()
        {
            // 遍历窗体中的所有控件  
            foreach (Control control in this.tableLayoutPanel1.Controls)
            {
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is NumericUpDown num && num.Tag?.ToString() == "ValueChanged")
                {
                    // 绑定 Click 事件  
                    num.ValueChanged += Numeric_ValueChanged;
                }
            }
        }

        private void Numeric_ValueChanged(object sender, EventArgs e)
        {
            if (sender is NumericUpDown clickedNumeric)
            {
                string Name = clickedNumeric.Name;
                switch (Name)
                {
                    case "EpoxtCH":
                        if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 && EpoxtCH.Value != DataModel.Instance.EpoxtCH)
                        {
                            if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                            {
                                _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.通道加载, EpoxtCH.Value.ToString());
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "EpoxtTime":
                        if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 &&
                            EpoxtMode.SelectedIndex > -1 && EpoxtMode.SelectedIndex < 2 &&
                            EpoxtTime.Value >= 0.001m && EpoxtTime.Value < 9999.999m && (double)EpoxtTime.Value != DataModel.Instance.EpoxtTime &&
                            EpoxtPressure.Value >= 30.0m && EpoxtPressure.Value <= 500.0m &&
                            EpoxtVacuum.Value >= -5.00m && EpoxtVacuum.Value <= 0)
                        {
                            if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                            {
                                _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出条件,
                                    EpoxtCH.Value.ToString(), EpoxtMode.SelectedIndex.ToString(), EpoxtTime.Value.ToString(), EpoxtPressure.Value.ToString(),
                                    EpoxtVacuum.Value.ToString());
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "EpoxtMode":
                        if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 &&
                            EpoxtMode.SelectedIndex > -1 && EpoxtMode.SelectedIndex < 2 && EpoxtMode.SelectedIndex != DataModel.Instance.EpoxtMode &&
                            EpoxtTime.Value >= 0.001m && EpoxtTime.Value < 9999.999m &&
                            EpoxtPressure.Value >= 30.0m && EpoxtPressure.Value <= 500.0m &&
                            EpoxtVacuum.Value >= -5.00m && EpoxtVacuum.Value <= 0)
                        {
                            if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                            {
                                _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出条件,
                                    EpoxtCH.Value.ToString(), EpoxtMode.SelectedIndex.ToString(), EpoxtTime.Value.ToString(), EpoxtPressure.Value.ToString(),
                                    EpoxtVacuum.Value.ToString());
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "EpoxtPressure":
                        if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 &&
                            EpoxtMode.SelectedIndex > -1 && EpoxtMode.SelectedIndex < 2 &&
                            EpoxtTime.Value >= 0.001m && EpoxtTime.Value < 9999.999m &&
                            EpoxtPressure.Value >= 30.0m && EpoxtPressure.Value <= 500.0m && (double)EpoxtPressure.Value != DataModel.Instance.EpoxtPressure &&
                            EpoxtVacuum.Value >= -5.00m && EpoxtVacuum.Value <= 0)
                        {
                            if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                            {
                                _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出条件,
                                    EpoxtCH.Value.ToString(), EpoxtMode.SelectedIndex.ToString(), EpoxtTime.Value.ToString(), EpoxtPressure.Value.ToString(),
                                    EpoxtVacuum.Value.ToString());
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "EpoxtVacuum":
                        if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 &&
                            EpoxtMode.SelectedIndex > -1 && EpoxtMode.SelectedIndex < 2 &&
                            EpoxtTime.Value >= 0.001m && EpoxtTime.Value < 9999.999m &&
                            EpoxtPressure.Value >= 30.0m && EpoxtPressure.Value <= 500.0m &&
                            EpoxtVacuum.Value >= -5.00m && EpoxtVacuum.Value <= 0 && (double)EpoxtVacuum.Value != DataModel.Instance.EpoxtVacuum)
                        {
                            if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                            {
                                _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出条件,
                                    EpoxtCH.Value.ToString(), EpoxtMode.SelectedIndex.ToString(), EpoxtTime.Value.ToString(), EpoxtPressure.Value.ToString(),
                                    EpoxtVacuum.Value.ToString());
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "EpoxtMode2":
                        if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 && EpoxtCH.Value != DataModel.Instance.EpoxtCH)
                        {
                            if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                            {
                                _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.通道加载, EpoxtCH.Value.ToString());
                                Thread.Sleep(50);
                            }
                        }
                        break;
                }
            }

        }



        private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_syncContext == null)
            {
                return;
            }

            #region SerialPort

            if (e.PropertyName == nameof(DataModel.EpoxtCH))
            {
                _syncContext.Post(_ => EpoxtCH.Value = DataModel.Instance.EpoxtCH, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtTime))
            {
                _syncContext.Post(_ => EpoxtTime.Value = (decimal)DataModel.Instance.EpoxtTime, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtPressure))
            {
                _syncContext.Post(_ => EpoxtPressure.Value = (decimal)DataModel.Instance.EpoxtPressure, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtVacuum))
            {
                _syncContext.Post(_ => EpoxtVacuum.Value = (decimal)DataModel.Instance.EpoxtVacuum, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtSHOT))
            {
                _syncContext.Post(_ => EpoxtSHOT.Value = DataModel.Instance.EpoxtSHOT, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtCurrentPressure))
            {
                _syncContext.Post(_ => EpoxtCurrentPressure.Value = (decimal)DataModel.Instance.EpoxtCurrentPressure, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtCurrentTime))
            {
                _syncContext.Post(_ => EpoxtCurrentTime.Value = (decimal)DataModel.Instance.EpoxtCurrentTime, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtMode))
            {
                _syncContext.Post(_ => {
                    if (DataModel.Instance.EpoxtMode > -1 && DataModel.Instance.EpoxtMode < 2)
                    {
                        EpoxtMode.SelectedIndex = DataModel.Instance.EpoxtMode;
                    }
                }, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtMode2))
            {
                _syncContext.Post(_ => {
                    if (DataModel.Instance.EpoxtMode2 > -1 && DataModel.Instance.EpoxtMode2 < 2)
                    {
                        EpoxtMode2.SelectedIndex = DataModel.Instance.EpoxtMode2;
                    }
                }, null);
            }

            #endregion


        }

        private void EpoxtMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 &&
                EpoxtMode.SelectedIndex > -1 && EpoxtMode.SelectedIndex < 2 && EpoxtMode.SelectedIndex != DataModel.Instance.EpoxtMode)
            {
                if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                {
                    if (EpoxtMode.SelectedIndex == 0)
                    {
                        _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.TIMED模式切换);
                    }
                    else
                    {
                        _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.MANUAL模式切换);
                    }

                    Thread.Sleep(50);
                }
            }
        }

        private void EpoxtMode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EpoxtCH.Value > 0 && EpoxtCH.Value < 401 && EpoxtCH.Value != DataModel.Instance.EpoxtCH &&
                EpoxtMode2.SelectedIndex > -1 && EpoxtMode2.SelectedIndex < 2 && EpoxtMode2.SelectedIndex != DataModel.Instance.EpoxtMode2)
            {
                if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                {
                    _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.Σ功能水头差自动校正,
                        EpoxtCH.Value.ToString(), EpoxtMode2.SelectedIndex.ToString());
                    Thread.Sleep(50);
                }
            }
        }

        private void checkSpot_CheckedChanged(object sender, EventArgs e)
        {
            if (checkSpot.Checked)
            {
                if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                {
                    _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出要求);
                }
            }
            else
            {
                if (_DispensingMachineManager.GetCurrentHardware().IsConnect)
                {
                    _DispensingMachineManager.GetCurrentHardware().Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出要求);
                }
            }
            LogRecorder.RecordUserOperationLog($"点胶配方:{EpoxtCH.Name}点胶", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
        }

        private void EpoxtCH_ValueChanged(object sender, EventArgs e)
        {

        }


        private void btnDrawCross_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将划胶！", "确认榜头Z轴已到合适高度？", "提示") == 1)
            {
                try
                {
                    LogRecorder.RecordUserOperationLog($"点胶配方:{EpoxtCH.Name}画胶", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                    DispenserUtility.Instance.DrawGreekCross((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value);
                    WarningBox.FormShow("成功！", "划胶动作完成！", "提示");
                }
                catch (Exception)
                {
                }

            }

        }

        private void FrmEpoxtTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            _enablePollingIO4 = false;
            e.Cancel = false;
        }

        private void FrmEpoxtTool_Load(object sender, EventArgs e)
        {
            _enablePollingIO4 = true;
            Task.Run(new Action(ReadSerialPortTask4));

            combDispensingMode.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumDispensingMode)))
            {
                combDispensingMode.Items.Add(item);
            }
        }


        private void ReadSerialPortTask4()
        {
            while (_enablePollingIO4)
            {
                Thread.Sleep(100);
                try
                {

                    ParseDataAndUpdateSerialPortEpoxt();
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortEpoxt,Error.", ex);
                }
            }
        }

        private void ParseDataAndUpdateSerialPortEpoxt()
        {

            if (_DispensingMachineManager.GetCurrentHardware() != null && _DispensingMachineManager.GetCurrentHardware().IsConnect)
            {
                List<decimal> data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取供气压力);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtCurrentPressure = (double)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取秒表);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtCurrentTime = (double)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取当前通道);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtCH = (int)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取吐出条件, DataModel.Instance.EpoxtCH.ToString());
                if (data?.Count > 4)
                {
                    DataModel.Instance.EpoxtMode = (int)data[1];
                    DataModel.Instance.EpoxtTime = (double)data[2];
                    DataModel.Instance.EpoxtPressure = (double)data[3];
                    DataModel.Instance.EpoxtVacuum = (double)data[4];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取计数器);
                if (data?.Count > 0)
                {
                    DataModel.Instance.EpoxtSHOT = (int)data[0];
                }
                Thread.Sleep(20);
                data = _DispensingMachineManager.GetCurrentHardware().Get(DispensingMachineControllerClsLib.MUSASHICommandenum.获取Σ功能水头差自动校正);
                if (data?.Count > 1)
                {
                    DataModel.Instance.EpoxtMode2 = (int)data[1];
                }
                Thread.Sleep(20);
                DataModel.Instance.PressureIsconnect = true;
                Thread.Sleep(50);
            }
            else
            {
                DataModel.Instance.PressureIsconnect = false;
            }

        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否校准点胶器？", "提示") == 1)
            {
                string dispensername = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"点胶器:{dispensername}校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.EpoxtRun(dispensername, 0);
                LoadSystemSettingsFromConfig();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "胶针是否在蘸胶位置？", "提示") == 1)
            {
                string dispensername = cmbSelRecipe.Text;
                LogRecorder.RecordUserOperationLog($"点胶器:{dispensername}蘸胶位置校准", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                SystemCalibration.Instance.DippingglueRun(dispensername, 0);
                LoadSystemSettingsFromConfig();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            FrmAddEpoxtTool frm = new FrmAddEpoxtTool();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DispenserSettings newTool = new DispenserSettings() { Name = frm.NewName };

                if (!string.IsNullOrEmpty(frm.TemplateName))
                {
                    var templateTool = _systemConfig.DispenserSettings.FirstOrDefault(i => i.Name == frm.TemplateName);
                    newTool = templateTool;
                    newTool.Name = frm.NewName;
                }
                _systemConfig.DispenserSettings.Add(newTool);
                _systemConfig.SaveConfig();

                cmbSelRecipe.Items.Clear();

                foreach (var pptool in _systemConfig.DispenserSettings)
                {
                    cmbSelRecipe.Items.Add(pptool.Name);
                }
                cmbSelRecipe.Text = frm.NewName;

                LoadSystemSettingsFromConfig();


                WarningBox.FormShow("成功", "添加完成。", "提示");

            }
            frm.Dispose();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string dispensername = cmbSelRecipe.Text;

                curDispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == dispensername);

                curDispenser.DispensingMode = (EnumDispensingMode)Enum.Parse(typeof(EnumDispensingMode), combDispensingMode.Text);

                curDispenser.TrackEpoxtSpotCoordinate.X = double.Parse(TrackEpoxtSpotCoordinateX.Text);
                curDispenser.TrackEpoxtSpotCoordinate.Y = double.Parse(TrackEpoxtSpotCoordinateY.Text);
                curDispenser.TrackEpoxtSpotCoordinate.Z = double.Parse(TrackEpoxtSpotCoordinateZ.Text);

                curDispenser.TrackBondCameraToEpoxtSpotCoordinate.X = double.Parse(TrackBondCameraToEpoxtSpotCoordinateX.Text);
                curDispenser.TrackBondCameraToEpoxtSpotCoordinate.Y = double.Parse(TrackBondCameraToEpoxtSpotCoordinateY.Text);
                curDispenser.TrackBondCameraToEpoxtSpotCoordinate.Z = double.Parse(TrackBondCameraToEpoxtSpotCoordinateZ.Text);

                curDispenser.EpoxtToDippingglueCoordinate.X = double.Parse(EpoxtToDippingglueCoordinateX.Text);
                curDispenser.EpoxtToDippingglueCoordinate.Y = double.Parse(EpoxtToDippingglueCoordinateY.Text);
                curDispenser.EpoxtToDippingglueCoordinate.Z = double.Parse(EpoxtToDippingglueCoordinateZ.Text);

                curDispenser.DispenserPosOffsetXWithBondCamera = -(float)(curDispenser.TrackEpoxtSpotCoordinate.X - curDispenser.TrackBondCameraToEpoxtSpotCoordinate.X);
                curDispenser.DispenserPosOffsetYWithBondCamera = (float)(curDispenser.TrackEpoxtSpotCoordinate.Y - curDispenser.TrackBondCameraToEpoxtSpotCoordinate.Y);

                DispenserPosOffsetXWithBondCamera.Text = curDispenser.DispenserPosOffsetXWithBondCamera.ToString();
                DispenserPosOffsetYWithBondCamera.Text = curDispenser.DispenserPosOffsetYWithBondCamera.ToString();
                curDispenser.DispenserSystemPosZMM = float.Parse(DispenserSystemPosZMM.Text);


                _systemConfig.SaveConfig();
                HardwareConfiguration.Instance.SaveConfig();

                //WarningBox.FormShow("成功。", "设置完成!", "提示");
            }
            catch (Exception ex)
            {
                _systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
            }

        }

        private void cmbSelRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSystemSettingsFromConfig();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否删除点胶器？", "提示") == 1)
            {
                var Dispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == cmbSelRecipe.Text);
                _systemConfig.DispenserSettings.Remove(Dispenser);
                LogRecorder.RecordUserOperationLog($"删除点胶器工具{cmbSelRecipe.Text}", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                cmbSelRecipe.Items.Clear();

                foreach (var Dispensertool in _systemConfig.DispenserSettings)
                {
                    cmbSelRecipe.Items.Add(Dispensertool.Name);
                }
                cmbSelRecipe.Text = "";
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将记录划胶的高度位置！", "确认已定位到合适的划胶位置？", "提示") == 1)
            {
                float DispenserWorkHeight = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);

                DispenserSystemPosZMM.Text = DispenserWorkHeight.ToString();

                curDispenser.DispenserSystemPosZMM = float.Parse(DispenserSystemPosZMM.Text);

                //_editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM = float.Parse(DispenserSystemPosZMM.Text.Trim());
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将升起!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.UpDispenserCylinder();
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将下降!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.DownDispenserCylinder();
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将画胶!", "相机是否对准画胶位置？确认画胶高度？", "提示") == 1)
            {
                string dispensername = cmbSelRecipe.Text;

                curDispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == dispensername);
                if (curDispenser != null)
                {
                    float BondX = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    float BondY = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    float BondZ = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        IOUtilityHelper.Instance.UpDispenserCylinder();
                        var despenserAndBondCameraOffsetX = float.IsNaN(curDispenser.DispenserPosOffsetXWithBondCamera)
                       ? -_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X : curDispenser.DispenserPosOffsetXWithBondCamera;
                        var despenserAndBondCameraOffsetY = float.IsNaN(curDispenser.DispenserPosOffsetYWithBondCamera)
                            ? _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y : curDispenser.DispenserPosOffsetYWithBondCamera;
                        var X = BondX + despenserAndBondCameraOffsetX;
                        var Y = BondY + despenserAndBondCameraOffsetY;

                        if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            IOUtilityHelper.Instance.DownDispenserCylinder();

                            var Z = curDispenser.DispenserSystemPosZMM;
                            ExecutionController.Instance.WaitIfPaused();
                            if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                if (false)
                                {
                                    //DispenserUtility.Instance.ExecutePointRecipe(EpoxtCH.Value.ToString());
                                }
                                else
                                {
                                    DispenserUtility.Instance.DrawGreekCross((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value);
                                }

                                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                {
                                    IOUtilityHelper.Instance.UpDispenserCylinder();

                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(BondX, BondY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                    {
                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, BondZ, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                        {
                                            WarningBox.FormShow("步骤提示", "画胶成功", "提示");
                                        }

                                    }
                                }
                                else
                                {
                                    WarningBox.FormShow("画胶失败", "画胶工具不存在", "提示");
                                }
                            }
                            else
                            {
                                WarningBox.FormShow("画胶失败", "画胶工具不存在", "提示");
                            }

                        }
                        else
                        {
                            WarningBox.FormShow("画胶失败", "画胶工具不存在", "提示");
                        }
                    }
                    else
                    {
                        WarningBox.FormShow("画胶失败", "画胶工具不存在", "提示");
                    }


                }
                else
                {
                    WarningBox.FormShow("画胶失败", "画胶工具不存在", "提示");
                }

            }

        }

        private void simpleButton6_Click_1(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将点胶!", "相机是否对准点胶位置？确认点胶高度？", "提示") == 1)
            {
                string dispensername = cmbSelRecipe.Text;

                curDispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == dispensername);
                if(curDispenser != null)
                {
                    float BondX = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    float BondY = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    float BondZ = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        IOUtilityHelper.Instance.UpDispenserCylinder();
                        var despenserAndBondCameraOffsetX = float.IsNaN(curDispenser.DispenserPosOffsetXWithBondCamera)
                       ? -_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X : curDispenser.DispenserPosOffsetXWithBondCamera;
                        var despenserAndBondCameraOffsetY = float.IsNaN(curDispenser.DispenserPosOffsetYWithBondCamera)
                            ? _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y : curDispenser.DispenserPosOffsetYWithBondCamera;
                        var X = BondX + despenserAndBondCameraOffsetX;
                        var Y = BondY + despenserAndBondCameraOffsetY;

                        if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            IOUtilityHelper.Instance.DownDispenserCylinder();

                            var Z = curDispenser.DispenserSystemPosZMM;
                            ExecutionController.Instance.WaitIfPaused();
                            if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                if (true)
                                {
                                    DispenserUtility.Instance.ExecutePointRecipe(EpoxtCH.Value.ToString());
                                }
                                else
                                {
                                    //DispenserUtility.Instance.DrawCross((float)numericUpDown1.Value, (float)numericUpDown2.Value);
                                }

                                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                {
                                    IOUtilityHelper.Instance.UpDispenserCylinder();

                                    if (_positioningSystem.BondXYUnionMovetoSystemCoor(BondX, BondY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                    {
                                        if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, BondZ, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                        {
                                            WarningBox.FormShow("步骤提示", "点胶成功", "提示");
                                        }

                                    }
                                }
                                else
                                {
                                    WarningBox.FormShow("点胶失败", "点胶工具不存在", "提示");
                                }
                            }
                            else
                            {
                                WarningBox.FormShow("点胶失败", "点胶工具不存在", "提示");
                            }

                        }
                        else
                        {
                            WarningBox.FormShow("点胶失败", "点胶工具不存在", "提示");
                        }
                    }
                    else
                    {
                        WarningBox.FormShow("点胶失败", "点胶工具不存在", "提示");
                    }


                }
                else
                {
                    WarningBox.FormShow("点胶失败", "点胶工具不存在", "提示");
                }

            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将测量划胶的高度位置！", "确认相机已定位到合适的划胶位置？", "提示") == 1)
            {
                try
                {
                    CreateWaitDialog();
                    BoardCardManager.Instance.GetCurrentController().IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                    //将激光测高仪移动到当前相机位置
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                    EnumCoordSetType.Relative);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                        EnumCoordSetType.Relative);
                    //读取激光测高仪读数
                    Thread.Sleep(500);
                    double distance = -1;
                    distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                    if (distance >= 0)
                    {
                        DataModel.Instance.LaserValue = distance / 10000.0f;
                    }
                    else
                    {
                        DataModel.Instance.LaserValue = 0;
                    }
                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 10000;
                    var curLaserMeasureH = DataModel.Instance.LaserValue;
                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 1000;
                    //根据校准数据及当前的激光测高仪读数计算吸嘴工作高度
                    var curBondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    //var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z;
                    var offsetZ = curBondZ - (float)TrackBondCameraToEpoxtSpotCoordinateZ.Value;
                    var offsetMeasureZ = -(curLaserMeasureH - _systemConfig.PositioningConfig.TrackLaserSensorZ);
                    var componentZ = offsetMeasureZ - offsetZ;

                    float DispenserWorkHeight = (float)((double)TrackEpoxtSpotCoordinateZ.Value + offsetZ - offsetMeasureZ - _systemConfig.PositioningConfig.BondOrigion.Z);

                    DispenserSystemPosZMM.Text = DispenserWorkHeight.ToString();

                    curDispenser.DispenserSystemPosZMM = float.Parse(DispenserSystemPosZMM.Text);

                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                        EnumCoordSetType.Relative);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                        EnumCoordSetType.Relative);
                    CloseWaitDialog();
                    WarningBox.FormShow("成功！", "测高完成！", "提示");
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, "BPHeightProgram-LaserMeasureHeight,Error.", ex);
                    WarningBox.FormShow("发生异常！", "测高失败！", "提示");
                }
                finally
                {
                    CloseWaitDialog();
                }

                //_editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM = float.Parse(DispenserSystemPosZMM.Text.Trim());
            }
        }


        /// <summary>
        /// 显示等待窗口
        /// </summary>
        protected void CreateWaitDialog(string description = null)
        {
            if (SplashScreenManager.Default == null)
            {
                SplashScreenManager.ShowForm(this.FindForm(), typeof(DemoWaitForm), false, true);
            }
            //SplashScreenManager.Default.Properties.ParentForm = this.FindForm();
            SplashScreenManager.Default.SetWaitFormCaption("");
            SplashScreenManager.Default.SetWaitFormDescription(string.IsNullOrEmpty(description) ? "Loading..." : description);
        }

        /// <summary>
        /// 关闭等待窗口
        /// </summary>
        protected void CloseWaitDialog()
        {
            if (SplashScreenManager.Default != null)
            {
                SplashScreenManager.CloseForm();
            }
        }

    }
}