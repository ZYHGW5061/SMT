using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using JobClsLib;
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
    public partial class RecipeStep_ComponentAccuracySettings : RecipeStepBase
    {
        private PointF _leftUpperCornerCoor;
        private PointF _rightUpperCornerCoor;
        private PointF _rightLowerCornerCoor;
        private PointF _leftLowerCornerCoor;
        private PointF _centerCoor;
        private PointF _componentSize;
        private float _componentRotateAngle;
        public RecipeStep_ComponentAccuracySettings()
        {
            InitializeComponent();
            _leftUpperCornerCoor = new PointF();
            _rightUpperCornerCoor = new PointF();
            _rightLowerCornerCoor = new PointF();
            _leftLowerCornerCoor = new PointF();
            _centerCoor = new PointF();
            _componentSize = new PointF();
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private ComponentAccuracyStepBasePage currentStepPage;
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
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
            _editRecipe.CurrentComponent.IsMaterialAccuracySettingsComplete = true;
            finished = true;
        }
        private CameraWindowGUI _parentCameraWnd;
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();


            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
            //CameraWindowGUI.Instance.SelectCamera(1);

            _parentCameraWnd = new CameraWindowGUI();
            _parentCameraWnd.InitVisualControl();
            _parentCameraWnd.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(_parentCameraWnd);
            if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
            {
                _parentCameraWnd.SelectCamera(1);
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
                var step = EnumDefineSetupRecipeComponentAccuracyStep.None;
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
                currentStepPage = new ComponentAccuracyStep_LUCorner();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftUpperCorner)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition)
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
                var step = EnumDefineSetupRecipeComponentAccuracyStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Component_Accuracy);
                //this.panelStepOperate.Controls.Clear();
                //currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //currentStepPage.Dock = DockStyle.Fill;
                //this.panelStepOperate.Controls.Add(currentStepPage);
                if (step != EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition)
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
                    //Carrier是华夫盒时二次校准完成之后将芯片原路放回
                    if (_editRecipe.CurrentComponent.CarrierType == EnumCarrierType.WafflePack)
                    {
                        //var offset = SystemConfiguration.Instance.PositioningConfig.PP1AndBondCameraOffset;
                        if (WarningBox.FormShow("动作确认！", "芯片原路放回？", "提示") == 1)
                        {
                            try
                            {
                                CreateWaitDialog();
                                if (JobInfosManager.Instance.CurrentComponentForProgramAccuracy != null)
                                {

                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                                    //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                                    //JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z = _editRecipe.CurrentComponent.ChipPPPickPos;
                                    //var offset = _systemConfig.PositioningConfig.PP1AndBondCameraOffset;

                                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, JobInfosManager.Instance.CurrentComponentForProgramAccuracy.X - offset.X, EnumCoordSetType.Absolute);
                                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Y + offset.Y, EnumCoordSetType.Absolute);

                                    //_editRecipe.CurrentComponent.PPSettings.WorkHeight = (float)JobInfosManager.Instance.CurrentComponentForProgramAccuracy.Z;
                                    //if (!PPUtility.Instance.PlaceViaSystemCoor(_editRecipe.CurrentComponent.PPSettings))
                                    //{
                                    //    CloseWaitDialog();
                                    //    WarningBox.FormShow("错误！", "放置芯片失败！", "提示");
                                    //}
                                    //else
                                    //{
                                    //    CloseWaitDialog();
                                    //    WarningBox.FormShow("完成！", "芯片已原路放回！", "提示");
                                    //}
                                }
                            }
                            catch (Exception ex)
                            {
                                LogRecorder.RecordLog(EnumLogContentType.Error, "编程-二次校准后芯片原路放回错误。", ex);
                            }
                            finally
                            {
                                CloseWaitDialog();
                            }
                        }
                    }
                }
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new ComponentAccuracyStep_LUCorner();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
                UpdateStepSignStatus();
                currentStepPage.LoadEditedRecipe(_editRecipe);
            }

            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftUpperCorner)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentAccuracyStep.EdgeSearch)
            //{
            //    this.btnNext.Visible = false;
            //}
            //else
            //{
            //    this.btnNext.Visible = true;
            //}
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition)
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

        private ComponentAccuracyStepBasePage GenerateStepPage(EnumDefineSetupRecipeComponentAccuracyStep step)
        {
            var ret = new ComponentAccuracyStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeComponentAccuracyStep.None:
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftUpperCorner:
                    ret = new ComponentAccuracyStep_LUCorner();
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentRightUpperCorner:
                    ret = new ComponentAccuracyStep_RUCorner();
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentRightLowerCorner:
                    ret = new ComponentAccuracyStep_RLCorner();
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftLowerCorner:
                    ret = new ComponentAccuracyStep_LLCorner();
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition:
                    ret = new ComponentAccuracyStep_Position(_parentCameraWnd);
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.LeftUpperEdgeSearch:
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.RighLowerEdgeSearch:
                    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeComponentAccuracyStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeComponentAccuracyStep.None:
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftUpperCorner:
                    _leftUpperCornerCoor = currentStepPage.LeftUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentRightUpperCorner:
                    _rightUpperCornerCoor = currentStepPage.RightUpperCornerCoor;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentRightLowerCorner:
                    _rightLowerCornerCoor = currentStepPage.RightLowerCornerCoor;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftLowerCorner:
                    _leftLowerCornerCoor = currentStepPage.LeftLowerCornerCoor;
                    CalculateComponentCenterCoor();
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondX, _centerCoor.X, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.BondY, _centerCoor.Y, EnumCoordSetType.Absolute);
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition:
                    //根据识别的结果计算当前芯片的中心和角度
                    if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                    {
                        var visionParam = _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.LineSearchParams.FirstOrDefault();
                        if (visionParam != null)
                        {
                            visionParam.OrigionAngle = _componentRotateAngle;
                        }
                    }
                    else if (_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                    {
                        var visionParam = _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.FirstOrDefault();
                        if (visionParam != null)
                        {
                            visionParam.OrigionAngle = _componentRotateAngle;
                        }
                    }

                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.LeftUpperEdgeSearch:
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.RighLowerEdgeSearch:
                    break;
                default:
                    break;
            }
        }
        private void CalculateComponentCenterCoor()
        {
            RectangleComputer.GetRectangleCenterByCornerCoordinates(_leftUpperCornerCoor, _rightUpperCornerCoor, _rightLowerCornerCoor, _leftLowerCornerCoor
                , out _centerCoor, out _componentSize, out _componentRotateAngle);
            LogRecorder.RecordLog(EnumLogContentType.Info, $"Accyracy-CalculateComponentCenterCoor,LeftUpperX:{_leftUpperCornerCoor.X},LeftUpperY:{_leftUpperCornerCoor.Y}," +
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
                case EnumDefineSetupRecipeComponentAccuracyStep.None:
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftUpperCorner:
                    step1Sign.Image = Properties.Resources.loc_left_top;
                    step2Sign.Image = Properties.Resources.loc_right_top_undo;
                    step3Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step4Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step5Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentRightUpperCorner:
                    step1Sign.Image = Properties.Resources.loc_left_top_done;
                    step2Sign.Image = Properties.Resources.loc_right_top;
                    step3Sign.Image = Properties.Resources.loc_right_bottom_undo;
                    step4Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step5Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentRightLowerCorner:
                    step1Sign.Image = Properties.Resources.loc_left_top_done;
                    step2Sign.Image = Properties.Resources.loc_right_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_bottom;
                    step4Sign.Image = Properties.Resources.loc_left_bottom_undo;
                    step5Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.SetComponentLeftLowerCorner:
                    step1Sign.Image = Properties.Resources.loc_left_top_done;
                    step2Sign.Image = Properties.Resources.loc_right_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step4Sign.Image = Properties.Resources.loc_left_bottom;
                    step5Sign.Image = Properties.Resources.recognize_undo;
                    break;
                case EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition:
                    step1Sign.Image = Properties.Resources.loc_left_top_done;
                    step2Sign.Image = Properties.Resources.loc_right_top_done;
                    step3Sign.Image = Properties.Resources.loc_right_bottom_done;
                    step4Sign.Image = Properties.Resources.loc_left_bottom_done;
                    step5Sign.Image = Properties.Resources.recognize;
                    break;
                default:
                    break;
            }

        }
        private void StepSignComplete()
        {
            step1Sign.Image = Properties.Resources.loc_left_top_done;
            step2Sign.Image = Properties.Resources.loc_right_top_done;
            step3Sign.Image = Properties.Resources.loc_right_bottom_done;
            step4Sign.Image = Properties.Resources.loc_left_bottom_done;
            step5Sign.Image = Properties.Resources.recognize_done;
        }
    }
}
