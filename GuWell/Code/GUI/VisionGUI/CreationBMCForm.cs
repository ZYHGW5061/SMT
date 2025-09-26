using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionGUI
{
    public partial class CreationBMCForm : Form
    {
        public int BondIdentifyBMCSpotNum, BondIdentifyBMCNum, UplookingIdentifyBMCSpotNum, UplookingIdentifyBMCNum, BondIdentifyBMCSubstrateSpotNum, BondIdentifyBMCSubstrateNum;
        string theClickButton = "cancel";

        public CreationBMCForm()
        {
            InitializeComponent();
        }

        public CreationBMCForm(int BondIdentifyBMCSpotNum, int BondIdentifyBMCNum, int UplookingIdentifyBMCSpotNum, int UplookingIdentifyBMCNum, int BondIdentifyBMCSubstrateSpotNum, int BondIdentifyBMCSubstrateNum)
        {
            InitializeComponent();

            if(BondIdentifyBMCSpotNum == 2)
            {
                comboBoxBondIdentifyBMCSpotNum.SelectedIndex = 1;
            }
            else
            {
                comboBoxBondIdentifyBMCSpotNum.SelectedIndex = 0;
            }
            if (BondIdentifyBMCNum == 1)
            {
                comboBoxBondIdentifyBMCNum.SelectedIndex = 1;
            }
            else if(BondIdentifyBMCNum == 2)
            {
                comboBoxBondIdentifyBMCNum.SelectedIndex = 2;
            }
            else
            {
                comboBoxBondIdentifyBMCNum.SelectedIndex = 0;
            }

            if (UplookingIdentifyBMCSpotNum == 2)
            {
                comboBoxUplookingIdentifyBMCSpotNum.SelectedIndex = 1;
            }
            else
            {
                comboBoxUplookingIdentifyBMCSpotNum.SelectedIndex = 0;
            }
            if (UplookingIdentifyBMCNum == 1)
            {
                comboBoxUplookingIdentifyBMCNum.SelectedIndex = 1;
            }
            else if (UplookingIdentifyBMCNum == 2)
            {
                comboBoxUplookingIdentifyBMCNum.SelectedIndex = 2;
            }
            else
            {
                comboBoxUplookingIdentifyBMCNum.SelectedIndex = 0;
            }

            if (BondIdentifyBMCSubstrateSpotNum == 2)
            {
                comboBoxBondIdentifyBMCSubstrateSpotNum.SelectedIndex = 1;
            }
            else
            {
                comboBoxBondIdentifyBMCSubstrateSpotNum.SelectedIndex = 0;
            }
            if (BondIdentifyBMCSubstrateNum == 1)
            {
                comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex = 1;
            }
            else if (BondIdentifyBMCSubstrateNum == 2)
            {
                comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex = 2;
            }
            else
            {
                comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex = 0;
            }
        }

        private void comboBoxBondIdentifyBMCSpotNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            BondIdentifyBMCSpotNum = comboBoxBondIdentifyBMCSpotNum.SelectedIndex - 1;
            comboBoxBondIdentifyBMCSubstrateSpotNum.SelectedIndex = comboBoxBondIdentifyBMCSpotNum.SelectedIndex;
            if (BondIdentifyBMCSpotNum < 0)
            {
                BondIdentifyBMCSpotNum = 0;
            }
        }

        private void comboBoxBondIdentifyBMCNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            BondIdentifyBMCNum = comboBoxBondIdentifyBMCNum.SelectedIndex;
            if (BondIdentifyBMCNum < 0)
            {
                BondIdentifyBMCNum = 0;
            }
        }

        private void comboBoxUplookingIdentifyBMCSpotNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            UplookingIdentifyBMCSpotNum = comboBoxUplookingIdentifyBMCSpotNum.SelectedIndex - 1;
            if (UplookingIdentifyBMCSpotNum < 0)
            {
                UplookingIdentifyBMCSpotNum = 0;
            }
        }

        private void comboBoxUplookingIdentifyBMCNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            UplookingIdentifyBMCNum = comboBoxUplookingIdentifyBMCNum.SelectedIndex;
            if (UplookingIdentifyBMCNum < 0)
            {
                UplookingIdentifyBMCNum = 0;
            }
        }

        private void comboBoxBondIdentifyBMCSubstrateSpotNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            BondIdentifyBMCSubstrateSpotNum = comboBoxBondIdentifyBMCSubstrateSpotNum.SelectedIndex - 1;
            if (BondIdentifyBMCSubstrateSpotNum < 0)
            {
                BondIdentifyBMCSubstrateSpotNum = 0;
            }
        }

        private void comboBoxBondIdentifyBMCSubstrateNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            BondIdentifyBMCSubstrateNum = comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex;
            if (BondIdentifyBMCSubstrateNum < 0)
            {
                BondIdentifyBMCSubstrateNum = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            theClickButton = "confirm";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            theClickButton = "cancel";
            this.Close();
        }


        public string showMessage(int BondIdentifyBMCSpotNum, int BondIdentifyBMCNum, int UplookingIdentifyBMCSpotNum, int UplookingIdentifyBMCNum, int BondIdentifyBMCSubstrateSpotNum, int BondIdentifyBMCSubstrateNum)
        {
            if (BondIdentifyBMCSpotNum == 2)
            {
                comboBoxBondIdentifyBMCSpotNum.SelectedIndex = 1;
            }
            else
            {
                comboBoxBondIdentifyBMCSpotNum.SelectedIndex = 0;
            }
            if (BondIdentifyBMCNum == 1)
            {
                comboBoxBondIdentifyBMCNum.SelectedIndex = 1;
            }
            else if (BondIdentifyBMCNum == 2)
            {
                comboBoxBondIdentifyBMCNum.SelectedIndex = 2;
            }
            else
            {
                comboBoxBondIdentifyBMCNum.SelectedIndex = 0;
            }

            if (UplookingIdentifyBMCSpotNum == 2)
            {
                comboBoxUplookingIdentifyBMCSpotNum.SelectedIndex = 1;
            }
            else
            {
                comboBoxUplookingIdentifyBMCSpotNum.SelectedIndex = 0;
            }
            if (UplookingIdentifyBMCNum == 1)
            {
                comboBoxUplookingIdentifyBMCNum.SelectedIndex = 1;
            }
            else if (UplookingIdentifyBMCNum == 2)
            {
                comboBoxUplookingIdentifyBMCNum.SelectedIndex = 2;
            }
            else
            {
                comboBoxUplookingIdentifyBMCNum.SelectedIndex = 0;
            }

            if (BondIdentifyBMCSubstrateSpotNum == 2)
            {
                comboBoxBondIdentifyBMCSubstrateSpotNum.SelectedIndex = 1;
            }
            else
            {
                comboBoxBondIdentifyBMCSubstrateSpotNum.SelectedIndex = 0;
            }
            if (BondIdentifyBMCSubstrateNum == 1)
            {
                comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex = 1;
            }
            else if (BondIdentifyBMCSubstrateNum == 2)
            {
                comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex = 2;
            }
            else
            {
                comboBoxBondIdentifyBMCSubstrateNum.SelectedIndex = 0;
            }
            this.ShowDialog();
            return theClickButton;
        }
    }
}
