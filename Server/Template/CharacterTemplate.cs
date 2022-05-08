namespace Server.Template
{
    public sealed class CharacterTemplate : EntityTemplate
    {
        public float Health;
        public float Damage;
        public float ArrowCount;
        public bool Death;

        public readonly BaseSpecificationForCharacter BaseSpecification;

        public CharacterTemplate(BaseSpecificationForCharacter baseSpecification)
        {
            BaseSpecification = baseSpecification;
        }
    }
}