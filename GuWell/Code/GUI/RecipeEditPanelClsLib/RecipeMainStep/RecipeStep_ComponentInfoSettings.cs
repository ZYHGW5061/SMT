using CommonPanelClsLib;
using ConfigurationClsLib;
using ControlPanelClsLib;
using DevExpress.Utils.Design;
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
    public partial class RecipeStep_ComponentInfoSettings : RecipeStepBase
    {
        /// <summary>
        /// 选择wafer控件
        /// /// </summary>
        private CtrlMaterialBox _materialBox;
        public RecipeStep_ComponentInfoSettings()
        {
            InitializeComponent();
            InitialControl();
            LoadPPTool();
            LoadESTool();
            if (!DesignTimeTools.IsDesignMode)
            {
                cmbMaterialBoxTool.Items.Clear();
                foreach (var item in MaterialBoxToolsConfiguration.Instance.MaterialBoxTools)
                {
                    cmbMaterialBoxTool.Items.Add(item.Name);
                }
                _materialBox = new CtrlMaterialBox();
                _materialBox.Dock = DockStyle.Fill;
                this.panelControl2.Controls.Add(_materialBox);
                _materialBox.isMultipleSelection = true;

            }
        }
        private void InitialControl()
        {
            cmbComponentCarrierType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumCarrierType)))
            {
                cmbComponentCarrierType.Items.Add(item);
            }
            cmbVisionPosUsedCamera.Items.Clear();
            cmbVisionPosUsedCamera.Items.Add(EnumCameraType.BondCamera);
            cmbVisionPosUsedCamera.Items.Add(EnumCameraType.WaferCamera);

            cmbVisionPositionMethod.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            {
                cmbVisionPositionMethod.Items.Add(item);
            }

            cmbVisionPositionMethod.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            {
                cmbVisionPositionMethod.Items.Add(item);
            }

            cmbAccuracyMethod.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumAccuracyMethod)))
            {
                cmbAccuracyMethod.Items.Add(item);
            }

            cmbAccuracyVisionMethod.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(EnumVisionPositioningMethod)))
            {
                cmbAccuracyVisionMethod.Items.Add(item);
            }
        }
        private void LoadPPTool()
        {
            cmbRelatedPPTool.Items.Clear();
            foreach (var item in SystemConfiguration.Instance.PPToolSettings)
            {
                cmbRelatedPPTool.Items.Add(item.Name);
            }
        }
        private void LoadESTool()
        {
            cmbRelatedESTool.Items.Clear();
            foreach (var item in SystemConfiguration.Instance.ESToolSettings)
            {
                cmbRelatedESTool.Items.Add(item.Name);
            }
        }
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            _editRecipe = recipe;
            seComponentThicknessMM.Text = _editRecipe.CurrentComponent.ThicknessMM.ToString();
            cmbComponentCarrierType.Text = _editRecipe.CurrentComponent.CarrierType.ToString();
            seCarrierThicknessMM.Text = _editRecipe.CurrentComponent.CarrierThicknessMM.ToString();
            cmbVisionPositionMethod.Text= _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionMethod.ToString();
            cmbAccuracyMethod.Text= _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod.ToString();
            cmbAccuracyVisionMethod.Text= _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod.ToString();

            cmbVisionPosUsedCamera.Text = _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera.ToString();

            cmbRelatedPPTool.Text = _editRecipe.CurrentComponent.RelatedPPToolName;
            cmbRelatedESTool.Text = _editRecipe.CurrentComponent.RelatedESToolName;
            cmbMaterialBoxTool.Text = _editRecipe.CurrentComponent.RelatedMaterialBoxToolName;


            if (!string.IsNullOrEmpty(_editRecipe.CurrentComponent.RelatedMaterialBoxToolName))
            {
                var templateTool = MaterialBoxToolsConfiguration.Instance.MaterialBoxTools.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.RelatedMaterialBoxToolName);
                if(templateTool!=null)
                {
                    _materialBox.Initialize(templateTool.SlotCount);
                }
                else
                {
                    _materialBox.Initialize();
                }
            }
            else
            {
                _materialBox.Initialize();
            }
            _materialBox.CheckeSelectedWafers(_editRecipe.CurrentComponent.ProcessWafers);
        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep)
        {

            currengStep = EnumRecipeStep.Component_InformationSettings;
            _editRecipe.CurrentComponent.ThicknessMM = float.Parse(seComponentThicknessMM.Text.Trim());
            _editRecipe.CurrentComponent.CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbComponentCarrierType.Text);
            _editRecipe.CurrentComponent.CarrierThicknessMM = float.Parse(seCarrierThicknessMM.Text.Trim());
            _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbVisionPositionMethod.Text);
            _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod = (EnumAccuracyMethod)Enum.Parse(typeof(EnumAccuracyMethod), cmbAccuracyMethod.Text);
            _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbAccuracyVisionMethod.Text);

            _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera = (EnumCameraType)Enum.Parse(typeof(EnumCameraType), cmbVisionPosUsedCamera.Text);

            _editRecipe.CurrentComponent.RelatedPPToolName = cmbRelatedPPTool.Text;
            _editRecipe.CurrentComponent.PPSettings.PPtoolName = cmbRelatedPPTool.Text;
            _editRecipe.CurrentComponent.RelatedESToolName = cmbRelatedESTool.Text;
            _editRecipe.CurrentComponent.RelatedMaterialBoxToolName = cmbMaterialBoxTool.Text;
            _editRecipe.CurrentComponent.ProcessWafers = _materialBox.GetSelectedWafers();
            _editRecipe.CurrentComponent.IsMaterialInfoSettingsComplete = true;
            finished = true;
        }

        private void cmbComponentCarrierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbComponentCarrierType.SelectedIndex == (int)EnumCarrierType.Wafer)
            {
                panelControl2.Enabled = true;
            }
            else
            {
                panelControl2.Enabled = false;
            }
        }

        private void btnManageMaterialBoxTool_Click(object sender, EventArgs e)
        {
            FrmMaterialBoxTool frm = new FrmMaterialBoxTool();
            frm.ShowDialog();

            cmbMaterialBoxTool.Items.Clear();
            foreach (var item in MaterialBoxToolsConfiguration.Instance.MaterialBoxTools)
            {
                cmbMaterialBoxTool.Items.Add(item.Name);
            }

            frm.Dispose();
        }

        private void cmbMaterialBoxTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbMaterialBoxTool.Text))
            {
                var templateTool = MaterialBoxToolsConfiguration.Instance.MaterialBoxTools.FirstOrDefault(i => i.Name == cmbMaterialBoxTool.Text);
                if (templateTool != null)
                {
                    //this.panelControl2.Controls.Clear();
                    //_materialBox = new CtrlMaterialBox();
                    //_materialBox.Dock = DockStyle.Fill;
                    //this.panelControl2.Controls.Add(_materialBox);
                    //_materialBox.isMultipleSelection = true;
                    if (_materialBox.WaferSlotCount != templateTool.SlotCount)
                    {
                        _materialBox.Initialize(templateTool.SlotCount);
                    }
                }
            }
        }
    }
}
