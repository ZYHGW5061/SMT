using BoardCardControllerClsLib;
using DevExpress.XtraCharts;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
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

namespace DynamometerGUI
{
    public partial class PressureCurveForm : Form
    {
        private SynchronizationContext _syncContext;

        IBoardCardController _boardCardController;


        public PressureCurveForm()
        {
            InitializeComponent();

            InitDevChart();

            

            _syncContext = SynchronizationContext.Current;

            DataModel.Instance.PropertyChanged += DataModel_PropertyChanged;
        }

        private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_syncContext == null)
            {
                return;
            }

            #region SerialPort

            if (e.PropertyName == nameof(DataModel.PressureValue1))
            {
                _syncContext.Post(_ => {
                    //double newValue = DataModel.Instance.PressureValue1;
                    //DateTime now = DateTime.Now;
                    //const string format = "HH:mm:ss";
                    //if (chartControl1.Series.Count == 0)
                    //{
                    //    // 创建新 Series 并添加到控件
                    //    Series series = new Series("平台压力", ViewType.Line);
                    //    chartControl1.Series.Add(series);
                    //}

                    //// 安全访问第一个 Series
                    //Series targetSeries = chartControl1.Series[0];
                    //if(newValue <= 0)
                    //{
                    //    newValue = 0;
                    //}
                    ////string time = now.TimeOfDay.ToString(format);
                    //// 添加带时间戳的数据点
                    //targetSeries.Points.Add(new SeriesPoint(now.TimeOfDay.TotalSeconds, newValue));

                    //// 自动滚动X轴（显示最新20秒）
                    //((XYDiagram)chartControl1?.Diagram)?.AxisX.VisualRange.SetMinMaxValues(
                    //    now.AddSeconds(-100),
                    //    now
                    //);
                }, null);
            }



            #endregion



            #region Motor

            if (e.PropertyName == nameof(DataModel.ZRPressureValue))
            {
                _syncContext.Post(_ => {
                    double newValue = DataModel.Instance.ZRPressureValue;
                    DateTime now = DateTime.Now;

                    // 添加带时间戳的数据点
                    chartControl2.Series[0].Points.Add(new SeriesPoint(now, newValue));

                    // 自动滚动X轴（显示最新20秒）
                    ((XYDiagram)chartControl2.Diagram).AxisX.VisualRange.SetMinMaxValues(
                        now.AddSeconds(-20),
                        now
                    );
                }, null);
            }

            if (e.PropertyName == nameof(DataModel.ZRAxisForceStation))
            {
                _syncContext.Post(_ => {
                    int Done = DataModel.Instance.ZRAxisForceStation;
                    switch(Done)
                    {
                        case 0:
                            label14.Text = "未激活";
                            break;
                        case 1:
                            label14.Text = "力控已完成";
                            break;
                        case 2:
                            label14.Text = "快进中";
                            break;
                        case 4:
                            label14.Text = "保压中";
                            break;
                        case 5:
                            label14.Text = "回退中";
                            break;
                        case 9:
                            label14.Text = "保留状态";
                            break;
                        default:
                            label14.Text = "其他状态";
                            break;
                    }
                }, null);
            }


