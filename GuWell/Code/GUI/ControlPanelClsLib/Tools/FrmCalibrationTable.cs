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
    public partial class FrmCalibrationTable : BaseForm
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

        public FrmCalibrationTable()
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
            camera.SelectCamera(0);
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





        private void FrmSingleStepRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.WindowState = FormWindowState.Minimized;
        }

        private void btnSetPos_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将记录校准台中心位置。", "请确认榜头相机中心已对准校准台中心！", "提示") == 1)
            {
                _systemConfig.PositioningConfig.CalibrationTableOrigion.X = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                _systemConfig.PositioningConfig.CalibrationTableOrigion.Y = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                var Altimetry=_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ) - _systemConfig.PositioningConfig.TrackOrigion.Z;
                _systemConfig.PositioningConfig.CalibrationTableOrigion.Z = Altimetry;
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功。", "校准台位置信息已保存！", "提示");
                //_systemConfig.PositioningConfig.CalibrationTableOrigion.X = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
            }
        }
    }


}
