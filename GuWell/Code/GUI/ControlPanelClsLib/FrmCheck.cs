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
using System.Diagnostics;
using System.IO;
using GlobalDataDefineClsLib;
using WestDragon.Framework;
using WestDragon.Framework.BaseLoggerClsLib;
using RecipeClsLib;
using WestDragon.Framework.UtilityHelper;
using ConfigurationClsLib;
using CameraControllerClsLib;
using System.Drawing.Imaging;
using System.Threading;
using CommonPanelClsLib;
using JobClsLib;
using GlobalToolClsLib;
using VisionGUI;
using PositioningSystemClsLib;
using PowerClsLib;
using IOUtilityClsLib;
using SystemCalibrationClsLib;
using VisionClsLib;
using ProductRunClsLib;
using VisionControlAppClsLib;

namespace ControlPanelClsLib
{
    public partial class FrmCheck : BaseForm
    {

        /// <summary>
        /// 是否人为点击Abort
        /// </summary>
        private bool _isAbort;

        string slotMap = "";
        public Action<EnumProcessStatus, string> OnShowProcessMessageAct { get; set; }
        public Action<string> OnRunningModeChanged { get; set; }
        public Action<bool> EnableNavigationButtonsAct { get; set; }
        private BondRecipe _currentRecipe;
        private string _currentLotID;
        private string _currentMaterialStripID;
        private string _currentSlotIndex;
        private DateTime _startTime;
        private int _marathonCounter = 0;
        /// <summary>
        /// 缓存队列机制,负责显示图片
        /// </summary>
        private BufferQueueMechanismOperation<byte[]> _dataBufferQueue;
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private CameraConfig _cameraConfig
        {
            get { return CameraManager.Instance.CurrentCameraConfig; }
        }
        private ICameraController _currentCamera
        {
            get { return CameraManager.Instance.CurrentCamera; }
        }

        private CameraManager _cameraManager
        {
            get { return CameraManager.Instance; }
        }
        public ICameraController BondCamera
        {
            get { return _cameraManager.GetCameraByID(EnumCameraType.BondCamera); }
        }
        public ICameraController UplookingCamera
        {
            get { return _cameraManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        public ICameraController WaferCamera
        {
            get { return _cameraManager.GetCameraByID(EnumCameraType.WaferCamera); }
        }

        MatchIdentificationParam BondCameraSubmountparam = new MatchIdentificationParam();
        MatchIdentificationParam BondCameraChipparam = new MatchIdentificationParam();

        MatchIdentificationParam UplookingCameraSubmountparam = new MatchIdentificationParam();
        MatchIdentificationParam UplookingCameraChipparam = new MatchIdentificationParam();

        MatchIdentificationParam BondCameraBondPositionparam = new MatchIdentificationParam();
        MatchIdentificationParam BondCameraCuttingPositionparam = new MatchIdentificationParam();

        MatchIdentificationParam BondCameraBlankingPositionparam = new MatchIdentificationParam();


        private bool _isMarathonJob;
        AutomaticJob _autoJob;
        private bool _isAutoJob = false;
        private List<JobMaterialInfo> _selectStripsForWeld = new List<JobMaterialInfo>();
        MaterialTransferExecutor _quickloadTransfer = new MaterialTransferExecutor();
        MaterialTransferExecutor _clearStripTransfer = new MaterialTransferExecutor();

        public FrmCheck()
        {
            CreateWaitDialog();
            InitializeComponent();
            this.InitControls();
            base.SetStyle(ControlStyles.Selectable, true);
            base.UpdateStyles();
            CloseWaitDialog();
        }
        CameraWindowGUI camera;

        private void InitControls()
        {
            panelControlCameraAera.Controls.Clear();
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //var camera = CameraWindowGUI.Instance;
            camera = new CameraWindowGUI();
            camera.Dock = DockStyle.Fill;
            camera.InitVisualControl();
            camera.SelectCamera(1);
            panelControlCameraAera.Controls.Add(camera);
        }
        private VisionControlAppClsLib.VisualControlManager _VisualManager
        {
            get { return VisionControlAppClsLib.VisualControlManager.Instance; }
        }
        public VisualControlApplications UplookingCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        XYZTCoordinateConfig _visionResult = new XYZTCoordinateConfig();
        PointF _newBMCCenter = new PointF();

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(camera, UplookingCameraVisual);

            MatchIdentificationParam param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
            UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

            _visionResult = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, param);

            if (_visionResult != null)
            {
                teAngle.Text = _visionResult?.Theta.ToString("0.0000");
                teCameraOffsetXBeforeR.Text = _visionResult.X.ToString("0.0000");
                teCameraOffsetYBeforeR.Text = _visionResult.Y.ToString("0.0000");
                WarningBox.FormShow("识别成功！", "完成", "提示");
            }
            else
            {
                WarningBox.FormShow("识别失败！", "完成", "提示");
            }
        }



