using CameraControllerClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
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

namespace RecipeEditPanelClsLib
{
    public partial class EpoxyApplicationStep_EpoxySettings : EpoxyApplicationStepBasePage
    {
        private CameraManager _cameraManager
        {
            get { return CameraManager.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        public EpoxyApplicationStep_EpoxySettings()
        {
            InitializeComponent();
        }
        public override EnumDefineEpoxyApplicationStep CurrentStep
        {
            get
            { return EnumDefineEpoxyApplicationStep.ExpoxySettings; }
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
                    throw new Exception("Recipe is null when execute LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                {
                    //ctrlLight1.CurrentLightType = EnumLightSourceType.SubstrateDirectField;
                    //ctrlLight2.CurrentLightType = EnumLightSourceType.SubstrateRingField;
                }
                else if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                {
                    //ctrlLight1.CurrentLightType = EnumLightSourceType.WaferDirectField;
                    //ctrlLight2.CurrentLightType = EnumLightSourceType.WaferRingField;
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineEpoxyApplicationStep currentStep)
        {
            try
            {
                currentStep = EnumDefineEpoxyApplicationStep.ExpoxySettings;

                var posX = (float)(_cameraManager.CurrentCameraType == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX)
                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX));
                var posY = (float)(_cameraManager.CurrentCameraType == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY)
                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY));
                FirstComponentPosition = new PointF(posX, posY);
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
        public void SetStepInfo(string info)
        {
            labelStepInfo.Text = info;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
