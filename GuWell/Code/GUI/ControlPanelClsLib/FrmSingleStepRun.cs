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

namespace ControlPanelClsLib
{
    public partial class FrmSingleStepRun : BaseForm
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
        public FrmSingleStepRun()
        {
            CreateWaitDialog();
            InitializeComponent();
            InitializePosition();
            this.InitControls();
            base.SetStyle(ControlStyles.Selectable, true);
            base.UpdateStyles();
            CloseWaitDialog();
        }
        private void InitControls()
        {
            panelControlCameraAera.Controls.Clear();
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //var camera = CameraWindowGUI.Instance;
            var camera = new CameraWindowGUI();
            camera.Dock = DockStyle.Fill;
            camera.InitVisualControl();
            panelControlCameraAera.Controls.Add(camera);
        }
        private void InitializePosition()
        {
            teSubmountPosX.Text = _systemConfig.PositioningConfig.BondSubmountOrigion.X.ToString();
            teSubmountPosY.Text = _systemConfig.PositioningConfig.BondSubmountOrigion.Y.ToString();
            teSubmountPosZ.Text = _systemConfig.PositioningConfig.BondSubmountOrigion.Z.ToString();

            teEutecticPosX.Text = _systemConfig.PositioningConfig.EutecticWeldingLocation.X.ToString();
            teEutecticPosY.Text = _systemConfig.PositioningConfig.EutecticWeldingLocation.Y.ToString();
            teEutecticPosZ.Text = _systemConfig.PositioningConfig.EutecticWeldingLocation.Z.ToString();

            teChipPosX.Text = _systemConfig.PositioningConfig.BondChipOrigion.X.ToString();
            teChipPosY.Text = _systemConfig.PositioningConfig.BondChipOrigion.Y.ToString();
            teChipPosZ.Text = _systemConfig.PositioningConfig.BondChipOrigion.Z.ToString();

            teAccuracyPosX.Text = _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString();
            teAccuracyPosY.Text = _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString();
            teAccuracyPosZ.Text = _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString();


            teBondPosX.Text = _systemConfig.PositioningConfig.BondPosition.X.ToString();
            teBondPosY.Text = _systemConfig.PositioningConfig.BondPosition.Y.ToString();
            teBondPosZ.Text = _systemConfig.PositioningConfig.BondPosition.Z.ToString();


            teBlankingPosX.Text = _systemConfig.PositioningConfig.CuttingPosition.X.ToString();
            teBlankingPosY.Text = _systemConfig.PositioningConfig.CuttingPosition.Y.ToString();
            teBlankingPosZ.Text = _systemConfig.PositioningConfig.CuttingPosition.Z.ToString();


        }

