using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraBars;
using ConfigurationClsLib;
using RecipeClsLib;
using GlobalDataDefineClsLib;
using WestDragon.Framework.UtilityHelper;
using CommonPanelClsLib;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeTree : XtraUserControl
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        /// <summary>
        /// Recipe节点被选择
        /// </summary>
        public Action<BondRecipe> OnRecipeSelected;

        public TreeListNode ParentRootNode
        {
            get
            {
                foreach (TreeListNode node in treeList1.Nodes.FirstNode.Nodes)
                {
                    var funncType = node.GetDisplayText(0);
                    if (funncType == "Bonder")
                    {
                        return node;
                    }
                }
                throw new NotSupportedException("不能没有Bonder节点");
            }
        }
        /// <summary>
        /// Recipe节点是否被选择
        /// </summary>
        public bool IsRecipeNodeSelected
        {
            get
            {
                return treeList1.Selection.Count > 0 && treeList1.Selection[0].Level != 0 && treeList1.Selection[0].Level != 1;
            }
        }
        /// <summary>
        /// 获取选择的节点对应的Recipe
        /// </summary>
        public BondRecipe RecipeSelected
        {
            get
            {
                if (treeList1.Selection.Count > 0 && treeList1.Selection[0].Level != 0 && treeList1.Selection[0].Level != 1)
                {
                    string recipeName = treeList1.Selection[0].GetValue(0).ToString();
                    treeList1.Selection[0].Tag = BondRecipe.LoadRecipe(recipeName);
                    var recipeFile = treeList1.Selection[0].Tag as BondRecipe;
                    return recipeFile;
                }
                return null;
            }
        }
        /// <summary>
        /// 当选择Recipe时执行
        /// </summary>
        public Action<BondRecipe> OnRecipeEditAct { get; set; }
        public List<string> OpenedRecipe;

        public RecipeTree()
        {
            InitializeComponent();
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
        }

        /// <summary>
        /// 加载全部的Recipes
        /// </summary>
        public void LoadRecipesFromSystem()
        {
            ParentRootNode.Nodes.Clear();
            this.Invoke(new Action(() =>
            {
                string recipeDir = _systemConfig.SystemDefaultDirectory + @"Recipes\Bonder";
                CommonProcess.EnsureFolderExist(recipeDir);
                CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\{1}\Components\", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString()));
                CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\{1}\BondPositions\", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString()));
                CommonProcess.EnsureFolderExist(string.Format(@"{0}Recipes\{1}\EpoxyApplication\", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString()));
                var recipeFiles = Directory.GetDirectories(recipeDir);
                for (int recipeIndex = 0; recipeIndex < recipeFiles.Length; recipeIndex++)
                {
                    var recipeName = Path.GetFileName(recipeFiles[recipeIndex]);
                    if (recipeName != "BondPositions" && recipeName != "Components" && recipeName != "EpoxyApplication")
                    {
                        TreeListNode recipeParentNode = null;
                        recipeParentNode = ParentRootNode;
                        var recipeNode = recipeParentNode.Nodes.Add();
                        recipeNode.SetValue(0, recipeName);
                        recipeNode.StateImageIndex = 1;
                        recipeNode.ImageIndex = 1;
                        recipeNode.SelectImageIndex = 1;
                    }
                }
                treeList1.ExpandAll();
                if (recipeFiles.Length > 0)
                {
                    treeList1.FocusedNode = treeList1.Nodes[0].Nodes[0];
                }
            }));
        }

        /// <summary>
        /// 新建产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnNewProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.OpenedRecipe != null && this.OpenedRecipe.Count >= 1)
            {
                XtraMessageBox.Show("Another recipe is editing now.", "Tips"); return;
            }
            var recipeNew = AddNewRecipe();
            if (OnRecipeEditAct != null)
            {
                OnRecipeEditAct(recipeNew);
            }
        }

        /// <summary>
        /// 节点右键菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitinfo = treeList1.CalcHitInfo(e.Location);
                if (hitinfo.HitInfoType != HitInfoType.Empty && hitinfo.HitInfoType != HitInfoType.None)
                {
                    hitinfo.Node.Selected = true;
                    if (hitinfo.Node.Level == 0 || hitinfo.Node.Level == 1)
                    {
                        //增加新产品节点
                        barbtnNewProduct.Visibility = BarItemVisibility.Always;
                        barbtnNewProduct.Tag = hitinfo.Node;
                        barbtnEditRecipe.Visibility = BarItemVisibility.Never;
                        barbtnDeleteRecipe.Visibility = BarItemVisibility.Never;
                        barbtnRecipeCopy.Visibility = BarItemVisibility.Never;
                        barbtnRecipeRename.Visibility = BarItemVisibility.Never;
                        popupMenu1.ShowPopup(this.PointToScreen(e.Location));
                    }
                    else if (hitinfo.Node.Level == 2)
                    {
                        //编辑Recipe
                        barbtnNewProduct.Visibility = BarItemVisibility.Never;
                        barbtnEditRecipe.Visibility = BarItemVisibility.Always;
                        barbtnDeleteRecipe.Visibility = BarItemVisibility.Always;
                        barbtnRecipeCopy.Visibility = BarItemVisibility.Always;
                        barbtnRecipeRename.Visibility = BarItemVisibility.Always;
                        barbtnEditRecipe.Tag = hitinfo.Node;
                        //treeList1.FocusedNode = hitinfo.Node;
                        popupMenu1.ShowPopup(this.PointToScreen(e.Location));
                    }
                }
            }
        }

        /// <summary>
        /// 双击鼠标左键时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hitinfo = treeList1.CalcHitInfo(e.Location);
                if (hitinfo.HitInfoType != HitInfoType.Empty && hitinfo.HitInfoType != HitInfoType.None)
                {
                    if (IsRecipeNodeSelected)
                    {
                        if (OnRecipeSelected != null)
                        {
                            OnRecipeSelected(RecipeSelected);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新建一个Recipe
        /// </summary>
        public BondRecipe AddNewRecipe()
        {
            BondRecipe recipe = null;
            var addProductDialog = new AddNewRecipeForm();
            if (addProductDialog.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                string newRecipeName = addProductDialog.RecipeName;
                TreeListNode recipeNodes = ParentRootNode;

                if (!IsExistRecipeName(newRecipeName, EnumRecipeType.Bonder))
                {
                    TreeListNode recipeNode = recipeNodes.Nodes.Add();
                    recipeNode.SetValue(0, newRecipeName);
                    BondRecipe recipeAdded = new BondRecipe()
                    {
                        RecipeName = newRecipeName
                    };
                    recipeNode.StateImageIndex = 1;
                    recipeNode.ImageIndex = 1;
                    recipeNode.SelectImageIndex = 1;
                    recipeNode.Tag = recipeAdded;
                    string fullRecipeName = string.Format(@"{0}Recipes\{1}\{2}\{3}.xml", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString(), newRecipeName, newRecipeName);
                    string fullRecipeFolder = string.Format(@"{0}Recipes\{1}\{2}\", _systemConfig.SystemDefaultDirectory, EnumRecipeType.Bonder.ToString(), newRecipeName);
                    string templateFolderName = $@"{_systemConfig.SystemDefaultDirectory}Recipes\{EnumRecipeType.Bonder.ToString()}\{newRecipeName}\TemplateConfig\";
                    CommonProcess.EnsureFolderExist(templateFolderName);
                    recipeAdded.NewRecipe(fullRecipeName,EnumRecipeStep.Create);
                    treeList1.ExpandAll();
                    recipe = recipeAdded;
                }
                else
                {
                    recipe = null;
                    WarningBox.FormShow("错误", $"配方已存在: {newRecipeName} ,请重试!", "提示");
                }
            }
            addProductDialog.Dispose();
            return recipe;
        }

        /// <summary>
        /// 删除选中的Recipe节点
        /// </summary>
        public void DeleteSelectedRecipe()
        {
            if (this.IsRecipeNodeSelected)
            {
                if ((XtraMessageBox.Show("Are you sure delete this recipe！", "warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)) == DialogResult.OK)
                {
                    var recipe = this.RecipeSelected;
                    if (OpenedRecipe != null && OpenedRecipe.Contains(recipe.RecipeName))
                    {
                        XtraMessageBox.Show(string.Format("Make sure the recipe {0} is closed!", recipe.RecipeName), "Warning");
                        return;
                    }
                    //删除Recipe内容
                    recipe.Delete();

                    TreeListNode recipeNodes = ParentRootNode;
                    //if (recipe.WaferSize == EnumWaferSize.INCH8)
                    //{
                    //    recipeNodes = ParentRootNodeFor8Inch;
                    //}
                    //else if (recipe.WaferSize == EnumWaferSize.INCH12)
                    //{
                    //    recipeNodes = ParentRootNodeFor12Inch;
                    //}
                    //删除节点
                    recipeNodes.Nodes.Remove(treeList1.Selection[0]);
                }
            }
        }

        /// <summary>
        /// 复制选中的Recipe节点
        /// </summary>
        public void CopySelectedRecipe()
        {
            if (this.IsRecipeNodeSelected)
            {
                //CopyRecipeForm copyRecipeForm = new CopyRecipeForm();
                //copyRecipeForm.RecipeName = treeList1.FocusedNode.GetValue(0).ToString() + "_copy";
                //if (copyRecipeForm.ShowDialog() == DialogResult.OK)
                //{
                //    string newRecipeName = copyRecipeForm.RecipeName;
                //    var recipe = this.RecipeSelected;
                //    if (!IsExistRecipeName(newRecipeName, EnumRecipeType.Welder))
                //    {
                //        //复制Recipe文件夹
                //        if (recipe.Copy(newRecipeName))
                //        {
                //            var recipeFullPathName = Path.GetFileName(string.Format(_systemConfig.SystemDefaultDirectory +
                //                @"Recipes\{0}\{1}", EnumRecipeType.Welder.ToString(), newRecipeName));

                //            TreeListNode recipeNodes = ParentRootNodeForWelder;


                //            TreeListNode recipeNode = recipeNodes.Nodes.Add();
                //            recipeNode.SetValue(0, recipeFullPathName);
                //            recipeNode.StateImageIndex = 1;
                //            recipeNode.ImageIndex = 1;
                //            recipeNode.SelectImageIndex = 1;
                            
                //            var newRecipe = ProcessRecipe.LoadRecipe(recipeFullPathName, EnumRecipeType.Welder);
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TrainFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TrainFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TrainFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TrainFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "LidTrainFile.contourmxml");
                //            }
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TemplateFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TemplateFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TemplateFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLid.TemplateFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "LidTrainImage.bmp");
                //            }

                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TrainFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TrainFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TrainFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TrainFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "LidMarkTrainFile.contourmxml");
                //            }
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TemplateFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TemplateFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TemplateFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLidMark.TemplateFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "LidMarkTrainImage.bmp");
                //            }

                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TrainFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TrainFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TrainFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TrainFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "ShellShortsideMarkTrainFile.contourmxml");
                //            }
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TemplateFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TemplateFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TemplateFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellShortsideMark.TemplateFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "ShellShortsideMarkTrainImage.bmp");
                //            }


                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TrainFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TrainFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TrainFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TrainFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "ShortsideShellTrainFile.contourmxml");
                //            }
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TemplateFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TemplateFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TemplateFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShortsideShell.TemplateFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "ShortsideShellTrainImage.bmp");
                //            }



                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TrainFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TrainFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TrainFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TrainFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "ShellLongsideMarkTrainFile.contourmxml");
                //            }
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TemplateFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TemplateFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TemplateFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForShellLongsideMark.TemplateFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "ShellLongsideMarkTrainImage.bmp");
                //            }


                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TrainFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TrainFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TrainFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TrainFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "LongsideShellTrainFile.contourmxml");
                //            }
                //            if (!string.IsNullOrEmpty(newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TemplateFileFullName))
                //            {
                //                var tempPath = newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TemplateFileFullName.Substring
                //                    (0, newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TemplateFileFullName.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                newRecipe.ProcessParameters.VisionParameters.ShapeMatchConfigForLongsideShell.TemplateFileFullName = Path.Combine(tempPath, copyRecipeForm.RecipeName, "TemplateConfig", "LongsideShellTrainImage.bmp");
                //            }
                //            //if (!string.IsNullOrEmpty(newRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath))
                //            //{
                //            //    var tempPath = newRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath.Substring(0, newRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath.LastIndexOf('\\'));
                //            //    tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //            //    newRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath = Path.Combine(tempPath, copyRecipeForm.RecipeName, "DarkRoiData.json");
                //            //}
                //            newRecipe.RecipeName = newRecipeName; 
                //            newRecipe.SaveRecipe();
                //            recipeNode.Tag = newRecipe;
                //        }
                //    }
                //    else
                //    {
                //        XtraMessageBox.Show(string.Format("Already exist recipe: {0} ,Please try it again!", newRecipeName), "Warning");
                //    }
                //}
                //copyRecipeForm.Dispose();
            }
        }

        private void RenameSelectedRecipe()
        {
            if (this.IsRecipeNodeSelected)
            {
                //int focusedColumnId = 0;
                //CopyRecipeForm copyRecipeForm = new CopyRecipeForm(); copyRecipeForm.Text = "Rename recipe";
                //copyRecipeForm.RecipeName = treeList1.FocusedNode.GetValue(focusedColumnId).ToString();  // 显示当前recipe名字
                //string srcRecipeName = treeList1.FocusedNode.GetValue(focusedColumnId).ToString();
                //string inch = treeList1.FocusedNode.ParentNode.GetValue(0).ToString();
                //if (copyRecipeForm.ShowDialog() == DialogResult.OK)
                //{
                //    if (OpenedRecipe != null && OpenedRecipe.Contains(srcRecipeName))
                //    {
                //        XtraMessageBox.Show(string.Format("Make sure the recipe {0} is closed!", srcRecipeName), "Warning");
                //        return;
                //    }
                //    string destRecipeName = copyRecipeForm.RecipeName;
                //    if (!IsExistRecipeName(destRecipeName, EnumRecipeType.Welder))
                //    {
                //        // 配方的新路径
                //        string destFolderPath = string.Format(_systemConfig.SystemDefaultDirectory + @"Recipes\{0}\{1}", inch, destRecipeName);

                //        // 移动原来recipe的内容到新路径下
                //        string srcFolderPath = string.Format(_systemConfig.SystemDefaultDirectory + @"Recipes\{0}\{1}", inch, srcRecipeName);
                //        if (System.IO.Directory.Exists(srcFolderPath))
                //        {
                //            try
                //            {
                //                System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(srcFolderPath);
                //                folder.MoveTo(destFolderPath);

                //                //更改recipe文件的名称
                //                var srcFileName = string.Format(destFolderPath + @"\{0}.xml", srcRecipeName);
                //                var dstFileName = string.Format(destFolderPath + @"\{0}.xml", destRecipeName);
                //                if (File.Exists(srcFileName))
                //                {
                //                    File.Move(srcFileName, dstFileName);
                //                }
                //                // 更改Recipe保存的名字属性
                //                var tmpRecipe = ProcessRecipe.LoadRecipe(dstFileName);
                //                if (tmpRecipe != null)
                //                {
                //                    //if (!string.IsNullOrEmpty(tmpRecipe.BrightFieldStep.InspectionAreas.RoiJsonPath))
                //                    //{
                //                    //    var tempPath = tmpRecipe.BrightFieldStep.InspectionAreas.RoiJsonPath.Substring(0, tmpRecipe.BrightFieldStep.InspectionAreas.RoiJsonPath.LastIndexOf('\\'));
                //                    //    tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                    //    tmpRecipe.BrightFieldStep.InspectionAreas.RoiJsonPath = Path.Combine(tempPath, copyRecipeForm.RecipeName, "BrightRoiData.json");
                //                    //}
                //                    //if (!string.IsNullOrEmpty(tmpRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath))
                //                    //{
                //                    //    var tempPath = tmpRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath.Substring(0, tmpRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath.LastIndexOf('\\'));
                //                    //    tempPath = tempPath.Substring(0, tempPath.LastIndexOf('\\'));
                //                    //    tmpRecipe.DarkFieldStep.InspectionAreas.RoiJsonPath = Path.Combine(tempPath, copyRecipeForm.RecipeName, "DarkRoiData.json");
                //                    //}
                //                    tmpRecipe.RecipeName = copyRecipeForm.RecipeName;
                //                    tmpRecipe.SaveRecipe(dstFileName);
                //                }
                //                // 在界面上显示重命名后的配方名字
                //                treeList1.FocusedNode.SetValue(focusedColumnId, destRecipeName);
                //            }
                //            catch (IOException)  //子文件夹被打开，移动到不同卷时会引发此异常
                //            {
                //                XtraMessageBox.Show(string.Format("Make sure the recipe {0} folder is closed!", srcRecipeName), "Warning");
                //            }
                //        }
                //        else
                //        {
                //            XtraMessageBox.Show(string.Format("Not found recipe named {0} ,Please refresh recipe list!", srcRecipeName), "Warning");
                //        }
                //    }
                //    else
                //    {
                //        XtraMessageBox.Show(string.Format("Already exist recipe: {0} ,Please try it again!", destRecipeName), "Warning");
                //    }
                //}
                //copyRecipeForm.Dispose();
            }
        }

        /// <summary>
        /// 编辑Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnEditRecipe_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.OpenedRecipe != null && this.OpenedRecipe.Count >= 1)
            {
                XtraMessageBox.Show("Another recipe is editing now.", "Tips"); return;
            }

            if (OnRecipeEditAct != null)
            {
                OnRecipeEditAct(this.RecipeSelected);
            }
        }

        /// <summary>
        /// 删除Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnDeleteRecipe_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeleteSelectedRecipe();
        }
        /// <summary>
        /// 复制Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnRecipeCopy_ItemClick(object sender, ItemClickEventArgs e)
        {
            CopySelectedRecipe();
        }

        /// <summary>
        /// 重命名Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnRecipeRename_ItemClick(object sender, ItemClickEventArgs e)
        {
            RenameSelectedRecipe();
        }

        /// <summary>
        /// 检查当前Recipe中该名称是否已存在,若已存在,则返回false
        /// </summary>
        /// <param name="newRecipeName">新名称</param>
        /// <returns></returns>
        private bool IsExistRecipeName(string newRecipeName, EnumRecipeType type)
        {
            string recipeDir = string.Format(@"{0}Recipes\{1}", _systemConfig.SystemDefaultDirectory, type.ToString());
            string[] recipeFiles = Directory.GetDirectories(recipeDir);
            foreach (string filePath in recipeFiles)
            {
                string recipeName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                if (recipeName.Equals(newRecipeName)) return true;
            }
            return false;
        }
        

    }
}
