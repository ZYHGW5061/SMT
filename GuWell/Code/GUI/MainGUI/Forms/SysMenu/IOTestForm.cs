using CommonPanelClsLib;
using ConfigurationClsLib;
using DevExpress.XtraEditors;
using DynamometerControllerClsLib;
using DynamometerManagerClsLib;
using GlobalDataDefineClsLib;
using HardwareManagerClsLib;
using IOUtilityClsLib;
using LaserSensorControllerClsLib;
using PositioningSystemClsLib;
using StageControllerClsLib;
using StageManagerClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionClsLib;
using WestDragon.Framework.UtilityHelper;
using static GlobalToolClsLib.GlobalCommFunc;

namespace MainGUI.Forms.SysMenu
{
    public partial class IOTestForm : DevExpress.XtraEditors.XtraForm
    {
        protected PositioningSystem _positionSystem
        {
            get
            {
                return PositioningSystem.Instance;
            }
        }
        /// <summary>
        /// STAGE
        /// </summary>
        private IStageController _stageEngine
        {
            get { return StageManager.Instance.GetCurrentController(); }
        }
        //当前激活轴
        public EnumStageAxis curActiveAxis = EnumStageAxis.None;

        //所有轴坐标
        private double[] allPos = new double[20];

        private DynamometerManager _DynamometerManager
        {
            get { return DynamometerManager.Instance; }
        }
        private IDynamometerController _DynamometerController
        {
            get
            {
                return _DynamometerManager.GetCurrentHardware();
            }
        }
        private ILaserSensorController _laserSensor
        {
            get { return HardwareManager.Instance.LaserSensor; }
        }
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }




