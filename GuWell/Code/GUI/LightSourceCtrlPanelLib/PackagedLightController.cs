using ConfigurationClsLib;
using DevExpress.Utils.Design;
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

namespace LightSourceCtrlPanelLib
{
    public partial class PackagedLightController : UserControl
    {
        private bool _isDirectMultiColor;
        private bool _isRingMultiColor;
        public PackagedLightController(bool isDirectMultiColor = false, bool isRingMultiColor = false)
        {
            InitializeComponent();
            _isDirectMultiColor = isDirectMultiColor;
            _isRingMultiColor = isRingMultiColor;
            if (!DesignTimeTools.IsDesignMode)
            {
                InitialControls();
            }
        }
        private LightSourceCtrlPanelLib.CtrlLight ctrlDirectLight;
        private LightSourceCtrlPanelLib.CtrlLight ctrlRingLight;
        private LightSourceCtrlPanelLib.CtrlRGBLight ctrlDirectRGBLight;
        private LightSourceCtrlPanelLib.CtrlRGBLight ctrlRingRGBLight;
        private void InitialControls()
        {
            if (_isDirectMultiColor)
            {
                ctrlDirectRGBLight = new CtrlRGBLight();
                ctrlDirectRGBLight.ApplyIntensityToHardware = true;
                ctrlDirectRGBLight.Dock = DockStyle.Fill;
                this.tableLayoutPanel1.Controls.Add(ctrlDirectRGBLight, 0, 0);
            }
            else
            {
                ctrlDirectLight = new CtrlLight();
                ctrlDirectLight.ApplyIntensityToHardware = true;
                ctrlDirectLight.Dock = DockStyle.Fill;
                this.tableLayoutPanel1.Controls.Add(ctrlDirectLight, 0, 0);
            }
            if (_isRingMultiColor)
            {
                ctrlRingRGBLight = new CtrlRGBLight();
                ctrlRingRGBLight.ApplyIntensityToHardware = true;
                ctrlRingRGBLight.Dock = DockStyle.Fill;
                this.tableLayoutPanel1.Controls.Add(ctrlRingRGBLight, 0, 1);
            }
            else
            {
                ctrlRingLight = new CtrlLight();
                ctrlRingLight.ApplyIntensityToHardware = true;
                ctrlRingLight.Dock = DockStyle.Fill;
                this.tableLayoutPanel1.Controls.Add(ctrlRingLight, 0, 1);
            }
        }
        public float RingLightBrightness
        {
            get
            {
                //return Convert.ToSingle((light_brightness_control.EditValue ?? "0").ToString());
                //return Convert.ToSingle(labelValue.Text);
                var ret = 0f;
                if (_isRingMultiColor)
                {
                    ret = ctrlRingRGBLight.Brightness;
                }
                else
                {
                    ret = ctrlRingLight.Brightness;
                }
                return ret;
            }
            set
            {
                if (_isRingMultiColor)
                {
                    ctrlRingRGBLight.Brightness = value;
                }
                else
                {
                    ctrlRingLight.Brightness = value;
                }
            }
        }
        public EnumLightColor RingLightColor
        {
            get
            {
                //return Convert.ToSingle((light_brightness_control.EditValue ?? "0").ToString());
                //return Convert.ToSingle(labelValue.Text);
                var ret = EnumLightColor.White;
                if (_isRingMultiColor)
                {
                    ret = ctrlRingRGBLight.CurrentLightColor;
                }

                return ret;
            }
            set
            {
                if (_isRingMultiColor)
                {
                    ctrlRingRGBLight.CurrentLightColor = value;
                }

            }
        }
        public EnumLightSourceType CurrentRingLightType
        {
            get
            {
                var ret = EnumLightSourceType.BondDirectField;
                if (_isRingMultiColor)
                {
                    ret = ctrlRingRGBLight.CurrentLightType;
                }
                else
                {
                    ret = ctrlRingLight.CurrentLightType;
                }
                return ret;
            }
            set
            {
                if (_isRingMultiColor)
                {
                    ctrlRingRGBLight.CurrentLightType = value;
                }
                else
                {
                    ctrlRingLight.CurrentLightType = value;
                }
            }
        }

        public float DirectLightBrightness
        {
            get
            {
                //return Convert.ToSingle((light_brightness_control.EditValue ?? "0").ToString());
                //return Convert.ToSingle(labelValue.Text);
                var ret = 0f;
                if (_isDirectMultiColor)
                {
                    ret = ctrlDirectRGBLight.Brightness;
                }
                else
                {
                    ret = ctrlDirectLight.Brightness;
                }
                return ret;
            }
            set
            {
                if (_isDirectMultiColor)
                {
                    ctrlDirectRGBLight.Brightness = value;
                }
                else
                {
                    ctrlDirectLight.Brightness = value;
                }
            }
        }
        public EnumLightColor DirectLightColor
        {
            get
            {
                //return Convert.ToSingle((light_brightness_control.EditValue ?? "0").ToString());
                //return Convert.ToSingle(labelValue.Text);
                var ret = EnumLightColor.White;
                if (_isDirectMultiColor)
                {
                    ret = ctrlDirectRGBLight.CurrentLightColor;
                }

                return ret;
            }
            set
            {
                if (_isDirectMultiColor)
                {
                    ctrlDirectRGBLight.CurrentLightColor = value;
                }

            }
        }
        public EnumLightSourceType CurrentDirectLightType
        {
            get
            {
                var ret = EnumLightSourceType.BondDirectField;
                if (_isDirectMultiColor)
                {
                    ret = ctrlDirectRGBLight.CurrentLightType;
                }
                else
                {
                    ret = ctrlDirectLight.CurrentLightType;
                }
                return ret;
            }
            set
            {
                if (_isDirectMultiColor)
                {
                    ctrlDirectRGBLight.CurrentLightType = value;
                }
                else
                {
                    ctrlDirectLight.CurrentLightType = value;
                }
            }
        }
    }
}
