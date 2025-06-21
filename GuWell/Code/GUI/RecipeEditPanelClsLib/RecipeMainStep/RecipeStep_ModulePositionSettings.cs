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
    public partial class RecipeStep_ModulePositionSettings : RecipeStepBase
    {
        private PointF _leftUpperCornerCoor;
        private PointF _rightUpperCornerCoor;
        private PointF _rightLowerCornerCoor;
        private PointF _leftLowerCornerCoor;
        private PointF _centerCoor;
        private PointF _size;
        private float _rotateAngle;
        public RecipeStep_ModulePositionSettings()
        {
            InitializeComponent();
            _leftUpperCornerCoor = new PointF();
            _rightUpperCornerCoor = new PointF();
            _rightLowerCornerCoor = new PointF();
            _leftLowerCornerCoor = new PointF();
            _centerCoor = new PointF();
            _size = new PointF();
        }
        private ModulePositionStepBasePage currentStepPage;
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

            currengStep = EnumRecipeStep.Module_PositionSettings;
            //if (currentStepPage != null)
            //{
            //    var step = EnumDefineSetupRecipeSubmountPositionStep.None;
            //    currentStepPage.NotifyStepFinished(out finished, out step);
            //    SaveStepParametersWhenStepFinished(step);
            //}
            _editRecipe.SubstrateInfos.IsModulePositionSettingsComplete = true;
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
            if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            {
                _parentCameraWnd.SelectCamera(0);
            }
            else if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            {
                _parentCameraWnd.SelectCamera(2);
            }
            else
            {
                _parentCameraWnd.SelectCamera(0);
            }
        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeModulePositionStep.None;
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
                currentStepPage = new ModulePositionStep_WorkHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeModulePositionStep.SetWorkHeight)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeModulePositionStep.VisionPosition)
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
                var step = EnumDefineSetupRecipeModulePositionStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Substrate_PositionSettings);
                //this.panelStepOperate.Controls.Clear();
                //currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //currentStepPage.Dock = DockStyle.Fill;
                //this.panelStepOperate.Controls.Add(currentStepPage);
                if (step != EnumDefineSetupRecipeModulePositionStep.VisionPosition)
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
                currentStepPage = new ModulePositionStep_WorkHeight();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
                UpdateStepSignStatus();
                currentStepPage.LoadEditedRecipe(_editRecipe);
            }

            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeModulePositionStep.SetWorkHeight)
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
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeModulePositionStep.VisionPosition)
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

        private ModulePositionStepBasePage GenerateStepPage(EnumDefineSetupRecipeModulePositionStep step)
        {
            var ret = new ModulePositionStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeModulePositionStep.None:
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetWorkHeight:
                    ret = new ModulePositionStep_WorkHeight();
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetLeftUpperCorner:
                    ret = new ModulePositionStep_SetLUCorner();
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetRightUpperCorner:
                    ret = new ModulePositionStep_SetRUCorner();
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetRightLowerCorner:
                    ret = new ModulePositionStep_SetRLCorner();
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetLeftLowerCorner:
                    ret = new ModulePositionStep_SetLLCorner();
                    break;
                case EnumDefineSetupRecipeModulePositionStep.VisionPosition:
                    ret = new ModulePositionStep_VisionPosition(_parentCameraWnd);
                    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeModulePositionStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeModulePositionStep.None:
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetWorkHeight:
                    //_editRecipe.SubstrateInfos.PPSettings.UsedPP = EnumUsedPP.SubmountPP;
                    //_editRecipe.SubstrateInfos.PPSettings.IsUseNeedle = false;
                    ////_editRecipe.CurrentComponent.PPSettings.WorkHeight = currentStepPage.PPWorkHeight;
                    //_editRecipe.SubstrateInfos.SubmountPPPickPos = currentStepPage.PPWorkHeight;
                    //_editRecipe.SubstrateInfos.SubmountPPPickPosOffset = currentStepPage.PPWorkHeightOffset;
                    ////计算往共晶台放置衬底时的工作Z

                    //  var stagePos  = (float)(_systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z + _editRecipe.SubstrateInfos.ThicknessMM);
                    //_editRecipe.SubstrateInfos.SubmountPPPlacePos = _positioningSystem.ConvertStagePosToSystemPos(EnumStageAxis.BondZ, stagePos);


                    _editRecipe.SubstrateInfos.ModuleTopZSystemPos = currentStepPage.ModuleTopplateHigherValueThanMarkTopplate;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetLeftUpperCorner:
                    _leftUpperCornerCoor = currentStepPage.LeftUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetRightUpperCorner:
                    _rightUpperCornerCoor = currentStepPage.RightUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetRightLowerCorner:
                    _rightLowerCornerCoor = currentStepPage.RightLowerCornerCoor;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetLeftLowerCorner:
                    _leftLowerCornerCoor = currentStepPage.LeftLowerCornerCoor;
                    CalculateCenterCoor();
                    _editRecipe.SubstrateInfos.ModuleWidthMM = _size.X;
                    _editRecipe.SubstrateInfos.ModuleHeightMM = _size.Y;
                    var usedCamera = EnumCameraType.BondCamera;
                    if(usedCamera==EnumCameraType.BondCamera)
                    {
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, _centerCoor.X,EnumCoordSetType.Absolute);
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    }
                    else if(usedCamera == EnumCameraType.WaferCamera)
                    {
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableX, _centerCoor.X, EnumCoordSetType.Absolute);
                        _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.WaferTableY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    }
                    _editRecipe.SubstrateInfos.FirstModuleCenterSystemLocation.X = _centerCoor.X;
                    _editRecipe.SubstrateInfos.FirstModuleCenterSystemLocation.Y = _centerCoor.Y;
                    //将物料移动到视野中心
                    break;
                case EnumDefineSetupRecipeModulePositionStep.VisionPosition:
                    var shapeMatchParam = _editRecipe.SubstrateInfos.PositionModuleVisionParameters.ShapeMatchParameters.FirstOrDefault();
                    if (shapeMatchParam != null)
                    {
                        shapeMatchParam.OrigionAngle = _rotateAngle;
                        //此处更新模板的特征和物料中心的偏移
                        shapeMatchParam.PatternOffsetWithMaterialCenter.X = _centerCoor.X - currentStepPage.PositionOfPattern.X;
                        shapeMatchParam.PatternOffsetWithMaterialCenter.Y = _centerCoor.Y - currentStepPage.PositionOfPattern.Y;

                        shapeMatchParam.PositionOfMaterialCenter.X = _centerCoor.X;
                        shapeMatchParam.PositionOfMaterialCenter.Y = _centerCoor.Y;
                        shapeMatchParam.BondTablePositionOfCreatePattern.X = currentStepPage.PositionOfPattern.X;
                        shapeMatchParam.BondTablePositionOfCreatePattern.Y = currentStepPage.PositionOfPattern.Y;
                    }

                    break;
                default:
                    break;
            }
        }
        private void CalculateCenterCoor()
        {
            RectangleComputer.GetRectangleCenterByCornerCoordinates(_leftUpperCornerCoor, _rightUpperCornerCoor, _rightLowerCornerCoor, _leftLowerCornerCoor
                , out _centerCoor, out _size, out _rotateAngle);
            LogRecorder.RecordLog(EnumLogContentType.Info, $"CalculateSubmountCenterCoor,LeftUpperX:{_leftUpperCornerCoor.X},LeftUpperY:{_leftUpperCornerCoor.Y}," +
                $"RightUpperX:{_rightUpperCornerCoor.X},RightUpperY:{_rightUpperCornerCoor.Y},RightLowerX:{_rightLowerCornerCoor.X},RightLowerY:{_rightLowerCornerCoor.Y}," +
                $"LeftLowerX:{_leftLowerCornerCoor.X},LeftLowerY:{_leftLowerCornerCoor.Y}" +
                $"CenterX:{_centerCoor.X},CenterY:{_centerCoor.Y},SizeX:{_size.X},SizeY:{_size.Y},Angle:{_rotateAngle}");
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
                case EnumDefineSetupRecipeModulePositionStep.None:
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetWorkHeight:
                    step1Sign.Image = Properties.Resources.height;
                    step2Sign.Image = Properties.Resources.loc_left_top_undo;
                    step3Sign.Image = Properties.Resources.loc_right_top_undo;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetLeftUpperCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top;
                    step3Sign.Image = Properties.Resources.loc_right_top_undo;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetRightUpperCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetRightLowerCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom;
                    step5Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.SetLeftLowerCorner:
                    step1Sign.Image = Properties.Resources.height_done;
                    step2Sign.Image = Properties.Resources.loc_left_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_top_done;
                    step4Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step5Sign.Image = Properties.Resources.loc_left_bottom;
                    step6Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeModulePositionStep.VisionPosition:
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
