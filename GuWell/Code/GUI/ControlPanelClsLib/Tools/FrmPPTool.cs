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
using WestDragon.Framework.UtilityHelper;

namespace ControlPanelClsLib
{
    public partial class FrmPPTool : BaseForm
    {
        public FrmPPTool()
        {
            InitializeComponent();
            if (!DesignTimeTools.IsDesignMode)
            {
                LoadExistTool();
                seBondPosX4LoadPP.Value = (decimal)_systemConfig.PositioningConfig.BondPosition4LoadPP.X;
                seBondPosY4LoadPP.Value = (decimal)_systemConfig.PositioningConfig.BondPosition4LoadPP.Y;
                seBondPosZ4LoadPP.Value = (decimal)_systemConfig.PositioningConfig.BondPosition4LoadPP.Z;
                seBondPosT4LoadPP.Value = (decimal)_systemConfig.PositioningConfig.BondPosition4LoadPP.Theta;
            }
        }
        private string _currentToolName;
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
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
            foreach (var item in _systemConfig.PPToolSettings)
            {
                cmbExistTool.Items.Add(item.Name);
            }
        }

        private void btnNewTool_Click(object sender, EventArgs e)
        {
            FrmAddPPTool frm = new FrmAddPPTool();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                PPToolSettings newTool = new PPToolSettings() { Name = frm.NewName };
                cmbExistTool.Text = frm.NewName;
                _currentToolName = frm.NewName;
                if(!string.IsNullOrEmpty(frm.TemplateName))
                {
                    var templateTool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == frm.TemplateName);
                    newTool = templateTool;
                    newTool.Name = frm.NewName;
                }
                _systemConfig.PPToolSettings.Add(newTool);
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功", "添加完成。", "提示");

            }
            frm.Dispose();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PPToolSettings currentTool=null;
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                currentTool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
            }
            if(currentTool!=null)
            {
                if(tabPane.SelectedPage== tabNavigationPageHeight)
                {
                    currentTool.AltimetryOnMark = ppTool_Height1.PPHeight;
                }
                else if(tabPane.SelectedPage == tabNavigationPageAlign)
                {
                    currentTool.ChipPPPosCompensateCoordinate1.X = ppTool_Alignment1.FirstCenterPosition.X;
                    currentTool.ChipPPPosCompensateCoordinate1.Y = ppTool_Alignment1.FirstCenterPosition.Y;
                    currentTool.ChipPPPosCompensateCoordinate1.Z = ppTool_Alignment1.FirstCenterPosition.Z;
                    currentTool.ChipPPPosCompensateCoordinate1.Theta = ppTool_Alignment1.FirstCenterPosition.Theta;

                    currentTool.ChipPPPosCompensateCoordinate2.X = ppTool_Alignment1.SecondCenterPosition.X;
                    currentTool.ChipPPPosCompensateCoordinate2.Y = ppTool_Alignment1.SecondCenterPosition.Y;
                    currentTool.ChipPPPosCompensateCoordinate2.Z = ppTool_Alignment1.SecondCenterPosition.Z;
                    currentTool.ChipPPPosCompensateCoordinate2.Theta = ppTool_Alignment1.SecondCenterPosition.Theta;
                }
                else if (tabPane.SelectedPage == tabNavigationPagePPESH)
                {
                    currentTool.PPESAltimetryParameter.ESStagePosition = ppesHeight1.ESStageHeight;
                    currentTool.PPESAltimetryParameter.PPSystemPosition = ppesHeight1.ESZSystemHeight;

                }
                else if (tabPane.SelectedPage == tabNavigationPageTeach)
                {
                    currentTool.RotationTablePos4LoadPP = (float)seRotationTablePos4LoadPP.Value;


                }
                _systemConfig.SaveConfig();
                WarningBox.FormShow("成功", "保存工具完成。", "提示");
            }
        }

        private void cmbExistTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentToolName = cmbExistTool.Text;
            PPToolSettings currentTool = null;
            if (!string.IsNullOrEmpty(_currentToolName))
            {
                currentTool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == _currentToolName);
            }
            if (currentTool != null)
            {
                seRotationTablePos4LoadPP.Value = (decimal)currentTool.RotationTablePos4LoadPP;
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

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否保存Load吸嘴位置？", "提示") == 1)
            {
                _systemConfig.PositioningConfig.BondPosition4LoadPP.X = (double)seBondPosX4LoadPP.Value;
                _systemConfig.PositioningConfig.BondPosition4LoadPP.Y = (double)seBondPosY4LoadPP.Value;
                _systemConfig.PositioningConfig.BondPosition4LoadPP.Z = (double)seBondPosZ4LoadPP.Value;
                _systemConfig.PositioningConfig.BondPosition4LoadPP.Theta = (double)seBondPosT4LoadPP.Value;
                _systemConfig.SaveConfig();
            }
        }

        private void btnLoadPP_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否加载吸嘴？请确认榜头上目前无吸嘴！", "提示") == 1)
            {
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 218.74, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, 388.8933, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 341.1824, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 77.86, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 300, EnumCoordSetType.Absolute);

                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, 410.2687, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 88.1397, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 128.4747, EnumCoordSetType.Absolute);

            }

            //if(_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute)==StageMotionResult.Success
            //&&_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -33.4925, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //&&_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, 388.8933, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //&&_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 341.1824, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //&&_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 77.86, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //&&_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 300, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //)
            //{
            //    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, 410.2687, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY,88.1397, EnumCoordSetType.Absolute) == StageMotionResult.Success
            //        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 128.4747, EnumCoordSetType.Absolute) == StageMotionResult.Success)
            //    {

            //    }
            //}

        }

        private void btnUnloadPP_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否卸载吸嘴？", "提示") == 1)
            {
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 218.74, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, 388.8933, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 300, EnumCoordSetType.Absolute);
                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 77.86, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, 341.1824, EnumCoordSetType.Absolute);

                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnCalibratePP_Click(object sender, EventArgs e)
        {

        }
    }
}
