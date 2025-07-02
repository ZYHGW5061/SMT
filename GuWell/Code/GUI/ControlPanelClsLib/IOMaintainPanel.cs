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
using GlobalDataDefineClsLib;
using IOUtilityClsLib;
using GlobalToolClsLib;
using System.Threading;
using BoardCardControllerClsLib;
using LightControllerManagerClsLib;
using ConfigurationClsLib;
using LaserSensorManagerClsLib;
using DynamometerManagerClsLib;
using WestDragon.Framework.BaseLoggerClsLib;

namespace ControlPanelClsLib
{
    public partial class IOMaintainPanel : UserControl
    {

        private LaserSensorManager _LaserSensorManager
        {
            get { return LaserSensorManager.Instance; }
        }
        private DynamometerManager _DynamometerManager
        {
            get { return DynamometerManager.Instance; }
        }

        public bool _enablePollingIO2 { set; get; }

        private SynchronizationContext _syncContext;
        private IBoardCardController _boardCardController;
        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        private LightControllerManager _LightControllerManager
        {
            get { return LightControllerManager.Instance; }
        }


        public IOMaintainPanel()
        {
            InitializeComponent();
            BindLabelClickEvents();

            _syncContext = SynchronizationContext.Current;

            _boardCardController = BoardCardManager.Instance.GetCurrentController();
        }


