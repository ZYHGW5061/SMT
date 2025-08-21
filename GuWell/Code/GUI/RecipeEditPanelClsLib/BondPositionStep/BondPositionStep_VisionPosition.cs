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
using UserManagerClsLib;
using VisionControlAppClsLib;
using VisionGUI;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class BondPositionStep_VisionPosition : BondPositionStepBasePage
    {
        private VisionGUI.VisualMatchControlGUI visualMatchControlGUI1;

        private VisionGUI.VisualLineFindControlGUI visualLineFindControlGUI1;

        private VisionGUI.VisualCircleFindControlGUI visualCircleFindControlGUI1;
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
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
        public BondPositionStep_VisionPosition(CameraWindowGUI cameraWnd, int PositionPointIndex = 1)
        {
            UsedCameraWnd = cameraWnd;
            InitializeComponent();
            PatternPositionOffsetWithVisionCenter = new XYZTCoordinateConfig();
        }
        public override EnumDefineSetupRecipeBondPositionStep CurrentStep
        {
            get
            { return EnumDefineSetupRecipeBondPositionStep.VisionPosition; }
        }

        public int RingLightIntensity { get; set; }

        /// <summary>
        /// 直光强度
        /// </summary>
        public int DirectLightIntensity { get; set; }

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
                    throw new Exception("Recipe is null when execute BondPositionStep_VisionPosition LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                var templateFolderName = $@"{_systemConfig.SystemDefaultDirectory}Recipes\BondPositions\{EditRecipe.CurrentBondPosition.Name}\TemplateConfig\";
                CommonProcess.EnsureFolderExist(templateFolderName);

                if (EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
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

                    if (recipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                    {
                        visualLineFindControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                        LineFindIdentificationParam param = new LineFindIdentificationParam();
                        param.DirectLightType = EnumDirectLightSourceType.RGB;
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                        visualLineFindControlGUI1.SetVisualParam(param);

                    }
                    else if (recipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                    {
                        visualLineFindControlGUI1.InitVisualControl(UsedCameraWnd, WaferCameraVisual);
                        LineFindIdentificationParam param = new LineFindIdentificationParam();
                        param.DirectLightType = EnumDirectLightSourceType.SingleR;
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.WaferRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.WaferDirectLightConfig.ChannelNumber);
                        visualLineFindControlGUI1.SetVisualParam(param);
                    }


                    var upEdgeSearchName = Path.Combine(templateFolderName, $"VisionTemplateUpEdgeVisionParameter.xml");
                    this.visualLineFindControlGUI1.UpEdgefilepath = upEdgeSearchName;

                    var rightEdgeSearchName = Path.Combine(templateFolderName, $"VisionTemplateRightEdgeVisionParameter.xml");
                    this.visualLineFindControlGUI1.RightEdgefilepath = rightEdgeSearchName;

                    var downEdgeSearchName = Path.Combine(templateFolderName, $"VisionTemplateDownEdgeVisionParameter.xml");
                    this.visualLineFindControlGUI1.DownEdgefilepath = downEdgeSearchName;

                    var leftEdgeSearchName = Path.Combine(templateFolderName, $"VisionTemplateLeftEdgeVisionParameter.xml");
                    this.visualLineFindControlGUI1.LeftEdgefilepath = leftEdgeSearchName;
                }
                else if (EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    this.visualMatchControlGUI1 = new VisionGUI.VisualMatchControlGUI();

                    // 
                    // visualMatchControlGUI1
                    // 
                    this.visualMatchControlGUI1.AngleRange = 15;
                    this.visualMatchControlGUI1.DirectLightintensity = 0;
                    this.visualMatchControlGUI1.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.visualMatchControlGUI1.Location = new System.Drawing.Point(3, 3);
                    this.visualMatchControlGUI1.MatchRunfilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchRun.contourmxml";
                    this.visualMatchControlGUI1.MatchTemplatefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchTemplate.contourmxml";
                    this.visualMatchControlGUI1.Name = "visualMatchControlGUI1";
                    this.visualMatchControlGUI1.OutlineAngle = 0F;
                    this.visualMatchControlGUI1.OutlineDeviation = new PointF();
                    this.visualMatchControlGUI1.RingLightintensity = 0;
                    this.visualMatchControlGUI1.Score = 0.5F;
                    this.visualMatchControlGUI1.SearchRoi = new RectangleFV();
                    this.visualMatchControlGUI1.Size = new System.Drawing.Size(339, 560);
                    this.visualMatchControlGUI1.TabIndex = 0;
                    this.visualMatchControlGUI1.Templateresult = null;
                    this.visualMatchControlGUI1.TemplateRoi = new RectangleFV();
                    this.tabPage1.Controls.Add(this.visualMatchControlGUI1);


                    if (recipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                    {
                        visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                        MatchIdentificationParam param = new MatchIdentificationParam();
                        param.DirectLightType = EnumDirectLightSourceType.RGB;
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                        visualMatchControlGUI1.SetVisualParam(param);

                    }
                    else if (recipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                    {
                        visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, WaferCameraVisual);
                        MatchIdentificationParam param = new MatchIdentificationParam();
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.WaferRingLightController.GetIntensity(HardwareConfiguration.Instance.WaferRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.WaferDirectLightController.GetIntensity(HardwareConfiguration.Instance.WaferDirectLightConfig.ChannelNumber);
                        visualMatchControlGUI1.SetVisualParam(param);
                    }


                    //var templateTrainFileName = Path.Combine(templateFolderName, "VisionTemplateTrainFile.contourmxml");
                    //this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                    //this.visualMatchControlGUI1.MatchRunfilepath = "";
                    var templateTrainFileName = Path.Combine(templateFolderName, $"VisionTemplateTrainFile.contourmxml");
                    var templateTrainParamName = Path.Combine(templateFolderName, $"VisionTemplateTrainParamFile.xml");
                    var templateRunFileName = Path.Combine(templateFolderName, $"VisionTemplateRunFile.xml");
                    this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                    this.visualMatchControlGUI1.MatchTemplateParampath = templateTrainParamName;
                    this.visualMatchControlGUI1.MatchRunfilepath = templateRunFileName;
                }
                else if (EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                {
                    this.visualCircleFindControlGUI1 = new VisionGUI.VisualCircleFindControlGUI();

                    // 
                    // visualMatchControlGUI1
                    // 
                    this.visualCircleFindControlGUI1.DirectLightintensity = 0;
                    this.visualCircleFindControlGUI1.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.visualCircleFindControlGUI1.Location = new System.Drawing.Point(3, 3);
                    this.visualCircleFindControlGUI1.CircleFindTemplatefilepath = "D:\\00-Work\\10-SVN\\01-贴片机\\00-Dev\\trunk\\BondTerminal\\MatchTemplate.contourmxml";
                    this.visualCircleFindControlGUI1.Name = "visualCircleFindControlGUI1";
                    this.visualCircleFindControlGUI1.RingLightintensity = 0;
                    this.visualCircleFindControlGUI1.Score = 1;
                    this.visualCircleFindControlGUI1.SearchRoi = new RectangleFV();
                    this.visualCircleFindControlGUI1.Size = new System.Drawing.Size(339, 560);
                    this.visualCircleFindControlGUI1.TabIndex = 0;
                    this.visualCircleFindControlGUI1.TemplateRoiCenter = new PointF();
                    this.visualCircleFindControlGUI1.SearchRoi = new RectangleFV();
                    this.visualCircleFindControlGUI1.TemplateRoiR = 5;
                    this.tabPage1.Controls.Add(this.visualCircleFindControlGUI1);

                    if (recipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                    {
                        this.visualCircleFindControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                        CircleFindIdentificationParam param = new CircleFindIdentificationParam();
                        param.DirectLightType = EnumDirectLightSourceType.RGB;
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                        this.visualCircleFindControlGUI1.SetVisualParam(param);

                    }
                    else if (recipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                    {
                        this.visualCircleFindControlGUI1.InitVisualControl(UsedCameraWnd, WaferCameraVisual);
                        CircleFindIdentificationParam param = new CircleFindIdentificationParam();
                        param.DirectLightType = EnumDirectLightSourceType.SingleR;
                        param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.WaferRingLightConfig.ChannelNumber);
                        param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.WaferDirectLightConfig.ChannelNumber);
                        this.visualCircleFindControlGUI1.SetVisualParam(param);
                    }



                    //var templateTrainFileName = Path.Combine(templateFolderName, "VisionTemplateTrainFile.contourmxml");
                    //this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                    //this.visualMatchControlGUI1.MatchRunfilepath = "";
                    var templateTrainFileName = Path.Combine(templateFolderName, $"VisionTemplateCircleTrainFile.contourmxml");
                    var templateTrainParamName = Path.Combine(templateFolderName, $"VisionTemplateCircleTrainParamFile.xml");
                    var templateRunFileName = Path.Combine(templateFolderName, $"VisionTemplateCircleRunFile.xml");
                    this.visualCircleFindControlGUI1.CircleFindTemplatefilepath = templateTrainFileName;

                }


            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeBondPositionStep currentStep)
        {
            try
            {
                currentStep = EnumDefineSetupRecipeBondPositionStep.VisionPosition;

                if (EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.EdgeSearch)
                {
                    Score = visualLineFindControlGUI1.Score;
                    RingLightIntensity = visualLineFindControlGUI1.RingLightintensity;
                    DirectLightIntensity = visualLineFindControlGUI1.DirectLightintensity;


                    LineFindIdentificationParam shapeMatchParam = visualLineFindControlGUI1.GetVisualParam();
                    //MatchIdentificationParam shapeMatchParam = new MatchIdentificationParam();
                    //shapeMatchParam.Score = visualMatchControlGUI1.Score;
                    //shapeMatchParam.MinAngle = -visualMatchControlGUI1.AngleRange;
                    //shapeMatchParam.MaxAngle = visualMatchControlGUI1.AngleRange;
                    //shapeMatchParam.RingLightintensity = visualMatchControlGUI1.RingLightintensity;
                    //shapeMatchParam.DirectLightintensity = visualMatchControlGUI1.DirectLightintensity;
                    //shapeMatchParam.SearchRoi = visualMatchControlGUI1.SearchRoi;


                    //shapeMatchParam.TemplateRoi = visualMatchControlGUI1.TemplateRoi;


                    //shapeMatchParam.Templatexml = visualMatchControlGUI1.MatchTemplatefilepath;
                    //shapeMatchParam.Runxml = "";
                    //var usedCamera = EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera;
                    var usedCamera = EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera;

                    shapeMatchParam.CameraZWorkPosition = (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ)
                        : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableZ));

                    shapeMatchParam.BondTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    shapeMatchParam.BondTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                    if (visualLineFindControlGUI1.Rectresult != null)
                    {
                        //获取特征距离视野中心的距离，单位mm
                        var offsetX = _positioningSystem.ConvertPixelPosToMMCenterPos(visualLineFindControlGUI1.Rectresult.Center.X, 1, usedCamera);
                        var offsetY = _positioningSystem.ConvertPixelPosToMMCenterPos(visualLineFindControlGUI1.Rectresult.Center.Y, 2, usedCamera);
                        //此处注意坐标轴的方向和相机坐标系的方向是否一致TBD
                        PositionOfCreatePattern = new PointF((float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) + offsetX
                            : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX) + offsetX),
                            (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + offsetY
                            : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY) + offsetY));


                        //EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.ShapeMatchParameters.Clear();
                        //EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.ShapeMatchParameters.Add(shapeMatchParam);
                        EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.LineSearchParams.Clear();
                        EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.LineSearchParams.Add(shapeMatchParam);
                    }
                    else
                    {
                        LogRecorder.RecordUserOperationLog("贴片位置Mark视觉定位 用户未识别矩形特征.", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, UserManager.Instance.CurrentUserName);
                    }

                }
                else if (EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.PatternSearch)
                {
                    Score = visualMatchControlGUI1.Score;
                    AngleRange = visualMatchControlGUI1.AngleRange;
                    RingLightIntensity = visualMatchControlGUI1.RingLightintensity;
                    DirectLightIntensity = visualMatchControlGUI1.DirectLightintensity;


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
                    //var usedCamera = EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera;
                    var usedCamera = EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera;

                    shapeMatchParam.CameraZWorkPosition = (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ)
                        : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableZ));

                    shapeMatchParam.BondTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    shapeMatchParam.BondTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                    if (visualMatchControlGUI1.Matchresult != null)
                    {
                        //获取特征距离视野中心的距离，单位mm
                        var offsetX = _positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.X, 1, usedCamera);
                        var offsetY = _positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.Y, 2, usedCamera);
                        //此处注意坐标轴的方向和相机坐标系的方向是否一致TBD
                        PositionOfCreatePattern = new PointF((float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) + offsetX
                            : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX) + offsetX),
                            (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + offsetY
                            : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY) + offsetY));


                        //EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.ShapeMatchParameters.Clear();
                        //EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.ShapeMatchParameters.Add(shapeMatchParam);
                        EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.Clear();
                        EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.Add(shapeMatchParam);
                    }
                    else
                    {
                        LogRecorder.RecordUserOperationLog("贴片位置Mark视觉定位 用户未识别轮廓特征.", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, UserManager.Instance.CurrentUserName);
                    }

                }
                else if (EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionMethod == EnumVisionPositioningMethod.CircleSearch)
                {
                    Score = visualCircleFindControlGUI1.Score;
                    RingLightIntensity = visualCircleFindControlGUI1.RingLightintensity;
                    DirectLightIntensity = visualCircleFindControlGUI1.DirectLightintensity;


                    CircleFindIdentificationParam shapeMatchParam = visualCircleFindControlGUI1.GetVisualParam();
                    //MatchIdentificationParam shapeMatchParam = new MatchIdentificationParam();
                    //shapeMatchParam.Score = visualMatchControlGUI1.Score;
                    //shapeMatchParam.MinAngle = -visualMatchControlGUI1.AngleRange;
                    //shapeMatchParam.MaxAngle = visualMatchControlGUI1.AngleRange;
                    //shapeMatchParam.RingLightintensity = visualMatchControlGUI1.RingLightintensity;
                    //shapeMatchParam.DirectLightintensity = visualMatchControlGUI1.DirectLightintensity;
                    //shapeMatchParam.SearchRoi = visualMatchControlGUI1.SearchRoi;


                    //shapeMatchParam.TemplateRoi = visualMatchControlGUI1.TemplateRoi;


                    //shapeMatchParam.Templatexml = visualMatchControlGUI1.MatchTemplatefilepath;
                    //shapeMatchParam.Runxml = "";
                    //var usedCamera = EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.VisionPositionUsedCamera;
                    var usedCamera = EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.VisionPositionUsedCamera;

                    shapeMatchParam.CameraZWorkPosition = (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ)
                        : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableZ));

                    shapeMatchParam.BondTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                    shapeMatchParam.BondTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                    shapeMatchParam.WaferTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                    if (visualCircleFindControlGUI1.results != null)
                    {
                        //获取特征距离视野中心的距离，单位mm
                        var offsetX = _positioningSystem.ConvertPixelPosToMMCenterPos(visualCircleFindControlGUI1.results.CircleCenter.X, 1, usedCamera);
                        var offsetY = _positioningSystem.ConvertPixelPosToMMCenterPos(visualCircleFindControlGUI1.results.CircleCenter.Y, 2, usedCamera);
                        //此处注意坐标轴的方向和相机坐标系的方向是否一致TBD
                        PositionOfCreatePattern = new PointF((float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) + offsetX
                            : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX) + offsetX),
                            (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + offsetY
                            : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY) + offsetY));


                        //EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.ShapeMatchParameters.Clear();
                        //EditRecipe.SubstrateInfos.PositionSustrateVisionParameters.ShapeMatchParameters.Add(shapeMatchParam);
                        EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.CircleSearchParameters.Clear();
                        EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.CircleSearchParameters.Add(shapeMatchParam);
                    }
                    else
                    {
                        LogRecorder.RecordUserOperationLog("贴片位置Mark视觉定位 用户未识别圆形特征.", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, UserManager.Instance.CurrentUserName);
                    }

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
