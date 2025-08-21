using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using JobClsLib;
using PositioningSystemClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCalibrationClsLib;
using UserManagerClsLib;
using VisionGUI;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace ControlPanelClsLib
{
    public partial class FrmSingleStepRun2 : DevExpress.XtraEditors.XtraForm
    {

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

        public List<string> ProdRecipeNameList;
        public BondRecipe curRecipe = null;

        private static string SystemDefaultDirectory = SystemConfiguration.Instance.SystemDefaultDirectory;
        private static string _epoxyApplicationSavePath = string.Format(@"{0}Recipes\EpoxyApplication\", SystemDefaultDirectory);


        MatchIdentificationParam BondCameraSubmountparam = new MatchIdentificationParam();
        MatchIdentificationParam BondCameraChipparam = new MatchIdentificationParam();

        MatchIdentificationParam UplookingCameraSubmountparam = new MatchIdentificationParam();
        MatchIdentificationParam UplookingCameraChipparam = new MatchIdentificationParam();

        MatchIdentificationParam BondCameraBondPositionparam = new MatchIdentificationParam();
        MatchIdentificationParam BondCameraCuttingPositionparam = new MatchIdentificationParam();

        MatchIdentificationParam BondCameraBlankingPositionparam = new MatchIdentificationParam();

        string EpoxyApplicationName;
        protected EpoxyApplication CurEpoxyApplication
        {
            get
            {
                string name = EpoxyApplicationName;
                return BondRecipe.LoadEpoxyApplicationByName(name);
            }
        }

        public FrmSingleStepRun2()
        {
            InitializeComponent();
            ProdRecipeNameList = getRecipeNameList();
            //fillProductList();

            cmbComponentCarrierType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumCarrierType)))
            {
                cmbComponentCarrierType.Items.Add(item);
            }

            comboBox1.Items.Clear();

            foreach (var dispenser in _systemConfig.DispenserSettings)
            {
                comboBox1.Items.Add(dispenser.Name);
            }

            cbPPName.Items.Clear();

            foreach (var PPTool in _systemConfig.PPToolSettings)
            {
                cbPPName.Items.Add(PPTool.Name);
            }

            cbNeedleName.Items.Clear();
            foreach (var item in _systemConfig.ESToolSettings)
            {
                cbNeedleName.Items.Add(item.Name);
            }

            comboBox2.Items.Clear();
            var childs = Directory.GetDirectories(_epoxyApplicationSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                comboBox2.Items.Add(childName);

                var xmlFile = $@"{_epoxyApplicationSavePath}\{childName}\{childName}.xml";
                var ret = XmlSerializeHelper.XmlDeserializeFromFile<EpoxyApplication>(xmlFile, Encoding.UTF8);
            }
            comboBox2.SelectedIndex = -1;


        }

        //获取生产配方列表
        private List<string> getRecipeNameList()
        {
            List<string> list = new List<string>();
            string recipeDir = _systemConfig.SystemDefaultDirectory + @"Recipes\Bonder";
            CommonProcess.EnsureFolderExist(recipeDir);
            CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\Components\", _systemConfig.SystemDefaultDirectory));
            CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\BondPositions\", _systemConfig.SystemDefaultDirectory));
            var recipeFiles = Directory.GetDirectories(recipeDir);
            for (int recipeIndex = 0; recipeIndex < recipeFiles.Length; recipeIndex++)
            {
                var recipeName = Path.GetFileName(recipeFiles[recipeIndex]);
                if (recipeName != "BondPositions" && recipeName != "Components" && recipeName != "EpoxyApplication")
                {
                    list.Add(recipeName);
                }
            }
            return list;
        }







        private void btnCreateSubmountTemplate_Click(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"创建贴片位置识别", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            BondCameraSubmountparam = _systemConfig.SystemCalibrationConfig.BondIdentifySubmountMatch;

            string name = "榜头相机创建贴片位置";
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
            LogRecorder.RecordUserOperationLog($"贴片位置识别", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            //Task.Factory.StartNew(new Action(() =>
            //{
            BondCameraSubmountparam = _systemConfig.SystemCalibrationConfig.BondIdentifySubmountMatch;

            XYZTCoordinateConfig offset = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, BondCameraSubmountparam);

            if (offset != null)
            {
                if (_positioningSystem.BondXYUnionMovetoStageCoor(offset.X, offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                {
                    WarningBox.FormShow("识别成功！", "完成识别，相机对准贴片位置", "提示");
                }
                else
                {

                }
                
            }
            else
            {
                WarningBox.FormShow("识别失败！", "完成", "提示");
            }
            

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


            XYZTCoordinateConfig offset = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, BondCameraChipparam);

            if (offset != null)
            {
                if (_positioningSystem.BondXYUnionMovetoStageCoor(offset.X, offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                {
                    WarningBox.FormShow("识别成功！", "完成识别，相机对准贴片位置", "提示");
                }
                else
                {

                }

            }
            else
            {
                WarningBox.FormShow("识别失败！", "完成", "提示");
            }


            CameraWindowGUI.Instance.SelectCamera(0);
            CameraWindowGUI.Instance.ClearGraphicDraw();

        }

        private void btnDispense_Click(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"点胶", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            string dispensername = comboBox1.Text;
            EpoxyApplicationName = comboBox2.Text;

            DispenserSettings curDispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == dispensername);
            if(curDispenser != null && CurEpoxyApplication != null)
            {
                var BondPositionSystemPosAfterVisionCalibrationX = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                var BondPositionSystemPosAfterVisionCalibrationY = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);

                if (curDispenser.DispensingMode == EnumDispensingMode.Dipping)
                {
                    var DippingX = (float)curDispenser.EpoxtToDippingglueCoordinate.X;
                    var DippingY = (float)curDispenser.EpoxtToDippingglueCoordinate.Y;

                    if (_positioningSystem.BondXYUnionMovetoStageCoor(DippingX, DippingY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        IOUtilityHelper.Instance.DownDispenserCylinder();
                        var Z = (float)SystemConfiguration.Instance.PositioningConfig.EpoxtToDippingglueCoordinate.Z;
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                        {
                            Thread.Sleep(50);
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {

                            }
                            else
                            {
                                IOUtilityHelper.Instance.UpDispenserCylinder();
                                LogRecorder.RecordLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                                return;
                            }
                        }


                    }
                    else
                    {
                        IOUtilityHelper.Instance.UpDispenserCylinder();
                        LogRecorder.RecordLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                        return;
                    }
                }



                var despenserAndBondCameraOffsetX = float.IsNaN(curDispenser.DispenserPosOffsetXWithBondCamera)
                    ? -_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X : curDispenser.DispenserPosOffsetXWithBondCamera;
                var despenserAndBondCameraOffsetY = float.IsNaN(curDispenser.DispenserPosOffsetYWithBondCamera)
                    ? _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y : curDispenser.DispenserPosOffsetYWithBondCamera;
                var X = BondPositionSystemPosAfterVisionCalibrationX + despenserAndBondCameraOffsetX;
                var Y = BondPositionSystemPosAfterVisionCalibrationY + despenserAndBondCameraOffsetY;
                if (_positioningSystem.BondXYUnionMovetoSystemCoor(X, Y, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                {
                    IOUtilityHelper.Instance.DownDispenserCylinder();
                    var Z = curDispenser.DispenserSystemPosZMM;
                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondZ, Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                    {
                        if (curDispenser.DispensingMode != EnumDispensingMode.Dipping)
                        {
                            if (CurEpoxyApplication.DispensePattern == EnumDispensePattern.Point)
                            {
                                DispenserUtility.Instance.ExecutePointRecipe(CurEpoxyApplication.DispenserRecipeName);
                            }
                            else
                            {
                                DispenserUtility.Instance.DrawCross(CurEpoxyApplication.DispensePatternWidthMM, CurEpoxyApplication.DispensePatternHeightMM);
                            }
                        }
                        else
                        {
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, SystemConfiguration.Instance.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {

                            }
                            else
                            {
                                IOUtilityHelper.Instance.UpDispenserCylinder();
                                LogRecorder.RecordLog(EnumLogContentType.Error, "StepAction_Dispense,Fail.");
                                return;
                            }

                        }

                    }

                }

            }
            else
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "DispenserSettings is null ro CurEpoxyApplication is null,Fail.");
            }



        }

        private void btnCreateChipTemplate_Click_1(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"创建芯片识别", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            EnumCarrierType CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbComponentCarrierType.Text);
            if(CarrierType == EnumCarrierType.WafflePack)
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
            else
            {
                BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;

                string name = "晶圆相机创建芯片模板";
                string title = "";
                VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
                visualMatch.InitVisualControl(CameraWindowGUI.Instance, SystemCalibration.Instance.WaferCameraVisual);

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
            
        }

        private void btnPositionChip_Click_1(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"识别芯片", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            EnumCarrierType CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbComponentCarrierType.Text);
            if (CarrierType == EnumCarrierType.WafflePack)
            {
                BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;
                XYZTCoordinateConfig offset = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.BondCamera, BondCameraChipparam);

                if (offset != null)
                {
                    if (_positioningSystem.BondXYUnionMovetoStageCoor(offset.X, offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                    {
                        WarningBox.FormShow("识别成功！", "完成识别，相机对准贴片位置", "提示");
                    }
                    else
                    {

                    }

                }
                else
                {
                    WarningBox.FormShow("识别失败！", "完成", "提示");
                }
            }
            else if(CarrierType == EnumCarrierType.WaferWafflePack)
            {
                BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;
                XYZTCoordinateConfig offset = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, BondCameraChipparam);

                if (offset != null)
                {
                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, offset.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                    || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, offset.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                    {
                        //移至中心失败
                        LogRecorder.RecordLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                        return;
                    }
                    else
                    {

                    }

                }
                else
                {
                    WarningBox.FormShow("识别失败！", "完成", "提示");
                }
            }
            else if (CarrierType == EnumCarrierType.Wafer)
            {
                BondCameraChipparam = _systemConfig.SystemCalibrationConfig.BondIdentifyChipMatch;
                XYZTCoordinateConfig offset = SystemCalibration.Instance.IdentificationAsync2(EnumCameraType.WaferCamera, BondCameraChipparam);
                if (offset != null)
                {
                    if (_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, offset.X, EnumCoordSetType.Relative) != StageMotionResult.Success
                                    || _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, offset.Y, EnumCoordSetType.Relative) != StageMotionResult.Success)
                    {
                        //移至中心失败
                        LogRecorder.RecordLog(EnumLogContentType.Error, "StepAction_PositionComponent-移至中心失败.");
                        return;
                    }
                    else
                    {

                    }

                }
                else
                {
                    WarningBox.FormShow("识别失败！", "完成", "提示");
                }
            }

            CameraWindowGUI.Instance.SelectCamera(0);
            CameraWindowGUI.Instance.ClearGraphicDraw();
        }

        private void btnPickChip_Click(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"拾取芯片", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            EnumCarrierType CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbComponentCarrierType.Text);

            if (WarningBox.FormShow("条件确认", "确认当前芯片的吸取位置在相机视野的中心？", "提示") == 1)
            {
                

                try
                {
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == cbPPName.Text);
                    if (CarrierType == EnumCarrierType.WafflePack)
                    {
                        //吸嘴移动到芯片中心上方
                        float targetA = 0;
                        var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        offset = pptool.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            offset.X = usedPPandBondCameraOffsetX;
                            offset.Y = usedPPandBondCameraOffsetY;
                        }
                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //芯片吸嘴T复位
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.BondXYUnionMovetoStageCoor(offset.X,offset.Y, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {

                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            PPWorkParameters pp = new PPWorkParameters();
                            pp.IsUseNeedle = false;
                            pp.UsedPP = EnumUsedPP.ChipPP;

                            pp.PickupStress = float.Parse(teChipPPPress.Text);

                            pp.SlowSpeedBeforePickup = float.Parse(teSlowSpeedBeforePickup.Text);
                            pp .SlowTravelBeforePickupMM = float.Parse(teSlowTravelBeforePickupMM.Text);

                            pp.SlowSpeedAfterPickup = float.Parse(teSlowSpeedAfterPickup.Text);
                            pp.SlowTravelAfterPickupMM = float.Parse(teSlowTravelAfterPickupMM.Text);
                            pp.UpDistanceMMAfterPicked = 10;

                            pp.DelayMSForVaccum = float.Parse(teVaccumDelayMS.Text);


                            if (pptool != null)
                            {
                                var systemPos = float.Parse(teChipPPPickPos.Text);
                                pp.PPToolZero = pptool.AltimetryOnMark;
                                pp.WorkHeight = (float)(systemPos);
                            }
                            else
                            {
                                var systemPos = float.Parse(teChipPPPickPos.Text);
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                pp.WorkHeight = (float)(systemPos);
                            }

                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //double CurrA = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);

                                _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, -targetA, EnumCoordSetType.Relative);



                                //CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();
                                //PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
                                //PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);
                                //PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

                                //double angle0 = CurrA;
                                //double angle = CurrA+ targetA;
                                //PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);
                                //ProductExecutor.Instance.CompensateXAfterPickupChip = point3.X;
                                //ProductExecutor.Instance.CompensateYAfterPickupChip = point3.Y;
                                LogRecorder.RecordLog(EnumLogContentType.Info, "StepAction_PickUpChip-End.");
                            }
                            else
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return;
                            }
                        }
                        else
                        {
                            LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return;
                        }
                    }
                    else if(CarrierType == EnumCarrierType.Wafer)
                    {
                        var usedESTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == cbNeedleName.Text);
                        if (usedESTool != null)
                        {
                            var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                            offset = pptool.PP1AndBondCameraOffset;
                            if (pptool != null)
                            {
                                var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                                var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                                offset.X = usedPPandBondCameraOffsetX;
                                offset.Y = usedPPandBondCameraOffsetY;
                            }
                            var offsetBCAndWC = _systemConfig.PositioningConfig.WaferCameraOrigion;
                            var offsetBCAndWC2 = usedESTool.BondIdentifyNeedleCenter;
                            if (_positioningSystem.BondZMovetoSafeLocation()
                            //顶针移动到零点
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, usedESTool.NeedleZeorPosition, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //物料中心移动到顶针上方
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, usedESTool.NeedleCenter.X, EnumCoordSetType.Relative) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, -usedESTool.NeedleCenter.Y, EnumCoordSetType.Relative) == StageMotionResult.Success
                            //芯片吸嘴物料中心上方
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC.X - usedESTool.NeedleCenter.X + curDealBP.chipPositionCompensation.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC.Y - usedESTool.NeedleCenter.Y + curDealBP.chipPositionCompensation.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + offsetBCAndWC2.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + offsetBCAndWC2.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //顶针座升起
                            && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, float.Parse(textEdit1.Text), EnumCoordSetType.Absolute) == StageMotionResult.Success
                            //拾取芯片
                            //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            && _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                            )
                            {
                                //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                                PPWorkParameters pp = new PPWorkParameters();
                                pp.IsUseNeedle = true;

                                pp.NeedleUpHeight = float.Parse(teNeedleUpHeight.Text);
                                pp.NeedleSpeed = float.Parse(teNeedleSpeed.Text);

                                pp.UsedPP = EnumUsedPP.ChipPP;

                                pp.PickupStress = float.Parse(teChipPPPress.Text);

                                pp.SlowSpeedBeforePickup = float.Parse(teSlowSpeedBeforePickup.Text);
                                pp.SlowTravelBeforePickupMM = float.Parse(teSlowTravelBeforePickupMM.Text);

                                pp.SlowSpeedAfterPickup = float.Parse(teSlowSpeedAfterPickup.Text);
                                pp.SlowTravelAfterPickupMM = float.Parse(teSlowTravelAfterPickupMM.Text);
                                pp.UpDistanceMMAfterPicked = 10;

                                pp.DelayMSForVaccum = float.Parse(teVaccumDelayMS.Text);


                                if (pptool != null)
                                {
                                    var systemPos = float.Parse(teChipPPPickPos.Text);
                                    pp.PPToolZero = pptool.AltimetryOnMark;
                                    pp.WorkHeight = (float)(systemPos);
                                }
                                else
                                {
                                    var systemPos = float.Parse(teChipPPPickPos.Text);
                                    pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                    pp.WorkHeight = (float)(systemPos);
                                }

                                IOUtilityClsLib.IOUtilityHelper.Instance.OpenESBaseVaccum();


                                if (PPUtility.Instance.PickViaSystemCoor(pp))
                                {
                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);
                                    _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisTheta, 0, EnumCoordSetType.Relative);
                                }
                                else
                                {
                                    IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                    LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                                    WarningBox.FormShow("错误", "拾取芯片失败！");
                                    return;
                                }
                            }
                            else
                            {
                                IOUtilityClsLib.IOUtilityHelper.Instance.CloseESBaseVaccum();
                                LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                                WarningBox.FormShow("错误", "拾取芯片失败！");
                                return;
                            }

                        }
                        else
                        {
                            LogRecorder.RecordLog(EnumLogContentType.Error, "芯片绑定的顶针工具无效！");
                            WarningBox.FormShow("错误", "拾取芯片失败！");
                            return;
                        }

                    }
                    else if(CarrierType == EnumCarrierType.WaferWafflePack)
                    {
                        var ppSystemOffset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                        ppSystemOffset = pptool.PP1AndBondCameraOffset;
                        if (pptool != null)
                        {
                            var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                            var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                            ppSystemOffset.X = usedPPandBondCameraOffsetX;
                            ppSystemOffset.Y = usedPPandBondCameraOffsetY;
                        }
                        var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;



                        if (_positioningSystem.BondZMovetoSafeLocation()
                        //芯片吸嘴物料中心上方
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X+ ProductExecutor.Instance.OffsetBeforePickupChip.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y- ProductExecutor.Instance.OffsetBeforePickupChip.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, ppSystemOffset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, ppSystemOffset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        ////拾取芯片
                        //&& _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 0, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        )
                        {
                            //XY联动移动到物料上方
                            EnumStageAxis[] multiAxis = new EnumStageAxis[3];
                            multiAxis[0] = EnumStageAxis.BondX;
                            multiAxis[1] = EnumStageAxis.BondY;
                            //multiAxis[2] = EnumStageAxis.ChipPPT;
                            multiAxis[2] = pptool.StageAxisTheta;
                            double[] targets = new double[3];
                            targets[0] = ppSystemOffset.X + bondcamera2wafercamera.X;
                            targets[1] = ppSystemOffset.Y + bondcamera2wafercamera.Y;
                            targets[2] = 0;
                            StageMotionResult result = _positioningSystem.MoveAixsToStageCoord(multiAxis, targets, EnumCoordSetType.Absolute);
                            if (result == StageMotionResult.Success)
                            {

                            }
                            else
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                                return;
                            }

                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,BondXTarget:{ppSystemOffset.X + bondcamera2wafercamera.X}");
                            LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,BondXCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX)}");
                            //拾取芯片，TBD - 此处的高度应该用吸嘴工具和物料参数计算
                            PPWorkParameters pp = new PPWorkParameters();
                            pp.IsUseNeedle = false;

                            pp.UsedPP = EnumUsedPP.ChipPP;

                            pp.PickupStress = float.Parse(teChipPPPress.Text);

                            pp.SlowSpeedBeforePickup = float.Parse(teSlowSpeedBeforePickup.Text);
                            pp.SlowTravelBeforePickupMM = float.Parse(teSlowTravelBeforePickupMM.Text);

                            pp.SlowSpeedAfterPickup = float.Parse(teSlowSpeedAfterPickup.Text);
                            pp.SlowTravelAfterPickupMM = float.Parse(teSlowTravelAfterPickupMM.Text);
                            pp.UpDistanceMMAfterPicked = 10;

                            pp.DelayMSForVaccum = float.Parse(teVaccumDelayMS.Text);

                            //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.RelatedPPToolName);
                            if (pptool != null)
                            {
                                //吸嘴工具原点
                                var systemPos = float.Parse(teChipPPPickPos.Text);
                                pp.PPToolZero = pptool.AltimetryOnMark;
                                pp.WorkHeight = (float)(systemPos);

                            }
                            else
                            {
                                //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                                var systemPos = float.Parse(teChipPPPickPos.Text);
                                pp.WorkHeight = (float)(systemPos);
                                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
                                //pp.WorkHeight = ppWorkSystemPos;
                            }


                            //var materialOrigionA = CurChipParam.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault().OrigionAngle;
                            //var targetA = ProductExecutor.Instance.OffsetBeforePickupChip.Theta - materialOrigionA;
                            //LogRecorder.RecordLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-visionAngle:{ProductExecutor.Instance.OffsetBeforePickupChip.Theta}");
                            //LogRecorder.RecordLog(EnumLogContentType.Error, $"StepAction_PickUpChipWithRotate-targetAngle:{targetA}");
                            if (PPUtility.Instance.PickViaSystemCoor(pp))
                            {
                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorBefore:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -targetA, EnumCoordSetType.Relative);
                                //LogRecorder.RecordLog(EnumLogContentType.Debug, $"StepAction_PickUpChipWithRotate,TCoorAfter:{_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT)}");
                            }
                            else
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                                return;
                            }
                        }
                        else
                        {
                            LogRecorder.RecordLog(EnumLogContentType.Error, "拾取芯片失败！");
                            return;
                        }
                    }
                    WarningBox.FormShow("动作结束！", "芯片拾取完成！", "提示");
                }
                catch (Exception ex)
                {
                    //CloseWaitDialog();
                    WarningBox.FormShow("流程异常！", "芯片拾取失败！", "提示");
                }
            }

        }

        private void btnPlaceChip_Click(object sender, EventArgs e)
        {
            LogRecorder.RecordUserOperationLog($"放下芯片", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

            //使用的吸嘴工具
            var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == cbPPName.Text);

            PPWorkParameters pp = new PPWorkParameters();
            pp.IsUseNeedle = false;

            pp.UsedPP = EnumUsedPP.ChipPP;

            pp.PickupStress = float.Parse(teChipPPPress.Text);

            pp.SlowSpeedBeforePickup = float.Parse(teSlowSpeedBeforePickup.Text);
            pp.SlowTravelBeforePickupMM = float.Parse(teSlowTravelBeforePickupMM.Text);

            pp.SlowSpeedAfterPickup = float.Parse(teSlowSpeedAfterPickup.Text);
            pp.SlowTravelAfterPickupMM = float.Parse(teSlowTravelAfterPickupMM.Text);
            pp.UpDistanceMMAfterPicked = 10;

            pp.DelayMSForPlace = float.Parse(sePlaceDelayMs.Text);
            pp.BreakVaccumTimespanMS = float.Parse(seBreakVaccumTimespanMs.Text);

            //var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == CurChipParam.RelatedPPToolName);
            if (pptool != null)
            {
                //吸嘴工具原点
                var systemPos = float.Parse(teChipPPPlacePos.Text);
                pp.WorkHeight = (float)(systemPos);
                pp.PPToolZero = pptool.AltimetryOnMark;

            }
            else
            {
                //TBD此处采用系统保存的吸嘴和顶针系统的位置数据
                var systemPos = float.Parse(teChipPPPickPos.Text);
                pp.WorkHeight = (float)(systemPos);
                pp.PPToolZero = (float)_systemConfig.PositioningConfig.TrackChipPPOrigion.Z;
            }

            Thread.Sleep(_systemConfig.TuningTimeMS);



            if (PPUtility.Instance.PlaceViaSystemCoor(pp, null, null, true))
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "StepAction_OnlyBondChip-End.");
            }
            else
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.RecordLog(EnumLogContentType.Error, "芯片贴装失败！");
                WarningBox.FormShow("错误", "芯片贴装失败！");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "吸嘴是否已经移动到吸取芯片高度？", "提示") == 1)
                {
                    string PPtoolname = cbPPName.Text;

                    float ChipTopplateHigherValueThanMarkTopplate = (float)_positioningSystem.ReadChipPPSystemPosition(PPtoolname);

                    teChipPPPickPos.Text = ChipTopplateHigherValueThanMarkTopplate.ToString();

                    WarningBox.FormShow("成功！", "吸取芯片高度示教完成！", "提示");

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置单步生产吸取芯片高度:{ChipTopplateHigherValueThanMarkTopplate}", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }



            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.ProductionLog(EnumLogContentType.Error, "吸取芯片高度示教失败！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (WarningBox.FormShow("动作确认", "吸嘴是否已经移动到放下芯片高度？", "提示") == 1)
                {
                    string PPtoolname = cbPPName.Text;

                    float ChipTopplateHigherValueThanMarkTopplate = (float)_positioningSystem.ReadChipPPSystemPosition(PPtoolname);

                    teChipPPPlacePos.Text = ChipTopplateHigherValueThanMarkTopplate.ToString();

                    WarningBox.FormShow("成功！", "放下芯片高度示教完成！", "提示");

                    LogRecorder.RecordUserOperationLog($"吸嘴:{PPtoolname}设置单步生产吸芯片高度:{ChipTopplateHigherValueThanMarkTopplate}", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                }



            }
            catch (Exception ex)
            {
                //_systemLogger.AddErrorContent("Errors occured while saving system Configure", ex);
                LogRecorder.ProductionLog(EnumLogContentType.Error, "放下芯片高度示教失败！");
            }
        }
    }
}