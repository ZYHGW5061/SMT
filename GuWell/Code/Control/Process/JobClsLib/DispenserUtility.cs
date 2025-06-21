using DispensingMachineControllerClsLib;
using DispensingMachineManagerClsLib;
using GlobalDataDefineClsLib;
using PositioningSystemClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.UtilityHelper;

namespace JobClsLib
{
    public class DispenserUtility
    {
        private static readonly object _lockObj = new object();
        private static volatile DispenserUtility _instance = null;
        public static DispenserUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new DispenserUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        private DispenserUtility()
        {
        }
        /// <summary>
        /// 点胶机控制器
        /// </summary>
        IDispensingMachineController _currentDispenseController
        {
            get
            {
                return DispensingMachineManager.Instance.GetCurrentHardware();
            }
        }
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }
        public bool ExecutePointRecipe(string recipeName, bool upCylinderAfterDispense = false,bool upBondZAfterDispense = true)
        {
            try
            {
                var ret = false;
                if (_currentDispenseController != null && _currentDispenseController.IsConnect)
                {
                    ret = _currentDispenseController.Set(MUSASHICommandenum.TIMED模式切换) & _currentDispenseController.Set(MUSASHICommandenum.通道加载, recipeName) & _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                    var dispenseInfo = _currentDispenseController.ReadDispensingParameters(Int32.Parse(recipeName));
                    Thread.Sleep((int)(dispenseInfo.Time*1000));

                }
                return ret;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                if (upBondZAfterDispense)
                {
                    //Z抬升
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -2, EnumCoordSetType.Relative);
                }
            }

        }
        public bool ExecutePoint(bool upCylinderAfterDispense = false, bool upBondZAfterDispense = true)
        {
            try
            {
                var ret = false;
                if (_currentDispenseController != null && _currentDispenseController.IsConnect)
                {
                    ret = _currentDispenseController.Set(MUSASHICommandenum.TIMED模式切换) & _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                }
                return ret;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (upBondZAfterDispense)
                {
                    //Z抬升
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -2, EnumCoordSetType.Relative);
                }
            }

        }
        /// <summary>
        /// 画十字
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="upCylinderAfterDispense"></param>
        /// <param name="upBondZAfterDispense"></param>
        /// <returns></returns>
        public bool DrawCross(float width,float height,bool upCylinderAfterDispense=false, bool upBondZAfterDispense = true)
        {
            try
            {
                var ret = false;
                if (_currentDispenseController != null && _currentDispenseController.IsConnect)
                {
                    if (_positioningSystem.SetAxisSpeed(EnumStageAxis.BondX, 0.3f) &&
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondY, 0.3f))
                    {
                        _currentDispenseController.Set(MUSASHICommandenum.MANUAL模式切换);

                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -3, EnumCoordSetType.Relative) == StageMotionResult.Success
                        //画横线
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, -width / 2, EnumCoordSetType.Relative) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {
                            _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                            System.Threading.Thread.Sleep(500);
                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, width, EnumCoordSetType.Relative) == StageMotionResult.Success)
                            {
                                _currentDispenseController.Set(MUSASHICommandenum.吐出要求);

                                //回归原点
                                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -3, EnumCoordSetType.Relative) == StageMotionResult.Success
                                && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, -width / 2, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                {

                                    //画竖线
                                    if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -height / 2, EnumCoordSetType.Relative) == StageMotionResult.Success
                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                    {
                                        _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                                        System.Threading.Thread.Sleep(500);
                                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, height, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                        {
                                            _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                                            //回归原点
                                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                            {
                                                _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -height / 2, EnumCoordSetType.Relative);
                                                ret = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if(upBondZAfterDispense)
                {
                    //Z抬升
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -2, EnumCoordSetType.Relative);
                }
            }
            
        }

        /// <summary>
        /// 画斜十字叉
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="upCylinderAfterDispense"></param>
        /// <param name="upBondZAfterDispense"></param>
        /// <returns></returns>
        public bool DrawGreekCross(float width, float height, bool upCylinderAfterDispense = false, bool upBondZAfterDispense = true)
        {
            try
            {
                var ret = false;
                if (_currentDispenseController != null && _currentDispenseController.IsConnect)
                {
                    if (_positioningSystem.SetAxisSpeed(EnumStageAxis.BondX, 0.3f) &&
                    _positioningSystem.SetAxisSpeed(EnumStageAxis.BondY, 0.3f))
                    {
                        var curStageCoorX = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
                        var curStageCoorY = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);

                        var line1StartPosX = curStageCoorX + width / 2;
                        var line1EndPosX = curStageCoorX - width / 2;
                        var line1StartPosY = curStageCoorY + height / 2;
                        var line1EndPosY = curStageCoorY - height / 2; 

                        var line2StartPosX = curStageCoorX + width / 2; 
                        var line2EndPosX = curStageCoorX - width / 2; 
                        var line2StartPosY = curStageCoorY - height / 2;
                        var line2EndPosY = curStageCoorY + height / 2;

                        _currentDispenseController.Set(MUSASHICommandenum.MANUAL模式切换);
                        if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -3, EnumCoordSetType.Relative) == StageMotionResult.Success
                        //画第一条线
                        && _positioningSystem.BondXYUnionMovetoStageCoor(line1StartPosX, line1StartPosY, EnumCoordSetType.Absolute) == StageMotionResult.Success
                        && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                        {
                            _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                            System.Threading.Thread.Sleep(500);
                            if (_positioningSystem.BondXYUnionMovetoStageCoor(line1EndPosX, line1EndPosY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                            {
                                _currentDispenseController.Set(MUSASHICommandenum.吐出要求);

                                //回归原点
                                if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                {

                                    //画第二条线
                                    if (_positioningSystem.BondXYUnionMovetoStageCoor(line2StartPosX, line2StartPosY, EnumCoordSetType.Absolute) == StageMotionResult.Success
                                    && _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                    {
                                        _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                                        System.Threading.Thread.Sleep(500);
                                        if (_positioningSystem.BondXYUnionMovetoStageCoor(line2EndPosX, line2EndPosY, EnumCoordSetType.Absolute) == StageMotionResult.Success)
                                        {
                                            _currentDispenseController.Set(MUSASHICommandenum.吐出要求);
                                            //回归原点
                                            if (_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -3, EnumCoordSetType.Relative) == StageMotionResult.Success)
                                            {
                                                //_positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -height / 2, EnumCoordSetType.Relative);
                                                ret = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (upBondZAfterDispense)
                {
                    //Z抬升
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, -2, EnumCoordSetType.Relative);
                }
            }

        }
    }
}
