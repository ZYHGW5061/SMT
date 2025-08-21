using DevExpress.XtraEditors;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WestDragon.Framework.BaseLoggerClsLib;

namespace SystemGUILib.Alarm
{
    public partial class FrmAlarmHistory : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 数据库日志
        /// </summary>
        private static SQLiteProgram _SQLiteProgram
        {
            get { return SQLiteProgram.Instance; }
        }
        public FrmAlarmHistory()
        {
            InitializeComponent();
        }


        public void ReadDataSystemLog()
        {
            List<LogContent> datas;
            if (_SQLiteProgram != null)
            {
                List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
                tableDictionarys = _SQLiteProgram.ReadDataAlarmLog((int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, (comboBox1.SelectedIndex - 1));


                //排序加转换
                if (tableDictionarys.Count > 0)
                {
                    // 按时间排序（需处理日期时间合并）  
                    var sortedData = tableDictionarys
                        .OrderBy(dict =>
                        {
                            // 合并日期和时间创建DateTime  
                            var dateStr = dict["Date"].Data as string;
                            var timeStr = dict["Time"].Data as string;

                            if (DateTime.TryParseExact($"{dateStr} {timeStr}",
                                "yyyy-MM-dd HH:mm:ss",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime combinedTime))
                            {
                                return combinedTime;
                            }
                            return DateTime.MaxValue; // 无效数据排到最后  
                        })
                        .ToList();

                    // 转换数据到TemperatureData对象  
                    datas = new List<LogContent>();
                    foreach (var dict in sortedData)
                    {
                        try
                        {
                            var tempData = new LogContent
                            {
                                Object = "System",
                                Message = dict["message"].Data.ToString(),
                                // 合并日期时间  
                                Time = DateTime.ParseExact(
                                    $"{dict["Date"].Data} {dict["Time"].Data}",
                                    "yyyy-MM-dd HH:mm:ss",
                                    CultureInfo.InvariantCulture),
                                Level = ((EnumLogContentType)dict["Type"].Data).ToString(),

                            };
                            datas.Add(tempData);
                        }
                        catch (Exception ex)
                        {
                            // 处理数据转换异常  
                            //Debug.WriteLine($"数据转换失败: {ex.Message}");
                            LogRecorder.RecordLog(EnumLogContentType.Error, $"数据转换失败 {ex.Message}");
                        }
                    }

                    //绑定数据  
                    var data = datas;
                    if (!gridControl1.IsHandleCreated)
                    {
                        return;
                    }

                    gridControl1.BeginInvoke(new Action(() =>
                    {
                        gridControl1.BeginUpdate();
                        gridControl1.DataSource = data;
                        gridControl1.EndUpdate();
                    }));

                }
                else
                {
                    // 处理空数据情况  
                    //Debug.WriteLine("没有找到符合条件的数据");
                    datas = new List<LogContent>(); // 或保持原列表  
                }
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            ReadDataSystemLog();
        }

        private void FrmAlarmHistory_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            int year = now.Year;      // 当前年份（如 2025）
            int month = now.Month;    // 当前月份（1-12）
            int day = now.Day;        // 当前日（1-31）
            numericUpDown1.Value = year;
            numericUpDown2.Value = month;
            numericUpDown3.Value = day;

            ReadDataSystemLog();
        }
    }
}