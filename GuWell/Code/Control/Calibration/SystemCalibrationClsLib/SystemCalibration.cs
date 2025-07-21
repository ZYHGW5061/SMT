using BoardCardControllerClsLib;
using CameraControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using DynamometerGUI;
using DynamometerManagerClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using HardwareManagerClsLib;
using LaserSensorControllerClsLib;
using LaserSensorManagerClsLib;
using PositioningSystemClsLib;
using StageControllerClsLib;
using StageCtrlPanelLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionClsLib;
using VisionControlAppClsLib;
using VisionGUI;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace SystemCalibrationClsLib
{
    public class SystemCalibration
    {

        #region Private File

        private static readonly object _lockObj = new object();
        private static volatile SystemCalibration _instance = null;
        public static SystemCalibration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new SystemCalibration();
                        }
                    }
                }
                return _instance;
            }
        }

        private SystemCalibration()
        {
            _boardCardController = BoardCardManager.Instance.GetCurrentController();
            config = _systemConfig.PositioningConfig;
        }

        private IBoardCardController _boardCardController;

        private StageCore stage = StageControllerClsLib.StageCore.Instance;
        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }

        private HardwareConfiguration _hardware
        {
            get { return HardwareConfiguration.Instance; }
        }
        private CameraConfig _BondcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.BondCamera); }
        }
        private CameraConfig _UplookingcameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.UplookingCamera); }
        }
        private CameraConfig _WafercameraConfig
        {
            get { return CameraManager.Instance.GetCameraConfigByID(EnumCameraType.WaferCamera); }
        }
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

        private ILaserSensorController _laserSensor
        {
            get { return HardwareManager.Instance.LaserSensor; }
        }

        private DynamometerDisplayForm Dynamometerform;



        VisualControlForm VForm;

        #endregion

        #region Public File

        public PositioningConfig config;

        /// <summary>
        /// 贴片机模式：0 共晶贴片机 1 点胶贴片机
        /// </summary>
        public int DeviceMode { get; set; } = 1;

        public double BondZOffset { get; set; } = 15;

        public double BondZOffset1 { get; set; } = 5;

        public double Zmin { get; set; } = 116;

        public double Zmax { get; set; } = 144;

        public double AutofocusRange { get; set; } = 1.5;

        public double AutofocusInterval { get; set; } = 0.3;

        public double AutoHeightRange { get; set; } = 1.5;

        public double AutoHeightInterval { get; set; } = -0.5;


        public float Score { get; set; }
        public int MinAngle { get; set; }
        public int MaxAngle { get; set; }

        public PPToolSettings currentppTool { get; set; }


        #endregion

        #region Private Mothed

        private static (double, double) ImageToXY(PointF point, PointF center, float pixelsizex, float pixelsizey)
        {
            double X = (point.X - center.X) * pixelsizex;

            double Y = (point.Y - center.Y) * pixelsizey;

            return (X, Y);

        }

        public static List<(double x, double y)> GetRectangleCoordinates(int rows, int columns, double width, double height)
        {
            List<(double x, double y)> coordinates = new List<(double x, double y)>();

            // Calculate the width and height of each subdivided rectangle
            double cellWidth = width / (columns - 1);
            double cellHeight = height / (rows - 1);

            // Calculate the starting point (top-left corner of the rectangle)
            double startX = -width / 2;
            double startY = height / 2;

            // Calculate all coordinates
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    double x = startX + col * cellWidth;
                    double y = startY - row * cellHeight;
                    coordinates.Add((x, y));
                }
            }

            return coordinates;
        }

        public static (double pixelsizeX, double pixelsizeY, double angle) CalculateCameraParameters(
        List<(double xs, double ys)> XYPosition,
        List<(double pixelx, double pixely)> XYPixelx,
        int Row, int Col)
        {
            if (XYPosition.Count < 2 || XYPixelx.Count < 2)
            {
                throw new ArgumentException("There must be at least two points in each list.");
            }

            int NanX = 0;
            int NanY = 0;

            int count = XYPosition.Count;
            double totalPixelSizeX = 0;
            double totalPixelSizeY = 0;
            double totalAngle = 0;

            double deltaXActual = XYPosition[0].xs - XYPosition[(Row - 1) + 0 * Row].xs;
            double deltaYActual = XYPosition[0].ys - XYPosition[(Row - 1) + 0 * Row].ys;

            double deltaXPixel = XYPixelx[0].pixelx - XYPixelx[(Row - 1) + 0 * Row].pixelx;
            double deltaYPixel = XYPixelx[0].pixely - XYPixelx[(Row - 1) + 0 * Row].pixely;

            double angleActual = Math.Atan2(Math.Abs(deltaYActual), Math.Abs(deltaXActual));
            double anglePixel = Math.Atan2(Math.Abs(deltaYPixel), Math.Abs(deltaXPixel));
            double angle = anglePixel - angleActual;

            double deltaXActual1 = XYPosition[(Row - 1) + 0 * Row].xs - XYPosition[(Row - 1) + (Col - 1) * Row].xs;
            double deltaYActual1 = XYPosition[(Row - 1) + 0 * Row].ys - XYPosition[(Row - 1) + (Col - 1) * Row].ys;

            double deltaXPixel1 = XYPixelx[(Row - 1) + 0 * Row].pixelx - XYPixelx[(Row - 1) + (Col - 1) * Row].pixelx;
            double deltaYPixel1 = XYPixelx[(Row - 1) + 0 * Row].pixely - XYPixelx[(Row - 1) + (Col - 1) * Row].pixely;


            //for (int i = 0; i < count - 1; i++)
            //{
            //    // Calculate the differences in actual coordinates and pixel coordinates
            //    double deltaXActual = XYPosition[i + 1].xs - XYPosition[i].xs;
            //    double deltaYActual = XYPosition[i + 1].ys - XYPosition[i].ys;
            //    double deltaXPixel = XYPixelx[i + 1].pixelx - XYPixelx[i].pixelx;
            //    double deltaYPixel = XYPixelx[i + 1].pixely - XYPixelx[i].pixely;

            //    // Calculate pixelsizeX and pixelsizeY
            //    double pixelsizeX = (Math.Abs(deltaXActual / deltaXPixel));
            //    double pixelsizeY = (Math.Abs(deltaYActual / deltaYPixel));

            //    // Calculate the angle between the actual coordinates and the pixel coordinates
            //    double angleActual = Math.Atan2(deltaYActual, deltaXActual);
            //    double anglePixel = Math.Atan2(deltaYPixel, deltaXPixel);
            //    double angle = anglePixel - angleActual;

            //    // Convert angle to degrees
            //    angle = angle * (180 / Math.PI);

            //    if (double.IsNaN(pixelsizeX))
            //    {
            //        pixelsizeX = 0;
            //        NanX++;
            //    }
            //    if (double.IsNaN(pixelsizeY))
            //    {
            //        pixelsizeY = 0;
            //        NanY++;
            //    }

            //    totalPixelSizeX += pixelsizeX;
            //    totalPixelSizeY += pixelsizeY;
            //    totalAngle += angle;
            //}

            //double averagePixelSizeX = totalPixelSizeX / (count - NanX - 1);
            //double averagePixelSizeY = totalPixelSizeY / (count - NanY - 1);
            //double averageAngle = totalAngle / (count - 1);

            double averagePixelSizeX = Math.Abs(deltaXActual / deltaXPixel);
            double averagePixelSizeY = Math.Abs(deltaYActual1 / deltaYPixel1);
            double averageAngle = angle;

            return (averagePixelSizeX, averagePixelSizeY, averageAngle);
        }

        public int ShowVisualForm(UserControl visualControlGUI, string Name, string title)
        {
            int result = -1;
            var formReadyEvent = new ManualResetEvent(false);

            var tcs = new TaskCompletionSource<int>();
            VisualControlForm VForm = new VisualControlForm();

            VForm.OnButtonClicked += (buttonResult) =>
            {
                result = buttonResult == "confirm" ? 1 : 0;
                formReadyEvent.Set();
            };



            VForm.InitializeGui(visualControlGUI);

            VForm.Location = new Point(1550, 150);

            VForm.FormShow(Name, title, true, tcs.SetResult, new Point(1550, 150));

            while (!formReadyEvent.WaitOne(100))
            {
                Application.DoEvents();
            }


            return result;

        }

        private void ShowCameraForm(bool show)
        {
            if (show)
            {
                CameraWindowForm.Instance.Show();
            }
            else
            {
                CameraWindowForm.Instance.Hide();
            }
        }

        /// <summary>
        /// 模态
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        private int ShowMessage2(string title, string content, string type)
        {
            int result = -1;
            var formReadyEvent = new ManualResetEvent(false);
            //WarningBox1.FormShow(title, content, type);
            MessageBox1 myMessageBox1 = new MessageBox1();
            myMessageBox1.OnButtonClicked += (buttonResult) =>
            {
                result = buttonResult == "confirm" ? 1 : 0;
                formReadyEvent.Set();
            };
            myMessageBox1.showMessage(title, content, type);

            while (!formReadyEvent.WaitOne(100))
            {
                Application.DoEvents();
            }

            return result;
        }
        /// <summary>
        /// 非模态
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int ShowMessageAsync(string title, string content, string type)
        {


            return ShowMessage2(title, content, type);




        }

        private void ShowStage()
        {

            FrmStageControl form = (Application.OpenForms["FrmStageControl"]) as FrmStageControl;
            if (form == null)
            {
                form = new FrmStageControl();
                form.Location = new Point(1550, 200);
                //form.Location = this.PointToScreen(new Point(1550, 150));
                //form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            else
            {
                form.Activate();
            }
        }

        private void CloseStage()
        {
            try
            {
                FrmStageControl form = Application.OpenForms.OfType<FrmStageControl>().FirstOrDefault();
                if (form != null)
                {
                    form.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭轴点动窗口时发生错误: " + ex.Message);
            }
        }

        private void ShowStageAxisMove()
        {
            FrmStageAxisMoveControl form = (Application.OpenForms["FrmStageAxisMoveControl"]) as FrmStageAxisMoveControl;
            if (form == null)
            {
                form = new FrmStageAxisMoveControl();
                form.Location = new Point(1550, 550);
                form.ShowLocation(new Point(1550, 600));
                //form.Location = this.PointToScreen(new Point(1550, 150));
                //form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            else
            {
                form.Activate();
            }
        }

        private void CloseStageAxisMove()
        {
            try
            {
                FrmStageAxisMoveControl form = Application.OpenForms.OfType<FrmStageAxisMoveControl>().FirstOrDefault();
                if (form != null)
                {
                    form.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭轴移动窗口时发生错误: " + ex.Message);
            }
        }

        private double ReadLaserSensor()
        {
            double value = 0;

            value = _laserSensor.ReadDistance() / 10000.0;
            if(value == -0.001)
            {
                Thread.Sleep(50);
                value = _laserSensor.ReadDistance() / 10000.0;
                Thread.Sleep(50);
                value = _laserSensor.ReadDistance() / 10000.0;
            }

            return value;
        }


        private void ShowDynamometer()
        {
            DynamometerDisplayForm form = (Application.OpenForms["DynamometerDisplayForm"]) as DynamometerDisplayForm;
            if (form == null)
            {
                form = new DynamometerDisplayForm();
                form.Location = new Point(1550, 100);
                form.ShowLocation(new Point(1550, 100));
                form.StartCollect();
                //form.Location = this.PointToScreen(new Point(1550, 150));
                //form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            else
            {
                form.Activate();
            }
        }

        private void CloseDynamometer()
        {
            try
            {
                DynamometerDisplayForm form = Application.OpenForms.OfType<DynamometerDisplayForm>().FirstOrDefault();
                if (form != null)
                {
                    form.StopCollect();
                    form.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭测力窗口时发生错误: " + ex.Message);
            }
        }

        private void ShowDynamometer2()
        {
            Dynamometerform = (Application.OpenForms["DynamometerDisplayForm2"]) as DynamometerDisplayForm;
            if (Dynamometerform == null)
            {
                Dynamometerform = new DynamometerDisplayForm();
                Dynamometerform.Location = new Point(1550, 100);
                Dynamometerform.ShowLocation(new Point(1550, 100));
                //form.StartCollect();
                //form.Location = this.PointToScreen(new Point(1550, 150));
                //form.Owner = this.FindForm();
                //lightform.StartPosition = FormStartPosition.CenterScreen;
                Dynamometerform.Show();
            }
            else
            {
                Dynamometerform.Activate();
            }
        }

        private void CloseDynamometer2()
        {
            try
            {
                Dynamometerform = Application.OpenForms.OfType<DynamometerDisplayForm>().FirstOrDefault();
                if (Dynamometerform != null)
                {
                    //form.StopCollect();
                    Dynamometerform.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭测力窗口时发生错误: " + ex.Message);
            }
        }


        private (double, double, double, bool) ShowCameraParamForm(string text3 = "Bond相机参数设置", double ImageWidthPixelsize = 1, double ImageHeightPixelsize = 1, double Angle = 0)
        {
            return CameraParamBox.FormShow(text3, ImageWidthPixelsize, ImageHeightPixelsize, Angle);
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

            _positioningSystem.MoveAixsToStageCoord(multiAxis, target1, EnumCoordSetType.Absolute);
        }

        private void AxisAbsoluteMove(EnumStageAxis axis, double target)
        {
            stage.ClrAlarm(axis);
            stage.AbloluteMoveSync(axis, target);
        }

        private void AxisRelativeMove(EnumStageAxis axis, double target)
        {
            stage.RelativeMoveSync(axis, (float)target);
        }

        private double ReadCurrentAxisposition(EnumStageAxis axis)
        {
            if((int)axis > 19)
            {
                return -1;
            }
            double position = stage.GetCurrentPosition(axis);
            //double position = 2;
            return position;
        }

        private void AxisHome(EnumStageAxis axis)
        {
            //stage.Home(axis);
        }



        //流程

        public bool InitCamera()
        {
            try
            {
                BondCameraVisual.SetOneMode();
                WaferCameraVisual.SetOneMode();
                UplookingCameraVisual.SetOneMode();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="range"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        private bool AutofocusAsync(EnumCameraType camera, double range, double interval, MatchIdentificationParam param)
        {
            bool Done = false;

            if (camera == EnumCameraType.BondCamera)
            {
                AxisRelativeMove(EnumStageAxis.BondZ, range / 2);

                Dictionary<double, double> data = new Dictionary<double, double>();


                for (int i = 0; i < (int)(range / interval); i++)
                {
                    AxisRelativeMove(EnumStageAxis.BondZ, -interval);

                    Bitmap bitmap = BondCameraVisual.GetBitmap();
                    Bitmap Showbitmap = new Bitmap(bitmap);
                    Bitmap Showbitmap1 = VisualAlgorithms.DeepClone(Showbitmap);

                    Rectangle rectangle = new Rectangle( (int)param.TemplateRoi.X, (int)param.TemplateRoi.Y, (int)param.TemplateRoi.Width, (int)param.TemplateRoi.Height);

                    double Num = VisualAlgorithms.CalculateImageSharpness(Showbitmap1, rectangle);

                    double position = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(Num.ToString());
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.text, true);
                    }));

                    data.Add(Num, position);

                    //bitmap.Dispose();
                    //Showbitmap.Dispose();
                    //Showbitmap1.Dispose();

                }

                var maxKey = data.Keys.Max();

                var currentlocation = data[maxKey];

                AxisAbsoluteMove(EnumStageAxis.BondZ, currentlocation + interval);

                for (int i = 0; i < (int)(2 * interval / (interval / 10)); i++)
                {
                    AxisRelativeMove(EnumStageAxis.BondZ, -interval / 10);

                    Bitmap bitmap = BondCameraVisual.GetBitmap();

                    Bitmap Showbitmap = new Bitmap(bitmap);
                    Bitmap Showbitmap1 = VisualAlgorithms.DeepClone(Showbitmap);

                    Rectangle rectangle = new Rectangle((int)param.TemplateRoi.X, (int)param.TemplateRoi.Y, (int)param.TemplateRoi.Width, (int)param.TemplateRoi.Height);

                    double Num = VisualAlgorithms.CalculateImageSharpness(Showbitmap1, rectangle);

                    double position = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(Num.ToString());
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.text, true);
                    }));

                    data.Add(Num, position);

                    //bitmap.Dispose();
                    //Showbitmap.Dispose();
                    //Showbitmap1.Dispose();

                }

                maxKey = data.Keys.Max();

                currentlocation = data[maxKey];

                AxisAbsoluteMove(EnumStageAxis.BondZ, currentlocation);

                Done = true;
            }

            if (camera == EnumCameraType.UplookingCamera)
            {
                AxisRelativeMove(EnumStageAxis.BondZ, range / 2);

                Dictionary<double, double> data = new Dictionary<double, double>();


                for (int i = 0; i < (int)(range / interval); i++)
                {
                    AxisRelativeMove(EnumStageAxis.BondZ, -interval);

                    Bitmap bitmap = UplookingCameraVisual.GetBitmap();

                    Bitmap Showbitmap = new Bitmap(bitmap);

                    Bitmap Showbitmap1 = VisualAlgorithms.DeepClone(Showbitmap);

                    Rectangle rectangle = new Rectangle((int)param.TemplateRoi.X, (int)param.TemplateRoi.Y, (int)param.TemplateRoi.Width, (int)param.TemplateRoi.Height);

                    double Num = VisualAlgorithms.CalculateImageSharpness(Showbitmap1, rectangle);

                    double position = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(Num.ToString());
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.text, true);
                    }));

                    data.Add(Num, position);


                    //bitmap.Dispose();
                    //Showbitmap.Dispose();
                    //Showbitmap1.Dispose();

                }

                var maxKey = data.Keys.Max();

                var currentlocation = data[maxKey];

                AxisAbsoluteMove(EnumStageAxis.BondZ, currentlocation + interval);

                for (int i = 0; i < (int)(2 * interval / (interval / 10)); i++)
                {
                    AxisRelativeMove(EnumStageAxis.BondZ, -interval / 10);

                    Bitmap bitmap = UplookingCameraVisual.GetBitmap();

                    Bitmap Showbitmap = new Bitmap(bitmap);

                    Bitmap Showbitmap1 = VisualAlgorithms.DeepClone(Showbitmap);

                    Rectangle rectangle = new Rectangle((int)param.TemplateRoi.X, (int)param.TemplateRoi.Y, (int)param.TemplateRoi.Width, (int)param.TemplateRoi.Height);

                    double Num = VisualAlgorithms.CalculateImageSharpness(Showbitmap1, rectangle);

                    double position = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(Num.ToString());
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.text, true);
                    }));

                    data.Add(Num, position);

                    //bitmap.Dispose();
                    //Showbitmap.Dispose();
                    //Showbitmap1.Dispose();

                }

                maxKey = data.Keys.Max();

                currentlocation = data[maxKey];

                AxisAbsoluteMove(EnumStageAxis.BondZ, currentlocation);

                Done = true;
            }

            if (camera == EnumCameraType.WaferCamera)
            {
                AxisRelativeMove(EnumStageAxis.WaferTableZ, range / 2);

                Dictionary<double, double> data = new Dictionary<double, double>();


                for (int i = 0; i < (int)(range / interval); i++)
                {
                    AxisRelativeMove(EnumStageAxis.WaferTableZ, -interval);

                    Bitmap bitmap = WaferCameraVisual.GetBitmap();

                    Bitmap Showbitmap = new Bitmap(bitmap);

                    Bitmap Showbitmap1 = VisualAlgorithms.DeepClone(Showbitmap);

                    Rectangle rectangle = new Rectangle((int)param.TemplateRoi.X, (int)param.TemplateRoi.Y, (int)param.TemplateRoi.Width, (int)param.TemplateRoi.Height);

                    double Num = VisualAlgorithms.CalculateImageSharpness(Showbitmap1, rectangle);

                    double position = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(Num.ToString());
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.text, true);
                    }));

                    data.Add(Num, position);

                    //bitmap.Dispose();
                    //Showbitmap.Dispose();
                    //Showbitmap1.Dispose();

                }

                var maxKey = data.Keys.Max();

                var currentlocation = data[maxKey];

                AxisAbsoluteMove(EnumStageAxis.WaferTableZ, currentlocation + interval);

                for (int i = 0; i < (int)(2 * interval / (interval / 10)); i++)
                {
                    AxisRelativeMove(EnumStageAxis.WaferTableZ, -interval / 10);

                    Bitmap bitmap = WaferCameraVisual.GetBitmap();

                    Bitmap Showbitmap = new Bitmap(bitmap);

                    Bitmap Showbitmap1 = VisualAlgorithms.DeepClone(Showbitmap);

                    Rectangle rectangle = new Rectangle((int)param.TemplateRoi.X, (int)param.TemplateRoi.Y, (int)param.TemplateRoi.Width, (int)param.TemplateRoi.Height);

                    double Num = VisualAlgorithms.CalculateImageSharpness(Showbitmap1, rectangle);

                    double position = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(Num.ToString());
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.text, true);
                    }));

                    data.Add(Num, position);

                    //bitmap.Dispose();
                    //Showbitmap.Dispose();
                    //Showbitmap1.Dispose();

                }

                maxKey = data.Keys.Max();

                currentlocation = data[maxKey];

                AxisAbsoluteMove(EnumStageAxis.WaferTableZ, currentlocation);

                Done = true;
            }


            return Done;
        }

        /// <summary>
        /// 识别中心
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public MatchResult IdentificationAsync(EnumCameraType camera, MatchIdentificationParam param)
        {
            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);
            //Task.Factory.StartNew(new Action(() =>
            //{

            if (camera == EnumCameraType.BondCamera)
            {
                //BondCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(param.RingLightintensity);
                BondCameraVisual.SetLightintensity(param);

                bool Done = BondCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = BondCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  BondCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = BondCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                bool En1 = BondCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = BondCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = BondCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return null;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = BondCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);


                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 0)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    //BondCameraVisual.SetDirectLightintensity(0);
                    //BondCameraVisual.SetRingLightintensity(0);

                    if (results[0].IsOk)
                    {
                        {

                            return results[0];
                        }
                    }



                }
            }

            if (camera == EnumCameraType.WaferCamera)
            {
                //WaferCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //WaferCameraVisual.SetRingLightintensity(param.RingLightintensity);
                WaferCameraVisual.SetLightintensity(param);

                bool Done = WaferCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = WaferCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  WaferCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = WaferCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                bool En1 = WaferCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = WaferCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = WaferCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return null;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = WaferCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);



                double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
                double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
                double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

                if (results != null && results.Count > 0)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    //WaferCameraVisual.SetDirectLightintensity(0);
                    //WaferCameraVisual.SetRingLightintensity(0);

                    if (results[0].IsOk)
                    {
                        {
                            return results[0];

                        }
                    }



                }
            }

            if (camera == EnumCameraType.UplookingCamera)
            {
                //UplookingCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(param.RingLightintensity);
                UplookingCameraVisual.SetLightintensity(param);

                bool Done = UplookingCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = UplookingCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  UplookingCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = UplookingCameraVisual.GetBitmap();

                Bitmap Showbitmap = new Bitmap(bitmap);

                bool En1 = UplookingCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = UplookingCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = UplookingCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return null;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = UplookingCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);

                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 0)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    //UplookingCameraVisual.SetDirectLightintensity(0);
                    //UplookingCameraVisual.SetRingLightintensity(0);

                    if (results[0].IsOk)
                    {
                        {
                            return results[0];

                        }
                    }



                }
            }

            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            //}));
            return null;
        }

        /// <summary>
        /// 识别并相对于相机中心的偏移坐标和角度
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public XYZTCoordinateConfig IdentificationAsync2(EnumCameraType camera, MatchIdentificationParam param)
        {

            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
            //string MatchTemplatefilepath;

            //string MatchRunfilepath;

            //RectangleF SearchRoi;

            //Task.Factory.StartNew(new Action(() =>
            //{

            if (camera == EnumCameraType.BondCamera)
            {
                //BondCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(param.RingLightintensity);
                Stopwatch sw = new Stopwatch();
                sw.Start();

                BondCameraVisual.SetLightintensity(param);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"设置光源强度{sw.ElapsedMilliseconds}ms \n");

                sw.Reset();
                sw.Start();

                bool Done = BondCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = BondCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  BondCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = BondCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"榜头相机获取图像{sw.ElapsedMilliseconds}ms \n");

                sw.Reset();
                sw.Start();

                bool En1 = BondCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = BondCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = BondCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return null;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = BondCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"榜头相机识别{sw.ElapsedMilliseconds}ms \n");

                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 0)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    //BondCameraVisual.SetDirectLightintensity(0);
                    //BondCameraVisual.SetRingLightintensity(0);

                    if (results[0].IsOk)
                    {
                        {
                            (BondX, BondY) = ImageToXY(results[0].MatchBox.Benchmark, new PointF(bitmap.Width / 2, bitmap.Height / 2), _BondcameraConfig.WidthPixelSize, _BondcameraConfig.HeightPixelSize);

                            offset = new XYZTCoordinateConfig();
                            offset.X = -BondX;
                            offset.Y = -BondY;
                            offset.Z = 0;
                            offset.Theta = results[0].MatchBox.Angle;
                            //LogRecorder.RecordLog(EnumLogContentType.Info, $"IdentificationAsync2-BondCamera,offsetX:{offset.X},offsetY:{offset.Y},offsetT:{offset.Theta}.");
                            return offset;
                        }
                    }
                    else
                    {
                        return null;
                    }



                }
                else
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    return null;
                }
            }

            if (camera == EnumCameraType.WaferCamera)
            {
                //WaferCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //WaferCameraVisual.SetRingLightintensity(param.RingLightintensity);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                WaferCameraVisual.SetLightintensity(param);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"设置光源强度{sw.ElapsedMilliseconds}ms \n");

                sw.Reset();
                sw.Start();

                bool Done = WaferCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = WaferCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  WaferCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = WaferCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"晶圆相机获取图像{sw.ElapsedMilliseconds}ms \n");

                sw.Reset();
                sw.Start();

                bool En1 = WaferCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = WaferCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = WaferCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return null;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = WaferCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"晶圆相机识别{sw.ElapsedMilliseconds}ms \n");

                double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
                double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
                double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

                if (results != null && results.Count > 0)
                {
                    if (results[0].IsOk)
                    {
                        Task.Factory.StartNew(new Action(() =>
                        {
                            CameraWindowGUI.Instance.ShowImage(Showbitmap);
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.GraphicDrawInit(results);
                            CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                        }));

                        //WaferCameraVisual.SetDirectLightintensity(0);
                        //WaferCameraVisual.SetRingLightintensity(0);

                        {
                            (WaferTableX, WaferTableY) = ImageToXY(results[0].MatchBox.Benchmark, new PointF(bitmap.Width / 2, bitmap.Height / 2), _WafercameraConfig.WidthPixelSize, _WafercameraConfig.HeightPixelSize);

                            offset = new XYZTCoordinateConfig();
                            offset.X = -WaferTableX;
                            offset.Y = WaferTableY;
                            offset.Z = 0;
                            offset.Theta = results[0].MatchBox.Angle;
                            LogRecorder.RecordLog(EnumLogContentType.Info, $"IdentificationAsync2-WaferCamera,offsetX:{offset.X},offsetY:{offset.Y},offsetT:{offset.Theta}.");
                            return offset;
                        }
                    }
                    else
                    {
                        return null;
                    }



                }
                else
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    return null;
                }
            }

            if (camera == EnumCameraType.UplookingCamera)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //UplookingCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(param.RingLightintensity);
                UplookingCameraVisual.SetLightintensity(param);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"设置光源强度{sw.ElapsedMilliseconds}ms \n");

                //Thread.Sleep(2000);

                sw.Reset();
                sw.Start();

                bool Done = UplookingCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = UplookingCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  UplookingCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = UplookingCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"仰视相机获取图像{sw.ElapsedMilliseconds}ms \n");

                //Thread.Sleep(2000);

                sw.Reset();
                sw.Start();

                bool En1 = UplookingCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = UplookingCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = UplookingCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return null;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = UplookingCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);

                sw.Stop();
                LogRecorder.RecordLog(EnumLogContentType.Info, $"仰视相机识别{sw.ElapsedMilliseconds}ms \n");


                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 0)
                {
                    if (results[0].IsOk)
                    {
                        Task.Factory.StartNew(new Action(() =>
                        {
                            CameraWindowGUI.Instance.ShowImage(Showbitmap);
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.GraphicDrawInit(results);
                            CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                        }));

                        //UplookingCameraVisual.SetDirectLightintensity(0);
                        //UplookingCameraVisual.SetRingLightintensity(0);

                        {
                            (BondX, BondY) = ImageToXY(results[0].MatchBox.Benchmark, new PointF(bitmap.Width / 2, bitmap.Height / 2), _UplookingcameraConfig.WidthPixelSize, _UplookingcameraConfig.HeightPixelSize);

                            offset = new XYZTCoordinateConfig();
                            offset.X = BondX;
                            offset.Y = BondY;
                            offset.Z = 0;
                            offset.Theta = results[0].MatchBox.Angle;
                            //LogRecorder.RecordLog(EnumLogContentType.Info, $"IdentificationAsync2-UplookingCamera,offsetX:{offset.X},offsetY:{offset.Y},offsetT:{offset.Theta}.");
                            return offset;
                        }
                    }
                    else
                    {
                        return null;
                    }


                }
                else
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));
                    return null;
                }
            }

            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            return null;
        }

        /// <summary>
        /// 识别并相对于相机中心的偏移坐标和角度
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public XYZTCoordinateConfig IdentificationAsync2(EnumCameraType camera, LineFindIdentificationParam param)
        {

            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            XYZTCoordinateConfig offset = new XYZTCoordinateConfig();
            //string MatchTemplatefilepath;

            //string MatchRunfilepath;

            //RectangleF SearchRoi;

            //Task.Factory.StartNew(new Action(() =>
            //{

            if (camera == EnumCameraType.BondCamera)
            {
                BondCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                BondCameraVisual.SetRingLightintensity(param.RingLightintensity);

                List<int> Scores = new List<int>();

                List<string> LinesFile = new List<string>();

                List<RectangleF> ROIs = new List<RectangleF>();
                List<bool> Scans = new List<bool>();

                LinesFile.Add(param.UpEdgefilepath);
                RectangleF rectangle = new RectangleF(param.UpEdgeRoi.X, param.UpEdgeRoi.Y, param.UpEdgeRoi.Width, param.UpEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(true);
                Scores.Add(param.UpEdgeScore);

                LinesFile.Add(param.DownEdgefilepath);
                rectangle = new RectangleF(param.DownEdgeRoi.X, param.DownEdgeRoi.Y, param.DownEdgeRoi.Width, param.DownEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(true);
                Scores.Add(param.DownEdgeScore);

                LinesFile.Add(param.LeftEdgefilepath);
                rectangle = new RectangleF(param.LeftEdgeRoi.X, param.LeftEdgeRoi.Y, param.LeftEdgeRoi.Width, param.LeftEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(false);
                Scores.Add(param.LeftEdgeScore);

                LinesFile.Add(param.RightEdgefilepath);
                rectangle = new RectangleF(param.RightEdgeRoi.X, param.RightEdgeRoi.Y, param.RightEdgeRoi.Width, param.RightEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(false);
                Scores.Add(param.RightEdgeScore);

                BondCameraVisual.ContinuousGetImage(false);

                List<LineResult> results = new List<LineResult>();

                Bitmap bitmap = BondCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                int ImageWidth = bitmap.Width;
                int ImageHeight = bitmap.Height;

                if (LinesFile.Count > 0)
                {
                    int i = 0;
                    foreach (string LineFindxml in LinesFile)
                    {
                        if (LineFindxml != null)
                        {
                            bool LineFindInited = BondCameraVisual.LineFindLoadRunPara(LineFindxml);

                            if (LineFindInited)
                            {
                                LineResult result = new LineResult();
                                bool Done = BondCameraVisual.LineFindRun(bitmap, Scores[i], ref result, ROIs[i], Scans[i]);
                                results.Add(result);
                            }

                            i++;
                        }
                    }
                }

                RectangleFA Rect = null;
                if (results.Count > 3)
                {
                    var line1 = CameraWindowGUI.CalculateLineEquation(results[0].Startpoint, results[0].Endpoint);
                    var line2 = CameraWindowGUI.CalculateLineEquation(results[1].Startpoint, results[1].Endpoint);
                    var line3 = CameraWindowGUI.CalculateLineEquation(results[2].Startpoint, results[2].Endpoint);
                    var line4 = CameraWindowGUI.CalculateLineEquation(results[3].Startpoint, results[3].Endpoint);

                    // 计算交点
                    var UL_intersection = CameraWindowGUI.FindIntersection(line1, line3);//左上交点
                    var DL_intersection = CameraWindowGUI.FindIntersection(line2, line3);//左下交点
                    var UR_intersection = CameraWindowGUI.FindIntersection(line1, line4);//右上交点
                    var DR_intersection = CameraWindowGUI.FindIntersection(line2, line4);//右下交点

                    Rect = new RectangleFA(UL_intersection, DL_intersection, UR_intersection, DR_intersection);

                }


                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 3)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.line, true);
                    }));

                    //BondCameraVisual.SetDirectLightintensity(0);
                    //BondCameraVisual.SetRingLightintensity(0);

                    //if (results[0].IsOk)
                    //{
                        {
                        (BondX, BondY) = ImageToXY(Rect.Center, new PointF(bitmap.Width / 2, bitmap.Height / 2), _BondcameraConfig.WidthPixelSize, _BondcameraConfig.HeightPixelSize);

                            offset = new XYZTCoordinateConfig();
                            offset.X = -BondX;
                            offset.Y = -BondY;
                            offset.Z = 0;
                            offset.Theta = Rect.Angle;
                        //LogRecorder.RecordLog(EnumLogContentType.Info, $"IdentificationAsync2-BondCamera,offsetX:{offset.X},offsetY:{offset.Y},offsetT:{offset.Theta}.");
                        return offset;
                        }
                    //}
                    //else
                    //{
                    //    return null;
                    //}



                }
                else
                {
                    return null;
                }
            }

            if (camera == EnumCameraType.WaferCamera)
            {
                WaferCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                WaferCameraVisual.SetRingLightintensity(param.RingLightintensity);

                List<int> Scores = new List<int>();

                List<string> LinesFile = new List<string>();

                List<RectangleF> ROIs = new List<RectangleF>();
                List<bool> Scans = new List<bool>();

                LinesFile.Add(param.UpEdgefilepath);
                RectangleF rectangle = new RectangleF(param.UpEdgeRoi.X, param.UpEdgeRoi.Y, param.UpEdgeRoi.Width, param.UpEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(true);
                Scores.Add(param.UpEdgeScore);

                LinesFile.Add(param.DownEdgefilepath);
                rectangle = new RectangleF(param.DownEdgeRoi.X, param.DownEdgeRoi.Y, param.DownEdgeRoi.Width, param.DownEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(true);
                Scores.Add(param.DownEdgeScore);

                LinesFile.Add(param.LeftEdgefilepath);
                rectangle = new RectangleF(param.LeftEdgeRoi.X, param.LeftEdgeRoi.Y, param.LeftEdgeRoi.Width, param.LeftEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(false);
                Scores.Add(param.LeftEdgeScore);

                LinesFile.Add(param.RightEdgefilepath);
                rectangle = new RectangleF(param.RightEdgeRoi.X, param.RightEdgeRoi.Y, param.RightEdgeRoi.Width, param.RightEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(false);
                Scores.Add(param.RightEdgeScore);

                WaferCameraVisual.ContinuousGetImage(false);

                List<LineResult> results = new List<LineResult>();

                Bitmap bitmap = WaferCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                int ImageWidth = bitmap.Width;
                int ImageHeight = bitmap.Height;

                if (LinesFile.Count > 0)
                {
                    int i = 0;
                    foreach (string LineFindxml in LinesFile)
                    {
                        if (LineFindxml != null)
                        {
                            bool LineFindInited = WaferCameraVisual.LineFindLoadRunPara(LineFindxml);

                            if (LineFindInited)
                            {
                                LineResult result = new LineResult();
                                bool Done = WaferCameraVisual.LineFindRun(bitmap, Scores[i], ref result, ROIs[i], Scans[i]);
                                results.Add(result);
                            }

                            i++;
                        }
                    }
                }

                RectangleFA Rect = null;
                if (results.Count > 3)
                {
                    var line1 = CameraWindowGUI.CalculateLineEquation(results[0].Startpoint, results[0].Endpoint);
                    var line2 = CameraWindowGUI.CalculateLineEquation(results[1].Startpoint, results[1].Endpoint);
                    var line3 = CameraWindowGUI.CalculateLineEquation(results[2].Startpoint, results[2].Endpoint);
                    var line4 = CameraWindowGUI.CalculateLineEquation(results[3].Startpoint, results[3].Endpoint);

                    // 计算交点
                    var UL_intersection = CameraWindowGUI.FindIntersection(line1, line3);//左上交点
                    var DL_intersection = CameraWindowGUI.FindIntersection(line2, line3);//左下交点
                    var UR_intersection = CameraWindowGUI.FindIntersection(line1, line4);//右上交点
                    var DR_intersection = CameraWindowGUI.FindIntersection(line2, line4);//右下交点

                    Rect = new RectangleFA(UL_intersection, DL_intersection, UR_intersection, DR_intersection);

                }


                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);


                

                if (results != null && results.Count > 3)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.line, true);
                    }));

                    //WaferCameraVisual.SetDirectLightintensity(0);
                    //WaferCameraVisual.SetRingLightintensity(0);

                    //if (results[0].IsOk)
                    //{
                        {
                            (BondX, BondY) = ImageToXY(Rect.Center, new PointF(bitmap.Width / 2, bitmap.Height / 2), _WafercameraConfig.WidthPixelSize, _WafercameraConfig.HeightPixelSize);

                            offset = new XYZTCoordinateConfig();
                            offset.X = -BondX;
                            offset.Y = BondY;
                            offset.Z = 0;
                            offset.Theta = Rect.Angle;
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"IdentificationAsync2-WaferCamera,offsetX:{offset.X},offsetY:{offset.Y},offsetT:{offset.Theta}.");
                        return offset;
                        }
                    //}
                    //else
                    //{
                    //    return null;
                    //}



                }
                else
                {
                    return null;
                }
            }

            if (camera == EnumCameraType.UplookingCamera)
            {
                UplookingCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                UplookingCameraVisual.SetRingLightintensity(param.RingLightintensity);

                List<int> Scores = new List<int>();

                List<string> LinesFile = new List<string>();

                List<RectangleF> ROIs = new List<RectangleF>();
                List<bool> Scans = new List<bool>();

                LinesFile.Add(param.UpEdgefilepath);
                RectangleF rectangle = new RectangleF(param.UpEdgeRoi.X, param.UpEdgeRoi.Y, param.UpEdgeRoi.Width, param.UpEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(true);
                Scores.Add(param.UpEdgeScore);

                LinesFile.Add(param.DownEdgefilepath);
                rectangle = new RectangleF(param.DownEdgeRoi.X, param.DownEdgeRoi.Y, param.DownEdgeRoi.Width, param.DownEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(true);
                Scores.Add(param.DownEdgeScore);

                LinesFile.Add(param.LeftEdgefilepath);
                rectangle = new RectangleF(param.LeftEdgeRoi.X, param.LeftEdgeRoi.Y, param.LeftEdgeRoi.Width, param.LeftEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(false);
                Scores.Add(param.LeftEdgeScore);

                LinesFile.Add(param.RightEdgefilepath);
                rectangle = new RectangleF(param.RightEdgeRoi.X, param.RightEdgeRoi.Y, param.RightEdgeRoi.Width, param.RightEdgeRoi.Height);
                ROIs.Add(rectangle);
                Scans.Add(false);
                Scores.Add(param.RightEdgeScore);

                UplookingCameraVisual.ContinuousGetImage(false);

                List<LineResult> results = new List<LineResult>();

                Bitmap bitmap = UplookingCameraVisual.GetBitmap();
                Bitmap Showbitmap = new Bitmap(bitmap);

                int ImageWidth = bitmap.Width;
                int ImageHeight = bitmap.Height;

                if (LinesFile.Count > 0)
                {
                    int i = 0;
                    foreach (string LineFindxml in LinesFile)
                    {
                        if (LineFindxml != null)
                        {
                            bool LineFindInited = UplookingCameraVisual.LineFindLoadRunPara(LineFindxml);

                            if (LineFindInited)
                            {
                                LineResult result = new LineResult();
                                bool Done = UplookingCameraVisual.LineFindRun(bitmap, Scores[i], ref result, ROIs[i], Scans[i]);
                                results.Add(result);
                            }

                            i++;
                        }
                    }
                }

                RectangleFA Rect = null;
                if (results.Count > 3)
                {
                    var line1 = CameraWindowGUI.CalculateLineEquation(results[0].Startpoint, results[0].Endpoint);
                    var line2 = CameraWindowGUI.CalculateLineEquation(results[1].Startpoint, results[1].Endpoint);
                    var line3 = CameraWindowGUI.CalculateLineEquation(results[2].Startpoint, results[2].Endpoint);
                    var line4 = CameraWindowGUI.CalculateLineEquation(results[3].Startpoint, results[3].Endpoint);

                    // 计算交点
                    var UL_intersection = CameraWindowGUI.FindIntersection(line1, line3);//左上交点
                    var DL_intersection = CameraWindowGUI.FindIntersection(line2, line3);//左下交点
                    var UR_intersection = CameraWindowGUI.FindIntersection(line1, line4);//右上交点
                    var DR_intersection = CameraWindowGUI.FindIntersection(line2, line4);//右下交点

                    Rect = new RectangleFA(UL_intersection, DL_intersection, UR_intersection, DR_intersection);

                }


                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 3)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.line, true);
                    }));

                    //UplookingCameraVisual.SetDirectLightintensity(0);
                    //UplookingCameraVisual.SetRingLightintensity(0);

                    //if (results[0].IsOk)
                    //{
                        {
                            (BondX, BondY) = ImageToXY(Rect.Center, new PointF(bitmap.Width / 2, bitmap.Height / 2), _UplookingcameraConfig.WidthPixelSize, _UplookingcameraConfig.HeightPixelSize);

                            offset = new XYZTCoordinateConfig();
                            offset.X = BondX;
                            offset.Y = BondY;
                            offset.Z = 0;
                            offset.Theta = Rect.Angle;
                        LogRecorder.RecordLog(EnumLogContentType.Info, $"IdentificationAsync2-UplookingCamera,offsetX:{offset.X},offsetY:{offset.Y},offsetT:{offset.Theta}.");
                        return offset;
                        }
                    //}
                    //else
                    //{
                    //    return null;
                    //}



                }
                else
                {
                    return null;
                }
            }

            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            return null;
        }


        /// <summary>
        /// 识别并移动到中心
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool IdentificationMoveAsync(EnumCameraType camera, MatchIdentificationParam param)
        {
            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            //string MatchTemplatefilepath;

            //string MatchRunfilepath;

            //RectangleF SearchRoi;

            //Task.Factory.StartNew(new Action(() =>
            //{

            if (camera == EnumCameraType.BondCamera)
            {
                //BondCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(param.RingLightintensity);
                BondCameraVisual.SetLightintensity(param);

                bool Done = BondCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = BondCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  BondCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = BondCameraVisual.GetBitmap();
                if(bitmap == null)
                {
                    return false;
                }

                Bitmap Showbitmap = new Bitmap(bitmap);

                bool En1 = BondCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = BondCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = BondCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return false;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = BondCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);


                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 0)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        CameraWindowGUI.Instance.ShowImage(Showbitmap);
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.GraphicDrawInit(results);
                        CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                    }));

                    //BondCameraVisual.SetDirectLightintensity(0);
                    //BondCameraVisual.SetRingLightintensity(0);

                    if (results[0].IsOk)
                    {
                        {
                            (BondX, BondY) = ImageToXY(results[0].MatchBox.Benchmark, new PointF(bitmap.Width / 2, bitmap.Height / 2), _BondcameraConfig.WidthPixelSize, _BondcameraConfig.HeightPixelSize);

                            AxisRelativeMove(EnumStageAxis.BondX, -BondX);

                            AxisRelativeMove(EnumStageAxis.BondY, -BondY);

                            return true;
                        }
                    }



                }
            }

            if (camera == EnumCameraType.WaferCamera)
            {
                //WaferCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //WaferCameraVisual.SetRingLightintensity(param.RingLightintensity);
                WaferCameraVisual.SetLightintensity(param);

                bool Done = WaferCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = WaferCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  WaferCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = WaferCameraVisual.GetBitmap();
                if (bitmap == null)
                {
                    return false;
                }
                Bitmap Showbitmap = new Bitmap(bitmap);

                bool En1 = WaferCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = WaferCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = WaferCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return false;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = WaferCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);

                double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
                double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
                double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

                if (results != null && results.Count > 0)
                {
                    if (results[0].IsOk)
                    {
                        Task.Factory.StartNew(new Action(() =>
                        {
                            CameraWindowGUI.Instance.ShowImage(Showbitmap);
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.GraphicDrawInit(results);
                            CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                        }));

                        //WaferCameraVisual.SetDirectLightintensity(0);
                        //WaferCameraVisual.SetRingLightintensity(0);

                        {
                            (WaferTableX, WaferTableY) = ImageToXY(results[0].MatchBox.Benchmark, new PointF(bitmap.Width / 2, bitmap.Height / 2), _WafercameraConfig.WidthPixelSize, _WafercameraConfig.HeightPixelSize);

                            AxisRelativeMove(EnumStageAxis.WaferTableX, -WaferTableX);

                            AxisRelativeMove(EnumStageAxis.WaferTableY, WaferTableY);

                            return true;
                        }
                    }



                }
            }

            if (camera == EnumCameraType.UplookingCamera)
            {
                //UplookingCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(param.RingLightintensity);
                UplookingCameraVisual.SetLightintensity(param);

                bool Done = UplookingCameraVisual.LoadMatchTrainXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Templatexml));

                Done = UplookingCameraVisual.LoadMatchRunXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, param.Runxml));

                //var result =  UplookingCameraVisual.MatchFindAsync(param.Score, param.MinAngle, param.MaxAngle, param.SearchRoi);

                Bitmap bitmap = UplookingCameraVisual.GetBitmap();
                if (bitmap == null)
                {
                    return false;
                }
                Bitmap Showbitmap = new Bitmap(bitmap);

                bool En1 = UplookingCameraVisual.MatchSetRunPara<float>(MatchParas.MinScore, 0.2f);
                bool En2 = UplookingCameraVisual.MatchSetRunPara<int>(MatchParas.AngleStart, param.MinAngle);
                bool En3 = UplookingCameraVisual.MatchSetRunPara<int>(MatchParas.AngleEnd, param.MaxAngle);

                if (!(En1 && En2 && En3))
                {
                    return false;
                }

                List<MatchResult> results = new List<MatchResult>();

                Done = UplookingCameraVisual.MatchRun(bitmap, param.Score, ref results, param.SearchRoi);

                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                if (results != null && results.Count > 0)
                {
                    if (results[0].IsOk)
                    {
                        Task.Factory.StartNew(new Action(() =>
                        {
                            CameraWindowGUI.Instance.ShowImage(Showbitmap);
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.GraphicDrawInit(results);
                            CameraWindowGUI.Instance.GraphicDraw(Graphic.match, true);
                        }));

                        //UplookingCameraVisual.SetDirectLightintensity(0);
                        //UplookingCameraVisual.SetRingLightintensity(0);

                        {
                            (BondX, BondY) = ImageToXY(results[0].MatchBox.Benchmark, new PointF(bitmap.Width / 2, bitmap.Height / 2), _UplookingcameraConfig.WidthPixelSize, _UplookingcameraConfig.HeightPixelSize);

                            AxisRelativeMove(EnumStageAxis.BondX, BondX);

                            AxisRelativeMove(EnumStageAxis.BondY, BondY);

                            return true;
                        }
                    }



                }
            }


            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);
            //WaferCameraVisual.SetDirectLightintensity(0);
            //WaferCameraVisual.SetRingLightintensity(0);
            //UplookingCameraVisual.SetDirectLightintensity(0);
            //UplookingCameraVisual.SetRingLightintensity(0);

            return false;
        }

        /// <summary>
        /// 榜头相机识别mark,并移动到中心
        /// </summary>
        private bool BondCameraIdentifymarkpoints(EnumMaskType maskType, MatchIdentificationParam param)
        {

            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.TrackOrigion)
            {
                BondX = config.TrackOrigion.X;
                BondY = config.TrackOrigion.Y;
                BondZ = config.TrackOrigion.Z;
            }
            else if (maskType == EnumMaskType.BondOrigion)
            {
                BondX = config.BondOrigion.X;
                BondY = config.BondOrigion.Y;
                BondZ = config.BondOrigion.Z;
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                BondX = config.LookupCameraOrigion.X;
                BondY = config.LookupCameraOrigion.Y;
                BondZ = config.LookupCameraOrigion.Z;
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                BondX = config.WaferCameraOrigion.X;
                BondY = config.WaferCameraOrigion.Y;
                BondZ = config.WaferCameraOrigion.Z;
            }
            else if (maskType == EnumMaskType.EutecticWeldingOrigion)
            {
                BondX = config.EutecticWeldingLocation.X;
                BondY = config.EutecticWeldingLocation.Y;
                BondZ = config.EutecticWeldingLocation.Z;
            }
            else if (maskType == EnumMaskType.CalibrationTableOrigion)
            {
                BondX = config.CalibrationTableOrigion.X;
                BondY = config.CalibrationTableOrigion.Y;
                BondZ = config.CalibrationTableOrigion.Z + config.TrackOrigion.Z;
            }




            AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);

            BondXYZAbsoluteMove(BondX, BondY, config.BondSafeLocation.Z);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            //BondCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
            //BondCameraVisual.SetRingLightintensity(param.RingLightintensity);
            BondCameraVisual.SetLightintensity(param);
            //AutofocusAsync(EnumCameraType.BondCamera, AutofocusRange, AutofocusInterval, param);


            //Task.Factory.StartNew(new Action(() =>
            //{
            for (int i = 0; i < 5; i++)
            {

                bool Done = IdentificationMoveAsync(EnumCameraType.BondCamera, param);

                if(Done == false)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别。", "提示");
                    if (result1 == 1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(0);
                }

            }

            return true;



        }

        /// <summary>
        /// 榜头相机识别mark,并移动到中心
        /// </summary>
        private bool BondCameraIdentifymarkpointsManual(EnumMaskType maskType)
        {


            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "榜头相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();


            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.TrackOrigion)
            {
                BondX = config.TrackOrigion.X;
                BondY = config.TrackOrigion.Y;
                BondZ = config.TrackOrigion.Z;



                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch);

                title = "创建轨道原点识别";
            }
            else if (maskType == EnumMaskType.BondOrigion)
            {
                BondX = config.BondOrigion.X;
                BondY = config.BondOrigion.Y;
                BondZ = config.BondOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch);

                title = "创建系统原点识别";
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                BondX = config.LookupCameraOrigion.X;
                BondY = config.LookupCameraOrigion.Y;
                BondZ = config.LookupCameraOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch);

                title = "创建仰视相机中心识别";
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                BondX = config.WaferCameraOrigion.X;
                BondY = config.WaferCameraOrigion.Y;
                //BondZ = config.WaferCameraOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch);

                title = "创建晶圆相机中心识别";
            }
            else if (maskType == EnumMaskType.EutecticWeldingOrigion)
            {
                BondX = config.EutecticWeldingLocation.X;
                BondY = config.EutecticWeldingLocation.Y;
                BondZ = config.EutecticWeldingLocation.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch);

                title = "创建共晶位置识别";
            }
            else if (maskType == EnumMaskType.CalibrationTableOrigion)
            {
                BondX = config.CalibrationTableOrigion.X;
                BondY = config.CalibrationTableOrigion.Y;
                BondZ = config.CalibrationTableOrigion.Z + config.TrackOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch);

                title = "创建校准台位置识别";
            }




            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            //提示创建识别

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return false;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }

            if (maskType == EnumMaskType.TrackOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch;

            }
            else if (maskType == EnumMaskType.BondOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch;
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch;
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch;
            }
            else if (maskType == EnumMaskType.EutecticWeldingOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch;
            }
            else if (maskType == EnumMaskType.CalibrationTableOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch;
            }

            //BondCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
            //BondCameraVisual.SetRingLightintensity(param.RingLightintensity);

            BondCameraVisual.SetLightintensity(param);
            //AutofocusAsync(EnumCameraType.BondCamera, AutofocusRange, AutofocusInterval, param);
            //Task.Factory.StartNew(new Action(() =>
            //{
            for (int i = 0; i < 5; i++)
            {

                bool done = IdentificationMoveAsync(EnumCameraType.BondCamera, param);

                if (done == false)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(0);
                }

            }

            return true;



        }

        /// <summary>
        /// 榜头相机识别mark,并移动到中心
        /// </summary>
        private bool BondCameraIdentifymarkpointsManual2(EnumMaskType maskType)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(100, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "榜头相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();


            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.TrackOrigion)
            {
                BondX = config.TrackOrigion.X;
                BondY = config.TrackOrigion.Y;
                BondZ = config.TrackOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch);

                title = "创建轨道原点识别";
            }
            else if (maskType == EnumMaskType.BondOrigion)
            {
                BondX = config.BondOrigion.X;
                BondY = config.BondOrigion.Y;
                BondZ = config.BondOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch);


                title = "创建系统原点识别";
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                BondX = config.LookupCameraOrigion.X;
                BondY = config.LookupCameraOrigion.Y;
                BondZ = config.LookupCameraOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch);

                title = "创建仰视相机中心识别";
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                BondX = config.WaferCameraOrigion.X;
                BondY = config.WaferCameraOrigion.Y;
                BondZ = config.WaferCameraOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch);

                title = "创建晶圆相机中心识别";
            }
            else if (maskType == EnumMaskType.EutecticWeldingOrigion)
            {
                BondX = config.EutecticWeldingLocation.X;
                BondY = config.EutecticWeldingLocation.Y;
                BondZ = config.EutecticWeldingLocation.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch);

                title = "创建共晶位置识别";
            }
            else if (maskType == EnumMaskType.CalibrationTableOrigion)
            {
                BondX = config.CalibrationTableOrigion.X;
                BondY = config.CalibrationTableOrigion.Y;
                BondZ = config.CalibrationTableOrigion.Z + config.TrackOrigion.Z;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch.Runxml;

                //BondCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //BondCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch);

                title = "创建校准台位置识别";
            }




            //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            //BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            //提示创建识别

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return false;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }

            if (maskType == EnumMaskType.TrackOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch;

            }
            else if (maskType == EnumMaskType.BondOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch;
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch;
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch;
            }
            else if (maskType == EnumMaskType.EutecticWeldingOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch;
            }
            else if (maskType == EnumMaskType.CalibrationTableOrigion)
            {
                _systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch;
            }

            return true;

        }


        /// <summary>
        /// 榜头相机识别mark,并移动到中心
        /// </summary>
        private void BondCameraIdentifymarkpointsInitManual(EnumMaskType maskType)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "榜头相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, BondCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();


            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.TrackOrigion)
            {
                BondX = config.TrackOrigion.X;
                BondY = config.TrackOrigion.Y;
                BondZ = config.TrackOrigion.Z;

                visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.RingLightintensity;
                visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.DirectLightintensity;
                visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Score;
                visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.MaxAngle;
                visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.TemplateRoi;
                visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.SearchRoi;
                //visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Templateresult;
                visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Templatexml;
                visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch.Runxml;


                title = "创建轨道原点识别";
            }
            else if (maskType == EnumMaskType.BondOrigion)
            {
                BondX = config.BondOrigion.X;
                BondY = config.BondOrigion.Y;
                BondZ = config.BondOrigion.Z;

                visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.RingLightintensity;
                visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.DirectLightintensity;
                visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Score;
                visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.MaxAngle;
                visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.TemplateRoi;
                visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.SearchRoi;
                //visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Templateresult;
                visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Templatexml;
                visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch.Runxml;


                title = "创建系统原点识别";
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                BondX = config.LookupCameraOrigion.X;
                BondY = config.LookupCameraOrigion.Y;
                BondZ = config.LookupCameraOrigion.Z;

                visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.RingLightintensity;
                visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.DirectLightintensity;
                visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Score;
                visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.MaxAngle;
                visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.TemplateRoi;
                visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.SearchRoi;
                //visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Templateresult;
                visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Templatexml;
                visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch.Runxml;

                title = "创建仰视相机中心识别";
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                BondX = config.WaferCameraOrigion.X;
                BondY = config.WaferCameraOrigion.Y;
                BondZ = config.WaferCameraOrigion.Z;

                visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.RingLightintensity;
                visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.DirectLightintensity;
                visualMatch.Score = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Score;
                visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.MaxAngle;
                visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.TemplateRoi;
                visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.SearchRoi;
                //visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Templateresult;
                visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Templatexml;
                visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch.Runxml;

                title = "创建晶圆相机中心识别";
            }




            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            //提示创建识别

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }
        }


        /// <summary>
        /// 晶圆相机识别mark,并移动到中心
        /// </summary>
        private bool WaferCameraIdentifymarkpoints(EnumMaskType maskType, MatchIdentificationParam param)
        {

            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(2);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
            double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
            double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

            if (maskType == EnumMaskType.WaferOrigion)
            {
                WaferTableX = config.WaferOrigion.X;
                WaferTableY = config.WaferOrigion.Y;
                WaferTableZ = config.WaferOrigion.Z;
            }




            AxisAbsoluteMove(EnumStageAxis.WaferTableX, WaferTableX);
            AxisAbsoluteMove(EnumStageAxis.WaferTableY, WaferTableY);
            AxisAbsoluteMove(EnumStageAxis.WaferTableZ, WaferTableZ);

            //WaferCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
            //WaferCameraVisual.SetRingLightintensity(param.RingLightintensity);

            WaferCameraVisual.SetLightintensity(param);

            //AutofocusAsync(EnumCameraType.WaferCamera, AutofocusRange, AutofocusInterval, param);
            //Task.Factory.StartNew(new Action(() =>
            //{
            for (int i = 0; i < 5; i++)
            {

                bool done = IdentificationMoveAsync(EnumCameraType.WaferCamera, param);

                if (done == false)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(2);
                }

            }

            return true;


        }

        /// <summary>
        /// 晶圆相机识别mark,并移动到中心
        /// </summary>
        private bool WaferCameraIdentifymarkpointsManual(EnumMaskType maskType)
        {

            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(2);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "晶圆相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, WaferCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();


            double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
            double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
            double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

            if (maskType == EnumMaskType.WaferOrigion)
            {
                WaferTableX = config.WaferOrigion.X;
                WaferTableY = config.WaferOrigion.Y;
                WaferTableZ = config.WaferOrigion.Z;


                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Runxml;

                //WaferCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //WaferCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);

                title = "创建晶圆原点识别";
            }




            AxisAbsoluteMove(EnumStageAxis.WaferTableX, WaferTableX);
            AxisAbsoluteMove(EnumStageAxis.WaferTableY, WaferTableY);
            AxisAbsoluteMove(EnumStageAxis.WaferTableZ, WaferTableZ);


            //提示创建识别

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return false;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }

            if (maskType == EnumMaskType.WaferOrigion)
            {
                _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch;
            }

            //WaferCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
            //WaferCameraVisual.SetRingLightintensity(param.RingLightintensity);

            WaferCameraVisual.SetLightintensity(param);

            //AutofocusAsync(EnumCameraType.WaferCamera, AutofocusRange, AutofocusInterval, param);
            //Task.Factory.StartNew(new Action(() =>
            //{
            for (int i = 0; i < 5; i++)
            {

                bool done = IdentificationMoveAsync(EnumCameraType.WaferCamera, param);

                if (done == false)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(2);
                }

            }

            return true;

        }

        /// <summary>
        /// 晶圆相机识别mark,并移动到中心
        /// </summary>
        private bool WaferCameraIdentifymarkpointsManual2(EnumMaskType maskType)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(2);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "晶圆相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, WaferCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();


            double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
            double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
            double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

            if (maskType == EnumMaskType.WaferOrigion)
            {
                WaferTableX = config.WaferOrigion.X;
                WaferTableY = config.WaferOrigion.Y;
                WaferTableZ = config.WaferOrigion.Z;


                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch.Runxml;

                //WaferCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //WaferCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);

                title = "创建晶圆原点识别";
            }




            //AxisAbsoluteMove(EnumStageAxis.WaferTableX, WaferTableX);
            //AxisAbsoluteMove(EnumStageAxis.WaferTableY, WaferTableY);
            //AxisAbsoluteMove(EnumStageAxis.WaferTableZ, WaferTableZ);


            //提示创建识别

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return false;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }

            if (maskType == EnumMaskType.WaferOrigion)
            {
                _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch;
            }

            return true;
        }



        /// <summary>
        /// 仰视相机识别mark,并移动到中心
        /// </summary>
        private bool UplookingCameraIdentifymarkpoints(EnumMaskType maskType, MatchIdentificationParam param)
        {


            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            double SubmountPPZ = 0;
            if (DeviceMode == 0)
            {
                if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);
            }

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                BondX = config.LookupChipPPOrigion.X;
                BondY = config.LookupChipPPOrigion.Y;
                BondZ = config.LookupChipPPOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                BondX = config.LookupChipPPOrigion.X + currentppTool.PPAndUCtoolOffset.X;
                BondY = config.LookupChipPPOrigion.Y + currentppTool.PPAndUCtoolOffset.Y;
                BondZ = config.LookupChipPPOrigion.Z + currentppTool.PPAndUCtoolOffset.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                BondX = config.LookupSubmountPPOrigion.X;
                BondY = config.LookupSubmountPPOrigion.Y;
                BondZ = config.LookupSubmountPPOrigion.Z;
                SubmountPPZ = config.SubmountPPWorkZ;

            }
            else if (maskType == EnumMaskType.LookupLaserSensorOrigion)
            {
                BondX = config.LookupLaserSensorOrigion.X;
                BondY = config.LookupLaserSensorOrigion.Y;
                BondZ = config.LookupLaserSensorOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, SubmountPPZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);


            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            //UplookingCameraVisual.SetDirectLightintensity(param.DirectLightintensity);
            //UplookingCameraVisual.SetRingLightintensity(param.RingLightintensity);

            UplookingCameraVisual.SetLightintensity(param);

            //AutofocusAsync(EnumCameraType.UplookingCamera, AutofocusRange, AutofocusInterval, param);
            //Task.Factory.StartNew(new Action(() =>
            //{
            for (int i = 0; i < 5; i++)
            {

                bool done = IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);

                if (done == false)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }

                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(1);
                }

            }
            //}));
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            return true;

        }

        /// <summary>
        /// 仰视相机识别mark,并移动到中心
        /// </summary>
        private bool UplookingCameraIdentifymarkpointsManual(EnumMaskType maskType)
        {


            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            string name = "仰视相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            double SubmountPPZ = 0;
            if (DeviceMode == 0)
            {
                if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);
            }
                

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                BondX = config.LookupChipPPOrigion.X;
                BondY = config.LookupChipPPOrigion.Y;
                BondZ = config.LookupChipPPOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);

                title = "创建uc工具识别";
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                BondX = config.LookupChipPPOrigion.X + currentppTool.PPAndUCtoolOffset.X;
                BondY = config.LookupChipPPOrigion.Y + currentppTool.PPAndUCtoolOffset.Y;
                BondZ = config.LookupChipPPOrigion.Z + currentppTool.PPAndUCtoolOffset.Z;
                SubmountPPZ = config.SubmountPPFreeZ;

                if(currentppTool.UplookingIdentifyPPtoolMatch.Templatexml == null)
                {
                    currentppTool.UplookingIdentifyPPtoolMatch.Templatexml = $"Config/SystemConfiguration/{currentppTool.PPName}/Template.contourmxml";
                }
                if (currentppTool.UplookingIdentifyPPtoolMatch.Runxml == null)
                {
                    currentppTool.UplookingIdentifyPPtoolMatch.Runxml = $"Config/SystemConfiguration/{currentppTool.PPName}/Run.contourmxml";
                }

                //visualMatch.RingLightintensity = currentppTool.UplookingIdentifyPPtoolMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = currentppTool.UplookingIdentifyPPtoolMatch.DirectLightintensity;
                //visualMatch.Score = currentppTool.UplookingIdentifyPPtoolMatch.Score;
                //visualMatch.AngleRange = currentppTool.UplookingIdentifyPPtoolMatch.MaxAngle;
                //visualMatch.TemplateRoi = currentppTool.UplookingIdentifyPPtoolMatch.TemplateRoi;
                //visualMatch.SearchRoi = currentppTool.UplookingIdentifyPPtoolMatch.SearchRoi;
                ////visualMatch.Templateresult = currentppTool.UplookingIdentifyPPtoolMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = currentppTool.UplookingIdentifyPPtoolMatch.Templatexml;
                //visualMatch.MatchRunfilepath = currentppTool.UplookingIdentifyPPtoolMatch.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(currentppTool.UplookingIdentifyPPtoolMatch);

                title = "创建芯片吸嘴识别";
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                BondX = config.LookupSubmountPPOrigion.X;
                BondY = config.LookupSubmountPPOrigion.Y;
                BondZ = config.LookupSubmountPPOrigion.Z;
                SubmountPPZ = config.SubmountPPWorkZ;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match);

                title = "创建基底吸嘴识别";
            }
            else if (maskType == EnumMaskType.LookupLaserSensorOrigion)
            {
                BondX = config.LookupLaserSensorOrigion.X;
                BondY = config.LookupLaserSensorOrigion.Y;
                BondZ = config.LookupLaserSensorOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch);

                title = "创建激光传感器识别";
            }
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, SubmountPPZ);
            else if(DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return false;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }


            //AutofocusAsync(EnumCameraType.UplookingCamera, AutofocusRange, AutofocusInterval);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match = param1;

                param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match;

            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                currentppTool.UplookingIdentifyPPtoolMatch = param1;

                param = currentppTool.UplookingIdentifyPPtoolMatch;

            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match = param1;

                param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match;
            }
            else if (maskType == EnumMaskType.LookupLaserSensorOrigion)
            {
                _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch;
            }
            //Task.Factory.StartNew(new Action(() =>
            //{
            for (int i = 0; i < 5; i++)
            {

                bool done = IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);

                if (done == false)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                if (CameraWindowGUI.Instance != null)
                {
                    CameraWindowGUI.Instance.SelectCamera(1);
                }

            }
            //}));
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            return true;
        }

        /// <summary>
        /// 仰视相机识别mark,并移动到中心
        /// </summary>
        private bool UplookingCameraIdentifymarkpointsManual2(EnumMaskType maskType)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double SubmountPPZ = 0;
            if (DeviceMode == 0)
            {
                SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);
            }


            if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "基底吸嘴移动到工作位置", "提示");
                if (result1 == 1)
                {
                    if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }

            }


            string name = "仰视相机识别";
            string title = "";
            VisualMatchControlGUI visualMatch = new VisualMatchControlGUI();
            visualMatch.InitVisualControl(CameraWindowGUI.Instance, UplookingCameraVisual);

            MatchIdentificationParam param = new MatchIdentificationParam();

            MatchIdentificationParam param1 = new MatchIdentificationParam();

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                BondX = config.LookupChipPPOrigion.X;
                BondY = config.LookupChipPPOrigion.Y;
                BondZ = config.LookupChipPPOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);

                title = "创建uc工具识别";
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                BondX = config.LookupChipPPOrigion.X + currentppTool.PPAndUCtoolOffset.X;
                BondY = config.LookupChipPPOrigion.Y + currentppTool.PPAndUCtoolOffset.Y;
                BondZ = config.LookupChipPPOrigion.Z + currentppTool.PPAndUCtoolOffset.Z;
                SubmountPPZ = config.SubmountPPFreeZ;

                if (currentppTool.UplookingIdentifyPPtoolMatch.Templatexml == null)
                {
                    currentppTool.UplookingIdentifyPPtoolMatch.Templatexml = $"Config/SystemConfiguration/{currentppTool.PPName}/Template.contourmxml";
                }
                if (currentppTool.UplookingIdentifyPPtoolMatch.Runxml == null)
                {
                    currentppTool.UplookingIdentifyPPtoolMatch.Runxml = $"Config/SystemConfiguration/{currentppTool.PPName}/Run.contourmxml";
                }

                //visualMatch.RingLightintensity = currentppTool.UplookingIdentifyPPtoolMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = currentppTool.UplookingIdentifyPPtoolMatch.DirectLightintensity;
                //visualMatch.Score = currentppTool.UplookingIdentifyPPtoolMatch.Score;
                //visualMatch.AngleRange = currentppTool.UplookingIdentifyPPtoolMatch.MaxAngle;
                //visualMatch.TemplateRoi = currentppTool.UplookingIdentifyPPtoolMatch.TemplateRoi;
                //visualMatch.SearchRoi = currentppTool.UplookingIdentifyPPtoolMatch.SearchRoi;
                ////visualMatch.Templateresult = currentppTool.UplookingIdentifyPPtoolMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = currentppTool.UplookingIdentifyPPtoolMatch.Templatexml;
                //visualMatch.MatchRunfilepath = currentppTool.UplookingIdentifyPPtoolMatch.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(currentppTool.UplookingIdentifyPPtoolMatch);

                title = "创建芯片吸嘴识别";
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                BondX = config.LookupSubmountPPOrigion.X;
                BondY = config.LookupSubmountPPOrigion.Y;
                BondZ = config.LookupSubmountPPOrigion.Z;
                SubmountPPZ = config.SubmountPPWorkZ;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match);

                title = "创建基底吸嘴识别";
            }
            else if (maskType == EnumMaskType.LookupLaserSensorOrigion)
            {
                BondX = config.LookupLaserSensorOrigion.X;
                BondY = config.LookupLaserSensorOrigion.Y;
                BondZ = config.LookupLaserSensorOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;

                //visualMatch.RingLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.RingLightintensity;
                //visualMatch.DirectLightintensity = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.DirectLightintensity;
                //visualMatch.Score = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Score;
                //visualMatch.AngleRange = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.MaxAngle;
                //visualMatch.TemplateRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.TemplateRoi;
                //visualMatch.SearchRoi = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.SearchRoi;
                ////visualMatch.Templateresult = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Templateresult;
                //visualMatch.MatchTemplatefilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Templatexml;
                //visualMatch.MatchRunfilepath = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch.Runxml;

                //UplookingCameraVisual.SetDirectLightintensity(visualMatch.DirectLightintensity);
                //UplookingCameraVisual.SetRingLightintensity(visualMatch.RingLightintensity);

                visualMatch.SetVisualParam(_systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch);

                title = "创建激光传感器识别";
            }


            //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            //BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            int Done = ShowVisualForm(visualMatch, name, title);

            if (Done == 0)
            {
                return false;
            }
            else
            {
                param1 = visualMatch.GetVisualParam();
            }


            //AutofocusAsync(EnumCameraType.UplookingCamera, AutofocusRange, AutofocusInterval);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match = param1;

                param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match;

            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                currentppTool.UplookingIdentifyPPtoolMatch = param1;

                param = currentppTool.UplookingIdentifyPPtoolMatch;

            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match = param1;

                param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match;
            }
            else if (maskType == EnumMaskType.LookupLaserSensorOrigion)
            {
                _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch = param1;

                param = _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch;
            }
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            return true;

        }


        /// <summary>
        /// 仰视相机识别榜头旋转补偿
        /// </summary>
        /// <param name="maskType"></param>
        /// <param name="param"></param>
        private bool UplookingCameraIdentifyMarkRotation(EnumMaskType maskType, MatchIdentificationParam param)
        {


            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
            double SubmountPPZ = 0;
            if (DeviceMode == 0)
            {
                SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);
            }

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                BondX = config.LookupChipPPOrigion.X;
                BondY = config.LookupChipPPOrigion.Y;
                BondZ = config.LookupChipPPOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                BondX = config.LookupChipPPOrigion.X + currentppTool.PPAndUCtoolOffset.X;
                BondY = config.LookupChipPPOrigion.Y + currentppTool.PPAndUCtoolOffset.Y;
                BondZ = config.LookupChipPPOrigion.Z + currentppTool.PPAndUCtoolOffset.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                BondX = config.LookupSubmountPPOrigion.X;
                BondY = config.LookupSubmountPPOrigion.Y;
                BondZ = config.LookupSubmountPPOrigion.Z;
                SubmountPPZ = config.SubmountPPWorkZ;
            }

            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, SubmountPPZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);
            }



            bool Done = IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);

            if(Done == false)
            {
                while (!Done)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {
                        Done = IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }

            double BondX0 = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY0 = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ0 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 180);
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 180);
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 180);
            }

            //Task.Factory.StartNew(new Action(() =>
            //{
            Done = IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);

            if (Done == false)
            {
                while(!Done)
                {
                    int result1 = ShowMessageAsync("动作确认", "识别失败！调整位置后点击<确认>按钮再次识别", "提示");
                    if (result1 == 1)
                    {
                        Done = IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }

            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }

            double BondX180 = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY180 = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ180 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };
                
                _systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                _systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };
                
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
            }
            else if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                currentppTool.ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                currentppTool.ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "SubmountPP").ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "SubmountPP").ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                _systemConfig.CalibrationConfig.SubmountPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                _systemConfig.CalibrationConfig.SubmountPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);

            }

           
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);


            return true;

        }

        /// <summary>
        /// 仰视相机识别榜头旋转补偿
        /// </summary>
        /// <param name="maskType"></param>
        /// <param name="param"></param>
        private bool UplookingCameraIdentifyMarkRotationManual(EnumMaskType maskType, MatchIdentificationParam param)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(1);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
            double SubmountPPZ = 0;
            if (DeviceMode == 0)
            {
                if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);
            }

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                BondX = config.LookupChipPPOrigion.X;
                BondY = config.LookupChipPPOrigion.Y;
                BondZ = config.LookupChipPPOrigion.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                BondX = config.LookupChipPPOrigion.X + currentppTool.PPAndUCtoolOffset.X;
                BondY = config.LookupChipPPOrigion.Y + currentppTool.PPAndUCtoolOffset.Y;
                BondZ = config.LookupChipPPOrigion.Z + currentppTool.PPAndUCtoolOffset.Z;
                SubmountPPZ = config.SubmountPPFreeZ;
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                BondX = config.LookupSubmountPPOrigion.X;
                BondY = config.LookupSubmountPPOrigion.Y;
                BondZ = config.LookupSubmountPPOrigion.Z;
                SubmountPPZ = config.SubmountPPWorkZ;
            }

            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, SubmountPPZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
            

            //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

            //BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

            //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);

                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "芯片吸嘴移动到仰视相机中心", "提示");
                if (result1 == 1)
                {
                    if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);

                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "芯片吸嘴移动到仰视相机中心", "提示");
                if (result1 == 1)
                {
                    if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);


                    config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);

                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "基底吸嘴移动到仰视相机中心", "提示");
                if (result1 == 1)
                {
                    //SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    //config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }
            }



            //IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);

            double BondX0 = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY0 = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ0 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 180);

                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "芯片吸嘴移动到仰视相机中心", "提示");
                if (result1 == 1)
                {
                    if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }
            }
            else if (maskType == EnumMaskType.LookupChipPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 180);

                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "芯片吸嘴移动到仰视相机中心", "提示");
                if (result1 == 1)
                {
                    if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 180);

                //ShowStage();



                int result1 = ShowMessageAsync("动作确认", "基底吸嘴移动到仰视相机中心", "提示");
                if (result1 == 1)
                {
                    if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    config.SubmountPPWorkZ = SubmountPPZ;
                }
                else
                {
                    return false;
                }
            }


            //IdentificationMoveAsync(EnumCameraType.UplookingCamera, param);

            double BondX180 = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY180 = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ180 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                _systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                _systemConfig.CalibrationConfig.ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
            }
            else if (maskType == EnumMaskType.LookupUCtoolOrigion)
            {
                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "ChipPP").ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                currentppTool.ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                currentppTool.ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);
            }
            else if (maskType == EnumMaskType.LookupSubmountPPOrigion)
            {
                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "SubmountPP").ChipPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                //_systemConfig.PPToolSettings.FirstOrDefault(s => s.PPName == "SubmountPP").ChipPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                _systemConfig.CalibrationConfig.SubmountPPPosCompensateCoordinate1 = new XYZTCoordinateConfig() { X = BondX0, Y = BondY0, Z = BondZ0 };

                _systemConfig.CalibrationConfig.SubmountPPPosCompensateCoordinate2 = new XYZTCoordinateConfig() { X = BondX180, Y = BondY180, Z = BondZ180 };

                AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);

            }

            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            return true;
        }


        /// <summary>
        /// 相机分辨率和角度计算
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="maskType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private (double pixelsizeX, double pixelsizeY, double angle) CameraParamCalibrationAsync(EnumCameraType camera, MatchIdentificationParam param)
        {
            if (camera == EnumCameraType.BondCamera)
            {
                List<(double xs, double ys)> XYPosition = GetRectangleCoordinates(_systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionArrayRowNum, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionArrayColNum, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionArrayWidthRange, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionArrayHeightRange);

                List<(double pixelx, double pixely)> XYPixel = new List<(double pixelx, double pixely)>();

                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);

                foreach (var coord in XYPosition)
                {
                    BondX = config.TrackOrigion.X + coord.xs;
                    BondY = config.TrackOrigion.Y + coord.ys;

                    AxisAbsoluteMove(EnumStageAxis.BondX, BondX);

                    AxisAbsoluteMove(EnumStageAxis.BondY, BondY);

                    MatchResult match = IdentificationAsync(camera, param);

                    if (match == null)
                    {
                        int result1 = ShowMessageAsync("识别失败", "未识别到轨道mark", "提示");
                        return (1, 1, 0);
                    }

                    XYPixel.Add((match.MatchBox.Benchmark.X, match.MatchBox.Benchmark.Y));

                }

                (double pixelsizeX, double pixelsizeY, double angle) = CalculateCameraParameters(XYPosition, XYPixel, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionArrayRowNum, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionArrayColNum);

                return (pixelsizeX, pixelsizeY, angle);
            }

            if (camera == EnumCameraType.WaferCamera)
            {
                List<(double xs, double ys)> XYPosition = GetRectangleCoordinates(_systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionArrayRowNum, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionArrayRowNum, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionArrayWidthRange, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionArrayHeightRange);

                List<(double pixelx, double pixely)> XYPixel = new List<(double pixelx, double pixely)>();

                double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
                double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);

                foreach (var coord in XYPosition)
                {
                    WaferTableX = config.WaferOrigion.X + coord.xs;
                    WaferTableY = config.WaferOrigion.Y + coord.ys;

                    AxisAbsoluteMove(EnumStageAxis.WaferTableX, WaferTableX);

                    AxisAbsoluteMove(EnumStageAxis.WaferTableY, WaferTableY);

                    MatchResult match = IdentificationAsync(camera, param);

                    if (match == null)
                    {
                        int result1 = ShowMessageAsync("识别失败", "未识别到晶圆盘mark", "提示");
                        return (1, 1, 0);
                    }

                    XYPixel.Add((match.MatchBox.Benchmark.X, match.MatchBox.Benchmark.Y));

                }

                (double pixelsizeX, double pixelsizeY, double angle) = CalculateCameraParameters(XYPosition, XYPixel, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionArrayRowNum, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionArrayRowNum);

                return (pixelsizeX, pixelsizeY, angle);
            }

            if (camera == EnumCameraType.UplookingCamera)
            {
                List<(double xs, double ys)> XYPosition = GetRectangleCoordinates(_systemConfig.SystemCalibrationConfig.UplookingIdentifyBondArrayRowNum, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBondArrayColNum, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBondArrayWidthRange, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBondArrayHeightRange);

                List<(double pixelx, double pixely)> XYPixel = new List<(double pixelx, double pixely)>();

                double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);

                foreach (var coord in XYPosition)
                {
                    BondX = config.LookupChipPPOrigion.X + coord.xs;
                    BondY = config.LookupChipPPOrigion.Y + coord.ys;

                    AxisAbsoluteMove(EnumStageAxis.BondX, BondX);

                    AxisAbsoluteMove(EnumStageAxis.BondY, BondY);

                    MatchResult match = IdentificationAsync(camera, param);

                    if (match == null)
                    {
                        int result1 = ShowMessageAsync("识别失败", "未识别到UC", "提示");
                        return (1, 1, 0);
                    }

                    XYPixel.Add((match.MatchBox.Benchmark.X, match.MatchBox.Benchmark.Y));

                }

                (double pixelsizeX, double pixelsizeY, double angle) = CalculateCameraParameters(XYPosition, XYPixel, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBondArrayRowNum, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBondArrayColNum);

                return (pixelsizeX, pixelsizeY, angle);
            }

            return (1, 1, 0);

        }

        /// <summary>
        /// 榜头旋转补偿计算
        /// </summary>
        /// <param name="maskType"></param>
        /// <param name="param"></param>
        private bool RotationCompensation(EnumMaskType maskType, MatchIdentificationParam param)
        {
            return UplookingCameraIdentifyMarkRotation(maskType, param);

        }

        /// <summary>
        /// 榜头旋转补偿计算
        /// </summary>
        /// <param name="maskType"></param>
        /// <param name="param"></param>
        private bool RotationCompensationManual(EnumMaskType maskType, MatchIdentificationParam param)
        {
            return UplookingCameraIdentifyMarkRotationManual(maskType, param);

        }




        /// <summary>
        /// 榜头到安全位置
        /// </summary>
        private bool BondToSafeAsync(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 0)
            {
                if (DeviceMode == 0)
                {
                    //AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                }
                else
                {
                    //_boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }
                

                double BondX = config.BondSafeLocation.X;
                double BondY = config.BondSafeLocation.Y;
                double BondZ = config.BondSafeLocation.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                BondXYZAbsoluteMove(BondX, BondY, BondZ);

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);

                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);
                }
                    


                Done = true;
            }
            else if (Mode == 1)
            {
                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                }
                else
                {
                    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }

                double BondX = config.BondSafeLocation.X;
                double BondY = config.BondSafeLocation.Y;
                double BondZ = config.BondSafeLocation.Z;

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                BondXYZAbsoluteMove(BondX, BondY, BondZ);

                AxisAbsoluteMove(EnumStageAxis.ChipPPT, 0);

                if (DeviceMode == 0)
                {
                    AxisAbsoluteMove(EnumStageAxis.SubmountPPT, 0);
                }

                Done = true;
            }
            else if (Mode == 2)
            {
                //ShowStage();

                int result1 = ShowMessageAsync("动作确认", "移动到安全位置", "提示");
                if (result1 == 1)
                {
                    double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    config.BondSafeLocation.X = BondX;
                    config.BondSafeLocation.Y = BondY;
                    config.BondSafeLocation.Z = BondZ;
                    Done = true;
                }
                else
                {
                    return Done;
                }

                if (DeviceMode == 0)
                {
                    double SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                    result1 = ShowMessageAsync("动作确认", "基底吸嘴移动到空闲位置", "提示");
                    if (result1 == 1)
                    {
                        if(DeviceMode == 0) SubmountPPZ = ReadCurrentAxisposition(EnumStageAxis.SubmountPPZ);

                        config.SubmountPPFreeZ = SubmountPPZ;
                        Done = true;
                    }
                    else
                    {
                        return Done;
                    }

                    //ShowMessage("动作确认", "移动到安全位置", "提示", (done) =>
                    //{
                    //    if (done == 1)
                    //    {
                    //        double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    //        double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    //        double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    //        config.BondSafeLocation.X = BondX;
                    //        config.BondSafeLocation.Y = BondY;
                    //        config.BondSafeLocation.Z = BondZ;
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }
                    //});

                }


            }
            return Done;

        }

        /// <summary>
        /// 榜头测高
        /// </summary>
        /// <param name="BondTool"> 1 榜头1 2 榜头2 3 激光传感器</param>
        /// <param name="Auto"> true 自动 false 手动</param>
        private bool BondToHeightmeasurementposition(int BondTool, EnumMaskType maskType, int Mode = 0)
        {
            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
            double BondZ1 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            double MaskBondX = 0;
            double MaskBondY = 0;
            double MaskBondZ = 0;

            string text1 = "";
            string text2 = "";

            AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);
            if (DeviceMode == 0)
                AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            else if (DeviceMode == 1)
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

            if (maskType == EnumMaskType.TrackOrigion)
            {
                MaskBondX = config.TrackOrigion.X;
                MaskBondY = config.TrackOrigion.Y;
                MaskBondZ = config.TrackOrigion.Z;

                text1 = "轨道原点测高";

                //if(Mode == 0)
                //{
                //    ShowDynamometer2();
                //}
                //else
                //{
                //    ShowDynamometer();
                //}

                
            }
            else if (maskType == EnumMaskType.WaferCameraOrigion)
            {
                MaskBondX = config.WaferCameraOrigion.X;
                MaskBondY = config.WaferCameraOrigion.Y;
                MaskBondZ = config.BondSafeLocation.Z + BondZOffset;

                text1 = "晶圆相机中心测高";
            }
            else if (maskType == EnumMaskType.LookupCameraOrigion)
            {
                MaskBondX = config.LookupCameraOrigion.X;
                MaskBondY = config.LookupCameraOrigion.Y;
                MaskBondZ = config.LookupCameraOrigion.Z;

                text1 = "仰视相机中心测高";

            }
            else if (maskType == EnumMaskType.EutecticWeldingOrigion)
            {
                MaskBondX = config.EutecticWeldingLocation.X;
                MaskBondY = config.EutecticWeldingLocation.Y;
                MaskBondZ = config.EutecticWeldingLocation.Z;

                text1 = "共晶台中心测高";
            }

            if (BondTool == 1)
            {
                BondX = MaskBondX + config.PP1AndBondCameraOffset.X;
                BondY = MaskBondY + config.PP1AndBondCameraOffset.Y;
                BondZ = MaskBondZ + BondZOffset;
                if((MaskBondZ + config.PP1AndBondCameraOffset.Z) > (Zmin + BondZOffset1))
                {
                    BondZ1 = MaskBondZ + config.PP1AndBondCameraOffset.Z + BondZOffset1;
                }
                else
                {
                    BondZ1 = config.BondSafeLocation.Z;
                }


                text2 = "uc工具到" + text1;

                //AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            }
            else if (BondTool == 0)
            {
                BondX = MaskBondX + config.PP1AndBondCameraOffset.X + currentppTool.PPAndUCtoolOffset.X;
                BondY = MaskBondY + config.PP1AndBondCameraOffset.Y + currentppTool.PPAndUCtoolOffset.Y;
                BondZ = MaskBondZ + BondZOffset;
                if ((MaskBondZ + config.PP1AndBondCameraOffset.Z) > (Zmin + BondZOffset1))
                {
                    BondZ1 = MaskBondZ + config.PP1AndBondCameraOffset.Z + BondZOffset1;
                }
                else
                {
                    BondZ1 = config.BondSafeLocation.Z;
                }


                text2 = "芯片吸嘴到" + text1;

                //AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
            }
            else if (BondTool == 2)
            {
                BondX = MaskBondX + config.PP2AndBondCameraOffset.X;
                BondY = MaskBondY + config.PP2AndBondCameraOffset.Y;
                BondZ = MaskBondZ + BondZOffset;
                if ((MaskBondZ + config.PP2AndBondCameraOffset.Z) > (Zmin + BondZOffset1))
                {
                    BondZ1 = MaskBondZ + config.PP2AndBondCameraOffset.Z + BondZOffset1;
                }
                else
                {
                    BondZ1 = config.BondSafeLocation.Z;
                }

                text2 = "基底吸嘴到" + text1;

                //AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPWorkZ);
            }
            else if (BondTool == 3)
            {
                BondX = MaskBondX + config.LaserSensorAndBondCameraOffset.X;
                BondY = MaskBondY + config.LaserSensorAndBondCameraOffset.Y;
                BondZ = MaskBondZ;

                text2 = "激光传感器到" + text1;
            }




            if (Mode == 0)
            {
                AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);

                if (BondTool == 1)
                {
                    if (DeviceMode == 0)
                        AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                    else if (DeviceMode == 1)
                        _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }
                if (BondTool == 0)
                {
                    if (DeviceMode == 0)
                        AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                    else if (DeviceMode == 1)
                        _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }
                else if (BondTool == 2)
                {

                    AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPWorkZ);
                }
                else if (BondTool == 3) 
                {
                    if (DeviceMode == 0)
                        AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                    else if (DeviceMode == 1)
                        _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }

                BondXYZAbsoluteMove(BondX, BondY, config.BondSafeLocation.Z);


                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);

                //ShowStage();

                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ1);

                bool Done = false;
                bool IsDone = false;

                //double InitZnumRead = Dynamometerform.ReadValue();
                //InitZnumRead = Dynamometerform.ReadValue();
                //InitZnumRead = Dynamometerform.ReadValue();

                double InitZnumRead = DataModel.Instance.PressureValue1;

                if (InitZnumRead < 0)
                {
                    return false;
                }

                while (!Done)
                {
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (BondZ2 < Zmin)
                    {
                        Done = false;
                        break;
                    }

                    AxisRelativeMove(EnumStageAxis.BondZ, -AutoHeightInterval);
                    Thread.Sleep(20);

                    //double ZnumRead = Dynamometerform.ReadValue();
                    double ZnumRead = DataModel.Instance.PressureValue1;
                    double[] pressure;
                    pressure = DynamometerManager.Instance.GetCurrentHardware().ReadAllValue();
                    if (pressure?.Length > 1)
                    {
                        DataModel.Instance.PressureValue1 = pressure[0];
                        DataModel.Instance.PressureValue2 = pressure[1];
                        ZnumRead = DataModel.Instance.PressureValue1;
                    }

                    LogRecorder.RecordLog(EnumLogContentType.Info, "BondZ:" + BondZ2);
                    LogRecorder.RecordLog(EnumLogContentType.Info, "DynamometerNum:" + ZnumRead);

                    if (ZnumRead > InitZnumRead + 10)
                    {
                        Done = true;
                        break;
                    }
                }

                if(Done == false)
                {
                    return false;
                }

                Done = false;
                PositioningSystem.Instance.SetAxisSpeed(EnumStageAxis.BondZ, 5);
                while (!Done)
                {
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (BondZ2 > Zmax)
                    {
                        Done = false;
                        break;
                    }

                    AxisRelativeMove(EnumStageAxis.BondZ, AutoHeightInterval/10);

                    Thread.Sleep(20);

                    //double ZnumRead = Dynamometerform.ReadValue();

                    double ZnumRead = DataModel.Instance.PressureValue1;
                    double[] pressure;
                    pressure = DynamometerManager.Instance.GetCurrentHardware().ReadAllValue();
                    if (pressure?.Length > 1)
                    {
                        DataModel.Instance.PressureValue1 = pressure[0];
                        DataModel.Instance.PressureValue2 = pressure[1];
                        ZnumRead = DataModel.Instance.PressureValue1;
                    }

                    LogRecorder.RecordLog(EnumLogContentType.Info, "BondZ:" + BondZ2);
                    LogRecorder.RecordLog(EnumLogContentType.Info, "DynamometerNum:" + ZnumRead);

                    if (ZnumRead <= InitZnumRead + 1)
                    {
                        Done = true;
                        break;
                    }
                }

                if (Done == false)
                {
                    return false;
                }

                Done = false;
                PositioningSystem.Instance.SetAxisSpeed(EnumStageAxis.BondZ, 2);
                while (!Done)
                {
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (BondZ2 < Zmin)
                    {
                        Done = false;
                        break;
                    }

                    AxisRelativeMove(EnumStageAxis.BondZ, -AutoHeightInterval / 50);

                    Thread.Sleep(20);

                    //double ZnumRead = Dynamometerform.ReadValue();

                    double ZnumRead = DataModel.Instance.PressureValue1;
                    double[] pressure;
                    pressure = DynamometerManager.Instance.GetCurrentHardware().ReadAllValue();
                    if (pressure?.Length > 1)
                    {
                        DataModel.Instance.PressureValue1 = pressure[0];
                        DataModel.Instance.PressureValue2 = pressure[1];
                        ZnumRead = DataModel.Instance.PressureValue1;
                    }

                    LogRecorder.RecordLog(EnumLogContentType.Info, "BondZ:" + BondZ2);
                    LogRecorder.RecordLog(EnumLogContentType.Info, "DynamometerNum:" + ZnumRead);

                    if (ZnumRead > InitZnumRead + 1)
                    {
                        Done = true;
                        break;
                    }
                }

                if (Done == false)
                {
                    return false;
                }

                Done = false;
                PositioningSystem.Instance.SetAxisSpeed(EnumStageAxis.BondZ, 2);
                while (!Done)
                {
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (BondZ2 > Zmax)
                    {
                        Done = false;
                        break;
                    }

                    AxisRelativeMove(EnumStageAxis.BondZ, AutoHeightInterval / 50);

                    Thread.Sleep(20);

                    //double ZnumRead = Dynamometerform.ReadValue();

                    double ZnumRead = DataModel.Instance.PressureValue1;
                    double[] pressure;
                    pressure = DynamometerManager.Instance.GetCurrentHardware().ReadAllValue();
                    if (pressure?.Length > 1)
                    {
                        DataModel.Instance.PressureValue1 = pressure[0];
                        DataModel.Instance.PressureValue2 = pressure[1];
                        ZnumRead = DataModel.Instance.PressureValue1;
                    }

                    LogRecorder.RecordLog(EnumLogContentType.Info, "BondZ:" + BondZ2);
                    LogRecorder.RecordLog(EnumLogContentType.Info, "DynamometerNum:" + ZnumRead);

                    if (ZnumRead <= InitZnumRead + 1)
                    {
                        Done = true;
                        break;
                    }
                }

                if (Done == false)
                {
                    return false;
                }

                Done = false;
                PositioningSystem.Instance.SetAxisSpeed(EnumStageAxis.BondZ, 1);
                while (!Done)
                {
                    double BondZ2 = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (BondZ2 < Zmin)
                    {
                        Done = false;
                        break;
                    }

                    AxisRelativeMove(EnumStageAxis.BondZ, -AutoHeightInterval / 100);

                    Thread.Sleep(20);

                    //double ZnumRead = Dynamometerform.ReadValue();

                    double ZnumRead = DataModel.Instance.PressureValue1;
                    double[] pressure;
                    pressure = DynamometerManager.Instance.GetCurrentHardware().ReadAllValue();
                    if (pressure?.Length > 1)
                    {
                        DataModel.Instance.PressureValue1 = pressure[0];
                        DataModel.Instance.PressureValue2 = pressure[1];
                        ZnumRead = DataModel.Instance.PressureValue1;
                    }

                    LogRecorder.RecordLog(EnumLogContentType.Info, "BondZ:" + BondZ2);
                    LogRecorder.RecordLog(EnumLogContentType.Info, "DynamometerNum:" + ZnumRead);

                    if (ZnumRead > InitZnumRead + 1)
                    {
                        Done = true;
                        break;
                    }
                }

                PositioningSystem.Instance.SetAxisSpeed(EnumStageAxis.BondZ, 10);

                int result1= 0;

                if (Done == false)
                {
                    return false;
                }
                else
                {
                    result1 = 1;
                }

                
                if (result1 == 1)
                {
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (maskType == EnumMaskType.TrackOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.TrackChipPPOrigion.X = BondX;
                            config.TrackChipPPOrigion.Y = BondY;
                            config.TrackChipPPOrigion.Z = BondZ;
                        }
                        else if(BondTool == 0)
                        {
                            currentppTool.PPAndUCtoolOffset.Z = BondZ - config.TrackChipPPOrigion.Z;
                        }
                        else if (BondTool == 2)
                        {
                            config.TrackSubmountPPOrigion.X = BondX;
                            config.TrackSubmountPPOrigion.Y = BondY;
                            config.TrackSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.TrackLaserSensorOrigion.X = BondX;
                            config.TrackLaserSensorOrigion.Y = BondY;
                            config.TrackLaserSensorOrigion.Z = BondZ;

                            //config.TrackLaserSensorZ = ReadLaserSensor();
                            
                            double distance = -1;
                            distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                            if (distance >= 0)
                            {
                                DataModel.Instance.LaserValue = distance / 10000.0f;
                            }
                            else
                            {
                                DataModel.Instance.LaserValue = 0;
                            }
                            config.TrackLaserSensorZ = DataModel.Instance.LaserValue;

                            if (config.TrackLaserSensorZ < 64.9 || config.TrackLaserSensorZ > 105.1)
                            {
                                return false;
                            }

                        }
                    }
                    else if (maskType == EnumMaskType.WaferCameraOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.WaferChipPPOrigion.X = BondX;
                            config.WaferChipPPOrigion.Y = BondY;
                            config.WaferChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.WaferSubmountPPOrigion.X = BondX;
                            config.WaferSubmountPPOrigion.Y = BondY;
                            config.WaferSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.WaferLaserSensorOrigion.X = BondX;
                            config.WaferLaserSensorOrigion.Y = BondY;
                            config.WaferLaserSensorOrigion.Z = BondZ;

                        }
                    }
                    else if (maskType == EnumMaskType.LookupCameraOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.LookupChipPPOrigion.X = BondX;
                            config.LookupChipPPOrigion.Y = BondY;
                            config.LookupChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.LookupSubmountPPOrigion.X = BondX;
                            config.LookupSubmountPPOrigion.Y = BondY;
                            config.LookupSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.LookupLaserSensorOrigion.X = BondX;
                            config.LookupLaserSensorOrigion.Y = BondY;
                            config.LookupLaserSensorOrigion.Z = BondZ;

                        }
                    }
                    else if (maskType == EnumMaskType.EutecticWeldingOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.EutecticWeldingChipPPLocation.X = BondX;
                            config.EutecticWeldingChipPPLocation.Y = BondY;
                            config.EutecticWeldingChipPPLocation.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.EutecticWeldingSubmountPPLocation.X = BondX;
                            config.EutecticWeldingSubmountPPLocation.Y = BondY;
                            config.EutecticWeldingSubmountPPLocation.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            double distance = -1;
                            distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                            if (distance >= 0)
                            {
                                DataModel.Instance.LaserValue = distance / 10000.0f;
                            }
                            else
                            {
                                DataModel.Instance.LaserValue = 0;
                            }
                            //double Znum = ReadLaserSensor();
                            double Znum = DataModel.Instance.LaserValue;

                            if (Znum < 64.9 || Znum > 105.1)
                            {
                                return false;
                            }
                            else
                            {
                                config.EutecticWeldingLaserSensorLocation.X = BondX;
                                config.EutecticWeldingLaserSensorLocation.Y = BondY;
                                config.EutecticWeldingLaserSensorLocation.Z = BondZ;



                                config.EutecticWeldingChipPPLocation.X = config.EutecticWeldingLocation.X + config.PP1AndBondCameraOffset.X;
                                config.EutecticWeldingChipPPLocation.Y = config.EutecticWeldingLocation.Y + config.PP1AndBondCameraOffset.Y;
                                config.EutecticWeldingChipPPLocation.Z = config.EutecticWeldingLocation.Z - Znum + config.TrackLaserSensorZ + config.PP1AndBondCameraOffset.Z;

                                config.EutecticWeldingSubmountPPLocation.X = config.EutecticWeldingLocation.X + config.PP2AndBondCameraOffset.X;
                                config.EutecticWeldingSubmountPPLocation.Y = config.EutecticWeldingLocation.Y + config.PP2AndBondCameraOffset.Y;
                                config.EutecticWeldingSubmountPPLocation.Z = config.EutecticWeldingLocation.Z - Znum + config.TrackLaserSensorZ + config.PP2AndBondCameraOffset.Z;
                            }

                            

                        }
                    }

                }
                else
                {
                    return false;
                }

            }
            else if (Mode == 1)
            {
                AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);

                if (BondTool == 1)
                {
                    if (DeviceMode == 0)
                        AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                    else if (DeviceMode == 1)
                        _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }
                if (BondTool == 0)
                {
                    if (DeviceMode == 0)
                        AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                    else if (DeviceMode == 1)
                        _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }
                else if (BondTool == 2)
                {

                    AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPWorkZ);
                }
                else if (BondTool == 3)
                {
                    if (DeviceMode == 0)
                        AxisAbsoluteMove(EnumStageAxis.SubmountPPZ, config.SubmountPPFreeZ);
                    else if (DeviceMode == 1)
                        _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                }

                BondXYZAbsoluteMove(BondX, BondY, config.BondSafeLocation.Z);


                AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);


                //BondXYZAbsoluteMove(BondX, BondY, BondZ);

                //ShowStage();

                int result1 = ShowMessageAsync("动作确认", text2, "提示");
                if (result1 == 1)
                {
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (maskType == EnumMaskType.TrackOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.TrackChipPPOrigion.X = BondX;
                            config.TrackChipPPOrigion.Y = BondY;
                            config.TrackChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 0)
                        {
                            currentppTool.PPAndUCtoolOffset.Z = BondZ - config.TrackChipPPOrigion.Z;
                        }
                        else if (BondTool == 2)
                        {
                            config.TrackSubmountPPOrigion.X = BondX;
                            config.TrackSubmountPPOrigion.Y = BondY;
                            config.TrackSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.TrackLaserSensorOrigion.X = BondX;
                            config.TrackLaserSensorOrigion.Y = BondY;
                            config.TrackLaserSensorOrigion.Z = BondZ;

                            double distance = -1;
                            distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                            if (distance >= 0)
                            {
                                DataModel.Instance.LaserValue = distance / 10000.0f;
                            }
                            else
                            {
                                DataModel.Instance.LaserValue = 0;
                            }
                            //config.TrackLaserSensorZ = ReadLaserSensor();
                            config.TrackLaserSensorZ = DataModel.Instance.LaserValue;

                            if (config.TrackLaserSensorZ < 64.9 || config.TrackLaserSensorZ > 105.1)
                            {
                                return false;
                            }

                        }
                    }
                    else if (maskType == EnumMaskType.WaferCameraOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.WaferChipPPOrigion.X = BondX;
                            config.WaferChipPPOrigion.Y = BondY;
                            config.WaferChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.WaferSubmountPPOrigion.X = BondX;
                            config.WaferSubmountPPOrigion.Y = BondY;
                            config.WaferSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.WaferLaserSensorOrigion.X = BondX;
                            config.WaferLaserSensorOrigion.Y = BondY;
                            config.WaferLaserSensorOrigion.Z = BondZ;

                        }
                    }
                    else if (maskType == EnumMaskType.LookupCameraOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.LookupChipPPOrigion.X = BondX;
                            config.LookupChipPPOrigion.Y = BondY;
                            config.LookupChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.LookupSubmountPPOrigion.X = BondX;
                            config.LookupSubmountPPOrigion.Y = BondY;
                            config.LookupSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.LookupLaserSensorOrigion.X = BondX;
                            config.LookupLaserSensorOrigion.Y = BondY;
                            config.LookupLaserSensorOrigion.Z = BondZ;

                        }
                    }
                    else if (maskType == EnumMaskType.EutecticWeldingOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.EutecticWeldingChipPPLocation.X = BondX;
                            config.EutecticWeldingChipPPLocation.Y = BondY;
                            config.EutecticWeldingChipPPLocation.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.EutecticWeldingSubmountPPLocation.X = BondX;
                            config.EutecticWeldingSubmountPPLocation.Y = BondY;
                            config.EutecticWeldingSubmountPPLocation.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            double distance = -1;
                            distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                            if (distance >= 0)
                            {
                                DataModel.Instance.LaserValue = distance / 10000.0f;
                            }
                            else
                            {
                                DataModel.Instance.LaserValue = 0;
                            }
                            //double Znum = ReadLaserSensor();
                            double Znum = DataModel.Instance.LaserValue;

                            if (Znum < 64.9 || Znum > 105.1)
                            {
                                return false;
                            }
                            else
                            {
                                config.EutecticWeldingLaserSensorLocation.X = BondX;
                                config.EutecticWeldingLaserSensorLocation.Y = BondY;
                                config.EutecticWeldingLaserSensorLocation.Z = BondZ;



                                config.EutecticWeldingChipPPLocation.X = config.EutecticWeldingLocation.X + config.PP1AndBondCameraOffset.X;
                                config.EutecticWeldingChipPPLocation.Y = config.EutecticWeldingLocation.Y + config.PP1AndBondCameraOffset.Y; ;
                                config.EutecticWeldingChipPPLocation.Z = config.EutecticWeldingLocation.Z - Znum + config.TrackLaserSensorZ + config.PP1AndBondCameraOffset.Z;

                                config.EutecticWeldingSubmountPPLocation.X = config.EutecticWeldingLocation.X + config.PP2AndBondCameraOffset.X; ;
                                config.EutecticWeldingSubmountPPLocation.Y = config.EutecticWeldingLocation.Y + config.PP2AndBondCameraOffset.Y; ;
                                config.EutecticWeldingSubmountPPLocation.Z = config.EutecticWeldingLocation.Z - Znum + config.TrackLaserSensorZ + config.PP2AndBondCameraOffset.Z;
                            }

                        }
                    }

                }
                else
                {
                    return false;
                }

            }
            else if (Mode == 2)
            {
                //ShowStage();
                //ShowStageAxisMove();


                int result1 = ShowMessageAsync("动作确认", text2, "提示");
                if (result1 == 1)
                {
                    BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                    BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                    BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                    if (maskType == EnumMaskType.TrackOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.TrackChipPPOrigion.X = BondX;
                            config.TrackChipPPOrigion.Y = BondY;
                            config.TrackChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 0)
                        {
                            currentppTool.PPAndUCtoolOffset.Z = BondZ - config.TrackChipPPOrigion.Z;
                        }
                        else if (BondTool == 2)
                        {
                            config.TrackSubmountPPOrigion.X = BondX;
                            config.TrackSubmountPPOrigion.Y = BondY;
                            config.TrackSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.TrackLaserSensorOrigion.X = BondX;
                            config.TrackLaserSensorOrigion.Y = BondY;
                            config.TrackLaserSensorOrigion.Z = BondZ;

                            double distance = -1;
                            distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                            if (distance >= 0)
                            {
                                DataModel.Instance.LaserValue = distance / 10000.0f;
                            }
                            else
                            {
                                DataModel.Instance.LaserValue = 0;
                            }
                            //config.TrackLaserSensorZ = ReadLaserSensor();
                            config.TrackLaserSensorZ = DataModel.Instance.LaserValue;

                            if (config.TrackLaserSensorZ < 64.9 || config.TrackLaserSensorZ > 105.1)
                            {
                                return false;
                            }

                        }
                    }
                    else if (maskType == EnumMaskType.WaferCameraOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.WaferChipPPOrigion.X = BondX;
                            config.WaferChipPPOrigion.Y = BondY;
                            config.WaferChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.WaferSubmountPPOrigion.X = BondX;
                            config.WaferSubmountPPOrigion.Y = BondY;
                            config.WaferSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.WaferLaserSensorOrigion.X = BondX;
                            config.WaferLaserSensorOrigion.Y = BondY;
                            config.WaferLaserSensorOrigion.Z = BondZ;

                        }
                    }
                    else if (maskType == EnumMaskType.LookupCameraOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.LookupChipPPOrigion.X = BondX;
                            config.LookupChipPPOrigion.Y = BondY;
                            config.LookupChipPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.LookupSubmountPPOrigion.X = BondX;
                            config.LookupSubmountPPOrigion.Y = BondY;
                            config.LookupSubmountPPOrigion.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.LookupLaserSensorOrigion.X = BondX;
                            config.LookupLaserSensorOrigion.Y = BondY;
                            config.LookupLaserSensorOrigion.Z = BondZ;

                        }
                    }
                    else if (maskType == EnumMaskType.EutecticWeldingOrigion)
                    {
                        if (BondTool == 1)
                        {
                            config.EutecticWeldingChipPPLocation.X = BondX;
                            config.EutecticWeldingChipPPLocation.Y = BondY;
                            config.EutecticWeldingChipPPLocation.Z = BondZ;
                        }
                        else if (BondTool == 2)
                        {
                            config.EutecticWeldingSubmountPPLocation.X = BondX;
                            config.EutecticWeldingSubmountPPLocation.Y = BondY;
                            config.EutecticWeldingSubmountPPLocation.Z = BondZ;
                        }
                        else if (BondTool == 3)
                        {
                            config.EutecticWeldingLaserSensorLocation.X = BondX;
                            config.EutecticWeldingLaserSensorLocation.Y = BondY;
                            config.EutecticWeldingLaserSensorLocation.Z = BondZ;

                        }
                    }

                }
                else
                {
                    return false;
                }
            }

            AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);
            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);
            return true;
        }


        private bool BondToEpoxtSpot(int Mode = 0)
        {
            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);
            bool Done = false;

            int result0 = ShowMessage("动作确认", "请安装点胶针", "提示");
            if (result0 == 1)
            {
                AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);
                _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 1);

                if (Mode == 1)
                {
                    //ShowStage();

                    if (config.TrackEpoxtSpotCoordinate.X != 0 && config.TrackEpoxtSpotCoordinate.Y != 0 && config.TrackEpoxtSpotCoordinate.Z != 0)
                    {
                        BondX = config.TrackEpoxtSpotCoordinate.X;
                        BondY = config.TrackEpoxtSpotCoordinate.Y;
                        BondZ = config.TrackEpoxtSpotCoordinate.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

                        //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);
                    }

                    int result1 = ShowMessageAsync("动作确认", "移动到点胶位置进行点胶", "提示");
                    if (result1 == 1)
                    {
                        int result2 = ShowMessageAsync("动作确认", "确认点胶", "提示");
                        if (result2 == 1)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 1);
                            Thread.Sleep(50);
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 0);

                            BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                            BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                            BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                            config.TrackEpoxtSpotCoordinate.X = BondX;
                            config.TrackEpoxtSpotCoordinate.Y = BondY;
                            config.TrackEpoxtSpotCoordinate.Z = BondZ;
                        }
                        else
                        {
                            return Done;
                        }
                    }
                    else
                    {
                        return Done;
                    }


                    AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);
                    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

                    if (config.TrackBondCameraToEpoxtSpotCoordinate.X != 0 && config.TrackBondCameraToEpoxtSpotCoordinate.Y != 0 && config.TrackBondCameraToEpoxtSpotCoordinate.Z != 0)
                    {
                        BondX = config.TrackBondCameraToEpoxtSpotCoordinate.X;
                        BondY = config.TrackBondCameraToEpoxtSpotCoordinate.Y;
                        BondZ = config.TrackBondCameraToEpoxtSpotCoordinate.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);
                    }

                    result1 = ShowMessageAsync("动作确认", "榜头相机对准点胶位置", "提示");
                    if (result1 == 1)
                    {
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                        config.TrackBondCameraToEpoxtSpotCoordinate.X = BondX;
                        config.TrackBondCameraToEpoxtSpotCoordinate.Y = BondY;
                        config.TrackBondCameraToEpoxtSpotCoordinate.Z = BondZ;
                    }
                    else
                    {
                        return Done;
                    }


                }
                else if (Mode == 0)
                {
                    //ShowStage();

                    if (config.TrackEpoxtSpotCoordinate.X != 0 && config.TrackEpoxtSpotCoordinate.Y != 0 && config.TrackEpoxtSpotCoordinate.Z != 0)
                    {
                        BondX = config.TrackEpoxtSpotCoordinate.X;
                        BondY = config.TrackEpoxtSpotCoordinate.Y;
                        BondZ = config.TrackEpoxtSpotCoordinate.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

                        //AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);
                    }

                    int result1 = ShowMessageAsync("动作确认", "移动到点胶位置进行点胶", "提示");
                    if (result1 == 1)
                    {
                        int result2 = ShowMessageAsync("动作确认", "确认点胶", "提示");
                        if (result2 == 1)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 1);
                            Thread.Sleep(50);
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 0);

                            BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                            BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                            BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                            config.TrackEpoxtSpotCoordinate.X = BondX;
                            config.TrackEpoxtSpotCoordinate.Y = BondY;
                            config.TrackEpoxtSpotCoordinate.Z = BondZ;
                        }
                        else
                        {
                            return Done;
                        }
                    }
                    else
                    {
                        return Done;
                    }


                    AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);
                    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

                    if (config.TrackBondCameraToEpoxtSpotCoordinate.X != 0 && config.TrackBondCameraToEpoxtSpotCoordinate.Y != 0 && config.TrackBondCameraToEpoxtSpotCoordinate.Z != 0)
                    {
                        BondX = config.TrackBondCameraToEpoxtSpotCoordinate.X;
                        BondY = config.TrackBondCameraToEpoxtSpotCoordinate.Y;
                        BondZ = config.TrackBondCameraToEpoxtSpotCoordinate.Z;

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ + BondZOffset);

                        BondXYZAbsoluteMove(BondX, BondY, BondZ + BondZOffset);

                        AxisAbsoluteMove(EnumStageAxis.BondZ, BondZ);
                    }

                    result1 = ShowMessageAsync("动作确认", "榜头相机对准点胶位置", "提示");
                    if (result1 == 1)
                    {
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                        config.TrackBondCameraToEpoxtSpotCoordinate.X = BondX;
                        config.TrackBondCameraToEpoxtSpotCoordinate.Y = BondY;
                        config.TrackBondCameraToEpoxtSpotCoordinate.Z = BondZ;
                    }
                    else
                    {
                        return Done;
                    }
                }
                else if (Mode == 2)
                {
                    //ShowStage();

                    int result1 = ShowMessageAsync("动作确认", "移动到点胶位置进行点胶", "提示");
                    if (result1 == 1)
                    {
                        int result2 = ShowMessageAsync("动作确认", "确认点胶", "提示");
                        if (result2 == 1)
                        {
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 1);
                            Thread.Sleep(50);
                            _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtDIS, 0);

                            BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                            BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                            BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                            config.TrackEpoxtSpotCoordinate.X = BondX;
                            config.TrackEpoxtSpotCoordinate.Y = BondY;
                            config.TrackEpoxtSpotCoordinate.Z = BondZ;
                        }
                        else
                        {
                            return Done;
                        }
                    }
                    else
                    {
                        return Done;
                    }


                    AxisAbsoluteMove(EnumStageAxis.BondZ, config.BondSafeLocation.Z);
                    _boardCardController.IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);

                    result1 = ShowMessageAsync("动作确认", "榜头相机对准点胶位置", "提示");
                    if (result1 == 1)
                    {
                        BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
                        BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
                        BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

                        config.TrackBondCameraToEpoxtSpotCoordinate.X = BondX;
                        config.TrackBondCameraToEpoxtSpotCoordinate.Y = BondY;
                        config.TrackBondCameraToEpoxtSpotCoordinate.Z = BondZ;
                    }
                    else
                    {
                        return Done;
                    }
                }
            }
            else
            {
                return Done;
            }

            

            return true;
        }

        /// <summary>
        /// 榜头相机移动到轨道原点，自动对焦，识别轨道原点
        /// </summary>
        private bool BondCameraIdentifyTrackOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = BondCameraIdentifymarkpointsManual(EnumMaskType.TrackOrigion);
            }
            else if (Mode == 0)
            {
                Done = BondCameraIdentifymarkpoints(EnumMaskType.TrackOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch);
            }
            else if (Mode == 2)
            {
                Done = BondCameraIdentifymarkpointsManual2(EnumMaskType.TrackOrigion);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.TrackOrigion.X = BondX;
            config.TrackOrigion.Y = BondY;
            config.TrackOrigion.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 计算榜头相机分辨率和角度
        /// </summary>
        private bool BondCameraParamCalibration(int Mode = 0)
        {
            double pixelsizeX = 1;
            double pixelsizeY = 1;
            double angle = 0;
            bool confirm = false;
            if (Mode == 1)
            {
                (pixelsizeX, pixelsizeY, angle) = CameraParamCalibrationAsync(EnumCameraType.BondCamera, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch);
            }
            else if (Mode == 0)
            {
                (pixelsizeX, pixelsizeY, angle) = CameraParamCalibrationAsync(EnumCameraType.BondCamera, _systemConfig.SystemCalibrationConfig.BondIdentifyTrackOrigionMatch);
            }
            else if (Mode == 2)
            {
                (pixelsizeX, pixelsizeY, angle, confirm) = ShowCameraParamForm("bond相机参数设置", _BondcameraConfig.WidthPixelSize, _BondcameraConfig.HeightPixelSize, _BondcameraConfig.Angle);
                if (confirm == false)
                {
                    pixelsizeX = _BondcameraConfig.WidthPixelSize;
                    pixelsizeY = _BondcameraConfig.HeightPixelSize;
                    angle = _BondcameraConfig.Angle;
                }
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);

            _BondcameraConfig.WidthPixelSize = (float)pixelsizeX;
            _BondcameraConfig.HeightPixelSize = (float)pixelsizeY;
            _BondcameraConfig.Angle = (float)angle;

            if ((pixelsizeX > 0 && pixelsizeX != 1) && (pixelsizeY > 0 && pixelsizeY != 1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点
        /// </summary>
        private bool WaferCameraIdentifyWaferOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = WaferCameraIdentifymarkpointsManual(EnumMaskType.WaferOrigion);
            }
            else if (Mode == 0)
            {
                Done = WaferCameraIdentifymarkpoints(EnumMaskType.WaferOrigion, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);
            }
            else if (Mode == 2)
            {
                Done = WaferCameraIdentifymarkpointsManual2(EnumMaskType.WaferOrigion);
            }

            WaferCameraVisual.SetDirectLightintensity(0);
            WaferCameraVisual.SetRingLightintensity(0);

            double WaferTableX = ReadCurrentAxisposition(EnumStageAxis.WaferTableX);
            double WaferTableY = ReadCurrentAxisposition(EnumStageAxis.WaferTableY);
            double WaferTableZ = ReadCurrentAxisposition(EnumStageAxis.WaferTableZ);

            config.WaferOrigion.X = WaferTableX;
            config.WaferOrigion.Y = WaferTableY;
            config.WaferOrigion.Z = WaferTableZ;

            return Done;
        }

        /// <summary>
        /// 计算晶圆相机分辨率和角度
        /// </summary>
        private bool WaferCameraParamCalibration(int Mode = 0)
        {
            double pixelsizeX = 1;
            double pixelsizeY = 1;
            double angle = 0;
            bool confirm = false;
            if (Mode == 1)
            {
                (pixelsizeX, pixelsizeY, angle) = CameraParamCalibrationAsync(EnumCameraType.WaferCamera, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);
            }
            else if (Mode == 0)
            {
                (pixelsizeX, pixelsizeY, angle) = CameraParamCalibrationAsync(EnumCameraType.WaferCamera, _systemConfig.SystemCalibrationConfig.WaferIdentifyWaferOrigionMatch);
            }
            else if (Mode == 2)
            {
                (pixelsizeX, pixelsizeY, angle, confirm) = ShowCameraParamForm("Wafer相机参数设置", _WafercameraConfig.WidthPixelSize, _WafercameraConfig.HeightPixelSize, _WafercameraConfig.Angle);
                if (confirm == false)
                {
                    pixelsizeX = _WafercameraConfig.WidthPixelSize;
                    pixelsizeY = _WafercameraConfig.HeightPixelSize;
                    angle = _WafercameraConfig.Angle;
                }
            }

            WaferCameraVisual.SetDirectLightintensity(0);
            WaferCameraVisual.SetRingLightintensity(0);

            _WafercameraConfig.WidthPixelSize = (float)pixelsizeX;
            _WafercameraConfig.HeightPixelSize = (float)pixelsizeY;
            _WafercameraConfig.Angle = (float)angle;


           

            if ((pixelsizeX > 0 && pixelsizeX != 1) && (pixelsizeY > 0 && pixelsizeY != 1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 榜头相机移动到晶圆相机中心，自动对焦，识别晶圆原点
        /// </summary>
        private bool BondCameraIdentifyWaferCameraOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = BondCameraIdentifymarkpointsManual(EnumMaskType.WaferCameraOrigion);
            }
            else if (Mode == 0)
            {
                Done = BondCameraIdentifymarkpoints(EnumMaskType.WaferCameraOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyWaferCameraOrigionMatch);
            }
            else if (Mode == 2)
            {
                Done = BondCameraIdentifymarkpointsManual2(EnumMaskType.WaferCameraOrigion);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.WaferCameraOrigion.X = BondX;
            config.WaferCameraOrigion.Y = BondY;
            config.WaferCameraOrigion.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 榜头相机移动到固晶位置，自动对焦，识别固晶台中心
        /// </summary>
        private bool BondCameraIdentifyEutecticWeldingOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = BondCameraIdentifymarkpointsManual(EnumMaskType.EutecticWeldingOrigion);
            }
            else if (Mode == 0)
            {
                Done = BondCameraIdentifymarkpoints(EnumMaskType.EutecticWeldingOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyEutecticWeldingMatch);
            }
            else if (Mode == 2)
            {
                Done = BondCameraIdentifymarkpointsManual2(EnumMaskType.EutecticWeldingOrigion);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.EutecticWeldingLocation.X = BondX;
            config.EutecticWeldingLocation.Y = BondY;
            config.EutecticWeldingLocation.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 榜头相机移动到校准台位置，自动对焦，识别校准台中心
        /// </summary>
        private bool BondCameraIdentifyCalibrationTableOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = BondCameraIdentifymarkpointsManual(EnumMaskType.CalibrationTableOrigion);
            }
            else if (Mode == 0)
            {
                Done = BondCameraIdentifymarkpoints(EnumMaskType.CalibrationTableOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyCalibrationTableMatch);
            }
            else if (Mode == 2)
            {
                Done = BondCameraIdentifymarkpointsManual2(EnumMaskType.CalibrationTableOrigion);
            }

            //BondCameraVisual.SetDirectLightintensity(0);
            //BondCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.CalibrationTableOrigion.X = BondX;
            config.CalibrationTableOrigion.Y = BondY;
            config.CalibrationTableOrigion.Z = BondZ - config.TrackOrigion.Z;

            return Done;
        }


        /// <summary>
        /// 榜头移动到仰视相机中心，自动对焦，识别吸嘴工具
        /// </summary>
        private bool UplookingCameraIdentifyPPtool(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = UplookingCameraIdentifymarkpointsManual(EnumMaskType.LookupChipPPOrigion);
            }
            else if (Mode == 0)
            {
                Done = UplookingCameraIdentifymarkpoints(EnumMaskType.LookupChipPPOrigion, currentppTool.UplookingIdentifyPPtoolMatch);
            }
            else if (Mode == 2)
            {
                Done = UplookingCameraIdentifymarkpointsManual2(EnumMaskType.LookupChipPPOrigion);
            }

            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            currentppTool.PPAndUCtoolOffset.X = BondX - config.LookupChipPPOrigion.X;
            currentppTool.PPAndUCtoolOffset.Y = BondX - config.LookupChipPPOrigion.Y;
            currentppTool.PPAndUCtoolOffset.Z = BondX - config.LookupChipPPOrigion.Z;

            return Done;
        }


        /// <summary>
        /// 榜头移动到仰视相机中心，自动对焦，识别UC工具
        /// </summary>
        private bool UplookingCameraIdentifyBond1(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = UplookingCameraIdentifymarkpointsManual(EnumMaskType.LookupUCtoolOrigion);
            }
            else if (Mode == 0)
            {
                Done = UplookingCameraIdentifymarkpoints(EnumMaskType.LookupUCtoolOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);
            }
            else if (Mode == 2)
            {
                Done = UplookingCameraIdentifymarkpointsManual2(EnumMaskType.LookupUCtoolOrigion);
            }

            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.LookupChipPPOrigion.X = BondX;
            config.LookupChipPPOrigion.Y = BondY;
            config.LookupChipPPOrigion.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 榜头移动到仰视相机中心，自动对焦，识别UC工具
        /// </summary>
        private bool UplookingCameraIdentifyBond2(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = UplookingCameraIdentifymarkpointsManual(EnumMaskType.LookupSubmountPPOrigion);
            }
            else if (Mode == 0)
            {
                Done = UplookingCameraIdentifymarkpoints(EnumMaskType.LookupSubmountPPOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match);
            }
            else if (Mode == 2)
            {
                Done = UplookingCameraIdentifymarkpointsManual2(EnumMaskType.LookupSubmountPPOrigion);
            }

            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.LookupSubmountPPOrigion.X = BondX;
            config.LookupSubmountPPOrigion.Y = BondY;
            config.LookupSubmountPPOrigion.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 激光传感器到仰视相机中心，自动对焦，识别光斑
        /// </summary>
        private bool UplookingCameraIdentifyLaserSensor(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = UplookingCameraIdentifymarkpointsManual(EnumMaskType.LookupLaserSensorOrigion);
            }
            else if (Mode == 0)
            {
                Done = UplookingCameraIdentifymarkpoints(EnumMaskType.LookupLaserSensorOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyLaserSensorMatch);
            }
            else if (Mode == 2)
            {
                Done = UplookingCameraIdentifymarkpointsManual2(EnumMaskType.LookupLaserSensorOrigion);
            }

            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.LookupLaserSensorOrigion.X = BondX;
            config.LookupLaserSensorOrigion.Y = BondY;
            config.LookupLaserSensorOrigion.Z = BondZ;

            return Done;
        }


        /// <summary>
        /// 计算晶圆相机分辨率和角度
        /// </summary>
        private bool UplookingCameraParamCalibration(int Mode = 0)
        {
            double pixelsizeX = 1;
            double pixelsizeY = 1;
            double angle = 0;
            bool confirm = false;
            if (Mode == 1)
            {
                (pixelsizeX, pixelsizeY, angle) = CameraParamCalibrationAsync(EnumCameraType.UplookingCamera, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);
            }
            else if (Mode == 0)
            {
                (pixelsizeX, pixelsizeY, angle) = CameraParamCalibrationAsync(EnumCameraType.UplookingCamera, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);
            }
            else if (Mode == 2)
            {
                (pixelsizeX, pixelsizeY, angle, confirm) = ShowCameraParamForm("Uplooking相机参数设置", _UplookingcameraConfig.WidthPixelSize, _UplookingcameraConfig.HeightPixelSize, _UplookingcameraConfig.Angle);
                if (confirm == false)
                {
                    pixelsizeX = _UplookingcameraConfig.WidthPixelSize;
                    pixelsizeY = _UplookingcameraConfig.HeightPixelSize;
                    angle = _UplookingcameraConfig.Angle;
                }
            }

            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            _UplookingcameraConfig.WidthPixelSize = (float)pixelsizeX;
            _UplookingcameraConfig.HeightPixelSize = (float)pixelsizeY;
            _UplookingcameraConfig.Angle = (float)angle;

            if((pixelsizeX > 0 && pixelsizeX != 1) && (pixelsizeY > 0 && pixelsizeY != 1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 榜头相机移动到系统原点，自动对焦，识别系统原点
        /// </summary>
        private bool BondCameraIdentifyBondOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = BondCameraIdentifymarkpointsManual(EnumMaskType.BondOrigion);
            }
            else if (Mode == 0)
            {
                Done = BondCameraIdentifymarkpoints(EnumMaskType.BondOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch);
            }
            else if (Mode == 2)
            {
                Done = BondCameraIdentifymarkpointsManual2(EnumMaskType.BondOrigion);
            }



            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.BondOrigion.X = BondX;
            config.BondOrigion.Y = BondY;
            config.BondOrigion.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 榜头相机移动到系统原点，自动对焦，识别系统原点 系统初始化
        /// </summary>
        private void BondCameraIdentifyBondOrigionInt(int Mode = 0)
        {
            if (Mode == 1)
            {
                BondCameraIdentifymarkpointsManual(EnumMaskType.BondOrigion);
            }
            else if (Mode == 0)
            {
                BondCameraIdentifymarkpoints(EnumMaskType.BondOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyBondOrigionMatch);
            }
            else if (Mode == 2)
            {
                BondCameraIdentifymarkpointsManual2(EnumMaskType.BondOrigion);
            }

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            if (config.BondOrigion.X != BondX)
            {
                AxisHome(EnumStageAxis.BondX);
            }
            if (config.BondOrigion.Y != BondY)
            {
                AxisHome(EnumStageAxis.BondY);
            }
            if (config.BondOrigion.Z != BondZ)
            {
                AxisHome(EnumStageAxis.BondZ);
            }

        }


        /// <summary>
        /// 榜头相机移动到仰视相机中心，自动对焦，识别仰视相机中心
        /// </summary>
        private bool BondCameraIdentifyLookupCameraOrigion(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = BondCameraIdentifymarkpointsManual(EnumMaskType.LookupCameraOrigion);
            }
            else if (Mode == 0)
            {
                Done = BondCameraIdentifymarkpoints(EnumMaskType.LookupCameraOrigion, _systemConfig.SystemCalibrationConfig.BondIdentifyLookupCameraOrigionMatch);
            }
            else if (Mode == 2)
            {
                Done = BondCameraIdentifymarkpointsManual2(EnumMaskType.LookupCameraOrigion);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);

            double BondX = ReadCurrentAxisposition(EnumStageAxis.BondX);
            double BondY = ReadCurrentAxisposition(EnumStageAxis.BondY);
            double BondZ = ReadCurrentAxisposition(EnumStageAxis.BondZ);

            config.LookupCameraOrigion.X = BondX;
            config.LookupCameraOrigion.Y = BondY;
            config.LookupCameraOrigion.Z = BondZ;

            return Done;
        }

        /// <summary>
        /// 榜头移动到仰视相机中心，自动对焦，识别UC工具
        /// </summary>
        private bool UplookingCameraIdentifyPPtoolRotationCompensation(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = RotationCompensation(EnumMaskType.LookupChipPPOrigion, currentppTool.UplookingIdentifyPPtoolMatch);
            }
            else if (Mode == 0)
            {
                Done = RotationCompensation(EnumMaskType.LookupChipPPOrigion, currentppTool.UplookingIdentifyPPtoolMatch);
            }
            else if (Mode == 2)
            {
                Done = RotationCompensationManual(EnumMaskType.LookupChipPPOrigion, currentppTool.UplookingIdentifyPPtoolMatch);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);
            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            return Done;
        }


        /// <summary>
        /// 榜头移动到仰视相机中心，自动对焦，识别UC工具
        /// </summary>
        private bool UplookingCameraIdentifyBond1RotationCompensation(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = RotationCompensation(EnumMaskType.LookupUCtoolOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);
            }
            else if (Mode == 0)
            {
                Done = RotationCompensation(EnumMaskType.LookupUCtoolOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);
            }
            else if (Mode == 2)
            {
                Done = RotationCompensationManual(EnumMaskType.LookupUCtoolOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond1Match);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);
            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            return Done;
        }

        /// <summary>
        /// 榜头移动到仰视相机中心，自动对焦，识别UC工具
        /// </summary>
        private bool UplookingCameraIdentifyBond2RotationCompensation(int Mode = 0)
        {
            bool Done = false;
            if (Mode == 1)
            {
                Done = RotationCompensation(EnumMaskType.LookupSubmountPPOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match);
            }
            else if (Mode == 0)
            {
                Done = RotationCompensation(EnumMaskType.LookupSubmountPPOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match);
            }
            else if (Mode == 2)
            {
                Done = RotationCompensationManual(EnumMaskType.LookupSubmountPPOrigion, _systemConfig.SystemCalibrationConfig.UplookingIdentifyBond2Match);
            }

            BondCameraVisual.SetDirectLightintensity(0);
            BondCameraVisual.SetRingLightintensity(0);
            UplookingCameraVisual.SetDirectLightintensity(0);
            UplookingCameraVisual.SetRingLightintensity(0);

            return Done;
        }

        #endregion

        #region Public Mothed

        /// <summary>
        /// 自动校准
        /// </summary>
        public void AutoRun()
        {
            config = _systemConfig.PositioningConfig.DeepCopy();

            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            ShowStage();
            ShowDynamometer();

            Task.Factory.StartNew(new Action(() =>
            {
                int Done = -1;
                int Mode = 0;
                bool Done1 = false;
                try
                {
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Debug.WriteLine("榜头相机移动到安全位置");
                    BondToSafeAsync(Mode);

                    while(true)
                    {
                        Debug.WriteLine("榜头相机移动到系统原点，自动对焦，识别");
                        Done1 = BondCameraIdentifyBondOrigion(Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "榜头相机校准系统原点坐标失败，是否重新创建模板继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }

                        Mode = 0;
                        break;
                    }

                    if (DeviceMode == 0)
                    {


                        #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync(Mode);

                        //提示安装UC工具
                        Done = ShowMessage("动作确认", "开始Uplooking相机校准，安装UC工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                            Done1 = UplookingCameraIdentifyBond1(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴坐标失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }

                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("仰视相机测量分辨率和角度");
                            Done1 = UplookingCameraParamCalibration(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准分辨率和角度失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }
                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头2移动到仰视相机中心，仰视相机自动对焦，识别UC");
                            Done1 = UplookingCameraIdentifyBond2(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Submount吸嘴失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }
                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }


                        while (true)
                        {
                            Debug.WriteLine("激光传感器移动到仰视相机中心，仰视相机自动对焦，识别光斑");
                            Done1 = UplookingCameraIdentifyLaserSensor(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准激光传感器失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }
                            Mode = 0;
                            break;
                        }



                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync();

                        UplookingCameraVisual.SetDirectLightintensity(220);
                        UplookingCameraVisual.SetRingLightintensity(150);

                        //提示安装CCU工具
                        int result1 = ShowMessageAsync("动作确认", "安装CCU工具", "提示");
                        if (result1 == 1)
                        {
                        }
                        else
                        {
                            return;
                        }

                        UplookingCameraVisual.SetDirectLightintensity(0);
                        UplookingCameraVisual.SetRingLightintensity(0);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到仰视相机中心，自动对焦，识别");
                            Done1 = BondCameraIdentifyLookupCameraOrigion(Mode);
                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准仰视相机中心失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }

                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头移动到安全位置");
                        BondToSafeAsync();


                        //Debug.WriteLine("提示取下CCU工具");
                        Done = ShowMessage("动作确认", "去掉CCU工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头1UC移动到仰视相机中心，测量吸嘴偏移");
                            Done1 = UplookingCameraIdentifyBond1RotationCompensation(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头2UC移动到仰视相机中心，测量吸嘴偏移");
                            Done1 = UplookingCameraIdentifyBond2RotationCompensation(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }




                        #endregion


                        #region 榜头相机校准 榜头测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "开始Bond相机校准，安装测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到轨道原点，自动对焦，识别轨道原点");
                            Done1 = BondCameraIdentifyTrackOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准轨道原点失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头相机测量分辨率和角度");
                            Done1 = BondCameraParamCalibration(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准分辨率和角度失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }



                        while (true)
                        {
                            Debug.WriteLine("激光传感器移动到轨道原点，测高");
                            //Done1 = BondToHeightmeasurementposition(3, EnumMaskType.TrackOrigion, Mode);
                            Done1 = BondToHeightmeasurementposition(3, EnumMaskType.TrackOrigion, 1);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "激光传感器移动到轨道原点测高失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头移动到测高位置，测高");
                            Done1 = BondToHeightmeasurementposition(1, EnumMaskType.TrackOrigion, Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Chip吸嘴移动到轨道原点测高失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头移动到测高位置，测高");
                            Done1 = BondToHeightmeasurementposition(2, EnumMaskType.TrackOrigion, Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Submount吸嘴移动到轨道原点测高失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }



                        #endregion


                        #region 晶圆相机校准 晶圆盘测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示去掉测高工具
                        Done = ShowMessage("动作确认", "开始Wafer相机校准，去掉测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }



                        while (true)
                        {
                            Debug.WriteLine("晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点");
                            Done1 = WaferCameraIdentifyWaferOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Wafer相机校准晶圆mark失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }


                        while (true)
                        {
                            Debug.WriteLine("晶圆相机测量分辨率和角度");
                            Done1 = WaferCameraParamCalibration(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "晶圆相机校准分辨率和角度失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }



                        while (true)
                        {
                            Debug.WriteLine("晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点");
                            Done1 = WaferCameraIdentifyWaferOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Wafer相机校准晶圆mark失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到晶圆盘mark，识别晶圆相机中心");
                            Done1 = BondCameraIdentifyWaferCameraOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准晶圆相机中心失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        ////提示安装测高工具
                        //Done = ShowMessage("动作确认", "安装测高工具", "提示");
                        //if (Done == 0)
                        //{
                        //    return;
                        //}

                        //if (CameraWindowGUI.Instance != null)
                        //{
                        //    CameraWindowGUI.Instance.ClearGraphicDraw();
                        //    CameraWindowGUI.Instance.SelectCamera(0);
                        //}


                        //while (true)
                        //{
                        //    Debug.WriteLine("榜头在晶圆盘mark附近测高");
                        //    Done1 = BondToHeightmeasurementposition(1, EnumMaskType.WaferCameraOrigion, Mode);

                        //    if (Done1 == false)
                        //    {
                        //        Done = ShowMessage("校准异常", "Chip吸嘴在晶圆盘mark附近测高失败，是否继续校准", "提示");
                        //        if (Done == 0)
                        //        {
                        //            Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                        //            if (Done == 0)
                        //            {
                        //                return;
                        //            }
                        //            else
                        //            {
                        //                Mode = 0;
                        //                break;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            Mode = 1;
                        //            continue;
                        //        }
                        //    }


                        //    Mode = 0;
                        //    break;
                        //}
                        ////BondToHeightmeasurementposition(2, EnumMaskType.WaferCameraOrigion, Mode);
                        ////BondToHeightmeasurementposition(3, EnumMaskType.WaferCameraOrigion, Mode);

                        #endregion


                        #region 共晶台校准 共晶台测高


                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "开始校准共晶台，去掉测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到共晶台，识别共晶台");
                            Done1 = BondCameraIdentifyEutecticWeldingOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机移动到共晶台，校准共晶台失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头在共晶台附近测高");
                            //Done1 = BondToHeightmeasurementposition(3, EnumMaskType.EutecticWeldingOrigion, Mode);
                            Done1 = BondToHeightmeasurementposition(3, EnumMaskType.EutecticWeldingOrigion, 1);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "激光传感器在共晶台附近测高失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        #endregion

                    }
                    else if(DeviceMode == 1)
                    {
                        #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync(Mode);

                        //提示安装UC工具
                        Done = ShowMessage("动作确认", "开始Uplooking相机校准，安装UC工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                            Done1 = UplookingCameraIdentifyBond1(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴坐标失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }

                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("仰视相机测量分辨率和角度");
                            Done1 = UplookingCameraParamCalibration(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准分辨率和角度失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }
                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }


                        while (true)
                        {
                            Debug.WriteLine("激光传感器移动到仰视相机中心，仰视相机自动对焦，识别光斑");
                            Done1 = UplookingCameraIdentifyLaserSensor(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准激光传感器失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }
                            Mode = 0;
                            break;
                        }



                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync();

                        UplookingCameraVisual.SetDirectLightintensity(220);
                        UplookingCameraVisual.SetRingLightintensity(150);

                        //提示安装CCU工具
                        int result1 = ShowMessageAsync("动作确认", "安装CCU工具", "提示");
                        if (result1 == 1)
                        {
                        }
                        else
                        {
                            return;
                        }

                        UplookingCameraVisual.SetDirectLightintensity(0);
                        UplookingCameraVisual.SetRingLightintensity(0);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到仰视相机中心，自动对焦，识别");
                            Done1 = BondCameraIdentifyLookupCameraOrigion(Mode);
                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准仰视相机中心失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }

                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头移动到安全位置");
                        BondToSafeAsync();


                        //Debug.WriteLine("提示取下CCU工具");
                        Done = ShowMessage("动作确认", "去掉CCU工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头1UC移动到仰视相机中心，测量吸嘴偏移");
                            Done1 = UplookingCameraIdentifyBond1RotationCompensation(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        #endregion


                        #region 榜头相机校准 榜头测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "开始Bond相机校准，安装测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到轨道原点，自动对焦，识别轨道原点");
                            Done1 = BondCameraIdentifyTrackOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准轨道原点失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头相机测量分辨率和角度");
                            Done1 = BondCameraParamCalibration(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准分辨率和角度失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }



                        while (true)
                        {
                            Debug.WriteLine("激光传感器移动到轨道原点，测高");
                            //Done1 = BondToHeightmeasurementposition(3, EnumMaskType.TrackOrigion, Mode);
                            Done1 = BondToHeightmeasurementposition(3, EnumMaskType.TrackOrigion, 1);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "激光传感器移动到轨道原点测高失败，是否重新测高", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头移动到测高位置，测高");
                            Done1 = BondToHeightmeasurementposition(1, EnumMaskType.TrackOrigion, Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Chip吸嘴移动到轨道原点测高失败，是否重新测高", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        while (true)
                        {
                            Debug.WriteLine("榜头移动到点胶位置");
                            Done1 = BondToEpoxtSpot(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "点胶校准失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }



                        #endregion


                        #region 晶圆相机校准 晶圆盘测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示去掉测高工具
                        Done = ShowMessage("动作确认", "开始Wafer相机校准，去掉测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }



                        while (true)
                        {
                            Debug.WriteLine("晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点");
                            Done1 = WaferCameraIdentifyWaferOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Wafer相机校准晶圆mark失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }


                        while (true)
                        {
                            Debug.WriteLine("晶圆相机测量分辨率和角度");
                            Done1 = WaferCameraParamCalibration(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "晶圆相机校准分辨率和角度失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }



                        while (true)
                        {
                            Debug.WriteLine("晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点");
                            Done1 = WaferCameraIdentifyWaferOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Wafer相机校准晶圆mark失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头相机移动到晶圆盘mark，识别晶圆相机中心");
                            Done1 = BondCameraIdentifyWaferCameraOrigion(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "榜头相机校准晶圆相机中心失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }

                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        ////提示安装测高工具
                        //Done = ShowMessage("动作确认", "安装测高工具", "提示");
                        //if (Done == 0)
                        //{
                        //    return;
                        //}

                        //if (CameraWindowGUI.Instance != null)
                        //{
                        //    CameraWindowGUI.Instance.ClearGraphicDraw();
                        //    CameraWindowGUI.Instance.SelectCamera(0);
                        //}


                        //while (true)
                        //{
                        //    Debug.WriteLine("榜头在晶圆盘mark附近测高");
                        //    Done1 = BondToHeightmeasurementposition(1, EnumMaskType.WaferCameraOrigion, Mode);

                        //    if (Done1 == false)
                        //    {
                        //        Done = ShowMessage("校准异常", "Chip吸嘴在晶圆盘mark附近测高失败，是否继续校准", "提示");
                        //        if (Done == 0)
                        //        {
                        //            Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                        //            if (Done == 0)
                        //            {
                        //                return;
                        //            }
                        //            else
                        //            {
                        //                Mode = 0;
                        //                break;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            Mode = 1;
                        //            continue;
                        //        }
                        //    }


                        //    Mode = 0;
                        //    break;
                        //}
                        ////BondToHeightmeasurementposition(2, EnumMaskType.WaferCameraOrigion, Mode);
                        ////BondToHeightmeasurementposition(3, EnumMaskType.WaferCameraOrigion, Mode);

                        #endregion

                    }



                    //榜头移动到安全位置
                    BondToSafeAsync();


                    //CloseStageAxisMove();
                    //CloseStage();
                    //CloseDynamometer();

                    string str = "系统";
                    List<TextContent> Text1 = new List<TextContent>();
                    List<TextContent> Text2 = new List<TextContent>();
                    TextContent text1;

                    if (DeviceMode == 0)
                    {
                        #region 显示文本


                        Text1 = new List<TextContent>()
                        {
                            new TextContent("BondCamera",(_BondcameraConfig.WidthPixelSize*1000).ToString("F3"),(_BondcameraConfig.HeightPixelSize*1000).ToString("F3"),(_BondcameraConfig.Angle).ToString("F3")),
                            new TextContent("WaferCamera",(_WafercameraConfig.WidthPixelSize*1000).ToString("F3"),(_WafercameraConfig.HeightPixelSize*1000).ToString("F3"),(_WafercameraConfig.Angle).ToString("F3")),
                            new TextContent("UplookingCamera",(_UplookingcameraConfig.WidthPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.HeightPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.Angle).ToString("F3")),


                        };



                        // 系统原点
                        if (_systemConfig.PositioningConfig.BondOrigion.X == config.BondOrigion.X &&
                            _systemConfig.PositioningConfig.BondOrigion.Y == config.BondOrigion.Y &&
                            _systemConfig.PositioningConfig.BondOrigion.Z == config.BondOrigion.Z)
                        {
                            text1 = new TextContent("系统原点", _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("系统原点",
                                _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3") + $"({config.BondOrigion.X})",
                                _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3") + $"({config.BondOrigion.Y})",
                                _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3") + $"({config.BondOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头安全位置
                        if (_systemConfig.PositioningConfig.BondSafeLocation.X == config.BondSafeLocation.X &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Y == config.BondSafeLocation.Y &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Z == config.BondSafeLocation.Z)
                        {
                            text1 = new TextContent("榜头安全位置", _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头安全位置",
                                _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3") + $"({config.BondSafeLocation.X})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3") + $"({config.BondSafeLocation.Y})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3") + $"({config.BondSafeLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupCameraOrigion.X == config.LookupCameraOrigion.X &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Y == config.LookupCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Z == config.LookupCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心", _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3") + $"({config.LookupCameraOrigion.X})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3") + $"({config.LookupCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3") + $"({config.LookupCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupChipPPOrigion.X == config.LookupChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Y == config.LookupChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Z == config.LookupChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心", _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3") + $"({config.LookupChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3") + $"({config.LookupChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3") + $"({config.LookupChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupSubmountPPOrigion.X == config.LookupSubmountPPOrigion.X &&
                            _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y == config.LookupSubmountPPOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z == config.LookupSubmountPPOrigion.Z)
                        {
                            text1 = new TextContent("基底吸嘴到仰视相机中心", _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X.ToString("F3") + $"({config.LookupSubmountPPOrigion.X})",
                                _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y.ToString("F3") + $"({config.LookupSubmountPPOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z.ToString("F3") + $"({config.LookupSubmountPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupLaserSensorOrigion.X == config.LookupLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y == config.LookupLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z == config.LookupLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心", _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3") + $"({config.LookupLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3") + $"({config.LookupLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3") + $"({config.LookupLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到轨道原点
                        if (_systemConfig.PositioningConfig.TrackOrigion.X == config.TrackOrigion.X &&
                            _systemConfig.PositioningConfig.TrackOrigion.Y == config.TrackOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackOrigion.Z == config.TrackOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到轨道原点", _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到轨道原点",
                                _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3") + $"({config.TrackOrigion.X})",
                                _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3") + $"({config.TrackOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3") + $"({config.TrackOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到轨道原点
                        if (_systemConfig.PositioningConfig.TrackChipPPOrigion.X == config.TrackChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Y == config.TrackChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Z == config.TrackChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点", _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3") + $"({config.TrackChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3") + $"({config.TrackChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3") + $"({config.TrackChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到轨道原点
                        if (_systemConfig.PositioningConfig.TrackSubmountPPOrigion.X == config.TrackSubmountPPOrigion.X &&
                            _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y == config.TrackSubmountPPOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z == config.TrackSubmountPPOrigion.Z)
                        {
                            text1 = new TextContent("基底吸嘴到轨道原点", _systemConfig.PositioningConfig.TrackSubmountPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到轨道原点",
                                _systemConfig.PositioningConfig.TrackSubmountPPOrigion.X.ToString("F3") + $"({config.TrackSubmountPPOrigion.X})",
                                _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y.ToString("F3") + $"({config.TrackSubmountPPOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z.ToString("F3") + $"({config.TrackSubmountPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到轨道原点
                        if (_systemConfig.PositioningConfig.TrackLaserSensorOrigion.X == config.TrackLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y == config.TrackLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z == config.TrackLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到轨道原点", _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到轨道原点",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3") + $"({config.TrackLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3") + $"({config.TrackLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3") + $"({config.TrackLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机识别轨道原点Z
                        if (_systemConfig.PositioningConfig.TrackLaserSensorZ == config.TrackLaserSensorZ)
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3") + $"({config.TrackLaserSensorZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴空闲Z
                        if (_systemConfig.PositioningConfig.SubmountPPFreeZ == config.SubmountPPFreeZ)
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3") + $"({config.SubmountPPFreeZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴工作Z
                        if (_systemConfig.PositioningConfig.SubmountPPWorkZ == config.SubmountPPWorkZ)
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3") + $"({config.SubmountPPWorkZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴与榜头相机偏移
                        if (_systemConfig.PositioningConfig.PP1AndBondCameraOffset.X == config.PP1AndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y == config.PP1AndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z == config.PP1AndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移", _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3") + $"({config.PP1AndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3") + $"({config.PP1AndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3") + $"({config.PP1AndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴与榜头相机偏移
                        if (_systemConfig.PositioningConfig.PP2AndBondCameraOffset.X == config.PP2AndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y == config.PP2AndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z == config.PP2AndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("基底吸嘴与榜头相机偏移", _systemConfig.PositioningConfig.PP2AndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴与榜头相机偏移",
                                _systemConfig.PositioningConfig.PP2AndBondCameraOffset.X.ToString("F3") + $"({config.PP2AndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y.ToString("F3") + $"({config.PP2AndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z.ToString("F3") + $"({config.PP2AndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器与榜头相机偏移
                        if (_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X == config.LaserSensorAndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y == config.LaserSensorAndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z == config.LaserSensorAndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移", _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferCameraOrigion.X == config.WaferCameraOrigion.X &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Y == config.WaferCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Z == config.WaferCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心", _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3") + $"({config.WaferCameraOrigion.X})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3") + $"({config.WaferCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3") + $"({config.WaferCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferChipPPOrigion.X == config.WaferChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Y == config.WaferChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Z == config.WaferChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心", _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3") + $"({config.WaferChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3") + $"({config.WaferChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3") + $"({config.WaferChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferSubmountPPOrigion.X == config.WaferSubmountPPOrigion.X &&
                            _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y == config.WaferSubmountPPOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z == config.WaferSubmountPPOrigion.Z)
                        {
                            text1 = new TextContent("基底吸嘴到晶圆相机中心", _systemConfig.PositioningConfig.WaferSubmountPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferSubmountPPOrigion.X.ToString("F3") + $"({config.WaferSubmountPPOrigion.X})",
                                _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y.ToString("F3") + $"({config.WaferSubmountPPOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z.ToString("F3") + $"({config.WaferSubmountPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 晶圆盘原点到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferOrigion.X == config.WaferOrigion.X &&
                            _systemConfig.PositioningConfig.WaferOrigion.Y == config.WaferOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferOrigion.Z == config.WaferOrigion.Z)
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心", _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3") + $"({config.WaferOrigion.X})",
                                _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3") + $"({config.WaferOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3") + $"({config.WaferOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到共晶焊接位置
                        if (_systemConfig.PositioningConfig.EutecticWeldingLocation.X == config.EutecticWeldingLocation.X &&
                            _systemConfig.PositioningConfig.EutecticWeldingLocation.Y == config.EutecticWeldingLocation.Y &&
                            _systemConfig.PositioningConfig.EutecticWeldingLocation.Z == config.EutecticWeldingLocation.Z)
                        {
                            text1 = new TextContent("榜头相机到共晶焊接位置", _systemConfig.PositioningConfig.EutecticWeldingLocation.X.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到共晶焊接位置",
                                _systemConfig.PositioningConfig.EutecticWeldingLocation.X.ToString("F3") + $"({config.EutecticWeldingLocation.X})",
                                _systemConfig.PositioningConfig.EutecticWeldingLocation.Y.ToString("F3") + $"({config.EutecticWeldingLocation.Y})",
                                _systemConfig.PositioningConfig.EutecticWeldingLocation.Z.ToString("F3") + $"({config.EutecticWeldingLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到共晶焊接位置
                        if (_systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X == config.EutecticWeldingChipPPLocation.X &&
                            _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y == config.EutecticWeldingChipPPLocation.Y &&
                            _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z == config.EutecticWeldingChipPPLocation.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到共晶焊接位置", _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到共晶焊接位置",
                                _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X.ToString("F3") + $"({config.EutecticWeldingChipPPLocation.X})",
                                _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y.ToString("F3") + $"({config.EutecticWeldingChipPPLocation.Y})",
                                _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z.ToString("F3") + $"({config.EutecticWeldingChipPPLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到共晶焊接位置
                        if (_systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X == config.EutecticWeldingSubmountPPLocation.X &&
                            _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y == config.EutecticWeldingSubmountPPLocation.Y &&
                            _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z == config.EutecticWeldingSubmountPPLocation.Z)
                        {
                            text1 = new TextContent("基底吸嘴到共晶焊接位置", _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到共晶焊接位置",
                                _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X.ToString("F3") + $"({config.EutecticWeldingSubmountPPLocation.X})",
                                _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y.ToString("F3") + $"({config.EutecticWeldingSubmountPPLocation.Y})",
                                _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z.ToString("F3") + $"({config.EutecticWeldingSubmountPPLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        #endregion

                    }
                    else if(DeviceMode == 1)
                    {
                        #region 显示文本


                        Text1 = new List<TextContent>()
                        {
                            new TextContent("BondCamera",(_BondcameraConfig.WidthPixelSize*1000).ToString("F3"),(_BondcameraConfig.HeightPixelSize*1000).ToString("F3"),(_BondcameraConfig.Angle).ToString("F3")),
                            new TextContent("WaferCamera",(_WafercameraConfig.WidthPixelSize*1000).ToString("F3"),(_WafercameraConfig.HeightPixelSize*1000).ToString("F3"),(_WafercameraConfig.Angle).ToString("F3")),
                            new TextContent("UplookingCamera",(_UplookingcameraConfig.WidthPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.HeightPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.Angle).ToString("F3")),


                        };



                        // 系统原点
                        if (_systemConfig.PositioningConfig.BondOrigion.X == config.BondOrigion.X &&
                            _systemConfig.PositioningConfig.BondOrigion.Y == config.BondOrigion.Y &&
                            _systemConfig.PositioningConfig.BondOrigion.Z == config.BondOrigion.Z)
                        {
                            text1 = new TextContent("系统原点", _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("系统原点",
                                _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3") + $"({config.BondOrigion.X})",
                                _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3") + $"({config.BondOrigion.Y})",
                                _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3") + $"({config.BondOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头安全位置
                        if (_systemConfig.PositioningConfig.BondSafeLocation.X == config.BondSafeLocation.X &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Y == config.BondSafeLocation.Y &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Z == config.BondSafeLocation.Z)
                        {
                            text1 = new TextContent("榜头安全位置", _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头安全位置",
                                _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3") + $"({config.BondSafeLocation.X})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3") + $"({config.BondSafeLocation.Y})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3") + $"({config.BondSafeLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupCameraOrigion.X == config.LookupCameraOrigion.X &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Y == config.LookupCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Z == config.LookupCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心", _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3") + $"({config.LookupCameraOrigion.X})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3") + $"({config.LookupCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3") + $"({config.LookupCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupChipPPOrigion.X == config.LookupChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Y == config.LookupChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Z == config.LookupChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心", _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3") + $"({config.LookupChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3") + $"({config.LookupChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3") + $"({config.LookupChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupLaserSensorOrigion.X == config.LookupLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y == config.LookupLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z == config.LookupLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心", _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3") + $"({config.LookupLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3") + $"({config.LookupLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3") + $"({config.LookupLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到轨道原点
                        if (_systemConfig.PositioningConfig.TrackOrigion.X == config.TrackOrigion.X &&
                            _systemConfig.PositioningConfig.TrackOrigion.Y == config.TrackOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackOrigion.Z == config.TrackOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到轨道原点", _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到轨道原点",
                                _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3") + $"({config.TrackOrigion.X})",
                                _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3") + $"({config.TrackOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3") + $"({config.TrackOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到轨道原点
                        if (_systemConfig.PositioningConfig.TrackChipPPOrigion.X == config.TrackChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Y == config.TrackChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Z == config.TrackChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点", _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3") + $"({config.TrackChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3") + $"({config.TrackChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3") + $"({config.TrackChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到轨道原点
                        if (_systemConfig.PositioningConfig.TrackLaserSensorOrigion.X == config.TrackLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y == config.TrackLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z == config.TrackLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到轨道原点", _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到轨道原点",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3") + $"({config.TrackLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3") + $"({config.TrackLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3") + $"({config.TrackLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 点胶针点胶位置
                        if (_systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.X == config.TrackEpoxtSpotCoordinate.X &&
                            _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Y == config.TrackEpoxtSpotCoordinate.Y &&
                            _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Z == config.TrackEpoxtSpotCoordinate.Z)
                        {
                            text1 = new TextContent("点胶针点胶位置", _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.X.ToString("F3"), _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("点胶针点胶位置",
                                _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.X.ToString("F3") + $"({config.TrackEpoxtSpotCoordinate.X})",
                                _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Y.ToString("F3") + $"({config.TrackEpoxtSpotCoordinate.Y})",
                                _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Z.ToString("F3") + $"({config.TrackEpoxtSpotCoordinate.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 相机对准点胶位置
                        if (_systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.X == config.TrackBondCameraToEpoxtSpotCoordinate.X &&
                            _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Y == config.TrackBondCameraToEpoxtSpotCoordinate.Y &&
                            _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Z == config.TrackBondCameraToEpoxtSpotCoordinate.Z)
                        {
                            text1 = new TextContent("相机对准点胶位置", _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.X.ToString("F3"), _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("相机对准点胶位置",
                                _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.X.ToString("F3") + $"({config.TrackBondCameraToEpoxtSpotCoordinate.X})",
                                _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Y.ToString("F3") + $"({config.TrackBondCameraToEpoxtSpotCoordinate.Y})",
                                _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Z.ToString("F3") + $"({config.TrackBondCameraToEpoxtSpotCoordinate.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机识别轨道原点Z
                        if (_systemConfig.PositioningConfig.TrackLaserSensorZ == config.TrackLaserSensorZ)
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3") + $"({config.TrackLaserSensorZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴空闲Z
                        if (_systemConfig.PositioningConfig.SubmountPPFreeZ == config.SubmountPPFreeZ)
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3") + $"({config.SubmountPPFreeZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴工作Z
                        if (_systemConfig.PositioningConfig.SubmountPPWorkZ == config.SubmountPPWorkZ)
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3") + $"({config.SubmountPPWorkZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴与榜头相机偏移
                        if (_systemConfig.PositioningConfig.PP1AndBondCameraOffset.X == config.PP1AndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y == config.PP1AndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z == config.PP1AndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移", _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3") + $"({config.PP1AndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3") + $"({config.PP1AndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3") + $"({config.PP1AndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 点胶针与榜头相机偏移
                        if (_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X == config.EpoxtAndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y == config.EpoxtAndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Z == config.EpoxtAndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("点胶针与榜头相机偏移", _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("点胶针与榜头相机偏移",
                                _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X.ToString("F3") + $"({config.EpoxtAndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y.ToString("F3") + $"({config.EpoxtAndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Z.ToString("F3") + $"({config.EpoxtAndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器与榜头相机偏移
                        if (_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X == config.LaserSensorAndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y == config.LaserSensorAndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z == config.LaserSensorAndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移", _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferCameraOrigion.X == config.WaferCameraOrigion.X &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Y == config.WaferCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Z == config.WaferCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心", _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3") + $"({config.WaferCameraOrigion.X})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3") + $"({config.WaferCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3") + $"({config.WaferCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferChipPPOrigion.X == config.WaferChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Y == config.WaferChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Z == config.WaferChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心", _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3") + $"({config.WaferChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3") + $"({config.WaferChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3") + $"({config.WaferChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 晶圆盘原点到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferOrigion.X == config.WaferOrigion.X &&
                            _systemConfig.PositioningConfig.WaferOrigion.Y == config.WaferOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferOrigion.Z == config.WaferOrigion.Z)
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心", _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3") + $"({config.WaferOrigion.X})",
                                _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3") + $"({config.WaferOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3") + $"({config.WaferOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);


                        #endregion
                    }



                    ListmessagesBox myMessageBox1 = new ListmessagesBox();
                    myMessageBox1.ShowText(str);
                    myMessageBox1.DataGridView1Show(Text1);
                    myMessageBox1.DataGridView2Show(Text2);
                    myMessageBox1.Show();
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Done = ShowMessageAsync("动作确认", "是否保存系统校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        _systemConfig.PositioningConfig = config;
                        _systemConfig.SaveConfig();
                        HardwareConfiguration.Instance.SaveConfig();
                    }
                    myMessageBox1.Close();
                    Done = ShowMessage("动作确认", "结束校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        //_systemConfig.SaveConfig();
                    }

                }
                catch
                {

                }

            }
            ));


        }

        /// <summary>
        /// 手动校准
        /// </summary>
        /// <param name="Mode"> 1 半自动 2 全手动</param>
        public void ManualRun(int Mode)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            ShowStage();
            ShowDynamometer();

            config = _systemConfig.PositioningConfig;

            Task.Factory.StartNew(new Action(() =>
            {
                int Done = -1;
                try
                {


                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Debug.WriteLine("榜头相机移动到系统原点，自动对焦，识别");
                    BondCameraIdentifyBondOrigion(Mode);

                    if(DeviceMode == 0)
                    {
                        #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync(Mode);

                        //提示安装UC工具
                        Done = ShowMessage("动作确认", "开始Uplooking相机校准，安装UC工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                        UplookingCameraIdentifyBond1(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("仰视相机测量分辨率和角度");
                        UplookingCameraParamCalibration(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头2移动到仰视相机中心，仰视相机自动对焦，识别UC");
                        UplookingCameraIdentifyBond2(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("激光传感器移动到仰视相机中心，仰视相机自动对焦，识别光斑");
                        UplookingCameraIdentifyLaserSensor(Mode);



                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync();

                        UplookingCameraVisual.SetDirectLightintensity(220);
                        UplookingCameraVisual.SetRingLightintensity(150);

                        //提示安装CCU工具
                        int result1 = ShowMessageAsync("动作确认", "安装CCU工具", "提示");
                        if (result1 == 1)
                        {
                        }
                        else
                        {
                            return;
                        }

                        UplookingCameraVisual.SetDirectLightintensity(0);
                        UplookingCameraVisual.SetRingLightintensity(0);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头相机移动到仰视相机中心，自动对焦，识别");
                        BondCameraIdentifyLookupCameraOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头移动到安全位置");
                        BondToSafeAsync();


                        //Debug.WriteLine("提示取下CCU工具");
                        Done = ShowMessage("动作确认", "去掉CCU工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头1UC移动到仰视相机中心，测量吸嘴偏移");
                        UplookingCameraIdentifyBond1RotationCompensation(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头2UC移动到仰视相机中心，测量吸嘴偏移");
                        UplookingCameraIdentifyBond2RotationCompensation(Mode);


                        #endregion


                        #region 榜头相机校准 榜头测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "开始Bond相机校准，安装测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机移动到轨道原点，自动对焦，识别轨道原点
                        BondCameraIdentifyTrackOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机测量分辨率和角度
                        BondCameraParamCalibration(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //激光传感器移动到轨道原点，测高
                        BondToHeightmeasurementposition(3, EnumMaskType.TrackOrigion, Mode);


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        //榜头移动到测高位置，测高
                        BondToHeightmeasurementposition(1, EnumMaskType.TrackOrigion, Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        BondToHeightmeasurementposition(2, EnumMaskType.TrackOrigion, Mode);



                        #endregion


                        #region 晶圆相机校准 晶圆盘测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示去掉测高工具
                        Done = ShowMessage("动作确认", "开始Wafer相机校准，去掉测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }

                        //晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点
                        WaferCameraIdentifyWaferOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }

                        //晶圆相机测量分辨率和角度
                        WaferCameraParamCalibration(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }

                        //晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点
                        WaferCameraIdentifyWaferOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机移动到晶圆盘mark，识别晶圆相机中心
                        BondCameraIdentifyWaferCameraOrigion(Mode);

                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "安装测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头在晶圆盘mark附近测高
                        //BondToHeightmeasurementposition(1, EnumMaskType.WaferCameraOrigion, Mode);
                        //BondToHeightmeasurementposition(2, EnumMaskType.WaferCameraOrigion, Mode);
                        //BondToHeightmeasurementposition(3, EnumMaskType.WaferCameraOrigion, Mode);

                        #endregion


                        #region 共晶台校准 共晶台测高

                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "开始校准共晶台，去掉测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机移动到共晶台，识别共晶台
                        BondCameraIdentifyEutecticWeldingOrigion(Mode);


                        ////榜头相机移动到安全位置
                        //BondToSafeAsync();

                        ////提示安装测高工具
                        //Done = ShowMessage("动作确认", "安装测高工具", "提示");
                        //if (Done == 0)
                        //{
                        //    return;
                        //}

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头在共晶台附近测高
                        BondToHeightmeasurementposition(3, EnumMaskType.EutecticWeldingOrigion, Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //BondToHeightmeasurementposition(2, EnumMaskType.EutecticWeldingOrigion, Mode);
                        ////BondToHeightmeasurementposition(3, EnumMaskType.EutecticWeldingOrigion, Mode);

                        //if (CameraWindowGUI.Instance != null)
                        //{
                        //    CameraWindowGUI.Instance.ClearGraphicDraw();
                        //    CameraWindowGUI.Instance.SelectCamera(0);
                        //}

                        #endregion


                    }
                    else if(DeviceMode == 1)
                    {
                        #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync(Mode);

                        //提示安装UC工具
                        Done = ShowMessage("动作确认", "开始Uplooking相机校准，安装UC工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                        UplookingCameraIdentifyBond1(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("仰视相机测量分辨率和角度");
                        UplookingCameraParamCalibration(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("激光传感器移动到仰视相机中心，仰视相机自动对焦，识别光斑");
                        UplookingCameraIdentifyLaserSensor(Mode);



                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync();

                        UplookingCameraVisual.SetDirectLightintensity(220);
                        UplookingCameraVisual.SetRingLightintensity(150);

                        //提示安装CCU工具
                        int result1 = ShowMessageAsync("动作确认", "安装CCU工具", "提示");
                        if (result1 == 1)
                        {
                        }
                        else
                        {
                            return;
                        }

                        UplookingCameraVisual.SetDirectLightintensity(0);
                        UplookingCameraVisual.SetRingLightintensity(0);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头相机移动到仰视相机中心，自动对焦，识别");
                        BondCameraIdentifyLookupCameraOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头移动到安全位置");
                        BondToSafeAsync();


                        //Debug.WriteLine("提示取下CCU工具");
                        Done = ShowMessage("动作确认", "去掉CCU工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头1UC移动到仰视相机中心，测量吸嘴偏移");
                        UplookingCameraIdentifyBond1RotationCompensation(Mode);

                        #endregion


                        #region 榜头相机校准 榜头测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "开始Bond相机校准，安装测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机移动到轨道原点，自动对焦，识别轨道原点
                        BondCameraIdentifyTrackOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机测量分辨率和角度
                        BondCameraParamCalibration(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //激光传感器移动到轨道原点，测高
                        BondToHeightmeasurementposition(3, EnumMaskType.TrackOrigion, Mode);


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }


                        //榜头移动到测高位置，测高
                        BondToHeightmeasurementposition(1, EnumMaskType.TrackOrigion, Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        BondToEpoxtSpot(Mode);



                        #endregion


                        #region 晶圆相机校准 晶圆盘测高


                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示去掉测高工具
                        Done = ShowMessage("动作确认", "开始Wafer相机校准，去掉测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }



                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }

                        //晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点
                        WaferCameraIdentifyWaferOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }

                        //晶圆相机测量分辨率和角度
                        WaferCameraParamCalibration(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(2);
                        }

                        //晶圆盘Mark点移动至晶圆相机中心，自动对焦，识别晶圆盘mark点
                        WaferCameraIdentifyWaferOrigion(Mode);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头相机移动到晶圆盘mark，识别晶圆相机中心
                        BondCameraIdentifyWaferCameraOrigion(Mode);

                        //榜头相机移动到安全位置
                        BondToSafeAsync();

                        //提示安装测高工具
                        Done = ShowMessage("动作确认", "安装测高工具", "提示");
                        if (Done == 0)
                        {
                            return;
                        }

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头在晶圆盘mark附近测高
                        //BondToHeightmeasurementposition(1, EnumMaskType.WaferCameraOrigion, Mode);
                        //BondToHeightmeasurementposition(2, EnumMaskType.WaferCameraOrigion, Mode);
                        //BondToHeightmeasurementposition(3, EnumMaskType.WaferCameraOrigion, Mode);

                        #endregion

                    }





                    //榜头移动到安全位置
                    BondToSafeAsync();



                    //CloseStageAxisMove();
                    //CloseStage();
                    //CloseDynamometer();

                    string str = "系统";
                    List<TextContent> Text1 = new List<TextContent>();
                    List<TextContent> Text2 = new List<TextContent>();
                    TextContent text1;

                    if (DeviceMode == 0)
                    {
                        #region 显示文本


                        Text1 = new List<TextContent>()
                        {
                            new TextContent("BondCamera",(_BondcameraConfig.WidthPixelSize*1000).ToString("F3"),(_BondcameraConfig.HeightPixelSize*1000).ToString("F3"),(_BondcameraConfig.Angle).ToString("F3")),
                            new TextContent("WaferCamera",(_WafercameraConfig.WidthPixelSize*1000).ToString("F3"),(_WafercameraConfig.HeightPixelSize*1000).ToString("F3"),(_WafercameraConfig.Angle).ToString("F3")),
                            new TextContent("UplookingCamera",(_UplookingcameraConfig.WidthPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.HeightPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.Angle).ToString("F3")),


                        };



                        // 系统原点
                        if (_systemConfig.PositioningConfig.BondOrigion.X == config.BondOrigion.X &&
                            _systemConfig.PositioningConfig.BondOrigion.Y == config.BondOrigion.Y &&
                            _systemConfig.PositioningConfig.BondOrigion.Z == config.BondOrigion.Z)
                        {
                            text1 = new TextContent("系统原点", _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("系统原点",
                                _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3") + $"({config.BondOrigion.X})",
                                _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3") + $"({config.BondOrigion.Y})",
                                _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3") + $"({config.BondOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头安全位置
                        if (_systemConfig.PositioningConfig.BondSafeLocation.X == config.BondSafeLocation.X &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Y == config.BondSafeLocation.Y &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Z == config.BondSafeLocation.Z)
                        {
                            text1 = new TextContent("榜头安全位置", _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头安全位置",
                                _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3") + $"({config.BondSafeLocation.X})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3") + $"({config.BondSafeLocation.Y})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3") + $"({config.BondSafeLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupCameraOrigion.X == config.LookupCameraOrigion.X &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Y == config.LookupCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Z == config.LookupCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心", _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3") + $"({config.LookupCameraOrigion.X})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3") + $"({config.LookupCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3") + $"({config.LookupCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupChipPPOrigion.X == config.LookupChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Y == config.LookupChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Z == config.LookupChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心", _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3") + $"({config.LookupChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3") + $"({config.LookupChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3") + $"({config.LookupChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupSubmountPPOrigion.X == config.LookupSubmountPPOrigion.X &&
                            _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y == config.LookupSubmountPPOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z == config.LookupSubmountPPOrigion.Z)
                        {
                            text1 = new TextContent("基底吸嘴到仰视相机中心", _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupSubmountPPOrigion.X.ToString("F3") + $"({config.LookupSubmountPPOrigion.X})",
                                _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y.ToString("F3") + $"({config.LookupSubmountPPOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z.ToString("F3") + $"({config.LookupSubmountPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupLaserSensorOrigion.X == config.LookupLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y == config.LookupLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z == config.LookupLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心", _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3") + $"({config.LookupLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3") + $"({config.LookupLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3") + $"({config.LookupLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到轨道原点
                        if (_systemConfig.PositioningConfig.TrackOrigion.X == config.TrackOrigion.X &&
                            _systemConfig.PositioningConfig.TrackOrigion.Y == config.TrackOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackOrigion.Z == config.TrackOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到轨道原点", _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到轨道原点",
                                _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3") + $"({config.TrackOrigion.X})",
                                _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3") + $"({config.TrackOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3") + $"({config.TrackOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到轨道原点
                        if (_systemConfig.PositioningConfig.TrackChipPPOrigion.X == config.TrackChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Y == config.TrackChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Z == config.TrackChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点", _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3") + $"({config.TrackChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3") + $"({config.TrackChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3") + $"({config.TrackChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到轨道原点
                        if (_systemConfig.PositioningConfig.TrackSubmountPPOrigion.X == config.TrackSubmountPPOrigion.X &&
                            _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y == config.TrackSubmountPPOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z == config.TrackSubmountPPOrigion.Z)
                        {
                            text1 = new TextContent("基底吸嘴到轨道原点", _systemConfig.PositioningConfig.TrackSubmountPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到轨道原点",
                                _systemConfig.PositioningConfig.TrackSubmountPPOrigion.X.ToString("F3") + $"({config.TrackSubmountPPOrigion.X})",
                                _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y.ToString("F3") + $"({config.TrackSubmountPPOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z.ToString("F3") + $"({config.TrackSubmountPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到轨道原点
                        if (_systemConfig.PositioningConfig.TrackLaserSensorOrigion.X == config.TrackLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y == config.TrackLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z == config.TrackLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到轨道原点", _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到轨道原点",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3") + $"({config.TrackLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3") + $"({config.TrackLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3") + $"({config.TrackLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机识别轨道原点Z
                        if (_systemConfig.PositioningConfig.TrackLaserSensorZ == config.TrackLaserSensorZ)
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3") + $"({config.TrackLaserSensorZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴空闲Z
                        if (_systemConfig.PositioningConfig.SubmountPPFreeZ == config.SubmountPPFreeZ)
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3") + $"({config.SubmountPPFreeZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴工作Z
                        if (_systemConfig.PositioningConfig.SubmountPPWorkZ == config.SubmountPPWorkZ)
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3") + $"({config.SubmountPPWorkZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴与榜头相机偏移
                        if (_systemConfig.PositioningConfig.PP1AndBondCameraOffset.X == config.PP1AndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y == config.PP1AndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z == config.PP1AndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移", _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3") + $"({config.PP1AndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3") + $"({config.PP1AndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3") + $"({config.PP1AndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴与榜头相机偏移
                        if (_systemConfig.PositioningConfig.PP2AndBondCameraOffset.X == config.PP2AndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y == config.PP2AndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z == config.PP2AndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("基底吸嘴与榜头相机偏移", _systemConfig.PositioningConfig.PP2AndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴与榜头相机偏移",
                                _systemConfig.PositioningConfig.PP2AndBondCameraOffset.X.ToString("F3") + $"({config.PP2AndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y.ToString("F3") + $"({config.PP2AndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z.ToString("F3") + $"({config.PP2AndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器与榜头相机偏移
                        if (_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X == config.LaserSensorAndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y == config.LaserSensorAndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z == config.LaserSensorAndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移", _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferCameraOrigion.X == config.WaferCameraOrigion.X &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Y == config.WaferCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Z == config.WaferCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心", _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3") + $"({config.WaferCameraOrigion.X})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3") + $"({config.WaferCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3") + $"({config.WaferCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferChipPPOrigion.X == config.WaferChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Y == config.WaferChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Z == config.WaferChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心", _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3") + $"({config.WaferChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3") + $"({config.WaferChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3") + $"({config.WaferChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferSubmountPPOrigion.X == config.WaferSubmountPPOrigion.X &&
                            _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y == config.WaferSubmountPPOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z == config.WaferSubmountPPOrigion.Z)
                        {
                            text1 = new TextContent("基底吸嘴到晶圆相机中心", _systemConfig.PositioningConfig.WaferSubmountPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferSubmountPPOrigion.X.ToString("F3") + $"({config.WaferSubmountPPOrigion.X})",
                                _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y.ToString("F3") + $"({config.WaferSubmountPPOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z.ToString("F3") + $"({config.WaferSubmountPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 晶圆盘原点到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferOrigion.X == config.WaferOrigion.X &&
                            _systemConfig.PositioningConfig.WaferOrigion.Y == config.WaferOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferOrigion.Z == config.WaferOrigion.Z)
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心", _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3") + $"({config.WaferOrigion.X})",
                                _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3") + $"({config.WaferOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3") + $"({config.WaferOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到共晶焊接位置
                        if (_systemConfig.PositioningConfig.EutecticWeldingLocation.X == config.EutecticWeldingLocation.X &&
                            _systemConfig.PositioningConfig.EutecticWeldingLocation.Y == config.EutecticWeldingLocation.Y &&
                            _systemConfig.PositioningConfig.EutecticWeldingLocation.Z == config.EutecticWeldingLocation.Z)
                        {
                            text1 = new TextContent("榜头相机到共晶焊接位置", _systemConfig.PositioningConfig.EutecticWeldingLocation.X.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到共晶焊接位置",
                                _systemConfig.PositioningConfig.EutecticWeldingLocation.X.ToString("F3") + $"({config.EutecticWeldingLocation.X})",
                                _systemConfig.PositioningConfig.EutecticWeldingLocation.Y.ToString("F3") + $"({config.EutecticWeldingLocation.Y})",
                                _systemConfig.PositioningConfig.EutecticWeldingLocation.Z.ToString("F3") + $"({config.EutecticWeldingLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到共晶焊接位置
                        if (_systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X == config.EutecticWeldingChipPPLocation.X &&
                            _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y == config.EutecticWeldingChipPPLocation.Y &&
                            _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z == config.EutecticWeldingChipPPLocation.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到共晶焊接位置", _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到共晶焊接位置",
                                _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X.ToString("F3") + $"({config.EutecticWeldingChipPPLocation.X})",
                                _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y.ToString("F3") + $"({config.EutecticWeldingChipPPLocation.Y})",
                                _systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z.ToString("F3") + $"({config.EutecticWeldingChipPPLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴到共晶焊接位置
                        if (_systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X == config.EutecticWeldingSubmountPPLocation.X &&
                            _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y == config.EutecticWeldingSubmountPPLocation.Y &&
                            _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z == config.EutecticWeldingSubmountPPLocation.Z)
                        {
                            text1 = new TextContent("基底吸嘴到共晶焊接位置", _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴到共晶焊接位置",
                                _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X.ToString("F3") + $"({config.EutecticWeldingSubmountPPLocation.X})",
                                _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y.ToString("F3") + $"({config.EutecticWeldingSubmountPPLocation.Y})",
                                _systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z.ToString("F3") + $"({config.EutecticWeldingSubmountPPLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        #endregion

                    }
                    else if (DeviceMode == 1)
                    {
                        #region 显示文本


                        Text1 = new List<TextContent>()
                        {
                            new TextContent("BondCamera",(_BondcameraConfig.WidthPixelSize*1000).ToString("F3"),(_BondcameraConfig.HeightPixelSize*1000).ToString("F3"),(_BondcameraConfig.Angle).ToString("F3")),
                            new TextContent("WaferCamera",(_WafercameraConfig.WidthPixelSize*1000).ToString("F3"),(_WafercameraConfig.HeightPixelSize*1000).ToString("F3"),(_WafercameraConfig.Angle).ToString("F3")),
                            new TextContent("UplookingCamera",(_UplookingcameraConfig.WidthPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.HeightPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.Angle).ToString("F3")),


                        };



                        // 系统原点
                        if (_systemConfig.PositioningConfig.BondOrigion.X == config.BondOrigion.X &&
                            _systemConfig.PositioningConfig.BondOrigion.Y == config.BondOrigion.Y &&
                            _systemConfig.PositioningConfig.BondOrigion.Z == config.BondOrigion.Z)
                        {
                            text1 = new TextContent("系统原点", _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("系统原点",
                                _systemConfig.PositioningConfig.BondOrigion.X.ToString("F3") + $"({config.BondOrigion.X})",
                                _systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3") + $"({config.BondOrigion.Y})",
                                _systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3") + $"({config.BondOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头安全位置
                        if (_systemConfig.PositioningConfig.BondSafeLocation.X == config.BondSafeLocation.X &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Y == config.BondSafeLocation.Y &&
                            _systemConfig.PositioningConfig.BondSafeLocation.Z == config.BondSafeLocation.Z)
                        {
                            text1 = new TextContent("榜头安全位置", _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3"), _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头安全位置",
                                _systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3") + $"({config.BondSafeLocation.X})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3") + $"({config.BondSafeLocation.Y})",
                                _systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3") + $"({config.BondSafeLocation.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupCameraOrigion.X == config.LookupCameraOrigion.X &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Y == config.LookupCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupCameraOrigion.Z == config.LookupCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心", _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3") + $"({config.LookupCameraOrigion.X})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3") + $"({config.LookupCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3") + $"({config.LookupCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupChipPPOrigion.X == config.LookupChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Y == config.LookupChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupChipPPOrigion.Z == config.LookupChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心", _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3") + $"({config.LookupChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3") + $"({config.LookupChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3") + $"({config.LookupChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到仰视相机中心
                        if (_systemConfig.PositioningConfig.LookupLaserSensorOrigion.X == config.LookupLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y == config.LookupLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z == config.LookupLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心", _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到仰视相机中心",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3") + $"({config.LookupLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3") + $"({config.LookupLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3") + $"({config.LookupLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到轨道原点
                        if (_systemConfig.PositioningConfig.TrackOrigion.X == config.TrackOrigion.X &&
                            _systemConfig.PositioningConfig.TrackOrigion.Y == config.TrackOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackOrigion.Z == config.TrackOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到轨道原点", _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到轨道原点",
                                _systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3") + $"({config.TrackOrigion.X})",
                                _systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3") + $"({config.TrackOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3") + $"({config.TrackOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到轨道原点
                        if (_systemConfig.PositioningConfig.TrackChipPPOrigion.X == config.TrackChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Y == config.TrackChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackChipPPOrigion.Z == config.TrackChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点", _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到轨道原点",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3") + $"({config.TrackChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3") + $"({config.TrackChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3") + $"({config.TrackChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器到轨道原点
                        if (_systemConfig.PositioningConfig.TrackLaserSensorOrigion.X == config.TrackLaserSensorOrigion.X &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y == config.TrackLaserSensorOrigion.Y &&
                            _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z == config.TrackLaserSensorOrigion.Z)
                        {
                            text1 = new TextContent("激光传感器到轨道原点", _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器到轨道原点",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3") + $"({config.TrackLaserSensorOrigion.X})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3") + $"({config.TrackLaserSensorOrigion.Y})",
                                _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3") + $"({config.TrackLaserSensorOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 点胶针点胶位置
                        if (_systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.X == config.TrackEpoxtSpotCoordinate.X &&
                            _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Y == config.TrackEpoxtSpotCoordinate.Y &&
                            _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Z == config.TrackEpoxtSpotCoordinate.Z)
                        {
                            text1 = new TextContent("点胶针点胶位置", _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.X.ToString("F3"), _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("点胶针点胶位置",
                                _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.X.ToString("F3") + $"({config.TrackEpoxtSpotCoordinate.X})",
                                _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Y.ToString("F3") + $"({config.TrackEpoxtSpotCoordinate.Y})",
                                _systemConfig.PositioningConfig.TrackEpoxtSpotCoordinate.Z.ToString("F3") + $"({config.TrackEpoxtSpotCoordinate.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 相机对准点胶位置
                        if (_systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.X == config.TrackBondCameraToEpoxtSpotCoordinate.X &&
                            _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Y == config.TrackBondCameraToEpoxtSpotCoordinate.Y &&
                            _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Z == config.TrackBondCameraToEpoxtSpotCoordinate.Z)
                        {
                            text1 = new TextContent("相机对准点胶位置", _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.X.ToString("F3"), _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Y.ToString("F3"), _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("相机对准点胶位置",
                                _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.X.ToString("F3") + $"({config.TrackBondCameraToEpoxtSpotCoordinate.X})",
                                _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Y.ToString("F3") + $"({config.TrackBondCameraToEpoxtSpotCoordinate.Y})",
                                _systemConfig.PositioningConfig.TrackBondCameraToEpoxtSpotCoordinate.Z.ToString("F3") + $"({config.TrackBondCameraToEpoxtSpotCoordinate.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机识别轨道原点Z
                        if (_systemConfig.PositioningConfig.TrackLaserSensorZ == config.TrackLaserSensorZ)
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机识别轨道原点Z", "", "", _systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3") + $"({config.TrackLaserSensorZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴空闲Z
                        if (_systemConfig.PositioningConfig.SubmountPPFreeZ == config.SubmountPPFreeZ)
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴空闲Z", "", "", _systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3") + $"({config.SubmountPPFreeZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 基底吸嘴工作Z
                        if (_systemConfig.PositioningConfig.SubmountPPWorkZ == config.SubmountPPWorkZ)
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("基底吸嘴工作Z", "", "", _systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3") + $"({config.SubmountPPWorkZ})", Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴与榜头相机偏移
                        if (_systemConfig.PositioningConfig.PP1AndBondCameraOffset.X == config.PP1AndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y == config.PP1AndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z == config.PP1AndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移", _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴与榜头相机偏移",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3") + $"({config.PP1AndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3") + $"({config.PP1AndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3") + $"({config.PP1AndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 点胶针与榜头相机偏移
                        if (_systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X == config.EpoxtAndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y == config.EpoxtAndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Z == config.EpoxtAndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("点胶针与榜头相机偏移", _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("点胶针与榜头相机偏移",
                                _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.X.ToString("F3") + $"({config.EpoxtAndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Y.ToString("F3") + $"({config.EpoxtAndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.EpoxtAndBondCameraOffset.Z.ToString("F3") + $"({config.EpoxtAndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 激光传感器与榜头相机偏移
                        if (_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X == config.LaserSensorAndBondCameraOffset.X &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y == config.LaserSensorAndBondCameraOffset.Y &&
                            _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z == config.LaserSensorAndBondCameraOffset.Z)
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移", _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3"), _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("激光传感器与榜头相机偏移",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.X})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Y})",
                                _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3") + $"({config.LaserSensorAndBondCameraOffset.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 榜头相机到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferCameraOrigion.X == config.WaferCameraOrigion.X &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Y == config.WaferCameraOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferCameraOrigion.Z == config.WaferCameraOrigion.Z)
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心", _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("榜头相机到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3") + $"({config.WaferCameraOrigion.X})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3") + $"({config.WaferCameraOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3") + $"({config.WaferCameraOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 芯片吸嘴到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferChipPPOrigion.X == config.WaferChipPPOrigion.X &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Y == config.WaferChipPPOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferChipPPOrigion.Z == config.WaferChipPPOrigion.Z)
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心", _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("芯片吸嘴到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3") + $"({config.WaferChipPPOrigion.X})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3") + $"({config.WaferChipPPOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3") + $"({config.WaferChipPPOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);

                        // 晶圆盘原点到晶圆相机中心
                        if (_systemConfig.PositioningConfig.WaferOrigion.X == config.WaferOrigion.X &&
                            _systemConfig.PositioningConfig.WaferOrigion.Y == config.WaferOrigion.Y &&
                            _systemConfig.PositioningConfig.WaferOrigion.Z == config.WaferOrigion.Z)
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心", _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3"), _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3"));
                        }
                        else
                        {
                            text1 = new TextContent("晶圆盘原点到晶圆相机中心",
                                _systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3") + $"({config.WaferOrigion.X})",
                                _systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3") + $"({config.WaferOrigion.Y})",
                                _systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3") + $"({config.WaferOrigion.Z})",
                                Color.Red);
                        }
                        Text2.Add(text1);


                        #endregion
                    }



                    ListmessagesBox myMessageBox1 = new ListmessagesBox();
                    myMessageBox1.ShowText(str);
                    myMessageBox1.DataGridView1Show(Text1);
                    myMessageBox1.DataGridView2Show(Text2);
                    myMessageBox1.Show();
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Done = ShowMessageAsync("动作确认", "是否保存系统校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        _systemConfig.PositioningConfig = config;
                        _systemConfig.SaveConfig();
                    }
                    Done = ShowMessage("动作确认", "结束校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        //_systemConfig.SaveConfig();
                    }

                }
                catch
                {

                }

            }
            ));

        }

        /// <summary>
        /// 系统初始化
        /// </summary>
        public void Initialization(int Mode)
        {
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    //榜头相机移动到安全位置
                    BondToSafeAsync();


                    //榜头相机移动到系统原点，自动对焦，识别
                    BondCameraIdentifyBondOrigionInt(Mode);


                    //榜头移动到安全位置
                    BondToSafeAsync();



                }));



            }
            catch
            {

            }
        }


        public void ChipRun(string PPtoolname,int Mode = 0)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            ShowDynamometer();

            config = _systemConfig.PositioningConfig;

            if(PPtoolname == "UC")
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    int Done = -1;
                    try
                    {
                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        bool Done1 = false;

                        #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync(0);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                            Done1 = UplookingCameraIdentifyBond1(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴坐标失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }

                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头1吸嘴移动到仰视相机中心，测量吸嘴偏移");
                        while (true)
                        {
                            Debug.WriteLine("榜头1吸嘴移动到仰视相机中心，测量吸嘴偏移");
                            Done1 = UplookingCameraIdentifyBond1RotationCompensation(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头移动到安全位置");
                        BondToSafeAsync();


                        #endregion


                        #region 榜头相机校准 榜头测高

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头移动到测高位置，测高
                        while (true)
                        {
                            Debug.WriteLine("榜头移动到测高位置，测高");
                            Done1 = BondToHeightmeasurementposition(1, EnumMaskType.TrackOrigion, Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Chip吸嘴移动到轨道原点测高失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }





                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }



                        #endregion

                        //榜头移动到安全位置
                        BondToSafeAsync();

                        //CloseStageAxisMove();
                        //CloseStage();
                        //CloseDynamometer();


                        //}

                        //提示取下CCU工具
                        Done = ShowMessageAsync("动作确认", "是否保存Chip吸嘴校准", "提示");
                        if (Done == 0)
                        {
                            //return;
                        }
                        else
                        {
                            _systemConfig.PositioningConfig = config;
                            _systemConfig.SaveConfig();
                        }
                        //提示取下CCU工具
                        Done = ShowMessage("动作确认", "结束校准", "提示");
                        if (Done == 0)
                        {
                            //return;
                        }
                        else
                        {
                            //_systemConfig.SaveConfig();
                        }
                    }
                    catch
                    {

                    }

                }
                ));
            }
            else
            {
                currentppTool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.PPName == PPtoolname);

                if (currentppTool != null)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        int Done = -1;
                        try
                        {
                            if (CameraWindowGUI.Instance != null)
                            {
                                CameraWindowGUI.Instance.SelectCamera(0);
                            }

                            bool Done1 = false;

                            #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                            Debug.WriteLine("榜头相机移动到安全位置");
                            BondToSafeAsync(0);

                            if (CameraWindowGUI.Instance != null)
                            {
                                CameraWindowGUI.Instance.ClearGraphicDraw();
                                CameraWindowGUI.Instance.SelectCamera(1);
                            }

                            while (true)
                            {
                                Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                                Done1 = UplookingCameraIdentifyPPtool(Mode);

                                if (Done1 == false)
                                {
                                    Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴坐标失败，是否重新创建模板继续校准", "提示");
                                    if (Done == 0)
                                    {
                                        Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                        if (Done == 0)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            Mode = 0;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Mode = 1;
                                        continue;
                                    }
                                }

                                Mode = 0;
                                break;
                            }


                            if (CameraWindowGUI.Instance != null)
                            {
                                CameraWindowGUI.Instance.ClearGraphicDraw();
                                CameraWindowGUI.Instance.SelectCamera(1);
                            }

                            Debug.WriteLine("榜头1吸嘴移动到仰视相机中心，测量吸嘴偏移");
                            while (true)
                            {
                                Debug.WriteLine("榜头1吸嘴移动到仰视相机中心，测量吸嘴偏移");
                                Done1 = UplookingCameraIdentifyPPtoolRotationCompensation(Mode);

                                if (Done1 == false)
                                {
                                    Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                                    if (Done == 0)
                                    {
                                        Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                        if (Done == 0)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            Mode = 0;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Mode = 1;
                                        continue;
                                    }
                                }


                                Mode = 0;
                                break;
                            }


                            if (CameraWindowGUI.Instance != null)
                            {
                                CameraWindowGUI.Instance.ClearGraphicDraw();
                                CameraWindowGUI.Instance.SelectCamera(0);
                            }

                            Debug.WriteLine("榜头移动到安全位置");
                            BondToSafeAsync();


                            #endregion


                            #region 榜头相机校准 榜头测高

                            if (CameraWindowGUI.Instance != null)
                            {
                                CameraWindowGUI.Instance.ClearGraphicDraw();
                                CameraWindowGUI.Instance.SelectCamera(0);
                            }

                            //榜头移动到测高位置，测高
                            while (true)
                            {
                                Debug.WriteLine("榜头移动到测高位置，测高");
                                Done1 = BondToHeightmeasurementposition(0, EnumMaskType.TrackOrigion, Mode);

                                if (Done1 == false)
                                {
                                    Done = ShowMessage("校准异常", "Chip吸嘴移动到轨道原点测高失败，是否继续校准", "提示");
                                    if (Done == 0)
                                    {
                                        Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                        if (Done == 0)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            Mode = 0;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Mode = 1;
                                        continue;
                                    }
                                }


                                Mode = 0;
                                break;
                            }





                            if (CameraWindowGUI.Instance != null)
                            {
                                CameraWindowGUI.Instance.ClearGraphicDraw();
                                CameraWindowGUI.Instance.SelectCamera(0);
                            }



                            #endregion

                            //榜头移动到安全位置
                            BondToSafeAsync();

                            //CloseStageAxisMove();
                            //CloseStage();
                            //CloseDynamometer();


                            //}

                            //提示取下CCU工具
                            Done = ShowMessageAsync("动作确认", "是否保存Chip吸嘴校准", "提示");
                            if (Done == 0)
                            {
                                //return;
                            }
                            else
                            {
                                _systemConfig.PositioningConfig = config;
                                _systemConfig.SaveConfig();
                            }
                            //提示取下CCU工具
                            Done = ShowMessage("动作确认", "结束校准", "提示");
                            if (Done == 0)
                            {
                                //return;
                            }
                            else
                            {
                                //_systemConfig.SaveConfig();
                            }
                        }
                        catch
                        {

                        }

                    }
                    ));
                }
                else
                {
                    int Done = ShowMessage("动作确认", "该吸嘴不存在", "提示");
                }

            }



        }

        public PPToolSettings ChipRunParam(string PPtoolname, int Mode = 0)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            config = _systemConfig.PositioningConfig;

            currentppTool = _systemConfig.PPToolSettings?.FirstOrDefault(tool => tool.PPName == PPtoolname);

            if (currentppTool != null)
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    int Done = -1;
                    try
                    {
                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        bool Done1 = false;

                        #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                        Debug.WriteLine("榜头相机移动到安全位置");
                        BondToSafeAsync(0);

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        while (true)
                        {
                            Debug.WriteLine("榜头1移动到仰视相机中心，仰视相机自动对焦，识别UC");
                            Done1 = UplookingCameraIdentifyPPtool(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴坐标失败，是否重新创建模板继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }

                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(1);
                        }

                        Debug.WriteLine("榜头1吸嘴移动到仰视相机中心，测量吸嘴偏移");
                        while (true)
                        {
                            Debug.WriteLine("榜头1吸嘴移动到仰视相机中心，测量吸嘴偏移");
                            Done1 = UplookingCameraIdentifyPPtoolRotationCompensation(Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }


                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        Debug.WriteLine("榜头移动到安全位置");
                        BondToSafeAsync();


                        #endregion


                        #region 榜头相机校准 榜头测高

                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }

                        //榜头移动到测高位置，测高
                        while (true)
                        {
                            Debug.WriteLine("榜头移动到测高位置，测高");
                            Done1 = BondToHeightmeasurementposition(0, EnumMaskType.TrackOrigion, Mode);

                            if (Done1 == false)
                            {
                                Done = ShowMessage("校准异常", "Chip吸嘴移动到轨道原点测高失败，是否继续校准", "提示");
                                if (Done == 0)
                                {
                                    Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                    if (Done == 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        Mode = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Mode = 1;
                                    continue;
                                }
                            }


                            Mode = 0;
                            break;
                        }





                        if (CameraWindowGUI.Instance != null)
                        {
                            CameraWindowGUI.Instance.ClearGraphicDraw();
                            CameraWindowGUI.Instance.SelectCamera(0);
                        }



                        #endregion

                        //榜头移动到安全位置
                        BondToSafeAsync();

                        CloseStageAxisMove();
                        CloseStage();
                        CloseDynamometer();


                        //}

                        ////提示取下CCU工具
                        //Done = ShowMessageAsync("动作确认", "是否保存Chip吸嘴校准", "提示");
                        //if (Done == 0)
                        //{
                        //    //return;
                        //}
                        //else
                        //{
                        //    _systemConfig.PositioningConfig = config;
                        //    _systemConfig.SaveConfig();
                        //}
                        ////提示取下CCU工具
                        //Done = ShowMessage("动作确认", "结束校准", "提示");
                        //if (Done == 0)
                        //{
                        //    //return;
                        //}
                        //else
                        //{
                        //    //_systemConfig.SaveConfig();
                        //}
                    }
                    catch
                    {

                    }

                }
));
            }
            else
            {
                int Done = ShowMessage("动作确认", "该吸嘴不存在", "提示");
            }

            return currentppTool;
        }


        public void SubmountRun(int Mode = 0)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            config = _systemConfig.PositioningConfig;

            Task.Factory.StartNew(new Action(() =>
            {
                int Done = -1;
                bool Done1 = false;
                try
                {
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    #region 仰视相机校准 测量榜头X、Y相对位置 吸嘴校准

                    Debug.WriteLine("榜头相机移动到安全位置");
                    BondToSafeAsync(0);

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(1);
                    }

                    Debug.WriteLine("榜头2移动到仰视相机中心，仰视相机自动对焦，识别UC");
                    while (true)
                    {
                        Debug.WriteLine("榜头2移动到仰视相机中心，仰视相机自动对焦，识别UC");
                        Done1 = UplookingCameraIdentifyBond2(Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "仰视相机校准Submount吸嘴失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }
                        Mode = 0;
                        break;
                    }

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(1);
                    }

                    Debug.WriteLine("榜头2UC移动到仰视相机中心，测量吸嘴偏移");
                    while (true)
                    {
                        Debug.WriteLine("榜头2UC移动到仰视相机中心，测量吸嘴偏移");
                        Done1 = UplookingCameraIdentifyBond2RotationCompensation(Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "仰视相机校准Chip吸嘴旋转偏移失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }


                        Mode = 0;
                        break;
                    }


                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Debug.WriteLine("榜头移动到安全位置");
                    BondToSafeAsync();


                    #endregion


                    #region 榜头相机校准 榜头测高

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    //榜头移动到测高位置，测高
                    while (true)
                    {
                        Debug.WriteLine("榜头移动到测高位置，测高");
                        Done1 = BondToHeightmeasurementposition(2, EnumMaskType.TrackOrigion, Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "Submount吸嘴移动到轨道原点测高失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }


                        Mode = 0;
                        break;
                    }




                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }



                    #endregion

                    //榜头移动到安全位置
                    BondToSafeAsync();

                    CloseStageAxisMove();
                    CloseStage();
                    CloseDynamometer();


                    //}

                    //提示取下CCU工具
                    Done = ShowMessageAsync("动作确认", "是否保存Submount吸嘴校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        _systemConfig.PositioningConfig = config;
                        _systemConfig.SaveConfig();
                    }
                    //提示取下CCU工具
                    Done = ShowMessage("动作确认", "结束校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        //_systemConfig.SaveConfig();
                    }
                }
                catch
                {

                }

            }
            ));

        }

        public void EpoxtRun(int Mode = 0)
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            config = _systemConfig.PositioningConfig;

            Task.Factory.StartNew(new Action(() =>
            {
                int Done = -1;
                bool Done1 = false;
                try
                {
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }
                    #region 榜头相机校准 榜头测高

                    while (true)
                    {
                        Debug.WriteLine("榜头移动到点胶位置");
                        Done1 = BondToEpoxtSpot(Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "点胶校准失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }


                        Mode = 0;
                        break;
                    }

                    #endregion

                    //榜头移动到安全位置
                    BondToSafeAsync();

                    CloseStageAxisMove();
                    CloseStage();
                    CloseDynamometer();


                    //}

                    //提示取下CCU工具
                    Done = ShowMessageAsync("动作确认", "是否保存Submount吸嘴校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        _systemConfig.PositioningConfig = config;
                        _systemConfig.SaveConfig();
                    }
                    //提示取下CCU工具
                    Done = ShowMessage("动作确认", "结束校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        //_systemConfig.SaveConfig();
                    }

                }
                catch
                {

                }

            }
            ));

        }


        /// <summary>
        /// 自动校准
        /// </summary>
        public void EutecticWeldingRun()
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            Task.Factory.StartNew(new Action(() =>
            {
                int Done = -1;
                int Mode = 0;
                bool Done1 = false;
                try
                {
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Debug.WriteLine("榜头相机移动到安全位置");
                    BondToSafeAsync(Mode);

                    #region 共晶台校准 共晶台测高


                    //提示安装测高工具
                    Done = ShowMessage("动作确认", "开始校准共晶台，去掉测高工具", "提示");
                    if (Done == 0)
                    {
                        return;
                    }

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    while (true)
                    {
                        Debug.WriteLine("榜头相机移动到共晶台，识别共晶台");
                        Done1 = BondCameraIdentifyEutecticWeldingOrigion(Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "榜头相机移动到共晶台，校准共晶台失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }


                        Mode = 0;
                        break;
                    }


                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }


                    while (true)
                    {
                        Debug.WriteLine("榜头在共晶台附近测高");
                        Done1 = BondToHeightmeasurementposition(3, EnumMaskType.EutecticWeldingOrigion, Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "激光传感器在共晶台附近测高失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }


                        Mode = 0;
                        break;
                    }

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }


                    #endregion

                    //榜头移动到安全位置
                    BondToSafeAsync();


                }
                catch
                {

                }
                finally
                {
                    //if ((CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                    //{
                    //    CameraWindowForm.Instance.Hide();
                    //}
                    CloseStageAxisMove();
                    CloseStage();
                    CloseDynamometer();



                    string str = "系统";
                    List<TextContent> Text1 = new List<TextContent>()
                        {
                            new TextContent("BondCamera",(_BondcameraConfig.WidthPixelSize*1000).ToString("F3"),(_BondcameraConfig.HeightPixelSize*1000).ToString("F3"),(_BondcameraConfig.Angle).ToString("F3")),
                            new TextContent("WaferCamera",(_WafercameraConfig.WidthPixelSize*1000).ToString("F3"),(_WafercameraConfig.HeightPixelSize*1000).ToString("F3"),(_WafercameraConfig.Angle).ToString("F3")),
                            new TextContent("UplookingCamera",(_UplookingcameraConfig.WidthPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.HeightPixelSize * 1000).ToString("F3"),(_UplookingcameraConfig.Angle).ToString("F3")),


                        };

                    List<TextContent> Text2 = new List<TextContent>()
                        {
                            new TextContent("系统原点",_systemConfig.PositioningConfig.BondOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.BondOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.BondOrigion.Z.ToString("F3")),
                            new TextContent("榜头安全位置",_systemConfig.PositioningConfig.BondSafeLocation.X.ToString("F3"),_systemConfig.PositioningConfig.BondSafeLocation.Y.ToString("F3"),_systemConfig.PositioningConfig.BondSafeLocation.Z.ToString("F3")),

                            new TextContent("榜头相机到仰视相机中心",_systemConfig.PositioningConfig.LookupCameraOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.LookupCameraOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.LookupCameraOrigion.Z.ToString("F3")),
                            new TextContent("芯片吸嘴到仰视相机中心",_systemConfig.PositioningConfig.LookupChipPPOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.LookupChipPPOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.LookupChipPPOrigion.Z.ToString("F3")),
                            new TextContent("基底吸嘴到仰视相机中心",_systemConfig.PositioningConfig.LookupSubmountPPOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.LookupSubmountPPOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.LookupSubmountPPOrigion.Z.ToString("F3")),
                            new TextContent("激光传感器到仰视相机中心",_systemConfig.PositioningConfig.LookupLaserSensorOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.LookupLaserSensorOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.LookupLaserSensorOrigion.Z.ToString("F3")),


                            new TextContent("榜头相机到轨道原点",_systemConfig.PositioningConfig.TrackOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.TrackOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.TrackOrigion.Z.ToString("F3")),
                            new TextContent("芯片吸嘴到轨道原点",_systemConfig.PositioningConfig.TrackChipPPOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.TrackChipPPOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.TrackChipPPOrigion.Z.ToString("F3")),
                            new TextContent("基底吸嘴到轨道原点",_systemConfig.PositioningConfig.TrackSubmountPPOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.TrackSubmountPPOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.TrackSubmountPPOrigion.Z.ToString("F3")),
                            new TextContent("激光传感器到轨道原点",_systemConfig.PositioningConfig.TrackLaserSensorOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.TrackLaserSensorOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z.ToString("F3")),
                            new TextContent("榜头相机识别轨道原点Z","","",_systemConfig.PositioningConfig.TrackLaserSensorZ.ToString("F3")),
                            new TextContent("基底吸嘴空闲Z","","",_systemConfig.PositioningConfig.SubmountPPFreeZ.ToString("F3")),
                            new TextContent("基底吸嘴工作Z","","",_systemConfig.PositioningConfig.SubmountPPWorkZ.ToString("F3")),


                            new TextContent("芯片吸嘴与榜头相机偏移",_systemConfig.PositioningConfig.PP1AndBondCameraOffset.X.ToString("F3"),_systemConfig.PositioningConfig.PP1AndBondCameraOffset.Y.ToString("F3"),_systemConfig.PositioningConfig.PP1AndBondCameraOffset.Z.ToString("F3")),
                            new TextContent("基底吸嘴与榜头相机偏移",_systemConfig.PositioningConfig.PP2AndBondCameraOffset.X.ToString("F3"),_systemConfig.PositioningConfig.PP2AndBondCameraOffset.Y.ToString("F3"),_systemConfig.PositioningConfig.PP2AndBondCameraOffset.Z.ToString("F3")),
                            new TextContent("激光传感器与榜头相机偏移",_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X.ToString("F3"),_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y.ToString("F3"),_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Z.ToString("F3")),

                            new TextContent("榜头相机到晶圆相机中心",_systemConfig.PositioningConfig.WaferCameraOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.WaferCameraOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.WaferCameraOrigion.Z.ToString("F3")),
                            new TextContent("芯片吸嘴到晶圆相机中心",_systemConfig.PositioningConfig.WaferChipPPOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.WaferChipPPOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.WaferChipPPOrigion.Z.ToString("F3")),
                            new TextContent("基底吸嘴到晶圆相机中心",_systemConfig.PositioningConfig.WaferSubmountPPOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.WaferSubmountPPOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.WaferSubmountPPOrigion.Z.ToString("F3")),
                            new TextContent("晶圆盘原点到晶圆相机中心",_systemConfig.PositioningConfig.WaferOrigion.X.ToString("F3"),_systemConfig.PositioningConfig.WaferOrigion.Y.ToString("F3"),_systemConfig.PositioningConfig.WaferOrigion.Z.ToString("F3")),

                            new TextContent("榜头相机到共晶焊接位置",_systemConfig.PositioningConfig.EutecticWeldingLocation.X.ToString("F3"),_systemConfig.PositioningConfig.EutecticWeldingLocation.Y.ToString("F3"),_systemConfig.PositioningConfig.EutecticWeldingLocation.Z.ToString("F3")),
                            new TextContent("芯片吸嘴到共晶焊接位置",_systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.X.ToString("F3"),_systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Y.ToString("F3"),_systemConfig.PositioningConfig.EutecticWeldingChipPPLocation.Z.ToString("F3")),
                            new TextContent("基底吸嘴到共晶焊接位置",_systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.X.ToString("F3"),_systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Y.ToString("F3"),_systemConfig.PositioningConfig.EutecticWeldingSubmountPPLocation.Z.ToString("F3")),


                        };


                    ListmessagesBox myMessageBox1 = new ListmessagesBox();
                    myMessageBox1.ShowText(str);
                    myMessageBox1.DataGridView1Show(Text1);
                    myMessageBox1.DataGridView2Show(Text2);
                    myMessageBox1.Show();

                    //using (ListmessagesBox myMessageBox1 = new ListmessagesBox())
                    //{
                    //    myMessageBox1.ShowText(str);
                    //    myMessageBox1.DataGridView1Show(Text1);
                    //    myMessageBox1.DataGridView1Show(Text2);
                    //    myMessageBox1.Show();



                    //}

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    //提示取下CCU工具
                    Done = ShowMessageAsync("动作确认", "是否保存系统校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        _systemConfig.SaveConfig();
                    }
                    //提示取下CCU工具
                    Done = ShowMessage("动作确认", "结束校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        //_systemConfig.SaveConfig();
                    }
                }

            }
            ));


        }


        /// <summary>
        /// 自动校准校准台
        /// </summary>
        public void CalibrationTableRun()
        {
            if (CameraWindowGUI.Instance != null)
            {
                CameraWindowGUI.Instance.SelectCamera(0);
            }
            if (!(CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
            {
                CameraWindowForm.Instance.ShowLocation(new Point(200, 200));
                CameraWindowForm.Instance.Show();
            }

            Task.Factory.StartNew(new Action(() =>
            {
                int Done = -1;
                int Mode = 0;
                bool Done1 = false;
                try
                {
                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    Debug.WriteLine("榜头相机移动到安全位置");
                    BondToSafeAsync(Mode);

                    #region 校准台校准


                    //提示安装测高工具
                    Done = ShowMessage("动作确认", "开始校准校准台，去掉测高工具", "提示");
                    if (Done == 0)
                    {
                        return;
                    }

                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    while (true)
                    {
                        Debug.WriteLine("榜头相机移动到校准台，识别校准台");
                        Done1 = BondCameraIdentifyCalibrationTableOrigion(Mode);

                        if (Done1 == false)
                        {
                            Done = ShowMessage("校准异常", "榜头相机移动到校准台，校准校准台失败，是否继续校准", "提示");
                            if (Done == 0)
                            {
                                Done = ShowMessage("动作确认", "是否继续进行其他坐标校准", "提示");
                                if (Done == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    Mode = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Mode = 1;
                                continue;
                            }
                        }


                        Mode = 0;
                        break;
                    }


                    if (CameraWindowGUI.Instance != null)
                    {
                        CameraWindowGUI.Instance.ClearGraphicDraw();
                        CameraWindowGUI.Instance.SelectCamera(0);
                    }

                    #endregion

                    //榜头移动到安全位置
                    BondToSafeAsync();


                }
                catch
                {

                }
                finally
                {
                    //if ((CameraWindowForm.Instance.IsHandleCreated && CameraWindowForm.Instance.Visible))
                    //{
                    //    CameraWindowForm.Instance.Hide();
                    //}
                    CloseStageAxisMove();
                    CloseStage();
                    CloseDynamometer();




                    //提示取下CCU工具
                    Done = ShowMessageAsync("动作确认", "是否保存系统校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        _systemConfig.SaveConfig();
                    }
                    //提示取下CCU工具
                    Done = ShowMessage("动作确认", "结束校准", "提示");
                    if (Done == 0)
                    {
                        //return;
                    }
                    else
                    {
                        //_systemConfig.SaveConfig();
                    }
                }

            }
            ));


        }


        #endregion

    }
}
