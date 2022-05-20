using Server.Models;
using Server.Contracts;

namespace Server.Utilite
{
    public static class ContractExtension
    {
        public static CharacterContract ToContract(this CharacterEntity character)
        {
            return new CharacterContract()
            {
                Id = character.Template.Id,
                Score = character.Score,
                LoginName = character.Login,
                Name = character.Name
            };
        }
    }
}