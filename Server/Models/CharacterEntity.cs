using System;
using Server.Enums;
using Server.Template;
using System.Threading.Tasks;

namespace Server.Models
{
    public sealed class CharacterEntity : Entity
    {
        public CharacterTemplate CharacterTemplate;
        public CharacterStats CharacterStats;
        public bool Online { get; set; }
        public int RoomId { get; set; }
        public bool IsDead { get; set; }

        public CharacterEntity(CharacterTemplate characterTemplate)
            : base(characterTemplate)
        {
            CharacterTemplate = characterTemplate;
            CharacterStats = new CharacterStats(this, characterTemplate.BaseSpecification);
        }

        public void SetOffline()
        {
            Online = false;
        }

        public override async Task<DamageResult> TakeDamage(CharacterEntity target)
        {
            throw new NotImplementedException();
        }
    }
}