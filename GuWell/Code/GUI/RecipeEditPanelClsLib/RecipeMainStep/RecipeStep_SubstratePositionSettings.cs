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
    public partial class RecipeStep_SubstratePositionSettings : RecipeStepBase
    {
        private PointF _leftUpperCornerCoor;
        private PointF _rightUpperCornerCoor;
        private PointF _rightLowerCornerCoor;
        private PointF _leftLowerCornerCoor;
        private PointF _centerCoor;
        private PointF _substratetSize;
        private float _substrateRotateAngle;
        public RecipeStep_SubstratePositionSettings()
        {
            InitializeComponent();
            _leftUpperCornerCoor = new PointF();
            _rightUpperCornerCoor = new PointF();
            _rightLowerCornerCoor = new PointF();
            _leftLowerCornerCoor = new PointF();
            _centerCoor = new PointF();
            _substratetSize = new PointF();
        }
        private SubstratePositionStepBasePage currentStepPage;
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

            currengStep = EnumRecipeStep.Substrate_PositionSettings;
            //if (currentStepPage != null)
            //{
            //    var step = EnumDefineSetupRecipeSubmountPositionStep.None;
            //    currentStepPage.NotifyStepFinished(out finished, out step);
            //    SaveStepParametersWhenStepFinished(step);
            //}
            _editRecipe.SubstrateInfos.IsMaterialPositionSettingsComplete = true;
            finished = true;
        }
        private CameraWindowGUI _parentCameraWnd;
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
            //if (_editRecipe.SubstrateInfos.PositionSubmountVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(0);
            //}
            //else if (_editRecipe.SubstrateInfos.PositionSubmountVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
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
                var step = EnumDefineSetupRecipeSubstratePositionStep.None;
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
                currentStepPage = new SubstratePositionStep_WorkHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstratePositionStep.SetMark2VisionParam)
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
                var step = EnumDefineSetupRecipeSubstratePositionStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Substrate_PositionSettings);
                //this.panelStepOperate.Controls.Clear();
                //currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //currentStepPage.Dock = DockStyle.Fill;
                //this.panelStepOperate.Controls.Add(currentStepPage);
                if (step != EnumDefineSetupRecipeSubstratePositionStep.SetMark2VisionParam)
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
                currentStepPage = new SubstratePositionStep_WorkHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
                UpdateStepSignStatus();
                currentStepPage.LoadEditedRecipe(_editRecipe);
            }

            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubmountPositionStep.VisionPosition)
            //{
            //    this.btnNext.Visible = false;
            //}
            //else
            //{
            //    this.btnNext.Visible = true;
            //}
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstratePositionStep.SetMark2VisionParam)
            {
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

        private SubstratePositionStepBasePage GenerateStepPage(EnumDefineSetupRecipeSubstratePositionStep step)
        {
            var ret = new SubstratePositionStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeSubstratePositionStep.None:
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight:
                    ret = new SubstratePositionStep_WorkHeight();
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetLeftUpperCorner:
                    ret = new SubstratePositionStep_SetLUCorner();
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetRightUpperCorner:
                    ret = new SubstratePositionStep_SetRUCorner();
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetRightLowerCorner:
                    ret = new SubstratePositionStep_SetRLCorner();
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetLeftLowerCorner:
                    ret = new SubstratePositionStep_SetLLCorner();
                    break;
                //case EnumDefineSetupRecipeSubstratePositionStep.VisionPosition:
                //    ret = new SubstratePositionStep_VisionPosition(_parentCameraWnd);
                //    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetMark1VisionParam:
                    ret = new SubstratePositionStep_SetFirstMark(_parentCameraWnd);
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetMark2VisionParam:
                    ret = new SubstratePositionStep_SetSecondMark(_parentCameraWnd);
                    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeSubstratePositionStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeSubstratePositionStep.None:
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight:

                    _editRecipe.SubstrateInfos.SubstrateTopZSystemPos = currentStepPage.SubstrateTopplateHigherValueThanMarkTopplate;

                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetLeftUpperCorner:
                    _leftUpperCornerCoor = currentStepPage.LeftUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetRightUpperCorner:
                    _rightUpperCornerCoor = currentStepPage.RightUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetRightLowerCorner:
                    _rightLowerCornerCoor = currentStepPage.RightLowerCornerCoor;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetLeftLowerCorner:
                    _leftLowerCornerCoor = currentStepPage.LeftLowerCornerCoor;
                    CalculateCenterCoor();
                    _editRecipe.SubstrateInfos.WidthMM = _substratetSize.X;
                    _editRecipe.SubstrateInfos.HeightMM = _substratetSize.Y;

                    _editRecipe.SubstrateInfos.FirstSubstrateCenterSystemLocation.X = _centerCoor.X;
                    _editRecipe.SubstrateInfos.FirstSubstrateCenterSystemLocation.Y = _centerCoor.Y;
                    //var usedCamera = _editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera;
                    //if (usedCamera == EnumCameraType.BondCamera)
                    //{
                    //    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, _centerCoor.X, EnumCoordSetType.Absolute);
                    //    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    //}
                    //else if (usedCamera == EnumCameraType.WaferCamera)
                    //{
                    //    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, _centerCoor.X, EnumCoordSetType.Absolute);
                    //    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    //}
                    //将物料移动到视野中心
                    break;
                //case EnumDefineSetupRecipeSubstratePositionStep.VisionPosition:


                //    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetMark1VisionParam:
                    var shapeMatchParam = _editRecipe.SubstrateInfos.PositionSustrateMarkVisionParameters.FirstOrDefault()?.ShapeMatchParameters.FirstOrDefault();
                    if (shapeMatchParam != null)
                    {
                        shapeMatchParam.OrigionAngle = _substrateRotateAngle;
                        //此处更新模板的特征和物料中心的偏移
                        shapeMatchParam.PatternOffsetWithMaterialCenter.X = _centerCoor.X - currentStepPage.PositionOfPattern.X;
                        shapeMatchParam.PatternOffsetWithMaterialCenter.Y = _centerCoor.Y - currentStepPage.PositionOfPattern.Y;

                        shapeMatchParam.PositionOfMaterialCenter.X = _centerCoor.X;
                        shapeMatchParam.PositionOfMaterialCenter.Y = _centerCoor.Y;
                    }
                    //此处更新模板的特征和物料中心的偏移
                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.X = currentStepPage.PositionOfPattern.X;
                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.Y = currentStepPage.PositionOfPattern.Y;


                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetMark2VisionParam:

                    var shapeMatchParam2 = _editRecipe.SubstrateInfos.PositionSustrateMarkVisionParameters[1].ShapeMatchParameters.FirstOrDefault();
                    if (shapeMatchParam2 != null)
                    {
                        shapeMatchParam2.OrigionAngle = _substrateRotateAngle;
                        //此处更新模板的特征和物料中心的偏移
                        shapeMatchParam2.PatternOffsetWithMaterialCenter.X = _centerCoor.X - currentStepPage.PositionOfPattern.X;
                        shapeMatchParam2.PatternOffsetWithMaterialCenter.Y = _centerCoor.Y - currentStepPage.PositionOfPattern.Y;

                        shapeMatchParam2.PositionOfMaterialCenter.X = _centerCoor.X;
                        shapeMatchParam2.PositionOfMaterialCenter.Y = _centerCoor.Y;
                    }
                    //此处更新模板的第二特征和物料中心的偏移
                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomeSecondPoint.X = currentStepPage.PositionOfPattern.X;
                    _editRecipe.SubstrateInfos.SubstrateCoordinateHomeSecondPoint.Y = currentStepPage.PositionOfPattern.Y;

                    break;
                default:
                    break;
            }
        }
        private void CalculateCenterCoor()
        {
            RectangleComputer.GetRectangleCenterByCornerCoordinates(_leftUpperCornerCoor, _rightUpperCornerCoor, _rightLowerCornerCoor, _leftLowerCornerCoor
                , out _centerCoor, out _substratetSize, out _substrateRotateAngle);
            LogRecorder.RecordLog(EnumLogContentType.Info, $"CalculateSubmountCenterCoor,LeftUpperX:{_leftUpperCornerCoor.X},LeftUpperY:{_leftUpperCornerCoor.Y}," +
                $"RightUpperX:{_rightUpperCornerCoor.X},RightUpperY:{_rightUpperCornerCoor.Y},RightLowerX:{_rightLowerCornerCoor.X},RightLowerY:{_rightLowerCornerCoor.Y}," +
                $"LeftLowerX:{_leftLowerCornerCoor.X},LeftLowerY:{_leftLowerCornerCoor.Y}" +
                $"CenterX:{_centerCoor.X},CenterY:{_centerCoor.Y},SizeX:{_substratetSize.X},SizeY:{_substratetSize.Y},Angle:{_substrateRotateAngle}");
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
                case EnumDefineSetupRecipeSubstratePositionStep.None:
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetWorkHeight:
                    stepSetHeightSign.Image = Properties.Resources.height;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top_undo;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top_undo;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom_undo;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom_undo;
                    stepSetMark1Sign.Image = Properties.Resources.mark1_undo;
                    stepSetMark2Sign.Image = Properties.Resources.mark2_undo;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetLeftUpperCorner:
                    stepSetHeightSign.Image = Properties.Resources.height_done;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top_undo;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom_undo;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom_undo;
                    stepSetMark1Sign.Image = Properties.Resources.mark1_undo;
                    stepSetMark2Sign.Image = Properties.Resources.mark2_undo;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetRightUpperCorner:
                    stepSetHeightSign.Image = Properties.Resources.height_done;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top_done;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom_undo;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom_undo;
                    stepSetMark1Sign.Image = Properties.Resources.mark1_undo;
                    stepSetMark2Sign.Image = Properties.Resources.mark2_undo;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetRightLowerCorner:
                    stepSetHeightSign.Image = Properties.Resources.height_done;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top_done;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top_done;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom_undo;
                    stepSetMark1Sign.Image = Properties.Resources.mark1_undo;
                    stepSetMark2Sign.Image = Properties.Resources.mark2_undo;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetLeftLowerCorner:
                    stepSetHeightSign.Image = Properties.Resources.height_done;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top_done;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top_done;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom_done;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom;
                    stepSetMark1Sign.Image = Properties.Resources.mark1_undo;
                    stepSetMark2Sign.Image = Properties.Resources.mark2_undo;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetMark1VisionParam:
                    stepSetHeightSign.Image = Properties.Resources.height_done;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top_done;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top_done;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom_done;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom_done;
                    stepSetMark1Sign.Image = Properties.Resources.mark1;
                    stepSetMark2Sign.Image = Properties.Resources.mark2_undo;
                    break;
                case EnumDefineSetupRecipeSubstratePositionStep.SetMark2VisionParam:
                    stepSetHeightSign.Image = Properties.Resources.height_done;
                    stepSetLUSign.Image = Properties.Resources.loc_left_top_done;
                    stepSetRUSign.Image = Properties.Resources.loc_right_top_done;
                    stepSetRLSign.Image = Properties.Resources.loc_right_bottom_done;
                    stepSetLLSign.Image = Properties.Resources.loc_left_bottom_done;
                    stepSetMark1Sign.Image = Properties.Resources.mark1_done;
                    stepSetMark2Sign.Image = Properties.Resources.mark2;
                    break;
                default:
                    break;
            }

        }
        private void StepSignComplete()
        {
            stepSetHeightSign.Image = Properties.Resources.height_done;
            stepSetLUSign.Image = Properties.Resources.loc_left_top_done;
            stepSetRUSign.Image = Properties.Resources.loc_right_top_done;
            stepSetRLSign.Image = Properties.Resources.loc_right_bottom_done;
            stepSetLLSign.Image = Properties.Resources.loc_left_bottom_done;
            stepSetMark1Sign.Image = Properties.Resources.mark1_done;
            stepSetMark2Sign.Image= Properties.Resources.mark2_done;
        }
    }
}
