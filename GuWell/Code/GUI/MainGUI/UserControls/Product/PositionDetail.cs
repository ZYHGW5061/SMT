using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainGUI.UserControls.Product
{
    public partial class PositionDetail : UserControl
    {
        public PositionDetail()
        {
            InitializeComponent();
        }
        public float CurrentBondPositionCompensationX
        {
            get
            {
                var ret = 0f;
                float.TryParse(teCompensationX.Text, out ret);
                return ret;
            }
        }

        public float CurrentBondPositionCompensationY
        {
            get
            {
                var ret = 0f;
                float.TryParse(teCompensationY.Text, out ret);
                return ret;
            }
        }

        public float CurrentBondPositionCompensationZ
        {
            get
            {
                var ret = 0f;
                float.TryParse(teCompensationZ.Text, out ret);
                return ret;
            }
        }

        public float CurrentBondPositionCompensationT
        {
            get
            {
                var ret = 0f;
                float.TryParse(teCompensationT.Text, out ret);
                return ret;
            }
        }
        public float CurrentBondPositionWorkHeight
        {
            get
            {
                var ret = 0f;
                float.TryParse(seBondWorkHeight.Text, out ret);
                return ret;
            }
        }
        public float CurrentBondPositionTheta
        {
            get
            {
                var ret = 0f;
                float.TryParse(teBPTheta.Text, out ret);
                return ret;
            }
        }
        public float CurrentDispenserPositionCompensationX
        {
            get
            {
                var ret = 0f;
                float.TryParse(teDispenserPositionCompensationX.Text, out ret);
                return ret;
            }
        }

        public float CurrentDispenserPositionCompensationY
        {
            get
            {
                var ret = 0f;
                float.TryParse(teDispenserPositionCompensationY.Text, out ret);
                return ret;
            }
        }

        public float CurrentDispenserPositionCompensationZ
        {
            get
            {
                var ret = 0f;
                float.TryParse(teDispenserPositionCompensationZ.Text, out ret);
                return ret;
            }
        }

        public float CurrentchipPositionCompensationX
        {
            get
            {
                var ret = 0f;
                float.TryParse(techipPositionCompensationX.Text, out ret);
                return ret;
            }
        }

        public float CurrentchipPositionCompensationY
        {
            get
            {
                var ret = 0f;
                float.TryParse(techipPositionCompensationY.Text, out ret);
                return ret;
            }
        }

        public float CurrentchipPositionCompensationZ
        {
            get
            {
                var ret = 0f;
                float.TryParse(techipPositionCompensationZ.Text, out ret);
                return ret;
            }
        }

        public void fillPositionDetail(BondingPositionSettings pos)
        {
            if (pos == null)
            {
                this.tePosName.Text = null;
                return;
            }

            this.tePosName.Text = pos.Name;

            teCompensationX.Text = pos.BondPositionCompensation.X.ToString();
            teCompensationY.Text = pos.BondPositionCompensation.Y.ToString();
            teCompensationZ.Text = pos.BondPositionCompensation.Z.ToString();
            teCompensationT.Text = pos.BondPositionCompensation.Theta.ToString();

            teBPTheta.Text = pos.BondPositionWithPatternOffset.Theta.ToString();

            seBondWorkHeight.Text = pos.SystemHeight.ToString();

            teDispenserPositionCompensationX.Text = pos.DispenserPositionCompensation.X.ToString();
            teDispenserPositionCompensationY.Text = pos.DispenserPositionCompensation.Y.ToString();
            teDispenserPositionCompensationZ.Text = pos.DispenserPositionCompensation.Z.ToString();

            techipPositionCompensationX.Text = pos.chipPositionCompensation.X.ToString();
            techipPositionCompensationY.Text = pos.chipPositionCompensation.Y.ToString();
            techipPositionCompensationZ.Text = pos.chipPositionCompensation.Z.ToString();

        }
    }
}
