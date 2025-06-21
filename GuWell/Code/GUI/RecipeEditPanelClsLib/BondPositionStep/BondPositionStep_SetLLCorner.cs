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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecipeEditPanelClsLib
{
    public partial class BondPositionStep_SetLLCorner : BondPositionStepBasePage
    {
        public BondPositionStep_SetLLCorner()
        {
            InitializeComponent();
        }
        private PackagedLightController _packagedLightController;
        public override EnumDefineSetupRecipeBondPositionStep CurrentStep 
        { 
            get 
            { return EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftLowerCorner; } 
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
                    throw new Exception("Recipe is null when execute SubmountPositionStep_SetLLCorner LoadEditedRecipe.");
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
        public override void NotifyStepFinished(out bool finished, out EnumDefineSetupRecipeBondPositionStep currentStep)
        {
            try
            {
                currentStep = EnumDefineSetupRecipeBondPositionStep.SetSubmountLeftLowerCorner;
                LeftLowerCornerCoor = new PointF((float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondX), (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondY));
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
