using ConfigurationClsLib;
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
    public partial class RecipeStep_SubstrateInfoSettings : RecipeStepBase
    {
        public RecipeStep_SubstrateInfoSettings()
        {
            InitializeComponent();
            InitialControl();
        }
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            _editRecipe = recipe;
            //seThicknessMM.Text = _editRecipe.SubmonutInfos.ThicknessMM.ToString();
            //cmbCarrierType.Text = _editRecipe.SubmonutInfos.CarrierType.ToString();
            //seCarrierThicknessMM.Text = _editRecipe.SubmonutInfos.CarrierThicknessMM.ToString();
            //cmbVisionPositionMethod.Text = _editRecipe.SubmonutInfos.PositionComponentVisionParameters.VisionPositionMethod.ToString();
            //cmbAccuracyMethod.Text = _editRecipe.SubmonutInfos.AccuracyComponentPositionVisionParameters.AccuracyMethod.ToString();
            //cmbAccuracyVisionMethod.Text = _editRecipe.SubmonutInfos.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod.ToString();
            //cmbVisionPosUsedCamera.Text = _editRecipe.SubmonutInfos.PositionSubmountVisionParameters.VisionPositionUsedCamera.ToString();

            //cmbBlankingMethod.Text = _editRecipe.BlankingParameters.BlankingMethod.ToString();
            //cmbRelatedPPTool.Text = _editRecipe.SubmonutInfos.RelatedPPToolName;
            //cmbRelatedESTool.Text = _editRecipe.SubmonutInfos.RelatedESToolName;

            ckeMultiSubstrate.Checked = _editRecipe.SubstrateInfos.IsMultiSubstrates;
            ckbMultiModule.Checked = _editRecipe.SubstrateInfos.IsMultiModules;
            ckbAlignModule.Checked = _editRecipe.SubstrateInfos.IsPositionModules;

            cmbPositionSubstratePointCount.Text = _editRecipe.SubstrateInfos.PositionSubstratePointCount.ToString();
            cmbPositionSubstrateMehtod.Text= _editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionMethod.ToString();

            cmbPositionModulePointCount.Text = _editRecipe.SubstrateInfos.PositionModulePointCount.ToString();
            cmbPositionModuleMehtod.Text = _editRecipe.SubstrateInfos.PositionModuleVisionParameters.VisionPositionMethod.ToString();
        }
        private void InitialControl()
        {
            //cmbCarrierType.Items.Clear();
            //foreach (var item in Enum.GetValues(typeof(EnumCarrierType)))
            //{
            //    cmbCarrierType.Items.Add(item);
            //}
            //cmbVisionPosUsedCamera.Items.Clear();
            //cmbVisionPosUsedCamera.Items.Add(EnumCameraType.BondCamera);
            ////cmbVisionPosUsedCamera.Items.Add(EnumCameraType.UplookingCamera);

            cmbPositionSubstrateMehtod.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            {
                cmbPositionSubstrateMehtod.Items.Add(item);
            }


            cmbPositionModuleMehtod.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            {
                cmbPositionModuleMehtod.Items.Add(item);
            }

            //cmbVisionPositionMethod.Items.Clear();
            //foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            //{
            //    cmbVisionPositionMethod.Items.Add(item);
            //}

            //cmbAccuracyMethod.Items.Clear();
            //foreach (var item in Enum.GetValues(typeof(EnumAccuracyMethod)))
            //{
            //    cmbAccuracyMethod.Items.Add(item);
            //}

            //cmbAccuracyVisionMethod.Items.Clear();
            //foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            //{
            //    cmbAccuracyVisionMethod.Items.Add(item);
            //}

            //cmbBlankingMethod.Items.Clear();
            //foreach (var item in Enum.GetValues(typeof(EnumBlankingMethod)))
            //{
            //    cmbBlankingMethod.Items.Add(item);
            //}
        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep)
        {

            currengStep = EnumRecipeStep.Substrate_InformationSettings;
            //_editRecipe.SubmonutInfos.ThicknessMM = float.Parse(seThicknessMM.Text.Trim());
            //_editRecipe.SubmonutInfos.CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbCarrierType.Text);
            //_editRecipe.SubmonutInfos.CarrierThicknessMM = float.Parse(seCarrierThicknessMM.Text.Trim());
            //_editRecipe.SubmonutInfos.PositionSubmountVisionParameters.VisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbVisionPositionMethod.Text);
            //_editRecipe.SubmonutInfos.AccuracySubmountPositionVisionParameters.AccuracyMethod = (EnumAccuracyMethod)Enum.Parse(typeof(EnumAccuracyMethod), cmbAccuracyMethod.Text);
            //_editRecipe.SubmonutInfos.AccuracySubmountPositionVisionParameters.AccuracyVisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbAccuracyVisionMethod.Text);

            //_editRecipe.SubmonutInfos.PositionSubmountVisionParameters.VisionPositionUsedCamera = (EnumCameraType)Enum.Parse(typeof(EnumCameraType), cmbVisionPosUsedCamera.Text);
            //_editRecipe.BlankingParameters.BlankingMethod = (EnumBlankingMethod)Enum.Parse(typeof(EnumBlankingMethod), cmbBlankingMethod.Text);
            //_editRecipe.SubmonutInfos.RelatedPPToolName = cmbRelatedPPTool.Text;
            //_editRecipe.SubmonutInfos.RelatedESToolName = cmbRelatedESTool.Text;

            _editRecipe.SubstrateInfos.IsMultiSubstrates= ckeMultiSubstrate.Checked;
            _editRecipe.SubstrateInfos.IsMultiModules= ckbMultiModule.Checked;
            _editRecipe.SubstrateInfos.IsPositionModules= ckbAlignModule.Checked;

            _editRecipe.SubstrateInfos.PositionSubstratePointCount = int.Parse(cmbPositionSubstratePointCount.Text.Trim());
            _editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbPositionSubstrateMehtod.Text);

            _editRecipe.SubstrateInfos.PositionModulePointCount = int.Parse(cmbPositionModulePointCount.Text.Trim());
            _editRecipe.SubstrateInfos.PositionModuleVisionParameters.VisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbPositionModuleMehtod.Text);

            _editRecipe.SubstrateInfos.IsMaterialInfoSettingsComplete = true;
            finished = true;
        }

        private void ckeMultiSubstrate_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
