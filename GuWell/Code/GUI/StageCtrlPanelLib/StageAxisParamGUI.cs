using BoardCardControllerClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
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

namespace StageCtrlPanelLib
{
    public partial class StageAxisParamGUI : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 当前控制的Stage的坐标轴
        /// </summary>
        private EnumStageAxis _currentStageAxis = EnumStageAxis.BondX;

        /// <summary>
        /// 当前控制的Stage的坐标轴
        /// </summary>
        public EnumStageAxis CurrentStageAxis
        {
            get
            {
                return _currentStageAxis;
            }
            set
            {
                //UpdateUnitToUI(false);
                _currentStageAxis = value;
            }
        }


        private SynchronizationContext _syncContext;
        private IBoardCardController _boardCardController;
        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }


        public StageAxisParamGUI()
        {
            InitializeComponent();

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

            _boardCardController = BoardCardManager.Instance.GetCurrentController();
        }

        private void StageAxisParamGUI_Load(object sender, EventArgs e)
        {
            foreach (var item in Enum.GetValues(typeof(EnumStageAxis)))
            {
                this.comboBoxSelAxis.Items.Add(item);
            }

            foreach (var item2 in Enum.GetValues(typeof(EnumStageType)))
            {
                this.comboBoxStageType.Items.Add(item2);
            }

            InitializeTool();

            InitializeOutputMotorStatus();
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

            }

        }




        private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_syncContext == null)
            {
                return;
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


        private void UpdateLabel(int row, int column, string newText, Color newColor, bool enable = true)
        {
            // 获取指定行列的控件  
            Control control = tableLayoutPanel4.GetControlFromPosition(column, row);


            // 检查该控件是否为 Label  
            if (control is Label label)
            {
                // 修改 Label 的 Text 和 ForeColor  
                if (newText != string.Empty)
                {
                    label.Text = newText;
                }

                label.ForeColor = newColor;
            }
            if (enable == false)
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

            if(_currentStageAxis == axis)
            {
                SetnumControl(numPosition, (decimal)position);

            }
        }

        private void SetnumControl(NumericUpDown numeric, decimal value)
        {
            if (value >= numeric.Minimum && value <= numeric.Maximum)
            {
                numeric.Value = value;
            }
        }

        private void UpdateSta(EnumStageAxis axis, EnumAxisStatus status, bool done)
        {
            UpdateLabel((int)axis, (int)status, string.Empty, (done == true) ? Color.Green : Color.DarkGray);



        }

        private void SetAxisSta(EnumStageAxis axis, int sta)
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

            if (_currentStageAxis == axis)
            {
                laError.ForeColor = ((sta & (1 << 1)) != 0) ? Color.Green : Color.DarkGray;
                laEnable.ForeColor = ((sta & (1 << 9)) != 0) ? Color.Green : Color.DarkGray;
                laPositivelimit.ForeColor = ((sta & (1 << 5)) != 0) ? Color.Green : Color.DarkGray;
                laNegativelimit.ForeColor = ((sta & (1 << 6)) != 0) ? Color.Green : Color.DarkGray;
                laPlanningMovement.ForeColor = ((sta & (1 << 10)) != 0) ? Color.Green : Color.DarkGray;
                laSmoothStop.ForeColor = ((sta & (1 << 7)) != 0) ? Color.Green : Color.DarkGray;
                laMotorInPlace.ForeColor = ((sta & (1 << 11)) != 0) ? Color.Green : Color.DarkGray;
                laStop.ForeColor = ((sta & (1 << 8)) != 0) ? Color.Green : Color.DarkGray;

            }

        }







        #endregion

        private void comboBoxSelAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            var curAxis = (EnumStageAxis)Enum.Parse(typeof(EnumStageAxis), this.comboBoxSelAxis.SelectedItem.ToString());
            _currentStageAxis = curAxis;
            ReadAxis();
        }

        private void ReadAxis()
        {
            float pos = 0;
            int sta = 0;
            if (_currentStageAxis == EnumStageAxis.BondX)
            {
                pos = DataModel.Instance.BondX;
                sta = DataModel.Instance.BondXSta;
            }
            else if (_currentStageAxis == EnumStageAxis.BondY)
            {
                pos = DataModel.Instance.BondY;
                sta = DataModel.Instance.BondYSta;
            }
            else if (_currentStageAxis == EnumStageAxis.BondZ)
            {
                pos = DataModel.Instance.BondZ;
                sta = DataModel.Instance.BondZSta;
            }
            else if (_currentStageAxis == EnumStageAxis.ChipPPT)
            {
                pos = DataModel.Instance.ChipPPT;
                sta = DataModel.Instance.ChipPPTSta;
            }
            else if (_currentStageAxis == EnumStageAxis.TransportTrack1)
            {
                pos = DataModel.Instance.TransportTrack1;
                sta = DataModel.Instance.TransportTrack1Sta;
            }
            else if (_currentStageAxis == EnumStageAxis.TransportTrack2)
            {
                pos = DataModel.Instance.TransportTrack2;
                sta = DataModel.Instance.TransportTrack2Sta;
            }
            else if (_currentStageAxis == EnumStageAxis.TransportTrack3)
            {
                pos = DataModel.Instance.TransportTrack3;
                sta = DataModel.Instance.TransportTrack3Sta;
            }
            else if (_currentStageAxis == EnumStageAxis.WaferTableX)
            {
                pos = DataModel.Instance.WaferTableX;
                sta = DataModel.Instance.WaferTableXSta;
            }
            else if (_currentStageAxis == EnumStageAxis.WaferTableY)
            {
                pos = DataModel.Instance.WaferTableY;
                sta = DataModel.Instance.WaferTableYSta;
            }
            else if (_currentStageAxis == EnumStageAxis.WaferTableZ)
            {
                pos = DataModel.Instance.WaferTableZ;
                sta = DataModel.Instance.WaferTableZSta;
            }
            else if (_currentStageAxis == EnumStageAxis.ESZ)
            {
                pos = DataModel.Instance.ESZ;
                sta = DataModel.Instance.ESZSta;
            }
            else if (_currentStageAxis == EnumStageAxis.NeedleZ)
            {
                pos = DataModel.Instance.NeedleZ;
                sta = DataModel.Instance.NeedleZSta;
            }

            SetnumControl(numPosition, (decimal)pos);

            laError.ForeColor = ((sta & (1 << 1)) != 0) ? Color.Green : Color.DarkGray;
            laEnable.ForeColor = ((sta & (1 << 9)) != 0) ? Color.Green : Color.DarkGray;
            laPositivelimit.ForeColor = ((sta & (1 << 5)) != 0) ? Color.Green : Color.DarkGray;
            laNegativelimit.ForeColor = ((sta & (1 << 6)) != 0) ? Color.Green : Color.DarkGray;
            laPlanningMovement.ForeColor = ((sta & (1 << 10)) != 0) ? Color.Green : Color.DarkGray;
            laSmoothStop.ForeColor = ((sta & (1 << 7)) != 0) ? Color.Green : Color.DarkGray;
            laMotorInPlace.ForeColor = ((sta & (1 << 11)) != 0) ? Color.Green : Color.DarkGray;
            laStop.ForeColor = ((sta & (1 << 8)) != 0) ? Color.Green : Color.DarkGray;


            AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(_currentStageAxis);
            comboBoxStageType.Text = _axisConfig.StageType.ToString();

            double speed = _boardCardController.GetAxisSpeed(_currentStageAxis);
            SetnumControl(numSpeed, (decimal)speed);
            double acceleration = _boardCardController.GetAcceleration(_currentStageAxis);
            SetnumControl(numAcceleration, (decimal)acceleration);
            double deceleration = _boardCardController.GetDeceleration(_currentStageAxis);
            SetnumControl(numDeceleration, (decimal)deceleration);

            double Smotheda = _axisConfig.Smotheda;
            SetnumControl(numSacceleration, (decimal)Smotheda);
            double Smothedj = _axisConfig.Smothedj;
            SetnumControl(numSJerk, (decimal)Smothedj);
            double MaxAxisSpeed = _axisConfig.MaxAxisSpeed;
            SetnumControl(numMaxSpeed, (decimal)MaxAxisSpeed);

            double LeftLimit = _boardCardController.GetSoftLeftLimit(_currentStageAxis);
            SetnumControl(numSoftLeftLimit, (decimal)LeftLimit);
            double RightLimit = _boardCardController.GetSoftRightLimit(_currentStageAxis);
            SetnumControl(numSoftRightLimit, (decimal)RightLimit);
            double Smooth = _axisConfig.Smooth;
            SetnumControl(numSmooth, (decimal)Smooth);
            double SmoothTime = _axisConfig.SmoothTime;
            SetnumControl(numSmoothTime, (decimal)SmoothTime);

            double CirclePulse = _axisConfig.CirclePulse;
            SetnumControl(numCirclePulse, (decimal)CirclePulse);
            double Lead = _axisConfig.Lead;
            SetnumControl(numLead, (decimal)Lead);


        }

        private void btnAbsoluteMove_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.MoveAbsoluteSync(_currentStageAxis, (double)numTarget.Value, 0);
        }

        private void btnRelativeMove_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.MoveRelativeSync(_currentStageAxis, (double)numDistance.Value, 0);
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.Enable(_currentStageAxis);
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.Disable(_currentStageAxis);
        }

        private void btnErrorClear_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.ClrAlarm(_currentStageAxis);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.StopMotion(_currentStageAxis);
        }

        private void btnLimitEffective_Click(object sender, EventArgs e)
        {
            AxisConfig _axisConfig = _hardwareConfig.StageConfig.GetAixsConfigByType(_currentStageAxis);
            if (_boardCardController != null)
                _boardCardController.SetSoftLeftAndRightLimit(_currentStageAxis,_axisConfig.SoftRightLimit, _axisConfig.SoftLeftLimit);
        }

        private void btnLimitFailure_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
                _boardCardController.CloseSoftLeftAndRightLimit(_currentStageAxis);
        }

        private void btnGoHome_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
            {
                var axis = _currentStageAxis;
                if (axis == EnumStageAxis.WaferTableZ || axis == EnumStageAxis.ESZ)
                {
                    _boardCardController.Home(axis, 18);
                }
                //else if (axis == EnumStageAxis.WaferTableX||axis==EnumStageAxis.NeedleZ)
                else if (axis == EnumStageAxis.NeedleZ)
                {
                    _boardCardController.Home(axis, 17);
                }
                else if (axis == EnumStageAxis.WaferTableY)
                {
                    _boardCardController.Home(axis, 18);
                }
                else if (axis == EnumStageAxis.WaferTableX)
                {
                    _boardCardController.Home(axis, 17);
                }
                else if (axis == EnumStageAxis.SubmountPPZ)
                {
                    _boardCardController.Home(axis, 101);
                }
                else if (axis == EnumStageAxis.SubmountPPT)
                {
                    _boardCardController.Home(axis, 33);
                }
            }
        }

        private void btnSetZero_Click(object sender, EventArgs e)
        {
            if (_boardCardController != null)
            {
                var axis = _currentStageAxis;
                _boardCardController.Home(axis, 35);
            }
        }
    }
}
