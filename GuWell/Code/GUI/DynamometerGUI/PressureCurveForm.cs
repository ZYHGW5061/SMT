using DevExpress.XtraCharts;
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
                    double newValue = DataModel.Instance.PressureValue1;
                    DateTime now = DateTime.Now;

                    // 添加带时间戳的数据点
                    chartControl1.Series[0].Points.Add(new SeriesPoint(now, newValue));

                    // 自动滚动X轴（显示最新20秒）
                    ((XYDiagram)chartControl1.Diagram).AxisX.VisualRange.SetMinMaxValues(
                        now.AddSeconds(-20),
                        now
                    );
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


            #endregion



        }


        // 初始化图表
        private void InitDevChart()
        {
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

        }
    }
}
