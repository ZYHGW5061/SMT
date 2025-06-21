using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.Utils.Design;
using GlobalDataDefineClsLib;
using PositioningSystemClsLib;
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
    public partial class FrmMaterialBoxTool : BaseForm
    {
        public FrmMaterialBoxTool()
        {
            InitializeComponent();
            if (!DesignTimeTools.IsDesignMode)
            {
                LoadExistTool();
            }
        }
        private string _currentToolName;
        /// <summary>
        /// 系统配置
        /// </summary>
        private MaterialBoxToolsConfiguration _systemConfig
        {
            get { return MaterialBoxToolsConfiguration.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private void LoadExistTool()
        {
            cmbExistTool.Items.Clear();
            foreach (var item in _systemConfig.MaterialBoxTools)
            {
                cmbExistTool.Items.Add(item.Name);
            }
        }

        private void btnNewTool_Click(object sender, EventArgs e)
        {
            FrmAddPPTool frm = new FrmAddPPTool();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MaterialBoxToolSettings newTool = new MaterialBoxToolSettings() { Name = frm.NewName };
                cmbExistTool.Text = frm.NewName;
                _currentToolName = frm.NewName;
                if(!string.IsNullOrEmpty(frm.TemplateName))
                {
                    var templateTool = _systemConfig.MaterialBoxTools.FirstOrDefault(i => i.Name == frm.TemplateName);
                    newTool = templateTool;
                    newTool.Name = frm.NewName;
                }
                _systemConfig.MaterialBoxTools.Add(newTool);
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功", "添加完成。", "提示");

            }
            frm.Dispose();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MaterialBoxToolSettings currentTool =null;
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                currentTool = _systemConfig.MaterialBoxTools.FirstOrDefault(i => i.Name == _currentToolName);
            }
            if(currentTool!=null)
            {
                //if(tabPane.SelectedPage== tabNavigationPageHeight)
                //{
                //    //currentTool.AltimetryOnMark = ppTool_Height1.PPHeight;
                //}
                currentTool.FirstSlotPosition = (float)seFirstSlotPosMM.Value;
                currentTool.SlotCount = Int32.Parse(cmbSlotCount.Text);
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功", "保存工具完成。", "提示");
            }
        }

        private void cmbExistTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentToolName = cmbExistTool.Text;
            RefreshToolParameter();
        }

        private void RefreshToolParameter()
        {
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                var tool = _systemConfig.MaterialBoxTools.FirstOrDefault(i => i.Name == _currentToolName);
                if (tool != null)
                {
                    seFirstSlotPosMM.Text = tool.FirstSlotPosition.ToString();
                    cmbSlotCount.Text = tool.SlotCount.ToString();
                }
            }
        }


        private void FrmPPTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (WarningBox.FormShow("即将关闭？", "确认工具参数已保存？", "提示") == 0)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = false;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