        private void btnPickSubmount_Click(object sender, EventArgs e)
        {
            if(WarningBox.FormShow("条件确认","确认当前衬底的吸取位置在榜头相机视野的中心？","提示")==1)
            {
                try
                {
                    CreateWaitDialog();
                    //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                    _positioningSystem.SubmountPPMovetoBondCameraCenter();
                    //计算吸嘴工作高度
                    var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var submountPPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                    //TODO:拾取衬底
                    PPWorkParameters ppParam = new PPWorkParameters();
                    ppParam.IsUseNeedle = false;
                    ppParam.UsedPP = EnumUsedPP.SubmountPP;

                    ppParam.WorkHeight = (float)submountPPWorkZ;
                    ppParam.PickupStress = 0.2f;

                    ppParam.SlowSpeedBeforePickup = 5f;
                    ppParam.SlowTravelBeforePickupMM = 5f;

                    ppParam.SlowSpeedAfterPickup = 5f;
                    ppParam.SlowTravelAfterPickupMM = 5f;
                    ppParam.UpDistanceMMAfterPicked = 10f;

                    ppParam.DelayMSForVaccum = 3000;

                    PPUtility.Instance.Pick(ppParam);
                    CloseWaitDialog();
                    WarningBox.FormShow("动作结束！", "衬底拾取完成！", "提示");
                }
                catch (Exception ex)
                {
                    CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "衬底拾取失败！", "提示");
                }

            }
        }

        private void btnPlaceSubmount_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("条件确认", "确认衬底要放置的位置在榜头相机视野的中心？", "提示") == 1)
            {
                //TODO:放下衬底
                try
                {
                    CreateWaitDialog();
                    //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                    _positioningSystem.SubmountPPMovetoBondCameraCenter();
                    //计算吸嘴工作高度
                    var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var PPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                    //TODO:拾取衬底
                    PPWorkParameters ppParam = new PPWorkParameters();
                    ppParam.IsUseNeedle = false;
                    ppParam.UsedPP = EnumUsedPP.SubmountPP;

                    ppParam.WorkHeight = (float)PPWorkZ;
                    ppParam.PickupStress = 0.2f;

                    ppParam.SlowSpeedBeforePickup = 5f;
                    ppParam.SlowTravelBeforePickupMM = 5f;

                    ppParam.SlowSpeedAfterPickup = 5f;
                    ppParam.SlowTravelAfterPickupMM = 5f;
                    ppParam.UpDistanceMMAfterPicked = 10f;

                    ppParam.DelayMSForVaccum = 3000;

                    PPUtility.Instance.Place(ppParam,false,new Action(() => PlaceSubmountAction()));
                    CloseWaitDialog();
                    WarningBox.FormShow("动作结束！", "衬底放置完成！", "提示");
                }
                catch (Exception ex)
                {
                    CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "衬底放置失败！", "提示");
                }
            }
        }
        /// <summary>
        /// 放衬底时开启共晶台吸气
        /// </summary>
        private void PlaceSubmountAction()
        {
            IOUtilityHelper.Instance.OpenEutecticPlatformPPVaccum();
            IOUtilityHelper.Instance.OpenSubmountPPBlow();
            Thread.Sleep(1000);
            IOUtilityHelper.Instance.CloseSubmountPPBlow();
        }
        private void btnPickChip_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("条件确认", "确认当前芯片的吸取位置在榜头相机视野的中心？", "提示") == 1)
            {

                try
                {
                    CreateWaitDialog();
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                    //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                    _positioningSystem.ChipPPMovetoBondCameraCenter();
                    //计算吸嘴工作高度
                    var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var PPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z;
                    //TODO:拾取衬底
                    PPWorkParameters ppParam = new PPWorkParameters();
                    ppParam.IsUseNeedle = false;
                    ppParam.UsedPP = EnumUsedPP.ChipPP;

                    ppParam.WorkHeight = (float)PPWorkZ;
                    ppParam.PickupStress = 0.2f;

                    ppParam.SlowSpeedBeforePickup = 5f;
                    ppParam.SlowTravelBeforePickupMM = 5f;

                    ppParam.SlowSpeedAfterPickup = 5f;
                    ppParam.SlowTravelAfterPickupMM = 5f;
                    ppParam.UpDistanceMMAfterPicked = 10f;

                    ppParam.DelayMSForVaccum = 3000;

                    PPUtility.Instance.Pick(ppParam);
                    CloseWaitDialog();
                    WarningBox.FormShow("动作结束！", "芯片拾取完成！", "提示");
                }
                catch (Exception ex)
                {
                    CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "芯片拾取失败！", "提示");
                }
            }
        }

        private void btnPlaceChip_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("条件确认", "确认芯片贴装位置在榜头相机视野的中心？", "提示") == 1)
            {
                //TODO:放下芯片
                try
                {
                    CreateWaitDialog();

                    double Angle = double.Parse(seBondRotateAngle.Text);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -Angle, EnumCoordSetType.Relative);

                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                    //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                    _positioningSystem.ChipPPMovetoBondCameraCenter();
                    //计算吸嘴工作高度
                    var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var PPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z;
                    //TODO:拾取衬底
                    PPWorkParameters ppParam = new PPWorkParameters();
                    ppParam.IsUseNeedle = false;
                    ppParam.UsedPP = EnumUsedPP.ChipPP;

                    ppParam.WorkHeight = (float)PPWorkZ;
                    ppParam.PickupStress = 0.2f;

                    ppParam.SlowSpeedBeforePickup = 5f;
                    ppParam.SlowTravelBeforePickupMM = 5f;

                    ppParam.SlowSpeedAfterPickup = 5f;
                    ppParam.SlowTravelAfterPickupMM = 5f;
                    ppParam.UpDistanceMMAfterPicked = 10f;

                    ppParam.DelayMSForVaccum = 3000;

                    PPUtility.Instance.Place(ppParam);
                    CloseWaitDialog();
                    WarningBox.FormShow("动作结束！", "芯片拾取完成！", "提示");
                }
                catch (Exception ex)
                {
                    CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "芯片拾取失败！", "提示");
                }

            }
        }

        private void btnStartEutectic_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("条件确认", "芯片已正确放置到共晶位置？", "提示") == 1)
            {
                try
                {
                    CreateWaitDialog();
                    if (PowerManager.Instance.GetFault())
                    {
                        PowerManager.Instance.Reset();
                    }

                    PowerManager.Instance.SetGP(1);
                    PowerManager.Instance.PowerRun();

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (!PowerManager.Instance.GetStopSignal())
                    {
                        sw.Stop();
                        if (sw.ElapsedMilliseconds > 30000)
                        {
                            PowerManager.Instance.PowerStop();

                            return;
                        }
                        sw.Start();
                        Thread.Sleep(500);

                    }
                    sw.Stop();
                    PowerManager.Instance.PowerStop();
                    CloseWaitDialog();
                    WarningBox.FormShow("动作结束！", $"共晶完成！共耗时<{sw.ElapsedMilliseconds}>ms", "提示");
                }
                catch (Exception ex)
                {
                    PowerManager.Instance.PowerStop();
                    CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "共晶失败！", "提示");
                }
            }
        }
        private void StartEutectic(int gpNum)
        {
            try
            {
                if(PowerManager.Instance.GetFault())
                {
                    PowerManager.Instance.Reset();
                }

                PowerManager.Instance.SetGP(gpNum);
                PowerManager.Instance.PowerRun();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!PowerManager.Instance.GetStopSignal())
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > 30000)
                    {
                        PowerManager.Instance.PowerStop();

                        return;
                    }
                    sw.Start();
                    Thread.Sleep(500);

                }
                sw.Stop();
                PowerManager.Instance.PowerStop();
  


                IOUtilityHelper.Instance.OpenChipPPBlow();
                Thread.Sleep(1000);
                IOUtilityHelper.Instance.CloseChipPPBlow();
            }
            catch (Exception ex)
            {
                PowerManager.Instance.PowerStop();
            }
        }
        private void btnMoveBondtoSafePos_Click(object sender, EventArgs e)
        {
            _positioningSystem.BondMovetoSafeLocation();
        }

        private void btnSetSubmountPos_Click(object sender, EventArgs e)
        {
            teSubmountPosX.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX).ToString();
            teSubmountPosY.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY).ToString();
            teSubmountPosZ.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString();

            _systemConfig.PositioningConfig.BondSubmountOrigion.X = float.Parse(teSubmountPosX.Text);
            _systemConfig.PositioningConfig.BondSubmountOrigion.Y = float.Parse(teSubmountPosY.Text);
            _systemConfig.PositioningConfig.BondSubmountOrigion.Z = float.Parse(teSubmountPosZ.Text);
            _systemConfig.SaveConfig();
        }

        private void btnSetChipPos_Click(object sender, EventArgs e)
        {
            teChipPosX.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX).ToString();
            teChipPosY.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY).ToString();
            teChipPosZ.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString();

            _systemConfig.PositioningConfig.BondChipOrigion.X = float.Parse(teChipPosX.Text);
            _systemConfig.PositioningConfig.BondChipOrigion.Y = float.Parse(teChipPosY.Text);
            _systemConfig.PositioningConfig.BondChipOrigion.Z = float.Parse(teChipPosZ.Text);
            _systemConfig.SaveConfig();
        }

        private void btnSetBondPos_Click(object sender, EventArgs e)
         {
            teBondPosX.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX).ToString();
            teBondPosY.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY).ToString();
            teBondPosZ.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString();

            _systemConfig.PositioningConfig.BondPosition.X = float.Parse(teBondPosX.Text);
            _systemConfig.PositioningConfig.BondPosition.Y = float.Parse(teBondPosY.Text);
            _systemConfig.PositioningConfig.BondPosition.Z = float.Parse(teBondPosZ.Text);
            _systemConfig.SaveConfig();
        }

        private void btnSetBlankingPos_Click(object sender, EventArgs e)
        {
            teBlankingPosX.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX).ToString();
            teBlankingPosY.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY).ToString();
            teBlankingPosZ.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString();

            _systemConfig.PositioningConfig.CuttingPosition.X = float.Parse(teBlankingPosX.Text);
            _systemConfig.PositioningConfig.CuttingPosition.Y = float.Parse(teBlankingPosX.Text);
            _systemConfig.PositioningConfig.CuttingPosition.Z = float.Parse(teBlankingPosX.Text);
            _systemConfig.SaveConfig();
        }
        private void btnSetEutecticPos_Click(object sender, EventArgs e)
        {
            teEutecticPosX.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX).ToString();
            teEutecticPosY.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY).ToString();
            teEutecticPosZ.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString();
        }

        private void btnSetAccuracyPos_Click(object sender, EventArgs e)
        {
            teAccuracyPosX.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX).ToString();
            teAccuracyPosY.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY).ToString();
            teAccuracyPosZ.Text = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ).ToString();
        }
        /// <summary>
        /// 单颗半自动执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "开始自动流程？", "提示") == 1)
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    AutoGoJing();
                }));
            }
        }

        private void AutoGoJing()
        {
            //if (WarningBox.FormShow("动作确认", "开始自动流程？", "提示") == 1)
            {
                if (string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text)
                || string.IsNullOrEmpty(teEutecticPosX.Text) || string.IsNullOrEmpty(teEutecticPosY.Text) || string.IsNullOrEmpty(teEutecticPosZ.Text)
                || string.IsNullOrEmpty(teChipPosX.Text) || string.IsNullOrEmpty(teChipPosY.Text) || string.IsNullOrEmpty(teChipPosZ.Text)
                || string.IsNullOrEmpty(teBondPosX.Text) || string.IsNullOrEmpty(teBondPosY.Text) || string.IsNullOrEmpty(teBondPosZ.Text)
                || string.IsNullOrEmpty(teBlankingPosX.Text) || string.IsNullOrEmpty(teBlankingPosY.Text) || string.IsNullOrEmpty(teBlankingPosZ.Text)
                || string.IsNullOrEmpty(teAccuracyPosX.Text) || string.IsNullOrEmpty(teAccuracyPosY.Text) || string.IsNullOrEmpty(teAccuracyPosZ.Text))
                {
                    WarningBox.FormShow("错误", "位置参数无效!", "提示");
                    return;
                }
                //榜头相机移动到衬底中心上方
                XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
                EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;

                double[] target1 = new double[2];

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);


                CameraWindowGUI.Instance.SelectCamera(0);


                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teSubmountPosZ.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teSubmountPosX.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teSubmountPosY.Text), EnumCoordSetType.Absolute);

                target1[0] = double.Parse(teSubmountPosX.Text);
                target1[1] = double.Parse(teSubmountPosY.Text);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                //识别并移动到视野中心。并获取识别的角度TBD
                BondCameraSubmountparam = _systemConfig.SystemCalibrationConfig.BondIdentifySubmountMatch;
                offset = BondCameraVisual(BondCameraSubmountparam);
                
                CameraWindowGUI.Instance.SelectCamera(0);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X, EnumCoordSetType.Relative);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y, EnumCoordSetType.Relative);

                //衬底角度补偿
                if(offset.Theta > 0)
                {
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, -offset.Theta, EnumCoordSetType.Relative);
                }
                else
                {
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
                }

                Thread.Sleep(500);

                CameraWindowGUI.Instance.ClearGraphicDraw();

                //吸嘴移动到衬底中心上方
                _positioningSystem.SubmountPPMovetoBondCameraCenter();
                //衬底吸嘴拾取衬底
                var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                var submountPPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                PPWorkParameters ppParam = new PPWorkParameters();
                ppParam.IsUseNeedle = false;
                ppParam.UsedPP = EnumUsedPP.SubmountPP;

                ppParam.WorkHeight = (float)submountPPWorkZ;
                ppParam.PickupStress = 0.2f;

                ppParam.SlowSpeedBeforePickup = 5f;
                ppParam.SlowTravelBeforePickupMM = 5f;

                ppParam.SlowSpeedAfterPickup = 5f;
                ppParam.SlowTravelAfterPickupMM = 5f;
                ppParam.UpDistanceMMAfterPicked = 10f;

                ppParam.DelayMSForVaccum = 3000;

                PPUtility.Instance.Pick(ppParam);

                //衬底角度补偿
                if (offset.Theta > 0)
                {
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, offset.Theta, EnumCoordSetType.Relative);
                }
                else
                {
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, offset.Theta, EnumCoordSetType.Relative);
                }


                //榜头移动到仰视相机中心。做二次校准TBD

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);

                XYZTCoordinateConfig Bondoffset = _systemConfig.PositioningConfig.LookupSubmountPPOrigion;
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, Bondoffset.X, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, Bondoffset.Y, EnumCoordSetType.Absolute);
                target1[0] = Bondoffset.X;
                target1[1] = Bondoffset.Y;

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPWorkZ, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, Bondoffset.Z + 0.2, EnumCoordSetType.Absolute);

                CameraWindowGUI.Instance.SelectCamera(1);
                //bitmap = UplookingCamera.GetImageAsync2();
                //pictureBox1.Image = bitmap;
                Thread.Sleep(2000);
                CameraWindowGUI.Instance.SelectCamera(0);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);



                //榜头相机移动到共晶台中心上方
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teEutecticPosZ.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teEutecticPosX.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teEutecticPosY.Text), EnumCoordSetType.Absolute);
                target1[0] = double.Parse(teEutecticPosX.Text);
                target1[1] = double.Parse(teEutecticPosY.Text);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                //bitmap = BondCamera.GetImageAsync2();
                //pictureBox1.Image = bitmap;
                CameraWindowGUI.Instance.SelectCamera(0);
                Thread.Sleep(2000);


                //吸嘴移动到共晶台上方
                _positioningSystem.SubmountPPMovetoBondCameraCenter();
                //衬底吸嘴放下衬底
                curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                var PPWorkZForPlaceSubmount = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z + (double)seSubmountThickness.Value;
                PPWorkParameters ppParamForPlaceSubmount = new PPWorkParameters();
                ppParamForPlaceSubmount.IsUseNeedle = false;
                ppParamForPlaceSubmount.UsedPP = EnumUsedPP.SubmountPP;

                ppParamForPlaceSubmount.WorkHeight = (float)PPWorkZForPlaceSubmount;
                ppParamForPlaceSubmount.PickupStress = 0.2f;

                ppParamForPlaceSubmount.SlowSpeedBeforePickup = 5f;
                ppParamForPlaceSubmount.SlowTravelBeforePickupMM = 5f;

                ppParamForPlaceSubmount.SlowSpeedAfterPickup = 5f;
                ppParamForPlaceSubmount.SlowTravelAfterPickupMM = 5f;
                ppParamForPlaceSubmount.UpDistanceMMAfterPicked = 10f;

                ppParamForPlaceSubmount.DelayMSForVaccum = 3000;

                PPUtility.Instance.Place(ppParamForPlaceSubmount, false, new Action(() => PlaceSubmountAction()));

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);


                //榜头相机移动到芯片中心上方
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teChipPosZ.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teChipPosX.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teChipPosY.Text), EnumCoordSetType.Absolute);
                target1[0] = double.Parse(teChipPosX.Text);
                target1[1] = double.Parse(teChipPosY.Text);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

               
                //识别芯片并移动到视野中心。并获取识别的角度TBD
                BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;
                offset = BondCameraVisual(BondCameraChipparam);
                CameraWindowGUI.Instance.SelectCamera(0);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X, EnumCoordSetType.Relative);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y, EnumCoordSetType.Relative);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute);

                Thread.Sleep(500);
                CameraWindowGUI.Instance.ClearGraphicDraw();

                //吸嘴移动到芯片中心上方
                _positioningSystem.ChipPPMovetoBondCameraCenter();
                //芯片吸嘴拾取芯片
                curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                var PPWorkZForPickChip = curBondZPos + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z;
                //TODO:拾取衬底
                PPWorkParameters ppParamForPickChip = new PPWorkParameters();
                ppParamForPickChip.IsUseNeedle = false;
                ppParamForPickChip.UsedPP = EnumUsedPP.ChipPP;

                ppParamForPickChip.WorkHeight = (float)PPWorkZForPickChip;
                ppParamForPickChip.PickupStress = 0.2f;

                ppParamForPickChip.SlowSpeedBeforePickup = 5f;
                ppParamForPickChip.SlowTravelBeforePickupMM = 5f;

                ppParamForPickChip.SlowSpeedAfterPickup = 5f;
                ppParamForPickChip.SlowTravelAfterPickupMM = 5f;
                ppParamForPickChip.UpDistanceMMAfterPicked = 10f;

                ppParamForPickChip.DelayMSForVaccum = 3000;

                PPUtility.Instance.Pick(ppParamForPickChip);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, offset.Theta, EnumCoordSetType.Relative);

                //榜头移动到仰视相机中心。做二次校准TBD

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);

                XYZTCoordinateConfig Bondoffset1 = _systemConfig.PositioningConfig.LookupChipPPOrigion;
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, Bondoffset1.X, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, Bondoffset1.Y, EnumCoordSetType.Absolute);
                target1[0] = Bondoffset1.X;
                target1[1] = Bondoffset1.Y;

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, Bondoffset1.Z + 0.2, EnumCoordSetType.Absolute);

                //bitmap = UplookingCamera.GetImageAsync2();
                //pictureBox1.Image = bitmap;
                CameraWindowGUI.Instance.SelectCamera(1);
                Thread.Sleep(2000);
                CameraWindowGUI.Instance.SelectCamera(0);

                //榜头相机移动到贴装位置上方（共晶台）
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teBondPosZ.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teBondPosX.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teBondPosY.Text), EnumCoordSetType.Absolute);

                target1[0] = double.Parse(teBondPosX.Text);
                target1[1] = double.Parse(teBondPosY.Text);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                //bitmap = BondCamera.GetImageAsync2();
                //pictureBox1.Image = bitmap;
                CameraWindowGUI.Instance.SelectCamera(0);
                Thread.Sleep(2000);

                BondCameraBondPositionparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBondPositionMatch;
                offset = BondCameraVisual(BondCameraBondPositionparam);
                CameraWindowGUI.Instance.SelectCamera(0);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X, EnumCoordSetType.Relative);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y, EnumCoordSetType.Relative);

                Thread.Sleep(500);
                CameraWindowGUI.Instance.ClearGraphicDraw();

                //最终贴装角度

                double Angle = double.Parse(seBondRotateAngle.Text) + offset.Theta;
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -Angle, EnumCoordSetType.Relative);



                //芯片吸嘴移动到贴装位置上方（共晶台）
                _positioningSystem.ChipPPMovetoBondCameraCenter();
                //芯片吸嘴放置芯片（共晶后抬起）
                curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                var PPWorkZForPlaceChip = curBondZPos + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z;
                PPWorkParameters ppParamForPlaceChip = new PPWorkParameters();
                ppParamForPlaceChip.IsUseNeedle = false;
                ppParamForPlaceChip.UsedPP = EnumUsedPP.ChipPP;

                ppParamForPlaceChip.WorkHeight = (float)PPWorkZForPlaceChip;
                ppParamForPlaceChip.PickupStress = 0.2f;

                ppParamForPlaceChip.SlowSpeedBeforePickup = 5f;
                ppParamForPlaceChip.SlowTravelBeforePickupMM = 5f;

                ppParamForPlaceChip.SlowSpeedAfterPickup = 5f;
                ppParamForPlaceChip.SlowTravelAfterPickupMM = 5f;
                ppParamForPlaceChip.UpDistanceMMAfterPicked = 10f;

                ppParamForPlaceChip.DelayMSForVaccum = 3000;

                PPUtility.Instance.Place(ppParamForPlaceChip, false, new Action(() => StartEutectic(1)));

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);


                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teBlankingPosZ.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teSubmountPosX.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teSubmountPosY.Text), EnumCoordSetType.Absolute);

                target1[0] = double.Parse(teBlankingPosX.Text);
                target1[1] = double.Parse(teBlankingPosY.Text);

                _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);

                //识别并移动到视野中心。并获取识别的角度TBD
                BondCameraBlankingPositionparam = _systemConfig.SystemCalibrationConfig.BondIdentifyCuttingPositionMatch;
                offset = BondCameraVisual(BondCameraBlankingPositionparam);

                CameraWindowGUI.Instance.SelectCamera(0);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X, EnumCoordSetType.Relative);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y, EnumCoordSetType.Relative);

                Thread.Sleep(500);

                //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                _positioningSystem.SubmountPPMovetoBondCameraCenter();
                //计算吸嘴工作高度
                curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                submountPPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                //TODO:拾取衬底
                ppParam = new PPWorkParameters();
                ppParam.IsUseNeedle = false;
                ppParam.UsedPP = EnumUsedPP.SubmountPP;

                ppParam.WorkHeight = (float)submountPPWorkZ;
                ppParam.PickupStress = 0.2f;

                ppParam.SlowSpeedBeforePickup = 5f;
                ppParam.SlowTravelBeforePickupMM = 5f;

                ppParam.SlowSpeedAfterPickup = 5f;
                ppParam.SlowTravelAfterPickupMM = 5f;
                ppParam.UpDistanceMMAfterPicked = 10f;

                ppParam.DelayMSForVaccum = 3000;

                IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();

                PPUtility.Instance.Pick(ppParam);


                if (string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text))
                {
                    WarningBox.FormShow("错误", "位置参数无效!", "提示");
                    return;
                }
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teSubmountPosZ.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teSubmountPosX.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teSubmountPosY.Text), EnumCoordSetType.Absolute);


                //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                _positioningSystem.SubmountPPMovetoBondCameraCenter();
                //计算吸嘴工作高度
                curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                var PPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                //TODO:拾取衬底
                ppParam = new PPWorkParameters();
                ppParam.IsUseNeedle = false;
                ppParam.UsedPP = EnumUsedPP.SubmountPP;

                ppParam.WorkHeight = (float)PPWorkZ;
                ppParam.PickupStress = 0.2f;

                ppParam.SlowSpeedBeforePickup = 5f;
                ppParam.SlowTravelBeforePickupMM = 5f;

                ppParam.SlowSpeedAfterPickup = 5f;
                ppParam.SlowTravelAfterPickupMM = 5f;
                ppParam.UpDistanceMMAfterPicked = 10f;

                ppParam.DelayMSForVaccum = 3000;

                PPUtility.Instance.Place(ppParam, false, new Action(() => PlaceSubmountAction()));

                IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();

                ////榜头相机移动到下料位置上方（共晶台）
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teBlankingPosZ.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teBlankingPosX.Text), EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teBlankingPosY.Text), EnumCoordSetType.Absolute);
                ////衬底吸嘴移动到下料位置上方（共晶台）
                //_positioningSystem.SubmountPPMovetoBondCameraCenter();
                ////衬底吸嘴拾取衬底(下料)
                //curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                //var PPWorkZForBlanking = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                //PPWorkParameters ppParamForBlanking = new PPWorkParameters();
                //ppParamForBlanking.IsUseNeedle = false;
                //ppParamForBlanking.UsedPP = EnumUsedPP.SubmountPP;

                //ppParamForBlanking.WorkHeight = (float)PPWorkZForBlanking;
                //ppParamForBlanking.PickupStress = 0.2f;

                //ppParamForBlanking.SlowSpeedBeforePickup = 5f;
                //ppParamForBlanking.SlowTravelBeforePickupMM = 5f;

                //ppParamForBlanking.SlowSpeedAfterPickup = 5f;
                //ppParamForBlanking.SlowTravelAfterPickupMM = 5f;
                //ppParamForBlanking.UpDistanceMMAfterPicked = 10f;

                //ppParamForBlanking.DelayMSForVaccum = 3000;

                //PPUtility.Instance.Pick(ppParamForBlanking);

                _positioningSystem.BondMovetoSafeLocation();

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, 0, EnumCoordSetType.Absolute);
            }

        }

        private void btnBlanking_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("条件确认", "确认当前衬底的下料位置在榜头相机视野的中心？", "提示") == 1)
            {
                //TODO:拾取衬底
                try
                {
                    CreateWaitDialog();
                    //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                    _positioningSystem.SubmountPPMovetoBondCameraCenter();
                    //计算吸嘴工作高度
                    var curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var submountPPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                    //TODO:拾取衬底
                    PPWorkParameters ppParam = new PPWorkParameters();
                    ppParam.IsUseNeedle = false;
                    ppParam.UsedPP = EnumUsedPP.SubmountPP;

                    ppParam.WorkHeight = (float)submountPPWorkZ;
                    ppParam.PickupStress = 0.2f;

                    ppParam.SlowSpeedBeforePickup = 5f;
                    ppParam.SlowTravelBeforePickupMM = 5f;

                    ppParam.SlowSpeedAfterPickup = 5f;
                    ppParam.SlowTravelAfterPickupMM = 5f;
                    ppParam.UpDistanceMMAfterPicked = 10f;

                    ppParam.DelayMSForVaccum = 3000;

                    IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();

                    PPUtility.Instance.Pick(ppParam);


                    if (string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text))
                    {
                        WarningBox.FormShow("错误", "位置参数无效!", "提示");
                        return;
                    }
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teSubmountPosZ.Text), EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teSubmountPosX.Text), EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teSubmountPosY.Text), EnumCoordSetType.Absolute);


                    //第一步：衬底吸嘴移动到榜头相机中心(只移XY)
                    _positioningSystem.SubmountPPMovetoBondCameraCenter();
                    //计算吸嘴工作高度
                    curBondZPos = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var PPWorkZ = curBondZPos + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z;
                    //TODO:拾取衬底
                    ppParam = new PPWorkParameters();
                    ppParam.IsUseNeedle = false;
                    ppParam.UsedPP = EnumUsedPP.SubmountPP;

                    ppParam.WorkHeight = (float)PPWorkZ;
                    ppParam.PickupStress = 0.2f;

                    ppParam.SlowSpeedBeforePickup = 5f;
                    ppParam.SlowTravelBeforePickupMM = 5f;

                    ppParam.SlowSpeedAfterPickup = 5f;
                    ppParam.SlowTravelAfterPickupMM = 5f;
                    ppParam.UpDistanceMMAfterPicked = 10f;

                    ppParam.DelayMSForVaccum = 3000;

                    PPUtility.Instance.Place(ppParam, false, new Action(() => PlaceSubmountAction()));

                    IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();

                    CloseWaitDialog();
                    WarningBox.FormShow("动作结束！", "衬底拾取完成！", "提示");
                }
                catch (Exception ex)
                {
                    CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "衬底拾取失败！", "提示");
                }
            }
        }

        private void btnCreateSubmountTemplate_Click(object sender, EventArgs e)
        {
            BondCameraSubmountparam = _systemConfig.SystemCalibrationConfig.BondIdentifySubmountMatch;

            string name = "榜头相机创建衬底模板";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, SystemCalibration.Instance.BondCameraVisual);

            visualMatch.SetVisualParam(BondCameraSubmountparam);

            int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return;
            }
            else
            {
                BondCameraSubmountparam = visualMatch.GetVisualParam();

                _systemConfig.SystemCalibrationConfig.BondIdentifySubmountMatch = BondCameraSubmountparam;

                _systemConfig.SaveConfig();
            }
        }

        private void btnPositionSubmount_Click(object sender, EventArgs e)
        {
            //Task.Factory.StartNew(new Action(() =>
            //{
                BondCameraSubmountparam = _systemConfig.SystemCalibrationConfig.BondIdentifySubmountMatch;

                SystemCalibration.Instance.IdentificationMoveAsync(EnumCameraType.BondCamera, BondCameraSubmountparam);

                CameraWindowGUI.Instance.SelectCamera(0);
            CameraWindowGUI.Instance.ClearGraphicDraw();
            //}));

        }

        private void btnCreateChipTemplate_Click(object sender, EventArgs e)
        {
            BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;

            string name = "榜头相机创建芯片模板";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, SystemCalibration.Instance.BondCameraVisual);

            visualMatch.SetVisualParam(BondCameraChipparam);

            int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return;
            }
            else
            {
                BondCameraChipparam = visualMatch.GetVisualParam();

                _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch = BondCameraChipparam;

                _systemConfig.SaveConfig();
            }
        }

        private void btnPositionChip_Click(object sender, EventArgs e)
        {
            //Task.Factory.StartNew(new Action(() =>
            //{
                BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;

                SystemCalibration.Instance.IdentificationMoveAsync(EnumCameraType.BondCamera, BondCameraChipparam);

                CameraWindowGUI.Instance.SelectCamera(0);
                CameraWindowGUI.Instance.ClearGraphicDraw();
            //}));
            
        }

        private XYZTCoordinateConfig BondCameraVisual(MatchIdentificationParam param)
        {
            try
            {
                XYZTCoordinateConfig result = new XYZTCoordinateConfig();

                result = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, param);

                return result;
            }
            catch
            {
                return null;
            }

        }

        private void btnCreateBondPositionTemplate_Click(object sender, EventArgs e)
        {
            BondCameraBondPositionparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBondPositionMatch;

            string name = "榜头相机创建贴装位置";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, SystemCalibration.Instance.BondCameraVisual);

            visualMatch.SetVisualParam(BondCameraBondPositionparam);

            int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return;
            }
            else
            {
                BondCameraBondPositionparam = visualMatch.GetVisualParam();

                _systemConfig.SystemCalibrationConfig.BondIdentifyBondPositionMatch = BondCameraBondPositionparam;

                _systemConfig.SaveConfig();
            }
        }

        private void btnCreateUplookingTemplate_Click(object sender, EventArgs e)
        {
            UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyChipMatch;

            string name = "仰视相机创建校准模板";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, SystemCalibration.Instance.UplookingCameraVisual);

            visualMatch.SetVisualParam(UplookingCameraChipparam);

            int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return;
            }
            else
            {
                UplookingCameraChipparam = visualMatch.GetVisualParam();

                _systemConfig.SystemCalibrationConfig.UplookingIdentifyChipMatch = UplookingCameraChipparam;

                _systemConfig.SaveConfig();
            }
        }

        private void btnUplookingChip_Click(object sender, EventArgs e)
        {
            UplookingCameraChipparam = _systemConfig.SystemCalibrationConfig.UplookingIdentifyChipMatch;

            SystemCalibration.Instance.IdentificationMoveAsync(EnumCameraType.UplookingCamera, UplookingCameraChipparam);

            CameraWindowGUI.Instance.SelectCamera(1);
            CameraWindowGUI.Instance.ClearGraphicDraw();
        }



        private void btnPositionBond_Click(object sender, EventArgs e)
        {
            //Task.Factory.StartNew(new Action(() =>
            //{
                BondCameraBondPositionparam = _systemConfig.SystemCalibrationConfig.BondIdentifyBondPositionMatch;

                SystemCalibration.Instance.IdentificationMoveAsync(EnumCameraType.BondCamera, BondCameraBondPositionparam);

                CameraWindowGUI.Instance.SelectCamera(0);
                CameraWindowGUI.Instance.ClearGraphicDraw();
            //}));
            
        }

        private void FrmSingleStepRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnGotoChipPos_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "移动到芯片位置？", "提示") == 1)
            {
                if (string.IsNullOrEmpty(teChipPosX.Text) || string.IsNullOrEmpty(teChipPosY.Text) || string.IsNullOrEmpty(teChipPosZ.Text))
                {
                    WarningBox.FormShow("错误", "位置参数无效!", "提示");
                    return;
                }
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teChipPosZ.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teChipPosX.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teChipPosY.Text), EnumCoordSetType.Absolute);             
            }
        }

        private void btnGotoSubmountPos_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "移动到衬底位置？", "提示") == 1)
            {
                if (string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text) || string.IsNullOrEmpty(teSubmountPosZ.Text))
                {
                    WarningBox.FormShow("错误", "位置参数无效!", "提示");
                    return;
                }
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, double.Parse(teSubmountPosZ.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, double.Parse(teSubmountPosX.Text), EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, double.Parse(teSubmountPosY.Text), EnumCoordSetType.Absolute);
            }
        }

        private void btnCreateBlankingPositionTemplate_Click(object sender, EventArgs e)
        {
            BondCameraBlankingPositionparam = _systemConfig.SystemCalibrationConfig.BondIdentifyCuttingPositionMatch;

            string name = "榜头相机创建下料位置";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, SystemCalibration.Instance.BondCameraVisual);

            visualMatch.SetVisualParam(BondCameraBlankingPositionparam);

            int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return;
            }
            else
            {
                BondCameraBlankingPositionparam = visualMatch.GetVisualParam();

                _systemConfig.SystemCalibrationConfig.BondIdentifyCuttingPositionMatch = BondCameraBlankingPositionparam;

                _systemConfig.SaveConfig();
            }
        }

        private void btnPositionBlanking_Click(object sender, EventArgs e)
        {
            //Task.Factory.StartNew(new Action(() =>
            //{
            BondCameraBlankingPositionparam = _systemConfig.SystemCalibrationConfig.BondIdentifyCuttingPositionMatch;

            SystemCalibration.Instance.IdentificationMoveAsync(EnumCameraType.BondCamera, BondCameraBlankingPositionparam);

            CameraWindowGUI.Instance.SelectCamera(0);
            CameraWindowGUI.Instance.ClearGraphicDraw();
            //}));
        }

        private void btnAbandonSubmount_Click(object sender, EventArgs e)
        {
            AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.SubmountPP);
        }

        private void btnAbandonChip_Click(object sender, EventArgs e)
        {
            AbandonMaterialUtility.Instance.Abandon(EnumUsedPP.ChipPP);
        }
    }


}
