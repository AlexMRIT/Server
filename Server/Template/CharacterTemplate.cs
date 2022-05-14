using Server.Service.Interface;

namespace Server.Template
{
    public sealed class CharacterTemplate : EntityTemplate
    {
        public float Health;
        public float Damage;
        public float ArrowCount;
        public bool Death;

        public readonly IBaseSpecification BaseSpecification;

        public CharacterTemplate(IBaseSpecification baseSpecification)
        {
            BaseSpecification = baseSpecification;
        }
    }
}