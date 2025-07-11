﻿using ConfigurationClsLib;
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
    public partial class RecipeStep_ModuleMapSettings : RecipeStepBase
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
        public RecipeStep_ModuleMapSettings()
        {
            InitializeComponent();
        }
        private ModuleMapStepBasePage currentStepPage;
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
            //_numbersofRows= _editRecipe.SubstrateInfos.RowCount;
            //_numbersofColumns= _editRecipe.SubstrateInfos.ColumnCount;
            //_columnPitchMM= _editRecipe.SubstrateInfos.PitchColumnMM;
            //_rowPitchMM= _editRecipe.SubstrateInfos.PitchRowMM;

            //FirstMaterialPosition=new PointF((float)_editRecipe.SubstrateInfos.FirstModuleHomeSystemLocation.X, (float)_editRecipe.SubstrateInfos.FirstModuleHomeSystemLocation.Y);
            //if (_editRecipe.SubstrateInfos.CarrierType != EnumCarrierType.Wafer)
            //{
            //    step4Sign.Visible = false;
            //    step5Sign.Visible = false;
            //    step6Sign.Visible = false;
            //}

            _numbersofRows = _editRecipe.CurrentSubstrate.RowCount;
            _numbersofColumns = _editRecipe.CurrentSubstrate.ColumnCount;
            _columnPitchMM = _editRecipe.CurrentSubstrate.PitchColumnMM;
            _rowPitchMM = _editRecipe.CurrentSubstrate.PitchRowMM;

            FirstMaterialPosition = new PointF((float)_editRecipe.CurrentSubstrate.FirstModuleHomeSystemLocation.X, (float)_editRecipe.CurrentSubstrate.FirstModuleHomeSystemLocation.Y);
            if (_editRecipe.CurrentSubstrate.CarrierType != EnumCarrierType.Wafer)
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

            currengStep = EnumRecipeStep.Module_MaterialMap;
            GenerateMap();
            //_editRecipe.SubstrateInfos.ModuleRowCount = _numbersofRows;
            //_editRecipe.SubstrateInfos.ModuleColumnCount = _numbersofColumns;
            //_editRecipe.SubstrateInfos.ModulePitchColumnMM = _columnPitchMM;
            //_editRecipe.SubstrateInfos.ModulePitchRowMM = _rowPitchMM;

            //_editRecipe.SubstrateInfos.FirstModuleHomeSystemLocation.X = FirstMaterialPosition.X;
            //_editRecipe.SubstrateInfos.FirstModuleHomeSystemLocation.Y = FirstMaterialPosition.Y;
            //_editRecipe.SubstrateInfos.IsModuleMapSettingsComplete = true;

            _editRecipe.CurrentSubstrate.ModuleRowCount = _numbersofRows;
            _editRecipe.CurrentSubstrate.ModuleColumnCount = _numbersofColumns;
            _editRecipe.CurrentSubstrate.ModulePitchColumnMM = _columnPitchMM;
            _editRecipe.CurrentSubstrate.ModulePitchRowMM = _rowPitchMM;

            _editRecipe.CurrentSubstrate.FirstModuleHomeSystemLocation.X = FirstMaterialPosition.X;
            _editRecipe.CurrentSubstrate.FirstModuleHomeSystemLocation.Y = FirstMaterialPosition.Y;
            _editRecipe.CurrentSubstrate.IsModuleMapSettingsComplete = true;

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
            //if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            //{
            //    cameraWnd.SelectCamera(0);
            //}
            //else if (_editRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
            //{
            //    cameraWnd.SelectCamera(2);
            //}
            //else
            //{
            //    cameraWnd.SelectCamera(0);
            //}
            if (_editRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
            {
                cameraWnd.SelectCamera(0);
            }
            else if (_editRecipe.CurrentSubstrate.PositionSustrateVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
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
                currentStepPage = new ModuleMapStep_PositionFirstModule();

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
            //if (_editRecipe.SubstrateInfos.CarrierType != EnumCarrierType.Wafer)
            //{
            //    //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstSubmountPos)
            //    //{
            //    //    ((SubstrateMapStep_PositionFirstSubstrate)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗衬底");
            //    //}
            //    //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
            //    //{

            //    //    ((SubstrateMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列衬底");

            //    //}
            //    //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
            //    //{

            //    //    ((SubstrateMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行衬底");

            //    //}
            //    if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
            //    {
            //        this.btnNext.Visible = false;
            //    }
            //    else
            //    {
            //        this.btnNext.Visible = true;
            //        this.btnNext.Text = "下一步";
            //    }
            //}
            //else
            //{
            //    if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint)
            //    {
            //        this.btnNext.Visible = false;
            //    }
            //    else
            //    {
            //        this.btnNext.Visible = true;
            //        this.btnNext.Text = "下一步";
            //    }
            //}

            if (_editRecipe.CurrentSubstrate.CarrierType != EnumCarrierType.Wafer)
            {
                //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstSubmountPos)
                //{
                //    ((SubstrateMapStep_PositionFirstSubstrate)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗衬底");
                //}
                //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
                //{

                //    ((SubstrateMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列衬底");

                //}
                //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
                //{

                //    ((SubstrateMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行衬底");

                //}
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

                //if (_editRecipe.SubstrateInfos.CarrierType == EnumCarrierType.Wafer)
                //{
                //    if (step != EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint)
                //    {
                //        this.panelStepOperate.Controls.Clear();
                //        currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //        currentStepPage.Dock = DockStyle.Fill;
                //        this.panelStepOperate.Controls.Add(currentStepPage);
                //    }
                //    else
                //    {
                //        StepSignComplete();
                //    }
                //}
                //else
                //{
                //    if (step != EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
                //    {
                //        this.panelStepOperate.Controls.Clear();
                //        currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                //        currentStepPage.Dock = DockStyle.Fill;
                //        this.panelStepOperate.Controls.Add(currentStepPage);
                //    }
                //    else
                //    {
                //        StepSignComplete();
                //    }
                //}

                if (_editRecipe.CurrentSubstrate.CarrierType == EnumCarrierType.Wafer)
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
                currentStepPage = new ModuleMapStep_PositionFirstModule();

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

            //if (_editRecipe.SubstrateInfos.CarrierType != EnumCarrierType.Wafer)
            //{
            //    //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstSubmountPos)
            //    //{
            //    //    ((SubstrateMapStep_PositionFirstSubstrate)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗Module");
            //    //}
            //    //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
            //    //{

            //    //    ((SubstrateMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列Module");

            //    //}
            //    //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
            //    //{

            //    //    ((SubstrateMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行Module");

            //    //}
            //    if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
            //    {
            //        //this.btnNext.Visible = false;
            //        this.btnNext.Text = "完成";
            //    }
            //    else
            //    {
            //        this.btnNext.Visible = true;
            //        this.btnNext.Text = "下一步";
            //    }
            //}
            //else
            //{
            //    if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetDeterminWaferRangeThirdPoint)
            //    {
            //        //this.btnNext.Visible = false;
            //        this.btnNext.Text = "完成";
            //    }
            //    else
            //    {
            //        this.btnNext.Visible = true;
            //        this.btnNext.Text = "下一步";
            //    }
            //}

            if (_editRecipe.CurrentSubstrate.CarrierType != EnumCarrierType.Wafer)
            {
                //if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetFirstSubmountPos)
                //{
                //    ((SubstrateMapStep_PositionFirstSubstrate)currentStepPage).SetStepInfo("步骤 1/3：Map设定：定位到第一颗Module");
                //}
                //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap)
                //{

                //    ((SubstrateMapStep_ColumnParam)currentStepPage).SetStepInfo("步骤 2/3：Map设定：定位到最后一列Module");

                //}
                //else if (currentStepPage.CurrentStep == EnumDefineSetupRecipeSubstrateMapStep.SetRowMap)
                //{

                //    ((SubstrateMapStep_RowParam)currentStepPage).SetStepInfo("步骤 3/3：Map设定：定位到最后一行Module");

                //}
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

        private ModuleMapStepBasePage GenerateStepPage(EnumDefineSetupRecipeSubstrateMapStep step)
        {
            var ret = new ModuleMapStepBasePage();
            switch (step)
            {
                case EnumDefineSetupRecipeSubstrateMapStep.None:
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetFirstMaterialPos:
                    ret = new ModuleMapStep_PositionFirstModule();
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetColumnMap:
                    ret = new ModuleMapStep_ColumnParam();
                    break;
                case EnumDefineSetupRecipeSubstrateMapStep.SetRowMap:
                    ret = new ModuleMapStep_RowParam();
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
        /// <summary>
        /// 生成Module MAP信息(基于第一个Substrate，基于Substrate坐标)
        /// </summary>
        private void GenerateMap()
        {
            //if (_numbersofRows != 0 && _numbersofColumns != 0)
            //{

            //    var mapAngle = Math.Atan((LastColumnMaterialPosition.Y - FirstMaterialPosition.Y) / (LastColumnMaterialPosition.X - FirstMaterialPosition.X));

            //    _rowPitchMM = (float)(Math.Abs((LastRowMaterialPosition.Y - LastColumnMaterialPosition.Y) * Math.Cos(mapAngle)) / (_numbersofRows - 1));
            //    _columnPitchMM = (float)(Math.Abs((LastColumnMaterialPosition.X - FirstMaterialPosition.X) * Math.Cos(mapAngle)) / (_numbersofColumns - 1));
            //    //var columnSpace = Math.Abs(LastRowComponentPosition.X - FirstComponentPosition.X) / (_numbersofColumns - 1);
            //    //var rowSpace = Math.Abs(LastColumnComponentPosition.Y - LastRowComponentPosition.Y) / (_numbersofRows - 1);
            //    //var componentWidth = _editRecipe.CurrentComponent.WidthMM;
            //    //var componentHeight = _editRecipe.CurrentComponent.HeightMM;
            //    var substrateCoorHomeX = _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.X;
            //    var substrateCoorHomeY = _editRecipe.SubstrateInfos.SubstrateCoordinateHomePoint.Y;
            //    //var offsetXModuleAndSubstrate = _editRecipe.SubstrateInfos.SubstrateMapInfos.FirstOrDefault().MaterialLocation.X - FirstMaterialPosition.X;
            //    //var offsetYModuleAndSubstrate = _editRecipe.SubstrateInfos.SubstrateMapInfos.FirstOrDefault().MaterialLocation.Y - FirstMaterialPosition.Y;

            //    //Module中心和寻找Map标记点的偏移
            //    var centerOffsetX = _editRecipe.SubstrateInfos.FirstModuleCenterSystemLocation.X - FirstMaterialPosition.X;
            //    var centerOffsetY = _editRecipe.SubstrateInfos.FirstModuleCenterSystemLocation.Y - FirstMaterialPosition.Y;

            //    var firstModuleRelativeCoorX = FirstMaterialPosition.X - substrateCoorHomeX;
            //    var firstModuleRelativeCoorY = FirstMaterialPosition.Y - substrateCoorHomeY;

            //    foreach (var item in _editRecipe.SubstrateInfos.SubstrateMapInfos)
            //    {
            //        var rotateCenterX = item.MaterialLocation.X- substrateCoorHomeX;
            //        var rotateCenterY = item.MaterialLocation.Y- substrateCoorHomeY;

            //        List<MaterialMapInformation> temp = new List<MaterialMapInformation>();
            //        if (_rowPitchMM != 0 && _columnPitchMM != 0)
            //        {
            //            _editRecipe.SubstrateInfos.ModuleMapInfos.Clear();
            //            var ID = 0;
            //            for (int i = 0; i < _numbersofColumns; i++)
            //            {
            //                for (int j = 0; j < _numbersofRows; j++)
            //                {
            //                    MaterialMapInformation material = new MaterialMapInformation();
            //                    var xNormanl = i * _columnPitchMM;
            //                    var yNormanl = j * _rowPitchMM;
            //                    //var centerX = rotateCenterX + (xNormanl - rotateCenterX) * Math.Cos(mapAngle) - (yNormanl - rotateCenterY) * Math.Sin(mapAngle);
            //                    //var centerY = rotateCenterY + (xNormanl - rotateCenterX) * Math.Sin(mapAngle) + (yNormanl - rotateCenterY) * Math.Cos(mapAngle);
            //                    //Map记录的是Substrate坐标系下的module中心的系统坐标系；
            //                    var centerX = firstModuleRelativeCoorX+rotateCenterX+ centerOffsetX + (xNormanl) * Math.Cos(mapAngle) - (yNormanl) * Math.Sin(mapAngle);
            //                    var centerY = firstModuleRelativeCoorY + rotateCenterY + centerOffsetY + (xNormanl) * Math.Sin(mapAngle) - (yNormanl) * Math.Cos(mapAngle);
            //                    material.MaterialLocation = new PointF() { X = (float)centerX, Y = (float)centerY };
            //                    material.MaterialCoordIndex = new Point(j, i);
            //                    material.MaterialNumber = ID++;
            //                    material.IsMaterialExist = true;
            //                    material.IsProcess = true;
            //                    material.Properties = MaterialProperties.Testable;
            //                    temp.Add(material);

            //                }
            //            }
            //        }
            //        //List<Dictionary<MaterialMapInformation, List<BondingPositionSettings>>> tempList = new List<Dictionary<MaterialMapInformation, List<BondingPositionSettings>>>();
            //        //foreach (var itemModule in temp)
            //        //{
            //        //    var newItem = new Dictionary<MaterialMapInformation, List<BondingPositionSettings>>();
            //        //    newItem.Add(itemModule, new List<BondingPositionSettings>());
            //        //    tempList.Add(newItem);
            //        //}
            //        _editRecipe.SubstrateInfos.ModuleMapInfos.Add(temp);
            //    }
            //}

            if (_numbersofRows != 0 && _numbersofColumns != 0)
            {

                var mapAngle = Math.Atan((LastColumnMaterialPosition.Y - FirstMaterialPosition.Y) / (LastColumnMaterialPosition.X - FirstMaterialPosition.X));

                _rowPitchMM = (float)(Math.Abs((LastRowMaterialPosition.Y - LastColumnMaterialPosition.Y) * Math.Cos(mapAngle)) / (_numbersofRows - 1));
                _columnPitchMM = (float)(Math.Abs((LastColumnMaterialPosition.X - FirstMaterialPosition.X) * Math.Cos(mapAngle)) / (_numbersofColumns - 1));
                //var columnSpace = Math.Abs(LastRowComponentPosition.X - FirstComponentPosition.X) / (_numbersofColumns - 1);
                //var rowSpace = Math.Abs(LastColumnComponentPosition.Y - LastRowComponentPosition.Y) / (_numbersofRows - 1);
                //var componentWidth = _editRecipe.CurrentComponent.WidthMM;
                //var componentHeight = _editRecipe.CurrentComponent.HeightMM;
                var substrateCoorHomeX = _editRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.X;
                var substrateCoorHomeY = _editRecipe.CurrentSubstrate.SubstrateCoordinateHomePoint.Y;
                //var offsetXModuleAndSubstrate = _editRecipe.CurrentSubstrate.SubstrateMapInfos.FirstOrDefault().MaterialLocation.X - FirstMaterialPosition.X;
                //var offsetYModuleAndSubstrate = _editRecipe.CurrentSubstrate.SubstrateMapInfos.FirstOrDefault().MaterialLocation.Y - FirstMaterialPosition.Y;

                //Module中心和寻找Map标记点的偏移
                var centerOffsetX = _editRecipe.CurrentSubstrate.FirstModuleCenterSystemLocation.X - FirstMaterialPosition.X;
                var centerOffsetY = _editRecipe.CurrentSubstrate.FirstModuleCenterSystemLocation.Y - FirstMaterialPosition.Y;

                var firstModuleRelativeCoorX = FirstMaterialPosition.X - substrateCoorHomeX;
                var firstModuleRelativeCoorY = FirstMaterialPosition.Y - substrateCoorHomeY;

                foreach (var item in _editRecipe.CurrentSubstrate.SubstrateMapInfos)
                {
                    var rotateCenterX = item.MaterialLocation.X - substrateCoorHomeX;
                    var rotateCenterY = item.MaterialLocation.Y - substrateCoorHomeY;

                    List<MaterialMapInformation> temp = new List<MaterialMapInformation>();
                    if (_rowPitchMM != 0 && _columnPitchMM != 0)
                    {
                        _editRecipe.CurrentSubstrate.ModuleMapInfos.Clear();
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
                                //Map记录的是Substrate坐标系下的module中心的系统坐标系；
                                var centerX = firstModuleRelativeCoorX + rotateCenterX + centerOffsetX + (xNormanl) * Math.Cos(mapAngle) - (yNormanl) * Math.Sin(mapAngle);
                                var centerY = firstModuleRelativeCoorY + rotateCenterY + centerOffsetY + (xNormanl) * Math.Sin(mapAngle) - (yNormanl) * Math.Cos(mapAngle);
                                material.MaterialLocation = new PointF() { X = (float)centerX, Y = (float)centerY };
                                material.MaterialCoordIndex = new Point(j, i);
                                material.MaterialNumber = ID++;
                                material.IsMaterialExist = true;
                                material.IsProcess = true;
                                material.Properties = MaterialProperties.Testable;
                                temp.Add(material);

                            }
                        }
                    }
                    //List<Dictionary<MaterialMapInformation, List<BondingPositionSettings>>> tempList = new List<Dictionary<MaterialMapInformation, List<BondingPositionSettings>>>();
                    //foreach (var itemModule in temp)
                    //{
                    //    var newItem = new Dictionary<MaterialMapInformation, List<BondingPositionSettings>>();
                    //    newItem.Add(itemModule, new List<BondingPositionSettings>());
                    //    tempList.Add(newItem);
                    //}
                    _editRecipe.CurrentSubstrate.ModuleMapInfos.Add(temp);
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
