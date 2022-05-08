namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncPhysicsDefence : StatFunction
    {
        public FuncPhysicsDefence() : base(CharacterStatId.PhysicsDefencePoint, 0x04)
        { }

        public static FuncPhysicsDefence Instance = new FuncPhysicsDefence();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.GetStrengthBonus(statFuncEnv.Character.CharacterStats.Strength));
        }
    }
}