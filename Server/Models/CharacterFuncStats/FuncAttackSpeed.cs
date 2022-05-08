namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncAttackSpeed : StatFunction
    {
        public FuncAttackSpeed() : base(CharacterStatId.AttackSpeed, 0xC)
        { }

        public static FuncAttackSpeed Instance = new FuncAttackSpeed();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.GetDextityBonus(statFuncEnv.Character.CharacterStats.Dextity));
        }
    }
}