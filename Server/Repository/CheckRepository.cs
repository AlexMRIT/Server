using System;
using Dapper;
using System.Linq;
using System.Data;
using Server.Utilite;
using Server.Service;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Server.Repository
{
    public sealed class CheckRepository : ICheckService
    {
        internal IDbConnection Db;

        private const int PingTimeoutMs = 3000;
        private const int PingRetryAttempts = 5;
        private const int MysqlServiceStartTimeoutMs = 3000;
        private const int MysqlServiceRetryAttempts = 5;
        private const string MysqlServiceName = "MySQL";

        private readonly string _host;
        private readonly string _database;

        public CheckRepository()
        {
            Db = new MySqlConnection("Server=127.0.0.1;Database=server;Uid=server;Pwd=gameserver;SslMode=none;");

            MySqlConnectionStringBuilder connStrBuilder = new MySqlConnectionStringBuilder(Db.ConnectionString);
            _host = connStrBuilder.Server;
            _database = connStrBuilder.Database;
        }

        public async Task<bool> PreCheckRepositoryAsync()
        {
            if (!CheckDatabaseHostPing())
                return false;

            if (!CheckMySqlService())
                return false;

            if (!await CheckDatabaseQuery())
                return false;

            return true;
        }

        private bool CheckDatabaseHostPing()
        {
            Console.WriteLine("Checking ping to database host...");

            bool isHostPinging = HostCheck.IsPingSuccessful(_host, PingTimeoutMs);

            for (int i = 1; !isHostPinging && (i <= PingRetryAttempts); i++)
            {
                Console.WriteLine($"Ping to database host '{_host}' has FAILED!");
                Console.WriteLine($"Retrying to ping...Retry attempt: {i}.");

                isHostPinging = HostCheck.IsPingSuccessful(_host, PingTimeoutMs);

                if (isHostPinging)
                    break;
            }

            if (isHostPinging)
                Console.WriteLine($"Ping to database host '{_host}' was SUCCESSFUL!");
            else
                Console.WriteLine($"Ping to database host '{_host}' has FAILED!");

            return isHostPinging;
        }

        private bool CheckMySqlService()
        {
            if (HostCheck.IsLocalIpAddress(_host))
            {
                Console.WriteLine("Database host running at localhost.");
                Console.WriteLine("Checking if MySQL Service is running at localhost...");

                if (!HostCheck.ServiceExists(MysqlServiceName))
                {
                    Console.WriteLine("MySQL Service does not exist at localhost Windows Services!");
                    return false;
                }

                bool isMySqlServiceRunning = HostCheck.IsServiceRunning(MysqlServiceName);

                for (int i = 1; !isMySqlServiceRunning && (i <= MysqlServiceRetryAttempts); i++)
                {
                    Console.WriteLine("MySQL Service was not found running at localhost!");
                    Console.WriteLine($"Trying to start MySQL service...Retry attempt: {i}.");

                    HostCheck.StartService(MysqlServiceName, MysqlServiceStartTimeoutMs);

                    isMySqlServiceRunning = HostCheck.IsServiceRunning(MysqlServiceName);

                    if (!isMySqlServiceRunning)
                        continue;

                    Console.WriteLine("MySQL Service started!");
                    break;
                }

                if (isMySqlServiceRunning)
                    Console.WriteLine("MySQL Service running at localhost.");
                else
                    Console.WriteLine("MySQL Service was not found running at localhost!");

                return isMySqlServiceRunning;
            }

            Console.WriteLine("Database host NOT running at localhost. MySQL Service check skipped.");
            return true;
        }

        private async Task<bool> CheckDatabaseQuery()
        {
            Console.WriteLine("Checking if query to database works...");

            bool isQuerySuccessful = await TryQueryDatabase();

            if (isQuerySuccessful)
                Console.WriteLine($"Query to database '{_database}' was SUCCESSFUL!");
            else
                Console.WriteLine($"Query to database '{_database}' has FAILED!");

            return isQuerySuccessful;
        }

        private async Task<bool> TryQueryDatabase()
        {
            try
            {
                return (await Db.QueryAsync("SELECT 1")).Any();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(TryQueryDatabase));
            }

            return false;
        }
    }
}