using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WestDragon.Framework.DataBaseManagerClsLib;

namespace AlarmManagementClsLib.DB
{
    public class AlarmRepository
    {
        private static readonly string connectionString = string.Format("server={0};database={1};uid={3};pwd={2}", DBManager.DBManagerHandler.DefaultParamsForLocalDb.Server, DBManager.DBManagerHandler.DefaultParamsForLocalDb.Database, DBManager.DBManagerHandler.DefaultParamsForLocalDb.Password, DBManager.DBManagerHandler.DefaultParamsForLocalDb.UserID);
        public int InsertAlarm(AlarmEntity alm)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"INSERT INTO [dbo].[AlarmManagement]
                                   ([DefineId]
                                   ,[Name]
                                   ,[Type]
                                   ,[Description]
                                   ,[State]
                                   ,[Source]
                                   ,[OccurrenceTime]
                                   ,[AcknowledgeTime]
                                   ,[RecoveryTime]
                                   ,[ClearTime]
                                   ,[ResetTime]
                                   ,[IsRecoverable]
                                   ,[RecoveryOption]
                                   ,[RecoveryFailText]
                                   ,[Cause]
                                   ,[Remark])
                             VALUES
                                   (@DefineId
                                   ,@Name
                                   ,@Type
                                   ,@Description
                                   ,@State
                                   ,@Source
                                   ,@OccurrenceTime
                                   ,@AcknowledgeTime
                                   ,@RecoveryTime
                                   ,@ClearTime
                                   ,@ResetTime
                                   ,@IsRecoverable
                                   ,@RecoveryOption
                                   ,@RecoveryFailText
                                   ,@Cause
                                   ,@Remark)
                             SELECT SCOPE_IDENTITY()";
                return connection.Query<int>(sql, alm).FirstOrDefault();
            }
        }
        public List<AlarmEntity> GetAlarmsViewList()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"SELECT [Id]
                                  ,[DefineId]
                                  ,[Name]
                                  ,[Type]
                                  ,[Description]
                                  ,[State]
                                  ,[OccurrenceTime]
                                  ,[AcknowledgeTime]
                                  ,[RecoveryTime]
                                  ,[ClearTime]
                                  ,[ResetTime]
                                  ,[IsRecoverable]
                                  ,[RecoveryOption]
                                  ,[RecoveryFailText]
                                  ,[Cause]
                                  ,[Remark]
                              FROM [dbo].[AlarmManagement] where State<>4";
                return connection.Query<AlarmEntity>(sql).ToList();
            }
        }
        public List<AlarmEntity> GetAlarmsHistoryList()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"SELECT [Id]
                                  ,[DefineId]
                                  ,[Name]
                                  ,[Type]
                                  ,[Description]
                                  ,[Source]
                                  ,[State]
                                  ,[OccurrenceTime]
                                  ,[AcknowledgeTime]
                                  ,[RecoveryTime]
                                  ,[ClearTime]
                                  ,[ResetTime]
                                  ,[IsRecoverable]
                                  ,[RecoveryOption]
                                  ,[RecoveryFailText]
                                  ,[Cause]
                                  ,[Remark]
                              FROM [dbo].[AlarmManagement] where State = 4";
                return connection.Query<AlarmEntity>(sql).ToList();
            }
        }
        public bool AcknowledgeAlarmById(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"UPDATE [dbo].[AlarmManagement]
                               SET [State] = 2
                                  ,[AcknowledgeTime]=getdate()
                             WHERE Id=@Id";
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }
        public bool RecoverAlarmById(int id, int recoveryOptions, string recoveryFailText)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"UPDATE [dbo].[AlarmManagement]
                               SET [State] = 3
                                  ,[RecoveryOptions]=@RecoveryOptions
                                  ,[RecoveryFailText]=@RecoveryFailText
                                  ,[RecoveryTime]=getdate()
                             WHERE Id=@Id";
                return connection.Execute(sql, new { Id = id, RecoveryOptions = recoveryOptions, RecoveryFailText = recoveryFailText }) > 0;
            }
        }
        public bool ClearAlarmById(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"UPDATE [dbo].[AlarmManagement]
                               SET [State] = 4
                                  ,[ClearTime]=getdate()
                             WHERE Id=@Id";
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }
        public bool BatchClearAlarms(List<int> ids)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var result = false;
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var sql = string.Format(@"UPDATE [dbo].[AlarmManagement]
                               SET [State] = 4
                                  ,[ClearTime]=getdate()
                             WHERE Id IN ({0})", string.Join(",", ids));
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Transaction = trans;//将事务赋值给command就可以了
                        var affectNum = cmd.ExecuteNonQuery();
                        //var affectNum = connection.Execute(sql, trans);
                        if (affectNum == ids.Count)
                        {
                            result = true;
                            trans.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Dispose();
                    connection.Close();
                }
                return result;
            }
        }
        public bool UpdateAlarmRecoverStateById(int id, int recoveryOption, string recoveryFailText)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"UPDATE  [dbo].[AlarmManagement]
                            SET     [State] = 3 ,
                                    [RecoveryTime] = GETDATE() ,
                                    RecoveryOption = @RecoveryOption ,
                                    RecoveryFailText = @RecoveryFailText
                            WHERE   Id = @Id";
                return connection.Execute(sql, new { Id = id, RecoveryOption = recoveryOption, RecoveryFailText = recoveryFailText }) > 0;
            }
        }
        public bool UpdateAlarmRecoverStateByInstanceId(string instanceId, int recoveryOption, string recoveryFailText)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"UPDATE  [dbo].[AlarmManagement]
                            SET     [State] = 3 ,
                                    [RecoveryTime] = GETDATE() ,
                                    RecoveryOption = @RecoveryOption ,
                                    RecoveryFailText = @RecoveryFailText
                            WHERE   InstanceId = @InstanceId;";
                return connection.Execute(sql, new { InstanceId = instanceId, RecoveryOption = recoveryOption, RecoveryFailText = recoveryFailText }) > 0;
            }
        }
    }
}
