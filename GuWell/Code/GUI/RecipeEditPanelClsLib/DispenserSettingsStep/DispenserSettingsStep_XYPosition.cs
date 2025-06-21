using CameraControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using JobClsLib;
using LightSourceCtrlPanelLib;
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

namespace RecipeEditPanelClsLib
{
    public partial class DispenserSettingsStep_XYPosition : DispenserSettingsStepBasePage
    {
        private PackagedLightController _packagedLightController;
        private CameraManager _cameraManager
        {
            get { return CameraManager.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        public DispenserSettingsStep_XYPosition()
        {
            InitializeComponent();
            DispenserPosition = new PointF();
        }
        public override EnumDefineDispenserSettingsStep CurrentStep
        {
            get
            { return EnumDefineDispenserSettingsStep.DispenserPosition; }
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
                    throw new Exception("Recipe is null when execute LoadEditedRecipe.");
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


                this.panelControl2.Controls.Clear();
                _packagedLightController.Dock = DockStyle.Fill;
                this.panelControl2.Controls.Add(_packagedLightController);
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefineDispenserSettingsStep currentStep)
        {
            try
            {
                currentStep = EnumDefineDispenserSettingsStep.DispenserPosition;


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
        public void SetStepInfo(string info)
        {
            labelStepInfo.Text = info;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将记录划胶器位置！", "确认相机十字中心已对准划胶位置？", "提示") == 1)
            {
                var posX = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                var posY = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                DispenserPosition = new PointF(posX, posY);
                DispenserPositionOffsetWithBondCamera = new PointF(_dispensePos.X - posX, _dispensePos.Y - posY);
            }
        }
        PointF _dispensePos = new PointF();
        private void btnDispenseGlue_Click(object sender, EventArgs e)
        {
            if(WarningBox.FormShow("即将点胶","确认胶针已移动到合适位置？","提示")==1)
            {
                var posX = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX);
                var posY = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY);
                _dispensePos = new PointF(posX, posY);
                //点胶
                DispenserUtility.Instance.ExecutePoint(false,false);
            }
        }

        private void btnUpDispenser_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将升起!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.UpDispenserCylinder();
            }
        }

        private void btnDownDispenser_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将下降!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.DownDispenserCylinder();
            }
        }
    }
}
