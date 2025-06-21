using GlobalDataDefineClsLib;
using IOUtilityClsLib;
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
using VisionGUI;
using WestDragon.Framework.UtilityHelper;

namespace ControlPanelClsLib
{
    public partial class EjectionSystemTool_NeedleZero : UserControl
    {
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        public EjectionSystemTool_NeedleZero()
        {
            InitializeComponent();
            InitialCameraControl();
        }
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            var cameraView = new CameraWindowGUI();
            cameraView.InitVisualControl();
            cameraView.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(cameraView);
            cameraView.SelectCamera(2);
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
        }

        private void btnESBaseUPDown_Click(object sender, EventArgs e)
        {

        }

        private void btnVaccumOnOff_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsESBaseVaccumOpened())
            {
                IOUtilityHelper.Instance.CloseESBaseVaccum();
            }
            else
            {
                IOUtilityHelper.Instance.OpenESBaseVaccum();
            }
        }

        private void btnNeedleGoZero_Click(object sender, EventArgs e)
        {
            _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, 0, EnumCoordSetType.Absolute);
        }
    }
}
