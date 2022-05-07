using System.Threading;

namespace Server.Models
{
    public sealed class CharacterStats
    {
        private readonly Calculator[] Calculators;
        private readonly CharacterEntity Player;

        public CharacterStats(CharacterEntity characterEntity)
        {
            Calculators = Calculator.GetCalculatorsForStats();
            Player = characterEntity;
        }

        private const int DefaultParameterStat = 1;
        private const int _baseStatParameter = 1;

        private int _strength = 1;
        private int _dextity = 1;
        private int _endurance = 1;

        private int _baseHealth = 50;
        private int _baseMinPhysicsDamagePoint = 1;
        private int _baseMaxPhysicsDamagePoint = 4;

        public int BaseHealth { get => _baseHealth; set => _baseHealth = value; }
        public int Strength { get => _strength; set => _strength = value; }
        public int Dextity { get => _dextity; set => _dextity = value; }
        public int Endurance { get => _endurance; set => _endurance = value; }
        public int BasePhysicsDamageMinChange { get => _baseMinPhysicsDamagePoint; set => _baseMinPhysicsDamagePoint = value; }
        public int BasePhysicsDamageMaxChange { get => _baseMaxPhysicsDamagePoint; set => _baseMaxPhysicsDamagePoint = value; }

        public float CriticalAddDamage => _baseStatParameter + CalculateStat(CharacterStatId.DamageCritical, DefaultParameterStat);
        public float AttackSpeed => _baseStatParameter + CalculateStat(CharacterStatId.AttackSpeed, DefaultParameterStat);
        public int HealthPoint => _baseStatParameter + BaseHealth + (int)CalculateStat(CharacterStatId.HealthPoint, DefaultParameterStat);
        public float LuckCriticalDamage => _baseStatParameter + CalculateStat(CharacterStatId.LuckCritical, DefaultParameterStat);
        public float LuckMiss => _baseStatParameter + CalculateStat(CharacterStatId.LuckMiss, DefaultParameterStat);
        public float MoveSpeed => _baseStatParameter + CalculateStat(CharacterStatId.MoveSpeed, DefaultParameterStat);
        public int PhysicsAttackMin => _baseMinPhysicsDamagePoint + (int)CalculateStat(CharacterStatId.PhysicsAttackPoint, DefaultParameterStat);
        public int PhysicsAttackMax => _baseMaxPhysicsDamagePoint;
        public int PhysicsDefence => _baseStatParameter + (int)CalculateStat(CharacterStatId.PhysicsDefencePoint, DefaultParameterStat);

        public float CalculateStat(CharacterStatId stat, float initial)
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