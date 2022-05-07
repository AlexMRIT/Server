using System;
using Server.Enums;
using Server.Template;
using System.Threading.Tasks;

namespace Server.Models
{
    public abstract class Entity
    {
        public EntityTemplate Template;

        public Entity(EntityTemplate entityTemplate)
        {
            Template = entityTemplate;
        }

        public virtual Task<DamageResult> TakeDamage(CharacterEntity target)
        {
            return Task.FromResult(DamageResult.DamageMiss);
        }
    }
}