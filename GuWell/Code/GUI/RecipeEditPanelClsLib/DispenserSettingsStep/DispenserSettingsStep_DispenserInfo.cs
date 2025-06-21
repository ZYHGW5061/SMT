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
    public partial class DispenserSettingsStep_DispenserInfo : DispenserSettingsStepBasePage
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
        public DispenserSettingsStep_DispenserInfo()
        {
            InitializeComponent();
        }
        public override EnumDefineDispenserSettingsStep CurrentStep
        {
            get
            { return EnumDefineDispenserSettingsStep.DispenserInfo; }
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
                cmbPredispensingMode.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(EnumPredispensingMode)))
                {
                    cmbPredispensingMode.Items.Add(item);
                }
                cmbPredispensingMode.Text = EditRecipe.DispenserSettings.PredispensingMode.ToString();
                sePredispensingTimes.Text= EditRecipe.DispenserSettings.PredispensingCount.ToString();
                sePredispensingIntervelMinutes.Text= EditRecipe.DispenserSettings.PredispensingIntervalMinute.ToString();
                sePredispensingIntervelSeconds.Text= EditRecipe.DispenserSettings.PredispensingIntervalSecond.ToString();
                sesePredispensingOffsetX.Text= EditRecipe.DispenserSettings.PredispensingOffsetXMM.ToString();
                sesePredispensingOffsetY.Text= EditRecipe.DispenserSettings.PredispensingOffsetYMM.ToString();
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
                currentStep = EnumDefineDispenserSettingsStep.DispenserInfo;

                EditRecipe.DispenserSettings.PredispensingMode = (EnumPredispensingMode)Enum.Parse(typeof(EnumPredispensingMode), cmbPredispensingMode.Text);
                EditRecipe.DispenserSettings.PredispensingCount = int.Parse(sePredispensingTimes.Text);
                EditRecipe.DispenserSettings.PredispensingIntervalMinute = int.Parse(sePredispensingIntervelMinutes.Text);
                EditRecipe.DispenserSettings.PredispensingIntervalSecond = int.Parse(sePredispensingIntervelSeconds.Text);
                EditRecipe.DispenserSettings.PredispensingOffsetXMM = float.Parse(sesePredispensingOffsetX.Text);
                EditRecipe.DispenserSettings.PredispensingOffsetYMM = float.Parse(sesePredispensingOffsetY.Text);
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
    }
}
