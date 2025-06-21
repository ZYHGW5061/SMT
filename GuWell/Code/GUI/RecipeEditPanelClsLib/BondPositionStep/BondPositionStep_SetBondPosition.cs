using CameraControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using LightSourceCtrlPanelLib;
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

namespace RecipeEditPanelClsLib
{
    public partial class BondPositionStep_SetBondPosition : BondPositionStepBasePage
    {
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        public BondPositionStep_SetBondPosition()
        {
            InitializeComponent();
            BondPositionOffset = new XYZTCoordinateConfig();
            BondPositionCompensation = new XYZTCoordinateConfig();

        }
        private PackagedLightController _packagedLightController;
        public override EnumDefineSetupRecipeBondPositionStep CurrentStep
        {
            get
            { return EnumDefineSetupRecipeBondPositionStep.SetBondPosition; }
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
                    throw new Exception("Recipe is null when execute BondPositionStep_SetBondPosition LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                _packagedLightController = new PackagedLightController(HardwareConfiguration.Instance.IsBondDirectLightMultiColor, HardwareConfiguration.Instance.IsBondRingLightMultiColor);
                if (HardwareConfiguration.Instance.IsBondDirectLightMultiColor)
                {
                    _packagedLightController.DirectLightColor = EnumLightColor.Red;
                    _packagedLightController.CurrentDirectLightType = EnumLightSourceType.BondDirectField;
                }
                else
                {
                    _packagedLightController.CurrentDirectLightType = EnumLightSourceType.BondDirectField;
                }
                if (HardwareConfiguration.Instance.IsBondRingLightMultiColor)
                {
                    _packagedLightController.RingLightColor = EnumLightColor.Red;
                    _packagedLightController.CurrentRingLightType = EnumLightSourceType.BondRingField;
                }
                else
                {
                    _packagedLightController.CurrentRingLightType = EnumLightSourceType.BondRingField;
                }
                this.panelControl3.Controls.Clear();
                _packagedLightController.Dock = DockStyle.Fill;
                this.panelControl3.Controls.Add(_packagedLightController);
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
                currentStep = EnumDefineSetupRecipeBondPositionStep.SetBondPosition;
                
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

        private void btnConfirmPos_Click(object sender, EventArgs e)
        {
            var CompensationX = float.Parse(seBPCompensationX.Text);
            var CompensationY = float.Parse(seBPCompensationY.Text);
            var CompensationT = float.Parse(seBPCompensationT.Text);
            //var offsetMMWithVisionCenter=_positioningSystem.ConvertPixelCoorToMMCenterCoor(new PointF(selVisionPosX, selVisionPosY), EnumCameraType.BondCamera);
            //var offsetYMMWithCenter=_positioningSystem.ConvertPixelPosToMMCenterPos(selVisionPosY, 1, EnumCameraType.BondCamera);
            //BondPositionOffset.X = offsetMMWithVisionCenter.Item1;
            //BondPositionOffset.Y = offsetMMWithVisionCenter.Item2;
            BondPositionCompensation.X = CompensationX;
            BondPositionCompensation.Y = CompensationY;
            BondPositionCompensation.Theta = CompensationT;
            BondPositionOffset.Theta = float.Parse(seBPRotateTheta.Text.Trim());
            WarningBox.FormShow("成功", "贴装位置计算完成！", "提示");
        }
    }
}
