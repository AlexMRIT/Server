namespace Server.Models
{
    public sealed class StatFunctionEnvironment
    {
        public CharacterEntity Character { get; set; }

        public float Value { get; set; }
        public float BaseValue { get; set; }

        public void AddValue(float value)
        {
            Value += value;
        }

        public void SubValue(float value)
        {
            Value -= value;
        }

        public void MulValue(float value)
        {
            Value *= value;
        }

        public void DivValue(float value)
        {
            Value /= value;
        }

        public void AddPrecent(float value)
        {
            Value += Value / 100 * value;
        }

        public void SubPrecent(float value)
        {
            Value -= Value / 100 * value;
        }
    }
}
