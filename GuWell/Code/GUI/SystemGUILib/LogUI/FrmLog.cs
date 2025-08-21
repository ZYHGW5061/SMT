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

        public void ReadDataProductionLog()
        {
            List<LogContent> datas;
            if (_SQLiteProgram != null)
            {
                List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
                tableDictionarys = _SQLiteProgram.ReadDataProductionLog((int)numericUpDown6.Value, (int)numericUpDown5.Value, (int)numericUpDown4.Value, (comboBox2.SelectedIndex - 1));


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
                    if (!gridControl2.IsHandleCreated)
                    {
                        return;
                    }

                    gridControl2.BeginInvoke(new Action(() =>
                    {
                        gridControl2.BeginUpdate();
                        gridControl2.DataSource = data;
                        gridControl2.EndUpdate();
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

        public void ReadDataUserOperationLog()
        {
            List<LogContent2> datas;
            if (_SQLiteProgram != null)
            {
                List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
                tableDictionarys = _SQLiteProgram.ReadDataUserOperationLog((int)numericUpDown9.Value, (int)numericUpDown8.Value, (int)numericUpDown7.Value, comboBox3.Text, (comboBox4.SelectedIndex - 1));


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
                                Time = DateTime.ParseExact(
                                    $"{dict["Date"].Data} {dict["Time"].Data}",
                                    "yyyy-MM-dd HH:mm:ss",
                                    CultureInfo.InvariantCulture),
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
                    if (!gridControl3.IsHandleCreated)
                    {
                        return;
                    }

                    gridControl3.BeginInvoke(new Action(() =>
                    {
                        gridControl3.BeginUpdate();
                        gridControl3.DataSource = data;
                        gridControl3.EndUpdate();
                    }));

                }
                else
                {
                    // 处理空数据情况  
                    //Debug.WriteLine("没有找到符合条件的数据");
                    datas = new List<LogContent2>(); // 或保持原列表  
                }
            }
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

            List<UserInfos> users =  UserManager.Instance.GetAllUsers();
            comboBox3.Items.Clear();
            foreach(var user in users)
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