            #endregion



        }


        // 初始化图表
        private void InitDevChart()
        {
            _boardCardController = BoardCardManager.Instance.GetCurrentController();
            double speedPos = 0;
            double keepTime = 0;
            double switchPos = 0;
            double backPos = 0;
            double speed = 0;
            double firstSpeed = 0;
            double secondSpeed = 0;
            double currentLimit = 0;
            _boardCardController.Get_ZRForceParamters(EnumStageAxis.SubmountPPZ, out speedPos, out keepTime, out switchPos, out backPos, out speed, out firstSpeed, out secondSpeed, out currentLimit);
            numericUpDown1.Value = (decimal)speedPos;
            numericUpDown2.Value = (decimal)keepTime;
            numericUpDown3.Value = (decimal)switchPos;
            numericUpDown4.Value = (decimal)backPos;
            numericUpDown5.Value = (decimal)speed;
            numericUpDown6.Value = (decimal)firstSpeed;
            numericUpDown7.Value = (decimal)secondSpeed;
            numericUpDown8.Value = (decimal)currentLimit;

            bool mode = _boardCardController.Get_ZRAxisForceMode(EnumStageAxis.SubmountPPZ);
            if(mode)
            {
                label13.Text = "力控模式";
            }
            else
            {
                label13.Text = "位置模式";
            }


            // 创建序列并设置为曲线图
            Series series = new Series("平台压力", ViewType.Spline);
            chartControl1.Series.Add(series);

            // 启用实时模式
            ((XYDiagram)chartControl1.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl1.Diagram).AxisX.VisualRange.SetMinMaxValues(0, 100);

            // 创建序列并设置为曲线图
            Series series2 = new Series("吸嘴压力", ViewType.Spline);
            chartControl2.Series.Add(series2);

            // 启用实时模式
            ((XYDiagram)chartControl2.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl2.Diagram).AxisX.VisualRange.SetMinMaxValues(0, 100);

            //XYDiagram2D diagram = chartControl1.Diagram as XYDiagram2D;
            //((XYDiagram)chartControl1.Diagram).EnableAxisXNavigation = true;
            //XYDiagram diagram = chartControl1.Diagram as XYDiagram;
            //((XYDiagram)chartControl1.Diagram).EnableAxisXNavigation = true;
            //((XYDiagram)chartControl1.Diagram).NavigationOptions.UseMouse = true;


            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.Equals(label13.Text, "位置模式"))
            {
                _boardCardController.Set_ZRAxisWorkMode(EnumStageAxis.SubmountPPZ, 1);
            }
            else if (string.Equals(label13.Text, "力控模式"))
            {
                _boardCardController.Set_ZRAxisWorkMode(EnumStageAxis.SubmountPPZ, 0);
            }
            bool mode = _boardCardController.Get_ZRAxisForceMode(EnumStageAxis.SubmountPPZ);
            if (mode)
            {
                label13.Text = "力控模式";
            }
            else
            {
                label13.Text = "位置模式";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double speedPos = (double)numericUpDown1.Value;
            double keepTime = (double)numericUpDown2.Value;
            double switchPos = (double)numericUpDown3.Value;
            double backPos = (double)numericUpDown4.Value;
            double speed = (double)numericUpDown5.Value;
            double firstSpeed = (double)numericUpDown6.Value;
            double secondSpeed = (double)numericUpDown7.Value;
            double currentLimit = (double)numericUpDown8.Value;
            _boardCardController.Set_ZRForceParamters(EnumStageAxis.SubmountPPZ, speedPos, keepTime, switchPos, backPos, speed, firstSpeed, secondSpeed, currentLimit);

            speedPos = 0;
            keepTime = 0;
            switchPos = 0;
            backPos = 0;
            speed = 0;
            firstSpeed = 0;
            secondSpeed = 0;
            currentLimit = 0;
            _boardCardController.Get_ZRForceParamters(EnumStageAxis.SubmountPPZ, out speedPos, out keepTime, out switchPos, out backPos, out speed, out firstSpeed, out secondSpeed, out currentLimit);
            numericUpDown1.Value = (decimal)speedPos;
            numericUpDown2.Value = (decimal)keepTime;
            numericUpDown3.Value = (decimal)switchPos;
            numericUpDown4.Value = (decimal)backPos;
            numericUpDown5.Value = (decimal)speed;
            numericUpDown6.Value = (decimal)firstSpeed;
            numericUpDown7.Value = (decimal)secondSpeed;
            numericUpDown8.Value = (decimal)currentLimit;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _boardCardController.Enable_ZRAxisForeceMode(EnumStageAxis.SubmountPPZ);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double newValue = DataModel.Instance.PressureValue1;
            DateTime now = DateTime.Now;
            const string format = "HH:mm:ss";
            if (chartControl1.Series.Count == 0)
            {
                // 创建新 Series 并添加到控件
                Series series = new Series("平台压力", ViewType.Line);
                chartControl1.Series.Add(series);
            }

            // 安全访问第一个 Series
            Series targetSeries = chartControl1.Series[0];
            if (newValue <= 0)
            {
                newValue = 0;
            }
            //string time = now.TimeOfDay.ToString(format);
            // 添加带时间戳的数据点
            targetSeries.Points.Add(new SeriesPoint(now.TimeOfDay.TotalSeconds, newValue));

            // 自动滚动X轴（显示最新20秒）
            ((XYDiagram)chartControl1?.Diagram)?.AxisX.VisualRange.SetMinMaxValues(
                now.AddSeconds(-50),
                now
            );

            SaveData((float)newValue, 0);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            _boardCardController.GoBack_ZRAxisForeceMode(EnumStageAxis.SubmountPPZ);
        }


        private void SaveData(float Pressure, float Pressure2)
        {
            string tablename = "PressureData";



            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm:ss:FFF");

            Dictionary<string, SQLData> tableDictionarys5 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData(currentDate, SQLDataType.STRING) },
                { "Time", new SQLData(currentTime, SQLDataType.STRING) },
                { "Pressure1", new SQLData(Pressure, SQLDataType.FLOAT) },
                { "Pressure2", new SQLData(Pressure2, SQLDataType.FLOAT) },
            };

            SQLiteProgram.Instance.AddData(tablename, tableDictionarys5);
        }
    }
}
