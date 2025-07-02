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
    public partial class ComponentPositionStep_VisionPosition : ComponentPositionStepBasePage
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
        public ComponentPositionStep_VisionPosition(CameraWindowGUI cameraWnd)
        {
            UsedCameraWnd = cameraWnd;
            InitializeComponent();
        }
        public override EnumDefineSetupRecipeComponentPositionStep CurrentStep
        {
            get
            { return EnumDefineSetupRecipeComponentPositionStep.VisionPosition; }
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
        public void InitializeVisionTool(VisualControlApplications visualApp)
        {
            visualMatchControlGUI1.InitVisualControl(CameraWindowGUI.Instance, visualApp);
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
                    throw new Exception("Recipe is null when execute ComponentPositionStep_VisionPosition LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                var templateFolderName = $@"{_systemConfig.SystemDefaultDirectory}Recipes\Components\{EditRecipe.CurrentComponent.Name}\TemplateConfig\";
                CommonProcess.EnsureFolderExist(templateFolderName);

                if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.BondCamera)
                {
                    visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, BondCameraVisual);
                    MatchIdentificationParam param = new MatchIdentificationParam();
                    param.DirectLightType = HardwareConfiguration.Instance.IsBondDirectLightMultiColor ? EnumDirectLightSourceType.RGB : EnumDirectLightSourceType.SingleR;
                    //param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondRingLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateRingLightConfig.ChannelNumber);
                    //param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.BondDirectLightController.GetIntensity(HardwareConfiguration.Instance.SubstrateDirectLightConfig.ChannelNumber);
                    visualMatchControlGUI1.SetVisualParam(param);
                }
                else if (recipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera == EnumCameraType.WaferCamera)
                {
                    visualMatchControlGUI1.InitVisualControl(UsedCameraWnd, WaferCameraVisual);
                    MatchIdentificationParam param = new MatchIdentificationParam();
                    param.DirectLightType = HardwareConfiguration.Instance.IsWaferDirectLightMultiColor ? EnumDirectLightSourceType.RGB : EnumDirectLightSourceType.SingleR;
                    //param.RingLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.WaferRingLightController.GetIntensity(HardwareConfiguration.Instance.WaferRingLightConfig.ChannelNumber);
                    //param.DirectLightintensity = (int)HardwareManagerClsLib.HardwareManager.Instance.WaferDirectLightController.GetIntensity(HardwareConfiguration.Instance.WaferDirectLightConfig.ChannelNumber);
                    visualMatchControlGUI1.SetVisualParam(param);
                }
                //var templateTrainFileName = Path.Combine(templateFolderName, $"PositionTemplateFile_{EditRecipe.CurrentComponent.Name}.contourmxml");
                //this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                //this.visualMatchControlGUI1.MatchRunfilepath = "";
                var templateTrainFileName = Path.Combine(templateFolderName, $"PositionTemplateFile_{EditRecipe.CurrentComponent.Name}.contourmxml");
                var templateTrainParamName = Path.Combine(templateFolderName, $"PositionTemplateFile_{EditRecipe.CurrentComponent.Name}TrainParam.xml");
                var templateRunFileName = Path.Combine(templateFolderName, $"PositionTemplateFile_{EditRecipe.CurrentComponent.Name}Run.xml");
                this.visualMatchControlGUI1.MatchTemplatefilepath = templateTrainFileName;
                this.visualMatchControlGUI1.MatchTemplateParampath = templateTrainParamName;
                this.visualMatchControlGUI1.MatchRunfilepath = templateRunFileName;
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeComponentPositionStep currentStep)
        {
            try
            {
                currentStep = EnumDefineSetupRecipeComponentPositionStep.VisionPosition;
                Score = visualMatchControlGUI1.Score;
                AngleRange = visualMatchControlGUI1.AngleRange;
                RingLightintensity= visualMatchControlGUI1.RingLightintensity;
                DirectLightintensity= visualMatchControlGUI1.DirectLightintensity;

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
                var usedCamera = EditRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera;
                shapeMatchParam.CameraZWorkPosition = (float)(usedCamera==EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ) 
                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableZ));

                shapeMatchParam.BondTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                shapeMatchParam.BondTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                shapeMatchParam.WaferTablePositionOfCreatePattern.X = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX);
                shapeMatchParam.WaferTablePositionOfCreatePattern.Y = _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY);

                //获取特征距离视野中心的距离，单位mm
                var offsetX=_positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.X,1, usedCamera);
                var offsetY=_positioningSystem.ConvertPixelPosToMMCenterPos(visualMatchControlGUI1.Matchresult.MatchBox.Center.Y,2, usedCamera);
                //此处注意坐标轴的方向和相机坐标系的方向是否一致TBD
                PositionOfPattern=new PointF((float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX) + offsetX
                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableX) + offsetX), 
                    (float)(usedCamera == EnumCameraType.BondCamera ? _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY) + offsetY
                    : _positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.WaferTableY) + offsetY));
                //shapeMatchParam.

                EditRecipe.CurrentComponent.PositionComponentVisionParameters.ShapeMatchParameters.Clear();
                EditRecipe.CurrentComponent.PositionComponentVisionParameters.ShapeMatchParameters.Add(shapeMatchParam);
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
