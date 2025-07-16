using BoardCardControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using StageControllerClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestDragon.Framework.UtilityHelper;

namespace SystemCalibrationClsLib
{
    public class ZRProcess
    {

        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }


        IBoardCardController _boardCardController;


        private StageCore stage = StageControllerClsLib.StageCore.Instance;

        private static readonly object _lockObj = new object();
        private static volatile ZRProcess _instance = null;
        public static ZRProcess Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ZRProcess();
                        }
                    }
                }
                return _instance;
            }
        }


        public ZRProcess()
        {
            _boardCardController = BoardCardManager.Instance.GetCurrentController();
        }

        private int ShowMessage(string title, string content, string type)
        {
            if (WarningBox.FormShow(title, content, type) == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        private void AxisAbsoluteMove(EnumStageAxis axis, double target)
        {
            stage.ClrAlarm(axis);
            stage.AbloluteMoveSync(axis, target);
        }

        /// <summary>
        /// 榜头移动
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        private void BondXYZAbsoluteMove(double X, double Y, double Z)
        {
            //stage.AbloluteMoveSync(new EnumStageAxis[3] { EnumStageAxis.BondX, EnumStageAxis.BondY, EnumStageAxis.BondZ }, new double[3] { X, Y, Z });
            stage.ClrAlarm(EnumStageAxis.BondZ);
            stage.AbloluteMoveSync(EnumStageAxis.BondZ, Z);

            //stage.ClrAlarm(EnumStageAxis.BondX);
            //stage.AbloluteMoveSync(EnumStageAxis.BondX, X);

            //stage.ClrAlarm(EnumStageAxis.BondY);
            //stage.AbloluteMoveSync(EnumStageAxis.BondY, Y);


            EnumStageAxis[] multiAxis = new EnumStageAxis[2];
            multiAxis[0] = EnumStageAxis.BondX;
            multiAxis[1] = EnumStageAxis.BondY;

            double[] target1 = new double[2];

            target1[0] = X;
            target1[1] = Y;

            stage.AbloluteMoveSync(multiAxis, target1);
        }

        /// <summary>
        /// 榜头到安全位置
        /// </summary>
        public bool BondToSafeAsync(int Mode = 0)
        {
            bool Done = false;
            try
            {
                _boardCardController.Set_ZRAxisWorkMode(EnumStageAxis.SubmountPPZ, 0);
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ);

                double BondX = _systemConfig.PositioningConfig.BondSafeLocation.X;
                double BondY = _systemConfig.PositioningConfig.BondSafeLocation.Y;
                double BondZ = _systemConfig.PositioningConfig.BondSafeLocation.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                BondXYZAbsoluteMove(BondX, BondY, BondZ);

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
                Done = true;
                return Done;
            }
            catch
            {
                return false;
            }

            

        }

        /// <summary>
        /// 榜头到测力平台位置
        /// </summary>
        public bool BondToPressureTableAsync(int Mode = 0)
        {
            bool Done = false;
            try
            {
                _boardCardController.Set_ZRAxisWorkMode(EnumStageAxis.SubmountPPZ, 0);
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, _systemConfig.PositioningConfig.SubmountPPFreeZ);

                double BondX = 158.528;
                double BondY = 105.703;
                double BondZ = 132.975;

                AxisAbsoluteMove(EnumStageAxis.BondZ, _systemConfig.PositioningConfig.BondSafeLocation.Z);

                BondXYZAbsoluteMove(BondX, BondY, _systemConfig.PositioningConfig.BondSafeLocation.Z);

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                Done = true;
                return Done;
            }
            catch
            {
                return false;
            }



        }

        private bool ZRToPressure()
        {
            bool Done = false;
            try
            {
                _boardCardController.Set_ZRAxisWorkMode(EnumStageAxis.SubmountPPZ, 1);
                _boardCardController.Enable_ZRAxisForeceMode(EnumStageAxis.SubmountPPZ);
                Done = true;
                return Done;
            }
            catch
            {
                return false;
            }
        }

        private bool stopRun = false;

        /// <summary>
        /// ZR运行
        /// </summary>
        /// <param name="PP">指定吸嘴</param>
        /// <param name="times">循环次数</param>
        /// <param name="delaytime">间隔时间</param>
        public void Run(int times, int delaytime)
        {
            try
            {

                Task.Factory.StartNew(new Action(async () =>
                {
                    

                    for (int i = 0; i < times; i++)
                    {
                        bool Done = false;

                        //榜头移动到安全位置
                        Done = BondToSafeAsync();
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "移动到安全位置失败", "提示");
                            if (Done1 == 0)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        //榜头移动到测力平台位置
                        Done = BondToPressureTableAsync();
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "移动到测力平台位置失败", "提示");
                            if (Done1 == 0)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        //ZR施加压力
                        Done = ZRToPressure();
                        if (Done == false)
                        {
                            int Done1 = ShowMessage("动作确认", "ZR触发失败", "提示");
                            if (Done1 == 0)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        Thread.Sleep(delaytime);

                        if(stopRun)
                        {
                            break;
                        }

                    }

                    //MoveTransport();

                    //榜头移动到安全位置
                    BondToSafeAsync();







                }));

            }
            catch
            {

            }

        }






    }
}
