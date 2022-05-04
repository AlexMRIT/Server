using System;
using Server.Template;

namespace Server.Models
{
    public sealed class CharacterEntity : Entity
    {
        public CharacterTemplate CharacterTemplate;

        public CharacterEntity(CharacterTemplate characterTemplate)
            : base(characterTemplate)
        {
            CharacterTemplate = characterTemplate;
        }
    }
}