namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncMoveSpeed : StatFunction
    {
        public FuncMoveSpeed() : base(CharacterStatId.MoveSpeed, 0xD)
        { }

        public static FuncMoveSpeed Instance = new FuncMoveSpeed();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.GetDextityBonus(statFuncEnv.Character.CharacterStats.Dextity));
        }
    }
}