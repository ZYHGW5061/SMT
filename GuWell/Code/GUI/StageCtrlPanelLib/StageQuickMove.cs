using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using PositioningSystemClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WestDragon.Framework.UtilityHelper;

namespace StageCtrlPanelLib
{
    public partial class StageQuickMove : UserControl
    {
        private object _refreshLockObj = new object();
        protected PositioningSystem _positionSystem
        {
            get
            {
                return PositioningSystem.Instance;
            }
        }
        private System.Windows.Forms.Timer _readPosTimer = new System.Windows.Forms.Timer();
        private SynchronizationContext _syncContext;
        private Point lastMousePosition;
        /// <summary>
        /// 硬件配置处理器
        /// </summary>
        private HardwareConfiguration _hardwareConfig
        {
            get { return HardwareConfiguration.Instance; }
        }
        public StageQuickMove()
        {
            InitializeComponent();
            InitialControl();
            InitializeTool();
            cmbSelectStageSystem.SelectedIndex = 0;
            cmbSelectAxis.SelectedIndex = 0;
            //var joyStickController = JoyStickManager.Instance.GetCurrentController();
            //if(joyStickController!=null)
            //{
            //    if(!joyStickController.ActStageSystemChanged.CheckDelegateRegistered((Action<EnumStageSystem>)StageSystemChanged))
            //    {
            //        joyStickController.ActStageSystemChanged += StageSystemChanged;
            //    }
            //    if (!joyStickController.ActSystemAxisChanged.CheckDelegateRegistered((Action<EnumSystemAxis>)SystemAxisChanged))
            //    {
            //        joyStickController.ActSystemAxisChanged += SystemAxisChanged;
            //    }
            //}
            _readPosTimer.Interval = 500;
            _readPosTimer.Tick += OnTimedEvent;
            _readPosTimer.Stop();
        }

        private void InitializeTool()
        {
            DataModel.Instance.PropertyChanged += DataModel_PropertyChanged;
            _syncContext = SynchronizationContext.Current;
        }


        private void OnTimedEvent(object sender, EventArgs e)
        {
            //UpdateAxisPosition();
        }

        private void StageSystemChanged(EnumStageSystem obj)
        {
            cmbSelectStageSystem.SelectedIndex = (int)obj;
        }
        private void SystemAxisChanged(EnumSystemAxis obj)
        {
            cmbSelectAxis.SelectedIndex= (int)obj;
        }
        private bool _positiveFlag = false;
        public Action<bool> PositiveQucikMoveAct { get; set; }
        public EnumStageSystem SelectedStageSystem { get; set; }
        public EnumSystemAxis SelectedAxisSystem { get; set; }
        EnumStageAxis yAxis = EnumStageAxis.BondY;
        EnumStageAxis xAxis = EnumStageAxis.BondX;
        private void InitialControl()
        {
            
            foreach (var item in Enum.GetValues(typeof(EnumStageSystem)))
            {
                cmbSelectStageSystem.Items.Add(item);
            }
            foreach (var item in Enum.GetValues(typeof(EnumSystemAxis)))
            {
                cmbSelectAxis.Items.Add(item);
            }
        }


