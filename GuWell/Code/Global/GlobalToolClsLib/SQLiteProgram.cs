using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalToolClsLib
{
    public class SQLData
    {
        public object Data { get; set; }

        public SQLDataType type { get; set; }


        public SQLData(object Data, SQLDataType type)
        {
            this.Data = Data;
            this.type = type;
        }

    }

    public enum SQLDataType
    {
        INT,
        FLOAT,
        STRING,
    }

    public class SQLiteProgram
    {

        private static readonly object _lockObj = new object();
        private static volatile SQLiteProgram _instance = null;
        public static SQLiteProgram Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new SQLiteProgram();
                        }
                    }
                }
                return _instance;
            }
        }

        SQLiteConnection m_dbConnection;

        public bool Init(string filePath = null)
        {
            try
            {
                string DatabasePath = "D:/GuWell/Logs/SQLiteData/GWDieBonderDatabase.db";
                if (filePath != null)
                {
                    DatabasePath = filePath + "GWDieBonderDatabase.db";
                }

                CreateNewDatabase(DatabasePath);
                connectToDatabase(DatabasePath);



                string tablename3 = "ProductionLog";
                Dictionary<string, SQLData> tableDictionarys3 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData("", SQLDataType.STRING) },
                { "Time", new SQLData("", SQLDataType.STRING) },
                { "Type", new SQLData(0, SQLDataType.INT) },
                { "message", new SQLData("", SQLDataType.STRING) },
            };
                createTable(tablename3, tableDictionarys3);

                string tablename4 = "SystemLog";
                Dictionary<string, SQLData> tableDictionarys4 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData("", SQLDataType.STRING) },
                { "Time", new SQLData("", SQLDataType.STRING) },
                { "Type", new SQLData(0, SQLDataType.INT) },
                { "message", new SQLData("", SQLDataType.STRING) },
            };
                createTable(tablename4, tableDictionarys4);

                string tablename2 = "AlarmLog";
                Dictionary<string, SQLData> tableDictionarys2 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData("", SQLDataType.STRING) },
                { "Time", new SQLData("", SQLDataType.STRING) },
                { "Type", new SQLData(0, SQLDataType.INT) },
                { "message", new SQLData("", SQLDataType.STRING) },
            };
                createTable(tablename2, tableDictionarys2);

                string tablename1 = "UserOperationLog";
                Dictionary<string, SQLData> tableDictionarys1 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData("", SQLDataType.STRING) },
                { "Time", new SQLData("", SQLDataType.STRING) },
                { "User", new SQLData("", SQLDataType.STRING) },
                { "Type", new SQLData(0, SQLDataType.INT) },
                { "message", new SQLData("", SQLDataType.STRING) },
            };
                createTable(tablename1, tableDictionarys1);

                string tablename5 = "PressureData";
                Dictionary<string, SQLData> tableDictionarys5 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData("", SQLDataType.STRING) },
                { "Time", new SQLData("", SQLDataType.STRING) },
                { "Pressure1", new SQLData(0, SQLDataType.FLOAT) },
                { "Pressure2", new SQLData(0, SQLDataType.FLOAT) },
            };
                createTable(tablename5, tableDictionarys5);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个空的数据库
        /// </summary>
        /// <param name="DatabasePath"></param>
        /// <returns></returns>
        public bool CreateNewDatabase(string DatabasePath)
        {
            try
            {
                if (File.Exists(DatabasePath))
                {
                    Console.WriteLine("数据库已存在，未进行覆盖。");

                }
                else
                {
                    // 创建数据库  
                    SQLiteConnection.CreateFile(DatabasePath);

                }
                return true;

            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 创建一个连接到指定数据库
        /// </summary>
        /// <param name="DatabasePath"></param>
        public bool connectToDatabase(string DatabasePath)
        {
            try
            {
                m_dbConnection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
                m_dbConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }




        /// <summary>
        /// 在指定数据库中创建一个table
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="tableDictionarys"></param>
        /// <returns></returns>
        public bool createTable(string tablename, Dictionary<string, SQLData> tableDictionarys)
        {
            try
            {
                if (m_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"CREATE TABLE IF NOT EXISTS {tablename} (Id INTEGER PRIMARY KEY AUTOINCREMENT";
                    foreach (var kvp in tableDictionarys)
                    {
                        if (kvp.Value.type == SQLDataType.STRING)
                        {
                            sql += $", {kvp.Key} varchar(20)";
                        }
                        else if (kvp.Value.type == SQLDataType.INT)
                        {
                            sql += $", {kvp.Key} int";
                        }
                        else if (kvp.Value.type == SQLDataType.FLOAT)
                        {
                            sql += $", {kvp.Key} float";
                        }

                    }
                    sql += ")";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        /// <summary>
        /// 在指定的table中添加数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="tableDictionarys"></param>
        /// <returns></returns>
        public bool AddData(string tablename, Dictionary<string, SQLData> tableDictionarys, int datasize = 512000)
        {
            try
            {

                if (m_dbConnection?.State == System.Data.ConnectionState.Open)
                {
                    string insertQuery = $"INSERT INTO {tablename} ({tableDictionarys.Keys.First()}";
                    int i = 0;
                    foreach (var kvp in tableDictionarys)
                    {
                        if (i > 0)
                        {
                            insertQuery += $", {kvp.Key}";
                        }
                        i++;

                    }
                    insertQuery += $") VALUES (@{tableDictionarys.Keys.First()}";
                    i = 0;
                    foreach (var kvp1 in tableDictionarys)
                    {
                        if (i > 0)
                        {
                            insertQuery += $", @{kvp1.Key}";
                        }

                        i++;
                    }
                    insertQuery += $");";

                    using (var insertCommand = new SQLiteCommand(insertQuery, m_dbConnection))
                    {
                        // 检查记录数量  
                        string countQuery = $"SELECT COUNT(*) FROM {tablename};";
                        using (var countCommand = new SQLiteCommand(countQuery, m_dbConnection))
                        {
                            int count = Convert.ToInt32(countCommand.ExecuteScalar());
                            if (count >= datasize) // 假设限制为512000条记录 大概100MB 355天的数据  
                            {
                                // 删除最早的记录  
                                string deleteQuery = $"DELETE FROM {tablename} WHERE Id = (SELECT MIN(Id) FROM {tablename});";
                                using (var deleteCommand = new SQLiteCommand(deleteQuery, m_dbConnection))
                                {
                                    deleteCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        foreach (var kvp in tableDictionarys)
                        {
                            insertQuery += $", {kvp.Key}";
                            insertCommand.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value.Data);

                        }
                        insertCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取指定的table
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="tableDictionarys"></param>
        /// <returns></returns>
        public bool ReadData(string tablename, ref List<Dictionary<string, SQLData>> tableDictionarys)
        {
            try
            {
                tableDictionarys = new List<Dictionary<string, SQLData>>();

                if (m_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    Dictionary<string, SQLData> tableDictionary_type = new Dictionary<string, SQLData>();

                    int L = 0;

                    string sql = $"PRAGMA table_info({tablename})";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string columnName = reader.GetString(1); // 列名  
                                string dataType = reader.GetString(2); // 数据类型  
                                int isPrimaryKey = reader.GetInt32(5); // 是否为主键（0或1）  

                                if (isPrimaryKey == 0)
                                {
                                    if (dataType == "varchar(20)")
                                    {
                                        tableDictionary_type.Add(columnName, new SQLData("", SQLDataType.STRING));
                                    }
                                    if (dataType == " varchar(20)")
                                    {
                                        tableDictionary_type.Add(columnName, new SQLData("", SQLDataType.STRING));
                                    }
                                    else if (dataType == "int")
                                    {
                                        tableDictionary_type.Add(columnName, new SQLData(0, SQLDataType.INT));
                                    }
                                    else if (dataType == "INT")
                                    {
                                        tableDictionary_type.Add(columnName, new SQLData(0, SQLDataType.INT));
                                    }
                                    else if (dataType == "float")
                                    {
                                        tableDictionary_type.Add(columnName, new SQLData(0, SQLDataType.FLOAT));
                                    }
                                    else if (dataType == "FLOAT")
                                    {
                                        tableDictionary_type.Add(columnName, new SQLData(0, SQLDataType.FLOAT));
                                    }

                                }
                            }
                        }
                    }

                    sql = $"SELECT * FROM {tablename}";


                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        // 执行查询并读取数据  
                        using (var reader = command.ExecuteReader())
                        {
                            // 遍历结果集  
                            while (reader.Read())
                            {
                                Dictionary<string, SQLData> tableDictionary_data = new Dictionary<string, SQLData>();
                                int i = 1;
                                foreach (var kvp in tableDictionary_type)
                                {
                                    if (kvp.Value.type == SQLDataType.STRING)
                                    {
                                        tableDictionary_data.Add(kvp.Key, new SQLData(reader.GetString(i), kvp.Value.type));
                                    }
                                    else if (kvp.Value.type == SQLDataType.INT)
                                    {
                                        tableDictionary_data.Add(kvp.Key, new SQLData(reader.GetInt32(i), kvp.Value.type));
                                    }
                                    else if (kvp.Value.type == SQLDataType.FLOAT)
                                    {
                                        tableDictionary_data.Add(kvp.Key, new SQLData(reader.GetFloat(i), kvp.Value.type));
                                    }

                                    i++;

                                }

                                tableDictionarys.Add(tableDictionary_data);


                            }
                        }
                    }
                }
                else
                {
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        /// <summary>
        /// 保存生产日志
        /// </summary>
        /// <param name="type">类型 Error = 0,Warn = 1,Info = 2,Debug = 3</param>
        /// <param name="log">内容</param>
        public void SaveProductionLog(int type, string log)
        {
            string tablename = "ProductionLog";



            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm:ss:FFF");

            Dictionary<string, SQLData> tableDictionarys5 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData(currentDate, SQLDataType.STRING) },
                { "Time", new SQLData(currentTime, SQLDataType.STRING) },
                { "Type", new SQLData(type, SQLDataType.INT) },
                { "message", new SQLData(log, SQLDataType.STRING) },
            };

            SQLiteProgram.Instance.AddData(tablename, tableDictionarys5);
        }

        /// <summary>
        /// 保存系统日志
        /// </summary>
        /// <param name="type">类型 Error = 0,Warn = 1,Info = 2,Debug = 3</param>
        /// <param name="log">内容</param>
        public void SaveSystemLog(int type, string log)
        {
            string tablename = "SystemLog";



            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm:ss:FFF");

            Dictionary<string, SQLData> tableDictionarys5 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData(currentDate, SQLDataType.STRING) },
                { "Time", new SQLData(currentTime, SQLDataType.STRING) },
                { "Type", new SQLData(type, SQLDataType.INT) },
                { "message", new SQLData(log, SQLDataType.STRING) },
            };

            SQLiteProgram.Instance.AddData(tablename, tableDictionarys5);
        }

        /// <summary>
        /// 保存报警日志
        /// </summary>
        /// <param name="type">类型 Error = 0,Warn = 1,Info = 2,Debug = 3</param>
        /// <param name="log">内容</param>
        public void SaveAlarmLog(int type, string log)
        {
            string tablename = "AlarmLog";



            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm:ss:FFF");

            Dictionary<string, SQLData> tableDictionarys5 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData(currentDate, SQLDataType.STRING) },
                { "Time", new SQLData(currentTime, SQLDataType.STRING) },
                { "Type", new SQLData(type, SQLDataType.INT) },
                { "message", new SQLData(log, SQLDataType.STRING) },
            };

            SQLiteProgram.Instance.AddData(tablename, tableDictionarys5);
        }


        /// <summary>
        /// 保存用户操作日志
        /// </summary>
        /// <param name="type">类型 Error = 0,Warn = 1,Info = 2,Debug = 3</param>
        /// <param name="log">内容</param>
        public void SaveUserOperationLog(string user, int type, string log)
        {
            string tablename = "UserOperationLog";

            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm:ss:FFF");

            Dictionary<string, SQLData> tableDictionarys5 = new Dictionary<string, SQLData>
            {
                { "Date", new SQLData(currentDate, SQLDataType.STRING) },
                { "Time", new SQLData(currentTime, SQLDataType.STRING) },
                { "User", new SQLData(user, SQLDataType.STRING) },
                { "Type", new SQLData(type, SQLDataType.INT) },
                { "message", new SQLData(log, SQLDataType.STRING) },
            };

            SQLiteProgram.Instance.AddData(tablename, tableDictionarys5);
        }

        public List<Dictionary<string, SQLData>> ReadAllProductionLog()
        {
            string tablename2 = "ProductionLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            ReadData(tablename2, ref tableDictionarys);

            return tableDictionarys;

        }

        public List<Dictionary<string, SQLData>> ReadDataProductionLog(int Year, int Month, int Day, int Type = -1)
        {
            string tablename2 = "ProductionLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            SQLiteProgram.Instance.ReadData(tablename2, ref tableDictionarys);

            DateTime startDate = new DateTime(Year, Month, Day);
            string targetDate = startDate.ToString("yyyy-MM-dd");


            // 安全版本（包含类型验证）  
            List<Dictionary<string, SQLData>> safeFilteredData = tableDictionarys
                .Where(dict =>
                {
                    if (dict.TryGetValue("Date", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.STRING &&
                               dateData.Data is string dateStr &&
                               dateStr == targetDate;
                    }
                    return false;
                })
                .ToList();

            if (Type >= 0)
            {
                List<Dictionary<string, SQLData>> safeFilteredData2 = safeFilteredData
                .Where(dict =>
                {
                    if (dict.TryGetValue("Type", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.INT &&
                               dateData.Data is int dateStr &&
                               dateStr == Type;
                    }
                    return false;
                })
                .ToList();

                return safeFilteredData2;
            }

            return safeFilteredData;

        }

        public List<Dictionary<string, SQLData>> ReadAllSystemLog()
        {
            string tablename2 = "SystemLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            ReadData(tablename2, ref tableDictionarys);

            return tableDictionarys;

        }

        public List<Dictionary<string, SQLData>> ReadDataSystemLog(int Year, int Month, int Day, int Type = -1)
        {
            string tablename2 = "SystemLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            SQLiteProgram.Instance.ReadData(tablename2, ref tableDictionarys);

            DateTime startDate = new DateTime(Year, Month, Day);
            string targetDate = startDate.ToString("yyyy-MM-dd");


            // 安全版本（包含类型验证）  
            List<Dictionary<string, SQLData>> safeFilteredData = tableDictionarys
                .Where(dict =>
                {
                    if (dict.TryGetValue("Date", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.STRING &&
                               dateData.Data is string dateStr &&
                               dateStr == targetDate;
                    }
                    return false;
                })
                .ToList();

            if (Type >= 0)
            {
                List<Dictionary<string, SQLData>> safeFilteredData2 = safeFilteredData
                .Where(dict =>
                {
                    if (dict.TryGetValue("Type", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.INT &&
                               dateData.Data is int dateStr &&
                               dateStr == Type;
                    }
                    return false;
                })
                .ToList();

                return safeFilteredData2;
            }

            return safeFilteredData;

        }

        public List<Dictionary<string, SQLData>> ReadAllAlarmLog()
        {
            string tablename2 = "AlarmLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            ReadData(tablename2, ref tableDictionarys);

            return tableDictionarys;

        }

        public List<Dictionary<string, SQLData>> ReadDataAlarmLog(int Year, int Month, int Day, int Type = -1)
        {
            string tablename2 = "AlarmLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            SQLiteProgram.Instance.ReadData(tablename2, ref tableDictionarys);

            DateTime startDate = new DateTime(Year, Month, Day);
            string targetDate = startDate.ToString("yyyy-MM-dd");


            // 安全版本（包含类型验证）  
            List<Dictionary<string, SQLData>> safeFilteredData = tableDictionarys
                .Where(dict =>
                {
                    if (dict.TryGetValue("Date", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.STRING &&
                               dateData.Data is string dateStr &&
                               dateStr == targetDate;
                    }
                    return false;
                })
                .ToList();

            if (Type >= 0)
            {
                List<Dictionary<string, SQLData>> safeFilteredData2 = safeFilteredData
                .Where(dict =>
                {
                    if (dict.TryGetValue("Type", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.INT &&
                               dateData.Data is int dateStr &&
                               dateStr == Type;
                    }
                    return false;
                })
                .ToList();

                return safeFilteredData2;
            }

            return safeFilteredData;

        }

        public List<Dictionary<string, SQLData>> ReadAllUserOperationLog()
        {
            string tablename2 = "UserOperationLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            ReadData(tablename2, ref tableDictionarys);

            return tableDictionarys;

        }

        public List<Dictionary<string, SQLData>> ReadDataUserOperationLog(int Year, int Month, int Day, string user = null, int Type = -1)
        {
            string tablename2 = "UserOperationLog";

            List<Dictionary<string, SQLData>> tableDictionarys = new List<Dictionary<string, SQLData>>();
            SQLiteProgram.Instance.ReadData(tablename2, ref tableDictionarys);

            DateTime startDate = new DateTime(Year, Month, Day);
            string targetDate = startDate.ToString("yyyy-MM-dd");


            // 安全版本（包含类型验证）  
            List<Dictionary<string, SQLData>> safeFilteredData = tableDictionarys
                .Where(dict =>
                {
                    if (dict.TryGetValue("Date", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.STRING &&
                               dateData.Data is string dateStr &&
                               dateStr == targetDate;
                    }
                    return false;
                })
                .ToList();

            if (user != null)
            {
                List<Dictionary<string, SQLData>> safeFilteredData2 = safeFilteredData
                .Where(dict =>
                {
                    if (dict.TryGetValue("User", out SQLData dateData))
                    {
                        return dateData.type == SQLDataType.STRING &&
                               dateData.Data is string dateStr &&
                               dateStr == user;
                    }
                    return false;
                })
                .ToList();

                if (Type >= 0)
                {
                    List<Dictionary<string, SQLData>> safeFilteredData3 = safeFilteredData2
                    .Where(dict =>
                    {
                        if (dict.TryGetValue("Type", out SQLData dateData))
                        {
                            return dateData.type == SQLDataType.INT &&
                                   dateData.Data is int dateStr &&
                                   dateStr == Type;
                        }
                        return false;
                    })
                    .ToList();

                    return safeFilteredData3;
                }

                return safeFilteredData2;
            }

            ////排序加转换
            //if (safeFilteredData.Count > 0)
            //{
            //    // 按时间排序（需处理日期时间合并）  
            //    var sortedData = safeFilteredData
            //        .OrderBy(dict =>
            //        {
            //            // 合并日期和时间创建DateTime  
            //            var dateStr = dict["Date"].Data as string;
            //            var timeStr = dict["Time"].Data as string;

            //            if (DateTime.TryParseExact($"{dateStr} {timeStr}",
            //                "yyyy-MM-dd HH:mm:ss",
            //                CultureInfo.InvariantCulture,
            //                DateTimeStyles.None,
            //                out DateTime combinedTime))
            //            {
            //                return combinedTime;
            //            }
            //            return DateTime.MaxValue; // 无效数据排到最后  
            //        })
            //        .ToList();

            //    // 转换数据到TemperatureData对象  
            //    datas = new List<TemperatureData>();
            //    foreach (var dict in sortedData)
            //    {
            //        try
            //        {
            //            var tempData = new TemperatureData
            //            {
            //                // 合并日期时间  
            //                Time = DateTime.ParseExact(
            //                    $"{dict["Date"].Data} {dict["Time"].Data}",
            //                    "yyyy-MM-dd HH:mm:ss",
            //                    CultureInfo.InvariantCulture),

            //                TempA = Convert.ToDouble(dict["UpTemperature"].Data),
            //                TempB = Convert.ToDouble(dict["MiddleTemperature"].Data),
            //                TempC = Convert.ToDouble(dict["DownTemperature"].Data)
            //            };
            //            datas.Add(tempData);
            //        }
            //        catch (Exception ex)
            //        {
            //            // 处理数据转换异常  
            //            Debug.WriteLine($"数据转换失败: {ex.Message}");
            //        }
            //    }

            //    //绑定数据  
            //    var data = datas;
            //    BindChartData(datas);

            //}
            //else
            //{
            //    // 处理空数据情况  
            //    Debug.WriteLine("没有找到符合条件的数据");
            //    datas = new List<TemperatureData>(); // 或保持原列表  
            //}

            return safeFilteredData;

        }



        //插入一些数据
        public void fillTable()
        {


            string tablename = "VacuumsData";



            Random random = new Random();

            // 设置上下限  
            double lowerLimit = 0.0001;
            double upperLimit = 10000;
            float vacuum1 = 100000;
            float vacuum2 = 100000;
            float vacuum3 = 100000;


            int i = 0;
            while (i < 0)
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                double randomValue = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                vacuum1 = (float)randomValue;
                randomValue = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                vacuum2 = (float)randomValue;
                randomValue = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                vacuum3 = (float)randomValue;

                Dictionary<string, SQLData> tableDictionarys_0 = new Dictionary<string, SQLData>
                {
                    { "Date", new SQLData(currentDate, SQLDataType.STRING) },
                    { "Time", new SQLData(currentTime, SQLDataType.STRING) },
                    { "Vacuum1", new SQLData(vacuum1, SQLDataType.FLOAT) },
                    { "Vacuum2", new SQLData(vacuum2, SQLDataType.FLOAT) },
                    { "Vacuum3", new SQLData(vacuum3, SQLDataType.FLOAT) },
                };

                AddData(tablename, tableDictionarys_0);

                i++;

                Thread.Sleep(5);
            }

            List<Dictionary<string, SQLData>> N = new List<Dictionary<string, SQLData>>();

            ReadData(tablename, ref N);
        }




        //使用sql查询语句，并显示结果
        void printHighscores()
        {
            string sql = "select * from Vacuums order by Vacuum1 desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Date: " + reader["Date"] + "\tTime: " + reader["Time"] + "\tV1: " + reader["Vacuum1"] + "\tV2: " + reader["Vacuum2"] + "\tV3: " + reader["Vacuum3"]);
            Console.ReadLine();
        }


    }
}
