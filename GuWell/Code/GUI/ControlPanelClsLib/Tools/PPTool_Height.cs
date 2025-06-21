using CommonPanelClsLib;
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
    public partial class PPTool_Height : UserControl
    {
        public PPTool_Height()
        {
            InitializeComponent();
        }
        public float PPHeight { get; set; }

        private void labelStepInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将保存吸嘴校准高度。", "请确认吸嘴已调整到吸附基准台面位置！") == 1)
            {
                //TBD:用吸嘴自动测力
                PPHeight = (float)PositioningSystem.Instance.ReadCurrentStagePosition(EnumStageAxis.BondZ);
            }
        }
    }
}
