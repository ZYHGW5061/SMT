using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using MathHelperClsLib;
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
    public partial class RecipeStep_SubstrateMapSettings : RecipeStepBase
    {
        private int _numbersofColumns;
        private int _numbersofRows;
        public float _rowPitchMM;
        public float _columnPitchMM;
        public PointF FirstMaterialPosition { get; set; }
        public PointF LastColumnMaterialPosition { get; set; }
        public PointF LastRowMaterialPosition { get; set; }
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        public RecipeStep_SubstrateMapSettings()
        {
            InitializeComponent();
        }
        private SubstrateMapStepBasePage currentStepPage;
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
            _numbersofRows= _editRecipe.SubstrateInfos.RowCount;
            _numbersofColumns= _editRecipe.SubstrateInfos.ColumnCount;
            _columnPitchMM= _editRecipe.SubstrateInfos.PitchColumnMM;
            _rowPitchMM= _editRecipe.SubstrateInfos.PitchRowMM;

            FirstMaterialPosition=new PointF((float)_editRecipe.SubstrateInfos.FirstSubstrateHomeSystemLocation.X, (float)_editRecipe.SubstrateInfos.FirstSubstrateHomeSystemLocation.Y);
            if (_editRecipe.SubstrateInfos.CarrierType != EnumCarrierType.Wafer)
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

            currengStep = EnumRecipeStep.Substrate_MaterialMap;
            GenerateMap();
            _editRecipe.SubstrateInfos.RowCount = _numbersofRows;
            _editRecipe.SubstrateInfos.ColumnCount = _numbersofColumns;
            _editRecipe.SubstrateInfos.PitchColumnMM = _columnPitchMM;
            _editRecipe.SubstrateInfos.PitchRowMM = _rowPitchMM;

            _editRecipe.SubstrateInfos.FirstSubstrateHomeSystemLocation.X = FirstMaterialPosition.X;
            _editRecipe.SubstrateInfos.FirstSubstrateHomeSystemLocation.Y = FirstMaterialPosition.Y;
            _editRecipe.SubstrateInfos.IsMaterialMapSettingsComplete = true;
            finished = true;
        }
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
            //if (_editRecipe.SubmonutInfos.PositionSubmountVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(0);
            //}
            //else if (_editRecipe.SubmonutInfos.PositionSubmountVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            //{
            //    CameraWindowGUI.Instance.SelectCamera(2);
            //}

            var cameraWnd = new CameraWindowGUI();
            cameraWnd.InitVisualControl();
            cameraWnd.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(cameraWnd);
            if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            {
                cameraWnd.SelectCamera(0);
            }
            else if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            {
                cameraWnd.SelectCamera(2);
            }
            else
            {
                cameraWnd.SelectCamera(0);
            }
        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefineSetupRecipeSubstrateMapStep.None;
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
                currentStepPage = new SubstrateMapStep_PositionFirstSubstrate();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubmountMapStep.SetRowMap)
            //{
            //    this.btnNext.Visible = false;
            //}
            //else
            //{
            //    this.btnNext.Visible = true;
            //}
            if (_editRecipe.SubstrateInfos.CarrierType != EnumCarrierType.Wafer)
            {
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos)
                {
                    ((SubstrateMapStep_PositionFirstSubstrate)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗基板");
                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
                {

                    ((SubstrateMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列基板");

                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
                {

                    ((SubstrateMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行基板");

                }
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
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
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint)
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
                var step = EnumDefineSetupRecipeSubstrateMapStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                _editRecipe.SaveRecipe(EnumRecipeStep.Substrate_MaterialMap);
                //this.panelStepOperate.Controls.Clear();
                //currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //currentStepPage.Dock = DockStyle.Fill;
                //this.panelStepOperate.Controls.Add(currentStepPage);

                if (_editRecipe.SubstrateInfos.CarrierType == EnumCarrierType.Wafer)
                {
                    if (step != EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint)
                    {
                        this.panelStepOperate.Controls.Clear();
                        currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                        currentStepPage.Dock = DockStyle.Fill;
                        this.panelStepOperate.Controls.Add(currentStepPage);
                    }
                    else
                    {
                        StepSignComplete();
                    }
                }
                else
                {
                    if (step != EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
                    {
                        this.panelStepOperate.Controls.Clear();
                        currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                        currentStepPage.Dock = DockStyle.Fill;
                        this.panelStepOperate.Controls.Add(currentStepPage);
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
                currentStepPage = new SubstrateMapStep_PositionFirstSubstrate();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            currentStepPage.LoadEditedRecipe(_editRecipe);
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }

            if (_editRecipe.SubstrateInfos.CarrierType != EnumCarrierType.Wafer)
            {
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos)
                {
                    ((SubstrateMapStep_PositionFirstSubstrate)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗基板");
                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
                {

                    ((SubstrateMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列基板");

                }
                else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
                {

                    ((SubstrateMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行基板");

                }
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
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
                if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint)
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
            //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubmountMapStep.SetRowMap)
            //{
            //    this.btnNext.Visible = false;
            //}
            //else
            //{
            //    this.btnNext.Visible = true;
            //}
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }

        private SubstrateMapStepBasePage GenerateStepPage(EnumDefineSetupRecipeSubstrateMapStep step)
        {
            var ret = new SubstrateMapStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeSubstrateMapStep.None:
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos:
                    ret = new SubstrateMapStep_PositionFirstSubstrate();
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap:
                    ret = new SubstrateMapStep_ColumnParam();
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetRowMap:
                    ret = new SubstrateMapStep_RowParam();
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
            if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
            {
                FrmInputColumnCount frm = new FrmInputColumnCount();
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _numbersofColumns = frm.NumberofColumns;
                    LoadNextStepPage();
                    UpdateStepSignStatus();
                }
            }
            else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
            {
                FrmInputRowCount frm = new FrmInputRowCount();
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _numbersofRows = frm.NumberofRows;
                    LoadNextStepPage();
                    UpdateStepSignStatus();
                    StepSignComplete();
                }
            }
            else
            {
                LoadNextStepPage();
                UpdateStepSignStatus();
            }
        }
        private void GenerateMap()
        {
            if (_numbersofRows != 0 && _numbersofColumns != 0)
            {
                //_rowPitchMM = (LastRowMaterialPosition.Y - FirstMaterialPosition.Y) / (_numbersofRows-1);
                //_columnPitchMM = (LastColumnMaterialPosition.X - FirstMaterialPosition.X) / (_numbersofColumns - 1);
                //if (_rowPitchMM != 0 && _columnPitchMM != 0)
                //{
                //    _editRecipe.SubmonutInfos.SubmountMapInfos.Clear();
                //    var ID = 0;
                //    for (int i = 0; i < _numbersofColumns; i++)
                //    {
                //        for (int j = 0; j < _numbersofRows; j++)
                //        {
                //            MaterialMapInformation material = new MaterialMapInformation();
                //            var centerX = i * _columnPitchMM;
                //            var centerY = j * _rowPitchMM;
                //            material.MaterialLocation = new PointF() { X = centerX, Y = centerY };
                //            material.MaterialCoordIndex = new Point(j, i);
                //            material.MaterialNumber = ID++;
                //            material.IsMaterialExist = true;
                //            material.IsProcess = true;
                //            material.Properties = MaterialProperties.Testable;
                //            _editRecipe.SubmonutInfos.SubmountMapInfos.Add(material);
                //        }
                //    }
                //}
                var mapAngle = 0d;
                if (_numbersofColumns != 1 && _numbersofRows != 1)
                {
                    _editRecipe.SubstrateInfos.SubstrateMapInfos.Clear();
                    mapAngle = Math.Atan((LastColumnMaterialPosition.Y - FirstMaterialPosition.Y) / (LastColumnMaterialPosition.X - FirstMaterialPosition.X));

                    _rowPitchMM = (float)(Math.Abs((LastRowMaterialPosition.Y - LastColumnMaterialPosition.Y) * Math.Cos(mapAngle)) / (_numbersofRows - 1));
                    _columnPitchMM = (float)(Math.Abs((LastColumnMaterialPosition.X - FirstMaterialPosition.X) * Math.Cos(mapAngle)) / (_numbersofColumns - 1));
                    //var columnSpace = Math.Abs(LastRowComponentPosition.X - FirstComponentPosition.X) / (_numbersofColumns - 1);
                    //var rowSpace = Math.Abs(LastColumnComponentPosition.Y - LastRowComponentPosition.Y) / (_numbersofRows - 1);
                    //var componentWidth = _editRecipe.CurrentComponent.WidthMM;
                    //var componentHeight = _editRecipe.CurrentComponent.HeightMM;
                    var rotateCenterX = FirstMaterialPosition.X;
                    var rotateCenterY = FirstMaterialPosition.Y;
                    if (_rowPitchMM != 0 && _columnPitchMM != 0)
                    {
                        _editRecipe.SubstrateInfos.SubstrateMapInfos.Clear();
                        var ID = 0;
                        for (int i = 0; i < _numbersofColumns; i++)
                        {
                            for (int j = 0; j < _numbersofRows; j++)
                            {
                                MaterialMapInformation material = new MaterialMapInformation();
                                var xNormanl = i * _columnPitchMM;
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
                                _editRecipe.SubstrateInfos.SubstrateMapInfos.Add(material);
                            }
                        }
                    }
                }
                else
                {
                    _editRecipe.SubstrateInfos.SubstrateMapInfos.Clear();
                    MaterialMapInformation material = new MaterialMapInformation();

                    material.MaterialLocation = new PointF() { X = (float)FirstMaterialPosition.X, Y = (float)FirstMaterialPosition.Y };
                    material.MaterialCoordIndex = new Point(1, 1);
                    material.MaterialNumber = 1;
                    material.IsMaterialExist = true;
                    material.IsProcess = true;
                    material.Properties = MaterialProperties.Testable;
                    _editRecipe.SubstrateInfos.SubstrateMapInfos.Add(material);
                }
            }
        }
        private void SaveStepParametersWhenStepFinished(EnumDefineSetupRecipeSubstrateMapStep step)
        {
            switch (step)
            {
                case EnumDefineSetupRecipeSubstrateMapStep.None:
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos:
                    FirstMaterialPosition = currentStepPage.FirstSubmountPosition;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap:
                    LastColumnMaterialPosition = currentStepPage.LastColumnSubmountPosition;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetRowMap:
                    LastRowMaterialPosition = currentStepPage.LastRowSubmountPosition;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeFirstPoint:

                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeSecondPoint:

                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint:

                    break;
                default:
                    break;
            }
        }
        private void UpdateStepSignStatus()
        {

            switch (currentStepPage.CurrentStep)
            {
                case EnumDefineSetupRecipeSubstrateMapStep.None:
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos:
                    step1Sign.Image = Properties.Resources.line_first;
                    step2Sign.Image = Properties.Resources.line_last_undo;
                    step3Sign.Image = Properties.Resources.the_last_undo;
                    step4Sign.Image = Properties.Resources.wafer_left_top_undo;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last;
                    step3Sign.Image = Properties.Resources.the_last_undo;
                    step4Sign.Image = Properties.Resources.wafer_left_top_undo;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetRowMap:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last;
                    step4Sign.Image = Properties.Resources.wafer_left_top_undo;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeFirstPoint:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last_done;
                    step4Sign.Image = Properties.Resources.wafer_left_top;
                    step5Sign.Image = Properties.Resources.wafer_right_top_undo;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeSecondPoint:
                    step1Sign.Image = Properties.Resources.line_first_done;
                    step2Sign.Image = Properties.Resources.line_last_done;
                    step3Sign.Image = Properties.Resources.the_last_done;
                    step4Sign.Image = Properties.Resources.wafer_left_top_done;
                    step5Sign.Image = Properties.Resources.wafer_right_top;
                    step6Sign.Image = Properties.Resources.wafer_right_bottom_undo;
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint:
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
