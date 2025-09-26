using ConfigurationClsLib;
using MathHelperClsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositioningSystemClsLib
{
    public class StageInterlockJudgeHelper
    {
        private static volatile StageInterlockJudgeHelper _instance = new StageInterlockJudgeHelper();
        private static readonly object _lockObj = new object();

        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static StageInterlockJudgeHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new StageInterlockJudgeHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        private StageInterlockJudgeHelper()
        {
        }

        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        public bool IsWafertableintheSafeZoneForESZMove()
        {
            var ret = false;
            var curWaferTableXPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.WaferTableX);
            var curWaferTableYPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.WaferTableY);
            PointF circleCenter = new PointF();
            var circleDiameter = 0f;
            CircleComputer.GetCircleCenterByThreeEdgePointsCoordinate(_systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1
                , _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2
                , _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3, out circleCenter, out circleDiameter);

            if (Math.Pow(curWaferTableXPos - circleCenter.X, 2) + Math.Pow(curWaferTableYPos - circleCenter.Y, 2) < Math.Pow(circleDiameter / 2f, 2))
            {
                ret = true;
            }

            return ret;
        }

        public bool IsWafertableXTargetSafe(double stageTarget)
        {
            var ret = false;
            var curWaferTableYPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.WaferTableY);
            var curESZPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.ESZ);
            if(curESZPos>_systemConfig.PositioningConfig.ESZWariningPos)
            {
                ret = true;
            }
            else
            {
                PointF circleCenter = new PointF();
                var circleDiameter = 0f;
                CircleComputer.GetCircleCenterByThreeEdgePointsCoordinate(_systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1
                    , _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2
                    , _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3, out circleCenter, out circleDiameter);

                if (Math.Pow(stageTarget - circleCenter.X, 2) + Math.Pow(curWaferTableYPos - circleCenter.Y, 2) < Math.Pow(circleDiameter / 2f, 2))
                {
                    ret = true;
                }
            }
            return ret;
        }
        public bool IsWafertableYTargetSafe(double stageTarget)
        {
            var ret = false;
            var curWaferTableXPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.WaferTableX);
            var curESZPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.ESZ);
            if (curESZPos > _systemConfig.PositioningConfig.ESZWariningPos)
            {
                ret = true;
            }
            else
            {
                PointF circleCenter = new PointF();
                var circleDiameter = 0f;
                CircleComputer.GetCircleCenterByThreeEdgePointsCoordinate(_systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint1
                    , _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint2
                    , _systemConfig.PositioningConfig.ESZSafeZoneofWaferTablePoint3, out circleCenter, out circleDiameter);

                if (Math.Pow(curWaferTableXPos - circleCenter.X, 2) + Math.Pow(stageTarget - circleCenter.Y, 2) < Math.Pow(circleDiameter / 2f, 2))
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsBondZTargetSafe(double stageTarget)
        {
            var ret = false;

            if (stageTarget > _systemConfig.PositioningConfig.BondZWariningPos)
            {
                ret = true;
            }
            else
            {
                var curBondXPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.BondX);
                var curBondYPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.BondY);
                if (Math.Abs(_systemConfig.PositioningConfig.WaferCameraOrigion.X - curBondXPos) < (_systemConfig.PositioningConfig.BondXYSafeRangeForBondZ.X / 2f)
                    && Math.Abs(_systemConfig.PositioningConfig.WaferCameraOrigion.Y - curBondYPos) < (_systemConfig.PositioningConfig.BondXYSafeRangeForBondZ.Y / 2f))
                {
                    ret = true;
                }
            }
            return ret;
        }
        public bool IsBondXYAllowedMove()
        {
            var ret = false;
            var curBondZPos = _positioningSystem.ReadCurrentStagePosition(GlobalDataDefineClsLib.EnumStageAxis.BondZ);
            if (curBondZPos > _systemConfig.PositioningConfig.BondZWariningPos)
            {
                ret = true;
            }
            return ret;
        }
    }
}
