using GlobalDataDefineClsLib;
using GlobalToolClsLib;
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

namespace ControlPanelClsLib
{
    public partial class PPToolAlignmentStep_PositionCenterAfterR180 : PPToolAlignmentStepBasePage
    {
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        public PPToolAlignmentStep_PositionCenterAfterR180()
        {
            InitializeComponent();
            CenterPosition = new XYZTCoordinateConfig();
            ctrlLight1.CurrentLightType = EnumLightSourceType.LookupRingField;
            ctrlLight1.ApplyIntensityToHardware = true;
            ctrlLight2.CurrentLightType = EnumLightSourceType.LookupDirectField;
            ctrlLight2.ApplyIntensityToHardware = true;
        }
        public override EnumDefinePPAlignStep CurrentStep
        {
            get
            { return EnumDefinePPAlignStep.PositionCenterSecond; }
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
                    throw new Exception("Recipe is null when execute ComponentPositionStep_SizeLUCorner LoadEditedRecipe.");
                }
                EditRecipe = recipe;
                
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEditedRecipe failed.", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void NotifyStepFinished(out bool finished, out EnumDefinePPAlignStep currentStep)
        {
            try
            {
                currentStep = EnumDefinePPAlignStep.PositionCenterSecond;
                CenterPosition.X = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                CenterPosition.Y = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                CenterPosition.Z = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                CenterPosition.Theta = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
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
