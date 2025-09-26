using GlobalDataDefineClsLib;
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

namespace ControlPanelClsLib.Recipe
{
    public partial class RecipeStep_Dispenser : RecipeStepBase
    {
        public RecipeStep_Dispenser()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载前一步定义的Recipe对象
        /// </summary>
        /// <param name="recipe"></param>
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            base.LoadEditedRecipe(recipe);
            if (_editRecipe != null )
            {
                
            }
        }
        /// <summary>
        /// 验证并通知recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currentStep)
        {
            currentStep = EnumRecipeStep.Configuration;         
            //完成该步定义
            finished = true;
        }
    }
}
