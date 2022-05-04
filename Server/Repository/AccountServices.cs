using Dapper;
using System.Linq;
using System.Data;
using Server.Utilite;
using Server.Contracts;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Server.Service
{
    public sealed class AccountServices : IAccountServices
    {
        private readonly IDbConnection Db;

        public AccountServices()
        {
            Db = new MySqlConnection("Server=127.0.0.1;Database=server;Uid=root;Pwd=server;SslMode=none;");
        }

        public async Task<bool> CheckIfAccountIsCorrectAsync(string login, string password)
        {
            try
            {
                return (await Db.QueryAsync<bool>("select distinct 1 from accounts where login=@_login and password=@_password", new
                {
                    _login = login,
                    _password = password
                })).Any();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(CheckIfAccountIsCorrectAsync));
            }

            return false;
        }

        public async Task<AccountContract> CreateAccountAsync(string login, string password)
        {
            try
            {
                int id = IdFactory.Instance.NextId();
                await Db.ExecuteAsync("insert into accounts (id, login, password) values (@_id, @_login, @_password)", new
                {
                    _id = id,
                    _login = login,
                    _password = password
                });

                AccountContract accountContract = new AccountContract(id)
                {
                    Login = login,
                    Password = password
                };

                return accountContract;
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(CheckIfAccountIsCorrectAsync));
            }

            return null;
        }

        public async Task<AccountContract> GetAccountByLoginAsync(string login)
        {
            try
            {
                return (await Db.QueryAsync<AccountContract>("select id as Id, login, password as Password from accounts where login=@_login", new
                {
                    _login = login
                })).FirstOrDefault();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(CheckIfAccountIsCorrectAsync));
            }

            return null;
        }
    }
}