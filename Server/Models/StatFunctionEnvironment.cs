namespace Server.Models
{
    public sealed class StatFunctionEnvironment
    {
        public CharacterEntity Character { get; set; }

        public double Value { get; set; }
        public double BaseValue { get; set; }

        public void AddValue(double value)
        {
            Value += value;
        }

        public void SubValue(double value)
        {
            Value -= value;
        }

        public void MulValue(double value)
        {
            Value *= value;
        }

        public void DivValue(double value)
        {
            Value /= value;
        }

        public void AddPrecent(double value)
        {
            Value += Value / 100 * value;
        }

        public void SubPrecent(double value)
        {
            Value -= Value / 100 * value;
        }
    }
}
