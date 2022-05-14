using Dapper;
using System.Linq;
using System.Data;
using Server.Service;
using Server.Utilite;
using Server.Contracts;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Server.Repository
{
    public sealed class CharacterServices : ICharacterServices
    {
        private readonly IDbConnection Db;

        public CharacterServices()
        {
            Db = new MySqlConnection(MySQLFormattedExpression.ConnectionString);
        }

        public async Task<bool> CheckIfCharacterNameExistsAsync(string name)
        {
            try
            {
                return (await Db.QueryAsync<bool>(MySQLFormattedExpression.CheckIfCharacterNameExists, new { _name = name })).Any();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(CheckIfCharacterNameExistsAsync));
            }

            return false;
        }

        public async Task<CharacterContract> CreateCharacterAsync(CharacterContract characterContract)
        {
            try
            {
                await Db.ExecuteAsync(MySQLFormattedExpression.CreateCharacter, new
                {
                    _id = characterContract.Id,
                    _score = characterContract.Score,
                    _login_name = characterContract.LoginName,
                    _name = characterContract.Name
                });

                return characterContract;
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(CreateCharacterAsync));
                return null;
            }
        }

        public async void DeleteCharacterAsync(CharacterContract characterContract)
        {
            try
            {
                await Db.ExecuteAsync(MySQLFormattedExpression.DeleteCharacter, new { _id = characterContract.Id });
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(DeleteCharacterAsync));
            }
        }

        public async Task<CharacterContract> GetCharacterByLoginAsync(string login)
        {
            try
            {
                return (await Db.QueryAsync<CharacterContract>(MySQLFormattedExpression.GetCharacterByLogin, new
                {
                    _login = login
                })).FirstOrDefault();
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(GetCharacterByLoginAsync));
            }

            return null;
        }

        public async void UpdateCharacterAsync(CharacterContract characterContract)
        {
            try
            {
                await Db.ExecuteAsync(MySQLFormattedExpression.UpdateCharacter, new
                {
                    _id = characterContract.Id,
                    _score = characterContract.Score,
                    _login_name = characterContract.LoginName,
                    _name = characterContract.Name
                });
            }
            catch (MySqlException exception)
            {
                ExceptionHandler.ExecuteMySQLException(exception, nameof(UpdateCharacterAsync));
            }
        }
    }
}