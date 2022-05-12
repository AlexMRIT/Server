namespace Server.Template
{
    public sealed class BaseSpecificationForCharacter
    {
        public readonly int Strenght;
        public readonly int Dextity;
        public readonly int Endurance;
        public readonly int BaseHealth;

        public BaseSpecificationForCharacter()
        {
            Strenght = 1;
            Dextity = 1;
            Endurance = 1;
            BaseHealth = 50;
        }
    }
}