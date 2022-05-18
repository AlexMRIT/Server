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
            Db = new MySqlConnection(MySQLFormattedExpression.ConnectionString);
        }

        public async Task<bool> CheckIfAccountIsCorrectAsync(string login, string password)
        {
            try
            {
                return (await Db.QueryAsync<bool>(MySQLFormattedExpression.CheckIfAccountIsCorrect, new
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
                await Db.ExecuteAsync(MySQLFormattedExpression.CreateAccount, new
                {
                    _id = id,
                    _login = login,
                    _password = password
                });

                AccountContract accountContract = new AccountContract()
                {
                    Id = id,
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
                return (await Db.QueryAsync<AccountContract>(MySQLFormattedExpression.GetAccountByLogin, new
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