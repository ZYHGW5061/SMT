using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.Utils.Design;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using HardwareManagerClsLib;
using IOUtilityClsLib;
using LaserSensorControllerClsLib;
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
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace ControlPanelClsLib
{
    public partial class PPESHeight : UserControl
    {
        public PPESHeight()
        {
            InitializeComponent();
            if (!DesignTimeTools.IsDesignMode)
            {
                InitialCameraControl();
            }
        }
        
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            //CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            //panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
            var cameraView = new CameraWindowGUI();
            cameraView.InitVisualControl();
            cameraView.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(cameraView);
            cameraView.SelectCamera(2);
        }
        /// <summary>
        /// 记录的系统坐标系
        /// </summary>
        public float ESZSystemHeight
        {
            get;
            set;
        }
        /// <summary>
        /// 记录ES高度(Stage坐标系)
        /// </summary>
        public float ESStageHeight
        {
            get;
            set;
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        private ILaserSensorController _laserSensor
        {
            get { return HardwareManager.Instance.LaserSensor; }
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

        private void btnConfirmHeight_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("高度确认", "请确认激光测距仪处于量程范围之内！", "提示") == 1)
            {
                try
                {
                    //读取激光测高仪读数
                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 10000.0;

                    var curLaserMeasureH = DataModel.Instance.LaserValue;
                    var curBondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z;
                    var offsetMeasureZ = curLaserMeasureH - _systemConfig.PositioningConfig.TrackLaserSensorZ;
                    var componentZ = offsetMeasureZ - offsetZ;
                    //此处纪录的是基于吸嘴坐标系的系统坐标
                    ESZSystemHeight = (float)-componentZ;
                    //PPHeight = (float)(_systemConfig.PositioningConfig.TrackChipPPOrigion.Z - offsetMeasureZ + offsetZ);
                    //PPHeight = _positioningSystem.ConvertStagePosToSystemPos(EnumStageAxis.BondZ, PPHeight);
                    ESStageHeight = (float)_positioningSystem.ReadCurrentStagePosition(EnumStageAxis.ESZ);

                    //_positioningSystem.MoveAxisToSystemCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                    WarningBox.FormShow("成功", "位置计算完成！", "提示");
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, "吸嘴工具计算位置异常。", ex);
                    WarningBox.FormShow("异常", "位置计算错误！", "提示");
                } 

            }
        }

        private void btnStartMeasureZ_Click(object sender, EventArgs e)
        {
            //if (WarningBox.FormShow("使用激光测距仪测高？", "激光测距仪将移动到顶针座上方，确认榜头Z轴处于较高的安全位置！", "提示") == 1)
            //{
            //    try
            //    {
                    
            //        var pos = _systemConfig.PositioningConfig.WaferCameraOrigion;
            //        var offset = _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset;
            //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, pos.X + offset.X, EnumCoordSetType.Absolute);
            //        _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, pos.Y + offset.Y, EnumCoordSetType.Absolute);
            //        WarningBox.FormShow("成功。", "激光测距仪已移动到顶针座上方！", "提示");
            //    }
            //    catch (Exception ex)
            //    {
            //        LogRecorder.RecordLog(EnumLogContentType.Error, "激光测距仪移动到顶针座上方失败。", ex);
            //        WarningBox.FormShow("异常！", "激光测距仪移动到顶针座上方失败！", "提示");
            //    }

            //}
        }
    }
}
