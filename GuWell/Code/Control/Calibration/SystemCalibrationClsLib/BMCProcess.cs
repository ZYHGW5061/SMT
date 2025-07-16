using BoardCardControllerClsLib;
using CameraControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using JobClsLib;
using PositioningSystemClsLib;
using SecondaryCalibrationClsLib;
using StageControllerClsLib;
using StageCtrlPanelLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionClsLib;
using VisionControlAppClsLib;
using VisionGUI;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace SystemCalibrationClsLib
{
    public class BMCProcess
    {
        #region private file

        private static readonly object _lockObj = new object();
        private static volatile BMCProcess _instance = null;
        public static BMCProcess Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new BMCProcess();
                        }
                    }
                }
                return _instance;
            }
        }

        public BMCProcess()
        {
            _boardCardController = BoardCardManager.Instance.GetCurrentController();
        }


        private StageCore stage = StageControllerClsLib.StageCore.Instance;
        private CameraConfig _BondcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.BondCamera); }
        }
        private CameraConfig _UplookingcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.UplookingCamera); }
        }
        private CameraConfig _WafercameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.WaferCamera); }
        }
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        private VisionControlAppClsLib.VisualControlManager _VisualManager
        {
            get { return VisionControlAppClsLib.VisualControlManager.Instance; }
        }
        public VisualControlApplications BondCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.BondCamera); }
        }
        public VisualControlApplications UplookingCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        public VisualControlApplications WaferCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.WaferCamera); }
        }

        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }

        private IBoardCardController _boardCardController;

        #endregion


        #region Public File

        /// <summary>
        /// 贴片机模式：0 共晶贴片机 1 点胶贴片机
        /// </summary>
        public int DeviceMode { get; set; } = 1;

        public double BondZOffset { get; set; } = 15;

        public double BMCToolthickness { get; set; } = 0.3;


        public float Score { get; set; }
        public int MinAngle { get; set; }
        public int MaxAngle { get; set; }

        public int EutecticWeldingVacuumTimeSleep { get; set; } = 500;
        public int PPtoolVacuumSleep { get; set; } = 500;

        /// <summary>
        /// XYZThera偏移量
        /// </summary>
        private XYZTOffsetConfig XYZToffset1 { get; set; }
        /// <summary>
        /// XYZThera偏移量
        /// </summary>
        private XYZTOffsetConfig XYZToffset2 { get; set; }


        /// <summary>
        /// XYZThera偏移量
        /// </summary>
        private XYZTOffsetConfig XYZToffset3 { get; set; }
        /// <summary>
        /// XYZThera偏移量
        /// </summary>
        private XYZTOffsetConfig XYZToffset4 { get; set; }

        /// <summary>
        /// 贴装完毕BMC小板和大板偏差
        /// </summary>
        private XYZTOffsetConfig XYZToffset5 { get; set; }

        /// <summary>
        /// 贴装完毕BMC小板和大板偏差
        /// </summary>
        private XYZTOffsetConfig XYZToffset6 { get; set; }

        private float Xoffset = 0;
        private float Yoffset = 0;
        private float Toffset = 0;


        private XYZTCoordinateConfig currentBondBMC;
        private XYZTCoordinateConfig currentBondCameraBMC;
        private XYZTCoordinateConfig currentBondBMCSubstrate;
        private XYZTCoordinateConfig currentBondCameraBMCSubstrate;


        private bool EnToIdentifyBMC = true;

        private bool EnToIdentifyUpBMC = true;


        #endregion


        #region private method

        private static (double, double) ImageToXY(PointF point, PointF center, float pixelsizex, float pixelsizey)
        {
            double X = (point.X - center.X) * pixelsizex;

            double Y = (point.Y - center.Y) * pixelsizey;

            return (X, Y);

        }

        private int ShowVisualForm(UserControl visualControlGUI, string Name, string title)
        {
            try
            {
                int REF = -1;
                using (VisualControlForm VForm = new VisualControlForm())
                {
                    VForm.InitializeGui(visualControlGUI);

                    //string hh = VForm
                    //Message(Name, title, true);
                    //if (hh == "next")
                    //{
                    //    REF = 1;
                    //}
                    //else
                    //{
                    //    REF = 0;
                    //}
                }
                return REF;
            }
            catch
            {
                return -1;
            }
        }

        private void ShowCameraForm(bool show)
        {
            if (show)
            {
                CameraWindowForm.Instance.Show();
            }
            else
            {
                CameraWindowForm.Instance.Hide();
            }
        }

        private void ShowStage()
        {

            FrmStageControl form = (Application.OpenForms["FrmStageControl"]) as FrmStageControl;
            if (form == null)
            {
                form = new FrmStageControl();
                form.Location = new Point(1550, 150);
                //form.Location = this.PointToScreen(new Point(1550, 150));
                //form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            else
            {
                form.Activate();
            }
        }

        public int GreationBMCFormShow(int BondIdentifyBMCSpotNum, int BondIdentifyBMCNum, int UplookingIdentifyBMCSpotNum, int UplookingIdentifyBMCNum, int BondIdentifyBMCSubstrateSpotNum, int BondIdentifyBMCSubstrateNum)
        {
            try
            {
                int REF = -1;
                using (CreationBMCForm myMessageBox1 = new CreationBMCForm())
                {
                    string hh = myMessageBox1.showMessage(BondIdentifyBMCSpotNum, BondIdentifyBMCNum, UplookingIdentifyBMCSpotNum, UplookingIdentifyBMCNum, BondIdentifyBMCSubstrateSpotNum, BondIdentifyBMCSubstrateNum);
                    if (hh == "confirm")
                    {
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum = myMessageBox1.BondIdentifyBMCSpotNum;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCNum = myMessageBox1.BondIdentifyBMCNum;
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum = myMessageBox1.UplookingIdentifyBMCSpotNum;
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCNum = myMessageBox1.UplookingIdentifyBMCNum;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum = myMessageBox1.BondIdentifyBMCSubstrateSpotNum;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateNum = myMessageBox1.BondIdentifyBMCSubstrateNum;
                        _systemConfig.SaveConfig();
                        REF = 1;
                    }
                    else
                    {
                        REF = 0;
                    }
                }
                return REF;
            }
            catch { return -1; }

        }

        private int ShowMessage(string title, string content, string type)
        {
            if (WarningBox.FormShow(title, content, type) == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }



        /// <summary>
        /// 榜头移动
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        private void BondXYZAbsoluteMove(double X, double Y, double Z)
        {
            //stage.AbloluteMoveSync(new EnumStageAxis[3] { EnumStageAxis.BondX, EnumStageAxis.BondY, EnumStageAxis.BondZ }, new double[3] { X, Y, Z });
            stage.ClrAlarm(EnumStageAxis.BondZ);
            stage.AbloluteMoveSync(EnumStageAxis.BondZ, Z);

            //stage.ClrAlarm(EnumStageAxis.BondX);
            //stage.AbloluteMoveSync(EnumStageAxis.BondX, X);

            //stage.ClrAlarm(EnumStageAxis.BondY);
            //stage.AbloluteMoveSync(EnumStageAxis.BondY, Y);


            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;

            double[] target1 = new double[2];

            target1[0] = X;
            target1[1] = Y;

            _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);
        }

        private void AxisAbsoluteMove(EnumStageAxis axis, double target)
        {
            stage.ClrAlarm(axis);
            stage.AbloluteMoveSync(axis, target);
        }

        private void WaferXYAbsoluteMove(double X, double Y)
        {
            stage.ClrAlarm(EnumStageAxis.WaferTableX);
            stage.ClrAlarm(EnumStageAxis.WaferTableY);

            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.WaferTableX;
            multiAxis[1] = EnumStageAxis.WaferTableY;

            double[] target1 = new double[2];

            target1[0] = X;
            target1[1] = Y;

            _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);
        }


        private void AxisRelativeMove(EnumStageAxis axis, double target)
        {
            stage.RelativeMoveSync(axis, (float)target);
        }

        private void AxisForward(EnumStageAxis axis)
        {
            stage.JogPositive(axis);
        }

        private void AxisStopForward(EnumStageAxis axis)
        {
            stage.StopJogPositive(axis);
        }

        private void AxisReverse(EnumStageAxis axis)
        {
            stage.JogNegative(axis);
        }

        private void AxisStopReverse(EnumStageAxis axis)
        {
            stage.StopJogNegative(axis);
        }

        private double ReadCurrentAxisposition(EnumStageAxis axis)
        {

            double position = stage.GetCurrentPosition(axis);
            //double position = 2;
            return position;
        }

        private void AxisHome(EnumStageAxis axis)
        {
            stage.Home(axis);
        }

        private XYZTCoordinateConfig BondCameraVisualTool(MatchIdentificationParam param)
        {
            try
            {
                XYZTCoordinateConfig result = new XYZTCoordinateConfig();

                result = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, param);

                return result;
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        private XYZTCoordinateConfig UplookingCameraVisualTool(MatchIdentificationParam param)
        {
            try
            {
                XYZTCoordinateConfig result = new XYZTCoordinateConfig();

                result = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, param);

                return result;
            }
            catch
            {
                return null;
            }

        }

        private void StartEutectic(int gpNum)
        {
            try
            {
                IOUtilityHelper.Instance.OpenTransportVaccum();
                //Thread.Sleep(100);

                IOUtilityHelper.Instance.OpenChipPPBlow();
                Thread.Sleep(100);
                IOUtilityHelper.Instance.CloseChipPPBlow();
            }
            catch (Exception ex)
            {

            }
        }



        private void MoveTransport()
        {
            //Task.Factory.StartNew(new Action(async () =>
            //{
            //    EnToIdentifyBMC = false;

            //    AxisForward(EnumStageAxis.TransportTrack1);
            //    AxisForward(EnumStageAxis.TransportTrack3);

            //    Thread.Sleep(1500);

            //    AxisStopForward(EnumStageAxis.TransportTrack1);
            //    AxisStopForward(EnumStageAxis.TransportTrack3);

            //    Thread.Sleep(100);

            //    AxisReverse(EnumStageAxis.TransportTrack1);
            //    AxisReverse(EnumStageAxis.TransportTrack3);

            //    Thread.Sleep(500);

            //    AxisStopReverse(EnumStageAxis.TransportTrack1);
            //    AxisStopReverse(EnumStageAxis.TransportTrack3);

            //    EnToIdentifyBMC = true;
            //}));

            
        }

        private void MoveWafer1()
        {
            //Task.Factory.StartNew(new Action(async () =>
            //{
            //    EnToIdentifyUpBMC = false;
            //    //WaferXYAbsoluteMove(SystemConfiguration.Instance.PositioningConfig.WaferOrigion.X, SystemConfiguration.Instance.PositioningConfig.WaferOrigion.Y);
            //    WaferXYAbsoluteMove(80, -150);
            //    EnToIdentifyUpBMC = true;
            //}));


        }

        private void MoveWafer2()
        {
            //Task.Factory.StartNew(new Action(async () =>
            //{
            //    WaferXYAbsoluteMove(1, -1);
            //}));

        }


        /// <summary>
        /// 榜头到安全位置
        /// </summary>
        private bool BondToSafeAsync(int Mode = 0)
        {
            DeviceMode = _systemConfig.SystemMode == EnumSystemMode.Eutectic ? 0 : 1;
            bool Done = false;
            if (Mode == 0)
            {
                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ);
                }
                else
                {
                    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }
                

                double BondX = _systemConfig.PositioningConfig.BondSafeLocation.X;
                double BondY = _systemConfig.PositioningConfig.BondSafeLocation.Y;
                double BondZ = _systemConfig.PositioningConfig.BondSafeLocation.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                BondXYZAbsoluteMove(BondX, BondY, BondZ);

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);

                
                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);
                }


                Done = true;
            }
            else if (Mode == 1)
            {
                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ);
                }
                else
                {
                    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }

                double BondX = _systemConfig.PositioningConfig.BondSafeLocation.X;
                double BondY = _systemConfig.PositioningConfig.BondSafeLocation.Y;
                double BondZ = _systemConfig.PositioningConfig.BondSafeLocation.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                BondXYZAbsoluteMove(BondX, BondY, BondZ);

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);

                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);
                }

                Done = true;
            }
            return Done;

        }

        /// <summary>
        /// 榜头相机移动到BMC位置，识别BMC
        /// </summary>
        /// <param name="Auto"></param>
        private bool BondCameraIdentifyBMCMoveAsync(int Mode = 0)
        {
            //while(!EnToIdentifyBMC)
            //{
            //    Thread.Sleep(50);
            //}

            if (CameraWindowGUI.Instance != null)
            {
                //CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "榜头相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
            //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);
            //BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);


            if (Mode == 0)
            {
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    sw.Stop();
                    LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机移动到芯片上方{sw.ElapsedMilliseconds}ms \n");

                    //Thread.Sleep(2000);

                    sw.Reset();
                    sw.Start();

                    //BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);
                    MatchIdentificationParam BondCameraChipparam2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                    //Thread.Sleep(2000);
                    XYZTCoordinateConfig offset_2 = BondCameraVisualTool(BondCameraChipparam2);

                    sw.Stop();
                    LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机识别BMC大板{sw.ElapsedMilliseconds}ms \n");

                    if (offset_2 == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    sw.Reset();
                    sw.Start();
                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    sw.Stop();
                    LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机识别BMC小板{sw.ElapsedMilliseconds}ms \n");
                    if (offset == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    XYZToffset6 = new XYZTOffsetConfig()
                    {
                        X = offset_2.X - offset.X,
                        Y = offset_2.Y - offset.Y,
                        Theta = offset_2.Theta - offset.Theta,
                    };

                    if (CameraWindowGUI.Instance != null)
                    {
                        //CameraWindowGUI.Instance.SelectCamera(0);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }

                    //EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    //double[] target = new double[2] { offset.X, offset.Y };
                    //_positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    currentBondBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                        Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
                {
                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

                    Thread.Sleep(2000);
                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    if (CameraWindowGUI.Instance != null)
                    {
                        //CameraWindowGUI.Instance.SelectCamera(0);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }

                    EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    double[] target = new double[2] { offset.X, offset.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                    double BondX1 = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    double BondY1 = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    double BondZ1 = ReadCurrentAxisposition(EnumStageAxis.BondZ);



                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);


                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2);

                    Thread.Sleep(2000);

                    MatchIdentificationParam BondCameraChipparam2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2;
                    XYZTCoordinateConfig offset2 = BondCameraVisualTool(BondCameraChipparam2);
                    if (offset == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    if (CameraWindowGUI.Instance != null)
                    {
                        //CameraWindowGUI.Instance.SelectCamera(0);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }

                    axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    target = new double[2] { offset.X, offset.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                    double BondX2 = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    double BondY2 = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                    {
                        X = (BondX1 + BondX2) / 2,
                        Y = (BondY1 + BondY2) / 2,
                        Z = BondZ,
                    };

                    currentBondCameraBMC = center;


                    double x1 = BondX1; // 第一个点的X坐标  
                    double y1 = BondY1; // 第一个点的Y坐标  
                    double x2 = BondX2; // 第二个点的X坐标  
                    double y2 = BondY2; // 第二个点的Y坐标  
                    double deltaX = x2 - x1;
                    double deltaY = y2 - y1;
                    double angleInRadians = Math.Atan2(deltaY, deltaX);
                    double angleInDegrees = angleInRadians * (180.0 / Math.PI);

                    double x = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset.X; // 原始X坐标  
                    double y = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset.Y; // 原始Y坐标  
                    double angleInDegreesoffset = angleInDegrees - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset.Theta;
                    double angleInRadiansoffset = angleInDegreesoffset * (Math.PI / 180.0);

                    // 计算新坐标  
                    double xNew = x * Math.Cos(angleInRadiansoffset) - y * Math.Sin(angleInRadiansoffset);
                    double yNew = x * Math.Sin(angleInRadiansoffset) + y * Math.Cos(angleInRadiansoffset);

                    

                    currentBondBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX1 + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + xNew,
                        Y = BondY1 + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + yNew,
                        Z = BondZ,
                        Theta = angleInDegreesoffset,
                    };

                    currentBondCameraBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX1 + xNew,
                        Y = BondY1 + yNew,
                        Z = BondZ,
                        Theta = angleInDegreesoffset,
                    };


                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = currentBondCameraBMC.X - (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset.X),
                        Y = currentBondCameraBMC.Y - (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset.Y),
                        Theta = angleInDegreesoffset,
                    };

                }


                if (CameraWindowGUI.Instance != null)
                {
                    //CameraWindowGUI.Instance.SelectCamera(0);
                    CameraWindowGUI.Instance.ClearGraphicDraw();
                }



            }
            else if (Mode == 1)
            {
                //ShowStage();
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
                {
                    title = "创建BMC特征点识别";
                    ShowMessage("动作确认", "创建BMC特征点识别", "提示");

                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }
                    else
                    {
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);
                        MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                        XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };
                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                            Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        currentBondCameraBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };

                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { currentBondCameraBMC.X, currentBondCameraBMC.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);

                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondCameraBMC;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset = new XYZTCoordinateConfig()
                        {
                            X = 0,
                            Y = 0,
                            Z = 0,
                            Theta = 0,
                        };

                        _systemConfig.SaveConfig();
                    }
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
                {
                    title = "创建BMC第一个特征点识别";
                    ShowMessage("动作确认", "创建BMC第一个特征点识别", "提示");

                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    double[] target = new double[2] { currentBondCameraBMC.X, currentBondCameraBMC.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondCameraBMC;

                    title = "创建BMC第二个特征点识别";
                    ShowMessage("动作确认", "创建BMC第二个特征点识别", "提示");

                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    VisualMatchControlGUI visualMatch2 = new VisualMatchControlGUI();
                    visualMatch2.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);
                    visualMatch2.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2);
                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2);

                    Done = SystemCalibration.Instance.ShowVisualForm(visualMatch2, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2 = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2;
                    offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    currentBondBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                        Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    target = new double[2] { currentBondCameraBMC.X, currentBondCameraBMC.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern = currentBondCameraBMC;

                    XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                    {
                        X = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X) / 2,
                        Y = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y) / 2,
                        Z = BondZ,
                    };

                    currentBondCameraBMC = center;


                    double x1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X; // 第一个点的X坐标  
                    double y1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y; // 第一个点的Y坐标  
                    double x2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X; // 第二个点的X坐标  
                    double y2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y; // 第二个点的Y坐标  
                    double deltaX = x2 - x1;
                    double deltaY = y2 - y1;
                    double angleInRadians = Math.Atan2(deltaY, deltaX);
                    double angleInDegrees = angleInRadians * (180.0 / Math.PI);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset = new XYZTCoordinateConfig()
                    {
                        X = center.X - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X,
                        Y = center.Y - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y,
                        Z = BondZ,
                        Theta = angleInDegrees,
                    };
                    currentBondBMC = new XYZTCoordinateConfig()
                    {
                        X = center.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X,
                        Y = center.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y,
                        Z = BondZ,
                    };
                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = 0,
                        Y = 0,
                        Theta = 0,
                    };
                    _systemConfig.SaveConfig();

                }



            }
            else if (Mode == 2)
            {
                //ShowStage();
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
                {
                    title = "创建BMC特征点识别";
                    ShowMessage("动作确认", "创建BMC特征点识别，手动移动对准特征点", "提示");

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }
                    else
                    {
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                        XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };
                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                            Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        currentBondCameraBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };

                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { currentBondCameraBMC.X, currentBondCameraBMC.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);

                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondCameraBMC;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset = new XYZTCoordinateConfig()
                        {
                            X = 0,
                            Y = 0,
                            Z = 0,
                            Theta = 0,
                        };

                        _systemConfig.SaveConfig();
                    }
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
                {
                    title = "创建BMC第一个特征点识别";
                    ShowMessage("动作确认", "创建BMC第一个特征点识别，手动移动对准第一个特征点", "提示");

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    double[] target = new double[2] { currentBondCameraBMC.X, currentBondCameraBMC.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondCameraBMC;


                    title = "创建BMC第二个特征点识别";
                    ShowMessage("动作确认", "创建BMC第二个特征点识别，手动移动对准第一个特征点", "提示");

                    VisualMatchControlGUI visualMatch2 = new VisualMatchControlGUI();
                    visualMatch2.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);
                    visualMatch2.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2);
                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2);

                    Done = SystemCalibration.Instance.ShowVisualForm(visualMatch2, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2 = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2;
                    offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    currentBondBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                        Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMC = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    target = new double[2] { currentBondCameraBMC.X, currentBondCameraBMC.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern = currentBondCameraBMC;

                    XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                    {
                        X = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X) / 2,
                        Y = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y) / 2,
                        Z = BondZ,
                    };

                    currentBondCameraBMC = center;


                    double x1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X; // 第一个点的X坐标  
                    double y1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y; // 第一个点的Y坐标  
                    double x2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X; // 第二个点的X坐标  
                    double y2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y; // 第二个点的Y坐标  
                    double deltaX = x2 - x1;
                    double deltaY = y2 - y1;
                    double angleInRadians = Math.Atan2(deltaY, deltaX);
                    double angleInDegrees = angleInRadians * (180.0 / Math.PI);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatchoffset = new XYZTCoordinateConfig()
                    {
                        X = center.X - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X,
                        Y = center.Y - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y,
                        Z = BondZ,
                        Theta = angleInDegrees,
                    };
                    currentBondBMC = new XYZTCoordinateConfig()
                    {
                        X = center.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X,
                        Y = center.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y,
                        Z = BondZ,
                    };
                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = 0,
                        Y = 0,
                        Theta = 0,
                    };
                    _systemConfig.SaveConfig();

                }




            }



            return true;

        }

        /// <summary>
        /// 榜头移动到BMC上方，关闭共晶台真空，吸取BMC，抬起榜头对BMC进行补偿（角度）
        /// </summary>
        private bool BondPickupBMC()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                //sw.Start();
                //Thread.Sleep(500);
                //CameraWindowGUI.Instance.ClearGraphicDraw();

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute);

                //sw.Stop();
                //LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴旋转到0°{sw.ElapsedMilliseconds}ms \n");

                double BondX = currentBondCameraBMC.X;
                double BondY = currentBondCameraBMC.Y;
                double BondZ = currentBondCameraBMC.Z;

                //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                //BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                //Thread.Sleep(1000);
                ////ShowMessage("动作确认", "是否吸取", "提示");

                BondX = currentBondBMC.X;
                BondY = currentBondBMC.Y;
                BondZ = currentBondBMC.Z;

                sw.Reset();
                sw.Start();

                //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                //BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + 0.5f);

                //BondXYZAbsoluteMove(BondX, BondY, BondZ);

                XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
                EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;
                multiAxis[2] = EnumStageAxis.ChipPPT;

                double[] target1 = new double[3];

                target1[0] = BondX;
                target1[1] = BondY;
                target1[2] = 0;

                //_positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Relative);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, BondZ, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴移动到BMC上方{sw.ElapsedMilliseconds}ms \n");

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute);

                //Thread.Sleep(500);
                //CameraWindowGUI.Instance.ClearGraphicDraw();


                //////芯片吸嘴拾取芯片
                ////var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                ////var PPWorkZForPickChip = curBondZPos + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z + BMCToolthickness;
                //////TODO:拾取衬底
                ////PPWorkParameters ppParamForPickChip = new PPWorkParameters();
                ////ppParamForPickChip.IsUseNeedle = false;
                ////ppParamForPickChip.UsedPP = EnumUsedPP.ChipPP;

                ////ppParamForPickChip.WorkHeight = (float)PPWorkZForPickChip;
                ////ppParamForPickChip.PickupStress = 0.2f;

                ////ppParamForPickChip.SlowSpeedBeforePickup = 5f;
                ////ppParamForPickChip.SlowTravelBeforePickupMM = 0.5f;

                ////ppParamForPickChip.SlowSpeedAfterPickup = 5f;
                ////ppParamForPickChip.SlowTravelAfterPickupMM = 2f;
                ////ppParamForPickChip.UpDistanceMMAfterPicked = 10f;

                ////ppParamForPickChip.DelayMSForVaccum = 100;

                ////IOUtilityHelper.Instance.CloseTransportVaccum();

                ////Thread.Sleep(10);

                ////PPUtility.Instance.Pick(ppParamForPickChip);

                //////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, currentBondBMC.Theta, EnumCoordSetType.Relative);

                ////_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);



                //TODO:拾取BMC
                PPWorkParameters ppParam = new PPWorkParameters();
                ppParam.IsUseNeedle = false;
                ppParam.UsedPP = EnumUsedPP.ChipPP;

                ppParam.PickupStress = 0f;

                ppParam.SlowSpeedBeforePickup = 5f;
                ppParam.SlowTravelBeforePickupMM = 0.5f;

                ppParam.SlowSpeedAfterPickup = 5f;
                ppParam.SlowTravelAfterPickupMM = 0.5f;
                ppParam.UpDistanceMMAfterPicked = 10f;

                ppParam.DelayMSForVaccum = 500;

                try
                {
                    var workheight = -28.75;
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == "PPtool1");

                    if (pptool != null)
                    {
                        ppParam.PPtoolName = pptool.Name;

                        var systemPos = workheight;

                        ppParam.PPToolZero = pptool.AltimetryOnMark;
                        ppParam.WorkHeight = (float)systemPos;
                    }
                    else
                    {
                        ppParam.PPtoolName = "PPtool1";
                        var systemPos = workheight;

                        ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                        ppParam.WorkHeight = (float)systemPos;
                    }
                    //拾取芯片
                    if (PPUtility.Instance.PickViaSystemCoor(ppParam, BeforePickChipFromBMCSubstrate))
                    {
                        LogRecorder.RecordLog(EnumLogContentType.Info, "从BMC基板拾取BMC成功！");
                        return true;
                    }
                    else
                    {
                        _positioningSystem.PPMovetoSafeLocation();
                        LogRecorder.RecordLog(EnumLogContentType.Error, "从BMC基板拾取BMC失败！");
                        return false;
                    }


                }
                catch (Exception)
                {

                    //throw;
                }



                return true;
            }
            catch
            {
                return false;
            }

        }

        public void AfterPlaceChipOnBMCSubstrate()
        {
            IOUtilityHelper.Instance.OpenTransportVaccum();
        }
        public void BeforePickChipFromBMCSubstrate()
        {
            IOUtilityHelper.Instance.CloseTransportVaccum();
        }

        /// <summary>
        /// 仰视相机创建识别 轮廓
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool UplookingMatchProcess(int Mode = 0)
        {
            try
            {
                //while (!EnToIdentifyUpBMC)
                //{
                //    Thread.Sleep(50);
                //}

                

                if (CameraWindowGUI.Instance != null)
                {
                    //CameraWindowGUI.Instance.SelectCamera(1);
                }
                if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                {
                    CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                    CameraWindowForm.Instance.Show();
                }

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                //吸嘴旋转补偿
                CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

                PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

                PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                //double angle = currentBondBMC.Theta;
                double angle = 0;
                double angle0 = 0;

                PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)0);

                PointF pointoffset = new PointF();
                if(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
                { 
                    pointoffset.X = (float)(point3.X); pointoffset.Y = (float)(point3.Y); 
                }
                else if(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
                {
                    pointoffset.X = (float)(point3.X + XYZToffset2.X); pointoffset.Y = (float)(point3.Y + XYZToffset2.Y);
                }

                //创建仰视识别
                string name = "仰视相机识别";
                string title = "";
                VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
                visualMatch.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);

                MatchIdentificationParam param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;

                MatchIdentificationParam param1 = new MatchIdentificationParam();

                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);
                //UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);


                if (Mode == 0)
                {
                    if (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum == 1)
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        BondX = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
                        BondY = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
                        BondZ = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                        BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                        sw.Stop();
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴移动到Uplooking相机中心{sw.ElapsedMilliseconds}ms \n");

                        sw.Reset();
                        sw.Start();
                        //Thread.Sleep(2000);
                        //XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
                        MatchIdentificationParam UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                        XYZTCoordinateConfig offset = UplookingCameraVisualTool(UplookingCameraChipparam);

                        sw.Stop();
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"Uplooking相机识别BMC小板{sw.ElapsedMilliseconds}ms \n");

                        if (offset == null)
                        {
                            int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                            if (result1 == 1)
                            {
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            sw.Reset();
                            sw.Start();

                            AxisAbsoluteMove(EnumStageAxis.ChipPPT, -offset.Theta + Toffset);

                            sw.Stop();
                            LogRecorder.RecordLog(EnumLogContentType.Info, $"BMC小板旋转补偿{sw.ElapsedMilliseconds}ms \n");

                            sw.Reset();
                            sw.Start();
                            //Thread.Sleep(2000);
                            //XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
                            UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                            offset = UplookingCameraVisualTool(UplookingCameraChipparam);

                            sw.Stop();
                            LogRecorder.RecordLog(EnumLogContentType.Info, $"Uplooking相机识别BMC小板{sw.ElapsedMilliseconds}ms \n");
                            if (offset == null)
                            {
                                int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                                if (result1 == 1)
                                {
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            XYZToffset3 = new XYZTOffsetConfig()
                            {
                                X = offset.X,
                                Y = offset.Y,
                                Theta = offset.Theta,
                            };

                            if (CameraWindowGUI.Instance != null)
                            {
                                //CameraWindowGUI.Instance.SelectCamera(1);
                                CameraWindowGUI.Instance.ClearGraphicDraw();
                            }

                            //EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                            //double[] target = new double[2] { offset.X, offset.Y };
                            //_positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                            BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                            BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                            BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                            currentBondBMC = new XYZTCoordinateConfig()
                            {
                                X = BondX + offset.X,
                                Y = BondY + offset.Y,
                                Z = BondZ,
                                Theta = offset.Theta,
                            };

                        }
                        

                    }
                    else if (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum == 2)
                    {
                        BondX = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + pointoffset.X;
                        BondY = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + pointoffset.Y;
                        BondZ = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                        UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);
                        Thread.Sleep(2000);
                        MatchIdentificationParam UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                        XYZTCoordinateConfig offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                        if (offset == null)
                        {
                            int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                            if (result1 == 1)
                            {
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        XYZTOffsetConfig XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.SelectCamera(1);
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                        }

                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { offset.X, offset.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                        double BondX1 = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        double BondY1 = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        double BondZ1 = ReadCurrentAxisposition(EnumStageAxis.BondZ);



                        BondX = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X + pointoffset.X;
                        BondY = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y + pointoffset.Y;
                        BondZ = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                        MatchIdentificationParam UplookingCameraChipparam2 = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2;
                        UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2);
                        Thread.Sleep(2000);
                        XYZTCoordinateConfig offset2 = UplookingCameraVisualTool(UplookingCameraChipparam2);
                        if (offset2 == null)
                        {
                            int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                            if (result1 == 1)
                            {
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        XYZTOffsetConfig XYZToffset2 = new XYZTOffsetConfig()
                        {
                            X = offset2.X,
                            Y = offset2.Y,
                            Theta = offset2.Theta,
                        };

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.SelectCamera(1);
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                        }

                        axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        target = new double[2] { offset2.X, offset2.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                        double BondX2 = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        double BondY2 = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                        XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                        {
                            X = (BondX1 + BondX2) / 2,
                            Y = (BondY1 + BondY2) / 2,
                            Z = BondZ,
                        };

                        currentBondBMC = center;


                        double x1 = BondX1; // 第一个点的X坐标  
                        double y1 = BondY1; // 第一个点的Y坐标  
                        double x2 = BondX2; // 第二个点的X坐标  
                        double y2 = BondY2; // 第二个点的Y坐标  
                        double deltaX = x2 - x1;
                        double deltaY = y2 - y1;
                        double angleInRadians = Math.Atan2(deltaY, deltaX);
                        double angleInDegrees = angleInRadians * (180.0 / Math.PI);

                        double x = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset.X; // 原始X坐标  
                        double y = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset.Y; // 原始Y坐标  
                        double angleInDegreesoffset = angleInDegrees - _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset.Theta;
                        double angleInRadiansoffset = angleInDegreesoffset * (Math.PI / 180.0);

                        // 计算新坐标  
                        double xNew = x * Math.Cos(angleInRadiansoffset) - y * Math.Sin(angleInRadiansoffset);
                        double yNew = x * Math.Sin(angleInRadiansoffset) + y * Math.Cos(angleInRadiansoffset);


                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX1 + xNew,
                            Y = BondY1 + yNew,
                            Z = BondZ,
                            Theta = angleInDegreesoffset,
                        };
                        XYZToffset3 = new XYZTOffsetConfig()
                        {
                            X = center.X - _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset.X,
                            Y = center.Y - _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset.Y,
                            Theta = angleInDegreesoffset,
                        };


                    }


                    if (CameraWindowGUI.Instance != null)
                    {
                        //CameraWindowGUI.Instance.SelectCamera(1);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }



                }
                else if (Mode == 1)
                {
                    //ShowStage();
                    if (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum == 1)
                    {
                        title = "创建BMC特征点识别";
                        ShowMessage("动作确认", "创建BMC特征点识别", "提示");

                        BondX = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                        BondY = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                        BondZ = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

                        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                        if (Done == 0)
                        {
                            return false;
                        }
                        else
                        {
                            _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch = visualMatch.GetVisualParam();
                            BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                            BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                            BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                            _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                            {
                                X = BondX,
                                Y = BondY,
                                Z = BondZ,
                            };

                            MatchIdentificationParam UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                            XYZTCoordinateConfig offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                            if (offset == null)
                            {
                                return false;
                            }
                            XYZTOffsetConfig XYZToffset1 = new XYZTOffsetConfig()
                            {
                                X = offset.X,
                                Y = offset.Y,
                                Theta = offset.Theta,
                            };
                            currentBondBMC = new XYZTCoordinateConfig()
                            {
                                X = BondX + offset.X,
                                Y = BondY + offset.Y,
                                Z = BondZ,
                                Theta = offset.Theta,
                            };

                            EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                            double[] target = new double[2] { currentBondBMC.X, currentBondBMC.Y };
                            _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);

                            _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondBMC;
                            XYZToffset3 = new XYZTOffsetConfig()
                            {
                                X = 0,
                                Y = 0,
                                Theta = 0,
                            };
                            _systemConfig.SaveConfig();
                        }
                    }
                    else if (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum == 2)
                    {
                        title = "创建BMC第一个特征点识别";
                        ShowMessage("动作确认", "创建BMC第一个特征点识别", "提示");

                        BondX = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                        BondY = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                        BondZ = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                        UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);
                        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

                        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                        if (Done == 0)
                        {
                            return false;
                        }

                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        MatchIdentificationParam UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                        XYZTCoordinateConfig offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZTOffsetConfig XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };
                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { currentBondBMC.X, currentBondBMC.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondBMC;

                        title = "创建BMC第二个特征点识别";
                        ShowMessage("动作确认", "创建BMC第二个特征点识别", "提示");

                        BondX = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X;
                        BondY = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y;
                        BondZ = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                        VisualMatchControlGUI visualMatch2 = new VisualMatchControlGUI();
                        visualMatch2.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);
                        visualMatch2.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2);
                        UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2);

                        Done = SystemCalibration.Instance.ShowVisualForm(visualMatch2, name, title);

                        if (Done == 0)
                        {
                            return false;
                        }

                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2 = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2;
                        offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZTOffsetConfig XYZToffset2 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };

                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        target = new double[2] { currentBondBMC.X, currentBondBMC.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern = currentBondBMC;

                        XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                        {
                            X = (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X) / 2,
                            Y = (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y) / 2,
                            Z = BondZ,
                        };

                        currentBondBMC = center;
                        double x1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X; // 第一个点的X坐标  
                        double y1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y; // 第一个点的Y坐标  
                        double x2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X; // 第二个点的X坐标  
                        double y2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y; // 第二个点的Y坐标  
                        double deltaX = x2 - x1;
                        double deltaY = y2 - y1;
                        double angleInRadians = Math.Atan2(deltaY, deltaX);
                        double angleInDegrees = angleInRadians * (180.0 / Math.PI);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset = new XYZTCoordinateConfig()
                        {
                            X = center.X,
                            Y = center.Y,
                            Z = center.Z,
                            Theta = angleInDegrees,
                        };
                        XYZToffset3 = new XYZTOffsetConfig()
                        {
                            X = 0,
                            Y = 0,
                            Theta = 0,
                        };
                        _systemConfig.SaveConfig();

                    }



                }
                else if (Mode == 2)
                {
                    //ShowStage();
                    if (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum == 1)
                    {
                        title = "创建BMC特征点识别";
                        ShowMessage("动作确认", "创建BMC特征点识别，手动移动对准特征点", "提示");

                        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

                        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                        if (Done == 0)
                        {
                            return false;
                        }
                        else
                        {
                            _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch = visualMatch.GetVisualParam();
                            BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                            BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                            BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                            _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                            {
                                X = BondX,
                                Y = BondY,
                                Z = BondZ,
                            };

                            MatchIdentificationParam UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                            XYZTCoordinateConfig offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                            if (offset == null)
                            {
                                return false;
                            }
                            XYZTOffsetConfig XYZToffset3 = new XYZTOffsetConfig()
                            {
                                X = offset.X,
                                Y = offset.Y,
                                Theta = offset.Theta,
                            };
                            currentBondBMC = new XYZTCoordinateConfig()
                            {
                                X = BondX + offset.X,
                                Y = BondY + offset.Y,
                                Z = BondZ,
                                Theta = offset.Theta,
                            };

                            EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                            double[] target = new double[2] { currentBondBMC.X, currentBondBMC.Y };
                            _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);

                            _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondBMC;
                            XYZToffset3 = new XYZTOffsetConfig()
                            {
                                X = 0,
                                Y = 0,
                                Theta = 0,
                            };
                            _systemConfig.SaveConfig();
                        }
                    }
                    else if (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum == 2)
                    {
                        title = "创建BMC第一个特征点识别";
                        ShowMessage("动作确认", "创建BMC第一个特征点识别，手动移动对准第一个特征点", "提示");

                        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

                        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                        if (Done == 0)
                        {
                            return false;
                        }

                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        MatchIdentificationParam UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
                        XYZTCoordinateConfig offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZTOffsetConfig XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };
                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { currentBondBMC.X, currentBondBMC.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern = currentBondBMC;


                        title = "创建BMC第二个特征点识别";
                        ShowMessage("动作确认", "创建BMC第二个特征点识别，手动移动对准第一个特征点", "提示");

                        VisualMatchControlGUI visualMatch2 = new VisualMatchControlGUI();
                        visualMatch2.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);
                        visualMatch2.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2);
                        UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2);

                        Done = SystemCalibration.Instance.ShowVisualForm(visualMatch2, name, title);

                        if (Done == 0)
                        {
                            return false;
                        }

                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2 = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2;
                        offset = UplookingCameraVisualTool(UplookingCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZTOffsetConfig XYZToffset2 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };

                        currentBondBMC = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        target = new double[2] { currentBondBMC.X, currentBondBMC.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern = currentBondBMC;

                        XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                        {
                            X = (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X) / 2,
                            Y = (_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y) / 2,
                            Z = BondZ,
                        };

                        currentBondBMC = center;
                        double x1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X; // 第一个点的X坐标  
                        double y1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y; // 第一个点的Y坐标  
                        double x2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X; // 第二个点的X坐标  
                        double y2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y; // 第二个点的Y坐标  
                        double deltaX = x2 - x1;
                        double deltaY = y2 - y1;
                        double angleInRadians = Math.Atan2(deltaY, deltaX);
                        double angleInDegrees = angleInRadians * (180.0 / Math.PI);
                        _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatchoffset = new XYZTCoordinateConfig()
                        {
                            X = center.X,
                            Y = center.Y,
                            Z = center.Z,
                            Theta = angleInDegrees,
                        };
                        XYZToffset3 = new XYZTOffsetConfig()
                        {
                            X = 0,
                            Y = 0,
                            Theta = 0,
                        };
                        _systemConfig.SaveConfig();

                    }




                }

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 榜头相机移动到BMC基板位置，识别BMC基板
        /// </summary>
        /// <param name="Auto"></param>
        private bool BondCameraIdentifyBMCSubstrateMoveAsync(int Mode = 0)
        {
            if (CameraWindowGUI.Instance != null)
            {
                //CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "榜头相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
            //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);
            BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);


            if (Mode == 0)
            {
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 1)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    sw.Stop();
                    LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机移动到BMC大板上方{sw.ElapsedMilliseconds}ms \n");

                    sw.Reset();
                    sw.Start();

                    //BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);
                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                    //Thread.Sleep(2000);
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);

                    sw.Stop();
                    LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机识别BMC大板{sw.ElapsedMilliseconds}ms \n");

                    if (offset == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    XYZToffset4 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    if (CameraWindowGUI.Instance != null)
                    {
                        //CameraWindowGUI.Instance.SelectCamera(0);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }

                    //EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    //double[] target = new double[2] { offset.X, offset.Y };
                    //_positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    currentBondBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                        Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 2)
                {
                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);
                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                    Thread.Sleep(2000);
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    XYZTOffsetConfig XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.SelectCamera(0);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }

                    EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    double[] target = new double[2] { offset.X, offset.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                    double BondX1 = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    double BondY1 = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    double BondZ1 = ReadCurrentAxisposition(EnumStageAxis.BondZ);



                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    MatchIdentificationParam BondCameraChipparam2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2;

                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2);
                    Thread.Sleep(2000);
                    XYZTCoordinateConfig offset2 = BondCameraVisualTool(BondCameraChipparam2);
                    if (offset2 == null)
                    {
                        int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                        if (result1 == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    XYZTOffsetConfig XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = offset2.X,
                        Y = offset2.Y,
                        Theta = offset2.Theta,
                    };

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.SelectCamera(0);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                    }

                    axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    target = new double[2] { offset2.X, offset2.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Relative);

                    double BondX2 = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    double BondY2 = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                    {
                        X = (BondX1 + BondX2) / 2,
                        Y = (BondY1 + BondY2) / 2,
                        Z = BondZ,
                    };

                    currentBondCameraBMCSubstrate = center;


                    double x1 = BondX1; // 第一个点的X坐标  
                    double y1 = BondY1; // 第一个点的Y坐标  
                    double x2 = BondX2; // 第二个点的X坐标  
                    double y2 = BondY2; // 第二个点的Y坐标  
                    double deltaX = x2 - x1;
                    double deltaY = y2 - y1;
                    double angleInRadians = Math.Atan2(deltaY, deltaX);
                    double angleInDegrees = angleInRadians * (180.0 / Math.PI);

                    double x = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset.X; // 原始X坐标  
                    double y = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset.Y; // 原始Y坐标  
                    double angleInDegreesoffset = angleInDegrees - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset.Theta;
                    double angleInRadiansoffset = angleInDegreesoffset * (Math.PI / 180.0);

                    // 计算新坐标  
                    double xNew = x * Math.Cos(angleInRadiansoffset) - y * Math.Sin(angleInRadiansoffset);
                    double yNew = x * Math.Sin(angleInRadiansoffset) + y * Math.Cos(angleInRadiansoffset);


                    currentBondBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX1 + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + xNew,
                        Y = BondY1 + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + yNew,
                        Z = BondZ,
                        Theta = angleInDegreesoffset,
                    };

                    currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX1 + xNew,
                        Y = BondY1 + yNew,
                        Z = BondZ,
                        Theta = angleInDegreesoffset,
                    };

                    XYZToffset4 = new XYZTOffsetConfig()
                    {
                        X = currentBondCameraBMCSubstrate.X - (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset.X),
                        Y = currentBondCameraBMCSubstrate.Y - (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset.Y),
                        Theta = angleInDegreesoffset,
                    };

                }


                if (CameraWindowGUI.Instance != null)
                {
                    //CameraWindowGUI.Instance.SelectCamera(0);
                    CameraWindowGUI.Instance.ClearGraphicDraw();
                }



            }
            else if (Mode == 1)
            {
                //ShowStage();
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 1)
                {
                    title = "创建BMCSubstrate特征点识别";
                    ShowMessage("动作确认", "创建BMCSubstrate特征点识别", "提示");

                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }
                    else
                    {
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                        XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };
                        currentBondBMCSubstrate = new XYZTCoordinateConfig()
                        {
                            X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                            Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };

                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { currentBondCameraBMCSubstrate.X, currentBondCameraBMCSubstrate.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);

                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = currentBondCameraBMCSubstrate;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset = new XYZTCoordinateConfig()
                        {
                            X = 0,
                            Y = 0,
                            Z = 0,
                            Theta = 0,
                        };
                        XYZToffset4 = new XYZTOffsetConfig()
                        {
                            X = 0,
                            Y = 0,
                            Theta = 0,
                        };
                        _systemConfig.SaveConfig();
                    }
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 2)
                {
                    title = "创建BMCSubstrate第一个特征点识别";
                    ShowMessage("动作确认", "创建BMCSubstrate第一个特征点识别", "提示");

                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);
                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    double[] target = new double[2] { currentBondCameraBMCSubstrate.X, currentBondCameraBMCSubstrate.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = currentBondCameraBMCSubstrate;

                    title = "创建BMCSubstrate第二个特征点识别";
                    ShowMessage("动作确认", "创建BMCSubstrate第二个特征点识别", "提示");

                    BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.X;
                    BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Y;
                    BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Z;

                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                    BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                    AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                    VisualMatchControlGUI visualMatch2 = new VisualMatchControlGUI();
                    visualMatch2.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);
                    visualMatch2.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2);
                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2);

                    Done = SystemCalibration.Instance.ShowVisualForm(visualMatch2, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2 = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2;
                    offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    currentBondBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                        Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    target = new double[2] { currentBondCameraBMCSubstrate.X, currentBondCameraBMCSubstrate.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern = currentBondCameraBMCSubstrate;

                    XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                    {
                        X = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.X) / 2,
                        Y = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Y) / 2,
                        Z = BondZ,
                    };

                    currentBondCameraBMCSubstrate = center;


                    double x1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X; // 第一个点的X坐标  
                    double y1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y; // 第一个点的Y坐标  
                    double x2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.X; // 第二个点的X坐标  
                    double y2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Y; // 第二个点的Y坐标  
                    double deltaX = x2 - x1;
                    double deltaY = y2 - y1;
                    double angleInRadians = Math.Atan2(deltaY, deltaX);
                    double angleInDegrees = angleInRadians * (180.0 / Math.PI);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset = new XYZTCoordinateConfig()
                    {
                        X = center.X - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X,
                        Y = center.Y - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y,
                        Z = BondZ,
                        Theta = angleInDegrees,
                    };
                    currentBondBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = center.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X,
                        Y = center.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y,
                        Z = BondZ,
                    };
                    XYZToffset4 = new XYZTOffsetConfig()
                    {
                        X = 0,
                        Y = 0,
                        Theta = 0,
                    };
                    _systemConfig.SaveConfig();

                }



            }
            else if (Mode == 2)
            {
                //ShowStage();
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 1)
                {
                    title = "创建BMCSubstrate特征点识别";
                    ShowMessage("动作确认", "创建BMCSubstrate特征点识别，手动移动对准特征点", "提示");

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }
                    else
                    {
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch = visualMatch.GetVisualParam();
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                        {
                            X = BondX,
                            Y = BondY,
                            Z = BondZ,
                        };

                        MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                        XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                        if (offset == null)
                        {
                            return false;
                        }
                        XYZToffset1 = new XYZTOffsetConfig()
                        {
                            X = offset.X,
                            Y = offset.Y,
                            Theta = offset.Theta,
                        };
                        currentBondBMCSubstrate = new XYZTCoordinateConfig()
                        {
                            X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                            Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };
                        currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                        {
                            X = BondX + offset.X,
                            Y = BondY + offset.Y,
                            Z = BondZ,
                            Theta = offset.Theta,
                        };

                        EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                        double[] target = new double[2] { currentBondCameraBMCSubstrate.X, currentBondCameraBMCSubstrate.Y };
                        _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);

                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = currentBondCameraBMCSubstrate;
                        _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset = new XYZTCoordinateConfig()
                        {
                            X = 0,
                            Y = 0,
                            Z = 0,
                            Theta = 0,
                        };
                        XYZToffset4 = new XYZTOffsetConfig()
                        {
                            X = 0,
                            Y = 0,
                            Theta = 0,
                        };
                        _systemConfig.SaveConfig();
                    }
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 2)
                {
                    title = "创建BMCSubstrate第一个特征点识别";
                    ShowMessage("动作确认", "创建BMCSubstrate第一个特征点识别，手动移动对准第一个特征点", "提示");

                    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch);

                    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                    XYZTCoordinateConfig offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZTOffsetConfig XYZToffset1 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    EnumStageAxis[] axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    double[] target = new double[2] { currentBondCameraBMCSubstrate.X, currentBondCameraBMCSubstrate.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern = currentBondCameraBMCSubstrate;


                    title = "创建BMCSubstrate第二个特征点识别";
                    ShowMessage("动作确认", "创建BMCSubstrate第二个特征点识别，手动移动对准第一个特征点", "提示");

                    VisualMatchControlGUI visualMatch2 = new VisualMatchControlGUI();
                    visualMatch2.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);
                    visualMatch2.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2);

                    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2);

                    Done = SystemCalibration.Instance.ShowVisualForm(visualMatch2, name, title);

                    if (Done == 0)
                    {
                        return false;
                    }

                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2 = visualMatch.GetVisualParam();
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern = new XYZTCoordinateConfig()
                    {
                        X = BondX,
                        Y = BondY,
                        Z = BondZ,
                    };

                    BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2;
                    offset = BondCameraVisualTool(BondCameraChipparam);
                    if (offset == null)
                    {
                        return false;
                    }
                    XYZTOffsetConfig XYZToffset2 = new XYZTOffsetConfig()
                    {
                        X = offset.X,
                        Y = offset.Y,
                        Theta = offset.Theta,
                    };

                    currentBondBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X + offset.X,
                        Y = BondY + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    currentBondCameraBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = BondX + offset.X,
                        Y = BondY + offset.Y,
                        Z = BondZ,
                        Theta = offset.Theta,
                    };
                    axis = new EnumStageAxis[2] { EnumStageAxis.BondX, EnumStageAxis.BondY };
                    target = new double[2] { currentBondCameraBMCSubstrate.X, currentBondCameraBMCSubstrate.Y };
                    _positioningSystem.MoveAixsToStageCoord(axis, target, EnumCoordSetType.Absolute);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern = currentBondCameraBMCSubstrate;

                    XYZTCoordinateConfig center = new XYZTCoordinateConfig()
                    {
                        X = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.X) / 2,
                        Y = (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y + _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Y) / 2,
                        Z = BondZ,
                    };

                    currentBondCameraBMCSubstrate = center;


                    double x1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X; // 第一个点的X坐标  
                    double y1 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y; // 第一个点的Y坐标  
                    double x2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.X; // 第二个点的X坐标  
                    double y2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2.BondTablePositionOfCreatePattern.Y; // 第二个点的Y坐标  
                    double deltaX = x2 - x1;
                    double deltaY = y2 - y1;
                    double angleInRadians = Math.Atan2(deltaY, deltaX);
                    double angleInDegrees = angleInRadians * (180.0 / Math.PI);
                    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatchoffset = new XYZTCoordinateConfig()
                    {
                        X = center.X - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.X,
                        Y = center.Y - _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch.BondTablePositionOfCreatePattern.Y,
                        Z = BondZ,
                        Theta = angleInDegrees,
                    };
                    currentBondBMCSubstrate = new XYZTCoordinateConfig()
                    {
                        X = center.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X,
                        Y = center.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y,
                        Z = BondZ,
                    };
                    XYZToffset4 = new XYZTOffsetConfig()
                    {
                        X = 0,
                        Y = 0,
                        Theta = 0,
                    };
                    _systemConfig.SaveConfig();

                }




            }



            return true;

        }

        /// <summary>
        /// 榜头移动到共晶台上方,榜头对BMC补偿，将BMC放到共晶台上，打开共晶台真空
        /// </summary>
        /// <param name="PP"></param>
        /// <param name="Auto"></param>
        private bool BondPutDownBMC()
        {
            try
            {
                if (CameraWindowGUI.Instance != null)
                {
                    //CameraWindowGUI.Instance.SelectCamera(0);
                }
                if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                {
                    CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                    CameraWindowForm.Instance.Show();
                }

                //吸嘴旋转补偿
                CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

                PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

                PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);



                //_boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);


                //榜头相机移动到贴装位置上方（共晶台）
                XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
                EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;
                //multiAxis[2] = EnumStageAxis.ChipPPT;

                double[] target1 = new double[2];
                //target1[0] = currentBondCameraBMCSubstrate.X + SystemConfiguration.Instance.PositioningConfig.EpoxtAndBondCameraOffset.X;
                //target1[1] = currentBondCameraBMCSubstrate.Y + SystemConfiguration.Instance.PositioningConfig.EpoxtAndBondCameraOffset.Y;

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                //_positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                //_boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 1);

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, currentBondCameraBMCSubstrate.Z + SystemConfiguration.Instance.PositioningConfig.EpoxtAndBondCameraOffset.Z + 8, EnumCoordSetType.Absolute);

                //Thread.Sleep(500);

                //_boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

                //target1[0] = currentBondCameraBMCSubstrate.X;
                //target1[1] = currentBondCameraBMCSubstrate.Y;

                //target1[0] = currentBondBMCSubstrate.X;
                //target1[1] = currentBondBMCSubstrate.Y;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                

                //Thread.Sleep(50);
                CameraWindowGUI.Instance.ClearGraphicDraw();

                //ShowMessage("动作确认", "是否吸取", "提示");
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                //最终贴装角度

                double Angle = 0;
                //if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
                //{
                //    Angle = 0 + XYZToffset1.Theta + XYZToffset3.Theta + XYZToffset4.Theta;
                //}
                //else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
                //{
                //    Angle = 0 + XYZToffset2.Theta + XYZToffset3.Theta + XYZToffset4.Theta;
                //}
                if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
                {
                    Angle = 0 + XYZToffset3.Theta - XYZToffset4.Theta;
                }
                else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
                {
                    Angle = 0 + XYZToffset3.Theta - XYZToffset4.Theta;
                }
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, currentBondCameraBMCSubstrate.Z + BondZOffset, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -Angle, EnumCoordSetType.Absolute);

                PointF point3 = PPCalibration.PPXYDeviationCal((float)0, (float)0);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴旋转补偿{sw.ElapsedMilliseconds}ms \n");


                //芯片吸嘴移动到贴装位置上方（共晶台）
                //_positioningSystem.ChipPPMovetoBondCameraCenter();

                //target1[0] = +XYZToffset3.X - XYZToffset4.X + point3.X;
                //target1[1] = +XYZToffset3.Y - XYZToffset4.Y + point3.Y;

                //target1[0] = XYZToffset3.X - XYZToffset4.X;
                //target1[1] = XYZToffset3.Y - XYZToffset4.Y;

                //target1[0] = +XYZToffset3.X + point3.X;
                //target1[1] = +XYZToffset3.Y + point3.Y;

                sw.Reset();
                sw.Start();

                //target1[0] = currentBondBMCSubstrate.X + XYZToffset3.X + point3.X;
                //target1[1] = currentBondBMCSubstrate.Y + XYZToffset3.Y + point3.Y;

                target1[0] = currentBondBMCSubstrate.X + XYZToffset3.X + point3.X - XYZToffset5.X + Xoffset;
                target1[1] = currentBondBMCSubstrate.Y + XYZToffset3.Y + point3.Y - XYZToffset5.Y + Yoffset;
                //target1[2] = -Angle;

                //_positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Relative);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, currentBondCameraBMCSubstrate.Z + 0, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                //double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                //double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                //double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);



                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴移动到BMC大板上方{sw.ElapsedMilliseconds}ms \n");

                //Thread.Sleep(10);

                ////芯片吸嘴放置芯片（共晶后抬起）
                //var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                //var PPWorkZForPlaceChip = curBondZPos + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z;
                //PPWorkParameters ppParamForPlaceChip = new PPWorkParameters();
                //ppParamForPlaceChip.IsUseNeedle = false;
                //ppParamForPlaceChip.UsedPP = EnumUsedPP.ChipPP;

                //ppParamForPlaceChip.WorkHeight = (float)PPWorkZForPlaceChip;
                //ppParamForPlaceChip.PickupStress = 0.2f;

                //ppParamForPlaceChip.SlowSpeedBeforePickup = 5f;
                //ppParamForPlaceChip.SlowTravelBeforePickupMM = 0.5f;

                //ppParamForPlaceChip.SlowSpeedAfterPickup = 5f;
                //ppParamForPlaceChip.SlowTravelAfterPickupMM = 1f;
                //ppParamForPlaceChip.UpDistanceMMAfterPicked = 10f;

                //ppParamForPlaceChip.DelayMSForVaccum = 3000;

                //PPUtility.Instance.Place(ppParamForPlaceChip, false, new Action(() => StartEutectic(1)));

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                //芯片吸嘴放置芯片（共晶后抬起）
                PPWorkParameters ppParam = new PPWorkParameters();
                ppParam.IsUseNeedle = false;
                ppParam.UsedPP = EnumUsedPP.ChipPP;

                ppParam.PickupStress = 0f;

                ppParam.SlowSpeedBeforePickup = 5f;
                ppParam.SlowTravelBeforePickupMM = 0.1f;

                ppParam.SlowSpeedAfterPickup = 5f;
                ppParam.SlowTravelAfterPickupMM = 0.1f;
                ppParam.UpDistanceMMAfterPicked = 10f;

                ppParam.DelayMSForVaccum = 50;
                ppParam.BreakVaccumTimespanMS = 50;


                var workheight = -28.7;
                var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == "PPtool1");

                if (pptool != null)
                {
                    ppParam.PPtoolName = pptool.Name;
                    var systemPos = workheight;

                    ppParam.PPToolZero = pptool.AltimetryOnMark;
                    ppParam.WorkHeight = (float)systemPos;
                }
                else
                {
                    ppParam.PPtoolName = "PPtool1";
                    var systemPos = workheight;

                    ppParam.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                    ppParam.WorkHeight = (float)systemPos;
                }

                if (!PPUtility.Instance.PlaceViaSystemCoor(ppParam, null, AfterPlaceChipOnBMCSubstrate, true))
                {
                    _positioningSystem.PPMovetoSafeLocation();
                    LogRecorder.RecordLog(EnumLogContentType.Error, "放置BMC到BMC基板失败！");
                    return false;
                }


                return true;
            }
            catch(Exception ex)
            {
                return false;
            }


        }

        /// <summary>
        /// 榜头相机移动到共晶台位置，识别BMC
        /// </summary>
        /// <param name="Auto"></param>
        private XYZTCoordinateConfig BondCameraIdentifyBMCAsync()
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 1 && _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 1)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机移动到BMC小板和大板上方{sw.ElapsedMilliseconds}ms \n");

                Thread.Sleep(200);

                sw.Reset();
                sw.Start();

                MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                XYZTCoordinateConfig center1 = BondCameraVisualTool(BondCameraChipparam);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机识别BMC大板位置{sw.ElapsedMilliseconds}ms \n");

                if (center1 != null)
                {

                }
                else
                {
                    int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                    return null;
                }
                sw.Reset();
                sw.Start();

                MatchIdentificationParam BondCameraChipparam2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                XYZTCoordinateConfig center2 = BondCameraVisualTool(BondCameraChipparam2);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"Bond相机识别BMC小板位置{sw.ElapsedMilliseconds}ms \n");

                if (center2 != null)
                {

                }
                else
                {
                    int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                    return null;
                }

                XYZTCoordinateConfig offset3 = new XYZTCoordinateConfig()
                {
                    X = center2.X - center1.X,
                    Y = center2.Y - center1.Y,
                    Theta = center2.Theta - center1.Theta,
                };
                

                return offset3;

            }
            else if (_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum == 2 && _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum == 2)
            {
                BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X;
                BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y;
                BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                Thread.Sleep(2000);

                MatchIdentificationParam BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch;
                XYZTCoordinateConfig center1 = BondCameraVisualTool(BondCameraChipparam);

                if (center1 != null)
                {

                }
                else
                {
                    int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                    return null;
                }
                MatchIdentificationParam BondCameraChipparam2 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch;
                XYZTCoordinateConfig center2 = BondCameraVisualTool(BondCameraChipparam2);
                if (center2 != null)
                {

                }
                else
                {
                    int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                    return null;
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(0);
                    CameraWindowGUI.Instance.ClearGraphicDraw();
                }


                BondX = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X;
                BondY = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y;
                BondZ = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);
                BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);
                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                Thread.Sleep(2000);

                MatchIdentificationParam BondCameraChipparam3 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateMatch2;
                XYZTCoordinateConfig center3 = BondCameraVisualTool(BondCameraChipparam3);

                if (center3 != null)
                {

                }
                else
                {
                    int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                    return null;
                }
                MatchIdentificationParam BondCameraChipparam4 = _systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2;
                XYZTCoordinateConfig center4 = BondCameraVisualTool(BondCameraChipparam4);
                if (center4 != null)
                {

                }
                else
                {
                    int result1 = SystemCalibration.Instance.ShowMessageAsync("动作确认", "识别失败", "提示");
                    return null;
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(0);
                    CameraWindowGUI.Instance.ClearGraphicDraw();
                }

                PointF point1 = new PointF((float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + center1.X), (float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + center1.Y));
                PointF point2 = new PointF((float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.X + center2.X), (float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch.BondTablePositionOfCreatePattern.Y + center2.Y));
                PointF point3 = new PointF((float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X + center3.X), (float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y + center3.Y));
                PointF point4 = new PointF((float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.X + center4.X), (float)(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch2.BondTablePositionOfCreatePattern.Y + center4.Y));

                PointF center5 = new PointF((point1.X + point3.X) / 2, (point1.Y + point3.Y) / 2);
                PointF center6 = new PointF((point2.X + point4.X) / 2, (point2.Y + point4.Y) / 2);

                double x1 = point1.X; // 第一个点的X坐标  
                double y1 = point1.Y; // 第一个点的Y坐标  
                double x2 = point3.X; // 第二个点的X坐标  
                double y2 = point3.Y; // 第二个点的Y坐标  
                double deltaX = x2 - x1;
                double deltaY = y2 - y1;
                double angleInRadians1 = Math.Atan2(deltaY, deltaX);

                x1 = point2.X; // 第一个点的X坐标  
                y1 = point2.Y; // 第一个点的Y坐标  
                x2 = point4.X; // 第二个点的X坐标  
                y2 = point4.Y; // 第二个点的Y坐标  
                deltaX = x2 - x1;
                deltaY = y2 - y1;
                double angleInRadians2 = Math.Atan2(deltaY, deltaX);

                XYZTCoordinateConfig offset3 = new XYZTCoordinateConfig()
                {
                    X = center6.X - center5.X,
                    Y = center6.Y - center5.Y,
                    Theta = angleInRadians1 - angleInRadians2,
                };
                return offset3;


            }
            else
            {
                return null;
            }

            

            

        }

        #endregion

        List<float> BondBMCXOffsets = new List<float>();
        List<float> BondBMCYOffsets = new List<float>();
        List<float> BondBMCThetaOffsets = new List<float>();

        List<float> UpBMCXOffsets = new List<float>();
        List<float> UpBMCYOffsets = new List<float>();
        List<float> UpBMCThetaOffsets = new List<float>();

       

        List<float> BondBMCSubstrateXOffsets = new List<float>();
        List<float> BondBMCSubstrateYOffsets = new List<float>();
        List<float> BondBMCSubstrateThetaOffsets = new List<float>();


        #region Public Method

        /// <summary>
        /// BMC运行
        /// </summary>
        /// <param name="PP">指定吸嘴</param>
        /// <param name="times">循环次数</param>
        /// <param name="delaytime">间隔时间</param>
        public void Run(int times, int delaytime)
        {
            try
            {
                SystemCalibration.Instance.InitCamera();
                if (CameraWindowGUI.Instance != null)
                {
                    //CameraWindowGUI.Instance.SelectCamera(0);
                }
                if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                {
                    CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                    CameraWindowForm.Instance.Show();
                }

                
                //ShowStage();

                EnToIdentifyBMC = true;
                EnToIdentifyUpBMC = true;

                List<float> XOffsets_1 = new List<float>();
                List<float> YOffsets_1 = new List<float>();

                List<float> XOffsets_2 = new List<float>();
                List<float> YOffsets_2 = new List<float>();

                List<float> XOffsets = new List<float>();
                List<float> YOffsets = new List<float>();
                List<float> ZOffsets = new List<float>();
                List<float> ThetaOffsets = new List<float>();

                XYZToffset5 = new XYZTOffsetConfig();

                


                Task.Factory.StartNew(new Action(async () =>
                {
                    //榜头移动到安全位置
                    BondToSafeAsync();

                    for (int i = 0; i < times + 6; i++)
                     {
                        bool Done = false;

                        LogRecorder.RecordLog(EnumLogContentType.Info, $"BMC Run:{i}.");
                        Thread.Sleep(delaytime);



                        //榜头移动到BMC位置，BMC识别
                        Done = BondCameraIdentifyBMCMoveAsync(0);
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "识别失败是否继续进行BMC", "提示");
                            if (Done1 == 0)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        //MoveWafer1();

                        //榜头移动到BMC上方，关闭BMC基板真空，吸取BMC
                        Done = BondPickupBMC();

                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "吸取BMC失败是否继续", "提示");
                            if (Done1 == 0)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }




                        //仰视识别BMC
                        Done = UplookingMatchProcess(0);
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "仰视相机BMC识别失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }

                        //MoveWafer2();

                        //榜头移动到BMC基板（纯手模式），创建贴片位置识别
                        Done = BondCameraIdentifyBMCSubstrateMoveAsync(0);
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "贴片位置识别失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }


                        //榜头移动到共晶台上方（纯手模式），将BMC放到共晶台上
                        Done = BondPutDownBMC();
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "放下BMC失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }

                        //XYZTCoordinateConfig XYZToffset = new XYZTCoordinateConfig();
                        //榜头相机移动到共晶台位置，识别BMC，记录BMC相对于相机中心的偏移
                        //XYZTCoordinateConfig XYZToffset = BondCameraIdentifyBMCAsync();

                        XYZTCoordinateConfig XYZToffset = new XYZTCoordinateConfig()
                        { X = 0, Y = 0, Theta = 0 };
                        
                        if(XYZToffset6 != null)
                        {
                            XYZToffset = new XYZTCoordinateConfig()
                            { X = XYZToffset6.X, Y = XYZToffset6.Y, Theta = XYZToffset6.Theta };
                        }
                        else
                        {
                            XYZToffset = null;
                        }

                        if (XYZToffset == null)
                        {
                            int Done1 = ShowMessage("动作确认", "识别失败是否继续进行BMC", "提示");
                            if (Done1 == 0)
                            {
                                break;
                            }
                            else
                            {
                                XYZToffset = new XYZTCoordinateConfig() { X = 0, Y = 0, Z = 0, Theta = 0 };
                            }
                        }

                        if (i <= 4)
                        {
                            XOffsets_1.Add((float)XYZToffset.X);
                            YOffsets_1.Add((float)XYZToffset.Y);
                        }

                        //if(i > 4)
                        //{
                        //    XYZToffset5 = new XYZTOffsetConfig()
                        //    {
                        //        X = XOffsets_1.Average(),
                        //        Y = YOffsets_1.Average(),
                        //        Theta = 0,
                        //    };

                            
                        //}

                        //if(i > 5)
                        //{
                        //    XOffsets_2.Add((float)XYZToffset.X);
                        //    YOffsets_2.Add((float)XYZToffset.Y);

                        //    XYZToffset5 = new XYZTOffsetConfig()
                        //    {
                        //        X = XOffsets_1.Average() + XOffsets_2.Average(),
                        //        Y = YOffsets_1.Average() + YOffsets_2.Average(),
                        //        Theta = 0,
                        //    };
                        //    if(XYZToffset1 != null)
                        //    {
                        //        BondBMCXOffsets.Add((float)XYZToffset1.X);
                        //        BondBMCYOffsets.Add((float)XYZToffset1.Y);
                        //        BondBMCThetaOffsets.Add((float)XYZToffset1.Theta);
                        //    }
                        //    if(XYZToffset3 != null)
                        //    {
                        //        UpBMCXOffsets.Add((float)XYZToffset3.X);
                        //        UpBMCYOffsets.Add((float)XYZToffset3.Y);
                        //        UpBMCThetaOffsets.Add((float)XYZToffset3.Theta);
                        //    }
                        //    if(XYZToffset4 != null)
                        //    {
                        //        BondBMCSubstrateXOffsets.Add((float)XYZToffset4.X);
                        //        BondBMCSubstrateYOffsets.Add((float)XYZToffset4.Y);
                        //        BondBMCSubstrateThetaOffsets.Add((float)XYZToffset4.Theta);
                        //    }
                            

                            

                        //    XOffsets.Add((float)XYZToffset.X);
                        //    YOffsets.Add((float)XYZToffset.Y);
                        //    ZOffsets.Add((float)XYZToffset.Z);
                        //    ThetaOffsets.Add((float)XYZToffset.Theta);
                        //}

                        if (i > 5)
                        {
                            XOffsets.Add((float)XYZToffset.X);
                            YOffsets.Add((float)XYZToffset.Y);
                            ZOffsets.Add((float)XYZToffset.Z);
                            ThetaOffsets.Add((float)XYZToffset.Theta);

                            if (XYZToffset1 != null)
                            {
                                BondBMCXOffsets.Add((float)XYZToffset1.X);
                                BondBMCYOffsets.Add((float)XYZToffset1.Y);
                                BondBMCThetaOffsets.Add((float)XYZToffset1.Theta);
                            }
                            if (XYZToffset3 != null)
                            {
                                UpBMCXOffsets.Add((float)XYZToffset3.X);
                                UpBMCYOffsets.Add((float)XYZToffset3.Y);
                                UpBMCThetaOffsets.Add((float)XYZToffset3.Theta);
                            }
                            if (XYZToffset4 != null)
                            {
                                BondBMCSubstrateXOffsets.Add((float)XYZToffset4.X);
                                BondBMCSubstrateYOffsets.Add((float)XYZToffset4.Y);
                                BondBMCSubstrateThetaOffsets.Add((float)XYZToffset4.Theta);
                            }

                        }


                    }

                    //MoveTransport();

                    //榜头移动到安全位置
                    BondToSafeAsync();



                    if (XOffsets != null && XOffsets.Count > 0)
                    {
                        float[] XOffsetsarry = XOffsets.ToArray();
                        float[] YOffsetsarry = YOffsets.ToArray();
                        float[] ZOffsetsarry = ZOffsets.ToArray();
                        float[] ThetaOffsetsarry = ThetaOffsets.ToArray();

                        double XOffsetmean = 0;
                        double XOffsetStandardDeviation = 0;
                        double XOffsetMax = 0;
                        double XOffsetMin = 0;
                        int XOffsetOut = 0;

                        double YOffsetmean = 0;
                        double YOffsetStandardDeviation = 0;
                        double YOffsetMax = 0;
                        double YOffsetMin = 0;
                        int YOffsetOut = 0;

                        double ZOffsetmean = 0;
                        double ZOffsetStandardDeviation = 0;
                        double ZOffsetMax = 0;
                        double ZOffsetMin = 0;
                        int ZOffsetOut = 0;

                        double ThetaOffsetmean = 0;
                        double ThetaOffsetStandardDeviation = 0;
                        double ThetaOffsetMax = 0;
                        double ThetaOffsetMin = 0;
                        int ThetaOffsetOut = 0;


                        BMCAlgorithms.NormalDistributionCal(XOffsetsarry, ref XOffsetmean, ref XOffsetStandardDeviation, ref XOffsetMax, ref XOffsetMin, ref XOffsetOut);
                        BMCAlgorithms.NormalDistributionCal(YOffsetsarry, ref YOffsetmean, ref YOffsetStandardDeviation, ref YOffsetMax, ref YOffsetMin, ref YOffsetOut);
                        BMCAlgorithms.NormalDistributionCal(ThetaOffsetsarry, ref ThetaOffsetmean, ref ThetaOffsetStandardDeviation, ref ThetaOffsetMax, ref ThetaOffsetMin, ref ThetaOffsetOut);

                        string XOffsetsarrystr1 = "Bond识别BMCX偏移：";
                        foreach (float value in BondBMCXOffsets)
                        {
                            XOffsetsarrystr1 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, XOffsetsarrystr1);
                        string YOffsetsarrystr1 = "Bond识别BMCY偏移：";
                        foreach (float value in BondBMCYOffsets)
                        {
                            YOffsetsarrystr1 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, YOffsetsarrystr1);
                        string ThetaOffsetsarrystr1 = "Bond识别BMCTheta偏移：";
                        foreach (float value in BondBMCThetaOffsets)
                        {
                            ThetaOffsetsarrystr1 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, ThetaOffsetsarrystr1);

                        string XOffsetsarrystr2 = "Up识别BMCX偏移：";
                        foreach (float value in UpBMCXOffsets)
                        {
                            XOffsetsarrystr2 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, XOffsetsarrystr2);
                        string YOffsetsarrystr2 = "Up识别BMCY偏移：";
                        foreach (float value in UpBMCYOffsets)
                        {
                            YOffsetsarrystr2 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, YOffsetsarrystr2);
                        string ThetaOffsetsarrystr2 = "Up识别BMCTheta偏移：";
                        foreach (float value in UpBMCThetaOffsets)
                        {
                            ThetaOffsetsarrystr2 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, ThetaOffsetsarrystr2);

                        string XOffsetsarrystr3 = "Bond识别BMC底板X偏移：";
                        foreach (float value in BondBMCSubstrateXOffsets)
                        {
                            XOffsetsarrystr3 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, XOffsetsarrystr3);
                        string YOffsetsarrystr3 = "Bond识别BMC底板Y偏移：";
                        foreach (float value in BondBMCSubstrateYOffsets)
                        {
                            YOffsetsarrystr3 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, YOffsetsarrystr3);
                        string ThetaOffsetsarrystr3 = "Bond识别BMC底板Theta偏移：";
                        foreach (float value in BondBMCSubstrateThetaOffsets)
                        {
                            ThetaOffsetsarrystr3 += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, ThetaOffsetsarrystr3);

                        string XOffsetsarrystr = "X轴：";
                        foreach (float value in XOffsetsarry)
                        {
                            XOffsetsarrystr += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, XOffsetsarrystr);
                        string YOffsetsarrystr = "Y轴：";
                        foreach (float value in YOffsetsarry)
                        {
                            YOffsetsarrystr += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, YOffsetsarrystr);
                        string ThetaOffsetsarrystr = "Theta轴：";
                        foreach (float value in ThetaOffsetsarry)
                        {
                            ThetaOffsetsarrystr += value + ", ";
                        }
                        LogRecorder.RecordLog(EnumLogContentType.Info, ThetaOffsetsarrystr);

                        string str = "";

                        LogRecorder.RecordLog(EnumLogContentType.Info, $"X轴补偿值:{XYZToffset5.X:F4}\n");
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"Y轴补偿值:{XYZToffset5.Y:F4}\n");

                        str = "BMC结果";
                        //输出BMC结果
                        ShowMessage(str, $"X轴:{XOffsetmean:F4}±{3 * XOffsetStandardDeviation:F4}" + $" { XOffsetMin:F4}~{ XOffsetMax:F4} \n" +
                            $"Y轴:{YOffsetmean:F4}±{3 * YOffsetStandardDeviation:F4}" + $" { YOffsetMin:F4}~{ YOffsetMax:F4} \n" +
                            $"Theta轴:{ ThetaOffsetmean:F4}±{ 3 * ThetaOffsetStandardDeviation:F4}" + $" { ThetaOffsetMin:F4}~{ ThetaOffsetMax:F4} \n", "提示");

                        LogRecorder.RecordLog(EnumLogContentType.Info, $"X轴:{XOffsetmean:F4}±{3 * XOffsetStandardDeviation:F4}" + $" { XOffsetMin:F4}~{ XOffsetMax:F4} \n");
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"Y轴:{YOffsetmean:F4}±{3 * YOffsetStandardDeviation:F4}" + $" { YOffsetMin:F4}~{ YOffsetMax:F4} \n");
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"Theta轴:{ ThetaOffsetmean:F4}±{ 3 * ThetaOffsetStandardDeviation:F4}" + $" { ThetaOffsetMin:F4}~{ ThetaOffsetMax:F4} \n");

                    }




                }));

            }
            catch
            {

            }

        }

        /// <summary>
        /// 创建BMC流程
        /// </summary>
        /// <param name="PP">指定吸嘴</param>
        /// <param name="Mode"> 1 半自动 2 全手动</param>
        public void CreationProcess(int Mode = 1)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }
            ShowStage();
            int Done = 1;
            //int Done = GreationBMCFormShow(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCSpotNum, _systemConfig.SystemCalibrationConfig.BondIdentifyBMCNum,
            //    _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCSpotNum, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCNum,
            //    _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateSpotNum, _systemConfig.SystemCalibrationConfig.BondIdentifyBMCSubstrateNum);
            if (Done == 1)
            {
                try
                {
                    Task.Factory.StartNew(new Action(async () =>
                    {
                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }
                        if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                        {
                            CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                            CameraWindowForm.Instance.Show();
                        }

                        BondToSafeAsync();

                        //榜头移动到BMC上方（纯手模式），创建BMC识别
                        bool Done0 = BondCameraIdentifyBMCMoveAsync(Mode);
                        if (Done0 == false)
                        {
                            int Done1 = ShowMessage("动作确认", "创建BMC识别失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }



                        //榜头移动到BMC上方（纯手模式），关闭共晶台真空，吸取BMC
                        Done0 = BondPickupBMC();
                        if (Done0 == false)
                        {
                            int Done1 = ShowMessage("动作确认", "吸取BMC失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }


                        //创建仰视识别
                        Done0 = UplookingMatchProcess(Mode);
                        if (Done0 == false)
                        {
                            int Done1 = ShowMessage("动作确认", "创建仰视相机BMC识别失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }

                        //榜头移动到BMC基板（纯手模式），创建贴片位置识别
                        Done0 = BondCameraIdentifyBMCSubstrateMoveAsync(Mode);
                        if (Done0 == false)
                        {
                            int Done1 = ShowMessage("动作确认", "创建贴片位置识别失败", "提示");
                            if (Done1 == 0)
                            {
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }


                        //榜头移动到共晶台上方（纯手模式），将BMC放到共晶台上
                        BondPutDownBMC();

                    }));

                }
                catch
                {

                }
            }
        }



        #endregion


    }
}
