using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using IOUtilityClsLib;
using PositioningSystemClsLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace JobClsLib
{
    public class PPUtility
    {
        private static readonly object _lockObj = new object();
        private static volatile PPUtility _instance = null;
        public static PPUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new PPUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        private PPUtility()
        {
        }
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        public bool Pick(PPWorkParameters param, bool isSingleStepRun = false, Action beforeAct = null)
        {
            var ret = false;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                var axisConfig = _hardwareConfig.StageConfig.AxisConfigList.FirstOrDefault(i => i.Type == EnumStageAxis.BondZ);
                SingleStepRunUtility.Instance.EnableSingleStep = isSingleStepRun;
                if (param.UsedPP == EnumUsedPP.SubmountPP)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                        {
                            //衬底PPZ移动到工作位
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPWorkZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                            {
                                return false;
                            }
                        }
                           
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        //衬底PPZ移动到空闲位
                        //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                        //{
                        //    return false;
                        //}
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                if (param.IsUseNeedle)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        _positioningSystem.SetAxisSpeed(EnumStageAxis.NeedleZ, param.NeedleSpeed);
                        //NeedleZ上升时绝对坐标变小
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, -param.NeedleUpHeight, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                        {
                            return false;
                        }
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                var terminal = param.WorkHeight - param.PickupStress;
                var quickTravelTarget = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    //快速下降
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, quickTravelTarget, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    //慢速下降
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedBeforePickup);
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, terminal, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    return true;
                })))
                {
                    return false;
                }
                if(!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    if (param.UsedPP == EnumUsedPP.ChipPP)
                    {
                        //开真空
                         bool done0 = IOUtilityHelper.Instance.OpenChipPPVaccum();
                        Thread.Sleep((int)param.DelayMSForVaccum);
                        return done0;
                    }
                    else if (param.UsedPP == EnumUsedPP.SubmountPP)
                    {
                        //开真空
                        bool done0 = IOUtilityHelper.Instance.OpenSubmountPPVaccum();
                        Thread.Sleep((int)param.DelayMSForVaccum);
                        return done0;
                    }
                    
                    return true;
                })))

                {
                    return false;
                }

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴缓降吸取芯片{sw.ElapsedMilliseconds}ms \n");

                sw.Reset();
                sw.Start();

                //慢速上升
                var slowTravelTargetAfterPickup = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedAfterPickup);
                    //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, slowTravelTargetAfterPickup, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    //{
                    //    return false;
                    //}
                    //慢速相对移动
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -param.SlowTravelAfterPickupMM, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    //快速上升
                    //var TargetAfterPickup = terminal + param.UpDistanceMMAfterPicked;
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, (float)axisConfig.AxisSpeed);
                    //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, TargetAfterPickup, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    //{
                    //    return false;
                    //}
                    return true;
                })))
                {
                    return false;
                }
                if (param.IsUseNeedle)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, param.NeedleUpHeight, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                        {
                            return false;
                        }
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                if (param.UsedPP == EnumUsedPP.SubmountPP)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        //衬底PPZ移动到空闲位
                        //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                        //{
                        //    return false;
                        //}
                        return true;
                    })))
                    {
                        return false;
                    }
                }

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴缓升{sw.ElapsedMilliseconds}ms \n");

                ret = true;
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "PP Pickup Error.", ex);
                ret = false;
            }
            finally
            {

            }
            return ret;
        }
        public bool Place(PPWorkParameters param, bool isSingleStepRun = false, Action actBeforeUp = null)
        {
            var ret = false;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                var axisConfig = _hardwareConfig.StageConfig.AxisConfigList.FirstOrDefault(i => i.Type == EnumStageAxis.BondZ);

                SingleStepRunUtility.Instance.EnableSingleStep = isSingleStepRun;
                if (param.UsedPP == EnumUsedPP.SubmountPP)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                        {
                            //衬底PPZ移动到工作位
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPWorkZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                            {
                                return false;
                            }
                        }
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        //衬底PPZ移动到空闲位
                        //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                        //{
                        //    return false;
                        //}
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                var terminal = param.WorkHeight;
                var quickTravelTarget = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    //快速下降
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, quickTravelTarget, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    //慢速下降
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedBeforePickup);
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, terminal, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    return true;
                })))
                {
                    return false;
                }
                if(!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    if (param.UsedPP == EnumUsedPP.ChipPP)
                    {
                        //关真空
                        return IOUtilityHelper .Instance.CloseChipPPVaccum();
                    }
                    else if (param.UsedPP == EnumUsedPP.SubmountPP)
                    {
                        //关真空
                        return IOUtilityHelper.Instance.CloseSubmountPPVaccum();
                    }
                    Thread.Sleep((int)param.DelayMSForVaccum);
                    return true;
                })))
                {
                    return false;
                }
                if (actBeforeUp != null)
                {
                    actBeforeUp();
                }

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴缓降放下芯片{sw.ElapsedMilliseconds}ms \n");

                sw.Reset();
                sw.Start();

                var slowTravelTargetAfterPickup = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    //慢速上升
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedAfterPickup);
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, slowTravelTargetAfterPickup, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    //快速上升
                    //var TargetAfterPickup = terminal + param.UpDistanceMMAfterPicked;
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, (float)axisConfig.AxisSpeed);
                    //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, TargetAfterPickup, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    //{
                    //    return false;
                    //}
                    return true;
                })))
                {
                    return false;
                }
                if (param.UsedPP == EnumUsedPP.SubmountPP)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                        {
                            //衬底PPZ移动到空闲位
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                            {
                                return false;
                            }
                        }
                            
                        return true;
                    })))
                    {
                        return false;
                    }
                }

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"吸嘴缓升{sw.ElapsedMilliseconds}ms \n");

                ret = true;
            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.RecordLog(EnumLogContentType.Error, "PP Place Error.", ex);
                ret = false;
            }
            finally
            {

            }
            return ret;
        }
        /// <summary>
        /// PP拾取，Z使用系统坐标系
        /// </summary>
        /// <param name="param"></param>
        /// <param name="isSingleStepRun"></param>
        /// <param name="beforeAct"></param>
        /// <returns></returns>
        public bool PickViaSystemCoor(PPWorkParameters param, Action beforeAct = null)
        {
            var ret = false;
            try
            {
                var axisConfig = _hardwareConfig.StageConfig.AxisConfigList.FirstOrDefault(i => i.Type == EnumStageAxis.BondZ);
                ////SingleStepRunUtility.Instance.EnableSingleStep = isSingleStepRun;
                //if (param.UsedPP == EnumUsedPP.SubmountPP)
                //{
                //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                //    {
                //        //衬底PPZ移动到工作位
                //        //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPWorkZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                //        //{
                //        //    return false;
                //        //}
                //        return true;
                //    })))
                //    {
                //        return false;
                //    }
                //}
                //else
                //{
                //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                //    {
                //        //衬底PPZ移动到空闲位
                //        //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                //        //{
                //        //    return false;
                //        //}
                //        return true;
                //    })))
                //    {
                //        return false;
                //    }
                //}

                if (param.PPtoolName != null)
                {
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == param.PPtoolName);
                    if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                    {
                        if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                        {
                            if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                            {
                                //衬底PPZ移动到工作位
                                if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                                {
                                    return false;
                                }
                            }
                               
                            return true;
                        })))
                        {
                            return false;
                        }
                    }

                }
                else
                {
                    return false;
                }

               
                //if (param.IsUseNeedle)
                //{
                //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                //     {
                //         _positioningSystem.SetAxisSpeed(EnumStageAxis.NeedleZ, param.NeedleSpeed);
                //         //NeedleZ上升时绝对坐标变小
                //         var actualAngle = param.NeedleUpHeight / (8f / 360f);
                //         if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, actualAngle, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                //         {
                //             return false;
                //         }
                //         return true;
                //     })))
                //    {
                //        return false;
                //    }
                //}
                var terminal = param.WorkHeight - param.PickupStress;
                //var terminal = param.WorkHeight;
                var quickTravelTarget = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                 {
                     LogRecorder.RecordLog(EnumLogContentType.Info, "PickViaSystemCoor-下降-Start.");
                     //快速下降
                     if (_positioningSystem.MoveChipPPToSystemCoord(param.PPToolZero, quickTravelTarget, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                     {
                         return false;
                     }
                     //慢速下降
                     _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedBeforePickup);
                     //if (_positioningSystem.MoveChipPPToSystemCoord(param.PPToolZero, terminal, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                     //{
                     //    return false;
                     //}

                     if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, param.SlowTravelAfterPickupMM, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                     {
                         return false;
                     }

                     LogRecorder.RecordLog(EnumLogContentType.Info, "PickViaSystemCoor-下降-End.");
                     return true;
                 })))
                {
                    return false;
                }
                //下压之后，再开吸嘴真空，再顶针顶起
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                 {
                     LogRecorder.RecordLog(EnumLogContentType.Info, "PickViaSystemCoor-开真空-Start.");
                     //if (param.UsedPP == EnumUsedPP.ChipPP)
                     //{
                     //    //开真空
                     //    if (!IOUtilityHelper.Instance.OpenChipPPVaccum()&& _systemConfig.JobConfig.EnableVaccumConfirm)
                     //    {
                     //        return false;
                     //    }
                     //}
                     //else if (param.UsedPP == EnumUsedPP.SubmountPP)
                     //{
                     //    //开真空
                     //    if (!IOUtilityHelper.Instance.OpenSubmountPPVaccum()&& _systemConfig.JobConfig.EnableVaccumConfirm)
                     //    {
                     //        return false;
                     //    }
                     //}

                     var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == param.PPtoolName);
                     //开真空
                     if (!IOUtilityHelper.Instance.OpenPPtoolVaccum(pptool.PPVaccumSwitch, pptool.PPVaccumNormally) && _systemConfig.JobConfig.EnableVaccumConfirm)
                     {
                         return false;
                     }


                     LogRecorder.RecordLog(EnumLogContentType.Info, "PickViaSystemCoor-开真空-End.");
                     return true;
                 })))
                {
                    return false;
                }

       
                if (param.IsUseNeedle)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        _positioningSystem.SetAxisSpeed(EnumStageAxis.NeedleZ, param.NeedleSpeed);
                        //NeedleZ上升时绝对坐标变小
                        var actualAngle = param.NeedleUpHeight / (8f / 360f);
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, actualAngle, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                        {
                            return false;
                        }
                        //Thread.Sleep(4000);
                        return true;
                    })))
                    {
                        return false;
                    }
                }

                Thread.Sleep((int)param.DelayMSForVaccum);

                if (beforeAct != null)
                {
                    beforeAct();
                }


                var slowTravelTargetAfterPickup = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                 {
                     LogRecorder.RecordLog(EnumLogContentType.Info, "PickViaSystemCoor-上升-Start.");
                     //慢速上升
                     _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedAfterPickup);
                     //if (_positioningSystem.MoveChipPPToSystemCoord(param.PPToolZero, slowTravelTargetAfterPickup, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                     //{
                     //    return false;
                     //}

                     //慢速相对移动抬升
                     if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -param.SlowTravelAfterPickupMM, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                     {
                         return false;
                     }
                     //快速抬升
                     _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, (float)axisConfig.AxisSpeed);
                     //if (param.UsedPP == EnumUsedPP.SubmountPP)
                     //{
                     //    if(_positioningSystem.PPMovetoSafeLocation() == StageMotionResult.Fail)
                     //    {
                     //        return false;
                     //    }
                     //}
                     //else
                     //{
                     //    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                     //    {
                     //        return false;
                     //    }
                     //}

                     if (_positioningSystem.PPMovetoSafeLocation() == StageMotionResult.Fail)
                     {
                         return false;
                     }

                     LogRecorder.RecordLog(EnumLogContentType.Info, "PickViaSystemCoor-上升-End.");
                     return true;
                 })))
                {
                    return false;
                }
                if (param.IsUseNeedle)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        var actualAngle = param.NeedleUpHeight / (8f / 360f);
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, -actualAngle, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                        {
                            return false;
                        }
                        return true;
                    })))
                    {
                        return false;
                    }
                }
                //if (param.UsedPP == EnumUsedPP.SubmountPP)
                //{
                //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                //     {
                //         //衬底PPZ移动到空闲位
                //         if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                //         {
                //             return false;
                //         }
                //         return true;
                //     })))
                //    {
                //        return false;
                //    }
                //}
                ret = true;
            }
            catch (Exception ex)
            {
                if (param.IsUseNeedle)
                {
                    var actualAngle = param.NeedleUpHeight / (8f / 360f);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, -actualAngle, EnumCoordSetType.Relative);
                }
                LogRecorder.RecordLog(EnumLogContentType.Error, "PP PickViaSystemCoor Error.", ex);
                ret = false;
            }
            finally
            {
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute);
                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute);
                _positioningSystem.BondZMovetoSafeLocation();
            }
            return ret;
        }
        /// <summary>
        ///  PP放置，Z使用系统坐标系
        /// </summary>
        /// <param name="param"></param>
        /// <param name="isSingleStepRun"></param>
        /// <param name="actBeforeUp"></param>
        /// <returns></returns>
        public bool PlaceViaSystemCoor(PPWorkParameters param, Action actBefore = null, Action actBeforeUp = null, bool isUpAfterPlace = true)
        {
            var ret = false;
            try
            {
                if (actBefore != null)
                {
                    actBefore();
                }
                LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-Start.");
                var axisConfig = _hardwareConfig.StageConfig.AxisConfigList.FirstOrDefault(i => i.Type == EnumStageAxis.BondZ);

                ////SingleStepRunUtility.Instance.EnableSingleStep = isSingleStepRun;
                //if (param.UsedPP == EnumUsedPP.SubmountPP)
                //{
                //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                //     {
                //         //衬底PPZ移动到工作位
                //         //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPWorkZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                //         //{
                //         //    return false;
                //         //}

                //         return true;

                //     })))
                //    {
                //        return false;
                //    }
                //}
                //else
                //{
                //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                //    {
                //        //衬底PPZ移动到空闲位
                //        //if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                //        //{
                //        //    return false;
                //        //}

                //        return true;

                //    })))
                //    {
                //        return false;
                //    }
                //}

                if (param.PPtoolName != null)
                {
                    var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == param.PPtoolName);
                    if (pptool.EnumPPtool == EnumPPtool.PPtool2)
                    {
                        if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                        {
                            if (_systemConfig.SystemMode == EnumSystemMode.Eutectic)
                            {
                                //衬底PPZ移动到工作位
                                if (_positioningSystem.MoveAixsToStageCoord(pptool.StageAxisZ, pptool.PPWorkZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                                {
                                    return false;
                                }
                            }
                               
                            return true;
                        })))
                        {
                            return false;
                        }
                    }

                }
                else
                {
                    return false;
                }


                var terminal = param.WorkHeight - param.PickupStress; ;
                var quickTravelTarget = terminal + param.SlowTravelAfterPickupMM;
                if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                {
                    LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-下降-Start.");
                    //快速下降
                    if (_positioningSystem.MoveChipPPToSystemCoord(param.PPToolZero, quickTravelTarget, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    //慢速下降
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedBeforePickup);
                    //if (_positioningSystem.MoveChipPPToSystemCoord(param.PPToolZero, terminal, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    //{
                    //    return false;
                    //}
                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, param.SlowTravelBeforePickupMM, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                    {
                        return false;
                    }
                    LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-下降-End.");
                    return true;
                })))
                {
                    return false;
                }
                //放置之后是否需要抬起
                if (isUpAfterPlace)
                {
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                     {
                         LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-VuccumOperate-Start.");
                         Thread.Sleep((int)param.DelayMSForPlace);
                         //if (param.UsedPP == EnumUsedPP.ChipPP)
                         //{
                         //    //关真空
                         //    IOUtilityHelper.Instance.CloseChipPPVaccum();
                         //    Thread.Sleep(10);
                         //    IOUtilityHelper.Instance.OpenChipPPBlow();
                         //    Thread.Sleep((int)param.BreakVaccumTimespanMS);
                         //    IOUtilityHelper.Instance.CloseChipPPBlow();
                         //}
                         //else if (param.UsedPP == EnumUsedPP.SubmountPP)
                         //{
                         //    //关真空
                         //    IOUtilityHelper.Instance.CloseSubmountPPVaccum();
                         //    Thread.Sleep(10);
                         //    IOUtilityHelper.Instance.OpenSubmountPPBlow();
                         //    Thread.Sleep((int)param.BreakVaccumTimespanMS);
                         //    IOUtilityHelper.Instance.CloseSubmountPPBlow();
                         //}


                         var pptool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == param.PPtoolName);
                         //关真空
                         IOUtilityHelper.Instance.ClosePPtoolVaccum(pptool.PPVaccumSwitch, pptool.PPVaccumNormally);
                         Thread.Sleep(10);
                         IOUtilityHelper.Instance.OpenPPtoolBlow(pptool.PPBlowSwitch);
                         Thread.Sleep((int)param.BreakVaccumTimespanMS);
                         IOUtilityHelper.Instance.ClosePPtoolBlow(pptool.PPBlowSwitch);

                         //Thread.Sleep((int)param.DelayMSForVaccum);
                         LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-VuccumOperate-End.");
                         return true;
                     })))
                    {
                        return false;
                    }

                    if (actBeforeUp != null)
                    {
                        actBeforeUp();
                    }
                    var slowTravelTargetAfterPickup = terminal + param.SlowTravelAfterPickupMM;
                    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    {
                        LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-上升-Start.");
                        //慢速上升
                        _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, param.SlowSpeedAfterPickup);
                        //if (_positioningSystem.MoveChipPPToSystemCoord(param.PPToolZero, slowTravelTargetAfterPickup, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                        //{
                        //    return false;
                        //}
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -param.SlowTravelAfterPickupMM, EnumCoordSetType.Relative) == StageMotionResult.Fail)
                        {
                            return false;
                        }
                        //快速上升
                        _positioningSystem.SetAxisSpeed(EnumStageAxis.BondZ, (float)axisConfig.AxisSpeed);
                        //if (param.UsedPP == EnumUsedPP.SubmountPP)
                        //{
                        //    if (_positioningSystem.PPMovetoSafeLocation() == StageMotionResult.Fail)
                        //    {
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                        //    {
                        //        return false;
                        //    }
                        //}

                        if (_positioningSystem.PPMovetoSafeLocation() == StageMotionResult.Fail)
                        {
                            return false;
                        }

                        LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-上升-End.");
                        return true;
                    })))
                    {
                        return false;
                    }
                    //if (param.UsedPP == EnumUsedPP.SubmountPP)
                    //{
                    //    if (!SingleStepRunUtility.Instance.RunAction(new Func<bool>(() =>
                    //    {
                    //    //衬底PPZ移动到空闲位
                    //    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ, EnumCoordSetType.Absolute) == StageMotionResult.Fail)
                    //        {
                    //            return false;
                    //        }
                    //        return true;
                    //    })))
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                LogRecorder.RecordLog(EnumLogContentType.Info, "PlaceViaSystemCoor-End.");
                ret = true;
            }
            catch (Exception ex)
            {
                _positioningSystem.PPMovetoSafeLocation();
                LogRecorder.RecordLog(EnumLogContentType.Error, "PP PlaceViaSystemCoor Error.", ex);
                ret = false;
            }
            finally
            {

            }
            return ret;
        }




        public void LoadPPTool(string ppToolName)
        {
            PPToolSettings currentTool = null;
            if (!string.IsNullOrEmpty(ppToolName))
            {
                currentTool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == ppToolName);
            }
            if (currentTool != null)
            {
                //旋转PPTool转台
                PositioningSystem.Instance.MoveAixsToStageCoord(EnumStageAxis.PPtoolBankTheta, currentTool.RotationTablePos4LoadPP, EnumCoordSetType.Absolute);
                //
            }

        }
        public void UnloadPPTool(string ppToolName)
        {
            PPToolSettings currentTool = null;
            if (!string.IsNullOrEmpty(ppToolName))
            {
                currentTool = _systemConfig.PPToolSettings.FirstOrDefault(i => i.Name == ppToolName);
            }
            if (currentTool != null)
            {
                //旋转PPTool转台
                PositioningSystem.Instance.MoveAixsToStageCoord(EnumStageAxis.PPtoolBankTheta, currentTool.RotationTablePos4LoadPP, EnumCoordSetType.Absolute);
                //
            }
        }
    }
}
