using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
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
using UserManagerClsLib;
using WestDragon.Framework.BaseLoggerClsLib;

namespace SystemGUILib.LogUI
{
    public partial class FrmLog : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 数据库日志
        /// </summary>
        private static SQLiteProgram _SQLiteProgram
        {
            get { return SQLiteProgram.Instance; }
        }

        public FrmLog()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadDataSystemLog();
        }

        public void ReadDataSystemLog()
        {
            List<LogContent> datas;
            if (_SQLiteProgram != null)
            {
                List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
                tableDictionarys = _SQLiteProgram.ReadDataSystemLog((int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, (comboBox1.SelectedIndex - 1));

                gridControl1.BeginUpdate();
                gridControl1.DataSource = null;
                gridControl1.EndUpdate();

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

                            return ParseDateTimeWithMilliseconds($"{dateStr} {timeStr}");

                            //if (DateTime.TryParseExact($"{dateStr} {timeStr}",
                            //    "yyyy-MM-dd HH:mm:ss",
                            //    CultureInfo.InvariantCulture,
                            //    DateTimeStyles.None,
                            //    out DateTime combinedTime))
                            //{
                            //    return combinedTime;
                            //}
                            //return DateTime.MaxValue; // 无效数据排到最后  
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
                                Time = ParseDateTimeWithMilliseconds(
                                    $"{dict["Date"].Data} {dict["Time"].Data}"),
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

                    DataTable data1 = new DataTable();
                    data1.Columns.Add("Data", typeof(string));
                    data1.Columns.Add("Time", typeof(string));
                    data1.Columns.Add("Level", typeof(string));
                    data1.Columns.Add("Message", typeof(string));

                    // 添加行
                    foreach (LogContent log in datas)
                    {
                        DataRow row = data1.NewRow();
                        row["Data"] = log.Time.ToString("yy-MM-dd");
                        row["Time"] = log.Time.ToString("HH:mm:ss:fff");
                        row["Level"] = log.Level;
                        row["Message"] = log.Message;
                        data1.Rows.Add(row);
                    }


                    if (!gridControl1.IsHandleCreated)
                    {
                        return;
                    }

                    //gridControl1.BeginInvoke(new Action(() =>
                    //{
                    //    gridControl1.BeginUpdate();
                    //    gridControl1.DataSource = data;
                    //    gridControl1.EndUpdate();
                    //}));

                    gridControl1.BeginUpdate();
                    gridControl1.DataSource = data1;
                    gridColumn1.FieldName = "Data";
                    gridColumn2.FieldName = "Time";
                    gridColumn3.FieldName = "Level";
                    gridColumn4.FieldName = "Message";
                    gridControl1.EndUpdate();

                }
                else
                {
                    // 处理空数据情况  
                    //Debug.WriteLine("没有找到符合条件的数据");
                    datas = new List<LogContent>(); // 或保持原列表  
                }
            }
        }

        public void ReadDataProductionLog()
        {
            List<LogContent> datas;
            if (_SQLiteProgram != null)
            {
                List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
                tableDictionarys = _SQLiteProgram.ReadDataProductionLog((int)numericUpDown6.Value, (int)numericUpDown5.Value, (int)numericUpDown4.Value, (comboBox2.SelectedIndex - 1));

                gridControl2.BeginUpdate();
                gridControl2.DataSource = null;
                gridControl2.EndUpdate();

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
                                Object = "Production",
                                Message = dict["message"].Data.ToString(),
                                // 合并日期时间  
                                Time = ParseDateTimeWithMilliseconds(
                                    $"{dict["Date"].Data} {dict["Time"].Data}"),
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

                    DataTable data1 = new DataTable();
                    data1.Columns.Add("Data", typeof(string));
                    data1.Columns.Add("Time", typeof(string));
                    data1.Columns.Add("Level", typeof(string));
                    data1.Columns.Add("Message", typeof(string));

                    // 添加行
                    foreach (LogContent log in datas)
                    {
                        DataRow row = data1.NewRow();
                        row["Data"] = log.Time.ToString("yy-MM-dd");
                        row["Time"] = log.Time.ToString("HH:mm:ss:fff");
                        row["Level"] = log.Level;
                        row["Message"] = log.Message;
                        data1.Rows.Add(row);
                    }


                    if (!gridControl1.IsHandleCreated)
                    {
                        return;
                    }

                    //gridControl1.BeginInvoke(new Action(() =>
                    //{
                    //    gridControl1.BeginUpdate();
                    //    gridControl1.DataSource = data;
                    //    gridControl1.EndUpdate();
                    //}));

                    gridControl2.BeginUpdate();
                    gridControl2.DataSource = data1;
                    gridColumn5.FieldName = "Data";
                    gridColumn6.FieldName = "Time";
                    gridColumn7.FieldName = "Level";
                    gridColumn8.FieldName = "Message";
                    gridControl2.EndUpdate();

                }
                else
                {
                    // 处理空数据情况  
                    //Debug.WriteLine("没有找到符合条件的数据");
                    datas = new List<LogContent>(); // 或保持原列表  
                }
            }
        }

        public void ReadDataUserOperationLog()
        {
            List<LogContent2> datas;
            if (_SQLiteProgram != null)
            {
                List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
                tableDictionarys = _SQLiteProgram.ReadDataUserOperationLog((int)numericUpDown9.Value, (int)numericUpDown8.Value, (int)numericUpDown7.Value, comboBox3.Text, (comboBox4.SelectedIndex - 1));

                gridControl3.BeginUpdate();
                gridControl3.DataSource = null;
                gridControl3.EndUpdate();

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
                    datas = new List<LogContent2>();
                    foreach (var dict in sortedData)
                    {
                        try
                        {
                            var tempData = new LogContent2
                            {
                                Object = "UserOperation",
                                Message = dict["message"].Data.ToString(),
                                // 合并日期时间  
                                Time = ParseDateTimeWithMilliseconds(
                                    $"{dict["Date"].Data} {dict["Time"].Data}"),
                                User = (dict["User"].Data).ToString(),
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

                    DataTable data1 = new DataTable();
                    data1.Columns.Add("Data", typeof(string));
                    data1.Columns.Add("Time", typeof(string));
                    data1.Columns.Add("User", typeof(string));
                    data1.Columns.Add("Level", typeof(string));
                    data1.Columns.Add("Message", typeof(string));

                    // 添加行
                    foreach (LogContent2 log in datas)
                    {
                        DataRow row = data1.NewRow();
                        row["Data"] = log.Time.ToString("yy-MM-dd");
                        row["Time"] = log.Time.ToString("HH:mm:ss:fff");
                        row["User"] = log.User;
                        row["Level"] = log.Level;
                        row["Message"] = log.Message;
                        data1.Rows.Add(row);
                    }


                    if (!gridControl1.IsHandleCreated)
                    {
                        return;
                    }

                    //gridControl1.BeginInvoke(new Action(() =>
                    //{
                    //    gridControl1.BeginUpdate();
                    //    gridControl1.DataSource = data;
                    //    gridControl1.EndUpdate();
                    //}));

                    gridControl3.BeginUpdate();
                    gridControl3.DataSource = data1;
                    gridColumn9.FieldName = "Data";
                    gridColumn10.FieldName = "Time";
                    gridColumn13.FieldName = "User";
                    gridColumn11.FieldName = "Level";
                    gridColumn12.FieldName = "Message";
                    gridControl3.EndUpdate();

                }
                else
                {
                    // 处理空数据情况  
                    //Debug.WriteLine("没有找到符合条件的数据");
                    datas = new List<LogContent2>(); // 或保持原列表  
                }
            }
        }

        public DateTime ParseDateTimeWithMilliseconds(string input)
        {
            // 检查是否包含毫秒部分（是否有第三个冒号）
            int colonCount = CountOccurrences(input, ':');

            if (colonCount == 3) // 有毫秒部分
            {
                int lastColonIndex = input.LastIndexOf(':');
                string datePart = input.Substring(0, lastColonIndex);
                string millisecondPart = input.Substring(lastColonIndex + 1);

                // 确保毫秒部分有3位（不足补零，超过截断）
                millisecondPart = millisecondPart.PadRight(3, '0').Substring(0, 3);
                string formatted = $"{datePart}.{millisecondPart}";

                return DateTime.ParseExact(
                    formatted,
                    "yyyy-MM-dd HH:mm:ss.fff",
                    CultureInfo.InvariantCulture
                );
            }
            else if (colonCount == 2) // 没有毫秒部分
            {
                // 直接解析，毫秒部分设为0
                return DateTime.ParseExact(
                    input,
                    "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture
                );
            }
            else
            {
                throw new FormatException("Invalid date-time format");
            }
        }

        private int CountOccurrences(string input, char character)
        {
            int count = 0;
            foreach (char c in input)
            {
                if (c == character) count++;
            }
            return count;
        }

        private void FrmLog_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            int year = now.Year;      // 当前年份（如 2025）
            int month = now.Month;    // 当前月份（1-12）
            int day = now.Day;        // 当前日（1-31）
            numericUpDown1.Value = year;
            numericUpDown2.Value = month;
            numericUpDown3.Value = day;
            numericUpDown6.Value = year;
            numericUpDown5.Value = month;
            numericUpDown4.Value = day;
            numericUpDown9.Value = year;
            numericUpDown8.Value = month;
            numericUpDown7.Value = day;

            List<UserInfos> users = UserManager.Instance.GetAllUsers();
            comboBox3.Items.Clear();
            foreach (var user in users)
            {
                comboBox3.Items.Add(user.username);
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            ReadDataSystemLog();

            ReadDataProductionLog();

            ReadDataUserOperationLog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadDataProductionLog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReadDataUserOperationLog();
        }
    }

    /// <summary>
    /// 日志类--用于日志的显示
    /// </summary>
    public class LogContent2
    {
        public string Object { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public string Level { get; set; }

        public string User { get; set; }
    }
}