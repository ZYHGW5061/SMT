using CommonPanelClsLib;
using ConfigurationClsLib;
using DispensingMachineControllerClsLib;
using DispensingMachineManagerClsLib;
using GlobalDataDefineClsLib;
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
    public partial class RecipeStep_EpoxyApplication : RecipeStepBase
    {
        public RecipeStep_EpoxyApplication()
        {
            InitializeComponent();
            InitialControl();
        }
        private void InitialControl()
        {
            cmbDispensePattern.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumDispensePattern)))
            {
                cmbDispensePattern.Items.Add(item);
            }
        }
        /// <summary>
        /// 点胶机控制器
        /// </summary>
        IDispensingMachineController _currentDispenseController
        {
            get
            {
                return DispensingMachineManager.Instance.GetCurrentHardware();
            }
        }
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            _editRecipe = recipe;
            cmbSelDispenserRecipe.Text = _editRecipe.CurrentEpoxyApplication.DispenserRecipeName;
            cmbDispensePattern.Text = _editRecipe.CurrentEpoxyApplication.DispensePattern.ToString();
            seDispensePatternWidth.Text = _editRecipe.CurrentEpoxyApplication.DispensePatternWidthMM.ToString();
            seDispensePatternHeight.Text = _editRecipe.CurrentEpoxyApplication.DispensePatternHeightMM.ToString();
            //seCarrierThicknessMM.Text = _editRecipe.CurrentComponent.CarrierThicknessMM.ToString();
            //cmbVisionPositionMethod.Text= _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionMethod.ToString();
            //cmbAccuracyMethod.Text= _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod.ToString();
            //cmbAccuracyVisionMethod.Text= _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod.ToString();

            //cmbVisionPosUsedCamera.Text = _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera.ToString();

            //cmbRelatedPPTool.Text = _editRecipe.CurrentComponent.RelatedPPToolName;
            //cmbRelatedESTool.Text = _editRecipe.CurrentComponent.RelatedESToolName;
            var dispenseInfo=_currentDispenseController.ReadDispensingParameters(Int32.Parse(_editRecipe.CurrentEpoxyApplication.DispenserRecipeName));
            seDispensePressure.Text = dispenseInfo.Pressure.ToString();
            seDispenseVaccumPressure.Text = dispenseInfo.Vacuum.ToString();
            seDispenseTimeS.Text = dispenseInfo.Time.ToString();
        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep)
        {

            currengStep = EnumRecipeStep.EpoxyApplications;
            _editRecipe.CurrentEpoxyApplication.DispenserRecipeName = cmbSelDispenserRecipe.Text;

            _editRecipe.CurrentEpoxyApplication.DispensePattern = (EnumDispensePattern)Enum.Parse(typeof(EnumDispensePattern), cmbDispensePattern.Text);
            _editRecipe.CurrentEpoxyApplication.DispensePatternWidthMM = float.Parse(seDispensePatternWidth.Text.Trim());
            _editRecipe.CurrentEpoxyApplication.DispensePatternHeightMM = float.Parse(seDispensePatternHeight.Text.Trim());
            //_editRecipe.CurrentComponent.ThicknessMM = float.Parse(seComponentThicknessMM.Text.Trim());
            //_editRecipe.CurrentComponent.CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbComponentCarrierType.Text);
            //_editRecipe.CurrentComponent.CarrierThicknessMM = float.Parse(seCarrierThicknessMM.Text.Trim());
            //_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbVisionPositionMethod.Text);
            //_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod = (EnumAccuracyMethod)Enum.Parse(typeof(EnumAccuracyMethod), cmbAccuracyMethod.Text);
            //_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbAccuracyVisionMethod.Text);

            //_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera = (EnumCameraType)Enum.Parse(typeof(EnumCameraType), cmbVisionPosUsedCamera.Text);

            //_editRecipe.CurrentComponent.RelatedPPToolName = cmbRelatedPPTool.Text;
            //_editRecipe.CurrentComponent.RelatedESToolName = cmbRelatedESTool.Text;

            _editRecipe.CurrentEpoxyApplication.IsCompleted = true;
            finished = true;
        }

        private void seCarrierThicknessMM_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void sePickupDelayMs_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void RingLightlabel_Click(object sender, EventArgs e)
        {

        }

        private void cmbComponentCarrierType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSetDispenseParam_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbSelDispenserRecipe.Text))
            {
                if (WarningBox.FormShow("即将设置点胶配方参数！", "是否设置点胶配方参数并保存？", "提示") == 1)
                {


                    _currentDispenseController.Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出条件,
                                                      cmbSelDispenserRecipe.Text, "0", seDispenseTimeS.Text.Trim(), seDispensePressure.Text.Trim(),
                                                      seDispenseVaccumPressure.Text);
                    WarningBox.FormShow("成功！", "点胶配方参数保存完成", "提示");

                }
            }
            else
            {
                WarningBox.FormShow("错误！", "请先选择点胶配方！", "提示");
            }
        }

        private void cmbSelDispenserRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dispenseInfo = _currentDispenseController.ReadDispensingParameters(Int32.Parse(cmbSelDispenserRecipe.Text));
            seDispensePressure.Text = dispenseInfo.Pressure.ToString();
            seDispenseVaccumPressure.Text = dispenseInfo.Vacuum.ToString();
            seDispenseTimeS.Text = dispenseInfo.Time.ToString();
        }
    }
}
