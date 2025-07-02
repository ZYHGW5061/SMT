using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using HardwareManagerClsLib;
using LaserSensorControllerClsLib;
using LightSourceCtrlPanelLib;
using PositioningSystemClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class ComponentPositionStep_WorkHeight : ComponentPositionStepBasePage
    {
        private PackagedLightController _packagedLightController;
        public ComponentPositionStep_WorkHeight()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        private ILaserSensorController _laserSensor
        {
            get { return HardwareManager.Instance.LaserSensor; }
        }
        public override EnumDefineSetupRecipeComponentPositionStep CurrentStep 
        { 
            get 
            { return EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight; } 
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        ///  加载前一步定义的Recipe对象
        /// </summary>
        /// <param name="recipe"></param>
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    throw new Exception("Recipe is null when execute ComponentPositionStep_SizeLUCorner LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                {
                    _packagedLightController = new PackagedLightController(HardwareConfiguration.Instance.IsBondDirectLightMultiColor, HardwareConfiguration.Instance.IsBondRingLightMultiColor);
                    if (HardwareConfiguration.Instance.IsBondDirectLightMultiColor)
                    {
                        _packagedLightController.DirectLightColor = EnumLightColor.Red;
                        _packagedLightController.CurrentDirectLightType = EnumLightSourceType.BondDirectField;
                    }
                    else
                    {
                        _packagedLightController.CurrentDirectLightType = EnumLightSourceType.BondDirectField;
                    }
                    if (HardwareConfiguration.Instance.IsBondRingLightMultiColor)
                    {
                        _packagedLightController.RingLightColor = EnumLightColor.Red;
                        _packagedLightController.CurrentRingLightType = EnumLightSourceType.BondRingField;
                    }
                    else
                    {
                        _packagedLightController.CurrentRingLightType = EnumLightSourceType.BondRingField;
                    }

                }
                else if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                {

                    _packagedLightController = new PackagedLightController(HardwareConfiguration.Instance.IsWaferDirectLightMultiColor, HardwareConfiguration.Instance.IsWaferRingLightMultiColor);
                    if (HardwareConfiguration.Instance.IsWaferDirectLightMultiColor)
                    {
                        _packagedLightController.DirectLightColor = EnumLightColor.Red;
                        _packagedLightController.CurrentDirectLightType = EnumLightSourceType.WaferDirectField;
                    }
                    else
                    {
                        _packagedLightController.CurrentDirectLightType = EnumLightSourceType.WaferDirectField;
                    }
                    if (HardwareConfiguration.Instance.IsWaferRingLightMultiColor)
                    {
                        _packagedLightController.RingLightColor = EnumLightColor.Red;
                        _packagedLightController.CurrentRingLightType = EnumLightSourceType.WaferRingField;
                    }
                    else
                    {
                        _packagedLightController.CurrentRingLightType = EnumLightSourceType.WaferRingField;
                    }

                }
                this.panelControl2.Controls.Clear();
                _packagedLightController.Dock = DockStyle.Fill;
                this.panelControl2.Controls.Add(_packagedLightController);
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeComponentPositionStep currentStep)
        {
            try
            {
                currentStep = EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight;
                finished = true;
            }
            finally
            {
                if (NotifySingleStepDefineFinished != null)
                {
                    base.NotifySingleStepDefineFinished(base.EditRecipe, new int[] { 8, 0 }, new int[] { 2, 0 });
                }
            }
        }

        private void btnMeasureHeight_Click(object sender, EventArgs e)
        {

        }

        private void btnAutoFocus_Click(object sender, EventArgs e)
        {

        }

        private void btnLaserMeasureHeight_Click(object sender, EventArgs e)
        {
            if (EditRecipe.CurrentComponent.CarrierType == EnumCarrierType.WafflePack)
            {
                //if (WarningBox.FormShow("使用激光测距仪自动测高？", "确认相机已定位到拾取位置！", "提示") == 1)
                //{
                //    try
                //    {
                //        CreateWaitDialog();
                //        //将激光测高仪移动到当前相机位置
                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                //        EnumCoordSetType.Relative);
                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                //            EnumCoordSetType.Relative);
                //        //读取激光测高仪读数
                //        var curLaserMeasureH = _laserSensor.ReadDistance() / 10000;
                //        //根据校准数据及当前的激光测高仪读数计算吸嘴工作高度
                //        var curBondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                //        var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z;
                //        var offsetMeasureZ = curLaserMeasureH - _systemConfig.PositioningConfig.TrackLaserSensorZ;
                //        var componentZ = offsetMeasureZ - offsetZ;
                //        //此处应该是根据关联的吸嘴工具获取参数
                //        //PPWorkHeight = (float)(_systemConfig.PositioningConfig.TrackChipPPOrigion.Z - offsetMeasureZ + offsetZ);
                //        ChipTopplateHigherValueThanMarkTopplate = (float)-componentZ;
                //        //ReadChipPPSystemPosition-TBD
                //        //PPWorkHeight = _positioningSystem.ConvertStagePosToSystemPos(EnumStageAxis.BondZ, PPWorkHeight);

                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                //            EnumCoordSetType.Relative);
                //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                //            EnumCoordSetType.Relative);
                //        CloseWaitDialog();
                //        WarningBox.FormShow("成功！", "测高完成！", "提示");
                //    }
                //    catch (Exception ex)
                //    {
                //        LogRecorder.RecordLog(EnumLogContentType.Error, "ComponentHeightProgram-LaserMeasureHeight,Error.", ex);
                //        WarningBox.FormShow("异常！", "测高失败！", "提示");
                //    }
                //    finally
                //    {
                //        CloseWaitDialog();
                //    }

                    
                //}
                if (WarningBox.FormShow("工作高度确认!", "确认吸嘴已到合适拾取高度？", "提示") == 1)
                {
                    ChipTopplateHigherValueThanMarkTopplate = (float)_positioningSystem.ReadChipPPSystemPosition(EditRecipe.CurrentComponent.PPSettings.PPtoolName);
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == EditRecipe.CurrentComponent.PPSettings.PPtoolName);
                    if (pptool != null)
                    {
                        if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                        {
                            if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                            {
                                _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPFreeZ, EnumCoordSetType.Absolute);
                            }
                        }
                    }
                    WarningBox.FormShow("成功！", "工作高度确认完成！", "提示");
                }
            }
            else if(EditRecipe.CurrentComponent.CarrierType == EnumCarrierType.Wafer)
            {
                if (WarningBox.FormShow("工作高度确认", "确认顶针座和吸嘴已到合适高度？", "提示") == 1)
                {
                    ESWorkHeight = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ESZ);
                    ChipTopplateHigherValueThanMarkTopplate = (float)_positioningSystem.ReadChipPPSystemPosition(EditRecipe.CurrentComponent.PPSettings.PPtoolName);
                    WarningBox.FormShow("成功！", "工作高度确认完成！", "提示");
                }
            }
            else if (EditRecipe.CurrentComponent.CarrierType == EnumCarrierType.WaferWafflePack)
            {
                if (WarningBox.FormShow("工作高度确认!", "确认吸嘴已到合适拾取高度？", "提示") == 1)
                {
                    ChipTopplateHigherValueThanMarkTopplate = (float)_positioningSystem.ReadChipPPSystemPosition(EditRecipe.CurrentComponent.PPSettings.PPtoolName);
                    WarningBox.FormShow("成功！", "工作高度确认完成！", "提示");
                }
            }
        }

        private void btnGotoPPCenter_Click(object sender, EventArgs e)
        {
            if (EditRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            {
                if (WarningBox.FormShow("吸嘴即将移动到榜头相机中心!", "请确认榜头处于安全高位？", "提示") == 1)
                {
                    var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                    var BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    var BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    XYZTCoordinateConfig bondcamera2wafercamera = new XYZTCoordinateConfig() { X = BondX, Y = BondY};
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == EditRecipe.CurrentComponent.PPSettings.PPtoolName);
                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        offset.X = usedPPandBondCameraOffsetX;
                        offset.Y = usedPPandBondCameraOffsetY;
                    }
                    //芯片吸嘴物料中心上方
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute);

                    if(pptool != null)
                    {
                        if(pptool.EnumPPtool == EnumPPtool.PPtool2)
                        {
                            if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                            {
                                _positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute);
                            }
                        }
                    }
                   

                }

            }
            else if (EditRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            {

                if (WarningBox.FormShow("吸嘴即将移动到晶圆相机中心!", "请确认榜头处于安全高位？", "提示") == 1)
                {
                    var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;
                    var bondcamera2wafercamera = _systemConfig.PositioningConfig.WaferCameraOrigion;
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == EditRecipe.CurrentComponent.PPSettings.PPtoolName);
                    if (pptool != null)
                    {
                        var usedPPandBondCameraOffsetX = pptool.LookuptoPPOrigion.X - _systemConfig.PositioningConfig.LookupCameraOrigion.X;
                        var usedPPandBondCameraOffsetY = pptool.LookuptoPPOrigion.Y - _systemConfig.PositioningConfig.LookupCameraOrigion.Y;
                        offset.X = usedPPandBondCameraOffsetX;
                        offset.Y = usedPPandBondCameraOffsetY;
                    }
                    //芯片吸嘴物料中心上方
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, offset.X + bondcamera2wafercamera.X, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, offset.Y + bondcamera2wafercamera.Y, EnumCoordSetType.Absolute);
                }
            }

            
        }
    }
}
