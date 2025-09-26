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
using System.Threading;
using DevExpress.Utils.Design;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using GlobalDataDefineClsLib;
using ConfigurationClsLib;
using HardwareManagerClsLib;
using LightControllerClsLib;

namespace LightSourceCtrlPanelLib
{
    public partial class CtrlLight : XtraUserControl
    {
        /// <summary>
        /// 当前暗场光源类型
        /// </summary>
        private EnumLightSourceType _curLightType;
        /// <summary>
        ///  当前暗场光源类型
        /// </summary>
        public EnumLightSourceType CurrentLightType
        {
            get { return _curLightType; }
            set 
            { 
                _curLightType = value;
                if (_curLightType == EnumLightSourceType.WaferRingField)
                {
                    labelLightType.Text="Wafer环光";
                }
                else if (_curLightType == EnumLightSourceType.BondRingField)
                {
                    labelLightType.Text = "Bond环光";
                }
                else if (_curLightType == EnumLightSourceType.LookupRingField)
                {
                    labelLightType.Text = "仰视环光";
                }
                else if (_curLightType == EnumLightSourceType.WaferDirectField)
                {
                    labelLightType.Text = "Wafer直光";
                }
                else if (_curLightType == EnumLightSourceType.BondDirectField)
                {
                    labelLightType.Text = "Bond直光";
                }
                else if (_curLightType == EnumLightSourceType.BondDirectRedField)
                {
                    labelLightType.Text = "Bond直红光";
                }
                else if (_curLightType == EnumLightSourceType.BondDirectGreenField)
                {
                    labelLightType.Text = "Bond直绿光";
                }
                else if (_curLightType == EnumLightSourceType.BondDirectBlueField)
                {
                    labelLightType.Text = "Bond直蓝光";
                }
                else
                {
                    labelLightType.Text = "仰视直光";
                }
                if (ApplyIntensityToHardware)
                {
                    //读取当前亮度
                    ReadCurrentBrightness();
                }
            }
        }
        /// <summary>
        /// 系统日志记录器
        /// </summary>
        private static IBaseLogger _systemDebugLogger
        {
            get { return LoggerManager.GetHandler().GetFileLogger(GlobalParameterSetting.SYSTEM_DEBUG_LOGGER_ID); }
        }

        /// <summary>
        /// 硬件配置
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }

        /// <summary>
        /// 硬件系统
        /// </summary>
        private HardwareManager _hardwareManager
        {
            get { return HardwareManager.Instance; }
        }

        /// <summary>
        /// 明场亮度或滤光片改变时
        /// </summary>
        public Action BrightFieldBrightnessChanged { get; set; }

        /// <summary>
        /// 是否需要将设置的强度应用到硬件
        /// </summary>
        public bool ApplyIntensityToHardware { get; set; }


        /// <summary>
        /// 获取或设置要使用的亮度
        /// </summary>
        public float Brightness
        {
            get
            {
                //return Convert.ToSingle((light_brightness_control.EditValue ?? "0").ToString());
                //return Convert.ToSingle(labelValue.Text);
                var ret = 0f;
                float.TryParse(labelValue.Text, out ret);
                return ret;
            }
            set
            {
                light_brightness_control.EditValue = value;
            }
        }

