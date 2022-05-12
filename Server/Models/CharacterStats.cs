using Server.Template;
using System.Threading;

namespace Server.Models
{
    public sealed class CharacterStats
    {
        private readonly Calculator[] Calculators;
        private readonly CharacterEntity Player;

        public CharacterStats(CharacterEntity characterEntity, BaseSpecificationForCharacter baseSpecification)
        {
            Calculators = Calculator.GetCalculatorsForStats();
            Player = characterEntity;

            _strength = baseSpecification.Strenght;
            _dextity = baseSpecification.Dextity;
            _endurance = baseSpecification.Endurance;
            _baseHealth = _currentHealth = baseSpecification.BaseHealth;
        }

        private const int DefaultParameterStat = 1;
        private const int _baseStatParameter = 1;

        private int _strength;
        private int _dextity;
        private int _endurance;

        private int _baseHealth = 50;
        private int _currentHealth = 50;
        private int _baseMinPhysicsDamagePoint = 1;
        private int _baseMaxPhysicsDamagePoint = 4;

        public int BaseHealth { get => _baseHealth; set => _baseHealth = value; }
        public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
        public int Strength { get => _strength; set => _strength = value; }
        public int Dextity { get => _dextity; set => _dextity = value; }
        public int Endurance { get => _endurance; set => _endurance = value; }
        public int BasePhysicsDamageMinChange { get => _baseMinPhysicsDamagePoint; set => _baseMinPhysicsDamagePoint = value; }
        public int BasePhysicsDamageMaxChange { get => _baseMaxPhysicsDamagePoint; set => _baseMaxPhysicsDamagePoint = value; }

        public double CriticalAddDamage => _baseStatParameter + CalculateStat(CharacterStatId.DamageCritical, DefaultParameterStat);
        public double AttackSpeed => _baseStatParameter + CalculateStat(CharacterStatId.AttackSpeed, DefaultParameterStat);
        public int HealthPoint => _baseStatParameter + BaseHealth + (int)CalculateStat(CharacterStatId.HealthPoint, DefaultParameterStat);
        public double LuckCriticalDamage => _baseStatParameter + CalculateStat(CharacterStatId.LuckCritical, DefaultParameterStat);
        public double LuckMiss => _baseStatParameter + CalculateStat(CharacterStatId.LuckMiss, DefaultParameterStat);
        public double MoveSpeed => _baseStatParameter + CalculateStat(CharacterStatId.MoveSpeed, DefaultParameterStat);
        public int PhysicsAttackMin => _baseMinPhysicsDamagePoint + (int)CalculateStat(CharacterStatId.PhysicsAttackPoint, DefaultParameterStat);
        public int PhysicsAttackMax => _baseMaxPhysicsDamagePoint;
        public int PhysicsDefence => _baseStatParameter + (int)CalculateStat(CharacterStatId.PhysicsDefencePoint, DefaultParameterStat);

        public double CalculateStat(CharacterStatId stat, float initial)
        {
            if (Calculators == null)
                return initial;

            Calculator calculator = Calculators[(int)stat];
            if (calculator == null || calculator.Size == 0)
                return initial;

            StatFunctionEnvironment statFuncEnv = new StatFunctionEnvironment
            {
                Character = Player,
                Value = initial
            };

            calculator.Calculate(statFuncEnv);
            return statFuncEnv.Value;
        }

        public void AddStatFunction(StatFunction func)
        {
            if (func == null || Calculators == null)
                return;

            int statId = (int)func.Stat;
            Interlocked.CompareExchange(ref Calculators[statId], new Calculator(), null);
            Calculators[statId].AddFunc(func);
        }
    }
}