namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncCriticalDamage : StatFunction
    {
        public FuncCriticalDamage() : base(CharacterStatId.LuckCritical, 0xE)
        { }

        public static FuncCriticalDamage Instance = new FuncCriticalDamage();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.GetStrengthBonus(statFuncEnv.Character.CharacterStats.Strength));
        }
    }
}