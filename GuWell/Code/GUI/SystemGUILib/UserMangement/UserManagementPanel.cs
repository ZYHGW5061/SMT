using CommonPanelClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserManagerClsLib;

namespace SystemGUILib.UserMangement
{
    public partial class UserManagementPanel : DevExpress.XtraEditors.XtraUserControl
    {

        /// <summary>
        /// 数据库
        /// </summary>
        //private  DBManager _dataBaseManager
        //{
        //    get { return DBManager.DBManagerHandler; }
        //}
        private string sql = "select a.id,a.username,a.password,a.description,a.userRightID,b.RightsType usertype from Users a inner join RightsInfo b  on a.userRightID=b.ID Order by a.ID";
        //获取当前行的数据
        private DataRow dataRow = null;
        private UserInfos _selUser = null;
        public UserManagementPanel()
        {
            InitializeComponent();
            UpdateUserList();
            //DataGridBinding(initalSql());
            //CtrlRightsManager rightsControl = new CtrlRightsManager();
            //rightsControl.Dock = DockStyle.Left;
            //panel1.Controls.Add(rightsControl);
        }


        #region 用户管理   
        /// <summary>
        /// 绑定用户DataGridView
        /// </summary>
        private void DataGridBinding(string sql)
        {
            //sql = "select * from Users Order by ID";
            //gridControl1.BeginUpdate();
            //gridControl1.DataSource = _dataBaseManager.SelectSqlCommand(sql);
            //gridControl1.EndUpdate();
            //gridControl1.RefreshDataSource();
        }
        private void UpdateUserList()
        {
            //sql = "select * from Users Order by ID";
            gridControl1.BeginUpdate();
            gridControl1.DataSource = UserManager.Instance.GetAllUsers();
            gridControl1.EndUpdate();
            gridControl1.RefreshDataSource();
        }
        /// <summary>
        /// GridView行点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            FrmUsers user = new FrmUsers("Add", _selUser);
            user.ShowDialog();
            UpdateUserList();
            //DataGridBinding(initalSql());
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (_selUser != null)
            {
                FrmUsers user = new FrmUsers("Edit", _selUser);
                user.ShowDialog();
                UpdateUserList();
                //DataGridBinding(initalSql());
            }
            else
            {
                WarningBox.FormShow("Tips", "Please select one user!", "Tips");
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Del_Click(object sender, EventArgs e)
        {
            if (_selUser != null)
            {
                if (WarningBox.FormShow("Verification", "Are you sure to delete the user？", "Warning!") == 1)
                {
                    string userName = _selUser.username.ToString();
                    if (UserManager.Instance.DeleteUser(userName))
                    {
                        WarningBox.FormShow("成功!", "删除账户完成!", "Tips");
                    }
                    else
                    {
                        WarningBox.FormShow("失败!", "删除账户失败!", "Tips");
                        return;
                    }
                }
            }
            else
            {
                WarningBox.FormShow("错误", "请先选择要删除的账户!", "Tips");
            }
            dataRow = null;
            UpdateUserList();
            //DataGridBinding(initalSql());
        }

        /// <summary>
        /// 用户查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectUser_Click(object sender, EventArgs e)
        {
            //if (this.txtName.Text.Length > 0)
            //{
            //    sql = string.Format("select * from Users where UserName  like '%{0}%'  and  userRightID !=4  Order by ID", this.txtName.Text);
            //}
            //else
            //{
            //    sql = initalSql();
            //}
            //_dataBaseManager.SingleSqlCommand(sql);
            //DataGridBinding(sql);
        }
        private string initalSql()
        {
            string sqlStr = string.Format("select a.id,a.username,a.password,a.description,a.userRightID,b.RightsType usertype from Users a inner join RightsInfo b  on a.userRightID=b.ID  where a.userRightID != 4 Order by a.ID");
            return sqlStr;
        }
        #endregion     


        #region 权限

        private TreeNode AllTreeNode = new TreeNode();
        /// <summary>
        /// 绑定权限下拉列表
        /// </summary>
        private void ComboxDataSource()
        {
            cmbRightsType.DataSource = UserRightsManager.Instance.GetAllRights();
            cmbRightsType.DisplayMember = "RightsType";
            cmbRightsType.ValueMember = "RightsID";
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        private void LoadRightsTreeDataSource()
        {
            AddTreeNode(AllTreeNode, 0);
            foreach (var item in AllTreeNode.Nodes)
            {
                treeUserRights.Nodes.Add((TreeNode)item);
            }
            treeUserRights.ExpandAll();
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="parentID"></param>
        private void AddTreeNode(TreeNode treeNode, int parentID)
        {
            List<FunctionInfo> listFunctionInfo = UserRightsManager.Instance.GetFunctionInfoByParentID(parentID);
            if (listFunctionInfo != null)
            {
                foreach (var item in listFunctionInfo)
                {
                    TreeNode node = new TreeNode();
                    node.Name = item.FunctionID.ToString();
                    node.Text = item.FunctionName;
                    treeNode.Nodes.Add(node);
                    AddTreeNode(node, item.FunctionID);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void GetFunctionListByUserId(Dictionary<string, FunctionInfo> dicFunctionInfo, int parentID)
        {
            var listFunctions = UserRightsManager.Instance.GetFunctionInfoByParentID(parentID);
            if (listFunctions != null)
            {
                foreach (var item in listFunctions)
                {
                    dicFunctionInfo.Add(item.FunctionName, item);
                    GetFunctionListByUserId(dicFunctionInfo, item.FunctionID);
                }
            }
        }

        /// <summary>
        /// 根据RightsID加载对应的权限
        /// </summary>
        /// <param name="rightsID"></param>
        private void LoadRightsTreeByRightsType(int rightsID)
        {
            var functionRights = UserRightsManager.Instance.GetFunctionRightsInfoByRightsID(rightsID);

            //foreach (TreeNode node in treeUserRights.Nodes)
            //{
            //    var ret = functionRights.FirstOrDefault(i => i.FunctionInfoID.ToString() == node.Name);
            //    if(ret!=null)
            //    {
            //        node.Checked = ret.Visible;
            //    }
            //    else
            //    {
            //        node.Checked = false;
            //    }
            //}
            RefreshRightsTree(functionRights, treeUserRights.Nodes);
            //foreach (var item in functionRights)
            //{
            //    TreeNode[]  nodes =  treeUserRights.Nodes.Find(item.FunctionInfoID.ToString(), true);
            //    if (nodes.Length>0)
            //    {
            //        nodes[0].Checked = item.Visible;
            //    }
            //}
        }
        private void RefreshRightsTree(List<FunctionRightsInfo> rightsInfo, TreeNodeCollection nodes)
        {
            foreach (TreeNode treeNode in nodes)
            {
                var ret = rightsInfo.FirstOrDefault(i => i.FunctionInfoID.ToString() == treeNode.Name);
                if (ret != null)
                {
                    treeNode.Checked = ret.Visible;
                }
                else
                {
                    treeNode.Checked = false;
                }
                if (treeNode.Nodes.Count > 0)
                {
                    RefreshRightsTree(rightsInfo, treeNode.Nodes);
                }
            }
        }
        private List<int> _settedRights = new List<int>();
        /// <summary>
        /// 保存权限功能信息
        /// </summary>
        /// <param name="nodes"></param>
        private void SaveToFunctionRights(TreeNodeCollection nodes)
        {
            _settedRights.Clear();
            RefreshSettedRight(nodes);
            UserRightsManager.Instance.UpdateUserLevelRights((int)cmbRightsType.SelectedValue, _settedRights);
        }
        private void RefreshSettedRight(TreeNodeCollection nodes)
        {
            foreach (var node in nodes)
            {
                TreeNode treeNode = (node as TreeNode);
                if (treeNode.Name == "1" ? true : treeNode.Checked)
                {
                    _settedRights.Add(int.Parse(treeNode.Name));
                }
                if (treeNode.Nodes.Count > 0)
                {
                    RefreshSettedRight(treeNode.Nodes);
                }
            }
        }
        private void SelecedNode(TreeNode node, bool isChecked)
        {
            foreach (var item in node.Nodes)
            {
                TreeNode treeNode = (item as TreeNode);
                treeNode.Checked = isChecked;
                if (treeNode.Nodes.Count > 0)
                {
                    SelecedNode(treeNode, isChecked);
                }
            }
        }

        /// <summary>
        /// 下拉框更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRightsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeUserRights.Nodes)
            {
                item.Checked = false;
            }
            LoadRightsTreeByRightsType((int)cmbRightsType.SelectedValue);
        }

        /// <summary>
        /// 复选框更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeUserRights_AfterCheck(object sender, TreeViewEventArgs e)
        {
            SelecedNode(e.Node, e.Node.Checked);
        }

        /// <summary>
        /// 保存权限设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToFunctionRights(treeUserRights.Nodes);
            WarningBox.FormShow("成功!", "设置完成!", "Tips");
            //XtraMessageBox.Show("Save success.","Tips");
        }

        private void UserManagementPanel_Load(object sender, EventArgs e)
        {
            ComboxDataSource();
            LoadRightsTreeDataSource();
            LoadRightsTreeByRightsType(1);
            cmbRightsType.SelectedIndexChanged += cmbRightsType_SelectedIndexChanged;
            treeUserRights.AfterCheck += treeUserRights_AfterCheck;
        } 
        #endregion
    }
}
