using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BondTerminal
{
    public partial class FrmLightControl : Form
    {
        public FrmLightControl()
        {
            InitializeComponent();
            cmbSelectLight.Items.Clear();
            //cmbVisionPosUsedCamera.Items.Add(EnumCameraType.BondCamera);
            //cmbVisionPosUsedCamera.Items.Add(EnumCameraType.WaferCamera);
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.BondRingField,
        EnumLightSourceType.BondRingField.GetDescription()));
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.BondDirectRedField,
        EnumLightSourceType.BondDirectRedField.GetDescription()));
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.BondDirectGreenField,
        EnumLightSourceType.BondDirectGreenField.GetDescription()));
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.BondDirectBlueField,
        EnumLightSourceType.BondDirectBlueField.GetDescription()));
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.LookupRingField,
        EnumLightSourceType.LookupRingField.GetDescription()));
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.LookupDirectField,
        EnumLightSourceType.LookupDirectField.GetDescription()));
            cmbSelectLight.Items.Add(new KeyValuePair<EnumLightSourceType, string>(
        EnumLightSourceType.WaferDirectField,
        EnumLightSourceType.WaferDirectField.GetDescription()));
            cmbSelectLight.DisplayMember = "Value";
            //foreach (var item in Enum.GetValues(typeof(EnumLightSourceType)))
            //{
            //    cmbSelectLight.Items.Add(item);
            //}
        }

        private void cmbSelectLight_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(cmbSelectLight.Text))
            {
                ctrlLight1.CurrentLightType = (EnumLightSourceType)cmbSelectLight.SelectedValue;
                //ctrlLight1.CurrentLightType = (EnumLightSourceType)Enum.Parse(typeof(EnumLightSourceType), cmbSelectLight.Text);
            }

        }
    }
}
