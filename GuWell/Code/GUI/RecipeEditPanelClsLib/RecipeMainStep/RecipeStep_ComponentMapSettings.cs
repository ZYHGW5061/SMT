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
using VisionGUI;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeStep_ComponentMapSettings : RecipeStepBase
    {
        private int _numbersofColumns;
        private int _numbersofRows;
        public float _rowPitchMM;
        public float _columnPitchMM;
        public PointF FirstComponentPosition { get; set; }
        public PointF LastColumnComponentPosition { get; set; }
        public PointF LastRowComponentPosition { get; set; }
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        public RecipeStep_ComponentMapSettings()
        {
            InitializeComponent();

        }
        private ComponentMapStepBasePage currentStepPage;
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
            if (_editRecipe.CurrentComponent.CarrierType != EnumCarrierType.Wafer)
            {
                step4Sign.Visible = false;
                step5Sign.Visible = false;
                step6Sign.Visible = false;
            }
        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep) 
        { 

            currengStep = EnumRecipeStep.Component_MaterialMap;
            GenerateComponentMap();
            _editRecipe.CurrentComponent.RowCount = _numbersofRows;
            _editRecipe.CurrentComponent.ColumnCount = _numbersofColumns;
            _editRecipe.CurrentComponent.PitchColumnMM = _columnPitchMM;
            _editRecipe.CurrentComponent.PitchRowMM = _rowPitchMM;

            _editRecipe.CurrentComponent.FirstComponentLocation.X = FirstComponentPosition.X;
            _editRecipe.CurrentComponent.FirstComponentLocation.Y = FirstComponentPosition.Y;
            _editRecipe.CurrentComponent.IsMaterialMapSettingsComplete = true;
            finished = true;
        }
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

            var cameraWnd = new CameraWindowGUI();
            cameraWnd.InitVisualControl();
            cameraWnd.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(cameraWnd);
            if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            {
                cameraWnd.SelectCamera(0);
            }
            else if (_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            {
                cameraWnd.SelectCamera(2);
            }
        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeComponentMapStep.None;
                //currentStepPage.NotifyStepFinished(out finished, out step);
                //SaveStepParametersWhenStepFinished(step);
                //_editRecipe.SaveRecipe();
                this.panelStepOperate.Controls.Clear();
                currentStepPage = GenerateStepPage(currentStepPage.CurrentStep - 1);
                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new ComponentMapStep_PositionFirstComp();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (_editRecipe.CurrentComponent.CarrierType != EnumCarrierType.Wafer)
            {
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos)
                {
                    ((ComponentMapStep_PositionFirstComp)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗芯片");
                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetColumnMap)
                {

                    ((ComponentMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列芯片");

                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetRowMap)
                {

                    ((ComponentMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行芯片");

                }
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetRowMap)
                {
                    this.btnNext.Visible = false;
                }
                else
                {
                    this.btnNext.Visible = true;
                    this.btnNext.Text = "下一步";
                }
            }
            else
            {

                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeThirdPoint)
                {
                    this.btnNext.Visible = false;
                }
                else
                {
                    this.btnNext.Visible = true;
                    this.btnNext.Text = "下一步";
                }
            }

            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }
        private void LoadNextStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeComponentMapStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Component_MaterialMap);
                if (_editRecipe.CurrentComponent.CarrierType != EnumCarrierType.Wafer)
                {
                    if (step != EnumDefineSetupRecipeComponentMapStep.SetRowMap)
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
                    if (step != EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeThirdPoint)
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
                
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new ComponentMapStep_PositionFirstComp();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
                UpdateStepSignStatus();
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (_editRecipe.CurrentComponent.CarrierType != EnumCarrierType.Wafer)
            {
                if(currentStepPage.CurrentStep==EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos)
                {
                    ((ComponentMapStep_PositionFirstComp)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗芯片");
                }
                else if(currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetColumnMap)
                {

                     ((ComponentMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列芯片");

                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetRowMap)
                {

                    ((ComponentMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行芯片");

                }
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetRowMap)
                {
                    //this.btnNext.Visible = false;
                    this.btnNext.Text = "完成";
                }
                else
                {
                    this.btnNext.Visible = true;
                    this.btnNext.Text = "下一步";
                }
            }
            else
            {
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeThirdPoint)
                {
                    //this.btnNext.Visible = false;
                    this.btnNext.Text = "完成";
                }
                else
                {
                    this.btnNext.Visible = true;
                    this.btnNext.Text = "下一步";
                }
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }

        private ComponentMapStepBasePage GenerateStepPage(EnumDefineSetupRecipeComponentMapStep step)
        {
            var ret = new ComponentMapStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeComponentMapStep.None:
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos:
                    ret = new ComponentMapStep_PositionFirstComp();
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetColumnMap:
                    ret = new ComponentMapStep_ColumnParam();
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetRowMap:
                    ret = new ComponentMapStep_RowParam();
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeFirstPoint:
                    ret = new ComponentMapStep_SetWaferEdgePoint1();
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeSecondPoint:
                    ret = new ComponentMapStep_SetWaferEdgePoint2();
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeThirdPoint:
                    ret = new ComponentMapStep_SetWaferEdgePoint3();
                    break;
                default:
                    break;
            }
            return ret;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            LoadPreviousStepPage();
            UpdateStepSignStatus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetColumnMap)
            {
                FrmInputColumnCount frm = new FrmInputColumnCount();
                frm.StartPosition = FormStartPosition.CenterParent;
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    _numbersofColumns = frm.NumberofColumns;
                    LoadNextStepPage();
                    //UpdateStepSignStatus();
                }
            }
            else if(currentStepPage.CurrentStep == EnumDefineSetupRecipeComponentMapStep.SetRowMap)
            {
                FrmInputRowCount frm = new FrmInputRowCount();
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _numbersofRows = frm.NumberofRows;
                    LoadNextStepPage();
                    //UpdateStepSignStatus();
                }
            }
            else
            {
                LoadNextStepPage();
                //UpdateStepSignStatus();
            }
        }
        private void GenerateComponentMap()
        {
            if (_numbersofRows != 0 && _numbersofColumns != 0)
            {

                var mapAngle = Math.Atan((LastColumnComponentPosition.Y - FirstComponentPosition.Y) / (LastColumnComponentPosition.X - FirstComponentPosition.X));

                _rowPitchMM = (float)(Math.Abs((LastRowComponentPosition.Y - LastColumnComponentPosition.Y) * Math.Cos(mapAngle)) / (_numbersofRows - 1));
                _columnPitchMM = (float)(Math.Abs((LastColumnComponentPosition.X - FirstComponentPosition.X) * Math.Cos(mapAngle)) / (_numbersofColumns - 1));
                //var columnSpace = Math.Abs(LastRowComponentPosition.X - FirstComponentPosition.X) / (_numbersofColumns - 1);
                //var rowSpace = Math.Abs(LastColumnComponentPosition.Y - LastRowComponentPosition.Y) / (_numbersofRows - 1);
                //var componentWidth = _editRecipe.CurrentComponent.WidthMM;
                //var componentHeight = _editRecipe.CurrentComponent.HeightMM;
                var rotateCenterX = FirstComponentPosition.X;
                var rotateCenterY = FirstComponentPosition.Y;
                if (_rowPitchMM != 0 && _columnPitchMM != 0)
                {
                    _editRecipe.CurrentComponent.ComponentMapInfos.Clear();
                    var ID = 0;
                    for (int i = 0; i < _numbersofColumns; i++)
                    {
                        for (int j = 0; j < _numbersofRows; j++)
                        {
                            MaterialMapInformation material = new MaterialMapInformation();
                            var xNormanl = i* _columnPitchMM;
                            var yNormanl = j * _rowPitchMM;
                            //var centerX = rotateCenterX + (xNormanl - rotateCenterX) * Math.Cos(mapAngle) - (yNormanl - rotateCenterY) * Math.Sin(mapAngle); 
                            //var centerY = rotateCenterY + (xNormanl - rotateCenterX) * Math.Sin(mapAngle) + (yNormanl - rotateCenterY) * Math.Cos(mapAngle);
                            var centerX = rotateCenterX + (xNormanl) * Math.Cos(mapAngle) - (yNormanl) * Math.Sin(mapAngle);
                            var centerY = rotateCenterY + (xNormanl) * Math.Sin(mapAngle) - (yNormanl) * Math.Cos(mapAngle);
                            material.MaterialLocation = new PointF() { X = (float)centerX, Y = (float)centerY };
                            material.MaterialCoordIndex = new Point(j, i);
                            material.MaterialNumber = ID++;
                            material.IsMaterialExist = true;
                            material.IsProcess = true;
                            material.Properties = MaterialProperties.Testable;
                            _editRecipe.CurrentComponent.ComponentMapInfos.Add(material);
                        }
                    }
                }
            }
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeComponentMapStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeComponentMapStep.None:
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos:
                    FirstComponentPosition = currentStepPage.FirstComponentPosition;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetColumnMap:
                    LastColumnComponentPosition = currentStepPage.LastColumnComponentPosition;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetRowMap:
                    LastRowComponentPosition = currentStepPage.LastRowComponentPosition;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeFirstPoint:
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeSecondPoint:
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeThirdPoint:
                    break;
                default:
                    break;
            }
        }
        private void UpdateStepSignStatus()
        {
            
            switch (currentStepPage.CurrentStep)
            {
                case EnumDefineSetupRecipeComponentMapStep.None:
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetFirstComponentPos:
                    step1Sign.Image = Properties.Resources.line_first;
                    step2Sign.Image = Properties.Resources.line_last_undo;
                    step3Sign.Image = Properties.Resources.the_last_undo;
                    step4Sign.Image = Properties.Resources.wafer_left_top_undo;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetColumnMap:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last;
                    step3Sign.Image = Properties.Resources.the_last_undo;
                    step4Sign.Image = Properties.Resources.wafer_left_top_undo;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetRowMap:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last;
                    step4Sign.Image = Properties.Resources.wafer_left_top_undo;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeFirstPoint:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last_done;
                    step4Sign.Image = Properties.Resources.wafer_left_top;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeSecondPoint:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last_done;
                    step4Sign.Image = Properties.Resources.wafer_left_top_done;
                    step5Sign.Image = Properties.Resources.wafer_right_top;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeComponentMapStep.SetDeterminWaferRangeThirdPoint:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last_done;
                    step4Sign.Image = Properties.Resources.wafer_left_top_done;
                    step5Sign.Image = Properties.Resources.wafer_right_top_done;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom;
                    break;
                default:
                    break;
            }


        }
        private void StepSignComplete()
        {
            step1Sign.Image = Properties.Resources.line_first_done;
            step2Sign.Image = Properties.Resources.line_last_done;
            step3Sign.Image = Properties.Resources.the_last_done;
            step4Sign.Image = Properties.Resources.wafer_left_top_done;
            step5Sign.Image = Properties.Resources.wafer_right_top_done;
            step6Sign.Image = Properties.Resources.wafer_right_bottom_done;
        }
    }
}
