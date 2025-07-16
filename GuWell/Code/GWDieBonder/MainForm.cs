using CommonPanelClsLib;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using StageCtrlPanelLib;
using MainGUI.Forms.ProductMenu;
using MainGUI.Forms.SysMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCalibrationClsLib;
using VisionControlAppClsLib;
using VisionGUI;
using PowerControlGUI;
using ControlPanelClsLib;
using MainGUI.Forms;
using PositioningSystemClsLib;
using AlarmManagementClsLib;
using RecipeEditPanelClsLib;
using ControlPanelClsLib.Tools;
using StageManagerClsLib;
using StageControllerClsLib;
using WestDragon.Framework.UtilityHelper;
using VisionClsLib;
using DynamometerGUI;

namespace BondTerminal
{
    public partial class MainForm : BaseForm
    {

        #region File

        CameraWindowForm CameraForm;

        private bool toolStripBtnCameraControlChecked = false;
        private bool CameraWindowGUIInited = false;

        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        private VisionControlAppClsLib.VisualControlManager _VisualManager
        {
            get { return VisionControlAppClsLib.VisualControlManager.Instance; }
        }
        public VisualControlApplications BondCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.BondCamera); }
        }
        public VisualControlApplications UplookingCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.UplookingCamera); }
        }
        public VisualControlApplications WaferCameraVisual
        {
            get { return _VisualManager.GetCameraByID(EnumCameraType.WaferCamera); }
        }

        #endregion
        /// <summary>
        /// 当前日期时间
        /// </summary>
        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _refreshRunTimeSpanTimer = new System.Windows.Forms.Timer();

        public MainForm()
        {
            InitializeComponent();
            GlobalCommFunc.MainForm = this;
            InitializeVisualForm();


        }

        private void InitializeVisualForm()
        {
            
            CameraForm = CameraWindowForm.Instance;
            FrmAlarm alarmFrmInstance = FrmAlarm.Instance;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }        

        private void MenuItemBondHeaderTest_Click(object sender, EventArgs e)
        {
            //BondHeaderTest test = new BondHeaderTest();
            //test.Show();
        }

        private void MenuItemTsTest_Click(object sender, EventArgs e)
        {
            //TSTest test = new TSTest();
            //test.Show();
        }

        private void MenuItemWaferTableTest_Click(object sender, EventArgs e)
        {
            //WaferTableTest test = new WaferTableTest();
            //test.Show();
        }

        private void MenuItemNeedleTest_Click(object sender, EventArgs e)
        {
            //NeedleSysTest test = new NeedleSysTest();
            //test.Show();
        }
        /// <summary>
        /// STAGE
        /// </summary>
        private IStageController _stageEngine
        {
            get { return StageManager.Instance.GetCurrentController(); }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            FrmInitialize startFrom = new FrmInitialize(this);
            if (startFrom.ShowDialog(this) == DialogResult.No)
            {
                Application.Exit();
                return;
            }
            //释放窗体资源
            startFrom.Dispose();
            startFrom = null;

            Login login = new Login();
            if (login.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }
            //释放窗体资源
            login.Dispose();
            login = null;
            if (WarningBox.FormShow("动作确认？", "是否回轴原点？", "提示") == 1)
            {
                try
                {
                    CreateWaitDialog();
                    _stageEngine[EnumStageAxis.ESZ].Home();
                    _stageEngine[EnumStageAxis.NeedleZ].Home();
                    _stageEngine[EnumStageAxis.WaferTableY].Home();
                    _stageEngine[EnumStageAxis.WaferTableX].Home();
                    _stageEngine[EnumStageAxis.WaferTableZ].Home();
                    //_stageEngine[EnumStageAxis.SubmountPPZ].Home();
                    //_stageEngine[EnumStageAxis.SubmountPPT].Home();
                    CloseWaitDialog();
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "轴回原点异常！", ex);
                    CloseWaitDialog();
                }

            }
            this._clockTimer = new System.Windows.Forms.Timer();
            this._clockTimer.Enabled = true;
            //this._clockTimer.SynchronizingObject = this;
            //this._clockTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.ClockTimerElapsedEventHandler);
            this._clockTimer.Tick += new System.EventHandler(this.ClockTimerElapsedEventHandler);
            //定时刷新状态
            this.toolStripStatusLabelRunTime.Text = $"设备已运行：0 分钟";
            //_refreshRunTimeSpanTimer.AutoReset = true;
            _refreshRunTimeSpanTimer.Interval = 60000;
            //_refreshRunTimeSpanTimer.Elapsed += OnTimerElapsedEvt;
            _refreshRunTimeSpanTimer.Tick += OnTimerElapsedEvt;
            _refreshRunTimeSpanTimer.Start();
        }

        /// <summary>
        /// 系统时间更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClockTimerElapsedEventHandler(object sender, System.EventArgs e)
        {
            DateTime time = DateTime.Now;
            this.toolStripStatusLabelNowTime.Text = time.ToShortDateString() +"  "+ time.ToLongTimeString();
        }
        private int _systemRunTimeMin = 0;
        private void OnTimerElapsedEvt(object sender, System.EventArgs e)
        {
            try
            {
                _systemRunTimeMin++;
                this.toolStripStatusLabelRunTime.Text = $"设备已运行：{_systemRunTimeMin} 分钟";
            }
            finally
            {
            }
        }
        private void toolStripBtnStageControl_Click(object sender, EventArgs e)
        {
            FrmStageControl form = (Application.OpenForms["FrmStageControl"]) as FrmStageControl;
            if (form == null)
            {
                form = new FrmStageControl();
                form.Location = this.PointToScreen(new Point(1550, 150));
                form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
            //FrmStageAxisMoveControl form1 = (Application.OpenForms["FrmStageAxisMoveControl"]) as FrmStageAxisMoveControl;
            //if (form1 == null)
            //{
            //    form1 = new FrmStageAxisMoveControl();
            //    form1.Location = this.PointToScreen(new Point(1550, 500));
            //    //form1.ShowLocation(new Point(1550, 600));
            //    form1.Owner = this.FindForm();
            //    form1.Show();
            //}
            //else
            //{
            //    form1.Activate();
            //}
        }


        private void toolStripBtnCameraControl_CheckedChanged(object sender, EventArgs e)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI camera = new CameraWindowGUI();
                camera.InitVisualControl();
                CameraWindowForm.Instance.InitializeWindow(camera);
                int CurrentCameraNum = CameraWindowGUI.Instance.CurrentCameraNum;

                CameraWindowGUI.Instance.Size = new Size(909, 755);
                CameraWindowGUI.Instance.SelectCamera(CurrentCameraNum);
                CameraWindowForm.Instance.Size = new System.Drawing.Size(933, 800);
                CameraWindowForm.Instance.ShowLocation(new Point(100, 200));
                CameraWindowForm.Instance.ControlBox = true;
                CameraForm.Owner = this.FindForm();
                CameraForm.Show();
            }
            else
            {

                CameraWindowGUI camera = new CameraWindowGUI();
                camera.InitVisualControl();
                CameraWindowForm.Instance.InitializeWindow(camera);
                CameraWindowForm.Instance.Size = new System.Drawing.Size(933, 800);
                CameraWindowForm.Instance.ShowLocation(new Point(100, 200));
                CameraWindowForm.Instance.ControlBox = true;
                CameraForm.Owner = this.FindForm();
                CameraForm.Show();

            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                CreateWaitDialog();
                FrmRecipePrograming form = (Application.OpenForms["FrmRecipePrograming"]) as FrmRecipePrograming;
                if (form == null)
                {
                    form = new FrmRecipePrograming();
                    //form.Location = this.PointToScreen(new Point(0, 350));
                    form.Location = new Point(100, 120);
                    form.Owner = this.FindForm();
                    form.Show(this);
                }
                else
                {
                    form.Activate();
                }

                
            }
            catch (Exception)
            {
            }
            finally
            {
                CloseWaitDialog();
            }
        }

        private void 自动校准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCalibration.Instance.AutoRun();
        }

        private void 系统初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_systemConfig.SaveConfig();
            //HardwareConfiguration.Instance.SaveConfig();
            //SystemCalibration.Instance.Initialization();
        }

        private void toolStripBtnLightControl_CheckedChanged(object sender, EventArgs e)
        {
            //if (toolStripBtnLightControl.Checked)
            //{
            //    string name = "榜头相机识别";
            //    string title = "";
            //    VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();

            //    visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            //    MatchIdentificationParam param = new MatchIdentificationParam();
            //    try
            //    {
            //        int REF = -1;
            //        using (VisualControlForm VForm = VisualControlForm.Instance)
            //        {
            //            VForm.InitializeGui(visualMatch);

            //            string hh = VForm.showMessage(Name, title, true);
            //            if (hh == "next")
            //            {
            //                REF = 1;
            //            }
            //            else
            //            {
            //                REF = 0;
            //            }
            //        }
            //        return REF;
            //    }
            //    catch
            //    {
            //        return -1;
            //    }
            //}
        }

        private void 自动生产ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //ProductRunForm form = new ProductRunForm();
            //form.Show();

            ProductRunForm form = (Application.OpenForms["ProductRunForm"]) as ProductRunForm;
            if (form == null)
            {
                form = new ProductRunForm();
                //form.Location = this.PointToScreen(new Point(1550, 150));
                form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                //SingleStepRunUtility.Instance.Continue();
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            IOTestForm ioTestForm = new IOTestForm();
            ioTestForm.Show();
        }

        private void iO测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOTestForm ioTestForm = new IOTestForm();
            ioTestForm.Show();
        }

        private void toolStripBtnCameraControl_Click(object sender, EventArgs e)
        {

        }

        private void toolStripBtnLightControl_Click(object sender, EventArgs e)
        {
            FrmLightControl form = (Application.OpenForms["FrmLightControl"]) as FrmLightControl;
            if (form == null)
            {
                form = new FrmLightControl();
                form.Location = this.PointToScreen(new Point(1250, 150));
                form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (WarningBox.FormShow("确认关闭？", "确认退出软件？", "提示") == 0)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        SystemConfiguration.Instance.SaveConfig();
                        e.Cancel = false;
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void 共晶台测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PowerControlForm form = (Application.OpenForms["PowerControl"]) as PowerControlForm;
            if (form == null)
            {
                form = new PowerControlForm();
                form.Location = this.PointToScreen(new Point(700, 150));
                form.ShowLocation(new Point(700, 150));
                form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void 单步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmSingleStepRun form = new FrmSingleStepRun();
            FrmSingleStepRun form = (Application.OpenForms["FrmSingleStepRun"]) as FrmSingleStepRun;
            if (form == null)
            {
                form = new FrmSingleStepRun();
                //form.Location = this.PointToScreen(new Point(1550, 150));
                form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void pP工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPPTool frm = new FrmPPTool();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void 顶针工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEjectionSystemTool frm = new FrmEjectionSystemTool();
            frm.ShowDialog();
            frm.Dispose();

        }

        private void 运动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStageMaintain form = (Application.OpenForms["FrmStageMaintain"]) as FrmStageMaintain;
            if (form == null)
            {
                form = new FrmStageMaintain();
                form.Location = this.PointToScreen(new Point(500, 500));
                form.Owner = this.FindForm();
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void iOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmIOMaintain form = (Application.OpenForms["FrmIOMaintain"]) as FrmIOMaintain;
            if (form == null)
            {
                form = new FrmIOMaintain();
                form.Location = this.PointToScreen(new Point(600, 500));
                form.Owner = this.FindForm();
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void 学习ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCalibration.Instance.ManualRun(2);
        }

        private void 半自动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCalibration.Instance.ManualRun(1);
        }

        private void chip吸嘴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCalibration.Instance.ChipRun("UC");
        }
        
        private void submount吸嘴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SystemCalibration.Instance.SubmountRun();
        }

        private void 创建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BMCProcess.Instance.CreationProcess();
        }

        private void 运行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BMCProcess.Instance.Run(_systemConfig.SystemCalibrationConfig.BMCtimes, _systemConfig.SystemCalibrationConfig.BMCdelaytime);
        }

        private void 系统配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParameterConfigForm form = new ParameterConfigForm();
            form.ShowDialog();
            form.Dispose();
        }

        private void tsbtnStandby_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "是否移动到安全位？", "提示") == 1)
            {
                PositioningSystem.Instance.BondMovetoSafeLocation();
            }
        }

        private void toolStripBtnLogout_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认", "确认登出？ ", "提示") == 1)
            {
                //SwitchToMainFunctionalArea();
                Login login = new Login();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    //UpdateAccessLevel();
                }
                else
                {
                    Application.Exit();
                    return;
                }
                //释放窗体资源
                login.Dispose();
            }
        }

        private void toolStripBtnAlarm_Click(object sender, EventArgs e)
        {
            //FrmSingleStepRun form = new FrmSingleStepRun();
            FrmAlarm form = (Application.OpenForms["FrmAlarm"]) as FrmAlarm;
            if (form == null)
            {
                form = FrmAlarm.Instance;
                form.Owner = this.FindForm();
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void 共晶台校准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCalibration.Instance.EutecticWeldingRun();
        }

        private void 单点控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOTestForm ioTestForm = new IOTestForm();
            ioTestForm.Show();
        }

        private void 点胶工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEpoxtTool form = (Application.OpenForms["FrmEpoxtTool"]) as FrmEpoxtTool;
            if (form == null)
            {
                form = new FrmEpoxtTool();
                form.Location = this.PointToScreen(new Point(500, 500));
                form.Owner = this.FindForm();
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void 手动创建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BMCProcess.Instance.CreationProcess(2);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripBtnHome_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("动作确认？", "是否进行系统初始化？", "提示") == 1)
            {
                try
                {
                    CreateWaitDialog();
                    StageManager.Instance.GetCurrentController().Home();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    CloseWaitDialog();
                }

            }
        }

        private void test按钮ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOTestForm form = new IOTestForm();
            form.Show();
            //int mode = 1;
            //if (mode == 0)
            //{
            //    string name = "榜头相机识别";
            //    string title = "";
            //    VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            //    visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            //    BondCameraVisual.SetLightintensity(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

            //    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

            //    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);
            //}
            //else if (mode == 1)
            //{
            //    string name = "仰视相机识别";
            //    string title = "";
            //    VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            //    visualMatch.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);

            //    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

            //    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);
            //}
            //else if (mode == 2)
            //{
            //    string name = "晶圆相机识别";
            //    string title = "";
            //    VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            //    visualMatch.InitVisualControl(CameraWindowGUI.Instance, WaferCameraVisual);

            //    visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);

            //    int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);
            //}

        }

        private void t轴校准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //吸嘴旋转补偿
            CalibrationAlgorithms PPCalibration = new CalibrationAlgorithms();

            PointF point1 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y);
            PointF point2 = new PointF((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X, (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y);

            PointF point4 = new PointF(((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.X - (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.X),((float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1.Y - (float)_systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2.Y));

            PPCalibration.PPRotateXYDeviationParamCal(point1, point2, 0, 180);


            double Angle = PositioningSystem.Instance.ReadCurrentStagePosition(EnumStageAxis.ChipPPT);

            if (Math.Abs(Angle) > 0.1)
            {
                //PositioningSystem.Instance.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, -Angle, EnumCoordSetType.Absolute);

                PointF point3 = PPCalibration.PPXYDeviationCal((float)0, (float)-Angle);
                XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
                EnumStageAxis[] multiAxis = new EnumStageAxis[2];
                multiAxis[0] = EnumStageAxis.BondX;
                multiAxis[1] = EnumStageAxis.BondY;

                double[] target1 = new double[2];
                target1[0] = point3.X;
                target1[1] = point3.Y;

                PositioningSystem.Instance.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Relative);
            }


           
        }

        private void 校验工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCheck frm = new FrmCheck();
            frm.Show();
        }

        private void 校准台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCalibrationTable frm = new FrmCalibrationTable();
            frm.ShowDialog();
        }

        private void 压力校准工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PressureCurveForm form = (Application.OpenForms["PressureCurveForm"]) as PressureCurveForm;
            if (form == null)
            {
                form = new PressureCurveForm();
                form.Location = this.PointToScreen(new Point(500, 500));
                form.Owner = this.FindForm();
                form.Show(this);
            }
            else
            {
                form.Activate();
            }
        }

        private void 运行ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZRProcess.Instance.Run(10, 2000);
        }

        private void 到安全位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZRProcess.Instance.BondToSafeAsync();
        }

        private void 到测力位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZRProcess.Instance.BondToPressureTableAsync();
        }

        //private void testToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    int mode = 0;
        //    if(mode == 0)
        //    {
        //        string name = "榜头相机识别";
        //        string title = "";
        //        VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
        //        visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

        //        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBMCMatch);

        //        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);
        //    }
        //    else if(mode == 1)
        //    {
        //        string name = "仰视相机识别";
        //        string title = "";
        //        VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
        //        visualMatch.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);

        //        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBMCMatch);

        //        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);
        //    }
        //    else if (mode == 2)
        //    {
        //        string name = "晶圆相机识别";
        //        string title = "";
        //        VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
        //        visualMatch.InitVisualControl(CameraWindowGUI.Instance, WaferCameraVisual);

        //        visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);

        //        int Done = SystemCalibration.Instance.ShowVisualForm(visualMatch, name, title);
        //    }


        //}
    }
}
