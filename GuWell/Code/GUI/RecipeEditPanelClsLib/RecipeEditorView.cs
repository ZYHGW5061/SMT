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
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraBars;
using GlobalDataDefineClsLib;
using CommonPanelClsLib;
using RecipeClsLib;
using ConfigurationClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using System.IO;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeEditorView : ViewBase
    {
        /// <summary>
        /// 记录当前正在编辑的Recipe对象
        /// </summary>
        private BondRecipe _editRecipe = null;

        /// <summary>
        /// 切换到Recipe子节点时执行的异步委托
        /// </summary>
        public Action<string, EnumRecipeRootStep> ChangetoRecipeNode { get; set; }
        public Action<string, EnumRecipeRootStep> ChangetoRecipeByChildNode { get; set; }

        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfiguration
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 系统日志记录器
        /// </summary>
        private static IBaseLogger _systemLogger
        {
            get { return LoggerManager.GetHandler().GetFileLogger(GlobalParameterSetting.SYSTEM_DEBUG_LOGGER_ID); }
        }

        private static string SystemDefaultDirectory = SystemConfiguration.Instance.SystemDefaultDirectory;
        private static string _substrateSavePath = string.Format(@"{0}Recipes\Substrate\", SystemDefaultDirectory);
        private static string _componentsSavePath = string.Format(@"{0}Recipes\Components\", SystemDefaultDirectory);
        private static string _bondPositionSavePath = string.Format(@"{0}Recipes\BondPositions\", SystemDefaultDirectory);
        private static string _epoxyApplicationSavePath = string.Format(@"{0}Recipes\EpoxyApplication\", SystemDefaultDirectory);

        /// <summary>
        /// 页面无参构造函数
        /// </summary>
        public RecipeEditorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面有参构造函数，传入Recipe对象
        /// </summary>
        /// <param name="recipe"></param>
        public RecipeEditorView(BondRecipe recipe)
            : this()
        {
            //传入Reipe对象
            _editRecipe = recipe;

            //委托初始化
            ChangetoRecipeNode = new Action<string, EnumRecipeRootStep>(ChangeToRecipeNodeView);
            ChangetoRecipeByChildNode = new Action<string, EnumRecipeRootStep>(ChangeToRecipeNodeViewByChildNode);
            //recipeNodeContainer.DisplayRecipeStepName = new Action<string>(OnDisplayRecipeStepName);
            RefreshRecipeNode();
        }

        private void RefreshRecipeNode()
        {
            //ParentTreeListAddSubstrateListNode(_editRecipe.SubstrateInfos.Name);
            //foreach (var item in _editRecipe.StepComponentList)
            //{
            //    ParentTreeListAddComponentListNode(item.Name);
            //}
            //foreach (var item in _editRecipe.StepBondingPositionList)
            //{
            //    ParentTreeListAddBondPositionListNode(item.Name);
            //}
            //foreach (var item in _editRecipe.StepEpoxyApplicationList)
            //{
            //    ParentTreeListAddEpoxyApplicationListNode(item.Name);
            //}

            var childs = Directory.GetDirectories(_substrateSavePath);
            for (int index = 0; index < childs.Length; index++)
            {
                var childName = Path.GetFileName(childs[index]);
                ParentTreeListAddSubstrateListNode(childName);
            }
            var childs2 = Directory.GetDirectories(_componentsSavePath);
            for (int index = 0; index < childs2.Length; index++)
            {
                var childName = Path.GetFileName(childs2[index]);
                ParentTreeListAddComponentListNode(childName);
            }
            var childs3 = Directory.GetDirectories(_bondPositionSavePath);
            for (int index = 0; index < childs3.Length; index++)
            {
                var childName = Path.GetFileName(childs3[index]);
                ParentTreeListAddBondPositionListNode(childName);
            }
            var childs4 = Directory.GetDirectories(_epoxyApplicationSavePath);
            for (int index = 0; index < childs4.Length; index++)
            {
                var childName = Path.GetFileName(childs4[index]);
                ParentTreeListAddEpoxyApplicationListNode(childName);
            }

        }

        /// <summary>
        /// 页面加载时执行
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                //默认展开所有节点
                this.treeRecipeNodes.ExpandAll();
                this.treeListColumn1.OptionsColumn.AllowEdit = false;
                this.treeListColumn2.OptionsColumn.AllowEdit = false;
                //刷新Recipe各子节点完成状态
                this.RefreshParentTreeNodeStatus();

                //默认为空
                this.lblsubViewCaption.Text = string.Empty;
            }
            finally
            {
                base.OnLoad(e);
            }
        }

        /// <summary>
        /// 通知整个Recipe定义完成
        /// </summary>
        /// <param name="recipe"></param>
        private void NotifyRecipeDefineCompleted(BondRecipe recipe, EnumRecipeStep currentStep)
        {
            ///更新_editRecipe的数据，不在此处赋值时，数据有时保存不成功。
            this._editRecipe = recipe;

            //保存Recipe对象
            this._editRecipe.SaveRecipe(currentStep);

            //刷新当前树上各Recipe节点完成状态
            RefreshParentTreeNodeStatus();
            RefreshChildTreeNodeStatus();
            //提示成功,是否进入下一页面编辑
            //TreeListNode nextStepNode = null;
            //var next = GetNextStepCaption(currentStep, out nextStepNode);
            //if (WarningBox.FormShow("保存成功", string.IsNullOrEmpty(next) ? "创建完成." : "单击<yes>转到下一步 ? ", "提示") == 1)
            if (WarningBox.FormShow("保存成功", "编辑已完成。", "提示") == 1)
            {
                //if (nextStepNode == null)
                //{
                //    ChangetoRecipeNode(next, EnumRecipeRootStep.None);
                //}
                //else
                //{
                //    if (nextStepNode.ParentNode == null)
                //    {
                //        ChangetoRecipeNode(next, EnumRecipeRootStep.None);
                //    }
                //    else
                //    {
                //        var parentNodeName = nextStepNode.ParentNode.GetDisplayText(0);
                //        var recipeRootStep = EnumRecipeRootStep.None;
                //        if (parentNodeName == "基板")
                //        {
                //            recipeRootStep = EnumRecipeRootStep.Submount;
                //        }
                //        else if (parentNodeName == "打胶")
                //        {
                //            recipeRootStep = EnumRecipeRootStep.Dispenser;
                //        }
                //        else if (parentNodeName == "贴装位置")
                //        {
                //            recipeRootStep = EnumRecipeRootStep.BondPosition;
                //        }
                //        else if (parentNodeName == "芯片")
                //        {
                //            recipeRootStep = EnumRecipeRootStep.Component;
                //        }
                //        ChangetoRecipeNode(next, recipeRootStep);
                //    }
                //    //将下一步功能节点设置为当前正在编辑的状态
                //    treeRecipeNodes.SetFocusedNode(nextStepNode);
                //}
            }
        }
        private RecipeNodeControl _previousFuncClient = null;
        /// <summary>
        /// 切换到子节点页面
        /// </summary>
        /// <param name="type"></param>
        private void ChangeToRecipeNodeView(string nodeCaption, EnumRecipeRootStep recipeRootStep)
        {
            this.Invoke(new Action(() =>
            {
                if (nodeCaption == "贴装位置" || nodeCaption == "基板" || nodeCaption == "芯片" || nodeCaption == "胶水设置") return;
                RecipeNodeControl funcClient = null;
                if (_previousFuncClient != null)
                {
                    _previousFuncClient.ReleaseResource();
                }
                //释放所有动态加载的控件
                while (recipeNodeContainer.Controls.Count > 0)
                {
                    var control = recipeNodeContainer.Controls[0];
                    recipeNodeContainer.Controls.Remove(control);
                    control.Dispose();
                    control = null;
                }
                if (recipeRootStep == EnumRecipeRootStep.Substrate || recipeRootStep == EnumRecipeRootStep.Component)
                {
                    return;
                }
                if (recipeRootStep == EnumRecipeRootStep.BondPosition)
                {
                    funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_BondPositionSettings));// { Dock = DockStyle.Fill };
                    funcClient.Visible = false;
                }
                else if (recipeRootStep == EnumRecipeRootStep.EpoxySettings)
                {
                    funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_EpoxyApplication));// { Dock = DockStyle.Fill };
                    funcClient.Visible = false;
                }
                else
                {
                    switch (nodeCaption)
                    {
                        case "划胶器":
                            //recipeRootStep = EnumRecipeRootStep.Configuration;
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_DispenserSettings));// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                            break;
                        case "工艺列表":
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ProductStep), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                            break;
                        //case "下料设置":
                        //    funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_BlankingSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                        //    funcClient.Visible = false;
                        //    break;
                        //case "工艺步骤":
                        //    break;
                        default:
                            funcClient = null;
                            break;
                    }
                }

                if (funcClient != null)
                {
                    _previousFuncClient = funcClient;
                    funcClient.Size = new Size(this.recipeNodeContainer.Width, this.recipeNodeContainer.Height + 40);
                    //funcClient.Parent = recipeNodeContainer;
                    //funcClient.Location = this.PointToScreen(new Point(3000, 150));
                    funcClient.Location = new Point(690, 180);
                    funcClient.NotifyRecipeSaved = NotifyRecipeDefineCompleted;
                    var note = "";
                    if (recipeRootStep == EnumRecipeRootStep.None)
                    {
                        //this.lblsubViewCaption.Text = string.Format("当前步骤：{0}", nodeCaption);
                        note = string.Format("当前步骤：{0}", nodeCaption);
                    }
                    else
                    {
                        //this.lblsubViewCaption.Text = string.Format("当前步骤：{0}-{1}", recipeRootStep, nodeCaption);
                        note = string.Format("当前步骤：{0}-{1}", GetRootStepName(recipeRootStep), nodeCaption);
                    }
                    funcClient.Text = note;
                    //this.recipeNodeContainer.Controls.Add(funcClient);
                    funcClient.ShowDialog();
                    //funcClient.Visible = true;
                }
            }));
        }
        private void ChangeToRecipeNodeViewByChildNode(string nodeCaption, EnumRecipeRootStep recipeRootStep)
        {
            this.Invoke(new Action(() =>
            {
                if (nodeCaption == "贴装位置" || nodeCaption == "基板" || nodeCaption == "芯片") return;
                RecipeNodeControl funcClient = null;
                if (_previousFuncClient != null)
                {
                    _previousFuncClient.ReleaseResource();
                }
                //释放所有动态加载的控件
                while (recipeNodeContainer.Controls.Count > 0)
                {
                    var control = recipeNodeContainer.Controls[0];
                    recipeNodeContainer.Controls.Remove(control);
                    control.Dispose();
                    control = null;
                }
                switch (nodeCaption)
                {
                    case "基本设置":
                        if (recipeRootStep == EnumRecipeRootStep.Substrate)
                        {
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_SubstrateInfoSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                        }
                        else
                        {
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ComponentInfoSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                        }
                        break;
                    case "定位设置":
                        if (recipeRootStep == EnumRecipeRootStep.Substrate)
                        {
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_SubstratePositionSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                        }
                        else
                        {
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ComponentPositionSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                        }
                        break;
                    case "Map设置":
                        if (recipeRootStep == EnumRecipeRootStep.Substrate)
                        {
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_SubstrateMapSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                        }
                        else
                        {
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ComponentMapSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                        }
                        break;
                    case "拾取设置":
                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ComponentPPSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;                       
                        break;
                    case "二次定位":

                            funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ComponentAccuracySettings), recipeRootStep);// { Dock = DockStyle.Fill };
                            funcClient.Visible = false;
                       
                        break;
                    case "Module定位设置":

                        funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ModulePositionSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                        funcClient.Visible = false;

                        break;
                    case "Module Map设置":

                        funcClient = new RecipeNodeControl(_editRecipe, typeof(RecipeStep_ModuleMapSettings), recipeRootStep);// { Dock = DockStyle.Fill };
                        funcClient.Visible = false;

                        break;
                    default:
                        funcClient = null;
                        break;
                }

                if (funcClient != null)
                {
                    _previousFuncClient = funcClient;
                    funcClient.Size = new Size(this.recipeNodeContainer.Width, this.recipeNodeContainer.Height + 40);
                    funcClient.Location = new Point(690, 180);
                    funcClient.NotifyRecipeSaved = NotifyRecipeDefineCompleted;
                    var note = "";
                    if (recipeRootStep == EnumRecipeRootStep.None)
                    {
                        //this.lblsubViewCaption.Text = string.Format("当前步骤：{0}", nodeCaption);
                        note = string.Format("当前步骤：{0}", nodeCaption);
                    }
                    else
                    {
                        var recipeRootStepName = GetRootStepName(recipeRootStep);
                        if (recipeRootStep == EnumRecipeRootStep.Substrate)
                        {
                            //_editRecipe.CurrentPositionSettingsName = nodeCaption;
                            //this.lblsubViewCaption.Text = string.Format("当前步骤：{0}-{1}", recipeRootStep, nodeCaption);
                            note = string.Format("当前步骤：{0}-{1}", recipeRootStepName, nodeCaption);
                        }
                        else if (recipeRootStep == EnumRecipeRootStep.Component)
                        {
                            //this.lblsubViewCaption.Text = string.Format("当前步骤：{0}-{1}", recipeRootStep, nodeCaption);
                            note = string.Format("当前步骤：{0}({1})-{2}", recipeRootStepName,_mainTreeCurNodeCaption, nodeCaption);
                        }
                        else
                        {
                            //this.lblsubViewCaption.Text = string.Format("当前步骤：{0}-{1}", recipeRootStep, nodeCaption);
                            note = string.Format("当前步骤：{0}-{1}", recipeRootStepName, nodeCaption);
                        }
                    }


                    funcClient.Text = note;
                    //this.recipeNodeContainer.Controls.Add(funcClient);
                    //funcClient.Visible = true;
                    funcClient.ShowDialog();
                }
            }));
        }

        private void OnDisplayRecipeStepName(string stepName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { OnDisplayRecipeStepName(stepName); });
                return;
            }
            this.lblsubViewCaption.Text = stepName;
        }

        /// <summary>
        /// 刷新Recipe子节点的完成状态,recipe都有默认值，在创建后认为是完成编辑了
        /// </summary>
        private void RefreshParentTreeNodeStatus()
        {
            if (_editRecipe != null)
            {
                foreach (TreeListNode node in treeRecipeNodes.Nodes)
                {
                    var funncType = node.GetDisplayText(0);
                    bool isCompleted = false;
                    bool isActive = true;
                    if (funncType == "配置")
                    {
                        //if (_editRecipe.MaterialInfo != null) { isCompleted = true; }
                        node.Tag = isActive;
                    }
                    else if (funncType == "打胶")
                    {
                        node.Tag = isActive;
                        //this.RefreshChildNodeStatus(node);

                    }
                    else if (funncType == "基板")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_Substrate();
                        this.RefreshSecondaryNodeStatus(node, "基板");
                    }
                    else if (funncType == "贴装位置")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_BondPositions();
                        this.RefreshSecondaryNodeStatus(node, "贴装位置");
                    }
                    else if (funncType == "芯片")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_Components();
                        this.RefreshSecondaryNodeStatus(node,"芯片");

                    }
                    else if (funncType == "共晶设置")
                    {

                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_EutecticSettings();
                    }
                    else if (funncType == "工艺列表")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_ProcessList();

                    }
                    else if (funncType == "下料设置")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_BlankingSettings();

                    }
                    else if (funncType == "划胶器")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_DispenserSettings();


                    }
                    else if (funncType == "胶水设置")
                    {
                        node.Tag = isActive;
                        isCompleted = _editRecipe.IsStepComplete_EpoxyApplication();
                        this.RefreshSecondaryNodeStatus(node, "胶水设置");

                    }
                    if (isCompleted)
                        node.ImageIndex = 3;
                    else
                        node.ImageIndex = 4;

                    if (!isActive)
                        node.ImageIndex = 2;
                    node.SelectImageIndex = node.ImageIndex;
                }
                treeRecipeNodes.ExpandAll();
            }
        }
        private void RefreshSecondaryNodeStatus(TreeListNode node,string parentNodeName)
        {
            bool isActive = false;
            if ((bool)node.Tag) isActive = true;
            isActive = true;
            foreach (TreeListNode childNode in node.Nodes)
            {
                var funncType = childNode.GetDisplayText(0);
                childNode.Tag = isActive;
                bool isCompleted = false;
                if(parentNodeName=="芯片")
                {
                    isCompleted = _editRecipe.IsStepComplete_Component(funncType);
                }
                else if(parentNodeName == "贴装位置")
                {
                    isCompleted = _editRecipe.IsStepComplete_BondPosition(funncType);
                }
                else if (parentNodeName == "胶水设置")
                {
                    isCompleted = _editRecipe.IsStepComplete_EpoxyApplication(funncType);
                }
                else if (parentNodeName == "基板")
                {
                    isCompleted = _editRecipe.IsStepComplete_Substrate();
                }
                if (isCompleted)
                    childNode.ImageIndex = 3;
                else
                    childNode.ImageIndex = 4;
                if (!isActive)
                {
                    childNode.ImageIndex = 2;
                }

                childNode.SelectImageIndex = childNode.ImageIndex;
            }
        }
        private void RefreshChildTreeNodeStatus()
        {
            bool isActive = true;
            foreach (TreeListNode childNode in treeChildNodes.Nodes)
            {
                var funncType = childNode.GetDisplayText(0);
                childNode.Tag = isActive;
                bool isCompleted = false;

                if (funncType == "基本设置")
                {
                    if (SelectedRootStep == EnumRecipeRootStep.Component)
                    {
                        isCompleted = _editRecipe.IsStepComplete_ComponentInfo(_mainTreeCurNodeCaption);
                    }
                    //else if (SelectedRootStep == EnumRecipeRootStep.Submount)
                    else
                    {
                        isCompleted = _editRecipe.IsStepComplete_SubstrateInfo();
                    }
                }
                else if (funncType == "定位设置")
                {
                    if (SelectedRootStep == EnumRecipeRootStep.Component)
                    {
                        isCompleted = _editRecipe.IsStepComplete_ComponentPosition(_mainTreeCurNodeCaption);
                    }
                    //else if (SelectedRootStep == EnumRecipeRootStep.Submount)
                    else
                    {
                        isCompleted = _editRecipe.IsStepComplete_SubstratePosition();
                    }
                }
                else if (funncType == "Map设置")
                {
                    if (SelectedRootStep == EnumRecipeRootStep.Component)
                    {
                        isCompleted = _editRecipe.IsStepComplete_ComponentMap(_mainTreeCurNodeCaption);
                    }
                    //else if (SelectedRootStep == EnumRecipeRootStep.Submount)
                    else
                    {
                        isCompleted = _editRecipe.IsStepComplete_SubstrateMap();
                    }
                }
                else if (funncType == "拾取设置")
                {
                    if (SelectedRootStep == EnumRecipeRootStep.Component)
                    {
                        isCompleted = _editRecipe.IsStepComplete_ComponentPPSettings(_mainTreeCurNodeCaption);
                    }
                    //else if (SelectedRootStep == EnumRecipeRootStep.Submount)
                    else
                    {
                        isCompleted = _editRecipe.IsStepComplete_SubmountPPSettings();
                    }
                }
                else if (funncType == "二次定位")
                {
                    if (SelectedRootStep == EnumRecipeRootStep.Component)
                    {
                        isCompleted = _editRecipe.IsStepComplete_ComponentAccuracy(_mainTreeCurNodeCaption);
                    }
                    //else if (SelectedRootStep == EnumRecipeRootStep.Submount)
                    else
                    {
                        isCompleted = _editRecipe.IsStepComplete_SubmountAccuracy();
                    }
                }
                else if (funncType == "Module定位设置")
                {

                    isCompleted = _editRecipe.IsStepComplete_ModulePosition();

                }
                else if (funncType == "Module Map设置")
                {

                    isCompleted = _editRecipe.IsStepComplete_ModuleMap();

                }
                if (isCompleted)
                    childNode.ImageIndex = 3;
                else
                    childNode.ImageIndex = 4;
                if (!isActive)
                {
                    childNode.ImageIndex = 2;
                }

                childNode.SelectImageIndex = childNode.ImageIndex;
            }
        }

        /// <summary>
        /// 获取下一步的步骤节点名称
        /// </summary>
        /// <returns></returns>
        private string GetNextStepCaption(EnumRecipeStep currentStep, out TreeListNode nextStepNode)
        {
            string nextStepCaption = "";
            string nextStepRootCaption = "";
            switch (currentStep)
            {
                case EnumRecipeStep.Create:
                    break;
                case EnumRecipeStep.Configuration:
                    nextStepCaption = "基本设置";
                    nextStepRootCaption = "基板";
                    break;
                case EnumRecipeStep.Substrate_InformationSettings:
                    nextStepCaption = "定位设置";
                    nextStepRootCaption = "基板";
                    break;
                case EnumRecipeStep.Substrate_PositionSettings:
                    nextStepCaption = "Map设置";
                    nextStepRootCaption = "基板";
                    break;
                case EnumRecipeStep.Substrate_MaterialMap:
                    nextStepCaption = "二次定位";
                    nextStepRootCaption = "基板";
                    break;
                case EnumRecipeStep.Substrate_Accuracy:
                    break;
                case EnumRecipeStep.BondPosition:
                    nextStepCaption = "基本设置";
                    nextStepRootCaption = "芯片";
                    break;
                case EnumRecipeStep.Component_InformationSettings:
                    nextStepCaption = "定位设置";
                    nextStepRootCaption = "芯片";
                    break;
                case EnumRecipeStep.Component_PositionSettings:
                    nextStepCaption = "Map设置";
                    nextStepRootCaption = "芯片";
                    break;
                case EnumRecipeStep.Component_MaterialMap:
                    nextStepCaption = "二次定位";
                    nextStepRootCaption = "芯片";
                    break;
                case EnumRecipeStep.Component_Accuracy:
                    nextStepCaption = "工艺列表";
                    nextStepRootCaption = "";
                    break;
                case EnumRecipeStep.ProductStepList:
                    break;
                case EnumRecipeStep.None:
                    break;
                default:
                    break;
            }
            //switch (currentStep)
            //{
            //    case EnumRecipeStep.Create:
            //        break;
            //    case EnumRecipeStep.Configuration:
            //        nextStepCaption = "基本设置";
            //        nextStepRootCaption = "基板";
            //        break;
            //    case EnumRecipeStep.Component_InformationSettings:
            //        break;
            //    case EnumRecipeStep.Component_PositionSettings:
            //        break;
            //    case EnumRecipeStep.Component_MaterialMap:
            //        break;
            //    case EnumRecipeStep.Component_Accuracy:
            //        break;
            //    case EnumRecipeStep.ProcessList:
            //        break;
            //    case EnumRecipeStep.None:
            //        break;
            //    default:
            //        break;
            //}

            if (string.IsNullOrEmpty(nextStepCaption))
            {
                nextStepNode = null;
            }
            else
            {
                nextStepNode = GetNextStepNode(nextStepCaption, nextStepRootCaption, treeRecipeNodes.Nodes);
                //nextStepCaption = nextStepCaption.Substring(nextStepCaption.IndexOf("_") + 1);
            }
            return nextStepCaption;
        }

        /// <summary>
        /// 递归查找下一步功能的节点
        /// </summary>
        /// <param name="nextStepCaption"></param>
        /// <returns></returns>
        private TreeListNode GetNextStepNode(string nextStepCaption, string nextStepRootCaption, TreeListNodes nextStepNodeRoot)
        {
            string rootNodeCaption = nextStepRootCaption;
            string childNodeCaption = nextStepCaption;

            TreeListNode nextStepNode = null;
            foreach (TreeListNode node in nextStepNodeRoot)
            {
                if (node.GetDisplayText(0) != rootNodeCaption) continue;
                foreach (TreeListNode childNode in node.Nodes)
                {
                    if (childNode.GetDisplayText(0) == childNodeCaption)
                    {
                        nextStepNode = childNode;
                        break;
                    }
                }
            }
            return nextStepNode;
        }
        private DateTime _laseClickTime = DateTime.Now;
        /// <summary>
        /// 双击Recip节点时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeRecipeNodes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //这里增加1s内不允许多次双击逻辑，避免Application.DoEvents()引起的重入问题，从而引起访问已释放的控件对象
            DateTime clickTime = DateTime.Now;
            TimeSpan clickTimeSpan = clickTime - _laseClickTime;
            if (clickTimeSpan.TotalMilliseconds < 1000)
            {
                return;
            }
            _laseClickTime = clickTime;
            TreeListHitInfo hi = treeRecipeNodes.CalcHitInfo(new Point(e.X, e.Y));
            TreeListNode node = hi.Node;
            if (node == null) return;
            node.SelectImageIndex = node.ImageIndex;
            if (node != null)
            {
                if (((bool)node.Tag) == true)
                {
                    EnumRecipeRootStep recipeRootStep = EnumRecipeRootStep.None;
                    if (node.ParentNode != null)
                    {
                        recipeRootStep = GetRootStepByName(node.ParentNode.GetDisplayText(0));
                    }
                    ChangetoRecipeNode.Invoke(node.GetDisplayText(0), recipeRootStep);
                }
                else
                {
                    return;//"该步骤当前不可用，完成之前步骤后该步骤才会可用
                }
            }
        }
        private EnumRecipeRootStep GetRootStepByName(string name)
        {
            var ret = EnumRecipeRootStep.None;
            if (name == "配置")
            {

            }
            else if (name == "打胶")
            {
                ret = EnumRecipeRootStep.Dispenser;

            }
            else if (name == "基板")
            {
                ret = EnumRecipeRootStep.Substrate;

            }
            else if (name == "贴装位置")
            {
                ret = EnumRecipeRootStep.BondPosition;

            }
            else if (name == "芯片")
            {
                ret = EnumRecipeRootStep.Component;

            }
            else if (name == "工艺列表")
            {

            }
            else if (name == "胶水设置")
            {
                ret = EnumRecipeRootStep.EpoxySettings;
            }
            return ret;
        }
        private string GetRootStepName(EnumRecipeRootStep step)
        {
            var ret = "";
            switch (step)
            {
                case EnumRecipeRootStep.None:
                    break;
                case EnumRecipeRootStep.Substrate:
                    ret = "基板";
                    break;
                case EnumRecipeRootStep.Dispenser:
                    break;
                case EnumRecipeRootStep.BondPosition:
                    ret = "贴装位置";
                    break;
                case EnumRecipeRootStep.Component:
                    ret = "芯片";
                    break;
                case EnumRecipeRootStep.EpoxySettings:
                    ret = "胶水设置";
                    break;
                default:
                    break;
            }
            return ret;
        }

        private string _mainTreeCurNodeCaption = string.Empty;
        private string _mainTreeCurNodeParentNodeCaption = string.Empty;
        EnumRecipeRootStep SelectedRootStep
        {
            get
            {
                var ret = EnumRecipeRootStep.None;
                if (!string.IsNullOrEmpty(_mainTreeCurNodeParentNodeCaption))
                {
                    ret = GetRootStepByName(_mainTreeCurNodeParentNodeCaption);
                }
                return ret;
            }
        }
    /// <summary>
    /// 单击Recip节点时发生
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void treeRecipeNodes_MouseClick(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hi = treeRecipeNodes.CalcHitInfo(new Point(e.X, e.Y));
            TreeListNode node = hi.Node;
            if (node == null) return;
            _mainTreeCurNodeCaption = node.GetDisplayText(0);
            if (node.ParentNode != null)
            {
                _mainTreeCurNodeParentNodeCaption = node.ParentNode.GetDisplayText(0);
            }
            else
            {
                _mainTreeCurNodeParentNodeCaption = "";
            }

            

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                node.SelectImageIndex = node.ImageIndex;
                if (node.GetDisplayText(0) != "基板" && node.GetDisplayText(0) != "芯片" && node.GetDisplayText(0) != "贴装位置" && node.GetDisplayText(0) != "胶水设置")
                {
                    if (node.ParentNode != null)
                    {
                        //选中芯片的子节点时，子节点树显示内容，并更新当前芯片模组名称
                        if (node.ParentNode.GetDisplayText(0) == "芯片")
                        {
                            RefreshChildNodesTree();
                            _editRecipe.CurrentComponentInfosName = _mainTreeCurNodeCaption;
         
                                node.SelectImageIndex = node.ImageIndex;
                                return;
                            



                        }
                        // 选中基板的子节点时，子节点树显示内容，并更新当前基板模组名称
                        else if (node.ParentNode.GetDisplayText(0) == "基板")
                        {
                            RefreshChildNodesTree();
                            _editRecipe.SubstrateInfos.Name = _mainTreeCurNodeCaption;

                            node.SelectImageIndex = node.ImageIndex;
                            return;




                        }
                        //选中贴装位置的子节点时，子节点树清空，并更新当前贴装位置模组名称
                        else if (node.ParentNode.GetDisplayText(0) == "贴装位置")
                        {
                            _editRecipe.CurrentBondPositionSettingsName = _mainTreeCurNodeCaption;
                            RefreshChildNodesTree(true);

                                node.SelectImageIndex = node.ImageIndex;
                                return;
                           
                        }
                        else if (node.ParentNode.GetDisplayText(0) == "胶水设置")
                        {
                            _editRecipe.CurrentEpoxyApplicationName = _mainTreeCurNodeCaption;
                            RefreshChildNodesTree(true);
                                node.SelectImageIndex = node.ImageIndex;
                                return;                         
                        }
                        else
                        {
                            RefreshChildNodesTree(true);
                        }
                    }
                    else
                    {
                        if (node.GetDisplayText(0) == "基板")
                        {
                            RefreshChildNodesTree();
                        }
                        else
                        {
                            //当选中节点为芯片和帖子位置这两个父节点时，清空子节点树
                            RefreshChildNodesTree(true);
                        }
                    }
                    return;
                }
                else
                {
                    if (node.GetDisplayText(0) != "基板")
                    {
                        RefreshChildNodesTree(true);
                    }
                    else
                    {
                        RefreshChildNodesTree();
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.barbtnAdd.Visibility = BarItemVisibility.Always;
                this.barbtnDelete.Visibility = BarItemVisibility.Always;
                if (node.GetDisplayText(0) == "基板" || node.GetDisplayText(0) == "芯片" || node.GetDisplayText(0) == "贴装位置" || node.GetDisplayText(0) == "胶水设置")
                {
                    popupMenu1.ShowPopup(this.PointToScreen(e.Location));
                }
                else if(node.ParentNode!=null&&(node.ParentNode.GetDisplayText(0) == "基板" || node.ParentNode.GetDisplayText(0) == "芯片"|| node.ParentNode.GetDisplayText(0) == "贴装位置" || node.ParentNode.GetDisplayText(0) == "胶水设置"))
                {
                    popupMenu2.ShowPopup(this.PointToScreen(e.Location));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_mainTreeCurNodeCaption == "基板")
            {
                FrmNew frm = new FrmNew();
                frm.SetFormTitle("新建基板");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ParentTreeListAddNode(frm.NewName);
                    RefreshChildNodesTree();
                    //RefreshSubNodeStatus();
                    RefreshParentTreeNodeStatus();
                }
            }
            else if (_mainTreeCurNodeCaption == "芯片")
            {
                FrmNewComponent frm = new FrmNewComponent();
                frm.SetFormTitle("新建芯片");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _editRecipe.StepComponentList.Add(new ProgramComponentSettings { Name = $"{ frm.NewName}_{_editRecipe.RecipeName}",MaterialType=EnumMaterialType.Chip });
                    ParentTreeListAddNode($"{ frm.NewName}_{_editRecipe.RecipeName}");
                    RefreshChildNodesTree();
                    RefreshParentTreeNodeStatus();

                }
            }
            else if (_mainTreeCurNodeCaption == "贴装位置")
            {
                FrmNewBP frm = new FrmNewBP();
                frm.SetFormTitle("新建贴装位置");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _editRecipe.StepBondingPositionList.Add(new BondingPositionSettings { Name = $"{ frm.NewName}_{_editRecipe.RecipeName}", FindBondPositionMethod = frm.FindBondPositionMethod });
                    ParentTreeListAddNode($"{ frm.NewName}_{_editRecipe.RecipeName}");
                    RefreshParentTreeNodeStatus();
                }
            }
            else if (_mainTreeCurNodeCaption == "胶水设置")
            {
                FrmNewComponent frm = new FrmNewComponent();
                frm.SetFormTitle("新建胶水设置");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _editRecipe.StepEpoxyApplicationList.Add(new EpoxyApplication { Name = $"{ frm.NewName}_{_editRecipe.RecipeName}"});
                    ParentTreeListAddNode($"{ frm.NewName}_{_editRecipe.RecipeName}");
                    RefreshParentTreeNodeStatus();
                }
            }
            //if (_curNodeCaption == "BrightField")
            //    _editRecipe.BrightFieldStep.IsActive = true;
            //else
            //    _editRecipe.DarkFieldStep.IsActive = true;
            //foreach (TreeListNode node in treeRecipeNodes.Nodes)
            //{
            //    if (node.GetDisplayText(0) == _curNodeCaption)
            //    {
            //        node.ImageIndex = 0;
            //        node.Tag = true;
            //        this.RefreshChildNodeStatus(node);
            //        treeRecipeNodes.MoveNext();
            //        break;
            //    }
            //}
            _editRecipe.SaveRecipe();
        }
        private void ParentTreeListAddNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == _mainTreeCurNodeCaption)
                {
                    var newNode = node.Nodes.Add();
                    newNode.SetValue(0, nodeName);
                    newNode.StateImageIndex = -1;
                    newNode.ImageIndex = 4;
                    newNode.SelectImageIndex = 4;
                    break;
                }
            }
            //treeRecipeNodes.MoveNext();
        }
        private void ParentTreeListAddSubstrateListNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "基板")
                {
                    var newNode = node.Nodes.Add();
                    newNode.SetValue(0, nodeName);
                    newNode.StateImageIndex = -1;
                    newNode.ImageIndex = 4;
                    newNode.SelectImageIndex = 4;
                    break;
                }
            }
        }
        private void ParentTreeListAddComponentListNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "芯片")
                {
                    var newNode = node.Nodes.Add();
                    newNode.SetValue(0, nodeName);
                    newNode.StateImageIndex = -1;
                    newNode.ImageIndex = 4;
                    newNode.SelectImageIndex = 4;
                    break;
                }
            }
        }
        private void ParentTreeListDeleteComponentChildNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "芯片")
                {
                    foreach (TreeListNode cnode in node.Nodes)
                    {
                        if(cnode.GetDisplayText(0) == nodeName)
                        {
                            cnode.Remove();
                            _mainTreeCurNodeCaption = "";
                            break;
                        }
                    }
                }
            }
        }
        private void ParentTreeListAddBondPositionListNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "贴装位置")
                {
                    var newNode = node.Nodes.Add();
                    newNode.SetValue(0, nodeName);
                    newNode.StateImageIndex = -1;
                    newNode.ImageIndex = 4;
                    newNode.SelectImageIndex = 4;
                    break;
                }
            }
        }
        private void ParentTreeListDeleteBondPositionChildNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "贴装位置")
                {
                    foreach (TreeListNode cnode in node.Nodes)
                    {
                        if (cnode.GetDisplayText(0) == nodeName)
                        {
                            cnode.Remove();
                            _mainTreeCurNodeCaption = "";
                            break;
                        }
                    }
                }
            }
        }

        private void ParentTreeListAddEpoxyApplicationListNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "胶水设置")
                {
                    var newNode = node.Nodes.Add();
                    newNode.SetValue(0, nodeName);
                    newNode.StateImageIndex = -1;
                    newNode.ImageIndex = 4;
                    newNode.SelectImageIndex = 4;
                    break;
                }
            }
        }
        private void ParentTreeListDeleteEpoxyApplicationChildNode(string nodeName)
        {
            foreach (TreeListNode node in treeRecipeNodes.Nodes)
            {
                if (node.GetDisplayText(0) == "胶水设置")
                {
                    foreach (TreeListNode cnode in node.Nodes)
                    {
                        if (cnode.GetDisplayText(0) == nodeName)
                        {
                            cnode.Remove();
                            _mainTreeCurNodeCaption = "";
                            break;
                        }
                    }
                }
            }
        }
        private void RefreshChildNodesTree(bool isClear = false)
        {
            treeChildNodes.Nodes.Clear();
            if (!isClear)
            {
                if (_mainTreeCurNodeParentNodeCaption == "基板")
                {
                    var node = treeChildNodes.Nodes.Add();
                    node.SetValue(0, "基本设置");
                    node.StateImageIndex = -1;
                    node.ImageIndex = 4;
                    node.SelectImageIndex = 4;
                    node.Tag = true;

                    var node1 = treeChildNodes.Nodes.Add();
                    node1.SetValue(0, "定位设置");
                    node1.StateImageIndex = -1;
                    node1.ImageIndex = 4;
                    node1.SelectImageIndex = 4;
                    node1.Tag = true;

                    var node2 = treeChildNodes.Nodes.Add();
                    node2.SetValue(0, "Map设置");
                    node2.StateImageIndex = -1;
                    node2.ImageIndex = 4;
                    node2.SelectImageIndex = 4;
                    node2.Tag = true;

                    var node3 = treeChildNodes.Nodes.Add();
                    node3.SetValue(0, "Module定位设置");
                    node3.StateImageIndex = -1;
                    node3.ImageIndex = 4;
                    node3.SelectImageIndex = 4;
                    node3.Tag = true;

                    var node4 = treeChildNodes.Nodes.Add();
                    node4.SetValue(0, "Module Map设置");
                    node4.StateImageIndex = -1;
                    node4.ImageIndex = 4;
                    node4.SelectImageIndex = 4;
                    node4.Tag = true;

                }
                else if (_mainTreeCurNodeParentNodeCaption == "芯片")
                {
                    var node = treeChildNodes.Nodes.Add();
                    node.SetValue(0, "基本设置");
                    node.StateImageIndex = -1;
                    node.ImageIndex = 4;
                    node.SelectImageIndex = 4;
                    node.Tag = true;

                    var node1 = treeChildNodes.Nodes.Add();
                    node1.SetValue(0, "定位设置");
                    node1.StateImageIndex = -1;
                    node1.ImageIndex = 4;
                    node1.SelectImageIndex = 4;
                    node1.Tag = true;

                    var node2 = treeChildNodes.Nodes.Add();
                    node2.SetValue(0, "Map设置");
                    node2.StateImageIndex = -1;
                    node2.ImageIndex = 4;
                    node2.SelectImageIndex = 4;
                    node2.Tag = true;
                    var node3 = treeChildNodes.Nodes.Add();
                    node3.SetValue(0, "拾取设置");
                    node3.StateImageIndex = -1;
                    node3.ImageIndex = 4;
                    node3.SelectImageIndex = 4;
                    node3.Tag = true;

                    var node4 = treeChildNodes.Nodes.Add();
                    node4.SetValue(0, "二次定位");
                    node4.StateImageIndex = -1;
                    node4.ImageIndex = 4;
                    node4.SelectImageIndex = 4;
                    node4.Tag = true;
                }

                RefreshChildTreeNodeStatus();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barbtnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (WarningBox.FormShow("动作确认！", "是否删除选中的项？", "提示") == 1)
            {
                if (_mainTreeCurNodeParentNodeCaption == "芯片")
                {

                    var removeItem = _editRecipe.StepComponentList.FirstOrDefault(c => c.Name == _mainTreeCurNodeCaption);
                    if (removeItem != null)
                    {
                        _editRecipe.StepComponentList.Remove(removeItem);
                    }
                    ParentTreeListDeleteComponentChildNode(_mainTreeCurNodeCaption);
                }
                else if (_mainTreeCurNodeParentNodeCaption == "贴装位置")
                {
                    var removeItem = _editRecipe.StepBondingPositionList.FirstOrDefault(c => c.Name == _mainTreeCurNodeCaption);
                    if (removeItem != null)
                    {
                        _editRecipe.StepBondingPositionList.Remove(removeItem);
                    }
                    ParentTreeListDeleteBondPositionChildNode(_mainTreeCurNodeCaption);
                }
                else if (_mainTreeCurNodeParentNodeCaption == "胶水设置")
                {
                    var removeItem = _editRecipe.StepEpoxyApplicationList.FirstOrDefault(c => c.Name == _mainTreeCurNodeCaption);
                    if (removeItem != null)
                    {
                        _editRecipe.StepEpoxyApplicationList.Remove(removeItem);
                    }
                    ParentTreeListDeleteEpoxyApplicationChildNode(_mainTreeCurNodeCaption);
                }
                _editRecipe.SaveRecipe();
                RefreshChildNodesTree(true);
            }

        }


        /// <summary>
        /// User control for testing recipe
        /// </summary>
        //private RecipeTestPanel _recipeTestPanel = null;

        /// <summary>
        /// Execute recipe for testing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestRecipe_Click(object sender, EventArgs e)
        {
            if (_editRecipe != null)
            {
                if (WarningBox.FormShow("动作确认", "执行当前配方？", "提示") == 1)
                {
                    //FrmSelectMaterialsToWeld selsectMaterial = new FrmSelectMaterialsToWeld();
                    try
                    {
                        //selsectMaterial.ShowMaterialMap(_editRecipe);
                        //if (selsectMaterial.ShowDialog() == DialogResult.OK)
                        //{
                        //    var selected = selsectMaterial.GetSelectedFeatures();
                        //}
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        //selsectMaterial.Dispose();
                        //selsectMaterial = null;
                    }
                }

            }
        }

        private void CloseTestRecipeFormHandler(object sender, FormClosingEventArgs e)
        {
            DialogResult ret = DevExpress.XtraEditors.XtraMessageBox.Show("Do you want to close this from now？", "Close？", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ret == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                //_recipeTestPanel.Abort();
                e.Cancel = false;  // 关闭窗体
            }
        }

        private void treeChildNodes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //这里增加1s内不允许多次双击逻辑，避免Application.DoEvents()引起的重入问题，从而引起访问已释放的控件对象
            DateTime clickTime = DateTime.Now;
            TimeSpan clickTimeSpan = clickTime - _laseClickTime;
            if (clickTimeSpan.TotalMilliseconds < 1000)
            {
                return;
            }
            _laseClickTime = clickTime;
            TreeListHitInfo hi = treeChildNodes.CalcHitInfo(new Point(e.X, e.Y));
            TreeListNode node = hi.Node;
            if (node == null) return;
            node.SelectImageIndex = node.ImageIndex;
            if (node != null)
            {
                if (((bool)node.Tag) == true)
                {
                    EnumRecipeRootStep recipeRootStep = EnumRecipeRootStep.None;
                    if (!string.IsNullOrEmpty(_mainTreeCurNodeParentNodeCaption))
                    {
                        recipeRootStep = GetRootStepByName(_mainTreeCurNodeParentNodeCaption);
                    }
                    else
                    {
                        if(_mainTreeCurNodeCaption=="基板")
                        {
                            recipeRootStep = EnumRecipeRootStep.Substrate;
                        }
                    }
                    ChangetoRecipeByChildNode.Invoke(node.GetDisplayText(0), recipeRootStep);
                }
                else
                {
                    return;//"该步骤当前不可用，完成之前步骤后该步骤才会可用
                }
            }
        }

        private void treeChildNodes_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {

        }
    }
}