        private void btnControlField_Click(object sender, EventArgs e)
        {
            if (_positiveFlag)
            {
                groupBoxControlField.Text = "Position";
                btnControlField.BackColor = Color.Gray;
                _positiveFlag = false;
                _readPosTimer.Stop();
                //JoyStickManager.Instance.GetCurrentController().HandleEnable(false);
                //if (_HCFAPLCController != null)
                //{
                //    _HCFAPLCController.UnsubscribeAxisPosition(AxisPositionChangeAct);
                //}
                //this.btnControlField.MouseMove -= ControlField_MouseMove;
                // 清除鼠标的剪辑区域
                Cursor.Clip = Rectangle.Empty;

                ////JoyStickControl.Instance.JoyStickEnable(false);
                //UnsubscribeIO();
            }
            else
            {
                groupBoxControlField.Text = "快捷移动";
                btnControlField.BackColor = Color.Yellow;

                //_readPosTimer.Start();
                //JoyStickManager.Instance.GetCurrentController().HandleEnable(true);
                //if (_HCFAPLCController != null)
                //{
                //    _HCFAPLCController.SubscribeAxisPosition(AxisPositionChangeAct);
                //}
                //this.btnControlField.MouseMove += ControlField_MouseMove;
                // 获取当前窗口的矩形区域
                var leftupScreenLoc = btnControlField.PointToScreen(new Point(0, 0));
                var rightbottomScreenLoc = btnControlField.PointToScreen(new Point(btnControlField.Width, btnControlField.Height));
                Rectangle formRect = new Rectangle(leftupScreenLoc.X, leftupScreenLoc.Y, btnControlField.Width, btnControlField.Height);
                // 设置鼠标的剪辑区域为窗口区域
                Cursor.Clip = formRect;

                Cursor.Position = new Point(leftupScreenLoc.X + btnControlField.Width / 2, leftupScreenLoc.Y + btnControlField.Height / 2);

                lastMousePosition = new Point(formRect.Width / 2, formRect.Height / 2);

                Thread.Sleep(100);

                _positiveFlag = true;

                //JoyStickControl.Instance.JoyStickEnable(true);
                //SubscribeIO();
            }
        }



