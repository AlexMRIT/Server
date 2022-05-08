namespace Server.Models.CharacterFuncStats
{
    internal sealed class FuncPhysicsAttack : StatFunction
    {
        public FuncPhysicsAttack() : base(CharacterStatId.PhysicsAttackPoint, 0x02)
        { }

        public static FuncPhysicsAttack Instance = new FuncPhysicsAttack();

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.GetStrengthBonus(statFuncEnv.Character.CharacterStats.Strength));
        }
    }
}