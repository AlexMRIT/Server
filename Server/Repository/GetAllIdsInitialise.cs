using Dapper;
using System.Linq;
using System.Data;
using Server.Service;
using Server.Utilite;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Server.Repository
{
    public sealed class GetAllIdsInitialise : IGetAllIdsInitialise
    {
        private readonly IDbConnection Db;

        public GetAllIdsInitialise()
        {
            Db = new MySqlConnection("Server=127.0.0.1;Database=server;Uid=root;Pwd=server;SslMode=none;");
        }

        public async Task<List<int>> GetAccountIdsListAsync()
        {
            try
            {
                return (await Db.QueryAsync<int>("select id from accounts")).ToList();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(GetAccountIdsListAsync));
            }

            return new List<int>(capacity: 0);
        }

        public async Task<List<int>> GetPlayerIdsListAsync()
        {
            try
            {
                return (await Db.QueryAsync<int>("select id from characters")).ToList();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(GetPlayerIdsListAsync));
            }

            return new List<int>(capacity: 0);
        }
    }
}