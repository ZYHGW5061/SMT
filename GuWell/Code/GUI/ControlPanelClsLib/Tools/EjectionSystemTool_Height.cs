using CommonPanelClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionGUI;

namespace ControlPanelClsLib
{
    public partial class EjectionSystemTool_Height : UserControl
    {
        public EjectionSystemTool_Height()
        {
            InitializeComponent();
            InitialCameraControl();
        }
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void btnVaccumOnOff_Click(object sender, EventArgs e)
        {

        }

        private void btnESUpDown_Click(object sender, EventArgs e)
        {

        }

        private void btnNeedleGoZero_Click(object sender, EventArgs e)
        {

        }
    }
}
