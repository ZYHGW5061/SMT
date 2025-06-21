using CommonPanelClsLib;
using DevExpress.XtraEditors;
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
    public partial class FrmInputRowCount : XtraForm
    {
        /// <summary>
        /// 获取输入的Recipe的名称
        /// </summary>
        public int NumberofRows
        {
            get 
            { 
                return _numberofRows;
            } 
        }
        private int _numberofRows;
        /// <summary>
        /// 页面构造函数，新建Recipe
        /// </summary>
        public FrmInputRowCount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 输入完毕，点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(teColumnCount.Text.Trim()))
            {
                WarningBox.FormShow("错误","请输入行数。","提示");
                return;
            }
            if (!int.TryParse(teColumnCount.Text.Trim(),out _numberofRows))
            {
                WarningBox.FormShow("错误", "请输入正确的内容。", "提示");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消新建Recipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
