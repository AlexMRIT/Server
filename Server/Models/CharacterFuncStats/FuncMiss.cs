namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncMiss : StatFunction
    {
        public FuncMiss() : base(CharacterStatId.LuckMiss, 0x11)
        { }

        public static FuncMiss Instance = new FuncMiss();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.GetDextityBonus(statFuncEnv.Character.CharacterStats.Dextity));
            statFuncEnv.AddValue(Formulas.GetStrengthBonus(statFuncEnv.Character.CharacterStats.Strength));
        }
    }
}