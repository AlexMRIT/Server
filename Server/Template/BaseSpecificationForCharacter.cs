using Server.Service.Interface;

namespace Server.Template
{
    public sealed class BaseSpecificationForCharacter : IBaseSpecification
    {
        private readonly int _strength;
        private readonly int _dextity;
        private readonly int _endurace;
        private readonly int _baseHealth;

        public BaseSpecificationForCharacter()
        {
            _strength = 1;
            _dextity = 1;
            _endurace = 1;
            _baseHealth = 50;
        }

        public int Strength => _strength;

        public int Dextity => _dextity;

        public int Endurance => _endurace;

        public int BaseHealth => _baseHealth;
    }
}