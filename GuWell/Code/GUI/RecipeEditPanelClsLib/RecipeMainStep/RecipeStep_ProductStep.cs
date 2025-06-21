using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
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
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;
using static GlobalToolClsLib.GlobalCommFunc;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeStep_ProductStep: RecipeStepBase
    {
        //ConfigService configService = new ConfigService();
        //ProductConfig curProdConfig;
        //List<ProductStep> productSteps;
        List<ProgramComponentSettings> componentsList;
        List<BondingPositionSettings> bondPosList;
        List<EpoxyApplication> _epoxyApplicationList;
        //List<EutecticParameters> eutecticList;
        ProductStep curStep;
        List<ProductStep> productSteps;
        ProgramComponentSettings curStepComp;
        BondingPositionSettings curStepBondingPos;
        //EutecticParameters curStepEutectic;
        private static string SystemDefaultDirectory = SystemConfiguration.Instance.SystemDefaultDirectory;
        private static string _componentsSavePath = string.Format(@"{0}Recipes\{1}\Components\", SystemDefaultDirectory, EnumRecipeType.Bonder.ToString());
        private static string _bondPositionSavePath = string.Format(@"{0}Recipes\{1}\BondPositions\", SystemDefaultDirectory, EnumRecipeType.Bonder.ToString());
        private static string _epoxyApplicationSavePath = string.Format(@"{0}Recipes\{1}\EpoxyApplication\", SystemDefaultDirectory, EnumRecipeType.Bonder.ToString());

        public RecipeStep_ProductStep()
        {
            InitializeComponent();
            init();

        }

        private void init()
        {
            /*List<ProductConfig> products = configService.getProductConfigList();
            cbComponentList.DataSource = products;
            cbComponentList.DisplayMember = "ConfigName";
            cbComponentList.SelectedIndex = -1;*/

            curStep = null;
            curStepComp = null;
            curStepBondingPos = null;
            //curStepEutectic = null;
            productSteps = new List<ProductStep>();
            bondPosList = new List<BondingPositionSettings>();
            _epoxyApplicationList = new List<EpoxyApplication>();
            //curProdConfig = new ProductConfig() { ProductName = "test产品配方" };
            //curProdConfig.ProductSteps = new List<ProductStep>();
            //lbSteps.DataSource = curProdConfig.ProductSteps;
            //lbSteps.DisplayMember = "StepName";
            //lbSteps.SelectedIndex = -1;            

            //初始化选项enable
            rbStepTypeDispense.Checked = true;
            cbBondPositionList.Enabled = true;
            cbComponentList.Enabled = true;

            //teProductName.Text = curProdConfig.ProductName;
        }

        private void LoadComponentList()
        {

            componentsList = new List<ProgramComponentSettings>();
            cbComponentList.Items.Clear();
            //cbComponentList.Items.Add(_editRecipe.SubstrateInfos.Name);
            var childs = Directory.GetDirectories(_componentsSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                cbComponentList.Items.Add(childName);
                var xmlFile = $@"{_componentsSavePath}\{childName}\{childName}.xml";
                var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramComponentSettings>(xmlFile, Encoding.UTF8);
                componentsList.Add(comp);
            }
            cbComponentList.SelectedIndex = -1;
        }

        private void LoadBondPositionList()
        {

            cbBondPositionList.Items.Clear();
            var childs = Directory.GetDirectories(_bondPositionSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                cbBondPositionList.Items.Add(childName);

                var xmlFile = $@"{_bondPositionSavePath}\{childName}\{childName}.xml";
                var ret = XmlSerializeHelper.XmlDeserializeFromFile<BondingPositionSettings>(xmlFile, Encoding.UTF8);
                bondPosList.Add(ret);
            }
            cbBondPositionList.SelectedIndex = -1;
        }
        private void LoadEpoxyApplicationList()
        {

            cmbSelEpoxyApplication.Items.Clear();
            var childs = Directory.GetDirectories(_epoxyApplicationSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                cmbSelEpoxyApplication.Items.Add(childName);

                var xmlFile = $@"{_epoxyApplicationSavePath}\{childName}\{childName}.xml";
                var ret = XmlSerializeHelper.XmlDeserializeFromFile<EpoxyApplication>(xmlFile, Encoding.UTF8);
                _epoxyApplicationList.Add(ret);
            }
            cmbSelEpoxyApplication.SelectedIndex = -1;
        }
        /// <summary>
        ///  加载前一步定义的Recipe对象
        /// </summary>
        /// <param name="recipe"></param>
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            try
            {
                base.LoadEditedRecipe(recipe);
                LoadComponentList();
                LoadBondPositionList();
                LoadEpoxyApplicationList();
                lbSteps.BeginUpdate();
                lbSteps.DataSource = _editRecipe.ProductSteps;
                lbSteps.EndUpdate();
                lbSteps.DisplayMember = "StepName";
                lbSteps.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                _systemDebugLogger.AddErrorContent($"LoadEditedRecipe:RecipeName{recipe.RecipeName}-ProductStep failed. ", ex);
            }
        }

        /// <summary>
        /// 验证并通知Recipe编辑主页面该步骤定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currentStep)
        {
            try
            {
                currentStep = EnumRecipeStep.ProductStepList;

                finished = true;
            }
            finally
            {
                if (NotifySingleStepDefineFinished != null)
                {
                    NotifySingleStepDefineFinished(_editRecipe, new int[] { 8, 0 }, new int[] { 2, 0 });
                }
            }
        }



        //=========================================================================================================================================



        private void lbSteps_DoubleClick(object sender, EventArgs e)
        {
            //清空
            curComponentDetail.FillComponentDetail(null);
            //curEuticticDetail.fillEutecticDetail(null);

            //取当前
            curStep = (ProductStep)lbSteps.SelectedItem;

            if (curStep == null)
            {
                return;
            }

            teCurStepName.Text = curStep.StepName;
            teCurStepType.Text = EnumHelper.GetDisplayName(curStep.productStepType);

            try
            {
                if (!string.IsNullOrWhiteSpace(curStep.ComponentName))
                {
                    //curStepComp = configService.loadComponentConfig(curStep.ComponentName);
                    curStepComp = componentsList.Find(t => t.Name == curStep.ComponentName);
                    curComponentDetail.FillComponentDetail(curStepComp,_editRecipe.SubstrateInfos);
                }
                else
                {
                    curComponentDetail.FillComponentDetail(null);
                }

                if (!string.IsNullOrWhiteSpace(curStep.BondingPositionName))
                {
                    //curStepBondingPos = configService.loadBondingPositionConfig(curStep.BondingPositionName);
                    curStepBondingPos = bondPosList.Find(t => t.Name == curStep.BondingPositionName);
                    positionDetail1.fillPositionDetail(curStepBondingPos);
                }
                else
                {
                    //curPosDetail.fillPositionDetail(null);
                }

                //if (!string.IsNullOrWhiteSpace(curStep.EutecticName))
                //{
                //    //curStepEutectic = configService.loadEutecticConfig(curStep.EutecticName);
                //    curStepEutectic = eutecticList.Find(t => t.ConfigName == curStep.EutecticName);
                //    curEuticticDetail.fillEutecticDetail(curStepEutectic);
                //}
                //else
                //{
                //    curEuticticDetail.fillEutecticDetail(null);
                //}

                //if (_editRecipe.EutecticParameters!=null)
                //{
                //    //curStepEutectic = configService.loadEutecticConfig(curStep.EutecticName);
                //    //curStepEutectic = eutecticList.Find(t => t.ConfigName == curStep.EutecticName);
                //    //curEuticticDetail.fillEutecticDetail(curStepEutectic);
                //}
                //else
                //{
                //    //curEuticticDetail.fillEutecticDetail(null);
                //}
                //cmbEutecticParamIndex.Text = _editRecipe.EutecticParameters.ParameterIndex.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生异常!\n" + ex.Message);
                return;
            }            
        }       



        private void btnStepItemMoveUp_Click(object sender, EventArgs e)
        {
            int index = lbSteps.SelectedIndex;
            if (index <= 0)
            {
                return;
            }

            lbSteps.DataSource = null;
            ProductStep tmp = productSteps[index - 1];
            _editRecipe.ProductSteps[index - 1] = _editRecipe.ProductSteps[index];
            _editRecipe.ProductSteps[index] = tmp;
            lbSteps.DataSource = _editRecipe.ProductSteps;
            lbSteps.DisplayMember = "StepName";
            lbSteps.SelectedIndex = index - 1;
        }

        private void btnStepItemMoveDown_Click(object sender, EventArgs e)
        {
            int index = lbSteps.SelectedIndex;
            if (index >= _editRecipe.ProductSteps.Count - 1)
            {
                return;
            }

            lbSteps.DataSource = null;
            ProductStep tmp = _editRecipe.ProductSteps[index + 1];
            _editRecipe.ProductSteps[index + 1] = _editRecipe.ProductSteps[index];
            _editRecipe.ProductSteps[index] = tmp;
            lbSteps.DataSource = _editRecipe.ProductSteps;
            lbSteps.DisplayMember = "StepName";
            lbSteps.SelectedIndex = index + 1;
        }

        //步骤类型更改        

        private void btnCreateProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teStepName.Text))
            {
                WarningBox.FormShow("发生错误！", "请先填写步骤名。", "提示");
                return;
            }
            ;
            if (_editRecipe.ProductSteps.Any(i=>i.StepName== teStepName.Text.Trim()))
            {
                WarningBox.FormShow("发生错误！", "已存在相同的名称步骤。", "提示");
                return;
            }


            lbSteps.DataSource = null;

            ProductStep step = new ProductStep();
            step.StepName = teStepName.Text.Trim();
            if (rbStepTypeDispense.Checked)
            {
                step.productStepType = EnumProductStepType.Dispense;
            }
            else if (rbStepTypeBondDie.Checked)
            {
                step.productStepType = EnumProductStepType.BondDie;
            }

            

            if (cbComponentList.SelectedItem != null)
            {
                //step.ComponentName = ((ComponentConfig)cbComponentList.SelectedItem).ConfigName;
                step.ComponentName = cbComponentList.SelectedItem.ToString();
            }

            if (cbBondPositionList.SelectedItem != null)
            {
                //step.EutecticName = ((EutecticConfig)cbBondPositionList.SelectedItem).ConfigName;
                step.BondingPositionName = cbBondPositionList.SelectedItem.ToString();
            }
            if (cmbSelEpoxyApplication.SelectedItem != null)
            {
                //step.EutecticName = ((EutecticConfig)cbBondPositionList.SelectedItem).ConfigName;
                step.EpoxyApplicationName = cmbSelEpoxyApplication.SelectedItem.ToString();
            }
            _editRecipe.ProductSteps.Add(step);

            lbSteps.DataSource = _editRecipe.ProductSteps;
            lbSteps.DisplayMember = "StepName";
            lbSteps.SelectedIndex = -1;

        }

        private void rbTypeTrans_CheckedChanged(object sender, EventArgs e)
        {
            //if (rbTypeMaterial.Checked){
            //    cbBondPositionList.Enabled = false;
            //    cbBondPositionList.SelectedIndex = -1;
            //    cbComponentList.Enabled = true;
            //}
        }

        private void rbTypeEutectic_CheckedChanged(object sender, EventArgs e)
        {
            //if (rbTypeEutectic.Checked)
            //{
            //    cbBondPositionList.Enabled = true;
            //    cbComponentList.Enabled = false;
            //    cbComponentList.SelectedIndex = -1;
            //}
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //this.Close();
        }


        private void btnStepItemDel_Click(object sender, EventArgs e)
        {
            int index = lbSteps.SelectedIndex;
            if (index >= _editRecipe.ProductSteps.Count || index < 0)
            {
                return;
            }
            DialogResult ret = MessageBox.Show("是否删除选中项？", "删除确认", MessageBoxButtons.YesNo);
            if (ret == DialogResult.Yes)
            {
                lbSteps.DataSource = null;
                _editRecipe.ProductSteps.RemoveAt(index);
                lbSteps.DataSource = _editRecipe.ProductSteps;
                lbSteps.DisplayMember = "StepName";
                lbSteps.SelectedIndex = index - 1;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //ProductRunForm form = new ProductRunForm();
            //form.MinimizeBox = false;
            //form.MaximizeBox = false;
            //form.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将保存修改？", "是否保存配方参数？") == 1)
            {
                SaveCurrentComponent();
                SaveBondPositionInfo();
                //_editRecipe.EutecticParameters.ParameterIndex = int.Parse(cmbEutecticParamIndex.Text);
                WarningBox.FormShow("成功。", "参数保存完成。");
            }
        }
        /// <summary>
        /// 保存芯片信息
        /// </summary>
        private void SaveCurrentComponent()
        {
            
            try
            {
                curStepComp.ChipPPPickSystemPos = curComponentDetail.CurrentChipPPPickPos;
                //curStepComp.ChipPPPlacePos = curComponentDetail.CurrentChipPPPlacePos;
                curStepComp.PPSettings.PickupStress = curComponentDetail.CurrentChipPPPress;
                curStepComp.PPSettings.DelayMSForVaccum = curComponentDetail.CurrentVaccumDelayMS;
                curStepComp.PPSettings.NeedleUpHeight = curComponentDetail.CurrentNeedleUpHeight;

                curStepComp.PPSettings.PlaceStress = curComponentDetail.CurrentPlaceStress;
                curStepComp.PPSettings.DelayMSForPlace = curComponentDetail.CurrentDelayMSForPlace;
                curStepComp.PPSettings.BreakVaccumTimespanMS = curComponentDetail.CurrentBreakVaccumTimespanMS;

                //_editRecipe.SubstrateInfos.SubmountPPPickPos = curComponentDetail.CurrentSubmountPPPickPos;
                //_editRecipe.SubstrateInfos.SubmountPPPlacePos = curComponentDetail.CurrentSubmountPPPlacePos;
                //_editRecipe.SubstrateInfos.PPSettings.PickupStress = curComponentDetail.CurrentSubmountPPPress;

                //_editRecipe.SubstrateInfos.PPSettings.DelayMSForVaccum = curComponentDetail.CurrentVaccumDelayMS;


                var xmlFile = $@"{_componentsSavePath}\{curStepComp.Name}\{curStepComp.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(curStepComp, xmlFile, Encoding.UTF8);
                _editRecipe.SaveRecipe();
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "RecipeStep_ProductStep-SaveCurrentComponent,Error.", ex);
            }

        }
        /// <summary>
        /// 保存贴装位置的修改
        /// </summary>
        private void SaveBondPositionInfo()
        {

            try
            {
                curStepBondingPos.BondPositionCompensation.X = positionDetail1.CurrentBondPositionCompensationX;
                curStepBondingPos.BondPositionCompensation.Y = positionDetail1.CurrentBondPositionCompensationY;
                curStepBondingPos.BondPositionCompensation.Z = positionDetail1.CurrentBondPositionCompensationZ;
                curStepBondingPos.BondPositionCompensation.Theta = positionDetail1.CurrentBondPositionCompensationT;
                curStepBondingPos.BondPositionWithPatternOffset.Theta = positionDetail1.CurrentBondPositionTheta;
                curStepBondingPos.BondPositionWithMaterialCenterOffset.Theta = positionDetail1.CurrentBondPositionTheta;
                curStepBondingPos.SystemHeight = positionDetail1.CurrentBondPositionWorkHeight;

                curStepBondingPos.DispenserPositionCompensation.X = positionDetail1.CurrentDispenserPositionCompensationX;
                curStepBondingPos.DispenserPositionCompensation.Y = positionDetail1.CurrentDispenserPositionCompensationY;
                curStepBondingPos.DispenserPositionCompensation.Z = positionDetail1.CurrentDispenserPositionCompensationZ;

                curStepBondingPos.chipPositionCompensation.X = positionDetail1.CurrentchipPositionCompensationX;
                curStepBondingPos.chipPositionCompensation.Y = positionDetail1.CurrentchipPositionCompensationY;
                curStepBondingPos.chipPositionCompensation.Z = positionDetail1.CurrentchipPositionCompensationZ;


                var xmlFile = $@"{_bondPositionSavePath}\{curStepBondingPos.Name}\{curStepBondingPos.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(curStepBondingPos, xmlFile, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "RecipeStep_ProductStep-SaveBPCompensation,Error.", ex);
            }

        }
    }
}