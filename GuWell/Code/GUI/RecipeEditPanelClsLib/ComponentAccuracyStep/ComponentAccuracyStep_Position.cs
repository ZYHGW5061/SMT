using CameraControllerClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using PositioningSystemClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionControlAppClsLib;
using VisionGUI;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class ComponentAccuracyStep_Position : ComponentAccuracyStepBasePage
    {
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private VisionControlAppClsLib.VisualControlManager _VisualManager
        {
            get { return VisionControlAppClsLib.VisualControlManager.Instance; }
        }

        public VisualControlApplications BondCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.BondCamera); }
        }
        public VisualControlApplications UplookingCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        public VisualControlApplications WaferCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.WaferCamera); }
        }
        public ComponentAccuracyStep_Position(CameraWindowGUI cameraWnd)
        {
            UsedCameraWnd = cameraWnd;
            InitializeComponent();
        }
        public override EnumDefineSetupRecipeComponentAccuracyStep CurrentStep
        {
            get
            { return EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition; }
        }

        public int RingLightintensity { get; set; }

        /// <summary>
        /// 直光强度
        /// </summary>
        public int DirectLightintensity { get; set; }

        /// <summary>
        /// 识别分数
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// 角度范围
        /// </summary>
        public int AngleRange { get; set; }
        private CameraManager _cameraManager
        {
            get { return CameraManager.Instance; }
        }
        private VisionGUI.VisualLineFindControlGUI visualLineFindControlGUI1;
        private VisionGUI.VisualMatchControlGUI visualMatchControlGUI1;
        /// <summary>
        ///  加载前一步定义的Recipe对象
        /// </summary>
        /// <param name="recipe"></param>
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    throw new Exception("Recipe is null when execute ComponentPositionStep_SizeLUCorner LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                InitialVisionTool();



            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }
        private void InitialVisionTool()
        {
            if(EditRecipe!=null)
            {
                var templateFolderName = $@"{_systemConfig.SystemDefaultDirectory}Recipes\Components\{EditRecipe.CurrentComponent.Name}\TemplateConfig\";
                CommonProcess.EnsureFolderExist(templateFolderName);
                if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod==EnumVisionPositioningMethod.EdgeSearch)
                {
                    this.visualLineFindControlGUI1 = new VisionGUI.VisualLineFindControlGUI();
                    this.visualLineFindControlGUI1.DirectLightintensity = 0;
                    this.visualLineFindControlGUI1.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.visualLineFindControlGUI1.DownEdgefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\DownEdgeFind.xml";
                    //this.visualLineFindControlGUI1.DownEdgeRoi = ((GlobalDataDefineClsLib.RectangleFV)(resources.GetObject("visualLineFindControlGUI1.DownEdgeRoi")));
                    this.visualLineFindControlGUI1.DownEdgeScore = 0;
                    this.visualLineFindControlGUI1.LeftEdgefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\LeftEdgeFind.xml";
                    //this.visualLineFindControlGUI1.LeftEdgeRoi = ((GlobalDataDefineClsLib.RectangleFV)(resources.GetObject("visualLineFindControlGUI1.LeftEdgeRoi")));
                    this.visualLineFindControlGUI1.LeftEdgeScore = 0;
                    this.visualLineFindControlGUI1.Location = new System.Drawing.Point(3, 3);
                    this.visualLineFindControlGUI1.MaxAngle = 15;
                    this.visualLineFindControlGUI1.MinAngle = -15;
                    this.visualLineFindControlGUI1.Name = "visualLineFindControlGUI1";
                    this.visualLineFindControlGUI1.RightEdgefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\RightEdgeFind.xml";
                    //this.visualLineFindControlGUI1.RightEdgeRoi = ((GlobalDataDefineClsLib.RectangleFV)(resources.GetObject("visualLineFindControlGUI1.RightEdgeRoi")));
                    this.visualLineFindControlGUI1.RightEdgeScore = 0;
                    this.visualLineFindControlGUI1.RingLightintensity = 0;
                    this.visualLineFindControlGUI1.Score = 0.5F;
                    this.visualLineFindControlGUI1.Size = new System.Drawing.Size(339, 560);
                    this.visualLineFindControlGUI1.TabIndex = 0;
                    this.visualLineFindControlGUI1.UpEdgefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\UpEdgeFind.xml";
                    //this.visualLineFindControlGUI1.UpEdgeRoi = ((GlobalDataDefineClsLib.RectangleFV)(resources.GetObject("visualLineFindControlGUI1.UpEdgeRoi")));
                    this.visualLineFindControlGUI1.UpEdgeScore = 0;
                    this.tabPage1.Controls.Add(this.visualLineFindControlGUI1);


                    if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                    {
                        visualLineFindControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                        LineFindIdentificationParam param = new LineFindIdentificationParam();
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                        visualLineFindControlGUI1.SetVisualParam(param);
                    }
                    else if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    {
                        visualLineFindControlGUI1.InitVisualControl(UsedCameraWnd, UplookingCameraVisual);
                        LineFindIdentificationParam param = new LineFindIdentificationParam();
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.LookupRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.LookupDirectLightConfig.ChannelNumber);
                        visualLineFindControlGUI1.SetVisualParam(param);
                    }

                    var upEdgeSearchName = Path.Combine(templateFolderName, $"SearchComponentUpEdgeVisionParameter_{EditRecipe.CurrentComponent.Name}.xml");
                    this.visualLineFindControlGUI1.UpEdgefilepath = upEdgeSearchName;

                    var rightEdgeSearchName = Path.Combine(templateFolderName, $"SearchComponentRightEdgeVisionParameter_{EditRecipe.CurrentComponent.Name}.xml");
                    this.visualLineFindControlGUI1.RightEdgefilepath = rightEdgeSearchName;

                    var downEdgeSearchName = Path.Combine(templateFolderName, $"SearchComponentDownEdgeVisionParameter_{EditRecipe.CurrentComponent.Name}.xml");
                    this.visualLineFindControlGUI1.DownEdgefilepath = downEdgeSearchName;

                    var leftEdgeSearchName = Path.Combine(templateFolderName, $"SearchComponentLeftEdgeVisionParameter_{EditRecipe.CurrentComponent.Name}.xml");
                    this.visualLineFindControlGUI1.LeftEdgefilepath = leftEdgeSearchName;
                }
                else if(EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    this.visualMatchControlGUI1 = new VisionGUI.VisualMatchControlGUI();
                    VisionClsLib.MatchTemplateResult matchTemplateResult1 = new VisionClsLib.MatchTemplateResult();
                    this.visualMatchControlGUI1.AngleRange = 15;
                    this.visualMatchControlGUI1.DirectLightintensity = 0;
                    this.visualMatchControlGUI1.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.visualMatchControlGUI1.Location = new System.Drawing.Point(3, 3);
                    this.visualMatchControlGUI1.MatchRunfilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchRun.contourmxml";
                    this.visualMatchControlGUI1.MatchTemplatefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchTemplate.contourmxml";
                    this.visualMatchControlGUI1.Name = "visualMatchControlGUI1";
                    this.visualMatchControlGUI1.OutlineAngle = 0F;
                    this.visualMatchControlGUI1.RingLightintensity = 0;
                    this.visualMatchControlGUI1.Score = 0.5F;
                    //this.visualMatchControlGUI1.SearchRoi = ((RectangleFV)(resources.GetObject("visualMatchControlGUI1.SearchRoi")));
                    this.visualMatchControlGUI1.Size = new System.Drawing.Size(339, 560);
                    this.visualMatchControlGUI1.TabIndex = 0;
                    this.visualMatchControlGUI1.Templateresult = matchTemplateResult1;
                    this.tabPage1.Controls.Add(this.visualMatchControlGUI1);

                    if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                    {
                        visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                        MatchIdentificationParam param = new MatchIdentificationParam();
                        param.DirectLightType = EnumDirectLightSourceType.RGB;
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                        visualMatchControlGUI1.SetVisualParam(param);
                    }
                    else if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    {
                        visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, UplookingCameraVisual);
                        MatchIdentificationParam param = new MatchIdentificationParam();
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.LookupRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.LookupDirectLightConfig.ChannelNumber);
                        visualMatchControlGUI1.SetVisualParam(param);
                    }
                    //var templateTrainFileName = Path.Combine(templateFolderName, $"Accuracy_PositionTemplateFile_{EditRecipe.CurrentComponent.Name}.contourmxml");
                    //this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                    //this.visualMatchControlGUI1.MatchRunfilepath = "";
                    var templateTrainFileName = Path.Combine(templateFolderName, $"Accuracy_PositionTemplateFile_{EditRecipe.CurrentBondPosition.Name}.contourmxml");
                    var templateTrainParamName = Path.Combine(templateFolderName, $"Accuracy_PositionTemplateFile_{EditRecipe.CurrentBondPosition.Name}TrainParam.xml");
                    var templateRunFileName = Path.Combine(templateFolderName, $"Accuracy_PositionTemplateFile_{EditRecipe.CurrentBondPosition.Name}Run.xml");
                    this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                    this.visualMatchControlGUI1.MatchTemplateParampath = templateTrainParamName;
                    this.visualMatchControlGUI1.MatchRunfilepath = templateRunFileName;
                }
            }
        }
        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeComponentAccuracyStep currentStep)
        {
            try
            {
                currentStep = EnumDefineSetupRecipeComponentAccuracyStep.AccuracyPosition;
                //Score = visualLineFindControlGUI1.Score;
                //RingLightintensity= visualLineFindControlGUI1.RingLightintensity;
                //DirectLightintensity= visualLineFindControlGUI1.DirectLightintensity;

                //LineFindIdentificationParam upEdgeParam = new LineFindIdentificationParam();
                if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    LineFindIdentificationParam upEdgeParam = visualLineFindControlGUI1.GetVisualParam();
                    upEdgeParam.Name = "SearchUpEdge";

                    //upEdgeParam.RingLightintensity = visualLineFindControlGUI1.RingLightintensity;
                    //upEdgeParam.DirectLightintensity = visualLineFindControlGUI1.DirectLightintensity;

                    //upEdgeParam.UpEdgeScore = visualLineFindControlGUI1.UpEdgeScore;
                    //upEdgeParam.RightEdgeScore = visualLineFindControlGUI1.RightEdgeScore;
                    //upEdgeParam.DownEdgeScore = visualLineFindControlGUI1.DownEdgeScore;
                    //upEdgeParam.LeftEdgeScore = visualLineFindControlGUI1.LeftEdgeScore;




                    //upEdgeParam.UpEdgeRoi = visualLineFindControlGUI1.UpEdgeRoi;
                    //upEdgeParam.RightEdgeRoi = visualLineFindControlGUI1.RightEdgeRoi;
                    //upEdgeParam.DownEdgeRoi = visualLineFindControlGUI1.DownEdgeRoi;
                    //upEdgeParam.LeftEdgeRoi = visualLineFindControlGUI1.LeftEdgeRoi;


                    //upEdgeParam.UpEdgefilepath = visualLineFindControlGUI1.UpEdgefilepath;
                    //upEdgeParam.RightEdgefilepath = visualLineFindControlGUI1.RightEdgefilepath;
                    //upEdgeParam.DownEdgefilepath = visualLineFindControlGUI1.DownEdgefilepath;
                    //upEdgeParam.LeftEdgefilepath = visualLineFindControlGUI1.LeftEdgefilepath;

                    //upEdgeParam.UsedCamera = _cameraManager.CurrentCameraType;
                    upEdgeParam.CameraZWorkPosition = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);
                    upEdgeParam.CameraZChipSystemWorkPosition = (float)_positioningSystem.ReadChipPPSystemPosition(EditRecipe.CurrentComponent.RelatedPPToolName);

                    upEdgeParam.BondTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    upEdgeParam.BondTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    upEdgeParam.WaferTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    upEdgeParam.WaferTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                    //LineFindIdentificationParam rightEdgeParam = new LineFindIdentificationParam();
                    //rightEdgeParam.Name = "SearchRightEdge";
                    //rightEdgeParam.RightEdgeScore = visualLineFindControlGUI1.RightEdgeScore;
                    //rightEdgeParam.RingLightintensity = visualLineFindControlGUI1.RingLightintensity;
                    //rightEdgeParam.DirectLightintensity = visualLineFindControlGUI1.DirectLightintensity;


                    //rightEdgeParam.RightEdgeRoi = visualLineFindControlGUI1.RightEdgeRoi;


                    //rightEdgeParam.RightEdgefilepath = visualLineFindControlGUI1.RightEdgefilepath;

                    ////rightEdgeParam.UsedCamera = _cameraManager.CurrentCameraType;
                    //rightEdgeParam.CameraZWorkPosition = (float)(_cameraManager.CurrentCameraType == EnumCameraType.UplookingCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ)
                    //    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableZ));

                    //rightEdgeParam.BondTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    //rightEdgeParam.BondTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    //rightEdgeParam.WaferTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    //rightEdgeParam.WaferTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);
                    ////记录识别的特征中心在Stage坐标的位置++++++++++++
                    ////shapeMatchParam.
                    //LineFindIdentificationParam downEdgeParam = new LineFindIdentificationParam();
                    //downEdgeParam.Name = "SearchDownEdge";
                    //downEdgeParam.DownEdgeScore = visualLineFindControlGUI1.DownEdgeScore;
                    //downEdgeParam.RingLightintensity = visualLineFindControlGUI1.RingLightintensity;
                    //downEdgeParam.DirectLightintensity = visualLineFindControlGUI1.DirectLightintensity;


                    //downEdgeParam.DownEdgeRoi = visualLineFindControlGUI1.DownEdgeRoi;


                    //downEdgeParam.DownEdgefilepath = visualLineFindControlGUI1.DownEdgefilepath;

                    ////downEdgeParam.UsedCamera = _cameraManager.CurrentCameraType;
                    //downEdgeParam.CameraZWorkPosition = (float)(_cameraManager.CurrentCameraType == EnumCameraType.UplookingCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ)
                    //    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableZ));

                    //downEdgeParam.BondTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    //downEdgeParam.BondTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    //downEdgeParam.WaferTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    //downEdgeParam.WaferTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);


                    //LineFindIdentificationParam leftEdgeParam = new LineFindIdentificationParam();
                    //leftEdgeParam.Name = "SearchLeftEdge";
                    //leftEdgeParam.LeftEdgeScore = visualLineFindControlGUI1.LeftEdgeScore;
                    //leftEdgeParam.RingLightintensity = visualLineFindControlGUI1.RingLightintensity;
                    //leftEdgeParam.DirectLightintensity = visualLineFindControlGUI1.DirectLightintensity;


                    //leftEdgeParam.LeftEdgeRoi = visualLineFindControlGUI1.LeftEdgeRoi;


                    //leftEdgeParam.LeftEdgefilepath = visualLineFindControlGUI1.LeftEdgefilepath;

                    ////leftEdgeParam.UsedCamera = _cameraManager.CurrentCameraType;
                    //leftEdgeParam.CameraZWorkPosition = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);

                    //leftEdgeParam.BondTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    //leftEdgeParam.BondTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    //leftEdgeParam.WaferTablePosition.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    //leftEdgeParam.WaferTablePosition.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                    EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.LineSearchParams.Clear();
                    EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.LineSearchParams.Add(upEdgeParam);
                    //EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.LineSearchParams.Add(rightEdgeParam);
                    //EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.LineSearchParams.Add(downEdgeParam);
                    //EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.LineSearchParams.Add(leftEdgeParam);
                }
                else if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    MatchIdentificationParam shapeMatchParam = visualMatchControlGUI1.GetVisualParam();
                    //MatchIdentificationParam shapeMatchParam = new MatchIdentificationParam();
                    //shapeMatchParam.Score = visualMatchControlGUI1.Score;
                    //shapeMatchParam.MinAngle = -visualMatchControlGUI1.AngleRange;
                    //shapeMatchParam.MaxAngle = visualMatchControlGUI1.AngleRange;
                    //shapeMatchParam.RingLightintensity = visualMatchControlGUI1.RingLightintensity;
                    //shapeMatchParam.DirectLightintensity = visualMatchControlGUI1.DirectLightintensity;
                    //shapeMatchParam.SearchRoi = visualMatchControlGUI1.SearchRoi;


                    //shapeMatchParam.TemplateRoi = visualMatchControlGUI1.TemplateRoi;


                    //shapeMatchParam.Templatexml = visualMatchControlGUI1.MatchTemplatefilepath;
                    shapeMatchParam.Runxml = "";
                    shapeMatchParam.CameraZWorkPosition = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);
                    shapeMatchParam.CameraZChipSystemWorkPosition = (float)_positioningSystem.ReadChipPPSystemPosition(EditRecipe.CurrentComponent.RelatedPPToolName);

                    shapeMatchParam.BondTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    shapeMatchParam.BondTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                    var usedCamera = EnumCameraType.BondCamera;
                    if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.CalibrationTable)
                    {
                        usedCamera = EnumCameraType.BondCamera;
                    }
                    else if (EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod == EnumAccuracyMethod.UplookingCamera)
                    {
                        usedCamera = EnumCameraType.UplookingCamera;
                    }
                    //获取特征距离视野中心的距离，单位mm
                    var offsetX = _positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.X, 1, usedCamera);
                    var offsetY = _positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.Y, 2, usedCamera);
                    //此处注意坐标轴的方向和相机坐标系的方向是否一致TBD
                    PositionOfPattern = new PointF((float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) + offsetX,
                        (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + offsetY);
                    //shapeMatchParam.

                    EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.Clear();
                    EditRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.ShapeMatchParameters.Add(shapeMatchParam);
                }
                finished = true;
            }
            finally
            {
                if (NotifySingleStepDefineFinished != null)
                {
                    base.NotifySingleStepDefineFinished(base.EditRecipe, new int[] { 8, 0 }, new int[] { 2, 0 });
                }
            }
        }
    }
}
