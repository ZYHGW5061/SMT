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
    public partial class BondPositionStep_VisionPosition : BondPositionStepBasePage
    {
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
                var templateFolderName = $@"{_systemConfig.SystemDefaultDirectory}Recipes\{EnumRecipeType.Bonder.ToString()}\BondPositions\{EditRecipe.CurrentBondPosition.Name}\TemplateConfig\";
                CommonProcess.EnsureFolderExist(templateFolderName);

                //if (recipe.CurrentComponent.PositionComponentVisionParameters.FirstOrDefault().VisionPositionUsedCamera == EnumCameraType.BondCamera)
                //{
                    visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                    MatchIdentificationParam param = new MatchIdentificationParam();
                    param.DirectLightType = EnumDirectLightSourceType.RGB;
                    param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                    param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                    visualMatchControlGUI1.SetVisualParam(param);

                //}
                //else if (recipe.CurrentComponent.PositionComponentVisionParameters.FirstOrDefault().VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                //{
                //    visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, WaferCameraVisual);
                //    MatchIdentificationParam param = new MatchIdentificationParam();
                //    param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.WaferRingLightController.GetIntensity(HardwareConfiguration.Instance.WaferRingLightConfig.ChannelNumber);
                //    param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.WaferDirectLightController.GetIntensity(HardwareConfiguration.Instance.WaferDirectLightConfig.ChannelNumber);
                //    visualMatchControlGUI1.SetVisualParam(param);
                //}

                var templateTrainFileName = Path.Combine(templateFolderName, $"VisionTemplateFile_{EditRecipe.CurrentBondPosition.Name}.contourmxml");
                var templateTrainParamName = Path.Combine(templateFolderName, $"VisionTemplateFile_{EditRecipe.CurrentBondPosition.Name}TrainParam.xml");
                var templateRunFileName = Path.Combine(templateFolderName, $"VisionTemplateFile_{EditRecipe.CurrentBondPosition.Name}Run.xml");
                this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                this.visualMatchControlGUI1.MatchTemplateParampath = templateTrainParamName;
                this.visualMatchControlGUI1.MatchRunfilepath = templateRunFileName;
                //this.visualMatchControlGUI1.MatchRunfilepath = "";
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
                Score = visualMatchControlGUI1.Score;
                AngleRange = visualMatchControlGUI1.AngleRange;
                RingLightIntensity= visualMatchControlGUI1.RingLightintensity;
                DirectLightIntensity= visualMatchControlGUI1.DirectLightintensity;

                //MatchIdentificationParam shapeMatchParam = new MatchIdentificationParam();
                MatchIdentificationParam shapeMatchParam = visualMatchControlGUI1.GetVisualParam();
                //shapeMatchParam.Score = visualMatchControlGUI1.Score;
                //shapeMatchParam.MinAngle = -visualMatchControlGUI1.AngleRange;
                //shapeMatchParam.MaxAngle = visualMatchControlGUI1.AngleRange;
                //shapeMatchParam.RingLightintensity = visualMatchControlGUI1.RingLightintensity;
                //shapeMatchParam.DirectLightintensity= visualMatchControlGUI1.DirectLightintensity;
                //shapeMatchParam.SearchRoi = visualMatchControlGUI1.SearchRoi;


                //shapeMatchParam.TemplateRoi =visualMatchControlGUI1.TemplateRoi;


                //shapeMatchParam.Templatexml = visualMatchControlGUI1.MatchTemplatefilepath;
                shapeMatchParam.Runxml = "";
                //此处的WorkZ需要使用物料厚度来计算(基于共晶台)

                //var stageZ=_systemConfig.PositioningConfig.EutecticWeldingLocation.Z + EditRecipe.SubstrateInfos.ThicknessMM;
                shapeMatchParam.CameraZWorkPosition = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);

                shapeMatchParam.BondTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                shapeMatchParam.BondTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                shapeMatchParam.WaferTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                shapeMatchParam.WaferTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                //获取特征距离视野中心的距离，单位mm
                if (visualMatchControlGUI1.Matchresult != null)
                {
                    var offsetX = _positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.X, 1, EnumCameraType.BondCamera);
                    var offsetY = _positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.Y, 2, EnumCameraType.BondCamera);
                    PatternPositionOffsetWithVisionCenter.X = offsetX;
                    PatternPositionOffsetWithVisionCenter.Y = offsetY;

                    var usedCamera = EnumCameraType.BondCamera;
                    PositionOfCreatePattern = new PointF((float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) + offsetX
                                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX) + offsetX),
                                    (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + offsetY
                                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY) + offsetY));

                    EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.Clear();
                    EditRecipe.CurrentBondPosition.VisionParametersForFindBondPosition.ShapeMatchParameters.Add(shapeMatchParam);
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
