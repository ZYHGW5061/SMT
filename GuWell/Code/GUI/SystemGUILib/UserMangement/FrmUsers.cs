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
    public partial class FrmUsers : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 数据库
        /// </summary>
        //private DBManager _dataBaseManager
        //{
        //    get { return DBManager.DBManagerHandler; }
        //}
        /// <summary>
        /// RightsData数据操作
        /// </summary>
        //private UserRightsManager _rightsDataManage = UserRightsManager.GetHandler();

        //private DataRow _dataRow = null;
        private UserInfos _selUser = null;
        private string _operation = "Edit";
        private string _password;

        public FrmUsers(string operation, UserInfos userInfo)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            this.txtID.Enabled = false;
            _selUser = userInfo;
            _operation = operation;

            this.cbxType.DataSource = UserManager.Instance.GetAllLevel();
            this.cbxType.DisplayMember = "RightsType";
            this.cbxType.ValueMember = "RightsID";

            BindingData();
        }

        private void BindingData()
        {
            if (_operation == "Add")
            {
                //this.txtID.Text = (LoginHelper.GetHandler.GetUserMaxID() + 1).ToString();
                this.txtName.Text = "";
                this.txtPassWord.Text = "";
                this.cbxType.SelectedIndex = 0;
                this.txtDescription.Text = "";
                this.tableLayoutPanel2.Controls.Add(this.btn_Add, 0, 0);
                this.btn_Add.Visible = true;
                this.btn_Edit.Visible = false;
                this.Text = "Add User";
            }
            else if (_operation == "Edit" && _selUser != null)
            {
                //this.txtID.Text = _dataRow[0].ToString();
                //this.txtName.Text = _dataRow[1].ToString();
                //this.txtPassWord.Text = _dataRow[2].ToString();
                //_password = _dataRow[2].ToString();
                //this.cbxType.SelectedIndex = (int)_dataRow[4] - 1;
                //this.txtDescription.Text = _dataRow[3].ToString();
                //this.tableLayoutPanel2.Controls.Add(this.btn_Edit, 0, 0);
                //this.btn_Add.Visible = false;
                //this.btn_Edit.Visible = true;
                //this.Text = "Edit User";
                this.txtID.Text = _selUser.id.ToString();
                this.txtName.Text = _selUser.username;
                this.txtPassWord.Text = _selUser.password;
                _password = _selUser.password;
                this.cbxType.SelectedIndex = UserManager.Instance.GetUserTypeIndex(_selUser.usertype) - 1;
                this.txtDescription.Text = _selUser.description;
                this.tableLayoutPanel2.Controls.Add(this.btn_Edit, 0, 0);
                this.btn_Add.Visible = false;
                this.btn_Edit.Visible = true;
                this.Text = "Edit User";
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            //int maxId = LoginHelper.GetHandler.GetUserMaxID() + 1;
            string Name = this.txtName.Text;
            string password = this.txtPassWord.Text;
            string userType = this.cbxType.Text;
            string description = this.txtDescription.Text;
            int userTypeID = (int)this.cbxType.SelectedIndex + 1;
            if (Name == "" || password == "" || userType == "")
            {
                WarningBox.FormShow("错误", "账户信息不能为空!", "Tips");
                return;
            }
            if (UserManager.Instance.AddUser(Name, password, userTypeID, description))
            {
                WarningBox.FormShow("成功!", "添加账户完成!", "Tips");
            }
            else
            {
                WarningBox.FormShow("失败!", "添加账户失败!", "Tips");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            //int id = Convert.ToInt32(this.txtID.Text);
            string Name = this.txtName.Text;
            string password = this.txtPassWord.Text;
            string userType = this.cbxType.Text;
            string description = this.txtDescription.Text;
            int userTypeID = (int)this.cbxType.SelectedIndex + 1;
            if (Name == "" || password == "" || userType == "")
            {
                WarningBox.FormShow("错误", "账户信息不能为空!", "Tips");
                return;
            }

            if (UserManager.Instance.ChangeUserInfos(_selUser.username, Name, password, userTypeID, description))
            {
                WarningBox.FormShow("成功!", "账户信息修改完成!", "Tips");
            }
            else
            {
                WarningBox.FormShow("失败!", "账户信息修改失败!", "Tips");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}