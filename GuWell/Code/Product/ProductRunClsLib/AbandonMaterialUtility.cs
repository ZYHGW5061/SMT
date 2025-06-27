using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using PositioningSystemClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace ProductRunClsLib
{
    public class AbandonMaterialUtility
    {
        private static readonly object _lockObj = new object();
        private static volatile AbandonMaterialUtility _instance = null;
        public static AbandonMaterialUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AbandonMaterialUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        private AbandonMaterialUtility()
        {
        }

        protected PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }


        //系统设置类
        protected SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        public GWResult Abandon(PPToolSettings ppTool = null)
        {
            try
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                bool zdone = _positioningSystem.BondZMovetoSafeLocation();
                if (zdone)
                {
                    if (ppTool == null)
                    {

                        double X = _systemConfig.PositioningConfig.AbandonMaterialPosition.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X;
                        double Y = _systemConfig.PositioningConfig.AbandonMaterialPosition.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y;

                        IOUtilityHelper.Instance.UpDispenserCylinder();
                        StageMotionResult xdone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                        StageMotionResult ydone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);
                        if (xdone == StageMotionResult.Success && ydone == StageMotionResult.Success)
                        {
                            //关真空
                            IOUtilityHelper.Instance.CloseChipPPVaccum();
                            Thread.Sleep(10);
                            IOUtilityHelper.Instance.OpenChipPPBlow();
                            Thread.Sleep(100);
                            IOUtilityHelper.Instance.CloseChipPPBlow();

                            return GlobalGWResultDefine.RET_SUCCESS;
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }


                    }
                    else
                    {

                        double X = _systemConfig.PositioningConfig.AbandonMaterialPosition.X + ppTool.PP1AndBondCameraOffset.X;
                        double Y = _systemConfig.PositioningConfig.AbandonMaterialPosition.Y + ppTool.PP1AndBondCameraOffset.Y;

                        if(_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                        {

                        }
                        else
                        {
                            IOUtilityHelper.Instance.UpDispenserCylinder();
                        }
                        
                        StageMotionResult xdone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                        StageMotionResult ydone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);

                        StageMotionResult zzdone = StageMotionResult.Success;

                        if (ppTool.EnumPPtool == EnumPPtool.PPtool2)
                        {
                            if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                            {
                                zzdone = _positioningSystem.MoveAixsToStageCoord(ppTool.StageAxisZ, ppTool.PPWorkZ, EnumCoordSetType.Absolute);
                            }
                        }

                        
                        
                        if (xdone == StageMotionResult.Success && ydone == StageMotionResult.Success && zzdone == StageMotionResult.Success)
                        {
                            //关真空
                            IOUtilityHelper.Instance.ClosePPtoolVaccum(ppTool.PPVaccumSwitch, ppTool.PPVaccumNormally);
                            Thread.Sleep(10);
                            IOUtilityHelper.Instance.OpenPPtoolBlow(ppTool.PPBlowSwitch);
                            Thread.Sleep(1000);
                            IOUtilityHelper.Instance.ClosePPtoolBlow(ppTool.PPBlowSwitch);

                            if (ppTool.EnumPPtool == EnumPPtool.PPtool2)
                            {
                                if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                                {
                                    zzdone = _positioningSystem.MoveAixsToStageCoord(ppTool.StageAxisZ, ppTool.PPFreeZ, EnumCoordSetType.Absolute);
                                }
                            }

                            return GlobalGWResultDefine.RET_SUCCESS;
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }



                    }

                }
                else
                {
                    return GlobalGWResultDefine.RET_FAILED;
                }
                return GlobalGWResultDefine.RET_FAILED;

            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Abandon Material Error.", ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.BondZMovetoSafeLocation();
            }

        }


        public GWResult Abandon(EnumUsedPP usedPP)
        {
            try
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                bool zdone = _positioningSystem.BondZMovetoSafeLocation();
                if(zdone)
                {
                    if (usedPP == EnumUsedPP.ChipPP)
                    {

                        double X = _systemConfig.PositioningConfig.AbandonMaterialPosition.X + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X;
                        double Y = _systemConfig.PositioningConfig.AbandonMaterialPosition.Y + _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y;

                        IOUtilityHelper.Instance.UpDispenserCylinder();
                        StageMotionResult xdone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                        StageMotionResult ydone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);
                        if(xdone == StageMotionResult.Success && ydone == StageMotionResult.Success)
                        {
                            //关真空
                            IOUtilityHelper.Instance.CloseChipPPVaccum();
                            Thread.Sleep(10);
                            IOUtilityHelper.Instance.OpenChipPPBlow();
                            Thread.Sleep(100);
                            IOUtilityHelper.Instance.CloseChipPPBlow();

                            return GlobalGWResultDefine.RET_SUCCESS;
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }

                        
                    }
                    else if (usedPP == EnumUsedPP.SubmountPP)
                    {

                        double X = _systemConfig.PositioningConfig.AbandonMaterialPosition.X + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.X;
                        double Y = _systemConfig.PositioningConfig.AbandonMaterialPosition.Y + _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y;

                        IOUtilityHelper.Instance.UpDispenserCylinder();
                        StageMotionResult xdone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, X, EnumCoordSetType.Absolute);
                        StageMotionResult ydone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, Y, EnumCoordSetType.Absolute);
                        StageMotionResult zzdone = _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPWorkZ, EnumCoordSetType.Absolute);
                        if (xdone == StageMotionResult.Success && ydone == StageMotionResult.Success && zzdone == StageMotionResult.Success)
                        {
                            //关真空
                            IOUtilityHelper.Instance.CloseSubmountPPVaccum();
                            Thread.Sleep(10);
                            IOUtilityHelper.Instance.OpenSubmountPPBlow();
                            Thread.Sleep(1000);
                            IOUtilityHelper.Instance.CloseSubmountPPBlow();

                            _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);

                            return GlobalGWResultDefine.RET_SUCCESS;
                        }
                        else
                        {
                            return GlobalGWResultDefine.RET_FAILED;
                        }
                        

                        
                    }

                }
                else
                {
                    return GlobalGWResultDefine.RET_FAILED;
                }
                return GlobalGWResultDefine.RET_FAILED;

            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Abandon Material Error.",ex);
                return GlobalGWResultDefine.RET_FAILED;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.BondZMovetoSafeLocation();
            }
            
        }
    }
}
