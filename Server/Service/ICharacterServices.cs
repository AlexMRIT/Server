using Server.Contracts;
using System.Threading.Tasks;

namespace Server.Service
{
    public interface ICharacterServices
    {
        Task<CharacterContract> GetCharacterByLoginAsync(string login);

        Task<bool> CheckIfCharacterNameExistsAsync(string name);

        void CreateCharacterAsync(CharacterContract characterContract);

        void UpdateCharacterAsync(CharacterContract characterContract);

        void DeleteCharacterAsync(CharacterContract characterContract);
    }
}