        private void BindLabelClickEvents()
        {
            // 遍历窗体中的所有控件  
            foreach (Control control in this.tableLayoutPanel5.Controls)
            {
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is Label label && label.Tag?.ToString() == "Check")
                {
                    // 绑定 Click 事件  
                    label.Click += Label_Click;
                }
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is NumericUpDown num && num.Tag?.ToString() == "ValueChanged")
                {
                    // 绑定 Click 事件  
                    num.ValueChanged += Numeric_ValueChanged;
                }
            }
            // 遍历窗体中的所有控件  
            foreach (Control control in this.tableLayoutPanel2.Controls)
            {
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is Label label && label.Tag?.ToString() == "Check")
                {
                    // 绑定 Click 事件  
                    label.Click += Label_Click;
                }
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is NumericUpDown num && num.Tag?.ToString() == "ValueChanged")
                {
                    // 绑定 Click 事件  
                    num.ValueChanged += Numeric_ValueChanged;
                }
            }
            // 遍历窗体中的所有控件  
            foreach (Control control in this.tableLayoutPanel4.Controls)
            {
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is Label label && label.Tag?.ToString() == "Check")
                {
                    // 绑定 Click 事件  
                    label.Click += Label_Click;
                }
                // 检查控件是否为 Label 且 Tag 属性为 "Check"  
                if (control is NumericUpDown num && num.Tag?.ToString() == "ValueChanged")
                {
                    // 绑定 Click 事件  
                    num.ValueChanged += Numeric_ValueChanged;
                }
            }
        }
        

        private void InitializeTool()
        {
            DataModel.Instance.PropertyChanged += DataModel_PropertyChanged;
            _syncContext = SynchronizationContext.Current;

            foreach (EnumStageAxis axis in Enum.GetValues(typeof(EnumStageAxis)))
            {
                if (axis == EnumStageAxis.None)
                {
                    continue;
                }
                AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(axis);

                if (_axisConfig?.RunningType == EnumRunningType.Actual)
                {
                    Updatename(axis, _axisConfig?.Type.ToString());
                }
                else
                {
                    Updatename(axis, _axisConfig?.Type.ToString(), false);
                }

            }

        }

        private void InitializeInputIOStatus()
        {
            #region InputIO

            ChipPPVaccumStatus.ForeColor = (DataModel.Instance.ChipPPVaccumNormally ? true : false) ? Color.Green : Color.DarkGray;
            TransportInPlaceSignal1.ForeColor = (DataModel.Instance.SubmountPPVaccumNormally ? true : false) ? Color.Green : Color.DarkGray;
            TransportInPlaceSignal2.ForeColor = (DataModel.Instance.EutecticError ? true : false) ? Color.Green : Color.DarkGray;
            TransportInPlaceSignal3.ForeColor = (DataModel.Instance.EutecticComplete ? true : false) ? Color.Green : Color.DarkGray;
            SafeDoorSensor1.ForeColor = (DataModel.Instance.SafeDoorSensor1 ? true : false) ? Color.Green : Color.DarkGray;
            SafeDoorSensor2.ForeColor = (DataModel.Instance.SafeDoorSensor2 ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtPON.ForeColor = (DataModel.Instance.EpoxtPON ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtDSO.ForeColor = (DataModel.Instance.EpoxtDSO ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtEND.ForeColor = (DataModel.Instance.EpoxtEND ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtERROR.ForeColor = (DataModel.Instance.EpoxtERROR ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtALARM.ForeColor = (DataModel.Instance.EpoxtALARM ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtALARM2.ForeColor = (DataModel.Instance.EpoxtALARM2 ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtRSM.ForeColor = (DataModel.Instance.EpoxtRSM ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtREADY.ForeColor = (DataModel.Instance.EpoxtREADY ? true : false) ? Color.Green : Color.DarkGray;


            #endregion


        }
        private void InitializeOutputIOStatus()
        {
            #region OutputIO

            ChipPPVaccumSwitch.ForeColor = (DataModel.Instance.ChipPPVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtliftCylinder.ForeColor = (DataModel.Instance.EpoxtliftCylinder ? true : false) ? Color.Green : Color.DarkGray;
            TransportCylinder1.ForeColor = (DataModel.Instance.TransportCylinder1 ? true : false) ? Color.Green : Color.DarkGray;
            MaterialPlatformVaccumSwitch.ForeColor = (DataModel.Instance.MaterialPlatformVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray;
            EjectionSystemVaccumSwitch.ForeColor = (DataModel.Instance.EjectionSystemVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray;
            WaferFingerCylinder.ForeColor = (DataModel.Instance.WaferFingerCylinder ? true : false) ? Color.Green : Color.DarkGray;
            TowerRedLight.ForeColor = (DataModel.Instance.TowerRedLight ? true : false) ? Color.Green : Color.DarkGray;
            TowerYellowLight.ForeColor = (DataModel.Instance.TowerYellowLight ? true : false) ? Color.Green : Color.DarkGray;
            TowerGreenLight.ForeColor = (DataModel.Instance.TowerGreenLight ? true : false) ? Color.Green : Color.DarkGray;
            ChipPPBlowSwitch.ForeColor = (DataModel.Instance.ChipPPBlowSwitch ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtDIS.ForeColor = (DataModel.Instance.EpoxtDIS ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtENABLE.ForeColor = (DataModel.Instance.EpoxtENABLE ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtTMD.ForeColor = (DataModel.Instance.EpoxtTMD ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtTMS.ForeColor = (DataModel.Instance.EpoxtTMS ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtC_CNT.ForeColor = (DataModel.Instance.EpoxtC_CNT ? true : false) ? Color.Green : Color.DarkGray;
            EpoxtRESET.ForeColor = (DataModel.Instance.EpoxtRESET ? true : false) ? Color.Green : Color.DarkGray;


            #endregion


        }
        private void InitializeOutputSerialPortStatus()
        {
            #region SerialPort

            BondRed.Value = DataModel.Instance.BondRed;
            BondGreen.Value = DataModel.Instance.BondGreen;
            BondBlue.Value = DataModel.Instance.BondBlue;
            BondRing.Value = DataModel.Instance.BondRing;
            WaferRed.Value = DataModel.Instance.WaferRed;
            WaferGreen.Value = DataModel.Instance.WaferGreen;
            WaferBlue.Value = DataModel.Instance.WaferBlue;
            WaferRing.Value = DataModel.Instance.WaferRing;
            UpLookingRed.Value = DataModel.Instance.UpLookingRed;
            UpLookingGreen.Value = DataModel.Instance.UpLookingGreen;
            UpLookingBlue.Value = DataModel.Instance.UpLookingBlue;
            UpLookingRing.Value = DataModel.Instance.UpLookingRing;
            LaserValue.Value = (decimal)DataModel.Instance.LaserValue;
            PressureValue1.Value = (decimal)DataModel.Instance.PressureValue1;
            PressureValue2.Value = (decimal)DataModel.Instance.PressureValue2;


            #endregion





        }

        private void InitializeOutputMotorStatus()
        {

            #region Motor

            UpdatePosition(EnumStageAxis.BondX, DataModel.Instance.BondX);
            SetAxisSta(EnumStageAxis.BondX, DataModel.Instance.BondXSta);

            UpdatePosition(EnumStageAxis.BondY, DataModel.Instance.BondY);
            SetAxisSta(EnumStageAxis.BondY, DataModel.Instance.BondYSta);
            UpdatePosition(EnumStageAxis.BondZ, DataModel.Instance.BondZ);
            SetAxisSta(EnumStageAxis.BondZ, DataModel.Instance.BondZSta);
            UpdatePosition(EnumStageAxis.ChipPPT, DataModel.Instance.ChipPPT);
            SetAxisSta(EnumStageAxis.ChipPPT, DataModel.Instance.ChipPPTSta);
            UpdatePosition(EnumStageAxis.PPtoolBankTheta, DataModel.Instance.PPtoolBankTheta);
            SetAxisSta(EnumStageAxis.PPtoolBankTheta, DataModel.Instance.PPtoolBankThetaSta);
            UpdatePosition(EnumStageAxis.DippingGlue, DataModel.Instance.DippingGlue);
            SetAxisSta(EnumStageAxis.DippingGlue, DataModel.Instance.DippingGlueSta);
            UpdatePosition(EnumStageAxis.TransportTrack1, DataModel.Instance.TransportTrack1);
            SetAxisSta(EnumStageAxis.TransportTrack1, DataModel.Instance.TransportTrack1Sta);
            UpdatePosition(EnumStageAxis.TransportTrack2, DataModel.Instance.TransportTrack2);
            SetAxisSta(EnumStageAxis.TransportTrack2, DataModel.Instance.TransportTrack2Sta);
            UpdatePosition(EnumStageAxis.TransportTrack3, DataModel.Instance.TransportTrack3);
            SetAxisSta(EnumStageAxis.TransportTrack3, DataModel.Instance.TransportTrack3Sta);
            UpdatePosition(EnumStageAxis.WaferTableY, DataModel.Instance.WaferTableY);
            SetAxisSta(EnumStageAxis.WaferTableY, DataModel.Instance.WaferTableYSta);
            UpdatePosition(EnumStageAxis.WaferTableX, DataModel.Instance.WaferTableX);
            SetAxisSta(EnumStageAxis.WaferTableX, DataModel.Instance.WaferTableXSta);
            UpdatePosition(EnumStageAxis.WaferTableZ, DataModel.Instance.WaferTableZ);
            SetAxisSta(EnumStageAxis.WaferTableZ, DataModel.Instance.WaferTableZSta);
            UpdatePosition(EnumStageAxis.WaferFilm, DataModel.Instance.WaferFilm);
            SetAxisSta(EnumStageAxis.WaferFilm, DataModel.Instance.WaferFilmSta);
            UpdatePosition(EnumStageAxis.WaferFinger, DataModel.Instance.WaferFinger);
            SetAxisSta(EnumStageAxis.WaferFinger, DataModel.Instance.WaferFingerSta);
            UpdatePosition(EnumStageAxis.WaferCassetteLift, DataModel.Instance.WaferCassetteLift);
            SetAxisSta(EnumStageAxis.WaferCassetteLift, DataModel.Instance.WaferCassetteLiftSta);
            UpdatePosition(EnumStageAxis.ESZ, DataModel.Instance.ESZ);
            SetAxisSta(EnumStageAxis.ESZ, DataModel.Instance.ESZSta);
            UpdatePosition(EnumStageAxis.NeedleZ, DataModel.Instance.NeedleZ);
            SetAxisSta(EnumStageAxis.NeedleZ, DataModel.Instance.NeedleZSta);
            UpdatePosition(EnumStageAxis.NeedleSwitch, DataModel.Instance.NeedleSwitch);
            SetAxisSta(EnumStageAxis.NeedleSwitch, DataModel.Instance.NeedleSwitchSta);

            UpdatePosition(EnumStageAxis.FilpToolTheta, DataModel.Instance.FilpToolTheta);
            SetAxisSta(EnumStageAxis.FilpToolTheta, DataModel.Instance.FilpToolThetaSta);

            UpdatePosition(EnumStageAxis.SubmountPPT, DataModel.Instance.SubmountPPT);
            SetAxisSta(EnumStageAxis.SubmountPPT, DataModel.Instance.SubmountPPTSta);

            UpdatePosition(EnumStageAxis.SubmountPPZ, DataModel.Instance.SubmountPPZ);
            SetAxisSta(EnumStageAxis.SubmountPPZ, DataModel.Instance.SubmountPPZSta);


            #endregion


        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (sender is Label clickedLabel)
            {
                string labelName = clickedLabel.Name;
                short bit = 1;
                switch (labelName)
                {
                    case "ChipPPVaccumSwitch":
                        if (DataModel.Instance.ChipPPVaccumSwitch)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPVaccumSwitch, 1);
                        }
                        break;
                    case "ChipPPBlowSwitch":
                        if (DataModel.Instance.ChipPPBlowSwitch)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.ChipPPBlowSwitch, 1);
                        }
                        break;
                    case "EpoxtliftCylinder":
                        if (DataModel.Instance.EpoxtliftCylinder)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 1);
                        }
                        break;
                    case "EpoxtDIS":
                        if (DataModel.Instance.EpoxtDIS)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 1);
                        }
                        break;
                    case "EpoxtENABLE":
                        if (DataModel.Instance.EpoxtENABLE)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtENABLE, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtENABLE, 1);
                        }
                        break;
                    case "EpoxtTMD":
                        if (DataModel.Instance.EpoxtTMD)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtTMD, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtTMD, 1);
                        }
                        break;
                    case "EpoxtTMS":
                        if (DataModel.Instance.EpoxtTMS)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtTMS, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtTMS, 1);
                        }
                        break;
                    case "EpoxtC_CNT":
                        if (DataModel.Instance.EpoxtC_CNT)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtC_CNT, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtC_CNT, 1);
                        }
                        break;
                    case "EpoxtRESET":
                        if (DataModel.Instance.EpoxtRESET)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtRESET, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtRESET, 1);
                        }
                        break;
                    case "TransportCylinder1":
                        if (DataModel.Instance.TransportCylinder1)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportCylinder1, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportCylinder1, 1);
                        }
                        break;
                    case "TransportCylinder2":
                        if (DataModel.Instance.TransportCylinder2)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportCylinder2, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportCylinder2, 1);
                        }
                        break;
                    case "TransportVaccumSwitch1":
                        if (DataModel.Instance.TransportVaccumSwitch1)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch1, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch1, 1);
                        }
                        break;
                    case "TransportVaccumSwitch2":
                        if (DataModel.Instance.TransportVaccumSwitch2)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch2, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TransportVaccumSwitch2, 1);
                        }
                        break;
                    case "EjectionSystemVaccumSwitch":
                        if (DataModel.Instance.EjectionSystemVaccumSwitch)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemVaccumSwitch, 1);
                        }
                        break;
                    case "EjectionSystemBlowSwitch":
                        if (DataModel.Instance.EjectionSystemBlowSwitch)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemBlowSwitch, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionSystemBlowSwitch, 1);
                        }
                        break;
                    case "WaferFingerCylinder":
                        if (DataModel.Instance.WaferFingerCylinder)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferFingerCylinder, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferFingerCylinder, 1);
                        }
                        break;
                    case "WaferClampCylinder":
                        if (DataModel.Instance.WaferClampCylinder)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferClampCylinder, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferClampCylinder, 1);
                        }
                        break;
                    case "WaferCassetteCylinder":
                        if (DataModel.Instance.WaferCassetteCylinder)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferCassetteCylinder, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferCassetteCylinder, 1);
                        }
                        break;
                    case "WaferTableVaccumSwitch":
                        if (DataModel.Instance.WaferTableVaccumSwitch)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.StatisticWaffleVaccumSwitch, 1);
                        }
                        break;
                    case "MaterialPlatformVaccumSwitch":
                        //if (DataModel.Instance.MaterialPlatformVaccumSwitch)
                        //{
                        //    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 0);
                        //}
                        //else
                        //{
                        //    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 1);
                        //}
                        break;
                    case "TowerYellowLight":
                        if (DataModel.Instance.TowerYellowLight)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerYellowLight, 1);
                        }
                        break;
                    case "TowerGreenLight":
                        if (DataModel.Instance.TowerGreenLight)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerGreenLight, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerGreenLight, 1);
                        }
                        break;
                    case "TowerRedLight":
                        if (DataModel.Instance.TowerRedLight)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerRedLight, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.TowerRedLight, 1);
                        }
                        break;
                    case "WaferCassetteLiftMotorBrake":
                        if (DataModel.Instance.WaferCassetteLiftMotorBrake)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferCassetteLiftMotorBrake, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.WaferCassetteLiftMotorBrake, 1);
                        }
                        break;
                    case "EjectionLiftMotorBrake":
                        if (DataModel.Instance.EjectionLiftMotorBrake)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionLiftMotorBrake, 0);
                        }
                        else
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EjectionLiftMotorBrake, 1);
                        }
                        break;

                    case "laBondXAlarm":
                        bit = 1;
                        if ((DataModel.Instance.BondXSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.BondX);
                        }
                        else
                        {
                        }
                        break;
                    case "laBondYAlarm":
                        bit = 1;
                        if ((DataModel.Instance.BondYSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.BondY);
                        }
                        else
                        {
                        }
                        break;
                    case "laBondZAlarm":
                        bit = 1;
                        if ((DataModel.Instance.BondZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.BondZ);
                        }
                        else
                        {
                        }
                        break;
                    case "laChipPPTAlarm":
                        bit = 1;
                        if ((DataModel.Instance.ChipPPTSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.ChipPPT);
                        }
                        else
                        {
                        }
                        break;
                    case "laPPtoolBankThetaAlarm":
                        bit = 1;
                        if ((DataModel.Instance.PPtoolBankThetaSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.PPtoolBankTheta);
                        }
                        else
                        {
                        }
                        break;
                    case "laDippingGlueAlarm":
                        bit = 1;
                        if ((DataModel.Instance.DippingGlueSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.DippingGlue);
                        }
                        else
                        {
                        }
                        break;
                    case "laTransportTrack1Alarm":
                        bit = 1;
                        if ((DataModel.Instance.TransportTrack1Sta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.TransportTrack1);
                        }
                        else
                        {
                        }
                        break;
                    case "laTransportTrack2Alarm":
                        bit = 1;
                        if ((DataModel.Instance.TransportTrack2Sta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.TransportTrack2);
                        }
                        else
                        {
                        }
                        break;
                    case "laTransportTrack3Alarm":
                        bit = 1;
                        if ((DataModel.Instance.TransportTrack3Sta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.TransportTrack3);
                        }
                        else
                        {
                        }
                        break;
                    case "laWaferTableXAlarm":
                        bit = 1;
                        if ((DataModel.Instance.WaferTableXSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.WaferTableX);
                        }
                        else
                        {
                        }
                        break;
                    case "laWaferTableYAlarm":
                        bit = 1;
                        if ((DataModel.Instance.WaferTableYSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.WaferTableY);
                        }
                        else
                        {
                        }
                        break;
                    case "laWaferTableZAlarm":
                        bit = 1;
                        if ((DataModel.Instance.WaferTableZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.WaferTableZ);
                        }
                        else
                        {
                        }
                        break;
                    case "laWaferFilmAlarm":
                        bit = 1;
                        if ((DataModel.Instance.WaferFilmSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.WaferFilm);
                        }
                        else
                        {
                        }
                        break;
                    case "laWaferFingerAlarm":
                        bit = 1;
                        if ((DataModel.Instance.WaferFingerSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.WaferFinger);
                        }
                        else
                        {
                        }
                        break;
                    case "laWaferCassetteLiftAlarm":
                        bit = 1;
                        if ((DataModel.Instance.WaferCassetteLiftSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.WaferCassetteLift);
                        }
                        else
                        {
                        }
                        break;
                    case "laESZAlarm":
                        bit = 1;
                        if ((DataModel.Instance.ESZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.ESZ);
                        }
                        else
                        {
                        }
                        break;
                    case "laNeedleZAlarm":
                        bit = 1;
                        if ((DataModel.Instance.NeedleZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.NeedleZ);
                        }
                        else
                        {
                        }
                        break;
                    case "laNeedleSwitchAlarm":
                        bit = 1;
                        if ((DataModel.Instance.NeedleSwitchSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.NeedleSwitch);
                        }
                        else
                        {
                        }
                        break;
                    case "laFilpToolThetaAlarm":
                        bit = 1;
                        if ((DataModel.Instance.FilpToolThetaSta & (1 << bit)) != 0)
                        {
                            _boardCardController.ClrAlarm(EnumStageAxis.FilpToolTheta);
                        }
                        else
                        {
                        }
                        break;


                    case "laBondXEnable":
                        bit = 1;
                        if ((DataModel.Instance.BondXSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.BondX);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.BondX);
                        }
                        break;
                    case "laBondYEnable":
                        bit = 1;
                        if ((DataModel.Instance.BondYSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.BondY);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.BondX);
                        }
                        break;
                    case "laBondZEnable":
                        bit = 1;
                        if ((DataModel.Instance.BondZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.BondZ);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.BondZ);
                        }
                        break;
                    case "laChipPPTEnable":
                        bit = 1;
                        if ((DataModel.Instance.ChipPPTSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.ChipPPT);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.ChipPPT);
                        }
                        break;
                    case "laPPtoolBankThetaEnable":
                        bit = 1;
                        if ((DataModel.Instance.PPtoolBankThetaSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.PPtoolBankTheta);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.PPtoolBankTheta);
                        }
                        break;
                    case "laDippingGlueEnable":
                        bit = 1;
                        if ((DataModel.Instance.DippingGlueSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.DippingGlue);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.DippingGlue);
                        }
                        break;
                    case "laTransportTrack1Enable":
                        bit = 1;
                        if ((DataModel.Instance.TransportTrack1Sta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.TransportTrack1);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.TransportTrack1);
                        }
                        break;
                    case "laTransportTrack2Enable":
                        bit = 1;
                        if ((DataModel.Instance.TransportTrack2Sta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.TransportTrack2);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.TransportTrack2);
                        }
                        break;
                    case "laTransportTrack3Enable":
                        bit = 1;
                        if ((DataModel.Instance.TransportTrack3Sta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.TransportTrack3);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.TransportTrack3);
                        }
                        break;
                    case "laWaferTableXEnable":
                        bit = 1;
                        if ((DataModel.Instance.WaferTableXSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.WaferTableX);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.WaferTableX);
                        }
                        break;
                    case "laWaferTableYEnable":
                        bit = 1;
                        if ((DataModel.Instance.WaferTableYSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.WaferTableY);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.WaferTableY);
                        }
                        break;
                    case "laWaferTableZEnable":
                        bit = 1;
                        if ((DataModel.Instance.WaferTableZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.WaferTableZ);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.WaferTableZ);
                        }
                        break;
                    case "laWaferFilmEnable":
                        bit = 1;
                        if ((DataModel.Instance.WaferFilmSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.WaferFilm);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.WaferFilm);
                        }
                        break;
                    case "laWaferFingerEnable":
                        bit = 1;
                        if ((DataModel.Instance.WaferFingerSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.WaferFinger);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.WaferFinger);
                        }
                        break;
                    case "laWaferCassetteLiftEnable":
                        bit = 1;
                        if ((DataModel.Instance.WaferCassetteLiftSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.WaferCassetteLift);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.WaferCassetteLift);
                        }
                        break;
                    case "laESZEnable":
                        bit = 1;
                        if ((DataModel.Instance.ESZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.ESZ);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.ESZ);
                        }
                        break;
                    case "laNeedleZEnable":
                        bit = 1;
                        if ((DataModel.Instance.NeedleZSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.NeedleZ);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.NeedleZ);
                        }
                        break;
                    case "laNeedleSwitchEnable":
                        bit = 1;
                        if ((DataModel.Instance.NeedleSwitchSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.NeedleSwitch);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.NeedleSwitch);
                        }
                        break;
                    case "laFilpToolThetaEnable":
                        bit = 1;
                        if ((DataModel.Instance.FilpToolThetaSta & (1 << bit)) != 0)
                        {
                            _boardCardController.Disable(EnumStageAxis.FilpToolTheta);
                        }
                        else
                        {
                            _boardCardController.Enable(EnumStageAxis.FilpToolTheta);
                        }
                        break;
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
                    case "BondRed":
                        if (BondRed.Value > -1 && BondRed.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField).IsConnected)
                            {
                                int brightness = (int)BondRed.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectRedField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "BondGreen":
                        if (BondGreen.Value > -1 && BondGreen.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField).IsConnected)
                            {
                                int brightness = (int)BondGreen.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectGreenField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "BondBlue":
                        if (BondBlue.Value > -1 && BondBlue.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField).IsConnected)
                            {
                                int brightness = (int)BondBlue.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.BondDirectBlueField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "BondRing":
                        if (BondRing.Value > -1 && BondRing.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.BondRingField).IsConnected)
                            {
                                int brightness = (int)BondRing.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.BondRingField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.BondRingField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "WaferRed":
                        if (WaferRed.Value > -1 && WaferRed.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField).IsConnected)
                            {
                                int brightness = (int)WaferRed.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectRedField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "WaferGreen":
                        if (WaferGreen.Value > -1 && WaferGreen.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField).IsConnected)
                            {
                                int brightness = (int)WaferGreen.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectGreenField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "WaferBlue":
                        if (WaferBlue.Value > -1 && WaferBlue.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField).IsConnected)
                            {
                                int brightness = (int)WaferBlue.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.WaferDirectBlueField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "WaferRing":
                        if (WaferRing.Value > -1 && WaferRing.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.WaferRingField).IsConnected)
                            {
                                int brightness = (int)WaferRing.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.WaferRingField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.WaferRingField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "UpLookingRed":
                        if (UpLookingRed.Value > -1 && UpLookingRed.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).IsConnected)
                            {
                                int brightness = (int)UpLookingRed.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "UpLookingGreen":
                        if (UpLookingGreen.Value > -1 && UpLookingGreen.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).IsConnected)
                            {
                                int brightness = (int)UpLookingGreen.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "UpLookingBlue":
                        if (UpLookingBlue.Value > -1 && UpLookingBlue.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).IsConnected)
                            {
                                int brightness = (int)UpLookingBlue.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.LookupDirectField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                    case "UpLookingRing":
                        if (UpLookingRing.Value > -1 && UpLookingRing.Value < 256)
                        {
                            if (_LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField).IsConnected)
                            {
                                int brightness = (int)UpLookingRing.Value;
                                _LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField).SetIntensity(brightness, _LightControllerManager.GetLightController(EnumLightSourceType.LookupRingField).Channel);
                                Thread.Sleep(50);
                            }
                        }
                        break;
                }
            }

        }

        private void RegisterIOChangedAct()
        {
            IOManager.Instance.RegisterIOChannelChangedEvent("EutecticError", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("EutecticComplete", IOChanged);

            IOManager.Instance.RegisterIOChannelChangedEvent("ChipPPVaccumSwitch", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("ChipPPBlowSwitch", IOChanged);

            IOManager.Instance.RegisterIOChannelChangedEvent("SubmountPPVaccumSwitch", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("SubmountPPBlowSwitch", IOChanged);

            IOManager.Instance.RegisterIOChannelChangedEvent("WaferTableVaccumSwitch", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("MaterialPlatformVaccumSwitch", IOChanged); 
            IOManager.Instance.RegisterIOChannelChangedEvent("EjectionSystemVaccumSwitch", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("EutecticPlatformVaccumSwitch", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("NitrogenValve", IOChanged);


            IOManager.Instance.RegisterIOChannelChangedEvent("TowerRedLight", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("TowerYellowLight", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("TowerGreenLight", IOChanged);

            IOManager.Instance.RegisterIOChannelChangedEvent("ChipPPVaccumNormally", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("SubmountPPVaccumNormally", IOChanged);

            IOManager.Instance.RegisterIOChannelChangedEvent("SafeDoorSensor1", IOChanged);
            IOManager.Instance.RegisterIOChannelChangedEvent("SafeDoorSensor2", IOChanged);

        }
        private void IOChanged(string ioName, object preValue, object newValue)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() => IOChanged(ioName, preValue, newValue)));
                return;
            }
            try
            {
                if (ioName == "ChipPPVaccumSwitch")
                {
                    ChipPPVaccumSwitch.ForeColor = (bool)newValue?Color.Green: Color.DarkGray;
                }
                else if (ioName == "ChipPPBlowSwitch")
                {
                    ChipPPBlowSwitch.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                if (ioName == "SubmountPPVaccumSwitch")
                {
                    EpoxtliftCylinder.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "SubmountPPBlowSwitch")
                {
                    EpoxtDIS.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                if (ioName == "WaferTableVaccumSwitch")
                {
                    WaferTableVaccumSwitch.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "MaterialPlatformVaccumSwitch")
                {
                    MaterialPlatformVaccumSwitch.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }

                if (ioName == "EjectionSystemVaccumSwitch")
                {
                    EjectionSystemVaccumSwitch.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "EutecticPlatformVaccumSwitch")
                {
                    TransportCylinder1.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "NitrogenValve")
                {
                    WaferFingerCylinder.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "TowerRedLight")
                {
                    TowerRedLight.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "TowerYellowLight")
                {
                    TowerYellowLight.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "TowerGreenLight")
                {
                    TowerGreenLight.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "EutecticError")
                {
                    TransportInPlaceSignal2.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "EutecticComplete")
                {
                    TransportInPlaceSignal3.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "ChipPPVaccumNormally")
                {
                    ChipPPVaccumStatus.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "SubmountPPVaccumNormally")
                {
                    TransportInPlaceSignal1.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "SafeDoorSensor1")
                {
                    SafeDoorSensor1.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
                else if (ioName == "SafeDoorSensor2")
                {
                    SafeDoorSensor2.ForeColor = (bool)newValue ? Color.Green : Color.DarkGray;
                }
            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
            }

        }

        private void IOCtrlPanel_Load(object sender, EventArgs e)
        {
            //RegisterIOChangedAct();

            InitializeInputIOStatus();
            InitializeOutputIOStatus();

            InitializeOutputSerialPortStatus();
            InitializeOutputMotorStatus();

            InitializeTool();

            _enablePollingIO2 = true;
            Task.Run(new Action(ReadSerialPortTask2));
        }


        private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(_syncContext == null)
            {
                return;
            }

            #region OutputIO

            if (e.PropertyName == nameof(DataModel.ChipPPVaccumSwitch))
            {
                _syncContext.Post(_ => ChipPPVaccumSwitch.ForeColor = (DataModel.Instance.ChipPPVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.ChipPPBlowSwitch))
            {
                _syncContext.Post(_ => ChipPPBlowSwitch.ForeColor = (DataModel.Instance.ChipPPBlowSwitch ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtliftCylinder))
            {
                _syncContext.Post(_ => EpoxtliftCylinder.ForeColor = (DataModel.Instance.EpoxtliftCylinder ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtDIS))
            {
                _syncContext.Post(_ => EpoxtDIS.ForeColor = (DataModel.Instance.EpoxtDIS ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtENABLE))
            {
                _syncContext.Post(_ => EpoxtENABLE.ForeColor = (DataModel.Instance.EpoxtENABLE ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtTMD))
            {
                _syncContext.Post(_ => EpoxtTMD.ForeColor = (DataModel.Instance.EpoxtTMD ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtTMS))
            {
                _syncContext.Post(_ => EpoxtTMS.ForeColor = (DataModel.Instance.EpoxtTMS ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtC_CNT))
            {
                _syncContext.Post(_ => EpoxtC_CNT.ForeColor = (DataModel.Instance.EpoxtC_CNT ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtRESET))
            {
                _syncContext.Post(_ => EpoxtRESET.ForeColor = (DataModel.Instance.EpoxtRESET ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportCylinder1))
            {
                _syncContext.Post(_ => TransportCylinder1.ForeColor = (DataModel.Instance.TransportCylinder1 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportCylinder2))
            {
                _syncContext.Post(_ => TransportCylinder2.ForeColor = (DataModel.Instance.TransportCylinder2 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportVaccumSwitch1))
            {
                _syncContext.Post(_ => TransportVaccumSwitch1.ForeColor = (DataModel.Instance.TransportVaccumSwitch1 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportVaccumSwitch2))
            {
                _syncContext.Post(_ => TransportVaccumSwitch2.ForeColor = (DataModel.Instance.TransportVaccumSwitch2 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EjectionSystemVaccumSwitch))
            {
                _syncContext.Post(_ => EjectionSystemVaccumSwitch.ForeColor = (DataModel.Instance.EjectionSystemVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EjectionSystemBlowSwitch))
            {
                _syncContext.Post(_ => EjectionSystemBlowSwitch.ForeColor = (DataModel.Instance.EjectionSystemBlowSwitch ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFingerCylinder))
            {
                _syncContext.Post(_ => WaferFingerCylinder.ForeColor = (DataModel.Instance.WaferFingerCylinder ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferClampCylinder))
            {
                _syncContext.Post(_ => WaferClampCylinder.ForeColor = (DataModel.Instance.WaferClampCylinder ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferCassetteCylinder))
            {
                _syncContext.Post(_ => WaferCassetteCylinder.ForeColor = (DataModel.Instance.WaferCassetteCylinder ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.MaterialPlatformVaccumSwitch))
            {
                _syncContext.Post(_ => MaterialPlatformVaccumSwitch.ForeColor = (DataModel.Instance.MaterialPlatformVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableVaccumSwitch))
            {
                _syncContext.Post(_ => WaferTableVaccumSwitch.ForeColor = (DataModel.Instance.WaferTableVaccumSwitch ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TowerYellowLight))
            {
                _syncContext.Post(_ => TowerYellowLight.ForeColor = (DataModel.Instance.TowerYellowLight ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TowerGreenLight))
            {
                _syncContext.Post(_ => TowerGreenLight.ForeColor = (DataModel.Instance.TowerGreenLight ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TowerRedLight))
            {
                _syncContext.Post(_ => TowerRedLight.ForeColor = (DataModel.Instance.TowerRedLight ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferCassetteLiftMotorBrake))
            {
                _syncContext.Post(_ => WaferCassetteLiftMotorBrake.ForeColor = (DataModel.Instance.WaferCassetteLiftMotorBrake ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EjectionLiftMotorBrake))
            {
                _syncContext.Post(_ => EjectionLiftMotorBrake.ForeColor = (DataModel.Instance.EjectionLiftMotorBrake ? true : false) ? Color.Green : Color.DarkGray, null);
            }

            #endregion

            #region InputIO

            if (e.PropertyName == nameof(DataModel.ChipPPVaccumNormally))
            {
                _syncContext.Post(_ => ChipPPVaccumStatus.ForeColor = (DataModel.Instance.ChipPPVaccumNormally ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportInPlaceSignal1))
            {
                _syncContext.Post(_ => TransportInPlaceSignal1.ForeColor = (DataModel.Instance.TransportInPlaceSignal1 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportInPlaceSignal2))
            {
                _syncContext.Post(_ => TransportInPlaceSignal2.ForeColor = (DataModel.Instance.TransportInPlaceSignal2 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.TransportInPlaceSignal3))
            {
                _syncContext.Post(_ => TransportInPlaceSignal3.ForeColor = (DataModel.Instance.TransportInPlaceSignal3 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferInPlaceSignal1))
            {
                _syncContext.Post(_ => WaferInPlaceSignal1.ForeColor = (DataModel.Instance.WaferInPlaceSignal1 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.SafeDoorSensor1))
            {
                _syncContext.Post(_ => SafeDoorSensor1.ForeColor = (DataModel.Instance.SafeDoorSensor1 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.SafeDoorSensor2))
            {
                _syncContext.Post(_ => SafeDoorSensor2.ForeColor = (DataModel.Instance.SafeDoorSensor2 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtPON))
            {
                _syncContext.Post(_ => EpoxtPON.ForeColor = (DataModel.Instance.EpoxtPON ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtDSO))
            {
                _syncContext.Post(_ => EpoxtDSO.ForeColor = (DataModel.Instance.EpoxtDSO ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtEND))
            {
                _syncContext.Post(_ => EpoxtEND.ForeColor = (DataModel.Instance.EpoxtEND ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtERROR))
            {
                _syncContext.Post(_ => EpoxtERROR.ForeColor = (DataModel.Instance.EpoxtERROR ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtALARM))
            {
                _syncContext.Post(_ => EpoxtALARM.ForeColor = (DataModel.Instance.EpoxtALARM ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtALARM2))
            {
                _syncContext.Post(_ => EpoxtALARM2.ForeColor = (DataModel.Instance.EpoxtALARM2 ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtRSM))
            {
                _syncContext.Post(_ => EpoxtRSM.ForeColor = (DataModel.Instance.EpoxtRSM ? true : false) ? Color.Green : Color.DarkGray, null);
            }
            if (e.PropertyName == nameof(DataModel.EpoxtREADY))
            {
                _syncContext.Post(_ => EpoxtREADY.ForeColor = (DataModel.Instance.EpoxtREADY ? true : false) ? Color.Green : Color.DarkGray, null);
            }


            #endregion

            #region SerialPort

            if (e.PropertyName == nameof(DataModel.BondRed))
            {
                _syncContext.Post(_ => BondRed.Value = DataModel.Instance.BondRed, null);
            }
            if (e.PropertyName == nameof(DataModel.BondGreen))
            {
                _syncContext.Post(_ => BondGreen.Value = DataModel.Instance.BondGreen, null);
            }
            if (e.PropertyName == nameof(DataModel.BondBlue))
            {
                _syncContext.Post(_ => BondBlue.Value = DataModel.Instance.BondBlue, null);
            }
            if (e.PropertyName == nameof(DataModel.BondRing))
            {
                _syncContext.Post(_ => BondRing.Value = DataModel.Instance.BondRing, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferRed))
            {
                _syncContext.Post(_ => WaferRed.Value = DataModel.Instance.WaferRed, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferGreen))
            {
                _syncContext.Post(_ => WaferGreen.Value = DataModel.Instance.WaferGreen, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferBlue))
            {
                _syncContext.Post(_ => WaferBlue.Value = DataModel.Instance.WaferBlue, null);
            }
            if (e.PropertyName == nameof(DataModel.WaferRing))
            {
                _syncContext.Post(_ => WaferRing.Value = DataModel.Instance.WaferRing, null);
            }
            if (e.PropertyName == nameof(DataModel.UpLookingRed))
            {
                _syncContext.Post(_ => UpLookingRed.Value = DataModel.Instance.UpLookingRed, null);
            }
            if (e.PropertyName == nameof(DataModel.UpLookingGreen))
            {
                _syncContext.Post(_ => UpLookingGreen.Value = DataModel.Instance.UpLookingGreen, null);
            }
            if (e.PropertyName == nameof(DataModel.UpLookingBlue))
            {
                _syncContext.Post(_ => UpLookingBlue.Value = DataModel.Instance.UpLookingBlue, null);
            }
            if (e.PropertyName == nameof(DataModel.UpLookingRing))
            {
                _syncContext.Post(_ => UpLookingRing.Value = DataModel.Instance.UpLookingRing, null);
            }
            if (e.PropertyName == nameof(DataModel.LaserValue))
            {
                _syncContext.Post(_ => LaserValue.Value = (decimal)DataModel.Instance.LaserValue, null);
            }
            if (e.PropertyName == nameof(DataModel.PressureValue1))
            {
                _syncContext.Post(_ => PressureValue1.Value = (decimal)DataModel.Instance.PressureValue1, null);
            }
            if (e.PropertyName == nameof(DataModel.PressureValue2))
            {
                _syncContext.Post(_ => PressureValue2.Value = (decimal)DataModel.Instance.PressureValue2, null);
            }


            #endregion



            #region Motor

            if (e.PropertyName == nameof(DataModel.BondX))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.BondX, DataModel.Instance.BondX), null);
            }
            if (e.PropertyName == nameof(DataModel.BondXSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.BondX, DataModel.Instance.BondXSta), null);
            }
            if (e.PropertyName == nameof(DataModel.BondY))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.BondY, DataModel.Instance.BondY), null);
            }
            if (e.PropertyName == nameof(DataModel.BondYSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.BondY, DataModel.Instance.BondYSta), null);
            }
            if (e.PropertyName == nameof(DataModel.BondZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.BondZ, DataModel.Instance.BondZ), null);
            }
            if (e.PropertyName == nameof(DataModel.BondZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.BondZ, DataModel.Instance.BondZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.ChipPPT))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.ChipPPT, DataModel.Instance.ChipPPT), null);
            }
            if (e.PropertyName == nameof(DataModel.ChipPPTSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.ChipPPT, DataModel.Instance.ChipPPTSta), null);
            }
            if (e.PropertyName == nameof(DataModel.PPtoolBankTheta))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.PPtoolBankTheta, DataModel.Instance.PPtoolBankTheta), null);
            }
            if (e.PropertyName == nameof(DataModel.PPtoolBankThetaSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.PPtoolBankTheta, DataModel.Instance.PPtoolBankThetaSta), null);
            }
            if (e.PropertyName == nameof(DataModel.DippingGlue))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.DippingGlue, DataModel.Instance.DippingGlue), null);
            }
            if (e.PropertyName == nameof(DataModel.DippingGlueSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.DippingGlue, DataModel.Instance.DippingGlueSta), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack1))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.TransportTrack1, DataModel.Instance.TransportTrack1), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack1Sta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.TransportTrack1, DataModel.Instance.TransportTrack1Sta), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack2))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.TransportTrack2, DataModel.Instance.TransportTrack2), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack2Sta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.TransportTrack2, DataModel.Instance.TransportTrack2Sta), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack3))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.TransportTrack3, DataModel.Instance.TransportTrack3), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack3Sta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.TransportTrack3, DataModel.Instance.TransportTrack3Sta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableY))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferTableY, DataModel.Instance.WaferTableY), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableYSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferTableY, DataModel.Instance.WaferTableYSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableX))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferTableX, DataModel.Instance.WaferTableX), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableXSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferTableX, DataModel.Instance.WaferTableXSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferTableZ, DataModel.Instance.WaferTableZ), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferTableZ, DataModel.Instance.WaferTableZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFilm))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferFilm, DataModel.Instance.WaferFilm), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFilmSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferFilm, DataModel.Instance.WaferFilmSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFinger))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferFinger, DataModel.Instance.WaferFinger), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFingerSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferFinger, DataModel.Instance.WaferFingerSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferCassetteLift))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferCassetteLift, DataModel.Instance.WaferCassetteLift), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferCassetteLiftSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferCassetteLift, DataModel.Instance.WaferCassetteLiftSta), null);
            }
            if (e.PropertyName == nameof(DataModel.ESZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.ESZ, DataModel.Instance.ESZ), null);
            }
            if (e.PropertyName == nameof(DataModel.ESZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.ESZ, DataModel.Instance.ESZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.NeedleZ, DataModel.Instance.NeedleZ), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.NeedleZ, DataModel.Instance.NeedleZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleSwitch))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.NeedleSwitch, DataModel.Instance.NeedleSwitch), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleSwitchSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.NeedleSwitch, DataModel.Instance.NeedleSwitchSta), null);
            }
            if (e.PropertyName == nameof(DataModel.FilpToolTheta))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.FilpToolTheta, DataModel.Instance.FilpToolTheta), null);
            }
            if (e.PropertyName == nameof(DataModel.FilpToolThetaSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.FilpToolTheta, DataModel.Instance.FilpToolThetaSta), null);
            }
            #region Motor

            if (e.PropertyName == nameof(DataModel.BondX))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.BondX, DataModel.Instance.BondX), null);
            }
            if (e.PropertyName == nameof(DataModel.BondXSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.BondX, DataModel.Instance.BondXSta), null);
            }
            if (e.PropertyName == nameof(DataModel.BondY))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.BondY, DataModel.Instance.BondY), null);
            }
            if (e.PropertyName == nameof(DataModel.BondYSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.BondY, DataModel.Instance.BondYSta), null);
            }
            if (e.PropertyName == nameof(DataModel.BondZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.BondZ, DataModel.Instance.BondZ), null);
            }
            if (e.PropertyName == nameof(DataModel.BondZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.BondZ, DataModel.Instance.BondZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.ChipPPT))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.ChipPPT, DataModel.Instance.ChipPPT), null);
            }
            if (e.PropertyName == nameof(DataModel.ChipPPTSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.ChipPPT, DataModel.Instance.ChipPPTSta), null);
            }
            if (e.PropertyName == nameof(DataModel.PPtoolBankTheta))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.PPtoolBankTheta, DataModel.Instance.PPtoolBankTheta), null);
            }
            if (e.PropertyName == nameof(DataModel.PPtoolBankThetaSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.PPtoolBankTheta, DataModel.Instance.PPtoolBankThetaSta), null);
            }
            if (e.PropertyName == nameof(DataModel.DippingGlue))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.DippingGlue, DataModel.Instance.DippingGlue), null);
            }
            if (e.PropertyName == nameof(DataModel.DippingGlueSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.DippingGlue, DataModel.Instance.DippingGlueSta), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack1))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.TransportTrack1, DataModel.Instance.TransportTrack1), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack1Sta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.TransportTrack1, DataModel.Instance.TransportTrack1Sta), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack2))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.TransportTrack2, DataModel.Instance.TransportTrack2), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack2Sta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.TransportTrack2, DataModel.Instance.TransportTrack2Sta), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack3))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.TransportTrack3, DataModel.Instance.TransportTrack3), null);
            }
            if (e.PropertyName == nameof(DataModel.TransportTrack3Sta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.TransportTrack3, DataModel.Instance.TransportTrack3Sta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableY))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferTableY, DataModel.Instance.WaferTableY), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableYSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferTableY, DataModel.Instance.WaferTableYSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableX))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferTableX, DataModel.Instance.WaferTableX), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableXSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferTableX, DataModel.Instance.WaferTableXSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferTableZ, DataModel.Instance.WaferTableZ), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferTableZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferTableZ, DataModel.Instance.WaferTableZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFilm))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferFilm, DataModel.Instance.WaferFilm), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFilmSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferFilm, DataModel.Instance.WaferFilmSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFinger))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferFinger, DataModel.Instance.WaferFinger), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferFingerSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferFinger, DataModel.Instance.WaferFingerSta), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferCassetteLift))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.WaferCassetteLift, DataModel.Instance.WaferCassetteLift), null);
            }
            if (e.PropertyName == nameof(DataModel.WaferCassetteLiftSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.WaferCassetteLift, DataModel.Instance.WaferCassetteLiftSta), null);
            }
            if (e.PropertyName == nameof(DataModel.ESZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.ESZ, DataModel.Instance.ESZ), null);
            }
            if (e.PropertyName == nameof(DataModel.ESZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.ESZ, DataModel.Instance.ESZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.NeedleZ, DataModel.Instance.NeedleZ), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.NeedleZ, DataModel.Instance.NeedleZSta), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleSwitch))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.NeedleSwitch, DataModel.Instance.NeedleSwitch), null);
            }
            if (e.PropertyName == nameof(DataModel.NeedleSwitchSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.NeedleSwitch, DataModel.Instance.NeedleSwitchSta), null);
            }
            if (e.PropertyName == nameof(DataModel.FilpToolTheta))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.FilpToolTheta, DataModel.Instance.FilpToolTheta), null);
            }
            if (e.PropertyName == nameof(DataModel.FilpToolThetaSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.FilpToolTheta, DataModel.Instance.FilpToolThetaSta), null);
            }
            if (e.PropertyName == nameof(DataModel.SubmountPPT))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.SubmountPPT, DataModel.Instance.SubmountPPT), null);
            }
            if (e.PropertyName == nameof(DataModel.SubmountPPTSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.SubmountPPT, DataModel.Instance.SubmountPPTSta), null);
            }
            if (e.PropertyName == nameof(DataModel.SubmountPPZ))
            {
                _syncContext.Post(_ => UpdatePosition(EnumStageAxis.SubmountPPZ, DataModel.Instance.SubmountPPZ), null);
            }
            if (e.PropertyName == nameof(DataModel.SubmountPPZSta))
            {
                _syncContext.Post(_ => SetAxisSta(EnumStageAxis.SubmountPPZ, DataModel.Instance.SubmountPPZSta), null);
            }


            #endregion



            #endregion



        }




        #region 电机

        private void UpdateLabel(int row, int column, string newText, Color newColor, bool enable = true)
        {
            // 获取指定行列的控件  
            Control control = tableLayoutPanel4.GetControlFromPosition(column, row);
            

            // 检查该控件是否为 Label  
            if (control is Label label)
            {
                // 修改 Label 的 Text 和 ForeColor  
                if(newText != string.Empty)
                {
                    label.Text = newText;
                }
                
                label.BackColor = newColor;
            }
            if(enable == false)
            {
                for (int i = 0; i < 10; i++)
                {
                    Control control0 = tableLayoutPanel4.GetControlFromPosition(i, row);
                    if (control0 != null)
                    {
                        control0.Visible = enable;
                        control0.Enabled = enable;
                    }
                }
            }
            
            
        }

        private void Updatename(EnumStageAxis axis, string name, bool enable = true)
        {
            UpdateLabel((int)axis, 0, name, Color.Transparent, enable);
        }

        private void UpdatePosition(EnumStageAxis axis, double position)
        {
            UpdateLabel((int)axis, 1, position.ToString("F3"), Color.Transparent);
        }

        private void UpdateSta(EnumStageAxis axis, EnumAxisStatus status, bool done)
        {
            UpdateLabel((int)axis, (int)status, string.Empty, (done == true)? Color.Green : Color.DarkGray);
        }

        private void SetAxisSta(EnumStageAxis axis,int sta)
        {
            short bit = 1;
            UpdateSta(axis, EnumAxisStatus.Alarm, (sta & (1 << bit)) != 0);
            bit = 9;
            UpdateSta(axis, EnumAxisStatus.Enable, (sta & (1 << bit)) != 0);
            bit = 5;
            UpdateSta(axis, EnumAxisStatus.PositiveLimit, (sta & (1 << bit)) != 0);
            bit = 6;
            UpdateSta(axis, EnumAxisStatus.NegativeLimit, (sta & (1 << bit)) != 0);
            bit = 10;
            UpdateSta(axis, EnumAxisStatus.PlanMotion, (sta & (1 << bit)) != 0);
            bit = 7;
            UpdateSta(axis, EnumAxisStatus.SmoothStop, (sta & (1 << bit)) != 0);
            bit = 11;
            UpdateSta(axis, EnumAxisStatus.NotorInPlace, (sta & (1 << bit)) != 0);
            bit = 8;
            UpdateSta(axis, EnumAxisStatus.EmergencyStop, (sta & (1 << bit)) != 0);

        }




        #endregion

        private void MaterialPlatformVaccumSwitch_Click(object sender, EventArgs e)
        {

        }

        private void LabelWaffleTableVaccumSwitch_Click(object sender, EventArgs e)
        {

        }

        private void TransportCylinder1_Click(object sender, EventArgs e)
        {

        }


        private void ReadSerialPortTask2()
        {
            while (_enablePollingIO2)
            {
                Thread.Sleep(100);
                try
                {

                    ParseDataAndUpdateSerialPortLaser();
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortLaser,Error.", ex);
                }
                try
                {
                    ParseDataAndUpdateSerialPortDynameter();

                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, $"ParseDataAndUpdateSerialPortDynameter,Error.", ex);
                }
            }
        }

        internal void ParseDataAndUpdateSerialPortLaser()
        {

            if (_LaserSensorManager.GetCurrentHardware() != null && _LaserSensorManager.GetCurrentHardware().IsConnect)
            {
                double distance = -1;
                distance = (double)_LaserSensorManager.GetCurrentHardware().ReadDistance();
                if (distance >= 0)
                {
                    DataModel.Instance.LaserValue = distance / 10000.0f;
                }
                else
                {
                    DataModel.Instance.LaserValue = 0;
                }

                DataModel.Instance.LaserIsconnect = true;
                Thread.Sleep(50);
            }
            else
            {
                DataModel.Instance.LaserIsconnect = false;
            }

        }
        internal void ParseDataAndUpdateSerialPortDynameter()
        {

            if (_DynamometerManager.GetCurrentHardware() != null && _DynamometerManager.GetCurrentHardware().IsConnect)
            {
                double[] pressure;
                pressure = _DynamometerManager.GetCurrentHardware().ReadAllValue();
                if (pressure?.Length > 1)
                {
                    DataModel.Instance.PressureValue1 = pressure[0];
                    DataModel.Instance.PressureValue2 = pressure[1];
                }

                DataModel.Instance.PressureIsconnect = true;
                Thread.Sleep(50);
            }
            else
            {
                DataModel.Instance.PressureIsconnect = false;
            }

        }


    }
}