        private void FrmSingleStepRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.WindowState = FormWindowState.Minimized;
        }

        private void btnCalOffset_Click(object sender, EventArgs e)
        {
            if (_visionResult != null)
            {
                //吸嘴旋转补偿
                CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

                PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

                PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                double angle = _visionResult.Theta;
                //double angle0 = 0;

                //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)0);

                var curChipCenterStagePosX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX) + _visionResult.X;
                var curChipCenterStagePosY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY) + _visionResult.Y;
                var centerOffset = new PointF();
                _newBMCCenter = PPCalibration.PPXYDeviationCal((float)curChipCenterStagePosX, (float)curChipCenterStagePosY, (float)angle,out centerOffset);

                teNewCenterX.Text = _newBMCCenter.X.ToString("0.0000");
                teNewCenterY.Text = _newBMCCenter.Y.ToString("0.0000");

                teRotateCenterX.Text = PPCalibration.Center.X.ToString("0.0000");
                teRotateCenterY.Text = PPCalibration.Center.Y.ToString("0.0000");

                teNewCenterOffsetX.Text = centerOffset.X.ToString("0.0000");
                teNewCenterOffsetY.Text = centerOffset.Y.ToString("0.0000");
            }
        
        }

        private void btnMove2Center_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("吸嘴移动到仰视中心？", "请确认榜头高度安全！", "提示") == 1)
            {
                _positioningSystem.ChipPPMovetoUplookingCameraCenter();

                teAngle.Text = "";

                teNewCenterX.Text = "";
                teNewCenterY.Text = "";

                teRotateCenterX.Text = "";
                teRotateCenterY.Text = "";

                teCameraOffsetXBeforeR.Text = "";
                teCameraOffsetYBeforeR.Text = "";

                teNewCenterOffsetX.Text = "";
                teNewCenterOffsetY.Text = "";

                teCameraOffsetX.Text = "";
                teCameraOffsetY.Text = "";

                teActualCenterOffsetX.Text = "";
                teActualCenterOffsetY.Text = "";
            }
        }

        private void btnRecogniseTwo_Click(object sender, EventArgs e)
        {
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(camera, UplookingCameraVisual);

            MatchIdentificationParam param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch;
            UplookingCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

            var visionResult1 = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.UplookingCamera, param);

            if (_visionResult != null)
            {
                teCameraOffsetX.Text = visionResult1.X.ToString("0.0000");
                teCameraOffsetY.Text = visionResult1.Y.ToString("0.0000");

                float CameraOffsetXBeforeR = 0f;
                float.TryParse(teCameraOffsetXBeforeR.Text, out CameraOffsetXBeforeR);
                float CameraOffsetYBeforeR = 0f;
                float.TryParse(teCameraOffsetYBeforeR.Text, out CameraOffsetYBeforeR);


                float CameraOffsetX = 0f;
                float.TryParse(teCameraOffsetX.Text, out CameraOffsetX);
                float CameraOffsetY = 0f;
                float.TryParse(teCameraOffsetY.Text, out CameraOffsetY);

                teActualCenterOffsetX.Text = (CameraOffsetX - CameraOffsetXBeforeR).ToString("0.0000");
                teActualCenterOffsetY.Text = (CameraOffsetY - CameraOffsetYBeforeR).ToString("0.0000");

                WarningBox.FormShow("识别成功！", "完成", "提示");
            }
            else
            {
                WarningBox.FormShow("识别失败！", "完成", "提示");
            }
        }
    }


}
