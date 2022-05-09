using NLog;
using NLog.Config;
using NLog.Targets;

namespace LittleBlog.Web.NLogConfig
{
    /// <summary>
    /// 代码配置NLog的配置项
    /// </summary>
    public static class NLogConfigExtension
    {
        public const string RunngingTargetName = "Running";

        public static void AddNLogByDatabase(string connectionString)
        {
            var config = LogManager.Configuration;

            var databaseTarget = config.FindTargetByName<DatabaseTarget>(RunngingTargetName);

            if (databaseTarget == null)
            {
                databaseTarget = new DatabaseTarget(RunngingTargetName);

            }

            databaseTarget.DBProvider = "Npgsql.NpgsqlConnection, Npgsql";
            databaseTarget.CommandType = System.Data.CommandType.Text;
            databaseTarget.ConnectionString = connectionString;
            databaseTarget.CommandText = "INSERT INTO \"Logs\" (\"LogLevel\", \"Message\", \"Logger\", \"Application\", \"Callsite\", \"Exception\", \"Logged\")VALUES(@LogLevel, @Message, @Logger, @Application, @Callsite, @Exception, CAST(@Logged AS TIMESTAMP))";

            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@Logged", "${date}"));
            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@LogLevel", "${level}"));
            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@Message", "${message}"));
            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@Logger", "${logger}"));
            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@Exception", "${exception:tostring}"));
            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@Callsite", "${callsite}"));
            databaseTarget.Parameters.Add(new DatabaseParameterInfo("@Application", "AspNetCoreNLog"));

            // 所有以LittleBlog开头的LoggerName，所以排除了Microsoft开头的Logger
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, databaseTarget, "LittleBlog.*");

            // Apply
            NLog.LogManager.Configuration = config;
        }

    }
}
