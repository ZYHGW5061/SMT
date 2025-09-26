using CommonPanelClsLib;
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

namespace StageCtrlPanelLib
{
    public partial class FrmStageMaintain : Form
    {
        public FrmStageMaintain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private void btnStartPPMove2Uplook_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("吸嘴移动到仰视中心？", "请确认榜头高度安全！", "提示") == 1)
            {
                _positioningSystem.ChipPPMovetoUplookingCameraCenter();
            }
        }
    }
}
