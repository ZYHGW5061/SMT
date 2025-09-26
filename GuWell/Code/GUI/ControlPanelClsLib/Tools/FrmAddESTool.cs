using CommonPanelClsLib;
using ConfigurationClsLib;
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

namespace ControlPanelClsLib
{
    public partial class FrmAddESTool : BaseForm
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 获取输入的名称
        /// </summary>
        public string NewName 
        {
            get 
            { 
                return GetValidFileName(textNewName.Text);
            } 
        }
        public string TemplateName
        {
            get
            {
                return GetValidFileName(cmbSelectTemplate.Text);
            }
        }
        /// <summary>
        /// 页面构造函数，新建Recipe
        /// </summary>
        public FrmAddESTool()
        {
            InitializeComponent();
            LoadExistESTool();
        }
        private void LoadExistESTool()
        {
            cmbSelectTemplate.Items.Clear();
            foreach (var item in _systemConfig.ESToolSettings)
            {
                cmbSelectTemplate.Items.Add(item.Name);
            }
        }
        /// <summary>
        /// 输入完毕，点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textNewName.Text.Trim()))
            {
                WarningBox.FormShow("错误","请输入名称。","提示");
                return;
            }
            if(_systemConfig.ESToolSettings.Any(i=>i.Name== textNewName.Text.Trim()))
            {
                WarningBox.FormShow("错误", "此名称的工具已存在，请更换名称。", "提示");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// 自动过滤字符串中非法的字符
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetValidFileName(string fileName)
        {
            StringBuilder fileNameBuilder = new StringBuilder(fileName);
            foreach (char inValidChar in Path.GetInvalidFileNameChars ())
            {
                fileNameBuilder.Replace(inValidChar.ToString (), string.Empty);
            }
            return fileNameBuilder.ToString ();
        }
    }
}
