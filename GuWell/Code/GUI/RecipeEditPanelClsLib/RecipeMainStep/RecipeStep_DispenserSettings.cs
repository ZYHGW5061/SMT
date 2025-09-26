using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using MathHelperClsLib;
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
using VisionControlAppClsLib;
using VisionGUI;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeStep_DispenserSettings : RecipeStepBase
    {

        private VisualControlManager _VisualManager
        {
            get { return VisualControlManager.Instance; }
        }
        public RecipeStep_DispenserSettings()
        {
            InitializeComponent();

        }
        private DispenserSettingsStepBasePage currentStepPage;
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        /// 加载Recipe内容
        /// </summary>
        /// <param name="recipe"></param>
        public override void LoadEditedRecipe(BondRecipe recipe) 
        { 
            _editRecipe = recipe;
            LoadNextStepPage();
            UpdateStepSignStatus();
            InitialCameraControl();
        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep) 
        { 

            currengStep = EnumRecipeStep.DispenserSettings;
            _editRecipe.DispenserSettings.IsCompleted = true;
            finished = true;
        }
        private CameraWindowGUI _parentCameraWnd;
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();

            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);

            //CameraWindowGUI.Instance.SelectCamera(0);


            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
            //if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(0);
            //}
            //else if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(2);
            //}
            _parentCameraWnd = new CameraWindowGUI();

            _parentCameraWnd.InitVisualControl();
            _parentCameraWnd.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(_parentCameraWnd);

            _parentCameraWnd.SelectCamera(0);

        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineDispenserSettingsStep.None;
                //currentStepPage.NotifyStepFinished(out finished, out step);
                //_editRecipe.SaveRecipe();
                this.panelStepOperate.Controls.Clear();
                currentStepPage = GenerateStepPage(currentStepPage.CurrentStep - 1);
                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new DispenserSettingsStep_DispenserInfo();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineDispenserSettingsStep.DispenserInfo)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineDispenserSettingsStep.DispenserWorkHeight)
            {
                this.btnNext.Visible = false;
            }
            else
            {
                this.btnNext.Visible = true;
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }
        private void LoadNextStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineDispenserSettingsStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Component_PositionSettings);
                //this.panelStepOperate.Controls.Clear();
                //currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //currentStepPage.Dock = DockStyle.Fill;
                //this.panelStepOperate.Controls.Add(currentStepPage);
                if (step != EnumDefineDispenserSettingsStep.DispenserWorkHeight)
                {
                    this.panelStepOperate.Controls.Clear();
                    currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                    currentStepPage.Dock = DockStyle.Fill;
                    this.panelStepOperate.Controls.Add(currentStepPage);
                    UpdateStepSignStatus();
                    currentStepPage.LoadEditedRecipe(_editRecipe);
                }
                else
                {
                    StepSignComplete();
                }
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new DispenserSettingsStep_DispenserInfo();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);

                UpdateStepSignStatus();
                currentStepPage.LoadEditedRecipe(_editRecipe);
            }

            if (currentStepPage.CurrentStep == EnumDefineDispenserSettingsStep.DispenserInfo)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentPositionStep.VisionPosition)
            //{
            //    this.btnNext.Visible = false;
            //    ((ComponentPositionStep_VisionPosition)currentStepPage).InitializeVisionTool(_VisualManager.GetCameraByID(EnumCameraType.BondCamera));
            //}
            //else
            //{
            //    this.btnNext.Visible = true;
            //}
            if (currentStepPage.CurrentStep == EnumDefineDispenserSettingsStep.DispenserWorkHeight)
            {
                this.btnNext.Text = "完成";
            }
            else
            {
                this.btnNext.Visible = true;
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }

        private DispenserSettingsStepBasePage GenerateStepPage(EnumDefineDispenserSettingsStep step)
        {
            var ret = new DispenserSettingsStepBasePage();
            switch (step)
            {
                case EnumDefineDispenserSettingsStep.None:
                    break;
                case EnumDefineDispenserSettingsStep.DispenserInfo:
                    ret = new DispenserSettingsStep_DispenserInfo();
                    break;
                case EnumDefineDispenserSettingsStep.DispenserWorkHeight:
                    ret = new DispenserSettingsStep_WorkHeight();
                    break;
                case EnumDefineDispenserSettingsStep.DispenserPosition:
                    ret = new DispenserSettingsStep_XYPosition();
                    break;
                //case EnumDefineDispenserSettingsStep.ExpoxySettings:
                //    ret = new DispenserSettingsStep_EpoxySettings();
                //    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineDispenserSettingsStep step)
        {
            switch (step)
            {
                case EnumDefineDispenserSettingsStep.None:
                    break;
                case EnumDefineDispenserSettingsStep.DispenserWorkHeight:
                    _editRecipe.DispenserSettings.DispenserSystemPosZMM = currentStepPage.DispenserWorkHeight;
                    break;
                case EnumDefineDispenserSettingsStep.DispenserInfo:
                    //_editRecipe.DispenserSettings.PredispensingMode = currentStepPage.pre;
                    break;
                case EnumDefineDispenserSettingsStep.DispenserPosition:
                    _editRecipe.DispenserSettings.DispenserSystemPosXMM = currentStepPage.DispenserPosition.X;
                    _editRecipe.DispenserSettings.DispenserSystemPosYMM = currentStepPage.DispenserPosition.Y;
                    _editRecipe.DispenserSettings.DispenserPosOffsetXWithBondCamera = currentStepPage.DispenserPositionOffsetWithBondCamera.X;
                    _editRecipe.DispenserSettings.DispenserPosOffsetYWithBondCamera = currentStepPage.DispenserPositionOffsetWithBondCamera.Y;
                    break;             
                //case EnumDefineDispenserSettingsStep.ExpoxySettings:


                //    break;
                default:
                    break;
            }
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            LoadPreviousStepPage();
            UpdateStepSignStatus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadNextStepPage();

        }
        private void UpdateStepSignStatus()
        {
            switch (currentStepPage.CurrentStep)
            {
                case EnumDefineDispenserSettingsStep.None:
                    break;
                case EnumDefineDispenserSettingsStep.DispenserInfo:
                    step1Sign.Image = Properties.Resources.height_undo;
                    step2Sign.Image = Properties.Resources.height_undo;
                    step3Sign.Image = Properties.Resources.height_undo;

                    break;
                case EnumDefineDispenserSettingsStep.DispenserPosition:
                    step1Sign.Image = Properties.Resources.height_undo;
                    step2Sign.Image = Properties.Resources.loc_left_top;
                    step3Sign.Image = Properties.Resources.height_undo;

                    break;
                case EnumDefineDispenserSettingsStep.DispenserWorkHeight:
                    step1Sign.Image = Properties.Resources.height;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.height;

                    break;
                default:
                    break;
            }

        }
        private void StepSignComplete()
        {
            step1Sign.Image = Properties.Resources.height_done;
            step2Sign.Image = Properties.Resources.loc_left_top_done;
            step3Sign.Image = Properties.Resources.height_done;
        }

        private void RecipeStep_DispenserSettings_SizeChanged(object sender, EventArgs e)
        {

        }

        private void RecipeStep_DispenserSettings_Load(object sender, EventArgs e)
        {
            if (_parentCameraWnd != null)
            {

            }
        }
    }
}
