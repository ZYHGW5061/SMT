using CommonPanelClsLib;
using ConfigurationClsLib;
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
    public partial class FrmEjectionSystemTool : BaseForm
    {
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        public FrmEjectionSystemTool()
        {
            InitializeComponent();
            LoadExistESTool();
        }
        private string _currentToolName;
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        private void LoadExistESTool()
        {
            cmbExistESTool.Items.Clear();
            foreach (var item in _systemConfig.ESToolSettings)
            {
                cmbExistESTool.Items.Add(item.Name);
            }
        }

        private void btnNewESTool_Click(object sender, EventArgs e)
        {
            FrmAddESTool frm = new FrmAddESTool();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ESToolSettings newTool = new ESToolSettings() { Name = frm.NewName };
                cmbExistESTool.Text = frm.NewName;
                _currentToolName= frm.NewName;
                if (!string.IsNullOrEmpty(frm.TemplateName))
                {
                    var templateTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == frm.TemplateName);
                    newTool = templateTool;
                    newTool.Name = frm.NewName;
                }
                _systemConfig.ESToolSettings.Add(newTool);
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功", "添加完成。", "提示");

            }
            frm.Dispose();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ESToolSettings currentTool = null;
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                currentTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
            }
            if (currentTool != null)
            {
                //if (tabPane.SelectedPage == tabNavigationPageBaseHeight)
                //{
                //    currentTool.ESBasePosition = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.ESZ);
                //    //记录激光测高仪的测量数据
                //}
                //else
                if (tabPane.SelectedPage == tabNavigationPageNeedleZero)
                {
                    currentTool.NeedleZeorPosition = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.NeedleZ);
                }
                else if (tabPane.SelectedPage == tabNavigationPageNeedleAlign)
                {
                    var pixelCoorX = ejectionSystemTool_XYMeasuring1.NeedleCenterPixelCoorX;
                    var pixelCoorY = ejectionSystemTool_XYMeasuring1.NeedleCenterPixelCoorY;
                    //currentTool.NeedleCenter.X = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    //currentTool.NeedleCenter.Y = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    var visionOffset=_positioningSystem.ConvertPixelCoorToMMCenterCoor(new PointF(pixelCoorX, pixelCoorY), EnumCameraType.WaferCamera);
                    currentTool.NeedleCenter.X =  visionOffset.Item1;
                    currentTool.NeedleCenter.Y =  visionOffset.Item2;
                }
                else if (tabPane.SelectedPage == tabNavigationPagePPESH)
                {
                    currentTool.ESZStagePosWhenMeasureHeight = ppesHeight1.ESStageHeight;
                    currentTool.ESZToplateHigherValueThanMarkTopplateWhenMeasureESZHeight = ppesHeight1.ESZSystemHeight;

                }
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功", "保存工具完成。", "提示");
            }
        }

        private void cmbExistESTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentToolName = cmbExistESTool.Text;
        }
    }
}
