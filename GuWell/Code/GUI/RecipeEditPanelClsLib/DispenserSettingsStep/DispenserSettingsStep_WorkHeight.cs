using CameraControllerClsLib;
using CommonPanelClsLib;
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
    public partial class DispenserSettingsStep_WorkHeight : DispenserSettingsStepBasePage
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
        public DispenserSettingsStep_WorkHeight()
        {
            InitializeComponent();
        }
        public override EnumDefineDispenserSettingsStep CurrentStep
        {
            get
            { return EnumDefineDispenserSettingsStep.DispenserWorkHeight; }
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
                //if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                //{
                //    ctrlLight1.CurrentLightType = EnumLightSourceType.SubstrateDirectField;
                //    ctrlLight2.CurrentLightType = EnumLightSourceType.SubstrateRingField;
                //}
                //else if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                //{
                //    ctrlLight1.CurrentLightType = EnumLightSourceType.WaferDirectField;
                //    ctrlLight2.CurrentLightType = EnumLightSourceType.WaferRingField;
                //}
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineDispenserSettingsStep currentStep)
        {
            try
            {
                currentStep = EnumDefineDispenserSettingsStep.DispenserWorkHeight;

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


        private void btnMeasureHeight_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将记录基板划胶的高度位置！", "确认已定位到合适的划胶位置？", "提示") == 1)
            {
                DispenserWorkHeight = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);
            }
        }

        private void btnDispenserUpDown_Click(object sender, EventArgs e)
        {

        }

        private void btnDispenseGlue_Click(object sender, EventArgs e)
        {

        }

        private void btnUpDispenser_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将升起!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.UpDispenserCylinder();
            }
        }

        private void btnDownDispenser_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将下降!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.DownDispenserCylinder();
            }
        }
    }
}
