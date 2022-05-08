namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncDamageCritical : StatFunction
    {
        public FuncDamageCritical() : base(CharacterStatId.DamageCritical, 0xF)
        { }

        public static FuncDamageCritical Instance = new FuncDamageCritical();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.GetStrengthBonus(statFuncEnv.Character.CharacterStats.Strength));
        }
    }
}