        private void ControlField_MouseMove(object sender, MouseEventArgs e)
        {
            //var leftupScreenLoc= btnControlField.PointToScreen(new Point(0,0));
            //var rightbottomScreenLoc= btnControlField.PointToScreen(new Point(btnControlField.Width, btnControlField.Height));
            //Cursor.Position = new Point(leftupScreenLoc.X + btnControlField.Width / 2, leftupScreenLoc.Y + btnControlField.Height / 2);

            if (_positiveFlag)
            {
                //RefreshCurrentXAxis();

                //if (e.Location.X == 0 && e.Location.Y == 0)
                //{
                //    return;
                //}
                //var moveSpeed = 5f;

                //int directFlags_X = 1, directFlags_Y = -1;//X/Y方向的标记位
                //int direction = directFlags_X;

                
                

                //if (xAxis == EnumStageAxis.WaferTableX)
                //{
                //    directFlags_X = -1;
                //}

                //if (yAxis == EnumStageAxis.ESZ || yAxis == EnumStageAxis.NeedleZ || yAxis == EnumStageAxis.BondZ)
                //{
                //    directFlags_Y = 1;
                //}

                //if (Math.Abs(e.Location.X - lastMousePosition.X) < 3)
                //{
                //    if (xAxis == EnumStageAxis.None)
                //    {
                //        return;
                //    }
                //    _positionSystem.StopJogPositive(xAxis);
                //    _positionSystem.StopJogNegative(xAxis);
                //}
                //if (Math.Abs(e.Location.Y - lastMousePosition.Y) < 3)
                //{
                //    if (yAxis == EnumStageAxis.None)
                //    {
                //        return;
                //    }
                //    _positionSystem.StopJogPositive(yAxis);
                //    _positionSystem.StopJogNegative(yAxis);
                //}
                //if (Math.Abs(e.Location.X - lastMousePosition.X) > 2)
                //{
                //    if (xAxis == EnumStageAxis.None)
                //    {
                //        return;
                //    }
                //    if (e.Location.X > lastMousePosition.X)
                //    {
                //        if (directFlags_X == 1)
                //        {
                //            _positionSystem.JogPositive(xAxis, moveSpeed);
                //        }
                //        else
                //        {
                //            _positionSystem.JogNegative(xAxis, moveSpeed);
                //        }
                //    }
                //    else
                //    {
                //        if (directFlags_X == 1)
                //        {
                //            _positionSystem.JogNegative(xAxis, moveSpeed);
                //        }
                //        else
                //        {
                //            _positionSystem.JogPositive(xAxis, moveSpeed);
                //        }
                //    }

                //    var leftupScreenLoc = btnControlField.PointToScreen(new Point(0, 0));
                //    var rightbottomScreenLoc = btnControlField.PointToScreen(new Point(btnControlField.Width, btnControlField.Height));
                //    Cursor.Position = new Point(leftupScreenLoc.X + btnControlField.Width / 2, leftupScreenLoc.Y + btnControlField.Height / 2);

                //}
                //if (Math.Abs(e.Location.Y - lastMousePosition.Y) > 2)
                //{
                //    if (yAxis == EnumStageAxis.None)
                //    {
                //        return;
                //    }
                //    if (e.Location.Y > lastMousePosition.Y)
                //    {
                //        if (directFlags_Y == 1)
                //        {
                //            _positionSystem.JogPositive(yAxis, moveSpeed);
                //        }
                //        else
                //        {
                //            _positionSystem.JogNegative(yAxis, moveSpeed);
                //        }
                //    }
                //    else
                //    {
                //        if (directFlags_Y == 1)
                //        {
                //            _positionSystem.JogNegative(yAxis, moveSpeed);
                //        }
                //        else
                //        {
                //            _positionSystem.JogPositive(yAxis, moveSpeed);
                //        }
                //    }

                //    var leftupScreenLoc = btnControlField.PointToScreen(new Point(0, 0));
                //    var rightbottomScreenLoc = btnControlField.PointToScreen(new Point(btnControlField.Width, btnControlField.Height));
                //    Cursor.Position = new Point(leftupScreenLoc.X + btnControlField.Width / 2, leftupScreenLoc.Y + btnControlField.Height / 2);
                //}



            }

        }
        private void btnControlField_KeyUp(object sender, KeyEventArgs e)
        {
            if (_positiveFlag)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    RefreshCurrentXAxis();
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    {
                        if (xAxis == EnumStageAxis.None)
                        {
                            return;
                        }
                    }
                    if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    {
                        if (yAxis == EnumStageAxis.None)
                        {
                            return;
                        }
                    }
                    int directFlags_X = 1, directFlags_Y = -1;//X/y方向的标记位
                    int direction = directFlags_X;
                    if (xAxis == EnumStageAxis.WaferTableX)
                    {
                        directFlags_X = -1;
                    }

                    if (yAxis == EnumStageAxis.ESZ || yAxis == EnumStageAxis.NeedleZ || yAxis == EnumStageAxis.BondZ)
                    {
                        directFlags_Y = 1;
                    }

                    var axis = EnumStageAxis.BondX;
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    {
                        axis = xAxis;
                        if (e.KeyCode == Keys.Right)
                        {
                            direction = -directFlags_X;
                        }
                        else
                        {
                            direction = directFlags_X;
                        }
                    }
                    else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    {
                        axis = yAxis;
                        if (e.KeyCode == Keys.Up)
                        {
                            direction = -directFlags_Y;
                        }
                        else
                        {
                            direction = directFlags_Y;
                        }
                    }

                    if (direction == 1)
                    {
                        _positionSystem.StopJogPositive(axis);
                    }
                    else
                    {
                        _positionSystem.StopJogNegative(axis);
                    }
                    //UpdateAxisPosition();
                }
            }
        }

        private void btnControlField_KeyDown(object sender, KeyEventArgs e)
        {
            if (_positiveFlag)
            {
                if(e.KeyCode==Keys.Tab)
                {
                    var change = cmbSelectAxis.SelectedIndex + 1;
                    if(change>cmbSelectAxis.Items.Count-1)
                    {
                        change = 0;
                    }
                    cmbSelectAxis.SelectedIndex = change;
                    e.Handled = true;
                    return;
                }
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    RefreshCurrentXAxis();
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    {
                        if(xAxis==EnumStageAxis.None)
                        {
                            return;
                        }
                    }
                    if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    {
                        if (yAxis == EnumStageAxis.None)
                        {
                            return;
                        }
                    }
                    var moveSpeed = 5f;
                    var shift = 10f;
                    var ctrl = 0.02f;

                    int directFlags_X = 1, directFlags_Y = -1;//X/Y方向的标记位
                    int direction = directFlags_X;

                    if (xAxis == EnumStageAxis.WaferTableX)
                    {
                        directFlags_X = -1;
                    }

                    if (yAxis == EnumStageAxis.ESZ || yAxis == EnumStageAxis.NeedleZ || yAxis == EnumStageAxis.BondZ)
                    {
                        directFlags_Y = 1;
                    }
                    if (yAxis == EnumStageAxis.NeedleZ)
                    {
                        moveSpeed = 0.5f;
                    }
                    var axis = EnumStageAxis.BondX;
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    {
                        axis = xAxis;
                        if (e.KeyCode == Keys.Right)
                        {
                            direction = -directFlags_X;
                        }
                        else
                        {
                            direction = directFlags_X;
                        }
                    }
                    else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    {
                        axis = yAxis;
                        if (e.KeyCode == Keys.Up)
                        {
                            direction = -directFlags_Y;
                        }
                        else
                        {
                            direction = directFlags_Y;
                        }
                    }
                    if (axis == EnumStageAxis.ESZ || axis == EnumStageAxis.NeedleZ || axis == EnumStageAxis.BondZ)
                    {
                        moveSpeed = 3f;
                    }
                    if (e.Control)
                    {
                        moveSpeed = moveSpeed * ctrl;
                    }
                    else if (e.Shift)
                    {
                        moveSpeed = moveSpeed * shift;
                    }
                    if (direction == 1)
                    {
                        _positionSystem.JogPositive(axis, moveSpeed);
                    }
                    else
                    {
                        _positionSystem.JogNegative(axis, moveSpeed);
                    }
                }
            }
        }


        private void cmbSelectStageSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedStageSystem = (EnumStageSystem)this.cmbSelectStageSystem.SelectedIndex;
            RefreshCmbSelectAxisItems(SelectedStageSystem);
        }
        private void RefreshCmbSelectAxisItems(EnumStageSystem stageSystem)
        {          
            cmbSelectAxis.Items.Clear();
            switch (stageSystem)
            {
                case EnumStageSystem.BondTable:
                    cmbSelectAxis.Items.Add("XY");
                    cmbSelectAxis.Items.Add("Focus");
                    break;
                case EnumStageSystem.WaferTable:
                    cmbSelectAxis.Items.Add("XY");
                    cmbSelectAxis.Items.Add("Focus");
                    cmbSelectAxis.Items.Add("WaferFilm");
                    break;
                case EnumStageSystem.ChipPP:
                    //cmbSelectAxis.Items.Add("Z");
                    cmbSelectAxis.Items.Add("Focus");
                    cmbSelectAxis.Items.Add("Theta");
                    break;
                case EnumStageSystem.Transport:
                    //cmbSelectAxis.Items.Add("Z");
                    cmbSelectAxis.Items.Add("Stage1");
                    cmbSelectAxis.Items.Add("Stage2");
                    cmbSelectAxis.Items.Add("Stage3");
                    break;
                case EnumStageSystem.WaferCassette:
                    cmbSelectAxis.Items.Add("X");
                    cmbSelectAxis.Items.Add("Z");
                    break;
                case EnumStageSystem.ES:
                    cmbSelectAxis.Items.Add("Z");
                    cmbSelectAxis.Items.Add("NeedleZ");
                    break;
                default:
                    break;
            }
            cmbSelectAxis.SelectedIndex = 0;
        }

        private void cmbSelectAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedAxisSystem = (EnumSystemAxis)Enum.Parse(typeof(EnumSystemAxis),this.cmbSelectAxis.Text);
        }

        private void btnControlField_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }
        private void RefreshCurrentXAxis()
        {
            switch (SelectedStageSystem)
            {
                case EnumStageSystem.BondTable:
                    switch (SelectedAxisSystem)
                    {
                        case EnumSystemAxis.XY:
                            xAxis = EnumStageAxis.BondX;
                            yAxis= EnumStageAxis.BondY;
                            break;
                        case EnumSystemAxis.Focus:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.BondZ;
                            break;
                        default:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.None;
                            break;
                    }
                    break;
                case EnumStageSystem.WaferTable:
                    switch (SelectedAxisSystem)
                    {
                        case EnumSystemAxis.XY:
                            xAxis = EnumStageAxis.WaferTableX;
                            yAxis = EnumStageAxis.WaferTableY;
                            break;
                        case EnumSystemAxis.Focus:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.WaferTableZ;
                            break;
                        case EnumSystemAxis.WaferFilm:
                            xAxis = EnumStageAxis.WaferFilm;
                            yAxis = EnumStageAxis.None;
                            break;
                        default:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.None;
                            break;
                    }
                    break;
                case EnumStageSystem.ChipPP:
                    switch (SelectedAxisSystem)
                    {
                        //case EnumSystemAxis.Z:
                        //    xAxis = EnumStageAxis.None;
                        //    yAxis = EnumStageAxis.ChipPPZ;
                        //    break;
                        case EnumSystemAxis.Focus:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.BondZ;
                            break;
                        case EnumSystemAxis.Theta:
                            xAxis = EnumStageAxis.ChipPPT;
                            yAxis = EnumStageAxis.None;
                            break;
                        default:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.None;
                            break;
                    }
                    break;
                case EnumStageSystem.Transport:
                    switch (SelectedAxisSystem)
                    {
                        case EnumSystemAxis.Stage1:
                            xAxis = EnumStageAxis.TransportTrack1;
                            yAxis = EnumStageAxis.None;
                            break;
                        case EnumSystemAxis.Stage2:
                            xAxis = EnumStageAxis.TransportTrack2;
                            yAxis = EnumStageAxis.None;
                            break;
                        case EnumSystemAxis.Stage3:
                            xAxis = EnumStageAxis.TransportTrack3;
                            yAxis = EnumStageAxis.None;
                            break;
                        default:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.None;
                            break;
                    }
                    break;
                case EnumStageSystem.WaferCassette:
                    switch (SelectedAxisSystem)
                    {
                        case EnumSystemAxis.X:
                            xAxis = EnumStageAxis.WaferFinger;
                            yAxis = EnumStageAxis.None;
                            break;
                        case EnumSystemAxis.Z:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.WaferCassetteLift;
                            break;
                        default:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.None;
                            break;
                    }
                    break;
                case EnumStageSystem.ES:
                    switch (SelectedAxisSystem)
                    {
                        case EnumSystemAxis.Z:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.ESZ;
                            break;
                        case EnumSystemAxis.NeedleZ:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.NeedleZ;
                            break;
                        default:
                            xAxis = EnumStageAxis.None;
                            yAxis = EnumStageAxis.None;
                            break;
                    }
                    break;
                default:
                    xAxis = EnumStageAxis.None;
                    yAxis = EnumStageAxis.None;
                    break;
            }
        }
        private void UpdateAxisPosition()
        {

            if (xAxis == EnumStageAxis.None && yAxis != EnumStageAxis.None)
            {
                ypos=_positionSystem.ReadCurrentStagePosition(yAxis).ToString("0.000");
                this.labelCurrentAxisPosition.Text = $"{ypos}";
            }
            else if (xAxis != EnumStageAxis.None && yAxis == EnumStageAxis.None)
            {
                xpos = _positionSystem.ReadCurrentStagePosition(xAxis).ToString("0.000");
                this.labelCurrentAxisPosition.Text = $"{xpos}";
            }
            else if (xAxis != EnumStageAxis.None && yAxis != EnumStageAxis.None)
            {
                xpos = _positionSystem.ReadCurrentStagePosition(xAxis).ToString("0.000");
                ypos = _positionSystem.ReadCurrentStagePosition(yAxis).ToString("0.000");
                this.labelCurrentAxisPosition.Text = $"{xpos},{ypos}";
            }

        }

        private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_syncContext == null)
            {
                return;
            }
            RefreshCurrentXAxis();
            #region Axis

            if (xAxis == EnumStageAxis.None && yAxis != EnumStageAxis.None)
            {
                if (e.PropertyName == nameof(DataModel.BondY) && yAxis == EnumStageAxis.BondY)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.BondY.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.BondZ) && yAxis == EnumStageAxis.BondZ)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.BondZ.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.WaferTableY) && yAxis == EnumStageAxis.WaferTableY)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.WaferTableY.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.WaferTableZ) && yAxis == EnumStageAxis.WaferTableZ)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.WaferTableZ.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.WaferCassetteLift) && yAxis == EnumStageAxis.WaferCassetteLift)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.WaferCassetteLift.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.ESZ) && yAxis == EnumStageAxis.ESZ)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.ESZ.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.NeedleZ) && yAxis == EnumStageAxis.NeedleZ)
                {
                    _syncContext.Post(_ => {
                        ypos = DataModel.Instance.NeedleZ.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{ypos}";
                    }, null);
                }
            }
            else if (xAxis != EnumStageAxis.None && yAxis == EnumStageAxis.None)
            {
                if (e.PropertyName == nameof(DataModel.BondX) && xAxis == EnumStageAxis.BondX)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.BondX.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.WaferTableX) && yAxis == EnumStageAxis.WaferTableX)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.WaferTableX.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.ChipPPT) && xAxis == EnumStageAxis.ChipPPT)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.ChipPPT.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.WaferFilm) && yAxis == EnumStageAxis.WaferFilm)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.WaferFilm.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.TransportTrack1) && yAxis == EnumStageAxis.TransportTrack1)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.TransportTrack1.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.TransportTrack2) && yAxis == EnumStageAxis.TransportTrack2)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.TransportTrack2.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.TransportTrack3) && yAxis == EnumStageAxis.TransportTrack3)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.TransportTrack3.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
                if (e.PropertyName == nameof(DataModel.WaferFinger) && yAxis == EnumStageAxis.WaferFinger)
                {
                    _syncContext.Post(_ => {
                        xpos = DataModel.Instance.WaferFinger.ToString("0.000");
                        this.labelCurrentAxisPosition.Text = $"{xpos}";
                    }, null);
                }
            }
            else if (xAxis != EnumStageAxis.None && yAxis != EnumStageAxis.None)
            {
                if (e.PropertyName == nameof(DataModel.BondX) && xAxis == EnumStageAxis.BondX)
                {
                    xpos = DataModel.Instance.BondX.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.WaferTableX) && xAxis == EnumStageAxis.WaferTableX)
                {
                    xpos = DataModel.Instance.WaferTableX.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.ChipPPT) && xAxis == EnumStageAxis.ChipPPT)
                {
                    xpos = DataModel.Instance.ChipPPT.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.WaferFilm) && yAxis == EnumStageAxis.WaferFilm)
                {
                    xpos = DataModel.Instance.WaferFilm.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.TransportTrack1) && yAxis == EnumStageAxis.TransportTrack1)
                {
                    xpos = DataModel.Instance.TransportTrack1.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.TransportTrack2) && yAxis == EnumStageAxis.TransportTrack2)
                {
                    xpos = DataModel.Instance.TransportTrack2.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.TransportTrack3) && yAxis == EnumStageAxis.TransportTrack3)
                {
                    xpos = DataModel.Instance.TransportTrack3.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.WaferFinger) && yAxis == EnumStageAxis.WaferFinger)
                {
                    xpos = DataModel.Instance.WaferFinger.ToString("0.000");
                }

                if (e.PropertyName == nameof(DataModel.BondY) && yAxis == EnumStageAxis.BondY)
                {
                    ypos = DataModel.Instance.BondY.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.BondZ) && yAxis == EnumStageAxis.BondZ)
                {
                    ypos = DataModel.Instance.BondZ.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.WaferTableY) && yAxis == EnumStageAxis.WaferTableY)
                {
                    ypos = DataModel.Instance.WaferTableY.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.WaferTableZ) && yAxis == EnumStageAxis.WaferTableZ)
                {
                    ypos = DataModel.Instance.WaferTableZ.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.WaferCassetteLift) && yAxis == EnumStageAxis.WaferCassetteLift)
                {
                    ypos = DataModel.Instance.WaferCassetteLift.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.ESZ) && yAxis == EnumStageAxis.ESZ)
                {
                    ypos = DataModel.Instance.ESZ.ToString("0.000");
                }
                if (e.PropertyName == nameof(DataModel.NeedleZ) && yAxis == EnumStageAxis.NeedleZ)
                {
                    ypos = DataModel.Instance.NeedleZ.ToString("0.000");
                }
                _syncContext.Post(_ => {
                    this.labelCurrentAxisPosition.Text = $"{xpos},{ypos}";
                }, null);
            }
            #endregion


        }


        private void SubscribeIO()
        {
            RefreshCurrentXAxis();
            UpdateAxisPosition();
            switch (xAxis)
            {
                case EnumStageAxis.None:
                    break;
                case EnumStageAxis.BondX:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.BondXPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.BondY:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.BondYPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.BondZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.BondZPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.WaferTableX:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.WaferTableXPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.WaferTableY:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.WaferTableYPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.WaferTableZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.WaferTableZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.ChipPPT:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.ChipTaPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.SubmountPPT:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.SubmountTPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.SubmountPPZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.SubmountZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.ESZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.ESBaseZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.NeedleZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.NeedleZPos", AxisPositionChangeAct);
                    break;
                default:
                    break;
            }
            switch (yAxis)
            {
                case EnumStageAxis.None:
                    break;
                case EnumStageAxis.BondX:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.BondXPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.BondY:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.BondYPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.BondZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.BondZPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.WaferTableX:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.WaferTableXPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.WaferTableY:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.WaferTableYPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.WaferTableZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.WaferTableZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.ChipPPT:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.ChipTaPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.SubmountPPT:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.SubmountTPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.SubmountPPZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.SubmountZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.ESZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.ESBaseZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.NeedleZ:
                    IOManager.Instance.RegisterIOChannelChangedEvent("Stage.NeedleZPos", AxisPositionChangeAct);
                    break;
                default:
                    break;
            }

        }
        private void UnsubscribeIO()
        {
            RefreshCurrentXAxis();
            switch (xAxis)
            {
                case EnumStageAxis.None:
                    break;
                case EnumStageAxis.BondX:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.BondXPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.BondY:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.BondYPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.BondZ:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.BondZPos", AxisPositionChangeAct);

                    break;
                case EnumStageAxis.WaferTableX:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.WaferTableXPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.WaferTableY:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.WaferTableYPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.WaferTableZ:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.WaferTableZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.ChipPPT:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.ChipTPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.SubmountPPT:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.SubmountTPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.SubmountPPZ:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.SubmountZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.ESZ:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.ESBaseZPos", AxisPositionChangeAct);
                    break;
                case EnumStageAxis.NeedleZ:
                    IOManager.Instance.UnregisterIOChannelChangedEvent("Stage.NeedleZPos", AxisPositionChangeAct);
                    break;
                default:
                    break;
            }
        }
        string xpos = "";
        string ypos = "";
        private void AxisPositionChangeAct(string ioName, object preValue, object newValue)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() => AxisPositionChangeAct(ioName, preValue, newValue)));
                return;
            }
            try
            {
                lock (_refreshLockObj)
                {
                    RefreshCurrentXAxis();
                    var changedAxis = IOManager.Instance.GetAxisByIOName(ioName);

                    string pos = "";
                    if (xAxis == EnumStageAxis.None&& yAxis != EnumStageAxis.None)
                    {
                        if (changedAxis == yAxis)
                        {
                            pos = $"{newValue.ToString()}";
                        }
                    }
                    else if(xAxis != EnumStageAxis.None && yAxis == EnumStageAxis.None)
                    {
                        if (changedAxis == xAxis)
                        {
                            pos = $"{newValue.ToString()}";
                        }
                    }
                    else if(xAxis != EnumStageAxis.None && yAxis != EnumStageAxis.None)
                    {
                        if (changedAxis == xAxis)
                        {
                            xpos = $"{newValue.ToString()}";
                        }
                        if (changedAxis == yAxis)
                        {
                            ypos = $"{newValue.ToString()}";
                        }
                        pos = $"{xpos},{ypos}";
                    }
                    else
                    {
                        pos = "";
                    }

                    this.labelCurrentAxisPosition.Text = pos;
                }
            }
            catch (Exception)
            {
            }
        }

        // 用于存储鼠标的上一个位置
        //private Point lastMousePosition;
        private void btnControlField_MouseMove(object sender, MouseEventArgs e)
        {
            if (_positiveFlag)
            {
                var leftupScreenLoc = btnControlField.PointToScreen(new Point(0, 0));
                var controlFieldCenter = new Point(leftupScreenLoc.X + btnControlField.Width / 2, leftupScreenLoc.Y + btnControlField.Height / 2);
                // 获取当前鼠标位置
                Point currentMousePosition = e.Location;
                if (currentMousePosition==lastMousePosition)
                {
                    return;
                }
                if (e.Location == controlFieldCenter)
                {
                    return;
                }
                if (lastMousePosition != Point.Empty)
                {
                    // 判断鼠标移动方向
                    if (currentMousePosition.X > lastMousePosition.X)
                    {
                        if (currentMousePosition.Y > lastMousePosition.Y)
                        {
                            // 向右下移动
                            //MessageBox.Show("鼠标向右下移动");
                        }
                        else if (currentMousePosition.Y < lastMousePosition.Y)
                        {
                            // 向右上移动
                            //MessageBox.Show("鼠标向右上移动");
                        }
                        else
                        {
                            // 向右移动
                            //MessageBox.Show("鼠标向右移动");
                        }
                    }
                    else if (currentMousePosition.X < lastMousePosition.X)
                    {
                        if (currentMousePosition.Y > lastMousePosition.Y)
                        {
                            // 向左下移动
                            //MessageBox.Show("鼠标向左下移动");
                        }
                        else if (currentMousePosition.Y < lastMousePosition.Y)
                        {
                            // 向左上移动
                            //MessageBox.Show("鼠标向左上移动");
                        }
                        else
                        {
                            // 向左移动
                            //MessageBox.Show("鼠标向左移动");
                        }
                    }
                    else
                    {
                        if (currentMousePosition.Y > lastMousePosition.Y)
                        {
                            // 向下移动
                            //MessageBox.Show("鼠标向下移动");
                        }
                        else if (currentMousePosition.Y < lastMousePosition.Y)
                        {
                            // 向上移动
                            //MessageBox.Show("鼠标向上移动");
                        }
                    }
                }
                // 更新上一个鼠标位置
                lastMousePosition = controlFieldCenter;
                Cursor.Position = controlFieldCenter;
            }
        }
        //protected override void Dispose(bool disposing)
        //{

        //    if (disposing)
        //    {
        //        // 释放托管资源
        //        UnsubscribeIO();
        //    }


        //}
    }
}
