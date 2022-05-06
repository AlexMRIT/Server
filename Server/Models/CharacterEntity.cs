using System;
using Server.Template;

namespace Server.Models
{
    public sealed class CharacterEntity : Entity
    {
        public CharacterTemplate CharacterTemplate;
        public bool Online = default;
        public int RoomId { get; set; }

        public CharacterEntity(CharacterTemplate characterTemplate)
            : base(characterTemplate)
        {
            CharacterTemplate = characterTemplate;
        }

        public void SetOffline()
        {
            Online = false;
        }
    }
}