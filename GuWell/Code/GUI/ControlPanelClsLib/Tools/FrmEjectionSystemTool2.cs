using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
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
using UserManagerClsLib;

namespace ControlPanelClsLib.Tools
{
    public partial class FrmEjectionSystemTool2 : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private string _currentToolName;
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        public FrmEjectionSystemTool2()
        {
            InitializeComponent();
            LoadExistESTool();
        }

        private void LoadExistESTool()
        {
            cmbExistESTool.Items.Clear();
            foreach (var item in _systemConfig.ESToolSettings)
            {
                cmbExistESTool.Items.Add(item.Name);
            }
        }

        private void cmbExistESTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentToolName = cmbExistESTool.Text;

            ESToolSettings currentTool = null;
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                currentTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
            }
            if (currentTool != null)
            {
               

                ChipPPPosBracketX.Text = currentTool.NeedleCenter.X.ToString();
                ChipPPPosBracketY.Text = currentTool.NeedleCenter.Y.ToString();

                LookuptoPPOrigionX.Text = currentTool.BondIdentifyNeedleCenter.X.ToString();
                LookuptoPPOrigionY.Text = currentTool.BondIdentifyNeedleCenter.Y.ToString();
                LookuptoPPOrigionZ.Text = currentTool.BondIdentifyNeedleCenter.Z.ToString();

                NeedleZeorPosition.Text = currentTool.NeedleZeorPosition.ToString();

            }
        }

        private void btnNewESTool_Click(object sender, EventArgs e)
        {
            FrmAddESTool frm = new FrmAddESTool();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ESToolSettings newTool = new ESToolSettings() { Name = frm.NewName };
                cmbExistESTool.Text = frm.NewName;
                _currentToolName = frm.NewName;
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

        private void btnSetup_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否已经输入顶针在晶圆相机中的像素坐标？", "提示") == 1)
            {
                ESToolSettings currentTool = null;
                if (!string.IsNullOrEmpty(_currentToolName))
                {
                    currentTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
                }
                if (currentTool != null)
                {
                    LogRecorder.RecordUserOperationLog($"顶针:{_currentToolName}设置顶针在晶圆相机中的位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                    var pixelCoorX = float.Parse(spinEdit2.Text);
                    var pixelCoorY = float.Parse(spinEdit1.Text);
                    //currentTool.NeedleCenter.X = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    //currentTool.NeedleCenter.Y = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    var visionOffset = _positioningSystem.ConvertPixelCoorToMMCenterCoor(new PointF(pixelCoorX, pixelCoorY), EnumCameraType.WaferCamera);
                    currentTool.NeedleCenter.X = visionOffset.Item1;
                    currentTool.NeedleCenter.Y = visionOffset.Item2;

                    ChipPPPosBracketX.Text = currentTool.NeedleCenter.X.ToString();
                    ChipPPPosBracketY.Text = currentTool.NeedleCenter.Y.ToString();

                    _systemConfig.SaveConfig();
                }
            }

           
           



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "榜头相机是否已经对准顶针中心？", "提示") == 1)
            {
                ESToolSettings currentTool = null;
                if (!string.IsNullOrEmpty(_currentToolName))
                {
                    currentTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
                }
                if (currentTool != null)
                {
                    LogRecorder.RecordUserOperationLog($"顶针:{_currentToolName}设置榜头相机对准顶针的位置", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                    double BondX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                    double BondY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
                    double BondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);

                    
                    currentTool.BondIdentifyNeedleCenter.X = BondX;
                    currentTool.BondIdentifyNeedleCenter.Y = BondY;
                    currentTool.BondIdentifyNeedleCenter.Z = BondZ;

                    LookuptoPPOrigionX.Text = BondX.ToString();
                    LookuptoPPOrigionY.Text = BondY.ToString();
                    LookuptoPPOrigionZ.Text = BondZ.ToString();

                    _systemConfig.SaveConfig();

                }
            }
                

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "顶针是否升降到与顶针座齐平？", "提示") == 1)
            {
                ESToolSettings currentTool = null;
                if (!string.IsNullOrEmpty(_currentToolName))
                {
                    currentTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
                }
                if (currentTool != null)
                {
                    LogRecorder.RecordUserOperationLog($"顶针:{_currentToolName}设置顶针原点高度", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);
                    double NeedleZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.NeedleZ);
                    currentTool.NeedleZeorPosition = (float)NeedleZ;

                    NeedleZeorPosition.Text = currentTool.NeedleZeorPosition.ToString();

                    _systemConfig.SaveConfig();
                }
            }
               

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            ESToolSettings currentTool = null;
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                currentTool = _systemConfig.ESToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
            }
            if (currentTool != null)
            {


                currentTool.NeedleCenter.X = double.Parse(ChipPPPosBracketX.Text);
               currentTool.NeedleCenter.Y = double.Parse(ChipPPPosBracketY.Text);

                currentTool.BondIdentifyNeedleCenter.X = double.Parse(LookuptoPPOrigionX.Text);
                currentTool.BondIdentifyNeedleCenter.Y = double.Parse(LookuptoPPOrigionY.Text);
                currentTool.BondIdentifyNeedleCenter.Z = double.Parse(LookuptoPPOrigionZ.Text);

                currentTool.NeedleZeorPosition = float.Parse(NeedleZeorPosition.Text);

                _systemConfig.SaveConfig();

            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否删除顶针？", "提示") == 1)
            {
                var ESTool = _systemConfig.ESToolSettings?.FirstOrDefault(tool => tool.Name == cmbExistESTool.Text);
                _systemConfig.ESToolSettings.Remove(ESTool);
                LogRecorder.RecordUserOperationLog($"删除顶针工具{cmbExistESTool.Text}", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, UserManager.Instance.CurrentUserName);

                cmbExistESTool.Items.Clear();

                foreach (var ES in _systemConfig.DispenserSettings)
                {
                    cmbExistESTool.Items.Add(ES.Name);
                }
                cmbExistESTool.Text = "";
            }
        }
    }
}