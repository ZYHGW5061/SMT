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
using VisionGUI;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeStep_BondPositionSettings : RecipeStepBase
    {
        private PointF _leftUpperCornerCoor;
        private PointF _rightUpperCornerCoor;
        private PointF _rightLowerCornerCoor;
        private PointF _leftLowerCornerCoor;
        private PointF _centerCoor;
        private PointF _submountSize;
        private float _submountRotateAngle;
        private XYZTCoordinateConfig _bondPositionOffset;
        private XYZTCoordinateConfig _bondPositionCompensation;
        private XYZTCoordinateConfig _patternPositionOffsetWithVisionCenter;
        public RecipeStep_BondPositionSettings()
        {
            InitializeComponent();
            _leftUpperCornerCoor = new PointF();
            _rightUpperCornerCoor = new PointF();
            _rightLowerCornerCoor = new PointF();
            _leftLowerCornerCoor = new PointF();
            _centerCoor = new PointF();
            _submountSize = new PointF();
            _bondPositionOffset = new XYZTCoordinateConfig();
            _bondPositionCompensation = new XYZTCoordinateConfig();
            _patternPositionOffsetWithVisionCenter = new XYZTCoordinateConfig();
        }
        private BondPositionStepBasePage currentStepPage;
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

            currengStep = EnumRecipeStep.BondPosition;

            _editRecipe.CurrentBondPosition.IsComplete = true;
            _editRecipe.CurrentBondPosition.BondPositionCompensation = _bondPositionCompensation;
            //精准模式即需要视觉识别
            if (_editRecipe.CurrentBondPosition.FindBondPositionMethod==EnumFindBondPositionMethod.Accuracy)
            {
                _editRecipe.CurrentBondPosition.BondPositionWithPatternOffset.X = _bondPositionOffset.X - _patternPositionOffsetWithVisionCenter.X;
                _editRecipe.CurrentBondPosition.BondPositionWithPatternOffset.Y = _bondPositionOffset.Y - _patternPositionOffsetWithVisionCenter.Y;
                _editRecipe.CurrentBondPosition.BondPositionWithPatternOffset.Theta = _bondPositionOffset.Theta;
                _editRecipe.CurrentBondPosition.BondPositionWithMaterialCenterOffset.Theta = _bondPositionOffset.Theta;
            }
            else
            {
                _editRecipe.CurrentBondPosition.BondPositionWithMaterialCenterOffset.X = _bondPositionOffset.X;
                _editRecipe.CurrentBondPosition.BondPositionWithMaterialCenterOffset.Y = _bondPositionOffset.Y;
                _editRecipe.CurrentBondPosition.BondPositionWithMaterialCenterOffset.Theta = _bondPositionOffset.Theta;
                _editRecipe.CurrentBondPosition.BondPositionWithPatternOffset.Theta = _bondPositionOffset.Theta;
            }
            finished = true;
        }
        private CameraWindowGUI _parentCameraWnd;
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
            //if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(0);
            //}
            //else if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(2);
            //}

            _parentCameraWnd = new CameraWindowGUI();
            _parentCameraWnd.InitVisualControl();
            _parentCameraWnd.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(_parentCameraWnd);
            //if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            //{
                _parentCameraWnd.SelectCamera(0);
            //}
            //else if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            //{
            //    _parentCameraWnd.SelectCamera(2);
            //}
        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeBondPositionStep.None;
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
                currentStepPage = new BondPositionStep_PPHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeBondPositionStep.SetPPHeight)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeBondPositionStep.SetBondPosition)
            {
                this.btnNext.Visible = false;
            }
            else
            {
                this.btnNext.Visible = true;
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
                var step = EnumDefineSetupRecipeBondPositionStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.BondPosition);
                if (currentStepPage.CurrentStep != EnumDefineSetupRecipeBondPositionStep.SetBondPosition)
                {
                    this.panelStepOperate.Controls.Clear();
                    currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                    currentStepPage.Dock = DockStyle.Fill;
                    this.panelStepOperate.Controls.Add(currentStepPage);
                    UpdateStepSignStatus();
                }
                else
                {
                    StepSignComplete();
                }
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new BondPositionStep_PPHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
                UpdateStepSignStatus();
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeBondPositionStep.SetPPHeight)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeBondPositionStep.SetBondPosition)
            {
                //this.btnNext.Visible = false;
                this.btnNext.Text = "完成";
            }
            else
            {
                this.btnNext.Visible = true;
                this.btnNext.Text = "下一步";
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }

        private BondPositionStepBasePage GenerateStepPage(EnumDefineSetupRecipeBondPositionStep step)
        {
            var ret = new BondPositionStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeBondPositionStep.None:
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetPPHeight:
                    ret = new BondPositionStep_PPHeight();
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftUpperCorner:
                    ret = new BondPositionStep_SetLUCorner();
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountRightUpperCorner:
                    ret = new BondPositionStep_SetRUCorner();
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountRightLowerCorner:
                    ret = new BondPositionStep_SetRLCorner();
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftLowerCorner:
                    ret = new BondPositionStep_SetLLCorner();
                    break;
                case EnumDefineSetupRecipeBondPositionStep.VisionPosition:
                    ret = new BondPositionStep_VisionPosition(_parentCameraWnd);
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetBondPosition:
                    ret = new BondPositionStep_SetBondPosition();
                    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeBondPositionStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeBondPositionStep.None:
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetPPHeight:
                    _editRecipe.CurrentBondPosition.SystemHeight = currentStepPage.BondPositionTopplateHigherValueThanMarkTopplate;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftUpperCorner:
                    _leftUpperCornerCoor = currentStepPage.LeftUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountRightUpperCorner:
                    _rightUpperCornerCoor = currentStepPage.RightUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountRightLowerCorner:
                    _rightLowerCornerCoor = currentStepPage.RightLowerCornerCoor;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftLowerCorner:
                    _leftLowerCornerCoor = currentStepPage.LeftLowerCornerCoor;
                    CalculateCenterCoor();
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, _centerCoor.X, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    break;
                case EnumDefineSetupRecipeBondPositionStep.VisionPosition:
                    _patternPositionOffsetWithVisionCenter = currentStepPage.PatternPositionOffsetWithVisionCenter;
                    _patternPositionOffsetWithVisionCenter.Theta = _submountRotateAngle;
                    var shapeMatchParam = _editRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.FirstOrDefault();
                    if (shapeMatchParam != null)
                    {
                        shapeMatchParam.OrigionAngle = _submountRotateAngle;
                        shapeMatchParam.PositionOfMaterialCenter.X = _centerCoor.X;
                        shapeMatchParam.PositionOfMaterialCenter.Y = _centerCoor.Y;

                        shapeMatchParam.PatternOffsetWithMaterialCenter.X = _centerCoor.X - currentStepPage.PositionOfCreatePattern.X;
                        shapeMatchParam.PatternOffsetWithMaterialCenter.Y = _centerCoor.Y - currentStepPage.PositionOfCreatePattern.Y;
                        shapeMatchParam.BondTablePositionOfCreatePattern.X = currentStepPage.PositionOfCreatePattern.X;
                        shapeMatchParam.BondTablePositionOfCreatePattern.Y = currentStepPage.PositionOfCreatePattern.Y;

                    }
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetBondPosition:
                    _bondPositionOffset = currentStepPage.BondPositionOffset;
                    _bondPositionCompensation = currentStepPage.BondPositionCompensation;
                    break;
                default:
                    break;
            }
            _editRecipe.SaveRecipe(EnumRecipeStep.BondPosition);
        }
        private void CalculateCenterCoor()
        {
            RectangleComputer.GetRectangleCenterByCornerCoordinates(_leftUpperCornerCoor, _rightUpperCornerCoor, _rightLowerCornerCoor, _leftLowerCornerCoor
                , out _centerCoor, out _submountSize, out _submountRotateAngle);
            LogRecorder.RecordLog(EnumLogContentType.Info, $"BP-CalculateSubmountCenterCoor,LeftUpperX:{_leftUpperCornerCoor.X},LeftUpperY:{_leftUpperCornerCoor.Y}," +
                    $"RightUpperX:{_rightUpperCornerCoor.X},RightUpperY:{_rightUpperCornerCoor.Y},RightLowerX:{_rightLowerCornerCoor.X},RightLowerY:{_rightLowerCornerCoor.Y}," +
                    $"LeftLowerX:{_leftLowerCornerCoor.X},LeftLowerY:{_leftLowerCornerCoor.Y}" +
                    $"CenterX:{_centerCoor.X},CenterY:{_centerCoor.Y},SizeX:{_submountSize.X},SizeY:{_submountSize.Y},Angle:{_submountRotateAngle}");
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
                case EnumDefineSetupRecipeBondPositionStep.None:
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetPPHeight:
                    step1Sign.Image = Properties.Resources.height;
                    step2Sign.Image = Properties.Resources.loc_left_top_undo;
                    step3Sign.Image = Properties.Resources.loc_right_top_undo;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    step7Sign.Image = Properties.Resources.bondpos_left_top_undo;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftUpperCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top;
                    step3Sign.Image = Properties.Resources.loc_right_top_undo;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    step7Sign.Image = Properties.Resources.bondpos_left_top_undo;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountRightUpperCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    step7Sign.Image = Properties.Resources.bondpos_left_top_undo;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountRightLowerCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    step7Sign.Image = Properties.Resources.bondpos_left_top_undo;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftLowerCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step5Sign.Image = Properties.Resources.loc_left_bottom;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    step7Sign.Image = Properties.Resources.bondpos_left_top_undo;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.VisionPosition:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_done;
                    step6Sign.Image = Properties.Resources.recognize;
                    step7Sign.Image = Properties.Resources.bondpos_left_top_undo;
                    break;
                case EnumDefineSetupRecipeBondPositionStep.SetBondPosition:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_done;
                    step6Sign.Image = Properties.Resources.recognize_done;
                    step7Sign.Image = Properties.Resources.bondpos_left_top;
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
            step7Sign.Image = Properties.Resources.bondpos_left_top_done;
        }
    }
}