        /// <summary>
        /// 当前控件关联的明场光源控制器
        /// </summary>
        private ILightSourceController _controller
        {
            get
            {
                if (_curLightType == EnumLightSourceType.WaferRingField)
                {
                    return HardwareManager.Instance.WaferRingLightController;
                }
                else if(_curLightType == EnumLightSourceType.BondRingField)
                {
                    return HardwareManager.Instance.BondRingLightController;
                }
                else if(_curLightType == EnumLightSourceType.LookupRingField)
                {
                    return HardwareManager.Instance.LookupRingLightController ;
                }
                else if (_curLightType == EnumLightSourceType.WaferDirectField)
                {
                    return HardwareManager.Instance.WaferDirectLightController;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectField)
                {
                    return HardwareManager.Instance.BondDirectLightController;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectRedField)
                {
                    return HardwareManager.Instance.BondDirectRedLightController;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectGreenField)
                {
                    return HardwareManager.Instance.BondDirectGreenLightController;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectBlueField)
                {
                    return HardwareManager.Instance.BondDirectBlueLightController;
                }
                else
                {
                    return HardwareManager.Instance.LookupDirectLightController;
                }
            }
        }
        private int _channelNumber
        {
            get
            {
                if (_curLightType == EnumLightSourceType.WaferRingField)
                {
                    return _hardwareConfig.WaferRingLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.BondRingField)
                {
                    return _hardwareConfig.SubstrateRingLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.LookupRingField)
                {
                    return _hardwareConfig.LookupRingLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.WaferDirectField)
                {
                    return _hardwareConfig.WaferDirectLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectField)
                {
                    return _hardwareConfig.SubstrateDirectLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectRedField)
                {
                    return _hardwareConfig.SubstrateDirectRedLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectGreenField)
                {
                    return _hardwareConfig.SubstrateDirectGreenLightConfig.ChannelNumber;
                }
                else if (_curLightType == EnumLightSourceType.BondDirectBlueField)
                {
                    return _hardwareConfig.SubstrateDirectBlueLightConfig.ChannelNumber;
                }
                else
                {
                    return _hardwareConfig.LookupDirectLightConfig.ChannelNumber;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CtrlLight()
        {
            InitializeComponent();
            if (!DesignTimeTools.IsDesignMode)
            {
                InitUIData();
            }
            ReadCurrentBrightness();
        }
        /// <summary>
        /// 初始化UI数据
        /// </summary>
        private void InitUIData()
        {
            if (_curLightType == EnumLightSourceType.WaferRingField)
            {
                light_brightness_control.Properties.Minimum = (int)_hardwareConfig.WaferRingLightConfig.MinIntensity;
                light_brightness_control.Properties.Maximum = (int)_hardwareConfig.WaferRingLightConfig.MaxIntensity;
            }
            else if (_curLightType == EnumLightSourceType.BondRingField)
            {
                light_brightness_control.Properties.Minimum = (int)_hardwareConfig.SubstrateRingLightConfig.MinIntensity;
                light_brightness_control.Properties.Maximum = (int)_hardwareConfig.SubstrateRingLightConfig.MaxIntensity;
            }
            else if(_curLightType == EnumLightSourceType.LookupRingField)
            {
                light_brightness_control.Properties.Minimum = (int)_hardwareConfig.LookupRingLightConfig.MinIntensity;
                light_brightness_control.Properties.Maximum = (int)_hardwareConfig.LookupRingLightConfig.MaxIntensity;
            }
            else if (_curLightType == EnumLightSourceType.WaferDirectField)
            {
                light_brightness_control.Properties.Minimum = (int)_hardwareConfig.WaferDirectLightConfig.MinIntensity;
                light_brightness_control.Properties.Maximum = (int)_hardwareConfig.WaferDirectLightConfig.MaxIntensity;
            }
            else if (_curLightType == EnumLightSourceType.BondDirectField)
            {
                light_brightness_control.Properties.Minimum = (int)_hardwareConfig.SubstrateDirectLightConfig.MinIntensity;
                light_brightness_control.Properties.Maximum = (int)_hardwareConfig.SubstrateDirectLightConfig.MaxIntensity;
            }
            else
            {
                light_brightness_control.Properties.Minimum = (int)_hardwareConfig.LookupDirectLightConfig.MinIntensity;
                light_brightness_control.Properties.Maximum = (int)_hardwareConfig.LookupDirectLightConfig.MaxIntensity;
            }
        }

        /// <summary>
        /// 首次加载
        /// </summary>
        protected override void OnFirstLoad()
        {
            if (DesignTimeTools.IsDesignMode)
            {
                return;
            }
            try
            {
            }
            finally
            {
                base.OnFirstLoad();
            }
        }

        /// <summary>
        /// 设置明场亮度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Apply()
        {
            try
            {
                if (_controller != null)
                {
                    float brightness = light_brightness_control.Value;
                    if (ApplyIntensityToHardware || brightness == 0)
                    {
                        _controller.SetIntensity(brightness, _channelNumber);
                    }
                    //Brightness = brightness;
                    //XtraMessageBox.Show("DarkField Brightness setting successed!", "Info");
                }
            }
            catch (Exception ex)
            {
                _systemDebugLogger.AddErrorContent("", ex);
            }
            finally
            {
                //if (ApplyIntensityToHardware)
                //{
                //    //读取当前亮度
                //    ReadCurrentBrightness();
                //}
            }
        }

        /// <summary>
        /// 读取当前亮度
        /// </summary>
        public void ReadCurrentBrightness()
        {
            try
            {
                if (_controller != null)
                {
                    var voltage = _controller.GetIntensity(_channelNumber);
                    light_brightness_control.EditValue = voltage;
                    labelValue.Text = voltage.ToString();
                }
            }
            catch (Exception ex)
            {
                _systemDebugLogger.AddErrorContent("ReadCurrentBrightness，Error", ex);
            }
        }

        private void light_brightness_control_EditValueChanged(object sender, EventArgs e)
        {
            //labelValue.Text = (light_brightness_control.EditValue ?? "0").ToString();

        }

        private void light_brightness_control_MouseUp(object sender, MouseEventArgs e)
        {
            //btnSet_Click(sender, e);
        }

        private void light_brightness_control_Scroll(object sender, EventArgs e)
        {
            labelValue.Text = (light_brightness_control.EditValue ?? "0").ToString();
            Apply();
        }
    }
}
