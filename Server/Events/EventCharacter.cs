using System;
using Server.Models;

namespace Server.Events
{
    public sealed class EventCharacter
    {
        private readonly CharacterEntity Player;

        public EventCharacter(CharacterEntity character)
        {
            Player = character;
        }

        public bool ValidDeathTarget(CharacterEntity target, Action action)
        {
            bool result = target.CharacterStats.CurrentHealth <= 0;

            if (result)
                action();

            return result;
        }
    }
}