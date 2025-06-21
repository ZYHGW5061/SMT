using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using IOUtilityClsLib;
using GlobalToolClsLib;
using CommonPanelClsLib;

namespace ControlPanelClsLib
{
    public partial class FrmAlarm : BaseForm
    {
        private static readonly object _lockObj = new object();
        private static volatile FrmAlarm _instance = null;
        public static FrmAlarm Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FrmAlarm();
                        }
                    }
                }
                return _instance;
            }
        }

        public FrmAlarm()
        {
            InitializeComponent();
            InitializeInputIOStatus();
            InitializeOutputIOStatus();

        }

        private void InitializeInputIOStatus()
        {

            
        }
        private void InitializeOutputIOStatus()
        {

        }


        private void ChipPPVaccumSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsChipPPVaccumOpened())
            {
                IOUtilityHelper.Instance.CloseChipPPVaccum();
            }
            else
            {
                IOUtilityHelper.Instance.OpenChipPPVaccum();
            }
        }

        private void ChipPPBlowSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsChipPPBlowOpened())
            {
                IOUtilityHelper.Instance.CloseChipPPBlow();
            }
            else
            {
                IOUtilityHelper.Instance.OpenChipPPBlow();
            }
        }

        private void SubmountPPVaccumSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsSubmountPPVaccumOpened())
            {
                IOUtilityHelper.Instance.CloseSubmountPPVaccum();
            }
            else
            {
                IOUtilityHelper.Instance.OpenSubmountPPVaccum();
            }
        }

        private void SubmountPPBlowSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsSubmountPPBlowOpened())
            {
                IOUtilityHelper.Instance.CloseSubmountPPBlow();
            }
            else
            {
                IOUtilityHelper.Instance.OpenSubmountPPBlow();
            }
        }

        private void EutecticPlatformVaccumSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsEutecticPlatformPPVaccumOpened())
            {
                IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();
            }
            else
            {
                IOUtilityHelper.Instance.OpenEutecticPlatformPPVaccum();
            }
        }

        private void WaffleTableVaccumSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsMaterailPlatformVaccumOpened())
            {
                IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
            }
            else
            {
                IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
            }
        }

        private void WaferTableVaccumSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsWaferTableVaccumOpened())
            {
                IOUtilityHelper.Instance.CloseWaferTableVaccum();
            }
            else
            {
                IOUtilityHelper.Instance.OpenWaferTableVaccum();
            }
        }

        private void ESBaseVaccumSwitch_Click(object sender, EventArgs e)
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

        private void NitrogenValveSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsNitrogenValveOpened())
            {
                IOUtilityHelper.Instance.CloseNitrogen();
            }
            else
            {
                IOUtilityHelper.Instance.OpenNitrogen();
            }
        }

        private void TowerRedLightSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsTowerRedLightOn())
            {
                IOUtilityHelper.Instance.TurnoffTowerRedLight();
            }
            else
            {
                IOUtilityHelper.Instance.TurnonTowerRedLight();
            }
        }

        private void YellowLightSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsTowerYellowLightOn())
            {
                IOUtilityHelper.Instance.TurnoffTowerYellowLight();
            }
            else
            {
                IOUtilityHelper.Instance.TurnonTowerYellowLight();
            }
        }

        private void GreenLightSwitch_Click(object sender, EventArgs e)
        {
            if (IOUtilityHelper.Instance.IsTowerGreenLightOn())
            {
                IOUtilityHelper.Instance.TurnoffTowerGreenLight();
            }
            else
            {
                IOUtilityHelper.Instance.TurnonTowerGreenLight();
            }
        }




        private void IOCtrlPanel_Load(object sender, EventArgs e)
        {

        }

        private void FrmAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
