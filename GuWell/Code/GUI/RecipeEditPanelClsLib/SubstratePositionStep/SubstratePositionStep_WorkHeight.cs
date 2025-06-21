using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using HardwareManagerClsLib;
using LaserSensorControllerClsLib;
using LaserSensorManagerClsLib;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class SubstratePositionStep_WorkHeight : SubstratePositionStepBasePage
    {
        private PackagedLightController _packagedLightController;
        public SubstratePositionStep_WorkHeight()
        {
            InitializeComponent();
        }
        public override EnumDefineSetupRecipeSubstratePositionStep CurrentStep 
        { 
            get 
            { return EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight; } 
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
        public override void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeSubstratePositionStep currentStep)
        {
            try
            {
                currentStep = EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight;
                //PPWorkHeight = 0f;
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

        private void btnLaserMeasureHeight_Click(object sender, EventArgs e)
        {

            if (WarningBox.FormShow("即将自动测量基板工作高度。", "确认榜头相机已定位到基板上方！", "提示") == 1)
            {
                try
                {
                    CreateWaitDialog();
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

                    //根据校准数据及当前的激光测高仪读数计算吸嘴工作高度
                    var curBondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z;
                    var offsetMeasureZ = curLaserMeasureH - _systemConfig.PositioningConfig.TrackLaserSensorZ;
                    var componentZ = offsetMeasureZ - offsetZ;
                    //PPWorkHeight = (float)(_systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z - offsetMeasureZ + offsetZ);
                    //转为系统坐标系存储
                    //PPWorkHeight = _positioningSystem.ConvertStagePosToSystemPos(EnumStageAxis.BondZ, PPWorkHeight);
                    SubstrateTopplateHigherValueThanMarkTopplate = (float)-componentZ;
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                        EnumCoordSetType.Relative);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                        EnumCoordSetType.Relative);
                    CloseWaitDialog();
                    WarningBox.FormShow("成功！", "测高完成！", "提示");
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, "SubmountHeightProgram-LaserMeasureHeight,Error.", ex);
                    WarningBox.FormShow("发生异常！", "测高失败！", "提示");
                }
                finally
                {
                    CloseWaitDialog();
                }



            }

        }

        private void btnAutoFocus_Click(object sender, EventArgs e)
        {

        }
    }
}
