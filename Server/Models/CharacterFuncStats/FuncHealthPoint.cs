namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncHealthPoint : StatFunction
    {
        public FuncHealthPoint() : base(CharacterStatId.HealthPoint, 0x00)
        { }

        public static FuncHealthPoint Instance = new FuncHealthPoint();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(1);
            statFuncEnv.MulValue(Formulas.GetEnduranceBonus(statFuncEnv.Character.CharacterStats.Endurance));
            statFuncEnv.AddValue(1);
        }
    }
}
