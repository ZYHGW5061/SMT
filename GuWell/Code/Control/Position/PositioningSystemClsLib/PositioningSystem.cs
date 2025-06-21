using CameraControllerClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using IOUtilityClsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WestDragon.Framework.UtilityHelper;

namespace PositioningSystemClsLib
{
    /// <summary>
    /// 定位系统，操作Stage硬件实现对指定坐标的定位，读数，坐标转换等.
    /// </summary>
    public class PositioningSystem
    {
        private static volatile PositioningSystem _instance = new PositioningSystem();
        private static readonly object _lockObj = new object();
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static PositioningSystem Instance
        {
            get
            {
                if(_instance==null)
                {
                    lock(_lockObj)
                    {
                        if(_instance==null)
                        {
                            _instance = new PositioningSystem();
                        }
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private StageMotionControl _stageMotionControl
        {
            get { return StageMotionControl.Istance; }
        }
        private CameraConfig _bondCameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.BondCamera); }
        }
        private CameraConfig _uplookingCameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.UplookingCamera); }
        }
        private CameraConfig _waferCameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.WaferCamera); }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        private PositioningSystem()
        {
        }

        /// <summary>
        /// 读取某个轴的系统坐标系位置
        /// </summary>
        /// <returns></returns>
        public double ReadCurrentSystemPosition(EnumStageAxis axis)
        {
            double systemPos = 0d;
            var stagePos=_stageMotionControl.GetCurrentPosition(axis);
            switch (axis)
            {
                case EnumStageAxis.None:
                    break;
                case EnumStageAxis.BondX:
                    systemPos = _systemConfig.PositioningConfig.BondOrigion.X - stagePos;
                    break;
                case EnumStageAxis.BondY:
                    systemPos = stagePos - _systemConfig.PositioningConfig.BondOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    systemPos = stagePos - _systemConfig.PositioningConfig.BondOrigion.Z;
                    break;
                case EnumStageAxis.WaferTableX:
                    systemPos = stagePos - _systemConfig.PositioningConfig.WaferOrigion.X;
                    break;
                case EnumStageAxis.WaferTableY:
                    systemPos = stagePos - _systemConfig.PositioningConfig.WaferOrigion.Y;
                    break;
                case EnumStageAxis.WaferTableZ:
                    systemPos = stagePos - _systemConfig.PositioningConfig.WaferOrigion.Z;
                    break;
                case EnumStageAxis.ChipPPT:
                    break;
                case EnumStageAxis.ChipPPZ:
                    //这里吸嘴的系统坐标系位置应该是以指定吸嘴在Mark点时的高度为0点的高度TBD
                    break;
                case EnumStageAxis.SubmountPPT:
                    break;
                case EnumStageAxis.SubmountPPZ:
                    break;
                case EnumStageAxis.ESZ:
                    break;
                case EnumStageAxis.NeedleZ:
                    break;
                default:
                    break;
            }
            return systemPos;
        }
        /// <summary>
        /// 读取芯片吸嘴的系统坐标系位置（系统原点依据于吸嘴工具的校准）
        /// </summary>
        /// <param name="ppName"></param>
        /// <returns></returns>
        public float ReadChipPPSystemPosition(string ppName)
        {
            float systemPos = 0f;
            var stagePos = _stageMotionControl.GetCurrentPosition(EnumStageAxis.BondZ);
            var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == ppName);
            if (pptool != null)
            {
                systemPos = (float)(stagePos - pptool.AltimetryOnMark);
            }
            else
            {
                systemPos = (float)(stagePos - _systemConfig.PositioningConfig.TrackChipPPOrigion.Z);
            }
            return systemPos;
        }
        /// <summary>
        /// 芯片吸嘴移动到系统坐标系位置（系统原点依据于吸嘴工具的校准）
        /// </summary>
        /// <param name="ppName"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public StageMotionResult MoveChipPPToSystemCoord(string ppName,double target, EnumCoordSetType type)
        {
            var ret = StageMotionResult.Success;
            float stagePos = 0f;
            var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == ppName);
            if (pptool != null)
            {
                stagePos = (float)(target + pptool.AltimetryOnMark);
            }
            else
            {
                stagePos = (float)(target + _systemConfig.PositioningConfig.TrackChipPPOrigion.Z);
            }
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, stagePos);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(EnumStageAxis.BondZ, target);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"MoveAixsToStageCoord,Axis:BondZ,Run-Exception.");
            }
            return ret;
        }
        /// <summary>
        /// 读取所有轴的Stage位置
        /// </summary>
        /// <returns></returns>
        public double[] ReadCurrentStagePosition()
        {
            return _stageMotionControl.GetCurrentPosition();
        }
        /// <summary>
        /// 读取指定轴的Stage位置
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double ReadCurrentStagePosition(EnumStageAxis axis)
        {
            return _stageMotionControl.GetCurrentPosition(axis);
        }
        /// <summary>
        /// Jog+
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        public void JogPositive(EnumStageAxis axis,float speed)
        {
            _stageMotionControl.JogPositive(axis, speed);
        }
        public void StopJogPositive(EnumStageAxis axis)
        {
            _stageMotionControl.StopJogPositive(axis);
        }
        /// <summary>
        /// Jog-
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        public void JogNegative(EnumStageAxis axis, float speed)
        {
            _stageMotionControl.JogNegative(axis, speed);
        }

        public void StopJogNegative(EnumStageAxis axis)
        {
            _stageMotionControl.StopJogNegative(axis);
        }      

        /// <summary>
        /// 移动指定轴到指定Stage坐标系位置
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="target"></param>
        /// <param name="type">绝对移动或者相对移动</param>
        public StageMotionResult MoveAixsToStageCoord(EnumStageAxis axis,double target, EnumCoordSetType type)
        {
            var ret = StageMotionResult.Success;
            //var targetPos = new MillimeterUnitValue<double>() { Value = target };
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(axis, target);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(axis, target);
            }
            if(ret==StageMotionResult.Fail)
            {
                throw new Exception($"MoveAixsToStageCoord,Axis:{axis},Run-Exception.");
            }
            return ret;
        }
        public StageMotionResult MoveAixsToStageCoord(EnumStageAxis[] axis, double[] target, EnumCoordSetType type)
        {
            var ret = StageMotionResult.Success;
            //var targetPos = new MillimeterUnitValue<double>() { Value = target };
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(axis, target);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(axis, target);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"MoveAixsToStageCoord,Axis:{axis},Run-Exception.");
            }
            return ret;
        }
        public StageMotionResult MoveAxisToSystemCoord(EnumStageAxis axis, double target, EnumCoordSetType type)
        {
            double stageAbsoluteTarget = target;
            double stageRelativeTarget = target;
            switch (axis)
            {
                case EnumStageAxis.None:
                    break;
                case EnumStageAxis.BondX:
                    stageAbsoluteTarget = _systemConfig.PositioningConfig.BondOrigion.X - target;
                    stageRelativeTarget = -target;
                    break;
                case EnumStageAxis.BondY:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.BondOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.BondOrigion.Z;
                    break;
                case EnumStageAxis.WaferTableX:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.WaferOrigion.X;
                    break;
                case EnumStageAxis.WaferTableY:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.WaferOrigion.Y;
                    break;
                case EnumStageAxis.WaferTableZ:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.WaferOrigion.Z;
                    break;
                case EnumStageAxis.ChipPPT:
                    break;
                case EnumStageAxis.ChipPPZ:
                    break;
                case EnumStageAxis.SubmountPPT:
                    break;
                case EnumStageAxis.SubmountPPZ:
                    break;
                case EnumStageAxis.ESZ:
                    break;
                case EnumStageAxis.NeedleZ:
                    break;
                default:
                    break;
            }
            var ret = StageMotionResult.Success;
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(axis, stageAbsoluteTarget);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(axis, stageRelativeTarget);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"MoveAixsToStageCoord,Axis:{axis},Run-Exception.");
            }
            return ret;
        }

        public StageMotionResult MoveAxisToSystemCoord(EnumStageAxis[] axis, double[] target, EnumCoordSetType type)
        {
            double[] stageAbsoluteTarget = new double[axis.Length];
            double[] stageRelativeTarget = new double[axis.Length];
            for (int i = 0; i < axis.Length; i++)
            {
                switch (axis[i])
                {
                    case EnumStageAxis.None:
                        break;
                    case EnumStageAxis.BondX:
                        stageAbsoluteTarget[i] = _systemConfig.PositioningConfig.BondOrigion.X - target[i];
                        stageRelativeTarget[i]= - target[i];
                        break;
                    case EnumStageAxis.BondY:
                        stageAbsoluteTarget[i] = target[i] + _systemConfig.PositioningConfig.BondOrigion.Y;
                        stageRelativeTarget[i] = target[i];
                        break;
                    case EnumStageAxis.BondZ:
                        stageAbsoluteTarget[i] = target[i] + _systemConfig.PositioningConfig.BondOrigion.Z;
                        stageRelativeTarget[i] = target[i];
                        break;
                    case EnumStageAxis.WaferTableX:
                        stageAbsoluteTarget[i] = target[i] + _systemConfig.PositioningConfig.WaferOrigion.X;
                        stageRelativeTarget[i] = target[i];
                        break;
                    case EnumStageAxis.WaferTableY:
                        stageAbsoluteTarget[i] = target[i] + _systemConfig.PositioningConfig.WaferOrigion.Y;
                        stageRelativeTarget[i] = target[i];
                        break;
                    case EnumStageAxis.WaferTableZ:
                        stageAbsoluteTarget[i] = target[i] + _systemConfig.PositioningConfig.WaferOrigion.Z;
                        stageRelativeTarget[i] = target[i];
                        break;
                    case EnumStageAxis.ChipPPT:
                        break;
                    case EnumStageAxis.ChipPPZ:
                        break;
                    case EnumStageAxis.SubmountPPT:
                        break;
                    case EnumStageAxis.SubmountPPZ:
                        break;
                    case EnumStageAxis.ESZ:
                        break;
                    case EnumStageAxis.NeedleZ:
                        break;
                    default:
                        break;
                }
            }


            var ret = StageMotionResult.Success;
            //var targetPos = new MillimeterUnitValue<double>() { Value = target };
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(axis, stageAbsoluteTarget);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(axis, stageRelativeTarget);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"MoveAixsToStageCoord,Axis:{axis},Run-Exception.");
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="target"></param>
        /// <param name="type">绝对移动或者相对移动</param>
        public void MoveBondCameraToSystemCoord(EnumStageAxis axis, double target, EnumCoordSetType type)
        {
            double stageAbsoluteTarget = 0d;
            double stageRelativeTarget = target;
            switch (axis)
            {
                case EnumStageAxis.BondX:
                    stageAbsoluteTarget = _systemConfig.PositioningConfig.BondOrigion.X - target;
                    stageRelativeTarget = -target;
                    break;
                case EnumStageAxis.BondY:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.BondOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    stageAbsoluteTarget = target + _systemConfig.PositioningConfig.BondOrigion.Z;
                    break;
                default:
                    break;
            }

            if (type == EnumCoordSetType.Absolute)
            {
                _stageMotionControl.AbsoluteMovingSync(axis, stageAbsoluteTarget);
            }
            else
            {
                _stageMotionControl.RelativeMovingSync(axis, stageRelativeTarget);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public void MoveChipPPToSystemCoord(EnumStageAxis axis, double target, EnumCoordSetType type)
        {
            double stageTarget = 0d;
            switch (axis)
            {
                case EnumStageAxis.BondX:
                    stageTarget = target + _systemConfig.PositioningConfig.ChipPPOrigion.X;
                    break;
                case EnumStageAxis.BondY:
                    stageTarget = target + _systemConfig.PositioningConfig.ChipPPOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    stageTarget = target + _systemConfig.PositioningConfig.ChipPPOrigion.Z;
                    break;
                case EnumStageAxis.ChipPPT:
                    stageTarget = target + _systemConfig.PositioningConfig.ChipPPOrigion.Theta;
                    break;
                default:
                    break;
            }

            if (type == EnumCoordSetType.Absolute)
            {
                _stageMotionControl.AbsoluteMovingSync(axis, stageTarget);
            }
            else
            {
                _stageMotionControl.RelativeMovingSync(axis, target);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public void MoveSubmountPPToSystemCoord(EnumStageAxis axis, double target, EnumCoordSetType type)
        {
            double stageTarget = 0d;
            switch (axis)
            {
                case EnumStageAxis.BondX:
                    stageTarget = target + _systemConfig.PositioningConfig.SubmountPPOrigion.X;
                    break;
                case EnumStageAxis.BondY:
                    stageTarget = target + _systemConfig.PositioningConfig.SubmountPPOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    stageTarget = target + _systemConfig.PositioningConfig.SubmountPPOrigion.Z;
                    break;
                case EnumStageAxis.ChipPPT:
                    stageTarget = target + _systemConfig.PositioningConfig.SubmountPPOrigion.Theta;
                    break;
                default:
                    break;
            }

            if (type == EnumCoordSetType.Absolute)
            {
                _stageMotionControl.AbsoluteMovingSync(axis, stageTarget);
            }
            else
            {
                _stageMotionControl.RelativeMovingSync(axis, target);
            }
        }
        public void MoveWaferTableToSystemCoord(EnumStageAxis axis, double target, EnumCoordSetType type)
        {
            double stageTarget = 0d;
            switch (axis)
            {
                case EnumStageAxis.WaferTableX:
                    stageTarget = target + _systemConfig.PositioningConfig.WaferOrigion.X;
                    break;
                case EnumStageAxis.WaferTableY:
                    stageTarget = target + _systemConfig.PositioningConfig.WaferOrigion.Y;
                    break;
                case EnumStageAxis.WaferTableZ:
                    stageTarget = target + _systemConfig.PositioningConfig.WaferOrigion.Z;
                    break;
                default:
                    break;
            }

            if (type == EnumCoordSetType.Absolute)
            {
                _stageMotionControl.AbsoluteMovingSync(axis, stageTarget);
            }
            else
            {
                _stageMotionControl.RelativeMovingSync(axis, target);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aixs"></param>
        /// <param name="target"></param>
        /// <param name="type">绝对移动或者相对移动</param>
        public void MoveBondCameraToWaferSystemCoord(EnumStageAxis axis, double target, EnumCoordSetType type)
        {
            double stageTarget = 0d;
            switch (axis)
            {
                case EnumStageAxis.BondX:
                    stageTarget = target + _systemConfig.PositioningConfig.WaferOrigion.X + _systemConfig.PositioningConfig.WaferCameraOrigion.X;
                    break;
                case EnumStageAxis.BondY:
                    stageTarget = target + _systemConfig.PositioningConfig.WaferOrigion.Y + _systemConfig.PositioningConfig.WaferCameraOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    stageTarget = target + _systemConfig.PositioningConfig.WaferOrigion.Z + _systemConfig.PositioningConfig.WaferCameraOrigion.Z;
                    break;
                default:
                    break;
            }

            if (type == EnumCoordSetType.Absolute)
            {
                _stageMotionControl.AbsoluteMovingSync(axis, stageTarget);
            }
            else
            {
                _stageMotionControl.RelativeMovingSync(axis, target);
            }
        }
        /// <summary>
        /// 将像素坐标转换成以视野中心为原点的MM坐标(正方向向右向下)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public (float, float) ConvertPixelCoorToMMCenterCoor(PointF point, EnumCameraType camera)
        {
            float X = 0f;

            float Y = 0f;
            switch (camera)
            {
                case EnumCameraType.None:
                    break;
                case EnumCameraType.BondCamera:
                    X = (float)((point.X - _bondCameraConfig.ImageSizeWidth / 2) * _bondCameraConfig.WidthPixelSize);
                    Y = (float)((point.Y - _bondCameraConfig.ImageSizeHeight / 2) * _bondCameraConfig.HeightPixelSize);
                    break;
                case EnumCameraType.UplookingCamera:
                    X = (float)((point.X - _uplookingCameraConfig.ImageSizeWidth / 2) * _uplookingCameraConfig.WidthPixelSize);
                    Y = (float)((point.Y - _uplookingCameraConfig.ImageSizeHeight / 2) * _uplookingCameraConfig.HeightPixelSize);
                    break;
                case EnumCameraType.WaferCamera:
                    X = (float)((point.X - _waferCameraConfig.ImageSizeWidth / 2) * _waferCameraConfig.WidthPixelSize);
                    Y = (float)((point.Y - _waferCameraConfig.ImageSizeHeight / 2) * _waferCameraConfig.HeightPixelSize);
                    break;
                default:
                    break;
            }
            return (X, Y);

        }
        /// <summary>
        /// 将视觉识别出的特征pixel位置转换为其距离视觉中心的MM偏移
        /// </summary>
        /// <param name="pixelPos"></param>
        /// <param name="axisIndex">1:X,2:Y</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public float ConvertPixelPosToMMCenterPos(float pixelPos,int axisIndex, EnumCameraType camera)
        {
            var ret = 0f;
            var pixelDistance = 0f;

            switch (camera)
            {
                case EnumCameraType.None:
                    break;
                case EnumCameraType.BondCamera:
                    if (axisIndex == 1)
                    {
                        pixelDistance=pixelPos - _bondCameraConfig.ImageSizeWidth / 2;
                        ret = (float)(pixelDistance * _bondCameraConfig.WidthPixelSize);
                    }
                    else if (axisIndex == 2)
                    {
                        pixelDistance = pixelPos - _bondCameraConfig.ImageSizeHeight / 2;
                        ret = (float)(pixelDistance * _bondCameraConfig.HeightPixelSize);
                    }
                    break;
                case EnumCameraType.UplookingCamera:
                    if (axisIndex == 1)
                    {
                        pixelDistance = pixelPos - _uplookingCameraConfig.ImageSizeWidth / 2;
                        ret = (float)(pixelDistance * _uplookingCameraConfig.WidthPixelSize);
                    }
                    else if (axisIndex == 2)
                    {
                        pixelDistance = pixelPos - _uplookingCameraConfig.ImageSizeHeight / 2;
                        ret = (float)(pixelDistance * _uplookingCameraConfig.HeightPixelSize);
                    }
                    break;
                case EnumCameraType.WaferCamera:
                    if (axisIndex == 1)
                    {
                        pixelDistance = pixelPos - _waferCameraConfig.ImageSizeWidth / 2;
                        ret = (float)(pixelDistance * _waferCameraConfig.WidthPixelSize);
                    }
                    else if (axisIndex == 2)
                    {
                        pixelDistance = pixelPos - _waferCameraConfig.ImageSizeHeight / 2;
                        ret = (float)(pixelDistance * _waferCameraConfig.HeightPixelSize);
                    }
                    break;
                default:
                    break;
            }

            return ret;
        }
        public float ConvertVisionPixelValueToMMValue(float pixelDistance, EnumCameraType camera, int axisIndex)
        {
            var ret = 0f;
            switch (camera)
            {
                case EnumCameraType.None:
                    break;
                case EnumCameraType.BondCamera:
                    if (axisIndex == 1)
                    {
                        ret = (float)(pixelDistance * _bondCameraConfig.WidthPixelSize);
                    }
                    else if (axisIndex == 2)
                    {
                        ret = (float)(pixelDistance * _bondCameraConfig.HeightPixelSize);
                    }
                    break;
                case EnumCameraType.UplookingCamera:
                    if (axisIndex == 1)
                    {
                        ret = (float)(pixelDistance * _uplookingCameraConfig.WidthPixelSize);
                    }
                    else if (axisIndex == 2)
                    {
                        ret = (float)(pixelDistance * _uplookingCameraConfig.HeightPixelSize);
                    }
                    break;
                case EnumCameraType.WaferCamera:
                    if (axisIndex == 1)
                    {
                        ret = (float)(pixelDistance * _waferCameraConfig.WidthPixelSize);
                    }
                    else if (axisIndex == 2)
                    {
                        ret = (float)(pixelDistance * _waferCameraConfig.HeightPixelSize);
                    }
                    break;
                default:
                    break;
            }

            return ret;

        }
        public XYZTCoordinate ConvertBondCameraStageCoordToSystemCoord(XYZTCoordinate stageCoord)
        {
            XYZTCoordinate systemCoord = new XYZTCoordinate();
            systemCoord.X = stageCoord.X - _systemConfig.PositioningConfig.BondOrigion.X;
            systemCoord.Y = stageCoord.Y - _systemConfig.PositioningConfig.BondOrigion.Y;
            systemCoord.Z = stageCoord.Z - _systemConfig.PositioningConfig.BondOrigion.Z;
            return systemCoord;
        }
        public XYZTCoordinate ConvertChipPPStageCoordToSystemCoord(XYZTCoordinate stageCoord)
        {
            XYZTCoordinate systemCoord = new XYZTCoordinate();
            systemCoord.X = stageCoord.X - _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
            systemCoord.Y = stageCoord.Y - _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
            systemCoord.Z = stageCoord.Z - _systemConfig.PositioningConfig.LookupChipPPOrigion.Z;
            return systemCoord;
        }

        public XYZTCoordinate ConvertSubmountPPStageCoordToSystemCoord(XYZTCoordinate stageCoord)
        {
            XYZTCoordinate systemCoord = new XYZTCoordinate();
            systemCoord.X = stageCoord.X - _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X;
            systemCoord.Y = stageCoord.Y - _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y;
            systemCoord.Z = stageCoord.Z - _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z;
            return systemCoord;
        }
        public XYZTCoordinate ConvertWaferTableStageCoordToSystemCoord(XYZTCoordinate stageCoord)
        {
            XYZTCoordinate systemCoord = new XYZTCoordinate();
            systemCoord.X = stageCoord.X - _systemConfig.PositioningConfig.WaferOrigion.X;
            systemCoord.Y = stageCoord.Y - _systemConfig.PositioningConfig.WaferOrigion.Y;
            systemCoord.Z = stageCoord.Z - _systemConfig.PositioningConfig.WaferOrigion.Z;
            return systemCoord;
        }

        public XYZTCoordinate ConvertBondCameraSystemCoordToStageCoord(XYZTCoordinate systemCoord)
        {
            XYZTCoordinate stageCoord = new XYZTCoordinate();
            stageCoord.X = _systemConfig.PositioningConfig.BondOrigion.X - systemCoord.X;
            stageCoord.Y = systemCoord.Y + _systemConfig.PositioningConfig.BondOrigion.Y;
            stageCoord.Z = systemCoord.Z + _systemConfig.PositioningConfig.BondOrigion.Z;
            return stageCoord;
        }
        public XYZTCoordinate ConvertChipPPSystemCoordToStageCoord(XYZTCoordinate systemCoord)
        {
            XYZTCoordinate stageCoord = new XYZTCoordinate();
            stageCoord.X = systemCoord.X + _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
            stageCoord.Y = systemCoord.Y + _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
            stageCoord.Z = systemCoord.Z + _systemConfig.PositioningConfig.LookupChipPPOrigion.Z;
            return stageCoord;
        }

        public XYZTCoordinate ConvertSubmountPPSystemCoordToStageCoord(XYZTCoordinate systemCoord)
        {
            XYZTCoordinate stageCoord = new XYZTCoordinate();
            stageCoord.X = systemCoord.X + _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X;
            stageCoord.Y = systemCoord.Y + _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y;
            stageCoord.Z = systemCoord.Z + _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z;
            return stageCoord;
        }
        public XYZTCoordinate ConvertWaferTableSystemCoordToStageCoord(XYZTCoordinate systemCoord)
        {
            XYZTCoordinate stageCoord = new XYZTCoordinate();
            stageCoord.X = systemCoord.X + _systemConfig.PositioningConfig.WaferOrigion.X;
            stageCoord.Y = systemCoord.Y + _systemConfig.PositioningConfig.WaferOrigion.Y;
            stageCoord.Z = systemCoord.Z + _systemConfig.PositioningConfig.WaferOrigion.Z;
            return stageCoord;
        }

        public XYZTCoordinate ConvertSystemCoordToStageCoord(ACoordinate systemCoord, EnumStageSystem stageSystem, EnumCoordSetType type)
        {
            XYZTCoordinate stageCoord = new XYZTCoordinate();
            ACoordinate XYZOrigin = GetStageSystemOrigion(stageSystem);
            stageCoord.X = -systemCoord.X + (type == EnumCoordSetType.Absolute ? XYZOrigin.X : 0);
            stageCoord.Y = -systemCoord.Y + (type == EnumCoordSetType.Absolute ? XYZOrigin.Y : 0);
            stageCoord.Theta = systemCoord.Theta;
            stageCoord.Z = systemCoord.Z + (type == EnumCoordSetType.Absolute ? XYZOrigin.Z : 0);
            return stageCoord;
        }
        public float ConvertStagePosToSystemPos(EnumStageAxis axis, float stagePos)
        {
            float systemPos = 0f;
            switch (axis)
            {
                case EnumStageAxis.None:
                    break;
                case EnumStageAxis.BondX:
                    systemPos = stagePos - (float)_systemConfig.PositioningConfig.BondOrigion.X;
                    break;
                case EnumStageAxis.BondY:
                    systemPos = stagePos - (float)_systemConfig.PositioningConfig.BondOrigion.Y;
                    break;
                case EnumStageAxis.BondZ:
                    systemPos = stagePos - (float)_systemConfig.PositioningConfig.BondOrigion.Z;
                    break;
                case EnumStageAxis.WaferTableX:
                    systemPos = stagePos - (float)_systemConfig.PositioningConfig.WaferOrigion.X;
                    break;
                case EnumStageAxis.WaferTableY:
                    systemPos = stagePos - (float)_systemConfig.PositioningConfig.WaferOrigion.Y;
                    break;
                case EnumStageAxis.WaferTableZ:
                    systemPos = stagePos - (float)_systemConfig.PositioningConfig.WaferOrigion.Z;
                    break;
                case EnumStageAxis.ChipPPT:
                    break;
                case EnumStageAxis.ChipPPZ:
                    break;
                case EnumStageAxis.SubmountPPT:
                    break;
                case EnumStageAxis.SubmountPPZ:
                    break;
                case EnumStageAxis.ESZ:
                    break;
                case EnumStageAxis.NeedleZ:
                    break;
                default:
                    break;
            }
            return systemPos;
        }
        public float GetAxisSpeed(EnumStageAxis axis)
        {
            var ret = 0f;
            ret =(float)_stageMotionControl.GetAxisSpeed(axis);
            return ret;
        }
        public bool SetAxisSpeed(EnumStageAxis axis,float speed)
        {
            return _stageMotionControl.SetAxisSpeed(axis, speed);
        }
        public void SetAxisSpeedForProgram()
        {
            _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondX, 50f);
            _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondY, 50f);
            _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondZ, 30f);
            if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
            {
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.SubmountPPZ, 30f);
            }
        }
        public void SetAxisSpeedForProduct()
        {
            AxisConfig axisConfig1 = HardwareConfiguration.Instance.StageConfig.GetAixsConfigByType(EnumStageAxis.BondX);
            AxisConfig axisConfig2 = HardwareConfiguration.Instance.StageConfig.GetAixsConfigByType(EnumStageAxis.BondY);
            AxisConfig axisConfig3 = HardwareConfiguration.Instance.StageConfig.GetAixsConfigByType(EnumStageAxis.BondZ);
            AxisConfig axisConfig4 = HardwareConfiguration.Instance.StageConfig.GetAixsConfigByType(EnumStageAxis.SubmountPPZ);
            if (_systemConfig.JobConfig.IsSlowSpeedRun)
            {
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondX, 40);
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondY, 40);
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondZ, 20);
                if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                {
                    _stageMotionControl.SetAxisSpeed(EnumStageAxis.SubmountPPZ, 20);
                }
            }
            else
            {
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondX, axisConfig1.AxisSpeed);
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondY, axisConfig2.AxisSpeed);
                _stageMotionControl.SetAxisSpeed(EnumStageAxis.BondZ, axisConfig3.AxisSpeed);
                if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                {
                    _stageMotionControl.SetAxisSpeed(EnumStageAxis.SubmountPPZ, axisConfig4.AxisSpeed);
                }
            }
        }
        public XYZTCoordinate GetStageSystemOrigion(EnumStageSystem stageSystem)
        {
            XYZTCoordinate retCoord = new XYZTCoordinate();
            switch (stageSystem)
            {
                case EnumStageSystem.BondTable:
                    retCoord.X = _systemConfig.PositioningConfig.BondOrigion.X;
                    retCoord.Y = _systemConfig.PositioningConfig.BondOrigion.Y;
                    retCoord.Z = _systemConfig.PositioningConfig.BondOrigion.Z;
                    retCoord.Theta = _systemConfig.PositioningConfig.BondOrigion.Theta;
                    break;
                case EnumStageSystem.WaferTable:
                    retCoord.X = _systemConfig.PositioningConfig.WaferOrigion.X;
                    retCoord.Y = _systemConfig.PositioningConfig.WaferOrigion.Y;
                    retCoord.Z = _systemConfig.PositioningConfig.WaferOrigion.Z;
                    retCoord.Theta = _systemConfig.PositioningConfig.WaferOrigion.Theta;
                    break;
                case EnumStageSystem.ChipPP:
                    break;
                case EnumStageSystem.ES:
                    break;
                default:
                    break;
            }
            return retCoord;
        }

        /// <summary>
        /// 读取轴状态
        /// 1 报警
        /// 5 正限位
        /// 6 负限位 
        /// 7 平滑停止 
        /// 8 急停 
        /// 9 使能 
        /// 10 规划运动 
        /// 11 电机到位
        /// </summary>
        public int GetAxisState(EnumStageAxis axis)
        {
            return _stageMotionControl.GetAxisState(axis);
        }

        /// <summary>
        /// 报警清除
        /// </summary>
        public void ClrAlarm(EnumStageAxis axis)
        {
            _stageMotionControl.ClrAlarm(axis);
        }

        /// <summary>
        /// 报警 / 限位无效
        /// action动作 ： 1 为生效，0为失效
        /// </summary>
        public void DisableAlarmLimit(EnumStageAxis axis)
        {
            _stageMotionControl.DisableAlarmLimit(axis);
        }
        /// <summary>
        /// 芯片吸嘴移动到榜头相机中心（只移XY）
        /// </summary>
        public void ChipPPMovetoBondCameraCenter()
        {
            var offset=_systemConfig.PositioningConfig.PP1AndBondCameraOffset;
            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;

            double[] target1 = new double[2];
            target1[0] = offset.X;
            target1[1] = offset.Y;
            _stageMotionControl.RelativeMovingSync(multiAxis, target1);
            //_stageMotionControl.RelativeMovingSync(EnumStageAxis.BondX, offset.X);
            //_stageMotionControl.RelativeMovingSync(EnumStageAxis.BondY, offset.Y);
        }
        /// <summary>
        /// 衬底吸嘴移动到榜头相机中心（只移XY）
        /// </summary>
        public void SubmountPPMovetoBondCameraCenter()
        {
            var offset = _systemConfig.PositioningConfig.PP2AndBondCameraOffset;
            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;

            double[] target1 = new double[2];
            target1[0] = offset.X;
            target1[1] = offset.Y;
            _stageMotionControl.RelativeMovingSync(multiAxis, target1);
            //_stageMotionControl.RelativeMovingSync(EnumStageAxis.BondX, offset.X);
            //_stageMotionControl.RelativeMovingSync(EnumStageAxis.BondY, offset.Y);
        }

        /// <summary>
        /// 芯片吸嘴移动到仰视相机中心（只移XY）
        /// </summary>
        public bool ChipPPMovetoUplookingCameraCenter()
        {
            double[] Target = new double[2];
            Target[0] = _systemConfig.PositioningConfig.LookupChipPPOrigion.X;
            Target[1] = _systemConfig.PositioningConfig.LookupChipPPOrigion.Y;
            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;
            var ret = false;
            if (_stageMotionControl.AbsoluteMovingSync(multiAxis, Target) == StageMotionResult.Success)
            {
                ret = true;
            }
            return ret;
            //var ret = false;
            //if(_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondX, _systemConfig.PositioningConfig.LookupChipPPOrigion.X)==StageMotionResult.Success
            //&&_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondY, _systemConfig.PositioningConfig.LookupChipPPOrigion.Y) == StageMotionResult.Success)
            //{
            //    ret = true;
            //}
            //return ret;
        }

        /// <summary>
        /// 芯片吸嘴移动到仰视相机中心（只移XY）
        /// </summary>
        public bool ChipPPMovetoCalibrationTableCenter(PPToolSettings ppTool=null)
        {
            double[] Target = new double[2];
            Target[0] = _systemConfig.PositioningConfig.CalibrationTableOrigion.X;
            Target[1] = _systemConfig.PositioningConfig.CalibrationTableOrigion.Y;
            if(ppTool!=null)
            {
                Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X-ppTool.ChipPPPosCompensateCoordinate1.X);
                Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y-ppTool.ChipPPPosCompensateCoordinate1.Y);
            }
            else
            {
                Target[0] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.X- _systemConfig.PositioningConfig.LookupChipPPOrigion.X);
                Target[1] -= (_systemConfig.PositioningConfig.LookupCameraOrigion.Y- _systemConfig.PositioningConfig.LookupChipPPOrigion.Y);
            }
            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;
            var ret = false;
            if (_stageMotionControl.AbsoluteMovingSync(multiAxis, Target) == StageMotionResult.Success)
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 榜头相机移动到校准台上方
        /// </summary>
        public bool BondCameraMovetoCalibrationTableCenter()
        {
            double[] Target = new double[2];
            Target[0] = _systemConfig.PositioningConfig.CalibrationTableOrigion.X;
            Target[1] = _systemConfig.PositioningConfig.CalibrationTableOrigion.Y;
            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;
            var ret = false;
            if (_stageMotionControl.AbsoluteMovingSync(multiAxis, Target) == StageMotionResult.Success)
            {
                ret = true;
            }
            return ret;
        }
        /// <summary>
        /// 衬底吸嘴移动到仰视相机中心（只移XY）
        /// </summary>
        public void SubmountPPMovetoUplookingCameraCenter()
        {
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondX, _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X);
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondY, _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y);
        }

        /// <summary>
        /// 芯片吸嘴移动到wafer相机中心（只移XY）
        /// </summary>
        public void ChipPPMovetoWaferCameraCenter()
        {
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondX, _systemConfig.PositioningConfig.WaferChipPPOrigion.X);
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondY, _systemConfig.PositioningConfig.WaferChipPPOrigion.Y);
        }

        /// <summary>
        /// 衬底吸嘴移动到wafer相机中心（只移XY）
        /// </summary>
        public void SubmountPPMovetoWaferCameraCenter()
        {
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondX, _systemConfig.PositioningConfig.WaferSubmountPPOrigion.X);
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondY, _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y);
        }
        /// <summary>
        /// 衬底吸嘴移动到共晶台上料中心
        /// </summary>
        public void SubmountPPMovetoEutecticCenter()
        {
            var offset = _systemConfig.PositioningConfig.PP2AndBondCameraOffset;
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondX, offset.X);
            _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondY, offset.Y);
        }
        public bool StageMovetoPrepareLocationForJob()
        {
            var ret = false;
            var safePos = _systemConfig.PositioningConfig.BondSafeLocation;
            IOUtilityHelper.Instance.UpDispenserCylinder();
            if (_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, safePos.Z) == StageMotionResult.Success
                && _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.ESZ, 0) == StageMotionResult.Success)
            {
                double[] Target = new double[2];
                Target[0] = safePos.X;
                Target[1] = safePos.Y;
                EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;
                if (_stageMotionControl.AbsoluteMovingSync(multiAxis, Target) == StageMotionResult.Success)
                {
                    ret = true;
                }
            }
            return ret;

        }
        /// <summary>
        /// 榜头移动到空闲位置
        /// </summary>
        public bool BondMovetoSafeLocation()
        {
            //var ret = false;
            //var offset = _systemConfig.PositioningConfig.BondSafeLocation;
            //IOUtilityHelper.Instance.UpDispenserCylinder();
            //if(_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, offset.Z)==StageMotionResult.Success
            //&&_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondX, offset.X) == StageMotionResult.Success
            //&&_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondY, offset.Y) == StageMotionResult.Success)
            //{
            //    ret = true;
            //}
            //return ret;


            var ret = false;
            var safePos = _systemConfig.PositioningConfig.BondSafeLocation;
            IOUtilityHelper.Instance.UpDispenserCylinder();
            if (_stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, safePos.Z) == StageMotionResult.Success)
            {
                double[] Target = new double[2];
                Target[0] = safePos.X;
                Target[1] = safePos.Y;
                EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;
                if (_stageMotionControl.AbsoluteMovingSync(multiAxis, Target) == StageMotionResult.Success)
                {
                    ret = true;
                }
            }
            return ret;

        }
        /// <summary>
        /// 榜头Z轴移动到安全位置
        /// </summary>
        public bool BondZMovetoSafeLocation()
        {
            var ret = false;
            var offset = _systemConfig.PositioningConfig.BondSafeLocation;
            IOUtilityHelper.Instance.UpDispenserCylinder();
            ret = _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, offset.Z) == StageMotionResult.Success ? true : false;
            return ret;
        }
        /// <summary>
        /// 吸嘴移动到安全位置（Z方向）
        /// </summary>
        public StageMotionResult PPMovetoSafeLocation()
        {
            try
            {
                //EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                //multiAxis[0] = EnumStageAxis.SubmountPPZ;
                //multiAxis[1] = EnumStageAxis.BondZ;
                //double[] targets = new double[2];
                //targets[0] = _systemConfig.PositioningConfig.SubmountPPFreeZ;
                //targets[1] = _systemConfig.PositioningConfig.BondSafeLocation.Z;
                //return MoveAixsToStageCoord(multiAxis, targets, EnumCoordSetType.Absolute);

                IOUtilityHelper.Instance.UpDispenserCylinder();
                return _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);
            }
            catch (Exception)
            {
                return StageMotionResult.Fail;
            }


        }
        public StageMotionResult BondXYUnionMovetoSystemCoor(double xTarget, double yTarget, EnumCoordSetType type)
        {
            double[] stageAbsoluteTarget = new double[2];
            double[] stageRelativeTarget = new double[2];

            stageAbsoluteTarget[0] = _systemConfig.PositioningConfig.BondOrigion.X - xTarget;
            stageRelativeTarget[0] = -xTarget;

            stageAbsoluteTarget[1] = yTarget + _systemConfig.PositioningConfig.BondOrigion.Y;
            stageRelativeTarget[1] = yTarget;


            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;
            var ret = StageMotionResult.Success;
            //var targetPos = new MillimeterUnitValue<double>() { Value = target };
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(multiAxis, stageAbsoluteTarget);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(multiAxis, stageRelativeTarget);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"BondXYUnionMovetoSystemCoor,Run-Exception.");
            }
            return ret;

        }
        public StageMotionResult BondXYUnionMovetoStageCoor(double xTarget, double yTarget, EnumCoordSetType type)
        {
            double[] Target = new double[2];
            Target[0] = xTarget;
            Target[1] = yTarget;
            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;
            var ret = StageMotionResult.Success;
            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(multiAxis, Target);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(multiAxis, Target);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"BondXYUnionMovetoStageCoor,Run-Exception.");
            }
            return ret;
        }
        public StageMotionResult MoveChipPPToSystemCoord(double zero, double target, EnumCoordSetType type)
        {
            var ret = StageMotionResult.Success;
            float stagePos = (float)(target + zero);

            if (type == EnumCoordSetType.Absolute)
            {
                ret = _stageMotionControl.AbsoluteMovingSync(EnumStageAxis.BondZ, stagePos);
            }
            else
            {
                ret = _stageMotionControl.RelativeMovingSync(EnumStageAxis.BondZ, target);
            }
            if (ret == StageMotionResult.Fail)
            {
                throw new Exception($"MoveAixsToStageCoord,Axis:BondZ,Run-Exception.");
            }
            return ret;
        }
    }

}
