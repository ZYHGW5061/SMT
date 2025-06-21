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
    public partial class RecipeStep_ComponentPositionSettings : RecipeStepBase
    {
        private PointF _leftUpperCornerCoor;
        private PointF _rightUpperCornerCoor;
        private PointF _rightLowerCornerCoor;
        private PointF _leftLowerCornerCoor;
        /// <summary>
        /// 系统坐标系
        /// </summary>
        private PointF _centerCoor;
        private PointF _componentSize;
        private float _componentRotateAngle;
        private VisualControlManager _VisualManager
        {
            get { return VisualControlManager.Instance; }
        }
        public RecipeStep_ComponentPositionSettings()
        {
            InitializeComponent();
            _leftUpperCornerCoor = new PointF();
            _rightUpperCornerCoor = new PointF();
            _rightLowerCornerCoor = new PointF();
            _leftLowerCornerCoor = new PointF();
            _centerCoor = new PointF();
            _componentSize = new PointF();
        }
        private ComponentPositionStepBasePage currentStepPage;
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

            currengStep = EnumRecipeStep.Component_PositionSettings;
            _editRecipe.CurrentComponent.IsMaterialPositionSettingsComplete = true;
            finished = true;
        }
        private CameraWindowGUI _parentCameraWnd;
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();

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
            if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            {
                _parentCameraWnd.SelectCamera(0);
            }
            else if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            {
                _parentCameraWnd.SelectCamera(2);
            }
        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeComponentPositionStep.None;
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
                currentStepPage = new ComponentPositionStep_WorkHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentPositionStep.VisionPosition)
            {
                this.btnNext.Text = "完成";
            }
            else
            {
                this.btnNext.Text = "下一步";
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }
        private void LoadNextStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeComponentPositionStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Component_PositionSettings);
                //this.panelStepOperate.Controls.Clear();
                //currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //currentStepPage.Dock = DockStyle.Fill;
                //this.panelStepOperate.Controls.Add(currentStepPage);
                if (step != EnumDefineSetupRecipeComponentPositionStep.VisionPosition)
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
                currentStepPage = new ComponentPositionStep_WorkHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);

                UpdateStepSignStatus();
                currentStepPage.LoadEditedRecipe(_editRecipe);
            }

            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight)
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
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentPositionStep.VisionPosition)
            {
                this.btnNext.Text = "完成";
            }
            //else
            //{
            //    this.btnNext.Visible = true;
            //}
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }

        private ComponentPositionStepBasePage GenerateStepPage(EnumDefineSetupRecipeComponentPositionStep step)
        {
            var ret = new ComponentPositionStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeComponentPositionStep.None:
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight:
                    ret = new ComponentPositionStep_WorkHeight();
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentLeftUpperCorner:
                    ret = new ComponentPositionStep_SizeLUCorner();
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentRightUpperCorner:
                    ret = new ComponentPositionStep_SizeRUCorner();
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentRightLowerCorner:
                    ret = new ComponentPositionStep_SizeRLCorner();
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentLeftLowerCorner:
                    ret = new ComponentPositionStep_SizeLLCorner();
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.VisionPosition:
                    ret = new ComponentPositionStep_VisionPosition(_parentCameraWnd);
                    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeComponentPositionStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeComponentPositionStep.None:
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight:
                    _editRecipe.CurrentComponent.PPSettings.UsedPP = EnumUsedPP.ChipPP;
                    if(_editRecipe.CurrentComponent.CarrierType==EnumCarrierType.Wafer)
                    {
                        _editRecipe.CurrentComponent.PPSettings.IsUseNeedle = true;
                        _editRecipe.CurrentComponent.ESBaseWorkPos = currentStepPage.ESWorkHeight;
                        _editRecipe.CurrentComponent.ChipPPPickSystemPos = currentStepPage.ChipTopplateHigherValueThanMarkTopplate;
                        //计算吸嘴工作拾取高度
                        //var usedPP = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _editRecipe.CurrentComponent.RelatedPPToolName);
                        //if(usedPP != null)
                        //{
                        //    //usedPP.PPESAltimetryParameter.PPPosition记录的已经是系统坐标系
                        //    var ppWorkSsytemPos = usedPP.PPESAltimetryParameter.PPSystemPosition - (currentStepPage.ESWorkHeight - usedPP.PPESAltimetryParameter.ESStagePosition) + _editRecipe.CurrentComponent.ThicknessMM + _editRecipe.CurrentComponent.CarrierThicknessMM;
                        //    //_editRecipe.CurrentComponent.ChipPPPickPos = _positioningSystem.ConvertStagePosToSystemPos(EnumStageAxis.BondZ, ppWorkStagePos);
                        //    _editRecipe.CurrentComponent.ChipPPPickSystemPos = ppWorkSsytemPos;
                        //}
                    }
                    else if (_editRecipe.CurrentComponent.CarrierType == EnumCarrierType.WafflePack|| _editRecipe.CurrentComponent.CarrierType == EnumCarrierType.WaferWafflePack)
                    {
                        _editRecipe.CurrentComponent.PPSettings.IsUseNeedle = false;
                        _editRecipe.CurrentComponent.ChipPPPickSystemPos = currentStepPage.ChipTopplateHigherValueThanMarkTopplate;
                    }

                    //var stagePos= (float)(_systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z + _editRecipe.SubstrateInfos.ThicknessMM+ _editRecipe.CurrentComponent.ThicknessMM);
                    ////需优化-TBD
                    //_editRecipe.CurrentComponent.ChipPPPlacePos = _positioningSystem.ConvertStagePosToSystemPos(EnumStageAxis.BondZ, stagePos);
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentLeftUpperCorner:
                    _leftUpperCornerCoor = currentStepPage.LeftUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentRightUpperCorner:
                    _rightUpperCornerCoor = currentStepPage.RightUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentRightLowerCorner:
                    _rightLowerCornerCoor = currentStepPage.RightLowerCornerCoor;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentLeftLowerCorner:
                    _leftLowerCornerCoor = currentStepPage.LeftLowerCornerCoor;
                    CalculateComponentCenterCoor();
                    _editRecipe.CurrentComponent.WidthMM = _componentSize.X;
                    _editRecipe.CurrentComponent.HeightMM = _componentSize.Y;
                    var usedCamera = _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera;
                    if (usedCamera == EnumCameraType.BondCamera)
                    {
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, _centerCoor.X, EnumCoordSetType.Absolute);
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    }
                    else if (usedCamera == EnumCameraType.WaferCamera)
                    {
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, _centerCoor.X, EnumCoordSetType.Absolute);
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    }
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.VisionPosition:
                    var shapeMatchParam = _editRecipe.CurrentComponent.PositionComponentVisionParameters.ShapeMatchParameters.FirstOrDefault();
                    if (shapeMatchParam != null)
                    {
                        shapeMatchParam.OrigionAngle = _componentRotateAngle;
                        //此处更新模板的特征中心和物料中心的偏移
                        shapeMatchParam.PatternOffsetWithMaterialCenter.X = _centerCoor.X - currentStepPage.PositionOfPattern.X;
                        shapeMatchParam.PatternOffsetWithMaterialCenter.Y = _centerCoor.Y - currentStepPage.PositionOfPattern.Y;

                        shapeMatchParam.PositionOfMaterialCenter.X = _centerCoor.X;
                        shapeMatchParam.PositionOfMaterialCenter.Y = _centerCoor.Y;
                    }

                    break;
                default:
                    break;
            }
        }
        private void CalculateComponentCenterCoor()
        {
            RectangleComputer.GetRectangleCenterByCornerCoordinates(_leftUpperCornerCoor, _rightUpperCornerCoor, _rightLowerCornerCoor, _leftLowerCornerCoor
                , out _centerCoor, out _componentSize, out _componentRotateAngle);

            LogRecorder.RecordLog(EnumLogContentType.Info, $"Position-CalculateComponentCenterCoor,LeftUpperX:{_leftUpperCornerCoor.X},LeftUpperY:{_leftUpperCornerCoor.Y}," +
                    $"RightUpperX:{_rightUpperCornerCoor.X},RightUpperY:{_rightUpperCornerCoor.Y},RightLowerX:{_rightLowerCornerCoor.X},RightLowerY:{_rightLowerCornerCoor.Y}," +
                    $"LeftLowerX:{_leftLowerCornerCoor.X},LeftLowerY:{_leftLowerCornerCoor.Y}" +
                    $"CenterX:{_centerCoor.X},CenterY:{_centerCoor.Y},SizeX:{_componentSize.X},SizeY:{_componentSize.Y},Angle:{_componentRotateAngle}");
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
                case EnumDefineSetupRecipeComponentPositionStep.None:
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetWorkHeight:
                    step1Sign.Image = Properties.Resources.height;
                    step2Sign.Image = Properties.Resources.loc_left_top_undo;
                    step3Sign.Image = Properties.Resources.loc_right_top_undo;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentLeftUpperCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top;
                    step3Sign.Image = Properties.Resources.loc_right_top_undo;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentRightUpperCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentRightLowerCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.SetComponentLeftLowerCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step5Sign.Image = Properties.Resources.loc_left_bottom;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentPositionStep.VisionPosition:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_done;
                    step6Sign.Image = Properties.Resources.recognize;
                    break;
                default:
                    break;
            }

        }
        private void StepSignComplete()
        {
            step1Sign.Image = Properties.Resources.height_done;
            step2Sign.Image = Properties.Resources.loc_left_top_done;
            step3Sign.Image = Properties.Resources.loc_right_top_done;
            step4Sign.Image = Properties.Resources.loc_right_bottom_done;
            step5Sign.Image = Properties.Resources.loc_left_bottom_done;
            step6Sign.Image = Properties.Resources.recognize_done;
        }
    }
}
