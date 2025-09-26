using GlobalDataDefineClsLib;
using MathHelperClsLib;
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
using VisionGUI;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeStep_ComponentPPSettings : RecipeStepBase
    {
        private float _submountRotateAngle;
        public RecipeStep_ComponentPPSettings()
        {
            InitializeComponent();
            var image = global::RecipeEditPanelClsLib.Properties.Resources.pickupsettings;
            Bitmap resizedImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
            this.pictureBox1.Image = resizedImage;
        }
        /// <summary>
        /// 加载Recipe内容
        /// </summary>
        /// <param name="recipe"></param>
        public override void LoadEditedRecipe(BondRecipe recipe) 
        { 
            _editRecipe = recipe;
            sePickupDelayMs.Text = _editRecipe.CurrentComponent.PPSettings.DelayMSForVaccum.ToString();
            sePickupStressMM.Text = _editRecipe.CurrentComponent.PPSettings.PickupStress.ToString();

            seNeedleUpHeight.Text = _editRecipe.CurrentComponent.PPSettings.NeedleUpHeight.ToString();
            seNeedleSpeed.Text = _editRecipe.CurrentComponent.PPSettings.NeedleSpeed.ToString();

            seBeforePickupSlowTravelMM.Text = _editRecipe.CurrentComponent.PPSettings.SlowTravelBeforePickupMM.ToString();
            seBeforePickupSlowSpeed.Text = _editRecipe.CurrentComponent.PPSettings.SlowSpeedBeforePickup.ToString();

            seAfterPickupSlowTravelMM.Text = _editRecipe.CurrentComponent.PPSettings.SlowTravelAfterPickupMM.ToString();
            seAfterPickupSlowSpeed.Text = _editRecipe.CurrentComponent.PPSettings.SlowSpeedAfterPickup.ToString();

            sePlaceDelayMs.Text = _editRecipe.CurrentComponent.PPSettings.DelayMSForPlace.ToString();
            sePlaceStress.Text = _editRecipe.CurrentComponent.PPSettings.PlaceStress.ToString();

            seBreakVaccumTimespanMs.Text = _editRecipe.CurrentComponent.PPSettings.BreakVaccumTimespanMS.ToString();

        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep) 
        { 

            currengStep = EnumRecipeStep.Component_PPSettings;
            _editRecipe.CurrentComponent.PPSettings.DelayMSForVaccum = float.Parse(sePickupDelayMs.Text.Trim());
            _editRecipe.CurrentComponent.PPSettings.PickupStress = float.Parse(sePickupStressMM.Text.Trim());

            _editRecipe.CurrentComponent.PPSettings.NeedleUpHeight = float.Parse(seNeedleUpHeight.Text.Trim());
            _editRecipe.CurrentComponent.PPSettings.NeedleSpeed = float.Parse(seNeedleSpeed.Text.Trim());

            _editRecipe.CurrentComponent.PPSettings.SlowTravelBeforePickupMM = float.Parse(seBeforePickupSlowTravelMM.Text.Trim());
            _editRecipe.CurrentComponent.PPSettings.SlowSpeedBeforePickup = float.Parse(seBeforePickupSlowSpeed.Text.Trim());

            _editRecipe.CurrentComponent.PPSettings.SlowTravelAfterPickupMM = float.Parse(seAfterPickupSlowTravelMM.Text.Trim());
            _editRecipe.CurrentComponent.PPSettings.SlowSpeedAfterPickup = float.Parse(seAfterPickupSlowSpeed.Text.Trim());

            _editRecipe.CurrentComponent.PPSettings.DelayMSForPlace = float.Parse(sePlaceDelayMs.Text.Trim());
            _editRecipe.CurrentComponent.PPSettings.PlaceStress = float.Parse(sePlaceStress.Text.Trim());

            _editRecipe.CurrentComponent.PPSettings.BreakVaccumTimespanMS = float.Parse(seBreakVaccumTimespanMs.Text.Trim());

            _editRecipe.CurrentComponent.IsMaterialPPSettingsComplete = true;
            finished = true;
        }
        private void InitialCameraControl()
        {

        }
    }
}