        public IOTestForm()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            //EnumHelper.FillComboBoxWithEnum(cbSelectAxis, typeof(EnumStageAxis));
            //cbSelectAxis.DataSource = Enum.GetValues(typeof(EnumStageAxis));
            //cbSelectAxis.SelectedItem = EnumStageAxis.None;
            EnumHelper.FillComboBoxWithEnumDesc(cbSelectAxis, typeof(EnumStageAxis));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //---------------------------- Bond轴点动部分 ------------------------------
        //Bond轴X 正向 按下移动
        private void btnBondJogXPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondX;
            float speed = float.Parse(teBondSpeedX.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.BondX, speed);
        }

        //Bond轴X 正向 抬起停止
        private void btnBondJogXPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.BondX);
        }

        //Bond轴X 反向 按下移动        
        private void btnBondJogXNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondX;
            float speed = float.Parse(teBondSpeedX.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.BondX, speed);
        }

        //Bond轴X 反向 抬起停止
        private void btnBondJogXNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.BondX);
        }

        //Bond轴Y 正向 按下移动
        private void btnBondJogYPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondY;
            float speed = float.Parse(teBondSpeedY.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.BondY, speed);
        }

        //Bond轴Y 正向 抬起停止
        private void btnBondJogYPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.BondY);
        }

        //Bond轴Y 反向 按下移动
        private void btnBondJogYNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondY;
            float speed = float.Parse(teBondSpeedY.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.BondY, speed);
        }

        //Bond轴Y 反向 抬起停止
        private void btnBondJogYNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.BondY);
        }

        //Bond轴Z 正向 按下移动
        private void btnBondJogZPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondZ;
            float speed = float.Parse(teBondSpeedZ.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.BondZ, speed);
        }

        //Bond轴Z 正向 抬起停止
        private void btnBondJogZPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.BondZ);
        }

        //Bond轴Z 反向 按下移动
        private void btnBondJogZNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondZ;
            float speed = float.Parse(teBondSpeedZ.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.BondZ, speed);
        }

        //Bond轴Z 反向 抬起停止
        private void btnBondJogZNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.BondZ);
        }

        //---------------------------- Bond轴定点MOVE部分 ------------------------------

        //Bond轴X 移动至指定位置
        private void btnBondMoveX_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondX;
            double target = double.Parse(teBondTargetX.Text);

            if (chkBondRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, target, EnumCoordSetType.Absolute);
            }
            
        }

        //Bond轴Y 移动至指定位置
        private void btnBondMoveY_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.BondY;
            double target = double.Parse(teBondTargetY.Text);
            
            if (chkBondRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, target, EnumCoordSetType.Absolute);
            }
        }

        //Bond轴Z 移动至指定位置
        private void btnBondMoveZ_Click(object sender, EventArgs e)
        {
            //curActiveAxis = EnumStageAxis.BondZ;
            //double target = double.Parse(teBondTargetZ.Text);

            //if (chkBondRelative.Checked)
            //{
            //    _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, target, EnumCoordSetType.Relative);
            //}
            //else
            //{
            //    _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, target, EnumCoordSetType.Absolute);
            //}
            //for (int i = 0; i < 20; i++)
            //{
            //    _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 55, EnumCoordSetType.Absolute);
            //    _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 155, EnumCoordSetType.Absolute);
            //}
        }

        //Bond轴XYZ三轴联动 移动至指定位置
        private void btnBondMoveXYZ_Click(object sender, EventArgs e)
        {

        }

        //---------------------------- 顶针座轴 点动部分 ------------------------------
        //顶针座轴Z 正向 按下移动
        private void btnNeedleBaseJogZPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.ESZ;
            float speed = float.Parse(teNeedleBaseSpeedZ.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.ESZ, speed);
        }

        //顶针座轴Z 正向 抬起停止
        private void btnNeedleBaseJogZPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.ESZ);
        }

        //顶针座轴Z 反向 按下移动
        private void btnNeedleBaseJogZNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.ESZ;
            float speed = float.Parse(teNeedleBaseSpeedZ.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.ESZ, speed);
        }

        //顶针座轴Z 反向 抬起停止
        private void btnNeedleBaseJogZNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.ESZ);
        }

        //---------------------------- 顶针座轴定点MOVE部分 ------------------------------
        //顶针座轴 Z 移动至指定位置
        private void btnNeedleBaseMoveZ_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.ESZ;
            double target = double.Parse(teNeedleBaseTargetZ.Text);
            
            if (chkNeedleBaseRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.ESZ, target, EnumCoordSetType.Absolute);
            }
        }

        //---------------------------- 顶针轴 点动部分 ------------------------------
        //顶针轴Z 正向 按下移动
        private void btnNeedleJogZPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.NeedleZ;
            float speed = float.Parse(teNeedleSpeedZ.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.NeedleZ, speed);
        }

        //顶针轴Z 正向 抬起停止
        private void btnNeedleJogZPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.NeedleZ);
        }

        //顶针轴Z 反向 按下移动
        private void btnNeedleJogZNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.NeedleZ;
            float speed = float.Parse(teNeedleSpeedZ.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.NeedleZ, speed);
        }

        //顶针轴Z 反向 抬起停止
        private void btnNeedleJogZNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.NeedleZ);
        }

        //---------------------------- 顶针轴定点MOVE部分 ------------------------------
        //顶针轴 Z 移动至指定位置
        private void btnNeedleMoveZ_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.NeedleZ;
            double target = double.Parse(teNeedleTargetZ.Text);
            
            if (chkNeedleRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.NeedleZ, target, EnumCoordSetType.Absolute);
            }
        }

        //---------------------------- 基板吸嘴轴 点动部分 ------------------------------
        //基板吸嘴轴Z 正向 按下移动
        private void btnSubstratePPJogZPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.SubmountPPZ;
            float speed = float.Parse(teSubstratePPSpeedZ.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.SubmountPPZ, speed);
        }

        //基板吸嘴轴Z 正向 抬起停止
        private void btnSubstratePPJogZPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.SubmountPPZ);
        }

        //基板吸嘴轴Z 反向 按下移动
        private void btnSubstratePPJogZNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.SubmountPPZ;
            float speed = float.Parse(teSubstratePPSpeedZ.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.SubmountPPZ, speed);
        }

        //基板吸嘴轴Z 反向 抬起停止
        private void btnSubstratePPJogZNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.SubmountPPZ);
        }

        //基板吸嘴轴Theta 正向 按下移动
        private void btnSubstratePPJogThetaPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.SubmountPPT;
            float speed = float.Parse(teSubstratePPSpeedTheta.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.SubmountPPT, speed);
        }

        //基板吸嘴轴Theta 正向 抬起停止
        private void btnSubstratePPJogThetaPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.SubmountPPT);
        }

        //基板吸嘴轴Theta 反向 按下移动
        private void btnSubstratePPJogThetaNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.SubmountPPT;
            float speed = float.Parse(teSubstratePPSpeedTheta.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.SubmountPPT, speed);
        }

        //基板吸嘴轴Theta 反向 抬起停止
        private void btnSubstratePPJogThetaNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.SubmountPPT);
        }

        //---------------------------- 基板吸嘴轴 定点MOVE部分 ------------------------------
        //基板吸嘴轴 Z 移动至指定位置
        private void teSubstratePPTMoveZ_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.SubmountPPZ;
            double target = double.Parse(teSubstratePPTargetZ.Text);
            
            if (chkSubstrateRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPZ, target, EnumCoordSetType.Absolute);
            }
        }

        //基板吸嘴轴 Theta 移动至指定位置
        private void teSubstratePPTMoveTheta_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.SubmountPPT;
            double target = double.Parse(teSubstratePPTargetTheta.Text);
            
            if (chkSubstrateRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.SubmountPPT, target, EnumCoordSetType.Absolute);
            }
        }


        //---------------------------- 芯片吸嘴轴 点动部分 ------------------------------
        //芯片吸嘴轴 Theta 正向 按下移动
        private void btnChipPPJogThetaPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.ChipPPT;
            float speed = float.Parse(teChipPPSpeedTheta.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.ChipPPT, speed);
        }

        //芯片吸嘴轴 Theta 正向 抬起停止
        private void btnChipPPJogThetaPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.ChipPPT);
        }

        //芯片吸嘴轴 Theta 反向 按下移动
        private void btnChipPPJogThetaNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.ChipPPT;
            float speed = float.Parse(teChipPPSpeedTheta.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.ChipPPT, speed);
        }

        //芯片吸嘴轴 Theta 反向 抬起停止
        private void btnChipPPJogThetaNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.ChipPPT);
        }

        //---------------------------- 芯片吸嘴轴 定点MOVE部分 ------------------------------
        //芯片吸嘴轴 Theta 移动至指定位置
        private void btnChipPPMoveTheta_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.ChipPPT;
            double target = double.Parse(teChipPPTargetTheta.Text);
            
            if (chkChipRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, target, EnumCoordSetType.Absolute);
            }
        }


        //---------------------------- 晶圆轴 点动部分 ------------------------------
        //晶圆轴 X 正向 按下移动
        private void btnWaferJogXPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableX;
            float speed = float.Parse(teWaferSpeedX.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.WaferTableX, speed);
        }

        //晶圆轴 X 正向 抬起停止
        private void btnWaferJogXPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.WaferTableX);
        }

        //晶圆轴 X 反向 按下移动
        private void btnWaferJogXNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableX;
            float speed = float.Parse(teWaferSpeedX.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.WaferTableX, speed);
        }

        //晶圆轴 Theta 反向 抬起停止
        private void btnWaferJogXNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.WaferTableX);
        }

        //晶圆轴 Y 正向 按下移动
        private void btnWaferJogYPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableY;
            float speed = float.Parse(teWaferSpeedY.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.WaferTableY, speed);
        }

        //晶圆轴 Y 正向 抬起停止
        private void btnWaferJogYPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.WaferTableY);
        }

        //晶圆轴 Y 反向 按下移动
        private void btnWaferJogYNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableY;
            float speed = float.Parse(teWaferSpeedY.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.WaferTableY, speed);
        }

        //晶圆轴 Y 反向 按下移动
        private void btnWaferJogYNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.WaferTableY);
        }

        //---------------------------- 晶圆轴 定点MOVE部分 ------------------------------
        //晶圆轴 X 移动至指定位置
        private void btnWaferMoveX_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableX;
            double target = double.Parse(teWaferTargetX.Text);
            
            if (chkWaferRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableX, target, EnumCoordSetType.Absolute);
            }
        }

        //晶圆轴 Y 移动至指定位置
        private void btnWaferMoveY_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableY;
            double target = double.Parse(teWaferTargetY.Text);
            
            if (chkWaferRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableY, target, EnumCoordSetType.Absolute);
            }
        }

        //晶圆轴 XY两轴联动 移动至指定位置
        private void btnWaferMoveXandY_Click(object sender, EventArgs e)
        {

        }

        //---------------------------- 俯视相机（晶圆相机）轴 点动部分 ------------------------------
        //俯视相机（晶圆相机）轴 Z 正向 按下移动
        private void btnLDCamJogZPos_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableZ;
            float speed = float.Parse(teLDCamSpeedZ.Text.Trim());
            _positionSystem.JogPositive(EnumStageAxis.WaferTableZ, speed);
        }

        //俯视相机（晶圆相机）轴 Z 正向 抬起停止
        private void btnLDCamJogZPos_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogPositive(EnumStageAxis.WaferTableZ);
        }

        //俯视相机（晶圆相机）轴 Z 反向 按下移动
        private void btnLDCamJogZNeg_MouseDown(object sender, MouseEventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableZ;
            float speed = float.Parse(teLDCamSpeedZ.Text.Trim());
            _positionSystem.JogNegative(EnumStageAxis.WaferTableZ, speed);
        }

        //俯视相机（晶圆相机）轴 Z 反向 按下移动
        private void btnLDCamJogZNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _positionSystem.StopJogNegative(EnumStageAxis.WaferTableZ);
        }

        //---------------------------- 俯视相机（晶圆相机）轴 定点MOVE部分 ------------------------------
        //俯视相机（晶圆相机） Z 移动至指定位置
        private void btnLDCamMoveZ_Click(object sender, EventArgs e)
        {
            curActiveAxis = EnumStageAxis.WaferTableZ;
            double target = double.Parse(teLDCamTargetZ.Text);
            
            if (chkLDCamRelative.Checked)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableZ, target, EnumCoordSetType.Relative);
            }
            else
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.WaferTableZ, target, EnumCoordSetType.Absolute);
            }
        }


        //-------------------------------------------------------------- timer -----------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            //teCurActiveAxisName.Text = EnumHelper.GetEnumDescription(curActiveAxis);
            ////获取各轴当前位置值
            //getAllAxisPos();

            //if (curActiveAxis == EnumStageAxis.None)
            //{
            //    return;
            //}

            ////读取各轴实时坐标 
            ////double[] pos = _positionSystem.ReadCurrentStagePosition();
            //double pos = _positionSystem.ReadCurrentSystemPosition(curActiveAxis);

            ////读取轴数据
            //lbAlarmFlag.BackColor = AxisSts(curActiveAxis, 1) ? Color.Red : Color.Lime;
            //lbEnableFlag.BackColor = AxisSts(curActiveAxis, 9) ? Color.Red : Color.Lime;
            //lbPosLimitFlag.BackColor = AxisSts(curActiveAxis, 5) ? Color.Red : Color.Lime;
            //lbNegLimitFlag.BackColor = AxisSts(curActiveAxis, 6) ? Color.Red : Color.Lime;
            //lbPlanMoveFlag.BackColor = AxisSts(curActiveAxis, 10) ? Color.Red : Color.Lime;
            //lbSmoothStopFlag.BackColor = AxisSts(curActiveAxis, 7) ? Color.Red : Color.Lime;
            //lbMotorReadyFlag.BackColor = AxisSts(curActiveAxis, 11) ? Color.Red : Color.Lime;
            //lbEStopFlag.BackColor = AxisSts(curActiveAxis, 8) ? Color.Red : Color.Lime;
        }


        //---------------------------------------------------------------- IO开关 ------------------------------------------------------------------------
        //打开 芯片吸嘴 吸气开关
        private void btnChipDrawOn_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.OpenChipPPVaccum();
        }

        //关闭 芯片吸嘴 吸气开关
        private void btnChipDrawOff_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.CloseChipPPVaccum();
        }

        //---------------------------------------------------------------- 轴状态 ------------------------------------------------------------------------
        /// 读取轴状态
        /// 1 报警
        /// 5 正限位
        /// 6 负限位 
        /// 7 平滑停止 
        /// 8 急停 
        /// 9 使能 
        /// 10 规划运动 
        /// 11 电机到位
        private bool AxisSts(EnumStageAxis axis, short bit)
        {
            if (curActiveAxis == EnumStageAxis.None)
            {
                return false;
            }

            int pSts;
            pSts = _positionSystem.GetAxisState(axis);
            if ((pSts & (1 << bit)) != 0)
            {
                return true;
            }

            return false;
        }

        private void btnClrAlarm_Click(object sender, EventArgs e)
        {
            _positionSystem.ClrAlarm(curActiveAxis);
        }

        private void btnDisableAlarmLimit_Click(object sender, EventArgs e)
        {
            _positionSystem.DisableAlarmLimit(curActiveAxis);
        }

        //获取各轴当前位置值
        private void getAllAxisPos()
        {
            allPos[(int)EnumStageAxis.BondX] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.BondX);
            teBondX.Text = allPos[(int)EnumStageAxis.BondX].ToString();

            allPos[(int)EnumStageAxis.BondY] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.BondY);
            teBondY.Text = allPos[(int)EnumStageAxis.BondY].ToString();

            allPos[(int)EnumStageAxis.BondZ] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
            teBondZ.Text = allPos[(int)EnumStageAxis.BondZ].ToString();

            allPos[(int)EnumStageAxis.ChipPPT] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);
            teChipPPT.Text = allPos[(int)EnumStageAxis.ChipPPT].ToString();

            allPos[(int)EnumStageAxis.ESZ] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.ESZ);
            teNeedleBaseZ.Text = allPos[(int)EnumStageAxis.ESZ].ToString();

            allPos[(int)EnumStageAxis.NeedleZ] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.NeedleZ);
            teNeedleZ.Text = allPos[(int)EnumStageAxis.NeedleZ].ToString();

            allPos[(int)EnumStageAxis.SubmountPPT] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.SubmountPPT);
            teSubstratePPT.Text = allPos[(int)EnumStageAxis.SubmountPPT].ToString();

            allPos[(int)EnumStageAxis.SubmountPPZ] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.SubmountPPZ);
            teSubstratePPZ.Text = allPos[(int)EnumStageAxis.SubmountPPZ].ToString();

            allPos[(int)EnumStageAxis.WaferTableX] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableX);
            teWaferX.Text = allPos[(int)EnumStageAxis.WaferTableX].ToString();

            allPos[(int)EnumStageAxis.WaferTableY] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableY);
            teWaferY.Text = allPos[(int)EnumStageAxis.WaferTableY].ToString();

            allPos[(int)EnumStageAxis.WaferTableZ] = _positionSystem.ReadCurrentStagePosition(EnumStageAxis.WaferTableZ);
            teLookDownCamZ.Text = allPos[(int)EnumStageAxis.WaferTableZ].ToString();
        }

        //红灯开关
        private void btnRedLight_Click(object sender, EventArgs e)
        {
            if(btnRedLight.Appearance.BackColor == Color.LightGray)
            {
                IOUtilityHelper.Instance.TurnonTowerRedLight();
                btnRedLight.Appearance.BackColor = Color.Red;
            }
            else
            {
                IOUtilityHelper.Instance.TurnoffTowerRedLight();
                btnRedLight.Appearance.BackColor = Color.LightGray;
            }
            
        }
        
        //绿灯开关
        private void btnGreenLight_Click(object sender, EventArgs e)
        {
            if (btnGreenLight.Appearance.BackColor == Color.LightGray)
            {
                IOUtilityHelper.Instance.TurnonTowerGreenLight();
                btnGreenLight.Appearance.BackColor = Color.Green;
            }
            else
            {
                IOUtilityHelper.Instance.TurnoffTowerGreenLight();
                btnGreenLight.Appearance.BackColor = Color.LightGray;
            }
        }

        //黄灯开关
        private void btnYellowLight_Click(object sender, EventArgs e)
        {
            if (btnYellowLight.Appearance.BackColor == Color.LightGray)
            {
                IOUtilityHelper.Instance.TurnonTowerYellowLight();
                btnYellowLight.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                IOUtilityHelper.Instance.TurnoffTowerYellowLight();
                btnYellowLight.Appearance.BackColor = Color.LightGray;
            }
        }

        //设备读值
        private void btnReadDeviceValue_Click(object sender, EventArgs e)
        {
            double presssure = ReadPressureValue();

            tePressValue.Text = presssure.ToString();

            double distance = ReadLaserSensor();

            teLaserRangeValue.Text = distance.ToString();

        }

        private double ReadPressureValue()
        {
            try
            {
                if (_DynamometerController.IsConnect)
                {
                    return _DynamometerController.ReadValue();
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        private double ReadLaserSensor()
        {
            try
            {
                double value = 0;

                value = _laserSensor.ReadDistance() / 10000.0;

                return value;
            }
            catch { return -1; }
            
        }




        //读取各个开关状态
        private void getIOStatus()
        {
            /*
            IOUtilityHelper.Instance.CloseChipPPBlow();
            IOUtilityHelper.Instance.CloseChipPPVaccum();
            IOUtilityHelper.Instance.CloseESBaseVaccum();
            IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
            IOUtilityHelper.Instance.CloseNitrogen();
            IOUtilityHelper.Instance.CloseSubmountPPBlow();
            IOUtilityHelper.Instance.CloseSubmountPPVaccum();
            IOUtilityHelper.Instance.OpenWaferTableVaccum();
            */

            int ret = 0;

            //芯片吸嘴真空
            ret = IOUtilityHelper.Instance.GetChipPPVaccumStatus();
            //lbChipPPVacuumStat.BackColor = (ret == 1) ? Color.Lime : Color.LightGray;

            ret = IOUtilityHelper.Instance.GetSubmountPPVaccumStatus();
            //lbChipPPVacuumStat.BackColor = (ret == 1) ? Color.Lime : Color.LightGray;
        }

        //传取开关状态Timer2
        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void btnChangeCurAxis_Click(object sender, EventArgs e)
        {
            curActiveAxis = (EnumStageAxis)cbSelectAxis.SelectedValue;
            //teCurActiveAxisName.Text = cbSelectAxis.GetItemText(cbSelectAxis.Items[cbSelectAxis.SelectedIndex]);
            teNLimit.Text= _stageEngine[curActiveAxis].GetSoftLeftLimit().ToString();
            tePLimit.Text= _stageEngine[curActiveAxis].GetSoftRightLimit().ToString();


        }

        private void btnStageTest_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作提醒", "确认是否进行Stage的循环运动？","提示") == 1)
            {
                _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondZ, 96, EnumCoordSetType.Absolute);
                _positionSystem.SetAxisSpeed(EnumStageAxis.BondX, 30);
                _positionSystem.SetAxisSpeed(EnumStageAxis.BondY, 30);
                EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;

                double[] target1 = new double[2];
                double[] target2 = new double[2];

                target1[0] = 110;
                target1[1] = 265;

                target2[0] = 325;
                target2[1] = 50;
                for (int i = 0; i < 10; i++)
                {
                    _positionSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);
                    Thread.Sleep(1000);
                    _positionSystem.MoveAixsToStageCoord(multiAxis, target2, EnumCoordSetType.Absolute);
                    Thread.Sleep(1000);
                }
                WarningBox.FormShow("完成", "循环运动已完成。", "提示");
            }

        }

        private void btnSubstrateDrawOn_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.OpenSubmountPPVaccum();
        }

        private void btnSubstrateDrawOff_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.CloseSubmountPPVaccum();
        }

        private void btnChipBlowOn_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.OpenChipPPBlow();
        }

        private void btnChipBlowOff_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.CloseChipPPBlow();
        }

        private void btnSubstrateBlowOn_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.OpenSubmountPPBlow();
        }

        private void btnSubstrateBlowOff_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.CloseSubmountPPBlow();
        }

        private void btnBondCamDLOn_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.OpenEutecticPlatformPPVaccum();
        }

        private void btnBondCamDLOff_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.CloseEutecticPlatformPPVaccum();
        }

        private void btnBondCamRLOn_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.OpenMaterailPlatformVaccum();
        }

        private void btnBondCamRLOff_Click(object sender, EventArgs e)
        {
            IOUtilityHelper.Instance.CloseMaterailPlatformVaccum();
        }

        private void btnChipPPMoveThetaCalibration_Click(object sender, EventArgs e)
        {
            CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

            PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
            PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

            PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);

            double angle = double.Parse(teChipPPT.Text);
            double angle0 = double.Parse(teChipPPT0.Text);

            PointF point3 = PPCalibration.PPXYDeviationCal((float)angle0, (float)angle);

            _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, point3.X, EnumCoordSetType.Relative);
            _positionSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, point3.Y, EnumCoordSetType.Relative);

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
    }
}