using Server.Service;
using Server.Utilite;
using Server.Contracts;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Server.Repository
{
    public sealed class CharacterServices : ICharacterServices
    {
        public async Task<bool> CheckIfCharacterNameExistsAsync(string name)
        {
            try
            {

            }
            catch (MySqlException exception)
            {
                
            }

            return false;
        }

        public async void CreateCharacterAsync(CharacterContract characterContract)
        {
            try
            {

            }
            catch (MySqlException exception)
            {

            }
        }

        public async void DeleteCharacterAsync(CharacterContract characterContract)
        {
            try
            {

            }
            catch (MySqlException exception)
            {

            }
        }

        public async Task<CharacterContract> GetCharacterByLoginAsync(string login)
        {
            try
            {

            }
            catch (MySqlException exception)
            {

            }

            return null;
        }

        public async void UpdateCharacterAsync(CharacterContract characterContract)
        {
            try
            {

            }
            catch (MySqlException exception)
            {

            }
        }
    }
}