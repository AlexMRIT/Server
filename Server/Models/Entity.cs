using System;
using Server.Template;

namespace Server.Models
{
    public abstract class Entity
    {
        public EntityTemplate Template;

        public Entity(EntityTemplate entityTemplate)
        {
            Template = entityTemplate;
        }
    }
}